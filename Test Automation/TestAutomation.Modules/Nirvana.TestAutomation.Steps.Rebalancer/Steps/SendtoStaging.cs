using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.Interfaces.Enums;


namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class SendtoStaging : RebalancerUIMap, ITestStep
    {
        /// <summary>
        /// Run Test
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            //UpdateClientConfig ob = new UpdateClientConfig();
            TestResult _result = new TestResult();
            try
            {
                OpenRebalancer();
                Wait(4000);
                RebalancerTabButton.Click(MouseButtons.Left);
                ViewBuyDivideSellList.Click(MouseButtons.Left);
                
                if (NirvanaAlert1.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
               
                //check uncheck "Use custodian as Executing Broker" option
                DataRow dr1 = testData.Tables[0].Rows[0];
                if (testData.Tables[0].Columns.Contains(TestDataConstants.Cusdtodian_BrokerRB))
                {
                    if (!String.IsNullOrEmpty(dr1[TestDataConstants.Cusdtodian_BrokerRB].ToString()))
                    {
                        if (UseCustodianBroker.IsChecked.ToString().ToUpper() != dr1[TestDataConstants.Cusdtodian_BrokerRB].ToString().ToUpper())
                        {
                            UseCustodianBroker.Click(MouseButtons.Left);
                        }
                    }
                }
                string BuySellList = String.Empty;
                int Send = 0;

                foreach (DataRow dr in testData.Tables[0].Rows)
                {

                    if (dr.Table.Columns.Contains(TestDataConstants.COL_SEND_TO_STAGING))
                    {
                        BuySellList = dr["SendToStaging"].ToString();
                    }
                    if (BuySellList.ToUpper().Equals("FALSE"))
                    {
                        break;
                    }
                }
                Send = BuySellList.ToUpper().Equals("FALSE") ? 1 : 0;

                if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_SEND_TO_STAGING))
                {
                    testData.Tables[0].Columns.Remove("SendToStaging");
                }
                //Export Data To Excel

                DataTable dTable = ExportTradeListGrid();

                foreach (DataRow row in dTable.Rows)
                {
                    decimal originalValue = Convert.ToDecimal(row["Buy/Sell Value"]);
                    // Round the value and remove decimals by casting to int
                    row["Buy/Sell Value"] = (int)Math.Round(originalValue);
                }

                testData.Tables[0].Columns["Quantity"].ColumnName = "Buy/Sell Qty";
                testData.Tables[0].Columns["Buy/Sell Value (Base)"].ColumnName = "Buy/Sell Value";
                DataTable ExcelData = DataUtilities.RemoveColumnsAndRows("Use Custodian Broker", testData.Tables[0]);
                List<string> errors = VerifyGrid(dTable, ExcelData);

                if (errors.Count == 0)
                {
                    if (!string.IsNullOrEmpty(BuySellList) && Send == 1)
                    {
                        Wait(1000);
                    }
                    else
                    {
                        Save.Click(MouseButtons.Left);
                        Wait(2000);
                        Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.LEFT_ARROWKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                        // Commented because of customized message box
                        //if (NirvanaAlert5.IsVisible)
                        //{
                        //    ButtonYes3.Click(MouseButtons.Left);
                        //}
                        SendtoStaging.Click(MouseButtons.Left);
                        Wait(2000);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.TABKEY);
                        Keyboard.SendKeys(KeyboardConstants.ENTERKEY);

                        // Commented beacause of Custom Message Box
                        //if (NirvanaAlert5.IsVisible)
                        //{
                        //    ButtonYes3.Click(MouseButtons.Left);
                        //}

                        /*if (File.Exists(ApplicationArguments.FilePathToVerify))
                        {
                           Console.WriteLine("The .xlsx file exists in the folder.");
                           Console.WriteLine(ApplicationArguments.FilePathToVerify);
                        }*/

                    }
                }



                else
                {
                    _result.ErrorMessage = String.Join("\n\r", errors);
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SendtoStaging");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                //close the Trade List UI
                CloseTradeList();
                Wait(4000);
                //Minimize Rebalancer
                KeyboardUtilities.MinimizeWindow(ref RebalanceTab);
            }
            return _result;
        }


        /// <summary>
        /// Verify the Exported Grid Data with the Excel Sheet Data
        /// </summary>
        /// <param name="dt1">Storing the exported data</param>
        /// <param name="dt2">Storing the excel data</param>
        /// <returns></returns>
        private List<string> VerifyGrid(DataTable dt1, DataTable dt2)
        {
            List<string> errors = new List<string>();
            List<string> columns = new List<string>();
            try
            {
                errors = Recon.RunRecon(dt1, dt2, columns, 0.01, false, false, ReconType.RoundingMatch, 2, MidpointRounding.AwayFromZero);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return errors;

        }

    }
}