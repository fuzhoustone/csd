
public class talkInfoGetTab : CsdTTable
{
    private static talkInfoGetTab instance = null;
    public static talkInfoGetTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoGetTab();
            instance.initParam();
        }
        return instance;
    }
   

    //玩家获得的话题表
    public const string csTalkID = "talkID"; 
    public const string csSayRoleIDLst = "roleIDLst"; //和哪些人物说过

    private const string csFileName = "talkInfoGet.csv";
    // private string csvFilePath;
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csTalkID);
        addKeyName(csSayRoleIDLst);
    }

    public void LoadDefFile()
    {
        LoadFile(csFileName);
    }

    public bool isSayByRoleID(int talkID,int roleID) {
        bool res = false;
        CSVRow tmpRow = GetRowFromKeyVal(csTalkID, talkID.ToString());
        if (tmpRow != null) {
            string roleLst = tmpRow.GetString(csSayRoleIDLst); //获得roleID列表
            res = hasRole(roleID, roleLst);
        }
        return res;
    }

    private bool hasRole(int roleID, string roleLst) {
        bool res = false;

        //待写

        return res;
    }

    public void addSayRoleToLst(int talkID, int roleID) {
        CSVRow tmpRow = GetRowFromKeyVal(csTalkID, talkID.ToString());
        if (tmpRow != null)
        {
            string roleLst = tmpRow.GetString(csSayRoleIDLst); //获得roleID列表
            if (hasRole(roleID, roleLst) == false) {
                roleLst = addRole(roleID, roleLst);
                tmpRow.SetString(csSayRoleIDLst,roleLst);
            }
        }
    }

    private string addRole(int roleID, string roleLst) {
        string res = roleLst;
        res = roleLst + "_" + roleID;
        return res;
    }

    public void AddRow(int lTalkID)
    {
        int newID = GetTableLength();
        string[] tmpLst = new string[3];
        //this.data.m_columnNameIndexer.ColumnCount = 4
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csID)] = newID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csTalkID)] = lTalkID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csSayRoleIDLst)] = "";
       
        this.AddCSVRow(tmpLst);
    }
}
