using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public class Enemy
{
    [SerializeField]
    int id;
    [SerializeField]
    string name;

    [SerializeField]
    List<string> skills;

    public Enemy(int id,string name, List<string> skills)
    //public Enemy(int id, string name)
    {
        this.id = id;
        this.name = name;
        this.skills = skills;
    }

    
}

public class testEnemy {
    //private Dictionary<int, Enemy> enemiesDct;

    public void testData() {
        List<string> strLst = new List<string>();
        strLst.Add("攻击");

        List<string> strLst2 = new List<string>();
        strLst2.Add("攻击2");
        strLst2.Add("恢复2");


        List<Enemy> enemies = new List<Enemy>();
        enemies.Add(new Enemy(0,"怪物1", strLst));
        enemies.Add(new Enemy(1,"怪物2", strLst2));
        //enemies.Add(new Enemy(0, "怪物1"));
        //enemies.Add(new Enemy(1, "怪物2"));
        string str = JsonUtility.ToJson(new Serialization<Enemy>(enemies));
        Debug.Log("enemies:" + str);

        List<Enemy> enemiestmp = JsonUtility.FromJson<Serialization<Enemy>>(str).ToList();
        string str2 = JsonUtility.ToJson(new Serialization<Enemy>(enemiestmp));
        Debug.Log("enemiestmp:" + str2);
        //Debug.Log(JsonUtility.ToJson(enemies));
    }
    public void debugLog() {
        // List<T> -> Json ( 例 : List<Enemy> )
        /*
        // Dictionary<TKey,TValue> -> Json( 例 : Dictionary<int, Enemy> )
        string str2 = JsonUtility.ToJson(new Serialization<int, Enemy>(this.enemies));
        // 输出 : {"keys":[1000,2000],"values":[{"name":"怪物1","skills":["攻击"]},{"name":"怪物2","skills":["攻击","恢复"]}]}

        // Json -> Dictionary<TKey,TValue>
        Dictionary<int, Enemy> enemiesDct = JsonUtility.FromJson<Serialization<int, Enemy>>(str).ToDictionary();
        */
    }
}

[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> target;
    public List<T> ToList() { return target; }

    public Serialization(List<T> target)
    {
        this.target = target;
    }
}

// Dictionary<TKey, TValue>
[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}



