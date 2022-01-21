using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DevionGames.UIWidgets;

public class UIMenuMgr : MonoBehaviour
{
    public Transform nextLevelPanel = null;
    public Transform panelDev = null;
    public Transform rewardLab = null;
    public Transform roleList = null;

    public Text killMonster;
    public Text levNum;

    public Image imgAttack1;
    public Image imgAttack2;
    public Image Def;
    public Image Run;

    public Notification noteMsg;
    public Notification noteMsgTop;
    public DialogBox m_DialogBox;

    public Transform canvasHP;

    private float attack1Amount = 0.0f;
    private float attack2Amount = 0.0f;

    public void updateReward(int num) {
        Text tmpTxt = rewardLab.gameObject.GetComponent<Text>();
        tmpTxt.text = num.ToString();
    }

    public void updateKillNum(int now, int allNum) {
        killMonster.text = now.ToString() + "/" + allNum.ToString();
    }

    public void updateLevNum(int num) {
        levNum.text = "第" + num.ToString() + "关";
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
