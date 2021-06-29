using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpColider : MonoBehaviour
{
    private int floorLayer;
    private const string floorLayStr = "floor";
    
    
    public delegate void jumpDownOverFunc();

    public jumpDownOverFunc jumpDownOver{
        get;
        set;
    }
   
    public bool isUse { //是否启用
        get;
        set;
    }

    void Start() { //enable时会启用
        floorLayer = LayerMask.NameToLayer("floor");
    }

    void OnCollisionStay(Collision collision) {
        if (isUse == false)
            return;
        if (collision == null)
            return;
        if (collision.gameObject.layer == floorLayer)
        { //只接受地板的碰撞
            if (jumpDownOver != null) {
                jumpDownOver();
                isUse = false;
                Debug.LogWarning("jumpDownOver");
            }
        }
    }
}
