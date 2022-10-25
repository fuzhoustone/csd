using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class roleBodyUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Image selImage;
    [SerializeField]
    private Text roleNameTxt;

    public int pID;
    private void Start()
    {
      //  lImage = this.gameObject.GetComponent<Image>();
    }

    public void initData(int lroleID,string roleName) {
       
        pID = lroleID;
        roleNameTxt.text = roleName;
        setSelActive(false);
    }

    public void setSelActive(bool isSel) {
        
        if (selImage.enabled != isSel)
            selImage.enabled = isSel;
    }

   
}
