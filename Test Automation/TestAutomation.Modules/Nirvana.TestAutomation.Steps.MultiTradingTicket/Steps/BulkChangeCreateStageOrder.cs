using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.MultiTradingTicket
{
    class BulkChangeCreateStageOrder : MultiTradingTicketUIMap, ITestStep
    {
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref  MultiTradingTicket_UltraFormManager_Dock_Area_Top);
                MultiTradingTicket.InvokeMethod("SelectAllOrders", null);
                // Maximize.Click(MouseButtons.Left);
                ///Wait(2000);
                // EditTrades(testData, sheetIndexToName, GrdTrades); 
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        BulkChange(dr, BtnStage);

                    }

                }
                BtnStage.Click(MouseButtons.Left);// if no trades needs bulk update,then it will only press on create order button 
                Wait(5000);
                if (testData.Tables[0].Rows.Count > 0)
                {
                    if (testData.Tables[0].Columns.Contains(TestDataConstants.COL_INFORMATION))
                    {
                        if (uiWindow1.IsValid)
                        {
                            if (testData.Tables[0].Rows[0][TestDataConstants.COL_INFORMATION].ToString().ToUpper() == "YES")
                            {
                                ButtonYes1.Click(MouseButtons.Left);

                            }
                            else if (testData.Tables[0].Rows[0][TestDataConstants.COL_INFORMATION].ToString().ToUpper() == "NO")
                            {
                                ButtonNo.Click(MouseButtons.Left);
                            }
                            else
                                ButtonYes1.Click(MouseButtons.Left);
                        }
                        if (testData.Tables[0].Rows[0][TestDataConstants.COL_INFORMATION].ToString().ToUpper() == "YES")
                        {
                            ButtonYes1.Click(MouseButtons.Left);

                        }

                    }
                }
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (PromptWindow_Fill_Panel.IsVisible)
                    {
                        BtnPlace.Click(MouseButtons.Left);
                    }
                }

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
    }
}
