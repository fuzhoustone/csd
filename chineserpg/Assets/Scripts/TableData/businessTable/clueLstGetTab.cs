

public class clueLstGetTab : CsdTTable
{
    private static clueLstGetTab instance = null;
    public static clueLstGetTab _instance()
    {
        if (instance == null)
        {
            instance = new clueLstGetTab();
            instance.initParam();
        }

        return instance;
    }

    //玩家获得的线索表
    public const string csClueID = "clueID"; // { get {return "nextID";} }
    public const string csIsPub = "isPublic";
    public const string csLook = "isLook";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csClueID);
        addKeyName(csIsPub);
        addKeyName(csLook);
    }

    public bool isGetClue(int lClueID, ref bool isLook) {
        bool isGet = false;
        CSVRow tmpRow = this.GetRowFromKeyVal(csClueID, lClueID.ToString());
        if (tmpRow != null) {
            isGet = true;
            tmpRow.GetBool(csIsPub);
            tmpRow.GetBool(csLook);
        }

        isLook = false;
        return isGet;
    }

    public void AddRow(int lClueID,bool isPub = false, bool isLook = false) {
        int newID = GetTableLength();
        string[] tmpLst = new string[4];
        //this.data.m_columnNameIndexer.ColumnCount = 4
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csID)] = newID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csClueID)] = lClueID.ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csIsPub)] = (isPub?1:0).ToString();
        tmpLst[this.data.m_columnNameIndexer.GetColumnIndex(csLook)] = (isLook?1:0).ToString();

        this.AddCSVRow(tmpLst);
    }
    /*
    public void SaveCSV() {
        string filePath = Application.dataPath + "/AssetItems/clueLstGet.csv";

        clueLstGetTab._instance().WriteFile(filePath);
    }
    */
}
