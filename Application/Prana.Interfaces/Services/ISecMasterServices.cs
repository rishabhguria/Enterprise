using Prana.BusinessObjects;
using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.Compliance.DataSendingObjects;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.BusinessObjects.SMObjects;
using Prana.Global;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace Prana.Interfaces
{
    public interface ISecMasterServices
    {
        string ConnectionString
        {
            set;
            get;
        }

        void Subscribe(int senderHashCode);
        List<SecMasterBaseObj> GetAllSecMasterData();
        List<SecMasterBaseObj> GetSecMasterDataForListSync(SecMasterRequestObj secMasterRequestObj, int senderCode);
        List<SecMasterBaseObj> GetSecMasterDataForListSync(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode, int senderCode);
        void GetSecMasterData(List<string> symbolList, ApplicationConstants.SymbologyCodes symbologyCode, int senderCode);
        SecMasterBaseObj GetSecMasterDataForSymbol(string Symbol);
        void GetSecMasterData(SecMasterRequestObj secMasterRequestObj, int senderCode);
        void SaveNewSymbolToSecurityMaster(SecMasterbaseList secMasterData);
        void GetSymbolLookupRequestedData(SymbolLookupRequestObject symbolLookupReqObj);
        /// <summary>
        /// Saves the share out standing in sec master.
        /// </summary>
        /// <param name="shareOutStandingSymbolWise">The share out standing symbol wise.</param>
        /// <param name="userId">The user identifier.</param>
        void SaveShareOutStandingInSecMaster(string symbol, double sharesOutStanding, string userId);
        DataSet GetFutureRootData();
        FutureRootData GetFutureRootData(String symbol);
        SecMasterGlobalPreferences GetSMPreferences();
        void SaveSMPreferences(SecMasterGlobalPreferences preferences);
        void SaveFutureRootData(DataTable dt, string userID, string requestID);
        void SaveUDAAttributesData(String message);
        void SaveAUECMappings(DataSet saveDataSetTemp);
        DataSet GetAUECMappings();
        event EventHandler<EventArgs<SecMasterBaseObj>> SecMstrDataResponse;
        event EventHandler<EventArgs<DataSet, SymbolLookupRequestObject>> SymbolLkUpDataResponse;
        event EventHandler<EventArgs<string, string, string>> StatusOfRequest;
        event EventHandler<EventArgs<string, object, string, string>> EventSendDataByResKey;
        void SetSecuritymasterDetails(PranaMessage pranaMsg);
        void SetSecuritymasterDetails(PranaBasicMessage taxLotDetail);
        void SetSecurityUDADetails(TaxLot taxlot);
        string GetOldCompanyNameForNameChange(Guid CAId);
        void SaveNewSymbolToSecurityMaster_Import(SecMasterbaseList secMasterData);
        void UpdateSymbolToSecurityMaster_Import(SecMasterUpdateDataByImportList secMasterData);

        #region Compliance section
        //Corrected events after commit from Ishan Gandhi revision no:21320 [Microsoft Managed Rule]
        /// <summary>
        /// Event which is raised when any SecurityObject is Received
        /// </summary>
        event EventHandler<EventArgs<SecMasterBaseObj>> SecurityObjectReceived;

        /// <summary>
        /// Event which is raised when any AuecDetails is Updated
        /// </summary>
        event EventHandler<EventArgs<AuecDetails>> AuecDetailsUpdated;

        /// <summary>
        /// Event which is raised when any UDAData is Received
        /// </summary>
        //event UDADataReceivedHandler UDADataReceived;

        /// <summary>
        /// event raised when future root data is saved
        /// </summary>
        event EventHandler<EventArgs<string, string, string>> FutureRootDataSavedResponse;
        #endregion

        void CentralSMDisconnected();
        Dictionary<string, Dictionary<int, string>> GetUDAAttributes();
        void GetAllHistOrOpenTradedSymbols(string UserID, string RequestID, Boolean isOpenSymbolRequest);
        void GetSMUIAdvncdSearchData(string SearchQuery, String userId, String requestID);

        /// <summary>
        /// Genric function to request pricing related data from historical api like bloomberg
        /// </summary>
        /// <param name="fields">should be either a string version of enum PricingDataType or the mapping for the string should be there with bloomberg field</param>
        /// <param name="secMasterReqObj"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        void GetGenericSMPrice(string requestID, String pricingSource, ConcurrentBag<string> fields, SecMasterRequestObj secMasterReqObj, DateTime startDate, DateTime endDate, Action<string, DataTable, bool, string> functionForResponse, bool isGetDataFromCacheOrDB);

        /// <summary>
        /// Save security on CSM if CH release otherwise save in local DB only
        /// </summary>
        /// <param name="reqObj"></param>
        void SaveSecurityInSecurityMaster(SecMasterbaseList reqObj);
        ConcurrentDictionary<string, StructPricingField> GetPricingFields();
        DataSet GetAccountSymbolUDAData();
        void SaveAccountWiseUDAData(DataSet accountSymbolUDAData);
        SecMasterSymbolSearchRes ReqSymbolSearch(SecMasterSymbolSearchReq searchReq);
        void SetHashCode();
        SerializableDictionary<string, DynamicUDA> GetDynamicUDAList();
        bool SaveDynamicUDA(DynamicUDA dynamicUda, string renamedKeys);
        bool CheckMasterValueAssigned(string tag, string value);
    }
    public delegate void SymbolUDADataResponse(DataSet ds);
}
