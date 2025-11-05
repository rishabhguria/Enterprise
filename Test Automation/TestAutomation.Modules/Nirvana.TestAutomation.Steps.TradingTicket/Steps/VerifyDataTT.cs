using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
   public class VerifyDataTT : TradingTicketUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        /// 
       
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenManualTradingTicket();
                if (testData != null)
                {
                    String step;
                    DataRow firstRow = testData.Tables[0].Rows[0];
                    if (firstRow["CommissionBasis"].ToString() != String.Empty && firstRow["Soft Basis"].ToString() != String.Empty && firstRow["Execution Instructions"].ToString() != String.Empty)
                    {
                        step = "BrokerWisePreference";
                    }
                    else
                    {
                        step = "UIPreference";
                    }
                    DataTable superset = VerifyDefaultData(testData, step);
                    DataTable subset = testData.Tables[sheetIndexToName[0]];
                    List<String> columns = new List<String>();
                    if (step.Equals("BrokerWisePreference"))
                    {
                        columns.Add("Broker");
                    }
                    var reconErrors = Recon.RunRecon(superset, subset, columns, 0.01);
                    //Wait(2000);
                    if (reconErrors.Count > 0)
                    {
                        _result.ErrorMessage = String.Join("\n\r", reconErrors);
                    }
                    return _result;
                }

              //  Wait(2000);
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
                KeyboardUtilities.CloseUI();
            }

            return _result;
        }

        /// <summary>
        /// Verifies the default data.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        private DataTable VerifyDefaultData(DataSet testData, String step)
        {
            try
            {
                TxtSymbol.Click(MouseButtons.Left);
                ExtentionMethods.CheckCellValueConditions(testData.Tables[0].Rows[0][TestDataConstants.COL_SYMBOL].ToString(), KeyboardConstants.ENTERKEY, true);
                Wait(1000);
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                Wait(2000);

                DataTable superset = CreateDataTable(step);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    DataRow dtrow = InsertData(superset.NewRow(), step, dr);
                    superset.Rows.Add(dtrow);
                }
                return superset;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Inserts the data.
        /// </summary>
        /// <param name="dtrow">The dtrow.</param>
        /// <param name="step">The step.</param>
        /// <param name="dr">The dr.</param>
        /// <returns></returns>
        public DataRow InsertData(DataRow dtrow, String step, DataRow dr)
        {
            try
            {
                dtrow["Symbol"] = TxtSymbol.Text;
                if (step.Equals("BrokerWisePreference"))
                {
                    if (!CmbBroker.Text.Equals(dr[TestDataConstants.COL_BROKER].ToString(), StringComparison.InvariantCultureIgnoreCase)) // change cmbobox only if value needs to be changed
                    {
                        ClearText(CmbBroker);
                        CmbBroker.Click(MouseButtons.Left);
                        Keyboard.SendKeys(dr[TestDataConstants.COL_BROKER].ToString());
                    }
                    Wait(1000);
                    dtrow["Broker"] = CmbBroker.Text;
                    Commision.Click(MouseButtons.Left);
                    dtrow["CommissionBasis"] = CmbCommissionBasis.Text;
                    dtrow["Soft Basis"] = CmbSoftCommissionBasis.Text;
                    Other.Click(MouseButtons.Left);
                    dtrow["Execution Instructions"] = CmbExecutionInstructions.Text;
                }
                else if (step.Equals("UIPreference"))
                {
                    dtrow["Broker"] = CmbBroker.Text;
                    dtrow["Order Type"] = CmbOrderType.Text;
                    dtrow["TIF"] = CmbTIF.Text;
                    dtrow["Strategy"] = CmbStrategy.Text;
                    dtrow["Account"] = CmbAllocation.Text;
                    Settlement.Click(MouseButtons.Left);
                    dtrow["Settlement Currency"] = CmbSettlementCurrency.Text;
                    Other.Click(MouseButtons.Left);
                    dtrow["Venue"] = CmbVenue.Text;
                    dtrow["Handling Instructions"] = CmbHandlingInstructions.Text;
                    dtrow["Trader"] = CmbTradingAccount.Text;
                    dtrow["Execution Instructions"] = CmbExecutionInstructions.Text;
                }

            }
            catch (Exception)
            {
                
                throw;
            }
            return dtrow;
        }
        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <param name="step">The step.</param>
        /// <returns></returns>
        private static DataTable CreateDataTable(String step)
        {
            DataTable defaultValues = new DataTable();
            try
            {
                defaultValues.Columns.Add("Symbol");
                defaultValues.Columns.Add("Broker");
                defaultValues.Columns.Add("Execution Instructions");

                if (step.Equals("BrokerWisePreference"))
                {
                    defaultValues.Columns.Add("CommissionBasis");
                    defaultValues.Columns.Add("Soft Basis");
                }
                else if (step.Equals("UIPreference"))
                {
                    defaultValues.Columns.Add("Venue");
                    defaultValues.Columns.Add("Order Type");
                    defaultValues.Columns.Add("TIF");
                    defaultValues.Columns.Add("Handling Instructions");
                    defaultValues.Columns.Add("Strategy");
                    defaultValues.Columns.Add("Account");
                    defaultValues.Columns.Add("Settlement Currency");
                    defaultValues.Columns.Add("Trader");
                }

            }
            catch (Exception)
            {
                
                throw;
            }

            return defaultValues;
        }
        /// <summary>
        /// Clears the text.
        /// </summary>
        /// <param name="cmb">The CMB.</param>
        private void ClearText(UIWindow cmb)
        {
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                while (cmb.Text.Length > 0 && timer.ElapsedMilliseconds < 50000)
                {
                    cmb.Click(MouseButtons.Left);
                    Keyboard.SendKeys("[END][SHIFT+HOME][BACKSPACE]");
                }
                timer.Stop();
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
