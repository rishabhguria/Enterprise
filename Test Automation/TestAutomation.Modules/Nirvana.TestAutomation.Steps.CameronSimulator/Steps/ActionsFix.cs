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



namespace Nirvana.TestAutomation.Steps.Simulator
{
    class ActionsFix : CameronSimulator,ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();

            try
            {
                string moduleName = "Action";
                string keyActionButton = "ActionButton";

                DriverManager.Initialize(); // Attach or start driver
                IWebDriver driver = DriverManager.Driver;
                WebDriverWait wait = DriverManager.Wait;

                if (driver == null || wait == null)
                    throw new Exception("Driver or Wait is not initialized. Make sure LoginClient ran first.");

                DataTable table = testData.Tables[sheetIndexToName[0]];

                foreach (DataRow dataRow in table.Rows)  // ✅ Iterate all rows
                {
                    // DataRow dataRow = testData.Tables[sheetIndexToName[0]].Rows[0];

                    string symbol = dataRow["Symbol"].ToString();
                    string quantity = dataRow["Quantity"].ToString();
                    string price = dataRow["Price"].ToString();
                    string actionsRaw = dataRow["Actions"].ToString(); // e.g., "AcknowledgeLink,RejectLink"

                    string[] actionLinkKeys = actionsRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string rawKey in actionLinkKeys)
                    {
                        string actionKey = rawKey.Trim();
                        try
                        {
                            // Re-locate the trade row after every page reload
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
                                throw new Exception("Trade row not found after page reload for action: " + actionKey);

                            // Re-select the checkbox
                            IWebElement checkbox = row.FindElement(By.CssSelector(".select-checkbox"));
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", checkbox);

                            // Open Action dropdown
                            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath(keyActionButton, moduleName))))
                                .Click();

                            // Click the specific action link
                            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath(actionKey, moduleName))))
                                .Click();


                            if (actionKey.Equals("Execute Qty/Price", StringComparison.OrdinalIgnoreCase))
                            {
                                HandleExecuteQtyPrice(driver, wait, dataRow);
                            }
                            else if (actionKey.Equals("Restate", StringComparison.OrdinalIgnoreCase))
                            {
                                HandleRestate(driver, wait, dataRow);
                            }

                            Thread.Sleep(3000);


                            // Wait for the reload to complete
                            Thread.Sleep(3000); // Replace with better reload wait if known
                        }
                        catch (Exception actionEx)
                        {
                            Console.WriteLine("Action failed: " + actionKey + " - " + actionEx.Message);
                        }
                    }
                }
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


        private void HandleExecuteQtyPrice(IWebDriver driver, WebDriverWait wait, DataRow dataRow)
        {
            string moduleName = "ExecuteQtyPrice";



            var Symelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("Symbol", moduleName))));
            Symelement.Clear();
            Symelement.SendKeys(dataRow["Symbol"].ToString());

            var LQtyelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("LastQty", moduleName))));
            LQtyelement.Clear();
            LQtyelement.SendKeys(dataRow["LastQty"].ToString());

            var LastPXelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("LastPx", moduleName))));
            LastPXelement.Clear();
            LastPXelement.SendKeys(dataRow["LastPx"].ToString());

            if (dataRow.Table.Columns.Contains("AccruedInterest"))
            {
                var Accruedlement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("AccruedInterest", moduleName))));
                Accruedlement.Clear();
                Accruedlement.SendKeys(dataRow["AccruedInterest"].ToString());
            }

            //  if (dataRow.Table.Columns.Contains("TagNumber") && dataRow.Table.Columns.Contains("Value"))
            //{
            //  wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath("AddAdditionalTag", moduleName))))
            //    .Click();

            //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("TagNumber", moduleName))))
            //  .SendKeys(dataRow["TagNumber"].ToString());

            //                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("Value", moduleName))))
            //                  .SendKeys(dataRow["Value"].ToString());
            //}

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath("SendExecution", moduleName))))
                .Click();
        }

        private void HandleRestate(IWebDriver driver, WebDriverWait wait, DataRow dataRow)
        {
            string moduleName = "Restate";

            var Orderelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("OrderQty", moduleName))));
            Orderelement.Clear();
            Orderelement.SendKeys(dataRow["OrderQty"].ToString());

            var Execelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("ExecRestatementReason", moduleName))));
            Execelement.Clear();
            Execelement.SendKeys(dataRow["ExecRestatementReason"].ToString());

            var Symbelement = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("Symbol", moduleName))));
            Symbelement.Clear();
            Symbelement.SendKeys(dataRow["Symbol"].ToString()); ;


            //         if (dataRow.Table.Columns.Contains("TagNumber") && dataRow.Table.Columns.Contains("Value"))
            //       {
            //         wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath("AddAdditionalTag", moduleName))))
            //           .Click();

            //     wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("TagNumber", moduleName))))
            //       .SendKeys(dataRow["TagNumber"].ToString());

            // wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(SamsaraHelperClass.SamsaraXpath("Value", moduleName))))
            //   .SendKeys(dataRow["Value"].ToString());
            //}

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(SamsaraHelperClass.SamsaraXpath("SendExecution", moduleName))))
                .Click();
        }
    }
}






