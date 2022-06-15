using System;
using System.Collections.Generic;
using System.IO;

public class StoryBgSceneRelationTab : CsdTTable
{
    public const string csStoryID = "storyID";
    public const string csSceneID = "sceneID";
    public const string csRoleID = "roleID";
    public const string csRoleFace = "roleFace";
    public const string csEffectID = "effectID";

    private static StoryBgSceneRelationTab instance = null;
    public static StoryBgSceneRelationTab _instance()
    {
        if (instance == null)
        {
            instance = new StoryBgSceneRelationTab();
        }
        return instance;
    }
}
