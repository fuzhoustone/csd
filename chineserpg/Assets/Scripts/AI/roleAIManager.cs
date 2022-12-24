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
    public const int ciChaptMaxTime = 20;
    public int freeTime;
    public int chaptID;

    private WordOutPut UIContxt;
    private int nextID;
    private const int csFinishEnd = -1;
    private bool InFreeTime = false;

    private int roleOrdAct = 0;
    private bool saySelf = false;

    public bool isFreeTimeNow() {
        return InFreeTime;
    }

    public void startFreeTime() {
        InFreeTime = true;
        saySelf = false;
        chaptExec();
    }

    public void endFreeTime() {
        InFreeTime = false;
    }

    //各章节活动时间
    public void chaptFreeTimeInit(int lChaptID, WordOutPut lUIText) {
        chaptID = lChaptID;
        freeTime = 0;
        UIContxt = lUIText;
        //更新各角色的友好度及敌对值
        //在有敌对的玩家中随机选一名进行行动

    }


    public void chaptExec() {
        //UI展现选项， 玩家不行动  或 玩家行动
        //玩家行动，则调用talkScene
        //不行动，则AI行动 AITurn();
        AITurn();
    }

    public void AITurn() {
        int tarRoleID = -1;
        int tmpRoleID =  getAIRoleTurn(ref tarRoleID);  //获取要行动的AIrole
        doThing(tmpRoleID, tarRoleID);

    }

    //计算AI中谁行动
    private int getAIRoleTurn(ref int tarRoleID) {
        int  res = -1;
        //先获得行动列表
        List<CSVRow> roleFriLst = roleFriendTab._instance().getEnemy();
        if (roleFriLst.Count <= 0) {
            roleFriLst = roleDefEnemyTab._instance().getDefEnemy();
        }

        CSVRow lResRow = null;
        //从各role不友好列表中，取行动值最高的
        int oldAct = 0;
        int nCount = roleFriLst.Count;
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpFriRow = roleFriLst[i];
            int tmpRoleID = tmpFriRow.GetInt(roleFriendTab.csRoleID);
            
            CSVRow tmpActRow = roleActTab._instance().GetRowFromKeyVal(roleActTab.csRoleID, tmpRoleID.ToString());
            int tmpActVal = tmpActRow.GetInt(roleActTab.csActVal);
            if (oldAct < tmpActVal) {
                oldAct = tmpActVal;
                lResRow = tmpFriRow;
            }
        }

        if (lResRow == null) {
            Debug.LogError("AI turn not AI to act");
        }

        res = lResRow.GetInt(roleFriendTab.csRoleID);  //roleDefEnemyTab.csRoleID
        tarRoleID = lResRow.GetInt(roleFriendTab.csTargetID); //roleDefEnemyTab.csTargetID
        return res;
    }

    private void doThing(int lRoleID, int lTarRoleID)
    {    //AI自动挑选最想说的话题
        CSVRow tmpRoleInfo = talkRoleInfoGetTab._instance().getTalkRoleInfo(lRoleID, lTarRoleID);
        if (tmpRoleInfo == null) {
            Debug.LogError("doThing error lroleID="+ lRoleID.ToString()+ ",lTarRoleID="+ lTarRoleID.ToString());
        }

        int tmpTalkID = tmpRoleInfo.GetInt(talkRoleInfoGetTab.csTalkRoleInfoID);
        int tmpTalkStoryID = talkRoleInfoTab._instance().GetValueFromID<int>(tmpTalkID, talkRoleInfoTab.csTalkStortyID,  -1);

        //UI 展现话题，onNextClick点击继续
        tmpRoleInfo.SetBool(talkRoleInfoGetTab.csIsUse, true);
        CSVRow tmpStoryRow = talkStoryTab._instance().GetRowFromID(tmpTalkStoryID);
        talkStoryUI(tmpStoryRow);
    }

    public void talkStoryUI(CSVRow talkStoryRow) {
        string msg = talkStoryRow.GetString(talkStoryTab.csContentCn);
        UIContxt.setContext(msg);
        nextID = talkStoryRow.GetInt(talkStoryTab.csNextID);
    }

    public void onNextClick() {
        if (nextID == csFinishEnd) //话题结束
        {
            if (saySelf == false)  //自由讨论
            {
                finishThing();
            }
            else {              //自述阶断
                talkSelf();
            }
        }
        else {
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
        gameDataManager.instance.chaptID++; 
    }


    public void talkSelfStart() {
        //按角色行动顺序，从1-6 分别自述
        roleOrdAct = 1;
        saySelf = true;
        talkSelf();
    }

    public void talkSelf() {
        
        if (roleOrdAct > 6) {
            Debug.Log("allTalkSelf");
            noteMsg.instance.noteUI.msgNoteBottom("下一章节内容制作中");
            return ;
        }

        int tmpRoleID = roleChaptActOrdTab._instance().GetValueFromKey<int, int>
                      (roleChaptActOrdTab.csActOrder, roleOrdAct, roleChaptActOrdTab.csRoleID, 0);
        roleOrdAct++;

        if (tmpRoleID == gameDataManager.instance.roleID)
        {  //玩家自己的
            //doThing(tmpRoleID, -1);
            talkSelf();  //暂时当作玩家自己也是NPC已说
        }
        else {
            doThing(tmpRoleID, -1);
        }

    }


}
