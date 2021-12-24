using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopInfoUI : MonoBehaviour
{
    public Image pIcon;
    //public Text pName;
    //public Text pDescription;
    //  public Button pBtn;
    public Image pTxtImage;
    public Text pBtnText;
    public Image pBackImage;
    public Action<int, shopInfoUI> btnCallBack;

   // private int cost = 0;
    public int roleID = 0;
    private const string csPicPath = "Textures/bosspic/";
    //private const string csFinish = "已解锁";
    //public void initData(string iconName, string name, string desc, int cost) {
    public void initData(string iconName, int pCost,int pRoleID, Action<int, shopInfoUI> pEvent)
    {
        roleID = pRoleID;
        btnCallBack = pEvent;
        string fileName = csPicPath + iconName;
        Texture2D texture1 = (Texture2D)Resources.Load(csPicPath + iconName) as Texture2D;
        pIcon.sprite = Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));

        //pName.text = name;
        //pDescription.text = desc;

      //  cost = pCost;
        
        if (pCost > 0) //未解锁
        {
            pBtnText.text = pCost.ToString();
            pTxtImage.gameObject.SetActive(true);
          //  pBtn.enabled = true;
          //  pBtn.gameObject.SetActive(true);
        }
        else {  //已解锁
            pTxtImage.gameObject.SetActive(false);
            //  pBtn.enabled = false;
            //  pBtn.gameObject.SetActive(false);
        }
        
    }

    public void setSelectActive(bool isShow) {
        pBackImage.gameObject.SetActive(isShow);
    }

    public void setBuyOpen() {
        pTxtImage.gameObject.SetActive(false);
    }

    public void btnEvent() {
        btnCallBack(roleID, this);
    }
}
