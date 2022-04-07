using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2D : MonoBehaviour
{
    // Start is called before the first frame update
    public RectInt bounds;
    public Vector2Int pos;

    public GameObject planePrefab;
    public GameObject wallPrefab;
    public Material material;

    //private Grid3D<GameObject> placeLst;

    public Grid2D<CellType> placeGrid; //外部传入的placeGrid

    public int nameIndex; //名字区分

    public int placeIndex; //地板区分

    public string roomName; //地板名字

    public bool hasGetPath; //是否有路径到达

    private GameObject parentObj;

    private const int csPosY = 0;

    public virtual void initData(Vector2Int location, Vector2Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid2D<CellType> pGrid, int pNameIndex, GameObject pParentObj = null)
    {
        pos = location;
        bounds = new RectInt(location, size);
        planePrefab = pPlaneObj; //add by csd
        wallPrefab = pWallObj;
        material = pMaterial;

        placeGrid = pGrid;

        nameIndex = pNameIndex;

        parentObj = pParentObj;

        placeIndex = 0;

        roomName = "room";

        hasGetPath = false;
    }

    public Room2D()
    {
        nameIndex = 0;
        placeIndex = 0;
        roomName = "UpDownHill";
      //  bounds = new RectInt(location, size);
    }

   

    //public Room2D(Vector2Int location, Vector2Int size)
    public Room2D(Vector2Int location, Vector2Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid2D<CellType> pGrid, int pNameIndex, GameObject pParentObj = null)
    {
        initData(location, size, pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj);
    }


    public static bool IntersectBuff(RectInt a, RectInt b)
    {
        return !((a.position.x >= (b.position.x + b.size.x)) 
            || ((a.position.x + a.size.x) <= b.position.x)
            || (a.position.y >= (b.position.y + b.size.y)) 
            || ((a.position.y + a.size.y) <= b.position.y));
    }

    public static bool Intersect(Room2D a, Room2D b)
    {
        return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
            || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
    }

    public Vector2 getCenterPos() {
        float posX = bounds.position.x-0.5f + bounds.size.x / 2.0f;
        float posY = bounds.position.y-0.5f + bounds.size.y / 2.0f;
        Vector2 resVect2 = new Vector2(posX, posY);
        return resVect2;
    }

    private GameObject makePlane(int posX, int posZ)
    {
        int worldPosX = bounds.position.x + posX;
        int worldPosY = csPosY;
        int worldPosZ = bounds.position.y + posZ;

        //GameObject go = Instantiate(planePrefab, new Vector3(worldPosX + 0.5f, worldPosY, worldPosZ + 0.5f), Quaternion.identity);
        GameObject go = Instantiate(planePrefab, new Vector3(worldPosX , worldPosY, worldPosZ ), Quaternion.identity, parentObj.transform);
        //go.GetComponent<MeshRenderer>().material = material;
        go.name = roomName+ nameIndex.ToString()+"_"+placeIndex.ToString();

        placeIndex++;
        return go;
    }

    public void makeAllPlane()
    {
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                GameObject tmpPlane = makePlane(x, y);
                placeGrid.setPlaceIndex(tmpPlane, new Vector2Int(pos.x + x, pos.y + y));
               // setPlaceIndex(tmpPlane, new Vector2Int(pos.x + x, pos.y + y));
                //flagPlacePos(tmpPlane, x, 0, z);
            }
        }
    }
    /*
    private void setPlaceIndex(GameObject pObj, Vector2Int tmpPos)
    {
        placeWall placeObj = pObj.GetComponent<placeWall>();
        placeGrid[tmpPos] = placeObj;
    }
    */
    private GameObject makeWall(int posX, int posZ, float rotationX = 0.0f, float rotationY = 0.0f, float rotationZ = 0.0f)
    {
        int worldPosX = bounds.position.x + posX;
        int worldPosY = csPosY;
        int worldPosZ = bounds.position.y + posZ;

        //float offsetX = 0.5f;
        //float offsetY = 0.5f;

        //GameObject go = Instantiate(wallPrefab, new Vector3(worldPosX+ offsetX, worldPosY, worldPosZ+ offsetY), Quaternion.identity);
        GameObject go = Instantiate(wallPrefab, new Vector3(worldPosX, worldPosY, worldPosZ), Quaternion.identity, parentObj.transform);
        go.transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);
        return go;
    }
    /*
    public placeWall getSpaceWall()
    {
        placeWall tmpSpaceWall = null;
        if (placeGrid[pos] != null)
        {
            tmpSpaceWall = placeGrid[pos].GetComponent<placeWall>();
            return tmpSpaceWall;
        }
        else
        {
            Vector3Int tmpPos = new Vector3Int(pos.x, csPosY, pos.y);
            string msg = "getSpaceWall not find plance pos:" + DebugMsg.instance.Msg(tmpPos);
            Debug.LogError(msg);
        }
        return tmpSpaceWall;
    }
    */
    private void setWallIndex(GameObject wallObj, Vector2Int placePos, WallPos tmpWallPos)
    {
        placeWall tmpSpaceWall = placeGrid.getPlaceWallObj(placePos);
        //placeWall tmpSpaceWall = placeGrid[placePos]; //获得地块对象
        if (tmpSpaceWall != null)
        {

            switch (tmpWallPos)
            {
                case WallPos.Top:
                    {
                        tmpSpaceWall.topWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Top";
                    }
                    break;
                case WallPos.Bottom:
                    {
                        tmpSpaceWall.bottomWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Bottom";
                    }
                    break;
                case WallPos.Left:
                    {
                        tmpSpaceWall.leftWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Left";
                    }
                    break;
                case WallPos.Right:
                    {
                        tmpSpaceWall.rightWall = wallObj;
                        wallObj.name = tmpSpaceWall.gameObject.name + "Right";
                    }
                    break;
                    // default: { } break;

            }
        }
        else
        {
            Vector3Int tmpPos = new Vector3Int(pos.x, csPosY, pos.y);
            string msg = "setWallIndex error! Wall not find ! pos " + DebugMsg.instance.Msg(tmpPos) + "\n";
            Debug.LogError(msg);
        }
    }


    public void makeTopBottomWall()
    {
        GameObject tmpWallObj = null;
        //x正轴朝右，z正轴朝上， 默认在左边，  只需旋转Y轴， 下面的墙需旋转270度， 上面90度
        for (int x = 0; x < bounds.size.x; x++)
        {
            int y = 0;  //下面的墙
            tmpWallObj = makeWall(x, y, 0.0f, 270.0f, 0.0f);   //下面的墙需旋转270度, 

            // if (isSetWallIndex)
            setWallIndex(tmpWallObj, new Vector2Int(pos.x + x, pos.y + y), WallPos.Bottom);

            y = bounds.size.y - 1;  //上面的墙
            tmpWallObj = makeWall(x, y, 0.0f, 90.0f, 0.0f);     //上面90度

            //  if (isSetWallIndex)
            setWallIndex(tmpWallObj, new Vector2Int(pos.x + x, pos.y + y), WallPos.Top);
        }
    }

    public void makeLeftRightWall(bool isSetWallIndex = true)
    {
        GameObject tmpWallObj = null;

        //x正轴朝右，z正轴朝上， 默认在左边，  只需旋转Y轴， 左边的墙需旋转0度， 右边的墙180度
        for (int y = 0; y < bounds.size.y; y++)
        {
            int x = 0;  //左边的墙
            tmpWallObj = makeWall(x, y, 0.0f, 0.0f, 0.0f);

            // if (isSetWallIndex)
            setWallIndex(tmpWallObj, new Vector2Int(pos.x + x, pos.y + y), WallPos.Left);

            x = bounds.size.x - 1;  //右边的墙
            tmpWallObj = makeWall(x, y, 0.0f, 180.0f, 0.0f);

            // if (isSetWallIndex)
            setWallIndex(tmpWallObj, new Vector2Int(pos.x + x, pos.y + y), WallPos.Right);

        }
    }

    //铺四面的墙
    public void makeAllWall()
    {
        makeTopBottomWall();
        makeLeftRightWall();
    }

}
