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

    private Camera mainCamera;
    private Canvas mainCanvas;

    public float xOffset;
    public float yOffset;

    private GameObject hpPrefab = null;

    private RectTransform hpUI;

    private GameObject hpObj = null;

    private bool isShowUI = false;

    [SerializeField]
    private UnityEngine.UI.Slider roleSlider = null;

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

        hpObj = Instantiate(hpPrefab, transform.position, Quaternion.identity, mainCanvas.transform);

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



    private void refreshHpSilder() {
        Vector3 offsetV3 = new Vector3(xOffset * transform.localScale.x, yOffset * transform.localScale.y, 0.0f);
        //Vector3 tmpWorldPos = transform.position + offsetV3;
        //Vector3 UIWorldPos = new Vector3(tmpWorldPos.x * transform.localScale.x,
        //                                 tmpWorldPos.y * transform.localScale.y,
        //                                 tmpWorldPos.z * transform.localScale.z);

        Vector2 player2DPosition = mainCamera.WorldToScreenPoint(transform.position  + offsetV3);
        //Vector2 player2DPosition = mainCamera.WorldToScreenPoint(UIWorldPos);
        //recTransform.position = player2DPosition + new Vector2(xOffset, yOffset);
        hpUI.position = player2DPosition;

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
