using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Recon
{
    public class ViewDataOnRecon : ReconUIMap, ITestStep
    {
       public static List<String> ViewDetails=new List<String>();

       
        
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenRecon();
                MaximizeRecon();
                InputDetails(testData, sheetIndexToName);
                ViewDetails=setDetails(testData, sheetIndexToName);
                BtnView.Click(MouseButtons.Left);

            }

            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeRecon();
            }
            return _result;
        }

            private void InputDetails(DataSet testData, Dictionary<int, string> sheetIndexToName)
            {
                String fromdate = string.Empty;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()))
                        fromdate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString()));
                    
                    DtFromDatePicker.Click(MouseButtons.Left);
                    ExtentionMethods.CheckCellValueConditions(fromdate, string.Empty, true);
                    DtToDatePicker.Click(MouseButtons.Left);
                    String todate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()))
                        todate = String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString()));
                    ExtentionMethods.CheckCellValueConditions(todate, string.Empty, true);

                    if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconClient].ToString() != String.Empty)
                    {
                        CmbbxClient.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconClient].ToString();
                    }

                    if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconType].ToString() != String.Empty)
                    {
                        CmbbxReconType.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconType].ToString();
                    }

                    if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconFormat].ToString() != String.Empty)
                    {
                        CmbbxReconTemplates.Properties[TestDataConstants.TEXT_PROPERTY] = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconFormat].ToString();
                    }

                }

                      
            }

            public List<String> setDetails(DataSet testData, Dictionary<int, string> sheetIndexToName)
            {
                List <String> details=new List<string>();

                details.Add(String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_FROM].ToString())));
                details.Add(String.Format(ExcelStructureConstants.COMMON_DATE_FORMAT, DateTime.Parse(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_TO].ToString())));
                details.Add( testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconType].ToString());   
                details.Add(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.Col_ReconFormat].ToString());
              return details;



            }
       

       
    }
    

}
