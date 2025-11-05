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
    class VerifyLayoutUnallocatedGrid : AllocationUIMap, ITestStep
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
                GridUnallocated.Click();
                if (VerifyLayout(testData) == false)
                    _res.IsPassed = false;
                GridUnallocated.Click();
              //  Wait(1000);
            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyLayoutUnallocatedGrid");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                Close1.WaitForObject();
                if (Close1.IsValid == true)
                    Close1.Click(MouseButtons.Left);
                MinimizeAllocation();
            }
            return _res;
        }
        /// <summary>
        /// a method to verify the unallocated grid Layout
        /// </summary>
        private bool VerifyLayout(DataSet testData)
        {
            try
            {
                bool result = true; 
                FieldChooserButton.Click(MouseButtons.Left);
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    Keyboard.SendKeys("[CTRL+A]");
                    Clipboard.SetText(dr["Column Name"].ToString());
                    Keyboard.SendKeys("[CTRL+V]");
                    if (CheckBox1.IsChecked==false)
                    {
                        result = false;
                        return result;
                    }
                    FieldChooserUnAllocatedTextBox.Click();
                }
                return result;
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
