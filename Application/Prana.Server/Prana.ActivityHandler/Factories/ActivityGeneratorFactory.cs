using Prana.ActivityHandler.BusinessObjects;
using Prana.ActivityHandler.Generators;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.ActivityHandler.Factories
{
    internal static class ActivityGeneratorFactory
    {
        internal static IActivityGenerator GetActivityGenerator(AssetCategory assetCategory)
        {
            try
            {
                switch (assetCategory)
                {
                    case AssetCategory.PrivateEquity:
                        return new PrivateEquityActivityGenerator();

                    case AssetCategory.Equity:
                        return new EquityActivityGenerator();

                    case AssetCategory.CreditDefaultSwap:
                        return new SwapActivityGenerator();

                    case AssetCategory.EquityOption:
                        return new EquityOptionActivityGenerator();

                    case AssetCategory.Future:
                        return new FutureActivityGenerator();

                    case AssetCategory.FutureOption:
                        return new FutureOptionActivityGenerator();

                    case AssetCategory.FX:
                        return new FXActivityGenerator();

                    case AssetCategory.FXForward:
                        return new FXForwardActivityGenerator();

                    case AssetCategory.FXOption:
                        return new FXOptionActivityGenerator();

                    case AssetCategory.FixedIncome:
                        return new FixedIncomeActivityGenerator();

                    case AssetCategory.ConvertibleBond:
                        return new ConvertibleBondActivityGenerator();

                    case AssetCategory.None:
                        return new CashTransactionActivityGenerator();

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
                return null;
            }
        }
    }
}
