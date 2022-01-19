using System;
using System.Collections.Generic;
using System.IO;

public class RoleInfoTable : CsdTTable<string> {
    public static string GetPrefab(int pID) {
        string res = "";
        string lPath = "";
        string lPreName = "";
        CSVRow lRow = GetRowFromID(pID);
        lPath = lRow.GetString("resPath");
        lPreName = lRow.GetString("Pre");
        res = lPath + "/" + lPreName;
        /*
        bool b1 = GetKeyString("ID",pID.ToString(), "resPath",out lPath);
        bool b2 = GetKeyString("ID", pID.ToString(), "Pre", out lPreName);
        if (b1 && b2) {
            res = lPath + "/" + lPreName;
        }*/
        return res;
    }
}
/*
public class RoleInfoTable
{

    public class roleElements
    {
        public int ID;
        public string Name;
        public string Des;
        public string Prefab;

        public roleElements(int id, string name,string des,string prefab)
        {
            ID = id;
            Name = name;
            Des = des;
            Prefab = prefab;
        }
    }

    public static List<roleElements> m_elements = new List<roleElements>();
   // public static List<T> m_elementTs = new List<T>();

    private const string _ID = "ID";
    private const string _Name = "Name";
    private const string _Des = "Des";
    private const string _Pre = "Pre";

    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _Name,
                       _Des,
                       _Pre,
                   };
        }
    }

    public static void Load(Stream stream)
    {
        if (stream == null) return;
        CSVData data = CSVLoader.Load(stream);
        for (int i = 0; i < data.RowCount; ++i)
        {
            var row = data.GetRow(i);
            var id = row.GetInt(_ID);
            var name = row.GetString(_Name);
            var des = row.GetString(_Des);
            var pre = row.GetString(_Pre);

            var tmprow = new roleElements(id, name, des, pre);
            m_elements.Add(tmprow);


        }
    }
  
  
    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static string GetPrefab(int id) {
        string pre = Get(id).Prefab;
        return pre;
    }

    public static roleElements Get(int id)
    {
		//return m_elements[id];
		for (int i = 0; i < m_elements.Count; ++i)
		{
			if (m_elements[i].ID == id) 
				return m_elements[i];
		}
		return null;
    }

    public static void Clear()
    {
        m_elements.Clear();
    }
}
*/