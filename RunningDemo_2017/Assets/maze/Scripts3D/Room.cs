﻿using System.Collections;
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
    // private Grid3D<GameObject> wallLst;
    public Room(Vector3Int location, Vector3Int size, GameObject pPlaneObj, GameObject pWallObj, Material pMaterial, Grid3D<placeWall> pGrid)
    {
        pos = location;
        bounds = new BoundsInt(location, size);
        planePrefab = pPlaneObj; //add by csd
        wallPrefab = pWallObj;
        material = pMaterial;

        placeGrid = pGrid;

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

        GameObject go = Instantiate(planePrefab, new Vector3(worldPosX + 0.5f, worldPosY, worldPosZ + 0.5f), Quaternion.identity);
        go.GetComponent<MeshRenderer>().material = material;
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

        GameObject go = Instantiate(wallPrefab, new Vector3(worldPosX, worldPosY, worldPosZ), Quaternion.identity);
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
    
    //获得每一个地板对应的墙, 传入相对坐标, 后续不再使用
    public placeWall getSpaceWall(Vector3Int tmpPos) {
        placeWall tmpSpaceWall = null;
        if (placeLst[tmpPos] != null)
        {
            tmpSpaceWall = placeLst[tmpPos].GetComponent<placeWall>();
            return tmpSpaceWall;
        }
        else {
            string msg = "getSpaceWall not find plance pos:" + DebugMsg.instance.Msg(tmpPos);
            Debug.LogError(msg);
        }
        return tmpSpaceWall;
    }
    */

    //设置墙与地板的索引，输入 墙的对象，地块的世界坐标，墙在地板中的位置
    private void setWallIndex(GameObject wallObj, Vector3Int placePos, WallPos tmpWallPos) {
        placeWall tmpSpaceWall = placeGrid[placePos]; //获得地块对象
        if (tmpSpaceWall != null) {
            switch (tmpWallPos)
            {
                case WallPos.Top: { tmpSpaceWall.topWall = wallObj; } break;
                case WallPos.Bottom: { tmpSpaceWall.bottomWall = wallObj; } break;
                case WallPos.Left: { tmpSpaceWall.leftWall = wallObj; } break;
                case WallPos.Right: { tmpSpaceWall.rightWall = wallObj; } break;
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
    public void makeTopBottomWall(bool isSetWallIndex = true) {
        GameObject tmpWallObj = null;
        for (int x = 0; x < bounds.size.x; x++)
        {
            int z = 0;  //下面的墙
            tmpWallObj = makeWall(x + 1, 0, z, 90.0f, 0.0f, 0.0f);   //X轴90度, X+1, 

            if(isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Bottom);
            //flagWallPos(tmpWallObj, x,0,z, WallPos.Bottom);

            z = bounds.size.z - 1;  //上面的墙
            tmpWallObj = makeWall(x, 0, z + 1, 90.0f, 0, 180.0f);     //X轴90度, X+1, 

            if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Top);
            // flagWallPos(tmpWallObj, x, 0, z, WallPos.Top);
        }
    }

    public void makeLeftRightWall(bool isSetWallIndex = true) {
        GameObject tmpWallObj = null;
        for (int z = 0; z < bounds.size.z; z++)
        {
            int x = 0;  //左边的墙
            tmpWallObj = makeWall(x, 1, z, 0, 180.0f, 90.0f);

            if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Left);
            //flagWallPos(tmpWallObj, x, 0, z, WallPos.Left);

            x = bounds.size.x - 1;  //右边的墙
            tmpWallObj = makeWall(x + 1, 1, z + 1, 0.0f, 0.0f, 90.0f);

            if (isSetWallIndex)
                setWallIndex(tmpWallObj, new Vector3Int(pos.x + x, pos.y, pos.z + z), WallPos.Right);
            //  flagWallPos(tmpWallObj, x, 0, z, WallPos.Right);
        }
    }

    //铺四面的墙
    public void makeAllWall()
    {
        makeTopBottomWall();
        makeLeftRightWall();

        /*

        GameObject tmpWallObj = null;
        //Y轴作为俯视， 左手坐标系， X向右
        //铺上下面的墙
        for (int x = 0; x < bounds.size.x; x++)
        {
            int z = 0;  //下面的墙
            tmpWallObj = makeWall(x + 1, 0, z, 90.0f, 0.0f, 0.0f);   //X轴90度, X+1, 

            setWallIndex(tmpWallObj, new Vector3Int(this.pos.x + x, this.pos.y, this.pos.z + z), WallPos.Bottom);
            //flagWallPos(tmpWallObj, x,0,z, WallPos.Bottom);

            z = bounds.size.z -1;  //上面的墙
            tmpWallObj = makeWall(x, 0, z + 1, 90.0f, 0, 180.0f);     //X轴90度, X+1, 

            setWallIndex(tmpWallObj, new Vector3Int(this.pos.x + x, this.pos.y, this.pos.z + z), WallPos.Top);
           // flagWallPos(tmpWallObj, x, 0, z, WallPos.Top);
        }

        //铺左右的墙
        for (int z = 0; z < bounds.size.z; z++)
        {
            int x = 0;  //左边的墙
            tmpWallObj = makeWall(x, 1, z, 0, 180.0f, 90.0f);

            setWallIndex(tmpWallObj, new Vector3Int(this.pos.x + x, this.pos.y, this.pos.z + z), WallPos.Left);
            //flagWallPos(tmpWallObj, x, 0, z, WallPos.Left);

            x = bounds.size.x -1;  //右边的墙
            tmpWallObj = makeWall(x + 1, 1, z + 1, 0.0f, 0.0f, 90.0f);

            setWallIndex(tmpWallObj, new Vector3Int(this.pos.x + x, this.pos.y, this.pos.z + z), WallPos.Right);
            //  flagWallPos(tmpWallObj, x, 0, z, WallPos.Right);
        }


        */
    }
}