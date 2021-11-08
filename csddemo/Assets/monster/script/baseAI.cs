using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;
using System;

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


    public float atkCDTime = 1.0f;
    public bool attackCD = false;

    /*
        public const string csStand = "stand";
        public const string csRun = "run";
        public const string csAttack = "attack1";
        public const string csAttack2 = "attack2";
        public const string csDie = "die";
    */
    public const float csAttackMax = 0.3f;  //攻击距离检测值，小于开战
    public const float csAttackAuto = 0.7f;  //攻击自动开场点，若小于就自动移动
    public const float csAttackReady = 1.0f;  //攻击警告点，用于记录后脱战的移动点



    void Start() {
        initData();
    }

    public void initData() {
       // Debug.LogWarning("baseAI.initData");
        enemyLst = new List<GameObject>();
        attackCD = false;
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

    public float calDistance(Transform a, Transform b) {
        float res = 0.0f;
        Vector2 aTmp = new Vector2(a.position.x, a.position.z);
        Vector2 bTmp = new Vector2(b.position.x, b.position.z);
        res = Vector2.Distance(aTmp, bTmp);
        return res;
    }

    //检测是否在攻击范围内
    public bool isInAttackRange(GameObject tmpObj) {
        bool inRange = false;
        float tmpDis = calDistance(tmpObj.transform, this.transform);
        
        if (tmpDis <= csAttackMax)
            inRange = true;
        
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
    /*
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
    */
    public bool hasEnemy()
    {
        bool res = false;
        if (enemyObj != null)
        {
            if (isLife(enemyObj))
            { //敌人存活
                res = true;
            }
            else
            {
                getNowNewEnemyFromLst();
            }
        }
        return res;
    }

    //根据输入按键来获得动作状态
    public roleState getHopeState(float h, float tmpv, int fireSoft, roleState nowState)
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

        if (enemyObj == null) //无攻击目标
        {
            if ((h == 0.0f) && (tmpv == 0.0f)) //没有输入
                res = roleState.stand;
            else
                res = roleState.run;
        }
        else {
            if (fireSoft == 1)
                res = roleState.attack;
            else if (fireSoft == 2)
                res = roleState.attack2;
            else if (fireSoft == 3)
                res = roleState.def;
            else
            {
                res = nowState;
            }
        }

        return res;
        
    }

    //若不是站立动作，动作切换为站立
    public void actToStand()
    {
        if (isAIState(roleState.stand) == false)
        {
            PlayAIState(roleState.stand);
        }
    }

    public void actToAttack(GameObject enemy)
    {
        //自己切换成攻击状态
        if (isAIState(roleState.attack) == false)
        {
            PlayAIState(roleState.attack);
          //  lookAtEnemy(this.gameObject, enemy);
        }
        //敌人的攻击状态，由敌人切换，无需此处理

        //血条的显示由 双方碰撞时产生,无需此处理

        //扣血由动作完成时计算,并结算死亡

    }

    public void startSkillCD() {
        attackCD = true;
        StartCoroutine(doSkillCD());
    }

    IEnumerator doSkillCD() {
        float time = 0;
        //float fadeLength = 5.0f;
        while (time < atkCDTime) {
            time += Time.deltaTime;
            yield return null;
        }

        attackCD = false;
    }


}
