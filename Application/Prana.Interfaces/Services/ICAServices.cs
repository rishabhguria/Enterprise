using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.ServiceCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;

namespace Prana.Interfaces
{

    [ServiceContract]
    public interface ICAServices : IServiceOnDemandStatus
    {
        IAllocationServices AllocationServices
        {
            set;
        }

        IClosingServices ClosingServices
        {
            set;
        }

        [OperationContract]
        void Initialize(int hashCode);
        [OperationContract]
        void ValidateCAInfo(CorporateActionType caType, ref DataTable caTable);
        [OperationContract]
        DataSet GetCAsForDateRange(CorporateActionType caType, DateTime fromDate, DateTime toDate, bool isApplied);
        // TODO : Check. only used in import.
        void SaveCAsOnly(DataRow dataRow);
        [OperationContract]
        bool SaveCAsOnly(DataTable dt);
        [OperationContract]
        bool UpdateCAsOnly(DataTable updateTable);
        [OperationContract]
        bool DeleteCAs(string caIDs);
        [OperationContract]
        CAPreviewResult PreviewCorporateActions(CorporateActionType caType, DataTable caTable, ref TaxlotBaseCollection taxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId);
        [OperationContract]
        TaxlotBaseCollection PreviewUndoCorporateActions(CorporateActionType caType, string caIDs, DataTable caTable);

        [OperationContract]
        bool SaveCorporateAction(CorporateActionType caType, string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID);
        [OperationContract(Name = "CashDivUndo")]
        bool UndoCorporateActions(CorporateActionType caType, string caIDs, TaxlotBaseCollection taxlots, bool isSmModificationRequired);
        [OperationContract]
        string CheckTaxlotsBeforeUndoPreview(CorporateActionType caType, Dictionary<string, DateTime> caWiseDates);
        [OperationContract]
        void ResetCache();
        [OperationContract]
        ///Gets Latest Corporate Action against a Symbol
        DataSet GetLatestCorpActionForSymbol(string caSymbols);
        [OperationContract(Name = "AllOtherCAUndo")]
        bool UndoCorporateActions(CorporateActionType caType, string caIDs, TaxlotBaseCollection taxlots);

        //event MsgReceivedDelegate CAResponseReceived;
        [OperationContract]
        void Clear();
    }
}
