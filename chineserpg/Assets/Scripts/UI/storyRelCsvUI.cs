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
   // public Dropdown roleNameLst;
    public InputField textCn;
    public InputField textEn;

    public Text checkNextIDObj;

    private const string csNextIDExist = "NextID存在";
    private const string csNextIDNotExist = "NextID不存在";

    //初始化
    private void Start()
    {
        roleLstInit();
        sceneLstInit();
    }

    private string repStr(string lVal) {
        string res = "";
        lVal = lVal.Trim();
        res = lVal.Replace(",", "，"); //半角换全角
        //res = res.Replace("\n","\\n");

        return res;
    }

    private void dropListInit(Dropdown lDropDown, CsdTTable tmpTab, string itemName) {
        string tmpRoleName = "";
        int nCount = tmpTab.GetTableLength();
        for (int i = 0; i < nCount; i++)
        {
            CSVRow tmpRow = tmpTab.GetRowFromIndex(i);
            tmpRoleName = tmpRow.GetString(itemName);
            lDropDown.options.Add(new Dropdown.OptionData(tmpRoleName));
        }
    }

    private void roleLstInit() {
        dropListInit(roleSay, roleNameTab._instance(), roleNameTab.csRoleName);
        /*
        string tmpRoleName = "";
        int nCount = roleInfoTab._instance().GetTableLength();
        for (int i = 0; i < nCount; i++)
        {
            CSVRow tmpRow = roleInfoTab._instance().GetRowFromIndex(i);
            tmpRoleName = tmpRow.GetString(roleInfoTab.csRoleName);
            roleSay.options.Add(new Dropdown.OptionData(tmpRoleName));
        }*/
    }

    private void sceneLstInit() {
        dropListInit(BgChange, bgScenePicTab._instance(), bgScenePicTab.csSceneName);
    }


    // <summary>
    // UI调用函数
    // </summary>
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
        int tempID = int.Parse(textID.text.Trim());
        CSVRow tmpRow = StoryRelationTab._instance().GetRowFromID(tempID);
        if (tmpRow != null) //保存
        {

        }
        else {  //新增

        }

        //提示保存成功
    }

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

    /*
    public void onUIRoleSayChange() {
        int tmp = roleSay.value;
        if (tmp == 0) //No
        {
         //   roleNameLst.transform.parent.gameObject.SetActive(false);
        }
        else { //Yes
         //   roleNameLst.transform.parent.gameObject.SetActive(true);
        }
    }
    */

    // <summary>
    // //////////外部调用
    // </summary>

    protected string getUIID() {
        return repStr(textID.text);
    }

    protected string getNextID()
    {
        return repStr(textID.text);
    }

    protected string getContextCn() {
        return repStr(textCn.text);
    }

    protected string getContextEn()
    {
        return repStr(textEn.text);
    }


    protected int getUIEffect()
    {
        int res = uiEffect.value;
        return res;
    }

    protected int getIsRoleSay() {
        int res = roleSay.value;
        if (res > 0)
            res = 1;
        return res;
    }

    private int getIDFromDropTab(Dropdown dropLst, CsdTTable tmpTab) {
        int tmpID = 0;
        int index = dropLst.value; //按表的顺序加载的
        if (index > 0)
        {
            CSVRow tmpRow = tmpTab.GetRowFromIndex(index - 1); //第一行为默认的“无”
            if (tmpRow != null)
                tmpID = tmpRow.GetInt(CsdTTable.csID);
        }
        return tmpID;
    }

    protected int getRoleID()
    {
        return getIDFromDropTab(roleSay, roleNameTab._instance());
    }

    protected int getSceneID() {
        return getIDFromDropTab(BgChange, bgScenePicTab._instance());
    }

    protected int getBgChange()
    {
        int res = BgChange.value;
        if (res > 0)
            res = 1;
        return res;
    }
    protected int getIsAutoSave()
    {
        int res = isAutoSave.value;
        return res;
    }
    protected int getIsKeyOption()
    {
        int res = isKeyOption.value;
        return res;
    }



}
