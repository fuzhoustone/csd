using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class Generator3D : MonoBehaviour {
    enum CellType {
        None,
        Room,
        Hallway,
        Stairs
    }

    [SerializeField]
    Vector3Int size;  //三维数组房间，但每个房间的大小随机
    [SerializeField]
    Vector3Int localOffset;
    [SerializeField]
    Vector3Int sizeOffset; 
    [SerializeField]
    int roomCount;  //房间数,若因空间问题无法产生，则房间不产生
    [SerializeField]
    Vector3Int roomMaxSize;  //每个房间的所占的最大格子数，随机产生
    [SerializeField]
    GameObject cubePrefab;
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material blueMaterial;
    [SerializeField]
    Material greenMaterial;

    //add by csd begin
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
    Material stairPlaceMaterial;
    

    [SerializeField]
    public GameObject upHillPrefab = null;  //上坡资源

    [SerializeField]
    public GameObject downHillPrefab = null; //下坡资源
    
    //add by csd end

    Random random;
    Grid3D<CellType> grid; //三维空间的坐标对应的类型， 多个坐标可能对应1个CellType类型
    List<Room> rooms;  //房间列表


    //add by csd begin
    Grid3D<placeWall> placeGrid; //地块的三维空间表
    List<HallWay> hallways; 
    List<GameObject> cubeDebugLst;
    List<GameObject> hallwayDebugLst;
    List<GameObject> stairDebugLst;
    List<GameObject> lineDebugLst;
    //add by csd end

    Delaunay3D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    //add by csd
    class pathVector {
        public Vector3Int sourVector { get; set; }
        public Vector3Int destVector { get; set; }
    }

    class stairVector {
        public Vector3Int currentVector { get; set; }
        public Vector3Int prevVector { get; set; }
    }

    List<pathVector> pathLst;

    //List<Vector3Int> hallWayLst;

    List<stairVector> stairsLst;
    //add end

    void Start() {
        random = new Random(0);
        grid = new Grid3D<CellType>(size, Vector3Int.zero);
        rooms = new List<Room>();

        //add by csd
        placeGrid = new Grid3D<placeWall>(size, Vector3Int.zero);

        hallways = new List<HallWay>();

        pathLst = new List<pathVector>(); 

        stairsLst = new List<stairVector>(); 

        cubeDebugLst = new List<GameObject>();
        hallwayDebugLst = new List<GameObject>();
        stairDebugLst = new List<GameObject>();
        lineDebugLst = new List<GameObject>();
        //add by csd end

        PlaceRooms();  //生成房间
        Triangulate(); //三角测量
        CreateHallways(); //创建走廊，过道
        PathfindHallways(); //走廊，过道寻路生成

        createPlance(); //生成房间
        
        createHallWayLst(); //生成过道间

        createStairsLst(); //生成楼梯色块

        hideAllCube();
        hideAllHallway();
        hideAllStairs();

        drawLinePath();
        

    }
    void OnGUI()
    {

        float btnWidth = 100.0f;
        float btnHeight = 100.0f;
        float btnPosY = btnHeight + 5.0f;

        int i = 0;

        /*
        if (GUI.Button(new Rect(Screen.width - btnWidth*2, btnHeight * i, btnWidth, btnHeight), "drawLine"))
        {
            drawLinePath();
               
        }
        i++;
        */

        if (GUI.Button(new Rect(Screen.width - btnWidth * 2, btnHeight * i, btnWidth, btnHeight), "hideHallWay"))
        {
            //hideLinePath();
            hideHallWay();

        }
        i++;
        

        if (GUI.Button(new Rect(Screen.width - btnWidth*2, btnHeight * i, btnWidth, btnHeight), "hideAllCube"))
        {
            hideAllCube();
        }
        i++;

        if (GUI.Button(new Rect(Screen.width - btnWidth*2, btnHeight * i, btnWidth, btnHeight), "hideAllHallWay"))
        {
            hideAllHallway();

        }
        i++;
       
        if (GUI.Button(new Rect(Screen.width - btnWidth*2, btnHeight * i, btnWidth, btnHeight), "hideAllStairs"))
        {
            hideAllStairs();

        }
        i++;

        //创建房间
        i = 0;

        /*
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
        */

        if (GUI.Button(new Rect(Screen.width - btnWidth, btnHeight * i, btnWidth, btnHeight), "createStairsLst"))
        {
            createStairsLst();
        }
        i++;

    }

        //生成房间
    void PlaceRooms() {
        for (int i = 0; i < roomCount; i++) {
            Vector3Int location = new Vector3Int( //给个随机的位置
                random.Next(0, size.x),
                random.Next(0, size.y),
                random.Next(0, size.z)
            );

            Vector3Int roomSize = new Vector3Int(  //对于+1 疑似可以不必+1
                random.Next(1, roomMaxSize.x + 1),
                random.Next(1, roomMaxSize.y + 1),
                random.Next(1, roomMaxSize.z + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize,planePrefab, wallPrefab, roomPlaceMaterial, placeGrid);
            //Room buffer = new Room(location + new Vector3Int(-1, 0, -1), roomSize + new Vector3Int(2, 0, 2));
            Room buffer = new Room(location + localOffset, roomSize + sizeOffset, planePrefab, wallPrefab, roomPlaceMaterial, null); 

            foreach (var room in rooms) { //判断房间区间是否可以加入
                if (Room.Intersect(room, buffer)) { //intersect里有个!, 若返回 true，说明无法添加
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x    //房间大小超出 外界环境的范围
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y
                || newRoom.bounds.zMin < 0 || newRoom.bounds.zMax >= size.z) {  
                add = false;
            }

            if (add) { //可新建房间
                rooms.Add(newRoom);
                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size);

                foreach (var pos in newRoom.bounds.allPositionsWithin) { //一个房间 占用有多个坐标，全部记录下, 房间与坐标的索引关系需创建
                    grid[pos] = CellType.Room;
                    //add by csd
                    grid.setGridDataObj(newRoom, pos);
                    grid.setDataIsCreate(pos, true);
                    //add end
                }
            }
        }
    }

    //三角测量
    void Triangulate() {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms) {
            vertices.Add(new Vertex<Room>((Vector3)room.bounds.position + ((Vector3)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay3D.Triangulate(vertices);
    }

    //创建走廊，过道
    void CreateHallways() {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> minimumSpanningTree = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(minimumSpanningTree);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (random.NextDouble() < 0.125) {
                selectedEdges.Add(edge);
            }
        }
    }

    public void drawLinePathfindHallways() {
        DungeonPathfinder3D aStar = new DungeonPathfinder3D(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector3Int((int)startPosf.x, (int)startPosf.y, (int)startPosf.z);
            var endPos = new Vector3Int((int)endPosf.x, (int)endPosf.y, (int)endPosf.z);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder3D.Node a, DungeonPathfinder3D.Node b) => {
                var pathCost = new DungeonPathfinder3D.PathCost();

                var delta = b.Position - a.Position;

                if (delta.y == 0)
                {
                    //flat hallway
                    pathCost.cost = Vector3Int.Distance(b.Position, endPos);    //heuristic

                    if (grid[b.Position] == CellType.Stairs)
                    {
                        return pathCost;
                    }
                    else if (grid[b.Position] == CellType.Room)
                    {
                        pathCost.cost += 5;
                    }
                    else if (grid[b.Position] == CellType.None)
                    {
                        pathCost.cost += 1;
                    }

                    pathCost.traversable = true;
                }
                else
                {
                    //staircase
                    if ((grid[a.Position] != CellType.None && grid[a.Position] != CellType.Hallway)
                        || (grid[b.Position] != CellType.None && grid[b.Position] != CellType.Hallway)) return pathCost;

                    pathCost.cost = 100 + Vector3Int.Distance(b.Position, endPos);    //base cost + heuristic

                    int xDir = Mathf.Clamp(delta.x, -1, 1);
                    int zDir = Mathf.Clamp(delta.z, -1, 1);
                    Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                    Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);

                    if (!grid.InBounds(a.Position + verticalOffset)
                        || !grid.InBounds(a.Position + horizontalOffset)
                        || !grid.InBounds(a.Position + verticalOffset + horizontalOffset))
                    {
                        return pathCost;
                    }

                    if (grid[a.Position + horizontalOffset] != CellType.None
                        || grid[a.Position + horizontalOffset * 2] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset * 2] != CellType.None)
                    {
                        return pathCost;
                    }

                    pathCost.traversable = true;
                    pathCost.isStairs = true;
                }

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

                        if (delta.y != 0) //y轴不同，涉及上楼或下楼的
                        {
                            int xDir = Mathf.Clamp(delta.x, -1, 1);
                            int zDir = Mathf.Clamp(delta.z, -1, 1);
                            Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                            Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);

                            grid[prev + horizontalOffset] = CellType.Stairs;
                            grid[prev + horizontalOffset * 2] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset * 2] = CellType.Stairs;
                            Debug.LogWarning("PlaceStairs");
                            /*
                            PlaceStairs(prev + horizontalOffset);
                            PlaceStairs(prev + horizontalOffset * 2);
                            PlaceStairs(prev + verticalOffset + horizontalOffset);
                            PlaceStairs(prev + verticalOffset + horizontalOffset * 2);
                            */
                        }

                         Debug.DrawLine(prev + new Vector3(0.5f, 0.5f, 0.5f), current + new Vector3(0.5f, 0.5f, 0.5f), Color.red, 100, false);
                    }
                }


                foreach (var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        // PlaceHallway(pos);
                        //Debug.LogWarning("PlaceHallway");
                    }
                }
            }
        }
    }



    private GameObject creatLineFlag(Vector3 pos, string pName) {
        GameObject obj = Instantiate(linePrefab, pos, Quaternion.identity);
        obj.name = pName;

        return obj;
    }

    //路径画线
    public void drawLinePath() {
        for (int i = 0; i < pathLst.Count; i++) 
        { 
            pathVector tmpPath = pathLst[i];

            //Vector3 sour = new Vector3(tmpPath.sourVector.x + 0.5f, tmpPath.sourVector.y + 0.5f, tmpPath.sourVector.z + 0.5f);
            //Vector3 dest = new Vector3(tmpPath.destVector.x + 0.5f, tmpPath.destVector.y + 0.5f, tmpPath.destVector.z + 0.5f);
            Vector3 sour = new Vector3(tmpPath.sourVector.x , tmpPath.sourVector.y + 0.5f, tmpPath.sourVector.z );
            Vector3 dest = new Vector3(tmpPath.destVector.x , tmpPath.destVector.y + 0.5f, tmpPath.destVector.z );

            GameObject obj1 = creatLineFlag(sour, "dl sour" + i.ToString());
            obj1.transform.LookAt(dest);

            Vector3 rolate = obj1.transform.eulerAngles;
            rolate.x = rolate.x + 270.0f;

            obj1.transform.eulerAngles = rolate;

            //GameObject obj2 = creatLineFlag(new Vector3(tmpPath.destVector.x + 0.5f, tmpPath.destVector.y + 0.5f, tmpPath.destVector.z + 0.5f), "dl dest" + i.ToString());

            lineDebugLst.Add(obj1);
            //lineDebugLst.Add(obj2);


            //Debug.DrawLine(tmpPath.sourVector + new Vector3(0.5f, 0.5f, 0.5f), tmpPath.destVector + new Vector3(0.5f, 0.5f, 0.5f), Color.red, 100, false);
            Debug.DrawLine(tmpPath.sourVector + new Vector3(0.0f, 0.5f, 0.0f), tmpPath.destVector + new Vector3(0.0f, 0.5f, 0.0f), Color.red, 100, false);
            //break;
        }
    }

    //一个接一个的隐藏
    private int hideIndex = 118;
    public void hideHallWay()
    {
       // if(hideIndex < pathLst.Count)
        for (int hideIndex = 0; hideIndex< pathLst.Count; hideIndex++)
        {
            pathVector tmpPath = pathLst[hideIndex];
            judgePlaceDelWall(tmpPath.sourVector, tmpPath.destVector);
           
        }

       //  hideIndex++;
    }

    public void hideLinePath() {
        for (int i = 0; i < lineDebugLst.Count; i++) {
            GameObject obj = lineDebugLst[i];
            obj.SetActive(false);
        }
        
    }


    public void createPlance()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
        //int i = 0;
        Room tmpRoom = rooms[i];
        tmpRoom.makeAllPlane();  //铺房间

        tmpRoom.makeAllWall();  //铺墙
        }
    }

    //楼梯加入列表
    private void addStairsLst(Vector3Int current, Vector3Int prev) {
        stairVector tmpPath = new stairVector();
        tmpPath.currentVector = current;
        tmpPath.prevVector = prev;
        stairsLst.Add(tmpPath);
    }


    //生成过道
    public void createHallWayLst()
    {
        for (int i = 0; i < hallways.Count; i++)
        {
            
            HallWay tmpHallWay = hallways[i];
            tmpHallWay.makeAllPlane();
            tmpHallWay.makeAllWall();
            //tmpHallWay.bounds.
            //PlaceHallway(pos);
        }
    }

    private int indexStairs = 0;
    public void createStairsLst() {
        for (int indexStairs = 0; indexStairs < stairsLst.Count; indexStairs++)
        {
            stairVector tmpPath = stairsLst[indexStairs];
            createStairs(tmpPath.currentVector, tmpPath.prevVector, false);
        }
        //     indexStairs++;

    }

    private void createStairs(Vector3Int current, Vector3Int prev, bool needCal) {
        var delta = current - prev;
        if (delta.y != 0)
        {
            int xDir = Mathf.Clamp(delta.x, -1, 1);
            int zDir = Mathf.Clamp(delta.z, -1, 1);
            Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
            Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);
            if (needCal) //是否计算
            {
                grid[prev + horizontalOffset] = CellType.Stairs;
                grid[prev + horizontalOffset * 2] = CellType.Stairs;
                grid[prev + verticalOffset + horizontalOffset] = CellType.Stairs;
                grid[prev + verticalOffset + horizontalOffset * 2] = CellType.Stairs;

                //生成色块
                PlaceStairs(prev + horizontalOffset);
                PlaceStairs(prev + horizontalOffset * 2);
                PlaceStairs(prev + verticalOffset + horizontalOffset);
                PlaceStairs(prev + verticalOffset + horizontalOffset * 2);

                addStairsLst(current, prev);
            }
            else {
                //色块的生成地板
                //所有的色块只生成某个方向的
                //4个色块， 2个楼梯的色块， 1个空中色块，1个实心色块
                //实心色块，不作处理
                //空中色块，不铺地板，只铺两边的墙
                //2个楼梯色块，铺一个楼梯地板，和两边的墙

                //上坡1和4色块, 空中为3色块，实心为2色块
                //下坡1和4色块，空中为2色块，实心为3色块

                /*
                PlaceStairs(prev + horizontalOffset);   //色块1
                PlaceStairs(prev + horizontalOffset * 2); //色块2
                PlaceStairs(prev + verticalOffset + horizontalOffset);  //色块3
                PlaceStairs(prev + verticalOffset + horizontalOffset * 2); //色块4
                */
                Vector3Int vect3Num1 = prev + horizontalOffset;
                Vector3Int vect3Num2 = prev + horizontalOffset * 2;
                Vector3Int vect3Num3 = prev + verticalOffset+ horizontalOffset;
                Vector3Int vect3Num4 = prev + verticalOffset+ horizontalOffset * 2;
                stairWay tmpWay = new stairWay(wallPrefab,upHillPrefab, downHillPrefab,prev,current, vect3Num1, vect3Num2, vect3Num3, vect3Num4);
                tmpWay.makeStairWay();
            }
        }
    }

    




    //走廊，过道寻路生成
    private void PathfindHallways() {
        DungeonPathfinder3D aStar = new DungeonPathfinder3D(size);

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector3Int((int)startPosf.x, (int)startPosf.y, (int)startPosf.z);
            var endPos = new Vector3Int((int)endPosf.x, (int)endPosf.y, (int)endPosf.z);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder3D.Node a, DungeonPathfinder3D.Node b) => {
                var pathCost = new DungeonPathfinder3D.PathCost();

                var delta = b.Position - a.Position;

                if (delta.y == 0) {
                    //flat hallway
                    pathCost.cost = Vector3Int.Distance(b.Position, endPos);    //heuristic

                    if (grid[b.Position] == CellType.Stairs) {
                        return pathCost;
                    } else if (grid[b.Position] == CellType.Room) {
                        pathCost.cost += 5;
                    } else if (grid[b.Position] == CellType.None) {
                        pathCost.cost += 1;
                    }

                    pathCost.traversable = true;
                } else {
                    //staircase
                    if ((grid[a.Position] != CellType.None && grid[a.Position] != CellType.Hallway)
                        || (grid[b.Position] != CellType.None && grid[b.Position] != CellType.Hallway)) return pathCost;

                    pathCost.cost = 100 + Vector3Int.Distance(b.Position, endPos);    //base cost + heuristic

                    int xDir = Mathf.Clamp(delta.x, -1, 1);
                    int zDir = Mathf.Clamp(delta.z, -1, 1);
                    Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                    Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);

                    if (!grid.InBounds(a.Position + verticalOffset)
                        || !grid.InBounds(a.Position + horizontalOffset)
                        || !grid.InBounds(a.Position + verticalOffset + horizontalOffset)) {
                        return pathCost;
                    }

                    if (grid[a.Position + horizontalOffset] != CellType.None
                        || grid[a.Position + horizontalOffset * 2] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset] != CellType.None
                        || grid[a.Position + verticalOffset + horizontalOffset * 2] != CellType.None) {
                        return pathCost;
                    }

                    pathCost.traversable = true;
                    pathCost.isStairs = true;
                }

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    var current = path[i];

                    if (grid[current] == CellType.None) {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0) {
                        var prev = path[i - 1]; //path有可能在循环中改变，未验证？？
                        //add by csd
                        createStairs(current, prev, true);
                        //add end

                        /*
                        var delta = current - prev;

                        if (delta.y != 0) {
                            int xDir = Mathf.Clamp(delta.x, -1, 1);
                            int zDir = Mathf.Clamp(delta.z, -1, 1);
                            Vector3Int verticalOffset = new Vector3Int(0, delta.y, 0);
                            Vector3Int horizontalOffset = new Vector3Int(xDir, 0, zDir);
                            
                            grid[prev + horizontalOffset] = CellType.Stairs;
                            grid[prev + horizontalOffset * 2] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset] = CellType.Stairs;
                            grid[prev + verticalOffset + horizontalOffset * 2] = CellType.Stairs;

                            PlaceStairs(prev + horizontalOffset);
                            PlaceStairs(prev + horizontalOffset * 2);
                            PlaceStairs(prev + verticalOffset + horizontalOffset);
                            PlaceStairs(prev + verticalOffset + horizontalOffset * 2);
                        }

                        Debug.DrawLine(prev + new Vector3(0.5f, 0.5f, 0.5f), current + new Vector3(0.5f, 0.5f, 0.5f), Color.blue, 100, false);
                       */
                        pathVector tmpPath = new pathVector();
                        tmpPath.sourVector = prev;
                        tmpPath.destVector = current;
                        pathLst.Add(tmpPath);

                    }
                }

                
                foreach (var pos in path) { //两个房间之间的一条路径
                    if (grid[pos] == CellType.Hallway) {
                        PlaceHallway(pos);

                        //add by csd begin
                        bool isCreate = grid.getDataIsCreate(pos);
                        if (isCreate == false)
                        {
                            HallWay newHallWay = new HallWay(pos, new Vector3Int(1, 1, 1), planePrefab, wallPrefab, hallWayPlaceMaterial, placeGrid);
                            hallways.Add(newHallWay);
                            grid.setGridDataObj(newHallWay, pos);
                            grid.setDataIsCreate(pos,true);
                        }
                        //add end
                    }
                }
            }
        }
    }


    public void hideAllCube() {
        hideAllCellType(cubeDebugLst);
    }

    public void hideAllHallway() {
        hideAllCellType(hallwayDebugLst);
    }

    public void hideAllStairs() {
        hideAllCellType(stairDebugLst);
    }

    private void hideAllCellType(List<GameObject> objLst) {
        for (int i = 0; i < objLst.Count; i++) {
            objLst[i].SetActive(false);
        }
    }

    private void PlaceCube(Vector3Int location, Vector3Int size, Material material, string cubeName = "cube") {
        GameObject go = Instantiate(cubePrefab, location, Quaternion.identity);
        go.GetComponent<Transform>().localScale = size;
        go.GetComponent<MeshRenderer>().material = material;
        go.name = cubeName;

        //add by csd begin
        if (cubeName == "cube")
        {
            cubeDebugLst.Add(go);
        }
        else if (cubeName == "Hallway")
        {
            hallwayDebugLst.Add(go);
        }
        else if (cubeName == "Stairs") {
            stairDebugLst.Add(go);
        }
       //add end
    }

    //房间
    private void PlaceRoom(Vector3Int location, Vector3Int size) {
        PlaceCube(location, size, redMaterial);

    }

    //过道
    private void PlaceHallway(Vector3Int location) {
        PlaceCube(location, new Vector3Int(1, 1, 1), blueMaterial, "Hallway");
    }

    //楼梯
    private void PlaceStairs(Vector3Int location) {
        PlaceCube(location, new Vector3Int(1, 1, 1), greenMaterial, "Stairs");
    }

    //传入地板的世界坐标， 墙的位置， 返回墙的gameobject
    private GameObject getWallObj(Vector3Int pPos, WallPos pWallPos) {
        GameObject res = null;
        placeWall tmpPlaceWall = placeGrid[pPos];
        if (pWallPos == WallPos.Bottom) {
            res = tmpPlaceWall.bottomWall;
        }
        else if (pWallPos == WallPos.Top)
        {
            res = tmpPlaceWall.topWall;
        }
        else if (pWallPos == WallPos.Left)
        {
            res = tmpPlaceWall.leftWall;
        }
        else if (pWallPos == WallPos.Right)
        {
            res = tmpPlaceWall.rightWall;
        }

        return res;
    }

    //获得坐标对应的地板
    private placeWall getPlaceWallByPos(Vector3Int pPos) {
        placeWall tmpPlaceWall = placeGrid[pPos];
        return tmpPlaceWall;
    }

    //判断是否为房间或过道
    private bool isRoomOrHillWay(Vector3Int pPos)
    {
        bool res = false;
        CellType tmpType = grid[pPos];
        if ((tmpType == CellType.Room) || (tmpType == CellType.Hallway))
            res = true;
        return res;
    }

    //把XY值相同的，Z相交集的墙隐藏, 
    private void setHideWallByZ(Vector3Int pTop, Vector3Int pBottom, bool pIsRoomOrHillWay1, bool pIsRoomOrHillWay2) {
        if (pIsRoomOrHillWay1) { 
            placeWall pTopPlaceWallObj = getPlaceWallByPos(pTop);
            pTopPlaceWallObj.bottomWall.SetActive(false);
        }

        if (pIsRoomOrHillWay2)
        {
            placeWall pBottomPlaceWallObj = getPlaceWallByPos(pBottom);
            pBottomPlaceWallObj.topWall.SetActive(false);
        }
    }

    //把YZ值相同的，X相交集的墙隐藏, 默认Y的都是同一层为0
    private void setHideWallByX(Vector3Int pLeft, Vector3Int pRight, bool pIsRoomOrHillWay1, bool pIsRoomOrHillWay2) {
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



    /*
private void setHideWallByX(int pPosY, int pPosZ, Room pLeft, Room pRight)
{
   // int pPosY = pLeft.pos.y;
    placeWall pLeftWallObj = pLeft.getSpaceWall(new Vector3Int(pLeft.bounds.size.x-1, pPosY, pPosZ - pLeft.pos.z));
    pLeftWallObj.rightWall.SetActive(false);

    placeWall pRightWallObj = pRight.getSpaceWall(new Vector3Int(0, pPosY, pPosZ - pRight.pos.z));
    pRightWallObj.leftWall.SetActive(false);
}

    */
    //判断两个相交的地块之间哪面墙要敲掉, 发现相交时返回true
    //源地板，  目标地板
    private bool judgePlaceDelWall(Vector3Int sourPos, Vector3Int destPos) {
        bool res = false;
        bool sourNeed = false; //是否要敲墙
        bool destNeed = false; //是否要敲墙

        Vector3Int sourPosParent = Vector3Int.zero;
        Vector3Int destPosParent = Vector3Int.zero;
        if (isRoomOrHillWay(sourPos))
            sourNeed = true;

        if (isRoomOrHillWay(destPos))
            destNeed = true;
        /*
        if ((grid[sourPos] == CellType.None) || (grid[destPos] == CellType.None)) {
            Debug.LogError("judgePlaceDelWall ");
            return res;
        }
        */
        if (sourNeed && destNeed)  //判断是否为同一个房间,只有房间和过道才加入， 楼梯的路径点未加入gird
        {
            Room sourRoom = grid.getGridDataObj(sourPos);
            Room destRoom = grid.getGridDataObj(destPos);

            if (sourRoom.pos == destRoom.pos)
            {
                Debug.LogWarning("is same place");
                return false;
            }
        }
        /*
        bool isSamePosY = false;

        int isSameCount = 0; //统计有几个值是相同的，若y值不同，最多x,z相同
                             //世界坐标系，基于左下角，大机率猜测：两相交的room中x,y,z必然有1-2个值相同, 需后续验证

        //先只考虑平面情况下的相交
        if (sourRoom.pos.y == destRoom.pos.y)
        {
            isSamePosY = true;
            isSameCount++;
        }
        
        if (isSamePosY)*/
        //if (sourPos.y == destPos.y)
        {
            if (sourPos.x == destPos.x)
            {  //Z轴相交集， X相同的
                if (sourPos.y == destPos.y) //Y轴相同
                {
                    //if (sourPos.z == destPos.z + 1) //sourPos在destPos的上面
                    if (sourPos.z > destPos.z)
                    {
                        setHideWallByZ(sourPos, destPos, sourNeed, destNeed);
                    }
                    else //if (sourPos.z + 1 == destPos.z)
                    {
                        setHideWallByZ(destPos, sourPos, destNeed, sourNeed);
                    }
                }
               
                    
               
            }
            else if (sourPos.z == destPos.z)
            { //X轴相交集， Z相同的

                if(sourPos.x > destPos.x)
                //if (sourPos.x == destPos.x + 1) //sourPos在destPos的右边
                {
                    setHideWallByX(destPos, sourPos, destNeed, sourNeed);
                }
                else //if (sourPos.x + 1 == destPos.x)
                {
                    setHideWallByX(sourPos, destPos, sourNeed, destNeed);
                }

            }

        }


        /*
        if (isSamePosY)
        { //平面相关

            //Z轴相交集， X相同的
            for (int x1 = 0; x1 < sourRoom.bounds.size.x; x1++)
            {
                int xSour = sourRoom.bounds.position.x + x1;
                for (int x2 = 0; x2 < destRoom.bounds.size.x; x2++)
                {
                    int xDest = destRoom.bounds.position.x + x2;
                    if (xSour == xDest)
                    { //z轴上有相交，把这两个room的这两个色块
                      //x相同， 判断sSour在xDest的z轴上方还是z轴上方, z与z-1的地块敲墙
                        if (sourRoom.bounds.position.z == destRoom.bounds.position.z + destRoom.bounds.size.z)
                        {
                            Vector3Int topVect3 = new Vector3Int(xSour, sourRoom.pos.y, sourRoom.bounds.position.z);
                            Vector3Int bottomVect3 = new Vector3Int(xSour, sourRoom.pos.y, sourRoom.bounds.position.z - 1);
                            setHideWallByZ(topVect3, bottomVect3);

                            //setHideWallByZ(0,xSour, sourRoom, destRoom);
                        }
                        else if (sourRoom.bounds.position.z + sourRoom.bounds.size.z == destRoom.bounds.position.z)
                        {
                            Vector3Int topVect3 = new Vector3Int(xSour, sourRoom.pos.y, destRoom.bounds.position.z);
                            Vector3Int bottomVect3 = new Vector3Int(xSour, sourRoom.pos.y, destRoom.bounds.position.z - 1);
                            setHideWallByZ(topVect3, bottomVect3);

                            //setHideWallByZ(0,xSour, destRoom, sourRoom);
                        }
                        else
                        {
                            Debug.LogError("");
                        }

                    }

                }
            }

            //X轴相交集， Z相同的
            for (int z1 = 0; z1 < sourRoom.bounds.size.z; z1++)
            {
                int zSour = sourRoom.bounds.position.z + z1;
                for (int z2 = 0; z2 < destRoom.bounds.size.z; z2++)
                {
                    int zDest = destRoom.bounds.position.z + z2;
                    if (zSour == zDest)
                    {
                        //x轴上有相交，把这两个room的这两个色块
                        //z相同， 判断Sour在Dest的x轴左方还是x轴右方   x与x-1位置
                        if (sourRoom.bounds.position.x == destRoom.bounds.position.x + destRoom.bounds.size.x) //sour在右边
                        {
                            Vector3Int leftVect3 = new Vector3Int(sourRoom.bounds.position.x - 1, sourRoom.pos.y, zSour);
                            Vector3Int rightVect3 = new Vector3Int(sourRoom.bounds.position.x, sourRoom.pos.y, zSour);
                            setHideWallByX(leftVect3, rightVect3);

                           // setHideWallByX(0, zSour, destRoom,sourRoom);
                        }
                        else if (sourRoom.bounds.position.x + sourRoom.bounds.size.x == destRoom.bounds.position.x) //sour在左边
                        {
                            Vector3Int leftVect3 = new Vector3Int(destRoom.bounds.position.x - 1, sourRoom.pos.y, zSour);
                            Vector3Int rightVect3 = new Vector3Int(destRoom.bounds.position.x, sourRoom.pos.y, zSour);
                            setHideWallByX(leftVect3, rightVect3);

                            //setHideWallByX(0, zSour, sourRoom, destRoom);
                        }
                        else
                        {
                            Debug.LogError("");
                        }
                    }
                }
            }
        }

        */
        return res;
    }

}
