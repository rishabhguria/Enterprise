namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for ContractListingType.
    /// </summary>
    public class ContractListingType
    {
        #region Private members

        private int _contractListingTypeID = int.MinValue;
        private string _type = string.Empty;

        #endregion

        public ContractListingType()
        {
        }
        public ContractListingType(int contractListingTypeID, string type)
        {
            _contractListingTypeID = contractListingTypeID;
            _type = type;
        }

        #region Properties

        public int ContractListingTypeID
        {
            get { return _contractListingTypeID; }
            set { _contractListingTypeID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion

    }
}
