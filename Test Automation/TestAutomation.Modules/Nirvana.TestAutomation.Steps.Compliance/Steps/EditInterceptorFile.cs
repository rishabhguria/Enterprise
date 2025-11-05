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
    public class EditInterceptorFile : ComplianceEngineUIMap, ITestStep
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
               // CreateTempFileAndCopyFromOriginal(DefaultInterceptorFile, TempInterceptorFile, NewInterceptorFile);
               
                
                var FileContent = TestDataConstants.FILE_CONTENT;
                //Temporary backup of original file to symbol_bak

                CreateTempFileAndCopyFromOriginal(DefaultInterceptorFile, TempInterceptorFile, NewInterceptorFile);
                List<string> FileContent2 = new List<string>();

                foreach (DataRow dr in testData.Tables[0].Rows)
                {
                    if (!String.IsNullOrEmpty(dr[TestDataConstants.SYMBOLWITHDATA].ToString()))
                    {
                        FileContent2.Add(dr[TestDataConstants.SYMBOLWITHDATA].ToString());
                       
                    }

                }
                string ReadFile = File.ReadLines(DefaultInterceptorFile).First();

                ReadFile = ReadFile + Environment.NewLine + string.Join(Environment.NewLine, FileContent2.ToArray());
               // string RemainingFileContent = string.Join(Environment.NewLine, FileContent2.ToArray());
              //  FileContent = FileContent + Environment.NewLine + string.Join(Environment.NewLine, FileContent2.ToArray());// taking harcoded columns values and joining it with the reamaining content of the list
                FileInfo fi = new FileInfo(DefaultInterceptorFile);
                using (TextWriter txtWriter = new StreamWriter(fi.Open(FileMode.Truncate)))
                {
                    txtWriter.Write(ReadFile);
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
