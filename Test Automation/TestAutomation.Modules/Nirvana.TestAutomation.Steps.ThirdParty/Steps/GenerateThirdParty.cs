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
    public class GenerateThirdParty : ThirdPartyUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenThirdParty();
                List<String> errors = new List<String>();
                errors = ThirdParty(testData, sheetIndexToName);

                if (errors.Count > 0)
                {
                    _result.ErrorMessage=String.Join(Environment.NewLine, errors);
                }
            }
            catch
            {
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
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Thirds the party.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        private List<string> ThirdParty(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                var grid = GrdJobView.MsaaObject;
                grid.CachedChildren[0].CachedChildren[5].Click(MouseButtons.Left);
                if (Error.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                if (FlatFileGenerationInformation.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);
                Wait(2000);
                grid.CachedChildren[0].CachedChildren[6].Click(MouseButtons.Left);
                if (Error.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                Wait(2000);
                grid.CachedChildren[0].CachedChildren[5].Click(MouseButtons.Left);
                if (Error.IsVisible)
                    ButtonOK.Click(MouseButtons.Left);
                if (FlatFileGenerationInformation.IsVisible)
                    ButtonYes.Click(MouseButtons.Left);
                DataTable gridForexData = CSVHelper.CSVAsDataTable(this.GrdThirdParty.InvokeMethod("GetAllVisibleDataFromTheGrid", null).ToString());
                DataTable subset = testData.Tables[sheetIndexToName[1]];
                List<String> columns = new List<String> { TestDataConstants.COL_CLIENTPRODID, TestDataConstants.COL_QUANTITY };
                List<String> errors = new List<String>();

                errors = Recon.RunRecon(gridForexData, subset, columns, 0.01);
                return errors;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}