﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
   // public TableSet dataTable;


    private int storyID,nextStoryID;
    //private Action<int> btnEvent;
    private const string csBgPicPath = "Textures/ScenePic/";
    //private bool updateflag = true;



    private void Start()
    {
       // Debug.Log("test start");
        //dataTable.initData();
        TableSet.instance.initData();
        toolBarManager.instance.getTopBar();

        Debug.LogWarning("storyscene start");
        if (btnLst != null) {
            btnLst.Clear();
        }
        else
            btnLst = new List<Button>();

        if (btn1)
        {
            btnLst.Add(btn1);
        }
        if (btn2) {
            btnLst.Add(btn2);
        }
        if (btn3) {
            btnLst.Add(btn3);
        }
        if(btn4) {
            btnLst.Add(btn4);
        }

        btnStoryLst = new List<int>();
        nextStoryID = 1;

        autoSaveData.instance().initParam(0);
        btnInit();
        showContentText(nextStoryID);
        /*
#if videotest
        sceneImage.gameObject.SetActive(false);
        showContentText(nextStoryID);
#endif
        */
        // videoCon.startPlay();
        /*
         //   \n只会在调试阶段显示
        Debug.LogWarning("换行test1:"+ContentText.text);
        ContentText.text = "换行\ntest";
        Debug.LogWarning("换行test2:"+ContentText.text);
        */
    }

    private void Update()
    {
       // if (updateflag)
       // {
       //     updateflag = false;
       //     Debug.Log("test update");
       // }
    }

    private void showVideo(int lid) {
        //storyID = nextStoryID;

        //获得视频名字

        //CSVRow tmpRow = StoryVideoTab._instance().GetRowFromID(nowStoryid);
        //string fileName = tmpRow.GetString(StoryVideoTab.csFileName);
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
#if videotest
        contextPanel.SetActive(false);
        btnPanel.SetActive(false);
        showVideo(lVideoID);
        nextStoryID = lStoryID;
#else
        showContentText(lStoryID);
#endif
    }

    public void btnInit() {
        //string tmpTag = this.tag;
        //int talkID = int.Parse(tmpTag);

        if (btn1)
            btn1.gameObject.SetActive(false);
        if (btn2)
            btn2.gameObject.SetActive(false);
        if (btn3)
            btn3.gameObject.SetActive(false);
        if (btn4)
            btn4.gameObject.SetActive(false);
#if !videotest
        btnPanel.SetActive(false);
        conNext.gameObject.SetActive(false);
#endif
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

    
    //显示剧情内容
    public void showContentText(int nowStoryid) {
//#if videotest
        contextPanel.SetActive(true);
//#endif
        storyID = nextStoryID;
        

        CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(nowStoryid);
        string msg = tmpRow.GetString(StoryRelationTab.csContentCN);
        //ContentText.text = stringReplace(msg);
        contentTextPut.setContext(msg);

        //contentText.

        //是否自动保存
        int isAutoSave = tmpRow.GetInt(StoryRelationTab.csIsAutoSave);
        if (isAutoSave == 1) {
            autoSaveData.instance().saveData(storyID);
        }

        nextStoryID = tmpRow.GetInt(StoryRelationTab.csNextID);
        if (nextStoryID == 0) { //出现选项的剧情
            showTalkSel(nowStoryid);
        }

        int tmpIsRoleSay = tmpRow.GetInt(StoryRelationTab.csIsRoleSay);
        if (tmpIsRoleSay == 1) //角色说的话
        {
            //判断角色是否发生改变
        }
        else { //背景傍白
            //角色头像是否要隐蔽
        }

        int tmpIsChangeBgScene = tmpRow.GetInt(StoryRelationTab.csNeedChangeBg);
        if(tmpIsChangeBgScene == 1) //场景需要切换
        {
            showBgScene(nowStoryid); 
        }


    }
    
    //UI上的next按扭触发剧情
    public void showUINextContentText() {

        showContentText(nextStoryID);
    }
    
    //显示对话选择
    public void showTalkSel(int talkID) {
        btnInit();
        List<talkOptionTab.optionObj> optionLst = talkOptionTab._instance().getOptionLst(talkID);
       // List<string> btnText = new List<string>();
        for (int i = 0; i < optionLst.Count; i++) {
           talkOptionTab.optionObj tmpObj = optionLst[i];
            
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
#if videotest
        conNext.gameObject.SetActive(false);
#endif
        btnPanel.SetActive(true);
    }

}