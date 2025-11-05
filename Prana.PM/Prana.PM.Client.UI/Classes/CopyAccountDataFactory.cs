using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.PM.Client.UI
{
    public static class CopyAccountDataFactory
    {
        internal static CopyAccountData GetAccountDataCopier(string tabName, int fromAccountID, List<int> toAccountIDs, DataTable datasource)
        {
            CopyAccountData copyAccountData = null;
            try
            {
                switch (tabName)
                {
                    case "tabPageMarkPrice":
                        copyAccountData = new MarkPriceCopyAccountData(fromAccountID, toAccountIDs, datasource);
                        break;
                    case "tabPageForexConversion":
                        copyAccountData = new ForexConversionCopyAccountData(fromAccountID, toAccountIDs, datasource);
                        break;
                    case "tabPagefxmarkPrices":
                        copyAccountData = new ForwardPointsCopyAccountData(fromAccountID, toAccountIDs, datasource);
                        break;
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
            return copyAccountData;
        }
    }
}
