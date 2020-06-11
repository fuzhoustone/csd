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
        
        //Debug.Log("OnStateEnter:"+ getStateName(stateInfo));
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        
        if (stateInfo.normalizedTime >= 1.0f){ //动画播完时
            Debug.Log("OnStateUpdate:" + getStateName(stateInfo));
            int  layer = animator.GetLayerIndex("Base Layer");
            string stateName = getStateName(stateInfo);
            if ((stateName == csRun) || (stateName == csStand))
            {
                animator.Play(stateName, layer, 0.0f);
            }
            else if (stateName == csJump) {
                animator.SetBool("jump", false);
                //string newStateName = getStateName();
                //animator.Play(csStand, layer, 0.0f);
            }
        }
        
       // stateInfo.normalizedTime
       //   Debug.Log("OnStateUpdate:"+getStateName(stateInfo));
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateExit(animator, stateInfo, layerIndex);
      //  Debug.Log("OnStateExit:"+getStateName(stateInfo));
    }



	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
