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
    public const string csRoleID = "roleID";
    public const string csTarRoleID = "tarRoleID";
    public const string csSayTalkRoleInfoID = "sayTalkRoleInfoID";
    public const string csTalkRoleInfoID = "talkRoleInfoID";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTarRoleID);
        addKeyName(csSayTalkRoleInfoID);
        addKeyName(csTalkRoleInfoID);
    }

    public void checkAddTalkRoleInfo(int lRoleID,int ltarID, int lSaytalkID) {
       // CSVRow res = null;
        for (int i = 0; i < m_elements.Count; ++i)
        {

            if ((m_elements[i].GetInt(csRoleID) == lRoleID) &&
              (m_elements[i].GetInt(csTarRoleID) == ltarID) &&
              (m_elements[i].GetInt(csSayTalkRoleInfoID) == lSaytalkID))
            {
                int tmpTalkID = m_elements[i].GetInt(csTalkRoleInfoID);
                int tmpRoleID = talkRoleInfoTab._instance().GetValueFromID<int>(tmpTalkID, talkRoleInfoTab.csRoleID, -1);
                if (tmpRoleID > 0)
                {
                    talkRoleInfoGetTab._instance().AddRow(tmpRoleID, tmpTalkID);
                }
            }

            
        }

    }


}
