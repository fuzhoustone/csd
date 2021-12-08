using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startScene : MonoBehaviour
{
    public Button continueBtn;
    public GameObject helpObj;
    public GameObject bossList;
    public DialogBox m_DialogBox;
    private UnityAction btnEvent1, btnEvent2, btnEvent3; //分别对应，1个，2个，3个按扭
    private bool hasRecord;
    private const string title = "请确认";
    private const string text = "新游戏将清空现有游戏记录，是否清空并开始新游戏";
    void Start()
    {
      
        hasRecord = hasGameRecord();
        if (hasRecord == true)
        {
            continueBtn.interactable = true;
            continueBtn.enabled = true;
        }
        else
        {
            continueBtn.interactable = false;
            continueBtn.enabled = false;
        }

        helpObj.SetActive(false);
        bossList.SetActive(false);
        //if(gameDialog.)
    }

    private bool hasGameRecord() {
        bool result = gameDataMgr.gameData().hasRecord();
        return result;
    }

    //切换到游戏场景
    private void changeGameScene() {
        SceneManager.LoadSceneAsync("mainScene");
    }

    /// <summary>
    /// UI按扭
    /// </summary>
    public void startNewGame() {
        if (bossList.activeSelf == true) {
            bossList.SetActive(false);
        }

        if (helpObj.activeSelf == true)
        {
            helpObj.SetActive(false);
        }

        if (hasRecord)
        {
            string[] buttons = new string[2];
            buttons[0] = "确定";
            buttons[1] = "取消";
            dialogBox(title, text, null, buttons, clearAndNewGame, closeDialog, null);
        }
        else { //切换场景
            changeGameScene();
        }
    }

    
    private void dialogBox(string title, string text, Sprite icon, string[] buttons,
                          UnityAction pBtnEvent1, UnityAction pBtnEvent2, UnityAction pBtnEvent3)
    {
        btnEvent1 = pBtnEvent1;
        btnEvent2 = pBtnEvent2;
        btnEvent3 = pBtnEvent3;
        m_DialogBox.Show(title, text, icon, boxResult, buttons);
    }

    private void boxResult(int param)
    {
        if (param == 0)
        {
            if (btnEvent1 != null)
            {
                btnEvent1();
            }
        }
        else if (param == 1)
        {
            if (btnEvent2 != null)
            {
                btnEvent2();
            }
        }
        else if (param == 2)
        {
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
        else
        {
            Debug.LogWarning("startSceneboxResult error");
            if (btnEvent3 != null)
            {
                btnEvent3();
            }
        }
    }

    private void clearAndNewGame() {
        gameDataMgr.gameData().clearLevelData();  //清空通关记录
        changeGameScene(); //切换场景开始游戏
    }

    private void closeDialog() {
        m_DialogBox.Close();
    }

    public void continueGame()
    {
        changeGameScene();
    }

    //图签
    public void shopList()
    {
        //Time.timeScale = 0;
        if (helpObj.activeSelf == true)
        {
            helpObj.SetActive(false);
        }

        if (m_DialogBox.enabled) {
            m_DialogBox.Close();
        }

        bossList.GetComponent<bossListUI>().showUI();
        //bossList.SetActive(true);
    }

    public void helpShow() {
        //Time.timeScale = 0;
        if (bossList.activeSelf == true)
        {
            bossList.SetActive(false);
        }

        helpObj.SetActive(true);
    }

    public void exitGame()
    {
#if UNITY_EDITOR
       
            UnityEditor.EditorApplication.isPlaying = false;
    #else
         Application.Quit();
    #endif
    }

}
