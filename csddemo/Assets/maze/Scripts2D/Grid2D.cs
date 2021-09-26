using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D<T> {
    T[] data;

    Room2D[] dataObjLst;  //add by csd
    bool[] dataIsCreate; //Room继承于MonoBehaviour无法通过is null来判断


    public Vector2Int Size { get; private set; }
    public Vector2Int Offset { get; set; }

    public Grid2D(Vector2Int size, Vector2Int offset) {
        Size = size;
        Offset = offset;

        data = new T[size.x * size.y];
        //add by csd
        int maxCount = size.x * size.y;
        dataObjLst = new Room2D[maxCount];
        dataIsCreate = new bool[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            dataIsCreate[i] = false;
        }
        //add by csd end
    }

    //add by csd
    private void setDataIsCreate(Vector2Int pos, bool value)
    {
        dataIsCreate[GetIndex(pos)] = value;
    }
    public bool getDataIsCreate(Vector2Int pos)
    {
        return dataIsCreate[GetIndex(pos)];
    }

    public void setGridDataObj(Room2D obj, Vector2Int pos)
    {
        dataObjLst[GetIndex(pos)] = obj;
        this.setDataIsCreate(pos, true);
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
