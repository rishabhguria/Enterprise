using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Interfaces;
using System.Collections.Generic;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class VerifyOrder : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                OrdersTab.Click(MouseButtons.Left);
                
                List<String> columns = new List<String>();
                DataTable subset = testData.Tables[sheetIndexToName[0]];
                try
                {
                    string StepName = "VerifyOrder";
                    DataSet columMapDs = DataUtilities.GetTestCaseTestData(ApplicationArguments.columnMappingFile, 1, 1, columns);
                    Nirvana.TestAutomation.Utilities.SamsaraCustomizableVerificationHandler.LinkExcelData(ref subset);
                    SamsaraCustomizableVerificationHandler.CustomizableVerificationHandler(ref StepName, columMapDs.Tables["VerificationHandlerOnEnterprise"], ref subset);
                }
                catch (Exception)
                { }
                List<String> errors = InputEnter(subset);
                if (errors.Count > 0)
                    _res.ErrorMessage = String.Join("\n\r", errors);
               
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyOrder");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseBlotter();
            }
            return _res;
        }
        private List<String> InputEnter(DataTable dTable)
        {
            try
            {
                Dictionary<string,string> SchemeNameIndexMapping = new Dictionary<string,string>();
                DataTable dt = dTable.Copy();
                GetAllColumnsOnGrid(dTable);                
                DataTable dtBlotter = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.DgBlotter1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtBlotter = DataUtilities.RemoveCommas(dtBlotter);

                if (dTable.Columns.Contains(TestDataConstants.COL_ALLOCATIONSCHEMENAME.ToString()))
                {
                    foreach (DataRow dr in dTable.Rows)
                    {
                        bool exist = true;
                        if (!String.IsNullOrEmpty(dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString()) && (dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString().Contains("Custom") || dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString().Contains("PTT") || dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString().Contains("Rebal") || dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString().Contains("Stage")))
                        {

                            string SchemeName = dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME].ToString();

                            dr[TestDataConstants.COL_ALLOCATIONSCHEMENAME] = String.Empty;

                       
                        DataRow[] DataRows = DataUtilities.GetMatchingDataRows(dtBlotter, dr);
                          
                        if (DataRows.Length > 0)
                        {
                            foreach (DataRow row in DataRows)
                            {
                                
                               if (!row["Allocation Scheme Name"].ToString().Contains(SchemeName))
                                {
                                    exist = false;
                                }

                                else if (row["Allocation Scheme Name"].ToString().Contains(SchemeName))
                                {
                                    exist = true;
                                    Console.WriteLine("Allocation Scheme Name present in UI and verified");
                                    /* Allocation Scheme Name column verification handling
                                     * https://dev.azure.com/NirvanaSolutions/NirvanaOne/_workitems/edit/60709
                                     * Modified by Rishabh Jaiswal
                                    */
                                    break;
                                }



                            }
                        }
 
                        }
                        if (exist.Equals(false))
                        {
                            throw new Exception("Allocation Scheme Name not present in UI"); 
 
                        }
    

                    }
                }
                List<String> columns = new List<string>();
                /*columns = (from DataColumn x in dTable.Columns
                select x.ColumnName).ToList();*/

                DataRow drow = null;
                if (dTable.Rows.Count > 0)
                {
                    drow = dTable.Rows[0];
                }
                // If Table contains mandatory columns then verify mandatory columns
                    if (dTable.Columns.Contains("MandatoryColumn"))
                    {

                        if (!String.IsNullOrEmpty(drow["MandatoryColumn"].ToString()))
                        {
                            columns = MandatoryColumns.OrderGrid();
                        }
                        else
                        {
                            columns.Add("Symbol");
                        }
                        dTable.Columns.Remove("MandatoryColumn");
                    }

                    else
                    {
                        columns.Add("Symbol");
                    }
                    if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
                    {
                        try
                        {
                            string StepName = "VerifyOrder";
                            SamsaraTestDataHandler(StepName, dtBlotter, dTable, new List<String>());

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error occur on SamsaraTestDataHandler :" + ex.Message);
                        }
                    }
                DataUtilities.VerifyDate(dTable, dtBlotter);
                List<String> errors = Recon.RunRecon(dtBlotter, dTable, columns, 0.01);

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
                this.DgBlotter1.InvokeMethod("AddColumnsToGrid", columns);
                SaveAllLayout.Click(MouseButtons.Left);
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
