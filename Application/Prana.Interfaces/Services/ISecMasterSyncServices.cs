using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System.Collections.Generic;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface ISecMasterSyncServices : IServiceOnDemandStatus
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, string> GetSecMasterData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode);

        /// <summary>
        /// GetDynamicUDAList() method to get the dynamic UDA
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        SerializableDictionary<string, DynamicUDA> GetDynamicUDAList();

        /// <summary>
        /// SaveDynamicUDA() method to save the dynamic UDA
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool SaveDynamicUDA(DynamicUDA dynamicUDA, string renamedKeys);

        /// <summary>
        /// GetSecMasterSymbolData() method to get the symbol data
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, SecMasterBaseObj> GetSecMasterSymbolData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode);

        /// <summary>
        /// Get Tickers By Symbol & Currency (eg If multiple symbol have same Cusip/ISIN but different currency)
        /// </summary>
        /// <param name="symbolCurrencyList">Dictionary of symbol and currencies</param>
        /// <param name="symbologyCodes"></param>
        /// <returns>Return Dictionary with Key = ReqestedSymbol^Currency, value  = tickerSymbol</returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, string> GetTickersBySymbolCurrency(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCodes);

        /// <summary>
        /// CheckMasterValueAssigned() method to check master value is used
        /// </summary>
        /// <param name="symbolList"></param>
        /// <param name="symbologyCode"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool CheckMasterValueAssigned(string tag, string value);

        /// <summary>
        /// Gets the currency identifier for symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int GetCurrencyIdForSymbol(string symbol);

        /// <summary>
        /// Gets the symbol list for the given symbol.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        SecMasterSymbolSearchRes SearchSymbols(SecMasterSymbolSearchReq request);

        /// <summary>
        /// Gets the MarketData Symbol for the given Ticker symbol.
        /// </summary>
        /// <param name="ticker">The Ticker Symbol.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        MarketDataSymbolResponse GetMarketDataSymbolFromTickerSymbol(string tickerSymbol);

        /// <summary>
        /// Gets the Ticker Symbol for the given MarketData symbol.
        /// </summary>
        /// <param name="ticker">The MarketData Symbol.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        MarketDataSymbolResponse GetTickerSymbolFromMarketData(SymbolData marketData);

        /// <summary>
        /// Gets the Asset category.
        /// </summary>
        /// <param name="Symbol">The Ticker Symbol.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        AssetCategory GetAssetCategoryForBloombergSymbology(string Symbol);
    }
}
