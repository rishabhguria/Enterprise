using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.ComponentModel;
using System.Data;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core.UIAutomationSupport;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.PortfolioManagement
{
    public class CheckPortfolioManagement : PortfolioManagementUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenConsolidationView();
                Wait(9000);
                //ExtentionMethods.WaitForVisible(ref PMGrid, 15);
                List<String> errors = CheckPM(testData, sheetIndexToName);
                Main.Click(MouseButtons.Left);
                if (errors.Count > 0)
                {
                    _result.IsPassed = false;
                    _result.ErrorMessage = String.Join(", ", errors.ToArray());
                }

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CheckPortfolioManagement");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            finally
            {
                PMclose();
            }
            return _result;
        }

        /// <summary>
        /// Checks the pm.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private List<string> CheckPM(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                Main.WaitForRespondingOrExited();
                Main.Click(MouseButtons.Left);
                AutomationElementWrapper wrapper = new AutomationElementWrapper(Main.MsaaObject.Children[1].WindowHandle);
                WaitOnItems(wrapper);
                Wait(10000);
                DataTable superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));

                if (superset.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(superset.Rows[0]["Cost Basis P&L (Base)"].ToString()) || superset.Rows[0]["Cost Basis P&L (Base)"].ToString() == "0")
                    {
                        Wait(5000);
                        superset = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.Main.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                    }
                }


                List<String> columns = new List<String>();
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                try
                {
                    string StepName = "CheckPortfolioManagement";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception)
                { }
                if (subset.Columns.Contains(TestDataConstants.COL_CHECK_ACCOUNTVALUE))
                {
                    try
                    {
                        
                        foreach (DataRow dr in subset.Rows)
                        {
                            DataRow tempdr = subset.NewRow();
                            if (!string.IsNullOrEmpty(dr[TestDataConstants.COL_CHECK_ACCOUNTVALUE].ToString()))
                            {
                                tempdr.ItemArray = dr.ItemArray.Clone() as object[];
                                tempdr[TestDataConstants.COL_CHECK_ACCOUNTVALUE] = string.Empty;

                                DataRow[] DataRows = DataUtilities.GetMatchingDataRows(superset, tempdr);


                                if (DataRows.Length > 0)
                                {
                                    foreach (DataRow row in DataRows)
                                    {
                                        if (string.IsNullOrEmpty(row[TestDataConstants.COL_ACCOUNT].ToString()))
                                        {
                                            dr[TestDataConstants.COL_CHECK_ACCOUNTVALUE] = string.Empty;
                                        }
                                        else if (!string.IsNullOrEmpty(row[TestDataConstants.COL_ACCOUNT].ToString()) && (!string.IsNullOrEmpty(dr[TestDataConstants.COL_CHECK_ACCOUNTVALUE].ToString())))
                                        {
                                            throw new Exception("CheckAccountValue failed");
                                        }
                                    }
                                }
                                else
                                    throw new Exception("No matching datarow found");
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message +"CheckAccountValue failed-'Either Account column contains value but IsAccountBlank is checked TRUE or Need to add more data top get precise row to get verified'");
                    }
                }
                List<String> errors = new List<String>();
                DataRow drow = null;
                if (subset.Rows.Count > 0)
                {
                    drow = subset.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                if (subset.Columns.Contains("MandatoryColumn"))
                {

                    if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                    {
                        columns = MandatoryColumns.PmColumns();
                    }
                    else
                    {
                        columns.Add("Symbol");
                        columns.Add("Order Side");
                    }
                    subset.Columns.Remove("MandatoryColumn");
                }
                else
                {
                    columns.Add("Symbol");
                    columns.Add("Order Side");
                }

               
                    try
                    {
                        string StepName = "CheckPortfolioManagement";
                        DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                        Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                        SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref superset);
                    }
                    catch (Exception)
                    { }
                


                errors = Recon.RunRecon(superset, subset, columns, 0.0000000001);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void WaitOnItems(AutomationElementWrapper wrapper)
        {
            try
            {
                Stopwatch tmr = new Stopwatch();
                tmr.Start();

                while (wrapper.Children.Count <= 1)
                {
                    if (tmr.ElapsedMilliseconds >= 30000)
                    {
                        break;
                    }
                }
                tmr.Stop();
            }
            catch (Exception)
            {                
                throw;
            }
        }
    }
}