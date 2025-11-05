using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects;
using System.Collections.ObjectModel;

namespace Prana.ExposureAndPNLCalculator
{
    public interface IGroupingComponent
    {
        string Name
        {
            get;
            set;
        }

        //TODO: change return type of GetCompressedData() to KeyedCollection<TKey, TValue> as might change in future after compression
        CompressedDataDictionaries GetCompressedData();
        OutputCompressedSummaries GetCalculatedSummaries();
        void SetInputData(Dictionary<int, ExposureAndPnlOrderCollection> uncompressedData, Dictionary<int, ExposureAndPnlOrderSummary> calculatedSummaries, ExposureAndPnlOrderCollection markedCollection, Dictionary<string, Dictionary<string, EpnlOrderGlobal>> distinctFundSetWiseOrderCollection, Dictionary<string, PartialPermissionExposureAndPnlOrderSummary> temp_distinctFundSetWiseSummary);

        //TODO: Pass compressed data in event DataCompressed
        event EventHandler DataCompressed;

        ExposurePnlCacheItemList GetContainingTaxLots(string compressedRowID, int fundID, ref Dictionary<string, Dictionary<string, EpnlOrderGlobal>> distinctFundSetWiseOrderCollection);

        Prana.BusinessObjects.AppConstants.ExPNLDashBoradType DashBoardType
        {
            get;
            set;
        }

        void Dispose();

    }
}
