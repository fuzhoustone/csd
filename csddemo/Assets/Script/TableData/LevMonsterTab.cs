using System;
using System.Collections.Generic;
using System.IO;

public class LevMonsterTab 
{
    public class levMonster
    {
        public int ID;
        public int RoleID;
        public int LvMin;
        public int LvMax;
        public levMonster(int id, int roleid, int lvmin, int lvmax)
        {
            ID = id;
            RoleID = roleid;
            LvMin = lvmin;
            LvMax = lvmax;
        }
    }

    public static List<levMonster> m_elements = new List<levMonster>();
    public static List<int> lvRoleLst = new List<int>();

    private const string _ID = "ID";
    private const string _roleID = "RoleID";
    private const string _lvMin = "lvMin";
    private const string _lvMax = "lvMax";

    private static Random randomID;

    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _roleID,
                       _lvMin,
                       _lvMax
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
            var roleid = row.GetInt(_roleID);
            var lvMin = row.GetInt(_lvMin);
            var lvMax = row.GetInt(_lvMax);

            var tmprow = new levMonster(id, roleid, lvMin, lvMax);
            m_elements.Add(tmprow);
        }
    }


    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static levMonster GetFromRoleID(int id)
    {
        for (int i = 0; i < m_elements.Count; ++i)
        {
            if (m_elements[i].ID == id)
                return m_elements[i];
        }
        return null;
    }

    public static void initMonsterIDFromLev(int lv) {
        //List<int> lvLst = new List<int>();
        lvRoleLst.Clear();
        for (int i = 0; i < m_elements.Count; ++i)
        {
            if ((m_elements[i].LvMin <= lv) && (lv <= m_elements[i].LvMax))
                lvRoleLst.Add(m_elements[i].RoleID);
        }

        int ranSeed = System.DateTime.Now.Second;
        randomID = new Random(ranSeed);
        //   return lvLst;
    }

    public static int getRandomMonsterID() {
        
        int nMax = lvRoleLst.Count;
        int index = randomID.Next(0, nMax-1);
        int monsterID = lvRoleLst[index];

        return monsterID;
    }

    public static void Clear()
    {
        m_elements.Clear();
    }
}
