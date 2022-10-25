using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleBodyUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image selImage;
   
    public int pID;
    private void Start()
    {
      //  lImage = this.gameObject.GetComponent<Image>();
    }

    public void initData(int lroleID) {
       
        pID = lroleID;
        setSelActive(false);
    }

    public void setSelActive(bool isSel) {
        
        if (selImage.enabled != isSel)
            selImage.enabled = isSel;
    }

   
}
