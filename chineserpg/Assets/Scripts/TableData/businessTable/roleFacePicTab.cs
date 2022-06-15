using System;
using System.Collections.Generic;
using System.IO;

public class roleFacePicTab : CsdTTable
{
    public const string csRoleID = "roleID";
    public const string csFaceID = "faceID";
    public const string csPicPath = "picPath";

    private static roleFacePicTab instance = null;
    public static roleFacePicTab _instance()
    {
        if (instance == null)
        {
            instance = new roleFacePicTab();
        }
        return instance;
    }
}
