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
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class MergeOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                //OrdersTab.Click(MouseButtons.Left);
                MaximizeBlotter();
                bool Continued = true;
                Continued = ClickonButtonAndCheckEnabled(testData.Tables[0]);

                OrdersTab.Click();
                if (Continued == true)
                {

                    DataTable Newtb = new DataTable();
                    Newtb = testData.Tables[0].Copy();
                    Newtb.Columns.Remove("Allow Merge");
                    Newtb.Columns.Remove("ButtonClick");

                    if (Newtb != null)
                    {
                        InputEnter(Newtb);
                    }
                    /*    
                {
                      foreach (DataRow dr in Newtb.Rows)
                      {
                         
                      }
                  }*/
                    MergeOrders1.Click(MouseButtons.Left);

                    DataTable dt = testData.Tables[0];
                    HandlePopup(dt);
                    WaitAndCloseBlotter();
                }


            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "MergeOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private void WaitAndCloseBlotter()
        {
            try
            {

                // ExtentionMethods.WaitForVisible(ref BlotterMain_UltraFormManager_Dock_Area_Top, 10);
                Wait(5000);
                BlotterMain2.BringToFront();
                BlotterMain_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                OrdersTab.Click();
                // Keyboard.SendKeys("(%{F4})");
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }
        private void InputEnter(DataTable dt)
        {
            try
            {

                var msaaObj = DgBlotter1.MsaaObject;

                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                foreach (DataRow dr in dt.Rows)
                {


                    if (dr.Table.Columns.Contains(TestDataConstants.COL_CHECKBOX))
                    {
                        if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_CHECKBOX].ToString()))
                        {
                            dr[TestDataConstants.COL_CHECKBOX] = dr[TestDataConstants.COL_CHECKBOX].ToString().ToLower();
                        }

                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                        int index = dtBlotter.Rows.IndexOf(dtRow);
                        if (!DataUtilities.checkList)
                        {
                            if (index < 0)
                            {
                                List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                                throw new Exception("Trade not found during MergeOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                            }
                        }
                        DgBlotter1.InvokeMethod("ScrollToRow", index);
                        Wait(5000);
                        msaaObj.CachedChildren[0].CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);

                        dtBlotter.Rows[index][TestDataConstants.COL_CHECKBOX] = dr[TestDataConstants.COL_CHECKBOX].ToString().ToLower().Equals(TestDataConstants.COL_FALSEVALUE) ? TestDataConstants.COL_TRUEVALUE : TestDataConstants.COL_FALSEVALUE;
                        // dr[TestDataConstants.COL_CHECKBOX] = dr[TestDataConstants.COL_CHECKBOX].ToString().ToLower().Equals(TestDataConstants.COL_FALSEVALUE) ? TestDataConstants.COL_TRUEVALUE : TestDataConstants.COL_FALSEVALUE;

                    }
                    else
                    {
                        DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                        int index = dtBlotter.Rows.IndexOf(dtRow);
                        if (!DataUtilities.checkList)
                        {
                            if (index < 0)
                            {
                                throw new Exception("Trade not found during MergeOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "]");
                            }
                        }
                        DgBlotter1.InvokeMethod("ScrollToRow", index);
                        Wait(5000);
                        msaaObj.CachedChildren[0].CachedChildren[index + 1].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                    }

                }



            }
            catch (Exception)
            {
                throw;
            }
        }


        private bool ClickonButtonAndCheckEnabled(DataTable dt)
        {
            try
            {

                foreach (DataRow dr in dt.Rows)
                {
                    string button = dr[TestDataConstants.COL_BLOTTERBTTN].ToString();
                    if (dr[TestDataConstants.COL_SYMBOL].ToString() != String.Empty || dr[TestDataConstants.COL_SIDE].ToString() != String.Empty || dr[TestDataConstants.COL_STATUS].ToString() != String.Empty)
                    {
                        return true;
                    }
                    else
                    //means empty except button(needs to verify merge button only)
                    {
                        if (button == "Summary" || button == "Working Subs" | button == "Custom Working Subs")
                        {
                            //mappedbutton.Click();
                            if (button == "Summary")
                            {
                                Summary.Click();
                            }
                            if (button == "Working Subs")
                            {
                                WorkingSubsTab.Click();
                            }
                            if (button == "Custom Working Subs")
                            {
                                CustomWorkingSubs.Click();
                            }
                            //Wait(5000);
                            if (MergeOrders.IsEnabled == true)
                            {

                                throw new Exception("MergeORder Tab is enabled");// mergeorder is  enabled
                                // mergeorder is not enabled
                            }

                        }
                        if (button == "Orders" || button == "Custom Orders")
                        {
                            //mappedbutton.Click();
                            if (button == "Orders")
                            {
                                OrdersTab.Click();
                            }
                            if (button == "Custom Orders")
                            {
                                CustomOrders.Click();
                            }
                           // Wait(5000);
                            if (MergeOrders.IsEnabled == false)
                            {
                                throw new Exception("MergeORder Tab is disabled"); // mergeorder is not enabled
                            }


                        }
                    }
                }

                return false;
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
