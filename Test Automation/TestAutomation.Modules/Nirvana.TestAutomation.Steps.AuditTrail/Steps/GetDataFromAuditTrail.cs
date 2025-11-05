using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.AuditTrail
{
    public class GetDataFromAuditTrail : AuditTrailUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            if (testData.Tables[0].Columns.Contains("GetData") &&  !string.IsNullOrWhiteSpace(testData.Tables[0].Rows[0]["GetData"].ToString())) {
                testData = DataUtilities.SetDate(testData, "FromDate", "ToDate");
            }
            OpenAuditTrail();
            try
            {
                String fromdate = string.Empty;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROMDATE].ToString()))
                        fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROMDATE].ToString()));
                    Textarea.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                    Textarea1.Click(MouseButtons.Left);
                    String todate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TODATE].ToString()))
                        todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TODATE].ToString()));
                    ExtentionMethods.CheckCellValueConditions(todate, string.Empty, true);
                    BtGetData.Click(MouseButtons.Left);
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

    }
}