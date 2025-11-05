namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Venue.
    /// </summary>
    public class Venue
    {
        #region Private and protected members.

        private int _venueID = int.MinValue;
        private string _venueName = string.Empty;
        private string _route = string.Empty;
        private int _venueTypeID = int.MinValue;
        private int _exchangeID = int.MinValue;

        #endregion

        public Venue()
        {
        }

        public Venue(int venueID, string venueName)
        {
            _venueID = venueID;
            _venueName = venueName;
        }

        #region Properties

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public string VenueName
        {
            get { return _venueName; }
            set { _venueName = value; }
        }

        public string Route
        {
            get { return _route; }
            set { _route = value; }
        }

        public int VenueTypeID
        {
            get { return _venueTypeID; }
            set { _venueTypeID = value; }
        }

        public int ExchangeID
        {
            get { return _exchangeID; }
            set { _exchangeID = value; }
        }

        #endregion
    }
}
