using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nirvana.TestAutomation.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Nirvana.TestAutomation.TestExecutor
{
    static class ReleaseControlManager
    {
        /// <summary>
        /// The instancelock
        /// </summary>
        private static readonly object _timerlock = new object();

        /// <summary>
        /// The privous test case identifier
        /// </summary>
        private static string _previousTestCaseId = string.Empty;

        /// <summary>
        /// The timer
        /// </summary>
        private static System.Threading.Timer _timer = null;

        /// <summary>
        /// The timer interval
        /// </summary>
        private static int _timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["StuckTCCheckTimerInterval"]) * 60000;

        /// <summary>
        /// The delimiter
        /// </summary>
        private static char delimiter = '~';

        /// <summary>
        /// The is writing new case
        /// </summary>
        private static bool isWritingNewCase = false;

        /// <summary>
        /// Determines whether [is application running] [the specified state].
        /// </summary>
        /// <param name="state">The state.</param>
        private static void IsApplicationRunning(object state)
        {
            try
            {
                if (!isWritingNewCase)
                {
                    lock (_timerlock)
                    {
                        String text = DateTime.Now.ToString("yyyyMMdd-HH:mm:ss") + delimiter + "Test case running" + delimiter + ApplicationArguments.TestCaseToBeRun;
                        File.AppendAllText("StuckTestCase.txt", text + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_ONLY_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Determines whether [is application running] [the specified state].
        /// </summary>
        /// <param name="state">The state.</param>
        internal static void UpdateNewTestCaseStarted()
        {
            try
            {
                lock (_timerlock)
                {
                    if (!ApplicationArguments.TestCaseToBeRun.Equals(_previousTestCaseId))
                    {
                        isWritingNewCase = true;
                        _timer.Change(_timerInterval, _timerInterval);
                        String text = DateTime.Now.ToString("yyyyMMdd-HH:mm:ss") + delimiter + "Test case started" + delimiter + ApplicationArguments.TestCaseToBeRun;
                        File.AppendAllText("StuckTestCase.txt", text + Environment.NewLine);
                        _previousTestCaseId = ApplicationArguments.TestCaseToBeRun;
                        isWritingNewCase = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isWritingNewCase = false;
                bool rethrow = ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_ONLY_POLICY);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Initialises this instance.
        /// </summary>
        public static void Initialise()
        {
            try
            {
                _timer = new System.Threading.Timer(new TimerCallback(IsApplicationRunning), null, _timerInterval, _timerInterval);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ExceptionHandlingConstants.LOG_ONLY_POLICY);
            }
        }
    }
}
