

public class roleStoryStartRelTab : CsdTTable
{
    private static roleStoryStartRelTab instance = null;
    public static roleStoryStartRelTab _instance()
    {
        if (instance == null)
        {
            instance = new roleStoryStartRelTab();
            instance.initParam();
        }

        return instance;
    }

    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csChaptID = "chaptID";
    public const string csStoryID = "storyID";
    
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csChaptID);
        addKeyName(csStoryID);
    }

    public int getStartStoryID(int roleID, int chaptID) {
        int res = 1;
        int nCount = this.GetTableLength();
        for (int i = 0; i < nCount; i++) {
           CSVRow tmpRow =  this.GetRowFromIndex(i);
           int tmpRoleID = tmpRow.GetInt(csRoleID);
           int tmpChaptID = tmpRow.GetInt(csChaptID);
            if ((roleID == tmpRoleID) && (chaptID == tmpChaptID)) {
                res = tmpRow.GetInt(csStoryID);
                break;
            }
        }
        return res;
    }
}
