using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleAIManager 
{
    private static roleAIManager _instance;
    public static roleAIManager instance
    {
        get {
            if (_instance == null)
            {
                _instance = new roleAIManager();
            }

            return _instance;
        }
    }

    //章节最大活动时间
    private const int ciChaptMaxTime = 10;
    private int freeTime;

    private int talkSelTime; //每讨论多少次出现让玩家的话题选择
    private int chaptID;

    private WordOutPut UIContxt;
    private Text roleNameTxt;
    

    private int nextID;
    private const int csFinishEnd = -1;
    private bool InFreeTime = false;
    private List<CSVRow> rolePKLst;

    private int roleOrdAct = 0;
    private bool saySelf = false;
    
    public delegate void storyBtnClick(List<storyOptionTab.optionObj> optionLst,
                                          Action<int, int> pAction);

    private storyBtnClick optBtnClick;
    private Action hideOpt;
    private Action showTalkSel;

    public bool isAITimeNow() {
        if (InFreeTime || saySelf)
            return true;
        else
            return false;
    }

    public void startFreeTime() {
        InFreeTime = true;
        saySelf = false;
        if (rolePKLst != null)
        {
            rolePKLst.Clear();
        }
        rolePKLst = null;

        chaptExec();
    }

    private void initRolePKLst() {
        if (rolePKLst != null) { 
            rolePKLst = null;
        }

        rolePKLst = roleFriendTab._instance().getEnemy();
        if (rolePKLst.Count <= 0)
        {
            rolePKLst = roleDefEnemyTab._instance().getDefEnemy();
        }
    }

    public void endFreeTime() {
        InFreeTime = false;
    }

    //各章节活动时间
    public void chaptFreeTimeInit(int lChaptID, WordOutPut lUIText, Text lNameTxt, 
                                  storyBtnClick lBtnClick, Action lHideOpt, Action lshowTalkSel) {
        chaptID = lChaptID;
        freeTime = 0;
        talkSelTime = 3;
        UIContxt = lUIText;
        roleNameTxt = lNameTxt;
        optBtnClick = lBtnClick;
        hideOpt = lHideOpt;
        showTalkSel = lshowTalkSel;

    }



    private void onSelfClick(int lTalkStoryID, int lTalkRoleGetID) {
        hideOpt();

        CSVRow tmpGet = talkRoleInfoGetTab._instance().GetRowFromID(lTalkRoleGetID);
        talkGetDoThing(tmpGet, gameDataManager.instance.roleID);
    }

    private void showOptLst() {  //显示玩家自述
        int lroleID = gameDataManager.instance.roleID;

        List<storyOptionTab.optionObj> optionLst = new List<storyOptionTab.optionObj>();

        int nCount = talkRoleInfoGetTab._instance().GetTableLength();
        for (int i = 0; i < nCount; i++) { 
            CSVRow tmpGetRow = talkRoleInfoGetTab._instance().GetRowFromIndex(i);
            int tmpGetID = tmpGetRow.GetInt(talkRoleInfoGetTab.csID);

            if ((tmpGetRow.GetInt(talkRoleInfoGetTab.csRoleID) == lroleID) && //未使用的话题
               (tmpGetRow.GetBool(talkRoleInfoGetTab.csIsUse) == false))
            {
                int talkRoleInfoID = tmpGetRow.GetInt(talkRoleInfoGetTab.csTalkRoleInfoID);
                CSVRow tmpRoleInfo = talkRoleInfoTab._instance().GetRowFromID(talkRoleInfoID);
                if (tmpRoleInfo.GetInt(talkRoleInfoTab.csTarRoleID) == -1) { //自述话题
                    int talkStoryID = tmpRoleInfo.GetInt(talkRoleInfoTab.csTalkStortyID);
                    CSVRow tmpStory = talkStoryTab._instance().GetRowFromID(talkStoryID);
                    string lTxtCn = tmpStory.GetString(talkStoryTab.csTalkTxtCn);
                    string lTxtEn = tmpStory.GetString(talkStoryTab.csTalkTxtEn);

                    storyOptionTab.optionObj tmpObj = new storyOptionTab.optionObj();
                    tmpObj.optionStrCn = lTxtCn;
                    tmpObj.optionStrEn = lTxtEn;
                    tmpObj.nextStoryID = talkStoryID;
                    tmpObj.noteID = tmpGetID;

                    optionLst.Add(tmpObj);
                }
            }
        }

        optBtnClick(optionLst, onSelfClick);
    }


    public void chaptExec() { //AI自由PK的执行
        //UI展现选项， 玩家不行动  或 玩家行动
        //玩家行动，则调用talkScene
        if (freeTime % talkSelTime == 0)
        {
            showTalkSel();
        }
        else {
            //不行动，则AI行动 AITurn();
            AITurn();
        }
    }

    public void AITurn() {
        int tarRoleID = -1;
        int tmpRoleID =  getAIRoleTurn(ref tarRoleID);  //获取要行动的AIrole
        doThing(tmpRoleID, tarRoleID);
        //对话后，由UI层onclick触发finishThing
    }

    //计算AI中谁行动
    private int getAIRoleTurn(ref int tarRoleID) {
        int  res = -1;

        if ((rolePKLst == null) || (rolePKLst.Count <= 0)) {
            initRolePKLst();
        }
        /*
        //先获得行动列表
        List<CSVRow> roleFriLst = roleFriendTab._instance().getEnemy();
        if (roleFriLst.Count <= 0) {
            roleFriLst = roleDefEnemyTab._instance().getDefEnemy();
        }
        */
        CSVRow lResRow = null;
        //从各role不友好列表中，取行动值最高的
        int oldAct = 0;
        int nCount = rolePKLst.Count;
        int index = -1;
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpFriRow = rolePKLst[i];
            int tmpRoleID = tmpFriRow.GetInt(roleFriendTab.csRoleID);
            
            CSVRow tmpActRow = roleActTab._instance().GetRowFromKeyVal(roleActTab.csRoleID, tmpRoleID.ToString());
            int tmpActVal = tmpActRow.GetInt(roleActTab.csActVal);
            if (oldAct < tmpActVal) {
                oldAct = tmpActVal;
                lResRow = tmpFriRow;
                index = i;
            }
        }

        if (lResRow == null) {
            Debug.LogError("AI turn not AI to act");
        }

        res = lResRow.GetInt(roleFriendTab.csRoleID);  //roleDefEnemyTab.csRoleID
        tarRoleID = lResRow.GetInt(roleFriendTab.csTargetID); //roleDefEnemyTab.csTargetID
        rolePKLst.Remove(lResRow);
        return res;
    }


    private void nextDoThing() {
        //freeTime--;
        nextID = csFinishEnd;
        onNextClick();
    }

    //传入 talkRoleInfoGetTab的某一行, 说话人的ID
    private void talkGetDoThing(CSVRow tmpRoleGet,int lRoleID) {
        tmpRoleGet.SetBool(talkRoleInfoGetTab.csIsUse, true); //设置某个话题已说
        talkRoleInfoGetTab._instance().SaveFile();

        //获得talkRoleInfoID
        int tmpTalkID = tmpRoleGet.GetInt(talkRoleInfoGetTab.csTalkRoleInfoID);
        //获得talkstoryID
        int tmpTalkStoryID = talkRoleInfoTab._instance().GetValueFromID<int>(tmpTalkID, talkRoleInfoTab.csTalkStortyID, -1);
        //某个话题被提到引发新的话题
        talkRoleInfoTalkingGetRuleTab._instance().checkAddTalkRoleInfo(tmpTalkStoryID);

        //UI展现话题， 等UI上的onclick事件触发后续
        CSVRow tmpStoryRow = talkStoryTab._instance().GetRowFromID(tmpTalkStoryID);
        talkStoryUI(tmpStoryRow, lRoleID);
    }

    private void doThing(int lRoleID, int lTarRoleID)
    {    //AI自动挑选最想说的话题（说过的不再说）
        CSVRow tmpRoleInfo = talkRoleInfoGetTab._instance().getTalkRoleInfo(lRoleID, lTarRoleID);
        if (tmpRoleInfo == null)
        { //若没有想说的
            nextDoThing();
            Debug.LogWarning("nextDoThing lroleID=" + lRoleID.ToString() + ",lTarRoleID=" + lTarRoleID.ToString());
        }
        else
        {  
            talkGetDoThing(tmpRoleInfo, lRoleID);
        }
    }

    //lRoleID：说话人的ID
    public void talkStoryUI(CSVRow talkStoryRow, int lRoleID = -1) {
        string msg = talkStoryRow.GetString(talkStoryTab.csContentCn);
        UIContxt.setContext(msg);

        if(lRoleID != -1)
        {
            CSVRow tmpRoleRow = roleNameTab._instance().GetRowFromID(lRoleID);
            roleNameTxt.text = tmpRoleRow.GetString(roleNameTab.csRoleName);
            roleNameTxt.gameObject.SetActive(true);
        }


        nextID = talkStoryRow.GetInt(talkStoryTab.csNextID);
    }

    

    public void onNextClick() {
        if (nextID == csFinishEnd) //某一个话题结束
        {
            if (saySelf == false)  //自由PK讨论
            {
                finishThing();
            }
            else {              //自述阶断
                talkSelf();
            }
        }
        else {
            //某个话题被提到引发新的话题
            talkRoleInfoTalkingGetRuleTab._instance().checkAddTalkRoleInfo(nextID);

            //UI展现当前话题
            CSVRow tmpStoryRow = talkStoryTab._instance().GetRowFromID(nextID);
            talkStoryUI(tmpStoryRow);
        }
    }

    public void finishThing() {
        freeTime++;
        if (freeTime < ciChaptMaxTime) //继续行动
            chaptExec();
        else {
            endFreeTime();
            nextChapt(); //进入下一章
        }
    }

    private void nextChapt() { //进入下一章
        noteMsg.instance.noteUI.msgNoteBottom("进入下一章");
        gameDataManager.instance.chaptID++; 
    }


    public void talkSelfStart() {
        //按角色行动顺序，从1-6 分别自述
        roleOrdAct = 1;
        saySelf = true;
        InFreeTime = false;
        talkSelf();
    }

    public void talkSelf() {

        if (roleOrdAct > 6)
        {
            Debug.Log("allTalkSelf");
            //noteMsg.instance.noteUI.msgNoteBottom("下一章节内容制作中");
            startFreeTime(); //切换成相互PK
        }
        else {
            int tmpRoleID = roleChaptActOrdTab._instance().GetValueFromKey<int, int>
                          (roleChaptActOrdTab.csActOrder, roleOrdAct, roleChaptActOrdTab.csRoleID, 0);
            roleOrdAct++;

            if (tmpRoleID == gameDataManager.instance.roleID)
            {  //玩家自己的
                //talkSelf();  //暂时当作玩家自己已说
                showOptLst(); //UI展现，由玩家选择自述话题
            }
            else
            {
                doThing(tmpRoleID, -1);
            }
        }
        

    }


}
