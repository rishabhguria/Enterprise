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
    class OpenTaxlotDetails : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                OpenTaxLot(testData.Tables[0]);


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

        private void OpenTaxLot(DataTable dtable)
        {
            try
            {
                Main.WaitForRespondingOrExited();
                Main.Click(MouseButtons.Left);
                AutomationElementWrapper wrapper = new AutomationElementWrapper(Main.MsaaObject.Children[1].WindowHandle);
                WaitOnItems(wrapper);
                var gridMssaObject = Main.MsaaObject;
                Wait(6000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(superset), dtable.Rows[0]);
                int index = superset.Rows.IndexOf(dtRow);
                Main.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList", 5000);
                Row.CachedChildren[index + 1].DoubleClick(MouseButtons.Left);


            }
            catch (Exception)
            {
                throw;
            }


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
