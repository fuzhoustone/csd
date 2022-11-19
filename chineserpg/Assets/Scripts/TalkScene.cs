﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkScene : MonoBehaviour
{
   // private const string csUnSelImage = "clueDis";  //线索未选中的图标
   // private const string csSelImage = "clue";   //线索选中的图标
   // private const string csPath = "UIPic/talkScene/";  //资源路径
    private const string csPreClueBtn = "Prefabs/UI/clueBtn";

   // private Texture2D pSelBg, pUnSelBg;
    private int oldClueIndex = 0;
    private List<clueBtnUI> clueLst;

    [SerializeField]
    private List<talkOptionUI> optLst;

    [SerializeField]
    private Transform clueViewPostLst;

    [SerializeField]
    private CanvasScaler canBg, canUI;

    // Start is called before the first frame update
    void Start()
    {
        //  pSelBg = (Texture2D)Resources.Load(csPath + csSelImage) as Texture2D;
        //  pUnSelBg = (Texture2D)Resources.Load(csPath + csUnSelImage) as Texture2D;
        TableSet.instance.initData();
        initData();
        canBg.matchWidthOrHeight = canAdvapt.instance.bgMatchWidHeight;
        canUI.matchWidthOrHeight = canAdvapt.instance.uiMatchWidHeight;
    }

    public void UIClose() {
        clueLst.Clear();
    }

    public void showUI() { 
        
    }

    public void initData()
    {
        clueLst = new List<clueBtnUI>();
        
        int nCount = talkInfoLstTab._instance().GetTableLength();
        for (int i = 1; i <= nCount; i++)
        {
            CSVRow tmpRow = talkInfoLstTab._instance().GetRowFromIndex(i-1);
            int tmpID = tmpRow.GetInt(talkInfoLstTab.csID);
            string tmpKeyStr = tmpRow.GetString(talkInfoLstTab.csContentCn);

            //int orderUIID = i - 1;
            //创建UI
            int indexClick = i - 1;
            UnityEngine.Object infoObj = Resources.Load(csPreClueBtn);
            GameObject tmpObj = GameObject.Instantiate(infoObj, clueViewPostLst) as GameObject;

            clueBtnUI tmpClue = tmpObj.GetComponent<clueBtnUI>();

            tmpClue.initData(tmpID, tmpKeyStr);
            clueLst.Add(tmpClue);

            Button tmpBtn = tmpObj.GetComponent<Button>();
            tmpBtn.onClick.AddListener(delegate ()
            {
                this.clueOnClick(indexClick);
            });
        }
        oldClueIndex = 0;
        setClueSel(oldClueIndex,true);
        clueBtnUI tmpBtnUI = clueLst[oldClueIndex];
        getClueShowToggle(tmpBtnUI.pTalkId);
        // getClue();
    }
/*
    public void getClue() {
        for (int i = 0; i < clueLstTab._instance().GetTableLength(); i++) {
            CSVRow tmpClueRow = clueLstTab._instance().GetRowFromIndex(i);
            int tmpRoleID = tmpClueRow.GetInt(clueLstTab.csRoleID);
            int tmpChaptID = tmpClueRow.GetInt(clueLstTab.csChaptID);
            int tmpClueID = tmpClueRow.GetInt(clueLstTab.csID);
            if ((tmpRoleID == gameDataManager.instance.roleID)
                && (tmpChaptID == gameDataManager.instance.chaptID)) {
                clueLstGetTab._instance().AddRow(tmpClueID);    
            }
        }

        clueLstGetTab._instance().SaveFile();
    }
*/
    public void clueOnClick(int tmpID)
    {
        Debug.LogWarning("clueOnClick id:"+tmpID.ToString());
        if (tmpID != oldClueIndex) {
            setClueSel(oldClueIndex, false);
            setClueSel(tmpID, true);
            oldClueIndex = tmpID;

            clueBtnUI tmpBtnUI = clueLst[tmpID];
            getClueShowToggle(tmpBtnUI.pTalkId);
        }
    }

    private void setClueSel(int index, bool isSel)
    {
        clueBtnUI tmpBtnUI = clueLst[index];
        tmpBtnUI.setSelActive(isSel);
    }

    private void getClueShowToggle(int ltalkID) {
        List<talkInfoOptionTab.talkOptionObj> tmpOptionLst = talkInfoOptionTab._instance().getOptionLst(ltalkID);
        for (int i = 0; i < optLst.Count; i++) {
            optLst[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < tmpOptionLst.Count; i++) {
            talkInfoOptionTab.talkOptionObj tmpOptObj = tmpOptionLst[i];
            talkOptionUI tmpTog = optLst[i];
            tmpTog.gameObject.SetActive(true);

            string tmpStr = tmpOptObj.optionStrCn;
            tmpTog.setTxt(tmpStr, tmpOptObj.optionID);
            //tmpOptObj.optionStrCn;
        }
    }

    public void optionBtnChange(Toggle t) {
        if (t.isOn)
        {
           talkOptionUI tmpUI = t.gameObject.GetComponent<talkOptionUI>();
           int tmpID = tmpUI.getOptionID();
           noteMsg.instance.noteUI.msgNoteBottom("optionID:"+tmpID.ToString());
        }
    }

}
