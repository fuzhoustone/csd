using System;
using System.Collections.Generic;
using System.IO;

public class StoryBgSceneRelationTab : CsdTTable
{
    private static StoryBgSceneRelationTab instance = null;
    public static StoryBgSceneRelationTab _instance()
    {
        if (instance == null)
        {
            instance = new StoryBgSceneRelationTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csStoryID = "storyID"; 
    public const string csSceneID = "sceneID"; 
    public const string csRoleID = "roleID"; 
    public const string csRoleFace = "roleFace";
    public const string csEffectID = "effectID";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csStoryID);
        addKeyName(csSceneID);
        addKeyName(csRoleID);
        addKeyName(csRoleFace);
        addKeyName(csEffectID);
    }
}
