using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterAttack : monsterStateMachine
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        
        if (stateInfo.normalizedTime >= 1.0f)
        { //动画播完时
            //aniControl.attackStateEnd();
            if (aiControl != null) {
                aiControl.stateAttackEnd();
            }
            //animator.Play(csAttack, mainLayer, 0.0f);  //从第0帧开始播
        }
    }

}
