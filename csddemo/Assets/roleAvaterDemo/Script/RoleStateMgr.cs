using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stoneState;

public class RoleStateMgr: MonoBehaviour,IbaseANI
{
    private Animation m_animationController = null;
    // public CharacterController roleControl = null;

    private baseAI selfAI = null;
   // private roleState oldRoleState = roleState.init;

    //动作的常量
    private const string csDefault = "breath";
    private const string csRun = "run";
    private const string csAttack1 = "attack1";
    private const string csAttack2 = "attack2";
   // private const string csAttack3 = "attack3";
    private const string csAttackEnd = "attack3";

    private const string csJump = "attack4";

    private const string csDie = "attack4";

    private const float csPlaySpeedTime = 0.1f;


    //跳跃过程中累计的时间
    private float jumpAllTime = 0.0f;
    
    //跳跃的完整时间含上升和下降
    private float jumpTime = 0.0f;

    /*
    //下落中是否发生碰撞
    public bool isJumpDownTouch {
         get;
         set;
    }
     */   

    public void initData(GameObject paraObj) {
        paraObj.GetComponent<Rigidbody>().freezeRotation = true;
        m_animationController = paraObj.GetComponent<Animation>();
        selfAI = this.transform.GetComponent<baseAI>();

        //设置播放速度，实际无效
        foreach (AnimationState state in m_animationController)
        {
            if ((state.name == csAttack1) || (state.name == csAttack2) || (state.name == csAttackEnd))
                state.speed = csPlaySpeedTime;
        }

        //oldRoleState = roleState.stand;
        selfAI.oldRoleState = roleState.stand;
        
        PlayState(roleState.stand);
    }


    /*
      public void setJumpDownTouch(bool pValue) {
          isJumpDownTouch = pValue;
      }

          public bool getJumpDownTouch() {
              return isJumpDownTouch;
          }
       */


/*    public void setJumpTime(float pTime) {
        jumpTime = pTime;
    }
   */
   /*
    public float getAllJumpTime() {
        return jumpAllTime;
    }
    */

        /*
    public void addAllJumpTime(float pTime) {
        jumpAllTime = jumpAllTime + pTime;
    }

    public void clearJumpTime() {
        jumpAllTime = 0;
        isJumpDownTouch = false;
    }
    */
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>

    public void PlayState(roleState state) {

        changeRoleState(state);
    }


    public bool isInPlayEntry(roleState stateName) {
        bool res = false;
        roleState nowState = getRoleNowState();
        if (nowState != stateName)
            res = false;
        else
            res = true;
        return res;
    }

    /*
    public bool attackStateEnd() {
        bool res = false;

        return res;
    }
    */


    
       //待UI实现
    public void dieStateEndAct() {
        Debug.LogError("game over");
    }
/*
    public void setToAttack(Vector3 rolePos) { }

    public void setStopAttack() {

    }

    public void setToStand() {

    }

    public bool IsInAttackState() {
        bool res = false;
        return res;
    }

    public bool IsDie() {
        bool res = false;
        return res;
    }
    */
    /// <summary>
    /// /////////////////////////////
    /// </summary>

/*
    public void playRoleDie()
    {
        changeRoleState(roleState.die);
    }
*
    public void playRoleStand() {
        changeRoleState(roleState.stand);
    }
*/
    private void changeRoleState(roleState pState)
    {
        
        switch (pState)
        {
            case roleState.init:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play(csDefault);
                  //  Debug.LogWarning("change state to stand");
                }
                break;
            case roleState.stand:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play(csDefault);
                   // Debug.LogWarning("change state to stand");
                }

                break;
            case roleState.run:
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play(csRun);
                 //   Debug.LogWarning("change state to run");
                }
                break;
            case roleState.attack:
                {
                    m_animationController.wrapMode = WrapMode.Once;
                   // App.Game.character.roleInstance.GetComponent<attcakStartEnd>().attackStart();
                    m_animationController.PlayQueued(csAttack1);
                    
                    m_animationController.PlayQueued(csAttack2);
                    m_animationController.PlayQueued(csAttackEnd);
                   // m_animationController.PlayQueued("attack4");
                 //   Debug.LogWarning("change state to attack");
                }
                break;
                /*
            case roleState.jump: {
                    clearJumpTime();
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play(csJump);
                  //  Debug.LogWarning("change state to jump");
                }
                break;*/
            case roleState.die: {
                    m_animationController.wrapMode = WrapMode.Once;
                    m_animationController.Play(csDie);
                  //  Debug.LogWarning("change state to die");
                }
                break;
            default:
                {

                    Debug.LogWarning("change state to default");
                }
                break;


        }
    }

     
    public roleState getRoleNowState()
    { //获得角色当前状态
        roleState res = roleState.stand;

        if (m_animationController != null)
        {
            if (m_animationController.IsPlaying(csDefault))
            {
                res = roleState.stand;
            }
            else if (m_animationController.IsPlaying(csRun))
            {
                res = roleState.run;
            }
            else if (
               m_animationController.IsPlaying(csAttack1)
            || m_animationController.IsPlaying(csAttack2)
            //|| m_animationController.IsPlaying(csAttack3)
              )
            {
                res = roleState.attack;
            }
            else if (m_animationController.IsPlaying(csAttackEnd)) //攻击动画未播完
            {
                if (m_animationController[csAttackEnd].normalizedTime < 1.0f)
                {
                    res = roleState.attack;
                  //  Debug.Log(" roleState is attackEnd");
                }
                else
                {
                    res = roleState.init;
                    //oldRoleState = res;
                    selfAI.oldRoleState = res;
                    //   Debug.Log("need set roleState init");
                }
            }
            else if (m_animationController.IsPlaying(csJump))
            {
                //if (m_animationController[csJump].normalizedTime < 1.0f)
                //{
                    res = roleState.jump;
                    Debug.Log(" roleState is jump");
                //}
                //else
                //{
                //    res = roleState.init;
                //    Debug.Log("need set roleState init");
                //}
            }
            else
            {
                res = roleState.init;
                //oldRoleState = res;
                selfAI.oldRoleState = res;
                // Debug.Log("need set roleState default init");
            }

        }

        return res;
    }


    /*
    private void setAttackEnd()
    {
        attcakStartEnd pAttackClass = App.Game.character.roleInstance.GetComponent<attcakStartEnd>();
      //  pAttackClass.attackJudge(0);
    }
    */
    
/*
    public bool updataRoleControl(float h, float tmpv, bool isfire, bool isJump = false)
    {
        roleState lState = getRoleNowState();

        roleState lHopeState = getHopeState(h, tmpv, isfire, isJump); //按键判断是否改变状态

        bool isChangeToJump = false;
        if ((lState != lHopeState) 
            && (oldRoleState != lHopeState))  //避免重复执行
        {
            if (lHopeState == roleState.jump) //切换成跳跃状态
                isChangeToJump = true;
            oldRoleState = lHopeState;
            PlayState(lHopeState);
        }
        

        return isChangeToJump;
    }
    */
}
