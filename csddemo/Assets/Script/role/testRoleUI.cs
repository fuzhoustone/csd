using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRoleUI : MonoBehaviour
{
    [SerializeField]
    private Transform lCamer;

    [SerializeField]
    private Transform lCan;
    // Start is called before the first frame update
    void Start()
    {
        roleProperty tmpPro = this.GetComponent<roleProperty>();
        tmpPro.InitData(lCamer, lCan,1);
        tmpPro.testShowUI();
    }

  
   
}
