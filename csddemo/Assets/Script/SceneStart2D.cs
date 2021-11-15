using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏开始
public class SceneStart2D : MonoBehaviour
{
    private bool isInit = false;
    private bool sceneIsFinish = false;
    private bool isSceneStart = false;

    private mainScene sceneMaze;
    private Main roleMain;
    // Start is called before the first frame update
    void Start()
    {
        //SceneStart();
    }

    public void SceneStart() {
        mainScene tmpScene = this.transform.GetComponent<mainScene>();
        tmpScene.createLandScape();


        sceneIsFinish = false;
    }

    // Update is called once per frame
    void Update()
    {
       // if (isSceneStart)
       // {
       //     return;
       // }

        if (isInit == false)
        {
            sceneMaze = this.gameObject.transform.GetComponent<mainScene>();
            roleMain = this.gameObject.transform.GetComponent<Main>();
            isInit = true;
        }

        if (sceneIsFinish == false)
        {
            if ((sceneMaze.getIsInit()) && (roleMain.getIsInit()))
            {
                roleMain.createRole(sceneMaze.firstPos);

                sceneMaze.friendRole.GetComponent<followRole>().mainObj = roleMain.character.roleInstance; //设置小弟跟随目标

                //sceneAlphaControl sceneAlpha = Camera.main.GetComponent<sceneAlphaControl>();
                //sceneAlpha._target = roleMain.character.roleInstance;

                sceneIsFinish = true;
            }
        }

    }

}
