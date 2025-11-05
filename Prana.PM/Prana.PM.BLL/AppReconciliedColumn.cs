
using Csla;

namespace Prana.PM.BLL
{
    public class AppReconciliedColumn : BusinessBase<AppReconciliedColumn>
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyHasChanged();
            }
        }

        private int _dataSourceColumnID;

        /// <summary>
        /// Gets or sets the app reconcilied column ID.
        /// </summary>
        /// <value>The app reconcilied column ID.</value>
        public int DataSourceColumnID
        {
            get { return _dataSourceColumnID; }
            set
            {
                _dataSourceColumnID = value;
                PropertyHasChanged();
            }
        }

        private string _appReconciliedColumnName;

        public string AppReconciliedColumnName
        {
            get { return _appReconciliedColumnName; }
            set
            {
                _appReconciliedColumnName = value;
                PropertyHasChanged();
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                PropertyHasChanged();
            }
        }

        private bool _isIncludedAsCash;

        public bool IsIncludedAsCash
        {
            get { return _isIncludedAsCash; }
            set
            {
                _isIncludedAsCash = value;
                PropertyHasChanged();
            }
        }

        private EntryType _type;

        public EntryType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                PropertyHasChanged();
            }
        }

        private DeviationSignList _deviationSign;

        /// <summary>
        /// Gets or sets the deviation sign.
        /// </summary>
        /// <value>The deviation sign.</value>
        public DeviationSignList DeviationSign
        {
            get { return _deviationSign; }
            set
            {
                _deviationSign = value;
                PropertyHasChanged();
            }
        }

        private double _acceptableDeviation;

        public double AcceptableDeviation
        {
            get { return _acceptableDeviation; }
            set
            {
                _acceptableDeviation = value;
                PropertyHasChanged();
            }
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

        private AcceptDataFrom _acceptDataFrom;

        public AcceptDataFrom AcceptDataFrom
        {
            get { return _acceptDataFrom; }
            set { _acceptDataFrom = value; }
        }

        public override bool IsValid
        {
            get { return base.IsValid; }
        }

        public override bool IsDirty
        {
            get { return base.IsDirty; }
        }

        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _id;
        }

    }
}
