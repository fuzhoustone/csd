using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物站立状态机
public class monsterStand : monsterStateMachine
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= 1.0f)
        { //动画播完时
            aniControl.standStateEnd();
            //animator.Play(csStand, mainLayer, 0.0f);  //从第0帧开始播
        }
    }
}
