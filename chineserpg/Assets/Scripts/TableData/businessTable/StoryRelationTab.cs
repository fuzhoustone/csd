using System;
using System.Collections.Generic;
using System.IO;

public class StoryRelationTab : CsdTTable
{  
    public const string csNextID = "nextID";
    public const string csUISort = "uiSort";
    public const string csContentCN = "content_cn";
    public const string csContentEn = "content_en";
    public const string csIsRoleSay = "isRoleSay";
    public const string csNeedChangeBg = "needchangeBg";
    public const string csIsAutoSave = "isSave"; //自动保存的存档点
    public const string csIsKeySave = "isKeySave"; //线索的记录点，必定要另外自动保存

    private static StoryRelationTab instance = null;
    public static StoryRelationTab _instance()
    {
        if (instance == null)
        {
            instance = new StoryRelationTab();
        }
        return instance;
    }
}
