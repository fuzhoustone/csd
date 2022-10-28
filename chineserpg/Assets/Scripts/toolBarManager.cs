using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toolBarManager
{
    private static toolBarManager _instance = null;

    public static toolBarManager instance {
        get{
            if (_instance == null)
            {
                _instance = new toolBarManager();
               // _instance.init();
            }

            return _instance;
        }
    }



   // private GameObject toolBarManagerObj;
   // private GameObject topBarObj;
    public topToolBarUI topBar = null;
    private selRoleDialogUI selRoleDlg;

    private string csTopBarPre = "Prefabs/UI/topToolBar";
    private string csSelRoleDlgPre = "Prefabs/UI/selRoleDlg";

    private void init() {
    //    toolBarManagerObj = new GameObject();
    //    toolBarManagerObj.name = "toolBarManObj";
        
   //     MonoBehaviour.DontDestroyOnLoad(toolBarManagerObj);
    }

    public void showTopBar() {
        if (topBar == null)
        {
            UnityEngine.Object tmpObj = Resources.Load(csTopBarPre);
            GameObject topbarObj = GameObject.Instantiate(tmpObj) as GameObject;
            topBar = topbarObj.GetComponent<topToolBarUI>();
            topBar.setChartName(1);
            selRoleDlg = null;
            MonoBehaviour.DontDestroyOnLoad(topbarObj);
        }
        else
            topBar.gameObject.SetActive(true);
    }

    /*
    public void showMission(bool isShow) {
        if (topBar == null)
        {
            UnityEngine.Object tmpObj = Resources.Load(csTopBarPre);
            GameObject topbarObj = GameObject.Instantiate(tmpObj) as GameObject;
            topBar = topbarObj.GetComponent<topToolBarUI>();
            topBar.setChartName(1);
            selRoleDlg = null;
            MonoBehaviour.DontDestroyOnLoad(topbarObj);
        }
        topBar.showMission(isShow);
    }
    */

    public void showSelRoleDlg(UnityEngine.Events.UnityAction<int> pEvent, int lSelfID = -1) {
        if (selRoleDlg == null)
        {
            UnityEngine.Object tmpObj = Resources.Load(csSelRoleDlgPre);
            GameObject selRoleDlgObj = GameObject.Instantiate(tmpObj) as GameObject;
            selRoleDlg = selRoleDlgObj.GetComponent<selRoleDialogUI>();
            MonoBehaviour.DontDestroyOnLoad(selRoleDlgObj);
        }
        
        selRoleDlg.showDialog(pEvent, lSelfID);


    }

}
