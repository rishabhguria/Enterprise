using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class AppReconciliedColumn
    {
        private string _appReconciliedColumnName;

        public string AppReconciliedColumnName
        {
            get { return _appReconciliedColumnName; }
            set { _appReconciliedColumnName = value; }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private bool _isIncludedAsCash;
        
        public bool IsIncludedAsCash
        {
            get { return _isIncludedAsCash; }
            set { _isIncludedAsCash = value; }
        }

        private EntryType _type;

        public EntryType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _acceptableDeviationSign;

        public string AcceptableDeviationSign
        {
            get { return _acceptableDeviationSign; }
            set { _acceptableDeviationSign = value; }
        }
	
        private double _acceptableDeviation;

        public double AcceptableDeviation
        {
            get { return _acceptableDeviation; }
            set { _acceptableDeviation = value; }
        }

        //private bool _isSourceDataAccepted;

        //public bool IsSourceDataAccepted
        //{
        //    get { return _isSourceDataAccepted; }
        //    set { _isSourceDataAccepted = value; }
        //}

        //private bool _isApplicationDataAccepted;

        //public bool IsApplicationDataAccepted
        //{
        //    get { return _isApplicationDataAccepted; }
        //    set { _isApplicationDataAccepted = value; }
        //}

        private AcceptDataFrom _acceptData;

        public AcceptDataFrom AcceptData
        {
            get { return _acceptData; }
            set { _acceptData = value; }
        }


    }
}
