using System.Data;
using System.Linq;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;
using System.Reflection;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    class ExportToExcel : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                OpenBlotter();
//Wait(6000);
                MaximizeBlotter();
                ExportToExcel.Click(MouseButtons.Left);
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                SaveAs.WaitForVisible();
                Clipboard.SetText(path + ExcelStructureConstants.BlotterExportFileName);
                Keyboard.SendKeys("[CTRL+V]");
                ButtonSave.Click(MouseButtons.Left);
                ConfirmSaveAs1.WaitForObject();
                if (ConfirmSaveAs1.IsValid == true)
                    ButtonYes3.Click(MouseButtons.Left);
                if (VerifyExport(uiMsaa1.WrappedMsaaObject.Name) == false)
                    _res.IsPassed = false;
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
                MinimizeBlotter();
            }
            return _res;
        }

        /// <summary>
        /// method to check if data exported correctly or not
        /// </summary>
        /// <param name="status"></param>
        private bool VerifyExport(string status)
        {
            bool result = false;
            try
            {
                if (status.Contains("Data exported")==true)
                    result = true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return result;
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
