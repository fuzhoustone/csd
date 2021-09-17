using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DamageCal;
using stoneState;


//小弟AI
/*
            if(战斗中) {
               等战斗结束
            }
            else {
              if(攻击范围内检测是否存在目标)｛
                  与攻击目标相互攻击
              ｝
              else if( 移动状态中){  OK
                 if(检测已达到stop距离){  
                       停止跟随，转换站立状态                  
                  }
                  else{    
                    继续跟随主角：发生位移，并保持移动状态，
                   }
              }
              else if(主角是否离的太远要进行跟随){  OK
                 切换移动状态，并发生位移
              }
              else{  OK
                  没有攻击目标，没有跟随需求：动作改为站立 
              }
            }

            */


public class followRole : baseAI
{
    public GameObject mainObj;   //跟随的人物
   // public GameObject enemyObj;  //是否有敌人
    

    //  const string csStand = "stand";
    //  const string csRun = "run";
    //  const string csAttack = "attack1";

    //跟随检测范围, csFollowStop 必需大于 csFollowMin
    const float csFollowMin = 0.4f;  //触发跟随
    const float csFollowMax = 2.0f;  //矩离太远不跟了
    const float csFollowStop = 0.2f;  //走到指定矩离，不再继续跟随 

    const float csMoveOffset = 0.002f;  //每帧移动范围

    //攻击检测范围
    //const float csAttackX = 0.1f;
    //const float csAttackZ = 0.1f;

    //private IbaseANI aniCon = null;

    public void initData() {
        
    }

    public override void stateStandEnd()
    {
        IbaseANI tmpAni = this.transform.GetComponent<IbaseANI>();
        tmpAni.PlayState(roleState.stand);
    }

    //攻击->攻击(触发制，动作完成判断)：双方相互攻击
    //攻击->待机(触发制，动作完成判断)
    public override void stateAttackEnd()
    {
        if (selPro == null) {
            roleProperty selPro = this.transform.GetComponent<roleProperty>();
        }

        if (enemyObj != null) {
            roleProperty enemyPro = enemyObj.transform.GetComponent<roleProperty>();
            int Hp = RoleDamageCal.instance.DamageCal(selPro, enemyPro);

            enemyPro.SubHpValue(Hp); //UI扣血显示
            if (enemyPro.hp <= 0) { //死亡
                baseAI enemyAI = enemyObj.transform.GetComponent<baseAI>();
                enemyAI.stateDieStart(); //处理 对方死亡动画及结算
                this.enemyObj = null;
            }
            getNowNewEnemyFromLst();
            if (enemyObj != null)
                lookAtEnemy(this.gameObject, enemyObj);
        }

        /*
        IbaseANI tmpAni = this.transform.GetComponent<monsterAniControl>();

        bool isAttack = tmpAni.attackStateEnd(); //伤害计算及UI显示，以及是否死亡的结算

        if (isAttack)
        {
            tmpAni.PlayState(roleState.attack);
        }
        else
        {
            tmpAni.PlayState(roleState.stand);
        }*/
    }

    public override void stateDieStart() {
        if (isAIState(roleState.die) == false) //死亡动画
        {
            PlayAIState(roleState.die);
            this.enemyObj = null;
        }
    }

