using Prana.BusinessObjects;
using Prana.LogManager;
using System;

namespace Prana.CommonDataCache
{
    public static class AUECRoundingRulesHelper
    {
        public static void ApplyRounding(PranaBasicMessage group)
        {
            try
            {
                int roundDigits = CachedDataManager.GetInstance.GetRoundDigitsFromAUECID(group.AUECID);

                if (roundDigits != int.MinValue)
                {
                    group.AvgPrice = Math.Round(group.AvgPrice, roundDigits);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }

}

