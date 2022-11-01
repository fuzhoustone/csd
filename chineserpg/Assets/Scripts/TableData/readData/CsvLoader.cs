using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public static class CSVLoader
{
    public static CSVData Load(Stream stream, char seperator = ',')
    {
        return Load(stream, Encoding.UTF8, seperator);
    }

    public static CSVData Load(Stream stream, Encoding encoding, char seperator = ',')
    {
        CSVData result;
        var reader = new StreamReader(stream, encoding);

        string columnNames;
        do
        {
            columnNames = reader.ReadLine();
        }
        //update by csd 跳过由半角与全角空格组成的空行
        while ((isNullLine(columnNames) == true) || (columnNames.StartsWith("``")));
        //update end
        //while (columnNames.StartsWith("``"));

        result = new CSVData(columnNames, seperator);
        string row;
        while (reader.EndOfStream == false)
        {
            row = reader.ReadLine();
            if (row.StartsWith("``")) continue;
            //add by csd 跳过空行
            if (isNullLine(row))
                continue;
            //add end
            result.Add(row, seperator, encoding);
        }
        reader.Close();
        return result;
    }

    private static bool isNullLine(string pVal) {
        bool res = false;
        string tmpVal = pVal;
        tmpVal = tmpVal.Replace(" ","");
        tmpVal = tmpVal.Replace("　", "");
        if (tmpVal.Equals(""))  //与""相等
            res = true;

        return res;
    }

}

public class CSVData : IEnumerable<CSVRow>
{
    public readonly CSVColumnNameIndexer m_columnNameIndexer;
    private List<CSVRow> m_datas = new List<CSVRow>();



    internal CSVData(string columnNames, char seperator)
    {
        m_columnNameIndexer = new CSVColumnNameIndexer(columnNames, seperator);
    }

    //add by csd
    public bool isAllColumnNameExists(List<string> lKeyLst) {
        bool isAll = true;
        int columnIndex;
        for (int i = 0; i < lKeyLst.Count; i++)
        {
            string tmpKeyName = lKeyLst[i];
            try
            {
                columnIndex = m_columnNameIndexer.GetColumnIndex(tmpKeyName);
            }
            catch
            {
                isAll = false;
            }
        }
        return isAll;
    }
    //add end

    public int RowCount
    {
        get { return m_datas.Count; }
    }

    public void Add(CSVRow tmpRow) {
        m_datas.Add(tmpRow);
    }

    public void Add(string row, char seperator, Encoding encoding)
    {
        var csvRow = new CSVRow(row, m_columnNameIndexer, seperator, encoding);
        m_datas.Add(csvRow);
    }

    public CSVRow GetRow(int i)
    {
        return m_datas[i];
    }

    public IEnumerator<CSVRow> GetEnumerator()
    {
        return m_datas.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int ColumnCount
    {
        get { return m_columnNameIndexer.ColumnCount; }
    }
}

public class CSVColumnNameIndexer
{
    private readonly Dictionary<string, int> m_columnDic = new Dictionary<string, int>();

    public List<string> getColDicLst() {
        List<string> res = new List<string>();

        foreach (KeyValuePair<string, int> kvp in m_columnDic) {
            res.Add(kvp.Key);
        }
       
        return res;
    }

    internal CSVColumnNameIndexer(string columnNames, char seperator)
    {
        string[] columnNameArray = columnNames.Split(new[] {seperator}, StringSplitOptions.None);
        for (int index = 0; index < columnNameArray.Length; index++)
        {
            string columnName = columnNameArray[index];
#if UNITY_EDITOR
            if (m_columnDic.ContainsKey(columnName))
            {
                UnityEngine.Debug.LogError(columnName + " is exists.");    
            }
#endif
            m_columnDic.Add(columnName, index);
        }
    }


    public int GetColumnIndex(string columnName)
    {
        if (m_columnDic.ContainsKey(columnName) == false) throw new Exception("Cannot find column : " + columnName);
        return m_columnDic[columnName];
    }

