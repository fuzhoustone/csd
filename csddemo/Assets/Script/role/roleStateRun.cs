using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleStateRun : monsterStateMachine
{
    // Start is called before the first frame update
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (stateInfo.normalizedTime >= 1.0f)
        { //动画播完时
          //从AI脚本中获得参数，是否自动转入站立动画
            if (aiControl != null)
            {
                aiControl.stateStandEnd();
            }
        }
    }
}
