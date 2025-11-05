using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ThirdPartyFlatFileSaveDetail
    {
        private int _companyThirdPartySaveDetailID = int.MinValue;
        private int _companyThirdPartyID = int.MinValue;
        private string _companyIdentifier = string.Empty;
        private string _saveGeneratedFileIn = string.Empty;
        private string _namingConvention = string.Empty;

        public ThirdPartyFlatFileSaveDetail()
        {

        }

        public int CompanyThirdPartySaveDetailID
        {
            get
            {
                return _companyThirdPartySaveDetailID;
            }

            set
            {
                _companyThirdPartySaveDetailID = value;
            }
        }

        public int CompanyThirdPartyID
        {
            get
            {
                return _companyThirdPartyID;
            }

            set
            {
                _companyThirdPartyID = value;
            }
        }

        public string CompanyIdentifier
        {
            get
            {
                return _companyIdentifier;
            }

            set
            {
                _companyIdentifier = value;
            }
        }
        public string SaveGeneratedFileIn
        {
            get
            {
                return _saveGeneratedFileIn;
            }

            set
            {
                _saveGeneratedFileIn = value;
            }
        }
        public string NamingConvention
        {
            get
            {
                return _namingConvention;
            }

            set
            {
                _namingConvention = value;
            }
        }

    }
}
