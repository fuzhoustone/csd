using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterDie : monsterStateMachine
{
    private bool isDie = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        isDie = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (isDie == false)
        {
            if (stateInfo.normalizedTime >= 1.0f)
            { //动画播完时，开始做淡出动画
                aiControl.stateDieEnd();
               // aniControl.dieStateEnd();
                isDie = true;
            }
        }
    }
}
