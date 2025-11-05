using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.Interfaces
{
    public interface ICorporateActionBaseRule
    {
        void Initialize(IPostTradeServices postTradeServicesInstance, int hashCode, IAllocationServices allocationServices, IClosingServices closingServices, object proxyPublishing, IActivityServices activityService, object pricingService, ISecMasterServices secmasterProxy, ICashManagementService cashManagementService);
        void ValidateCAInfo(ref DataRowCollection caRows);

        CAPreviewResult PreviewCorporateActions(DataRowCollection caRows, ref TaxlotBaseCollection taxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId);
        bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID);

        string CheckTaxlotsBeforeUndoPreview(Dictionary<string, DateTime> caWiseDates);
        TaxlotBaseCollection PreviewUndoCorporateActions(string caIDs, DataRowCollection caRows);
        bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots, bool isSMModificationRequired);
        bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots);
    }

    public delegate void PreviewCAResponseHandler(TaxlotBaseCollection taxlots);
}
