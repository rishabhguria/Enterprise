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
    class VerifyOrderSide : TradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenManualTradingTicket();
                string symbol = testData.Tables[0].Rows[0]["Symbol"].ToString();
                string side = testData.Tables[0].Rows[0]["OrderSideToVerify"].ToString();
                string quantity = string.Empty;
                if(testData.Tables[0].Columns.Contains("Quantity"))
                    quantity = testData.Tables[0].Rows[0]["Quantity"].ToString();
                string AllocationMethod = string.Empty;
                if (testData.Tables[0].Columns.Contains("AllocationMethod"))
                    AllocationMethod = testData.Tables[0].Rows[0]["AllocationMethod"].ToString();

                if (!string.IsNullOrEmpty(symbol)) 
                {
                    if (!symbol.ToUpper().Equals(TxtSymbol.Properties[TestDataConstants.TEXT_PROPERTY].ToString().ToUpper()))
                    {
                        throw new Exception("Excel has Symbol " + symbol + " but UI is showing " + TxtSymbol.Properties[TestDataConstants.TEXT_PROPERTY].ToString());
                    }
                    else
                    {
                        Console.WriteLine(symbol + " symbol verified");
                    }
                }

                if (!string.IsNullOrEmpty(side))
                {
                    if (!side.ToUpper().Equals(CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY].ToString().ToUpper()))
                    {
                        throw new Exception("Excel has order side " + side + " but UI is showing " + CmbOrderSide.Properties[TestDataConstants.TEXT_PROPERTY].ToString());
                    }
                    else {
                        Console.WriteLine(side + " orderside verified");
                    }
                }

                if (!string.IsNullOrEmpty(quantity))
                {
                    if (!quantity.ToUpper().Equals(NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY].ToString().ToUpper()))
                    {
                        throw new Exception("Excel has quantity " + quantity + " but UI is showing " + NmrcQuantity.Properties[TestDataConstants.TEXT_PROPERTY].ToString());
                    }
                    else
                    {
                        Console.WriteLine(quantity + " quantity verified");
                    }
                }

                if (!string.IsNullOrEmpty(AllocationMethod))
                {
                    if (!AllocationMethod.ToUpper().Equals(CmbAllocation.Properties[TestDataConstants.TEXT_PROPERTY].ToString().ToUpper()))
                    {
                        throw new Exception("Excel has AllocationMethod " + AllocationMethod + " but UI is showing " + CmbAllocation.Properties[TestDataConstants.TEXT_PROPERTY].ToString());
                    }
                    else
                    {
                        Console.WriteLine(AllocationMethod + " AllocationMethod verified");
                    }
                }

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
                KeyboardUtilities.CloseWindow(ref TradingTicket_UltraFormManager_Dock_Area_Top);
            }
            return _res;
        }
    }
}
