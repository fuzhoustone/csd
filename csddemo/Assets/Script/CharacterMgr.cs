using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//所有角色控制器
public class UCharacterMgr  {

	private UCombineSkinnedMgr skinnedMgr = null;
	public UCombineSkinnedMgr CombineSkinnedMgr { get{ return skinnedMgr; } }

    private int characterIndex = 0;
    private UCharacterController characterDic = null;
    //private Dictionary<int,UCharacterController> characterDic = new Dictionary<int, UCharacterController>();

    public UCharacterMgr () {

		skinnedMgr = new UCombineSkinnedMgr ();
	}

    public UCharacterController Generatecharacter(string strPre) {
        UCharacterController instance = new UCharacterController(strPre);
        characterDic = instance;
        return instance;
    }

    /*
	public UCharacterController Generatecharacter (string skeleton, string weapon, string head, string chest, string hand, string feet, bool combine = false)
	{

        UCharacterController instance = new UCharacterController (characterIndex,skeleton,weapon,head,chest,hand,feet,combine);
        characterDic = instance;
        //characterDic.Add(characterIndex,instance);
		//characterIndex ++;

		return instance;
	}
    */
    public void changeChar() {
        characterDic.onlyRoleDestory();
        characterDic = null;
    }

    public void RemoveChar ()
	{
        characterDic.dataDestory();
        characterDic = null;
    }

	public void Update () {
#if DebugRole
        return ;
#endif

        if (characterDic != null)
            characterDic.Update();

        /*
        foreach (UCharacterController character in characterDic.Values)
		{
			character.Update();
		}
        */
	}
}
