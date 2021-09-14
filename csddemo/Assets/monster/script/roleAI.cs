using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DamageCal;
using stoneState;

public class roleAI : baseAI
{
    public GameObject enemyObj;  //是否有敌人
    private roleProperty selPro = null;

    private IbaseANI aniCon = null;

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

            enemyPro.SubHpValue(Hp); //UI显示
            if (enemyPro.hp <= 0)
            { //死亡
                baseAI enemyAI = enemyObj.transform.GetComponent<baseAI>();
                enemyAI.stateDieStart();
                this.enemyObj = null;
                /*
                IbaseANI enemyANI = enemyObj.transform.GetComponent<IbaseANI>();
                if (enemyANI.isInPlayEntry(roleState.die) == false) //死亡动画
                {
                    enemyANI.PlayState(roleState.die);
                }*/

            }
        }
    }

    public override void stateDieStart()
    {
        if (isState(roleState.die) == false) //死亡动画
        {
            PlayState(roleState.die);
            this.enemyObj = null;
        }
    }

    private bool isState(roleState state)
    {
        bool res = false;
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        res = aniCon.isInPlayEntry(state);

        return res;
    }

    //切换动作状态
    private void PlayState(roleState state)
    {
        if (aniCon == null)
        {
            aniCon = this.gameObject.GetComponent<IbaseANI>();
        }

        aniCon.PlayState(state);
    }

}
