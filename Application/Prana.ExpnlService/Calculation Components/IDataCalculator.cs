using Prana.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExpnlService
{
    public interface IDataCalculator
    {
        void SetCompanyID(int companyID);
        void CalculateData(ExposureAndPnlOrderCollection uncalculatedData, ref Dictionary<int, ExposureAndPnlOrderCollection> accountWiseOrderCollection, Dictionary<int, List<int>> distinctAccountPermissionSets);
        void CalculateSummariesFromData(ref Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummaryCollection, ref Dictionary<int, ExposureAndPnlOrderSummary> accountwiseSummary, ref Dictionary<int, ExposureAndPnlOrderCollection> calculatedData, Dictionary<int, List<int>> distinctAccountPermissionSets, bool isNAVSaved);
        void CalculateIndexReturns(DataSet indexSymbols, ref DataTable returnsTable, Dictionary<string, double> indicesMarkPriceCache);
        void UpdateCurrentDatesAndClearanceTime(Dictionary<int, DateTime> updatedAuecWiseAdjustedCurrentDates, Dictionary<int, DateTime> updatedClearanceTimes);
        void ClearSummaryCalculatorCache();
    }
}
