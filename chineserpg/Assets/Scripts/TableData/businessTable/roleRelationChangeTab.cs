

public class roleRelationChangeTab : CsdTTable
{
    private static roleRelationChangeTab instance = null;
    public static roleRelationChangeTab _instance()
    {
        if (instance == null)
        {
            instance = new roleRelationChangeTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csChaptID = "chaptID";
    public const string csRoleID = "roleID";
    public const string csTargetID = "targetID";
    public const string csValue = "value";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csChaptID);
        addKeyName(csRoleID);
        addKeyName(csTargetID);
        addKeyName(csValue);
    }
}
