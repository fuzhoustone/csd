public class talkClueRuleTab : CsdTTable
{
    private static talkClueRuleTab instance = null;
    public static talkClueRuleTab _instance()
    {
        if (instance == null)
        {
            instance = new talkClueRuleTab();
            instance.initParam();
        }
        return instance;
    }

    //玩家获得的话题表
    public const string csRoleID = "roleID";
    public const string csChaptID = "chaptID";
    public const string csTalkID = "talkID";
    public const string csTalkRoleID = "talkRoleID"; //和哪些人物说过
    public const string csNote = "note";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csChaptID);
        addKeyName(csTalkID);
        addKeyName(csTalkRoleID);
        addKeyName(csNote);
        addKeyName(csTalkID);

    }
}
