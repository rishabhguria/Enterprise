using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.Compliance
{
    class VerifyAlertHistory : ComplianceEngineUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                DataTable TempTestData = testData.Tables[0].Copy();
                OpenComplianceEngine();
                AlertHistory.Click(MouseButtons.Left);
                if (!UltraTabControl1.IsAttached)
                {
                    Wait(10000);
                }

                if (testData.Tables[0].Columns.Contains("VerifyExportState"))
                {
                    bool isVerificationNeeded = true;
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["VerifyExportState"].ToString()) && string.Equals(dr["VerifyExportState"].ToString(),"Export",StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = SqlUtilities.GetDataFromQuery("SELECT * FROM T_CompanyMarketDataProvider where CompanyId>0", "Client");
                            bool ExportState = true;
                            
                            
                            if (string.Equals(dt.Rows[0]["IsMarketDataBlocked"].ToString(), "1", StringComparison.OrdinalIgnoreCase) || 
                            string.Equals(dt.Rows[0]["IsMarketDataBlocked"].ToString(), "True", StringComparison.OrdinalIgnoreCase))
                            {
                                ExportState = false;
                            }

                            
                            if (UltraPanel1.IsVisible)
                            {
                                if (UltraBtnExport.IsVisible)
                                {
                                    if (UltraBtnExport.IsEnabled.Equals(ExportState))
                                    {
                                        Console.WriteLine("Export button state verified");
                                    }
                                    else
                                    {
                                        throw new Exception("Export button availablity is " + UltraBtnExport.IsEnabled.ToString() + " but state is " + ExportState);
                                    }
                                }
                               
                                Wait(3000);
                            }
                        }
                    }
                   // TempTestData = testData.Tables[0].Copy();
                    string RemoveColumns = ConfigurationManager.AppSettings["RemoveColumnsFromCompliancedt"];
                    TempTestData = DataUtilities.RemoveColumnsAndRows(RemoveColumns, TempTestData);
                    DataUtilities.DeleteRowsIfAllEmpty(TempTestData);

                    if (TempTestData.Rows.Count == 0)
                        isVerificationNeeded = false;
                    
                    if (!isVerificationNeeded)
                    {
                        return _res;
                    }
                }
                TempTestData = DataUtilities.RemoveCommas(TempTestData);
                List<String> errors = CheckAlertHistory(TempTestData);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyAlertHistory");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                //MinimizeComplianceEngine();
            }
            return _res;
        }

        private List<String> CheckAlertHistory(DataTable testData)
        {
            try
            {
                string StepName = "VerifyAlertHistory";
                int currentpagesize = 0;
                if (UltraNumericEditorPageSize.IsEnabled)
                {

                    {
                        currentpagesize = Convert.ToInt32(UltraNumericEditorPageSize.Text.Length);
                        while (currentpagesize > 0)
                        {
                            UltraNumericEditorPageSize.Click(MouseButtons.Left);
                            Keyboard.SendKeys(KeyboardConstants.SHIFTENDKEY);
                            Keyboard.SendKeys(KeyboardConstants.BACKSPACEKEY);
                            currentpagesize--;
                            Wait(2000);
                        }
                        UltraNumericEditorPageSize.Click(MouseButtons.Left);
                        // Keyboard.SendKeys(TestDataConstants.COL_PAGESIZE.ToString());
                        //string temp = ConfigurationManager.AppSettings["PageSize"];
                        Keyboard.SendKeys(ConfigurationManager.AppSettings["PageSize"]);
                    }
                }
                if (GetData.IsEnabled)
                {
                    GetData.Click(MouseButtons.Left);
                }
                GetAllColumnsOnGrid(testData);
                //DataTable dtComp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.UltraPnlGridMainClientArea.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                DataTable dtComp = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.UltraAlertGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtComp = DataUtilities.RemoveCommas(dtComp);
                List<String> columns = new List<string>();
                /*columns = (from DataColumn x in dTable.Columns
                select x.ColumnName).ToList();*/
                //columns.Add("User Name");
                columns.Add("Name");
                // columns.Add("Parameters");

                DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref testData);
                SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref  dtComp);

                List<String> errors = Recon.RunRecon(dtComp, testData, columns, 0.01);
                return errors;

            }

            catch (Exception)
            {
                throw;
            }
        }

        private void GetAllColumnsOnGrid(DataTable dTable)
        {
            try
            {
                List<string> columns = new List<string>();
                foreach (DataColumn item in dTable.Columns)
                {
                    columns.Add(item.ColumnName);
                }
                this.UltraAlertGrid.InvokeMethod("AddColumnsToGrid", columns);

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
            //base.Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
