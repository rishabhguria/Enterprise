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
using OpenQA.Selenium.Support.UI;


namespace Nirvana.TestAutomation.Steps.Simulator
{
    class LoginFix : CameronSimulator, ITestStep, IOpenFinTestStep
    {

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            // ✅ Forward to RunOpenFinTest with "samsara" as default PD
            return RunOpenFinTest(testData, sheetIndexToName, "samsara");
        }

        // ✅ Interface-required method
        public TestResult RunOpenFinTest(DataSet testData, Dictionary<int, string> sheetIndexToName, string PD)
        {
            TestResult _res = new TestResult();

            try
            {
                string moduleName = "Login";
                string KeyNameEmail = "EmailInput";
                string KeyNamePassword = "PasswordInput";
                string KeyNameButton = "LoginButton";
                string KeyNameFix = "Fix4m";
                string KeyNameBlotter = "BlotterIn";

                var table = SamsaraHelperClass.ExcelToSamsaraDataTable(ConfigurationManager.AppSettings["SamsaraDataFile"])[moduleName];

                //WebDriver driver = null;
                //ChromeOptions options = new ChromeOptions();
                //options.DebuggerAddress = "localhost:8084";
                //driver = new RemoteWebDriver(new Uri("http://localhost:9515"), options);



                DriverManager.Initialize();

                IWebDriver driver = DriverManager.Driver;
                WebDriverWait wait = DriverManager.Wait;

                driver.Navigate().GoToUrl("https://app.fixsim.com/Account/Login?ReturnUrl=%2FHome");

                DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows[0];

                // Username
                if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_LOGINUSER].ToString()))
                {
                    string username = dataRow[TestDataConstants.COL_LOGINUSER].ToString();
                    ApplicationArguments.ReleaseUserName = username;
                    driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNameEmail, moduleName)))
                          .SendKeys(username);

                    // wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNameEmail, moduleName)))).SendKeys(username);

                }

                // Password
                if (!string.IsNullOrEmpty(dataRow[TestDataConstants.COL_LOGINPASSWORD].ToString()))
                {
                    string password = dataRow[TestDataConstants.COL_LOGINPASSWORD].ToString();
                    ApplicationArguments.ReleasePassword = password;

                    // wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNamePassword, moduleName)))).SendKeys(password);
                    driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNamePassword, moduleName)))
                           .SendKeys(password);

                }


                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNameButton, moduleName)))).Click();

                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNameFix, moduleName)))).Click();
                IWebElement blotter = driver.FindElement(By.XPath(SamsaraHelperClass.SamsaraXpath(KeyNameBlotter, moduleName)));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", blotter);


                _res.IsPassed = true;
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }

            return _res;
        }
    }
}
