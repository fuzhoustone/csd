

using System.Collections.Generic;

public class roleFriendTab : CsdTTable
{
    private static roleFriendTab instance = null;
    public static roleFriendTab _instance()
    {
        if (instance == null)
        {
            instance = new roleFriendTab();
            instance.initParam();
        }
        return instance;
    }
    

    //玩家获得的线索表
    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csTargetID = "targetID";
    public const string csValue = "value";

    private const string csFileName = "roleFriend.csv";

    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csTargetID);
        addKeyName(csValue);
    }

    public void LoadDefFile()
    {
        LoadFile(csFileName);
    }

    //更新某个章节各分物关系值
    public void LoadChaptChage(int lChapt) {
        int nCount = roleRelationChangeTab._instance().GetTableLength();
        for (int i = 0; i < nCount; i++) {
            CSVRow tmpRow = roleRelationChangeTab._instance().GetRowFromIndex(i);
            if (tmpRow.GetInt(roleRelationChangeTab.csChaptID) == lChapt) {
                int tmpRoleID = tmpRow.GetInt(roleRelationChangeTab.csRoleID);
                int tmpTargetID = tmpRow.GetInt(roleRelationChangeTab.csTargetID);
                int tmpValue = tmpRow.GetInt(roleRelationChangeTab.csValue);

                CSVRow friendRow = GetRowFromKey2<int, int>(csRoleID, tmpRoleID,
                                                             csTargetID, tmpTargetID
                                                             );
               int val = friendRow.GetInt(csValue);
               friendRow.SetIngeger(csValue, tmpValue + val);
            }
        }

        SaveFile();
    }

    //取各AI的友好度为负数的
    public List<CSVRow> getEnemy() {
        List<CSVRow> res = new List<CSVRow>();

        int nCount = GetTableLength();
        int actCount = roleNameTab._instance().GetTableLength();
        for (int a = 0; a < actCount; a++) { //根据角色列表，取角色的敌对值
           CSVRow tmoRole = roleNameTab._instance().GetRowFromIndex(a);
           int tmpRoleID = tmoRole.GetInt(roleNameTab.csID);
           int tmpOldFriVal = 0;
           CSVRow tmpRoleEmyRow = null;
            if (gameDataManager.instance.roleID == tmpRoleID) { //玩家自身的友好度无需计算
                continue;
            }

            //取友好度最差的一条记录
            for (int i = 0; i < nCount; i++)
            {
                CSVRow tmpFriRow = GetRowFromIndex(i);
                if (tmpFriRow.GetInt(csRoleID) == tmpRoleID) {
                    int tmpFriVal = tmpFriRow.GetInt(csValue);
                    if (tmpFriVal < tmpOldFriVal) {  //最不友好的
                        tmpRoleEmyRow = tmpFriRow;
                        tmpOldFriVal = tmpFriVal;
                    }
                }
            }
            if (tmpRoleEmyRow != null) { //有可行动的不友好对象
                res.Add(tmpRoleEmyRow);
            }

        }

        return res;
    }


/*
    public List<CSVRow> getDefEnemy() {
        return roleDefEnemyTab.m_elements;


        
    }
*/
}
