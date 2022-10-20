
public class eventListTab : CsdTTable
{
    public const string csContent = "content";
    public const string csEventSystemID = "eventSystemID";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csContent);
        addKeyName(csEventSystemID);
    }
    private static eventListTab instance = null;
    public static eventListTab _instance()
    {
        if (instance == null)
        {
            instance = new eventListTab();
            instance.initParam();
        }
        return instance;
    }
}
