using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace Nirvana.TestAutomation.Steps.Expnl
{
    public partial class RefreshExpnlUi : ExPNLUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                PranaExpnlServiceUIApplication.Start();
                if (PranaExpnlServiceUIApplication.IsVisible)
                {   
                    Wait(5000);
                    PranaExpnlServiceUIApplication.BringToFront();
                    int maxRetry = 3;
                    int currentRetry = 0;
                    do
                    {
                        UltraButtonRefreshData.Click(MouseButtons.Left);
                        currentRetry++;
                        Wait(2000);
                    } while (currentRetry < maxRetry);
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
                try
                {
                    Process[] processes = Process.GetProcessesByName("Prana.ExpnlServiceUI");
                    foreach (Process process in processes)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error killing the 'Prana.ExpnlServiceUI' process: " + ex.Message);
                    throw;
                }
            }
            return _result;
        }
    }
}
