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
    public class Allocate : AllocationUIMap, ITestStep
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
                Keyboard.SendKeys(KeyboardConstants.ALT_SPACE_X);
                AllocateUnallocatePinTab.Click(MouseButtons.Left);
                btnAllocate.DoubleClick(MouseButtons.Left);
                if (NirvanaAllocation.IsVisible)
                {
                    ButtonOK.Click(MouseButtons.Left);
                }
                if (SavewDivideStatus.Bounds.X >= 0 && SavewDivideStatus.Bounds.Y >= 0)
                {
                    SavewDivideStatus.Click(MouseButtons.Left);
                    SavewDivideStatus.Click(MouseButtons.Left);
                }
                if (SavewDivideoStatus.Bounds.X >= 0 && SavewDivideoStatus.Bounds.Y >= 0)
                {
                    SavewDivideoStatus.Click(MouseButtons.Left);
                    SavewDivideoStatus.Click(MouseButtons.Left);
                }
               // Wait(1000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "Allocate");
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