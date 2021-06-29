using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMsg : MonoBehaviour
{
    private static DebugMsg _instance;

    public static DebugMsg instance {
        get {
            if (_instance == null) {
                _instance = new DebugMsg();
            }
            return _instance;
        }
    }
    public string Msg(Vector3Int pos) {
        string msg = "";
        msg = string.Format("x=%d y=%d, z=%d",pos.x, pos.y, pos.z);
        
        return msg;
    }
}
