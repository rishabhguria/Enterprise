using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    public class AddressDetails
    {
        #region Private Members
        private string _address1 = string.Empty;
        private string _address2 = string.Empty;
        private int _countryId = int.MinValue;
        private int _stateId = int.MinValue;
        private string _zip = string.Empty;
        private string _workNumber = string.Empty;
        private string _faxNumber = string.Empty; 
        #endregion

        #region Public Properties
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

        public int CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }

        public int StateId
        {
            get { return _stateId; }
            set { _stateId = value; }
        }

        public string Zip
        {
            get { return _zip; }
            set { _zip = value; }
        }

        public string WorkNumber
        {
            get { return _workNumber; }
            set { _workNumber = value; }
        }

        public string FaxNumber
        {
            get { return _faxNumber; }
            set { _faxNumber = value; }
        } 
        #endregion
	
    }
}
