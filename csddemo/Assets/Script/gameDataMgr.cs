//using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


//using UnityEngine.Events;

    /*
[System.Serializable]
public class bossCanUse
{
    [SerializeField]
    public int id { get; set; }
    [SerializeField]
    public bool canUse { get; set; }
}
*/
[System.Serializable]
public class bossTag
{
    [SerializeField]
    public List<string> useLst;

    public bossTag()
    {
        this.useLst = new List<string>();
    }

    public void addBossUse(int pID)
    {
       // bossCanUse tmpUse = new bossCanUse();

       // tmpUse.id = pID;
       // tmpUse.canUse = true;
        useLst.Add(pID.ToString());
    }

    public bool canUseById(int id)
    {
        bool res = false;
        
        if (useLst.IndexOf(id.ToString()) >= 0)
            res = true;
        /*
        //int nCount = m_bossTag.bossUseLst.Count;
        for (int i = 1; i <= useLst.Count; i++)
        {
            int tmpID = useLst[i - 1];
            //bossCanUse tmpTag = useLst[i-1];
            if (tmpID == id)
            {
                res = true;
                break;
            }
        }
        */
        return res;
    }


}
public class gameDataMgr
{


    public const int csRoleNum = 21; //实际应该用21
    //魔物当前的剩余血量
    public class roleData {
        public int mazeLevel; //关卡等级
       // public int roleLevel; //人数等级
        public int rewardNum; //奖励个数
        public int[] bosshp = new int[csRoleNum];
       
    }
    /*
    [System.Serializable]
    public class bossHpPro {
        public int id;
        public int hp;
    }
    */
    private static gameDataMgr instance = null;
    public bossTag m_bossTag;
    public roleData m_roleData;
    private string m_ModelFileName = ""; //存放图签解锁的文件
    private string m_RoleFileName = ""; //存放通关记录的文件

    private bool findRecord = false;
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

        m_bossTag = new bossTag();
        m_roleData = new roleData();
    }

    private void initRoleData() {
        m_roleData = new roleData();
        m_roleData.rewardNum = 0;
        m_roleData.mazeLevel = 1;
        m_roleData.bosshp = new int[csRoleNum];
        for (int i = 0; i < csRoleNum; i++) {
            m_roleData.bosshp[i] = 100;
            //m_roleData.bosshp[i].id = i + 1;
            //m_roleData.bosshp[i].hp = 100;
        }
    }

    private void initModelData() {
        m_bossTag = new bossTag();
    }


    //保存图签解锁
    private void saveModelData()
    {

        
        //if (m_bossTag.useLst.Count > 0)
        //{
            string[] jsonStr = m_bossTag.useLst.ToArray();
            
            File.WriteAllLines(m_ModelFileName, jsonStr);
        //}

          //  File.WriteAllText(m_ModelFileName, "");
    }


    private void saveRoleData() {
        string jsonStr = JsonUtility.ToJson(m_roleData);
        File.WriteAllText(m_RoleFileName, jsonStr);
        findRecord = true;
    }


    private void LoadModelData(string[] itemsJson)
    {
        if (itemsJson.Length > 0)
            m_bossTag.useLst = new List<string>(itemsJson);

    }

    //加载所有数据
    private void loadAllData()
    {
        if (File.Exists(m_ModelFileName))
        {
            string[] jsonStr = File.ReadAllLines(m_ModelFileName);
            LoadModelData(jsonStr);
            //m_bossTag.readData(jsonStr);
            //m_bossTag = JsonUtility.FromJson<bossTag>(jsonStr);
            // m_bossTag = JsonUtility.FromJson<Serialization<bossTag>>(jsonStr).target;

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
            findRecord = true;
        }
        else
        {
            
            initRoleData();
            saveRoleData();
            findRecord = false;
        }

        //testEnemy tmpTest = new testEnemy();
        //tmpTest.testData();
    }

    //记录当前主角等级
    //public void saveRoleLevel(int pLevel) {
    //    saveRoleData();
    //}
    /*
        public bool canUseById(int id) {
            bool res = false;
            //int nCount = m_bossTag.bossUseLst.Count;
            for (int i = 1; i <= m_bossTag.bossUseLst.Count; i++) {
                bossCanUse tmpTag = m_bossTag.bossUseLst[i];
                if (tmpTag.id == id) {
                    res = true;
                    break;
                }
            }

            return res;
        }
    */
    //记录成就点个数
    public void saveRewardNumData(int num) {
        m_roleData.rewardNum = num;
        saveRoleData();
    }



    public bool costRewardNum(int pCost, int pRoleID) {
        bool res = false;
        if (m_roleData.rewardNum >= pCost)
        {
            m_roleData.rewardNum -= pCost;
            //m_roleData.bosshp[pRoleID] = true;
            //m_bossTag.bossUse[pRoleID] = true;

            m_bossTag.addBossUse(pRoleID);

            saveRoleData();
            saveModelData();
            res = true;
        }

        return res;
    }

    public void saveNextLevelData() {
        m_roleData.mazeLevel++;
        saveRoleData();
    }

    public void saveNewGameLevelData() {
        m_roleData.mazeLevel = 1;
        saveRoleData();
    }

    /*
    //记录当前通关数
    public void saveLevelData(int pLevel) {
        m_roleData.mazeLevel = pLevel;
        saveRoleData();
    }
    */
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

    
    public bool hasRecord() {
               
        return findRecord;
    }
    
}
