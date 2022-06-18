
public class bgScenePicTab : CsdTTable
{
    public const string csScenePic = "scenePic";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csScenePic);
    }
    private static bgScenePicTab instance = null;
    public static bgScenePicTab _instance()
    {
        if (instance == null)
        {
            instance = new bgScenePicTab();
            instance.initParam();
        }
        return instance;
    }
}
