using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.Client.UI
{
    class MarkPriceCopyAccountData : CopyAccountData
    {
        private int fromAccountID;
        private List<int> toAccountIDs;
        private Dictionary<string, DataRow> copyFromRowsWithKeys = new Dictionary<string, DataRow>();

        public MarkPriceCopyAccountData(int fromAccountID, List<int> toAccountIDs, DataTable datasource)
        {
            try
            {
                this.fromAccountID = fromAccountID;
                this.toAccountIDs = toAccountIDs;
                foreach (DataRow row in datasource.Rows)
                {
                    if (row.Field<int>("AccountID") == fromAccountID && row.Field<int>("AUECId") != 32 && row.Field<int>("AUECId") != 33)
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

        public override void CopyDataToAccounts(DataTable datasource)
        {
            try
            {
                foreach (DataRow row in datasource.Rows)
                {
                    if (row.Field<int>("AccountID") != fromAccountID && toAccountIDs.Contains(row.Field<int>("AccountID")) && row.Field<int>("AUECId") != 32 && row.Field<int>("AUECId") != 33)
                    {
                        string rowKey = GetRowKey(row);
                        if (copyFromRowsWithKeys.ContainsKey(rowKey))
                        {
                            DataRow dr = copyFromRowsWithKeys[rowKey];
                            for (int i = 8; i < row.Table.Columns.Count; i++)
                            {
                                row[i] = dr[i];
                            }
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

        public override sealed string GetRowKey(DataRow row)
        {
            return row.Field<string>("Symbol");
        }
    }
}
