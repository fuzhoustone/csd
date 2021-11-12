using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attcakStartEnd : MonoBehaviour {

    public bool isInAttack = false;

    /*
    //攻击了一次
    public void attackOnce() {
        App.Game.character.monsterSubHp();
    }
    */
    void Start() {
        //isInAttack = false;
    }
    /*
    public void attackStart() {
        attackJudge(1);
    }

    public void attackEnd() {
        attackJudge(0);
    }
    
    private void attackJudge(int pFlagInt) {
        bool res = false;

        string pFlagStr = "str";
        string msg = "flag" + pFlagStr + ":" + pFlagInt.ToString();
        //Debug.LogWarning("flag" + pFlagStr + ":" + pFlagInt.ToString());
       // Debug.LogWarning(msg);

        if (pFlagInt == 1)
        {
            res = true;
        }
        else
            res = false;
         

        if (res != isInAttack)
        {
            isInAttack = res;
           // Debug.Log(msg);
        }
    }

    */

}
