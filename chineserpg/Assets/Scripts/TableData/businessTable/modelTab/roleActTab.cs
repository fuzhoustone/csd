using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roleActTab : CsdTTable
{
    // Start is called before the first frame update
    private static roleActTab instance = null;
    public static roleActTab _instance()
    {
        if (instance == null)
        {
            instance = new roleActTab();
            instance.initParam();
        }
        return instance;
    }


    //玩家获得的线索表
    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csActVal = "actVal";
    

    private const string csFileName = "roleAct.csv";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csActVal);
        
    }

    public void InifDefFile()
    {
        InitFileName(csFileName);
    }
}
