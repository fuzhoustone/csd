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

    //移动速度
    private const float csMoveSpeed = 1.0f;

    //摄像机移动速度
    private const float csMoveCamerVSpeed = 20.0f;
    private const float csMoveCameHSpeed = 20.0f;

    //跳的高度值
    private const float csJumpHeight = 0.5f;
    //跳跃时，升空及下降的时间
    private const float csJumpUpOrDownTime = 3.0f;

    //跳跃时的重力加速度
    private float jumpA;

    //跳跃时间累计
    private float jumpTimeAdd;

    //旋转速度，y,z
    private const float csRolateY = 40.0f;
    private const float csRolateZ = 25.0f;
    /*
    //拉进拉远的速度
    private const float csScale = 1.0f;

    private const float csMaxScale = 5.0f;
    private const float csMinScale = 0.4f;

    //摄像机与人物默认的矩离
    private const float csDefaultCamerPosX = 0.0f;
    private const float csDefaultCamerPosY = 1.0f;  //10.0f
    private const float csDefaultCamerPosZ = -1.0f;  //-10.0f
    */
     // 水平移动速度
    public float moveHSpeed;
    // 跳跃高度
    public float jumpHeight;

    // 跳跃标志 //当前不跳
    int m_jumpState = 0;
    // 最大速度
    public float maxVSpeed = 10.0f;
    // 最小速度
    public float minVSpeed = 5.0f;

    //鼠标点击时，移动的速度
    public float clickMoveSpeed = 0.1f;

    //世界坐标->wsad控制的坐标系的变换矩阵
    private Matrix4x4 controlMat;

    //人物参照点坐标系的Y轴旋转角度
    private float roleRotatY = 90.0f;

    //摄相机缩放参数
    public float scaleCamerParam = 1.0f;

    private Canvas roleCanvas = null;

    private PosRefer camerByRoleRefer; // 摄像机参照人物关系

    private Vector3 roleMoveRotat; //人物移动方向

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

    public void initData(GameObject paraObj, Transform pCameraTransform, Transform pRoleTranform, Vector3 pPos, Canvas pCanvas) {
        roleControl = paraObj.GetComponent<CharacterController>();

        // 摄像机距离人物的距离
        cameraDistance = 10.0f;

        // 摄像机距离人物最大的距离
        cameraDistanceMax = 15.0f;

        // 摄像机距离人物最小的距离
        cameraDistanceMin = 10.0f;

        moveDirection = Vector3.zero;
        // 水平移动速度
         moveHSpeed = csMoveSpeed;
        // 跳跃高度
        jumpHeight = csJumpHeight;

        // 跳跃标志
         m_jumpState = 0;

        // 最大速度
         maxVSpeed = 10.0f;
        // 最小速度
        // minVSpeed = 7.0f;
         minVSpeed = csMoveSpeed;

        //鼠标点击时，移动的速度
         clickMoveSpeed = 0.1f;


        moveVSpeed = minVSpeed;

        //摄相机缩放参数    
        //scaleParam = 1.0f;
        roleRotatY = 90.0f;

        //计算跳跃的加速度
        //s=0.5*a*t*t  a= s/0.5/t/t 
        //jumpA = csJumpHeight / 0.5f / csJumpUpOrDownTime / csJumpUpOrDownTime;
        //jumpTimeAdd = 0.0f;
        scaleCamerParam = 1.0f;
        roleCanvas = pCanvas;
        setCameraAndTrans(pCameraTransform, pRoleTranform);

        roleTranform.transform.position = pPos;
        roleTranform.eulerAngles = new Vector3(0, 90, 0);

        camerByRoleRefer = new PosRefer();
        camerByRoleRefer.initRefer(); // 摄像机参照人物关系初始化

        roleMoveRotat = new Vector3(0, 90, 0);

        resetCamerPosFromRole();
        //控制坐标系初始化
        //initControlCoordinateSystem();
        //cameraTransform.LookAt(roleTranform.transform);
    }
    /*
     
          1 //创建平移 旋转 缩放矩阵 可以理解为一个坐标系
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

    //世界坐标系 -》 控制坐标系，主要涉及转向的计算 
    private void initControlCoordinateSystem(bool updateRotatY = true) {
        controlMat = Matrix4x4.TRS(roleTranform.transform.position, Quaternion.Euler(roleMoveRotat), Vector3.one); //不能受角色朝向影响
    }

    private void updateControlMatFromRoltateY(float roltateY) {
        float oldY = controlMat.rotation.eulerAngles.y;
        float newY = controlMat.rotation.eulerAngles.y + roltateY;
        if (newY > 360.0f) {
            newY -= 360.0f;
        }
        else if (newY < -360.0f) {
            newY += 360.0f;
        }

        roleMoveRotat = new Vector3(0, newY, 0);

        //人物坐标系与世界坐标系 变换矩阵更新
        initControlCoordinateSystem();

    }


    public void test() {
        
    }

    private void scaleCanvas() {
        roleCanvas.scaleFactor = 1.0f / scaleCamerParam;
    }

    //private float scaleParam = 1.0f;
    //摄相机对着人物拉进拉远
    public void scaleCamer(float scale, float pDeltaTime) {

        //计算摄像机与人物的矩离的缩放比例
        scaleCamerParam = camerByRoleRefer.updateScale(scale, pDeltaTime);

        //获得摄相机在人物坐标系中的默认点
        Vector3 tmpCamer = camerByRoleRefer.referDefPosition;

        //计算摄相机在人物坐标系中的缩放后的相对坐标，人物坐标系以人物移动方向为参照
        Vector3 camerNewPosInRole = camerByRoleRefer.referDefPosition * scaleCamerParam;

        //先确定人物的坐标系 ->世界坐标系平移矩阵
        //roleMoveRotat 人物移动方向
        Matrix4x4 tmpMat = Matrix4x4.TRS(roleTranform.position, Quaternion.Euler(roleMoveRotat), Vector3.one);

        //计算摄相机在世界坐标系下的坐标
        Vector3 newPos = tmpMat.MultiplyPoint(camerNewPosInRole);

        cameraTransform.position = newPos;

        cameraTransform.LookAt(roleTranform);
        //血条等UI大小修改
        scaleCanvas();
    }

    //摄相机绕着人物旋转， 角度始终lookat人物,暂不使用,后续需要使用时重写
    public void rolationFromRoleZ(float rolation, float pDeltaTime) {


         //构建 参照点坐标系->世界坐标系变换矩阵，涉及平移， 摄像机的世界坐标旋转角度
         Matrix4x4 tmpMat2 = Matrix4x4.TRS(roleTranform.transform.position, Quaternion.Euler(0,cameraTransform.eulerAngles.y, 0), Vector3.one);
         //计算出摄相机 在参照点坐标系中的点
         Vector3 tmpCamer = tmpMat2.inverse.MultiplyPoint(cameraTransform.transform.position);

         //参照点坐标系进行旋转
         tmpMat2.SetTRS(roleTranform.transform.position, Quaternion.Euler(rolation * csRolateZ * pDeltaTime, cameraTransform.eulerAngles.y, 0), Vector3.one);

         //利用 变换矩阵，计算机摄相机在世界坐标中的新点
         Vector3 newPos = tmpMat2.MultiplyPoint(tmpCamer);

         cameraTransform.transform.position = newPos;
         
        //摄相机朝向人物
        cameraTransform.LookAt(roleTranform.transform);

    }


    //计算B点在A坐标系下，饶A点旋转角度后的， 相对坐标是多少
    //input  B在A坐标系下的坐标，  B在A坐标系下旋转的角度
    //返回   B在A坐标系下，旋转后的新相对坐标
    private Vector3 GetRotatePos(Vector3 BPosInA, Vector3 rotate) {
        /*把A想像成世界坐标
         * 计算世界坐标系下旋转rolate角度后的坐标 = 构建矩阵CMat
         * C = Matrix4x4.TRS（原点，旋转角色，Vector3.One）
         *世界坐标系下的点旋转后的 坐标为：CMat.MultiplyPoint(posBInA) 为旋转后B的新坐标
         */
        Matrix4x4 CMat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(rotate), Vector3.one);
        Vector3 newPos = CMat.MultiplyPoint(BPosInA);
        return newPos;
    }


    //计算B点在A点坐标系下的坐标(无视B的移动方向)
    private Vector3 GetBPosInA(Vector3 WorldAPos,  //A的世界坐标
                              Vector3 WorldARotate,  //A的移动方向
                              Vector3 WorldBPos,
                              out Matrix4x4 AToWorldMat) {   //B的世界坐标

        /*构建A->世界坐标系的变换矩阵 AToWorldMat;
        *世界坐标系->的变换矩阵 WorldToAMat;
        *利用WorldToAMat，传入B的世界坐标，得出B在A坐标系下的相对坐标posBInA
        */
        AToWorldMat = Matrix4x4.TRS(WorldAPos, Quaternion.Euler(WorldARotate), Vector3.one);
        Vector3 posBInA = AToWorldMat.inverse.MultiplyPoint(WorldBPos);
        return posBInA;
    }

    //摄相机绕着参照点旋转， 角度始终lookat人物并非参照点
    public void rolationCamerY(float rolation,float pDeltaTime) {
        /*
         1.先计算视角旋转后摄相机的位置
         2.再让摄相机lookat人物
         3.根据旋转的角度update人物移动的参照坐标系
         */

        //1. 先确定参照点的世界坐标， 参照点的X和Z为人物坐标，Y轴的值为摄相机的值
        Vector3 tmpRolatVect = new Vector3(roleTranform.position.x, cameraTransform.position.y, roleTranform.position.z);

        //参照点的移动方向, 按操控的参照点角度
        Vector3 tmpDefauRolat = roleMoveRotat;

        //计算B点在A坐标系下的坐标
        Matrix4x4 AToWorldMat; //A坐标系->世界坐标系变换矩阵
        Vector3 tmpBposInA = GetBPosInA(tmpRolatVect, tmpDefauRolat, cameraTransform.position, out AToWorldMat);

        //计算本次要旋转角度
        Vector3 tmpRolation = new Vector3(0, rolation * csRolateY * pDeltaTime, 0);

        //算出摄相机旋转后，在参照点坐标系下的坐标
        Vector3 newPos = GetRotatePos(tmpBposInA, tmpRolation);

        //将参照点坐标系下的坐标转换为 世界坐标
        cameraTransform.position = AToWorldMat.MultiplyPoint(newPos);

        //2.摄相机朝向人物
        cameraTransform.LookAt(roleTranform);

        //3.人物操控坐标系更新
        updateControlMatFromRoltateY(rolation * csRolateY * pDeltaTime);

    }


    //计算摄像机的世界坐标，根据人物坐标系，以及与人物的参照关系
    private Vector3 getCamerNewPosFromRoleRefer() {

        //计算摄相机在人物坐标系下的坐标
        Vector3 camerPosInRole = camerByRoleRefer.referDefPosition * camerByRoleRefer.scaleOldCamerParam;

        //人物坐标系与世界坐标系变换矩阵
        Matrix4x4 roleMat = Matrix4x4.TRS(roleTranform.position, Quaternion.Euler(roleMoveRotat), Vector3.one); //不能受角色朝向影响
        
        //计算摄像机的世界坐标(根据摄像机在人物坐标系下的 默认坐标)
        Vector3 newPos = roleMat.MultiplyPoint(camerPosInRole);

        return newPos;
    }

    //复原摄像机，让摄像机矩离人背后一段矩离角度
    public void resetCamerPosFromRole() {

        //人物移动朝向修改
        initControlCoordinateSystem();

        //摄像机缩放比例还原
        scaleCamerParam = 1.0f;
        camerByRoleRefer.scaleOldCamerParam = scaleCamerParam;

        //摄像机的世界坐标
        cameraTransform.position = getCamerNewPosFromRoleRefer();

        //摄像机朝向人物
        cameraTransform.LookAt(roleTranform);
        
        //UI血条的缩放
        scaleCanvas();
    }

   
    //摄相机上下左右移动，用矩阵换算的方式，基于当前镜头方向
    public void moveCamerWSADWorldPosFromCamerControlMat(float leftright, float downup, float pDeltaTime) {
        //WS 对应Z轴正负，    AD对应X轴 负正
        Vector3 tmpPos = new Vector3(leftright * csMoveCamerVSpeed * pDeltaTime / 30, 0, downup * csMoveCameHSpeed * pDeltaTime / 30);

        //人物旋转Y轴角度自由，其它方向不能超过正负180度产生翻转
        //构建以平行于世界坐标的坐标系矩阵, 此坐标系原点为摄像机坐标，x与z轴与世界坐标系相同，只有y轴角度按摄像机计算，
        Matrix4x4 tmpCamerMat = Matrix4x4.TRS(cameraTransform.transform.position, Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0),Vector3.one);

        Vector3 newPos = tmpCamerMat.MultiplyPoint(tmpPos);
        cameraTransform.transform.position = newPos;
    }

    //摄相机上下左右移动，用矩阵换算的方式,将不再使用
    public void moveCamerWSADWorldPosFromControlMat(float leftright, float downup)
    {
        //WS 对应Z轴正负，    AD对应X轴 负正
        Vector3 tmpPos = new Vector3(leftright * moveVSpeed / 30, 0, downup * moveVSpeed / 30);

        Vector3 newMatPos = controlMat.MultiplyPoint(tmpPos);
        Vector3 pMove = newMatPos - new Vector3(0, 0, 0);

        Vector3 nowPos = cameraTransform.transform.position;
        Vector3 newPos = new Vector3(nowPos.x + pMove.x, nowPos.y + pMove.y, nowPos.z + pMove.z);
        cameraTransform.transform.position = newPos;
        
    }

    //人物移动后坐标计算，用矩阵换算的方式
    public void updateRoleWorldPosFromControlMat(float leftright, float downup, float pDelayTime, float offsetY) {
        //世界坐标系，人物坐标系，人物设置坐标要用世界坐标系下
        //人物在人物坐标系上, 人物与人物坐标系，只是偏移上的差异，没有旋转角度

        //人物坐标系，不以人物当前模型位置及旋转为参照，以镜头复原时的人物坐标及角度为参照
        //WS 对应Z轴正负，    AD对应X轴 负正
        //计算人物坐标系下的坐标偏移量(不是以人物当前位置来参照的)
        //control 只有旋转视角时才发生更新，因此计算的坐标都只是移动的世界坐标
        Vector3 tmpPos = new Vector3(leftright * moveVSpeed * pDelayTime, offsetY, downup * moveVSpeed * pDelayTime);

        //人物坐标系下移动后的坐标，转为世界坐标系中的坐标
        Vector3 newMatPos = controlMat.MultiplyPoint(tmpPos);

        //人物坐标系中原点在世界坐标系中的坐标
        Vector3 posZero = controlMat.MultiplyPoint(new Vector3(0,0,0));

        //计算世界坐标系下的，人物移动的坐标偏移值，
        Vector3 pMove = newMatPos - posZero;

        //人物的当前坐标 加上 偏移值，为移动后的坐标
        roleTranform.position = roleTranform.position + pMove;


        //摄像机跟据和人物的缩放比例,人物的移动方向，重新计算位置, lookat等暂不改
        cameraTransform.position = getCamerNewPosFromRoleRefer();
    }
