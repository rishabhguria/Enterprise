using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    class SelectTradeOnPM : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                SelectTrade(testData.Tables[0]);
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

        private void SelectTrade(DataTable dataTable)
        {
            dataTable = DataUtilities.RemovePercent(DataUtilities.RemoveCommas(dataTable));
            Main.WaitForRespondingOrExited();
            Main.Click(MouseButtons.Left);
            AutomationElementWrapper wrapper = new AutomationElementWrapper(Main.MsaaObject.Children[1].WindowHandle);
            WaitOnItems(wrapper);
            var gridMssaObject = Main.MsaaObject;
            Wait(6000);
            DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
            superset = DataUtilities.RemovePercent(DataUtilities.RemoveCommas(superset));
            DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(superset), dataTable.Rows[0]);
            int index = superset.Rows.IndexOf(dtRow);
            if (!DataUtilities.checkList)
            {
                if (index < 0)
                {
                    List<String> errors = Recon.RunRecon(superset, dataTable, new List<string>(), 0.01);
                    throw new Exception("Trade not found during SelectTradeOnPM step. [Symbol= " + dataTable.Rows[0]["Symbol"] + "], Position = [" + dataTable.Rows[0]["Position"] + "] Order Side = [" + dataTable.Rows[0]["Order Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                }
            }
            Main.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
            var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList", 5000);
            Row.CachedChildren[index + 1].Click(MouseButtons.Left);
        }

        private void WaitOnItems(AutomationElementWrapper wrapper)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();

                while (wrapper.Children.Count <= 1)
                {
                    if (tmr.ElapsedMilliseconds >= 30000)
                    {
                        break;
                    }
                }
                tmr.Stop();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
