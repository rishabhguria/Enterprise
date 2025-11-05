using Bloomberg.Library;
using Bloomberglp.Blpapi;
using Prana.BusinessObjects;
using System;

namespace Prana.SAPIAdapter
{
    //public class Requests : Dictionary<string, SymbolRequest>
    //{       
    //}
    public class SymbolRequest
    {
        public SymbolData Data;
        public SecurityInfo Info;
        public int Flags;

        public SymbolRequest(string symbol)
        {
            Info = new SecurityInfo(symbol);
            Data = Create(Info);
        }
        public SymbolRequest(string symbol, int flags)
        {
            Info = new SecurityInfo(symbol);
            Data = Create(Info);
            Flags = flags;
        }

        /// <summary>
        /// Get Symbol Data
        /// </summary>
        /// <param name="SecurityInfo"></param>
        /// <param name="md"></param>
        /// <param name="topicName"></param>
        /// <returns></returns>
        private static SymbolData Create(SecurityInfo SecurityInfo /*, MarketDataEvents md, string topicName*/)
        {
            if (SecurityInfo.Asset.Equals("Equity", StringComparison.OrdinalIgnoreCase))
            {
                EquitySymbolData data = new EquitySymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.Equity;

                return data;
            }
            else if (SecurityInfo.Asset.Equals("Indices", StringComparison.OrdinalIgnoreCase) || SecurityInfo.Asset.Equals("Index", StringComparison.OrdinalIgnoreCase))
            {
                IndexSymbolData data = new IndexSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.Indices;

                return data;
            }
            else if (SecurityInfo.Asset == "FutureOption")
            {
                OptionSymbolData data = new OptionSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.FutureOption;
                //data.OpenInterest = md.GetValue<Int32>(md.TopicName, "RT_OPEN_INTEREST", (Int32)data.OpenInterest);

                return data;
            }
            else if (SecurityInfo.Asset == "Option")
            {
                OptionSymbolData data = new OptionSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.Option;
                //data.OpenInterest = md.GetValue<Int32>(md.TopicName, "RT_OPEN_INTEREST", (Int32)data.OpenInterest);

                return data;
            }
            else if (SecurityInfo.Asset == "Future")
            {
                FutureSymbolData data = new FutureSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.Future;
                //data.OpenInterest = md.GetValue<Int32>(md.TopicName, "RT_OPEN_INTEREST", (Int32)data.OpenInterest);

                return data;
            }
            else if (SecurityInfo.Asset == "FX")
            {
                FxContractSymbolData data = new FxContractSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.FX;

                return data;
            }
            else if (SecurityInfo.Asset == "FXForward")
            {
                FxForwardContractSymbolData data = new FxForwardContractSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.FXForward;

                return data;
            }
            else if (SecurityInfo.Asset == "EquityOption")
            {
                OptionSymbolData data = new OptionSymbolData();
                data.CategoryCode = Prana.BusinessObjects.AppConstants.AssetCategory.EquityOption;

                return data;
            }
            throw new NotFoundException("Asset type not defined in UserSubscription.SymbolData()");

            //TODO: Add rest
        }
    }
}
