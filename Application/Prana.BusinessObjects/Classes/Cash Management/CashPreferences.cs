using System;
using System.Xml.Serialization;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CashPreferences
    {
        private int _ID;
        public virtual int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private DateTime _cashMgmtStartDate;
        public virtual DateTime CashMgmtStartDate
        {
            get { return _cashMgmtStartDate; }
            set { _cashMgmtStartDate = value; }
        }

        private double _marginPercentage;
        public virtual double MarginPercentage
        {
            get { return _marginPercentage; }
            set { _marginPercentage = value; }
        }

        private bool _isCalculatePnL;
        public virtual bool IsCalculatePnL
        {
            get { return _isCalculatePnL; }
            set { _isCalculatePnL = value; }
        }

        private bool _isCalculateDividend;
        public virtual bool IsCalculateDividend
        {
            get { return _isCalculateDividend; }
            set { _isCalculateDividend = value; }
        }

        private bool _RealizedPLchkbox;
        public virtual bool IsRealizedPL
        {
            get { return _RealizedPLchkbox; }
            set { _RealizedPLchkbox = value; }
        }

        private bool _TotalPLchkbox;
        public virtual bool IsTotalPL
        {
            get { return _TotalPLchkbox; }
            set { _TotalPLchkbox = value; }
        }

        private bool _isCalculateBondAccurals;
        public virtual bool IsCalculateBondAccurals
        {
            get { return _isCalculateBondAccurals; }
            set { _isCalculateBondAccurals = value; }
        }

        private bool _isBreakRealizedPnlSubaccount;
        public virtual bool IsBreakRealizedPnlSubaccount
        {
            get { return _isBreakRealizedPnlSubaccount; }
            set { _isBreakRealizedPnlSubaccount = value; }
        }

        private bool _isCreateManualJournals;
        public virtual bool IsCreateManualJournals
        {
            get { return _isCreateManualJournals; }
            set { _isCreateManualJournals = value; }
        }

        private bool _isCalculateCollateral;
        public virtual bool IsCalculateCollateral
        {
            get { return _isCalculateCollateral; }
            set { _isCalculateCollateral = value; }
        }

        private string _isCalculateCollateralFrequencyInterest;
        public virtual string IsCalculateCollateralFrequencyInterest
        {
            get { return _isCalculateCollateralFrequencyInterest; }
            set { _isCalculateCollateralFrequencyInterest = value; }
        }


        #region CHMW-3141
        //[Foreign Positions Settling in Base Currency] Add preferences in cash management module to show/hide settlement journal entries
        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3141
        private bool _isCashSettlementEntriesVisible;

        public virtual bool IsCashSettlementEntriesVisible
        {
            get { return _isCashSettlementEntriesVisible; }
            set { _isCashSettlementEntriesVisible = value; }
        }


        /// <summary>
        /// The symbol wise revaluation date
        /// </summary>
        private DateTime _symbolWiseRevaluationDate;

        /// <summary>
        /// Gets or sets the symbol wise revaluation date.
        /// </summary>
        /// <value>
        /// The symbol wise revaluation date.
        /// </value>
        public virtual DateTime SymbolWiseRevaluationDate
        {
            get { return _symbolWiseRevaluationDate; }
            set { _symbolWiseRevaluationDate = value; }
        }
        #endregion

        //added by: Bharat raturi, 1 jul 2014
        //purpose: add accountid to save the preferences by accountid
        private int _accountID;
        public virtual int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }

        private bool _isPublishRevaluationData;
        public virtual bool IsPublishRevaluationData
        {
            get { return _isPublishRevaluationData; }
            set { _isPublishRevaluationData = value; }
        }

        private bool _isAccruedTillSettlement;
        public virtual bool IsAccruedTillSettlement
        {
            get { return _isAccruedTillSettlement; }
            set { _isAccruedTillSettlement = value; }
        }
    }

    [XmlRoot("CashManagementPreferences")]
    public class CashSavePreference : IPreferenceData
    {
        public string _yesterdayEndColumn;
        public string _dayEndDataColumn;
        public string _transcationDetailColumn;

        //added by: Bharat raturi, 1 jul 2014
        //purpose: add accountid to save the preferences by accountid
        private int _accountID;

        public CashSavePreference()
        {
            _yesterdayEndColumn = string.Empty;
            _dayEndDataColumn = string.Empty;
            _transcationDetailColumn = string.Empty;
            _accountID = int.MinValue;
        }

        public string YesterdayEndColumn
        {
            get { return _yesterdayEndColumn; }
            set { _yesterdayEndColumn = value; }
        }

        public string DayEndDataColumn
        {
            get { return _dayEndDataColumn; }
            set { _dayEndDataColumn = value; }
        }

        public string TranscationDetailColumn
        {
            get { return _transcationDetailColumn; }
            set { _transcationDetailColumn = value; }
        }

        public int AccountID
        {
            get { return _accountID; }
            set { _accountID = value; }
        }
    }
}
