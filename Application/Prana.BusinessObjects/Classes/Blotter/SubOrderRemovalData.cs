using System.Collections.Generic;

namespace Prana.BusinessObjects.Classes.Blotter
{
    public class SubOrderRemovalData
    {
        private string subOrdersClOrderIds;
        public string SubOrdersClOrderIds
        {
            get { return subOrdersClOrderIds; }
            set { subOrdersClOrderIds = value; }
        }

        private int companyUserID;
        public int CompanyUserID
        {
            get { return companyUserID; }
            set { companyUserID = value; }
        }

        private List<int> uniqueTradingAccounts;
        public List<int> UniqueTradingAccounts
        {
            get { return uniqueTradingAccounts; }
            set { uniqueTradingAccounts = value; }
        }
    }
}
