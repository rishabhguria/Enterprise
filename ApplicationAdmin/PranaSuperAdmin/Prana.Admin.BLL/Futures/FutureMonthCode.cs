namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for FutureMonthCode.
    /// </summary>
    public class FutureMonthCode
    {
        #region Private members

        private int _futureMonthCodeID = int.MinValue;
        private string _futureMonth = string.Empty;
        private string _abbreviation = string.Empty;

        #endregion

        public FutureMonthCode()
        {
        }
        public FutureMonthCode(int futureMonthCodeID, string futureMonth, string abbreviation)
        {
            _futureMonthCodeID = futureMonthCodeID;
            _futureMonth = futureMonth;
            _abbreviation = abbreviation;
        }

        #region Properties

        public int FutureMonthCodeID
        {
            get { return _futureMonthCodeID; }
            set { _futureMonthCodeID = value; }
        }

        public string FutureMonth
        {
            get { return _futureMonth; }
            set { _futureMonth = value; }
        }

        public string Abbreviation
        {
            get { return _abbreviation; }
            set { _abbreviation = value; }
        }

        #endregion
    }
}
