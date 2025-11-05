using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.DailyValuation
{
    public class FilterBlankMP : BlankMarkPriceUIMap, ITestStep
    {
         public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                GrdPivotDisplay.WaitForVisible();
                String columnName = string.Empty;
                String list = string.Empty;
                List<string> filterList = new List<string>();
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {

                    for (int i = 0; i < testData.Tables[sheetIndexToName[0]].Rows.Count; i++)
                    {
                        columnName = string.Empty;
                        list = string.Empty;
                        filterList.Clear();

                        columnName = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_NAME].ToString();
                        list = testData.Tables[sheetIndexToName[0]].Rows[i][TestDataConstants.COL_FILTERLIST].ToString();
                        if (list.ToUpper().Equals("ALL"))
                        {
                            list = "";
                        }
                     
                        foreach (string colName in list.Split(','))
                        {
                            filterList.Add(colName);
                        }
                        DataTable dtDailyVolatility = CSVHelper.CSVAsDataTable(GrdPivotDisplay.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                      
                        this.GrdPivotDisplay.InvokeMethod("AddFilter", columnName, filterList);
                       
                       
                       // Wait(5000);
                    }
                }
            }
             catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
           
            return _result;
        }


    }
}
