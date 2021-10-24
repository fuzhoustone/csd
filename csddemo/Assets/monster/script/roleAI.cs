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
    }

    public override void stateDieStart()
    {
        if (isAIState(roleState.die) == false) //死亡动画
        {
            PlayAIState(roleState.die);
            this.enemyObj = null;
        }
    }

    public void updataAIRoleControl(float h, float tmpv, bool isfire, bool isJump = false)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        roleState lState = aniCon.getRoleNowState();

        roleState lHopeState = getHopeState(h, tmpv, isfire, isJump,lState); //按键判断是否改变状态

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


    private void Update()
    {
        if (selfIsLive()) {
            if (hasEnemy()) {
                if (isAIState(roleState.run) == false) { //不是移动中，就自动攻击

                    if (isAIState(roleState.attack) == false) {
                        lookAtEnemy(this.gameObject, enemyObj); //修改朝向
                    }
                    
                    actToAttack(enemyObj);
                }
            }
        }
    }
}
