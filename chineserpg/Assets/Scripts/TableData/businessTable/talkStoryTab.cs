using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkStoryTab : CsdTTable
{
   
    private static talkStoryTab instance = null;
    public static talkStoryTab _instance()
    {
        if (instance == null)
        {
            instance = new talkStoryTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csNextID = "nextID";
    public const string csTalkTxtCn = "talkTxtCn";
    public const string csTalkTxtEn = "talkTxtEn";
    public const string csContentCn = "contentCn";
    public const string csContentEn = "contentEn";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csNextID);
        addKeyName(csTalkTxtCn);
        addKeyName(csTalkTxtEn); 
        addKeyName(csContentCn);
        addKeyName(csContentEn);
    }
}
