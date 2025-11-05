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
    public class SelectTradeOnOrderGrid : BlotterUIMap, ITestStep
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
                    List<String> columns = new List<String>();
                    DataTable subset = testData.Tables[sheetIndexToName[0]];
                    try
                    {
                        string StepName = "SelectTradeOnOrderGrid";
                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                    }
                    catch (Exception)
                    { }
                    foreach (DataRow dr in subset.Rows)
                    {
                        InputEnter(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SelectTradeOnOrderGrid");
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
                bool flag = false;
                string toolTip = string.Empty;
                if (dr.Table.Columns.Contains(TestDataConstants.CheckBox) && dr[TestDataConstants.CheckBox].ToString().Equals("ToggleState_On"))
                {
                    flag = true;
                    dr[TestDataConstants.CheckBox] = string.Empty;
                }
                else if (dr.Table.Columns.Contains(TestDataConstants.CheckBox) && !string.IsNullOrEmpty(dr[TestDataConstants.CheckBox].ToString())) {
                    dr[TestDataConstants.CheckBox] = string.Empty;
                }
                if (dr.Table.Columns.Contains(TestDataConstants.Action_on_toolbar) && !string.IsNullOrEmpty(dr[TestDataConstants.Action_on_toolbar].ToString()))
                {
                    toolTip = dr[TestDataConstants.Action_on_toolbar].ToString();
                    dr[TestDataConstants.Action_on_toolbar] = string.Empty;
                }
                var msaaObj = DgBlotter1.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtBlotter), dr);
                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (!DataUtilities.checkList)
                {
                    if (index < 0)
                    {
                        List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                        throw new Exception("Trade not found during SelectTradeOnOrderGrid step. [Symbol= " + dr["Symbol"] + "], Quantity = [" + dr["Target Qty"] + "] Side = [" + dr["Side"] + "] \nRecon Error: " + String.Join("\n\r", errors));
                    }
                }
                DgBlotter1.InvokeMethod("ScrollToRow", index);
                if (flag && !msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[3].IsChecked)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].CachedChildren[3].Click(MouseButtons.Left);
                }
                else
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Left);
                }

                if (!string.IsNullOrEmpty(toolTip))
                {
                    UIAutomationElement accountComboItem = new UIAutomationElement();
                    accountComboItem.AutomationName = dr[TestDataConstants.Action_on_toolbar].ToString();
                    accountComboItem.Comment = null;
                    accountComboItem.ItemType = "";
                    accountComboItem.MatchedIndex = 0;
                    accountComboItem.Name = toolTip;
                    accountComboItem.Parent = this.ClientArea_Toolbars_Dock_Area_Top3;
                    accountComboItem.UIObjectType = TestAutomationFX.UI.UIObjectTypes.Unknown;
                    accountComboItem.UseCoordinatesOnClick = true;
                    accountComboItem.Click();

                    if (Warning2.IsVisible)
                        ButtonOK2.Click(MouseButtons.Left);
                    if (NirvanaBlotter.IsVisible)
                        ButtonYes.Click(MouseButtons.Left);
                    if (Warning.IsVisible)
                        ButtonOK.Click(MouseButtons.Left);
                }


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
