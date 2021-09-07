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
    public float xOffset;

    [SerializeField]
    public float yOffset;

    // Start is called before the first frame update
    void Start()
    {

    }
    /*
    //调试使用Hp的3D模型
    public void testHp3D() {
        roleProperty tmpPro = this.GetComponent<roleProperty>();
        //        tmpPro.creatPointFlag();
        tmpPro.setLineOffset(xOffset, yOffset);

        //根据其父节点的transform各属性构建坐标系
        Vector3 parPos = this.transform.position;
        Quaternion tmpRota = this.transform.rotation;
        Vector3 parScale = this.transform.localScale;

        Matrix4x4 tmpMat = Matrix4x4.TRS(parPos, tmpRota, parScale); //不能受角色朝向影响

        Vector3 tmpPos = new Vector3(0, 0, 0);

        Vector3 newMatPos = tmpMat.MultiplyPoint(tmpPos);

        Vector3 offsetV3 = new Vector3(xOffset * transform.localScale.x, yOffset * transform.localScale.y, 0.0f);
        tmpPro.creatHp3DFlag(offsetV3, "testLinePoint");
    }
    */
    //根据Hp3D模型的位置，调试使用Hp的UI坐标
    public void testHpUIShow() {
        roleProperty tmpPro = this.GetComponent<roleProperty>();

        tmpPro.InitData(CamerTransform, CanvasTransform);

        tmpPro.showUI();
    }

}
