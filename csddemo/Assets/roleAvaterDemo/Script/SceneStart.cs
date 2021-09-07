using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private bool isInit = false;
    private bool sceneIsFinish = false;
    private bool isSceneStart = false;

    private Generator3D sceneMaze;
    private Main roleMain;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSceneStart) {
            return ;
        }

        if (isInit == false) {
            sceneMaze = this.gameObject.transform.GetComponent<Generator3D>();
            roleMain = this.gameObject.transform.GetComponent<Main>();
        }
        if (sceneIsFinish == false) {
            if ((sceneMaze.getIsInit()) && (roleMain.getIsInit())) {
                roleMain.createRole(sceneMaze.firstPos);

                sceneMaze.friendRole = roleMain.character.roleInstance; //设置小弟跟随目标
                

                sceneAlphaControl sceneAlpha = Camera.main.GetComponent<sceneAlphaControl>();
                sceneAlpha._target = roleMain.character.roleInstance;
                
                sceneIsFinish = true;
            }
        }

        

        

    }
}
