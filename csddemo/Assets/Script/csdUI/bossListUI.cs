﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class bossListUI : MonoBehaviour
{
    public Transform viewPortLst;
    //public Transform scrollViewParent;
    public TextAsset bossTab;
    private const string csBossinfo = "bossinfo";
    private bool isFirstShow = true;
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

        this.gameObject.SetActive(true);
    }

    //加载显示UI
    public void initData() {
        /*
        using (var stream = new MemoryStream(bossTab.bytes))
        {
            BossInfoTable.Load(stream);
        }
        */
        Object bossInfoObj = Resources.Load("Prefab/UI/" + csBossinfo);

        int nCount = BossInfoTable.GetTableLength();
        float csPosY = -122.0f;
        float allHeight = csPosY * (-1) * nCount;
        RectTransform tmpTrans = viewPortLst.GetComponent<RectTransform>();
        tmpTrans.sizeDelta = new Vector2(tmpTrans.sizeDelta.x, allHeight);

        for (int i = 0; i < nCount; i++) {
            BossInfoTable.bossElements tmpBoss = BossInfoTable.m_elements[i];
            float posY = csPosY * (i-1);
            GameObject tmpObj = GameObject.Instantiate(bossInfoObj, viewPortLst) as GameObject;

            RectTransform tmp = tmpObj.GetComponent<RectTransform>();
            tmp.anchoredPosition3D = new Vector3(0, posY, 0);

            bossInfoUI tmpUI = tmpObj.GetComponent<bossInfoUI>();
            int tmpcost = 0;
            bool isUse = gameDataMgr.gameData().m_bossTag.bossUse[i];
            if (isUse) {
                tmpcost = 0;
            }
            else {
               
                tmpcost = tmpBoss.Cost;
            }
            tmpUI.initData(tmpBoss.Pic, tmpBoss.Name, tmpBoss.Des, tmpcost);
        }
    }

    public void UIclose() {
        gameObject.SetActive(false);
    }
    
}
