using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;

public class baseAI : MonoBehaviour
{
    public delegate void StateActEnd();
    public GameObject enemyObj;  //是否有敌人
    private List<GameObject> enemyLst; 
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

    private const float csAttackMax = 0.3f;  //攻击距离最远值
    

    void Start() {
        initData();
    }

    public void initData() {
        enemyLst = new List<GameObject>();
    }

    //后续清空场景使用
    public void clearData() {
        enemyLst.Clear();
    }

    private int getEnemyFromLst(GameObject enemyObj) {
        int index = -1;
        for (int i = 0; i < enemyLst.Count; i++)
        {
            if (enemyObj == enemyLst[i])
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public void addEnemyToLst(GameObject enemyObj) {
        int index = getEnemyFromLst(enemyObj);
        if (index < 0) {
            enemyLst.Add(enemyObj);
        }
    }

    //检测是否在攻击范围内
    public bool isInAttackRange(GameObject tmpObj) {
        bool inRange = false;
        Vector3 enemyPos = tmpObj.transform.position;
        Vector3 thisPos = this.transform.position;

        float xAbs = Mathf.Abs(enemyPos.x - thisPos.x);
        float zAbs = Mathf.Abs(enemyPos.z - thisPos.z);

        if ((xAbs <= csAttackMax) && (zAbs <= csAttackMax)) //都在攻击范围内
        {
            inRange = true;
        }

        return inRange;
    }

    private bool isLife(GameObject tmpObj) {
        bool res = true;
        if (tmpObj.GetComponent<roleProperty>().hp > 0)
            res = true;
        else
            res = false;

        return res;
    }

    //检测当前敌人是否存活，是否在攻击范围内，
    // 若当前敌人已死或跑开，则从列表中挑选目标为当前敌人
    public GameObject getNowNewEnemyFromLst() {
        GameObject newEnemy = null;
        
        if (enemyObj != null)
        {
            bool needRemove = false;
            if (isLife(enemyObj) == false)
            {
                needRemove = true;
            }
            else if (isInAttackRange(enemyObj) == false) {
                needRemove = true;
            }

            if (needRemove) {
                removeEnemyFromLst(enemyObj);
                enemyObj = null;
            }
        }

        if(enemyObj == null) //当前为空，或被移除了
        {
            //先全部清理掉列表中不可攻击的目标
            for (int i = enemyLst.Count - 1; i > 0; i--) {
                GameObject tmpObj =enemyLst[i];
                if ((isLife(tmpObj) == false) || (isInAttackRange(tmpObj) == false))
                {
                    enemyLst.RemoveAt(i);
                }
             }

            if (enemyLst.Count > 0) { //还存在可攻击的目标
                enemyObj = enemyLst[0];
                newEnemy = enemyObj;
            }

        }

        return newEnemy;
    }

    public bool removeEnemyFromLst(GameObject enemyObj) {
        bool hasFind = false;
        int index = getEnemyFromLst(enemyObj);
        if (index >= 0) {
            hasFind = true;
            enemyLst.RemoveAt(index);
        }
        return hasFind;
    }

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
            if (isLife(enemyObj))
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
            if (isLife(enemyObj))
            { //敌人存活
                res = true;
            }
        }
        return res;
    }

    //根据输入按键来获得动作状态
    public roleState getHopeState(float h, float tmpv, bool isfire, bool isKeyJump, roleState nowState)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

       // roleState nowState = aniCon.getRoleNowState();
        roleState res = roleState.init;
        
        if (nowState == roleState.die)
        {
            res = nowState;
            return res;
        }

       // roleState tmpState = roleState.init;
        if ((h == 0.0f) && (tmpv == 0.0f)) //没有输入
        {
            if (enemyObj != null)  //有敌人，直接攻击
                res = roleState.attack;
            else                   //没敌人，就站立
                res = roleState.stand;
            /*
             if ((nowState == roleState.init)
                || (nowState == roleState.run)
                || (nowState == roleState.stand)) {
                res = roleState.stand;
            }

            else if (nowState == roleState.attack) {
                if(enemyObj != null)
                    res = roleState.attack;
                else
                    res = roleState.stand;
            }

             */
        }
        else {
            res = roleState.run;
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
