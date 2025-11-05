using System;
using System.Collections.Generic;
using System.Text;

namespace Nirvana.Admin.PositionManagement.BusinessObjects
{
    /// <summary>
    /// The class containing the CompanyTypeID and CompanyTypeName
    /// </summary>
    public class CompanyType
    {
        #region Private and protected members.

        private int _companyTypeID = 0;
        private string _companyType = string.Empty;

        #endregion

        public CompanyType()
        {
        }

        public CompanyType(int companyTypeID, string companyType)
        {
            _companyTypeID = companyTypeID;
            _companyType = companyType;
        }

        #region Properties

        public int CompanyTypeID
        {
            get { return _companyTypeID; }
            set { _companyTypeID = value; }
        }

        public string Type
        {
            get { return _companyType; }
            set { _companyType = value; }
        }

        #endregion
    }
}
