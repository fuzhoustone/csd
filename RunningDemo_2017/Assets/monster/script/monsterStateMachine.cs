using UnityEngine;
using System.Collections;

public class monsterStateMachine : StateMachineBehaviour
{
    public const string csStand = "stand";
    public const string csAttack = "attack1";
    public const string csAttack2 = "attack2";
    public const string csDie = "die";

    private const string csMainAniLayer = "mainAniLayer";

    public int mainLayer;
    public monsterAniControl aniControl;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        mainLayer = animator.GetLayerIndex(csMainAniLayer);
        aniControl = animator.gameObject.transform.GetComponent<monsterAniControl>();
    }
}
