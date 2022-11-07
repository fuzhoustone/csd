using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DevionGames.UIWidgets;

public class noteMsg { 
    private static noteMsg _instance = null;
    public static noteMsg instance
    {
        get {
            if (_instance == null) {
                _instance = new noteMsg();
                _instance.initParam();
            }
            return _instance;
        }
    }

    private string csNotePre = "Prefabs/UI/notificationManager";
    public notificaitonManagerUI noteUI = null;
    private void initParam() {
        UnityEngine.Object tmpObj = Resources.Load(csNotePre);
        GameObject tmpGameObj = GameObject.Instantiate(tmpObj) as GameObject;
        noteUI = tmpGameObj.GetComponent<notificaitonManagerUI>();
        MonoBehaviour.DontDestroyOnLoad(tmpGameObj);
    }
}

public class notificaitonManagerUI : MonoBehaviour
{
    public Notification noteBottom;
    public void msgNoteBottom(string msg)
    {
        noteBottom.AddItem(msg);
    }

}
