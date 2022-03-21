using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.Events;


[System.Serializable]
public class Serialization<T> {
    [SerializeField]
    //List<T> target;
    public T target;
    //public List<T> toList(){ return target; }
    public Serialization(T target) {
        this.target = target;
    }

}

[System.Serializable]
public class bossCanUse
{
    [SerializeField]
    public int id { get; set; }
    [SerializeField]
    public bool canUse { get; set; }
}

[System.Serializable]
public class bossTag
{
    [SerializeField]
    public List<bossCanUse> useLst = new List<bossCanUse>();

    public bossTag(List<bossCanUse> useLst)
    {
        this.useLst = useLst;
    }

    public void addBossUse(int pID)
    {
        bossCanUse tmpUse = new bossCanUse();

        tmpUse.id = pID;
        tmpUse.canUse = true;
        useLst.Add(tmpUse);
    }

    public bool canUseById(int id)
    {
        bool res = false;
        //int nCount = m_bossTag.bossUseLst.Count;
        for (int i = 1; i <= useLst.Count; i++)
        {
            bossCanUse tmpTag = useLst[i-1];
            if (tmpTag.id == id)
            {
                res = true;
                break;
            }
        }

        return res;
    }

    public string DataToJson()
    {
        string res = "";
       // res = JsonUtility.ToJson(new Serialization<bossCanUse>(useLst));
        return res;
    }

    public void readData(string pStr)
    {
      //  List<bossCanUse> useLst = JsonUtility.FromJson<Serialization<bossCanUse>>(pStr).toList();
    }

}
public class gameDataMgr
{
  

    //魔物当前的剩余血量
    public class roleData {
        public int mazeLevel; //关卡等级
       // public int roleLevel; //人数等级
        public int rewardNum; //奖励个数
        public int[] bosshp = new int[21];
       
    }

    [System.Serializable]
    public class bossHpPro {
        public int id;
        public int hp;
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
        m_bossTag = new bossTag(new List<bossCanUse>());
        //m_bossTag.bossUseLst = new List<bossCanUse>();
        //m_bossTag.bossUseLst = new bossLst();
        /*
        m_bossTag.bossUse = new bool[20];

        for (int i = 0; i < 20; i++) {
            m_bossTag.bossUse[i] = false;
        }
        */
    }
    private void saveRoleData() {
        string jsonStr = JsonUtility.ToJson(m_roleData);
        File.WriteAllText(m_RoleFileName, jsonStr);
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

    /*
    public void deblockModel(int id) {
        m_bossTag.bossUse[id-1] = true;
        RoleProTable.rolePro tmpPro = RoleProTable.GetFromRoleID(id);
        m_roleData.bosshp[id - 1] = tmpPro.MaxHp; 
        saveModelData();
    }
    */
    //保存图签解锁
    private void saveModelData() {
        //string jsonStr = m_bossTag.DataToJson();
        string jsonStr = JsonUtility.ToJson(new Serialization<bossTag>(m_bossTag));
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

            //m_bossTag.readData(jsonStr);
            //m_bossTag = JsonUtility.FromJson<bossTag>(jsonStr);
             m_bossTag = JsonUtility.FromJson<Serialization<bossTag>>(jsonStr).target;
            
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
