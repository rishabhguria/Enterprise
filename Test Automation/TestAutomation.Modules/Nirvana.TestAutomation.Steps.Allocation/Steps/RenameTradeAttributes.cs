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
    public class RenameTradeAttributes : PreferencesUIMap, ITestStep
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
                OpenAllocation();
                OpenAttributeRenaming();
                SetPreferences(testData, sheetIndexToName);
                CloseAttributeRenaming();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "RenameTradeAttributes");
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
        /// Sets the Attribute Renaming.
        /// </summary>
        private void SetPreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                InputPreferences(testData.Tables[sheetIndexToName[0]].Rows[0]);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Click on the attribute and edit it
        /// </summary>
        /// <param name="dtRow"></param>
        private void InputPreferences(DataRow dtRow)
        {

            try
            {
                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString().Equals(String.Empty))
                {

                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[0].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString(), "");
                }

                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString().Equals(String.Empty))
                {
                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[1].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString(), "");
                }

                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString().Equals(String.Empty))
                {
                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[2].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString(), "");
                }

                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString().Equals(String.Empty))
                {
                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[3].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString(), "");
                }

                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString().Equals(String.Empty))
                {
                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[4].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString(), "");
                }

                if (!dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString().Equals(String.Empty))
                {
                    AttributeRenamingGrid.AutomationElementWrapper.CachedChildren[5].CachedChildren[1].WpfClick();
                    ExtentionMethods.UpdateCellValueConditions(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString(), "");
                }
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
