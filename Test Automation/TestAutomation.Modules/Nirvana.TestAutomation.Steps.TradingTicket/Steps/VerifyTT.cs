using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;


namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    public class VerifyTT : TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenManualTradingTicket();
                string sheetName = sheetIndexToName[0];
                _res.ErrorMessage = CheckTTData(testData, sheetName);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyTT");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.MinimizeWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
            }
            return _res;
        }

        private string CheckTTData(DataSet testData, string sheetName)
        {
            try
            {
                String verifyRoundLotsStrip = String.Empty;
                String roundLotVal = String.Empty;
                String symbol = String.Empty;
                String button_status = String.Empty;
                String position = String.Empty;

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_SYMBOL))
                {
                    symbol = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_SYMBOL].ToString();
                }


                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_TARGET_QUANTITY_IN))
                {
                    if (!String.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_TARGET_QUANTITY_IN].ToString()))
                    {
                        if (testData.Tables[sheetName].Rows[0][TestDataConstants.COL_TARGET_QUANTITY_IN].ToString().ToUpper() != UpDownEdit2.IsVisible.ToString().ToUpper())
                        {
                            throw new Exception("Target quantity column's visiblity is set as " + UpDownEdit2.IsVisible.ToString().ToUpper() + " but TargetQuantityIn containes " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_TARGET_QUANTITY_IN].ToString());
                        }
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ROUND_LOTS))
                {
                    roundLotVal = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_ROUND_LOTS].ToString();
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Button_Status))
                {
                    button_status = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Button_Status].ToString().ToUpper();
                }

                if (!String.IsNullOrEmpty(button_status))
                {
                    if (button_status.Equals("TRUE") && !NmrcQuantity.IsEnabled)
                    {
                        throw new Exception("TT buttons are Disabled and Excel has True Value in Column Quantity");
                    }
                    else if (button_status.Equals("FALSE") && NmrcQuantity.IsEnabled)
                    {
                        throw new Exception("TT buttons are Enabled and Excel has False Value in Column Quantity");
                    }
                }

                if (NmrcPrice.IsEnabled && NmrcQuantity.IsEnabled)
                {
                    if (!String.IsNullOrEmpty(symbol))
                    {
                        if (TxtSymbol.MsaaObject.Value != symbol)
                        {
                            throw new Exception("Excel has " + symbol + " but UI has " + TxtSymbol.MsaaObject.Value);
                        }
                    }

                }


                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_ROUND_LOTS))
                {
                    roundLotVal = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_ROUND_LOTS].ToString();
                }


                if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_ALLOCATION_METHOD].ToString()))
                {
                    if (!CmbAllocation.Properties["Text"].ToString().Equals(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_ALLOCATION_METHOD].ToString()))
                    {
                        throw new Exception("Values on Excel is " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_ALLOCATION_METHOD].ToString() + " and Values on Grid is " + CmbAllocation.Properties["Text"]);
                    }
                }


                if (!String.IsNullOrEmpty(roundLotVal))
                {
                    if (roundLotVal == LblRoundLot.MsaaObject.Name)
                    {
                        Console.WriteLine("VALUES ARE MATCHED");
                    }
                    else
                    {
                        throw new Exception("Values on Excel is " + roundLotVal + " and Values on Grid is " + LblRoundLot.MsaaObject.Name);
                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Position.ToString()))
                {
                    if (!String.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Position].ToString()))
                    {
                        position = testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Position].ToString();
                    }
                }

                if (!String.IsNullOrEmpty(position))
                {
                    if (LblPosition.MsaaObject.Name == position)
                    {

                        Console.WriteLine("VALUES ARE MATCHED");
                    }
                    else
                    {
                        throw new Exception("Values on Excel is " + position + " and Values on Grid is " + LblPosition.MsaaObject.Name);
                    }
                }


                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Symbol_Name.ToString()))
                {
                    if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Symbol_Name].ToString()))
                    {
                        if (UltraFormCaptionArea.Value.ToString().Contains(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Symbol_Name].ToString()))
                        {
                            Console.WriteLine("Symbol value matched");
                        }
                        else
                        {
                            throw new Exception("Excel is showing " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Symbol_Name].ToString() + "But title baar is showing " + UltraFormCaptionArea.Value.ToString());
                        }

                    }
                }

                if (testData.Tables[sheetName].Columns.Contains(TestDataConstants.COL_Last.ToString()))
                {
                    if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Last].ToString()))
                    {
                        if (LblLast.MsaaObject.Name != testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Last].ToString())
                        {
                            throw new Exception("Values on Excel is " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Last].ToString() + " and Values on Grid is " + LblLast.MsaaObject.Name);
                        }
                    }
                    if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Change].ToString()))
                    {
                        if (LblChange.MsaaObject.Name != testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Change].ToString())
                        {
                            throw new Exception("Values on Excel is " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Change].ToString() + " and Values on Grid is " + LblChange.MsaaObject.Name);
                        }
                    }
                    if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Bid].ToString()))
                    {
                        if (LblBid.MsaaObject.Name != testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Bid].ToString())
                        {
                            throw new Exception("Values on Excel is " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Bid].ToString() + " and Values on Grid is " + LblBid.MsaaObject.Name);
                        }
                    }
                    if (!string.IsNullOrEmpty(testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Ask].ToString()))
                    {
                        if (LblAsk.MsaaObject.Name != testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Ask].ToString())
                        {
                            throw new Exception("Values on Excel is " + testData.Tables[sheetName].Rows[0][TestDataConstants.COL_Ask].ToString() + " and Values on Grid is " + LblAsk.MsaaObject.Name);
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

            return null;
        }
    }
}
