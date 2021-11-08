using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using stoneState;

public class monsterAniControl : MonoBehaviour, IbaseANI
{
    /*
    const string strStand = "stand";

    const string strRun = "run";
    const string strAttack = "attack1";
    const string strAttack2 = "attack2";
    const string strDie = "die";
    */
    const string csInAttack = "InAttack";
    const string csHp = "Hp";
    const string csMainAniLayer = "mainAniLayer";

    
    [SerializeField]
    public string strStand = "stand";
    public string strRun = "run";
    public string strAttack = "attack1";
    public string strAttack2 = "attack2";
    public string strDef = "defend";
    public string strDie = "die";


    private string nowState = "";
    private Animator animator = null;
    private int mainLayer = -2;
    private bool isInAttack = false;

    // private Material mMaterial = null;
    private Material mAlphaMai = null;
    private roleProperty mMonsterPro = null;
    //   private roleProperty mRolePro = null;

    private UCharacterController charInstance = null;


    void Start()
    {
        animator = GetComponent<Animator>();
        mainLayer = animator.GetLayerIndex(csMainAniLayer);
        mAlphaMai = new Material(Shader.Find("Unity Shaders Book/Chapter 7/NormalMapWorldAlpha"));
        isInAttack = false;

        mMonsterPro = this.transform.GetComponent<roleProperty>();

    }

    //是否处于某个 动画状态（最后一帧播完也算）
    private bool isInState(string stateName) {
        bool res = false;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(stateName))
            res = true;

        return res;
    }

    public void initData(GameObject paraObj) {
        if (animator == null) {
            animator = GetComponent<Animator>();
            mainLayer = animator.GetLayerIndex(csMainAniLayer);
            mAlphaMai = new Material(Shader.Find("Unity Shaders Book/Chapter 7/NormalMapWorldAlpha"));
            isInAttack = false;

            mMonsterPro = this.transform.GetComponent<roleProperty>();
        }
    }

    private string getStateStrName(roleState entryName) {
        string statestr = strStand;
        switch (entryName)
        {
            case roleState.init:
                {
                    statestr = strStand;
                }
                break;
            case roleState.stand:
                {
                    statestr = strStand;
                }

                break;
            case roleState.run:
                {
                    statestr = strRun;
                }
                break;
            case roleState.attack:
                {
                    statestr = strAttack;
                }
                break;
            case roleState.attack2:
                {
                    statestr = strAttack2;
                }
                break;
            case roleState.def:
                {
                    statestr = strDef;
                }
                break;
            case roleState.die:
                {
                    statestr = strDie;
                }
                break;
            default:
                {
                    statestr = strStand;
                }
                break;
        }

        return statestr;
    }

    //判断当前动画是否正在播， 播至最后一帧或播放其它动画返回false
    public bool isInPlayEntry(roleState stateName)
    {
        string entryName = getStateStrName(stateName);
        bool res = false;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName(entryName))
        {
            //动画播到最后一帧 或未开始
            //if ((info.normalizedTime >= 1.0f) || (info.normalizedTime < 0.0f))
            if (info.normalizedTime >= 1.0f) //动画播到最后一帧
            {
                res = false;
            }
            else //未开始或正在播
            {  //动画正在播
                res = true;

            }
        }
        else
            res = false;

        return res;
    }

    public void PlayState(roleState stateName) {

        string state = getStateStrName(stateName);
        animator.Play(state, mainLayer, 0.0f);  //从第0帧开始播
    }

    /*
    public bool calAttackHp(roleProperty tmpPro) {
        bool res = false;
        res = App.Game.character.SubHp(mMonsterPro, tmpPro);
        return res;
    }
    */
    /*
    //攻击动画播至最后一帧，不改变状态会不停触发
    public bool attackStateEnd() {
        //计算伤害,并UI显示
        bool res = false;
        res = App.Game.character.roleSubHp(mMonsterPro);

        // animator.Play(csAttack, mainLayer, 0.0f);  //从第0帧开始播
        return res;
    }
    */
    private void dieFadeOutEnd() {
        mMonsterPro.hideUI();
        this.gameObject.SetActive(false);
        stageMgr.stage().addClearMonster();
    }

    private void StartDieFadeOut() {



        Transform body = this.transform.Find("Body");
        if (body != null)
        {
            SkinnedMeshRenderer tRender = body.GetComponent<SkinnedMeshRenderer>();
            tRender.material = mAlphaMai;
        }

        StartCoroutine(dieFadeOutIEn(dieFadeOutEnd));
    }

    const float fFadeOutTime = 5.0f;
    IEnumerator dieFadeOutIEn(Action OnEnd)
    {
        float time = 0;

        //float fadeLength = 5.0f;
        while (time < fFadeOutTime) // 还需另外设置跳出循环的条件
        {
            time += Time.deltaTime;
            float colorA = Mathf.InverseLerp(1, 0, time / fFadeOutTime);
            if (mAlphaMai != null)
                mAlphaMai.SetFloat("_AlphaScale", colorA);
            //mMaterial.shader
            yield return null;
        }

        if (OnEnd != null) {
            OnEnd();
        }

    }

    //死亡动画播至最后一帧,只触发一次
    public void dieStateEndAct() {
        StartDieFadeOut();
    }

    //待完成
    public roleState getRoleNowState() {
        roleState resState = roleState.init;
        //需把所有状态遍历一遍才可知
        if (animator == null) {
            return resState;
        }

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1.0f)
        {
            resState = roleState.init;
        }
        else
        {
            if (info.IsName(strStand)) {
                resState = roleState.stand;
            }
            else if(info.IsName(strRun)) {
                resState = roleState.run;
            }
            else if (info.IsName(strAttack))
            {
                resState = roleState.attack;
            }
            else if (info.IsName(strAttack2))
            {
                resState = roleState.attack2;
            }
            else if (info.IsName(strDef))
            {
                resState = roleState.def;
            }
            else if (info.IsName(strDie))
            {
                resState = roleState.die;
            }
        }
        

         return resState;
    }



       
    void Update()
    {
        /*
        //怪物死亡
        int hp = animator.GetInteger(csHp);
        if (mMonsterPro.hp != hp) {
            hp = mMonsterPro.hp;
            animator.SetInteger(csHp, hp);
        }

        if (hp <= 0)
        {
            //不在死亡动画播放中
            if (isInState(csDie) == false)
                animator.Play(csDie);

            return;
        }
        */
        //怪物存活
        /*
        // 攻击状态  与 站立状态的变更
        bool isInAttack = IsInAttackState();
        if (isInAttack)
        {
            if (isInState(csAttack) == false)
                animator.Play(csAttack);
        }
        else {
         
        }
        */
    }
}
