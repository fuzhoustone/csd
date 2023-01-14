
public class talkInfoLstGetTab : CsdTTable
{
    private static talkInfoLstGetTab instance = null;
    public static talkInfoLstGetTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoLstGetTab();
            instance.initParam();
        }
        return instance;
    }
   

    //玩家获得的话题表
    public const string csTalkInfoLstID = "talkInfoLstID"; 
    public const string csSayRoleIDLst = "roleIDLst"; //和哪些人物已知

    private const string csFileName = "talkInfoLstGet.csv";
    private const string csSplitID = "_";
    // private string csvFilePath;
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csTalkInfoLstID);
        addKeyName(csSayRoleIDLst);
    }

    public void InifDefFile()
    {
        InitFileName(csFileName);
    }

    public bool isSayByRoleID(int talkID,int roleID) {
        bool res = false;
        CSVRow tmpRow = GetRowFromKeyVal(csTalkInfoLstID, talkID.ToString());
        if (tmpRow != null) {
            string roleLst = tmpRow.GetString(csSayRoleIDLst); //获得roleID列表
            res = hasRole(roleID, roleLst);
        }
        return res;
    }

    private bool hasRole(int roleID, string roleLst) {
        bool res = false;
        if (roleLst.Equals("") != true) {
            string[] tmpLst = roleLst.Split(csSplitID.ToCharArray());
            for (int i = 0; i < tmpLst.Length; i++) {
                if (int.Parse(tmpLst[i]) == roleID) {
                    res = true;
                    break;
                }
            }

        }

        return res;
    }

    public void addSayRoleToLst(int talkID, int roleID) {
        CSVRow tmpRow = GetRowFromKeyVal(csTalkInfoLstID, talkID.ToString());
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
        res = roleLst + csSplitID + roleID;
        return res;
    }

    public void AddRow(int lTalkLstID)
    {
        int newID = GetTableLength();
        string[] tmpLst = new string[3];
        //this.data.m_columnNameIndexer.ColumnCount = 4
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csID)] = newID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csTalkInfoLstID)] = lTalkLstID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csSayRoleIDLst)] = "";
       
        this.AddCSVRow(tmpLst);
    }
}
