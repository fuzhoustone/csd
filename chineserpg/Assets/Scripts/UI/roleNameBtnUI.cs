using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleNameBtnUI : MonoBehaviour
{
    [SerializeField]
    private Text pText;

    [SerializeField]
    private Outline pOutline;

    [SerializeField]
    private Image pImage;

    [SerializeField]
    private Color pSelTextCol;
    [SerializeField]
    private Color pSelTextOutLineCol;
    //   private const string csColSelText = "DDB453";  //221 180  83

    [SerializeField]
    private Color pDisSelTextCol;
    [SerializeField]
    private Color pDisSelTextOutLineCol;
    //   private const string csColDisSelText = "9E9E9E"; //158 158 158
    [SerializeField]
    private Texture2D pSelBg;

    [SerializeField]
    private Texture2D pUnSelBg;

    public int pId;
    public string roleName;

    private Action<int> pEvent;

    public void initData(int lId, string lroleName)
    {
        pId = lId;
        roleName = lroleName;
        pText.text = roleName;
        setSelActive(false);
    }

    public void setSelActive(bool isActive)
    {
        if (isActive == true)
        {
            pImage.sprite = Sprite.Create(pSelBg, new Rect(0, 0, pSelBg.width, pSelBg.height), new Vector2(0.5f, 0.5f));
            pText.color = pSelTextCol;
            pOutline.effectColor = pSelTextOutLineCol;
        }
        else
        {
            pImage.sprite = Sprite.Create(pUnSelBg, new Rect(0, 0, pUnSelBg.width, pUnSelBg.height), new Vector2(0.5f, 0.5f));
            pText.color = pDisSelTextCol;
            pOutline.effectColor = pDisSelTextOutLineCol;
        }
    }

    public void onCallBackClick()
    {
        pEvent?.Invoke(pId);
    }
}
