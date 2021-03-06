﻿/*

    It is a Unity project that display how to build the avatar equipment system in Unity.
    Equipment system is very important in the Game, specially in MMO Game.

    Normally, equipment system contains tow important parts. 
    Since the appearance of equipments are different(the mesh are different), so to merge these meshes together is necessary. 
    Second, after merge meshes, the new mesh contains many materials(in this project, it has 4 material), that means it has at least 4 drawcalls(depends in the shader).
    So to merge materials together will reduce drawcalls and improve game performance.

    Auther: ZouChunyi
    E-mail: zmafly@163.com

*/

using UnityEngine;
using System.Collections;
//using UCharacterMgr;

/// <summary>
/// A simple framework of the game.
/// </summary>


//初始化创建场景，初始化创建角色，初始化UI，以及update
public class Main : MonoBehaviour {

    // 摄像机位置
    public Transform cameraTransform;

    //UI摄像机
    public Transform uiCammeraTransform;

    //UI画布
    public Canvas roleCanvas;

    // 游戏管理器，场景管理器
    public GameManager gameManager;
    private bool isInit = false;
    private bool isStart = false;

    //总的角色控制类，控制角色换肤，动作，
    public UCharacterController character = null;

    private readonly string[] index = new string[] { "004", "006", "008" };
    /// <summary>
    /// Config default equipment informations.
    /// </summary>
	private const int DEFAULT_WEAPON = 0;
    private const int DEFAULT_HEAD = 0;//2;
    private const int DEFAULT_CHEST = 0;
    private const int DEFAULT_HAND = 0;
    private const int DEFAULT_FEET = 0;//1;
    private const bool DEFAULT_COMBINEMATERIAL = true;

    /// <summary>
    /// Use this for GUI display.
    /// </summary>
    public bool combine = DEFAULT_COMBINEMATERIAL;
    private bool[] weapon_list = new bool[3];
    private bool[] head_list = new bool[3];
    private bool[] chest_list = new bool[3];
    private bool[] hand_list = new bool[3];
    private bool[] feet_list = new bool[3];

    /// <summary>
    /// The avatar in the scene.
    /// </summary>

    public bool getIsInit() {
        return isInit;
    }

    private void initData() {
        weapon_list[DEFAULT_WEAPON] = true;
        head_list[DEFAULT_HEAD] = true;
        chest_list[DEFAULT_CHEST] = true;
        hand_list[DEFAULT_HAND] = true;
        feet_list[DEFAULT_FEET] = true;
        isInit = true;
    }

    public void createRole(Vector3 pPos) {
        // create an avatar
        character = App.Game.CharacterMgr.Generatecharacter(
            "ch_pc_hou",
            "ch_we_one_hou_" + index[DEFAULT_WEAPON],
            "ch_pc_hou_" + index[DEFAULT_HEAD] + "_tou",
            "ch_pc_hou_" + index[DEFAULT_CHEST] + "_shen",
            "ch_pc_hou_" + index[DEFAULT_HAND] + "_shou",
            "ch_pc_hou_" + index[DEFAULT_FEET] + "_jiao",
            combine);

        App.Game.gameManager = this.gameManager;
        // App.Game.character = this.character;

        character.roleInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        character.initData(cameraTransform, character.roleInstance.transform, pPos, roleCanvas);

        isStart = true;

        //   App.Game.character.rolePosCamer.setCameraAndTrans(cameraTransform, character.roleInstance.transform);

        //App.Game.character.roleInstance = App.Game.character.roleChangeColorWeaponMgr.roleInstance;
        //App.Game.CharacterMgr

        // AvaterAnimationController tmpAnimaCon = character.roleInstance.GetComponent<AvaterAnimationController>();
        // tmpAnimaCon.setCamera(cameraTransform, gameManager);
    }

    private void screenAdapt() {

        int ManualWidth = 960;
        int ManualHeight = 640;
        int manualHeight;
        if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
            manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        else
            manualHeight = ManualHeight;
        Camera camera = uiCammeraTransform.GetComponent<Camera>();
        float scale = System.Convert.ToSingle(manualHeight / 640f);
        camera.fieldOfView *= scale;

    }

    // Use this for initialization
    void Start() {
        screenAdapt();
        // Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("weapon"), LayerMask.GetMask("weapon", "attackBox"));
        //这种方法。第一个参数是带设置的Layer，第二个参数是可以与该Layer发生碰撞的Mask.运行一下，我们就看到碰撞矩阵发生了变化。
        initData();
        //update by csd
        //createRole(new Vector3(5, 0.005f, -5));
        //isStart = true;
        //update end
    }



    // Update is called once per frame
    void Update() {
        if (isStart) {
            App.Game.Update();
        }

    }

    private void initRole() {
        Generator3D tmpMaze = this.gameObject.transform.GetComponent<Generator3D>();
        if (tmpMaze != null)
            createRole(tmpMaze.firstPos);
        else
            createRole(new Vector3(5, 0.005f, -5));
    }

