using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class topToolBarUI : MonoBehaviour
{
    // Start is called before the first frame update
    //private roleInfoDlgUI infoDlgUI = null;
   // [SerializeField]
   // private roleInfoDlgUI infoDlg;

    [SerializeField]
    private Button backBtn; //返回按扭

    [SerializeField]
    private Button talkBtn; //公聊私聊按扭

    [SerializeField]
    private Button roleInfoBtn; //角色信息按扭

    [SerializeField]
    private Text chartTxt;

    [SerializeField]
    private GameObject missionPal;
    //private string csRoleInfoDlgPre = "Prefabs/UI/roleNameBtn/roleInfoDialog";
    [SerializeField]
    private Text missionTitle;
    [SerializeField]
    private Text missionContext;

    //[SerializeField]
    //private CanvasScaler canBg;

    private int lChartID = 0;

    //private void Start()
    //{
    //    canBg.matchWidthOrHeight = canAdvapt.instance.bgMatchWidHeight;
    //}

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

    //显示当前任务
    public void showMission(bool isShow) {
        if (missionPal.activeSelf != isShow)
        {
            missionPal.SetActive(isShow);
            if (isShow) {
                CSVRow tmpRow = missionLstTab._instance().getMission(gameDataManager.instance.roleID, gameDataManager.instance.chaptID);
                string tmpMission = tmpRow.GetString(missionLstTab.csContentCn);
                missionContext.text = tmpMission;

            }
        }
    }

    //显示角色信息
    public void showRoleInfo()
    {
        toolBarManager.instance.showRoleInfoDlg();
        setBackBtnVisible(true);
    }

    public void setBackBtnVisible(bool isVisible) {
        backBtn.gameObject.SetActive(isVisible);
    }

    public void setRoleInfoBtnVisible(bool isVis) {
        roleInfoBtn.gameObject.SetActive(isVis);
    }

    public void setTalkBtnVisible(bool isVis) {
        talkBtn.gameObject.SetActive(isVis);
    }

    //私聊界面
    public void showSelRoleDlg() {
        toolBarManager.instance.showSelRoleDlg(chageToTalkScene, gameDataManager.instance.roleID);
       // setBackBtnVisible(true);
    }

    public void chageToTalkScene(int lroleID) {
        toolBarManager.instance.hideSelRoleDlg();
        toolBarManager.instance.hideRoleInfoDlg();
        setBackBtnVisible(true);
        sceneName.instance.changeScene(sceneName.csTalkScene);
       
    }

    public void changeToStartScene() {
     //   hideAllDlg();
        toolBarManager.instance.hideTopBar();
        sceneName.instance.changeSceneSingle(sceneName.csStartScene);
    }

    public void hideAllDlg() {
        toolBarManager.instance.hideRoleInfoDlg();
        
    }

    public void backBtnClick() {
        hideAllDlg();
        sceneName.instance.changeScene(sceneName.csStoryScene); //切换上一个场景，如果上一个场景不为空
        StorySceneTopBtnConfig();
    }

    public void StorySceneTopBtnConfig() {
        setBackBtnVisible(false);
        setTalkBtnVisible(gameDataManager.instance.isShowTalkBtn());
        setRoleInfoBtnVisible(gameDataManager.instance.isShowRoleInfoBtn());
    }

}
