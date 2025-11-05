using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.GeneralLedger
{
    class VerifyCashTransaction : CashTransactionsUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCashTransactions();
                StringBuilder cashErrors = new StringBuilder(string.Empty);
                List<String> errors = VerifyCashTransactionsData(testData, sheetIndexToName[0]);
                if (errors.Count > 0)
                    cashErrors.Append("CashTransactionTabErrors:-" + String.Join("\n\r", errors));
                if (!string.IsNullOrEmpty(cashErrors.ToString()))
                {
                    _res.ErrorMessage = cashErrors.ToString();
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
        public List<String> VerifyCashTransactionsData(DataSet testData, string sheetName)
        {
            try
            {
                DataTable subset = testData.Tables[sheetName];
                List<String> colList = new List<String>();
                for (int i = 0; i < subset.Columns.Count; i++)
                {
                    colList.Add(subset.Columns[i].ColumnName);
                }
                GrdCashDividends.Click(MouseButtons.Left);
                GrdCashDividends.InvokeMethod("AddColumnsToGrid", colList);
                GrdCashDividends.InvokeMethod("RemoveGrouping", null);
                DataTable dtCashGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(GrdCashDividends.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                List<String> columns = GetKeyColumnsForCashTransaction();
                List<String> errors = new List<String>();
                errors = Recon.RunRecon(dtCashGrid, subset, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
