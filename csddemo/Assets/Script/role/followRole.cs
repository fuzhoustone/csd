﻿using System.Collections;
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
    
    //跟随检测范围, csFollowStop 必需大于 csFollowMin
    const float csFollowMin = 0.4f;  //触发跟随
    const float csFollowMax = 1.0f;  //矩离太远不跟了
    const float csFollowStop = 0.2f;  //走到指定矩离，不再继续跟随 

    const float csMoveOffset = 0.002f;  //每帧移动范围

    //攻击检测范围
    //const float csAttackX = 0.1f;
    //const float csAttackZ = 0.1f;

    //private IbaseANI aniCon = null;

    public override void stateStandEnd()
    {
        IbaseAnimator tmpAni = this.transform.GetComponent<IbaseAnimator>();
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

        startSkillCD();

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
            aniCon = this.gameObject.GetComponent<IbaseAnimator>();
        }
        aniCon.dieStateEndAct();
    }
    
    //若不是移动动作，动作切换为移动
    private void actToMove() {
        if (isAIState(roleState.run) == false) {
            PlayAIState(roleState.run);
        }
    }

    private void Update()
    {
        if (selfIsLive()) {
            if (hasEnemy()) //有敌人
            {
                if (attackCD) {
                    actToStand();
                }
                else
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
