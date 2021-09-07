using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneAlphaControl : MonoBehaviour
{
    //上次碰撞到的物体
    private List<GameObject> lastColliderObject;

    //本次碰撞到的物体
    private List<GameObject> colliderObject;

    //本次碰撞并需要设置的物体
    private List<GameObject> needSetObject;

    // 人物主角（之后通过名字识别？还是tag？目前手动拖过来）  
    public GameObject _target;

    [SerializeField]
    private bool isCal = false;

    public Material wallNormalMater;
    public Material wallHalfAlphaMater;

    private int wallLayer;
    // 临时接收，用于存储  
    // private Renderer _tempRenderer;
    public void drawSceneAlpha() {
        isCal = true;
    }

    void Start()
    {
        lastColliderObject = new List<GameObject>();
        colliderObject = new List<GameObject>();
        needSetObject = new List<GameObject>();

        wallLayer = LayerMask.NameToLayer("wall");
      //  isCal = false;
    }

    public void clearData() {
        lastColliderObject.Clear();
        colliderObject.Clear();
        needSetObject.Clear();

        isCal = false;
    }

    //判断是否为新的碰撞体，并将旧的碰撞体列表中的相同项设为空
    private bool isNewColliderObject(GameObject pObj) {
        bool res = true;
        int delIndex = -1;
        for (int i = 0; i < lastColliderObject.Count; i++)
        {
            //if (lastColliderObject[i] != null)
            //{
                if (lastColliderObject[i] == pObj)
                {
                //lastColliderObject[i] = null;
                    delIndex = i;
                    Debug.Log("lastColliderObjet remove:"+ lastColliderObject[i].transform.parent.name);
                    res = false;
                    break;
                }
            //}
        }

        if (res == false) {
            lastColliderObject.RemoveAt(delIndex);
        }

        return res;
    }

    void Update()
    {
        if (isCal == false)
            return ;

        // 调试使用：红色射线，仅Scene场景可见     

        lastColliderObject.Clear();
        //将 colliderObject 中所有的值添加进 lastColliderObject
        for (int i = 0; i < colliderObject.Count; i++)
            lastColliderObject.Add(colliderObject[i]);

        colliderObject.Clear(); //初始化本次碰撞的列表
        needSetObject.Clear();


#if UNITY_EDITOR
    //    Debug.DrawLine(transform.position, _target.transform.position, Color.blue);
#endif
        /*射线可以从头部起始*/
        //这里是计算射线的方向，从主角发射方向是射线机方向
        /*
        Vector3 aim = _target.transform.position;
        //得到方向
        Vector3 ve = (_target.transform.position - transform.position).normalized;
        float an = transform.eulerAngles.y;
        aim -= an * ve;

        //在场景视图中可以看到这条射线
        //Debug.DrawLine(target.position, aim, Color.red);

        RaycastHit[] hit;
        hit = Physics.RaycastAll(_target.transform.position, aim, 100f);//起始位置、方向、距离
        */

        Vector3 aim = transform.position;
        //得到方向
        Vector3 ve = (transform.position - _target.transform.position).normalized;
        float an = transform.eulerAngles.y;
        aim -= an * ve;

        //在场景视图中可以看到这条射线
        //Debug.DrawLine(target.position, aim, Color.red);

        RaycastHit[] hit;
        hit = Physics.RaycastAll(transform.position, aim, 100f);//起始位置、方向、距离

        if (hit.Length > 0) //有发生碰撞
        {    
            for (int i = 0; i < hit.Length; i++)
            {
                GameObject pObj = hit[i].collider.gameObject;
                Renderer _tempRenderer = pObj.GetComponent<Renderer>();
                Transform parentTran = pObj.transform.parent;
                // GameObject parentObj = pObj<GameObject>();
                if ((pObj.layer == wallLayer) && (_tempRenderer != null) )
                {
                    colliderObject.Add(pObj);  //添加进本次碰撞的列表
                    Debug.Log("colliderObject Add :" + pObj.transform.parent.name);

                    if (isNewColliderObject(pObj)) {  //是新增的碰撞体
                        needSetObject.Add(pObj);
                        Debug.Log("needSetObj Add:"+pObj.transform.parent.name);
                    }
                }
            }
        }

        //还原lastColliderObject中的材质透明度
        //值不为null时则可恢复默认状态
        for (int i = 0; i < lastColliderObject.Count; i++)
        {
            //if (lastColliderObject[i] != null)
            resetMaterialsAlpha(lastColliderObject[i].gameObject);//恢复上次物体材质透明度
        }
        

        //设置本次新增碰撞的材质
        for (int i = 0; i < needSetObject.Count; i++) {
            SetMaterialsAlpha(needSetObject[i].gameObject);
        }

        isCal = false;
    }



    //恢复障碍物的透明度
    private void resetMaterialsAlpha(GameObject pObj) {
        Debug.Log("resetMaterialsAlpha :" + pObj.transform.parent.name);
        Renderer tmpRender = pObj.GetComponent<Renderer>();
        if (tmpRender != null)
        {
            tmpRender.material = wallNormalMater;
        }
    }

    // 修改障碍物的透明度  
    private void SetMaterialsAlpha(GameObject pObj)
    {
        Debug.Log("SetMaterialsAlpha :" + pObj.transform.parent.name);
        Renderer tmpRender = pObj.GetComponent<Renderer>();
        if (tmpRender != null) {
            tmpRender.material = wallHalfAlphaMater;
        }
        // 一个游戏物体的某个部分都可以有多个材质球  
       /*
        int materialsCount = _renderer.materials.Length;
        for (int i = 0; i < materialsCount; i++)
        {

            // 获取当前材质球颜色  
            Color color = _renderer.materials[i].color;

            // 设置透明度（0--1）  
            color.a = Transpa;

            // 设置当前材质球颜色（游戏物体上右键SelectShader可以看见属性名字为_Color）  
            _renderer.materials[i].SetColor("_Color", color);
        }
        */
    }
}
