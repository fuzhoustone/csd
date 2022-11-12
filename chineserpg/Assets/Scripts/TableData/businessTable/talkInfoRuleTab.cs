public class talkInfoRuleTab : CsdTTable
{
    private static talkInfoRuleTab instance = null;
    public static talkInfoRuleTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoRuleTab();
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
