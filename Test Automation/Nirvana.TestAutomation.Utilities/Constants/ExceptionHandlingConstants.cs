using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nirvana.TestAutomation.Utilities
{
    public static class ExceptionHandlingConstants
    {
        /// <summary>
        /// The log and capture policy
        /// </summary>
        public const string LOG_AND_CAPTURE_POLICY = "LogAndCapturePolicy";
        /// <summary>
        /// The log and throw policy
        /// </summary>
        public const string LOG_AND_THROW_POLICY = "LogAndThrowPolicy";
        /// <summary>
        /// The log only policy
        /// </summary>
        public const string LOG_ONLY_POLICY = "LogOnlyPolicy";
        /// <summary>
        /// The capture and throw policy
        /// </summary>
        public const string CAPTURE_AND_THROW_POLICY = "CaptureAndThrowPolicy";
        /// <summary>
        /// The capture only policy
        /// </summary>
        public const string CAPTURE_ONLY_POLICY = "CaptureOnlyPolicy";
    }
}
