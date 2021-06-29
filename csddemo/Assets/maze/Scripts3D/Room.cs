using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

enum WallPos
{
    Top,
    Bottom,
    Left,
    Right
}

public class Room : MonoBehaviour
{


    public BoundsInt bounds;
    public Vector3Int pos;

    public GameObject planePrefab;
    public GameObject wallPrefab;
    public Material material;

    //private Grid3D<GameObject> placeLst;

    public Grid3D<placeWall> placeGrid; //外部传入的placeGrid

    public int nameIndex; //名字区分

    public int placeIndex; //地板区分

    public string roomName; //地板名字

    private GameObject parentObj;
    // private Grid3D<GameObject> wallLst;
    public Room() {
        nameIndex = 0;
        placeIndex = 0;
        roomName = "UpDownHill";
    }

    public Room(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid, int pNameIndex, GameObject pParentObj = null)
    {
        pos = location;
        bounds = new BoundsInt(location, size);
        planePrefab = pPlaneObj; //add by csd
        wallPrefab = pWallObj;
        material = pMaterial;

        placeGrid = pGrid;

        nameIndex = pNameIndex;

        parentObj = pParentObj;

        placeIndex = 0;

        roomName = "room";

       // placeLst = new Grid3D<GameObject>(size, Vector3Int.zero);

        //add end

    }

