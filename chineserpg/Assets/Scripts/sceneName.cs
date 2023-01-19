

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneName
{
    public const string csStartScene = "startScene";
    public const string csStoryScene = "storyScene";
    public const string csTalkScene = "talkScene";

    private static sceneName _instance = null;
    public static sceneName instance
    {
        get {
            if (_instance == null)
            {
                _instance = new sceneName();
                _instance.initParam();
            }
            return _instance;
        }
    }

    private string oldSceneName;
    public Action<int> pSceneCloseAction;
    private void initParam() {
        oldSceneName = "";
        pSceneCloseAction = null;
    }

    private void setSceneActive(Scene tmpSce,bool pActive) {
        GameObject[] objLst = tmpSce.GetRootGameObjects();
        
         for (int i = 0; i <objLst.Length; i++) {
            objLst[i].SetActive(pActive);
         }

    }


    public void changeSceneSingle(string sceName) {
        oldSceneName = sceName;
        SceneManager.LoadScene(sceName, LoadSceneMode.Single);
    }

    public void setSceneChangeAction(Action<int> lSceneAct = null) {
        pSceneCloseAction = lSceneAct;
    }

    public void changeScene(string sceName) {

        if (oldSceneName != "") {
            Scene nowScene = SceneManager.GetSceneByName(oldSceneName);
            if (nowScene.isLoaded)
            {
                setSceneActive(nowScene, false);
            }
        }

        oldSceneName = sceName;
        Scene nextScene = SceneManager.GetSceneByName(sceName);
        bool sceneIsValid = nextScene.isLoaded;
        if (sceneIsValid == false) //不存在此场景
        {
            SceneManager.LoadScene(sceName, LoadSceneMode.Additive);
        }
        else {
            setSceneActive(nextScene,true);
        }

    }
}
