using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageMgr
{ 
    private static stageMgr instance = null;
    public static stageMgr stage() {
        if (instance == null)
        {
            instance = new stageMgr();
            instance.initData();
        }
        return instance;

    }
    public void initData()
    {
        level = 0;
        finishEvent = null;
    }

    public int level;
    public int rewardNum;

    //public delegate bool isRoomFunc(Vector3Int pos, out placeWall pRoom);
    //public delegate bool finshCallback(int num);
    Action<int> finishEvent;
    private int monsterLimitMin;  //最早消灭的怪物数
    private int nowNum;           //当前累计数量

    //最少消灭的怪物数
    public void initStage(int lLevel, int monsterNum, Action<int> callEvent) {
        level = lLevel;
        monsterLimitMin = monsterNum;
        finishEvent = callEvent;

        rewardNum = 0;
        CsdUIControlMgr.uiMgr().uiMenu.updateReward(rewardNum);
        nowNum = 0;
    }

    public void addClearMonster() {
        nowNum++;
        if (nowNum >= monsterLimitMin) {  //符合通关条件
            if (finishEvent != null) {
                finishEvent(rewardNum);
            }
        }
    }

    public void roleLose() {
        Debug.LogError("主角挂了");
    }

    public void addReward() {
        rewardNum++;
        CsdUIControlMgr.uiMgr().uiMenu.updateReward(rewardNum);
    }
}
