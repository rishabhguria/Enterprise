using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Reflection;
using System.IO;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.ShortLocate
{
    public class DownloadFile : ShortLocateUIMap, ITestStep
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
                OpenShortLocateUI();
                UltraPanel1ClientArea3.Click(MouseButtons.Left);
                UltraPictureBoxDownload.Click(MouseButtons.Left);
                DataRow dr = testData.Tables[sheetIndexToName[0]].Rows[0];
                string format = dr[TestDataConstants.COL_FileFormat].ToString();
                if (format.Equals("CSV"))
                    ChkCSV.Click(MouseButtons.Left);
                else 
                    ChkExcel.Click(MouseButtons.Left);
                Btn_OK.Click(MouseButtons.Left);
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + TestDataConstants.CAP_AUTOMATION_FOLDER + @"\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                TextBoxFilename1.Click(MouseButtons.Left);
                Keyboard.SendKeys(path + ExcelStructureConstants.ShortLocateDownloadedFile);
                ButtonSave.Click(MouseButtons.Left);
                if (ConfirmSaveAs.IsVisible)
                {
                    ButtonYes.Click(MouseButtons.Left);
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
                KeyboardUtilities.CloseWindow(ref ShortLocate_UltraFormManager_Dock_Area_Top);
            }
            return _result;
        }
    }
}
