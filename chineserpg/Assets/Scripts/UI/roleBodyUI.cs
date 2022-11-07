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

    [SerializeField]
    private GameObject rootObj;
    

    public int pID;
    private Outline txtOutline;
    private void Start()
    {
      //  lImage = this.gameObject.GetComponent<Image>();
    }

    public void initData(int lroleID,string roleName) {
       
        pID = lroleID;
        roleNameTxt.text = roleName;
        txtOutline = roleNameTxt.gameObject.GetComponent<Outline>();
        setSelActive(false);
    }

    public void setSelActive(bool isSel) {
        
        bool isGrey = !isSel;
        UISetGrey.SetUIDrey(rootObj, isGrey);

        txtOutline.enabled = isSel;
//        if (btn.interactable != isSel)
//            btn.interactable = isSel;
        /*
        if (selImage.enabled != isSel)
            selImage.enabled = isSel;
        */
    }

   
}
