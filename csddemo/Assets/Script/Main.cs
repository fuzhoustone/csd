/*

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


//初始化创建角色，初始化UI，以及update
public class Main : MonoBehaviour {

    // 摄像机位置
    public Transform cameraTransform;

    public Transform mapCamerTransform;

    public Transform monParentTransform;
    //UI摄像机
    public Transform uiCammeraTransform;

    //UI画布
    public Canvas roleCanvas;

    //开发人员列表UI
    //private GameObject developerUI;

    // 游戏管理器，场景管理器
    //public GameManager gameManager;
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

    private void initRoleLst(bool[] tmpLst, int defVal) {
        for (int i = 0; i < tmpLst.Length; i++) {
            tmpLst[i] = false;
        }
        tmpLst[defVal] = true;
    }

    private void initData() {
        /*
        weapon_list[DEFAULT_WEAPON] = true;
        head_list[DEFAULT_HEAD] = true;
        chest_list[DEFAULT_CHEST] = true;
        hand_list[DEFAULT_HAND] = true;
        feet_list[DEFAULT_FEET] = true;
        */
        initRoleLst(weapon_list, DEFAULT_WEAPON);
        initRoleLst(head_list, DEFAULT_HEAD);
        initRoleLst(chest_list, DEFAULT_CHEST);
        initRoleLst(hand_list, DEFAULT_HAND);
        initRoleLst(feet_list, DEFAULT_FEET);
        isInit = true;
    }

    public void roleClear() {
        if (character != null) {
            App.Game.CharacterMgr.RemoveChar();
            character = null;
        }
    }

    public void roleShow() {
        if (character != null)
        {
            character.roleInstance.SetActive(true);
            character.roleResume();
        }
    }
    /*
    public void clearScene3D() {
        character.clearSceneAlpha(); 
        roleHide();

        Generator3D gen3D = this.gameObject.GetComponent<Generator3D>();
        gen3D.clearAllMonster();
        gen3D.clearAllMaze();
        gen3D.clearAllHpUI();
    }
    */
    public void showScene() {
        roleShow();
    }

    public void clearRole() {
        // GameObject.Destroy(character);
        character = null;
    }
   

    //创建主角
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

        addRoleData(character.roleInstance);
       // App.Game.gameManager = this.gameManager;
        // App.Game.character = this.character;

        character.roleInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        character.initData(cameraTransform, character.roleInstance.transform, pPos, roleCanvas, mapCamerTransform, character.roleFlagInstance.transform, monParentTransform);

        isStart = true;

        //   App.Game.character.rolePosCamer.setCameraAndTrans(cameraTransform, character.roleInstance.transform);

        //App.Game.character.roleInstance = App.Game.character.roleChangeColorWeaponMgr.roleInstance;
        //App.Game.CharacterMgr

        // AvaterAnimationController tmpAnimaCon = character.roleInstance.GetComponent<AvaterAnimationController>();
        // tmpAnimaCon.setCamera(cameraTransform, gameManager);
    }

    //UI适配
    public void screenAdapt() {
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

    // Use this for initialization
    void Start() {
        screenAdapt();  


        // Physics2D.SetLayerCollisionMask(LayerMask.NameToLayer("weapon"), LayerMask.GetMask("weapon", "attackBox"));
        //这种方法。第一个参数是带设置的Layer，第二个参数是可以与该Layer发生碰撞的Mask.运行一下，我们就看到碰撞矩阵发生了变化。
        initData();
        //update by csd
#if DebugRole
        createRole(new Vector3(5, 0.005f, -5));
#endif
        //isStart = true;
        //update end
    }

    // Update is called once per frame
    void Update() {
        if (isStart) {
            App.Game.Update();
        }

    }

    public void resetCamerClick() {
        App.Game.character.rolePosCamer.resetCamerPosFromRole();
    }

    //角色防御
    public void roleDef() {
        App.Game.character.roleDef();
    }

    //角色攻击1
    public void roleAttack1() {
        App.Game.character.roleAttack1();
    }
    
    //角色攻击2
    public void roleAttack2() {
        App.Game.character.roleAttack2();
    }

    public void showDevelopUI()
    {
       // if (developerUI != null) {
            CsdUIControlMgr.uiMgr().uiMenu.panelDev.gameObject.SetActive(true);
      //  }
    }
    public void HideDevlopUI() {
        Debug.Log("HideDevlopUI");
        //if (developerUI != null)
        //{
            CsdUIControlMgr.uiMgr().uiMenu.panelDev.gameObject.SetActive(false);
        //}
    }

    /// <summary>
    /// 添加主角属性
    /// </summary>
    /// <param name="obj"></param>
    private const int csLayerRole = 11;
    private void addRoleData(GameObject obj) {
        setRoleTagLayer(obj);
        addRigidbody(obj);
       // addMonsterColliderCode(obj);
       // addAniControl(obj);

        obj.AddComponent<roleCollider>();
        obj.AddComponent<modelAnimatorControl>();

        addRoleProperty(obj);
        obj.AddComponent<roleAI>();
    }

    private void setRoleTagLayer(GameObject obj)
    {
        obj.tag = "Role";
        obj.layer = csLayerRole;
    }

    private GameObject getHpPoint(Transform parent)
    {
        GameObject res = parent.gameObject;
        int nCount = parent.childCount;
        for (int i = 0; i < nCount; i++)
        {
            Transform tmp = parent.GetChild(i);
            if (tmp.name == "HpPoint")
            {
                res = tmp.gameObject;
                break;
            }
        }

        return res;
    }

    private void addRoleProperty(GameObject obj)
    {
        roleProperty pro = obj.AddComponent<roleProperty>();
        pro.roleSort = 1;
        pro.hpMax = 100;
      //  pro.mpMax = 100;
        pro.hp = pro.hpMax;
     //   pro.mp = pro.mpMax;
        pro.attack = 1;
        pro.level = 1;
        pro.speed = 0.5f;
        pro.turnTime = 0.0f;
        pro.HpUIPoint = getHpPoint(obj.transform);
    }

    private void addRigidbody(GameObject obj)
    {
        Rigidbody rd = obj.AddComponent<Rigidbody>();
        rd.mass = 1.0f;
        rd.drag = 0.0f;
        rd.angularDrag = 0.5f;
        rd.useGravity = true;
        rd.isKinematic = false;
        rd.interpolation = RigidbodyInterpolation.None;
        rd.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rd.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// UI调试
    /// </summary>
    /// <param name="obj"></param>

#if DEBUG_role

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

    }

#endif

}

