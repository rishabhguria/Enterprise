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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class SelectCheckBoxOnCustomTab : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                MaximizeBlotter();
                // Wait(1000);
                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString()))
                {
                    OpenTab(testData.Tables[0].Rows[0][TestDataConstants.TAB_NAME].ToString());
                }
                if (testData != null)
                {
                    InputEnter(testData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SelectCheckBoxOnCustomTab");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
        private void InputEnter(DataTable dt)
        {
            try
            {
                /* Removing columns before the main operation is made possible here through AppConfig
                 Modified by Yash Gupta
                 * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60623
                 */
                string columnsToRemove = ConfigurationManager.AppSettings["SelectCheckBoxOnCustomTab_ColumnsToRemove"];
                List<string> colsToRemove = new List<string>();
                if (!string.IsNullOrEmpty(columnsToRemove))
                {
                    colsToRemove = columnsToRemove
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(c => c.Trim())
                        .ToList();
                }
                // Update custom grid name as wrong grid name used
                var msaaObj = DgBlotter4.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(
                    this.DgBlotter4.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()
                );

                foreach (DataRow dr in dt.Rows)
                {
                    
                    foreach (string colName in colsToRemove)
                    {
                        if (dr.Table.Columns.Contains(colName))
                        {
                            dr[colName] = DBNull.Value;
                        }
                    }

                    DataRow dtRow = DataUtilities.GetMatchingDataRow(
                        DataUtilities.RemoveTrailingZeroes(dtBlotter), dr
                    );

                    int index = dtBlotter.Rows.IndexOf(dtRow);

                    if (!DataUtilities.checkList)
                    {
                        if (index < 0)
                        {
                            List<String> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                            throw new Exception(
                                "Trade not found during SelectCheckBoxOnCustomTab step. [Symbol= "
                                + dr["Symbol"] + "], Quantity = ["
                                + dr["Target Qty"] + "] Side = ["
                                + dr["Side"] + "]\nRecon Error: " + String.Join("\n\r", errors)
                            );
                        }
                    }

                    DgBlotter4.InvokeMethod("ScrollToRow", index);
                    Wait(5000);
                    try
                    {
                        var maingrid = msaaObj.CachedChildren[0].FindDescendantByName("OrderBindingList row " + index, 4000);
                        maingrid.FindDescendantByName("", 3000).Click(MouseButtons.Left);

                    }
                    catch
                    {
                        var maingrid = msaaObj.CachedChildren[0].FindDescendantByName("OrderBindingList row 1", 4000);
                        maingrid.FindDescendantByName("", 3000).Click(MouseButtons.Left);
                    }
                }
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
