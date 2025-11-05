namespace Prana.BusinessObjects
{
    public class PMUIPrefs
    {
        private int _numberOfCustomViewsAllowed = 5;

        public int NumberOfCustomViewsAllowed
        {
            get { return _numberOfCustomViewsAllowed; }
            set { _numberOfCustomViewsAllowed = value; }
        }

        private int _numberOfVisibleColumnsAllowed = 14;
        public int NumberOfVisibleColumnsAllowed
        {
            get { return _numberOfVisibleColumnsAllowed; }
            set { _numberOfVisibleColumnsAllowed = value; }
        }

        private bool _fetchDataFromHistoricalDb = false;
        public bool FetchDataFromHistoricalDb
        {
            get { return _fetchDataFromHistoricalDb; }
            set { _fetchDataFromHistoricalDb = value; }
        }

    }
}
