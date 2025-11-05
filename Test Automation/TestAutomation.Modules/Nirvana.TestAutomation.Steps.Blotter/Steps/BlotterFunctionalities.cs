using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Blotter
{
  public class BlotterFunctionalities:BlotterUIMap,ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                Boolean found;
                OpenBlotter();
                //Wait(12000);
                DataRow dr = null;
                if (testData.Tables.Count > 2)
                {
                    OrdersTab.Click(MouseButtons.Left);
                    found = SelectBlotterTrades(DgBlotter1, testData.Tables[sheetIndexToName[0]].Rows[0]);
                    found = SelectBlotterTrades(DgBlotter2, testData.Tables[sheetIndexToName[1]].Rows[0]);
                    dr = testData.Tables[sheetIndexToName[2]].Rows[0];
                }
                else
                {
                    if (testData.Tables[sheetIndexToName[0]].Rows[0]["BlotterGrid"].ToString().Equals("WorkingSubs"))
                    {
                        WorkingSubsTab.Click(MouseButtons.Left);
                        found = SelectBlotterTrades(DgBlotter, testData.Tables[sheetIndexToName[0]].Rows[0]);

                    }
                    else if (testData.Tables[sheetIndexToName[0]].Rows[0]["BlotterGrid"].ToString().Equals("Order"))
                    {
                        OrdersTab.Click(MouseButtons.Left);
                        DgBlotter1.Click(MouseButtons.Left);
                        found = SelectBlotterTrades(DgBlotter1, testData.Tables[sheetIndexToName[0]].Rows[0]);
                    }
                    dr = testData.Tables[sheetIndexToName[1]].Rows[0];
                }


                MouseController.RightClick();
                if (dr["Trade"].ToString() != string.Empty)
                {
                    Trade1.Click(MouseButtons.Left);
                }
                else if (dr["Edit"].ToString() != string.Empty)
                {
                    EditOrders.Click(MouseButtons.Left);
                }
                else if (dr["Trade (new Sub)"].ToString() != string.Empty)
                {
                    TradeNewSub.Click(MouseButtons.Left);
                }
                else if (dr["Replace"].ToString() != string.Empty)
                {
                    Replace.Click(MouseButtons.Left);
                   // Wait(5000);
                }
                else if (dr["Cancel"].ToString() != string.Empty)
                {
                    Cancel.Click(MouseButtons.Left);
                    ButtonYes1.Click(MouseButtons.Left);
                   // Wait(5000);
                }
                BlotterMain2.BringToFront();

             
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                KeyboardUtilities.CloseWindow(ref BlotterMain_UltraFormManager_Dock_Area_Top);
            }
            return _res;
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
