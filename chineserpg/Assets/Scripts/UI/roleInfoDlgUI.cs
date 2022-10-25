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

        int nCount = 5;
        for (int i = 1; i <= nCount; i++)
        {
            //int orderUIID = i - 1;
            //创建UI

            UnityEngine.Object infoObj = Resources.Load(csPreClueBtn);
            GameObject tmpObj = GameObject.Instantiate(infoObj, nameViewPostLst) as GameObject;

            roleNameBtnUI tmpUIData = tmpObj.GetComponent<roleNameBtnUI>();
            tmpUIData.initData(i - 1);
            nameLst.Add(tmpUIData);

            Button tmpBtn = tmpObj.GetComponent<Button>();
            tmpBtn.onClick.AddListener(delegate ()
            {
                this.roleNameOnClick(tmpUIData.pId);
            });
        }
        oldClueIndex = 0;
        setRoleNameSel(oldClueIndex, true);

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
        //roleHeadPic
        roleNameText.text = "角色" + tmpID.ToString();
        roleHeadText.text = "角色" + tmpID.ToString() + "testest";

        clearInfoMsg();

        int nCount = tmpID;
        for (int i = 1; i <= nCount; i++)
        {
            UnityEngine.Object infoObj = Resources.Load(csPreInfoMsg);
            GameObject tmpObj = GameObject.Instantiate(infoObj, infoViewLst) as GameObject;
            roleInfoMsgUI tmpUIData = tmpObj.GetComponent<roleInfoMsgUI>();
            string testMsg = "角色" + tmpID.ToString() + "msgmsg";
            tmpUIData.setData(testMsg);
        }

    }
}
