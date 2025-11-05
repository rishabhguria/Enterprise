using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using TestAutomationFX.Core;
using TestAutomationFX.UI;

namespace Nirvana.TestAutomation.Steps.TradingTicket
{
   public class CreateNewSubOrder : TradingTicketUIMap, ITestStep
    {
       
       public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //Wait(5000);
                ExtentionMethods.WaitForVisible(ref TradingTicket1, 20);
                if (TradingTicket1.IsVisible)
                {
                    if (testData != null)
                    {
                        foreach (DataRow dr in testData.Tables[0].Rows)
                        {

                            InputDetails(dr, BtnDoneAway);
                            BtnDoneAway.Click(MouseButtons.Left);
                            if (UltraButtonYes1.IsVisible)
                            {
                                UltraButtonYes1.Click(MouseButtons.Left);
                            }
                            if (PromptWindow_Fill_Panel.IsVisible)
                            {
                                if (dr[TestDataConstants.COL_PopUpQuantity].ToString() != String.Empty && NmrcTargetQuantity1.IsEnabled)
                                {
                                    BtnEdit.Click(MouseButtons.Left);
                                    BtnDoneAway.Click(MouseButtons.Left);
                                    NmrcTargetQuantity1.Properties[TestDataConstants.TEXT_PROPERTY] = dr[TestDataConstants.COL_PopUpQuantity].ToString();
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                                else
                                {
                                    BtnPlace.Click(MouseButtons.Left);
                                }
                            }
                            if (dr.Table.Columns.Contains(TestDataConstants.COL_ALLOW_TRADE))
                            {
                                if (dr[TestDataConstants.COL_ALLOW_TRADE].ToString() != String.Empty)
                                {
                                    CheckCompliance(dr);
                                }
                            }
                            if (TradingTicket1.IsValid)
                            {
                                if (TradingTicket2.IsVisible)
                                {
                                    ButtonYes1.Click(MouseButtons.Left);
                                }
                                //if (TradingTicket1.IsValid)
                                //{
                                //    if (TradingRulesViolatedPopUp.IsVisible)
                                //    {
                                //        UltraButtonYes.Click(MouseButtons.Left);
                                //    }
                                //}
                            }
                            if (TradingTicket1.IsValid)
                                {
                                    if (TradingRulesViolatedPopUp.IsVisible)
                                    {
                                        if (dr.Table.Columns.Contains(TestDataConstants.Col_Shares_Outstanding_PopUp))
                                            if (dr[TestDataConstants.Col_Shares_Outstanding_PopUp].Equals("No"))
                                            {
                                                UltraButtonNo.Click(MouseButtons.Left);
                                            }
                                            else
                                            {
                                                UltraButtonYes.Click(MouseButtons.Left);
                                            }
                                        else
                                        {
                                            UltraButtonYes.Click(MouseButtons.Left);
                                        }
                                    }
                                }
                                Wait(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "CreateNewSubOrder");
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
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