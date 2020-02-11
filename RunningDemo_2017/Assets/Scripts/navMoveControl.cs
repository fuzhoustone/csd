using UnityEngine;
using UnityEngine.AI;
//using System.Collections;

public class navMoveControl : MonoBehaviour {

    public LayerMask whatIsGround;

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

    public NavMeshAgent navAgent = null;
    //  public Animation animationController = null;
    // Use this for initialization
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        m_animator = GetComponent<Animator>();
        if (m_animator == null)
            print("null");
        moveVSpeed = minVSpeed;
        mainCamera = cameraTransform.GetComponent<Camera>();
        //  m_animator.SetBool("Jump", false);
        //  m_animator.SetBool("Stand", true);


        isMouseMove = false;
        navAgent = GetComponent<NavMeshAgent>();
        //newPos = this.transform.position;
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
            if (Physics.Raycast(ray, out hit, 1000,whatIsGround))
            {
                //if (navAgent != null)
                //    navAgent.SetDestination(hit.point);
                if (hit.collider != null)
                {
                    print("hit:" + hit.collider.name);
                    GameObject m_currSelectObj = hit.collider.gameObject;
                    isMouseMove = true;
                    // m_animator.SetBool("Stand", false);
                    //newPos = m_currSelectObj.transform.position;
                    newPos = new Vector3(hit.point.x, 0, hit.point.z);
                    if (navAgent != null)
                        navAgent.SetDestination(newPos);
                        //   navAgent.SetDestination(hit.point);
                        Debug.LogWarning("start move newPos.x=" + newPos.x.ToString() + " newPos.z=" + newPos.z.ToString());
                    // Instantiate(cube.transform, newPos, cube.transform.rotation);
                }
            }
        }
    }

    private bool IsInEntry(string entryName)
    {
        AnimatorStateInfo stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.fullPathHash == Animator.StringToHash(entryName))
        {
            return true;
        }
        else
            return false;
    }

    // Update is called once per frame

    void Update()
    {
        // 游戏结束
        if (gameManager.isEnd)
        {
            return;
        }

        if (IsInEntry("Base Layer.jump") == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // 起跳
                Debug.LogWarning("set jump");
                m_animator.SetBool("jump", true);
                // m_jumpBeginTime = m_animator.GetTime();
            }
        }

        if (IsInEntry("Base Layer.jump"))
        {
            m_jumpState = 1;
        }
        else
            m_jumpState = 0;

        float h = Input.GetAxis("Horizontal");
        float tmpv = Input.GetAxis("Vertical");
        bool isKeyDownMove = true;
        if ((h == 0.0f) && (tmpv == 0.0f))
        {
            isKeyDownMove = false;
            //  m_animator.SetBool("Stand", true);
        }
        else
        {

            isMouseMove = false;
        }

        if (isKeyDownMove) //有新的按键移动，取消目标点移动
        {
          //  m_animator.SetBool("Stand", false);
            updateCamerChartPos(this.transform, h, tmpv);
        }
        else
        { //按原目标点移动
            
            checkNewMousePos(); //检测新的移动点
            if (isMoveMouseDest()) //是否移向目标点
            { //检测是否移到目的点
                Transform trans = this.transform;

                //moveChartToDestPos(trans, destPos);
                updateCameraPos(trans);
                                

            }
            else
            { //不移动
               // m_animator.SetBool("Stand", true);
            }
            
        }


        //  if (IsInEntry("Base Layer.Stand")) {
        if (Input.GetKey(KeyCode.J))
        {
            m_animator.SetBool("Attack", true);
        }
        //  }


    }

    //更改摄像机位置，使跟随人物
    private void updateCameraPos(Transform trans) {
        /*
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
        */
    }

    private void moveChartToDestPos(Transform trans, Vector3 destPos, bool isMoveCamer = true)
    {
        trans.position = destPos;

        if (isMoveCamer) //更改摄像机位置，使跟随人物
        {
            updateCameraPos(trans);
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


}
