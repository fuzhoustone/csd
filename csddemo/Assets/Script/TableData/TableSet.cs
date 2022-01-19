using System.Collections;
using System.IO;
//using Assets.Script.Engine;
//using Assets.Script.Engine.Option;
using UnityEngine;

public class TableSet : MonoBehaviour
{
    public TextAsset roleItem;
    public TextAsset roleProperty;
    public TextAsset shopItemTab;
    public TextAsset levMonsterTab;

   private void Start()
   {
       initData();
    }

    public void initData()
    {
        using (var stream = new MemoryStream(roleItem.bytes))
        {
            RoleInfoTable.Load(stream);
            //StoneTable.CsdTTable<string>.Load(stream);
        }

        using (var stream = new MemoryStream(roleProperty.bytes))
        {
            RoleProTable.Load(stream);
        }

        using (var stream = new MemoryStream(shopItemTab.bytes))
        {
            ShopItemTable.Load(stream);
        }

        using (var stream = new MemoryStream(levMonsterTab.bytes))
        {
            LevMonsterTab.Load(stream);
        }
    }
       
 
}
