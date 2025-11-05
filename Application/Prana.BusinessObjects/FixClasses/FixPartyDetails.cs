using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class FixPartyDetails
    {
        SerializableDictionary<string, string> _venueDetails = new SerializableDictionary<string, string>();
        SerializableDictionary<string, SerializableDictionary<string, string>> _assetWiseDictionary = new SerializableDictionary<string, SerializableDictionary<string, string>>();

        public FixPartyDetails(int partyID, string partyName, string SenderCompID, string TargetCompID, int Port, string targetSubID, string hostName, int originatorType, int brokerConnectionType, string fixDllAdapterName, DateTime resetTime)
        {
            _partyID = partyID;
            _senderCompID = SenderCompID;
            _targetCompID = TargetCompID;
            _port = Port;
            _partyName = partyName;
            _targetSubID = targetSubID;
            _hostName = hostName;
            _originatorType = (PranaServerConstants.OriginatorType)originatorType;
            _brokerConnectionType = (PranaServerConstants.BrokerConnectionType)brokerConnectionType;
            _dllName = fixDllAdapterName;
            _resetTime = resetTime;
            _connectionID = Convert.ToInt32("9" + originatorType + "" + partyID);
        }

        #region Public Properties
        int _connectionID;
        public int ConnectionID
        {
            get
            {
                return _connectionID;
            }
        }

        int _partyID = int.MinValue;
        public int PartyID
        {
            get
            {
                return _partyID;
            }
        }

        string _partyName = string.Empty;
        public string PartyName
        {
            get
            {
                return _partyName;
            }
        }

        int _port = int.MinValue;
        public int Port
        {
            get
            {
                return _port;
            }
        }

        int _symbology = 0;
        public int Symbology
        {
            set { _symbology = value; }
            get { return _symbology; }
        }

        string _hostName = string.Empty;
        public string HostName
        {
            get
            {
                return _hostName;
            }
        }

        string _senderCompID = string.Empty;
        public string SenderCompID
        {
            get
            {
                return _senderCompID;
            }
        }

        string _targetCompID = string.Empty;
        public string TargetCompID
        {
            get
            {
                return _targetCompID;
            }
        }

        private bool _useAcknowledge = false;
        public bool UseAcknowledge
        {
            get
            {
                return _useAcknowledge;
            }
        }

        private int _acknowledgeTime = int.MinValue;
        public int AcknowledgeTime
        {
            get
            {
                return _acknowledgeTime;
            }
        }

        private PranaInternalConstants.ConnectionStatus _buySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        public PranaInternalConstants.ConnectionStatus BuySideStatus
        {
            get
            {
                return _buySideStatus;
            }
            set
            {
                _buySideStatus = value;
            }
        }

        private PranaInternalConstants.ConnectionStatus _buyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        public PranaInternalConstants.ConnectionStatus BuyToSellSideStatus
        {
            get
            {
                return _buyToSellSideStatus;
            }
            set
            {
                _buyToSellSideStatus = value;
            }
        }

        private Int64 _lastSeqNumberRecevied = Int64.MinValue;
        public Int64 LastSeqNumberRecevied
        {
            get { return _lastSeqNumberRecevied; }
            set { _lastSeqNumberRecevied = value; }
        }

        private string _targetSubID = string.Empty;
        public string TargetSubID
        {
            get { return _targetSubID; }
        }

        private PranaServerConstants.OriginatorType _originatorType = PranaServerConstants.OriginatorType.BuySide;
        public PranaServerConstants.OriginatorType OriginatorType
        {
            get { return _originatorType; }
        }

        private PranaServerConstants.BrokerConnectionType _brokerConnectionType;
        public PranaServerConstants.BrokerConnectionType BrokerConnectionType
        {
            get { return _brokerConnectionType; }
        }

        private string _dllName;
        public string FixDllName
        {
            get { return _dllName; }
        }

        private DateTime _resetTime;
        public DateTime ResetTime
        {
            get { return _resetTime; }
        }

        Dictionary<string, string> _extraTagCollection = new Dictionary<string, string>();
        public Dictionary<string, string> ExtraTagCollection
        {
            get { return _extraTagCollection; }
        }
        #endregion

        #region Public Methods
        public void GetDestinationAndDeliverToCompID(string venueID, string securityType, out string deliverToCompID, out string exDestination)
        {
            deliverToCompID = string.Empty;
            exDestination = string.Empty;
            if (_assetWiseDictionary.ContainsKey(securityType))
            {
                _venueDetails = _assetWiseDictionary[securityType];
                if (_venueDetails.ContainsKey(venueID))
                {
                    string[] destANDdeliverToCompID = _venueDetails[venueID].Split(',');
                    exDestination = destANDdeliverToCompID[0];
                    deliverToCompID = destANDdeliverToCompID[1];
                }
            }
        }

        public string GetDeliverToCompID(string venueID, string securityType)
        {
            if (_assetWiseDictionary.ContainsKey(securityType))
            {
                _venueDetails = _assetWiseDictionary[securityType];
                if (_venueDetails.ContainsKey(venueID))
                {
                    string[] destANDdeliverToCompID = _venueDetails[venueID].Split(',');


                    return destANDdeliverToCompID[1];
                }
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        public Dictionary<string, string> GetAssetDictionary(string assetType)
        {
            if (!_assetWiseDictionary.ContainsKey(assetType))
            {
                SerializableDictionary<string, string> venueDetails = new SerializableDictionary<string, string>();
                _assetWiseDictionary.Add(assetType, venueDetails);
                return venueDetails;
            }
            else
            {
                return _assetWiseDictionary[assetType];
            }
        }

        public void AddExtraTag(string tagNumber, string tagValue)
        {
            if (!_extraTagCollection.ContainsKey(tagNumber))
            {
                _extraTagCollection.Add(tagNumber, tagValue);
            }
        }

        public void SetConnectionValues(FixPartyDetails fixPartyDetails)
        {
            _buySideStatus = fixPartyDetails.BuySideStatus;
            _buyToSellSideStatus = fixPartyDetails.BuyToSellSideStatus; ;
        }
        #endregion
    }
}