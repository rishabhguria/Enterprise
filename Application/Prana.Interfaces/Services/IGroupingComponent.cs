using Prana.BusinessObjects;
using System;
using System.Collections.Generic;

namespace Prana.Interfaces
{
    public interface IGroupingComponent : IDisposable
    {
        CompressedDataDictionaries GetCompressedData();
        OutputCompressedSummaries GetCalculatedSummaries();
        void SetInputData(Dictionary<int, ExposureAndPnlOrderCollection> unCompressedData, Dictionary<int, ExposureAndPnlOrderSummary> compressedAccountSummaries, ExposureAndPnlOrderCollection markedCollection, Dictionary<int, DistinctAccountSetWiseSummaryCollection> distinctAccountSetWiseSummaryCollection);
        event EventHandler DataCompressed;
        string GroupingComponentName { get; }
        ExposurePnlCacheItemList GetContainingTaxLots(string compressedRowID, int accountID, int distinctAccountPermissionKey);
    }
}
