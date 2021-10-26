using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuMgr : MonoBehaviour
{
    public Transform nextLevelPanel = null;
    public Transform panelDev = null;
    public Transform rewardLab = null;



    public void updateReward(int num) {
        Text tmpTxt = rewardLab.gameObject.GetComponent<Text>();
        tmpTxt.text = num.ToString();
    }
}
