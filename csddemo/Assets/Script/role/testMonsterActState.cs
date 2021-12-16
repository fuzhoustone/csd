using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMonsterActState : MonoBehaviour
{
    [SerializeField]
    public Transform CamerTransform;

    [SerializeField]
    public Transform CanvasTransform;

    [SerializeField]
    public Transform roleTransform;

    [SerializeField]
    public Vector3 pPos;


    // Start is called before the first frame update
    void Start()
    {
        addRolePro(roleTransform.gameObject);

        roleProperty tmpPro = roleTransform.GetComponent<roleProperty>();

        tmpPro.InitData(CamerTransform, CanvasTransform);

        tmpPro.showUI();
    }
   
    //根据Hp3D模型的位置，调试使用Hp的UI坐标
    public void testHpUIShow() {
        roleProperty tmpPro = roleTransform.GetComponent<roleProperty>();

        tmpPro.testShowUI(pPos);
    }

    public void refreshTestUIShow()
    {
        roleProperty tmpPro = roleTransform.GetComponent<roleProperty>();

        tmpPro.testShowUI(pPos);
    }

    private void addRolePro(GameObject obj)
    {
        roleProperty pro = obj.AddComponent<roleProperty>();
        pro.roleSort = 0;
        pro.hpMax = 100;
        pro.hp = pro.hpMax;
        pro.attack = 1;
        pro.level = 1;
        pro.speed = 0.5f;
        pro.HpUIPoint = getHpPoint(obj.transform);
    }

    private GameObject getHpPoint(Transform parent)
    {
        GameObject res = parent.gameObject;
        int nCount = parent.childCount;
        for (int i = 0; i < nCount; i++)
        {
            Transform tmp = parent.GetChild(i);
            if (tmp.name == "HpPoint")
            {
                res = tmp.gameObject;
                break;
            }
        }

        return res;
    }

    

}
