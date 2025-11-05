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
using TestAutomationFX.Core;

namespace Nirvana.TestAutomation.Steps.Blotter
{
    public class RemoveTab : BlotterUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                OpenBlotter();
                if (testData != null)
                {
                    foreach (DataRow dr in testData.Tables[0].Rows)
                    {
                        WorkingSubsTab.MsaaName = dr[TestDataConstants.REMOVE_TAB_NAME].ToString();
                        WorkingSubsTab.WaitForVisible();
                        if (WorkingSubsTab.IsValid)
                        {
                            WorkingSubsTab.Click();
                            CtrlBlotterMain00.Click(MouseButtons.Right);
                            RemoveTab.Click(MouseButtons.Left);
                            ButtonYes.WaitForVisible();
                            if(ButtonYes.IsValid)
                            	ButtonYes.Click(MouseButtons.Left);
                        }
                     }
                }
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
                MinimizeBlotter();
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
