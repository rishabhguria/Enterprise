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
using System.Windows.Forms;
using TestAutomationFX.Core;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Configuration;
using Nirvana.TestAutomation.Factory;
using Nirvana.TestAutomation.Interfaces.Enums;

namespace Nirvana.TestAutomation.Steps.PranaClient
{
    public partial class MoveFile : PranaClientUIMap, ITestStep
    {
        /// <summary>
        /// Updating config files
        /// </summary>
        /// <param name="testData">gets input of key and value pair to update</param>
        /// <param name="sheetIndexToName">sheet name to use from excel for this step</param>
        /// <returns></returns>
        public TestResult RunTest(System.Data.DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();
            try
            {
                string sourceFilePath = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_SourcePath].ToString();
                string destinationFilePath = testData.Tables[sheetIndexToName[0]].Rows[0][TestDataConstants.COL_DestPath].ToString();
                string batchFilePath = ConfigurationManager.AppSettings["MoveFileBatchPath"];
                if (sourceFilePath.Contains("TestAutomation"))
                {
                    //Source and Final Path will be handled for every release
                    string start = @"\DistributedAutomation\";
                    string end = @"\Release\";

                    int startIndex = sourceFilePath.IndexOf(start);
                    int endIndex = sourceFilePath.IndexOf(end);

                    startIndex += start.Length;
                    string result = sourceFilePath.Substring(startIndex, endIndex - startIndex);


                    string replaceable = ConfigurationManager.AppSettings["-runDescription"].Split(' ')[0];
                    sourceFilePath = sourceFilePath.Replace(result, "TestAutomation" + replaceable);
                    destinationFilePath = destinationFilePath.Replace(result, "TestAutomation" + replaceable);
                }
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = batchFilePath;
                startInfo.Arguments = "\"" + sourceFilePath + "\" \"" + destinationFilePath + "\"";
                startInfo.UseShellExecute = false;

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_THROW_POLICY);
                if (rethrow)
                    throw;
            }
            return _result;

        }
    }
}
