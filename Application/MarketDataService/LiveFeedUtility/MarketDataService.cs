using BusinessObjects;
using Prana.MarketDataService.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LiveFeedUtility
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = true)]
    [CallbackBehavior(UseSynchronizationContext = true)]
    class MarketDataService : IMarketDataService
    {
        public void AdviseSymbol(MDServiceReqObject smObject)
        {
            throw new NotImplementedException();
        }

        public void AdviseSymbolList(List<MDServiceReqObject> smObject)
        {
            throw new NotImplementedException();
        }

        public void DeleteSymbol(MDServiceReqObject smObject)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, string> SnapshotSymbol(MDServiceReqObject reqObj)
        {
            try
            {
                Logger.LogMessage("Snapshot requested for symbol: " + reqObj.Ticker);

                DataTable openSymbolTable = new DataTable();
                openSymbolTable.Columns.Add(new DataColumn("Symbol", typeof(string)));
                openSymbolTable.Columns.Add(new DataColumn("BloombergSymbol", typeof(string)));
                openSymbolTable.Columns.Add(new DataColumn("Asset", typeof(string)));
                openSymbolTable.Columns.Add(new DataColumn("Exchange", typeof(string)));

                openSymbolTable.Rows.Add(new object[] { reqObj.Ticker, reqObj.BloombergSymbol, reqObj.Asset, reqObj.Exchange });
                openSymbolTable = MarketDataProviderManager.MapDataProviderWithSymbolInfo(openSymbolTable);

                DataTable liveFeedDataTable = LiveFeedProviderFactory.GetLiveFeedData(openSymbolTable);
                if (liveFeedDataTable != null && liveFeedDataTable.Rows.Count > 0)
                {
                    Dictionary<string, string> resp = new Dictionary<string, string>();
                    foreach (DataColumn col in liveFeedDataTable.Columns)
                    {
                        resp[col.ColumnName] = liveFeedDataTable.Rows[0][col].ToString();
                    }
                    Logger.LogMessage("Snapshot response sent for symbol: " + reqObj.Ticker + Environment.NewLine);
                    return resp;
                }
                else
                {
                    Logger.LogMessage("Snapshot not found for requested symbol: " + reqObj.Ticker + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return null;
        }

        /// <summary>
        /// Gets the sm data.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetSMData(MDServiceReqObject obj)
        {
            try
            {
                Logger.LogMessage("SM Data requested for symbol: " + obj.Ticker);
                ILiveFeedProvider smProvider = LiveFeedProviderFactory.GetSMDataProvider();
                if (smProvider != null)
                {
                    Dictionary<string, string> resp = smProvider.ValidateSymbol(obj.Ticker,obj.Asset);
                    if(resp!=null)
                        Logger.LogMessage("SM Data response sent for symbol: " + obj.Ticker + Environment.NewLine);
                    else
                        Logger.LogMessage("SM Data not found for symbol: " + obj.Ticker + Environment.NewLine);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
            return null;
        }
    }
}
