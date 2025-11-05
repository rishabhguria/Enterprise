using Prana.ServiceGateway.Models;

namespace Prana.ServiceGateway.Contracts
{
    /// <summary>
    /// Interface for Providing Contract for Logging Helper Classes
    /// </summary>
    public interface ILoggingHelper
    {
        /// <summary>
        /// Logs Frontend Data to Log Files
        /// </summary>
        /// <param name="loggerInfo">The Object that provides logging information.</param>
        /// <returns>void</returns>
        public void LogMessage(LoggerInfo loggerInfo);
    }
}
