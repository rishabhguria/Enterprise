using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class Venue
    {
        int _venueID = int.MinValue;
        string _name = string.Empty;

        public Venue()
        {
        }

        public Venue(int venuID, string venueName)
        {
            _venueID = venuID;
            _name = venueName;
        }

        public int VenueID
        {
            get { return _venueID; }
            set { _venueID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
