using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleInfoDlgUI : MonoBehaviour
{
    private const string csPreClueBtn = "Prefabs/UI/roleNameBtn";

    // private Texture2D pSelBg, pUnSelBg;
    private int oldClueIndex = 0;
    private List<roleNameBtnUI> nameLst;
    public Transform nameViewPostLst;

    void Start()
    {
        initData();
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

    public void roleNameOnClick(int tmpID)
    {
        Debug.LogWarning("clueOnClick id:" + tmpID.ToString());
        if (tmpID != oldClueIndex)
        {
            setRoleNameSel(oldClueIndex, false);
            setRoleNameSel(tmpID, true);
            oldClueIndex = tmpID;
        }
    }

    private void setRoleNameSel(int index, bool isSel)
    {
        roleNameBtnUI tmpBtnUI = nameLst[index];
        tmpBtnUI.setSelActive(isSel);

    }
}
