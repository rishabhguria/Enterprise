using Csla;
using System;

namespace Prana.PM.BLL
{
    /// <summary>
    /// The class containing the CompanyTypeID and CompanyTypeName
    /// </summary>
    [Serializable()]
    public class CompanyType : BusinessBase<CompanyType>
    {
        #region Private and protected members.

        private int _companyTypeID;
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
            set
            {
                _companyTypeID = value;
                PropertyHasChanged();
            }

        }

        public string Type
        {
            get { return _companyType; }
            set
            {
                _companyType = value;
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

        //private int _id;
        /// <summary>
        /// TODO : Can't afford to have this ID property in multiuer environment
        /// </summary>
        /// <returns></returns>
        protected override object GetIdValue()
        {
            return _companyTypeID;
        }

        #endregion
    }
}
