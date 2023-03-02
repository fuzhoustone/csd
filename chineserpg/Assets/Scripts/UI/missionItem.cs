using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class missionItem : MonoBehaviour
{
    [SerializeField]
    private Text missionTxt;

    [SerializeField]
    private ContentSizeFitter sizeCont;

    [SerializeField]
    private VerticalLayoutGroup vertLayout;

    public void setText(string lTxt) {
        missionTxt.text = lTxt;
     //   sizeCont.SetLayoutVertical();
        vertLayout.SetLayoutVertical();
        sizeCont.SetLayoutVertical();
     //   vertLayout.SetLayoutVertical();
    }

    public float getTxtHeight() {
        float res = 0.0f;
        res = missionTxt.rectTransform.sizeDelta.y;
        //res = missionTxt.transform.re
        return res;
    }

}
