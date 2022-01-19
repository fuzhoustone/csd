using System;
using System.Collections.Generic;
using System.IO;

    public class CsdTTable<T>
    {
        public static List<CSVRow> m_elements = new List<CSVRow>();
        //  public static List<CsvKeyName> mKeyNameLst;
        private const string csID = "ID";

        public static void Load(Stream stream)
        {
            if (stream == null) return;
            CSVData data = CSVLoader.Load(stream);
            for (int i = 0; i < data.RowCount; ++i)
            {
                var row = data.GetRow(i);
                m_elements.Add(row);
            }
        }

        public static int GetTableLength()
        {
            return m_elements.Count;
        }

        public static CSVRow GetRowFromID(int id)
        {
            for (int i = 0; i < m_elements.Count; ++i)
            {
                if (m_elements[i].GetInt(csID) == id)
                    return m_elements[i];
            }
            return null;
        }

        /*
         * 调用： GetRowFromID<roleInfoRow>(类的对象);
         * 传入类名CSVRow，类名2，CSVRow的对象，
         * 返回类名2的对象
        GetRowFromID<T>



        public static void GetRowFromIDT(int id, out T resVal) {
            T outVal = (T)Enum.Parse(typeof(T), "");
            resVal = outVal;
        }
        */
        private static bool GetKeyValue(string keyName, T keyVal, string returnName, out T resVal)
        {
            bool isFind = false;
            resVal = m_elements[0].GetEnum<T>(keyName);
            for (int i = 0; i < m_elements.Count; ++i)
            {
                T tmpVal = m_elements[i].GetEnum<T>(keyName);
                if (object.Equals(tmpVal, keyVal))
                {
                    isFind = true;
                    resVal = m_elements[i].GetEnum<T>(returnName);
                    break;
                }
            }

           // resVal = outVal;
            return isFind;
        }

        public static bool GetKeyString(string keyName, T keyVal, string returnName, out string resVal)
        {
            T tmpVal;
            bool isFind = GetKeyValue(keyName, keyVal, returnName, out tmpVal);
            if (isFind)
                resVal = tmpVal.ToString();
            else
                resVal = "0";
            return isFind;
        }

        public static bool GetKeyUInt(string keyName, T keyVal, string returnName, out uint resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = uint.Parse(tmpVal);
            return isFind;
        }

        public static bool GetKeyBool(string keyName, T keyVal, string returnName, out bool resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = int.Parse(tmpVal) != 0;
            return isFind;
        }

        public static bool GetKeyULong(string keyName, T keyVal, string returnName, out ulong resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = ulong.Parse(tmpVal);
            return isFind;
        }

        public static bool GetKeyLong(string keyName, T keyVal, string returnName, out long resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = long.Parse(tmpVal);
            return isFind;
        }

        public static bool GetKeyDouble(string keyName, T keyVal, string returnName, out double resVal)
        {
            string rowData;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out rowData);
            try
            {
                resVal = double.Parse(rowData);
            }
            catch
            {
                UnityEngine.Debug.LogError(string.Format("Format Exception {0} : {1}", keyName, rowData));
            }
            resVal = double.NaN;

            return isFind;
        }

        public static bool GetKeyFloat(string keyName, T keyVal, string returnName, out float resVal)
        {
            string rowData;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out rowData);
            try
            {
                resVal = float.Parse(rowData);
            }
            catch
            {
                UnityEngine.Debug.LogError(string.Format("Format Exception {0} : {1}", keyName, rowData));
            }
            resVal = float.NaN;

            return isFind;
        }

        public static bool GetKeyShort(string keyName, T keyVal, string returnName, out short resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = short.Parse(tmpVal);
            return isFind;
        }

        public static bool GetKeyByte(string keyName, T keyVal, string returnName, out byte resVal)
        {
            string tmpVal;
            bool isFind = GetKeyString(keyName, keyVal, returnName, out tmpVal);
            resVal = byte.Parse(tmpVal);
            return isFind;
        }

        public static void Clear()
        {
            m_elements.Clear();
        }
        /*
        public static void keyInit(string[] nameLst)
        {
            mKeyNameLst = new List<CsvKeyName>();
            int nCount = nameLst.Length;
            for (int i = 0; i < nCount; i++)
            {
                string nameVal = nameLst[i];
                CsvKeyName tmpKeyName = new CsvKeyName();
                tmpKeyName.keyName = nameVal;
                tmpKeyName.keyVal = "";
            }
        }
        */
    }

