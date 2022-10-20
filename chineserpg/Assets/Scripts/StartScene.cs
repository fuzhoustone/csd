﻿using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private Transform uiCammeraTransform;

    [SerializeField]
    private Button continueBtn;

//    [SerializeField]
//    private GameObject helpObj;

    [SerializeField]
    private DialogBox m_DialogBox;

    [SerializeField]
    private selRoleDialogUI selRoleDialog;

    private UnityAction btnEvent1, btnEvent2, btnEvent3; //分别对应，1个，2个，3个按扭
    private bool hasRecord;
    private const string title = "请确认";
    private const string text = "新游戏将清空现有游戏记录，是否清空并开始新游戏";
/*
    [SerializeField]
    private Text startTxt;
    [SerializeField]
    private Text continueTxt;
    [SerializeField]
    private Text shopTxt;
    [SerializeField]
    private Text helpTxt;
    [SerializeField]
    private Text exitTxt;
    [SerializeField]
    private Text gameTxt;
*/
    /*
    private void steamShow() {
        startTxt.text = "start";
        continueTxt.text = "continue";
        shopTxt.text = "shop";
        helpTxt.text = "control";
        exitTxt.text = "exit";

        gameTxt.text = "monster maze";
    }
    */
    public void screenAdapt()
    {
        //Debug.Log("screenAdapt");
        int ManualWidth = 960;
        int ManualHeight = 640;
        float designHeight = 640.0f;
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        if (uiCammeraTransform != null)
        {
            Camera camera = uiCammeraTransform.GetComponent<Camera>();
            float scale = System.Convert.ToSingle(manualHeight / designHeight);
            camera.fieldOfView *= scale;
        }

    }

    void Start()
    {


    }

    //切换到游戏场景
    private void changeGameScene(int roleID) {
        SceneManager.LoadSceneAsync("storyScene");
    }

    /// <summary>
    /// UI按扭
    /// </summary>
    public void startNewGame() {
        /*
                if (helpObj.activeSelf == true)
                {
                    helpObj.SetActive(false);
                }
        */
        selRoleDialog.showDialog(changeGameScene);

          //切换场景
        //changeGameScene();
       
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

    /*
    private void clearAndNewGame() {
        changeGameScene(); //切换场景开始游戏
    }
    */
    private void closeDialog() {
        m_DialogBox.Close();
    }

    public void continueGame()
    {
      //  changeGameScene();
    }

    /*
    public void helpShow() {

        helpObj.SetActive(true);
    }
    */

    public void showDevelopers() { 
    
    }

    public void setTing() { 
    
    }

    public void setLanguage() { 
    
    }

    public void loadGame() { 
        
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
