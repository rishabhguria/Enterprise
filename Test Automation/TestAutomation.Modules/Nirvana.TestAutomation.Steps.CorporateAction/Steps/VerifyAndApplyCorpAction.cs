using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.UI;
using TestAutomationFX.Core;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public partial class VerifyAndApplyCorpAction : CorporateActionUIMap, ITestStep
    {
       /// <summary>
       /// Run the Test
       /// </summary>
       /// <param name="testData"></param>
       /// <param name="sheetIndexToName"></param>
       /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenCorporateActionsUI();
                List<String> errors = CheckSpinOff(testData.Tables[0]);
                if (errors.Count == 0)
                {
                    Apply.DoubleClick(MouseButtons.Left);
                    if (CorporateAction1.IsVisible)
                    {
                        ButtonYes.Click(MouseButtons.Left);
                    }
                }
                else
                {
                    _result.ErrorMessage = String.Join("\n\r", errors); 
                }
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
                KeyboardUtilities.MinimizeWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }

        /// <summary>
        /// Verify the grid data after previewing the Spin Off
        /// </summary>
        /// <param name="dtable"></param>
        /// <returns></returns>
        public List<string> CheckSpinOff(DataTable dtable)
        {
            List<String> _errors = new List<string>();
            try
            {
                ViewAllColumnsOnGrid(dtable);
                {
                    DataTable dtCorporateAction = DataUtilities.RemoveTrailingZeroes(CSVHelper.CSVAsDataTable(this.GrdPositions.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString()));
                    dtCorporateAction = DataUtilities.RemoveCommas(dtCorporateAction);
                    List<String> columns = new List<string>();
                    _errors = Recon.RunRecon(dtCorporateAction, dtable, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _errors;
        }
    }
}
