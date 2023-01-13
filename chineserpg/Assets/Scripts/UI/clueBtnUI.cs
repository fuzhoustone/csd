using System;
using UnityEngine;
using UnityEngine.UI;

public class clueBtnUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text pText;

    [SerializeField]
    private Image pImage;

    [SerializeField]
    private Color pSelTextCol;
    //   private const string csColSelText = "DDB453";  //221 180  83
    
    [SerializeField]
    private Color pDisSelTextCol;
    //   private const string csColDisSelText = "9E9E9E"; //158 158 158
    [SerializeField]
    private Texture2D pSelBg;

    [SerializeField]
    private Texture2D pUnSelBg;

    private const float cfImageWidth = 187.0f;
    private const float cfImageHeight = 61.0f;
 
    private Action<int> pEvent;
    public int pTalkInfoLstId;

    public void initData(int lId,string lKeyName) {
        pTalkInfoLstId = lId;
        pText.text = lKeyName;
        setSelActive(false);
    }

    public void setSelActive(bool isActive) {
        if (isActive == true)
        {
            pImage.sprite = Sprite.Create(pSelBg, new Rect(0, 0, pSelBg.width, pSelBg.height), new Vector2(0.5f, 0.5f));
            pText.color = pSelTextCol;
        }
        else {
            pImage.sprite = Sprite.Create(pUnSelBg, new Rect(0, 0, pUnSelBg.width, pUnSelBg.height), new Vector2(0.5f, 0.5f));
            pText.color = pDisSelTextCol;
        }

    }
    /*
    public void onCallBackClick() {
        pEvent?.Invoke(pId);
    }
    */
    

}
