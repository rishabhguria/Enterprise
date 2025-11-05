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
    public class CancelSubOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                PranaApplication.BringToFrontOnAttach = false;
                //OpenBlotter();
                //OrdersTab.Click(MouseButtons.Left);
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CancelSubOrder");
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

        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter2.MsaaObject;
                var maingrid = msaaObj;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter2.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during CancelSubOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter2.InvokeMethod("ScrollToRow", index);
                var rowIndex = index + 1;
                try
                {
                    maingrid = msaaObj.FindDescendantByName("OrderBindingList row " + rowIndex, 4000);

                }
                catch
                {
                    maingrid = msaaObj.FindDescendantByName("OrderBindingList row 1", 4000);
                }
                maingrid.Click(MouseButtons.Right);
                //msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);

                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (isClicked == false)
                {
                    DgBlotter2.MsaaObject.FindDescendantByName("OrderBindingList row 1", 4000).Click(MouseButtons.Right);
                    //msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (Cancel.IsVisible)
                    {
                        Cancel.Click(MouseButtons.Left);
                    }
                    else
                    {
                        Console.WriteLine("Menu Item {0} is not visible", Cancel.MsaaName);
                    }
                }

                //Cancel.Click(MouseButtons.Left);
                if (dr.Table.Columns.Contains(TestDataConstants.Col_CancelOrderPopup))
                {
                    if (Confirmation.IsEnabled)
                    {
                        if (dr[TestDataConstants.Col_CancelOrderPopup].ToString().ToUpper() == "YES")
                        {
                            ButtonYes1.Click(MouseButtons.Left);
                        }
                        else if (dr[TestDataConstants.Col_CancelOrderPopup].ToString().ToUpper() == "NO")
                        {
                            ButtonNo1.Click(MouseButtons.Left);
                        }
                        else
                            ButtonYes1.Click(MouseButtons.Left);
                    }                   
                }
                else if (Confirmation.IsVisible)
                {
                    // Wait(3000);
                    ButtonYes1.Click(MouseButtons.Left);
                }
                if (Warning1.IsVisible)
                {
                    ButtonOK1.Click();
                }
                /*if (Warning3.IsVisible)
                {
                    ButtonOK3.Click();
                }*/
            }
            catch (Exception)
            {
                throw;
            }
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
