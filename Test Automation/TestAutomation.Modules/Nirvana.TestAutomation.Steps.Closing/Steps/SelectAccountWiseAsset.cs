using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class SelectAccountWiseAsset : ClosingPreferencesUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingPreferences();
                    PreferencesMain_UltraFormManager_Dock_Area_Top.DoubleClick();
                   //  SelectAsset(dr);
                     if (!ColumnHeader.IsChecked)
                     {
                         ColumnHeader.Click(MouseButtons.Left);
                     }
                    BtnSave.Click(MouseButtons.Left);
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
                KeyboardUtilities.CloseWindow(ref PreferencesMain_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        //protected void SelectAsset(DataRow dr)
        //{
        //    try
        //    {
        //        //var msaaObj = GrdClosingMethod.MsaaObject;
        //        //DataTable currentDataGrid = CSVHelper.CSVAsDataTable(this.GrdClosingMethod.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
        //        //DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(currentDataGrid), dr);
        //        //int index = currentDataGrid.Rows.IndexOf(dtRow);
        //        //GrdClosingMethod.InvokeMethod("ScrollToRow", index);
        //        //Wait(1000);
        //        //msaaObj.CachedChildren[0].CachedChildren[index + 1].Click(MouseButtons.Right);
        //        if (!ColumnHeader.IsChecked)
        //        {
        //            ColumnHeader.Click(MouseButtons.Left);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //            throw;
        //    }
        //}

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
