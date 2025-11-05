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
    public class CancelAllSubs : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
                if (testData.Tables[0].Columns.Contains(TestDataConstants.Col_CustomTabName) && testData.Tables[0].Rows[0][TestDataConstants.Col_CustomTabName].ToString() != String.Empty)
                {
                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.Col_CustomTabName].ToString()))
                    {
                        OpenTab(testData.Tables[0].Rows[0][TestDataConstants.Col_CustomTabName].ToString());
                    }
                }
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CancelAllSubs");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private void InputEnter(DataRow dr)
        {
            try
            {
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                if (dr.Table.Columns.Contains(TestDataConstants.Col_CancelAllSubsOnHeader) && dr[TestDataConstants.Col_CancelAllSubsOnHeader].ToString().Equals("Yes"))
                {
                    CancelAllSubs1.Click(MouseButtons.Left);
                    if (NirvanaBlotter.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                }
                else
                {
                    int index = dtBlotter.Rows.IndexOf(dtRow);
                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                            throw new Exception("Trade not found during CancelAllSubs step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                        }
                    }
                    DgBlotter1.InvokeMethod("ScrollToRow", index);
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    bool isClicked = false;
                    try
                    {
                        isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Cancel_All_Subs);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    if (isClicked == false)
                    {
                        msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                        if (CancelAllSubs.IsVisible)
                        {
                            CancelAllSubs.Click(MouseButtons.Left);
                        }
                        else
                        {
                            Console.WriteLine("Menu Item {0} is not visible", CancelAllSubs.MsaaName);
                        }
                    }
                }               
                /*if (NirvanaBlotter.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);*/
                if (dr.Table.Columns.Contains(TestDataConstants.Col_CancelOrderPopup))
                {
                    if (NirvanaBlotter.IsEnabled)
                    {
                        if (dr[TestDataConstants.Col_CancelOrderPopup].ToString().ToUpper() == "YES")
                        {
                            ButtonYes.Click(MouseButtons.Left);
                        }
                        else if (dr[TestDataConstants.Col_CancelOrderPopup].ToString().ToUpper() == "NO")
                        {
                            ButtonNo.Click(MouseButtons.Left);
                        }
                        else
                            ButtonYes.Click(MouseButtons.Left);
                    }
                }
                else if (NirvanaBlotter.IsEnabled)
                    ButtonYes.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void OpenTab(string str)
        {
            try
            {
                UIAutomationElement accountComboItem = new UIAutomationElement();
                accountComboItem.AutomationName = str;
                accountComboItem.Comment = null;
                accountComboItem.ItemType = "";
                accountComboItem.MatchedIndex = 0;
                accountComboItem.Name = str;
                accountComboItem.Parent = this.BlotterTabControl1;
                accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                accountComboItem.UseCoordinatesOnClick = true;
                accountComboItem.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw new Exception("Custom tab with name " + str + " does not exist");
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
