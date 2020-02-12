using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class woodsetbreak : MonoBehaviour {

    public bool isBroken = false;
    public bool isSetInit = false;
    private TimerDestruct tDes;
    private BoxCollider tbox;

    void Start(){
        tDes = GetComponent<TimerDestruct>();
        tbox = GetComponent<BoxCollider>();
    }

    void Update() {
        if (isSetInit) {
            isSetInit = false;
            if (tDes != null)
            {
                tDes.setInit();
            }
        }

        if (isBroken) {
            isBroken = false;
        
            if (tDes != null) {
                tDes.setEnable();
                tbox.enabled = false;
            }
        }
    }
}
