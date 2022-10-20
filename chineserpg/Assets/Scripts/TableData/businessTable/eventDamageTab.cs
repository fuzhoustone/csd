
public class eventDamageTab : CsdTTable
{
    public const string csRoleProID = "roleProID";
    public const string csDamage = "damage";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleProID);
        addKeyName(csDamage);
    }
    private static eventDamageTab instance = null;
    public static eventDamageTab _instance()
    {
        if (instance == null)
        {
            instance = new eventDamageTab();
            instance.initParam();
        }
        return instance;
    }
}
