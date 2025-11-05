namespace Prana.TradeManager.Extension
{
    public class TTHotKeyPreferencesDetails
    {
        # region Private Fields
        private int _companyUserHotKeyID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private string _companyUserHotKeyName = string.Empty;
        private string _hotKeyPreferenceNameValue = string.Empty;
        private bool _isFavourites = false;
        private int _hotKeySequence = int.MinValue;
        private string _moduleName = string.Empty;
        private string _hotButtontype = string.Empty;
        #endregion

        #region Public Properties
        public int CompanyUserHotKeyID
        {
            get
            {
                return this._companyUserHotKeyID;
            }

            set
            {
                this._companyUserHotKeyID = value;
            }
        }

        public int CompanyUserID
        {
            get
            {
                return this._companyUserID;
            }

            set
            {
                this._companyUserID = value;
            }
        }
        public string CompanyUserHotKeyName
        {
            get
            {
                return this._companyUserHotKeyName;
            }

            set
            {
                this._companyUserHotKeyName = value;
            }
        }

        public string HotKeyPreferenceNameValue
        {
            get
            {
                return this._hotKeyPreferenceNameValue;
            }

            set
            {
                this._hotKeyPreferenceNameValue = value;
            }
        }

        public bool IsFavourites
        {
            get
            {
                return this._isFavourites;
            }

            set
            {
                this._isFavourites = value;
            }
        }

        public int HotKeySequence
        {
            get
            {
                return this._hotKeySequence;
            }

            set
            {
                this._hotKeySequence = value;
            }
        }

        public string Module
        {
            get
            {
                return this._moduleName;
            }
            set
            {
                this._moduleName = value;
            }
        }

        public string HotButtontype
        {
            get
            {
                return this._hotButtontype;
            }
            set
            {
                this._hotButtontype = value;
            }
        }
        #endregion


        public TTHotKeyPreferencesDetails()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
