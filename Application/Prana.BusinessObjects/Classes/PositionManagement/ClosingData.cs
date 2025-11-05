using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects.PositionManagement
{
    [Serializable]
    public class ClosingData
    {
        private bool _isPartial = false;
        public bool IsPartial
        {
            get { return _isPartial; }
            set { _isPartial = value; }
        }

        private bool _isError = false;
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; }
        }

        private List<TaxLot> _taxlots = new List<TaxLot>();
        public List<TaxLot> Taxlots
        {
            get { return _taxlots; }
            set { _taxlots = value; }
        }

        private List<TaxLot> _taxlotsToPopulate = new List<TaxLot>();
        public List<TaxLot> TaxlotsToPopulate
        {
            get { return _taxlotsToPopulate; }
            set { _taxlotsToPopulate = value; }
        }

        private List<Position> _closedPositions = new List<Position>();
        public List<Position> ClosedPositions
        {
            get { return _closedPositions; }
            set { _closedPositions = value; }
        }

        private Dictionary<string, TaxLot> _updatedTaxlots = new Dictionary<string, TaxLot>();
        public Dictionary<string, TaxLot> UpdatedTaxlots
        {
            get { return _updatedTaxlots; }
            set { _updatedTaxlots = value; }
        }

        private List<TaxLot> _unSavedTaxlots = new List<TaxLot>();
        public List<TaxLot> UnSavedTaxlots
        {
            get { return _unSavedTaxlots; }
            set { _unSavedTaxlots = value; }
        }

        private List<TaxLot> _conflictedTaxlots = new List<TaxLot>();
        public List<TaxLot> ConflictedTaxlots
        {
            get { return _conflictedTaxlots; }
            set { _conflictedTaxlots = value; }
        }

        private StringBuilder _errorMsg = new StringBuilder();
        public StringBuilder ErrorMsg
        {
            get { return _errorMsg; }
            set { _errorMsg = value; }
        }

        private bool _isDataClosed = false;
        public bool IsDataClosed
        {
            get { return _isDataClosed; }
            set { _isDataClosed = value; }
        }

        private string _positionsToUnwind = string.Empty;
        public string PositionsToUnwind
        {
            get { return _positionsToUnwind; }
            set { _positionsToUnwind = value; }
        }

        public bool IsNavLockFailed
        {
            get;
            set;
        }

        public string NavLockFailedTemplates
        {
            get;
            set;
        }
    }
}
