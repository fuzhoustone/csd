using System;
using System.Collections.Generic;
using System.IO;

public class CsdTTable  
{
    public List<CSVRow> m_elements = new List<CSVRow>();
    public const string csID = "ID";
    public List<string> m_colomuns = new List<string>(); 
    public void addKeyName(string lKey) {
        m_colomuns.Add(lKey);
    }

    private bool checkKeyNameExists(CSVRow lRow) {
        bool isPass = true;
        for (int i = 0; i < m_colomuns.Count; i++) {
            string tmpKeyName = m_colomuns[i];
            
            bool isExists = lRow.Exist(tmpKeyName);
            if (isExists) {
                UnityEngine.Debug.LogError("checkKeyNameExists Columns not tmpKeyName");
                isPass = false;
            }
        }

        return isPass;
    }

    public void Load(Stream stream)
    {
        if (stream == null) return;
        CSVData data = CSVLoader.Load(stream);
        bool isPass = data.isAllColumnNameExists(m_colomuns); //检查所有列名是否都存在

        for (int i = 0; i < data.RowCount; ++i)
        {
            var row = data.GetRow(i);
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

    public CSVRow GetRowFromID(int id)
    {
        for (int i = 0; i < m_elements.Count; ++i)
        {
            if (m_elements[i].GetInt(csID) == id)
                return m_elements[i];
        }
        return null;
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
    public T GetKeyValueFromID<K,T>(string keyName, K keyVal,string valName, T defVal)
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
 }

