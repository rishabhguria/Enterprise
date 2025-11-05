using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.BussinessObjects;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using System.IO;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Steps.Rebalancer;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class ActionOnRSAImportGrid : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName) {
            TestResult _result = new TestResult();
            try
            {
                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["ContinueImport"].ToString())) {
                    Continue.Click(MouseButtons.Left);
                    if (RASImportWindow.IsVisible)
                    {
                        KeyboardUtilities.CloseWindow(ref ImportStatus1);
                    }
                    if (NirvanaAlert5.IsVisible)
                    {
                        ButtonYes3.Click(MouseButtons.Left);
                    }
                }

                if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["AbortImport"].ToString()))
                {
                    Abort.Click(MouseButtons.Left);
                    if (NirvanaAlert5.IsVisible) {
                        ButtonYes3.Click(MouseButtons.Left);
                    }
                }

                if (testData.Tables[0].Columns.Contains("SecurityMaster"))
                {
                    if (!string.IsNullOrEmpty(testData.Tables[0].Rows[0]["SecurityMaster"].ToString()))
                    {
                        SecurityMaster.Click(MouseButtons.Left);
                    }
                }

                Wait(2000);
                RebalanceAcrossSecurities2.Click(MouseButtons.Left);

            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;
        }
    }
}
