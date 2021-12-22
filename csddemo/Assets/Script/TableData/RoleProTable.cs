using System;
using System.Collections.Generic;
using System.IO;

public class RoleProTable  {

    public class rolePro
    {
        public int ID;
        public int Atk;
        public int Def;
        public int MaxHp;
		public int Ele;
       // public int MaxHp;
        public rolePro(int id, int atk, int def, int hp, int ele)
        {
            ID = id;
            Atk = atk;
            Def = def;
            MaxHp = hp;
            Ele = ele;
        }
    }

    public static List<rolePro> m_elements = new List<rolePro>();
   
    private const string _ID = "ID";
    private const string _Atk = "Atk";
    private const string _Def = "Def";
    private const string _MaxHp = "Hp";
    private const string _Ele = "Ele";
    
    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _Atk,
                       _Def,
                       _MaxHp,
                       _Ele
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
            var atk = row.GetInt(_Atk);
            var def = row.GetInt(_Def);
            var hp = row.GetInt(_MaxHp);
            var ele = row.GetInt(_Ele);

            var tmprow = new rolePro(id, atk, def, hp, ele);
            m_elements.Add(tmprow);
        }
    }
  
  
    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static rolePro Get(int id)
    {
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
