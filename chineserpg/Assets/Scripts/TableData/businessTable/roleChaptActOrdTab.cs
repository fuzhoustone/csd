

public class roleChaptActOrdTab : CsdTTable
{
    private static roleChaptActOrdTab instance = null;
    public static roleChaptActOrdTab _instance()
    {
        if (instance == null)
        {
            instance = new roleChaptActOrdTab();
            instance.initParam();
        }

        return instance;
    }


    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csActOrder = "actOrder";
    public const string csRoleNote = "roleNote";

    public void initParam() {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csActOrder);
        addKeyName(csRoleNote);
    }



}
