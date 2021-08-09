using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairWay : MonoBehaviour
{
  //  public GameObject planePrefab;
    public GameObject wallPrefab;
    //  public Material material;
    //  private Grid3D<placeWall> placeGrid;
    // private Grid3D<cellType2.CellType> grid;
    public delegate bool isRoomFunc(Vector3Int pos, out placeWall pRoom);

    private isRoomFunc isRoomByPos;

    private GameObject upHillPrefab = null;  //上坡资源

    private GameObject downHillPrefab = null; //下坡资源

    private Vector3Int prev; //路径起始点

    private Vector3Int current; //路径的终点

    private bool isUpHill; //是否上坡

    private Vector3Int PlaceStairs1; //楼梯的起始
    private Vector3Int PlaceStairs4; //楼梯的终点位置

    private int nameIndex; //名字序号

    private GameObject parentObj;

    const string csStairWayName = "UpDownHill";
    //private Vector3Int PlaceStairsAir; //空中的格子


    /*
    public stairWay(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid) : base(location, size, pPlaneObj, pWallObj, pMaterial, pGrid)
    {
        
    }
    */

    public void initData(GameObject pWallObj, isRoomFunc pCallBack,
                    GameObject pUpHill, GameObject pDownHill,
                   Vector3Int pPrev, Vector3Int pCurrent,
                   Vector3Int pPlaceStairs1, Vector3Int pPlaceStairs2, Vector3Int pPlaceStairs3, Vector3Int pPlaceStairs4,
                   int pNameIndex, GameObject pParentObj) {

        wallPrefab = pWallObj;
        isRoomByPos = pCallBack;


        upHillPrefab = pUpHill;
        downHillPrefab = pDownHill;
        prev = pPrev;
        current = pCurrent;

        PlaceStairs1 = pPlaceStairs1;
        PlaceStairs4 = pPlaceStairs4;
        parentObj = pParentObj;

        //上坡1和4色块, 空中为3色块，无需处理的实心为2色块
        //下坡1和4色块，空中为2色块，无需处理的实心为3色块
        if (prev.y < current.y) //上坡
        {
            isUpHill = true;
            // PlaceStairsAir = pPlaceStairs3;
        }
        else if (prev.y > current.y) //下坡
        {
            isUpHill = false;
            //  PlaceStairsAir = pPlaceStairs2;
        }
        else
        {
            Debug.LogError("stairWay error y is same");
        }

        nameIndex = pNameIndex;
    }

    //设置楼梯的参数
    public stairWay(GameObject pWallObj, isRoomFunc pCallBack,
                    GameObject pUpHill, GameObject pDownHill,
                   Vector3Int pPrev, Vector3Int pCurrent,
                   Vector3Int pPlaceStairs1,Vector3Int pPlaceStairs2, Vector3Int pPlaceStairs3, Vector3Int pPlaceStairs4, 
                   int pNameIndex, GameObject pParentObj)
    {

        initData( pWallObj,  pCallBack,
                     pUpHill,  pDownHill,
                    pPrev,  pCurrent,
                    pPlaceStairs1,  pPlaceStairs2,  pPlaceStairs3,  pPlaceStairs4,
                    pNameIndex,  pParentObj);

        /*
        wallPrefab = pWallObj;
        isRoomByPos = pCallBack;
       

        upHillPrefab = pUpHill;
        downHillPrefab = pDownHill;
        prev = pPrev;
        current = pCurrent;

        PlaceStairs1 = pPlaceStairs1;
        PlaceStairs4 = pPlaceStairs4;
        parentObj = pParentObj;

        //上坡1和4色块, 空中为3色块，无需处理的实心为2色块
        //下坡1和4色块，空中为2色块，无需处理的实心为3色块
        if (prev.y < current.y) //上坡
        {
            isUpHill = true;
           // PlaceStairsAir = pPlaceStairs3;
        }
        else if(prev.y > current.y) //下坡
        {
            isUpHill = false;
          //  PlaceStairsAir = pPlaceStairs2;
        }
        else
        {
            Debug.LogError("stairWay error y is same");
        }

        nameIndex = pNameIndex;
        */
    }

    
    public void makeStairWay() {
        //生成楼梯地板1，4,并拆掉相邻房间的墙
        makeHillPlace();
        //makeHillPlace(PlaceStairs4);

        //生成墙
        makeHillWall(PlaceStairs1, csStairWayName + nameIndex.ToString() + "_1_");
        makeHillWall(PlaceStairs4, csStairWayName + nameIndex.ToString() + "_4_");
        //makeHillWall(PlaceStairsAir);
    }

    //生成上坡或下坡的地板,输入当前色块的坐标
    private void makeHillPlace()
    {
       // GameObject hillPrefab = null;
        GameObject hill1Prefab = null;
        GameObject hill4Prefab = null;
        float rotationY = 0.0f;

        if (isUpHill)
        { //上坡
           // hillPrefab = upHillPrefab;
            hill1Prefab = upHillPrefab;
            hill4Prefab = downHillPrefab;

        }
        else 
        { //下坡
          //  hillPrefab = downHillPrefab;
            hill1Prefab = downHillPrefab;
            hill4Prefab = upHillPrefab;
        }


        //Vector3Int preRoomPos = Vector3Int.zero;
        //Vector3Int currentRoomPos = Vector3Int.zero;


        placeWall preRoom = null; // new placeWall();
        placeWall currentRoom = null; // new placeWall();
        bool preIsRoom = isRoomByPos(prev, out preRoom);
        bool currentIsRoom = isRoomByPos(current, out currentRoom);
        
        //默认是z轴正方向
        if (prev.z < current.z) //z轴正方向
        {
            rotationY = 0.0f;
            if (preIsRoom)
                preRoom.topWall.SetActive(false);
            if (currentRoom)
                currentRoom.bottomWall.SetActive(false);
        }

        else if (prev.x < current.x)  //x轴正方向  
        {
            rotationY = 90.0f;
            if (preIsRoom)
                preRoom.rightWall.SetActive(false);
            if (currentRoom)
                currentRoom.leftWall.SetActive(false);
        }
        
        
        else if (prev.z > current.z) //z轴负方向
        {
            rotationY = 180.0f;
            if (preIsRoom)
                preRoom.bottomWall.SetActive(false);
            if (currentRoom)
                currentRoom.topWall.SetActive(false);

        }
        else if (prev.x > current.x)  //x轴负方向  
        {
            rotationY = 270.0f;
            if (preIsRoom)
                preRoom.leftWall.SetActive(false);
            if (currentRoom)
                currentRoom.rightWall.SetActive(false);
        }

        // GameObject go = Instantiate(hillPrefab, new Vector3(staticStair.x +0.5f, staticStair.y, staticStair.z+0.5f), Quaternion.identity);
        GameObject go1 = Instantiate(hill1Prefab, new Vector3(PlaceStairs1.x, PlaceStairs1.y, PlaceStairs1.z), Quaternion.identity, parentObj.transform);
        go1.transform.eulerAngles = new Vector3(0.0f, rotationY, 0.0f);
        go1.name = csStairWayName + nameIndex.ToString() + "_1_";

        GameObject go4 = Instantiate(hill4Prefab, new Vector3(PlaceStairs4.x, PlaceStairs4.y, PlaceStairs4.z), Quaternion.identity, parentObj.transform);
        go4.transform.eulerAngles = new Vector3(0.0f, rotationY + 180.0f, 0.0f);
        go4.name = csStairWayName + nameIndex.ToString() + "_4_";
    }

    //生成铺上坡或下坡的墙, 输入 路径的前后坐标, 当前色块的坐标
    private void makeHillWall(Vector3Int staticStair, string pName)
    {

        //Room tmpRoom = new Room(staticStair, new Vector3Int(1, 1, 1), null, wallPrefab, null, null, 0, parentObj);
        Room tmpRoom = parentObj.AddComponent<Room>();
        tmpRoom.initData(staticStair, new Vector3Int(1, 1, 1), null, wallPrefab, null, null, 0, parentObj);

        if (prev.x == current.x)
        {  //x值相同，z值不同， 只生成left,right的墙
            tmpRoom.makeStairLeftRightWall(pName);
        }
        else if (prev.z == current.z)
        { //x值不同，z值相同,  只生成top,bottom的墙
            tmpRoom.makeStairTopBottomWall(pName);
        }
    }

}
