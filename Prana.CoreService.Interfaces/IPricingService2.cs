using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.LogManager;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.ServiceModel;

namespace Prana.CoreService.Interfaces
{
    [ServiceContract(CallbackContract = typeof(IServiceStatusCallback))]
    public interface IPricingService2 : IServiceStatus, IServiceOnDemandStatus, IContainerService, IPranaServiceCommon
    {
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SetRiskLoggingStatus(bool isRiskLoggingEnabled);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<bool> GetRiskLoggingStatus();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RiskAPIEachCalDurationThreasholdToLogUpdateTime(int timeValue);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<int> GetRiskAPIEachCalDurationThreasholdToLogUpdateTime();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<EnumerationValue>> GetPricingModels();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<WinDaleParams> GetWinDaleParams();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SaveWinDaleParams(WinDaleParams winDaleParams);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<MarketDataProvider?> GetFeedProvider();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<SecondaryMarketDataProvider?> GetSecondaryFeedProvider();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task UpdateLiveFeedDetails(string username, string password, string host, [Optional, DefaultParameterValue("")] string port,
            [Optional, DefaultParameterValue("")] string supportUsername, [Optional, DefaultParameterValue("")] string supportPassword);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SecondaryUpdateLiveFeedDetails(string username, string password);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RestartLiveFeed();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> DataManagerSetup();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> SecondaryDataManagerSetup();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task DataManagerClose();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RequestSymbol(string requestedSymbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task GetServices();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task InitializeLiveFeedViewerData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<SymbolData>> GetLiveFeedDataList();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task StopLiveFeedViewerData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task DeleteAdvisedSymbol(string symbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<string>> GetAdvicedSymbols();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, string>> GetTickersLastStatusCode();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, int>> GetSubscriptionInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, string>> GetUserInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<List<Dictionary<string, string>>> GetUserPermissionsInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<object> GetLiveDataDirectlyFromFeed();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetUpdatedLiveFeedDataFromLiveCache();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SetDebugEnableDisable(bool isDebugEnable, double pctTolerance);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Tuple<bool, double>> GetDebugEnableDisableParams();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<FactSetContractType?> GetFactsetContractType();

        #region MarketDataAdapter
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, MarketDataSymbolResponse>> GetAllMarketDataSymbolInformation();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task RefreshMarketDataSymbolInformation(string tickerSymbol);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<Dictionary<string, SymbolData>> GetSnapshotsSymbolData();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<DataSet> GetSAPIRequestFieldData(string requestField);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task SaveSAPIRequestFieldData(DataSet saveDataSetTemp, string requestField);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        System.Threading.Tasks.Task<DataTable> GetSubscribedSymbolsMonitoringData();

        #endregion
    }
}