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

    [SerializeField]
    private Transform rewardLab = null;

    public Transform roleList = null;

    [SerializeField]
    private Text killMonster;
    [SerializeField]
    private Text levNum;

    [SerializeField]
    private Image imgAttack1;
    [SerializeField]
    private Image imgAttack2;
    [SerializeField]
    private Image Def;
    [SerializeField]
    private Image Run;
    

    public Notification noteMsg;
    public Notification noteMsgTop;
    public DialogBox m_DialogBox;

    public Transform canvasHP;

    private float attack1Amount = 0.0f;
    private float attack2Amount = 0.0f;


    [SerializeField]
    private Text levTitle;
    [SerializeField]
    private Text monsterTitle;
    [SerializeField]
    private Text rewardTitle;
    [SerializeField]
    private Text attackLab1;
    [SerializeField]
    private Text attackLab2;
    [SerializeField]
    private Text changeLab;
    [SerializeField]
    private Text calmainLab;

    private void steamShow() {
        levTitle.text = "Level:";
        monsterTitle.text = "Kill:";
        rewardTitle.text = "Reward:";
        attackLab1.text = "attack1";
        attackLab2.text = "attack2";
        changeLab.text = "change";
        calmainLab.text = "main\nmenu";
    }

    private void Start()
    {
#if streamScreen
        steamShow();
#endif
    }

    public void updateReward(int num) {
        Text tmpTxt = rewardLab.gameObject.GetComponent<Text>();
        tmpTxt.text = num.ToString();
    }

    public void updateKillNum(int now, int allNum) {
        killMonster.text = now.ToString() + "/" + allNum.ToString();
    }

    public void updateLevNum(int num) {
        levNum.text = num.ToString();

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