    /*
     //a在b的x,y,z轴的是否不相交
         */
    public static bool Intersect(Room a, Room b) //判断b是否与a有交集， 后续考虑取消 !符号
    {
        return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) 
             || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)  
             || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y))  
             || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y)  
             || (a.bounds.position.z >= (b.bounds.position.z + b.bounds.size.z))  
             || ((a.bounds.position.z + a.bounds.size.z) <= b.bounds.position.z));
    }

    //铺一块地板
    private GameObject makePlane(int posX, int posZ)
    {
        int worldPosX = bounds.position.x + posX;
        int worldPosY = bounds.position.y;
        int worldPosZ = bounds.position.z + posZ;

        //GameObject go = Instantiate(planePrefab, new Vector3(worldPosX + 0.5f, worldPosY, worldPosZ + 0.5f), Quaternion.identity);
        GameObject go = Instantiate(planePrefab, new Vector3(worldPosX , worldPosY, worldPosZ ), Quaternion.identity, parentObj.transform);
        go.GetComponent<MeshRenderer>().material = material;
        go.name = roomName+ nameIndex.ToString()+"_"+placeIndex.ToString();

        placeIndex++;
        return go;
    }
    //铺地板
    public void makeAllPlane()
    {
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int z = 0; z < bounds.size.z; z++)
            {
                GameObject tmpPlane = makePlane(x, z);
                setPlaceIndex(tmpPlane, new Vector3Int(pos.x+x, pos.y, pos.z+z));
                //flagPlacePos(tmpPlane, x, 0, z);
            }
        }
    }

    //铺一面墙
    private GameObject makeWall(int posX, int posY, int posZ, float rotationX = 0.0f, float rotationY = 0.0f, float rotationZ = 0.0f)
    {
        int worldPosX = bounds.position.x + posX;
        int worldPosY = bounds.position.y + posY;
        int worldPosZ = bounds.position.z + posZ;

        //float offsetX = 0.5f;
        //float offsetY = 0.5f;

        //GameObject go = Instantiate(wallPrefab, new Vector3(worldPosX+ offsetX, worldPosY, worldPosZ+ offsetY), Quaternion.identity);
        GameObject go = Instantiate(wallPrefab, new Vector3(worldPosX , worldPosY, worldPosZ ), Quaternion.identity, parentObj.transform);
        go.transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        return go;
    }

    //标记地板的位置
    private void setPlaceIndex(GameObject  pObj, Vector3Int tmpPos) {
        placeWall placeObj =  pObj.GetComponent<placeWall>();
        placeGrid[tmpPos] = placeObj;
    }



    /*
    private void flagPlacePos(GameObject pObj, int x, int y, int z)
    {
        Vector3Int tmpPos = new Vector3Int(x, y, z);
        placeLst[tmpPos] = pObj;
    }
     */
    //获得地板对应的墙
    public placeWall getSpaceWall() {
        placeWall tmpSpaceWall = null;
        if (placeGrid[pos] != null)
        {
            tmpSpaceWall = placeGrid[pos].GetComponent<placeWall>();
            return tmpSpaceWall;
        }
        else {
            string msg = "getSpaceWall not find plance pos:" + DebugMsg.instance.Msg(pos);
            Debug.LogError(msg);
        }
        return tmpSpaceWall;
    }
   

    //设置墙与地板的索引，输入 墙的对象，地块的世界坐标，墙在地板中的位置
    private void setWallIndex(GameObject wallObj, Vector3Int placePos, WallPos tmpWallPos) {
        placeWall tmpSpaceWall = placeGrid[placePos]; //获得地块对象
        if (tmpSpaceWall != null) {
            
            switch (tmpWallPos)
            {
                case WallPos.Top: { tmpSpaceWall.topWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Top";
                    } break;
                case WallPos.Bottom: { tmpSpaceWall.bottomWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Bottom";
                    } break;
                case WallPos.Left: { tmpSpaceWall.leftWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Left";
                    } break;
                case WallPos.Right: { tmpSpaceWall.rightWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Right";
                    } break;
                    // default: { } break;

            }
        }
        else
        {
            string msg = "setWallIndex error! Wall not find ! pos " + DebugMsg.instance.Msg(placePos) + "\n";
            Debug.LogError(msg);
        }
    }

    /*
    //标识墙的位置
    private void flagWallPos(GameObject pObj, int x,int y, int z, WallPos tmpWallPos) {


        Vector3Int tmpPos = new Vector3Int(x, y, z);
        if (placeLst[tmpPos] != null)
        {
            placeWall tmpSpaceWall = placeLst[tmpPos].GetComponent<placeWall>(); 
            if (tmpSpaceWall != null)
            {
                switch (tmpWallPos)
                {
                    case WallPos.Top: { tmpSpaceWall.topWall = pObj; } break;
                    case WallPos.Bottom: { tmpSpaceWall.bottomWall = pObj; } break;
                    case WallPos.Left: { tmpSpaceWall.leftWall = pObj; } break;
                    case WallPos.Right: { tmpSpaceWall.rightWall = pObj; } break;
                        // default: { } break;

                }
            }
            else {
                string msg = "flagWallPos error! spaceWall not find ! pos " + DebugMsg.instance.Msg(tmpPos) +"\n";
                Debug.LogError(msg);
            }
        }
        else {
            string msg = "flagWallPos error! place not find ! pos " + DebugMsg.instance.Msg(tmpPos) + "\n";
            Debug.LogError(msg);
        }

        //wallLst[pos] = pObj;
    }

    */
    //传入所属地块的唯一性名字
    public void makeStairTopBottomWall(string pName) {
        GameObject tmpWallObj = null;
        int x = 0;
        int z = 0;  //下面的墙
        tmpWallObj = makeWall(x, 0, z, 0.0f, 270.0f, 0.0f);   //下面的墙需旋转270度, 
        tmpWallObj.name = pName + "Bottom";

        z = bounds.size.z - 1;  //上面的墙
        tmpWallObj = makeWall(x, 0, z, 0.0f, 90.0f, 0.0f);     //上面90度
        tmpWallObj.name = pName + "Top";
    }

    //传入所属地块的唯一性名字
    public void makeStairLeftRightWall(string pName) {
        GameObject tmpWallObj = null;
        int x = 0;  //左边的墙
        int z = 0;
        tmpWallObj = makeWall(x, 0, z, 0.0f, 0.0f, 0.0f);
        tmpWallObj.name = pName + "Left";

        x = bounds.size.x - 1;  //右边的墙
        tmpWallObj = makeWall(x, 0, z, 0.0f, 180.0f, 0.0f);
        tmpWallObj.name = pName + "Right";
    }

    public void makeTopBottomWall() {
        GameObject tmpWallObj = null;
        //x正轴朝右，z正轴朝上， 默认在左边，  只需旋转Y轴， 下面的墙需旋转270度， 上面90度
        for (int x = 0; x < bounds.size.x; x++)
        {
            int z = 0;  //下面的墙
            tmpWallObj = makeWall(x, 0, z, 0.0f, 270.0f, 0.0f);   //下面的墙需旋转270度, 

           // if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Bottom);

            z = bounds.size.z - 1;  //上面的墙
            tmpWallObj = makeWall(x, 0, z, 0.0f, 90.0f, 0.0f);     //上面90度

          //  if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Top);
        }
    }

    public void makeLeftRightWall(bool isSetWallIndex = true) {
        GameObject tmpWallObj = null;

        //x正轴朝右，z正轴朝上， 默认在左边，  只需旋转Y轴， 左边的墙需旋转0度， 右边的墙180度
        for (int z = 0; z < bounds.size.z; z++)
        {
            int x = 0;  //左边的墙
            tmpWallObj = makeWall(x, 0, z, 0.0f, 0.0f, 0.0f);

           // if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Left);

            x = bounds.size.x - 1;  //右边的墙
            tmpWallObj = makeWall(x, 0, z, 0.0f, 180.0f, 0.0f);

           // if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Right);
            
        }
    }

    //铺四面的墙
    public void makeAllWall()
    {
        makeTopBottomWall();
        makeLeftRightWall();
    }
}
