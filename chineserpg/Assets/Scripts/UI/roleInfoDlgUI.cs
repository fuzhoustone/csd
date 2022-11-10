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
    private List<roleNameBtnUI> nameLst = null;

    [SerializeField]
    private Transform nameViewPostLst;

    [SerializeField]
    private Transform infoViewLst;

    [SerializeField]
    private roleHeadUI roleHead;

    [SerializeField]
    private Text roleNameText;

    [SerializeField]
    private Text roleHeadText;


    void Start()
    {
        
    }

    public void showUI() {
        this.gameObject.SetActive(true);
        if((nameLst == null) ||(nameLst.Count <=0))
            initData();
        
    }

    public void UIClose()
    {
        this.gameObject.SetActive(false);
    }

    public void clearData() { 
        
    }

    public void initData()
    {
        nameLst = new List<roleNameBtnUI>();

        int nCount = roleNameTab._instance().GetTableLength();
       
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
      //  Debug.LogWarning("clueOnClick id:" + tmpID.ToString());
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
        roleHead.setImage(tmpNameBtnUI.pId);

        CSVRow tmpRow = roleNameTab._instance().GetRowFromID(tmpNameBtnUI.pId);

        string titleName = tmpRow.GetString(roleNameTab.csTitle);
        roleHeadText.text = titleName;

        clearInfoMsg();

        
        // int nCount = tmpID;
        
        int nowRoleID = tmpNameBtnUI.pId;
        int nowChaptID = gameDataManager.instance.chaptID;
        

        int nCount = clueLstTab._instance().GetTableLength();
        //nowChaptID = 1;
        for (int i = 1; i <= nCount; i++) 
        {

            CSVRow tmpClueRow = clueLstTab._instance().GetRowFromIndex(i - 1);
            int tmpRoleID = tmpClueRow.GetInt(clueLstTab.csRoleID);
            int tmpChaptID = tmpClueRow.GetInt(clueLstTab.csChaptID);
            int tmpClueID = tmpClueRow.GetInt(clueLstTab.csID);
            if ((tmpRoleID == nowRoleID) && (nowChaptID >= tmpChaptID))
            {
                //检查线索是否已公开    （自身的线索，在对应章节 线索获得表 需自动加入）
                CSVRow tmpGetRow = clueLstGetTab._instance().GetRowFromKeyVal(clueLstGetTab.csClueID, tmpClueID.ToString());
                if (tmpGetRow != null) {
                    bool isPub = tmpGetRow.GetBool(clueLstGetTab.csIsPub);
                    bool isLook = tmpGetRow.GetBool(clueLstGetTab.csLook);
                    int  clueGetID = tmpGetRow.GetInt(clueLstGetTab.csID);

                    UnityEngine.Object infoObj = Resources.Load(csPreInfoMsg);
                    GameObject tmpObj = GameObject.Instantiate(infoObj, infoViewLst) as GameObject;
                    roleInfoMsgUI tmpUIData = tmpObj.GetComponent<roleInfoMsgUI>();
                    string testMsg = tmpClueRow.GetString(clueLstTab.csContentCn);
                    tmpUIData.setData(clueGetID, testMsg, isLook, isPub);
                }
            }

        }

    }
}
