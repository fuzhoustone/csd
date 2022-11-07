using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clueLstTab : CsdTTable
{
    private static clueLstTab instance = null;
    public static clueLstTab _instance()
    {
        if (instance == null)
        {
            instance = new clueLstTab();
            instance.initParam();
        }

        return instance;
    }

    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csChaptID = "chaptID";
    public const string csContentCn = "contentCn";
    public const string csContentEn = "contentEn";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csChaptID);
        addKeyName(csContentCn);
        addKeyName(csContentEn);
    }

    public List<int> getClueLst(int roleID, int chaptID) {
        List<int> res = new List<int>();
        for (int i = 0; i < this.GetTableLength(); i++) {
            CSVRow tmpRow = this.GetRowFromIndex(i);
            int tmpRoleID = tmpRow.GetInt(csRoleID);
            int tmpChaptID = tmpRow.GetInt(csChaptID);
            if ((tmpRoleID == roleID) && (tmpChaptID == chaptID)) {
                int tmpID = tmpRow.GetInt(csID);
                res.Add(tmpID);
            }
        }

        return res;
    }

}
