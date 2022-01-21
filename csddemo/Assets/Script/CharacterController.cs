using UnityEngine;
using System.Collections;

using DamageCal;
using System;

public class UCharacterController {

    //角色对象
    public GameObject roleInstance = null;
 //   public GameObject roleFlagInstance = null;

    //角色换肤管理
    public RoleChangeColorWeapon roleChangeColorWeaponMgr = null;

    //角色动作状态管理
    //public RoleStateMgr mainRoleState = null;

    public roleAI mainRoleState = null;

    //角色位置及Camer管理
    public RolePosAndCamerMgr rolePosCamer = null;

   // public RoleDamageCal roleDamageManager = null;

    private roleProperty mainPro = null;
    private GameObject attackMonster = null; //被玩家攻击的怪物
    private Transform monsterParent = null;

    private bool isStart = false;

    private bool roleIsAttack = false; //玩家是否正在攻击中
    private bool roleIsEscape = false; //玩家逃跑中
    private bool roleIsDie = false; //玩家死亡

    private csdsleep skillAttack1 = null;
    private csdsleep skillAttack2 = null;
    private csdsleep skillDef = null;
    private csdsleep skillEscape = null;


    public UCharacterController(string preStr) {
        roleChangeColorWeaponMgr = new RoleChangeColorWeapon(preStr);
        roleInstance = roleChangeColorWeaponMgr.GetRoleInstance();
    }

    /*
    public UCharacterController (int index,string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false) {

        roleChangeColorWeaponMgr = new RoleChangeColorWeapon(index, skeleton, weapon, head, chest, hand, feet, combine);
        roleInstance = roleChangeColorWeaponMgr.GetRoleInstance();
     //   roleFlagInstance = roleChangeColorWeaponMgr.GetRoleFlagInstance();
    }
*/
    //跳的高度值
    private const float csJumpHeightMax = 0.5f;
    //跳跃时，升空的时间
    private const float csJumpUpTime = 1.0f;

    //跳跃时上一帧的Y高度
    private float oldY;

    //跳跃时起跳时的高度
    //private float jumpStartY;

    //跳跃时的重力加速度
    private float jumpA = 0.0f;

    //跳跃时间累计
    //private float jumpTimeAdd;

    //检查是否要跳跃
    private jumpColider jumpCheck;

    //镜头的虚化
    private sceneAlphaControl sceneAlpha;

    public void initData(Transform pCameraTransform, 
                         Transform pRoleTranform,
                         Vector3 pPos, 
                         Canvas pCanvas, 
                         Transform pMapCamerTransform, 
                         //Transform pRoleFlagTrans,
                         Transform pMonsterParent) {
        App.Game.character = this;
        //mainRoleState = new RoleStateMgr();
        //mainRoleState = roleInstance.transform.GetComponent<RoleStateMgr>();
        //mainRoleState.initData(roleInstance);
        monsterParent = pMonsterParent;

        mainRoleState = roleInstance.transform.GetComponent<roleAI>();
        mainRoleState.AIInitData(roleInstance);

        rolePosCamer = new RolePosAndCamerMgr();
        rolePosCamer.initData(roleInstance, pCameraTransform, pRoleTranform,pPos, pCanvas, pMapCamerTransform);

        mainPro = roleInstance.transform.GetComponent<roleProperty>();
        mainPro.InitData(pCameraTransform, pCanvas.transform,1);

        //计算跳跃的加速度
        //s=0.5*a*t*t  a= s/0.5/t/t 
        jumpA = csJumpHeightMax / 0.5f / csJumpUpTime / csJumpUpTime;

        updateGravity();
        // setJumpDownRigidBody();
        // test();
        //testGrav();
        sceneAlpha = null;
        attackMonster = null;

        isStart = true;

        roleIsAttack = false;
        roleIsEscape = false;
        roleIsDie = false;
        skillInit();

    }

    public void onlyRoleDestory() {
        roleInstance = null;

        roleChangeColorWeaponMgr.dataDestory();
        roleChangeColorWeaponMgr = null;

        mainRoleState = null;
        //角色位置及Camer管理
        rolePosCamer = null;

        mainPro = null;
        attackMonster = null; //被玩家攻击的怪物



        roleIsAttack = false; //玩家是否正在攻击中
        roleIsEscape = false; //玩家逃跑中
        roleIsDie = false; //玩家死亡
        skillDestory();
    }

    public void dataDestory()
    {
        isStart = false;
        onlyRoleDestory();


    }
    public void rolePause() {
        isStart = false;
    }

    public void roleResume() {
        isStart = true;
    }

