

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
                _instance = new sceneName();
            return _instance;
        }
    }

    private string oldSceneName;

    private void setSceneActive(Scene tmpSce,bool pActive) {
        GameObject[] objLst = tmpSce.GetRootGameObjects();
        
         for (int i = 0; i <objLst.Length; i++) {
            objLst[i].SetActive(pActive);
         }

    }


    public void changeSceneSingle(string sceName) {
        SceneManager.LoadScene(sceName, LoadSceneMode.Single);
    }

    public void changeScene(string sceName) {

        Scene nowScene = SceneManager.GetActiveScene();
        if (nowScene.isLoaded)
        {
            setSceneActive(nowScene, false);
         //   if(saveOldSceName)
         //       oldSceneName = nowScene.name;
        }

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
/*
    public void changePrevScene(bool saveOldSceneName) {
        //if(prevSceneName.Equals("") == false)
        //    changeScene(prevSceneName, false);
        changeScene(oldSceneName, saveOldSceneName);
    }
*/
}
