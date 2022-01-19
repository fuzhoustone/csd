using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjCommon
{
    public static GameObject getObjNode(Transform parent, string nodeStr)
    {
        GameObject res = parent.gameObject;
        int nCount = parent.childCount;
        for (int i = 0; i < nCount; i++)
        {
            Transform tmp = parent.GetChild(i);
            if (tmp.name == nodeStr)
            {
                res = tmp.gameObject;
                break;
            }
        }

        return res;
    }

    public static void skinUpdate(int pID,Transform pParent) {
        CSVRow lRow = RoleInfoTable.isUseSkin(pID);
        if (lRow != null)
        { //需要换肤
            string skinName = RoleInfoTable.GetSkin(lRow);
            string skinNode = lRow.GetString("skinNode");
            changeSkin(pParent.transform, skinNode, skinName);
        }
    }

    //修改皮肤,需要修改的才调用, 若skinPathName为""则不修改
    private static bool changeSkin(Transform parent, string nodeStr, string skinPathName) {
        bool isSuccess = false;
        if (skinPathName == "")
            return isSuccess;

        GameObject tmpObj = getObjNode(parent, nodeStr);
        if (tmpObj != null) {
            Material skinMat = Resources.Load<Material>("Prefab/Model/"+skinPathName);
            tmpObj.GetComponent<SkinnedMeshRenderer>().material = skinMat;
        }

        return isSuccess;
    }

}
