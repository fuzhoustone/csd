
public class roleTrustTab : CsdTTable
{
    public const string csSourRoleID = "sourRoleID";
    public const string csDestRoleID = "destRoleID";
    public const string csTrust = "trust";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csSourRoleID);
        addKeyName(csDestRoleID);
        addKeyName(csTrust);
    }
    private static roleTrustTab instance = null;
    public static roleTrustTab _instance()
    {
        if (instance == null)
        {
            instance = new roleTrustTab();
            instance.initParam();
        }
        return instance;
    }
}