    public void clearSceneAlpha() {
        if (sceneAlpha != null) {
            sceneAlpha.clearData();
        }
    }
    /*
	public void ChangeHeadEquipment (string equipment,bool combine = false)
	{
        if (roleChangeColorWeaponMgr != null)
            roleChangeColorWeaponMgr.ChangeHeadEquipment(equipment, combine);
      
	}
	
	public void ChangeChestEquipment (string equipment,bool combine = false)
	{
        if (roleChangeColorWeaponMgr != null)
            roleChangeColorWeaponMgr.ChangeChestEquipment(equipment, combine);
     
	}
	
	public void ChangeHandEquipment (string equipment,bool combine = false)
	{
        if (roleChangeColorWeaponMgr != null)
            roleChangeColorWeaponMgr.ChangeHandEquipment(equipment, combine);
      
	}
	
	public void ChangeFeetEquipment (string equipment,bool combine = false)
	{
        if (roleChangeColorWeaponMgr != null)
            roleChangeColorWeaponMgr.ChangeFeetEquipment(equipment, combine);

       // ChangeEquipment (3, equipment, combine);
	}
	
	public void ChangeWeapon (string weapon)
	{
        if (roleChangeColorWeaponMgr != null)
            roleChangeColorWeaponMgr.ChangeWeapon(weapon);

    }

    
    private void jumpDownOver() {
        mainRoleState.isJumpDownTouch = true;
        oldY = 0;
        Rigidbody roleRigid = roleInstance.transform.GetComponent<Rigidbody>();
        roleRigid.constraints = RigidbodyConstraints.FreezeRotation; //启用自由落体
    }
    */
    private bool isUseGrav = false;
    private float g = 0.0f;

    void Awake() {
        if (jumpA != 0)
        {
            if (Physics.gravity.y != jumpA)
            {
                Physics.gravity = new Vector3(0, jumpA, 0);
                Debug.LogWarning("update gravity");
            }
        }
    }

    public void test() {

        Debug.LogWarning("update grav" + jumpA.ToString());
        //isUseGrav = true;
        //g = g + 9.81f/3.0f;
        //Debug.LogWarning(g.ToString());
        //testGrav();
    }

    private void updateGravity() {
        Physics.gravity = new Vector3(0, jumpA * -1, 0);
    }

    private void skillInit() {
        if (skillEscape == null)
        {
            skillEscape = CsdUIControlMgr.uiMgr().uiMenu.gameObject.AddComponent<csdsleep>();
            skillEscape.sleepInit(fEscapeTime, setRoleNotInEscape, null);
        }

        if (skillAttack1 == null) {
            skillAttack1 = CsdUIControlMgr.uiMgr().uiMenu.gameObject.AddComponent<csdsleep>();
            skillAttack1.sleepInit(attack1Sleep, callBackAttack1, updateUIAttack1);
            CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack1Amount(attack1Sleep, attack1Sleep);
        }

        if (skillAttack2 == null)
        {
            skillAttack2 = CsdUIControlMgr.uiMgr().uiMenu.gameObject.AddComponent<csdsleep>();
            skillAttack2.sleepInit(attack2Sleep, callBackAttack2, updateUIAttack2);
            CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack2Amount(attack2Sleep, attack2Sleep);
        }

        if (skillDef == null)
        {
            skillDef = CsdUIControlMgr.uiMgr().uiMenu.gameObject.AddComponent<csdsleep>();
        }
    }

    private void skillDestory() {
        skillAttack1.stopSleep();
        skillAttack2.stopSleep();
        skillDef.stopSleep();
        skillEscape.stopSleep();
        
        GameObject.Destroy(skillAttack1);
        GameObject.Destroy(skillAttack2);
        GameObject.Destroy(skillDef);
        GameObject.Destroy(skillEscape);

    }
    /*
    private void drawSceneAlpha() {
        if(sceneAlpha == null)
            sceneAlpha = Camera.main.GetComponent<sceneAlphaControl>();

        sceneAlpha.drawSceneAlpha();
    }
    */
    private float attack1Sleep = 5.0f;
    private float attack2Sleep = 5.0f;
    private void updateUIAttack1(float val)
    {
        CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack1Amount(val, attack1Sleep);
    }
    private void updateUIAttack2(float val)
    {
        CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack2Amount(val, attack2Sleep);
    }
    private void callBackAttack1() {
        skillAttack1.isCD = false;
        CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack1Amount(attack1Sleep, attack1Sleep);
    }

    private void callBackAttack2()
    {
        skillAttack2.isCD = false;
        CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack1Amount(attack2Sleep, attack2Sleep);
    }



