using System;

namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for IfThenCondition.
    /// </summary>
    public class DataRoutingLogicRoutingPathObject
    {
        private int iRLID = -1;
        private int iTradingAccountDefaultID = -1;
        private string strRLName = "New";
        private System.Collections.ArrayList alConditions = new System.Collections.ArrayList();
        private System.Collections.ArrayList alTCPV = new System.Collections.ArrayList();



        public DataRoutingLogicRoutingPathObject()
        {
            //
            // TODO: Add constructor logic here
            //
            alConditions.Add(new BLL.DataRoutingLogicConditionAndJoinObject());
            alTCPV.Add(new BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject());
        }

        #region properties

        public string RoutingPathName
        {
            get
            {
                return strRLName;
            }
            set
            {
                if (value == null || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = "";
                }

                strRLName = value;
            }

        }

        public int RoutingPathID
        {
            get
            {
                return iRLID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iRLID = value;
            }

        }


        #region trading account
        public int TradingAccountCPVenueCount
        {
            get
            {
                return alTCPV.Count;
            }

            set
            {

                if (value < 0)
                {
                    throw new Exception("Count can't be negative");
                }

                if (alTCPV.Count < value)
                {
                    for (int i = (value - alTCPV.Count); i > 0; i--)
                    {
                        alTCPV.Add(new BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject());
                    }

                }
                else if (alTCPV.Count > value)
                {
                    alTCPV.RemoveRange(value, alTCPV.Count - value);
                }
            }

        }


        public int TradingAccountDefaultID
        {
            get
            {
                return iTradingAccountDefaultID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iTradingAccountDefaultID = value;
            }

        }


        public int TradingAccountID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).TradingAccountID;
        }


        public int TradingAccountID(int _iIndex, int _iTradingAccountID)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).TradingAccountID = _iTradingAccountID;

            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).TradingAccountID;
        }


        public int CounterPartyID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyID;
        }


        public int CounterPartyID(int _iIndex, int _iCounterPartyID)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyID = _iCounterPartyID;

            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyID;
        }


        public int VenueID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).VenueID;
        }


        public int VenueID(int _iIndex, int _iVenueID)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).VenueID = _iVenueID;

            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).VenueID;
        }


        public int CounterPartyVenueID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyVenueID;
        }


        public int CounterPartyVenueID(int _iIndex, int _iCounterPartyVenueID)
        {
            if (_iIndex < 0 || _iIndex > alTCPV.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyVenueID = _iCounterPartyVenueID;

            return ((BLL.DataRoutingLogicTradingAccountAndCounterPartyVenueObject)(alTCPV[_iIndex])).CounterPartyVenueID;
        }



        #endregion

        #region conditons

        public int ConditionsCount
        {
            get
            {
                return alConditions.Count;
            }

            set
            {

                if (value < 0)
                {
                    throw new Exception("Count can't be negative");
                }

                if (alConditions.Count < value)
                {
                    for (int i = (value - alConditions.Count); i > 0; i--)
                    {
                        alConditions.Add(new BLL.DataRoutingLogicConditionAndJoinObject());
                    }

                }
                else if (alConditions.Count > value)
                {
                    alConditions.RemoveRange(value, alConditions.Count - value);
                }
            }

        }


        /// <summary>
        /// get the parameter Id for the particular index of conditon criterion
        /// </summary>
        /// <param name="_iIndex"></param>
        /// <returns></returns>
        public int ParameterID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterID;
        }

        /// <summary>
        /// Set the Parameter Id for a particular index of contions criteria
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="ParameterID"></param>
        /// <returns></returns>
        public int ParameterID(int _iIndex, int _iParameterID)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterID = _iParameterID;

            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterID;
        }


        public string ParameterValue(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterValue;
        }


        public string ParameterValue(int _iIndex, string _strParameterValue)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterValue = _strParameterValue;

            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).ParameterValue;
        }

        public int JoinConditonID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).JoinConditonID;
        }


        public int JoinConditonID(int _iIndex, int _iJoinConditonID)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).JoinConditonID = _iJoinConditonID;

            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).JoinConditonID;
        }


        public int OperatorID(int _iIndex)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).OperatorID;
        }


        public int OperatorID(int _iIndex, int _iOperatorID)
        {
            if (_iIndex < 0 || _iIndex > alConditions.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).OperatorID = _iOperatorID;

            return ((BLL.DataRoutingLogicConditionAndJoinObject)(alConditions[_iIndex])).OperatorID;
        }





        #endregion

        #endregion























    }
}
