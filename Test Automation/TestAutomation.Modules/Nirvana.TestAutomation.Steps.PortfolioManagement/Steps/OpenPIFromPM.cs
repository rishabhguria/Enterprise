using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public class OpenPIFromPM : PortfolioManagementUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
               // Wait(5000);
                Main.Click(MouseButtons.Left);
            
                DataTable dtPMGrid = CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                var gridMssaObject = Main.MsaaObject;
                foreach (DataRow row in testData.Tables[sheetIndexToName[0]].Rows)
                {
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtPMGrid), row);
                    int indexLong = dtPMGrid.Rows.IndexOf(dtRow);

                    if (indexLong >= 0)
                    {
                        Main.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, indexLong);
                        var Row = gridMssaObject.FindDescendantByName("ExposurePnlCacheItemList", 3000);
                        Row.CachedChildren[indexLong + 1].Click(MouseButtons.Left);
                        
                        Row.CachedChildren[indexLong + 1].Click(MouseButtons.Right);
                        if (!PopupMenuDropDown.IsVisible)
                        {
                            Row.CachedChildren[indexLong + 1].Click(MouseButtons.Left);
                            Main.Click(MouseButtons.Right);
                        }
                        PricingInputs.Click(MouseButtons.Left);
                        //Wait(5000);
                        break;
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
            finally
            {
            }
            return _result;
        }
    }
}
