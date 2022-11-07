using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class talkOptionUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Text lab;

    private int pOptionID;
    public void setTxt(string lTxt,int lOptID) {
        lab.text = lTxt;
        pOptionID = lOptID;
    }
    public int getOptionID() {
        return pOptionID;
    }
}
