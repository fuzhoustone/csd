using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csdsleep : MonoBehaviour
{
    // Start is called before the first frame update
    private float sleepTime = 1.0f;
    public bool isCD;

    private bool isStop = false;
    Action callBack;
    Action<float> callByFrame; 

    public void sleepInit(float val, Action callEvent, Action<float> byFrameEvent) {
        sleepTime = val;
        callBack = callEvent;
        callByFrame = byFrameEvent;
    }

    public void startEvent() {
        StartCoroutine(doSleep());
    }

    public void stopSleep() {
        isStop = true;
       // StopAllCoroutines();
    }

    /*
    public bool isInCD() {
        return isCD;
    }
*/
   // const float fEscapeTime = 5.0f;
    IEnumerator doSleep()
    {
        float time = 0;
        isCD = true;
        //float fadeLength = 5.0f;
        while (time < sleepTime) // 还需另外设置跳出循环的条件
        {
            if (isStop)
            {
                yield return null;
            }

            time += Time.deltaTime;
            if (callByFrame != null) {
                callByFrame(time);
            }

            yield return null;
        }

        if (isStop == false)
        {
            isCD = false;
            if (callBack != null)
            {
                callBack();
            }
        }

    }
    // Update is called once per frame

}
