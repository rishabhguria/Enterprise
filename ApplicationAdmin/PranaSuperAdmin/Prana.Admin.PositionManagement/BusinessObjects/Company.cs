using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// Summary description for Company.
    /// </summary>
    public class Company
    {
        //TODO: To allocate regions for the respective tabs.

        #region Private members and Properties

        private CompanyNameID _companyNameID;

        public CompanyNameID CompanyNameID
        {
            get 
            {
                if (_companyNameID == null)
                {
                    _companyNameID = new CompanyNameID();
                }

                return _companyNameID;
            }
            set { _companyNameID = value; }
        }


        private CompanyType _companyType;

        public CompanyType CompanyType
        {
            get 
            { 
                
                if (_companyType == null)
                {
                    _companyType = new CompanyType();
                }

                return _companyType;
            }
                
            set { _companyType = value; }
        }

        private AddressDetails _addressDetails;

        public AddressDetails AddressDetails
        {
            get 
            {
                if (_addressDetails == null)
                {
                    _addressDetails = new  AddressDetails();
                }


                return _addressDetails; 
            }
            set { _addressDetails = value; }
        }

        private int _numberOfUserLicences;

        public int NumberOfUserLicences
        {
            get { return _numberOfUserLicences; }
            set { _numberOfUserLicences = value; }
        }

        private string _adminFirstName;

        public string AdminFirstName
        {
            get { return _adminFirstName; }
            set { _adminFirstName = value; }
        }

        private string _adminLastName;

        public string AdminLastName
        {
            get { return _adminLastName; }
            set { _adminLastName = value; }
        }

        private string _adminTitle;

        public string AdminTitle
        {
            get { return _adminTitle; }
            set { _adminTitle = value; }
        }

        private User _user;

        public User AdminUser
        {
            get 
            {
                if (_user == null)
                {
                    _user = new User();

                }

                return _user; 
            }
            set { _user = value; }
        }

        private string _adminEmail;

        public string AdminEmail
        {
            get { return _adminEmail; }
            set { _adminEmail = value; }
        }

        private string _adminWorkNumber;

        public string AdminWorkNumber
        {
            get { return _adminWorkNumber; }
            set { _adminWorkNumber = value; }
        }

        private string _adminCellNumber;

        public string AdminCellNumber
        {
            get { return _adminCellNumber; }
            set { _adminCellNumber = value; }
        }

        private string _adminPagerNumber;

        public string AdminPagerNumber
        {
            get { return _adminPagerNumber; }
            set { _adminPagerNumber = value; }
        }

        private string _adminHomeNumber;

	    public string AdminHomeNumber
	    {
		    get { return _adminHomeNumber;}
		    set { _adminHomeNumber = value;}
	    }
	
	    private string _adminFaxNumber;

	    public string AdminFaxNumber
	    {
		    get { return _adminFaxNumber;}
		    set { _adminFaxNumber = value;}
	    }	
	
        

        #endregion
    }

}
