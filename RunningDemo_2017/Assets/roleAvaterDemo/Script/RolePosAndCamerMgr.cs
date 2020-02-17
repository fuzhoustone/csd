using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePosAndCamerMgr  {

    private CharacterController roleControl = null;

    //角色的位置
    public Transform roleTranform;

    // 摄像机位置
    public Transform cameraTransform;
    private Camera mainCamer;

    // 摄像机距离人物的距离
    public float cameraDistance = 10.0f;

    // 摄像机距离人物最大的距离
    public float cameraDistanceMax = 15.0f;

    // 摄像机距离人物最小的距离
    public float cameraDistanceMin = 10.0f;

    // 前进移动速度
    float moveVSpeed;

    private Vector3 moveDirection = Vector3.zero;
    // 水平移动速度
    public float moveHSpeed = 5.0f;
    // 跳跃高度
    public float jumpHeight = 5.0f;

    // 跳跃标志 //当前不跳
    int m_jumpState = 0;
    // 最大速度
    public float maxVSpeed = 10.0f;
    // 最小速度
    public float minVSpeed = 5.0f;

    //鼠标点击时，移动的速度
    public float clickMoveSpeed = 0.1f;

    //世界坐标->wsad控制的坐标系的轩换矩阵
    private Matrix4x4 controlMat;

    public void setInitPosRota() {

    }

    private void setCameraAndTrans(Transform pCameraTransform, Transform pRoleTranform)
    {
        cameraTransform = pCameraTransform;
        mainCamer = pCameraTransform.GetComponent<Camera>();

        roleTranform = pRoleTranform;
     //   mainCamera = cameraTransform.GetComponent<Camera>();
       // gameManager = pGameManager;
       // isInit = true;
    }

    private void posChange(Vector3 pSource) {

        Matrix4x4 m = Matrix4x4.zero;
        float pRolate = 90.0f / 180.0f;
        m.m00 = Mathf.Cos(pRolate);
        m.m02 = Mathf.Sin(pRolate);
        m.m11 = Mathf.Sin(pRolate);
        m.m20 = -1* Mathf.Sin(pRolate);
        m.m22 = Mathf.Cos(pRolate);
        m.m33 = 1.0f;


    }

    public void initData(GameObject paraObj, Transform pCameraTransform, Transform pRoleTranform) {
        roleControl = paraObj.GetComponent<CharacterController>();

        // 摄像机距离人物的距离
        cameraDistance = 10.0f;

        // 摄像机距离人物最大的距离
        cameraDistanceMax = 15.0f;

        // 摄像机距离人物最小的距离
        cameraDistanceMin = 10.0f;

        moveDirection = Vector3.zero;
        // 水平移动速度
         moveHSpeed = 5.0f;
        // 跳跃高度
        jumpHeight = 5.0f;


        // 跳跃标志
         m_jumpState = 0;

        // 最大速度
         maxVSpeed = 10.0f;
        // 最小速度
         minVSpeed = 5.0f;

        //鼠标点击时，移动的速度
         clickMoveSpeed = 0.1f;


        moveVSpeed = minVSpeed;


        setCameraAndTrans(pCameraTransform, pRoleTranform);

        roleTranform.transform.position = new Vector3(5, 0.125f, -5);
        roleTranform.eulerAngles = new Vector3(0, 90, 0);

        //character.roleInstance.transform.position = new Vector3(5, 0.125f, -5);
        //character.roleInstance.transform.eulerAngles = new Vector3(0, 90, 0);

        // moveDirection = new Vector3(roleTranform.position.x, roleTranform.position.y, roleTranform.position.z);
        //控制坐标系初始化
        initControlCoordinateSystem();
    }

    /*
     
          1 //创建平移 旋转 缩放矩阵 可以理解为一个坐标系（不知道对不对。。）
 2         Matrix4x4 mat = Matrix4x4.TRS(new Vector3(1,1,1),Quaternion.Euler(0,90,0),Vector3.one);

 3         //得到在这个坐标系点（2,2,2）在世界坐标系的坐标
 4         print(mat.MultiplyPoint(new Vector3(2,2,2)));
 5         //在世界坐标系点（2,2,2）在mat变换下的坐标
 6         //局部坐标*mat = 世界坐标
 7         //世界坐标*mat的逆 = 局部坐标
 8         print(mat.inverse.MultiplyPoint(new Vector3(2,2,2)));
 9         //MultiplyVector方法 感觉没啥用
10         //把方向向量dir 做了一个旋转
11         Vector3 dir = new Vector3(3,2,3);
12         print (mat.MultiplyVector (dir) == Quaternion.Euler (0, 90, 0) * dir);
         
         */

    /*
     摄相机的上下左右
         参照点坐标系下，摄相机与参照点的世界坐标偏移可称为参照点坐标系统的，xyz向量

         参照点是场景中的某个点，以场景为坐标系的，上下左右平移

         左右上下旋转，以参照点是人物为中心点，绕着其中两个轴，球形移动

        拉进拉远，以参照点中心点
         
         */


    /*需实现的功能:
     * 1. 人物某个参照点坐标系按wsad移动及朝向
     *   1.参照点坐标本身受wsad影响移动进行
     
     * 2. 摄相机跟随某个参照点移动
     *    2.1 参照点本身会移动
          2.2 transform.lookat(Vector3); 可使摄相机朝向某个点
          2.3 参照点与摄相机的初始世界坐标 偏移，记录后，可视 向量
          2.4 参照点与摄相机的 向量移动，按键操作 左旋转，右旋转，拉近，拉远



        */

    //世界坐标系 -》 控制坐标系，主要涉及转向的计算 
    private void initControlCoordinateSystem() {
        Vector3 tmpPos = new Vector3(0, 0, 0);
        controlMat = Matrix4x4.TRS(tmpPos, Quaternion.Euler(0, 90, 0), Vector3.one); //不能受角色朝向影响

    }

    //计算初始移动参考点坐标 与世界坐标系之间的转换关系
    public void initMoveReference() {

    }

    //更新参考点坐标， 只在镜头变更时需要用到，目前不需要使用
    public void updateMoveReference() {

    }


    //以世界坐标系为参考的人物坐标位移计算

    //确认初始坐标（在此初始点，W就是向上，S向下，A向左，D向右）
    //根据按键的wsad来修改直接修改世界坐标

    private void test() {
        //人物的移动
        //某参考点，不一定是基于摄相机的，应是 基于 世界坐标系，缩放，旋转，偏移后的点，根据初始值可计算出参考点的矩阵变换关系（在此初始点，W就是向上，S向下，A向左，D向右）

        //角色的偏移= 基于某参考点 的 观察坐标系 下的偏移  直接转换为  世界坐标系下的偏移
        
        
        //角色的偏移= 基于某参考点 的 观察坐标系 下的偏移  转换为  基于角色自身坐标系下的偏移
        //角色自身坐标系的偏移  转换  世界坐标系的偏移

        //摄像机的移动
        //摄像机以人物的世界坐标系为参考，进行偏移

    }

    //人物坐标计算，用矩阵换算的方向
    public void updateRoleWorldPosFromControlMat(float leftright, float downup) {
        //WS 对应Z轴正负，    AD对应X轴 负正
        Vector3 tmpPos = new Vector3(leftright * moveVSpeed/30, 0, downup * moveVSpeed/30);

        
        Vector3 newMatPos = controlMat.MultiplyPoint(tmpPos);
        //  Vector3 newPos = new Vector3(newMatPos.x, 0, newMatPos.z); 

        Vector3 nowPos = roleTranform.transform.position;
        //Vector3 nowPos = new Vector3(roleTranform.transform.position.x, roleTranform.transform.position.y, roleTranform.transform.position.z);
        Vector3 pMove = newMatPos - new Vector3(0,0,0);

        //Vector3 newPos = new Vector3(nowPos.x + pMove.x, nowPos.y, nowPos.z + pMove.z);
        //

        
        Vector3 newPos = new Vector3(nowPos.x + pMove.x, nowPos.y + pMove.y, nowPos.z + pMove.z);
        
        roleTranform.transform.position = newPos;

       // initControlCoordinateSystem();

    }

    //人物坐标计算，用直接用xyz换算
    public void updateRoleWorldPosFromXYZCal(float leftright, float downup) {
       
        //基于世界坐标系，世界左手坐标系的人物移动，此游戏人物移动按世界X轴正前方，  y轴为高， 一般为0, W/S 对应X轴 正/负， A/D对 Z轴 负/正与世界Z轴方向相反，与人物模型的缩放无关，受旋转影响 x轴要*-1
        Vector3 pMove = new Vector3(downup * moveVSpeed / 30, 0, leftright * -1 * moveVSpeed / 30);

        //Vector3 nowPos = roleTranform.transform.position;
        Vector3 nowPos = new Vector3(roleTranform.transform.position.x, 0, roleTranform.transform.position.z);

        Vector3 newPos = new Vector3(nowPos.x + pMove.x, 0, nowPos.z + pMove.z);
        roleTranform.transform.position = newPos;
        // string msg = "roleTranform.transform.position.x: " + roleTranform.transform.position.x.ToString();
        //moveRolePosFromWorldPos(tmpPos);
       // msg += "   To " + roleTranform.transform.position.x.ToString();
       // Debug.Log(msg);
    }


    //基于世界坐标的 人物移动及朝向修改
    public void updateRolePosWorld(float leftright, float downup) { 
        //先修改坐标
       // updateRoleWorldPosFromXYZCal(leftright, downup);
        updateRoleWorldPosFromControlMat(leftright, downup);

        //再修改朝向
        Vector2 tmpPos2D = new Vector2();
        tmpPos2D.x = downup;
        tmpPos2D.y = leftright;

        float tmpRotat = 90.0f;
        float rotal = calRoleRotation(tmpPos2D, tmpRotat); //获得角色绕Y轴旋转朝向， 

        

        Vector3 tmpEulerAngles = new Vector3(roleTranform.transform.eulerAngles.x, rotal, roleTranform.transform.eulerAngles.z);
        roleTranform.eulerAngles = tmpEulerAngles;

        
    }

    //角色与摄像机的变换矩阵关系
    private Vector3 CamerFromRole;

    //受角色的坐标变更影响， 摄相机坐标位置调整, pOffSet，角色坐标的偏移值
    public void updateCamerPosFromRolePos(Vector3 pOffSet) {
        
    }


    //根据摄像机按键操作pOffSet，修改摄像机的位置,同时修改变换矩阵 计算CamerFromRole
    private void updateCamerVector3FormRoleControl(Vector3 pOffSet) {

    }

    //根据摄相机与人的变换矩阵关系，以及人物的位移， 刷新修改摄像机的位置
    private void updateCamerPosFromCamerVector3(Vector3 pOffSet) {

    }

    //根据当前的摄相机位置朝向及人物位置，设置摄相机与人的变换矩阵初始值
    private void setCamerVector3FromCamerAndRole() {
        //cameraTransform  摄像机transform
        //roleTranform     角色transform
        //计算CamerFromRole  角色 -》 摄像机的变换矩阵关系
        //  Matrix4x4 pmatri = new Matrix4x4();

        //世界坐标转换 以摄相机为中心矩阵关系
        Matrix4x4 camerMatrix = mainCamer.worldToCameraMatrix;

        //摄相机中的点，转换为世界坐标的矩阵关系
        Matrix4x4 worldcamerMatrix = mainCamer.cameraToWorldMatrix;

        
        //
        //Camera.worldToCameraMatrix
    }




    //用roleControl的控制人物移动，暂时不用
    public void updateRolePos(float h, float tmpv)
    {
        //人物移动
        moveDirection = new Vector3(h, 0, tmpv); //Allows for player input
        moveDirection = roleTranform.TransformDirection(moveDirection); //How to move
        moveDirection *= moveVSpeed; //How fast to move
                                     //  }
           
        //moveDirection.y -= 0 * Time.deltaTime;
        moveDirection.y = 0;  //没有跳跃，不用计算Y轴


       // roleControl.SimpleMove
        //Move the controller
        // roleControl.Move(moveDirection * Time.deltaTime);
    }

    //原控制摄相机的移动
    public void updateCamerChartPos(Transform trans, float h, float tmpv)
    {

        Vector3 vSpeed = new Vector3(trans.forward.x, trans.forward.y, roleTranform.forward.z) * moveVSpeed * tmpv;


        Vector3 hSpeed = new Vector3(trans.right.x, trans.right.y, trans.right.z) * moveHSpeed * h;

        Vector3 jumpSpeed = new Vector3(trans.up.x, trans.up.y, trans.up.z) * jumpHeight * m_jumpState;


        Vector3 vCameraSpeed = new Vector3(trans.forward.x, trans.forward.y, trans.forward.z) * minVSpeed;
        Vector3 destPos = trans.position + (vSpeed + hSpeed + jumpSpeed) * Time.deltaTime;

        moveChartToDestPos(trans, destPos);

    }

    private void moveChartToDestPos(Transform trans, Vector3 destPos, bool isMoveCamer = true)
    {
        trans.position = destPos;

        if (isMoveCamer)
        {
            if (trans.position.x - cameraTransform.position.x < cameraDistanceMin)
            {
                moveVSpeed = minVSpeed;
                cameraTransform.position = new Vector3(trans.position.x - cameraDistanceMin, cameraTransform.position.y, cameraTransform.position.z);
            }
            // 超过时 让摄像机赶上
            else if (trans.position.x - cameraTransform.position.x > cameraDistanceMax)
            {
                moveVSpeed = minVSpeed;
                cameraTransform.position = new Vector3(trans.position.x - cameraDistanceMax, cameraTransform.position.y, cameraTransform.position.z);
            }
        }
    }


    
    private void moveRolePosFromWorldPos(Vector3 pMove)
    {

        Vector3 nowPos = roleTranform.transform.position;
        Vector3 newPos = new Vector3(nowPos.x + pMove.x, nowPos.y + pMove.y, nowPos.z + pMove.z);
        roleTranform.transform.position = newPos;
    }


    //是否带有上
    private bool isUp(Vector2 pPos)
    {
        return pPos.x > 0.0f;
    }

    private bool isDown(Vector2 pPos)
    {
        return pPos.x < 0.0f;
    }

    private bool isNoneUpDown(Vector2 pPos)
    {
        return pPos.x == 0.0f;
    }

    private bool isLeft(Vector2 pPos)
    {
        return pPos.y < 0.0f;
    }

    private bool isRight(Vector2 pPos)
    {
        return pPos.y > 0.0f;
    }

    private bool isNoneLeftRight(Vector2 pPos)
    {
        return pPos.y == 0.0f;
    }

    private float calRoleRotation(Vector2 pPos, float initRotation)
    {


        float moveRotation = 0.0f;

        //上， 无左右
        if (isUp(pPos) && (isNoneLeftRight(pPos)))
        {
            moveRotation = 45.0f * 0;
        }

        //右上
        else if ((isRight(pPos)) && (isUp(pPos)))
        {
            moveRotation = 45.0f * 1;
        }

        //右， 无上下
        else if ((isRight(pPos)) && (isNoneUpDown(pPos)))
        {
            moveRotation = 45.0f * 2;
        }

        //右下
        else if ((isRight(pPos)) && (isDown(pPos)))
        {
            moveRotation = 45.0f * 3;
        }

        //下，无左右
        else if ((isDown(pPos)) && (isNoneLeftRight(pPos)))
        {
            moveRotation = 45.0f * 4;
        }

        //左下
        else if ((isLeft(pPos)) && (isDown(pPos)))
        {
            moveRotation = 45.0f * 5;
        }

        //左，无上下
        else if ((isLeft(pPos)) && (isNoneUpDown(pPos)))
        {
            moveRotation = 45.0f * 6;
        }

        //左上
        else if ((isLeft(pPos)) && (isUp(pPos)))
        {
            moveRotation = 45.0f * 7;
        }

        /*
        //上，无左右
        else if ((isUp(pPos)) && (isNoneLeftRight(pPos)))
        {

        }
        */

        float res = initRotation + moveRotation;

        if (res > 360.0f)
        {
            res = res - 360.0f;
        }

        return res;



    }

}
