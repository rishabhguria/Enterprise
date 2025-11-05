using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Factory;
using CommandType = Nirvana.TestAutomation.Interfaces.Enums.CommandType;

namespace Nirvana.TestAutomation.Steps.Allocation
{
    class CleanUp : AllocationUIMap, ITestStep
    {
        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="testData">The test data.</param>
        /// <returns></returns>
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _res = new TestResult();
            try
            {
                //SqlUtilities.ExecuteQuery("delete from Pm_taxlots" + Environment.NewLine + "delete from t_journal" + Environment.NewLine + "delete from t_allactivity" + Environment.NewLine +
                   // "delete from t_level2allocation" + Environment.NewLine + "delete from t_tradedorders" + Environment.NewLine + "delete from t_journal" + Environment.NewLine + "delete from t_fundallocation");
                //ICommandUtilities cmdUtilities = null;
                //cmdUtilities = ExecuteCommandTypeFactory.SetExecutionCommandType(CommandType.Bat);
                //cmdUtilities.ExecuteCommand("CleanUp.Bat");
            }
            catch (Exception ex)
            {
                _res.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
            }
            return _res;
         }  
    }
}
