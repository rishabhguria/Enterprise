using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.Data;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class ExportCalculatedPreference : CalculatedPreferencesUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenCalculatedPreferences();
                ExportCalculatedPreferenceName(testData, sheetIndexToName);
                Records.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_R);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "ExportCalculatedPreference");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }

        /// <summary>
        /// Exports the calculated name of the preference.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
        private void ExportCalculatedPreferenceName(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
             {
                Dictionary<string, int> preferenceIndexMap = GetPreferenceIndexMap();
                string exportFromPrefName = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_PREFERENCE_NAME].ToString();
                // Reverting the changes made with out proper testing
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_EXPORT_FOLDER_PATH].ToString();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string exportPath=path+"\\"+exportFromPrefName;
               
                if (!exportFromPrefName.Equals(string.Empty))
                {
                    Records.Click(MouseButtons.Left);
                    if (preferenceIndexMap.ContainsKey(exportFromPrefName))
                    {
                        Records.AutomationElementWrapper.FindDescendantByName(exportFromPrefName).CachedChildren[0].CachedChildren[0].WpfClickLeftBound(MouseButtons.Right); 
                        ExportPreference.Click(MouseButtons.Left);
                        Wait(1000);
                        clearText(TextBoxFilename);
                        Keyboard.SendKeys(exportPath);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                        if (Chooselocationtosaveyourfile.IsVisible && Chooselocationtosaveyourfile1.IsVisible)
                            ButtonYes2.Click(MouseButtons.Left);
                    }
                    else
                    {
                        throw new Exception(MessageConstants.MSG_PREF_NOT_FOUND+" for export");
                    }
                } 
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
