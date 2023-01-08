using System.Collections.Generic;

public class storyOptionTab : CsdTTable
{
    private static storyOptionTab instance = null;
    public static storyOptionTab _instance()
    {
        if (instance == null)
        {
            instance = new storyOptionTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csStoryID  ="storyID";
    public const string csNextStoryID ="nextStoryID";
    public const string csVideoID = "videoID";
    public const string csOptionCn = "optionCn";  
    public const string csOptionEn = "optionEn"; 
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csStoryID);
        addKeyName(csNextStoryID);
        addKeyName(csOptionCn);
        addKeyName(csOptionEn);
    }
  
    public class optionObj {
        public string optionStrCn { get; set;}
        public string optionStrEn { get; set; }
        public int nextStoryID { get; set; }

        public int noteID { get; set; }
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
                tmpObj.noteID = 0; // m_elements[i].GetInt(csVideoID);

                res.Add(tmpObj);
            }
        }
        return res;
    }

}
