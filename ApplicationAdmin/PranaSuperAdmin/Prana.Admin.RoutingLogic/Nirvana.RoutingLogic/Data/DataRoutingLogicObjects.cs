using System;
namespace Prana.Admin.RoutingLogic.BLL
{
    /// <summary>
    /// Summary description for DataRoutingLogic.
    /// </summary>
    public class DataRoutingLogicObjects
    {
        private BLL.DataRoutingLogicRLObject RLogic = new BLL.DataRoutingLogicRLObject();
        private BLL.DataRoutingLogicCompanyGroupObject Group = new BLL.DataRoutingLogicCompanyGroupObject();

        private static BLL.DataRoutingLogicObjects instance;//= new DataRoutingLogicObjects();


        private DataRoutingLogicObjects()
        {
            //
            // TODO: Add constructor logic here
            //


        }



        public static BLL.DataRoutingLogicObjects GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataRoutingLogicObjects();
                }
                return instance;
            }
        }


        private BLL.DataRoutingLogicBaseObject RLGroupRef(string _strRLGroup)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                return this.RLogic;
            }
            else
            {
                return this.Group;
            }
        }

        public int ID(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.ID;
        }
        public int ID(string _strRLGroup, int _iID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.ID = _iID;

            return objRLBase.ID;
        }


        public string Name(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.Name;
        }
        public string Name(string _strRLGroup, string _strName)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.Name = _strName;

            return objRLBase.Name;
        }


        public int ClientCount(string _strRLGroup)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);
            return objGroup.ClientCount;
        }
        public int ClientCount(string _strRLGroup, int _iCount)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);
            objGroup.ClientCount = _iCount;

            return objGroup.ClientCount;
        }



        public int ClientID(string _strRLGroup, int _iClientIndex)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);
            return objGroup.ClientID(_iClientIndex);
        }
        public int ClientID(string _strRLGroup, int _iClientIndex, int _iClientID)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);


            return objGroup.ClientID(_iClientIndex, _iClientID);
        }


        public int ClientApplyRL(string _strRLGroup, int _iClientIndex)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);
            return objGroup.ApplyRLClient(_iClientIndex);
        }
        public int ClientApplyRL(string _strRLGroup, int _iClientIndex, int _iApplyRL)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);

            return objGroup.ApplyRLClient(_iClientIndex, _iApplyRL);
        }


        public int ApplyRL(string _strRLGroup)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);
            return objGroup.ApplyRL;
        }
        public int ApplyRL(string _strRLGroup, int _iApplyRL)
        {
            if (_strRLGroup.Equals("") || _strRLGroup.Equals(null) || _strRLGroup.Equals(System.DBNull.Value))
            {
                throw new Exception(" unable to distinguish b/w  RL and group tab");
            }

            if (_strRLGroup.Trim().Equals("RL"))
            {
                throw new Exception(" RL don't have client List ");
            }

            BLL.DataRoutingLogicCompanyGroupObject objGroup = (BLL.DataRoutingLogicCompanyGroupObject)RLGroupRef(_strRLGroup);

            objGroup.ApplyRL = _iApplyRL;
            return objGroup.ApplyRL;
        }




        public int AssetID(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.AssetID;
        }
        public int AssetID(string _strRLGroup, int _iAssetID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.AssetID = _iAssetID;

            return objRLBase.AssetID;
        }



        public int UnderLyingID(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.UnderLyingID;
        }
        public int UnderLyingID(string _strRLGroup, int _iUnderLyingID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.UnderLyingID = _iUnderLyingID;

            return objRLBase.UnderLyingID;
        }

        public int ExchangeID(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.ExchangeID;
        }
        public int ExchangeID(string _strRLGroup, int _iExchangeID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.ExchangeID = _iExchangeID;

            return objRLBase.ExchangeID;
        }

        public int AUECID(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.AUECID;
        }
        public int AUECID(string _strRLGroup, int _iAUECID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.AUECID = _iAUECID;

            return objRLBase.AUECID;
        }






        public int RoutingPathCount(string _strRLGroup)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.RoutingPathCount;
        }
        public int RoutingPathCount(string _strRLGroup, int _iRoutingPathCount)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            objRLBase.RoutingPathCount = _iRoutingPathCount;

            return objRLBase.RoutingPathCount;
        }

        public int RoutingPathID(string _strRLGroup, int _iRoutingPathIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.RoutingPathID(_iRoutingPathIndex);
        }
        public int RoutingPathID(string _strRLGroup, int _iRoutingPathIndex, int _iRoutingPathID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.RoutingPathID(_iRoutingPathIndex, _iRoutingPathID);
        }

        public string RoutingPathName(string _strRLGroup, int _iRoutingPathIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.RoutingPathName(_iRoutingPathIndex);
        }
        public string RoutingPathName(string _strRLGroup, int _iRoutingPathIndex, string _strRoutingPathName)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.RoutingPathName(_iRoutingPathIndex, _strRoutingPathName);
        }



        public int TradingAccountCount(string _strRLGroup, int _iRoutingPathIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.TradingAccountCount(_iRoutingPathIndex);
        }
        public int TradingAccountCount(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCount)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.TradingAccountCount(_iRoutingPathIndex, _iTradingAccountCount);
        }



        public int TradingAccountID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.TradingAccountID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex);
        }
        public int TradingAccountID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iTradingAccountID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.TradingAccountID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex, _iTradingAccountID);
        }



        public int TradingAccountIDDefault(string _strRLGroup, int _iRoutingPathIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.TradingAccountIDDefault(_iRoutingPathIndex);
        }
        public int TradingAccountIDDefault(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.TradingAccountIDDefault(_iRoutingPathIndex, _iTradingAccountID);
        }



        public int CounterPartyID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.CounterPartyID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex);
        }
        public int CounterPartyID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iCounterPartyID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.CounterPartyID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex, _iCounterPartyID);
        }



        public int VenueID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.VenueID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex);
        }
        public int VenueID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iVenueID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.VenueID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex, _iVenueID);
        }




        public int CounterPartyVenueID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.CounterPartyVenueID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex);
        }
        public int CounterPartyVenueID(string _strRLGroup, int _iRoutingPathIndex, int _iTradingAccountCPVenueIndex, int _iCounterPartyVenueID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.CounterPartyVenueID(_iRoutingPathIndex, _iTradingAccountCPVenueIndex, _iCounterPartyVenueID);
        }





        public int ConditionsCount(string _strRLGroup, int _iRoutingPathIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.ConditionsCount(_iRoutingPathIndex);
        }
        public int ConditionsCount(string _strRLGroup, int _iRoutingPathIndex, int _iCount)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.ConditionsCount(_iRoutingPathIndex, _iCount);
        }



        public int ParameterID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.ParameterID(_iRoutingPathIndex, _iConditionIndex);
        }
        public int ParameterID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex, int _iParameterID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.ParameterID(_iRoutingPathIndex, _iConditionIndex, _iParameterID);
        }




        public string ParameterValue(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.ParameterValue(_iRoutingPathIndex, _iConditionIndex);
        }
        public string ParameterValue(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex, string _strParameterValue)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.ParameterValue(_iRoutingPathIndex, _iConditionIndex, _strParameterValue);
        }




        public int JoinConditonID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.JoinConditonID(_iRoutingPathIndex, _iConditionIndex);
        }
        public int JoinConditonID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex, int _iJoinConditonID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.JoinConditonID(_iRoutingPathIndex, _iConditionIndex, _iJoinConditonID);
        }



        public int OperatorID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);
            return objRLBase.OperatorID(_iRoutingPathIndex, _iConditionIndex);
        }
        public int OperatorID(string _strRLGroup, int _iRoutingPathIndex, int _iConditionIndex, int _iOperatorID)
        {
            BLL.DataRoutingLogicBaseObject objRLBase = RLGroupRef(_strRLGroup);

            return objRLBase.OperatorID(_iRoutingPathIndex, _iConditionIndex, _iOperatorID);
        }





    }
}
