namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for Client.
    /// </summary>
    public class DataRoutingLogicCompanyClientObject
    {

        private int iClientID = -1;
        private int iApplyRL = 0;

        public DataRoutingLogicCompanyClientObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        public int ApplyRL
        {
            get
            {
                return iApplyRL;
            }
            set
            {
                if (value <= 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = 0;
                }
                else
                {
                    value = 1;
                }

                iApplyRL = value;
            }
        }

        public int ClientID
        {
            get
            {
                return iClientID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }
                iClientID = value;
            }



        }
    }
}
