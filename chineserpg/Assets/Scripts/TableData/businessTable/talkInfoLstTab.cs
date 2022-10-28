

public class talkInfoLstTab : CsdTTable
{
    private static talkInfoLstTab instance = null;
    public static talkInfoLstTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoLstTab();
            instance.initParam();
        }

        return instance;
    }

    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csStoryID = "storyID";
    public const string csContentCn = "contentCn";
    public const string csContentEn = "contentEn";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csStoryID);
        addKeyName(csContentCn);
        addKeyName(csContentEn);

    }

}
