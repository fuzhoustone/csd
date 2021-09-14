using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class animationEvent : MonoBehaviour
{
    //private IbaseANI aniControl = null;
    private baseAI aiControl = null;

    void Start() {
        aiControl = this.transform.GetComponent<baseAI>();
    }

    // Start is called before the first frame update
    public void standEnd() {
        //aniControl.state
        if (aiControl != null) {
            aiControl.stateStandEnd();
        }
    }

    public void attackEnd() {
        if (aiControl != null)
        {
            aiControl.stateAttackEnd();
        }
    }
    public void runEnd() {

    }

    public void dieEnd() {
        if (aiControl != null)
        {
            aiControl.stateDieEnd();
        }
    }

}
