using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class selRoleDialogUI : MonoBehaviour
{
    private UnityAction<int> callEvent;
    private int roleId;
    public void showDialog(UnityAction<int> pEvent) {
        callEvent = pEvent;
        this.gameObject.SetActive(true);
    }

    public void closeDialog() {
        this.gameObject.SetActive(false);
    }

    public void OKclick() {
        this.gameObject.SetActive(false);

        if (callEvent != null)
            callEvent(roleId);
        
    }


}
