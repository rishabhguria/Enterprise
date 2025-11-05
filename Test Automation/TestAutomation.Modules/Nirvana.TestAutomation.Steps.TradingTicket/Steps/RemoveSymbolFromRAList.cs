using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public partial class RemoveSymbolFromRAList : TTRestristedAllowedUIMap, ITestStep
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
                OpenRestrictedAllowedTab();
                Wait(500);
                PranaUltraGrid1.Click(MouseButtons.Left);               
                RemoveSymbol(testData, sheetIndexToName);
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

        private void RemoveSymbol(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            DataTable dtgrid = CSVHelper.CSVAsDataTable(this.PranaUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
            DataTable table = new DataTable();
            table.Clear();            
            table = testData.Tables[0];
            DataColumn dc= new DataColumn();

            if (testData.Tables[0].Columns.Count > 0 && dtgrid.Columns.Count > 0)            
            {
                testData.Tables[0].Columns[0].ColumnName = dtgrid.Columns[0].ColumnName;
            }
            

            foreach (DataRow dr in table.Rows)
            {
                DataTable dtgrid1 = CSVHelper.CSVAsDataTable(this.PranaUltraGrid1.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataRow dtRow = DataUtilities.GetMatchingDataRow(DataUtilities.RemoveTrailingZeroes(dtgrid1), dr);
                int index = dtgrid1.Rows.IndexOf(dtRow);
                var msaobj = PranaUltraGrid1.MsaaObject;
                
                PranaUltraGrid1.InvokeMethod(TestDataConstants.COL_SCROLLTOROW, index);
                Wait(1000);
                //var Row = PranaUltraGrid1.MsaaObject.FindDescendantByName("RAList",3000);
                int row = msaobj.CachedChildren.Count;
                msaobj.CachedChildren[row-1].CachedChildren[index+1].Click(MouseButtons.Left);
                                
                Wait(1000);
                RemoveButton.Click(MouseButtons.Left);
                
            }
        }
    }
}
