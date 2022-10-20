
public class rolePropertyTab : CsdTTable
{
    public const string csRoleName = "roleName";
    public const string csHP = "HP";
    public const string csEventSystemID = "eventSystemID";
    
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleName);
        addKeyName(csHP);
        addKeyName(csEventSystemID);
    }
    private static rolePropertyTab instance = null;
    public static rolePropertyTab _instance()
    {
        if (instance == null)
        {
            instance = new rolePropertyTab();
            instance.initParam();
        }
        return instance;
    }
}
