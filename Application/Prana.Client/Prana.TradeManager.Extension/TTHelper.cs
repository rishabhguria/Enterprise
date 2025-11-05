namespace Prana.TradeManager.Extension
{
    /// <summary>
    /// Summary description for TradingTicketHelper.
    /// </summary>
    public class TTHelper
    {
        # region Private Fields
        private int _auecID = int.MinValue;
        private int _assetID = int.MinValue;
        private int _underlyingID = int.MinValue;

        private int _counterpartyID = int.MinValue;
        private int _venueID = int.MinValue;
        private string _underlyingName = string.Empty;
        #endregion

        #region Public Properties
        public int UnderlyingID
        {
            get
            {
                return this._underlyingID;
            }

            set
            {
                this._underlyingID = value;
            }
        }

        public string UnderlyingName
        {
            get
            {
                return this._underlyingName;
            }

            set
            {
                this._underlyingName = value;
            }
        }
        public int CounterpartyID
        {
            get
            {
                return this._counterpartyID;
            }

            set
            {
                this._counterpartyID = value;
            }
        }
        public int VenueID
        {
            get
            {
                return this._venueID;
            }

            set
            {
                this._venueID = value;
            }
        }
        public int AuecID
        {
            get
            {
                return this._auecID;
            }

            set
            {
                this._auecID = value;
            }
        }
        public int AssetID
        {
            get
            {
                return this._assetID;
            }

            set
            {
                this._assetID = value;
            }
        }
        #endregion


        public TTHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }
}
