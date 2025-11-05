using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Raturi: Adding new classes RevaluationUpdateDetail and BalanceUpdateDetail
    /// http://jira.nirvanasolutions.com:8080/browse/PRANA-7649
    /// </summary>
    [Serializable]
    /// <summary>
    /// Class showing the revaluation details
    /// </summary>
    public class RevaluationUpdateDetail
    {
        public DateTime LastRevaluationDate { get; set; }
        public bool isUpdatedReval { get; set; }
        public RevaluationUpdateDetail(DateTime revaluationDate, bool isUpdatedRevaluation)
        {
            LastRevaluationDate = revaluationDate;
            isUpdatedReval = isUpdatedRevaluation;
        }
    }

    [Serializable]
    /// <summary>
    /// Class for balance update details
    /// </summary>
    public class BalanceUpdateDetail
    {
        public DateTime LastBalanceDate { get; set; }
        public bool isUpdatedBalance { get; set; }

        public BalanceUpdateDetail(DateTime revaluationDate, bool isUpdatedRevaluation)
        {
            LastBalanceDate = revaluationDate;
            isUpdatedBalance = isUpdatedRevaluation;
        }
    }

    public class LastCalculatedBalanceDate
    {
        private int _ID;
        public virtual int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private int _subAcID;
        public virtual int SubAcID
        {
            get { return _subAcID; }
            set { _subAcID = value; }
        }

        private int _accountID;
        public virtual int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private int _currencyID;
        public virtual int CurrencyID
        {
            get { return _currencyID; }
            set { _currencyID = value; }
        }

        private DateTime _lastCalcDate;
        public virtual DateTime LastCalcDate
        {
            get { return _lastCalcDate; }
            set { _lastCalcDate = value; }
        }

        private bool _updatedBalance;
        public virtual bool UpdatedBalance
        {
            get { return _updatedBalance; }
            set { _updatedBalance = value; }
        }

        public virtual string GetKey()
        {
            return _accountID.ToString() + "_" + _currencyID.ToString() + "_" + _subAcID.ToString();
        }
    }
}