    internal int ColumnCount{get { return m_columnDic.Count; }}
}

public class CSVRow
{
    private readonly CSVColumnNameIndexer m_columnNameIndexer;
    private readonly string[] m_rowDatas;

    private static readonly byte[] g_columnStr = new byte[2048];

    public CSVColumnNameIndexer GetCSVColumnNameIndexer() {
        return m_columnNameIndexer;
    }

    //add by csd copy by CSVRow
    public CSVRow(string[] values, CSVColumnNameIndexer columnNameIndexer) {
        m_columnNameIndexer = columnNameIndexer;
        m_rowDatas = new string[columnNameIndexer.ColumnCount];
        for (int i = 0; i < columnNameIndexer.ColumnCount; i++) {
            m_rowDatas[i] = values[i];
        }

    }
//end

    public CSVRow(string row, CSVColumnNameIndexer columnNameIndexer, char seperator, Encoding encoding)
    {
        m_columnNameIndexer = columnNameIndexer;

        m_rowDatas = new string[columnNameIndexer.ColumnCount];
        int rowCount = 0;

        int columnByteCount = 0;
        bool inColumnBlock = false;
        bool specialCharOfFront = false;

        var rowBytes = encoding.GetBytes(row);

        for (int i = 0; i < rowBytes.Length; ++i)
        {
            if (rowCount >= m_rowDatas.Length) break;

            var oneChar = rowBytes[i];

            //
            if (specialCharOfFront == false && IsSpecialChar(oneChar))
            {
                specialCharOfFront = true;
                continue;
            }

            if (specialCharOfFront)
            {
                if (IsSpecialChar(oneChar) == false) inColumnBlock = !inColumnBlock;
                specialCharOfFront = false;
            }

            //
            if (inColumnBlock)
            {
                g_columnStr[columnByteCount++] = oneChar;
            }
            //
            else
            {
                if (IsSeperatorChar(seperator, oneChar)) AddRowData(g_columnStr, ref columnByteCount, ref rowCount, encoding);
                else g_columnStr[columnByteCount++] = oneChar;
            }
        }
        if (rowCount <= m_rowDatas.Length) AddRowData(g_columnStr, ref columnByteCount, ref rowCount, encoding);

        //m_rowDatas = Regex.Split(row, seperator + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        //for (int i = 0; i < m_rowDatas.Length; i++)
        //{
        //    string data = m_rowDatas[i];
        //    if (data.EndsWith("\""))
        //    {
        //        data = data.Remove(data.Length - 1);
        //    }
        //    if (data.StartsWith("\""))
        //    {
        //        data = data.Remove(0, 1);
        //    }
        //    m_rowDatas[i] = Regex.Replace(data, "\"\"", "\"");
        //}
    }

    private static bool IsSpecialChar(byte oneChar)
    {
        return oneChar == '\"';
    }

    private static bool IsSeperatorChar(char seperator, byte oneChar)
    {
        return oneChar == seperator;
    }

    private void AddRowData(byte[] columnStr, ref int columnByteCount, ref int rowCount, Encoding encoding)
    {
        m_rowDatas[rowCount++] = encoding.GetString(columnStr, 0, columnByteCount);
        columnByteCount = 0;
    }

    public int GetInt(string columnName)
    {
        string rowData = GetString(columnName);
        try
        {
            return int.Parse(rowData);
        }
        catch
        {
            UnityEngine.Debug.LogError(string.Format("Format Exception {0} : {1}",columnName, rowData));
        }
        return -1;
    }

    public short GetShort(string columnName)
    {
        string rowData = GetString(columnName);
        return short.Parse(rowData);
    }

    public byte GetByte(string columnName)
    {
        string rowData = GetString(columnName);
        return byte.Parse(rowData);
    }

    public float GetFloat(string columnName)
    {
        string rowData = GetString(columnName);
        try
        {
            return float.Parse(rowData);
        }
        catch
        {
            UnityEngine.Debug.LogError(string.Format("Format Exception {0} : {1}", columnName, rowData));
        }
        return float.NaN;
    }

