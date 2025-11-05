using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;

namespace Nirvana.TestAutomation.Steps.CleanUp
{
    public class DeleteClientData : ITestStep 
    {
       
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {  
                
             //DeleteClientDataUsingScript();
             
                   
                
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
        public void DeleteClientDataUsingScript()
        {
            try
            {
                    ICommandUtilities cmdUtilities = null;
                    cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                    cmdUtilities.ExecuteCommand("ShutdownRelease.Bat");
                    cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.SqlQuery);
                    cmdUtilities.ExecuteCommand("DeleteClientData.sql");
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
