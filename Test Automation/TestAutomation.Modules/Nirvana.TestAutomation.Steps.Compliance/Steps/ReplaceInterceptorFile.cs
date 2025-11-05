using Nirvana.TestAutomation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Utilities;
using TestAutomationFX.Core;
using TestAutomationFX.UI;
using Nirvana.TestAutomation.Utilities.Constants;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces.Enums;
using System.IO;
using System.Configuration;
using System.Globalization;


namespace Nirvana.TestAutomation.Steps.Compliance
{
    public class ReplaceInterceptorFile : ComplianceEngineUIMap, ITestStep
    {
        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                string DefaultInterceptorFile = ConfigurationManager.AppSettings["DefaultInterceptorFile"];
                string NewInterceptorFile = ConfigurationManager.AppSettings["NewInterceptorFile"];
                 string TempInterceptorFile = ConfigurationManager.AppSettings["TempInterceptorFile"];
                //Temporary backup of original file to symbol_bak
                CreateTempFileAndCopyFromOriginal(DefaultInterceptorFile, TempInterceptorFile, NewInterceptorFile);

                // if both file path contains files only then newinterceptor file values in esper possible
                if (File.Exists(DefaultInterceptorFile) && File.Exists(NewInterceptorFile))
                {
                    File.Copy(NewInterceptorFile,DefaultInterceptorFile,true);
                }

                // if newinterceptor file is empty  or path doesn't exist 
                if (!File.Exists(NewInterceptorFile) || String.IsNullOrEmpty(NewInterceptorFile))
                {
                    Console.WriteLine("newinterceptor file is empty  or File path doesn't exist");
                    throw new ApplicationException("new interceptor file is empty  or File path doesn't exist");

                }
                

                return _result;
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;
                return _result;
            }
        }
    }
}
