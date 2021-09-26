using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class mainScene : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        Hallway
    }
    /*
    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    */

    [SerializeField]
    Vector2Int size;
    [SerializeField]
    int roomCount;
    [SerializeField]
    Vector2Int roomMaxSize;
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;
    //add by csd
    [SerializeField]
    Material greenMaterial;
    [SerializeField]
    GameObject linePrefab;

    [SerializeField]
    GameObject planePrefab;

    [SerializeField]
    GameObject wallPrefab;

    [SerializeField]
    Material roomPlaceMaterial;

    [SerializeField]
    Material hallWayPlaceMaterial;

    [SerializeField]
    public GameObject cubeParent = null;  //色块的父节点

    [SerializeField]
    public GameObject mazeParent = null; //地板，墙的父节点

    [SerializeField]
    public GameObject pathParent = null; //路径的父节点

    [SerializeField]
    public GameObject monsterPrefab = null; //怪物的预制

    [SerializeField]
    public Transform camerTransform = null; //主摄像机

    [SerializeField]
    public Transform canvasTransform = null; //画布

    [SerializeField]
    public Transform monsterManagerTrans = null;  //怪物管理

    //怪物ID索引
    private int monsterID = 0;

    Grid2D<placeWall> placeGrid; //地块的三维空间表
    List<HallWay2D> hallways;
    List<GameObject> cubeDebugLst;
    List<GameObject> hallwayDebugLst;
    //List<GameObject> stairDebugLst;
    List<GameObject> lineDebugLst;
    public GameObject friendRole; //主角的小弟

    [SerializeField]
    public Vector3Int firstPos = Vector3Int.zero;
    private bool isInitFinish = false;

    private const int csPosY = 0;

    //add end

    Random random;
    Grid2D<CellType> grid;
    List<Room2D> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;


    //add
    class pathVector
    {
        public Vector2Int sourVector { get; set; }
        public Vector2Int destVector { get; set; }
    }

    List<pathVector> pathLst; //寻路的路径点

    //List<Vector3Int> hallWayLst;

   // List<stairVector> stairsLst;

    int roomIndex = 0;
    //end

    public bool getIsInit()
    {
        return isInitFinish;
    }

    void Start()
    {
        createLandScape();
    }

    void createLandScape()
    {
        int ranSeed = System.DateTime.Now.Second;
        Debug.LogWarning("Random seed:" + ranSeed.ToString());
        random = new Random(ranSeed);
        //random = new Random(0);

        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room2D>();
        /*
        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
        */
        //add by csd
        placeGrid = new Grid2D<placeWall>(size, Vector2Int.zero);

        hallways = new List<HallWay2D>();

        pathLst = new List<pathVector>();

        cubeDebugLst = new List<GameObject>();
        hallwayDebugLst = new List<GameObject>();
       // stairDebugLst = new List<GameObject>();
        lineDebugLst = new List<GameObject>();
        monsterID = 0;
        //add by csd end

        createScene();
    }

    private void clearScene()
    {
        isInitFinish = false;

    }

    private void createScene()
    {
        PlaceRooms();  //生成房间
        Triangulate(); //三角测量
        CreateHallways(); //创建走廊，过道
        PathfindHallways(); //走廊，过道寻路生成


        createPlance(); //生成房间

#if !TESTPATH
        createHallWayLst(); //生成过道间

      //  createStairsLst(); //生成楼梯

        hideHallWay(); //隐藏墙壁并生成怪物

#if DebugCube
                drawLinePath();
#endif
#endif

        isInitFinish = true;
    }

    public void createPlance()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            //int i = 0;
            Room2D tmpRoom = rooms[i];
            tmpRoom.makeAllPlane();  //铺房间
            if (i == 0)
            {
                firstPos = new Vector3Int(tmpRoom.pos.x, csPosY, tmpRoom.pos.y);
                // break;
            }
            //break;
#if !TESTPATH
            tmpRoom.makeAllWall();  //铺墙
