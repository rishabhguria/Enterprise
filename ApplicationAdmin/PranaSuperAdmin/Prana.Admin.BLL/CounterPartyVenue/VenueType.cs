namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Venue.
    /// </summary>
    public class VenueType
    {
        #region Private and protected members.

        private int _venueTypeID = int.MinValue;
        private string _type = string.Empty;

        #endregion

        public VenueType()
        {
        }

        public VenueType(int venueTypeID, string type)
        {
            _venueTypeID = venueTypeID;
            _type = type;
        }

        #region Properties

        public int VenueTypeID
        {
            get { return _venueTypeID; }
            set { _venueTypeID = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #endregion
    }
}
