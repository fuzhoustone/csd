

public class roleFriendTab : CsdTTable
{
    private static roleFriendTab instance = null;
    public static roleFriendTab _instance()
    {
        if (instance == null)
        {
            instance = new roleFriendTab();
            instance.initParam();
        }
        return instance;
    }
    

    //玩家获得的线索表
    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csTargetID = "targetID";
    public const string csValue = "value";

    private const string csFileName = "roleFriend.csv";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTargetID);
        addKeyName(csValue);
    }

    public void LoadDefFile()
    {
        LoadFile(csFileName);
    }

    //更新某个章节各分物关系值
    public void LoadChaptChage(int lChapt) {
        int nCount = roleRelationChangeTab._instance().GetTableLength();
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpRow = roleRelationChangeTab._instance().GetRowFromIndex(i);
            if (tmpRow.GetInt(roleRelationChangeTab.csChaptID) == lChapt) {
                int tmpRoleID = tmpRow.GetInt(roleRelationChangeTab.csRoleID);
                int tmpTargetID = tmpRow.GetInt(roleRelationChangeTab.csTargetID);
                int tmpValue = tmpRow.GetInt(roleRelationChangeTab.csValue);

                CSVRow friendRow = GetValueFromKey2<int, int>(csRoleID, tmpRoleID,
                                                             csTargetID, tmpTargetID
                                                             );
               int val = friendRow.GetInt(csValue);
               friendRow.SetIngeger(csValue, tmpValue + val);
            }
        }

        SaveFile();
    }


}
