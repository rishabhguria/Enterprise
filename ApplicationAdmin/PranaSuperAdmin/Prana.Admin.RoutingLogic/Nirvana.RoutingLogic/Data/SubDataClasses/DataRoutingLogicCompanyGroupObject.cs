using System;
namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for DataClient.
    /// </summary>
    public class DataRoutingLogicCompanyGroupObject : BLL.DataRoutingLogicBaseObject
    {
        private int iApplyRL = 1;

        private System.Collections.ArrayList alClient = new System.Collections.ArrayList();

        public DataRoutingLogicCompanyGroupObject()
        {
            //
            // TODO: Add constructor logic here
            //

        }


        public int ApplyRL
        {
            get
            {
                return this.iApplyRL;
            }
            set
            {
                if (value < 0 || value >= 2 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = 2;
                }
                this.iApplyRL = value;

            }

        }


        public int ClientCount
        {
            get
            {
                return this.alClient.Count;
            }

            set
            {

                if (value < 0)
                {
                    throw new Exception("Count can't be negative");
                }

                if (this.alClient.Count < value)
                {
                    for (int i = (value - alClient.Count); i > 0; i--)
                    {
                        this.alClient.Add(new BLL.DataRoutingLogicCompanyClientObject());
                    }

                }
                else if (this.alClient.Count > value)
                {
                    this.alClient.RemoveRange(value, this.alClient.Count - value);
                }
            }
        }



        public int ApplyRLClient(int _iClientIndex)
        {
            if (_iClientIndex < 0 || _iClientIndex > this.alClient.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicCompanyClientObject)(this.alClient[_iClientIndex])).ApplyRL;
        }


        public int ApplyRLClient(int _iClientIndex, int _iApplyRL)
        {
            if (_iClientIndex < 0 || _iClientIndex > this.alClient.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicCompanyClientObject)(this.alClient[_iClientIndex])).ApplyRL = _iApplyRL;
            return ((BLL.DataRoutingLogicCompanyClientObject)(this.alClient[_iClientIndex])).ApplyRL;

        }



        public int ClientID(int _iClientIndex)
        {
            if (_iClientIndex < 0 || _iClientIndex > alClient.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicCompanyClientObject)(alClient[_iClientIndex])).ClientID;
        }


        public int ClientID(int _iClientIndex, int _iClientID)
        {
            if (_iClientIndex < 0 || _iClientIndex > alClient.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicCompanyClientObject)(alClient[_iClientIndex])).ClientID = _iClientID;
            return ((BLL.DataRoutingLogicCompanyClientObject)(alClient[_iClientIndex])).ClientID;
        }


    }
}
