using Nirvana.TestAutomation.BussinessObjects;
using Nirvana.TestAutomation.Interfaces;
using Nirvana.TestAutomation.Steps.Simulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using Nirvana.TestAutomation.Utilities.Constants;

namespace Nirvana.TestAutomation.Steps.Simulator
{
    public class ClearAllFixLogs : CameronSimulator, ITestStep
    {
        private const int MAX_RETRY = 3;
        private const int RETRY_DELAY_MS = 1000;

        public TestResult RunTest(DataSet testData, Dictionary<int, string> sheetIndexToName)
        {
            TestResult _result = new TestResult();

            try
            {
                string logPath = System.Configuration.ConfigurationManager.AppSettings["AllFixLog"];

                if (string.IsNullOrEmpty(logPath))
                {
                    throw new Exception("Log path is not configured in AppSettings with key 'AllFixLog'.");
                }

                bool cleared = TryClearFileWithRetries(logPath, MAX_RETRY, RETRY_DELAY_MS);

                if (!cleared)
                {
                    throw new Exception("Failed to clear log file after " + MAX_RETRY + " attempts: " + logPath);
                }

                return _result;
            }
            catch (Exception ex)
            {
                _result.IsPassed = false;
                _result.ErrorMessage = "Exception occurred: " + ex.Message;

                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_AND_CAPTURE_POLICY);
                if (rethrow)
                    throw;

                return _result;
            }
        }

        private bool TryClearFileWithRetries(string filePath, int maxRetries, int delayMilliseconds)
        {
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Truncate, FileAccess.Write, FileShare.ReadWrite))
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        writer.Write(string.Empty);
                    }
                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(delayMilliseconds);
                }
            }
            return false;
        }
    }
}