	void OnGUI () {

        float btnWidth = 100.0f;
        float btnHeight = 100.0f;
        float btnPosY = btnHeight + 5.0f;


        GUI.Button (new Rect (0, 0, btnWidth, btnHeight),   "Euipments 1");
		GUI.Button (new Rect (btnWidth, 0, btnWidth, btnHeight), "Euipments 2");
		GUI.Button (new Rect (btnWidth*2, 0, btnWidth, btnHeight), "Euipments 3");
        

		for (int i = 0; i < weapon_list.Length; i++) {
			
			if (GUI.Button (new Rect (i * btnWidth, btnPosY, btnWidth, btnHeight), "武器" + (weapon_list[i] ? "(√)" : ""))) {
				
				if (!weapon_list [i]) {
					for (int j = 0; j < weapon_list.Length; j++) {
						weapon_list [j] = false;
					}
					weapon_list [i] = true;
					
					character.ChangeWeapon ("ch_we_one_hou_" + index[i]);
				}
			}
		}
       
		for (int i = 0; i < head_list.Length; i++) {

			if (GUI.Button (new Rect (i * btnWidth, btnPosY*2, btnWidth, btnHeight), "头" + (head_list[i] ? "(√)" : ""))) {

				if (!head_list [i]) {
					for (int j = 0; j < head_list.Length; j++) {
						head_list [j] = false;
					}
					head_list [i] = true;

					character.ChangeHeadEquipment ("ch_pc_hou_" + index[i] + "_tou", combine);
				}
			}
		}

		for (int i = 0; i < chest_list.Length; i++) {

			if (GUI.Button (new Rect (i * btnWidth, btnPosY*3, btnWidth, btnHeight), "身体" + (chest_list[i] ? "(√)" : ""))) {

				if (!chest_list [i]) {
					for (int j = 0; j < chest_list.Length; j++) {
						chest_list [j] = false;
					}
					chest_list [i] = true;

					character.ChangeChestEquipment ("ch_pc_hou_" + index[i] + "_shen", combine);
				}
			}
		}

		for (int i = 0; i < hand_list.Length; i++) {

			if (GUI.Button (new Rect (i * btnWidth, btnPosY*4, btnWidth, btnHeight), "手" + (hand_list[i] ? "(√)" : ""))) {

				if (!hand_list [i]) {
					for (int j = 0; j < hand_list.Length; j++) {
						hand_list [j] = false;
					}
					hand_list [i] = true;

					character.ChangeHandEquipment("ch_pc_hou_" + index[i] + "_shou", combine);
				}
			}
		}

		for (int i = 0; i < feet_list.Length; i++) {

			if (GUI.Button (new Rect (i * btnWidth, btnPosY*5, btnWidth, btnHeight), "腿" + (feet_list[i] ? "(√)" : ""))) {

				if (!feet_list [i]) {
					for (int j = 0; j < feet_list.Length; j++) {
						feet_list [j] = false;
					}
					feet_list [i] = true;

					character.ChangeFeetEquipment("ch_pc_hou_" + index[i] + "_jiao", combine);
				}
			}
		}


        if (GUI.Button(new Rect(Screen.width - btnWidth, 0, btnWidth, btnHeight), "getState"))
        {
            App.Game.character.mainRoleState.printRoleState();
        }

        if (GUI.Button(new Rect(Screen.width - btnWidth, btnPosY, btnWidth, btnHeight), "镜头复原"))
        {
            App.Game.character.rolePosCamer.resetCamerPosFromRole();
          
          
        }
        
        if (GUI.Button(new Rect(Screen.width - btnWidth, btnPosY*2, btnWidth, btnHeight), "Escape"))
        {
            /*
            Generator3D tmpMaze = this.gameObject.transform.GetComponent<Generator3D>();
            if (tmpMaze != null)
                createRole(tmpMaze.firstPos);
            else
                createRole(new Vector3(5, 0.005f, -5));
                */

            //App.Game.character.rolePosCamer.test();
            App.Game.character.setRoleInEscape();
            StartCoroutine(escapeIEn());
        }

        if (GUI.Button(new Rect(Screen.width - btnWidth, btnPosY * 3, btnWidth, btnHeight), "rolationCamerY"))
        {
            // App.Game.character.test();
            App.Game.character.rolePosCamer.rolationCamerY(90.0f, 1.0f/40.0f);
            //App.Game.character.rolePosCamer.test(0, 3.0f);
        }


    }

    const float fEscapeTime = 5.0f;
    IEnumerator escapeIEn()
    {
        float time = 0;

        //float fadeLength = 5.0f;
        while (time < fEscapeTime) // 还需另外设置跳出循环的条件
        {
            time += Time.deltaTime;
            yield return null;
        }

        App.Game.character.setRoleNotEscape();
    }

}

