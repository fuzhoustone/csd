using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleAIManager 
{
    //章节最大活动时间
    public const int ciChaptMaxTime = 20;
    public int freeTime;
    public int chaptID;

    //各章节活动时间
    public void chaptFreeTimeInit(int lChaptID) {
        chaptID = lChaptID;
        freeTime = ciChaptMaxTime;
        //更新各角色的友好度及敌对值
        //在有敌对的玩家中随机选一名进行行动

    }

    public void chaptExec() {
        //玩家的执行
        //AI的执行
        //每一轮执行，都让玩家选择：不参与，还是参与，至于无事可做
        if (freeTime < ciChaptMaxTime)
        {   //freeTime在玩家行动或AI行动时累加
            isPlayTurn();//等待player的UI交互
            { //play不行动
                if (getThing()) //获得AI的行动
                { 
                    freeTime++;
                    doThing();
                    //chaptExec();  //AI角色对话后执行
                }
            }
        }
        else {
            finishThing();
        }
    }

    public void isPlayTurn() {
        //bool res = false;
        //显示玩家的选择

        //return res;
    }

    public bool getThing() {
        bool res = false;

        return res;
    }

    public void doThing()
    {


    }

    public void finishThing() { 
    
    }

}
