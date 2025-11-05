using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using Nirvana.TestAutomation.Interfaces;
using System.Data;
using Nirvana.TestAutomation.Steps.Closing.Classes;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;

namespace Nirvana.TestAutomation.Steps.Closing
{
    class Expire : ExerciseTradesTab, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                OpenClosingUI();
                ExpirationDivideSettlement.Click(MouseButtons.Left);
                ExpireMenuClick();
                ClosingCommonMethods.PerformClosingOperation(testData, sheetIndexToName, GrdCashandExpire);
                BtnSave.Click(MouseButtons.Left);
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
                MinimizeClosing();
            }
            return _result;
        }
    
        /// <summary>
        /// Physical Settlement Menu Click
        /// </summary>
        private void ExpireMenuClick()
        {
            try
            {
                GrdAccountUnexpired.Click(MouseButtons.Right);
                Expire.Click(MouseButtons.Left);
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
