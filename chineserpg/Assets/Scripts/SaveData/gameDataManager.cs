﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameDataManager
{
    private static gameDataManager _instance = null;
    public static gameDataManager instance
    {
        get {
            if (_instance == null) {
                _instance = new gameDataManager();
            }
            return _instance;
        }
    }

    public int roleID { get; set; }
    public int chaptID { get; set; }
    public int storyID { get; set; }

    public bool isShowRoleInfoBtn() {
        bool res = false;
        if (roleAIManager.instance.getTalkState() == roleAIManager.talkState.storyShow)
        {
            res = false;
        }
        else
            res = true;
        
        return res;
    }

    public bool isShowTalkBtn() {
        bool res = false;
        if (roleAIManager.instance.getTalkState() == roleAIManager.talkState.talkPlayerFreedom)
        {
            res = true;
        }

        return res;
    }

    

}
