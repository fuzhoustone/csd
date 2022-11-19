using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canAdvapt 
{
    private static canAdvapt _instance = null;
    public static canAdvapt instance {
        get {
            if (_instance == null) {
                _instance = new canAdvapt();
                _instance.initParam();
            }
            return _instance;
        }
    }

    public float bgMatchWidHeight { get; set; }
    public float uiMatchWidHeight { get; set; }

    public const float cfWidth = 1920.0f;
    public const float cfHeight = 1080.0f;
    private void initParam() {
        float nowWidHei = System.Convert.ToSingle(Screen.width) / Screen.height; //1.6
        float setWidHei = cfWidth / cfHeight ;  //1.77777...
        if (nowWidHei < setWidHei)
        {
            bgMatchWidHeight = 1.0f;
            uiMatchWidHeight = 0.0f;
        }
        else {
            bgMatchWidHeight = 0.0f;
            uiMatchWidHeight = 1.0f;
        }
        
    }
}
