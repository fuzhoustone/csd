

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
        return m_elements;

    }
}
