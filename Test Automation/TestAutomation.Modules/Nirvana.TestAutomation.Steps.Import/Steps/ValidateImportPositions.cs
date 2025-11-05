using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.BussinessObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.UIAutomation;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.Configuration;

namespace Nirvana.TestAutomation.Steps.Import
{
    class ValidateImportPositions : ImportUIMap, IUIAutomationTestStep
    {
        public TestResult RunUIAutomationTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult result = new TestResult();

            try
            {
                if (testData == null || testData.Tables.Count == 0)
                {
                    return result;
                }
                Keyboard.SendKeys(ConfigurationManager.AppSettings["OPEN_IMPORT"]);
                //ExtentionMethods.WaitForVisible(ref ImportData1, 15);
                
                var requiredColumns = new[] { "Continue", "Abort", "Security Master" };
                var table = testData.Tables[0];

                foreach (var columnName in requiredColumns)
                {
                    if (!table.Columns.Contains(columnName))
                    {
                        return result;
                    }
                }

                UIAutomationHelper uiAutomationHelperImport = new UIAutomationHelper();
                uiAutomationHelperImport.ValidateImportPositions("ImportPositionsDisplayForm", table);
                if (ImportData2.IsVisible) {
                    ButtonOK.Click(MouseButtons.Left);
                }
                result.IsPassed = true;
            }
            catch (Exception ex)
            {
                result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                {
                    throw;
                }
            }

            finally
            {
                KeyboardUtilities.CloseWindow(ref ImportData_UltraFormManager_Dock_Area_Top);
            }

            return result;
        }

        /// <summary>
        /// Disposes resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
