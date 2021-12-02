using System;
using System.Collections.Generic;
using System.IO;

public class BossInfoTable  {

    public class bossElements
    {
        public int ID;
        public string Name;
        public string Des;
        public string Pic;
		public int Cost;       

        public bossElements(int id, string name,string des,string pic, int cost)
        {
            ID = id;
            Name = name;
            Des = des;
			Pic = pic;
            Cost = cost;
        }
    }

    public static List<bossElements> m_elements = new List<bossElements>();
   
    private const string _ID = "ID";
    private const string _Name = "Name";
    private const string _Des = "Des";
    private const string _Pic = "Pic";
	private const string _Cost = "Cost";
    
    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _Name,
                       _Des,
                       _Pic,
                       _Cost
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
            var pic = row.GetString(_Pic);
            var cost = row.GetInt(_Cost);
         

            var element = new bossElements(id, name, des, pic, cost);
            m_elements.Add(element);


        }
    }
  
  
    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static bossElements Get(int id)
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
