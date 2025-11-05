using System.Data;

namespace Prana.PM.Client.UI
{
    public abstract class CopyAccountData
    {
        public abstract string GetRowKey(DataRow row);
        public abstract void CopyDataToAccounts(DataTable datasource);
    }
}
