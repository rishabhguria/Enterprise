namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for CompanyCounterPartyVenue.
    /// </summary>
    public class CompanyCounterPartyVenue
    {
        #region Private and protected members.

        private int _companyCounterPartyCVID = int.MinValue;
        private int _counterPartyVenueID = int.MinValue;
        private int _companyID = int.MinValue;
        private string _counterPartyVenueDisplayName = string.Empty;

        #endregion
        public CompanyCounterPartyVenue()
        {
        }
        public CompanyCounterPartyVenue(int companyCounterPartyCVID, string counterPartyVenueDisplayName)
        {
            _companyCounterPartyCVID = companyCounterPartyCVID;
            _counterPartyVenueDisplayName = counterPartyVenueDisplayName;
        }

        public CompanyCounterPartyVenue(int companyCounterPartyCVID, int counterPartyVenueID, int companyID, string counterPartyVenueDisplayName)
        {
            _companyCounterPartyCVID = companyCounterPartyCVID;
            _counterPartyVenueID = counterPartyVenueID;
            _companyID = companyID;
            _counterPartyVenueDisplayName = counterPartyVenueDisplayName;
        }

        #region Properties
        public int CompanyCounterPartyCVID
        {
            get { return _companyCounterPartyCVID; }
            set { _companyCounterPartyCVID = value; }
        }

        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        #endregion

        public string CounterPartyVenueDisplayName
        {
            get
            {
                return _counterPartyVenueDisplayName;
            }
            set { _counterPartyVenueDisplayName = value; }
        }

        //		public string CounterPartyVenueDisplayName
        //		{
        //			get
        //			{
        //				if(_counterPartyVenueDisplayName == string.Empty)
        //				{
        //					CounterPartyVenue counterPartyVenue = CounterPartyManager.GetCounterPartyVenue(_counterPartyVenueID);
        //					if(counterPartyVenue != null)
        //					{
        //						_counterPartyVenueDisplayName = counterPartyVenue.DisplayName;
        //					}
        //				}
        //				return _counterPartyVenueDisplayName;
        //			}			
        //		}
    }
}
