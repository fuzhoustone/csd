using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleProperty:MonoBehaviour
{
    [SerializeField]
    public int hpMax;
    [SerializeField]
    public int mpMax;
    [SerializeField]
    public int hp;
    [SerializeField]
    public int mp;

    [SerializeField]
    public int attack;
    [SerializeField]
    public int level;
    [SerializeField]
    public float speed;

    public float turnTime;
    public float nowTurnTime;
    public bool isTurn;

    private Camera mainCamera;
    private Canvas mainCanvas;

    [SerializeField]
    public float xOffset;

    [SerializeField]
    public float yOffset;

    [SerializeField]
    public Vector3 uiPosition;

    private GameObject hpPrefab = null;

    private RectTransform hpUI;

    private GameObject hpObj = null;

    private bool isShowUI = false;

    [SerializeField]
    private UnityEngine.UI.Slider roleSlider = null;

    [SerializeField]
    private GameObject HpUIPoint = null;

    private const string csHpUI = "Prefabs/hpSlider";

    public void InitData(Transform pCamerTransform, Transform pCanvasTransform) {
        //hpMax = 100;
        hp = hpMax;
       // mpMax = 100;
        mp = mpMax;
       // attack = 20;
       // level = 1;
       // speed = 0.5f;

        mainCamera = pCamerTransform.GetComponent<Camera>();
        mainCanvas = pCanvasTransform.GetComponent<Canvas>();

        createHpUI();
        
    }

    void Update()
    {

        if (isShowUI)
            refreshHpSilder();
    }

    public void createHpUI()
    {
        hpPrefab = (GameObject)Resources.Load(csHpUI);

        uiPosition = transform.position;

        hpObj = Instantiate(hpPrefab, uiPosition, Quaternion.identity, mainCanvas.transform);

        hpUI = hpObj.GetComponent<RectTransform>();

        roleSlider = hpObj.GetComponent<UnityEngine.UI.Slider>();

        updateHpValue(hp);

        hpObj.SetActive(false);
        isShowUI = false;

    }

    //扣血
    public void SubHpValue(int value) {
        hp = hp - value;
        if (hp < 0)
            hp = 0;

        updateHpValue(hp);
    }
    
    

    public void updateHpValue(int value) {
        hp = value;
        if (hp <= hpMax)
            roleSlider.value = (float)hp * 100.0f / (float)hpMax ;
        else
            roleSlider.value = 100.0f;
    }

    public void showUI()
    {
        hpUI.gameObject.SetActive(true);
        isShowUI = true;
        refreshHpSilder();
    }

    public void hideUI()
    {
        hpUI.gameObject.SetActive(false);
        isShowUI = false;
    }


    //世界坐标系下的偏移坐标
    private Vector3 calOffSetWorld(Vector3 uiVector3) {
        Vector3 res = new Vector3(0, 0, 0);
        Matrix4x4 uiMat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0,0,0), Vector3.one); 

        return res;
    }
    /*
     人物
    9，0.264995，16
    0，90，0
    0.1，0.1，0.1

        摄像机
        8,1,16
        45,90,0
        1,1,1

*/
    
        /*
         1. 构建人物为原点的坐标系
         2. 假设并调试在人物坐标系下的血条坐标(不是看unity中的transform)
         
         */



        //画点的模型
    public void drawPoint() {
        
    }


    public void setLineOffset(float x, float y) {
        xOffset = x;
        yOffset = y;
    }

   
    //GameObject pathParent;

    //[SerializeField]
    //GameObject pointPrefab;
/*
    public GameObject creatHp3DFlag(Vector3 pos, string pName)
    {
        pathParent = this.gameObject;

        GameObject obj = Instantiate(pointPrefab, pos, Quaternion.identity, pathParent.transform);
        obj.name = pName;

        return obj;
    }
    */
    private void refreshHpSilder() {
        // UI坐标= mainCamera.WorldToScreenPoint(传入人物血条的世界坐标)

        // 人物血条的世界坐标显示UI的绑点 = 人物世界坐标 +  世界坐标系下的偏移坐标
        // 世界坐标系下的偏移坐标 = 以设计期人物为原点的坐标系， UI为具体坐标值, 转换为世界坐标
        if (HpUIPoint != null) {
            Vector3 offsetV3 = HpUIPoint.transform.position;
            Vector2 player2DPosition = mainCamera.WorldToScreenPoint(offsetV3);
            hpUI.position = player2DPosition;
        }
        
        /*
        Vector3 offsetV3 = new Vector3(xOffset * transform.localScale.x, yOffset * transform.localScale.y, 0.0f);
        //Vector3 tmpWorldPos = transform.position + offsetV3;
        //Vector3 UIWorldPos = new Vector3(tmpWorldPos.x * transform.localScale.x,
        //                                 tmpWorldPos.y * transform.localScale.y,
        //                                 tmpWorldPos.z * transform.localScale.z);

        Vector2 player2DPosition = mainCamera.WorldToScreenPoint(transform.position  + offsetV3);
        //Vector2 player2DPosition = mainCamera.WorldToScreenPoint(UIWorldPos);
        //recTransform.position = player2DPosition + new Vector2(xOffset, yOffset);
        hpUI.position = player2DPosition;
        */
        /*
        //血条超出屏幕就不显示
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            hpUI.gameObject.SetActive(false);
        }
        else
        {
            hpUI.gameObject.SetActive(true);
        }
        */
    }
}
