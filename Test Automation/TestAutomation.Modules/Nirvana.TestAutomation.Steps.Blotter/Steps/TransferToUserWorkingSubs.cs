using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class TransferToUserWorkingSubs : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                WorkingSubsTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr, testData.Tables[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "TransferToUserWorkingSubs");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }
        private TestResult InputEnter(DataRow dr, DataTable table)
        {
            TestResult _res = new TestResult();
            try
            {
                var msaaObj = DgBlotter.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                string userName = dr[TestDataConstants.COL_USERNAME].ToString();
                DataTable dTemp = new DataTable();
                dTemp = table.Clone();
                dTemp.ImportRow(dr);
                dTemp.Columns.Remove(TestDataConstants.COL_USERNAME);
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dTemp.Rows[0]);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during TransferToUserWorkingSubs step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index+ 1].Click(MouseButtons.Right);

                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Transfer_to_User);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (TransfertoUser.IsVisible)
                    {
                        TransfertoUser.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", TransfertoUser.MsaaName);
                    }
                }
                
                
                //TransfertoUser.Click(MouseButtons.Right);
                var msaa = PopupMenuTransfertoUser.MsaaObject;
                msaa.Click(userName);
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

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
