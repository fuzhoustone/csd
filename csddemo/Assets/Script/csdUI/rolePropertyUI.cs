using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rolePropertyUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Text monText;
    public Text atkText;
    public Text defText;
    public Text hpText;
    //public Text lvText;
    public Text eleText;
    public Button btnBuy;

    private Action callEvent;

    private const string csElement0 = "无";
    private const string csElement1 = "火";  //red  
    private const string csElement2 = "风";  //green 
    private const string csElement3 = "水";  //blue
    private const string csElement4 = "土";  //albino
    private const string csElement5 = "圣";  //gold
    private const string csElement6 = "暗";  //purple，darkblue

    public void showData(int roleID, Action pEvent) {
        //roleID用于展现UI
        callEvent = pEvent;
        RoleProTable.rolePro tmpPro = RoleProTable.GetFromRoleID(roleID);
        refreshData(tmpPro);

        RoleInfoTable.roleElements tmpEle = RoleInfoTable.Get(roleID);
        monText.text = tmpEle.Name;
    }

    public void onBuyItem() {
        //如果够买才触发
    }

    public void refreshData(RoleProTable.rolePro pObj) {
        atkText.text = pObj.Atk.ToString();
        defText.text = pObj.Def.ToString();
        hpText.text =  pObj.MaxHp.ToString();
        //lvText.text = pObj.level.ToString();
        eleText.text = getElementName(pObj.Ele);
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
            case 5:
                res = csElement5;
                break;
            case 6:
                res = csElement6;
                break;

            default:
                res = csElement0;
                break;
        }
        return res;
    }

}
