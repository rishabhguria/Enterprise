namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CounterPartyType.
    /// </summary>
    public class CounterPartyType
    {
        #region Private and protected members.

        private int _counterPartyTypeID = int.MinValue;
        private string _type = string.Empty;

        #endregion

        public CounterPartyType()
        {
        }

        public CounterPartyType(int counterPartyTypeID, string type)
        {
            _counterPartyTypeID = counterPartyTypeID;
            _type = type;
        }

        #region Properties

        public int CounterPartyTypeID
        {
            get { return _counterPartyTypeID; }
            set { _counterPartyTypeID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion
    }
}