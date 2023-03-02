using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkRoleInfoTalkingGetRuleTab : CsdTTable
{
    // Start is called before the first frame update
    private static talkRoleInfoTalkingGetRuleTab instance = null;
    public static talkRoleInfoTalkingGetRuleTab _instance()
    {
        if (instance == null)
        {
            instance = new talkRoleInfoTalkingGetRuleTab();
            instance.initParam();
        }
        return instance;
    }
    //roleID	tarRoleID	talkRoleInfoID
   // public const string csRoleID = "roleID";
   // public const string csTarRoleID = "tarRoleID";
    public const string csSayTalkStoryID = "sayTalkStoryID";
    public const string csTalkRoleInfoID = "talkRoleInfoID";
    public void initParam()
    {
        addKeyName(csID);
      //  addKeyName(csRoleID);
      //  addKeyName(csTarRoleID);
        addKeyName(csSayTalkStoryID);
        addKeyName(csTalkRoleInfoID);
    }

    //某个话题被提到,引发新的话题
    public void checkAddTalkRoleInfo(int lSayTalkStoryID) {
        for (int i = 0; i < m_elements.Count; ++i)
        {
            if  (m_elements[i].GetInt(csSayTalkStoryID) == lSayTalkStoryID)
            {
                int tmpTalkID = m_elements[i].GetInt(csTalkRoleInfoID);
                if(tmpTalkID >= 0)
                {
                    int tmpRoleID = talkRoleInfoTab._instance().GetValueFromID<int>(tmpTalkID, talkRoleInfoTab.csRoleID, -1);
                    if ((tmpRoleID > 0) &&
                        (talkRoleInfoGetTab._instance().hasRow(tmpRoleID, tmpTalkID) == false))
                    {
                        talkRoleInfoGetTab._instance().AddRow(tmpRoleID, tmpTalkID);
                    }
                }
            }
        }

        talkRoleInfoGetTab._instance().SaveFile();
    }


}
