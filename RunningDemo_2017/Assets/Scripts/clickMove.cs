using UnityEngine;
using System.Collections;

public class clickMove : MonoBehaviour {
    public GameObject cube  = null;
    public Camera Camera = null;

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {  //创建一个射线，该射线从主摄像机中发出，而发出点是鼠标点击的位置 

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null)
                {
                    print("hit:" + hit.collider.name);
                    GameObject m_currSelectObj = hit.collider.gameObject;

                    Vector3 newPos = new Vector3(hit.point.x, hit.point.y, hit.point.z);

                    Instantiate(cube.transform, newPos, cube.transform.rotation);
                }
                else
                {

                }
            }
        }
    }
}
