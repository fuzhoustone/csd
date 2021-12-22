using System;
using System.Collections.Generic;
using System.IO;

public class ShopItemTable
{
    public class shopElements
    {
        public int ID;
        public string Pic;
        public int Cost;

        public shopElements(int id, string pic, int cost)
        {
            ID = id;
            Pic = pic;
            Cost = cost;
        }
    }

    public static List<shopElements> m_elements = new List<shopElements>();

    private const string _ID = "ID";
    private const string _Pic = "Pic";
    private const string _Cost = "Cost";

    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _Pic,
                       _Cost,
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
            var pic = row.GetString(_Pic);
            var cost = row.GetInt(_Cost);

            var tmprow = new shopElements(id, pic, cost);
            m_elements.Add(tmprow);


        }
    }


    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static shopElements Get(int id)
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
