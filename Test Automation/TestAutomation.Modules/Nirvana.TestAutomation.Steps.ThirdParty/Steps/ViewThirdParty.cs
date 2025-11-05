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
using System.IO;

namespace Nirvana.TestAutomation.Steps.ThirdParty
{
    class ViewThirdParty : ThirdPartyUIMap, ITestStep
    {
        /// <summary>
        /// RunTest method takes two parameter-
        /// 1.testData of DataSet type which gives xls sheet data.
        /// 2.sheetIndexToName of Dictionary<int,string> type which gives xls sheet No. and sheet name
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //Open Third Party Manager.
                OpenThirdPartyManager();

                //Deselect already selected column header
                Wait(1000);
                DeselectGridCheckbox();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        MatchedSelectRow(dr);
                    }
                }

                //Minimize Third Party Manager
                MinimizeThirdPartyManager();
            }
            catch(Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }



        /// <summary>
        /// matchedSelect method compare test case of Datagrid with xls sheet and perform view operation.
        /// </summary>
        /// <param name="dr"></param>
        private void MatchedSelectRow(DataRow dr)
        {
            try
            {
                var msaaObj = GrdJob.MsaaObject;
                DataTable dtGrid = CSVHelper.CSVAsDataTable(this.GrdJob.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtGrid), dr);
                int index = dtGrid.Rows.IndexOf(dtRow);
                GrdJob.InvokeMethod("ScrollToRow", index);
                //Select index of matched row
                msaaObj.CachedChildren[0].FindDescendantByName("", 3000).Click(MouseButtons.Left);

                //Click on View button
                msaaObj.CachedChildren[0].FindDescendantByName("View", 3000).Click(MouseButtons.Left);
                Wait(3000);
                if (FlatFileGenerationInformation.IsVisible)
                    ButtonYes1.Click(MouseButtons.Left);

            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
