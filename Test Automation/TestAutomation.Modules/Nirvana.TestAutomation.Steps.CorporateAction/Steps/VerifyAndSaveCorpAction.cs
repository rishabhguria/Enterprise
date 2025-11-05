using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.CorporateAction
{
    public class VerifyAndSaveCorpAction : CorporateActionUIMap,ITestStep
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

            // Instance of CheckSpinOff method from VerifyAndApplyCorpAction class 
            var _instance = new VerifyAndApplyCorpAction();
            try
            {
                OpenCorporateActionsUI();
                List<String> errors = _instance.CheckSpinOff(testData.Tables[0]);
                if (errors.Count > 0)
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
                }
                SaveCorporateAction.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
            }

            finally
            {
                KeyboardUtilities.MinimizeWindow(ref FrmCorporateActionNew_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
    }
}
