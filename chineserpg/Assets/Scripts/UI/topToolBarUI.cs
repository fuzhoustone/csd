using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class topToolBarUI : MonoBehaviour
{
    // Start is called before the first frame update
    //private roleInfoDlgUI infoDlgUI = null;
    [SerializeField]
    private GameObject infoDlg;

    [SerializeField]
    private Text chartTxt;

    [SerializeField]
    private GameObject missionPal;
    //private string csRoleInfoDlgPre = "Prefabs/UI/roleNameBtn/roleInfoDialog";
    [SerializeField]
    private Text missionTitle;
    [SerializeField]
    private Text missionContext;

    private int lChartID = 0;

    public void setChartName(int chartID) {
        string tmpChart = "";
        lChartID = chartID;
        switch (chartID){
            case 1:
                tmpChart = "第一章"; break;
            case 2:
                tmpChart = "第二章"; break;
            case 3:
                tmpChart = "第三章"; break;
            case 4:
                tmpChart = "第四章"; break;
            case 5:
                tmpChart = "终章"; break;
        }
        chartTxt.text = tmpChart;

    }

    public void showMission(bool isShow) {
        if (missionPal.activeSelf != isShow)
        {
            missionPal.SetActive(isShow);
            if (isShow) {
                CSVRow tmpRow = missionLstTab._instance().getMission(2,lChartID);
                string tmpMission = tmpRow.GetString(missionLstTab.csContentCn);
                missionContext.text = tmpMission;

            }
        }
    }

    public void showRoleInfo()
    {
        if (infoDlg.activeSelf == false) {
            infoDlg.SetActive(true);
        }

        /*
        if (infoDlgUI == null)
        {
            UnityEngine.Object infoObj = Resources.Load(csRoleInfoDlgPre);
            GameObject tmpObj = GameObject.Instantiate(infoObj) as GameObject;
            infoDlgUI = tmpObj.GetComponent<roleInfoDlgUI>();
        }

        if (infoDlgUI.gameObject.activeSelf == false)
            infoDlgUI.showUI();
        */
    }


    public void showSelRoleDlg() {
        toolBarManager.instance.showSelRoleDlg(null,2);
    }
}
