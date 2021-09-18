using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using stoneState;

public class testRoleStateMgr : MonoBehaviour
{
    // 摄像机位置
    public Transform cameraTransform;

    //UI摄像机
    public Transform uiCammeraTransform;

    //UI画布
    public Canvas roleCanvas;

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

    public bool combine = DEFAULT_COMBINEMATERIAL;
    private bool[] weapon_list = new bool[3];
    private bool[] head_list = new bool[3];
    private bool[] chest_list = new bool[3];
    private bool[] hand_list = new bool[3];
    private bool[] feet_list = new bool[3];
    private bool isInit = false;
    private bool isStart = false;
    private bool sceneIsFinish = false;


    //总的角色控制类，控制角色换肤，动作，
    public UCharacterController character = null;

    private baseAI roleAI;

    private void initData()
    {
        weapon_list[DEFAULT_WEAPON] = true;
        head_list[DEFAULT_HEAD] = true;
        chest_list[DEFAULT_CHEST] = true;
        hand_list[DEFAULT_HAND] = true;
        feet_list[DEFAULT_FEET] = true;
        isInit = true;
    }

    public bool getIsInit()
    {
        return isInit;
    }


    // Start is called before the first frame update
    void Start()
    {
        initData();
        if (sceneIsFinish == false)
        {
            sceneIsFinish = true;
            createRole(new Vector3(9, 0.265f, 16));
        }
    }

    public void createRole(Vector3 pPos)
    {
        // create an avatar
        character = App.Game.CharacterMgr.Generatecharacter(
            "ch_pc_hou",
            "ch_we_one_hou_" + index[DEFAULT_WEAPON],
            "ch_pc_hou_" + index[DEFAULT_HEAD] + "_tou",
            "ch_pc_hou_" + index[DEFAULT_CHEST] + "_shen",
            "ch_pc_hou_" + index[DEFAULT_HAND] + "_shou",
            "ch_pc_hou_" + index[DEFAULT_FEET] + "_jiao",
            combine);

        character.roleInstance.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        character.initData(cameraTransform, character.roleInstance.transform, pPos, roleCanvas);

        character.roleInstance.GetComponent<roleCollider>().enabled = false;
        character.roleInstance.GetComponent<animationEvent>().enabled = false;
        roleAI = character.roleInstance.GetComponent<baseAI>();

        isStart = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isInit == false)
        {
            return;
        }

        if (isStart)
        {
            App.Game.Update();
        }
    }

    public void roleRun() {
        roleSetAct(roleState.run);
    }

    public void roleStand() {
        roleSetAct(roleState.stand);
    }

    public void roleAttack() {
        roleSetAct(roleState.attack);
    }


    private void roleSetAct(roleState tmpState)
    {
        roleAI.PlayAIState(tmpState);
    }
        
        

}
