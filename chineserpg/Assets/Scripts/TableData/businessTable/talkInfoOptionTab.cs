using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkInfoOptionTab : CsdTTable
{
    private static talkInfoOptionTab instance = null;
    public static talkInfoOptionTab _instance()
    {
        if (instance == null)
        {
            instance = new talkInfoOptionTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csTalkID = "talkID";
    public const string csStoryID = "storyID";
    public const string csOptionCn = "optionCn";
    public const string csOptionEn = "optionEn";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csTalkID);
        addKeyName(csStoryID);
        addKeyName(csOptionCn);
        addKeyName(csOptionEn);
    }

    public class talkOptionObj
    {
        public int optionID { get; set; }
        public string optionStrCn { get; set; }
        public string optionStrEn { get; set; }
        public int stroyID { get; set; }
    }

    public List<talkOptionObj> getOptionLst(int lId)
    {
        List<talkOptionObj> res = new List<talkOptionObj>();

        for (int i = 0; i < m_elements.Count; ++i)
        {
            if (m_elements[i].GetInt(csTalkID) == lId)
            {
                talkOptionObj tmpObj = new talkOptionObj();
                tmpObj.optionID = m_elements[i].GetInt(csID);
                tmpObj.stroyID = m_elements[i].GetInt(csStoryID);
                tmpObj.optionStrCn = m_elements[i].GetString(csOptionCn);
                tmpObj.optionStrEn = m_elements[i].GetString(csOptionEn);

                res.Add(tmpObj);
            }
        }
        return res;
    }
}
