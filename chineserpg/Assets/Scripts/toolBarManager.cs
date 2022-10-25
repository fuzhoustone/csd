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
                _instance.init();
            }

            return _instance;
        }
    }



   // private GameObject toolBarManagerObj;
    private GameObject topBarObj;
    public topToolBarUI topBar;

    private string csTopBarPre = "Prefabs/UI/topToolBar";

    private void init() {
    //    toolBarManagerObj = new GameObject();
    //    toolBarManagerObj.name = "toolBarManObj";
        UnityEngine.Object tmpObj = Resources.Load(csTopBarPre);
        GameObject topbarObj = GameObject.Instantiate(tmpObj) as GameObject;
        topBar = topbarObj.GetComponent<topToolBarUI>();

        MonoBehaviour.DontDestroyOnLoad(topbarObj);
   //     MonoBehaviour.DontDestroyOnLoad(toolBarManagerObj);
    }

    public void getTopBar() {
        
    }

}
