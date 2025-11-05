using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Import
{
    class SelectAllOnImport : ImportUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string importType = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_Import_Type].ToString();
                TradesUpload(importType);
                bool isPopupVisible = false;
                if (NirvanaWarning.IsVisible)
                {
                    ButtonNo.Click(MouseButtons.Left);
                    isPopupVisible = true;
                }
                if (ImportData2.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                if (isPopupVisible)
                {
                    TradesUpload(importType);
                }
                Wait(5000);
                GridRunUpload.Click(MouseButtons.Left);
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
                KeyboardUtilities.CloseWindow(ref ImportData_UltraFormManager_Dock_Area_Top);
            }
            return _result;
             }
        private void TradesUpload(string importType)
        {
            try
            {
               

                /*if (importType.Equals("Transaction"))
                    TransactionHeader.Click(MouseButtons.Left);
                else if (importType.Equals("Mark Price"))
                    MarkPriceHeader.Click(MouseButtons.Left);
                else if (importType.Equals("Forex Price"))
                    ForexPriceHeader.Click(MouseButtons.Left);
                else if (importType.Equals("Activities"))
                    ActivitiesHeader.Click(MouseButtons.Left);
                else if (importType.Equals("Double Entry Cash"))
                {
                    DoubleEntryCashHeader.Click(MouseButtons.Left);
                    ColumnHeader1.Click(MouseButtons.Left);
                }
                else if (importType.Equals("Multileg Journal Import"))
                    ColumnHeader.Click(MouseButtons.Left);
                else if (importType.Equals("Double Entry Journal Import"))
                    ColumnHeader1.Click(MouseButtons.Left);
                else if (importType.Equals("Staged Order"))
                {
                    var msaaObj = GrdImportData.MsaaObject;
                    
                    ColumnHeader3.Click(MouseButtons.Left);
                    
                }*/
                var msaaObj = GrdImportData.MsaaObject;
                msaaObj.CachedChildren[1].CachedChildren[0].FindDescendantByName("", 3000).Click(MouseButtons.Left);
                    btnContinue.Click(MouseButtons.Left);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
