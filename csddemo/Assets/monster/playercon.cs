using UnityEngine;
using System.Collections;

public class playercon : StateMachineBehaviour {
    const string csStand = "Stand";
    const string csRun = "run";
    const string csAttack = "puhch1";
    const string csJump = "jump";

    private string getStateName(AnimatorStateInfo stateInfo) {
        string res = "not findname";
        if (stateInfo.IsName(csStand))
            res = csStand;
        else if (stateInfo.IsName(csRun))
            res = csRun;
        else if (stateInfo.IsName(csAttack))
            res = csAttack;
        else if (stateInfo.IsName(csJump))
            res = csJump;
        return res;
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        
        if (stateInfo.normalizedTime >= 1.0f){ //动画播完时
            int  layer = animator.GetLayerIndex("Base Layer");
            string stateName = getStateName(stateInfo);
            if  (stateName == csStand) //站立完成后，默认继续循环
            {
                animator.Play(csStand, layer, 0.0f);  //从第0帧开始播
                //animator.Play(csStand);
            }
            else if (stateName == csRun) //跑步完成后，默认站立
            {
                animator.Play(csStand, layer, 0.0f);  //从第0帧开始播
                //animator.Play(csStand);
            }

            else if (stateName == csJump) { //跳跃完成后，默认用站立
                animator.Play(csStand);
            }
        }
        
       // stateInfo.normalizedTime
       //   Debug.Log("OnStateUpdate:"+getStateName(stateInfo));
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
       // string stateName = getStateName(stateInfo);
       // if(stateName == csStand)
       //     Debug.Log("OnStateExit:"+getStateName(stateInfo));
    }



	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
