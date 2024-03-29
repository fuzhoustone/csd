﻿using System.Collections;
using System.IO;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    private const string csTableSetPre = "Prefabs/TableSet";
    private bool isInit = false;
    private static TableSet _instance = null;
    public static TableSet instance
    {
        get {
            if (_instance == null) {
                UnityEngine.Object tmpObj = Resources.Load(csTableSetPre);
                GameObject tabObj = GameObject.Instantiate(tmpObj) as GameObject;
                _instance = tabObj.GetComponent<TableSet>();

                MonoBehaviour.DontDestroyOnLoad(tabObj);
            }

            return _instance;
        }
    }


    public TextAsset storyRel;
    public TextAsset storyBgSceneRel;
    public TextAsset bgScenePic;
    public TextAsset roleFacePic;
    public TextAsset talkOption;
    public TextAsset roleNameLst;
  //  public TextAsset storyVideo;

  //  public TextAsset roleProperty;
  //  public TextAsset roleTrust;
  //  public TextAsset eventDamage;
  //  public TextAsset eventList;
//    public TextAsset eventSystemType;
    public TextAsset clueLst;
  //  public TextAsset clueLstGet;
    public TextAsset missionLst;
    public TextAsset talkInfoLst;
    public TextAsset talkInfoLstGetRule;
    public TextAsset talkInfoOptionLst;
    public TextAsset roleStoryStartRel;

    //public TextAsset talkInfoGet;
    public TextAsset talkclueRule;

    public TextAsset roleRelChange;

    public TextAsset talkRoleInfo;
    public TextAsset talkStory;
    public TextAsset talkRoleInfoTalkingGetRule;
    public TextAsset talkRoleInfoChaptGetRule;
    public TextAsset roleDefEnemy;
    public TextAsset roleActOrd;
    public void initData()
    {
        if (isInit)
            return;

        using (var stream = new MemoryStream(storyRel.bytes))
        {
            StoryRelationTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(storyBgSceneRel.bytes))
        {
            StoryBgSceneRelationTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(bgScenePic.bytes))
        {
            bgScenePicTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(roleFacePic.bytes))
        {
            roleFacePicTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(roleNameLst.bytes))
        {
            roleNameTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkOption.bytes))
        {
            storyOptionTab._instance().Load(stream);
        }
        /*
                using (var stream = new MemoryStream(storyVideo.bytes))
                {
                    StoryVideoTab._instance().Load(stream);
                }


                using (var stream = new MemoryStream(roleProperty.bytes))
                {
                    rolePropertyTab._instance().Load(stream);
                }

                using (var stream = new MemoryStream(roleTrust.bytes))
                {
                    roleTrustTab._instance().Load(stream);
                }
                
          using (var stream = new MemoryStream(eventDamage.bytes))
          {
              eventDamageTab._instance().Load(stream);
          }

          using (var stream = new MemoryStream(eventList.bytes))
          {
              eventListTab._instance().Load(stream);
          }

          using (var stream = new MemoryStream(eventSystemType.bytes))
          {
              eventSystemTypeTab._instance().Load(stream);
          }
        */
        using (var stream = new MemoryStream(clueLst.bytes))
        {
            clueLstTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(missionLst.bytes))
        {
            missionLstTab._instance().Load(stream);
        }
        using (var stream = new MemoryStream(talkInfoLst.bytes))
        {
            talkInfoLstTab._instance().Load(stream);
        }
        using (var stream = new MemoryStream(talkInfoLstGetRule.bytes))
        {
            talkInfoLstGetRuleTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkInfoOptionLst.bytes))
        {
            talkInfoOptionTab._instance().Load(stream);
        }
        

        using (var stream = new MemoryStream(roleStoryStartRel.bytes))
        {
            roleStoryStartRelTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkclueRule.bytes))
        {
            talkClueRuleTab._instance().Load(stream);
        }



        using (var stream = new MemoryStream(roleRelChange.bytes))
        {
            
            roleRelationChangeTab._instance().Load(stream);
        }

        clueLstGetTab._instance().InifDefFile();
        talkInfoLstGetTab._instance().InifDefFile();
        roleFriendTab._instance().InifDefFile();
        roleActTab._instance().InifDefFile();
        talkRoleInfoGetTab._instance().InifDefFile();

        using (var stream = new MemoryStream(talkRoleInfo.bytes))
        {

            talkRoleInfoTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkStory.bytes))
        {

            talkStoryTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(talkRoleInfoTalkingGetRule.bytes))
        {

            talkRoleInfoTalkingGetRuleTab._instance().Load(stream);
        }
       
        using (var stream = new MemoryStream(talkRoleInfoChaptGetRule.bytes))
        {

            talkRoleInfoChaptGetRuleTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(roleDefEnemy.bytes))
        {

            roleDefEnemyTab._instance().Load(stream);
        }

        using (var stream = new MemoryStream(roleActOrd.bytes))
        {

            roleChaptActOrdTab._instance().Load(stream);
        }
        

        isInit = true;

    }
       
 
}
