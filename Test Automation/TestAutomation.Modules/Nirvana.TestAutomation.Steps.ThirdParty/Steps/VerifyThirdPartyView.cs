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
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.ThirdParty
{
    class VerifyThirdPartyView:ThirdPartyUIMap,ITestStep
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

                List<String> errors = VerifyData(testData.Tables[0]);
                if (errors.Count > 0)
                    _result.ErrorMessage = String.Join("\n\r", errors);
                //Minimize Third Party Manager
                //MinimizeThirdPartyManager();
                KeyboardUtilities.CloseWindow(ref FrmThirdParty_UltraFormManager_Dock_Area_Top);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }

        /// <summary>
        /// Verify data after View, allocated on third party grid
        /// </summary>
        /// <param name="dTable"></param>
        /// <returns></returns>
        private List<String> VerifyData(DataTable dTable)
        {
            List<String> errors = new List<string>();
            try
            {
                DataTable dtGrid = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdThirdParty.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                dtGrid = DataUtilities.RemoveCommas(dtGrid);
                List<String> columns = new List<string>();
                errors = Recon.RunRecon(dtGrid, dTable, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;
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
