using UnityEngine;
using System.Collections;

using DamageCal;

public class UCharacterController {

    //角色对象
    public GameObject roleInstance = null;  

    //角色换肤管理
    public RoleChangeColorWeapon roleChangeColorWeaponMgr = null;

    //角色动作状态管理
    public RoleStateMgr mainRoleState = null;

    //角色位置及Camer管理
    public RolePosAndCamerMgr rolePosCamer = null;

   // public RoleDamageCal roleDamageManager = null;

    private roleProperty mainPro = null;
    private GameObject attackMonster = null; //被玩家攻击的怪物

    private bool isStart = false;

    private bool roleIsAttack = false; //玩家是否正在攻击中
    private bool roleIsEscape = false; //玩家逃跑中
    private bool roleIsDie = false; //玩家死亡

    public UCharacterController (int index,string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false) {

        roleChangeColorWeaponMgr = new RoleChangeColorWeapon(index, skeleton, weapon, head, chest, hand, feet, combine);
        roleInstance = roleChangeColorWeaponMgr.GetRoleInstance();
       // mainPro = roleInstance.transform.GetComponent<roleProperty>();
        //roleDamageManager = new RoleDamageCal();
        //roleInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    }
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

    public void initData(Transform pCameraTransform, Transform pRoleTranform, Vector3 pPos, Canvas pCanvas) {
        App.Game.character = this;
        //mainRoleState = new RoleStateMgr();
        mainRoleState = roleInstance.transform.GetComponent<RoleStateMgr>();

        mainRoleState.initData(roleInstance);
        //mainRoleState.setJumpTime(csJumpUpTime * 2);
        /*
        jumpCheck = roleInstance.transform.GetComponent<jumpColider>();
        jumpCheck.jumpDownOver = jumpDownOver;
        jumpCheck.isUse = false;
        */
        rolePosCamer = new RolePosAndCamerMgr();
        rolePosCamer.initData(roleInstance, pCameraTransform, pRoleTranform,pPos, pCanvas);

        mainPro = roleInstance.transform.GetComponent<roleProperty>();
        mainPro.InitData(pCameraTransform, pCanvas.transform);

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

    /*
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

    /*
     //设置玩家为攻击状态
     public void setRoleAttack(GameObject monsterObj) {
         //玩家UI显示
         //roleProperty mainPro = roleInstance.transform.GetComponent<roleProperty>();
         mainPro.showUI();

         attackMonster = monsterObj;
         Vector3 rolePos = attackMonster.transform.position;

         if (roleIsEscape == false) //玩家不在逃跑中
         {
             //玩家朝向怪物
             Vector3 lookPos = new Vector3(rolePos.x, roleInstance.transform.position.y, rolePos.z);
             roleInstance.transform.LookAt(rolePos);

             //玩家切换攻击状态
             mainRoleState.updataRoleControl(0, 0, true, false);

             roleIsAttack = true;
         }
     }


     //恢复玩家为不攻击状态
     public void setRoleIdle() {
         mainRoleState.updataRoleControl(0, 0, false, false);
         //roleProperty mainPro = roleInstance.transform.GetComponent<roleProperty>();
         mainPro.hideUI();
     }
     */
    private void drawSceneAlpha() {
        if(sceneAlpha == null)
            sceneAlpha = Camera.main.GetComponent<sceneAlphaControl>();

        sceneAlpha.drawSceneAlpha();
    }

    public void setRoleInEscape() {
        roleIsEscape = true;
    }

    public void setRoleNotEscape() {
        roleIsEscape = false;
    }

    /*
    //玩家攻击怪物
    public void monsterSubHp()
    {
        roleProperty monsterPro = attackMonster.transform.GetComponent<roleProperty>();
        int damage =  RoleDamageCal.instance.DamageCal(mainPro, monsterPro);
//        int damage = roleDamageManager.DamageCal(mainPro, monsterPro);
        monsterPro.SubHpValue(damage);

        if (monsterPro.hp == 0) { //怪物死亡
            //怪物死亡动画在 monsterAniControl.cs中自行判断处理

            //玩家停止攻击
            roleIsAttack = false;

            //玩家UI隐藏
            mainPro.hideUI();

            //玩家强制转为站立状态
            mainRoleState.playRoleStand();
            
        }

    }
    */

    public bool SubHp(roleProperty attackPro, roleProperty DefPro) {
        bool defIsDie = true;
        //int damage = roleDamageManager.DamageCal(attackPro, DefPro);
        int damage = RoleDamageCal.instance.DamageCal(attackPro, DefPro);
        DefPro.SubHpValue(damage);

        if (DefPro.hp == 0) //受击方死亡
        { 
            defIsDie = true;

        }

        return defIsDie;
    }
    /*
    //玩家受到攻击, 返回是否继续攻击
    public bool roleSubHp(roleProperty attackPro) {
        bool res = true;
        //int damage = roleDamageManager.DamageCal(attackPro, mainPro);
        int damage = RoleDamageCal.instance.DamageCal(attackPro, mainPro);
        mainPro.SubHpValue(damage);

        if (mainPro.hp == 0) { //玩家死亡
            roleIsDie = true;

            //怪物停止攻击, 怪物血条保持显示，玩家血条保持显示
            IbaseANI attackControl = attackPro.gameObject.transform.GetComponent<IbaseANI>();
            attackControl.setStopAttack();

            //玩家播放倒地动画
            mainRoleState.playRoleDie();

            res = false;
        }

        return res;
    }
    */
       public void Update () {
         if (isStart == false) {
              //  Debug.Log("game not start characterController");
                return;
         }

        if (roleIsDie) { //玩家死亡不再时行操作
            return ;
        }

         float pDeltaTime = Time.deltaTime;
        
         float leftright = Input.GetAxis("Horizontal");
         float downup = Input.GetAxis("Vertical");
         bool isfire = Input.GetButton("Fire1");
         bool isSetJump = Input.GetButton("Jump");
        //test();

        if ((roleIsEscape == false)) //不逃跑并正在攻击中
        {
            if (roleIsAttack)
            {
                leftright = 0.0f;
                downup = 0.0f;
                isfire = true;
            }
        }
        else {
            isfire = false;
        }

        float offsetY = 0.0f;

#if usejump

        if (mainRoleState.getRoleNowState() == RoleStateMgr.roleState.jump) { //跳跃状态中
            mainRoleState.addAllJumpTime(pDeltaTime);
            float allTime = mainRoleState.getAllJumpTime(); //已经跳的时间
            if (allTime <= csJumpUpTime) //计算人物坐标系中跳的高度
            { //上升中
                
                float nowY = 0.5f * jumpA * allTime * allTime;
                offsetY = nowY - oldY;
                oldY = nowY;
                
            }
            else 
            { 
                if (jumpCheck.isUse == false) { //开始下降，启用自由落体
                    jumpCheck.isUse = true;
                    Rigidbody roleRigid = roleInstance.transform.GetComponent<Rigidbody>();
                    roleRigid.constraints = RigidbodyConstraints.FreezeRotation; //启用自由落体
                    
                }
                /*
                if (jumpCheck.enabled)  //判断是否落地, 下落中
                {
                    float t1 = allTime - csJumpUpTime; ////计算下降了多长时间
                    float downY = 0.5f * jumpA * t1 * t1; //下落的距离
                    float nowY = csJumpHeightMax - downY;
                    offsetY = nowY - oldY;
                    oldY = nowY;

                    offsetY = 0.0f;
                }
                else { //已落地，由碰撞触发机制，不在update中执行
                    
                }
               */
            }
        }

        bool isChangeToJump = mainRoleState.updataRoleControl(leftright, downup, isfire, isSetJump ); //按键改变角色动作状态

        if (isChangeToJump) {//起跳
            Rigidbody roleRigid = roleInstance.transform.GetComponent<Rigidbody>();

            roleRigid.constraints = RigidbodyConstraints.FreezePositionY;
            roleRigid.freezeRotation = true;
            oldY = 0.0f; // jumpStartY;
        }
#else
            //目前不使用跳跃功能
            mainRoleState.updataRoleControl(leftright, downup, isfire, false);
#endif

            if ((leftright != 0.0f) || (downup != 0.0f) || (offsetY != 0.0f)) //角色是否有位移
            {
                rolePosCamer.updateRolePosWorld(leftright, downup, pDeltaTime, offsetY); //改变角色位置及朝向, 基于世界坐标

                drawSceneAlpha(); //场景虚化处理
                                  //rolePosCamer.updateRolePos(tmpv,h); //改变角色位置, 基于roleControl， 暂不考虑

                //判断并取消破坏物件判定，之后的考虑

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

            drawSceneAlpha(); //场景虚化处理
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