#endif
        }
    }


    public void createHallWayLst()
    {
        for (int i = 0; i < hallways.Count; i++)
        {

            HallWay2D tmpHallWay = hallways[i];
            tmpHallWay.makeAllPlane();
            tmpHallWay.makeAllWall();
            //tmpHallWay.bounds.
            //PlaceHallway(pos);
        }
    }

    private void addMonster(Vector3 pPos, bool isMonster)
    {
        if (isMonster == false)
        {
            pPos.x += 0.2f;
            pPos.z += 0.2f;
        }

        GameObject tmpMonster = Instantiate(monsterPrefab, pPos, Quaternion.identity, monsterManagerTrans);
        tmpMonster.name = "monster_" + monsterID.ToString();
        monsterID++;

        roleProperty tmpPro = tmpMonster.GetComponent<roleProperty>();


        if (isMonster)
        {
            tmpPro.InitData(camerTransform, canvasTransform, 0);
            CapsuleCollider tmpColl = tmpMonster.GetComponent<CapsuleCollider>();
            //tmpColl.isTrigger = true;
            tmpMonster.AddComponent<monsterNormalAI>();
        }
        else
        {
            tmpPro.InitData(camerTransform, canvasTransform, 1);
            followRole tmpFollow = tmpMonster.AddComponent<followRole>();
            //tmpFollow.mainObj = friendRole;
            friendRole = tmpMonster;
        }

        //go.GetComponent<MeshRenderer>().material = material;
        //go.name = roomName + nameIndex.ToString() + "_" + placeIndex.ToString();
    }

    public void hideHallWay()
    {
        for (int hideIndex = 0; hideIndex < pathLst.Count; hideIndex++)
        {
            pathVector tmpPath = pathLst[hideIndex];
            bool isMonster = true;
            if (hideIndex == 0)
            {
                isMonster = false;
            }

            judgePlaceDelWall(tmpPath.sourVector, tmpPath.destVector, isMonster);


            if (hideIndex == 0)
            {
                firstPos = new Vector3Int(tmpPath.sourVector.x, csPosY,tmpPath.sourVector.y);
            }



        }

        //    hideIndex++;
    }


    private bool judgePlaceDelWall(Vector2Int sourPos, Vector2Int destPos, bool isMonster)
    {
        bool res = false;
        bool sourNeed = false; //是否要敲墙
        bool destNeed = false; //是否要敲墙

        Vector3Int sourPosParent = Vector3Int.zero;
        Vector3Int destPosParent = Vector3Int.zero;
        if (isRoomOrHillWay(sourPos)) //判断是否为房间或过道
            sourNeed = true;

        if (isRoomOrHillWay(destPos)) //判断是否为房间或过道
            destNeed = true;

        if (sourNeed && destNeed)  //判断是否为同一个房间,只有房间和过道才加入， 楼梯的路径点未加入gird
        {
            Room2D sourRoom = grid.getGridDataObj(sourPos);
            Room2D destRoom = grid.getGridDataObj(destPos);

            if (sourRoom.pos == destRoom.pos)
            {
                //Debug.LogWarning("is same place");
                return false;
            }
        }
       
        if (sourPos.x == destPos.x)
        {  //Z轴相交集， X相同的
           // if (sourPos.y == destPos.y) //Y轴相同
           // {
                addMonster(new Vector3(sourPos.x, csPosY, sourPos.y), isMonster);

                //if (sourPos.z == destPos.z + 1) //sourPos在destPos的上面
                if (sourPos.y > destPos.y)
                {
                    setHideWallByZ(sourPos, destPos, sourNeed, destNeed);
                }
                else //if (sourPos.z + 1 == destPos.z)
                {
                    setHideWallByZ(destPos, sourPos, destNeed, sourNeed);
                }
         //   }
        }
        else if (sourPos.y == destPos.y)
        { //X轴相交集， Z相同的
            addMonster(new Vector3(sourPos.x, csPosY, sourPos.y), isMonster);

            if (sourPos.x > destPos.x)
            //if (sourPos.x == destPos.x + 1) //sourPos在destPos的右边
            {
                setHideWallByX(destPos, sourPos, destNeed, sourNeed);
            }
            else //if (sourPos.x + 1 == destPos.x)
            {
                setHideWallByX(sourPos, destPos, sourNeed, destNeed);
            }

        }
        return res;
    }



    private bool isRoomOrHillWay(Vector2Int pPos)
    {
        bool res = false;
        CellType tmpType = grid[pPos];
        if ((tmpType == CellType.Room) || (tmpType == CellType.Hallway))
            res = true;
        return res;
    }

    private void setHideWallByZ(Vector2Int pTop, Vector2Int pBottom, bool pIsRoomOrHillWay1, bool pIsRoomOrHillWay2)
    {
        if (pIsRoomOrHillWay1)
        {
            placeWall pTopPlaceWallObj = getPlaceWallByPos(pTop);
            pTopPlaceWallObj.bottomWall.SetActive(false);
        }

        if (pIsRoomOrHillWay2)
        {
            placeWall pBottomPlaceWallObj = getPlaceWallByPos(pBottom);
            pBottomPlaceWallObj.topWall.SetActive(false);
        }
    }

    private void setHideWallByX(Vector2Int pLeft, Vector2Int pRight, bool pIsRoomOrHillWay1, bool pIsRoomOrHillWay2)
    {
        if (pIsRoomOrHillWay1)
        {
            placeWall pLeftPlaceWallObj = getPlaceWallByPos(pLeft);
            pLeftPlaceWallObj.rightWall.SetActive(false);
        }

        if (pIsRoomOrHillWay2)
        {
            placeWall pRightPlaceWallObj = getPlaceWallByPos(pRight);
            pRightPlaceWallObj.leftWall.SetActive(false);
        }
    }

    private placeWall getPlaceWallByPos(Vector2Int pPos)
    {
        placeWall tmpPlaceWall = placeGrid[pPos];
        return tmpPlaceWall;
    }



    void PlaceRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            Vector2Int roomSize = new Vector2Int(
                random.Next(1, roomMaxSize.x + 1),
                random.Next(1, roomMaxSize.y + 1)
            );

            bool add = true;
            GameObject newRoomObj = new GameObject();
            newRoomObj.transform.SetParent(mazeParent.transform);
            newRoomObj.name = "room" + location.x.ToString() + "_" + location.y.ToString() + "_" + location.y.ToString();

            Room2D newRoom = newRoomObj.AddComponent<Room2D>();
            newRoom.initData(location, roomSize, planePrefab, wallPrefab, roomPlaceMaterial, placeGrid, roomIndex, newRoomObj);
            //Room2D newRoom = new Room2D(location, roomSize);
            //Room2D buffer = new Room2D(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            RectInt buffer = new RectInt(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms)
            {
                if (Room2D.IntersectBuff(room.bounds, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
            {
                add = false;
            }

            if (add)
            {
                rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);
                //add by csd
                roomIndex++;
                //end

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                    //add by csd
                    grid.setGridDataObj(newRoom, pos);
                    //grid.setDataIsCreate(pos, true);
                    //add end
                }
            }
            else
            {
                GameObject.Destroy(newRoomObj);
            }
        }
    }

    void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms)
        {
            vertices.Add(new Vertex<Room2D>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways()
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (random.NextDouble() < 0.125)
            {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways()
    {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room2D>).Item;
            var endRoom = (edge.V as Vertex<Room2D>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;

                        //add by csd
                        pathVector tmpPath = new pathVector();
                        tmpPath.sourVector = prev;
                        tmpPath.destVector = current;
                        pathLst.Add(tmpPath);
                        //add end
                    }
                }

                foreach (var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        PlaceHallway(pos);

                        //add by csd begin
                        bool isCreate = grid.getDataIsCreate(pos);
                        if (isCreate == false)
                        {
                            //HallWay newHallWay = new HallWay(pos, new Vector3Int(1, 1, 1), planePrefab, wallPrefab, hallWayPlaceMaterial, placeGrid, roomIndex, mazeParent);
                            GameObject newHallWayObj = new GameObject();
                            newHallWayObj.transform.SetParent(mazeParent.transform);
                            newHallWayObj.name = "hallway" + pos.x.ToString() + "_" + pos.y.ToString() + "_" + pos.y.ToString();

                            HallWay2D newHallWay = newHallWayObj.AddComponent<HallWay2D>();
                            newHallWay.initDataHallWay(pos, new Vector2Int(1, 1), planePrefab, wallPrefab, hallWayPlaceMaterial, placeGrid, roomIndex, newHallWayObj);

                            /*
                            HallWay newHallWay = mazeParent.AddComponent<HallWay>();
                            newHallWay.initData(pos, new Vector3Int(1, 1, 1), planePrefab, wallPrefab, hallWayPlaceMaterial, placeGrid, roomIndex, mazeParent);
                            */

                            hallways.Add(newHallWay);
                            grid.setGridDataObj(newHallWay, pos);

                            roomIndex++;
                            // grid.setDataIsCreate(pos,true);
                        }
                        //add end
                    }
                }
            }
        }
    }

    void PlaceCube(Vector2Int location, Vector2Int size, Material material)
    {
        GameObject go = Instantiate(cubePrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
        go.GetComponent<Transform>().localScale = new Vector3(size.x, 1, size.y);
        go.GetComponent<MeshRenderer>().material = material;
    }

    void PlaceRoom(Vector2Int location, Vector2Int size)
    {
        PlaceCube(location, size, redMaterial);
    }

    void PlaceHallway(Vector2Int location)
    {
        PlaceCube(location, new Vector2Int(1, 1), blueMaterial);
    }

#if debug_maze
    void OnGUI()
#else
    private void noUseGUI()
#endif
    {

        float btnWidth = 100.0f;
        float btnHeight = 100.0f;
        float btnPosY = btnHeight + 5.0f;

        int i = 0;


        if (GUI.Button(new Rect(Screen.width - btnWidth * 2, btnHeight * i, btnWidth, btnHeight), "drawLine"))
        {
            drawLinePath();

        }
        i++;


        if (GUI.Button(new Rect(Screen.width - btnWidth * 2, btnHeight * i, btnWidth, btnHeight), "hideHallWay"))
        {
            //hideLinePath();
            hideHallWay();

        }
        i++;


        if (GUI.Button(new Rect(Screen.width - btnWidth * 2, btnHeight * i, btnWidth, btnHeight), "hideAllCube"))
        {
            hideAllCube();
        }
        i++;

        if (GUI.Button(new Rect(Screen.width - btnWidth * 2, btnHeight * i, btnWidth, btnHeight), "hideAllHallWay"))
        {
            hideAllHallway();

        }
        i++;

        //创建房间
        i = 0;


        if (GUI.Button(new Rect(Screen.width - btnWidth, btnHeight * i, btnWidth, btnHeight), "createPlance"))
        {
            createPlance();
        }
        i++;

        if (GUI.Button(new Rect(Screen.width - btnWidth, btnHeight * i, btnWidth, btnHeight), "createHallWayLst"))
        {
            createHallWayLst();
        }
        i++;


    }

    public void drawLinePath()
    {
#if !DebugCube
        return;
#endif

        for (int i = 0; i < pathLst.Count; i++)
        {
            pathVector tmpPath = pathLst[i];

            //Vector3 sour = new Vector3(tmpPath.sourVector.x + 0.5f, tmpPath.sourVector.y + 0.5f, tmpPath.sourVector.z + 0.5f);
            //Vector3 dest = new Vector3(tmpPath.destVector.x + 0.5f, tmpPath.destVector.y + 0.5f, tmpPath.destVector.z + 0.5f);
            Vector3 sour = new Vector3(tmpPath.sourVector.x, csPosY + 0.5f, tmpPath.sourVector.y);
            Vector3 dest = new Vector3(tmpPath.destVector.x, csPosY + 0.5f, tmpPath.destVector.y);

            GameObject obj1 = creatLineFlag(sour, "dl sour" + i.ToString());
            obj1.transform.LookAt(dest);

            Vector3 rolate = obj1.transform.eulerAngles;
            rolate.x = rolate.x + 270.0f;

            obj1.transform.eulerAngles = rolate;

            //GameObject obj2 = creatLineFlag(new Vector3(tmpPath.destVector.x + 0.5f, tmpPath.destVector.y + 0.5f, tmpPath.destVector.z + 0.5f), "dl dest" + i.ToString());

            lineDebugLst.Add(obj1);
            //lineDebugLst.Add(obj2);


            //Debug.DrawLine(tmpPath.sourVector + new Vector3(0.5f, 0.5f, 0.5f), tmpPath.destVector + new Vector3(0.5f, 0.5f, 0.5f), Color.red, 100, false);
           // Debug.DrawLine(tmpPath.sourVector + new Vector3(0.0f, 0.5f, 0.0f), tmpPath.destVector + new Vector3(0.0f, 0.5f, 0.0f), Color.red, 100, false);
            //break;
        }
    }


    private GameObject creatLineFlag(Vector3 pos, string pName)
    {
        GameObject obj = Instantiate(linePrefab, pos, Quaternion.identity, pathParent.transform);
        obj.name = pName;

        return obj;
    }
    public void hideAllCube()
    {
        hideAllCellType(cubeDebugLst);
    }

    public void hideAllHallway()
    {
        hideAllCellType(hallwayDebugLst);
    }

    private void hideAllCellType(List<GameObject> objLst)
    {
        for (int i = 0; i < objLst.Count; i++)
        {
            objLst[i].SetActive(false);
        }
    }

    private void PlaceCube(Vector3Int location, Vector3Int size, Material material, string cubeName = "cube")
    {
#if !DebugCube
        return;
#endif

        GameObject go = Instantiate(cubePrefab, location, Quaternion.identity, cubeParent.transform);
        go.GetComponent<Transform>().localScale = size;
        go.GetComponent<MeshRenderer>().material = material;
        go.name = cubeName;

        if (cubeName == "cube")
        {
            cubeDebugLst.Add(go);
        }
        else if (cubeName == "Hallway")
        {
            hallwayDebugLst.Add(go);
        }

    }

    public void clearAllMonster()
    {
        if (monsterManagerTrans.gameObject != null)
        {
            delTransformAllChild(monsterManagerTrans);
        }
    }

    public void clearAllMaze()
    {
        if (mazeParent != null)
        {
            delTransformAllChild(mazeParent.transform);
        }
    }

    public void clearAllHpUI()
    {
        if (canvasTransform.gameObject != null)
        {
            delTransformAllChild(canvasTransform);
        }
    }

    public void delTransformAllChild(Transform pTran)
    {
        int nCount = pTran.childCount;
        if (nCount > 0)
        {
            for (int i = 0; i < nCount; i++)
            {
                Transform tmpTran = pTran.GetChild(i);
                delTransform(tmpTran);
            }

            GameObject.Destroy(pTran.gameObject);
        }
    }


    public void delTransform(Transform pTran)
    {
        int nCount = pTran.childCount;
        if (nCount <= 0)
        {
            GameObject.Destroy(pTran.gameObject);
        }
        else
        {
            for (int i = 0; i < nCount; i++)
            {
                Transform tmpTran = pTran.GetChild(i);
                delTransform(tmpTran);
            }

            GameObject.Destroy(pTran.gameObject);
        }
    }

    public void clearRooms()
    {
        Debug.LogWarning("clearRooms");
        foreach (var room in rooms)
        {
            delTransform(room.transform);
        }

        rooms.Clear();
    }
}
