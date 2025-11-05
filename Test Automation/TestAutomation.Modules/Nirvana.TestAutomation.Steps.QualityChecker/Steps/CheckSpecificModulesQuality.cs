using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.QualityChecker
{
    class CheckSpecificModulesQuality: QualityCheckerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                RestoreWindow();
                string modulename;
                for (int i = 0; i < testData.Tables[0].Rows.Count;i++ )
                {
                    DataRow dr = testData.Tables[0].Rows[i];
                    modulename = dr[0].ToString();
                    if (DetectionScripts.IsChecked == true)
                    {
                        DetectionScripts.Click(MouseButtons.Left);
                        Keyboard.SendKeys("[SPACE]");
                    }
                    ClickOnModule(modulename);
                    Diagnose();
                    Wait(10000);
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
                MinimizeWindow();
            }
            return _res;
        }

        /// <summary>
        /// Check the desired script
        /// </summary>
        /// <param name="disposing"></param>
        private void ClickOnModule(string modulename)
        {
            try
            {
                Admin.MsaaName = modulename;
                Admin.Click(MouseButtons.Left);
                Keyboard.SendKeys("[SPACE]");
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Diagnose the given scripts
        /// </summary>
        /// <param name="disposing"></param>
        private void Diagnose()
        {
            try
            {
                BtnDiagnose.Click(MouseButtons.Left);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }
    }
}
