namespace Prana.PM.BLL
{
    public class ReconDataSource
    {
        public ReconDataSource()
        {
        }

        private int _thirdPartyID = int.MinValue;

        public int ThirdPartyID
        {
            get { return _thirdPartyID; }
            set { _thirdPartyID = value; }
        }

        private string _dataSourceName = string.Empty;

        public string DataSourceName
        {
            get { return _dataSourceName; }
            set { _dataSourceName = value; }
        }

        private string _dataSourceShortName = string.Empty;

        public string DataSourceShortName
        {
            get { return _dataSourceShortName; }
            set { _dataSourceShortName = value; }
        }


        public override string ToString()
        {
            return _dataSourceName;
        }

    }
}
