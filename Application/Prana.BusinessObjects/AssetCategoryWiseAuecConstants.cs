using Prana.LogManager;
using System;
using System.Configuration;

namespace Prana.BusinessObjects
{
    public class AssetCategoryWiseAuecConstants
    {
        public static readonly int DefaultEquityAUECID;
        public static readonly int DefaultOptionAUECID;
        public static readonly int DefaultFutureOptionAUECID;
        public static readonly int DefaultFutureAUECID;
        public static readonly int DefaultForwardAUECID;
        public static readonly int DefaultIndicesAUECID;
        public static readonly int DefaultFxAUECID;
        public static readonly int DefaultFixedIncomeAUECID;
        static AssetCategoryWiseAuecConstants()
        {
            try
            {
                DefaultEquityAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultEquityAUECID"]);
                DefaultOptionAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultOptionAUECID"]);
                DefaultFutureOptionAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFutureOptionAUECID"]);
                DefaultFutureAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFutureAUECID"]);
                DefaultForwardAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultForwardAUECID"]);
                DefaultIndicesAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultIndicesAUECID"]);
                DefaultFxAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFxAUECID"]);
                DefaultFixedIncomeAUECID = int.Parse(ConfigurationManager.AppSettings["DefaultFixedIncomeAUECID"]);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

    }
}
