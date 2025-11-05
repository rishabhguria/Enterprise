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

namespace Nirvana.TestAutomation.Steps.Simulator
{
   class OpenLiveTT : CameronSimulator,ITestStep
    {
       public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
       {
           TestResult _res = new TestResult();
           try
           {
               OpenLiveTT();
               ClearUI();
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
