using System.Collections;
using System.IO;
//using Assets.Script.Engine;
//using Assets.Script.Engine.Option;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    public TextAsset bossInfoTab;
    public TextAsset bossPro;

   private void Start()
   {
       initData();
    }

    public void initData()
    {
        using (var stream = new MemoryStream(bossInfoTab.bytes))
        {
            BossInfoTable.Load(stream);
        }

        using (var stream = new MemoryStream(bossPro.bytes))
        {
            BossProTable.Load(stream);
        }

    }
       
 
}
