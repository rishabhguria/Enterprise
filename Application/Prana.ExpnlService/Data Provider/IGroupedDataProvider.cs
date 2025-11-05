using Prana.BusinessObjects;
using Prana.Global;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExpnlService
{
    public interface IGroupedDataProvider
    {
        ExposureAndPnlOrderCollection GetUncalculatedDataClone();

        void SetQuartzScheduler();

        void GetUpdatedDataFromDB(List<int> auecsList);

        void Close();

        event EventHandler LogOnScreen;

        event EventHandler<EventArgs<EPnlOrder, ApplicationConstants.TaxLotState, bool>> UpdateRemoveOrder;

        DataSet IndexSymbols
        {
            get;
        }

        DataTable DtIndicesReturn
        {
            get;
        }

        Dictionary<int, NAVStruct> NAVValues
        {
            get;
        }

        Dictionary<int, NAVStruct> ShadowNAVValues
        {
            get;
        }

        int CompanyID
        {
            get;
        }

        Dictionary<int, ExposureAndPnlOrderSummary> Summaries
        {
            get;
        }

        event EventHandler UpdateIndicesMarkCacheEvent;

        void UpdateIndicesMarkCache(Dictionary<string, double> _indicesMarkPriceCache);

        DataSet IndicesMarkPrice
        {
            get;
        }

        Dictionary<string, List<string>> GetSymbolWiseTaxLotIDs();

        Dictionary<string, bool> GetSymbolWiseCalculationMarking();

        PMUIPrefs PMUIPrefs
        {
            get;
        }
    }
}
