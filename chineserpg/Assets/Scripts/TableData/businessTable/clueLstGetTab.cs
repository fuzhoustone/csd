using System.IO;
using UnityEngine;

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

    private const string csFileName = "clueLstGet.csv";
    private string csvFilePath;
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

    

    public string checkAndNewFile() {

        string tarPath = Application.persistentDataPath;
        if (!Directory.Exists(tarPath))
        {
            Directory.CreateDirectory(tarPath);
        }

        string sourFile = Application.dataPath + "/Resources/Items/modelItems/" + csFileName;
        csvFilePath = tarPath + "/" + csFileName;
        if (File.Exists(csvFilePath) == false) {
            File.Copy(sourFile, csvFilePath, true); //覆盖模式
        }
        return csvFilePath;
    }

    public void LoadDefFile() {
        string tarFile = checkAndNewFile();

        try
        {
            using (FileStream fsSource = new FileStream(tarFile,
                                FileMode.Open, FileAccess.Read))
            {
                this.Load(fsSource);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("方法Read()异常" + ex);
        }

    }

    public void SaveFile()
    {


       // string filePath = Application.dataPath + "/AssetItems/clueLstGet.csv";

        this.WriteFile(csvFilePath);
    }

}
