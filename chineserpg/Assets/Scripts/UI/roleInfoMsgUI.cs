using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleInfoMsgUI : MonoBehaviour
{
    [SerializeField]
    private Text infoText;

    public void setData(string msgInfo) {
        infoText.text = msgInfo;
    }
    
}
