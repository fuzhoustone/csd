using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeWall : MonoBehaviour
{
    [SerializeField]
    public Vector3Int worldPos; //在世界坐标系中的坐标

    //[SerializeField]
    //public Generator3D.CellType cellType;  //grid中有标识

    [SerializeField]
    public GameObject topWall;

    [SerializeField]
    public GameObject bottomWall;

    [SerializeField]
    public GameObject leftWall;

    [SerializeField]
    public GameObject rightWall;
}
