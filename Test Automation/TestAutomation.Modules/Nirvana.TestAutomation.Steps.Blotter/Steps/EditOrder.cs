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
    public class EditOrder : BlotterUIMap, ITestStep
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
                    
                    InputEnter(testData.Tables[0]);
                    
                   // Wait(3000);
                    
                }
                
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                if (testData.Tables[0].Rows.Count > 1)
                {
                    Keyboard.SendKeys(KeyboardConstants.SPACE);
                }
                if (testData.Tables[0].Rows.Count == 1)
                {
                    CloseBlotter();
                }
                
            }
            return _res;
        }
        private void InputEnter(DataTable dTable)
        {
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                int lastindex = dTable.Rows.Count;
                foreach (DataRow dr in dTable.Rows)
                {
                    --lastindex;
                    DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during EditOrder step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
                    Wait(5000);
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[3].Click(MouseButtons.Left);
                    if (lastindex == 0)
                    {
                        msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[5].Click(MouseButtons.Right);
                        bool isClicked = false;
                        Wait(3000);

                        try
                        {
                            isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Edit_Order_s);
                    }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                }
                        if (isClicked == false)
                        {
                            Console.WriteLine("New code Testing");
                            msaaObj.FindDescendantByName("OrderBindingList", 3000).CachedChildren[index + 1].Click(MouseButtons.Right);
                EditOrders.Click(MouseButtons.Left);
            }
                    }
                }
               // msaaObj.CachedChildren[0].CachedChildren[lastindex].CachedChildren[1].CachedChildren[5].Click(MouseButtons.Right);
                //EditOrders.Click(MouseButtons.Left);
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

