using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvaterAnimationController : MonoBehaviour
{
    // 摄像机位置
    public Transform cameraTransform;
    // 摄像机距离人物的距离
    public float cameraDistance = 10.0f;

    // 摄像机距离人物最大的距离
    public float cameraDistanceMax = 15.0f;

    // 摄像机距离人物最小的距离
    public float cameraDistanceMin = 10.0f;

    // 游戏管理器
    public GameManager gameManager;
    // 前进移动速度
    float moveVSpeed;

    private Vector3 moveDirection = Vector3.zero;
    // 水平移动速度
    public float moveHSpeed = 5.0f;
    // 跳跃高度
    public float jumpHeight = 5.0f;
    // 动画播放器
    Animator m_animator;
   
    // 起跳时间
    double m_jumpBeginTime;
    // 跳跃标志
    int m_jumpState = 0;
    // 最大速度
    public float maxVSpeed = 10.0f;
    // 最小速度
    public float minVSpeed = 5.0f;

    //鼠标点击时，移动的速度
    public float clickMoveSpeed = 0.1f;

    private Camera mainCamera = null;

    private Vector3 newPos;
    private bool isMouseMove = true;

    private Animation m_animationController = null;
    private CharacterController roleControl = null;
    int state = 0;
    int stateStand = 0;
    int stateRun = 1;
    int stateAttack = 2;
    //public bool isInAttack = false;
    public attcakStartEnd pAttackClass = null;

    private enum roleState {
        init = -1,
        stand = 0,
        run = 1,
        attack = 2
    }

    public bool isInit = false;
    // Use this for initialization
    void Start()
    {
       // isInAttack = false;
        GetComponent<Rigidbody>().freezeRotation = true;
        m_animationController = GetComponent<Animation>();
        roleControl = GetComponent<CharacterController>();
       

        moveVSpeed = minVSpeed;
        

        m_jumpState = 0;
        isMouseMove = false;
        moveDirection = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
      //  state = stateStand;
        //        isInit = false;
    }

    public void setCamera(Transform pCameraTransform, GameManager pGameManager) {
        cameraTransform = pCameraTransform;
        mainCamera = cameraTransform.GetComponent<Camera>();
        gameManager = pGameManager;
        isInit = true;
    }

    private bool isSameFloat(float a, float b)
    {
        bool res = false;
        float cal = a - b;
        if ((cal < 0.01f) &&
             (cal > -0.01f)
           )
            res = true;

        return res;
    }

    private bool isMoveMouseDest()
    {
        bool res = false;
        if (isMouseMove)
        {
            if (isSameFloat(this.transform.position.x, newPos.x) && isSameFloat(this.transform.position.z, newPos.z))
            {
                isMouseMove = false;
                res = false;
                // m_animator.SetBool("Stand", true);
                m_animationController.Play("breath");
                Debug.LogWarning("move finish x:" + this.transform.position.x.ToString() + " z:" + this.transform.position.z.ToString());
            }
            else
            {
                res = true;
            }
        }
        return res;
    }

    private void checkNewMousePos()
    {
        if (Input.GetMouseButtonDown(0))
        {  //创建一个射线，该射线从主摄像机中发出，而发出点是鼠标点击的位置 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null)
                {
                    print("hit:" + hit.collider.name);
                    GameObject m_currSelectObj = hit.collider.gameObject;
                    isMouseMove = true;
                    m_animationController.Play("run");
                    // m_animator.SetBool("Stand", false);
                    newPos = new Vector3(hit.point.x, 0, hit.point.z);
                    Debug.LogWarning("start move newPos.x=" + newPos.x.ToString() + " newPos.z=" + newPos.z.ToString());
                    // Instantiate(cube.transform, newPos, cube.transform.rotation);
                }
            }
        }
    }

    public void printRoleState(int pRoleState = -1) {
        roleState pState = (roleState)(pRoleState);
        if (pState == roleState.init)
        {
            //获取当前值打log
            pState = getRoleNowState();
        }
        else {
            //使用传入的值打log
        }

        Debug.LogWarning("roleState is:" + pState.ToString());
    }

    private void changeRoleState(roleState pState) {
        switch (pState) {
            case roleState.init: {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("breath");
                    Debug.Log("change state to stand");
                }
                break;
            case roleState.stand: {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("breath");
                    Debug.Log("change state to stand");
                }
               
                break;
            case roleState.run: {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("run");
                    Debug.Log("change state to run");
                }
                break;
            case roleState.attack: {
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

                    Debug.Log("change state to default");
                }
                break;


        }
    }

    private roleState getHopeState(float h, float tmpv, bool isfire) {
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

    private roleState getRoleNowState() { //获得角色当前状态
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
                    Debug.LogWarning(" roleState is attack4");
                }
                else
                {
                    res = roleState.init;
                    Debug.LogWarning("need set roleState init");
                }
            }
            else {
                res = roleState.init;
                Debug.LogWarning("need set roleState default init");
            }
            
        }    
        
        return res;
    }

    private void updateRolePos(float h, float tmpv) {
        //人物移动
        moveDirection = new Vector3(h, 0, tmpv); //Allows for player input
        moveDirection = transform.TransformDirection(moveDirection); //How to move
        moveDirection *= moveVSpeed; //How fast to move
                                     //  }

        moveDirection.y -= 0 * Time.deltaTime;
        //Move the controller
        roleControl.Move(moveDirection * Time.deltaTime);
    }

    private bool updataRoleControl(float h,float tmpv, bool isfire) {
        

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
       roleState lState = getRoleNowState();

       roleState lHopeState = getHopeState(h,tmpv, isfire);

        if (lState != lHopeState) {
            changeRoleState(lHopeState);
        }
        /*
        //动作变更
        if (isInAttack() == false) { //不在攻击状态中
            if ((h == 0.0f) && (tmpv == 0.0f))
            {
                // if (m_animationController.isPlaying) {

                // }

                if (state == stateRun) //站立
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("breath");
                    state = stateStand;
                }
            }
            else
            {
                if (state == stateStand)  //跑步
                {
                    m_animationController.wrapMode = WrapMode.Loop;
                    m_animationController.Play("run");
                    state = stateRun;
                }
              //  isMouseMove = false;
            }
        }

        // moveChartToDestPos(trans, moveDirection);
        //  if (Input.GetButton("Jump"))
        //  {
        //      moveDirection.y = jumpHeight;
        //  }

        //Apply gravity
        */

        return true;
    }
    /*
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject != null) {
         //   Debug.LogWarning("hitCollider: "+hit.gameObject.name);
        }
     //   PushCharacterIsOverlap(hit.collider);
    }
   
    private bool isInAttack() {
       //return true;
        if(m_animationController == null)
            return false;

        if (
            m_animationController.IsPlaying("attack1") ||
            m_animationController.IsPlaying("attack2") ||
            m_animationController.IsPlaying("attack3") ||
            m_animationController.IsPlaying("attack4")
            )
            return true;
        else
            return false;
    }
     */
    void Update()
    {
        if (isInit == false)
        {
            return;
        }

        // 游戏结束
        if (gameManager.isEnd)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float tmpv = Input.GetAxis("Vertical");
        bool isfire = Input.GetButton("Fire1");
        //当前非攻击状态，则转为攻击状态

        updateRolePos(h, tmpv); //改变角色位置

        bool isUpdata = updataRoleControl(h,tmpv, isfire); //改变角色动作
       //if (isUpdata)
       //     return;

        bool isKeyDownMove = true;
        if ((h == 0.0f) && (tmpv == 0.0f))
        {
            isKeyDownMove = false;
        }
        else
        {

            isMouseMove = false;
        }

       


        if (isKeyDownMove) //有新的按键移动，取消目标点移动
        {
            // m_animator.SetBool("Stand", false);
           // if(isInAttack() == false)
          //      m_animationController.Play("run");
            updateCamerChartPos(this.transform, h, tmpv);  //摄像机跟随
        }

        else if(false)
        { //按原目标点移动
            //updateCamerChartPos(this.transform, h, tmpv);
            //m_animationController.Play("breath");
            //return;

            checkNewMousePos(); //检测新的移动点
            if (isMoveMouseDest()) //是否移向目标点
            { //检测是否移到目的点
                Transform trans = this.transform;
                Vector3 destPos = new Vector3(trans.position.x, trans.position.y, trans.position.z);

                if (newPos.x > trans.position.x)
                {
                    tmpv = clickMoveSpeed * 1;
                    destPos.x = destPos.x + tmpv;
                    if (destPos.x > newPos.x)
                    {
                        Debug.Log("destPos.x > newPos.x  destPos.x=" + destPos.x.ToString() + " newPos.x=" + newPos.x);
                        destPos.x = newPos.x;

                    }
                }
                else if (newPos.x < trans.position.x)
                {
                    tmpv = clickMoveSpeed * -1;
                    destPos.x = destPos.x + tmpv;
                    if (destPos.x < newPos.x)
                    {
                        Debug.Log("destPos.x < newPos.x destPos.x=" + destPos.x.ToString() + " newPos.x=" + newPos.x);
                        destPos.x = newPos.x;

                    }
                }
                else
                {

                }


                if (newPos.z < trans.position.z)
                {
                    h = clickMoveSpeed * -1;
                    destPos.z = destPos.z + h;
                    if (destPos.z < newPos.z)
                    {
                        Debug.Log("destPos.z > newPos.z destPos.z=" + destPos.z.ToString() + " newPos.z=" + newPos.z);
                        destPos.z = newPos.z;

                    }
                }
                else if (newPos.z > trans.position.z)
                {
                    h = clickMoveSpeed * 1;
                    destPos.z = destPos.z + h;
                    if (destPos.z > newPos.z)
                    {
                        Debug.Log("destPos.z < newPos.z destPos.z=" + destPos.z.ToString() + " newPos.z=" + newPos.z);
                        destPos.z = newPos.z;

                    }
                }
                else
                {
                    h = 0;
                }

                moveChartToDestPos(trans, destPos);

            }
            else
            { //不移动
                m_animationController.Play("breath");
                //   m_animator.SetBool("Stand", true);
            }
            
        }






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
    private void updateCamerChartPos(Transform trans, float h, float tmpv)
    {
        
        Vector3 vSpeed = new Vector3(trans.forward.x, trans.forward.y, this.transform.forward.z) * moveVSpeed * tmpv;


        Vector3 hSpeed = new Vector3(trans.right.x, trans.right.y, trans.right.z) * moveHSpeed * h;

        Vector3 jumpSpeed = new Vector3(trans.up.x, trans.up.y, trans.up.z) * jumpHeight * m_jumpState;


        Vector3 vCameraSpeed = new Vector3(trans.forward.x, trans.forward.y, trans.forward.z) * minVSpeed;
        Vector3 destPos = trans.position + (vSpeed + hSpeed + jumpSpeed) * Time.deltaTime;

        moveChartToDestPos(trans, destPos);
       
    }

    private void updateCamerPos(Transform trans, float h, float tmpv)
    {
        Vector3 vSpeed = new Vector3(trans.forward.x, trans.forward.y, trans.forward.z) * moveVSpeed;
        //Vector3 vSpeed = new Vector3(trans.forward.x, trans.forward.y, this.transform.forward.z) * moveVSpeed * tmpv;
        Vector3 hSpeed = new Vector3(trans.right.x, trans.right.y, trans.right.z) * moveHSpeed * h;
        Vector3 jumpSpeed = new Vector3(trans.up.x, trans.up.y, trans.up.z) * jumpHeight * m_jumpState;


        Vector3 vCameraSpeed = new Vector3(trans.forward.x, trans.forward.y, trans.forward.z) * minVSpeed;
        trans.position += (vSpeed + hSpeed + jumpSpeed) * Time.deltaTime;
        cameraTransform.position += (vCameraSpeed) * Time.deltaTime;
        if (trans.position.x - cameraTransform.position.x < cameraDistance)
        {
            moveVSpeed += 0.1f;
            if (moveVSpeed > maxVSpeed)
            {
                moveVSpeed = maxVSpeed;
            }
        }
        // 超过时 让摄像机赶上
        else if (trans.position.x - cameraTransform.position.x > cameraDistance)
        {
            moveVSpeed = minVSpeed;
            cameraTransform.position = new Vector3(trans.position.x - cameraDistance, cameraTransform.position.y, cameraTransform.position.z);
        }
        // 摄像机超过人物
        if (cameraTransform.position.x - trans.position.x > 0.0001f)
        {
            Debug.Log("你输啦！！！！！！！！！！");
            gameManager.isEnd = true;
        }
    }

    /*
    void OnGUI()
    {
        if (gameManager.isEnd)
        {
            GUIStyle style = new GUIStyle();

            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 40;
            style.normal.textColor = Color.red;
            GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "你输了~", style);

        }
    }
   
    void OnTriggerEnter(Collider other)
    {
        // 如果是抵达点
        if (other.name.Equals("ArrivePos"))
        {
            gameManager.changeRoad(other.transform);
        }
        // 如果是透明墙
        else if (other.tag.Equals("AlphaWall"))
        {
            // 没啥事情
        }
        // 如果是障碍物
        else if (other.tag.Equals("Obstacle"))
        {

        }
    }

     */
}
