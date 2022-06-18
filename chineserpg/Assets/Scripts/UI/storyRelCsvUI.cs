using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storyRelCsvUI : MonoBehaviour
{
    public InputField textID;
  //  public InputField inputNextID;
    public InputField textNextID;

    public Dropdown uiEffect;
    public Dropdown roleSay;
    public Dropdown BgChange;
    public Dropdown isAutoSave;
    public Dropdown isKeyOption;
   // public InputField inputCn;
    public InputField textCn;

  //  public InputField inputEn;
    public InputField textEn;

    public Text checkNextIDObj;

    private string repStr(string lVal) {
        string res = "";
        lVal = lVal.Trim();
        res = lVal.Replace(",", "，"); //半角换全角
        //res = res.Replace("\n","\\n");

        return res;
    }

    //UI调用
    public void UISelID()
    {
        int tempID = int.Parse(textID.text.Trim());
        CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(tempID);
        if (tmpRow != null)
        {
            textNextID.text = tmpRow.GetString(StoryRelationTab.csNextID);
            uiEffect.value = tmpRow.GetInt(StoryRelationTab.csUISort);
            roleSay.value = tmpRow.GetInt(StoryRelationTab.csIsRoleSay);
            BgChange.value = tmpRow.GetInt(StoryRelationTab.csNeedChangeBg);
            isAutoSave.value = tmpRow.GetInt(StoryRelationTab.csIsAutoSave);
            isKeyOption.value = tmpRow.GetInt(StoryRelationTab.csIsKeySave);
            textCn.text = tmpRow.GetString(StoryRelationTab.csContentCN);
            textEn.text = tmpRow.GetString(StoryRelationTab.csContentEn);
        }
    }
    public void UIAddID() {
        int tmpID = StoryRelationTab._instance().getNewID();
        textID.text = tmpID.ToString();
        textNextID.text = "0";
        uiEffect.value = 0;
        roleSay.value = 0;
        BgChange.value = 0;
        isAutoSave.value = 0;
        isKeyOption.value = 0;
        textCn.text = "Need input";
        textEn.text = "Need input";
    }

    public void UISaveRow() {

    }

    private const string csNextIDExist = "NextID存在";
    private const string csNextIDNotExist = "NextID不存在";
    public void UICheckNextID() {
        int tempNextID = int.Parse(textNextID.text.Trim());
        CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(tempNextID);
        if (tmpRow != null)
        {
            //nextID检测通过
            checkNextIDObj.text = csNextIDExist;
            checkNextIDObj.color = new Color(0, 0, 0);
        }
        else {
            checkNextIDObj.text = csNextIDNotExist;
            checkNextIDObj.color = new Color(255, 0, 0);
        }
        checkNextIDObj.gameObject.SetActive(true);
    }

    public void onUINextIDChange() {
        checkNextIDObj.gameObject.SetActive(false);
    }


    /// <summary>
    /// //////////
    /// </summary>
    /// <returns></returns>
    public string getUIID() {
        return repStr(textID.text);
    }

    public string getNextID()
    {
        return repStr(textID.text);
    }

    public string getContextCn() {
        return repStr(textCn.text);
    }

    public string getContextEn()
    {
        return repStr(textEn.text);
    }


    public int getUIEffect()
    {
        int res = getDropDown(uiEffect);
        return res;
    }

    public int getIsRoleSay() {
        int res = getDropDown(roleSay);
        return res;
    }
    public int getBgChange()
    {
        int res = getDropDown(BgChange);
        return res;
    }
    public int getIsAutoSave()
    {
        int res = getDropDown(isAutoSave);
        return res;
    }
    public int getIsKeyOption()
    {
        int res = getDropDown(isKeyOption);
        return res;
    }

    private int getDropDown(Dropdown tmpDd) {
        return tmpDd.value;
    }

}
