using Prana.BusinessObjects.AppConstants;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    public class TaxlotClosingInfo
    {
        private ClosingStatus _closingStatus;
        public ClosingStatus ClosingStatus
        {
            get { return _closingStatus; }
            set
            {
                _closingStatus = value;

            }
        }
        private DateTime _AUECMaxModifiedDate = DateTime.MinValue;

        public DateTime AUECMaxModifiedDate
        {
            get { return _AUECMaxModifiedDate; }
            set { _AUECMaxModifiedDate = value; }
        }

        private int _closingAlgo;
        public int ClosingAlgo
        {
            get { return _closingAlgo; }
            set { _closingAlgo = value; }
        }

        private double _openQuantity;
        public double OpenQuantity
        {
            get { return _openQuantity; }
            set { _openQuantity = value; }
        }

        private string _taxlotID;
        public string TaxLotID
        {
            get { return _taxlotID; }
            set { _taxlotID = value; }
        }

        // contains a mapping of exercised underlyingIDs and the parent closingTaxlotID.
        private Dictionary<string, string> _dictExercisedUnderlying = new Dictionary<string, string>();

        public Dictionary<string, string> DictExercisedUnderlying
        {
            get { return _dictExercisedUnderlying; }
            set { _dictExercisedUnderlying = value; }

        }
        private List<string> _listClosingId = new List<string>();
        public List<string> ListClosingId
        {
            get { return _listClosingId; }
            set { _listClosingId = value; }

        }

        private bool? _isManualyExerciseAssign = null;
        public virtual bool? IsManualyExerciseAssign
        {
            get { return _isManualyExerciseAssign; }
            set { _isManualyExerciseAssign = value; }
        }
    }
}
