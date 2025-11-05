namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for AUEC.
    /// </summary>
    public class DataRoutingLogicAUECObject
    {

        private int iAssetID = 0;
        private int iUnderLyingID = 0;
        private int iExchangeID = 0;
        private int iAUECID = -1;

        public DataRoutingLogicAUECObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int AssetID
        {
            get
            {
                return iAssetID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iAssetID = value;
            }
        }


        public int UnderLyingID
        {
            get
            {
                return iUnderLyingID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iUnderLyingID = value;
            }
        }


        public int ExchangeID
        {
            get
            {
                return iExchangeID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iExchangeID = value;
            }
        }

        public int AUECID
        {
            get
            {
                return iAUECID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iAUECID = value;
            }
        }

    }

}

