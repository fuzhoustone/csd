using UnityEngine;
using System.Collections;

public class testanimation : MonoBehaviour
{

    // 摄像机位置
    public Transform cameraTransform;
    // 摄像机距离人物的距离
    public float cameraDistance;

    // 摄像机距离人物最大的距离
    public float cameraDistanceMax;

    // 摄像机距离人物最小的距离
    public float cameraDistanceMin;

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
    public float maxVSpeed = 8.0f;
    // 最小速度
    public float minVSpeed = 5.0f;

    //鼠标点击时，移动的速度
    public float clickMoveSpeed = 0.1f;

    private Camera mainCamera = null;

    private Vector3 newPos;
    private bool isMouseMove = true;

    private Animation m_animation;
    //  public Animation animationController = null;
    void Start()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        m_animation = GetComponent<Animation>();
    }

    void Update()
    {
        // 游戏结束
        if (gameManager.isEnd)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float tmpv = Input.GetAxis("Vertical");
        bool isKeyDownMove = true;
        if ((h == 0.0f) && (tmpv == 0.0f))
        {
            isKeyDownMove = false;
         //   m_animation.PlayQueued("Stand");
            //  m_animator.SetBool("Stand", true);
        }
        else
        {
            m_animation.PlayQueued("run");
            isMouseMove = false;
        }


    }
 }