    public double GetDouble(string columnName)
    {
        string rowData = GetString(columnName);
        try
        {
            return double.Parse(rowData);
        }
        catch
        {
            UnityEngine.Debug.LogError(string.Format("Format Exception {0} : {1}", columnName, rowData));
        }
        return double.NaN;
    }

    public string GetString(string columnName)
    {
        int columnIndex = m_columnNameIndexer.GetColumnIndex(columnName);
        return m_rowDatas[columnIndex];
    }

    //add by csd begin
    public void SetString(string columnName, string columnVal)
    {
        int columnIndex = m_columnNameIndexer.GetColumnIndex(columnName);
        m_rowDatas[columnIndex] = columnVal;
    }

    //add end

    public uint GetUInt(string columnName)
    {
        string rowData = GetString(columnName);
        return uint.Parse(rowData);
    }

    public bool GetBool(string columnName)
    {
        string rowData = GetString(columnName);
        if (string.IsNullOrEmpty(rowData)) return false;
        return int.Parse(rowData) != 0;
    }

    //add by csd
    public void SetBool(string columnName, bool columnVal) {
        int columnIndex = m_columnNameIndexer.GetColumnIndex(columnName);

        m_rowDatas[columnIndex] = (columnVal ? "1" : "0");
    }
    //add end

    public ulong GetULong(string columnName)
    {
        string rowData = GetString(columnName);
        return ulong.Parse(rowData);
    }

    public long GetLong(string columnName)
    {
        string rowData = GetString(columnName);
        return long.Parse(rowData);
    }

    public T GetEnum<T>(string columnName)
    {
        string rowData = GetString(columnName);
        return (T)Enum.Parse(typeof(T), rowData);
    }

    //add by csd
    public T GetVal<T>(string columnName,T defVal) {
        string rowData = GetString(columnName);
        T res = defVal;
        try
        {
            res = (T)Convert.ChangeType(rowData, typeof(T));
        }
        catch
        {
        }
        return res;
    }
    //add by csd end
    //safe

    public int SafeGetInt(string columnName, int defaultValue=0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        int result;
        if (int.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public short SafeGetShort(string columnName, short defaultValue = 0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        short result;
        if (short.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public float SafeGetFloat(string columnName, float defaultValue = 0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        float result;
        if (float.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public double SafeGetDouble(string columnName, double defaultValue=0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        double result;
        if (double.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public string SafeGetString(string columnName, string defaultValue="")
    {
        if (Exist(columnName) == false) return defaultValue;
        return GetString(columnName);
    }

    public uint SafeGetUInt(string columnName, uint defaultValue=0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        uint result;
        if (uint.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public bool SafeGetBool(string columnName, bool defaultValue=false)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        bool result;
        if (bool.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }

    public ulong SafeGetULong(string columnName, ulong defaultValue=0)
    {
        if (Exist(columnName) == false) return defaultValue;
        string rowData = GetString(columnName);

        ulong result;
        if (ulong.TryParse(rowData, out result) == false) return defaultValue;
        return result;
    }
    public T SafeGetEnum<T>(string columnName, T defaultValue)
    {
        if (Exist(columnName) == false) return defaultValue;

        string rowData = GetString(columnName);
        if (Enum.IsDefined(typeof(T), rowData) == false) return defaultValue;

        return GetEnum<T>(columnName);
    }

    public T SafeGetEnum<T>(string columnName, T defaultValue, out bool success)
    {
        success = false;
        if (Exist(columnName) == false)
        {
            return defaultValue;
        }

        string rowData = GetString(columnName);
        if (Enum.IsDefined(typeof (T), rowData) == false)
        {
            return defaultValue;
        }

        success = true;
        return GetEnum<T>(columnName);
    }

    public bool Exist(string columnName)
    {
        int columnIndex;
        try
        {
            columnIndex = m_columnNameIndexer.GetColumnIndex(columnName);
        }
        catch
        {
            return false;
        }

        if (columnIndex >= m_rowDatas.Length) return false;
        return string.IsNullOrEmpty(m_rowDatas[columnIndex]) == false;
    }
}
