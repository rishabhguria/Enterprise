using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class BasketDetail
    {
        #region Private Members
        private bool _updated = false;
        private string _msgType = string.Empty;
        private string _asset = string.Empty;
        private string _underLying = string.Empty;
        private string _user = string.Empty;
        private string _tradingAccount = string.Empty;

        private string _basketID = string.Empty;
        private string _currentWaveID = string.Empty;
        private string _currentGroupID = string.Empty;

        private string _bidType = string.Empty;
        private string _tradedBasketID = string.Empty;
        private string _serverReceiveTime = string.Empty;
        private string _upLoadedBasketID = string.Empty;
        private string _basketName = string.Empty;

        private string _tradingAccountVisible = string.Empty;
        private int _assetID = int.MinValue;
        private int _underLyingID = int.MinValue;
        private string _exchangeList = string.Empty;
        private int _baseCurrencyID = int.MinValue;
        private int _notionalValue = int.MinValue;
        private double _bchMarkValue = 0.0;

        [field: NonSerializedAttribute()]
        private Waves _basketWaves = new Waves();
        private string _templateID;
        private bool _hasWaves;
        private bool _tradingStarted = false;
        private bool _canCreateWave = false;
        private int _userID;
        private int _tradingAccountID = int.MinValue;
        private List<string> _displayColumnList = new List<string>();
        private double _basketValue = 0.0;
        private bool _isStatusCompleted;
        private Dictionary<string, Wave> _dictWaveCollection = new Dictionary<string, Wave>();
        private OrderCollection _basketOrders = null;
        #endregion

        public BasketDetail()
        {
            _dictWaveCollection = new Dictionary<string, Wave>();
            _basketOrders = new OrderCollection();
            _basketWaves = new Waves();
        }

        public void AddWaves(Waves waves)
        {
            try
            {
                foreach (Wave wave in waves)
                {
                    AddWave(wave);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void AddWave(Wave wave)
        {
            try
            {
                _dictWaveCollection.Add(wave.WaveID, wave);
                _basketWaves.Add(wave);
                _hasWaves = true;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void FillOrdersInWave(Waves waves)
        {
            try
            {
                double wavePercentage = 0;
                int waveTotalCount = waves.Count;
                foreach (Wave wave in waves)
                {
                    wavePercentage += wave.Percentage;
                }

                foreach (Order order in _basketOrders)
                {
                    if (order.CanCreateSubOrders())
                    {
                        Int64 waveTotalQty = Convert.ToInt64((order.AvailableQtyForSubOrders * wavePercentage) / 100);
                        int count = 1;
                        double sumofWaveQty = 0;
                        foreach (Wave wave in waves)
                        {
                            Order newOrder = (Order)order.Clone();
                            newOrder.SetDefaultValues();
                            newOrder.Parent = order;
                            newOrder.ClientOrderID = IDGenerator.GenerateClientOrderID();
                            newOrder.ParentClientOrderID = order.ClientOrderID;
                            if (count < waveTotalCount)
                            {
                                double qty = Convert.ToInt64((order.AvailableQtyForSubOrders * wave.Percentage) / 100);
                                newOrder.Quantity = qty;
                                sumofWaveQty = sumofWaveQty + qty;
                            }
                            else
                            {
                                newOrder.Quantity = waveTotalQty - sumofWaveQty;
                            }
                            count++;
                            newOrder.WaveID = wave.WaveID;
                            newOrder.UnsentQty = newOrder.Quantity;
                            wave.WaveOrders.Add(newOrder);
                        }
                    }
                }
                foreach (Wave wave in waves)
                {
                    foreach (Order WaveOrder in wave.WaveOrders)
                    {
                        WaveOrder.Parent.CreateSubOrder(WaveOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public Order GetOrder(string clientOrderID)
        {
            return _basketOrders.GetOrder(clientOrderID);
        }

        public Wave GetWave(string waveID)
        {
            return _dictWaveCollection[waveID];
        }

        #region Properties
        public double BasketValue
        {
            get
            {
                _basketValue = 0;
                foreach (Order order in this.BasketOrders)
                {
                    _basketValue = order.Quantity * order.Price + _basketValue;
                }

                return _basketValue;
            }

        }

        public OrderCollection BasketOrders
        {
            get { return _basketOrders; }
            set { _basketOrders = value; }
        }

        public int AssetID
        {
            set { _assetID = value; }
            get { return _assetID; }
        }

        public string Asset
        {
            set { _asset = value; }
            get { return _asset; }
        }

        public int UnderLyingID
        {
            set { _underLyingID = value; }
            get { return _underLyingID; }
        }

        public string UnderLying
        {
            set { _underLying = value; }
            get { return _underLying; }
        }

        public string User
        {
            set { _user = value; }
            get { return _user; }
        }

        public string TradingAccount
        {
            set { _tradingAccount = value; }
            get { return _tradingAccount; }
        }

        public string ExchangeList
        {
            set { _exchangeList = value; }
            get { return _exchangeList; }
        }

        public string BasketID
        {
            set { _basketID = value; }
            get { return _basketID; }
        }

        public string CurrentWaveID
        {
            set { _currentWaveID = value; }
            get { return _currentWaveID; }
        }

        public string CurrentGroupID
        {
            set { _currentGroupID = value; }
            get { return _currentGroupID; }
        }

        public string BidType
        {
            set { _bidType = value; }
            get { return _bidType; }
        }

        public string TradedBasketID
        {
            set { _tradedBasketID = value; }
            get { return _tradedBasketID; }
        }

        public string TradedTime
        {
            set { _serverReceiveTime = value; }
            get { return _serverReceiveTime; }
        }

        public double BanchMarkValue
        {
            set { _bchMarkValue = value; }
            get { return _bchMarkValue; }
        }

        public string UpLoadedBasketID
        {
            set { _upLoadedBasketID = value; }
            get { return _upLoadedBasketID; }
        }

        public string BasketName
        {
            set { _basketName = value; }
            get { return _basketName; }
        }

        public int NotionalValue
        {
            set { _notionalValue = value; }
            get { return _notionalValue; }
        }

        public string TradingAccountVisible
        {
            set { _tradingAccountVisible = value; }
            get { return _tradingAccountVisible; }
        }

        public int BaseCurrencyID
        {
            set { _baseCurrencyID = value; }
            get { return _baseCurrencyID; }
        }

        public Waves Waves
        {
            get { return _basketWaves; }
        }

        public string TemplateID
        {
            get { return _templateID; }
            set { _templateID = value; }
        }

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public int TradingAccountID
        {
            get { return _tradingAccountID; }
            set { _tradingAccountID = value; }
        }

        public bool HasWaves
        {
            get { return _hasWaves; }
            set { _hasWaves = value; }
        }

        public bool TradingStarted
        {
            get { return _tradingStarted; }
            set { _tradingStarted = value; }
        }

        public bool ISStatusCompleted
        {
            get { return _isStatusCompleted; }
            set { _isStatusCompleted = value; }
        }

        public bool CanCreateWaves
        {
            get { return _canCreateWave; }
            set { _canCreateWave = value; }
        }

        public List<string> DisplayColumnList
        {
            get { return _displayColumnList; }
            set { _displayColumnList = value; }
        }

        public string MsgType
        {
            get { return _msgType; }
            set { _msgType = value; }
        }

        public bool Updated
        {
            set { _updated = value; }
            get { return _updated; }
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_msgType);
            sb.Append(Seperators.SEPERATOR_1);//0
            sb.Append(_assetID);
            sb.Append(Seperators.SEPERATOR_1);//1
            sb.Append(_baseCurrencyID);
            sb.Append(Seperators.SEPERATOR_1);//2
            sb.Append(_basketID);
            sb.Append(Seperators.SEPERATOR_1);//3
            sb.Append(_currentWaveID);
            sb.Append(Seperators.SEPERATOR_1);//4
            sb.Append(_tradedBasketID);
            sb.Append(Seperators.SEPERATOR_1);//5
            sb.Append(_tradingAccountID);
            sb.Append(Seperators.SEPERATOR_1);//6
            sb.Append(_underLyingID);
            sb.Append(Seperators.SEPERATOR_1);//7
            sb.Append(_userID);
            sb.Append(Seperators.SEPERATOR_1);//8
            //Start of Orders
            sb.Append("|");
            foreach (Order order in _basketOrders)
            {
                sb.Append(order.ToString());
                sb.Append("|");
            }
            return sb.ToString();
        }

        public BasketDetail Clone()
        {
            BasketDetail clonedBasket = new BasketDetail();
            //get Details from Uploaded basket
            clonedBasket.BasketName = _basketName;
            clonedBasket.BasketID = IDGenerator.GenerateClientBasketID();
            clonedBasket.AssetID = _assetID;
            clonedBasket.UnderLyingID = _underLyingID;
            clonedBasket.UpLoadedBasketID = _upLoadedBasketID;
            clonedBasket.TemplateID = _templateID;
            clonedBasket.DisplayColumnList = _displayColumnList;
            clonedBasket.UserID = _userID;
            OrderCollection orders = _basketOrders.Clone();
            // Add These orders in Basket
            foreach (Order order in orders)
            {
                order.ClientOrderID = IDGenerator.GenerateClientOrderID();
                order.ParentClientOrderID = order.ClientOrderID;
                clonedBasket.BasketOrders.Add(order);
            }
            return clonedBasket;
        }

        public double CumQty
        {
            get
            {
                return _basketOrders.CumQty;
            }
        }

        public double Quantity
        {
            get
            {
                return _basketOrders.Quantity;
            }

        }
        public double ExeValue
        {
            get
            {
                return _basketOrders.ExeValue;
            }
        }
    }
}