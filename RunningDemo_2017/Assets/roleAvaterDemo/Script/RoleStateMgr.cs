using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleStateMgr
{
    public enum roleState
    {
        init = -1,
        stand = 0,
        run = 1,
        attack = 2
    }

    private Animation m_animationController = null;
    // public CharacterController roleControl = null;

    public void initData(GameObject paraObj) {
        paraObj.GetComponent<Rigidbody>().freezeRotation = true;
        m_animationController = paraObj.GetComponent<Animation>();

        changeRoleState(roleState.stand);

        // state = stateStand;
        //  roleControl = paraObj.GetComponent<CharacterController>();
    }
    /*
        void Start()
        {
            initData();
        }
        */
    public void printRoleState(int pRoleState = -1)
    {
        roleState pState = (roleState)(pRoleState);
        if (pState == roleState.init)
        {
            //获取当前值打log
            pState = getRoleNowState();
        }
        else
        {
            //使用传入的值打log
        }

        Debug.Log("roleState is:" + pState.ToString());
    }

    public void changeRoleState(roleState pState)
    {
        switch (pState)
        {
            case roleState.init:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("breath");
                    Debug.LogWarning("change state to stand");
                }
                break;
            case roleState.stand:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("breath");
                    Debug.LogWarning("change state to stand");
                }

                break;
            case roleState.run:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("run");
                    Debug.LogWarning("change state to run");
                }
                break;
            case roleState.attack:
                {
                    m_animationController.wrapMode = WrapMode.Once;
                    m_animationController.PlayQueued("attack1");
                    m_animationController.PlayQueued("attack2");
                    m_animationController.PlayQueued("attack3");
                    m_animationController.PlayQueued("attack4");
                    Debug.LogWarning("change state to attack");
                }
                break;

            default:
                {

                    Debug.LogWarning("change state to default");
                }
                break;


        }
    }

    public roleState getHopeState(float h, float tmpv, bool isfire)
    {
        roleState nowState = getRoleNowState();
        if (nowState == roleState.attack)
        { //攻击状态中，不能被其它打断
            //roleState = roleState.attack;
            return roleState.attack;
        }

        roleState res = roleState.stand;
        if (isfire)
            res = roleState.attack;
        else
        {
            if ((h == 0.0f) && (tmpv == 0.0f))
                res = roleState.stand;
            else
                res = roleState.run;
        }

        return res;
    }

    public roleState getRoleNowState()
    { //获得角色当前状态
        roleState res = roleState.stand;

        if (m_animationController != null)
        {
            if (m_animationController.IsPlaying("breath"))
            {
                res = roleState.stand;
            }
            else if (m_animationController.IsPlaying("run"))
            {
                res = roleState.run;
            }
            else if (
            m_animationController.IsPlaying("attack1") ||
            m_animationController.IsPlaying("attack2") ||
            m_animationController.IsPlaying("attack3"))
            {
                res = roleState.attack;
            }
            else if (m_animationController.IsPlaying("attack4"))
            {
                if (m_animationController["attack4"].normalizedTime < 1.0f)
                {
                    res = roleState.attack;
                    Debug.Log(" roleState is attack4");
                }
                else
                {
                    res = roleState.init;
                    Debug.Log("need set roleState init");
                }
            }
            else
            {
                res = roleState.init;
                Debug.Log("need set roleState default init");
            }

        }

        return res;
    }


    private void setAttackEnd()
    {
        attcakStartEnd pAttackClass = App.Game.character.roleInstance.GetComponent<attcakStartEnd>();
      //  pAttackClass.attackJudge(0);
    }

    

    public bool updataRoleControl(float h, float tmpv, bool isfire)
    {

        /*
        // if (roleControl.isGrounded)
        // {
        //人物移动
        moveDirection = new Vector3(h, 0, tmpv); //Allows for player input
        moveDirection = transform.TransformDirection(moveDirection); //How to move
        moveDirection *= moveVSpeed; //How fast to move
                                     //  }

        moveDirection.y -= 0 * Time.deltaTime;
        //Move the controller
        roleControl.Move(moveDirection * Time.deltaTime);
        */
        roleState lState = getRoleNowState();

        roleState lHopeState = getHopeState(h, tmpv, isfire);

        if (lState != lHopeState)
        {
            changeRoleState(lHopeState);
        }
        

        return true;
    }
}
