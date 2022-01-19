using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class shopListUI : MonoBehaviour
{
    public Transform viewPortLst;
    public rolePropertyUI roleProUI;
    public Text rewardTxt;
    //public Transform scrollViewParent;
   // public TextAsset bossTab;
    private const string csShopInfo = "shopinfo";
    //private const string csItemProperUI = "rolePropertyUI";
    private bool isFirstShow = true;
    private shopInfoUI oldSelUI = null;
    private void Start()
    {
        Debug.LogWarning("bossListUI start");
      //  initData();
    }

    public void showUI() {
        if (isFirstShow) {
            isFirstShow = false;
            initData();
        }
        refreshReward();
        this.gameObject.SetActive(true);
    }

    private void refreshReward() {
        rewardTxt.text = gameDataMgr.gameData().m_roleData.rewardNum.ToString();
    }

    //加载显示UI
    public void initData() {
        /*
        using (var stream = new MemoryStream(bossTab.bytes))
        {
            BossInfoTable.Load(stream);
        }
        */
        Object bossInfoObj = Resources.Load("Prefab/UI/" + csShopInfo);

        int nCount = ShopItemTable.GetTableLength();
        float csPosY = -140.0f;
        float allHeight = csPosY * (-1) * nCount;
        RectTransform tmpTrans = viewPortLst.GetComponent<RectTransform>();
        tmpTrans.sizeDelta = new Vector2(tmpTrans.sizeDelta.x, allHeight);

        for (int i = 0; i < nCount; i++) {
            ShopItemTable.shopElements tmpBoss = ShopItemTable.m_elements[i];
            int roleID = tmpBoss.ID;
          //  CSVRow roleEle = RoleInfoTable.GetRowFromID(roleID);
            float posY = csPosY * (i-1);
            GameObject tmpObj = GameObject.Instantiate(bossInfoObj, viewPortLst) as GameObject;

            RectTransform tmp = tmpObj.GetComponent<RectTransform>();
            tmp.anchoredPosition3D = new Vector3(0, posY, 0);

            shopInfoUI tmpUI = tmpObj.GetComponent<shopInfoUI>();
            int tmpcost = 0;
            bool isUse = gameDataMgr.gameData().m_bossTag.bossUse[i];
            if (isUse) 
                tmpcost = 0;
            else 
                tmpcost = tmpBoss.Cost;
            
            // tmpUI.initData(tmpBoss.Pic, roleEle.Name, roleEle.Des, tmpcost);
            tmpUI.initData(tmpBoss.Pic, tmpcost, roleID, onClick);

            if (i == 0) //给个默认选项
            {
                onClick(roleID, tmpUI);
            }

        }
    }

    private void onClick(int roleID, shopInfoUI nowInfoUI) {
        if (oldSelUI != null) {
            oldSelUI.setSelectActive(false); 
        }

        oldSelUI = nowInfoUI;
        oldSelUI.setSelectActive(true);
        //roleID
        roleProUI.showData(oldSelUI.roleID, oldSelUI.cost, onHideBuyTxt);
    }

    private void onHideBuyTxt() {
        oldSelUI.hideBuyTxt();
        oldSelUI.cost = 0;
        refreshReward();
    }

    public void UIclose() {
        gameObject.SetActive(false);
    }
    
}
