using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//以参照物为原点的坐标系下的各种属性
public class PosRefer //: MonoBehaviour
{
    private const float csDefaultCamerPosX = 0.0f;
    private const float csDefaultCamerPosY = 1.0f;  //9.9
    private const float csDefaultCamerPosZ = -1.0f;  //-10

    private const float csDefaultCamerRotateX = 45.0f;
    private const float csDefaultCamerRotateY = 0.0f;  
    private const float csDefaultCamerRotateZ = 0.0f;

    //  private const float csDefaultCamerScaleX = 1.0f;
    //  private const float csDefaultCamerScaleY = 1.0f;  
    //  private const float csDefaultCamerScaleZ = 1.0f;  
    private const float csDefaultCamerScale = 1.0f;

    public Vector3 referDefPosition   //参照物坐标系下的坐标
          { set;
            get;
          }

    public Vector3 referRotate {   //参照物坐标系下的旋转角度
            set;
            get;
         }

    
    public Vector3 referScale()
    {  //参照物坐标系下的缩放， 默认都是1
        Vector3 tmpScale = new Vector3(scaleOldCamerParam, scaleOldCamerParam, scaleOldCamerParam);
        return tmpScale;
    }


    
    //当前参照物坐标系下的缩放， 初始值是1
    public float scaleOldCamerParam {
        set;
        get;
    }

    //private float scaleNowCamerParam;
    

    public void initRefer() {
        referDefPosition = new Vector3(csDefaultCamerPosX, csDefaultCamerPosY, csDefaultCamerPosZ);

        referRotate = new Vector3(csDefaultCamerRotateX, csDefaultCamerRotateY, csDefaultCamerRotateZ);

        scaleOldCamerParam = csDefaultCamerScale;
       // referScale = new Vector3(csDefaultCamerScaleX, csDefaultCamerScaleY, csDefaultCamerScaleZ);
    }

    private const float csScaleParam = 0.005f;

    private const float csMaxScale = 5.0f;
    private const float csMinScale = 0.4f;

    public float updateScale(float scale, float pDeltaTime) {
        float newScale = scaleOldCamerParam * ( 1 + scale * csScaleParam);

        if (newScale < csMinScale)
        {
            // tmpScale = csMinScale;
            // scaleParam = csMinScale;
            newScale = csMinScale;
            Debug.LogWarning("scaleCamer min value:" + scaleOldCamerParam.ToString());
        }
        else if (newScale > csMaxScale)
        {
            //tmpScale = 5.0f;
            //scaleParam = csMaxScale;
            newScale = csMaxScale;
            Debug.LogWarning("scaleCamer max value" + scaleOldCamerParam.ToString());
        }

        scaleOldCamerParam = newScale;

        Debug.Log("scaleNowCamerParam: " + scaleOldCamerParam.ToString());
        return scaleOldCamerParam;
    }

}