    /// <summary>
    /// 技能按扭触发事件
    /// </summary>
    private const string csNoEnemy = "没有敌人";
    public void roleAttack1() {
        if (mainRoleState.enemyObj != null)
        {
            if (skillAttack1.isCD == false)
            {
                skillAttack1.isCD = true;
                // CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack1Amount(0.0f, attack1Sleep);
                mainRoleState.AIRoleSkill(1);
                skillAttack1.startEvent();
            }
        }
        else {
            CsdUIControlMgr.uiMgr().msgNote(csNoEnemy);
        }
    }

    public bool roleHasEnemy() {
        if (mainRoleState.enemyObj != null)
            return true;
        else
            return false;
    }

    public void roleAttack2() {
        if (mainRoleState.enemyObj != null)
        {
            if (skillAttack2.isCD == false)
            {
                skillAttack2.isCD = true;
                // CsdUIControlMgr.uiMgr().uiMenu.updateImageAttack2Amount(0.0f, attack2Sleep);
                mainRoleState.AIRoleSkill(2);
                skillAttack2.startEvent();
            }
        }
        else {
            CsdUIControlMgr.uiMgr().msgNote(csNoEnemy);
        }
    }

    public void roleDef()
    {
        if (mainRoleState.enemyObj != null)
        {
            mainRoleState.AIRoleSkill(3);
        }
    }

    const float fEscapeTime = 5.0f;
    private void setRoleInEscape() {
        if (skillEscape.isCD == false)
        {
            roleIsEscape = true;
            skillEscape.startEvent();
        }

        //    StartCoroutine(escapeIEn());
    }

    private void setRoleNotInEscape() {
        roleIsEscape = false;
    }

    private void updateReady() {
        if (mainRoleState.isAutoRun == true) //战斗开场前的自动移动
        {
            //不接受控制
        }
        else if (mainRoleState.oldReadyMonster == null) {
            mainRoleState.checkReadyMonster(monsterParent);
        }

        


    }
   
        
         
         
        


    public void Update () {  //手动输入检测
         if (isStart == false) {
              //  Debug.Log("game not start characterController");
                return;
         }

        
        if (mainRoleState.selfIsLive() == false) { //玩家死亡不再时行操作
            return ;
        }



         float pDeltaTime = Time.deltaTime;
        
         float leftright = Input.GetAxis("Horizontal");
         float downup = Input.GetAxis("Vertical");
         
         float offsetY = 0.0f;
        if (mainRoleState.enemyObj != null) //有敌人
        {
            leftright = 0.0f;
            downup = 0.0f;
        }

        //目前不使用跳跃功能，根据输入来修改主角的动作状态
        mainRoleState.updataAIRoleControl(leftright, downup, 0);


            if ((leftright != 0.0f) || (downup != 0.0f) || (offsetY != 0.0f)) //角色有位移
            {
                rolePosCamer.updateRolePosWorld(leftright, downup, pDeltaTime, offsetY); //改变角色位置及朝向, 基于世界坐标

                //drawSceneAlpha(); //场景虚化处理

                //更改摄像机位置， 基于人的世界坐标
                //   rolePosCamer.updateCamerChartPos(roleInstance.transform, leftright, downup);  //摄像机判断并跟随
                //rolePosCamer.moveCamerWSADWorldPosFromControlMat(leftright, downup);
            }

         

#if camerdebug        
        //摄相机平行场景 上下左右移动
        float camerleftRight = Input.GetAxis("HorizontalCamer");
        float camerDownUp = Input.GetAxis("VerticalCamer");
        if ((camerleftRight != 0.0f) || (camerDownUp != 0.0f)) {
           // rolePosCamer.moveCamerWSADWorldPosFromControlMat(camerleftRight, camerDownUp);
            rolePosCamer.moveCamerWSADWorldPosFromCamerControlMat(camerleftRight, camerDownUp, pDeltaTime);
        }
#endif
        //计算摄相机是否要进行旋转
       
        float camerRotationY = Input.GetAxis("CamerRotationY");
        if (camerRotationY != 0.0f) {
            rolePosCamer.rolationCamerY(camerRotationY, pDeltaTime);

            //drawSceneAlpha(); //场景虚化处理
        }

#if camerdebug
        float camerRotationZ = Input.GetAxis("CamerRotationZ");
        if (camerRotationZ != 0.0f)
        {
            rolePosCamer.rolationFromRoleZ(camerRotationZ, pDeltaTime);
        }
#endif

        //人物拉进拉远
        float camerScale = Input.GetAxis("CamerScale");
        if (camerScale != 0.0f) {
            rolePosCamer.scaleCamer(camerScale, pDeltaTime);
        }
        
    }
}
