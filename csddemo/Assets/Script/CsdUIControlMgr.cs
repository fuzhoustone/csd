using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsdUIControlMgr
{
    private static CsdUIControlMgr instance = null;
    public static CsdUIControlMgr uiMgr()
    {
        if (instance == null)
        {
            instance = new CsdUIControlMgr();
        }
        return instance;
    }

    public UIMenuMgr uiMenu;
    public void initData(UIMenuMgr param)
    {
        uiMenu = param;
    }

}
