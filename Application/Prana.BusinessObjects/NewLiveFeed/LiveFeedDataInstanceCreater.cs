using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects
{
    public class LiveFeedDataInstanceCreater
    {
        //private static char _splitter = Seperators.SEPERATOR_3;
        public static SymbolData CreateDataInstance(string message)
        {
            SymbolData liveFeedData = null;
            try
            {

                string[] data = message.Split(Seperators.SEPERATOR_3);
                int i = 0;
                AssetCategory catcode = (AssetCategory)Enum.Parse(typeof(AssetCategory), data[0]);

                switch (catcode)
                {
                    case AssetCategory.Equity:
                        liveFeedData = new EquitySymbolData(data, ref i);
                        break;
                    case AssetCategory.EquityOption:
                        liveFeedData = new OptionSymbolData(data, ref i);
                        break;
                    case AssetCategory.Future:
                        liveFeedData = new FutureSymbolData(data, ref i);
                        break;
                    case AssetCategory.FutureOption:
                        //liveFeedData = new FutureOptionSymbolData(data, ref i);
                        liveFeedData = new OptionSymbolData(data, ref i);
                        break;
                    case AssetCategory.Indices:
                        liveFeedData = new IndexSymbolData(data, ref i);
                        break;
                    case AssetCategory.Forex:
                        liveFeedData = new FxSymbolData(data, ref i);
                        break;
                    case AssetCategory.FixedIncome:
                        liveFeedData = new FixedIncomeSymbolData(data, ref i);
                        break;
                    case AssetCategory.FX:
                        liveFeedData = new FxContractSymbolData(data, ref i);
                        break;
                    case AssetCategory.FXForward:
                        liveFeedData = new FxForwardContractSymbolData(data, ref i);
                        break;
                    case AssetCategory.ConvertibleBond:
                        //All properties of Convertible Bonds are similar with FixedIncome
                        liveFeedData = new FixedIncomeSymbolData(data, ref i);
                        break;
                }

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

            return liveFeedData;

        }
    }
}
