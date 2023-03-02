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

    [SerializeField]
    private GameObject missionItem1;

    [SerializeField]
    private GameObject missionItem2;

    [SerializeField]
    private GameObject missionItem3;

    [SerializeField]
    private Animation missPalAni;
    private const string csAniMissionShow = "missionShow";
    private const string csAniMissionHide = "missionHide";


    private int lChartID = 0;
    private bool isShowMission = false;
   // private bool isRefreshMissionLst = false;


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

    public void setMissionAni(bool isShow) {
        if (isShow)
        {
            missPalAni.clip = missPalAni.GetClip(csAniMissionShow);
        }
        else {
            missPalAni.clip = missPalAni.GetClip(csAniMissionHide);
        }
        missPalAni.Play();


    }


    //显示当前任务
    public void showMission(bool isShow) {
        if (isShowMission != isShow)
        {
            isShowMission = isShow;
           // missionPal.SetActive(isShow);
            if (isShow) {
               // isRefreshMissionLst = true;
                missionItem1.SetActive(false);
                missionItem2.SetActive(false);
                missionItem3.SetActive(false);
                List<CSVRow> tmpMissionLst = missionLstTab._instance().getMission(gameDataManager.instance.roleID, gameDataManager.instance.chaptID);
                for (int i = 0; i < tmpMissionLst.Count; i++) { //最多三条任务
                    CSVRow tmpRow = tmpMissionLst[i];
                    string tmpMission = tmpRow.GetString(missionLstTab.csContentCn);
                    missionItem tmpItem = null;
                    if (i == 0)
                    {
                        missionItem1.SetActive(true);
                        tmpItem = missionItem1.GetComponent<missionItem>();
                    }
                    else if (i == 1)
                    {
                        missionItem2.SetActive(true);
                        tmpItem = missionItem2.GetComponent<missionItem>();
                    }
                    else {
                        missionItem3.SetActive(true);
                        tmpItem = missionItem3.GetComponent<missionItem>();
                    }
                    tmpItem.setText(tmpMission);
                    
                    float width = tmpItem.transform.GetComponent<RectTransform>().sizeDelta.x;
                    float height = tmpItem.getTxtHeight();
                    tmpItem.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
                    
                }

                setMissionAni(true);
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
