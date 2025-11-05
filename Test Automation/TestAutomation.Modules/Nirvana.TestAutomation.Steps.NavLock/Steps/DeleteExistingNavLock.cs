using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.NavLock
{
    class DeleteExistingNavLock : NavLockUIMap, ITestStep
    {
        /// <summary>
        /// Run the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <param name="sheetIndexToName">The sheet name.</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenNavLock();
                Wait(2000);
                NavLockDelete.Click(MouseButtons.Left);
                if (testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_CONFIRM].ToString().ToUpper().Equals("TRUE"))
                {
                    BtnEdit.Click(MouseButtons.Left);
                    Console.WriteLine("NavLock Date not deleted");

                }
                else
                {
                    BtnPlace.Click(MouseButtons.Left);
                    Console.WriteLine("NavLock Date deleted");
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
                if (FrmNavLock.IsEnabled)
                {
                    KeyboardUtilities.CloseWindow(ref AboutPrana_UltraFormManager_Dock_Area_Top);
                }
            }
            return _result;
        }
    }
}
