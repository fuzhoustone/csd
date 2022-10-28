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
}
