using System;
using System.Data.SqlTypes;

namespace Prana.Admin.BLL
{
    //class for getting the details of the account
    public class AccountDetails : Account
    {
        private int currency;
        private DateTime inceptionDate;
        private DateTime onBoardDate;
        //private DateTime lockDate;
        private string lockDate;
        private int closingMethodology;
        private int secondarySortCriteria;
        private int lockSchedule;
        private bool _isSwapAccount;
        public AccountModifiedType modifiedType;
        private bool _isActive;
        //private int companyThirdParty;

        public AccountDetails()
        {
            Currency = int.MinValue;
            inceptionDate = DateTime.Now;
            onBoardDate = DateTime.Now;
            LockDate = SqlDateTime.MinValue.ToString();
            AccountID = 0;
            AccountName = string.Empty;
            AccountShortName = string.Empty;
            CompanyID = 0;
            CompanyPrimeBrokerClearerID = int.MinValue;
            modifiedType = AccountModifiedType.None;
            LockSchedule = int.MinValue;
            SecondarySortCriteria = int.MinValue;
            _isActive = false;
        }

        public int Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public DateTime InceptionDate
        {
            get { return inceptionDate; }
            set { inceptionDate = value; }
        }

        public DateTime OnBoardDate
        {
            get { return onBoardDate; }
            set { onBoardDate = value; }
        }

        //public DateTime LockDate
        public string LockDate
        {
            get { return lockDate; }
            set { lockDate = value; }
        }

        public int ClosingMethodology
        {
            get { return closingMethodology; }
            set { closingMethodology = value; }
        }

        public int SecondarySortCriteria
        {
            get { return secondarySortCriteria; }
            set { secondarySortCriteria = value; }
        }

        public int LockSchedule
        {
            get { return lockSchedule; }
            set { lockSchedule = value; }
        }

        public bool IsSwapAccount
        {
            get { return _isSwapAccount; }
            set { _isSwapAccount = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }
        //public int CompanyThirdParty
        //{
        //    get { return companyThirdParty; }
        //    set { companyThirdParty = value; }
        //}

        /// <summary>
        /// added by: Bharat Raturi, 09-jun-2014
        /// Enum to indicate the status of the account
        /// </summary>
        public enum AccountModifiedType
        {
            None,
            Added,
            Deleted,
            Updated
        }
    }
}
