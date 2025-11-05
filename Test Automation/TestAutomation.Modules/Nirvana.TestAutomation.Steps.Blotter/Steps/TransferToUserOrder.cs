using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class TransferToUserOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "TransferToUserOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private TestResult InputEnter(DataRow dr, DataTable table)
        {
            TestResult _res = new TestResult();
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                string userName = dr[TestDataConstants.COL_USERNAME].ToString();
                DataTable dTemp = new DataTable();
                dTemp = table.Clone();
                dTemp.ImportRow(dr);
                dTemp.Columns.Remove(TestDataConstants.COL_USERNAME);
                if( dTemp.Columns.Contains(TestDataConstants.COL_ALLOWTRANSFERTOUSER))
                dTemp.Columns.Remove(TestDataConstants.COL_ALLOWTRANSFERTOUSER);
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dTemp.Rows[0]);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during TransferToUserOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter1.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
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
                
                var msaa = PopupMenuTransfertoUser.MsaaObject;
                msaa.Click(userName);
               // Wait(2000);
                if (Warning1.IsVisible)
                {
                    if (table.Columns.Contains(TestDataConstants.COL_ALLOWTRANSFERTOUSER))
                    {
                        if (dr[TestDataConstants.COL_ALLOWTRANSFERTOUSER].ToString().ToUpper().Equals("YES"))
                        {
                            ButtonYes4.Click(MouseButtons.Left);
                        }
                        else if (dr[TestDataConstants.COL_ALLOWTRANSFERTOUSER].ToString().ToUpper().Equals("NO"))
                        {
                            ButtonNo2.Click(MouseButtons.Left);
                        }
                    }
                    else
                        ButtonYes4.Click(MouseButtons.Left);
                }
                if (Warning1.IsVisible)
                {
                    if (ButtonYes2.IsVisible)
                    {
                        ButtonYes2.Click(MouseButtons.Left);
                    }
                }
                if (Warning2.IsVisible)
                {
                   // Wait(3000);
                    ButtonOK2.Click(MouseButtons.Left);
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
