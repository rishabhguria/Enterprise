using System;
namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for DataRL.
    /// </summary>
    public class DataRoutingLogicBaseObject
    {
        private System.Collections.ArrayList alRoutingPath = new System.Collections.ArrayList();
        private string strName = "New";
        private int iID = -1;
        private DataRoutingLogicAUECObject AUEC = new DataRoutingLogicAUECObject();



        public DataRoutingLogicBaseObject()
        {
            //
            // TODO: Add constructor logic here
            //

            alRoutingPath.Add(new BLL.DataRoutingLogicRoutingPathObject());
        }

        public int ID
        {
            get
            {
                return iID;
            }
            set
            {
                if (value < 0 || value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = -1;
                }

                iID = value;
            }
        }

        public string Name
        {
            get
            {
                return strName;
            }
            set
            {
                if (value.Equals(null) || value.Equals(System.DBNull.Value))
                {
                    value = "";
                }

                strName = value;
            }
        }

        public int AssetID
        {
            get
            {
                return AUEC.AssetID;
            }
            set
            {
                AUEC.AssetID = value;
            }
        }


        public int UnderLyingID
        {
            get
            {
                return AUEC.UnderLyingID;
            }
            set
            {
                AUEC.UnderLyingID = value;
            }
        }


        public int ExchangeID
        {
            get
            {
                return AUEC.ExchangeID;
            }
            set
            {
                AUEC.ExchangeID = value;
            }
        }

        public int AUECID
        {
            get
            {
                return AUEC.AUECID;
            }
            set
            {
                AUEC.AUECID = value;
            }
        }



        public int RoutingPathCount
        {
            get
            {
                return alRoutingPath.Count;
            }

            set
            {

                if (value < 0)
                {
                    throw new Exception("Count can't be negative");
                }

                if (alRoutingPath.Count < value)
                {
                    for (int i = (value - alRoutingPath.Count); i > 0; i--)
                    {
                        alRoutingPath.Add(new BLL.DataRoutingLogicRoutingPathObject());
                    }

                }
                else if (alRoutingPath.Count > value)
                {
                    alRoutingPath.RemoveRange(value, alRoutingPath.Count - value);
                }
            }

        }


        public int RoutingPathID(int _iRoutingPathIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathID;
        }


        public int RoutingPathID(int _iRoutingPathIndex, int _iRoutingPathID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathID = _iRoutingPathID;
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathID;
        }

        public string RoutingPathName(int _iRoutingPathIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathName;
        }


        public string RoutingPathName(int _iRoutingPathIndex, string _strRoutingPathName)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathName = _strRoutingPathName;
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).RoutingPathName;
        }


        public int TradingAccountCount(int _iRoutingPathIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountCPVenueCount;
        }
        public int TradingAccountCount(int _iRoutingPathIndex, int _iCount)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountCPVenueCount = _iCount;
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountCPVenueCount;
        }




        public int TradingAccountID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountID(_iTradingAccountCPVenueIndex);
        }


        public int TradingAccountID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iTradingAccountID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountID(_iTradingAccountCPVenueIndex, _iTradingAccountID);
        }

        public int TradingAccountIDDefault(int _iRoutingPathIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountDefaultID;
        }


        public int TradingAccountIDDefault(int _iRoutingPathIndex, int _iTradingAccountIDDefault)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountDefaultID = _iTradingAccountIDDefault;
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).TradingAccountDefaultID;
        }


        public int CounterPartyID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).CounterPartyID(_iTradingAccountCPVenueIndex);
        }


        public int CounterPartyID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iCounterPartyID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).CounterPartyID(_iTradingAccountCPVenueIndex, _iCounterPartyID);
        }



        public int VenueID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).VenueID(_iTradingAccountCPVenueIndex);
        }


        public int VenueID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iVenueID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).VenueID(_iTradingAccountCPVenueIndex, _iVenueID);
        }


        public int CounterPartyVenueID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).CounterPartyVenueID(_iTradingAccountCPVenueIndex);
        }


        public int CounterPartyVenueID(int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iCounterPartyVenueID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).CounterPartyVenueID(_iTradingAccountCPVenueIndex, _iCounterPartyVenueID);
        }




        public int ConditionsCount(int _iRoutingPathIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ConditionsCount;
        }
        public int ConditionsCount(int _iRoutingPathIndex, int _iCount)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ConditionsCount = _iCount;
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ConditionsCount;
        }




        public int ParameterID(int _iRoutingPathIndex, int _iConditionIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ParameterID(_iConditionIndex);
        }


        public int ParameterID(int _iRoutingPathIndex, int _iConditionIndex, int _iParameterID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ParameterID(_iConditionIndex, _iParameterID);
        }


        public string ParameterValue(int _iRoutingPathIndex, int _iConditionIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ParameterValue(_iConditionIndex);
        }


        public string ParameterValue(int _iRoutingPathIndex, int _iConditionIndex, string _strParameterValue)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).ParameterValue(_iConditionIndex, _strParameterValue);
        }



        public int JoinConditonID(int _iRoutingPathIndex, int _iConditionIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).JoinConditonID(_iConditionIndex);
        }


        public int JoinConditonID(int _iRoutingPathIndex, int _iConditionIndex, int _iJoinConditonID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).JoinConditonID(_iConditionIndex, _iJoinConditonID);
        }


        public int OperatorID(int _iRoutingPathIndex, int _iConditionIndex)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }
            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).OperatorID(_iConditionIndex);
        }


        public int OperatorID(int _iRoutingPathIndex, int _iConditionIndex, int _iOperatorID)
        {
            if (_iRoutingPathIndex < 0 || _iRoutingPathIndex > alRoutingPath.Count - 1)
            {
                throw new Exception("Index can't be negative or more than the Count");
            }

            return ((BLL.DataRoutingLogicRoutingPathObject)(alRoutingPath[_iRoutingPathIndex])).OperatorID(_iConditionIndex, _iOperatorID);
        }


    }
}
