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
    class BulkUpdateUsingMTT : MultiTradingTicketUIMap, ITestStep 
    {
        /// <summary>
        /// Run test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                //Keyboard.SendKeys(KeyboardConstants.ENTERKEY);
                MultiTradingTicket_UltraFormManager_Dock_Area_Top.Click(MouseButtons.Left);
                KeyboardUtilities.MaximizeWindow(ref  MultiTradingTicket_UltraFormManager_Dock_Area_Top);

                // Maximize.Click(MouseButtons.Left);
                Wait(2000);
                // EditTrades(testData, sheetIndexToName, GrdTrades); 
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        BulkChange(dr, BtnDoneAway);
                       
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
            finally
            {
             /*   if (MultiTradingTicket.IsValid)
                {
                    KeyboardUtilities.MinimizeWindow(ref MultiTradingTicket_UltraFormManager_Dock_Area_Top);
                }*/
            }
            return _result;
        }
    }
}
