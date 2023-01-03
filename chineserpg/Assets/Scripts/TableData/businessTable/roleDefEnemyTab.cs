

using System.Collections.Generic;

public class roleDefEnemyTab : CsdTTable
{
    private static roleDefEnemyTab instance = null;
    public static roleDefEnemyTab _instance()
    {
        if (instance == null)
        {
            instance = new roleDefEnemyTab();
            instance.initParam();
        }

        return instance;
    }

    //ID,roleID,defEnemyID
    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csTargetID = "targetID";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTargetID);
    }

    public List<CSVRow> getDefEnemy()
    {
        List<CSVRow> res = new List<CSVRow>();

        int nCount = GetTableLength();
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpRow = GetRowFromIndex(i);
            int tmpRoleID = tmpRow.GetInt(csRoleID);
            int ltmpTarID = tmpRow.GetInt(csTargetID);

            if (talkRoleInfoGetTab._instance().hasNoSayTalkRoleInfo(tmpRoleID, ltmpTarID))
            {
                res.Add(tmpRow);
            }
        }

        return res;

    }
}
