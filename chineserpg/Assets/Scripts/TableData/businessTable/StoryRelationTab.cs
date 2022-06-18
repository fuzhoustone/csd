

public class StoryRelationTab : CsdTTable
{  
    private static StoryRelationTab instance = null;
    public static StoryRelationTab _instance()
    {
        if (instance == null)
        {
            instance = new StoryRelationTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csNextID = "nextID"; // { get {return "nextID";} }
    public const string csUISort = "uiSort";
    public const string csContentCN  = "content_cn";
    public const string csContentEn  =  "content_en";
    public const string csIsRoleSay  =   "isRoleSay";
    public const string csNeedChangeBg = "needchangeBg";
    public const string csIsAutoSave = "isSave";  //自动保存的存档点
    public const string csIsKeySave = "isKeySave";  //线索的记录点，必定要另外自动保存

    public void initParam() {
        addKeyName(csID);
        addKeyName(csNextID);
        addKeyName(csUISort);
        addKeyName(csContentCN);
        addKeyName(csContentEn);
        addKeyName(csIsRoleSay);
        addKeyName(csNeedChangeBg);
        addKeyName(csIsAutoSave);
        addKeyName(csIsKeySave);
    }
}
