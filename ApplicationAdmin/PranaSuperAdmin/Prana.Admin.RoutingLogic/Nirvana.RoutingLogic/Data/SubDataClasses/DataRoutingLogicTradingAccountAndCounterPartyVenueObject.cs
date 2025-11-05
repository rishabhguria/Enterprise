namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for TradingAccountAndCounterPartyVenue.
    /// </summary>
    public class DataRoutingLogicTradingAccountAndCounterPartyVenueObject
    {
        private int iTradingAccountID = -1;
        private int iCounterPartyVenueID = -1;
        private int iCounterPartyID = -1;
        private int iVenueID = -1;

        public DataRoutingLogicTradingAccountAndCounterPartyVenueObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region properties

        public int TradingAccountID
        {
            get
            {
                return iTradingAccountID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iTradingAccountID = value;
            }
        }



        public int CounterPartyVenueID
        {
            get
            {
                return iCounterPartyVenueID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iCounterPartyVenueID = value;
            }
        }

        public int CounterPartyID
        {
            get
            {
                return iCounterPartyID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iCounterPartyID = value;
            }
        }

        public int VenueID
        {
            get
            {
                return iVenueID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iVenueID = value;
            }
        }

        #endregion



    }
}
