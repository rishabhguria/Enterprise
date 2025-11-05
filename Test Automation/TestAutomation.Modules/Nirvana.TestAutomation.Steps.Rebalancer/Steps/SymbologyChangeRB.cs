using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Nirvana.TestAutomation.Steps.Rebalancer;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using System.IO;

namespace Nirvana.TestAutomation.Steps.Rebalancer
{
    class SymbologyChangeRB : RebalancerUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                CopyTTGeneralPref();
                OpenGeneralPreferences();
                CmbSymbology.Properties[TestDataConstants.TEXT_PROPERTY] = "Bloomberg Symbol";
                BtnSave.Click(MouseButtons.Left);
                Wait(5000);
                BtnClose.Click(MouseButtons.Left);
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
