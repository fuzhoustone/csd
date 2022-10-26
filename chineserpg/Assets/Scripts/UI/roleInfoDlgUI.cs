using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleInfoDlgUI : MonoBehaviour
{
    private const string csPreClueBtn = "Prefabs/UI/roleNameBtn";
    private const string csPreInfoMsg = "Prefabs/UI/roleInfoMsg";

    // private Texture2D pSelBg, pUnSelBg;
    private int oldClueIndex = 0;
    private List<roleNameBtnUI> nameLst;

    [SerializeField]
    private Transform nameViewPostLst;

    [SerializeField]
    private Transform infoViewLst;

    [SerializeField]
    private Image roleHeadPic;

    [SerializeField]
    private Text roleNameText;

    [SerializeField]
    private Text roleHeadText;


    void Start()
    {
        initData();
    }

    public void showUI() { 
    
    }

    public void UIClose()
    {
       
    }

    public void initData()
    {
        nameLst = new List<roleNameBtnUI>();

        int nCount = roleNameTab._instance().GetTableLength();
       
        //int nCount = 5;
        for (int i = 1; i <= nCount; i++)
        {
            //int orderUIID = i - 1;
            //创建UI
            CSVRow tmpRow = roleNameTab._instance().GetRowFromIndex(i - 1);
            int tmpID = tmpRow.GetInt(roleNameTab.csID);
            string tmpRoleName = tmpRow.GetString(roleNameTab.csRoleName);

            UnityEngine.Object infoObj = Resources.Load(csPreClueBtn);
            GameObject tmpObj = GameObject.Instantiate(infoObj, nameViewPostLst) as GameObject;
            roleNameBtnUI tmpUIData = tmpObj.GetComponent<roleNameBtnUI>();
            tmpUIData.initData(tmpID, tmpRoleName);

            nameLst.Add(tmpUIData);
            int tmpIndex = i - 1;
            Button tmpBtn = tmpObj.GetComponent<Button>();
            tmpBtn.onClick.AddListener(delegate ()
            {
                this.roleNameOnClick(tmpIndex);
            });
        }
        oldClueIndex = 0;
        setRoleNameSel(oldClueIndex, true);
        roleInfoData(oldClueIndex);
    }

    private void setRoleNameSel(int index, bool isSel)
    {
        roleNameBtnUI tmpBtnUI = nameLst[index];
        tmpBtnUI.setSelActive(isSel);
    }

    public void roleNameOnClick(int tmpID)
    {
        Debug.LogWarning("clueOnClick id:" + tmpID.ToString());
        if (tmpID != oldClueIndex)
        {
            setRoleNameSel(oldClueIndex, false);
            setRoleNameSel(tmpID, true);
            oldClueIndex = tmpID;

            roleInfoData(tmpID);
        }
    }

    private void clearInfoMsg() {
        for (int i = infoViewLst.childCount; i >= 1; i--) {
            Transform tmpTs = infoViewLst.GetChild(i-1);
            GameObject.Destroy(tmpTs.gameObject);
            
        }
    }

    private void roleInfoData(int tmpID)
    {
        roleNameBtnUI tmpNameBtnUI = nameLst[tmpID];
        //roleHeadPic
        roleNameText.text = tmpNameBtnUI.roleName;

        
        CSVRow tmpRow = roleNameTab._instance().GetRowFromID(tmpNameBtnUI.pId);

        string titleName = tmpRow.GetString(roleNameTab.csTitle);
        roleHeadText.text = titleName;

        clearInfoMsg();

        int nCount = clueLstTab._instance().GetTableLength();
        // int nCount = tmpID;
        
        int nowRoleID = tmpNameBtnUI.pId;
        int nowChartID = 1;
        for (int i = 1; i <= nCount; i++)
        {

            CSVRow tmpClueRow = clueLstTab._instance().GetRowFromIndex(i - 1);
            int tmpRoleID = tmpClueRow.GetInt(clueLstTab.csRoleID);
            int tmpChartID = tmpClueRow.GetInt(clueLstTab.csChartID);
            if((tmpRoleID == nowRoleID) && (nowChartID >= tmpChartID))
            { 
                UnityEngine.Object infoObj = Resources.Load(csPreInfoMsg);
                GameObject tmpObj = GameObject.Instantiate(infoObj, infoViewLst) as GameObject;
                roleInfoMsgUI tmpUIData = tmpObj.GetComponent<roleInfoMsgUI>();
                string testMsg = tmpClueRow.GetString(clueLstTab.csContentCn);
                tmpUIData.setData(testMsg);
            }
        }

    }
}
