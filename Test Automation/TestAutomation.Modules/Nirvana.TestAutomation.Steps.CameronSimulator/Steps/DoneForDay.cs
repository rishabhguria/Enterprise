using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Simulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nirvana.TestAutomation.Utilities;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities.Constants;


namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class DoneForDay : CameronSimulator, ITestStep
    {
         public BussinessObjects.TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
      {
          TestResult _res = new TestResult();
          try
          {
              AccessBridgeHelper.Inititalize();

              ExtentionMethods.VerifyAndControlSimulatorAction(ApplicationArguments.JarPath, ApplicationArguments.ScriptFilePath, TestDataConstants.SetResponseTo, TestDataConstants.DoneForDay);

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
