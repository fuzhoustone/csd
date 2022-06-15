using System;
using System.Collections.Generic;
using System.IO;

public class talkOptionTab : CsdTTable
{
    public const string csStoryID = "storyID";
    public const string csNextStoryID = "nextStoryID";
    public const string csOptionCn = "optionCn";
    public const string csOptionEn = "optionEn";

    private static talkOptionTab instance = null;
    public static talkOptionTab _instance()
    {
        if (instance == null)
        {
            instance = new talkOptionTab();
        }
        return instance;
    }
    public class optionObj {
        public string optionStrCn { get; set;}
        public string optionStrEn { get; set; }
        public int nextStoryID { get; set; }
    }

    public List<optionObj> getOptionLst(int lId) {
        List<optionObj> res = new List<optionObj>();

        for (int i = 0; i < m_elements.Count; ++i)
        {
            if (m_elements[i].GetInt(csStoryID) == lId)
            {
                optionObj tmpObj = new optionObj();
                tmpObj.nextStoryID = m_elements[i].GetInt(csNextStoryID);
                tmpObj.optionStrCn = m_elements[i].GetString(csOptionCn);
                tmpObj.optionStrEn = m_elements[i].GetString(csOptionEn);

                res.Add(tmpObj);
            }
               // return m_elements[i];
        }
        return res;
    }

}
