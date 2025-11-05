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
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class VerifyDayEndCash: DayEndCashUIMap,ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenDayEndCash();
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                string toDate = String.Format(ExcelStructureConstants.DATE_FORMAT,Convert.ToDateTime(DtToDate.Text.ToString()));
                UIMsaa pageTab = SelectDateTab(toDate);
                pageTab.Click(MouseButtons.Left);
                List<String> errors = VerifyDayEndCashData(testData, sheetIndexToName[0], UltraGrid);
                if (errors.Count > 0)
                    cashErrors.Append("DayEndCashTabErrors:-" + String.Join("\n\r", errors));
                if (!string.IsNullOrEmpty(cashErrors.ToString()))
                {
                    _res.ErrorMessage=cashErrors.ToString();
                }
                
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeGeneralLedger();
            }
            return _res;
        }

        public UIMsaa SelectDateTab(string toDate)
        {
            try
            {
                UIMsaa dateTab = new UIMsaa();
                dateTab.Comment = null;
                dateTab.MsaaName = toDate;
                dateTab.Parent = TabCntlDayEndData;
                dateTab.Role = AccessibleRole.PageTab;
                dateTab.UIObjectType = UIObjectTypes.Unknown;
                dateTab.UseCoordinatesOnClick = true;
                return dateTab;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Verifies non trading transactions data
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetName"></param>
        /// <param name="TransactionsGrid"></param>
        /// <returns></returns>
        private List<String> VerifyDayEndCashData(DataSet testData, string sheetName, UIUltraGrid CashGrid)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                CashGrid.InvokeMethod("AddColumnsToGrid", colList);
                CashGrid.InvokeMethod("RemoveGrouping", null);                
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(CashGrid.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));                
                List<String> columns = new List<String>();
                columns = GetKeyColumnsForDayEndCash();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.01);
                return errors;
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
