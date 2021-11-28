using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startScene : MonoBehaviour
{
    public Button continueBtn;
    public GameObject helpObj;
    private bool hasRecord;
    void Start()
    {
        // continueBtn.GetComponent<Button>();
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
        
    }

    //待实现
    private bool hasGameRecord() {
        bool result = false;
        return result;
    }

    //切换到游戏场景
    private void changeGameScene() {

    }

    /// <summary>
    /// UI按扭
    /// </summary>
    public void startNewGame() {
        if (hasRecord)
        {

        }
        else { //切换场景
            changeGameScene();
        }
    }

    public void continueGame()
    {
        changeGameScene();
    }

    //图签
    public void achieveList()
    {

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
