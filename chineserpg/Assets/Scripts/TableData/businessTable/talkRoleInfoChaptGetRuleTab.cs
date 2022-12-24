using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkRoleInfoChaptGetRuleTab : CsdTTable
{
    private static talkRoleInfoChaptGetRuleTab instance = null;
    public static talkRoleInfoChaptGetRuleTab _instance()
    {
        if (instance == null)
        {
            instance = new talkRoleInfoChaptGetRuleTab();
            instance.initParam();
        }
        return instance;
    }

    // roleID	chaptID	talkRoleInfoID

    public const string csRoleID = "roleID";
    public const string csChaptID = "chaptID";
    public const string csTalkRoleInfoID = "talkRoleInfoID";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csChaptID);
        addKeyName(csTalkRoleInfoID);
    }

    //各人物每个章节自动获得的 话题
    public void getTalkRoleInfoFromChapt(int lchaptID) {
        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpRow = GetRowFromIndex(i);
            if (tmpRow.GetInt(csChaptID) == lchaptID) {
                int  talkRIID = tmpRow.GetInt(csTalkRoleInfoID);
                int tmpRoleID = talkRoleInfoTab._instance().GetValueFromID<int>(talkRIID, talkRoleInfoTab.csRoleID, -1);
                if (tmpRoleID > 0) {
                    talkRoleInfoGetTab._instance().AddRow(tmpRoleID, talkRIID);
                }
            }
        }

        talkRoleInfoGetTab._instance().SaveFile();
    }

}
