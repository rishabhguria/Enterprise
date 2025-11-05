using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class CAOnProcessObjects : IDisposable
    {
        private DataTable _caTable = new DataTable();
        //private bool _isRedo;
        private bool _isNameChangeOnly;
        private DateTime _fromDate;
        private TaxlotBaseCollection _taxlotBaseCollection = new TaxlotBaseCollection();
        private Guid _caID;
        private string _caIDs;
        private string _Symbol;
        private string _corporateActionListString;
        private CorporateActionType _CAType;
        private string _newSymbol;
        private string _companyName;

        public CAOnProcessObjects()
        {
            ///Table name assigned to help in serialization 
            _caTable.TableName = "CATable";
        }

        private ClosingData _closingData;
        /// <summary>
        /// Assigns the closing related data of original transac and withdrawal transac in namechange
        /// </summary>
        public ClosingData ClosingData
        {
            get { return _closingData; }
            set { _closingData = value; }
        }

        public CorporateActionType CAType
        {
            get { return _CAType; }
            set { _CAType = value; }
        }

        public DataTable CATable
        {
            get { return _caTable; }
            set { _caTable = value; }
        }


        private Object _message;

        public Object Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }

        public string NewSymbol
        {
            get { return _newSymbol; }
            set { _newSymbol = value; }
        }

        public Guid CorporateActionID
        {
            get { return _caID; }
            set { _caID = value; }
        }

        public string CorporateActionIDs
        {
            get { return _caIDs; }
            set { _caIDs = value; }
        }

        public string CorporateActionListString
        {
            get { return _corporateActionListString; }
            set { _corporateActionListString = value; }
        }

        private int _affectedPositions;

        public int AffectedPositions
        {
            get { return _affectedPositions; }
            set { _affectedPositions = value; }
        }

        private bool _isSaved = false;

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }


        private bool _isApplied;

        public bool IsApplied
        {
            get { return _isApplied; }
            set { _isApplied = value; }
        }

        public bool IsRedo
        {
            get { return _isNameChangeOnly; }
            set { _isNameChangeOnly = value; }
        }

        //public bool IsNameChangeOnly
        //{
        //    get { return _isRedo; }
        //    set { _isRedo = value; }
        //}

        private bool _isExist = false;

        public bool IsExist
        {
            get { return _isExist; }
            set { _isExist = value; }
        }


        public DateTime FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }


        private DateTime _toDate;

        public DateTime ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        public TaxlotBaseCollection Taxlots
        {
            get { return _taxlotBaseCollection; }
            set { _taxlotBaseCollection = value; }
        }

        private List<AllocationGroup> _newGeneratedTaxlots;
        /// <summary>
        /// Contains the newly generated taxlot xml with the help of posttradecachemanager
        /// </summary>
        public List<AllocationGroup> NewGeneratedTaxlots
        {
            get { return _newGeneratedTaxlots; }
            set { _newGeneratedTaxlots = value; }
        }


        //private string _parentControl;

        //public string ParentControl
        //{
        //    get { return _parentControl; }
        //    set { _parentControl = value; }
        //}

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_caTable != null)
                        _caTable.Dispose();
                    if (CATable != null)
                        CATable.Dispose();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
    }
}
