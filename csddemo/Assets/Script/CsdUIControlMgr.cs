using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    private UnityAction btnEvent1, btnEvent2, btnEvent3; //分别对应，1个，2个，3个按扭
    public void initData(UIMenuMgr param)
    {
        uiMenu = param;
    }

    public void msgNote(string msg) {
        uiMenu.noteMsg.AddItem(msg);
    }


    public void msgNoteTop(string msg = "进入战斗停止移动") {
        uiMenu.noteMsgTop.AddItem(msg);
    }

    public void dialogBox(string title, string text, Sprite icon, string[] buttons,
                          UnityAction pBtnEvent1, UnityAction pBtnEvent2, UnityAction pBtnEvent3)
    {
        btnEvent1 = pBtnEvent1;
        btnEvent2 = pBtnEvent2;
        btnEvent3 = pBtnEvent3;
        uiMenu.m_DialogBox.Show(title, text, icon, boxResult, buttons);
    }

    private void boxResult(int param) {
        if (param == 0)
        {
            if (btnEvent1 != null) {
                btnEvent1();
            }
        }
        else if (param == 1)
        {
            if (btnEvent2 != null)
            {
                btnEvent2();
            }
        }
        else if (param == 2)
        {
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
        else {
            Debug.LogWarning("boxResult error");
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
    }

}
