using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Nirvana.TestAutomation.BussinessObjects;
using TestAutomationFX.Core;
using System.Configuration;
using OpenQA.Selenium.Interactions;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.CameronSimulator
{
    class DeleteOrderFix : ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                string moduleName = "Action";
                string deleteButtonKey = "DeleteAllOrdersButton"; // key defined in Samsara config

                DriverManager.Initialize();
                IWebDriver driver = DriverManager.Driver;
                WebDriverWait wait = DriverManager.Wait;

                if (driver == null || wait == null)
                    throw new Exception("Driver or Wait is not initialized. Make sure LoginClient ran first.");

                DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows[0];

                string symbol = dataRow["Symbol"].ToString();
                string quantity = dataRow["Quantity"].ToString();
                string price = dataRow["Price"].ToString();

                // Locate the trade row
                IWebElement row = wait.Until(drv =>
                {
                    try
                    {
                        var element = drv.FindElement(By.XPath(
                            string.Format("//tr[.//td[contains(normalize-space(), '{0}')] " +
                                          "and .//td[contains(normalize-space(), '{1}')] " +
                                          "and .//td[contains(normalize-space(), '{2}')]]",
                                          symbol, quantity, price)));
                        return element.Displayed ? element : null;
                    }
                    catch { return null; }
                });

                if (row == null)
                    throw new Exception("Trade row not found for deletion.");

                // Select the checkbox in the row
                IWebElement checkbox = row.FindElement(By.CssSelector(".select-checkbox"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);

                // Click DeleteAllOrdersButton using XPath from config
                string deleteButtonXpath = SamsaraHelperClass.SamsaraXpath(deleteButtonKey, moduleName);
                IWebElement deleteButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(deleteButtonXpath)));
                deleteButton.Click();


                _res.IsPassed = true;
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow) throw;
            }

            return _res;
        }
    }
}
