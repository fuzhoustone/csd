using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkScene : MonoBehaviour
{
   // private const string csUnSelImage = "clueDis";  //线索未选中的图标
   // private const string csSelImage = "clue";   //线索选中的图标
   // private const string csPath = "UIPic/talkScene/";  //资源路径
    private const string csPreClueBtn = "Prefabs/UI/clueBtn";

   // private Texture2D pSelBg, pUnSelBg;
    private int oldClueIndex = 0;
    private List<clueBtnUI> clueLst;
    public Transform clueViewPostLst;
    //  private 

    // Start is called before the first frame update
    void Start()
    {
      //  pSelBg = (Texture2D)Resources.Load(csPath + csSelImage) as Texture2D;
      //  pUnSelBg = (Texture2D)Resources.Load(csPath + csUnSelImage) as Texture2D;
        initData();
    }

    public void UIClose() {
        clueLst.Clear();
    }

    public void showUI() { 
        
    }

    public void initData()
    {
        clueLst = new List<clueBtnUI>();

        int nCount = 5;
        for (int i = 1; i <= nCount; i++)
        {
            //int orderUIID = i - 1;
            //创建UI
                                                         
            UnityEngine.Object infoObj = Resources.Load(csPreClueBtn);
            GameObject tmpObj = GameObject.Instantiate(infoObj, clueViewPostLst) as GameObject;

            clueBtnUI tmpClue = tmpObj.GetComponent<clueBtnUI>();
            tmpClue.initData(i-1);
            clueLst.Add(tmpClue);

            Button tmpBtn = tmpObj.GetComponent<Button>();
            tmpBtn.onClick.AddListener(delegate ()
            {
                this.clueOnClick(tmpClue.pId);
            });
        }
        oldClueIndex = 0;
        setClueSel(oldClueIndex,true);
        
    }

    public void clueOnClick(int tmpID)
    {
        Debug.LogWarning("clueOnClick id:"+tmpID.ToString());
        if (tmpID != oldClueIndex) {
            setClueSel(oldClueIndex, false);
            setClueSel(tmpID, true);
            oldClueIndex = tmpID;
        }
    }

    private void setClueSel(int index, bool isSel)
    {
        clueBtnUI tmpBtnUI = clueLst[index];
        tmpBtnUI.setSelActive(isSel);

    }

    public void optionBtnChange(Toggle t) {
        if(t.isOn)
            Debug.Log("optionBtnChange true:"+t.name);

    }

}
