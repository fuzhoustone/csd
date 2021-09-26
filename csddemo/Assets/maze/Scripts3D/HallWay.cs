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
}

public class HallWay2D : Room2D
{
    //public HallWay preHallWay;
    public HallWay2D(Vector2Int location, Vector2Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid2D<placeWall> pGrid, int pNameIndex, GameObject pParentObj) : base(location, size, pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj)
    {
        roomName = "HallWay";
    }


    public void initDataHallWay(Vector2Int location, Vector2Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid2D<placeWall> pGrid, int pNameIndex, GameObject pParentObj = null)
    {
        initData(location, size, pPlaneObj, pWallObj, pMaterial, pGrid, pNameIndex, pParentObj);

        roomName = "HallWay";
    }
}
