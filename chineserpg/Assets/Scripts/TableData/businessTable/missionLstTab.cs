﻿
using System.Collections.Generic;

public class missionLstTab : CsdTTable
{
    private static missionLstTab instance = null;
    public static missionLstTab _instance()
    {
        if (instance == null)
        {
            instance = new missionLstTab();
            instance.initParam();
        }

        return instance;
    }

    public const string csRoleID = "roleID"; // { get {return "nextID";} }
    public const string csChartID = "chartID";
    public const string csContentCn = "contentCn";
    public const string csContentEn = "contentEn";
    public void initParam()
    {
        addKeyName(csID);
        addKeyName(csRoleID);
        addKeyName(csChartID);
        addKeyName(csContentCn);
        addKeyName(csContentEn);

    }

    public List<CSVRow> getMission(int lroleID, int lChartID) {
        List<CSVRow> res = new List<CSVRow>();
        CSVRow resRow = null;
        int nCount = this.GetTableLength();
        for (int i = 1; i <= nCount; i++) {
            CSVRow tmpRow = GetRowFromIndex(i-1);
            int tmpRoleID = tmpRow.GetInt(csRoleID);
            int tmpChartID = tmpRow.GetInt(csChartID);
            if ((tmpRoleID == lroleID) && (tmpChartID == lChartID)) {
                resRow = tmpRow;
                res.Add(resRow);
            }
        }
        return res;
    }

}
