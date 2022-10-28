using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class selRoleDialogUI : MonoBehaviour
{
    private UnityAction<int> callEvent;
    private int selRoleId;
    private int selfRoleId = -2;
    private int oldIndex = 0;

    [SerializeField]
    private List<roleBodyUI> roleStartLst;
    [SerializeField]
    private List<roleBodyUI> roleTalkLst;

    [SerializeField]
    private GameObject palListRow12;

    [SerializeField]
    private GameObject palListRow13;

    [SerializeField]
    private Text titleTxt;

    private const string csSelCosPlayCn = "选择一个想扮演的角色";
    private const string csSelCosPlayEn = "";
    private const string csSelTalkCn = "选择一个想私聊的角色";
    private const string csSelTalkEn = "";
    private List<roleBodyUI> roleLst;

    public void initData(int pSelfRoleID) {
        selfRoleId = pSelfRoleID;
        if (pSelfRoleID >= 0) //选择其它人聊天
        {
            roleLst = roleTalkLst;
            palListRow12.SetActive(true);
            palListRow13.SetActive(false);
            titleTxt.text = csSelTalkCn;
        }
        else //选择一个角色玩
        {  
            roleLst = roleStartLst;
            palListRow12.SetActive(false);
            palListRow13.SetActive(true);
            titleTxt.text = csSelCosPlayCn;
        }

        int nCount = roleLst.Count;
        int roleRowIndex = 1;
        for (int i = 1; i <= nCount; i++)
        {
            int index = i - 1;
            roleBodyUI tmpUIData = roleLst[index];

            //第一个角色是不能选的，所以roleRowIndex不-1
            CSVRow tmpRow = roleNameTab._instance().GetRowFromIndex(roleRowIndex);
            int tmpID = tmpRow.GetInt(roleNameTab.csID);
            roleRowIndex++;
            if (pSelfRoleID == tmpID) //只需要跳一个
            {
                tmpRow = roleNameTab._instance().GetRowFromIndex(roleRowIndex);
                tmpID = tmpRow.GetInt(roleNameTab.csID);
                roleRowIndex++;
            }

            string tmpRoleName = tmpRow.GetString(roleNameTab.csRoleName);
            tmpUIData.initData(tmpID,tmpRoleName);
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
        selRoleId = roleLst[oldIndex].pID;

    }

    public void onClick(int tmpID)
    {
        if (tmpID != oldIndex)
        {
            setRoleSel(oldIndex, false);
            setRoleSel(tmpID, true);
            oldIndex = tmpID;

            selRoleId = roleLst[tmpID].pID; 
          //  roleInfoData(tmpID);
        }
    }

    private void setRoleSel(int index, bool isSel)
    {
        roleBodyUI tmpBtnUI = roleLst[index];
        tmpBtnUI.setSelActive(isSel);
    }

    public void showDialog(UnityAction<int> pEvent, int lSelfID) {
        if ( selfRoleId  != lSelfID) { 
            callEvent = pEvent;
            initData(lSelfID);
        }

        this.gameObject.SetActive(true);
    }

    public void showUI() {
        this.gameObject.SetActive(true);
    }

    public void closeDialog() {
        this.gameObject.SetActive(false);
    }

    public void OKclick() {
        this.gameObject.SetActive(false);

        if (callEvent != null)
            callEvent(selRoleId);
        
    }


}
