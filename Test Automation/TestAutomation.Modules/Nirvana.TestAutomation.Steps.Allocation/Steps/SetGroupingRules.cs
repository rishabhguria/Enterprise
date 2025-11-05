using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.TestDataProvider;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;


namespace Nirvana.TestAutomation.Steps.Allocation
{
    public class SetGroupingRules : PreferencesUIMap, ITestStep
    {

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                OpenAutoGrouping();
                SetPreferences(testData, sheetIndexToName);
                CloseAutoGrouping();
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "SetGroupingRules");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                MinimizeAllocation();
            }
            return _res;
        }

        /// <summary>
        /// sets the Auto Grouping preferences
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void SetPreferences(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                DataRow dtRow = testData.Tables[sheetIndexToName[0]].Rows[0];
                //foreach (DataRow dtRow in testData.Tables[sheetIndexToName[0]].Rows)
                //{
                InputPreferences(dtRow);
                //}
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        /// <summary>
        /// Opens the Auto Groupings.
        /// </summary>
        private void OpenAutoGrouping()
        {
            try
            {
                Preferences.Click(MouseButtons.Left);
                AutoGroupingRules.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Closes the Auto Grouping window
        /// </summary>
        private void CloseAutoGrouping()
        {
            try
            {
                Save.Click(MouseButtons.Left);
                Close.Click(MouseButtons.Left);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Checks and unchecks the preferences according to a row in the input sheet
        /// </summary>
        /// <param name="dtRow"></param>
        private void InputPreferences(DataRow dtRow)
        {
            try
            {
                bool result = false;
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_AUTOALLOCATION].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_AUTOALLOCATION].ToString(), out result);
                    if (result && !ChkboxAutoGroup.IsChecked && ChkboxAutoGroup.IsEnabled)
                    {
                        ChkboxAutoGroup.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxAutoGroup.IsChecked && ChkboxAutoGroup.IsEnabled)
                    {
                        ChkboxAutoGroup.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_BROKER].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_BROKER].ToString(), out result);
                    if (result && !ChkboxBroker.IsChecked)
                    {
                        ChkboxBroker.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxBroker.IsChecked)
                    {
                        ChkboxBroker.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_VENUE].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_VENUE].ToString(), out result);
                    if (result && !ChkboxVenue.IsChecked)
                    {
                        ChkboxVenue.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxVenue.IsChecked)
                    {
                        ChkboxVenue.Click(MouseButtons.Left);
                    }
                }
                bool resultTradeDate = ChkboxTradeDate.IsChecked;
                bool resultProcessDate = ChkboxProcessDate.IsChecked;
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADEDATE].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADEDATE].ToString(), out resultTradeDate);
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_PROCESSDATE].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_PROCESSDATE].ToString(), out resultProcessDate);
                }
                if (resultTradeDate || resultProcessDate)
                {
                    if(resultTradeDate && !ChkboxTradeDate.IsChecked)
                        ChkboxTradeDate.Click(MouseButtons.Left);
                    if(resultProcessDate && !ChkboxProcessDate.IsChecked)
                        ChkboxProcessDate.Click(MouseButtons.Left);
                    if (!resultTradeDate && ChkboxTradeDate.IsChecked)
                        ChkboxTradeDate.Click(MouseButtons.Left);
                    if (!resultProcessDate && ChkboxProcessDate.IsChecked)
                        ChkboxProcessDate.Click(MouseButtons.Left);
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADING_ACCOUNT].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADING_ACCOUNT].ToString(), out result);
                    if (result && !ChkboxTradingAccount.IsChecked)
                    {
                        ChkboxTradingAccount.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradingAccount.IsChecked)
                    {
                        ChkboxTradingAccount.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_ASSET_CLASS].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_ASSET_CLASS].ToString(), out result);
                    if (result && !ChkboxAssetClass.IsChecked && ChkboxAssetClass.IsEnabled)
                    {
                        ChkboxAssetClass.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxAssetClass.IsChecked && ChkboxAssetClass.IsEnabled)
                    {
                        ChkboxAssetClass.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE1].ToString(), out result);
                    if (result && !ChkboxTradeAttribute1.IsChecked)
                    {
                        ChkboxTradeAttribute1.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute1.IsChecked)
                    {
                        ChkboxTradeAttribute1.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE2].ToString(), out result);
                    if (result && !ChkboxTradeAttribute2.IsChecked)
                    {
                        ChkboxTradeAttribute2.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute2.IsChecked)
                    {
                        ChkboxTradeAttribute2.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE3].ToString(), out result);
                    if (result && !ChkboxTradeAttribute3.IsChecked)
                    {
                        ChkboxTradeAttribute3.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute3.IsChecked)
                    {
                        ChkboxTradeAttribute3.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE4].ToString(), out result);
                    if (result && !ChkboxTradeAttribute4.IsChecked)
                    {
                        ChkboxTradeAttribute4.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute4.IsChecked)
                    {
                        ChkboxTradeAttribute4.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE5].ToString(), out result);
                    if (result && !ChkboxTradeAttribute5.IsChecked)
                    {
                        ChkboxTradeAttribute5.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute5.IsChecked)
                    {
                        ChkboxTradeAttribute5.Click(MouseButtons.Left);
                    }
                }
                if (!string.IsNullOrEmpty(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString()))
                {
                    Boolean.TryParse(dtRow[TestDataConstants.COL_TRADE_ATTRIBUTE6].ToString(), out result);
                    if (result && !ChkboxTradeAttribute6.IsChecked)
                    {
                        ChkboxTradeAttribute6.Click(MouseButtons.Left);
                    }
                    else if (!result && ChkboxTradeAttribute6.IsChecked)
                    {
                        ChkboxTradeAttribute6.Click(MouseButtons.Left);
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
