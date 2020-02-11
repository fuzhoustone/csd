using UnityEngine;
using System.Collections;

public class Player1Control : MonoBehaviour {

    private Animator animator;
    private UnityEditor.Animations.AnimatorController ac;
    private UnityEditor.Animations.AnimatorStateMachine sm;
    void Start() {

        animator = GetComponent<Animator>();
        ac = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
        for (int x = 0; x < animator.layerCount; x++) {
            //sm = animator.layers[x];
            sm = ac.layers[x].stateMachine;
            Debug.Log(sm.name);
        }

        sm = ac.layers[0].stateMachine;
        

    }


    private void printState()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("jump"))
        {
            Debug.LogWarning("state is:jump");
        }

        else if (info.IsName("Stand"))
        {
            Debug.LogWarning("state is:Stand");
        }

        else if (info.IsName("run"))
        {
            Debug.LogWarning("state is:run");
        }

        else if (info.IsName("punch1"))
        {
            Debug.LogWarning("state is:punch1");
        }
        else {
            Debug.LogWarning("state is:null");
        }

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

    void OnGUI() {
        float btnWidth = 100.0f;
        float btnHeight = 100.0f;
        float btnPosY = btnHeight + 5.0f;
        if (GUI.Button(new Rect(Screen.width - btnWidth, 0, btnWidth, btnHeight), "getState"))
        {
            printState();
        }
    }


    void Update() {

       // return;

            if (Input.GetButtonDown("Jump"))
            {
                Debug.LogWarning("set in Jump");
                if (isInEntry("Base Layer.jump") == false)
                    animator.SetBool("jump", true);
            }

            else if (Input.GetAxisRaw("Vertical") != 0)
            {
                if (isInEntry("Stand"))
                    animator.SetBool("Stand", false);
               
            }
            else {
                if (isInEntry("run"))
                    animator.SetBool("Stand", true);
               
            }
   
        
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
