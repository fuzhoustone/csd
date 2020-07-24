using UnityEngine;
using System.Collections;

public class UCharacterController {

    //角色对象
    public GameObject roleInstance = null;  

    //角色换肤管理
    public RoleChangeColorWeapon roleChangeColorWeaponMgr = null;

    //角色动作状态管理
    public RoleStateMgr mainRoleState = null;

    //角色位置及Camer管理
    public RolePosAndCamerMgr rolePosCamer = null;

    private bool isStart = false;

    public UCharacterController (int index,string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false) {

        roleChangeColorWeaponMgr = new RoleChangeColorWeapon(index, skeleton, weapon, head, chest, hand, feet, combine);
        roleInstance = roleChangeColorWeaponMgr.GetRoleInstance();
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

    public void initData(Transform pCameraTransform, Transform pRoleTranform, Vector3 pPos, Canvas pCanvas) {
        App.Game.character = this;
        mainRoleState = new RoleStateMgr();
        mainRoleState.initData(roleInstance);
        mainRoleState.setJumpTime(csJumpUpTime * 2);

        jumpCheck = roleInstance.transform.GetComponent<jumpColider>();
        jumpCheck.jumpDownOver = jumpDownOver;
        jumpCheck.isUse = false;

        rolePosCamer = new RolePosAndCamerMgr();
        rolePosCamer.initData(roleInstance, pCameraTransform, pRoleTranform,pPos, pCanvas);
        

        //计算跳跃的加速度
        //s=0.5*a*t*t  a= s/0.5/t/t 
        jumpA = csJumpHeightMax / 0.5f / csJumpUpTime / csJumpUpTime;

        updateGravity();
        // setJumpDownRigidBody();
        // test();
        //testGrav();

          isStart = true;
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

    private void jumpDownOver() {
        mainRoleState.isJumpDownTouch = true;
        oldY = 0;
        Rigidbody roleRigid = roleInstance.transform.GetComponent<Rigidbody>();
        roleRigid.constraints = RigidbodyConstraints.FreezeRotation; //启用自由落体
    }

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

       public void Update () {
        if (isStart == false) {
          //  Debug.Log("game not start characterController");
            return;
        }


        float pDeltaTime = Time.deltaTime;
        float leftright = Input.GetAxis("Horizontal");
        float downup = Input.GetAxis("Vertical");
        bool isfire = Input.GetButton("Fire1");
        bool isSetJump = Input.GetButton("Jump");
        //test();

        float offsetY = 0.0f;
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
        

        if ((leftright != 0.0f) || (downup != 0.0f) || (offsetY != 0.0f)) //角色是否有位移
        {
            rolePosCamer.updateRolePosWorld(leftright, downup, pDeltaTime, offsetY); //改变角色位置及朝向, 基于世界坐标

            //rolePosCamer.updateRolePos(tmpv,h); //改变角色位置, 基于roleControl， 暂不考虑


            //判断并取消破坏物件判定，之后的考虑

            //更改摄像机位置， 基于人的世界坐标
            //   rolePosCamer.updateCamerChartPos(roleInstance.transform, leftright, downup);  //摄像机判断并跟随
            //rolePosCamer.moveCamerWSADWorldPosFromControlMat(leftright, downup);
        }

        
        //摄相机平行场景 上下左右移动
        float camerleftRight = Input.GetAxis("HorizontalCamer");
        float camerDownUp = Input.GetAxis("VerticalCamer");
        if ((camerleftRight != 0.0f) || (camerDownUp != 0.0f)) {
           // rolePosCamer.moveCamerWSADWorldPosFromControlMat(camerleftRight, camerDownUp);
            rolePosCamer.moveCamerWSADWorldPosFromCamerControlMat(camerleftRight, camerDownUp, pDeltaTime);
            
        }

        //计算摄相机是否要进行旋转
        float camerRotationY = Input.GetAxis("CamerRotationY");
        if (camerRotationY != 0.0f) {
            rolePosCamer.rolationCamerY(camerRotationY, pDeltaTime);
        }

        float camerRotationZ = Input.GetAxis("CamerRotationZ");
        if (camerRotationZ != 0.0f)
        {
            rolePosCamer.rolationFromRoleZ(camerRotationZ, pDeltaTime);
        }


        //人物拉进拉远
        float camerScale = Input.GetAxis("CamerScale");
        if (camerScale != 0.0f) {
            rolePosCamer.scaleCamer(camerScale, pDeltaTime);
        }
        
    }
}
