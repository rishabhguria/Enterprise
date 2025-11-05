using Prana.LogManager;
using System;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    /// <summary>
    /// returns open webkit browser.
    /// </summary>
    internal class BrowserFactoryHandler
    {
        internal static WebBrowserControl GetBrowserControl()
        {
            try
            {
                return new WebBrowserControl();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }
    }
}