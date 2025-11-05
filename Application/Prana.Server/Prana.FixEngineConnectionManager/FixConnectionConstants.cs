using Prana.BusinessObjects;
namespace Prana.FixEngineConnectionManager
{

    public class FixConnectionStatus
    {
        private int _counterPartyID = int.MinValue;
        private PranaInternalConstants.ConnectionStatus _buySideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        private PranaInternalConstants.ConnectionStatus _buyToSellSideStatus = PranaInternalConstants.ConnectionStatus.DISCONNECTED;
        private bool _underTroubleShootingMode = false;
        public FixConnectionStatus(int counterPartyID, PranaInternalConstants.ConnectionStatus buySideStatus, PranaInternalConstants.ConnectionStatus buyToSellSideStatus, bool underTroubleShooting)
        {
            _counterPartyID = counterPartyID;
            _buySideStatus = buySideStatus;
            _buyToSellSideStatus = buyToSellSideStatus;
            _underTroubleShootingMode = underTroubleShooting;

        }

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
        public bool UnderTroubleShootingMode
        {
            get
            {
                return _underTroubleShootingMode;
            }

            set
            {
                _underTroubleShootingMode = value;
            }
        }
        public int CounterPartyID
        {
            get
            {
                return _counterPartyID;
            }

            set
            {
                _counterPartyID = value;
            }
        }

    }

}
