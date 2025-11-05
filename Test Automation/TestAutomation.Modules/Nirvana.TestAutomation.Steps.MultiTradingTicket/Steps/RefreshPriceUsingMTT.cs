using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    public class RefreshPriceUsingMTT : MultiTradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                MultiTradingTicket.BringToFront();
               // Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                //SelectAllTradeCheckBox.Click(MouseButtons.Left);
                MultiTradingTicket.InvokeMethod("UnselectAllOrders", null);
                Wait(1000);
                if (testData != null)
                {
                    InputEnter(testData.Tables[0]);
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

        private string InputEnter(DataTable dTable)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = dTable.Copy();

                string errorMessage = string.Empty;


                foreach (DataRow dr in dTable.Rows)
                {
                    var msaaObj = GrdTrades.MsaaObject;
                    DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.GrdTrades.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                    dTable.Columns.Remove(TestDataConstants.Col_Shares_Outstanding_PopUp);
                    if (dTable.Columns.Contains(TestDataConstants.COL_PopUpQuantity))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_PopUpQuantity);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_WARNING_RESPONSE))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_WARNING_RESPONSE);
                    }
                    if (dTable.Columns.Contains(TestDataConstants.COL_VERIFY_CLICKABILITY))
                    {
                        dTable.Columns.Remove(TestDataConstants.COL_VERIFY_CLICKABILITY);
                    }
                    DataRow[] dRows = DataUtilities.GetMatchingMultipleDataRows(DataUtilities.RemoveTrailingZeroes(dtBlotter), DataUtilities.RemoveTrailingZeroes(dTable), errorMessage);
                    // DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                    int index = 0;
                    foreach (var row in dRows)
                    {
                        index = dtBlotter.Rows.IndexOf(row);
                        GrdTrades.InvokeMethod("ScrollToRow", index);
                        Wait(1000);
                        msaaObj.FindDescendantByName("OrderBindingList", 5000).CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                        Wait(1000);
                    }
                    //SaveData();
                    BtnRefreshPrice.Click(MouseButtons.Left);
                    Wait(5000);
                    if (uiWindow1.IsVisible)
                    {
                        ButtonOK.Click(MouseButtons.Left);

                    }
                    Wait(1000);
                    break;
                }
                return errorMessage;
            }
            catch (Exception) { throw; }
        }
    }
}
