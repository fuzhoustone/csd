using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class roleListUI : MonoBehaviour
{
    public class roleInfo {
        public int roleID { get; set; }
        public string picName { get; set; }
        public int hp { get; set; }

        public int maxHp { get; set; }

        public roleInfoUI infoUI { get; set; }
    }

    public Transform viewPortLst;
    public Button btnOK;

    public Action<int> calBackEvent;
    //public Transform scrollViewParent;
    // public TextAsset bossTab;
     private const string csRoleInfo = "roleInfoUI";
     private const string csPicPath = "Textures/bosspic/";
    private int oldRoleIndex = 0;
    private List<roleInfo> uiRole;

    private GameObject setInfo(roleInfo pInfo,int orderUIID ) {
        //加载图片
        string fileName = csPicPath + pInfo.picName;
        Texture2D texture1 = (Texture2D)Resources.Load(csPicPath + pInfo.picName) as Texture2D;

        //创建UI
        UnityEngine.Object infoObj = Resources.Load("Prefab/UI/" + csRoleInfo);
        GameObject tmpObj = GameObject.Instantiate(infoObj, viewPortLst) as GameObject;
        //Image tmpImage = tmpObj.GetComponent<Image>();
        //tmpImage.sprite = Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));

        //hp显示及背景默认隐藏
        roleInfoUI tmpInfoUI = tmpObj.GetComponent<roleInfoUI>();
        tmpInfoUI.hpSlider.value = pInfo.hp * 100.0f / pInfo.maxHp;
        tmpInfoUI.backImage.gameObject.SetActive(false);
        tmpInfoUI.picIcon.sprite = Sprite.Create(texture1, new Rect(0, 0, texture1.width, texture1.height), new Vector2(0.5f, 0.5f));
        tmpInfoUI.roleID = pInfo.roleID;
        pInfo.infoUI = tmpInfoUI;
       // uiRole.Add(pInfo);
        //int order = uiRole.Count - 1;
        tmpInfoUI.orderUIID = orderUIID;

        //点击事件绑定
        Button tmpBtn = tmpObj.GetComponent<Button>();
        tmpBtn.onClick.AddListener(delegate ()
        {
            this.OnClick(orderUIID);
        });
        return tmpObj;
    }

    //设置选中
    private void setSel(int orderUIID, bool isSel) {
       roleInfoUI tmpInfo = uiRole[orderUIID].infoUI;
       tmpInfo.backImage.gameObject.SetActive(isSel);
    }

    //设置点击
    public void OnClick(int tmpID) {
        if (tmpID != oldRoleIndex) {
            setSel(oldRoleIndex, false);
            setSel(tmpID, true);
            oldRoleIndex = tmpID;
        }
    }

    //确定选择的怪物
    public void OKBtnClick() {
        if (calBackEvent != null) {
            roleInfoUI tmpInfo = uiRole[oldRoleIndex].infoUI;
            calBackEvent(tmpInfo.roleID);
        }

        UIclose();
    }

    private void initData() {
        uiRole = new List<roleInfo>();
        int nCount = 0;
        gameDataMgr.bossTag tmpTag = gameDataMgr.gameData().m_bossTag;
        for (int i = 0; i < tmpTag.bossUse.Length; i++) {
            if (tmpTag.bossUse[i] == true) {
                roleInfo tmpInfo = new roleInfo();
                
                tmpInfo.hp = gameDataMgr.gameData().m_roleData.bosshp[i];

                RoleProTable.rolePro tmpPro = RoleProTable.GetFromRoleID(i);
                tmpInfo.maxHp = tmpPro.MaxHp;
                tmpInfo.roleID = tmpPro.ID;

                ShopItemTable.shopElements tmpEle = ShopItemTable.Get(i);
                //RoleInfoTable.roleElements tmpEle = RoleInfoTable.Get(i+1);
                tmpInfo.picName = tmpEle.Pic;

                uiRole.Add(tmpInfo);
                nCount++;
            }
        }

        //OKBtnClick
        btnOK.onClick.AddListener(delegate ()
        {
            this.OKBtnClick();
        });
    }

    //加载显示UI
    public void showUI(int roleID, Action<int> pEvent)
    {
        calBackEvent = pEvent;

        initData();
        int nCount = uiRole.Count;
        //int nCount = pData.mRoleData.Count;
        int nRowMax = 3;
        int nLine = Mathf.CeilToInt((float)nCount/(float)nRowMax);

        float csPosX = 160.0f;
        float csPosY = 124.0f;
        float allHeight = csPosY * nLine;
        RectTransform tmpTrans = viewPortLst.GetComponent<RectTransform>();
        tmpTrans.sizeDelta = new Vector2(csPosX * 3, allHeight);

        for (int i = 1; i <= nCount; i++)
        {
            int orderUIID = i - 1;
            roleInfo tmpPro= uiRole[orderUIID];
            //roleProperty tmpPro = pData.mRoleData[i - 1];
            if (tmpPro.roleID == roleID) { //设置默认选中的UIID
                oldRoleIndex = orderUIID;
            }
            //设置信息
            GameObject tmpObj = setInfo(tmpPro, orderUIID);

            //调整位置
            RectTransform tmp = tmpObj.GetComponent<RectTransform>();
            int orderY = orderUIID / nRowMax; 
            int orderX = orderUIID - nRowMax * orderY;
            float tmpPosX = csPosX * orderX;
            float tmpPosY = csPosY * orderY;

            tmp.anchoredPosition3D = new Vector3(tmpPosX, tmpPosY, 0);
        }
        setSel(oldRoleIndex, true);
        this.gameObject.SetActive(true);
    }

    public void UIclose()
    {
        //GameObjDataTemp.tempData().refreshHpUI = true;
        CsdUIControlMgr.uiMgr().uiMenu.canvasHP.gameObject.SetActive(true);

        for (int i = 0; i < viewPortLst.childCount; i++)
        {
            Transform pTran = viewPortLst.GetChild(i);
            GameObject.Destroy(pTran.gameObject);
        }

        uiRole.Clear();
        btnOK.onClick.RemoveAllListeners();

        gameObject.SetActive(false);
    }
}
