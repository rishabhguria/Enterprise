using Prana.BusinessObjects;
using System.Collections.Generic;

namespace Prana.ExpnlService.Grouping_Components.Views_and_SummaryCalculators.Compressors
{
    public interface ICompressor
    {
        CompressedDataDictionaries GetData(Dictionary<int, ExposureAndPnlOrderCollection> TaxLots, ExposureAndPnlOrderCollection markedCollection, Dictionary<int, DistinctAccountSetWiseSummaryCollection> CompressedDistinctAccountSetWiseSummaryCollection, Dictionary<int, ExposureAndPnlOrderSummary> CompressedAccountSummaries);
        ExposurePnlCacheItemList GetContainingTaxlots(string compressedRowID, int accountID, DistinctAccountSetWiseSummaryCollection outputAccountSetWiseConsolidatedSummary);
    }
}
