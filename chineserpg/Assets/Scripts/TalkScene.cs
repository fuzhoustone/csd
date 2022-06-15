using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkScene : MonoBehaviour
{
    public Text ContentText; //剧情内容
    public Image sceneImage; //背景场景

    private List<Button> btnLst; //对话选择

    public Button btn1;
    public Button btn2;
    public Button btn3;
    public Button btn4;

    public GameObject talkPanel;
    public GameObject contextPanel;
    public Button conNext;

    private int storyID,nextStoryID;
    //private Action<int> btnEvent;
    private const string csBgPicPath = "Textures/ScenePic/";   


    private void Start()
    {
        
        if (btnLst != null) {
            btnLst.Clear();
        }
        else
            btnLst = new List<Button>();

        if (btn1)
        {
            btnLst.Add(btn1);
            btn1.onClick.AddListener(btnClick);
        }
        if (btn2) {
            btnLst.Add(btn2);
            btn2.onClick.AddListener(btnClick);
        }
        if (btn3) {
            btnLst.Add(btn3);
            btn3.onClick.AddListener(btnClick);
        }
        if(btn4) {
            btnLst.Add(btn4);
            btn4.onClick.AddListener(btnClick);
        }

        nextStoryID = 1;
        //showContentText(1);
    }

    public void btnClick() {
        string tmpTag = this.tag;
        //int talkID = int.Parse(tmpTag);
        if (btn1)
            btn1.gameObject.SetActive(false);
        if (btn2)
            btn2.gameObject.SetActive(false);
        if (btn3)
            btn3.gameObject.SetActive(false);
        if (btn4)
            btn4.gameObject.SetActive(false);
        talkPanel.SetActive(false);
        conNext.gameObject.SetActive(true);
    }

    //切换背景的场景
    private void showBgScene(int lid) {
        //获得场景图片的ID

        int bgPicID = StoryBgSceneRelationTab._instance().GetKeyValueFromID<int, int>
            (StoryBgSceneRelationTab.csStoryID, lid, StoryBgSceneRelationTab.csSceneID, 0);
        string bgPicName = bgScenePicTab._instance().GetValueFromID<string>(bgPicID, bgScenePicTab.csScenePic, "");

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
        storyID = nextStoryID;
        
        CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(nowStoryid);
        string msg = tmpRow.GetString(StoryRelationTab.csContentCN);
        nextStoryID = tmpRow.GetInt(StoryRelationTab.csNextID);

        //string msg = StoryRelationTab.GetValueFromID<string>(nowStoryid, StoryRelationTab.csContentCN, "");
        ContentText.text = msg;

        //int isSel = tmpRow.GetInt(StoryRelationTab.csIsTalkSel);
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
        List<talkOptionTab.optionObj> optionLst = talkOptionTab._instance().getOptionLst(talkID);
       // List<string> btnText = new List<string>();
        for (int i = 0; i < optionLst.Count; i++) {
           talkOptionTab.optionObj tmpObj = optionLst[i];
           Button tmpBtn = btnLst[i];
           Transform tmpChild = tmpBtn.transform.GetChild(0);
           Text tmpBtnText = tmpChild.GetComponent<Text>();
           tmpBtnText.text = tmpObj.optionStrCn;
           tmpBtn.gameObject.SetActive(true);
           tmpBtn.onClick.RemoveAllListeners();
           //tmpBtn.tag = tmpObj.nextStoryID.ToString();
           tmpBtn.onClick.AddListener(btnClick);
        }

        conNext.gameObject.SetActive(false);
        talkPanel.SetActive(true);
    }

}
