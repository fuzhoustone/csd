
public class StoryVideoTab : CsdTTable
{
    private static StoryVideoTab instance = null;
    public static StoryVideoTab _instance()
    {
        if (instance == null)
        {
            instance = new StoryVideoTab();
            instance.initParam();
        }
        return instance;
    }

    public const string csFileName = "videoFileName"; // { get {return "nextID";} }
    public const string csOptionID = "optionID";
    public const string csSceneName = "sceneName";


    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csFileName);
        addKeyName(csOptionID);
        addKeyName(csSceneName);
    }
}
