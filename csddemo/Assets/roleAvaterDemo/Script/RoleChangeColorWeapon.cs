using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleChangeColorWeapon  {

    /// <summary>
    /// GameObject reference
    /// </summary>
    public GameObject pRoleInstance = null;  //角色对象
    public GameObject pRoleFlagInstance = null;  //角色对象
    public GameObject WeaponInstance = null;
    public GameObject WeaponInstance_l = null;

    /// <summary>
    /// Equipment informations
    /// </summary>
    private const string csRoleFlagRes = "roleflag";
	public string skeleton;
    public string equipment_head;
    public string equipment_chest;
    public string equipment_hand;
    public string equipment_feet;

    /// <summary>
    /// The unique id in the scene
    /// </summary>
	public int index;

    /// <summary>
    /// Other vars
    /// </summary>
    //public bool rotate = false;
    // public int animationState = 0;

    // private Animation animationController = null;
    //   private AvaterAnimationController animCon = null;

    // public attcakStartEnd attackClass = null;


    /*
    public void setInAttack(bool flag) {
        animCon.isInAttack = flag;
    }
    */

    public GameObject GetRoleInstance() {
        return pRoleInstance;
    }

    public GameObject GetRoleFlagInstance() {
        return pRoleFlagInstance;
    }

    public void dataDestory() {
        
        if(pRoleInstance != null)
            GameObject.Destroy(pRoleInstance);
        if(pRoleFlagInstance != null)
            GameObject.Destroy(pRoleFlagInstance);
        if (WeaponInstance != null)
            GameObject.Destroy(WeaponInstance);
        if (WeaponInstance_l != null)
            GameObject.Destroy(WeaponInstance_l);
    }

    public RoleChangeColorWeapon(int index, string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false)
    {

        //Creates the skeleton object
        Object res = Resources.Load("Prefab/" + skeleton);
        Object flagRes = Resources.Load("Prefab/" + csRoleFlagRes);
        //App.Game.character.roleInstance = GameObject.Instantiate(res) as GameObject;
        pRoleInstance = GameObject.Instantiate(res) as GameObject;

        pRoleFlagInstance = GameObject.Instantiate(flagRes) as GameObject;

        this.index = index;
        this.skeleton = skeleton;
        this.equipment_head = head;
        this.equipment_chest = chest;
        this.equipment_hand = hand;
        this.equipment_feet = feet;

        string[] equipments = new string[4];
        equipments[0] = head;
        equipments[1] = chest;
        equipments[2] = hand;
        equipments[3] = feet;

        CombineSkinned(pRoleInstance, equipments, combine);
        CombineSkinned(pRoleFlagInstance, equipments, combine);
        /*
        // Create and collect other parts SkinnedMeshRednerer
        SkinnedMeshRenderer[] meshes = new SkinnedMeshRenderer[4];
        SkinnedMeshRenderer[] flagmeshes = new SkinnedMeshRenderer[4];

        GameObject[] objects = new GameObject[4];
        GameObject[] flagobjects = new GameObject[4];

        for (int i = 0; i < equipments.Length; i++)
        {

            res = Resources.Load("Prefab/" + equipments[i]);
            objects[i] = GameObject.Instantiate(res) as GameObject;
            flagobjects[i] = GameObject.Instantiate(res) as GameObject;

            meshes[i] = objects[i].GetComponentInChildren<SkinnedMeshRenderer>();
            flagmeshes[i] = flagobjects[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }

        // Combine meshes
        App.Game.CharacterMgr.CombineSkinnedMgr.CombineObject(pRoleInstance, meshes, combine);
        App.Game.CharacterMgr.CombineSkinnedMgr.CombineObject(pRoleFlagInstance, flagmeshes, combine);

        // Delete temporal resources
        for (int i = 0; i < objects.Length; i++)
        {

            GameObject.DestroyImmediate(objects[i].gameObject);
            GameObject.DestroyImmediate(flagobjects[i].gameObject);
        }
        */


        // Create weapon
        res = Resources.Load("Prefab/" + weapon);

        WeaponInstance_l = setWeaponInstance(res, "weapon_hand_l");

    }

    /*
    private void setInitAttackBox(string boxName)
    {
        Transform[] transforms = WeaponInstance_l.GetComponentsInChildren<Transform>();
        foreach (Transform joint in transforms)
        {
            if (joint.name == boxName)
            {// find the joint (need the support of art designer)
                //pWeaponInstance.transform.parent = joint.gameObject.transform;
                attackCollider box = joint.GetComponent<attackCollider>();
                if (box != null)
                {
                   // box.roleInstance = ;
                    Debug.LogWarning("set attackRoleinstance");
                }
                break;
            }
        }
    }
    */
/*
    public bool isInAttack()
    {
        bool res = false;
        if (attackClass != null)
        {
            res = attackClass.isInAttack;
        }
        return res;
    }
    */

    private GameObject setWeaponInstance(Object res, string weapon_hand)
    {

        GameObject pWeaponInstance = GameObject.Instantiate(res) as GameObject;

        Transform[] transforms = pRoleInstance.GetComponentsInChildren<Transform>();
        foreach (Transform joint in transforms)
        {
            if (joint.name == weapon_hand)
            {// find the joint (need the support of art designer)
                pWeaponInstance.transform.parent = joint.gameObject.transform;
                break;
            }
        }

        // Init weapon relative informations
        pWeaponInstance.transform.localScale = Vector3.one;
        pWeaponInstance.transform.localPosition = Vector3.zero;
        pWeaponInstance.transform.localRotation = Quaternion.identity;

        return pWeaponInstance;
    }

    public void ChangeHeadEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(0, equipment, combine);
    }

    public void ChangeChestEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(1, equipment, combine);
    }

    public void ChangeHandEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(2, equipment, combine);
    }

    public void ChangeFeetEquipment(string equipment, bool combine = false)
    {
        ChangeEquipment(3, equipment, combine);
    }

    public void ChangeWeapon(string weapon)
    {
        Object res = Resources.Load("Prefab/" + weapon);
        //GameObject oldWeapon = WeaponInstance;
        if (WeaponInstance != null)
            WeaponInstance = ChangeWeaponBase(res, WeaponInstance);

        if (WeaponInstance_l != null)
            WeaponInstance_l = ChangeWeaponBase(res, WeaponInstance_l);
    }

    private GameObject ChangeWeaponBase(Object res, GameObject oldWeapon)
    {
        GameObject newWeapon = GameObject.Instantiate(res) as GameObject;
        newWeapon.transform.parent = oldWeapon.transform.parent;
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localScale = Vector3.one;
        newWeapon.transform.localRotation = Quaternion.identity;

        GameObject.Destroy(oldWeapon);

        return newWeapon;
    }

    private void CombineSkinned(GameObject pInstance, string[] equipments, bool combine) {
        Object res = null;
        SkinnedMeshRenderer[] meshes = new SkinnedMeshRenderer[4];
        GameObject[] objects = new GameObject[4];
        for (int i = 0; i < equipments.Length; i++)
        {

            res = Resources.Load("Prefab/" + equipments[i]);
            objects[i] = GameObject.Instantiate(res) as GameObject;
            meshes[i] = objects[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }

        App.Game.CharacterMgr.CombineSkinnedMgr.CombineObject(pInstance, meshes, combine);

        for (int i = 0; i < objects.Length; i++)
        {

            GameObject.DestroyImmediate(objects[i].gameObject);
        }
    }

    public void ChangeEquipment(int index, string equipment, bool combine = false)
    {
        switch (index)
        {

            case 0:
                equipment_head = equipment;
                break;
            case 1:
                equipment_chest = equipment;
                break;
            case 2:
                equipment_hand = equipment;
                break;
            case 3:
                equipment_feet = equipment;
                break;
        }

        string[] equipments = new string[4];
        equipments[0] = equipment_head;
        equipments[1] = equipment_chest;
        equipments[2] = equipment_hand;
        equipments[3] = equipment_feet;

        CombineSkinned(pRoleInstance, equipments, combine);
        CombineSkinned(pRoleFlagInstance, equipments, combine);
        /*
        Object res = null;
        SkinnedMeshRenderer[] meshes = new SkinnedMeshRenderer[4];
        GameObject[] objects = new GameObject[4];
        for (int i = 0; i < equipments.Length; i++)
        {

            res = Resources.Load("Prefab/" + equipments[i]);
            objects[i] = GameObject.Instantiate(res) as GameObject;
            meshes[i] = objects[i].GetComponentInChildren<SkinnedMeshRenderer>();
        }

        App.Game.CharacterMgr.CombineSkinnedMgr.CombineObject(pRoleInstance, meshes, combine);

        for (int i = 0; i < objects.Length; i++)
        {

            GameObject.DestroyImmediate(objects[i].gameObject);
        }
        */
    }

}
