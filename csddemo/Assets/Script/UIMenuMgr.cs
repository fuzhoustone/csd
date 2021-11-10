﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuMgr : MonoBehaviour
{
    public Transform nextLevelPanel = null;
    public Transform panelDev = null;
    public Transform rewardLab = null;

    public Image imgAttack1;
    public Image imgAttack2;
    public Image Def;
    public Image Run;

    private float attack1Amount = 0.0f;
    private float attack2Amount = 0.0f;

    public void updateReward(int num) {
        Text tmpTxt = rewardLab.gameObject.GetComponent<Text>();
        tmpTxt.text = num.ToString();
    }

    public void updateImageAttack1Amount(float val, float count) {
        updateImageAmount(val,count, imgAttack1, ref attack1Amount);
    }

    public void updateImageAttack2Amount(float val, float count)
    {
        updateImageAmount(val, count, imgAttack2, ref attack2Amount);
    }

    private void updateImageAmount(float val, float count, Image img, ref float oldAmount) {
        float tmpAmount = val / count;
        float tmp = Math.Abs(tmpAmount - oldAmount);
        if ((val == 0.0f)||(val >= count) ||(tmp > 0.02)) {
            oldAmount = tmpAmount;
            img.fillAmount = tmpAmount;
        }
    }


}