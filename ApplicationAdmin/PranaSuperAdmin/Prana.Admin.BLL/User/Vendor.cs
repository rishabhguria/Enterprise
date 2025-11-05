namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Vendor.
    /// </summary>
    public class Vendor
    {
        #region Private members

        private int _vendorID = int.MinValue;
        private string _vendorName = string.Empty;
        private string _lastName = string.Empty;
        private string _firstName = string.Empty;
        private string _shortName = string.Empty;
        private string _title = string.Empty;
        private string _product = string.Empty;
        private string _comment = string.Empty;
        private string _mailingAddress = string.Empty;
        private string _eMail = string.Empty;
        private string _telephoneWork = string.Empty;
        private string _telephoneHome = string.Empty;
        private string _telephoneMobile = string.Empty;
        private string _telephonePager = string.Empty;
        private string _fax = string.Empty;
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;

        #endregion

        #region Constructors

        public Vendor()
        {
        }

        #endregion

        #region Properties

        public int VendorID
        {
            get { return _vendorID; }
            set { _vendorID = value; }
        }

        public string VendorName
        {
            get { return _vendorName; }
            set { _vendorName = value; }
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

        public string Product
        {
            get { return _product; }
            set { _product = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
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

        public string TelephonePager
        {
            get { return _telephonePager; }
            set { _telephonePager = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string Address1
        {
            get { return _address1; }
            set { _address1 = value; }
        }

        public string Address2
        {
            get { return _address2; }
            set { _address2 = value; }
        }

        #endregion
    }
}
