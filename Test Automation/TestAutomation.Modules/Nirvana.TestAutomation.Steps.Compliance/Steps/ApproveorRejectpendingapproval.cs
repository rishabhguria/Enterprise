using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;
using Nirvana.TestAutomation.UIAutomation;
using System.Configuration;


namespace Nirvana.TestAutomation.Steps.Compliance
{
    class ApproveorRejectpendingapproval : ComplianceEngineUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenComplianceEngine();
                Wait(8000);
                // Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                PendingApproval.Click(MouseButtons.Left);
                Wait(2000);
                DataTable  TempTestData = testData.Tables[0].Copy();
                if (testData.Tables[0].Columns.Contains("VerifyExportState"))
                {
                    UIAutomationHelper uihelper = new UIAutomationHelper();
                    DataUtilities.IUIAutomationFileLoader();
                    bool isVerificationNeeded = true;
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["VerifyExportState"].ToString()) && string.Equals(dr["VerifyExportState"].ToString(), "Export", StringComparison.OrdinalIgnoreCase))
                        {
                            DataTable dt = SqlUtilities.GetDataFromQuery("SELECT * FROM T_CompanyMarketDataProvider where CompanyId>0", "Client");
                            bool ExportState = true;
                             
                            
                            if (string.Equals(dt.Rows[0]["IsMarketDataBlocked"].ToString(), "1", StringComparison.OrdinalIgnoreCase) || 
                            string.Equals(dt.Rows[0]["IsMarketDataBlocked"].ToString(), "True", StringComparison.OrdinalIgnoreCase))
                            {
                                ExportState = false;
                            }
                             

                            try
                            {
                                uihelper.VerifyComplianceExportVisibility(dr["VerifyExportState"].ToString(), !ExportState);
                            }
                            catch (Exception ex)
                            {
                                SamsaraHelperClass.CaptureMyScreen("VerifyExportState", ApplicationArguments.TestCaseToBeRun, "ApproveorRejectpendingapproval");
                                _res.IsPassed = false;
                                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                                if (rethrow)
                                    throw;

                            }

                           
                        }
                                      
                    }

                    
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



                string StepName = "ApproveorRejectpendingapproval";
                DataTable SelectData = new DataTable();
                SelectData = TempTestData.Copy();
                if (SelectData.Columns.Contains("ApproveorRejectAll"))
                {
                    SelectData.Columns.Remove("ApproveorRejectAll");
                }
                List<String> columns = new List<string>();
                DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref SelectData);
                SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref  SelectData);
               
                if (SelectData != null)
                {
                    foreach (DataRow dr in SelectData.Rows)
                    {
                        SelectPendingApproval(dr);
                    }
                }

                if (UltraPanel12.IsEnabled)
                {
                    DataRow dt = testData.Tables[0].Rows[0];
                    if (dt[TestDataConstants.COL_APPROVEORREJECTALL].ToString() == "Approve")
                    { Approve.Click(MouseButtons.Left); }
                    else if (dt[TestDataConstants.COL_APPROVEORREJECTALL].ToString() == "Reject")
                    { Block.Click(MouseButtons.Left); }
                    else if (String.IsNullOrWhiteSpace(dt[TestDataConstants.COL_APPROVEORREJECTALL].ToString()))
                    {
                        throw new Exception("COL_APPROVEORREJECTALL is empty");

                    }


                }



            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ApproveorRejectpendingapproval");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeComplianceEngine();
            }
            return _res;
        }
        private void SelectPendingApproval(DataRow dr)
        {
            try
            {
                var msaaObj = UltraPendingApprovalGrid.MsaaObject;
                DataTable dtComp = CSVHelper.CSVAsDataTable(this.UltraPendingApprovalGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtComp), dr);
                int index = dtComp.Rows.IndexOf(dtRow);
                UltraPendingApprovalGrid.InvokeMethod("ScrollToRow", index);
                index = index + 1;
                // msaaObj.CachedChildren[0].CachedChildren[index].Click(MouseButtons.Left);
                ExtentionMethods.WaitForVisible(ref ComplianceEngine_UltraFormManager_Dock_Area_Top, 40);
                // Wait(5000);
                //msaaObj.CachedChildren[1].CachedChildren[1].CachedChildren[index].Click(MouseButtons.Left);
                var row = msaaObj.FindDescendantByName("BindingList`1 row " + index, 3000);
                row.FindDescendantByName("", 3000).Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(true);
            GC.SuppressFinalize(this);
        }




    }
}
