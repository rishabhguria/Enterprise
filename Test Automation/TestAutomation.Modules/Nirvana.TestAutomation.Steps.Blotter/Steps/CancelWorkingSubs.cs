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
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class CancelWorkingSubs : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                WorkingSubsTab.Click(MouseButtons.Left);
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
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CancelWorkingSubs");
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
                string popupval = string.Empty;
                if (dr.Table.Columns.Contains("CancelOrderPopup(Yes/No)"))
                {
                    popupval = dr["CancelOrderPopup(Yes/No)"].ToString();
                    dr["CancelOrderPopup(Yes/No)"] = string.Empty;
                }

                var msaaObj = DgBlotter.MsaaObject;
                DataTable dtBlotter = CSVHelper.CSVAsDataTable(this.DgBlotter.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());

                /* Removing columns before the main operation is made possible here through AppConfig
                 Modified by Yash Gupta
                 * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60623
                 */
                string stepKey = this.GetType().Name + "_ColumnsToRemove";
                string columnsConfig = ConfigurationManager.AppSettings[stepKey];

                DataTable tempTable = dr.Table.Copy();
                if (!string.IsNullOrEmpty(columnsConfig))
                {
                    var columnsToRemove = columnsConfig.Split(',')
                                                       .Select(c => c.Trim())
                                                       .Where(c => !string.IsNullOrEmpty(c))
                                                       .ToList();

                    foreach (var colName in columnsToRemove)
                    {
                        if (tempTable.Columns.Contains(colName))
                            tempTable.Columns.Remove(colName);
                    }
                }

                DataRow tempRow = tempTable.NewRow();
                foreach (DataColumn col in tempTable.Columns)
                {
                    tempRow[col.ColumnName] = dr[col.ColumnName];
                }

                DataRow dtRow = DataUtilities.GetMatchingDataRow(
                    DataUtilities.RemoveTrailingZeroes(dtBlotter),
                    tempRow
                );

                int index = dtBlotter.Rows.IndexOf(dtRow);
                if (index < 0)
                {
                    List<string> errors = Recon.RunRecon(dtBlotter, dr.Table, new List<string>(), 0.01);
                    throw new Exception(
                        "Trade not found during CancelWorkingSubs step. [Symbol= " + dr["Symbol"] +
                        "], Quantity = [" + dr["Quantity"] + "] Side = [" + dr["Side"] +
                        "]\nRecon Error: " + string.Join("\n\r", errors)
                    );
                }

                DgBlotter.InvokeMethod("ScrollToRow", index);
                msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);

                bool isClicked = false;
                try
                {
                    isClicked = pickFromMenuItem(PopupMenuContext, TestDataConstants.Cancel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                if (!isClicked)
                {
                    msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
                    if (Cancel.IsVisible)
                        Cancel.Click(MouseButtons.Left);
                    else
                        Console.WriteLine("Menu Item {0} is not visible", Cancel.MsaaName);
                }

                if (!dr.Table.Columns.Contains("IgnorePopup"))
                {
                    if (dr.Table.Columns.Contains(TestDataConstants.Col_CancelOrderPopup))
                    {
                        if (ButtonYes1.IsEnabled)
                        {
                            string popupChoice = dr[TestDataConstants.Col_CancelOrderPopup].ToString().ToUpper();
                            if (popupChoice == "YES")
                                ButtonYes1.Click(MouseButtons.Left);
                            else if (popupChoice == "NO")
                                ButtonNo1.Click(MouseButtons.Left);
                            else
                                ButtonYes1.Click(MouseButtons.Left);
                        }
                    }
                    else if (ButtonYes1.IsVisible)
                        ButtonYes1.Click(MouseButtons.Left);
                    else if (ButtonOK1.IsVisible)
                        ButtonOK1.Click(MouseButtons.Left);
                }
            }
            catch
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
