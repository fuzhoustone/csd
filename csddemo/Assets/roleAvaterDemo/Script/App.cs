using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App
{
    private static App app = new App();

    public static App Game { get { return app; } }

    

    private Main main = null;
    private bool isStart = false;
    

    private UCharacterMgr characterMgr = new UCharacterMgr();
    public UCharacterMgr CharacterMgr { get { return characterMgr; } }

   // public GameManager gameManager { set; get; }
    public UCharacterController character { set; get; }


    public void Update()
    {
       // if (gameManager != null) {
         //   if (gameManager.isStart) { //场景也初始化好了
                characterMgr.Update();  //人物池中所有的人物进行update
           // }
        //}
    }
}
