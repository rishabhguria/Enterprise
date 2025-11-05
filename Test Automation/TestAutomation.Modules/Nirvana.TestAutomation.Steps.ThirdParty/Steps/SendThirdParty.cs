using System;
using System.ComponentModel;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using System.Collections.Generic;
using System.Windows.Forms;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.ThirdParty
{
    public class SendThirdParty : ThirdPartyUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// 

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenThirdParty();
                var grid = GrdJobView.MsaaObject;
                grid.CachedChildren[0].CachedChildren[5].Click(MouseButtons.Left);// Click View button
                if (Error.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                if (FlatFileGenerationInformation.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);
                Wait(2000);
                grid.CachedChildren[0].CachedChildren[6].Click(MouseButtons.Left);// Click Generate button
                if (Error.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                Wait(2000);
                grid.CachedChildren[0].CachedChildren[7].Click(MouseButtons.Left); // Click Send button
                Wait(3000);
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
                KeyboardUtilities.CloseWindow(ref FrmThirdParty_UltraFormManager_Dock_Area_Top);         
            }
            return _result;
        }

        /// <summary>
        /// Opens the third party.
        /// </summary>
        private void OpenThirdParty()
        {
            try
            {
                Tools.Click(MouseButtons.Left);
                ThirdPartyManager.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}