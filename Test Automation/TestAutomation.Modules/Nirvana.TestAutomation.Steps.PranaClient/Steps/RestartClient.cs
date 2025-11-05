using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using System.Diagnostics;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    [UITestFixture]
    public partial class RestartClient : PranaClientUIMap, ITestStep
    {
        /// <summary>
        /// Restart the Client. 
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                try
                {
                    if (PranaMain.IsVisible)
                    {
                        ControlPartOfPranaMain_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                        if (SaveLayout.IsVisible)
                        {
                            ButtonNo.Click(MouseButtons.Left);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Prana application is not Visible");
                }
                Wait(2000);
                Process[] _process = Process.GetProcessesByName("Prana");
                foreach (Process proc in _process)
                    proc.Kill();
                ExtentionMethods.WaitForVisibleUIApplication(ref PranaApplication, 40);
                Wait(3000);

                //If testdata is present ,it restart according to its values ,else it restart according to the Default values( ReleaseUserName , ReleasePassword)
                if (testData !=null && testData.Tables.Contains("RestartClient"))
                {
                    DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows[0];
                    string user = dataRow[TestDataConstants.COL_LOGINUSER].ToString();
                    ApplicationArguments.ReleaseUserName = user;
                    string password = dataRow[TestDataConstants.COL_LOGINPASSWORD].ToString();
                    ApplicationArguments.ReleasePassword = password;
                }

                PranaApplication.BringToFront();
                TxtLoginID.Click(MouseButtons.Left);
                Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                TxtPassword.Click(MouseButtons.Left);
                Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                BtnLogin.Click(MouseButtons.Left);
                if (CustomMessageBox.IsEnabled)
                {
                    UltraOkButton.Click();
                }
                return _result;
            }
            catch
            {
                _result.IsPassed = false;
                return _result;
                throw;
            }
        }

    }
}
