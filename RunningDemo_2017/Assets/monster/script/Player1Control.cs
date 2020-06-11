using UnityEngine;
using System.Collections;

public class Player1Control : MonoBehaviour {

    private Animator animator;
    void Start() {

        animator = GetComponent<Animator>();
    }

      private bool isInEntry(string entryName) {
        bool res = false;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(entryName)) //跳的状态下不允许打断
        {
            res = true;
/*
            if ((info.normalizedTime >= 1.0f)|| (info.normalizedTime < 0.0f))
            {
                if (entryName == "jump") //Base Layer.jump
                {
             //       animator.SetBool("IsJumping", false);
                }
            //    res = false;
            }
            else {
              //  res = true;
                
            }
            */
            
        }
        else
            res = false;

        return res;
    }

    void Update() {
        
        /*
        if (isInEntry("jump"))
        {
           // Debug.LogWarning("set in Jump false");
          //  animator.SetBool("IsJumping", false);
        }
        else {*/
            if (Input.GetButtonDown("Jump"))
            {
                Debug.LogWarning("set in Jump");
                if (isInEntry("Base Layer.jump") == false)
                    animator.SetBool("jump", true);
            }

            else
            { 
                if (Input.GetAxisRaw("Vertical") != 0)
                {
                    if (isInEntry("Stand")) { 
                        animator.SetBool("Stand", false);
                        Debug.Log("test change to run");
                    }

                }
                else {
                    if (isInEntry("Stand") == false) { 
                        animator.SetBool("Stand", true);
                        Debug.Log("test change to stand");
                    }

                }
            }
        // }

        /*
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("jump")) //跳的状态下不允许打断
        {
            //return;
            if (info.normalizedTime >= 1.0f)
            {
               // Debug.LogWarning("injump");
            }
            else {
                Debug.LogWarning("set jump false");
                animator.SetBool("IsJumping", false);
               // animator.SetBool("IsRunning",false);
            }
        }
        else if (Input.GetButton("Jump"))
        {
            Debug.LogWarning("set jump true");
            animator.SetBool("IsJumping", true);
        }
        else {
            if (Input.GetAxisRaw("Vertical") != 0) 
            {
                animator.SetBool("IsRunning", true);
            }
            if (info.normalizedTime >= 1.0f)
            {


                //   animator.SetBool("IsJumping", false);
                if (info.IsName("run"))
                {
                    //Debug.LogWarning("run is finish");
                    animator.SetBool("IsRunning", false);
                }

                //播放完毕，要执行的内容
            }


            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                //Debug.LogWarning("run is stop");
                animator.SetBool("IsRunning", false);
            }
           
        }
        */
    }
}
