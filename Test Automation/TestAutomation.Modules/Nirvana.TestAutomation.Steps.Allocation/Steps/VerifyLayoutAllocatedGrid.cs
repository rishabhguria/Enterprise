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
    class VerifyLayoutAllocatedGrid : AllocationUIMap, ITestStep
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
                GridAllocated.Click();
                if (VerifyLayout(testData)==false)
                    _res.IsPassed = false;
                GridAllocated.Click();
                Wait(1000);

            }
            catch (Exception ex)
            {
                SamsaraHelperClass.CaptureMyScreen("", ApplicationArguments.TestCaseToBeRun, "VerifyLayoutAllocatedGrid");
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            finally
            {
                Close2.WaitForObject();
                if(Close2.IsValid==true)
                Close2.Click(MouseButtons.Left);
                MinimizeAllocation();
            }
            return _res;
        }
        /// <summary>
        /// a method to select columns in field chooser
        /// </summary>
        private bool VerifyLayout(DataSet testData)
        {
            try
            {
                bool result = true; 
                FieldChooserButton1.Click(MouseButtons.Left);
                DataRow dr1 = testData.Tables[0].Rows[0];
                string level = dr1["Level"].ToString();
                FieldGroupSelector.Click(MouseButtons.Left);
                Keyboard.SendKeys(level[0].ToString());
                Keyboard.SendKeys("[ENTER]");
                FieldChooserAllocatedTextBox.Click();
                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    Keyboard.SendKeys("[CTRL+A]");
                    Clipboard.SetText(dr["Column Name"].ToString());
                    Keyboard.SendKeys("[CTRL+V]");
                    if (CheckBox.IsChecked==false)
                    {
                        result = false;
                        return result;    
                    }
                    FieldChooserAllocatedTextBox.Click();
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
