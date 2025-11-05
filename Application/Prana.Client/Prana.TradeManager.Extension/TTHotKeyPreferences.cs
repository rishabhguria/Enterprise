namespace Prana.TradeManager.Extension
{
    public class TTHotKeyPreferences
    {
        # region Private Fields
        private int _companyUserHotKeyPreferenceID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private string _hotKeyPreferenceElements = string.Empty;
        private bool _enableBookMarkIcon = false;
        private bool _hotKeyOrderChanged = false;
        private bool _tTTogglePreferenceForWeb = false;
        #endregion

        #region Public Properties
        public int CompanyUserHotKeyPreferenceID
        {
            get
            {
                return this._companyUserHotKeyPreferenceID;
            }

            set
            {
                this._companyUserHotKeyPreferenceID = value;
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
        public string HotKeyPreferenceElements
        {
            get
            {
                return this._hotKeyPreferenceElements;
            }

            set
            {
                this._hotKeyPreferenceElements = value;
            }
        }
        public bool EnableBookMarkIcon
        {
            get
            {
                return this._enableBookMarkIcon;
            }

            set
            {
                this._enableBookMarkIcon = value;
            }
        }
        public bool HotKeyOrderChanged
        {
            get
            {
                return this._hotKeyOrderChanged;
            }

            set
            {
                this._hotKeyOrderChanged = value;
            }
        }

        public bool TTTogglePreferenceForWeb
        {
            get
            {
                return this._tTTogglePreferenceForWeb;
            }

            set
            {
                this._tTTogglePreferenceForWeb = value;
            }
        }
        #endregion


        public TTHotKeyPreferences()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
