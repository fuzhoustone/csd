using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopInfoUI : MonoBehaviour
{
    public Image pIcon;
    public Text pName;
    public Text pDescription;
    public Button pBtn;
    public Text pBtnText;
    public Action btnCallBack;

    private int pCost = 0;
    private const string csPicPath = "Textures/bosspic/";
    //private const string csFinish = "已解锁";
    public void initData(string iconName, string name, string desc, int cost) {
        string fileName = csPicPath + iconName;
        Texture2D texture1 = (Texture2D)Resources.Load(csPicPath + iconName) as Texture2D;
        pIcon.sprite = Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));

        pName.text = name;
        pDescription.text = desc;

        pCost = cost;
        
        if (pCost > 0) //未解锁
        {
            pBtnText.text = pCost.ToString();
            pBtn.enabled = true;
            pBtn.gameObject.SetActive(true);
        }
        else {  //已解锁
            pBtn.enabled = false;
            pBtn.gameObject.SetActive(false);
        }

    }

    public void btnEvent() {
        if (pCost > 0) {
            
        }
        
    }
}
