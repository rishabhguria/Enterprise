using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class StageOrderRemovalData
    {
        private string parentClOrderIds;
        public string ParentClOrderIds
        {
            get { return parentClOrderIds; }
            set { parentClOrderIds = value; }
        }

        private bool isAllSubOrdersRemovable;
        public bool IsAllSubOrdersRemovable
        {
            get { return isAllSubOrdersRemovable; }
            set { isAllSubOrdersRemovable = value; }
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

        private bool isComingFromRollOver;
        public bool IsComingFromRollOver
        {
            get { return isComingFromRollOver; }
            set { isComingFromRollOver = value; }
        }
    }
}