    public override void stateDieEnd() {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }
        aniCon.dieStateEndAct();
    }
    /*
    private bool isState(roleState state) {
        bool res = false;
        if (aniCon == null) {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        res = aniCon.isInPlayEntry(state);

        return res;
    }
   
    //切换动作状态
    private void PlayState(roleState state) {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        aniCon.PlayState(state);
    }
     */
    //若不是移动动作，动作切换为移动
    private void actToMove() {
        if (isAIState(roleState.run) == false) {
            PlayAIState(roleState.run);
        }
    }

    //若不是站立动作，动作切换为站立
    private void actToStand() {
        if (isAIState(roleState.stand) == false)
        {
            PlayAIState(roleState.stand);
        }
    }

    /*
    //攻击敌人
    private void actToAttack(GameObject enemy) {
        //自己切换成攻击状态
        if (isState(roleState.attack) == false) {
            PlayState(roleState.attack);
        }
        //敌人的攻击状态，由敌人切换，无需此处理

        //血条的显示由 双方碰撞时产生,无需此处理

        //扣血由动作完成时计算,并结算死亡

    }

    private bool hasEnemy() {
        bool res = false;
        if (enemyObj != null) {
            res = true;
        }
        return res;
    }
    */
    //待完成，相关的


    //根据HP判断是否死亡
    /*
    private bool selfIsLive() {
        bool res = true;
        if (selPro == null) {
            selPro = this.transform.GetComponent<roleProperty>();
        }

        if (selPro.hp <= 0)
            res = false;

        return res;
    }
   
    //基本完成
    private bool isInFight() {
        bool res = false;
        if (enemyObj != null) { //存在敌人
            if (enemyObj.GetComponent<roleProperty>().hp > 0) { //敌人存活
                res = true;
            }
        }

        return res;
    }

     */

    private void Update()
    {
        if (selfIsLive()) {
            if (isInFight()) //战斗标识中
            {
                actToAttack(enemyObj); // 等战斗结束,无需处理

            }
            else { //非战斗状态
                if (hasEnemy()) //有敌人
                {
                    actToAttack(enemyObj); //切换攻击状态攻击敌人
                }
                else if (mainObj != null) //有跟随目标
                {
                    float offsetX = 0.0f;
                    float offsetZ = 0.0f;

                    if (isAIState(roleState.run)) { //当前移动中
                        if (isStopFollow(ref offsetX, ref offsetZ)) //是否已达到stop距离
                        {
                            Debug.LogWarning("isStopFollow");
                            actToStand(); //停止跟随，转换站立状态
                        }
                        else
                        { //继续跟随主角：发生位移，并保持移动状态，
                            //isFollow(ref offsetX, ref offsetZ); //必然产生跟随

                            Vector3 thisPos = this.transform.position;
                            Vector3 newPos = new Vector3(thisPos.x + offsetX,
                                                         thisPos.y,
                                                         thisPos.z + offsetZ);
                            this.transform.LookAt(mainObj.transform);
                            this.transform.position = newPos;
                            actToMove(); 
                        }
                    }
                    else if( isFollow(ref offsetX, ref offsetZ)) //是否切换成跟随
                    {
                        Vector3 thisPos = this.transform.position;
                        Vector3 newPos = new Vector3(thisPos.x + offsetX,
                                                     thisPos.y,
                                                     thisPos.z + offsetZ);
                        this.transform.LookAt(mainObj.transform);
                        this.transform.position = newPos;
                        actToMove();
                    }
                    else
                    {
                        actToStand();
                    }
                }
            }
        }
       
    }
    //是否停止跟随
    private bool isStopFollow(ref float offsetX, ref float offsetZ) {
        bool isFollow = false;
        offsetX = 0.0f;
        offsetZ = 0.0f;

        Vector3 mainPos = mainObj.transform.position;
        Vector3 thisPos = this.transform.position;

        float xAbs = Mathf.Abs(mainPos.x - thisPos.x);
        float zAbs = Mathf.Abs(mainPos.z - thisPos.z);
        bool xFollow = true;
        bool zFollow = true;

        if ((xAbs >= csFollowStop) && (xAbs <= csFollowMax)) //大于stop的，并且未到走散，就继续跟
        {
            xFollow = true; //x坐标需要继续跟
            if (mainPos.x > thisPos.x)
                offsetX = csMoveOffset;
            else
                offsetX = -1 * csMoveOffset;
        }
        else
        {
            if (xAbs < csFollowStop)
            { //X坐标不需要移动
                xFollow = false; //x坐标不跟了
            }

            else // 等价于 else if (xAbs > csFollowMax)  
            {
                //已走散，不必跟了
                return false;
            }
        }

        if ((zAbs >= csFollowStop) && (zAbs <= csFollowMax))  //大于stop的，并且未到走散，就继续跟
        {
            zFollow = true;
            if (mainPos.z > thisPos.z)
                offsetZ = csMoveOffset;
            else
                offsetZ = -1 * csMoveOffset;
        }
        else
        {
            if (zAbs < csFollowStop)
            { //z坐标不需要移动
                zFollow = false;
            }
            else   //  等价于 else if (zAbs > csFollowMax)
            {  //已走散，不必跟了
                return false;
            }
        }

        if (xFollow || zFollow) { //x轴或z轴有一个必需跟随的
            isFollow = true;   //需要继续跟随
        }

        return (!isFollow);
    }


    //是否进行跟随,并返回移动距离
    private bool isFollow(ref float offsetX, ref float offsetZ) {
        bool res = false;

        Vector3 mainPos = mainObj.transform.position;
        Vector3 thisPos = this.transform.position;

        float xAbs = Mathf.Abs(mainPos.x - thisPos.x);
        float zAbs = Mathf.Abs(mainPos.z - thisPos.z);

        offsetX = 0.0f;
        offsetZ = 0.0f;

        if ((xAbs >= csFollowMin) && (xAbs <= csFollowMax)) {
            res = true;
            if (mainPos.x > thisPos.x)
                offsetX = csMoveOffset;
            else
                offsetX = -1 * csMoveOffset;
        }

        if ((zAbs >= csFollowMin) && (zAbs <= csFollowMax))
        {
            res = true;
            if (mainPos.z > thisPos.z)
                offsetZ = csMoveOffset;
            else
                offsetZ = -1 * csMoveOffset;
        }



        return res;
    }

}
