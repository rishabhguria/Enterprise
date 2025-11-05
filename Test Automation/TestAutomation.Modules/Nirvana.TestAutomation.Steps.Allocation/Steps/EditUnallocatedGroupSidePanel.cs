using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    internal class EditUnallocatedGroupSidePanel : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Begins the test execution
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenAllocation();
                AllocationGrids.Click(MouseButtons.Left);
                AllocationGrids1.Click(MouseButtons.Left);
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                UpdateTrades(testData, sheetIndexToName);
                Apply.DoubleClick(MouseButtons.Left);
               // Wait(1000);
                if (NirvanaAllocation.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                 //   Wait(1000);
                } 
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                    SavewDivideStatus.Click(MouseButtons.Left);
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                    SavewDivideoStatus.Click(MouseButtons.Left);
                //Wait(1000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "EditUnallocatedGroupSidePanel");
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
        /// Enters the updated trade values
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="sheetIndexToName"></param>
        private void UpdateTrades(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                int count = 0;
                EditTrade.Click(MouseButtons.Left);
                Dictionary<string, UIAutomationElement> columnControlMap = GetColumnControlMap();
                DataTable dt = testData.Tables[sheetIndexToName[0]];
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        switch (col.ColumnName)
                        {
                            case "Commission":
                                HeaderSiteBasicDetails.Click(MouseButtons.Left);
                                Wait(500);
                                break;
                            case "AUECFee2":
                            case "InternalComments":
                            case "TradeAttribute1":
                                count = 0;
                                while(count<15)
                                {
                                IncreaseBtn.Click(MouseButtons.Left);
                                Wait(500);
                                count++;
                                }
                                Wait(500);
                                break;
                        }
                        if (!string.IsNullOrEmpty(row[col].ToString()) && columnControlMap.ContainsKey(col.ColumnName))
                        {
                            EnterValueInControl(columnControlMap[col.ColumnName], row[col].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// returns map of column name to control
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, UIAutomationElement> GetColumnControlMap()
        {
            try
            {
                Dictionary<string, UIAutomationElement> map = new Dictionary<string, UIAutomationElement>();
                map.Add("Symbol", TextBox5);
                map.Add("Asset", TextBox6);
                map.Add("ExecutedQty", CumQty);
                map.Add("AvgPrice", AvgPrice1);
                map.Add("Side", ComboBox12);
                map.Add("TradeDate", TradeDateTimeEditor);
                map.Add("ProcessDate", XamMaskedEditor2);
                map.Add("OriginalPurchaseDate", XamMaskedEditor3);
                map.Add("SettlementDate", XamMaskedEditor4);
                map.Add("TotalCommAndFees", TotalCommissionandFees);
                map.Add("Commission", Commission);
                map.Add("SoftCommission", SoftCommission);
                map.Add("OtherBrokerFees", OtherBrokerFees);
                map.Add("ClearingBrokerFees", ClearingBrokerFee);
                map.Add("StampDuty", StampDuty);
                map.Add("TransactionLevy", TransactionLevy);
                map.Add("AUECFee1", AUECFee1);
                map.Add("TaxOnCommissions", TaxOnCommissions);
                map.Add("AUECFee2", AUECFee2);
                map.Add("SecFee", SecFee);
                map.Add("OccFee", OccFee);
                map.Add("OrfFee", OrfFee);
                map.Add("Broker", ComboBoxBroker);
                map.Add("Venue", ComboBoxVenue);
                map.Add("Description", Description1);
                map.Add("InternalComments", InternalComments1);
                map.Add("FXRate", FXRate1);
                map.Add("FxConvOperator", ComboBoxFxConvOperator);
                map.Add("SettleFxRate", SettlCurrFxRate);
                map.Add("SettleFxOperator", ComboBoxSettlFxOperator);
                map.Add("SettlementPrice", SettlCurrAmt);
                map.Add("AccruedInterest", AccruedInterest1);
                map.Add("TradeAttribute1", TradeAttribute11);
                map.Add("TradeAttribute2", TradeAttribute21);
                map.Add("TradeAttribute3", TradeAttribute31);
                map.Add("TradeAttribute4", TradeAttribute41);
                map.Add("TradeAttribute5", TradeAttribute51);
                map.Add("TradeAttribute6", TradeAttribute61);

                return map;
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