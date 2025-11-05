namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Trader.
    /// </summary>
    public class Trader
    {
        int _traderID = int.MinValue;
        string _firstName = string.Empty;
        string _lastName = string.Empty;
        string _shortName = string.Empty;
        string _title = string.Empty;
        string _eMail = string.Empty;
        string _telephoneWork = string.Empty;
        string _telephoneCell = string.Empty;
        string _pager = string.Empty;
        string _telephoneHome = string.Empty;
        string _fax = string.Empty;
        int _companyID = int.MinValue;
        bool _isReferenced = false;

        public Trader(int traderID, string firstName)
        {
            _traderID = traderID;
            _firstName = firstName;
        }
        public Trader(int traderID)
        {
            _traderID = traderID;
            if (_traderID == int.MinValue)
                _shortName = Global.ApplicationConstants.C_COMBO_SELECT;
        }
        public Trader(int traderID, string shortName, string firstName)
        {
            _traderID = traderID;
            _shortName = shortName;
            _firstName = firstName;
        }
        public Trader()
        {
        }


        public int TraderID
        {
            get
            {
                return _traderID;
            }

            set
            {
                _traderID = value;
            }
        }


        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                _firstName = value;
            }
        }


        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
            }
        }


        public string ShortName
        {
            get
            {
                return _shortName;
            }

            set
            {
                _shortName = value;
            }
        }


        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
            }
        }


        public string EMail
        {
            get
            {
                return _eMail;
            }

            set
            {
                _eMail = value;
            }
        }


        public string TelephoneWork
        {
            get
            {
                return _telephoneWork;
            }

            set
            {
                _telephoneWork = value;
            }
        }


        public string TelephoneCell
        {
            get
            {
                return _telephoneCell;
            }

            set
            {
                _telephoneCell = value;
            }
        }


        public string Pager
        {
            get
            {
                return _pager;
            }

            set
            {
                _pager = value;
            }
        }


        public string TelephoneHome
        {
            get
            {
                return _telephoneHome;
            }

            set
            {
                _telephoneHome = value;
            }
        }


        public string Fax
        {
            get
            {
                return _fax;
            }

            set
            {
                _fax = value;
            }
        }

        public int CompanyID
        {
            get
            {
                return _companyID;
            }
            set
            {
                _companyID = value;
            }
        }
        public void SetReference(bool bRef)
        {
            _isReferenced = bRef;

        }
        public bool GetReference()
        {
            return _isReferenced;

        }
    }


}
