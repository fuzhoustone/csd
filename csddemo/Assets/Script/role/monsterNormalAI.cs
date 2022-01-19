using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DamageCal;
using stoneState;
public class monsterNormalAI : baseAI
{
  //  public GameObject enemyObj;  //是否有敌人
//    private roleProperty selPro = null;

 //   private IbaseANI aniCon = null;

    /*
普通怪物AI： 
        1. 死亡状态：优先于所有
        2. 初始状态：站立
        3. 其它状态：
        待机站立->待机站立(触发制，动作时完成判断，默认)： OK
        待机->攻击: 打断现有的待机状态，触发制，转攻击状态, 由玩家方触发
        
        待机->警戒：触发制 or 轮询, 后续实现
        警戒->警戒, 警戒->出击（update中轮询）：保持站立动作，并警戒一定范围，后续实现
		出击->出击, 出击->攻击, 出击->跑回 (update中轮询)：以原始警戒的范围为中心，出现了敌人，开始移动，后续实现

		攻击->攻击(触发制，动作完成判断)：双方相互攻击   
        攻击->待机(触发制，动作完成判断)：
     */

    // Update is called once per frame
    public override void stateStandEnd() {
        IbaseAnimator tmpAni = this.transform.GetComponent<IbaseAnimator>();
        tmpAni.PlayState(roleState.stand);
    }

    //攻击->攻击(触发制，动作完成判断)：双方相互攻击
    //攻击->待机(触发制，动作完成判断)
    public override void stateAttackEnd() {

        if (selPro == null)
        {
            roleProperty selPro = this.transform.GetComponent<roleProperty>();
        }

        if (enemyObj != null)
        {
            roleProperty enemyPro = enemyObj.transform.GetComponent<roleProperty>();
            int Hp = RoleDamageCal.instance.DamageCal(selPro, enemyPro);

            enemyPro.SubHpValue(Hp); //UI显示
            if (enemyPro.hp <= 0)
            { //死亡
                baseAI enemyAI = enemyObj.transform.GetComponent<baseAI>();
                enemyAI.stateDieStart();

                getNowNewEnemyFromLst(); //死亡，挑选新的敌人
            }
            
            if(enemyObj != null)
                lookAtEnemy(this.gameObject, enemyObj);
        }

        startSkillCD();
    }

    public override void stateDieStart()
    {
        if (isAIState(roleState.die) == false) //死亡动画
        {
            PlayAIState(roleState.die);
            this.enemyObj = null;
        }
    }

    public override void stateDieEnd()
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseAnimator>();
        }
        aniCon.dieStateEndAct();

    }

    private void Update()
    {
        if (selfIsLive())
        {
            //if (isInFight()) //战斗标识中
            //{
            //    actToAttack(enemyObj); // 等战斗结束,无需处理
            //}
            if (hasEnemy()) //有敌人
            {
                if (attackCD)
                {
                    actToStand();
                }
                else
                    actToAttack(enemyObj); //切换攻击状态攻击敌人
            }
        }
    }
}
