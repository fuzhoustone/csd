using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class gameDataMgr
{
    /*
     1. 对两个json文件进行读取
     2. 通关数,累积点数 不断更新
     3. 图签，需设置格式
         Boar,Night,Soul,Terror,Usurper
         */

    //魔物是否可用
    /*
    public class bossUse {
        public int id { get; set; }
        public bool canUse { get; set; }
    }

    //魔物的当前血量
    public class bossHp {
        public int id { get; set; }
        public int hp { get; set; }
    }
*/
    //魔物解锁记录
    public class bossTag
    {
        public bool[] bossUse = new bool[20];
    }

    //魔物当前的剩余血量
    public class roleData {
        public int mazeLevel; //关卡等级
       // public int roleLevel; //人数等级
        public int rewardNum; //奖励个数
        public int[] bosshp = new int[20];
    }

    private static gameDataMgr instance = null;
    public bossTag m_bossTag;
    public roleData m_roleData;
    private string m_ModelFileName = ""; //存放图签解锁的文件
    private string m_RoleFileName = ""; //存放通关记录的文件
    public static gameDataMgr gameData()
    {
        if (instance == null)
        {
            instance = new gameDataMgr();
            instance.initParam();
            instance.loadAllData();
        }
        return instance;
    }

    private const string csModelFile = "model.data";
    private const string csRoleFile = "role.data";
    private void initParam() {
        m_ModelFileName = Application.persistentDataPath + "//"+ csModelFile;
        m_RoleFileName = Application.persistentDataPath + "//" + csRoleFile;
    }

    private void initRoleData() {
        m_roleData = new roleData();
        m_roleData.rewardNum = 0;
        m_roleData.mazeLevel = 1;
        m_roleData.bosshp = new int[20];
        for (int i = 0; i < 20; i++) {
            m_roleData.bosshp[i] = 100;
            //m_roleData.bosshp[i].id = i + 1;
            //m_roleData.bosshp[i].hp = 100;
        }
    }

    private void initModelData() {
        m_bossTag = new bossTag();
        m_bossTag.bossUse = new bool[20];

        for (int i = 0; i < 20; i++) {
            m_bossTag.bossUse[i] = false;

            /*
            bossUse tmpUse = new bossUse();
            tmpUse.id = i + 1;
            tmpUse.canUse = false;
            m_bossTag.bossUse.Add(tmpUse);
            */
        }
    }
    private void saveRoleData() {
        string jsonStr = JsonUtility.ToJson(m_roleData);
        File.WriteAllText(m_RoleFileName, jsonStr);
    }

    //记录当前主角等级
    //public void saveRoleLevel(int pLevel) {
    //    saveRoleData();
    //}
    
    //记录成就点个数
    public void addRewardNumData() {
        m_roleData.rewardNum += 1;
        saveRoleData();
    }

    //记录当前通关数
    public void saveLevelData(int pLevel) {
        m_roleData.mazeLevel = pLevel;
        saveRoleData();
    }

    //清空当前通关数
    public void clearLevelData() {
        m_roleData.mazeLevel = 1;
        saveRoleData();
    }

    //返回当前通关层次
    public int getLevelNum() {
        int res = 0;
        return res;
    }

    public void deblockModel(int id) {
        m_bossTag.bossUse[id-1] = true;
        BossProTable.bossPro tmpPro = BossProTable.Get(id);
        m_roleData.bosshp[id - 1] = tmpPro.MaxHp; 
        saveModelData();
    }

    //保存图签解锁
    private void saveModelData() {
        
        string jsonStr = JsonUtility.ToJson(m_bossTag);
        File.WriteAllText(m_ModelFileName, jsonStr);
    }

    public bool hasRecord() {
        bool res = false;
        if (File.Exists(m_RoleFileName)) {
            res = true;
        }
        return res;
    }

    //加载所有数据
    private void loadAllData() {
        if (File.Exists(m_ModelFileName))
        {
            string jsonStr = File.ReadAllText(m_ModelFileName);
            m_bossTag = JsonUtility.FromJson<bossTag>(jsonStr);
        }
        else
        {
            initModelData();
            saveModelData();
        }

        if (File.Exists(m_RoleFileName))
        {
            string jsonStr = File.ReadAllText(m_RoleFileName);
            m_roleData = JsonUtility.FromJson<roleData>(jsonStr);
        }
        else
        {
            initRoleData();
            saveRoleData();
        }
    }



    /*内部数据加载*/


}
