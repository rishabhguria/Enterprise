namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for SymbolConvention.
    /// </summary>
    public class SymbolConvention
    {
        int _symbolConventionID = int.MinValue;
        string _symboConventionName = string.Empty;
        string _shortName = string.Empty;
        int _counterPartyVenueID = int.MinValue;

        public SymbolConvention()
        {
        }

        public SymbolConvention(int symbolConventionID, string symbolConventionName, string shortName)
        {
            _symbolConventionID = symbolConventionID;
            _symboConventionName = symbolConventionName;
            _shortName = shortName;
        }

        public int SymbolConventionID
        {
            get
            {
                return _symbolConventionID;
            }

            set
            {
                _symbolConventionID = value;
            }
        }


        public string SymbolConventionName
        {
            get
            {
                return _symboConventionName;
            }

            set
            {
                _symboConventionName = value;
            }
        }

        public string ShortName
        {
            get
            {
                return _shortName;
            }

            set
            {
                _shortName = value;
            }
        }
        public int CounterPartyVenueID
        {
            get { return _counterPartyVenueID; }
            set { _counterPartyVenueID = value; }
        }
    }
}
