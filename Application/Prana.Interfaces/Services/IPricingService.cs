using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.LiveFeed;
using Prana.Global;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Prana.Interfaces
{
    [ServiceContract(CallbackContract = typeof(ILiveFeedCallback))]
    public interface IPricingService : IServiceOnDemandStatus
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void Subscribe();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UnSubscribe([Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void Initialize(IQueueProcessor outPricingQueue);

        /// <summary>
        /// It represents whether the live feed connection on the server is active or not
        /// </summary>
        bool IsLiveFeedActive
        {
            [OperationContract()]
            [FaultContract(typeof(PranaAppException))]
            get;
        }

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        SymbolData GetDynamicSymbolData(string symbol);

        [OperationContract(Name = "GetDynamicSymbolDataOverload")]
        [FaultContract(typeof(PranaAppException))]
        SymbolData GetDynamicSymbolData(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode);

        [OperationContract(IsOneWay = true)]
        //TODO : Third parameter is used to pass ILiveFeedCallback which is not required. but since the call is done from optionservermanager, operationcontext.current is returning null and hence not able to refer and save the ILiveFeedCallback instance.
        // Need to solve this problem later.
        // Set CompleteInfo to true if you need the complete symbol information. e.g. - When securitymaster requests
        // the data from live-feed for saving in db, then it would need the complete information and this function
        // has to be called with completeinfo = true.
        void RequestSnapshot(List<string> symbols, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, [Optional, DefaultParameterValue(true)] bool completeInfo);

        [OperationContract(IsOneWay = true)]
        void RequestSMData(List<string> symbols, ApplicationConstants.SymbologyCodes symbologyCode);

        [OperationContract(IsOneWay = true, Name = "RequestSnapshotOverload")]
        void RequestSnapshot(List<fxInfo> fxSymbols, ApplicationConstants.SymbologyCodes symbologyCode, bool isGreekRequired, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, [Optional, DefaultParameterValue(true)] bool completeInfo);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<UserOptModelInput> GetOMIDataFromCache(bool fetchZeroPositionData, string symbols = "");

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool SaveOMIData(DataSet ds, List<string> modifiedSymbolsList);

        event EventHandler<EventArgs<bool>> LiveFeedConnectionStatusChanged;

        //event EventHandler<EventArgs<string, List<OptionStaticData>>> OptionChainResponse;

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        LiveFeedPreferences GetOMILiveFeedPreferences();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveOMILiveFeedPreferences(LiveFeedPreferences Preferences);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void InitializeMarkPricesCache(string AUECString);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void AdjustMarkPriceByTodaysSplitFactor(string todayAUECString, bool isUpdated, Dictionary<int, DateTime> dictMarketEndTime);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        double GetMarkPriceForDateAndSymbol(DateTime date, string symbol);

        /// <summary>
        /// Returns the latest available mark price for given symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<Tuple<string, DateTime>, double> GetMarkPricesForOptExpiry(List<Tuple<string, DateTime>> symbolDatePairs);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, double> GetMarkPricesForSymbolAndExactDate(Dictionary<string, DateTime> dictSymbolWithSettlementDate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        double GetMarkPricesForSymbolOnExactDate(string symbol, DateTime date);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDate(DateTime date, int dateMethodology, bool isFxFxForwardData);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool GetSameDayClosedDataConfigValue();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveMarkPrices(DataTable dt, bool isAutoApproved);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveBeta(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveOutStandings(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SubscribeStaticOptionChain(string underlyingSymbol, OptionChainFilter optionChainFilter);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task UnsubscribeStaticOptionChain(string underlyingSymbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveForexRate(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, bool> CheckIfInternationalSymbols(List<string> symbols);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveRunUploadFileDataForOMI(List<UserOptModelInput> omiValueCollection);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<UserOptModelInput> GetDataFromOMI(List<string> symbols);

        //added by: Bharat Raturi
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<DateTime, Dictionary<int, Dictionary<string, MarkPriceInfo>>> GetMarkPriceForDateRangeWithAccounts(string xmlAccount, DateTime startDate, DateTime endDate, int dateMethodology, bool isFxFxForwardData, int filter);

        // It ask for the yesterday mark price for any account + symbol. The yesterday date is the same as provided by expnl using clearance logic
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetMarkPriceForAccountSymbolCollection(ref Dictionary<int, Dictionary<string, double>> dictAccountSymbolCollection);

        //added by: Bharat Raturi
        //purpose: get the unapproved mark prices from the system
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataTable GetUnapprovedMarkPrices(DateTime startDate, DateTime endDate);

        //added by: Bharat Raturi
        //purpose: approve the unapproved mark prices in the db
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int ApproveMarkPrices(DataTable dtMarkPrice);

        //added by: Bharat Raturi
        //purpose: approve the unapproved mark prices in the db
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int RescindMarkPrices(DataTable dtMarkPrice);

        // purpose : To save forex currency pair in db
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveStandardCurrencyPair(DataTable dtCurrencyPair);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveVolatility(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveVWAP(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveDividendYield(DataTable dt);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<string> GetAdvicedSymbols();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void CheckAndAdviseSymbol(string symbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void CheckAndAdviseSymbolBulk(List<string> symbols);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void CheckAndAdviseSymbolForFX(string symbol, int fromCurrency, int toCurrency, AssetCategory categoryCode);

        [OperationContract]
        void DeleteAdvisedSymbol(string symbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void DeleteSymbolFromPI(string symbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SavePerformanceNumberValues(DataTable dt);

        [OperationContract(IsOneWay = true)]
        void RequestSymbol_TTandPTT(string symbol, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, bool isSnapshot);

        [OperationContract(IsOneWay = true)]
        void RequestSnapshotForCompliance(List<string> requestedSymbols);

        [OperationContract(IsOneWay = true)]
        void RemoveSnapshotForCompliance(List<string> symbolsToBeRemoved);

        [OperationContract(IsOneWay = true)]
        void RequestMultipleSymbols(List<string> symbolSet, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass, bool isSnapshot);

        [OperationContract(IsOneWay = true)]
        void RemoveSymbol_TTandPTT(string symbol, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass);

        [OperationContract(IsOneWay = true)]
        void RemoveMultipleSymbols(List<string> symbolSet, [Optional, DefaultParameterValue(null)] ILiveFeedCallback callBackClass);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, SymbolData> GetLiveFeedForSymbolList(Dictionary<string, AdviceSymbolInfo> symbols);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, SymbolData> GetLiveFeedForSymbolListTouch(Dictionary<string, AdviceSymbolInfo> symbols);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveCollateralValues(DataTable dtCollateralPriceTemp);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataTable GetCollateralPriceDateWise(DateTime dateSelected, int type);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, string> GetTickersLastStatusCode();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, int> GetSubscriptionInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        DataSet GetSAPIRequestFieldData(string requestField);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, string> GetUserInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Dictionary<string, string>> GetUserPermissionsInformation();

        void RestartLiveFeed();
    }
}