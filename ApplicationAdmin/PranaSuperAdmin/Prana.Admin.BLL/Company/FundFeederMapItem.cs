namespace Prana.Admin.BLL
{
    public class AccountFeederMapItem
    {
        int recordID;
        int accountID;
        int feederAccountID;
        decimal allocatedAmount;
        int companyID;
        int currencyID;

        public AccountFeederMapItem()
        {
            RecordID = 0;
            FeederAccountID = 0;
            AccountID = 0;
            AllocatedAmount = 0.0M;
            CompanyID = 0;
            CurrencyID = 0;
        }

        public int RecordID
        {
            get
            {
                return recordID;
            }
            set
            {
                recordID = value;
            }
        }
        public int AccountID
        {
            get
            {
                return accountID;
            }
            set
            {
                accountID = value;
            }
        }
        public int FeederAccountID
        {
            get
            {
                return feederAccountID;
            }
            set
            {
                feederAccountID = value;
            }
        }
        public decimal AllocatedAmount
        {
            get
            {
                return allocatedAmount;
            }
            set
            {
                allocatedAmount = value;
            }
        }
        public int CompanyID
        {
            get
            {
                return companyID;
            }
            set
            {
                companyID = value;
            }
        }
        public int CurrencyID
        {
            get
            {
                return currencyID;
            }
            set
            {
                currencyID = value;
            }
        }

        enum ModifiedType
        {
            NoChange,
            Add,
            Edit,
            Delete
        };
    }
}
