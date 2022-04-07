using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T> {
    T[] data;
    placeWall[] wallData; //add by csd, 记录每个格子对应的一个格子的地板及墙3D模型对象
    Room2D[] dataObjLst;  //add by csd，只是索引的记录，记录每个格子对应的Room2D, 一个Room2D存在多个格子，用于根据格子坐标查询room

  //  bool[] dataIsCreate; //Room继承于MonoBehaviour无法通过is null来判断（之前用new方式创建对象产生的）


    public Vector2Int Size { get; private set; }
    public Vector2Int Offset { get; set; } //目前均为zero

    public Grid2D(Vector2Int size, Vector2Int offset) {
        Size = size;
        Offset = offset;

        data = new T[size.x * size.y];
        //add by csd
        initData();
        //add by csd end
    }

    public void initData() {
        int maxCount = Size.x * Size.y;
        dataObjLst = new Room2D[maxCount];
        wallData = new placeWall[maxCount];
       // dataIsCreate = new bool[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
         //   dataIsCreate[i] = false;
            dataObjLst[i] = null;
            wallData[i] = null;
        }
    }

    public void clearData(T value) {
        int maxCount = Size.x * Size.y;
        for (int i = 0; i < maxCount; i++)
        {
            data[i] = value;
            wallData[i] = null;
        }
    }

    //add by csd
    /*
    private void setDataIsCreate(Vector2Int pos, bool value)
    {
        dataIsCreate[GetIndex(pos)] = value;
    }
    */
    public bool getDataIsCreate(Vector2Int pos)
    {
        bool res = false;
        //return dataIsCreate[GetIndex(pos)];
        Room2D tmpData = dataObjLst[GetIndex(pos)];
        if (tmpData != null)
            res = true;
        else
            res = false;
        return res;
        
    }

    public void setGridDataObj(Room2D obj, Vector2Int pos)
    {
        dataObjLst[GetIndex(pos)] = obj;
      //  this.setDataIsCreate(pos, true);
    }

    public void setPlaceIndex(GameObject pObj, Vector2Int tmpPos)
    {
        placeWall placeObj = pObj.GetComponent<placeWall>();
        wallData[GetIndex(tmpPos)] = placeObj;
       // placeGrid[tmpPos] = placeObj;
    }

    public placeWall getPlaceWallObj(Vector2Int pos) {
        placeWall res = wallData[GetIndex(pos)];
        return res;
    }


    public Room2D getGridDataObj(Vector2Int pos)
    {
        Room2D res = dataObjLst[GetIndex(pos)];
        return res;
    }
    //add end

    public int GetIndex(Vector2Int pos) {
        return pos.x + (Size.x * pos.y);
    }

    public bool InBounds(Vector2Int pos) {
        return new RectInt(Vector2Int.zero, Size).Contains(pos + Offset);
    }

    public T this[int x, int y] {
        get {
            return this[new Vector2Int(x, y)];
        }
        set {
            this[new Vector2Int(x, y)] = value;
        }
    }

    public T this[Vector2Int pos] {
        get {
            pos += Offset;
            return data[GetIndex(pos)];
        }
        set {
            pos += Offset;
            data[GetIndex(pos)] = value;
        }
    }
}
