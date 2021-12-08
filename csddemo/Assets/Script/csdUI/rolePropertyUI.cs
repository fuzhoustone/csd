using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rolePropertyUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text atkText;
    public Text defText;
    public Text hpText;
    //public Text lvText;
    public Text eleText;

    public const string csElement0 = "无";
    public const string csElement1 = "风";
    public const string csElement2 = "土";
    public const string csElement3 = "水";
    public const string csElement4 = "火";

    public void refreshData(roleProperty pObj) {
        atkText.text = pObj.attack.ToString();
        defText.text = pObj.def.ToString();
        hpText.text = pObj.hp.ToString() + "/" + pObj.hpMax.ToString();
        //lvText.text = pObj.level.ToString();
        eleText.text = getElementName(pObj.element);
    }

    public void uiClose() {
        gameObject.SetActive(false);
    }

    private string getElementName(int element) {
        string res = "";
        switch (element) {
            case 0:
                res = csElement0;
                break;
            case 1:
                res = csElement1;
                break;
            case 2:
                res = csElement2;
                break;
            case 3:
                res = csElement3;
                break;
            case 4:
                res = csElement4;
                break;
            
        }
        return res;
    }

}
