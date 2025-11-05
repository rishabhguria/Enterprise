using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class RunProrata : AllocationUIMap, ITestStep
    {

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                if (!testData.Tables[sheetIndexToName[0]].Columns.Contains("Continue"))
                {
                    OpenAllocation();
                    Records.Click(MouseButtons.Left);
                    Prorata.Click(MouseButtons.Left);
                }
               
                String date = string.Empty;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                    if (!dr.Table.Columns.Contains("Continue"))
                    {
                        if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DATE].ToString()))
                            date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0]["Date"].ToString()));

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DATE].ToString()))
                        {
                            string tempDate = DataUtilities.DateHandler(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DATE].ToString());
                            date = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(tempDate));
                        }
                    }
                    FromDate1.Click(MouseButtons.Left);
                    
                    ExtentionMethods.CheckCellValueConditions(date, string.Empty, true);

                    if (dr.Table.Columns.Contains(TestDataConstants.COL_SCHEME_BASIS))
                    {
                        CmbboxSchemeBasis.WaitForObject();

                        if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_SCHEME_BASIS].ToString()))
                        {
                            if (!CmbboxSchemeBasis.IsValid)
                            {
                                _res.IsPassed = false;
                                _res.ErrorMessage = "Cannot set scheme basis because advanced UI permission is turned off.";
                            }
                            else
                            {
                                ControlPartOfCmbboxSchemeBasis.Click(MouseButtons.Left);
                                Dictionary<String, int> Schemebasis = CreateDictionary(CmbboxSchemeBasis);
                               // Wait(2000);
                                CmbboxSchemeBasis.AutomationElementWrapper.CachedChildren[Schemebasis[dr[TestDataConstants.COL_SCHEME_BASIS].ToString()]].CachedChildren[0].WpfClick();
                            }
                        }
                    }

                    if (dr.Table.Columns.Contains(TestDataConstants.COL_SCHEME_NAME) &&  (!String.IsNullOrEmpty(dr[TestDataConstants.COL_SCHEME_NAME].ToString())))
                    {
                        CmbboxSchemeName.WaitForObject();
                        CmbboxSchemeName.Click(MouseButtons.Left);
                        if (!string.IsNullOrEmpty(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_SCHEME_NAME].ToString()))
                        {
                            if (!CmbboxSchemeName.IsValid)
                            {
                                _res.IsPassed = false;
                                _res.ErrorMessage += "Cannot set scheme name because advanced UI permission is turned off.";
                            }
                            else
                            {
                                ExtentionMethods.CheckCellValueConditions(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_SCHEME_NAME].ToString(), string.Empty, true);
                                Keyboard.SendKeys("[ENTER]");
                            }
                        }
                    }
                }
                Continue.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RunProrata");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// returns map of Scheme name to control
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, UIAutomationElement> GetColumnControlMap()
        {
            try
            {
                Dictionary<string, UIAutomationElement> map = new Dictionary<string, UIAutomationElement>();
                map.Add("SchemeName", CmbboxSchemeName);
                return map;
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
