using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkInfoLstGetRuleTab : CsdTTable
{
    private static talkInfoLstGetRuleTab instance = null;
    public static talkInfoLstGetRuleTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoLstGetRuleTab();
            instance.initParam();
        }
        return instance;
    }

    // talkInfoLstID	roleID	chaptID	sayTalkStoryID

    public const string csTalkLstID = "talkInfoLstID";
    public const string csRoleID = "roleID";
    public const string csChaptID = "chaptID";
    public const string csTalkStoryID = "sayTalkStoryID";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csTalkLstID);
        addKeyName(csRoleID);
        addKeyName(csChaptID);
        addKeyName(csTalkStoryID);
    }

    public void getTalkLstFromChapt(int lchaptID)
    {
        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++)
        {
            CSVRow tmpRow = GetRowFromIndex(i);
            if((tmpRow.GetInt(csRoleID) == gameDataManager.instance.roleID) 
                && (tmpRow.GetInt(csChaptID) == lchaptID))
            {
                int talkLstID = tmpRow.GetInt(csTalkLstID);
                talkInfoLstGetTab._instance().AddRow(talkLstID);
            }
        }

        talkInfoLstGetTab._instance().SaveFile();
    }

    public void checkAddTalkLst(int lSayTalkStoryID)
    {
        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++)
        {
            CSVRow tmpRow = GetRowFromIndex(i);
            if ((tmpRow.GetInt(csRoleID) == gameDataManager.instance.roleID)
              && (tmpRow.GetInt(csTalkStoryID) == lSayTalkStoryID))
            {
                int talkLstID = tmpRow.GetInt(csTalkLstID);
                //此信息还未添加
                if (talkInfoLstGetTab._instance().GetRowFromKeyVal(talkInfoLstGetTab.csTalkInfoLstID, talkLstID.ToString()) == null)
                {
                    talkInfoLstGetTab._instance().AddRow(talkLstID);
                    talkInfoLstGetTab._instance().SaveFile();
                }

                break;
            }
        }

    }

}
