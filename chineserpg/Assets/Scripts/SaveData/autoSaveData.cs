using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[Serializable]
public class keyData
{
    [SerializeField]
    public List<string> storyKeyLst;

    public keyData(List<string> lst)
    {
        this.storyKeyLst = lst;
    }
}

[Serializable]
public class processChart
{ //记录剧情走到哪了
    public int storyID;
}

public class autoSaveData //对指定档案的当前进度存档与读档
{
    private static autoSaveData data = null;
    public static autoSaveData instance() {
        if (data == null) {
            data = new autoSaveData();
        }

        return data;
    }



    private const string csSaveData = "/saveData.data";
    private const string csKeyData = "/keyData.data";

    private const int ciDefautStoryID = 1;
    private string dataPath;
    private string keyPath;
    private string rootPath;
    private processChart storyChart;
    private keyData storyKeyHistory;


    public void initParam(int lId) {
        rootPath = Application.persistentDataPath + "//" + lId.ToString();
        dataPath = rootPath + csSaveData;
        keyPath = rootPath + csKeyData;

        if (Directory.Exists(rootPath) == false)
        {
            Directory.CreateDirectory(rootPath);
        }

        loadData();
        loadKey();

    }

    public void saveData(int lstoryID) {
        storyChart.storyID = lstoryID;
        string str = JsonUtility.ToJson(storyChart);
        File.WriteAllText(dataPath, str);
    }

    //加入关键剧情线
    public void saveKeyData(int lstoryID) {
        storyKeyHistory.storyKeyLst.Add(lstoryID.ToString());

        //string str = JsonUtility.ToJson(new Serialization<string>(storyKeyHistory.storyKeyLst));
        string str = JsonUtility.ToJson(storyKeyHistory);

        File.WriteAllText(keyPath,str);

        //string[] storyHis = storyKeyHistory.storyKeyLst.ToArray();
        //File.WriteAllLines(keyPath, storyHis);
    }

    public int loadData() {
        int lStoryID = 0;

        if (File.Exists(dataPath))
        {
            string jsonStr = File.ReadAllText(dataPath);
            storyChart = JsonUtility.FromJson<processChart>(jsonStr);
        }
        else {
            storyChart = new processChart();
            saveData(ciDefautStoryID);
        }

        return lStoryID;
    }

    public void loadKey() {
        if (File.Exists(keyPath))
        {
            string storyHis = File.ReadAllText(keyPath);
            storyKeyHistory = JsonUtility.FromJson<keyData>(storyHis);

        }
        else
        {
            storyKeyHistory = new keyData(new List<string>());
            //storyKeyHistory.storyKeyLst = ;
            saveKeyData(ciDefautStoryID);
        }
    }
}
