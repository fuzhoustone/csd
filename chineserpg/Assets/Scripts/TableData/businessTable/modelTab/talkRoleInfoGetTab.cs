using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkRoleInfoGetTab : CsdTTable
{
    private static talkRoleInfoGetTab instance = null;
    public static talkRoleInfoGetTab _instance()
    {
        if (instance == null)
        {
            instance = new talkRoleInfoGetTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csRoleID = "roleID";
    public const string csTalkRoleInfoID = "talkRoleInfoID";
    public const string csIsUse = "isUse";

    private const string csFileName = "talkRoleInfoGet.csv";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTalkRoleInfoID);
        addKeyName(csIsUse);
    }

    public void InifDefFile()
    {
        InitFileName(csFileName);
    }

    public bool hasNoSayTalkRoleInfo(int lroleID, int lTarID) {
        bool res = false;
        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++)
        {  //从已获得话题中挑选
            CSVRow tmpGetRow = GetRowFromIndex(i);
            if ((tmpGetRow.GetInt(csRoleID) == lroleID) &&
                 (tmpGetRow.GetBool(csIsUse) == false))
            {
                int tmpTRIID = tmpGetRow.GetInt(csTalkRoleInfoID);
                CSVRow tmpTalkRow = talkRoleInfoTab._instance().GetRowFromKey2<int, int>
                             (talkRoleInfoTab.csID, tmpTRIID,
                              talkRoleInfoTab.csTarRoleID, lTarID);

                if (tmpTalkRow != null)
                {
                    res = true;
                    break;
                }
            }
        }
               
        return res;
    
    }

    //获取要聊天的话题
    public CSVRow getTalkRoleInfo(int lroleID, int lTarID)
    {
        CSVRow res = null;
        int oldPri = -1;
        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++) {  //从已获得话题中挑选
           CSVRow tmpGetRow =  GetRowFromIndex(i);
           if( (tmpGetRow.GetInt(csRoleID) == lroleID) &&
                (tmpGetRow.GetBool(csIsUse) == false))
           {
                int tmpTRIID = tmpGetRow.GetInt(csTalkRoleInfoID);
                CSVRow tmpTalkRow = talkRoleInfoTab._instance().GetRowFromKey2<int,int>
                             (talkRoleInfoTab.csID,tmpTRIID, 
                              talkRoleInfoTab.csTarRoleID, lTarID);

                if (tmpTalkRow != null) {
                    int tmpPriority = tmpTalkRow.GetInt(talkRoleInfoTab.csPriority);
                    if (oldPri < tmpPriority)
                    {
                        tmpPriority = oldPri;
                        res = tmpGetRow;
                    }
                }
           }
        }


        return res;

    }

    public void AddRow(int lroleID,int talRoleInfoID)
    {
        int newID = GetTableLength();
        string[] tmpLst = new string[4];

        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csID)] = newID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csRoleID)] = lroleID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csTalkRoleInfoID)] = talRoleInfoID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csIsUse)] = "0";

        this.AddCSVRow(tmpLst);
    }
}
