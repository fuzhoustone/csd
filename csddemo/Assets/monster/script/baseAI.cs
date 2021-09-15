using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;

public class baseAI : MonoBehaviour
{
    public delegate void StateActEnd();
    public GameObject enemyObj;  //是否有敌人
    public roleProperty selPro = null;
    public IbaseANI aniCon = null;
    public roleState oldRoleState = roleState.init;

    public event StateActEnd EventStandEnd;
    public event StateActEnd EventAttackEnd;
    public event StateActEnd EventDieStart;
    public event StateActEnd EventDieEnd;

    public const string csStand = "stand";
    public const string csRun = "run";
    public const string csAttack = "attack1";
    public const string csAttack2 = "attack2";
    public const string csDie = "die";

    public virtual void stateStandEnd() {
        if(EventStandEnd != null)
            EventStandEnd();
    }

    public virtual void stateAttackEnd()
    {
        if(EventAttackEnd != null)
            EventAttackEnd();
    }

    public virtual void stateDieStart()
    {
        if(EventDieStart != null)
            EventDieStart();
    }

    public virtual void stateDieEnd()
    {
        if(EventDieEnd != null)
            EventDieEnd();
    }

    public void setEnemyObj(GameObject tmpObj) {
        enemyObj = tmpObj;
    }

    //朝向不修改Y轴，只修改x,z
    public void lookAtEnemy(GameObject selfObj, GameObject enemyObj) {
        Vector3 lookPos = new Vector3(enemyObj.transform.position.x, 
                                      selfObj.transform.position.y,
                                      enemyObj.transform.position.z);
        selfObj.transform.LookAt(lookPos);
    }

    public bool selfIsLive()
    {
        bool res = true;
        if (selPro == null)
        {
            selPro = this.transform.GetComponent<roleProperty>();
        }

        if (selPro.hp <= 0)
            res = false;

        return res;
    }

    public bool isAIState(roleState state)
    {
        bool res = false;
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        res = aniCon.isInPlayEntry(state);

        return res;
    }

    public void AIInitData(GameObject paraObj)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        aniCon.initData(paraObj);
    }

    //切换动作状态
    public void PlayAIState(roleState state)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        aniCon.PlayState(state);
    }

    public bool isInFight()
    {
        bool res = false;
        if (enemyObj != null)
        { //存在敌人
            if (enemyObj.GetComponent<roleProperty>().hp > 0)
            { //敌人存活
                res = true;
            }
        }

        return res;
    }

    public bool hasEnemy()
    {
        bool res = false;
        if (enemyObj != null)
        {
            res = true;
        }
        return res;
    }

    public void actToAttack(GameObject enemy)
    {
        //自己切换成攻击状态
        if (isAIState(roleState.attack) == false)
        {
            PlayAIState(roleState.attack);
        }
        //敌人的攻击状态，由敌人切换，无需此处理

        //血条的显示由 双方碰撞时产生,无需此处理

        //扣血由动作完成时计算,并结算死亡

    }


   

}
