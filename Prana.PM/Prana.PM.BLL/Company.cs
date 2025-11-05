using Csla;
using Prana.BusinessObjects.PositionManagement;
using System;

namespace Prana.PM.BLL
{
    /// <summary>
    /// Summary description for Company.
    /// </summary>
    [Serializable()]
    public class Company : BusinessBase<Company>
    {
        //TODO: To allocate regions for the respective tabs.
        public Company()
        {
            MarkAsChild();
        }

        #region Private members and Properties

        private CompanyNameID _companyNameID = new CompanyNameID();

        public CompanyNameID CompanyNameID
        {
            get
            {
                return _companyNameID;
            }
            set
            {
                _companyNameID = value;
                PropertyHasChanged();
            }
        }


        private CompanyType _companyType = new CompanyType();

        public CompanyType CompanyType
        {
            get
            {
                return _companyType;
            }

            set
            {
                _companyType = value;
                PropertyHasChanged();
            }
        }

        private AddressDetails _addressDetails = new AddressDetails();

        public AddressDetails AddressDetails
        {
            get
            {
                return _addressDetails;
            }
            set
            {
                _addressDetails = value;
                PropertyHasChanged();
            }
        }

        private int _numberOfUserLicences;

        public int NumberOfUserLicences
        {
            get { return _numberOfUserLicences; }
            set
            {
                _numberOfUserLicences = value;
                PropertyHasChanged();
            }
        }



        private User _user = new User();

        public User AdminUser
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                PropertyHasChanged();
            }
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
            return _companyNameID.ID;
        }

        #endregion
    }

}
