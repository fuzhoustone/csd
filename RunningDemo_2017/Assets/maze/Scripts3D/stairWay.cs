using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairWay : MonoBehaviour
{
  //  public GameObject planePrefab;
    public GameObject wallPrefab;
  //  public Material material;
  //  private Grid3D<placeWall> placeGrid;

    private GameObject upHillPrefab = null;  //上坡资源

    private GameObject downHillPrefab = null; //下坡资源

    private Vector3Int prev; //路径起始点

    private Vector3Int current; //路径的终点

    private bool isUpHill; //是否上坡

    private Vector3Int PlaceStairs1; //楼梯的起始
    private Vector3Int PlaceStairs4; //楼梯的终点位置
    private Vector3Int PlaceStairsAir; //空中的格子


    /*
    public stairWay(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid) : base(location, size, pPlaneObj, pWallObj, pMaterial, pGrid)
    {
        
    }
    */
    //设置楼梯的参数
    public stairWay(GameObject pWallObj,  
                    GameObject pUpHill, GameObject pDownHill,
                   Vector3Int pPrev, Vector3Int pCurrent,
                   Vector3Int pPlaceStairs1,Vector3Int pPlaceStairs2, Vector3Int pPlaceStairs3, Vector3Int pPlaceStairs4)
    {

        wallPrefab = pWallObj;
       
       

        upHillPrefab = pUpHill;
        downHillPrefab = pDownHill;
        prev = pPrev;
        current = pCurrent;

        PlaceStairs1 = pPlaceStairs1;
        PlaceStairs4 = pPlaceStairs4;
        //上坡1和4色块, 空中为3色块，无需处理的实心为2色块
        //下坡1和4色块，空中为2色块，无需处理的实心为3色块
        if (prev.y < current.y) //上坡
        {
            isUpHill = true;
            PlaceStairsAir = pPlaceStairs3;
        }
        else if(prev.y > current.y) //下坡
        {
            isUpHill = false;
            PlaceStairsAir = pPlaceStairs2;
        }
        else
        {
            Debug.LogError("stairWay error y is same");
        }

    }

    
    public void makeStairWay() {
        //生成楼梯地板1，4
        makeHillPlace(PlaceStairs1);
        makeHillPlace(PlaceStairs4);

        //生成墙
        makeHillWall(PlaceStairs1);
        makeHillWall(PlaceStairs4);
        //makeHillWall(PlaceStairsAir);
    }

    //生成上坡或下坡的地板,输入当前色块的坐标
    private void makeHillPlace(Vector3Int staticStair)
    {
        GameObject hillPrefab = null;
        float rotationY = 0.0f;

        if (isUpHill)
        { //上坡
            hillPrefab = upHillPrefab;
        }
        else 
        { //下坡
            hillPrefab = downHillPrefab;
        }

        //默认是z轴正方向
        if (prev.z < current.z) //z轴正方向
        {
            rotationY = 0.0f;
        }

        else if (prev.x < current.x)  //x轴正方向  
        {
            rotationY = 90.0f;
        }
        
        
        else if (prev.z > current.z) //z轴负方向
        {
            rotationY = 180.0f;
        }
        else if (prev.x > current.x)  //x轴负方向  
        {
            rotationY = 270.0f;
        }

        // GameObject go = Instantiate(hillPrefab, new Vector3(staticStair.x +0.5f, staticStair.y, staticStair.z+0.5f), Quaternion.identity);
        GameObject go = Instantiate(hillPrefab, new Vector3(staticStair.x, staticStair.y, staticStair.z), Quaternion.identity);
        go.transform.eulerAngles = new Vector3(0.0f, rotationY, 0.0f);
    }

    //生成铺上坡或下坡的墙, 输入 路径的前后坐标, 当前色块的坐标
    private void makeHillWall(Vector3Int staticStair)
    {
        Room tmpRoom = new Room(staticStair, new Vector3Int(1, 1, 1), null, wallPrefab, null, null);
        if (prev.x == current.x)
        {  //x值相同，z值不同， 只生成left,right的墙
            tmpRoom.makeLeftRightWall(false);
        }
        else if (prev.z == current.z)
        { //x值不同，z值相同,  只生成top,bottom的墙
            tmpRoom.makeTopBottomWall(false);
        }
    }

}
