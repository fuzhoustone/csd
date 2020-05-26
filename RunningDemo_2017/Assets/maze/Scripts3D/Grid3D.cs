using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3D<T> {
    T[] data;
    //添加objID ,data中 1个或多个都指向同一个gameobject
    Room[] dataObjLst;  //add by csd
    bool[] dataIsCreate; //Room继承于MonoBehaviour无法通过is null来判断

    public Vector3Int Size { get; private set; }
    public Vector3Int Offset { get; set; } //后续可以考虑删除

    public Grid3D(Vector3Int size, Vector3Int offset) {
        Size = size;
        Offset = offset;

        data = new T[size.x * size.y * size.z];
        //add by csd
        int maxCount = size.x * size.y * size.z;
        dataObjLst = new Room[maxCount]; 
        dataIsCreate = new bool[maxCount];
        for (int i=0; i< maxCount; i++) {
            dataIsCreate[i] = false;
        }
        //add by csd end
    }

    //add by csd
    public void setDataIsCreate(Vector3Int pos,bool value) {
        dataIsCreate[GetIndex(pos)] = value;
    }
    public bool getDataIsCreate(Vector3Int pos) {
        return dataIsCreate[GetIndex(pos)];
    }

    public void setGridDataObj(Room obj, Vector3Int pos) {
        dataObjLst[GetIndex(pos)] = obj;
    }

    public Room getGridDataObj(Vector3Int pos) {
        Room res = dataObjLst[GetIndex(pos)];
        return res;
    }
    //add end

    public int GetIndex(Vector3Int pos) {
        return pos.x + (Size.x * pos.y) + (Size.x * Size.y * pos.z);
    }

    public bool InBounds(Vector3Int pos) {
        return new BoundsInt(Vector3Int.zero, Size).Contains(pos + Offset);
    }

    public T this[int x, int y, int z] {
        get {
            return this[new Vector3Int(x, y, z)];
        }
        set {
            this[new Vector3Int(x, y, z)] = value;
        }
    }

    public T this[Vector3Int pos] {
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
