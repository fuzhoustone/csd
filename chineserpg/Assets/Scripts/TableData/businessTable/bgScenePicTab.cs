using System;
using System.Collections.Generic;
using System.IO;

public class bgScenePicTab : CsdTTable
{
    public const string csScenePic = "scenePic";

    private static bgScenePicTab instance = null;
    public static bgScenePicTab _instance()
    {
        if (instance == null)
        {
            instance = new bgScenePicTab();
        }
        return instance;
    }
}
