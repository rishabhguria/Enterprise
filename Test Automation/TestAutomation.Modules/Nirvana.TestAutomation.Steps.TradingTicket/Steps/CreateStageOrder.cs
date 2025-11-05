using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using System.IO;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Nirvana.TestAutomation.Steps.TradingTicket.TradingTicketUIMap" />
    /// <seealso cref="Nirvana.TestAutomation.Interfaces.ITestStep" />
 public   class CreateStageOrder : TradingTicketUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">Name of the sheet index to.</param>
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
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        InputDetails(dr,BtnCreateOrder);
                        BtnCreateOrder.Click(MouseButtons.Left);
                        Wait(5000);
                        if (UltraButtonYes1.IsVisible)
                        {
                            UltraButtonYes1.Click(MouseButtons.Left);
                        }

                        if (ComplianceAlertPopUp1.IsVisible)
                        {
                            //ExportData();
                            if (ResponseButton3.IsVisible)
                            {
                                ResponseButton3.Click(MouseButtons.Left);
                            }
                        }
                        
                        if (TradingTicket1.IsValid)
                        {
                            if (TradingTicket2.IsVisible)
                            {
                                ButtonYes1.Click(MouseButtons.Left);
                            }
                            if (popUpWindow.IsVisible)
                            {
                                string error = popUpWindow.MsaaObject.CachedChildren[2].ToString();
                                ButtonOK1.Click(MouseButtons.Left);
                                if (!error.Contains("Trading Tool"))
                                {
                                    _result.ErrorMessage = error;
                                }
                            }
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                if (dr[TestDataConstants.COL_EditOrder].ToString() != String.Empty && dr[TestDataConstants.COL_EditOrder].ToString().Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    BtnEdit.Click(MouseButtons.Left);
                                }
                                else
                                {
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                            }
                            if (ComplianceAlertPopUp1.IsVisible)
                            {
                                //ExportData();
                                if (ResponseButton3.IsVisible)
                                {
                                    ResponseButton3.Click(MouseButtons.Left);
                                }
                            }
                            Wait(1000);
                        }
                        Wait(1000);
                    }

                }  
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateStageOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                CloseTradingTicket();
            }
           // Wait(1000);
            return _result;
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
