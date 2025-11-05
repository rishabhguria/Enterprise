namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for ConditionAndJoin.
    /// </summary>
    public class DataRoutingLogicConditionAndJoinObject
    {



        private int iParameterID = -1;
        private string strParameterValue = "";
        private int iJoinConditonID = 0;
        private int iOperatorID = 0;

        public DataRoutingLogicConditionAndJoinObject()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region properties

        public int ParameterID
        {
            get
            {
                return iParameterID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iParameterID = value;
            }
        }



        public string ParameterValue
        {
            get
            {
                return strParameterValue;
            }
            set
            {
                if (value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = "";
                }

                strParameterValue = value;
            }
        }

        public int JoinConditonID
        {
            get
            {
                return iJoinConditonID;
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

                iJoinConditonID = value;
            }
        }

        public int OperatorID
        {
            get
            {
                return iOperatorID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = 0;
                }

                iOperatorID = value;
            }
        }

        #endregion


    }
}
