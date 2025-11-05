using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.Client.UI
{
    class ForwardPointsCopyAccountData : CopyAccountData
    {
        private int fromAccountID;
        private List<int> toAccountIDs;
        private Dictionary<string, DataRow> copyFromRowsWithKeys = new Dictionary<string, DataRow>();

        public ForwardPointsCopyAccountData(int fromAccountID, List<int> toAccountIDs, System.Data.DataTable datasource)
        {
            try
            {
                this.fromAccountID = fromAccountID;
                this.toAccountIDs = toAccountIDs;
                foreach (DataRow row in datasource.Rows)
                {
                    if (row.Field<int>("AccountID") == fromAccountID && (row.Field<string>("AUECIdentifier").Equals("FX-FXFWD") || row.Field<string>("AUECIdentifier").Equals("FX-FX")))
                    {
                        copyFromRowsWithKeys.Add(GetRowKey(row), row);
                    }
                }
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

        public override void CopyDataToAccounts(System.Data.DataTable datasource)
        {
            try
            {
                foreach (DataRow row in datasource.Rows)
                {
                    if (row.Field<int>("AccountID") != fromAccountID && toAccountIDs.Contains(row.Field<int>("AccountID")) && ((row.Field<string>("AUECIdentifier").Equals("FX-FXFWD") || row.Field<string>("AUECIdentifier").Equals("FX-FX"))))
                    {
                        string rowKey = GetRowKey(row);
                        if (copyFromRowsWithKeys.ContainsKey(rowKey))
                        {
                            DataRow dr = copyFromRowsWithKeys[rowKey];
                            row[5] = dr[5];
                            row[8] = Double.Parse(row[5].ToString()) + Double.Parse(row[6].ToString());
                        }
                    }
                }
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

        public override sealed string GetRowKey(System.Data.DataRow row)
        {
            return row.Field<string>("Symbol");
        }
    }
}
