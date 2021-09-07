using System;
using System.Collections.Generic;
using System.IO;

public class PopFirstChargeTable  {

    public class PopElements
    {
        public int ID;
        public int Priority;
        public string Des;
        public string BackGroundURL;
        public string Jumpto;
        public string EventIntervalStart;
        public string EventIntervalEnd;
       
        public string PlacementKey;

        public PopElements(int id, string url,
                                   int priority, string des, string jumpto, string placementKey, string eventIntervalStart, string eventIntervalEnd)
        {
            ID = id;
            BackGroundURL = url;
            Priority = priority;
            Des = des;
            Jumpto = jumpto;
            PlacementKey = placementKey;
            EventIntervalStart = eventIntervalStart;
            EventIntervalEnd = eventIntervalEnd;

        }
    }
    public static bool Over;
  //  public static DontShowEventHistory m_eventHistory = new DontShowEventHistory();
    public static List<PopElements> m_elements = new List<PopElements>();
   
    private const string _ID = "ID";
    private const string _Des = "Des";
    private const string _BackgroundURL = "Background_URL";
    private const string _Jumpto = "Jumpto";
    private const string _Priority = "Priority";
    private const string _EventIntervalStart = "EventIntervalStart";
    private const string _EventIntervalEnd = "EventIntervalEnd";
    private const string _PlacemetKey = "PlacemetKey";
    public static string[] ColumnNames
    {
        get
        {
            return new string[]
                   {
                       _ID,
                       _Des,
                       _BackgroundURL,
                       _Jumpto,
                       _Priority,
                       _EventIntervalStart,
                       _EventIntervalEnd,
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
            var background = row.GetString(_BackgroundURL);
            var des = row.GetString(_Des);
            var Jumpto = row.GetString(_Jumpto);
            var Priority = row.GetInt(_Priority);
            var EventIntervalEnd = row.GetString(_EventIntervalEnd);
            var EventIntervalStart = row.GetString(_EventIntervalStart);
            var placementKey = row.GetString(_PlacemetKey);
          

            var element = new PopElements(id, background, Priority, des, Jumpto, placementKey, EventIntervalStart, EventIntervalEnd);
            m_elements.Add(element);


        }
    }
  
  
  
    public static bool IsOpenTime(PopElements m_elements) {

        DateTime now = DateTime.Now;
        DateTime start = DateTime.Parse(m_elements.EventIntervalStart);
        DateTime end = DateTime.Parse(m_elements.EventIntervalEnd);
        int a = DateTime.Compare(now, start);
        int b = DateTime.Compare(end, now);
        if (a > 0 && b > 0 )
        {

            return true;
        }
        return false;
    }
  
	public static List<PopElements> GetCheckOpenTimeListDate(){
		var list = new List<PopElements>();

		foreach (var go in m_elements)
		{
			if (!IsOpenTime(go)) {

				continue;
			}
			list.Add(go);
		}


		return list;
	}

    public static List<PopElements> GetCheckListDate(bool CheckOpenTime,bool CheckTodayShow,int pro) {

        var list = new List<PopElements>();

        foreach (var go in m_elements)
        {
            if (CheckOpenTime) {

                if (!IsOpenTime(go)) {

                    continue;
                }


            }
            if (CheckTodayShow)
            {


            }
            if (pro!=go.Priority) {

                continue;

            }
            list.Add(go);
        }


        return list;


    }
    public static int GetTableLength()
    {
        return m_elements.Count;
    }

    public static PopElements Get(int id)
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
