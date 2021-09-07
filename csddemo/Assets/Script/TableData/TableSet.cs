using System.Collections;
using System.IO;
//using Assets.Script.Engine;
//using Assets.Script.Engine.Option;
using UnityEngine;

public class TableSet : MonoBehaviour
{

    public TextAsset m_PopFirstChargeTable;
    private int m_loadedCount = 0;

    public int LoadedCount
    {
        get { return m_loadedCount; }
    }


    public const int TotalCount = 1; //2017.08.17


    public void Load()
    {
        m_loadedCount = 0;

        LoadmPopChargeTable();

    }

    private void LoadmPopChargeTable()
    {

        using (var stream = new MemoryStream(m_PopFirstChargeTable.bytes))
        {
            PopFirstChargeTable.Load(stream);
        }
        ++m_loadedCount;
    }

   
    public IEnumerator LoadCoroutine()
    {
        m_loadedCount = 0;
        //
        LoadmPopChargeTable();
        yield return null;

    }
   
    public static void Clear()
    {
        PopFirstChargeTable.Clear();
    }
}
