
public class eventSystemTypeTab : CsdTTable
{
    public const string csEventSystemNote = "eventSystemNote";
    
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csEventSystemNote);
    }
    private static eventSystemTypeTab instance = null;
    public static eventSystemTypeTab _instance()
    {
        if (instance == null)
        {
            instance = new eventSystemTypeTab();
            instance.initParam();
        }
        return instance;
    }
}
