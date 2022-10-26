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
    private GameObject missionPal;
    //private string csRoleInfoDlgPre = "Prefabs/UI/roleNameBtn/roleInfoDialog";
    [SerializeField]
    private Text missionTitle;
    [SerializeField]
    private Text missionContext;


    public void showMission(bool isShow) {
        if(missionPal.activeSelf != isShow)
            missionPal.SetActive(isShow);
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
}
