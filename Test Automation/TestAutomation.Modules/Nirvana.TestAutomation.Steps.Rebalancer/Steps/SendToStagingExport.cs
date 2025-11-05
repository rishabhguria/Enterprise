using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.Interfaces.Enums;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    public class SendToStagingExport : RebalancerUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            DataRow dr = testData.Tables[0].Rows[0];
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
            try
            {
                OpenRebalancer();
               // Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
                ViewBuyDivideSellList.Click(MouseButtons.Left);
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }

                //Export Data To Excel


                DataTable dTable = ExportTradeListGrid();

                foreach (DataRow row in dTable.Rows)
                {
                    decimal originalValue = Convert.ToDecimal(row["Buy/Sell Value"]);
                    // Round the value and remove decimals by casting to int
                    row["Buy/Sell Value"] = (int)Math.Round(originalValue);
                }

                bool RequiresApprovalPopup = false;
                testData.Tables[0].Columns["Quantity"].ColumnName = "Buy/Sell Qty";
                testData.Tables[0].Columns["Buy/Sell Value (Base)"].ColumnName = "Buy/Sell Value";
                List<string> errors = VerifyGrid(dTable, testData.Tables[0]);
                if (errors.Count == 0)
                {
                    Save.Click(MouseButtons.Left);
                    Wait(2000);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    // Commented because of customized message box
                    //if (NirvanaAlert5.IsVisible)
                    //{
                    //    ButtonYes3.Click(MouseButtons.Left);
                    //}
                    SendtoStaging.Click(MouseButtons.Left);
                    /*Wait(5000);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Wait(3000);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Wait(3000);
                    Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                    Wait(3000);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);*/
                    Wait(2000);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(KeyboardConstants.TABKEY);
                    Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                    // Commented beacause of Custom Message Box
                    //if (NirvanaAlert5.IsVisible)
                    //{
                    //    ButtonYes3.Click(MouseButtons.Left);
                    //}


                    Wait(5000);

                    Capture(path + @"\ScreenShot.bmp");//Save Capture SS

                    ComplianceAlertPopUp.BringToFront();
                    if (ComplianceAlertPopUp.IsVisible || ComplianceAlertPopUp.IsEnabled)
                    {
                        string allowTrade = string.Empty;
                        if (!String.IsNullOrWhiteSpace(dr[TestDataConstants.COL_ALLOW_TRADE].ToString()))
                            allowTrade = dr[TestDataConstants.COL_ALLOW_TRADE].ToString();

                        ComplianceAlertPopUp.BringToFront();
                        AlertPopupGridCompliance.Click();
                        DataTable dtCompliancePopUp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.AlertPopupGridCompliance.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

                        RequiresApprovalPopup = CheckAlertTypeContainsRequiresApprovalAndHandleNotes(dtCompliancePopUp);

                        Wait(5000);
                        if (allowTrade.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ResponseButton.Click(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                        else if (allowTrade.Equals("No", StringComparison.CurrentCultureIgnoreCase) || allowTrade.Equals(String.Empty, StringComparison.CurrentCultureIgnoreCase))
                        {
                            CancelButton.Click(MouseButtons.Left);
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }


                        if (RequiresApprovalPopup.Equals(true) && dr[TestDataConstants.COL_ALLOW_TRADE].ToString().ToUpper().Equals("YES"))
                        {
                            Wait(9000);
                            Keyboard.SendKeys(KeyboardConstants.TABKEY);
                            Keyboard.SendKeys(KeyboardConstants.SPACE);
                        }

                    }
                }
            }
            /* if (GroupBox.IsVisible)
             {
                 SendtoStaging3.Click(MouseButtons.Left);
                 if (NirvanaAlert5.IsVisible)
                 {
                     ButtonNo3.Click(MouseButtons.Left);
                     if (ComplianceAlertPopUp.IsVisible)
                     {
                         string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                         if (!Directory.Exists(path))
                         {
                             Directory.CreateDirectory(path);
                         }
                         ExportButton.Click(MouseButtons.Left);
                         if (SaveAs2.IsVisible)
                         {
                             Clipboard.SetText(path + ExcelStructureConstants.RebalancerCompExportFile);
                             ButtonSave2.Click(MouseButtons.Left);
                             Keyboard.SendKeys("[CTRL+V]");
                             Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                             ButtonYes6.Click(MouseButtons.Left);
                             Wait(5000);
                         }
                         if (AlertsDataExport.IsVisible)
                         {
                             AlertsDataExport.BringToFront();
                             ButtonOK3.Click(MouseButtons.Left);
                         }
                         ResponseButton.Click(MouseButtons.Left);
                     }
                 }
             }
         }*/

            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SendToStagingExport");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                //close the Trade List UI
                CloseTradeList();
                Wait(4000);
                //Minimize Rebalancer
                
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
            }
            return _result;
        }
        /// <summary>
        /// Verify the Exported Grid Data with the Excel Sheet Data
        /// </summary>
        /// <param name="dt1">Storing the exported data</param>
        /// <param name="dt2">Storing the excel data</param>
        /// <returns></returns>
        private List<string> VerifyGrid(DataTable dt1, DataTable dt2)
        {
            List<string> errors = new List<string>();
            List<string> columns = new List<string>();
            DataTable Verifydata = new DataTable();
            Verifydata = dt2.Copy();
            Verifydata.Columns.Remove("AllowTrade");
            try
            {
                errors = Recon.RunRecon(dt1, Verifydata, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;

        }
        private bool CheckAlertTypeContainsRequiresApprovalAndHandleNotes(DataTable UiDataTable)
        {
            try
            {
                if (UiDataTable.Rows.Count > 0)
                {
                    int index = 0;
                    bool requiresapproval = false;
                    Dictionary<int, string> alerttypeindexer = new Dictionary<int, string>();

                    foreach (DataRow dr in UiDataTable.Rows)
                    {
                        if (dr[TestDataConstants.COL_ALERT_TYPE_V].ToString().Equals("Requires Approval"))
                        {
                            requiresapproval = true;
                            // alerttypeindexer.Add(index, dr[TestDataConstants.COL_ALERT_TYPE_V].ToString());
                        }

                        if (dr[TestDataConstants.COL_ALERT_TYPE_V].ToString().Equals("Soft with Notes"))
                        {
                            alerttypeindexer.Add(index, dr[TestDataConstants.COL_ALERT_TYPE_V].ToString());
                        }
                        index++;
                    }
                    if (alerttypeindexer.Count > 0)
                    {
                        foreach (var item in alerttypeindexer)
                        {
                            if (item.Value.Equals("Soft with Notes"))
                            {
                                if (item.Key == 0)
                                {
                                    UserNotes.Click(MouseButtons.Left);
                                }
                                else if (item.Key == 1)
                                {
                                    UserNotes1.Click(MouseButtons.Left);
                                }
                                else if (item.Key == 2)
                                {
                                    UserNotes2.Click(MouseButtons.Left);
                                }

                                Keyboard.SendKeys(TestDataConstants.COL_USER_NOTES);

                            }
                        }

                    }
                    if (requiresapproval.Equals(true))
                    {
                        return true;
                    }

                }

                return false;
            }
            catch (Exception) { throw; }
        }

        public void Capture(string CapturedFilePath)
        {
            try
            {
                Bitmap bitmap = new Bitmap
              (Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                Graphics graphics = Graphics.FromImage(bitmap as System.Drawing.Image);
                graphics.CopyFromScreen(25, 25, 25, 25, bitmap.Size);

                bool checkiffileexist = fileexist(CapturedFilePath);
                if (checkiffileexist.Equals(true))
                {
                    File.Delete(CapturedFilePath);
                }

                bitmap.Save(CapturedFilePath, ImageFormat.Bmp);
            }
            catch (Exception) { throw; }
        }

        public bool fileexist(string path)
        {
            if (File.Exists(path))
                return true;
            else
                return false;
        }
    }
}
