using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private Transform uiCammeraTransform;

    [SerializeField]
    private Button continueBtn;

    [SerializeField]
    private CanvasScaler canBg, canUI;

    //    [SerializeField]
    //    private GameObject helpObj;

    [SerializeField]
    private DialogBox m_DialogBox;

    private UnityAction btnEvent1, btnEvent2, btnEvent3; //分别对应，1个，2个，3个按扭
    private bool hasRecord;
    private const string title = "请确认";
    private const string text = "新游戏将清空现有游戏记录，是否清空并开始新游戏";


/*
    [SerializeField]
    private Text startTxt;
    [SerializeField]
    private Text continueTxt;
    [SerializeField]
    private Text shopTxt;
    [SerializeField]
    private Text helpTxt;
    [SerializeField]
    private Text exitTxt;
    [SerializeField]
    private Text gameTxt;
*/
    /*
    private void steamShow() {
        startTxt.text = "start";
        continueTxt.text = "continue";
        shopTxt.text = "shop";
        helpTxt.text = "control";
        exitTxt.text = "exit";

        gameTxt.text = "monster maze";
    }
    */
    public void screenAdapt()
    {
        //Debug.Log("screenAdapt");
        int ManualWidth = 960;
        int ManualHeight = 640;
        float designHeight = 640.0f;
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        if (uiCammeraTransform != null)
        {
            Camera camera = uiCammeraTransform.GetComponent<Camera>();
            float scale = System.Convert.ToSingle(manualHeight / designHeight);
            camera.fieldOfView *= scale;
        }

    }

    void Start()
    {
        TableSet.instance.initData();
        canBg.matchWidthOrHeight = canAdvapt.instance.bgMatchWidHeight;
        canUI.matchWidthOrHeight = canAdvapt.instance.uiMatchWidHeight;
    }

    //切换到游戏场景
    private void changeGameScene(int roleID) {
        newGameDataInit(roleID);

        sceneName.instance.changeSceneSingle(sceneName.csStoryScene);
        toolBarManager.instance.showTopBar();
        toolBarManager.instance.topBar.StorySceneTopBtnConfig();
        

    }

    /// <summary>
    /// UI按扭
    /// </summary>
    public void startNewGame() {

        toolBarManager.instance.showSelRoleDlg(changeGameScene);
       
    }

    private void newGameDataInit(int roleID) {
        gameDataManager.instance.roleID = roleID;
        gameDataManager.instance.chaptID = 1;

        clueLstGetTab._instance().checkAndNewFile();
        clueLstGetTab._instance().LoadFile();

        talkInfoLstGetTab._instance().checkAndNewFile();
        talkInfoLstGetTab._instance().LoadFile();

        roleFriendTab._instance().checkAndNewFile();
        roleFriendTab._instance().LoadFile();

        roleActTab._instance().checkAndNewFile();
        roleActTab._instance().LoadFile();

        talkRoleInfoGetTab._instance().checkAndNewFile();
        talkRoleInfoGetTab._instance().LoadFile();

        //noteMsg.instance.noteUI.msgNoteBottom("newGameDataInit filefinish");

        talkRoleInfoChaptGetRuleTab._instance().getTalkRoleInfoFromChapt(gameDataManager.instance.chaptID);
        talkInfoLstGetRuleTab._instance().getTalkLstFromChapt(gameDataManager.instance.chaptID);
        //noteMsg.instance.noteUI.msgNoteBottom("newGameDataInit finish");
        
    }


    private void dialogBox(string title, string text, Sprite icon, string[] buttons,
                          UnityAction pBtnEvent1, UnityAction pBtnEvent2, UnityAction pBtnEvent3)
    {
        btnEvent1 = pBtnEvent1;
        btnEvent2 = pBtnEvent2;
        btnEvent3 = pBtnEvent3;
        m_DialogBox.Show(title, text, icon, boxResult, buttons);
    }

    private void boxResult(int param)
    {
        if (param == 0)
        {
            if (btnEvent1 != null)
            {
                btnEvent1();
            }
        }
        else if (param == 1)
        {
            if (btnEvent2 != null)
            {
                btnEvent2();
            }
        }
        else if (param == 2)
        {
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
        else
        {
            Debug.LogWarning("startSceneboxResult error");
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
    }

    /*
    private void clearAndNewGame() {
        changeGameScene(); //切换场景开始游戏
    }
    */
    private void closeDialog() {
        m_DialogBox.Close();
    }

    public void continueGame()
    {
        noteMsg.instance.noteUI.msgNoteBottom("test");
      //  changeGameScene();
    }

    /*
    public void helpShow() {

        helpObj.SetActive(true);
    }
    */

    public void showDevelopers() { 
    
    }

    public void setTing() { 
    
    }

    public void setLanguage() { 
    
    }

    public void loadGame() { 
        
    }

    public void exitGame()
    {
#if UNITY_EDITOR
       
            UnityEditor.EditorApplication.isPlaying = false;
    #else
         Application.Quit();
    #endif
    }

}
