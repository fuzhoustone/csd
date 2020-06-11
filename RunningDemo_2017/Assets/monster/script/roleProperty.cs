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
    public RectTransform recTransform;

    public GameObject hpPrefab = null;

    public GameObject hpObj = null;

    public void testInitData(Transform pCamerTransform, Transform pCanvasTransform) {
        hpMax = 100;
        hp = 50;
        mpMax = 100;
        mp = 100;
        attack = 20;
        level = 1;
        speed = 0.5f;

        mainCamera = pCamerTransform.GetComponent<Camera>();
        mainCanvas = pCanvasTransform.GetComponent<Canvas>();

        createHpUI();
    }

    public void createHpUI()
    {
       // GUI.Slider();
        hpObj = Instantiate(hpPrefab, transform.position, Quaternion.identity, mainCanvas.transform);

        recTransform = hpObj.GetComponent<RectTransform>();

       
    }

    void Update() {
        refreshHpSilder();
    }

    private void refreshHpSilder() {
        Vector3 offsetV3 = new Vector3(0.0f, yOffset, 0.0f);
        Vector2 player2DPosition = mainCamera.WorldToScreenPoint(transform.position + offsetV3);
        //recTransform.position = player2DPosition + new Vector2(xOffset, yOffset);
        recTransform.position = player2DPosition;
        

        //血条超出屏幕就不显示
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            recTransform.gameObject.SetActive(false);
        }
        else
        {
            recTransform.gameObject.SetActive(true);
        }
    }
}
