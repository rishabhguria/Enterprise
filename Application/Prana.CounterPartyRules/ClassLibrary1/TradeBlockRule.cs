using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.DatabaseManager;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Prana.CounterPartyRules
{
    class TradeBlockRule : DllBaseRule
    {
        private static readonly object _locker = new object();
        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            try
            {
                if (!msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TradeBlocked))
                {
                    msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TradeBlocked, "1");
                }
                BackgroundWorker bgW = new BackgroundWorker();
                bgW.DoWork += new DoWorkEventHandler(bgW_DoWork);
                bgW.RunWorkerAsync(msg);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void bgW_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (_locker)
            {
                try
                {
                    Prana.BusinessObjects.FIX.PranaMessage msg = (Prana.BusinessObjects.FIX.PranaMessage)e.Argument;

                    QueryData queryData = new QueryData();
                    queryData.StoredProcedureName = "P_SaveBlockedTradeMessage";
                    queryData.DictionaryDatabaseParameter.Add("@pranaMessage", new DatabaseParameter()
                    {
                        IsOutParameter = false,
                        ParameterName = "@pranaMessage",
                        ParameterType = DbType.String,
                        ParameterValue = msg.ToString()
                    });
                    queryData.CommandTimeout = 300;
                    DatabaseManager.DatabaseManager.ExecuteDataSet(queryData);
                }
                catch (Exception ex)
                {
                    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                    if (rethrow)
                    {
                        throw;
                    }
                }
            }
        }

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            return true;
        }

        public override void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingMessageFields, string repeatingTag)
        {
            return;
        }
    }
}
