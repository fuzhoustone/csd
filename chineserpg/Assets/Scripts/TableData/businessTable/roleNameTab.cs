
public class roleNameTab : CsdTTable
{
    private static roleNameTab instance = null;
    public static roleNameTab _instance() {
        if (instance == null) {
            instance = new roleNameTab();
            instance.initParam();
        }

        return instance;
    }

    public const string csRoleName = "roleName"; // { get {return "nextID";} }
    public const string csTitle = "title";
    public const string csNormalName = "normalName";
    public const string csNote = "note";
    public void initParam() {
        addKeyName(csID);
        addKeyName(csRoleName);
        addKeyName(csTitle);
        addKeyName(csNormalName);
        addKeyName(csNote);

    }
}
