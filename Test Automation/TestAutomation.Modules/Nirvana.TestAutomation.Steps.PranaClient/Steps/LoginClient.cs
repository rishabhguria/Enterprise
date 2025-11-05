using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Reflection;
using System.Configuration;
using OpenQA.Selenium.Interactions;
using System.Threading;
using System.IO;
using System.Diagnostics;
using Nirvana.TestAutomation.UIAutomation;
using UIAutomationClient;
using System.Runtime.InteropServices;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    class LoginClient : PranaClientUIMap, ITestStep, IOpenFinTestStep
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public TestResult RunOpenFinTest(DataSet testData, Dictionary<int, string> sheetIndexToName, string PD)
        {
            TestResult _res = new TestResult();
            // LogOutUser();
            // StartSamsaraApplication();
            string moduleName = "Login";
            var table = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraDataFile"])[moduleName];
            TestResult _result = new TestResult();
            if (PD.ToLower() == "samsara")
            {
                WebDriver driver = null;
                try
                {
                    ChromeOptions options = new ChromeOptions();
                    options.DebuggerAddress = "localhost:8084";
                    driver = new RemoteWebDriver(new Uri("http://localhost:9515"), options);
                }
                catch {
                    driver = SamsaraHelperClass.restartChromeDriver(driver);
                }

                if (!SwitchWindow.SwitchToWindow(driver, "Login"))
                {
                    if (SwitchWindow.SwitchToWindow(driver, "Dock"))
                    {
                        string KeyName = "More";
                        string moduleName2 = "HomeDock";
                        var element = driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyName, moduleName2)));
                        element.Click();
                        Actions actions = new Actions(driver);
                        //actions.KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("m").SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
                         //actions.Click(element).KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("m").SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
                        Wait(3000);
                        Keyboard.SendKeys(KeyboardConstants.DOWN_ARROWKEY);
                        Keyboard.SendKeys("M");
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                    }
                    return new TestResult();
                }

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    string KeyName = table.Rows[i]["Key Name"].ToString();
                    string action = table.Rows[i]["Actions"].ToString();
                    string VariableName = table.Rows[i]["Variable Name"].ToString();
                    string moduleName2 = "HomeDock";
                    if (action == "SendKeys")
                    {
                        try
                        {
                            driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyName, moduleName))).SendKeys(SamsaraHelperClass.GetVariableValue(VariableName));
                        }
                        catch
                        {
                            _res.IsPassed = false;
                            throw;
                        }
                    }
                    else if (KeyName == "Wait")
                    {
                        try
                        {
                            Console.WriteLine("Waiting...");
                           string timeInSecondsAsString = table.Rows[i]["Time(In Seconds)"].ToString();
                           if (!string.IsNullOrEmpty(timeInSecondsAsString))
                           {
                               int timeInSeconds = int.Parse(timeInSecondsAsString);
                               Thread.Sleep(timeInSeconds * 1000);
                           }
                        }
                        catch(Exception)
                        {
                        }
                    }
                    else if (KeyName == "Review Security Popup")
                    {
                        try
                        {
                            IUIAutomation automation = new CUIAutomation();
                            IUIAutomationElement mainWindow = automation.GetRootElement().FindFirst(
                        TreeScope.TreeScope_Children,
                        automation.CreatePropertyCondition(30005, " Review Security Permissions"));

                            IUIAutomationElement buttonElement = mainWindow.FindFirst(TreeScope.TreeScope_Children, automation.CreatePropertyCondition(30011, "3"));

                            if (buttonElement != null)
                            {
                                IUIAutomationInvokePattern invokePattern = buttonElement.GetCurrentPattern(10000) as IUIAutomationInvokePattern;
                                if (invokePattern != null)
                                {
                                    invokePattern.Invoke();
                                }
                                Thread.Sleep(15000);
                            }
                        }
                        catch { }
                    }
                    else if (action == "Click")
                    {
                        Wait(10000);
                        try
                        {
                            driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyName, moduleName2))).Click();
                        }
                        catch{}
                        if (string.Equals(KeyName, "More", StringComparison.OrdinalIgnoreCase))
                        {
                            Wait(3000);
                            Keyboard.SendKeys("M");
                        }
                    }
                    else if (KeyName == "ShiftWindow")
                    {

                        SamsaraHelperClass.MoveWindow(ref driver, ref action);
                    }
                    else if ( KeyName== "SwitchWindow")
                    {
                        SwitchWindow.SwitchToWindow(driver, action);
                    }
                }
                //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Username_InputBox", "Login"))).SendKeys("Support2");
                //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Password_InputBox", "Login"))).SendKeys("Nirvana@1");
                //Wait(5000);
                //driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath("Login_Button", "Login"))).Click();
                //Wait(10000);

            }
            return _res;
        }

        public void StartSamsaraApplication()
        {
            if (ApplicationArguments.ProductDependency.ToLower() == "samsara")
            {
                try
                {
                    Wait(10000);
                    /*ProcessStartInfo StartChromeExe = new ProcessStartInfo();
                    StartChromeExe.FileName = "Webapplication.bat";
                    StartChromeExe.WorkingDirectory = ConfigurationManager.AppSettings["ChromeDriverExePath"];
                    StartChromeExe.WindowStyle = ProcessWindowStyle.Minimized;
                    Process ChromeProcess = new Process();
                    ChromeProcess.StartInfo = StartChromeExe;
                    ChromeProcess.Start();
                    Wait(60000);*/

                    string baseFileName = "output";
                    string extension = "txt";
                    string uniqueFileName = DataUtilities.GetUniqueFileName(baseFileName, extension);
                    string sourceFilePath = Path.Combine(ConfigurationManager.AppSettings["SamsaraDirectory"], uniqueFileName);
                    //string sourceFilePath = ConfigurationManager.AppSettings["SamsaraDirectory"] + "\\output.txt";
                    try
                    {

                        if (File.Exists(sourceFilePath))
                        {
                            File.Delete(sourceFilePath);
                        }
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }

                    ProcessControlManager.ProcessStarter1("Webapplication.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"], uniqueFileName);
                    CommonMethods.VerifyStringInTextFile((sourceFilePath), "http://", 6000);
                    ProcessControlManager.ProcessStarter("YarnStartBatch.bat", ConfigurationManager.AppSettings["ChromeDriverExePath"]);



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private void LogOutUser()
        {
            WebDriver driver = null;
            try
            {
                
                ChromeOptions options = new ChromeOptions();
                options.DebuggerAddress = "localhost:8084";
                driver = new RemoteWebDriver(new Uri("http://localhost:9515"), options);
            }
            catch { }
            
            string name = string.Empty;
            try
            {
                foreach (var handle in driver.WindowHandles)
                {
                    name = handle;
                    Console.WriteLine("Try to switch window to " + "Dock");

                    driver.SwitchTo().Window(name);
                    Console.WriteLine(driver.Title);
                    string a = driver.Title;
                    if (driver.Title.Contains("Dock"))
                    {
                        driver.FindElement(By.XPath("/html/body/div[1]/div[2]/div/div/ul[2]/li[4]/span/button")).Click();
                        System.Threading.Thread.Sleep(2000);
                        driver.FindElement(By.XPath("/html/body/div[2]/div/div[2]/button[2]")).Click();
                        System.Threading.Thread.Sleep(5000);
                        break;
                    }
                }
                
                //bool found = false;
            //long start = System.currentTimeMillis();
            //while (!found)
            //{
                // }
                //Task.Delay(1000);

            }

            catch (Exception e)
            {
                // some windows may get closed during Runtime startup
                // so may get this exception depending on timing
                Console.WriteLine("Ignoring NoSuchWindowException " + name + e);
            }
        }



        /// <summary>
        /// Run the step.
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                if (PranaApplication.IsVisible)
                {

                    Process[] processes = Process.GetProcessesByName("Prana");
                    if (processes.Length == 0)
                    {
                        Console.WriteLine("Prana Not running");
                    }
                    else
                    {
                        Process.GetProcessesByName("Prana")[0].Kill();
                    }
                    Wait(2000);
                    PranaApplication.Start();
                    Wait(5000);
                }
                InputParameters(testData, sheetIndexToName);
                Wait(1000);
                BtnLogin.Click();
                Wait(5000);
                if (CustomMessageBox.IsVisible)
                {
                    UltraOkButton.Click();
                }
                Wait(2000);
                ApplicationArguments.ReleaseUserName = "Support1";
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
        /// For Login into Nirvana Client Application
        /// </summary>
        /// <param name="testData">The test data</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        protected void InputParameters(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                //There can be only 1 row in LoginClient as there can be only 1 login user and password at a time
                DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows[0];
                if (dataRow[TestDataConstants.COL_LOGINUSER].ToString() != string.Empty) //Login UserName
                {
                    TxtLoginID.Click(MouseButtons.Left);
                    string item = dataRow[TestDataConstants.COL_LOGINUSER].ToString();
                    ApplicationArguments.ReleaseUserName = item;
                    Keyboard.SendKeys(ApplicationArguments.ReleaseUserName);
                }

                if (dataRow[TestDataConstants.COL_LOGINPASSWORD].ToString() != string.Empty)
                {
                    TxtPassword.Click(MouseButtons.Left);
                    string item = dataRow[TestDataConstants.COL_LOGINPASSWORD].ToString(); // Login Password
                    ApplicationArguments.ReleasePassword = item;
                    Keyboard.SendKeys(ApplicationArguments.ReleasePassword);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
