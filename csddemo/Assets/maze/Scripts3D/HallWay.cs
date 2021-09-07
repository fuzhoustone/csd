using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;

public class HallWay : Room
{
    //public HallWay preHallWay;
    public HallWay(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid, int pNameIndex, GameObject pParentObj) : base(location, size, pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj)
    {
        roomName = "HallWay";
    }

    
    public void initDataHallWay(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid, int pNameIndex, GameObject pParentObj = null)
    {
        initData(location,size,pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj);
        
        roomName = "HallWay";
    }
    
    /*
    public override void initData(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid, int pNameIndex, GameObject pParentObj)
    {
        this.initData(location, size, pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj);

        roomName = "HallWay";
    }
    */
    /*
    //估计没用了
    public void calHallwayNoUseWall() {
        if ((preHallWay != null) && (preHallWay.pos.y == this.pos.y)) {
            spaceWall preSpaceWall = preHallWay.getSpaceWall(preHallWay.pos);
            spaceWall thisSpaceWall = preHallWay.getSpaceWall(this.pos);

            if (preHallWay.pos.x == this.pos.x) //判断上下
            {
                if (preHallWay.pos.z > this.pos.z) //pre 在 this 上面
                {
                    preSpaceWall.bottomWall.SetActive(false);
                    thisSpaceWall.topWall.SetActive(false);
                }
                else  //pre 在 this 下面
                {  
                    preSpaceWall.topWall.SetActive(false);
                    thisSpaceWall.bottomWall.SetActive(false);
                }
            }
            else if (preHallWay.pos.z == this.pos.z) { //判断左右
                if (preHallWay.pos.x < this.pos.x) //pre 在this的左边
                {
                    preSpaceWall.rightWall.SetActive(false);
                    thisSpaceWall.leftWall.SetActive(false);
                }
                else
                { //pre 在this的右边
                    preSpaceWall.leftWall.SetActive(false);
                    thisSpaceWall.rightWall.SetActive(false);
                }
            }
        }
    }
    */
}