/*
    //人物坐标计算，用直接用xyz换算,目前不使用
    public void updateRoleWorldPosFromXYZCal(float leftright, float downup) {
       
        //基于世界坐标系，世界左手坐标系的人物移动，此游戏人物移动按世界X轴正前方，  y轴为高， 一般为0, W/S 对应X轴 正/负， A/D对 Z轴 负/正与世界Z轴方向相反，与人物模型的缩放无关，受旋转影响 x轴要*-1
        Vector3 pMove = new Vector3(downup * moveVSpeed / 30, 0, leftright * -1 * moveVSpeed / 30);

        //Vector3 nowPos = roleTranform.transform.position;
        Vector3 nowPos = new Vector3(roleTranform.transform.position.x, 0, roleTranform.transform.position.z);

        Vector3 newPos = new Vector3(nowPos.x + pMove.x, 0, nowPos.z + pMove.z);
        roleTranform.transform.position = newPos;
        // string msg = "roleTranform.transform.position.x: " + roleTranform.transform.position.x.ToString();
       // msg += "   To " + roleTranform.transform.position.x.ToString();
       // Debug.Log(msg);
    }
    */
    public float getRolePosYWorld() {
        return roleTranform.position.y;
    }


    //基于世界坐标的 人物移动及朝向修改
    public void updateRolePosWorld(float leftright, float downup,float pDelayTime, float offsetY) {
        //先修改坐标
        // updateRoleWorldPosFromXYZCal(leftright, downup);
       // offsetY = 0.0f;
        updateRoleWorldPosFromControlMat(leftright, downup, pDelayTime, offsetY);

        if ((leftright != 0.0f) || (downup != 0.0f)) //朝向未发生改变
        {
            //再修改朝向
            Vector2 tmpPos2D = new Vector2();
            tmpPos2D.x = downup;
            tmpPos2D.y = leftright;
            
            //float tmpRotat = roleRotatY;
            float rotal = calRoleRotation(tmpPos2D, controlMat.rotation.eulerAngles.y); //获得角色绕Y轴旋转朝向， 
            Vector3 tmpEulerAngles = new Vector3(roleTranform.transform.eulerAngles.x, rotal, roleTranform.transform.eulerAngles.z);
            roleTranform.eulerAngles = tmpEulerAngles;
        }
        
    }

    /*
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
    */
    
    //以下计算人物模型的朝向//
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
