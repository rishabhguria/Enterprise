using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nirvana.TestAutomation.Steps;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.IO;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;
using System.Runtime.InteropServices;
using System.Threading;

namespace Nirvana.TestAutomation.Steps.Compliance
{
    class CheckCompliance : PopUpUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
               // Thread.Sleep(3000);
                string allowTrade= string.Empty;
                if (testData.Tables[sheetIndexToName[0]].Rows.Count > 0)
                {
                    BlockedAlertsViewer.WaitForVisible();
                    if (!String.IsNullOrWhiteSpace(testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ALLOW_TRADE].ToString()))
                        allowTrade = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_ALLOW_TRADE].ToString();


                        if (allowTrade.Equals("Yes", StringComparison.CurrentCultureIgnoreCase) && BtnYes.IsVisible)
                        {
                            BtnYes.Click();
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                        else if (allowTrade.Equals("No", StringComparison.CurrentCultureIgnoreCase) && BtnNo.IsVisible)
                        {
                            BtnNo.Click();
                            Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                        }
                        if (SpBase.IsVisible)
                        {
                            UbtnDismissAll.Click(MouseButtons.Left);
                        }


                }
                Wait(TestDataConstants.COMPLIANCE_PROCESS_WAIT);
                return _result;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
       
    }
}

