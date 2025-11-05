namespace Prana.Admin.BLL
{
    public class Calendar
    {
        public Calendar()
        {
        }

        private string _calendar;

        public string CalendarName
        {
            get { return _calendar; }
            set { _calendar = value; }
        }

        private int _calendarID;

        public int CalendarID
        {
            get { return _calendarID; }
            set { _calendarID = value; }
        }
        private int _calendarYear;

        public int CalendarYear
        {
            get { return _calendarYear; }
            set { _calendarYear = value; }
        }

        public Calendar(int calendarID, string calendarName, int calendarYear)
        {
            _calendarID = calendarID;
            _calendar = calendarName;
            _calendarYear = calendarYear;
        }
    }
}
