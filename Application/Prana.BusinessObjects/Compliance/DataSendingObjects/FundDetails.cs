using System;

namespace Prana.BusinessObjects.Compliance.DataSendingObjects
{
    /// <summary>
    /// Definition of AccountDetails
    /// Used to send data to esper
    /// </summary>
    public class AccountDetails
    {
        int accountId;

        public int AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }
        String accountShortName;

        public String AccountShortName
        {
            get { return accountShortName; }
            set { accountShortName = value; }
        }
        String accountLongName;

        public String AccountLongName
        {
            get { return accountLongName; }
            set { accountLongName = value; }
        }
        int masterFundId;

        public int MasterFundId
        {
            get { return masterFundId; }
            set { masterFundId = value; }
        }
        String masterFundName;

        public String MasterFundName
        {
            get { return masterFundName; }
            set { masterFundName = value; }
        }
        int companyId;

        public int CompanyId
        {
            get { return companyId; }
            set { companyId = value; }
        }

        int baseCurrencyId;

        public int BaseCurrencyId
        {
            get { return baseCurrencyId; }
            set { baseCurrencyId = value; }
        }
    }
}
