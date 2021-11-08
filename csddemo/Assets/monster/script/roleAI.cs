using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DamageCal;
using stoneState;

public class roleAI : baseAI
{
    
    public override void stateStandEnd()
    {
        IbaseANI tmpAni = this.transform.GetComponent<IbaseANI>();
        tmpAni.PlayState(roleState.stand);
    }

    public override void stateAttackEnd()
    {

        if (selPro == null)
        {
            roleProperty selPro = this.transform.GetComponent<roleProperty>();
        }

        if (enemyObj != null)
        {
            roleProperty enemyPro = enemyObj.transform.GetComponent<roleProperty>();
            int Hp = RoleDamageCal.instance.DamageCal(selPro, enemyPro);

            enemyPro.SubHpValue(Hp); //UI扣血显示
            if (enemyPro.hp <= 0) //敌人死亡
            { 
                baseAI enemyAI = enemyObj.transform.GetComponent<baseAI>();
                enemyAI.stateDieStart();
                //this.enemyObj = null;
            }
            getNowNewEnemyFromLst();
            if (enemyObj != null)
                lookAtEnemy(this.gameObject, enemyObj);
        }
        stateStandEnd();

    }

    public override void stateDieStart()
    {
        if (isAIState(roleState.die) == false) //死亡动画
        {
            PlayAIState(roleState.die);
            this.enemyObj = null;
        }
    }

    public void AIRoleSkill(int fireSoft) {
        updataAIRoleControl(0.0f, 0.0f, fireSoft);
    }

    public void updataAIRoleControl(float h, float tmpv, int fireSoft)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        roleState lState = aniCon.getRoleNowState();

        roleState lHopeState = getHopeState(h, tmpv, fireSoft, lState); //按键判断是否改变状态

        // bool isChangeToJump = false;
        if ((lState != lHopeState)
           // && (oldRoleState != lHopeState)
            )  //避免重复执行
        {
            //  if (lHopeState == roleState.jump) //切换成跳跃状态
            //      isChangeToJump = true;
            oldRoleState = lHopeState;
            aniCon.PlayState(lHopeState);
        }

        if ((lHopeState == roleState.stand)|| (lHopeState == roleState.run)) { //移动停止时再检测敌人
            if (enemyObj != null) {
                getNowNewEnemyFromLst();
             }
        }
        

        //return isChangeToJump;

    }

    private const int csNullType = -1;
    private const int csStopType = 0;
    private const int csAutoType = 1;
    private const int csReadyType = 2;

    private Vector3 oldPos = Vector3.zero;
    public GameObject oldReadyMonster = null;
    public bool isAutoRun = false;

    public int checkReadyMonster() {
        int res = csReadyType;
        isAutoRun = false;
        float tmpDis = calDistance(oldReadyMonster.transform, this.transform);
        if (tmpDis <= csAttackMax)
            res = csStopType;
        else if (tmpDis <= csAttackAuto) {
            res = csAutoType;
            isAutoRun = true;
        }
        else if (tmpDis <= csAttackReady)
            res = csReadyType;
        else {
            res = csNullType;
            oldReadyMonster = null;
        }
        return res;
    }

    //检测所有敌人，是否有在攻击准备范围内
    public bool checkReadyMonster(Transform pTran)
    {
        bool res = false;
        int nCount = pTran.childCount;
        if (nCount > 0)
        {
            for (int i = 0; i < nCount; i++)
            {
                Transform tmpTran = pTran.GetChild(i);
                if (tmpTran.gameObject.activeSelf == true)
                {
                    float tmpDistan = calDistance(tmpTran, this.transform);
                   // if (tmpDistan <= csAttackMax)
                   //     pType = csStopType;    //停下，开始攻击
                   // else if (tmpDistan <= csAttackAuto)
                   //     pType = csAutoType;    //可以自动移动
                    if (tmpDistan <= csAttackReady) {
                        oldReadyMonster = tmpTran.gameObject;
                        res = true;
                        break;   //攻击警告范围
                    }
                }
            }
        }

        return res;
    }
    private void Update()
    {
        if (selfIsLive()) {
            if (hasEnemy()) {
                if (isAIState(roleState.run) == false) { //不是移动中，就自动攻击

                    if ((isAIState(roleState.attack) == false)
                         && (isAIState(roleState.attack2) == false))
                    {
                        lookAtEnemy(this.gameObject, enemyObj); //修改朝向
                    }
                    
                    //actToAttack(enemyObj);
                }
            }
        }
    }
}
