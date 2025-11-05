namespace Prana.Admin.BLL
{
    public class FeederAccountItem
    {
        private int feederAccountID;
        private string feederAccountName;
        private string feederShortName;
        private decimal feederAmount;
        private int feederCurrency;
        private int feederCompanyID;
        private decimal feederRemainingAmount;
        private decimal feederAllocatedAmount;

        public FeederAccountItem()
        {
            FeederAccountName = string.Empty;
            FeederShortName = string.Empty;
            FeederAmount = 0.0M;
            FeederCurrency = 0;
            FeederAllocatedAmount = 0.0M;
            FeederRemainingAmount = 0.0M;
        }

        public FeederAccountItem(string feedName, string feedShortName, decimal feedAmount, int feedCurrency)
        {
            FeederAccountName = feedName;
            FeederShortName = feedShortName;
            FeederAmount = feedAmount;
            FeederCurrency = feedCurrency;
            FeederCompanyId = feederCompanyID;
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

        public string FeederAccountName
        {
            get
            {
                return feederAccountName;
            }
            set
            {
                feederAccountName = value;
            }
        }

        public string FeederShortName
        {
            get
            {
                return feederShortName;
            }
            set
            {
                feederShortName = value;
            }
        }

        public decimal FeederAmount
        {
            get
            {
                return feederAmount;
            }
            set
            {
                feederAmount = value;
            }
        }
        public int FeederCurrency
        {
            get
            {
                return feederCurrency;
            }
            set
            {
                feederCurrency = value;
            }
        }

        public int FeederCompanyId
        {
            get
            {
                return feederCompanyID;
            }
            set
            {
                feederCompanyID = value;
            }
        }

        public decimal FeederRemainingAmount
        {
            get
            {
                return feederRemainingAmount;
            }
            set
            {
                feederRemainingAmount = value;
            }
        }

        public decimal FeederAllocatedAmount
        {
            get
            {
                return feederAllocatedAmount;
            }
            set
            {
                feederAllocatedAmount = value;
            }
        }
    }
}
