using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsdTTable  
{
    protected List<CSVRow> m_elements = new List<CSVRow>();
    protected CSVData data;
    public const string csID = "ID";
    public List<string> m_colomuns = new List<string>(); //用于检查字段名是存在，非必须
    private int maxID = 0;
    private string csvFileName,modeFileName;

    public void addKeyName(string lKey) {
        m_colomuns.Add(lKey);
    }

    public int getNewID() {
        return maxID + 1;
    }

    public void refreshMaxID(int val) {
        if (maxID < val) {
            maxID = val;
        }
    }

    private bool checkKeyNameExists(CSVRow lRow) {
        bool isPass = true;
        for (int i = 0; i < m_colomuns.Count; i++) {
            string tmpKeyName = m_colomuns[i];
            
            bool isExists = lRow.Exist(tmpKeyName);
            if (isExists == false)
            {
                UnityEngine.Debug.LogError("checkKeyNameExists Columns not tmpKeyName");
                isPass = false;
            }
        }

        return isPass;
    }

    public void Load(Stream stream)
    {
        if (stream == null) return;
        data = CSVLoader.Load(stream);  //data是new出来的，原先的data理论上内存有泄漏
        bool isPass = data.isAllColumnNameExists(m_colomuns); //检查所有列名是否都存在

        for (int i = 0; i < data.RowCount; ++i)
        {
            var row = data.GetRow(i);
            int tmpID = row.GetInt(csID);
            if (tmpID > maxID)
                maxID = tmpID;
            m_elements.Add(row);
        }
    }

    public int GetTableLength()
    {
        return m_elements.Count;
    }

    public void Clear()
    {
        m_elements.Clear();
    }

    //根据ID返回
    public CSVRow GetRowFromID(int id)
    {
        for (int i = 0; i < m_elements.Count; ++i)
        {
            if (m_elements[i].GetInt(csID) == id)
                return m_elements[i];
        }
        Debug.LogWarning("CSVRow is null,ID="+id.ToString() );
        return null;
    }

    //遍历表用的
    public CSVRow GetRowFromIndex(int i) {
        if (m_elements.Count > i) {
            return m_elements[i];
        }

        return null;
    }

    public CSVRow GetRowFromKeyVal(string keyName, string checkVal)
    {
        CSVRow resRow = null;
        for (int i = 0; i <m_elements.Count; i++)
        {
            
            CSVRow tmpRow = m_elements[i];
            string keyVal = tmpRow.GetString(keyName);
            if(keyVal.Equals(checkVal))
            {
                resRow = tmpRow;
                break;
            }
                
        }
        return resRow;
    }

    //根据id值，返回特定字段的值,需给返回默认值
    public T GetValueFromID<T>(int id, string valName, T defVal) {
        CSVRow tmpRoleID = GetRowFromID(id);
        T outVal = defVal;
        if (tmpRoleID != null) {
            outVal = tmpRoleID.GetVal<T>(valName, defVal);
        }

        return outVal;
    }

    //根据key字段及key字段的值，返回val字段的值,需给val默认值,
    //key只支持整型，string,浮点型
    public T GetValueFromKey<K,T>(string keyName, K keyVal,string valName, T defVal)
    {
        T res = defVal;
        for (int i = 0; i < m_elements.Count; ++i)
        {
            K keyDefVal = (K)Convert.ChangeType("0", typeof(K));
            K tmpKey = m_elements[i].GetVal<K>(keyName, keyDefVal);
            if(tmpKey.Equals(keyVal))
            {
                CSVRow tmpRow = m_elements[i];
                res = tmpRow.GetVal(valName, defVal);
                break;
            }
        }
        return res;
    }

    //根据key,key2字段及其的值，返回CSVRow
    //key,key2只支持:整型，string,浮点型
    public CSVRow GetRowFromKey2<K,M>(string keyName, K keyVal, 
                                      string keyName2, M keyVal2)
    {
        CSVRow res = null;
        for (int i = 0; i < m_elements.Count; ++i)
        {
            K keyDefVal = (K)Convert.ChangeType("0", typeof(K));
            K tmpKey = m_elements[i].GetVal<K>(keyName, keyDefVal);

            M keyDefVal2 = (M)Convert.ChangeType("0", typeof(M));
            M tmpKey2 = m_elements[i].GetVal<M>(keyName2, keyDefVal2);
            if ((tmpKey.Equals(keyVal)) &&
                (tmpKey2.Equals(keyVal2)) 
               )
            {
                res = m_elements[i];
                break;
            }
        }
        return res;
    }

    public void AddCSVRow(string[] lValues) {
        CSVRow tmpRow = new CSVRow(lValues, data.m_columnNameIndexer);
        data.Add(tmpRow);
        m_elements.Add(tmpRow);
    }

    public void WriteFile(string filePath) {
        using (var writeStream = new FileStream(filePath, FileMode.Create))
        {
            CSVWriter.Write(data, writeStream, m_colomuns.ToArray());
        }
    }

    public void checkAndNewFile() //检查并覆盖新建文件
    {
        if (File.Exists(modeFileName) == false) { 
            
        }
        else
       // if (File.Exists(csvFileName) == false)
       // {
            File.Copy(modeFileName, csvFileName, true); //覆盖模式
       // }
      //  return csvFileName;
    }

    public void InitFileName(string pFileName) {
        string tarPath = Application.persistentDataPath;
        if (!Directory.Exists(tarPath))
        {
            Directory.CreateDirectory(tarPath);
        }

        //modeFileName = Application.dataPath + "/Resources/Items/modelItems/" + pFileName;
        modeFileName = Application.streamingAssetsPath+ "/Resources/Items/modelItems/" + pFileName;
        csvFileName = tarPath + "/" + pFileName;
        
    }

    public void LoadFile(){
        string tarFile = csvFileName; //检查文件，不存在就新建文件

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

        this.WriteFile(csvFileName);
    }

}

