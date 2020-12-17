using UnityEngine;
using System.Collections;

public class Player1Control : MonoBehaviour {

    const string csStand = "Stand";
    const string csRun = "run";
    const string csAttack = "puhch1";
    const string csAttack2 = "puhch2";
    const string csJump = "jump";
    const string csDie = "die";

    private Animator animator = null;
    private int mainLayer = -2; 
    void Start() {
        animator = GetComponent<Animator>();
        mainLayer = animator.GetLayerIndex("Base Layer");
        
    }



      private bool isInEntry(string entryName) {
        bool res = false;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(entryName)) 
        {
            //动画播到最后一帧 或未开始
            if ((info.normalizedTime >= 1.0f)|| (info.normalizedTime < 0.0f)) 
            {
                res = false;
            }
            else {  //动画正在播
                res = true;
                
            }
        }
        else
            res = false;

        return res;
    }

    void Update() {
        int hp = animator.GetInteger("Hp");
        if (hp <= 0) {
            //不在死亡动画播放中
            if(animator.GetCurrentAnimatorStateInfo(0).IsName(csDie) == false)
                animator.Play(csDie);

            return;
        }

        if (Input.GetButtonDown("Jump")) // 按了一下跳跃， 跳跃过程中能否再次跳跃，这个无关系
        {
            Debug.LogWarning("set in Jump");
            animator.Play(csJump);
        }
        else
        {
            if (isInEntry(csJump)) //跳跃中不允许打断
            {
                return ;
            }

            //其它状态
            if (Input.GetButtonDown("Fire1"))
            {
               // animator.SetBool("Attack", true);
            }
            else if (Input.GetButtonDown("Fire2")) {
               // animator.SetBool("Attack2", true);
            }
            else 
            {
                if (Input.GetAxisRaw("Vertical") == 0) //无移动的按键
                {
                    //if (animator.GetCurrentAnimatorStateInfo(0).IsName(csStand) == false)
                    if (isInEntry(csStand) == false) 
                        animator.Play(csStand, mainLayer, 0.0f);
                }
                else
                {
                    //if (animator.GetCurrentAnimatorStateInfo(0).IsName(csRun) == false)
                    if (isInEntry(csRun) == false)
                        animator.Play(csRun, mainLayer, 0.0f);
                }
            }
            
        }
        
    }
}
