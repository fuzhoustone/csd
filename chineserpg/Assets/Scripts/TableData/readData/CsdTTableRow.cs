using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsdTTableRow
{
    public List<CsvKeyName> mKeyNameLst;

    public void keyInit(CSVRow pCsvRow) {
        CSVColumnNameIndexer tmpIndex = pCsvRow.GetCSVColumnNameIndexer();
        List<string> nameLst = tmpIndex.getColDicLst();

        mKeyNameLst = new List<CsvKeyName>();
        int nCount = nameLst.Count;
        for (int i = 0; i < nCount; i++)
        {
            string nameVal = nameLst[i];
            CsvKeyName tmpKeyName = new CsvKeyName();
            tmpKeyName.keyName = nameVal;
           // tmpKeyName.keyVal = pCsvRow.GetString(nameVal);
        }
    }

    public string getKeyVal(string pKey) {
        string res = "";

        return res;
    }

}
