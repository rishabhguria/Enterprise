using System;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// Summary description for ThirdPartyType.
    /// </summary>
    [Serializable]
    public class ThirdPartyType
    {
        int _thirdPartyTypeID = int.MinValue;
        string _thirdPartyTypeName = string.Empty;

        #region Constructors
        public ThirdPartyType()
        {
        }
        public ThirdPartyType(int thirdPartyTypeID, string thirdPartyTypeName)
        {
            _thirdPartyTypeID = thirdPartyTypeID;
            _thirdPartyTypeName = thirdPartyTypeName;
        }
        #endregion

        #region Properties
        public int ThirdPartyTypeID
        {
            get { return _thirdPartyTypeID; }
            set { _thirdPartyTypeID = value; }
        }
        public string ThirdPartyTypeName
        {
            get { return _thirdPartyTypeName; }
            set { _thirdPartyTypeName = value; }
        }

        #endregion
    }
}
