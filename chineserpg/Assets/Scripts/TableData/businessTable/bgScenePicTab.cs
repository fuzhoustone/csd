
public class bgScenePicTab : CsdTTable
{
    public const string csPicName = "picName";
    public const string csSceneName = "sceneName";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csPicName);
        addKeyName(csSceneName);
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
