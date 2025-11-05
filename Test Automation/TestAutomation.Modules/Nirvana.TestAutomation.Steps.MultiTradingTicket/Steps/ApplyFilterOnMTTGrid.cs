using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class ApplyFilterOnMTTGrid : MultiTradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
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


                        foreach (string colName in list.Split(','))
                        {
                            filterList.Add(colName);
                        }
                        this.GrdTrades.InvokeMethod("AddFilter", columnName, filterList);
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
