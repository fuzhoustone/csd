using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



[Serializable]
public class EnemyDemo
{
    [SerializeField]
    int id;
    [SerializeField]
    string name;

    [SerializeField]
    List<string> skills;

    public EnemyDemo(int id, string name, List<string> skills)
    //public Enemy(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.skills = skills;
    }


}

public class testEnemyDemo
{
    public void testData()
    {
        List<string> strLst = new List<string>();
        strLst.Add("攻击");

        List<string> strLst2 = new List<string>();
        strLst2.Add("攻击2");
        strLst2.Add("恢复2");


        List<EnemyDemo> enemies = new List<EnemyDemo>();
        enemies.Add(new EnemyDemo(0, "怪物1", strLst));
        enemies.Add(new EnemyDemo(1, "怪物2", strLst2));
        //enemies.Add(new Enemy(0, "怪物1"));
        //enemies.Add(new Enemy(1, "怪物2"));
        string str = JsonUtility.ToJson(new Serialization<EnemyDemo>(enemies));
        Debug.Log("enemies:" + str);

        List<EnemyDemo> enemiestmp = JsonUtility.FromJson<Serialization<EnemyDemo>>(str).ToList();
        string str2 = JsonUtility.ToJson(new Serialization<EnemyDemo>(enemiestmp));
        Debug.Log("enemiestmp:" + str2);
        //Debug.Log(JsonUtility.ToJson(enemies));
    }
}