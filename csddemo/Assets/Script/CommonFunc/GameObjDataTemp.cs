using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjDataTemp
{
    private static GameObjDataTemp instance = null;
    public static GameObjDataTemp tempData() {
        if (instance == null)
        {
            instance = new GameObjDataTemp();
            instance.initParam();
        }

        return instance;
    }


    public bool refreshHpUI {
        get;
        set;
    }

    private void initParam() {
        refreshHpUI = true;
    }



}
