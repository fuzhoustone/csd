using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StoryScene : MonoBehaviour
{
    public Text ContentText; //剧情内容
    public WordOutPut contentTextPut;

    public Image sceneImage; //背景场景

    private List<Button> btnLst; //对话选择
    private List<int> btnStoryLst; //对话选择对应的storyID跳转

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    public GameObject btnPanel;
    public GameObject contextPanel;
    public Button conNext;

    [SerializeField]
    private Text titleRoleTxt;
    [SerializeField]
    private Image roleImage;
    // public TableSet dataTable;

    [SerializeField]
    private CanvasScaler canBg, canUI;

    private int storyID,nextStoryID;
    //private Action<int> btnEvent;
    private const string csBgPicPath = "Textures/ScenePic/";
    //private const string csTalkSceneName = "talkScene";

    private const int ciShowTalkScene = -1;
    private bool chaptInit = false; //章节初始化标志


    private void Start()
    {
        canBg.matchWidthOrHeight = canAdvapt.instance.bgMatchWidHeight;
        canUI.matchWidthOrHeight = canAdvapt.instance.uiMatchWidHeight;
        initParam();
    }

    public void initParam() {
        TableSet.instance.initData();
        toolBarManager.instance.showTopBar();

        // Debug.LogWarning("storyscene start");
        if (btnLst != null)
        {
            btnLst.Clear();
        }
        else
            btnLst = new List<Button>();

        if (btn1)
        {
            btnLst.Add(btn1);
        }
        if (btn2)
        {
            btnLst.Add(btn2);
        }
        if (btn3)
        {
            btnLst.Add(btn3);
        }
        if (btn4)
        {
            btnLst.Add(btn4);
        }

        btnStoryLst = new List<int>();

        nextStoryID = roleStoryStartRelTab._instance().getStartStoryID(gameDataManager.instance.roleID, gameDataManager.instance.chaptID);

        //nextStoryID = 20;

        autoSaveData.instance().initParam(0);
        btnInit();
        showContentText(nextStoryID);
        chaptInit = false;
        roleAIManager.instance.chaptFreeTimeInit(gameDataManager.instance.chaptID, contentTextPut, titleRoleTxt);
    }

    private void showVideo(int lid) {
        
        string fileName = StoryVideoTab._instance().GetValueFromKey<int, string>(
                                                        StoryVideoTab.csOptionID, lid,
                                                        StoryVideoTab.csFileName, "");
        //string msg = tmpRow.GetString(StoryRelationTab.csContentCN);
        //ContentText.text = stringReplace(msg);


        /*
        //是否自动保存
        int isAutoSave = tmpRow.GetInt(StoryRelationTab.csIsAutoSave);
        if (isAutoSave == 1)
        {
            autoSaveData.instance().saveData(storyID);
        }
        
        nextStoryID = tmpRow.GetInt(StoryRelationTab.csNextID);
        if (nextStoryID == 0)
        { //出现选项的剧情
            showTalkSel(nowStoryid);
        }
        
        int tmpIsRoleSay = tmpRow.GetInt(StoryRelationTab.csIsRoleSay);
        if (tmpIsRoleSay == 1) //角色说的话
        {
            //判断角色是否发生改变
        }
        else
        { //背景傍白
            //角色头像是否要隐蔽
        }

        int tmpIsChangeBgScene = tmpRow.GetInt(StoryRelationTab.csNeedChangeBg);
        if (tmpIsChangeBgScene == 1) //场景需要切换
        {
            showBgScene(nowStoryid);
        }
        */
    }

    private void btnClick(int lStoryID,int lVideoID) {
        autoSaveData.instance().saveKeyData(lStoryID);
        autoSaveData.instance().saveData(lStoryID);

        showContentText(lStoryID);

    }

    public void btnInit() {

        if (btn1)
            btn1.gameObject.SetActive(false);
        if (btn2)
            btn2.gameObject.SetActive(false);
        if (btn3)
            btn3.gameObject.SetActive(false);
        if (btn4)
            btn4.gameObject.SetActive(false);

        conNext.gameObject.SetActive(true);

    }

    private string stringReplace(string lVal) {
        string res = "";
        res = lVal.Replace("\\n","\n");
        return res;
    }

    //切换背景的场景
    private void showBgScene(int lid) {
        //获得场景图片的ID

        int bgPicID = StoryBgSceneRelationTab._instance().GetValueFromKey<int, int>
            (StoryBgSceneRelationTab.csStoryID, lid, StoryBgSceneRelationTab.csSceneID, 0);
        string bgPicName = bgScenePicTab._instance().GetValueFromID<string>(bgPicID, bgScenePicTab.csPicName, "");

        if (bgPicName == "") {
            Debug.LogError("bgPicName is null, storyid is"+lid.ToString());
        }

        string picPathName = csBgPicPath + bgPicName;  //不含png扩展名
                                                       // string picPathName = csBgPicPath + "boar_blue.png";
        
        Texture2D tmpPic = (Texture2D)Resources.Load(picPathName) as Texture2D;
        sceneImage.sprite = Sprite.Create(tmpPic, new Rect(0, 0, tmpPic.width, tmpPic.height), new Vector2(0.5f, 0.5f));

    }

    private void getClueInStory() {
        //根据当前章节及角色自动获得线索
        //gameDataManager.instance.chaptID;
        //gameDataManager.instance.roleID;
        List<int> tmpClueLst = clueLstTab._instance().getClueLst(gameDataManager.instance.roleID, gameDataManager.instance.chaptID);
        for (int i = 0; i < tmpClueLst.Count; i++) {
            int tmpID = tmpClueLst[i];
            clueLstGetTab._instance().AddRow(tmpID);
        }
        clueLstGetTab._instance().SaveFile();
        noteMsg.instance.noteUI.msgNoteBottom("你获得新的线索");
    }
    
    //显示剧情内容
    public void showContentText(int nowStoryid) {
        gameDataManager.instance.storyID = nowStoryid;
        if(nowStoryid == ciShowTalkScene) //固定剧情结束，进入聊天环节, 
        {
            //每章节限制调用一次
            // SceneManager.LoadSceneAsync(csTalkSceneName);
            if (chaptInit == false)
            {
                chaptInit = true;

                getClueInStory();
                toolBarManager.instance.topBar.showMission(true);
               // roleAIManager.instance.startFreeTime();
                roleAIManager.instance.talkSelfStart();
            }
            else {
              //  roleAIManager.instance.talkSelf();
            }
            
        }
        else
        {


            toolBarManager.instance.topBar.showMission(false);
            contextPanel.SetActive(true);

            storyID = nextStoryID;


            CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(nowStoryid);
            string msg = tmpRow.GetString(StoryRelationTab.csContentCN);
            contentTextPut.setContext(msg);

            //是否自动保存
            int isAutoSave = tmpRow.GetInt(StoryRelationTab.csIsAutoSave);
            if (isAutoSave == 1)
            {
                autoSaveData.instance().saveData(storyID);
            }

            nextStoryID = tmpRow.GetInt(StoryRelationTab.csNextID);
            if (nextStoryID == 0)
            { //出现选项的剧情
                showTalkSel(nowStoryid);
            }

            int tmpRoleID = tmpRow.GetInt(StoryRelationTab.csIsRoleSay);
            if (tmpRoleID > 0) //角色说的话
            {
                //判断角色是否发生改变
                titleRoleTxt.gameObject.SetActive(true);
                roleImage.gameObject.SetActive(true);
                CSVRow tmpRoleRow = roleNameTab._instance().GetRowFromID(tmpRoleID);
                titleRoleTxt.text = tmpRoleRow.GetString(roleNameTab.csRoleName);


            }
            else
            { //背景傍白
              //角色头像是否要隐蔽
                titleRoleTxt.gameObject.SetActive(false);
                roleImage.gameObject.SetActive(false);
            }

            int tmpIsChangeBgScene = tmpRow.GetInt(StoryRelationTab.csNeedChangeBg);
            if (tmpIsChangeBgScene == 1) //场景需要切换
            {
                showBgScene(nowStoryid);
            }
        }
    }
    
    //UI上的next按扭触发剧情
    public void showUINextContentText() {
        //判断是否自由行动时间
        if (roleAIManager.instance.isAITimeNow()) {
            roleAIManager.instance.onNextClick();
        }
        else
            showContentText(nextStoryID);
    }
    
    //显示对话选择
    public void showTalkSel(int talkID) {
        btnInit();
        List<storyOptionTab.optionObj> optionLst = storyOptionTab._instance().getOptionLst(talkID);
       // List<string> btnText = new List<string>();
        for (int i = 0; i < optionLst.Count; i++) {
           storyOptionTab.optionObj tmpObj = optionLst[i];
            
           Button tmpBtn = btnLst[i];
           Transform tmpChild = tmpBtn.transform.GetChild(0);
           Text tmpBtnText = tmpChild.GetComponent<Text>();
           tmpBtnText.text = stringReplace(tmpObj.optionStrCn);
           tmpBtn.gameObject.SetActive(true);
           tmpBtn.onClick.RemoveAllListeners();
           tmpBtn.onClick.AddListener(delegate (){
           btnClick(tmpObj.nextStoryID,tmpObj.videoID);
           }
            );
           

        }
        btnPanel.SetActive(true);
    }

}
