﻿using System.Collections;
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
    public const int ciChaptMaxTime = 5;
    public int freeTime;
    public int chaptID;

    private WordOutPut UIContxt;
    private Text roleNameTxt;

    private int nextID;
    private const int csFinishEnd = -1;
    private bool InFreeTime = false;
    private List<CSVRow> rolePKLst;

    private int roleOrdAct = 0;
    private bool saySelf = false;


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
    public void chaptFreeTimeInit(int lChaptID, WordOutPut lUIText, Text lNameTxt) {
        chaptID = lChaptID;
        freeTime = 0;
        UIContxt = lUIText;
        roleNameTxt = lNameTxt;
        //更新各角色的友好度及敌对值
        //在有敌对的玩家中随机选一名进行行动

    }


    public void chaptExec() { //AI自由PK的执行
        //UI展现选项， 玩家不行动  或 玩家行动
        //玩家行动，则调用talkScene
        //不行动，则AI行动 AITurn();
        AITurn();  
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
            int tmpTalkID = tmpRoleInfo.GetInt(talkRoleInfoGetTab.csTalkRoleInfoID);
            int tmpTalkStoryID = talkRoleInfoTab._instance().GetValueFromID<int>(tmpTalkID, talkRoleInfoTab.csTalkStortyID, -1);

            tmpRoleInfo.SetBool(talkRoleInfoGetTab.csIsUse, true);
            talkRoleInfoGetTab._instance().SaveFile();

            CSVRow tmpStoryRow = talkStoryTab._instance().GetRowFromID(tmpTalkStoryID);
            talkStoryUI(tmpStoryRow, lRoleID); //UI展现话题， 等UI上的onclick事件触发后续
        }
    }

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
        else { //暂时不测
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
               //doThing(tmpRoleID, -1);
                talkSelf();  //暂时当作玩家自己已说
            }
            else
            {
                doThing(tmpRoleID, -1);
            }
        }
        

    }


}
