using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using stoneState;

public class RoleStateMgr: MonoBehaviour,IbaseANI
{
    private Animation m_animationController = null;
    // public CharacterController roleControl = null;

    private roleState oldRoleState = roleState.init;

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

        //设置播放速度，实际无效
        foreach (AnimationState state in m_animationController)
        {
            if ((state.name == csAttack1) || (state.name == csAttack2) || (state.name == csAttackEnd))
                state.speed = csPlaySpeedTime;
        }


        oldRoleState = roleState.stand;
        changeRoleState(roleState.stand);
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



    public void dieStateEnd() {
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

    private roleState getHopeState(float h, float tmpv, bool isfire, bool isKeyJump)
    {
        roleState nowState = getRoleNowState();
        roleState res = roleState.init;
        /*
        if (nowState == roleState.attack)
        { //攻击状态中，不能被其它打断
            //roleState = roleState.attack;
            res = nowState;
            return res;
        }
        */
/*
        if (nowState == roleState.jump)
        { //跳跃状态中，累计时间未到不能切换
            res = nowState;

            if (jumpAllTime <= jumpTime) //跳跃上升中，  //继续保持跳跃
            {
                res = nowState;
                //跳跃的累计时间超过 跳跃时间, 落地切换成站立

            }
            else if (isJumpDownTouch == false) //下降中，继续保持跳跃动作
            {
                res = nowState;
            }
            else
            { //落地，改为站立动作
                clearJumpTime();
                res = roleState.stand;
            }


            return res;
        }
        */
        if (nowState == roleState.die) {
            res = nowState;
            return res;
        }

        //接下来都是能打断的状态
        roleState tmpState = roleState.init;
        if ((h == 0.0f) && (tmpv == 0.0f))
            tmpState = roleState.stand;
        else
            tmpState = roleState.run;

        if (isfire)
            tmpState = roleState.attack;
        else if (isKeyJump) //有跳跃按键
            tmpState = roleState.jump;

        res = tmpState;

        return res;
        /*
        //其它状态下，
        if (isfire) {  //按下了攻击键
            if ((nowState == roleState.init) //攻击能打断的状态如下
                || (nowState == roleState.stand)
                || (nowState == roleState.run)
                )
                res = roleState.attack;
            else  
                res = nowState;
        }
        else
        {
            if (isKeyJump) //有跳跃按键
            {
                res = roleState.jump;
            }
            else //无跳跃
            {
                if ((h == 0.0f) && (tmpv == 0.0f))
                    res = roleState.stand;
                else
                    res = roleState.run;
            }
        }

        return res;
        */
    }

    private roleState getRoleNowState()
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
                    oldRoleState = res;
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
                oldRoleState = res;
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
            changeRoleState(lHopeState);
        }
        

        return isChangeToJump;
    }
}
