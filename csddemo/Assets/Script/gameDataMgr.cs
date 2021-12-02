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
    public class bossTag {
        public int[] Boar = new int[4];
        public int[] Night = new int[4];
        public int[] Soul = new int[4];
        public int[] Terror = new int[4];
        public int[] Usurper = new int[4];
    }

    public class roleData {
        public int mazeLevel; //关卡等级
        public int roleLevel; //人数等级
        public int rewardNum; //奖励个数
    }

    private static gameDataMgr instance = null;
    private bossTag m_bossTag;
    private roleData m_roleData;
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
        m_roleData.roleLevel = 1;
    }

    private void initModelData() {
        m_bossTag = new bossTag();
        for (int i = 0; i < 4; i++) {
            m_bossTag.Boar[i] = 0;
            m_bossTag.Night[i] = 0;
            m_bossTag.Soul[i] = 0;
            m_bossTag.Terror[i] = 0;
            m_bossTag.Usurper[i] = 0;

        }
    }
    private void saveRoleData() {
        string jsonStr = JsonUtility.ToJson(m_roleData);
        File.WriteAllText(m_RoleFileName, jsonStr);
    }

    //记录当前主角等级
    public void saveRoleLevel(int pLevel) {
        m_roleData.roleLevel = pLevel;
        saveRoleData();
    }
    
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


    //保存图签解锁
    public void saveModelData() {
        
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
        if (File.Exists(m_RoleFileName))
        {
            string jsonStr = File.ReadAllText(m_RoleFileName);
            m_roleData = JsonUtility.FromJson<roleData>(jsonStr);
        }
        else {
            initRoleData();
            saveRoleData();
        }

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

    }


    /*内部数据加载*/


}
