using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class roleAI
{
    public int selfRoleID;
    public int enemyRoleID;
    public List<CSVRow> roleFriLst;

    public void initParam(int roleID) {
        selfRoleID = roleID;
        roleFriLst = new List<CSVRow>();
        int nCount = roleFriendTab._instance().GetTableLength();
        for (int i = 0; i < nCount; i++) {
           CSVRow tmpRow = roleFriendTab._instance().GetRowFromIndex(i);
           int tmpRoleID = tmpRow.GetInt(roleFriendTab.csRoleID);
           if (tmpRoleID == selfRoleID) {
                roleFriLst.Add(tmpRow);
           }
        }

    }

    //获得敌对值最高的ID
    public int getEnemyID() {
        int resRoleID = -1;
        int val = 0; //表示中立
        for (int i = 0; i < roleFriLst.Count; i++) {
            CSVRow tmpRow = roleFriLst[i];
            int tmpRoleID = tmpRow.GetInt(roleFriendTab.csTargetID);
            int tmpval = tmpRow.GetInt(roleFriendTab.csValue);
            if (val >= tmpval) {
                val = tmpval;
                resRoleID = tmpRoleID;
            }
        }

        if (val < 0)
            enemyRoleID = resRoleID;
        else
            enemyRoleID = -1;

        return resRoleID;
    }

    //攻击敌人
    public void attackEnemy() {
        if (enemyRoleID > 0) { 
            
        }
    }

    //受到攻击
    public void defAttack(int talkID, int roleID) { 
        
    }
    

}
