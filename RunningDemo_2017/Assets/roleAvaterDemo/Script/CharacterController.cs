using UnityEngine;
using System.Collections;

public class UCharacterController {

    //角色对象
    public GameObject roleInstance = null;  

    //角色换肤管理
    public RoleChangeColorWeapon roleChangeColorWeaponMgr = null;

    //角色动作状态管理
    public RoleStateMgr roleState = null;

    //角色位置及Camer管理
    public RolePosAndCamerMgr rolePosCamer = null;

    private bool isStart = false;

    public UCharacterController (int index,string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false) {

        roleChangeColorWeaponMgr = new RoleChangeColorWeapon(index, skeleton, weapon, head, chest, hand, feet, combine);
        roleInstance = roleChangeColorWeaponMgr.GetRoleInstance();
    }
   
    public void initData(Transform pCameraTransform, Transform pRoleTranform) {
        App.Game.character = this;
        roleState = new RoleStateMgr();
        roleState.initData(roleInstance);

        rolePosCamer = new RolePosAndCamerMgr();
        rolePosCamer.initData(roleInstance, pCameraTransform, pRoleTranform);

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

   	public void Update () {
        if (isStart == false) {
          //  Debug.Log("game not start characterController");
            return;
        }

        float leftright = Input.GetAxis("Horizontal");
        float downup = Input.GetAxis("Vertical");
        bool isfire = Input.GetButton("Fire1");
       
        bool isUpdataState = roleState.updataRoleControl(leftright, downup, isfire); //改变角色动作状态

        if ((leftright != 0.0f) || (downup != 0.0f)) //角色是否有位移
        {
            rolePosCamer.updateRolePosWorld(leftright, downup); //改变角色位置及朝向, 基于世界坐标

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
            rolePosCamer.moveCamerWSADWorldPosFromCamerControlMat(camerleftRight, camerDownUp);
            
        }

        //计算摄相机是否要进行旋转
        float camerRotationY = Input.GetAxis("CamerRotationY");
        if (camerRotationY != 0.0f) {
            rolePosCamer.rolationCamerY(camerRotationY);
        }

        float camerRotationZ = Input.GetAxis("CamerRotationZ");
        if (camerRotationZ != 0.0f)
        {
            rolePosCamer.rolationFromRoleZ(camerRotationZ);
        }


        //人物拉进拉远
        float camerScale = Input.GetAxis("CamerScale");
        if (camerScale != 0.0f) {
            rolePosCamer.scaleCamer(camerScale);
        }
        
    }
}
