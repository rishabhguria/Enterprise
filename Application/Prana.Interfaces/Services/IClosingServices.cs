using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using Prana.PostTrade.BusinessObjects;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Prana.Interfaces
{
    [ServiceContract]
    public interface IClosingServices : IServiceOnDemandStatus
    {
        //DB  Section
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<ClosingData> GetAllClosingData(DateTime FromDate, DateTime Todate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string customConditions);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData GetClosingDataForASymbol(string symbol, string CommaSeparatedAccountIds, string orderSideTagValue, string groupID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Position> GetNetPositionsForTaxlotIds(string CommaSeparatedTaxlotIds);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveCloseTradesData(ClosingData ClosedData);

        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //void RefreshClosingData();
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void RefreshCAData();


        //TODO : Need to remove it later.
        //[OperationContract]
        //void GetClosingandCAData();
        //CA and Closing Check

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        PostTradeEnums.Status CheckGroupStatus(AllocationGroup Group);


        //[OperationContract]
        //bool CheckGroupClosingStatus(AllocationGroup Group);
        //[OperationContract]
        //bool CheckGroupCorporateActionStatus(AllocationGroup Group);

        //[OperationContract]
        //bool CheckexercisedOrPhysical(AllocationGroup group);

        //[OperationContract]
        //PostTradeEnums.Status CheckTaxlotStatus(string taxlotID, DateTime date);

        //[OperationContract]
        //PostTradeEnums.Status CheckStatus(string taxlotID);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        bool CheckClosingStatus(string taxlotID, DateTime caEffectiveDate);

        // Check if corporate action is applied or Underlying of Exercised or physical settlement is already closed
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, StatusInfo> ArePositionEligibletoUnwind(Dictionary<string, DateTime> taxlotID);

        //Excercise/Expire Section
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        string ArePositionElligibleToExpire(string taxlotID, int auecID, bool IsCurrentDateClosing, DateTime CloseTradeDate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        StringBuilder ArePositionElligibleToExercise(string taxlotID, DateTime OTCPositionDate);

        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //OTCPosition CreatePositionForPhysicalSettlement(OTCPosition otcPos, AssetCategory asset, string sidetagValue, int putOrCall);
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        string GetOrderSideTagValueForPhysicalSettlement(AssetCategory asset, string sidetagValue, double underlyingPosQty, int putOrCall, DateTime settleDate, string underlyingSymbol, string accountID, bool isCurrentDayClosing, ref string transactionType);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, Dictionary<string, PhysicalSettlementDto>> GetOrderSideTagValueForPhysicalSettlementGroup(List<TaxLot> taxlots, bool isCurrentDayClosing);

        //Unwind Section
        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //void UnwindPositions(List<Position> listPositions);

        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]

        //Closing Section
        //[OperationContract]
        // ClosingData AutomaticClosing(List<TaxLot> buyTaxlots, List<TaxLot> sellTaxlots, PostTradeEnums.CloseTradeAlogrithm algorithm, bool IsShortWithBuyAndBuyToCover);    
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData AutomaticClosingBasedOnTemplates(List<ClosingTemplate> closingTemplates);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData UnwindClosingBasedOnTemplates(List<ClosingTemplate> closingTemplates);

        // commented by omshiv, 
        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //ClosingData AutomaticClosingOnManualOrPresetBasis(List<TaxLot> buyTaxLotsAndPositions, List<TaxLot> sellTaxLotsAndPositions, PostTradeEnums.CloseTradeAlogrithm algorithm, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, bool isManual, bool isDragDrop, bool isFromServer, PostTradeEnums.SecondarySortCriteria secondarySort, bool isVirtualClosingPopulate, bool isOverrideWithUserClosing, bool isMatchStrategy);

        //[OperationContract]
        //ClosingData ClosePosition(TaxLot targetTaxLot, TaxLot draggedTaxLot, bool IsShortWithBuyAndBuyToCover);
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<TaxLot> RunVirtualClosing(List<TaxLot> Taxlots, DateTime toDate, PostTradeEnums.CloseTradeAlogrithm algorithm, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose);

        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //ClosingData AutomaticClosing_Allocation(DateTime toDate);
        //PresetAlgoPrefernces
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SavePreferences(ClosingPreferences ClosingPreferences);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingPreferences GetPreferences();

        //[OperationContract]
        //void UpdatePreferences(ClosingPreferences ClosingPreferences);
        //[OperationContract]
        //[FaultContract(typeof(PranaAppException))]
        //bool SaveACBDataIntoDB();
        [OperationContract]
        void UpdateExercisedTaxlotsDictionary(string parentTaxlotID, string UnderlyingID, string parentTaxlottoCloseId);

        ClosingData FetchUpdatedTaxlotsFromDB(string TaxLotIDList, List<string> groupIDs);

        List<string> GetIDsToUnwindFromDB(Dictionary<string, DateTime> dictSymbols);

        List<string> UnWindClosingFromDatabase(string TaxlotClosingID, bool isBasedOnTemplates);

        void UnwindPositions(string TaxlotClosingIDWithClosingDate);


        // Returns both the Closing GroupID and UnderlyingGroupID(created by exercise) for every parent taxlotID  
        Dictionary<string, Dictionary<string, string>> GetExercisedUnderlyingGroupIDs(AllocationGroup ParentGroup);

        bool CheckExercisedOrPhysicalGenerated(AllocationGroup group);
        bool CheckIsExercised(AllocationGroup group);
        //[OperationContract]
        //Dictionary<string, List<string>> CheckifUnderlingClosed(List<string> TaxlotIds);
        void GetExcercisedGroupIDsFromDB(string FromAuecDatesString, string ToAuecDatesString, int userID);

        #region commented by omshiv, ACA cleanup

        //  [OperationContract]
        //  int CalculateAndSaveACAData();

        //[OperationContract]
        //void CalculateAndSaveACADataForSymbol(Dictionary<string, DateTime> dictSymbols);

        //[OperationContract]
        //List<string> GetACAAccounts(); 
        #endregion

        void UpdateTradeDates(string FromAuecDatesString, string ToAuecDatesString);
        bool CheckCorporateActionStatus(string taxlotID, DateTime dateToCheck);
        void SetTaxlotClosingStatus(TaxLot taxlot);
        void RefreshCADataonNewThread();

        List<Position> GetNetPositionsFromDB(DateTime FromDate, DateTime CloseTradeDate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string CommaSeparatedCustomConditions, PostTradeEnums.DateType closingDateType = PostTradeEnums.DateType.ProcessDate);
        // this is used to close positions on the basis of notional changes, this method is used in spin-off corporate action
        // closingbasis means normal closing or notional change closing
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData CloseDataforPairedTaxlots(List<TaxLot> buyTaxLotsAndPositions, List<TaxLot> sellTaxLotsAndPositions, bool closingbasis);

        /// <summary>
        /// This is used to close positions on the basis of notional changes, This method is used for Cost Adjustment Closing
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">buy side taxlots to close</param>
        /// <param name="sellTaxLotsAndPositions">sell side taxlots to close</param>
        /// <param name="closingbasis">Normal/Notional change closing</param>
        /// <param name="mode">closing mode</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData CloseDataforPairedTaxlotsOnMode(List<TaxLot> buyTaxLotsAndPositions, List<TaxLot> sellTaxLotsAndPositions, bool closingbasis, ClosingMode mode);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        int SaveClosedData(ClosingData closedData);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetPNLForSymbol(string accountName, string symbol, DateTime startDate, DateTime endDate, out Double accountRealizedPNL, out Double accountUnRealizedPNL, out Double SymbolRealizedPNL, out Double SymbolUnRealizedPNL);
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UpdatePreferencesFromDB();

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData VirtualUnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void UpdateClosingAlgoInfoCache(Dictionary<string, int> dictionary);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetTaxlotClosingStatusWithMaxModifiedDate(TaxLot taxlot, out ClosingStatus ClosingStatus, out DateTime AUECModifiedDate);

        //Modified by omshiv, sending closing params in object
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        ClosingData AutomaticClosingOnManualOrPresetBasis(ClosingParameters closingParams);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Task<CostAdjustmentResult> GetAllOpenTaxlots(DateTime FromDate, DateTime CloseTradeDate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string CommaSeparatedCustomConditions);

        /// <summary>
        /// Gets all open CostAdjustmentTaxlots
        /// </summary>
        /// <param name="taxlotIDList">list of Taxlot ids</param>
        /// <returns>list of CostAdjustmentTaxlots</returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<CostAdjustmentTaxlot> GetAllOpenCostAdjustmentTaxlots(List<string> taxlotIDList);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        CostAdjustmentResult AdjustCost(List<CostAdjustmentParameter> parameter, List<AllocationGroup> originalList);
        [OperationContract]
        Dictionary<string, TaxlotClosingInfo> GetClosingStatusInfo(DateTime toDate, DateTime fromDate, DateTime toTradeDate, DateTime fromTradeDate);

        /// <summary>
        /// Get list of taxlots on which cost adjustment has been applied
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void GetTaxLotsLatestCostAdjustmentDatesFromDB();

        // Added By : Manvendra Prajapati
        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8746
        /// <summary>
        /// Return closing date type, on the bases of preference 
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        PostTradeEnums.DateType GetClosingDateType();

        //Check if corporate action is applied or Underlying of Exercised or physical settlement is already closed in future date
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, StatusInfo> GetFutureDateClosingInfo(string taxlotClosingIds);

        /// <summary>
        /// Refresh closing data cache
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void RefreshClosingData();

        /// <summary>
        /// Checks the group status.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        Dictionary<string, PostTradeEnums.Status> GetGroupStatus(List<AllocationGroup> groups);

        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        void SetActicityServices(IActivityServices activityServices);

        /// <summary>
        /// Gets the closing transaction exceptions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(PranaAppException))]
        List<Position> GetClosingTransactionExceptions(DateTime fromDate, DateTime toDate, string accountIDs);

        /// <summary>
        /// Calculates the other fees for basic order information.
        /// </summary>
        /// <param name="basicOrderInfo">The basic order information.</param>
        /// <returns></returns>
        [OperationContract]
        BasicOrderInfo CalculateOtherFeesForBasicOrderInfo(BasicOrderInfo basicOrderInfo);
    }
}
