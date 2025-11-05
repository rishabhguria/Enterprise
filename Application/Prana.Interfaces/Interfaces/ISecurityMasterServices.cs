using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Interfaces
{
    public interface ISecurityMasterServices
    {
        void SendRequest(SecMasterRequestObj secMasterReqObj);
        void SendMessage(QueueMessage message);
        event EventHandler Disconnected;
        event EventHandler Connected;
        event EventHandler<EventArgs<SecMasterBaseObj>> SecMstrDataResponse;
        event EventHandler<EventArgs<DataSet>> SymbolLookUpDataResponse;
        event EventHandler<EventArgs<UDASymbolDataCollection>> udaUISymbolDataResponse;
        event EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>> UDAAttributesResponse;
        event EventHandler<EventArgs<List<SecMasterBaseObj>>> EventTradedSMDataUIRes;
        event EventHandler<EventArgs<int, int, int>> SecMstrBulkResponse;

        //on in Used UDA data recieved
        event EventHandler<EventArgs<Dictionary<string, Dictionary<int, string>>>> EventInUsedUDARes;
        event EventHandler<EventArgs<DataSet>> AUECMappingGetDataResponse;

        //on future data recieved
        event EventHandler<EventArgs<QueueMessage>> FutSymbolRootDataRes;
        event EventHandler<EventArgs<DataSet>> FutureRootSymbolDataResponse;
        event EventHandler<EventArgs<SecMasterGlobalPreferences>> SMGlobalPrefencesResponse;
        event EventHandler<EventArgs<DataSet>> AccountWiseUDADataResponse;

        event EventHandler<EventArgs<string>> CentralSMConnected;
        event EventHandler<EventArgs<string>> CentralSMDisconnected;

        /// <summary>
        /// future root data response
        /// </summary>
        event EventHandler<EventArgs<QueueMessage>> FutRootDataSaveRes;

        bool IsConnected { get; set; }
        bool IsCSMConnected { get; set; }
        void ConnectToServer();
        System.Threading.Tasks.Task ConnectToServerAsync();
        void DisConnect();
        List<SecMasterBaseObj> SendRequestList(SecMasterRequestObj secMasterReqObj);
        void SaveNewSymbols(SecMasterbaseList secMasterData);
        void SaveShareOutstanding(string symbol, double sharesOutstanding);
        void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj);
        void GetSymbolsUDAData(UDADataReqObj udaDataReqObj);

        event EventHandler<EventArgs<QueueMessage>> ResponseCompleted;
        ICommunicationManager TradeCommunicationManager { get; set; }
        void SaveNewSymbols_Import(SecMasterbaseList secMasterData);
        void UpdateSymbols_Import(SecMasterUpdateDataByImportList secMasterData);
        void EnRichSecMasterObj(DataTable dt);
        List<SecMasterBaseObj> GetSMCachedData(SecMasterRequestObj secMasterReqObj);
        void Retry(bool shouldRetry);
        void SaveUDAData(Dictionary<String, Dictionary<string, object>> UDADataCol);

        //request for Data from Security master services like UDA Attributes, invalide symbol list, stuck trades etc   
        void DataReqByKeyFrmServer(String DataReqKey, string RequestID);
        void GetAllUDAAtrributes();
        void GetInUsedUDAIds();

        /// <summary>
        /// Used to get MarkPrice info from feed like Bloomberg, Use requested date inside the secmasterRequest object if you need to get data for a specific date.
        /// </summary>
        /// <param name="fields">list of fields to request of type PricingDataType.ToString() or strings which have mapping for bloomberg fields</param>
        /// <param name="secMasterReqObj">Request object filled with all the symbolDataRows to be requested</param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="requestUUID">A Guid so that you can identify which UI on client has requested the data</param>
        /// <param name="clientFunction">The function to call in response the QueueMessage has following data Datatable, pricingSuccess, comment</param>
        void GetMarkPricesForSymbolAndDate(String secondaryPricingSource, List<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Guid requestUUID, Action<QueueMessage, Guid> clientFunction, bool isGetDataFromCacheOrDB);

        void RefreshServerCache();
        void RequestFieldsDictionary();
        void GetAUECMappings();
        void SaveAUECMappings(DataSet saveDataSetTemp);
        void searchSymbols(SecMasterSymbolSearchReq req);

        event EventHandler<EventArgs<SecMasterSymbolSearchRes>> SecMstrDataSymbolSearcResponse;
    }

    public delegate void UDASymbolDataResponse_ReqObj(UDASymbolDataCollection UDASymbolDataCol, UDADataReqObj udaDataReqObj);
}
