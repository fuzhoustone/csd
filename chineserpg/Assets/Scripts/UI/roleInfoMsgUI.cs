using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleInfoMsgUI : MonoBehaviour
{
    [SerializeField]
    private Text infoText;

    [SerializeField]
    private GameObject isPubObj;

    [SerializeField]
    private GameObject isLookObj;

    private bool isLook, isPub;
    private int clueGetID;
    public void setData(int lID,string msgInfo,bool lIsLook,bool lIsPub) {
        infoText.text = msgInfo;
        isLook = lIsLook;
        isPub = lIsPub;
        clueGetID = lID;
        if (isPub)
            isPubObj.SetActive(false);
        else
            isPubObj.SetActive(true);

        //if (isLook)
            isLookObj.SetActive(isLook);
        //else
        //    isLookObj.SetActive(false);


    }

    public void OnClick() {
        
        if (isLook == false)
        {
            isLook = true;
            isLookObj.SetActive(isLook);

            CSVRow tmpRow = clueLstGetTab._instance().GetRowFromID(clueGetID);
            tmpRow.SetBool(clueLstGetTab.csLook, isLook);

            clueLstGetTab._instance().SaveFile();
        }
    }

}
