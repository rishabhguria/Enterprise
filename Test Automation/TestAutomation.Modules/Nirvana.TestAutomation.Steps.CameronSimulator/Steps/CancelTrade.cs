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

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class CancelTrade : CameronSimulator,ITestStep
    {
        TestResult _res = new TestResult();
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            try
            {
                AccessBridgeHelper.SendMessage(CameronConstants.gridCommand, GetTradeIndex(testData.Tables[0]));
                Wait(500); 
                AccessBridgeHelper.SendMessage(CameronConstants.buttonCommand, CameronConstants.canButton);
                _res.IsPassed = true;
            }
            catch (Exception ex)
            {
                _res.ErrorMessage = ex.Message;
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
        }
    }
}
