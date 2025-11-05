using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class User
    {
        #region Local variables

        int _userID = int.MinValue;
        string _lastName = string.Empty;
        string _firstName = string.Empty;
        string _shortName = string.Empty;
        string _title = string.Empty;
        string _mailingAddress = string.Empty;
        string _eMail = string.Empty;
        string _telephoneWork = string.Empty;
        string _telephoneHome = string.Empty;
        string _telephoneMobile = string.Empty;
        string _fax = string.Empty;

        #endregion

        #region Constructors

        public User()
        {
        }

        public User(int userID, string lastName, string firstName,
            string shortName, string title, string mailingAddress,
            string eMail, string telephoneWork, string telephoneHome,
            string telephoneMobile, string fax)
        {
            _userID = userID;
            _lastName = lastName;
            _firstName = firstName;
            _shortName = shortName;
            _title = title;
            _mailingAddress = mailingAddress;
            _eMail = eMail;
            _telephoneWork = telephoneWork;
            _telephoneHome = telephoneHome;
            _telephoneMobile = telephoneMobile;
            _fax = fax;
        }

        #endregion

        #region Properties

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string MailingAddress
        {
            get { return _mailingAddress; }
            set { _mailingAddress = value; }
        }

        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }

        public string TelephoneWork
        {
            get { return _telephoneWork; }
            set { _telephoneWork = value; }
        }

        public string TelephoneHome
        {
            get { return _telephoneHome; }
            set { _telephoneHome = value; }
        }

        public string TelephoneMobile
        {
            get { return _telephoneMobile; }
            set { _telephoneMobile = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        #endregion
    }
}
