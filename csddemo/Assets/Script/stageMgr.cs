using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        CsdUIControlMgr.uiMgr().uiMenu.updateKillNum(nowNum, monsterLimitMin);
    }

    public void addClearMonster() {
        nowNum++;
        CsdUIControlMgr.uiMgr().uiMenu.updateKillNum(nowNum, monsterLimitMin);
        if (nowNum >= monsterLimitMin) {  //符合通关条件
            if (finishEvent != null) {
                finishEvent(rewardNum);
            }
        }
    }

    private void continueFight() {
        Debug.LogWarning("主角复活");
    }

    private void callBackMain() {
        Debug.LogWarning("返回主菜单");

        gameDataMgr.gameData().clearLevelData(); //清空通关记录

        SceneManager.LoadSceneAsync("startScene");
        
    }

    public void roleLose() {
        Debug.LogWarning("主角挂了");
        string title = "战斗失败";
        string text = "战斗失败，复活继续战斗，或返回主菜单";
        string[] buttons = new string[2];
        buttons[0] = "复活";
        buttons[1] = "返回主菜单";

        CsdUIControlMgr.uiMgr().dialogBox(title,text,null, buttons, continueFight, callBackMain, null);
    }



    public void addReward() {
        rewardNum++;
        CsdUIControlMgr.uiMgr().uiMenu.updateReward(rewardNum);
    }
}
