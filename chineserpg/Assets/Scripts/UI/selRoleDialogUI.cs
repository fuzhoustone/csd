﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class selRoleDialogUI : MonoBehaviour
{
    private UnityAction<int> callEvent;
    private int roleId;
    
    private int oldIndex = 0;

    [SerializeField]
    private List<roleBodyUI> roleStartLst;
    [SerializeField]
    private List<roleBodyUI> roleTalkLst;

    [SerializeField]
    private GameObject palListRow12;

    [SerializeField]
    private GameObject palListRow13;

    private List<roleBodyUI> roleLst;

    public void initData(int pSelfRoleID) {
        if (pSelfRoleID >= 0) //选择其它人聊天
        {
            roleLst = roleTalkLst;
            palListRow12.SetActive(true);
            palListRow13.SetActive(false);
        }
        else //选择一个角色玩
        {  
            roleLst = roleStartLst;
            palListRow12.SetActive(false);
            palListRow13.SetActive(true);
        }

        int nCount = roleLst.Count;
        int roleRowIndex = 1;
        for (int i = 1; i <= nCount; i++)
        {
            int index = i - 1;
            roleBodyUI tmpUIData = roleLst[index];

            CSVRow tmpRow = roleNameTab._instance().GetRowFromIndex(roleRowIndex -1 );
            int tmpID = tmpRow.GetInt(roleNameTab.csID);
            roleRowIndex++;
            if (pSelfRoleID == tmpID)
            {
                tmpRow = roleNameTab._instance().GetRowFromIndex(roleRowIndex - 1);
                tmpID = tmpRow.GetInt(roleNameTab.csID);
            }

            tmpUIData.initData(tmpID);
            //roleLst.Add(tmpUIData);

            Button tmpBtn = tmpUIData.gameObject.GetComponent<Button>();
            tmpBtn.onClick.RemoveAllListeners();
            tmpBtn.onClick.AddListener(delegate ()
            {
                this.onClick(index);
            });
            
        }
        oldIndex = 0;
        setRoleSel(oldIndex, true);

    }

    public void onClick(int tmpID)
    {
        if (tmpID != oldIndex)
        {
            setRoleSel(oldIndex, false);
            setRoleSel(tmpID, true);
            oldIndex = tmpID;

            roleId = roleLst[tmpID].pID; 
          //  roleInfoData(tmpID);
        }
    }

    private void setRoleSel(int index, bool isSel)
    {
        roleBodyUI tmpBtnUI = roleLst[index];
        tmpBtnUI.setSelActive(isSel);
    }

    public void showDialog(UnityAction<int> pEvent, int lSelfID = -1) {
        callEvent = pEvent;
        initData(lSelfID);
        this.gameObject.SetActive(true);
    }

    public void closeDialog() {
        this.gameObject.SetActive(false);
    }

    public void OKclick() {
        this.gameObject.SetActive(false);

        if (callEvent != null)
            callEvent(roleId);
        
    }


}