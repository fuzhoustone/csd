using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkRoleInfoTab : CsdTTable
{
   
    private static talkRoleInfoTab instance = null;
    public static talkRoleInfoTab _instance()
    {
        if (instance == null)
        {
            instance = new talkRoleInfoTab();
            instance.initParam();
        }
        return instance;
    }
    //roleID	tarRoleID	talkStortyID	priority

    public const string csRoleID = "roleID";
    public const string csTarRoleID = "tarRoleID";
    public const string csTalkStortyID = "talkStortyID";
    public const string csPriority = "priority";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTarRoleID);
        addKeyName(csTalkStortyID);
        addKeyName(csPriority);
    }

    public CSVRow getTalkRoleInfo(int lroleID,int lTarID) {
        CSVRow res = null;
        int oldPri = -1;
        for (int i = 0; i < m_elements.Count; ++i)
        {
            int tmpRoleID = m_elements[i].GetInt(csRoleID);
            int tmpTarRoleID = m_elements[i].GetInt(csTarRoleID);

            if((tmpRoleID == lroleID) &&(tmpTarRoleID == lTarID))
            {
                int tmpPriority = m_elements[i].GetInt(csPriority);
                if (oldPri < tmpPriority)
                    tmpPriority = oldPri;
                    res = m_elements[i];
                
            }
        }
        return res;
    }


}
