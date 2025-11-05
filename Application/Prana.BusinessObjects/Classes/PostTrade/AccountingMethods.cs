using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public static class AccountingMethods
    {
        /// <summary>
        /// SetDefaultTableAndSchema
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="closingAssets"></param>
        /// <param name="ds"></param>
        public static void SetDefaultTableAndSchema(Dictionary<int, string> accounts, Dictionary<int, string> closingAssets, DataSet ds)
        {
            DataTable dt = new DataTable();
            dt.TableName = "AccountingMethods";
            dt.Columns.Add("Select", typeof(bool));
            dt.Columns.Add("AssetName");
            dt.Columns.Add("FundName");
            dt.Columns.Add("FundID");
            dt.Columns.Add("ClosingAlgo");
            dt.Columns.Add("SecondarySort");
            dt.Columns.Add("ClosingField");
            foreach (KeyValuePair<int, string> asset in closingAssets)
            {
                foreach (KeyValuePair<int, string> account in accounts)
                {
                    dt.Rows.Add(false, asset.Value, account.Value, account.Key, 2, 0, 0);
                }
            }
            ds.Tables.Add(dt);
        }
    }
}
