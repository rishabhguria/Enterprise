using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.CalculationService.Models
{
    internal class RowCalculationMarketDataBased
    {
        public void UpdateMarketDataDependentValues(RowCalculationBaseNav source)
        {
            if (source == null) return;
            Accrual = source.Accrual;
            AvgPrice = source.AvgPrice;
            BetaAdjExpBase = source.BetaAdjExpBase;
            CostBPnLLoc = source.CostBPnLLoc;
            CostBPnLB = source.CostBPnLB;
            CurCash = source.CurCash;
            DPnLLocal = source.DPnLLocal;
            DPnLBase = source.DPnLBase;
            NetExpLocal = source.NetExpLocal;
            NetExpBase = source.NetExpBase;
            GrossExpBase = source.GrossExpBase;
            NetMVLocal = source.NetMVLocal;
            NetMVBase = source.NetMVBase;
            AccNav = source.AccNav;
            FeedPriceB = source.FeedPriceB;
            FeedPriceL = source.FeedPriceL;
            Qty = source.Qty;
            UnqId = source.UnqId;
            Symbol = source.Symbol;
            AccId = source.AccId;
            MFId = source.MFId;
            MFNav = source.MFNav;
            CurFxRate = source.CurFxRate;
            NtnlBase = source.NtnlBase;
            ShExpBase = source.ShExpBase;
            LoExpBase = source.LoExpBase;
            GrossExpBaseAcc = source.GrossExpBaseAcc;
            GrossExpBaseMF = source.GrossExpBaseMF;
            DeltaAdjPos = source.DeltaAdjPos;
            AssetId = source.AssetId;
            PermOverride = source.PermOverride;
            PricingStatus = source.PricingStatus;
            YesterdayMVBase = source.YesterdayMVBase;
            SODNavAcc = source.SODNavAcc;
            SODNavMF = source.SODNavMF;
            TdyRtrnValB = source.TdyRtrnValB;
        }

        #region Class Members

        private string _symbol;
        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }

        private double _avgPrice;
        public double AvgPrice
        {
            get { return _avgPrice; }
            set { _avgPrice = value; }
        }

        private int _accountId;
        public int AccId
        {
            get { return _accountId; }
            set { _accountId = value; }
        }

        private int _masterFundId;
        public int MFId
        {
            get { return _masterFundId; }
            set { _masterFundId = value; }
        }

        private double _masterFundNav;
        public double MFNav
        {
            get { return _masterFundNav; }
            set { _masterFundNav = value; }
        }

        private double _quantity;
        public double Qty
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        private double _netMarketValueBase;
        public double NetMVBase
        {
            get { return _netMarketValueBase; }
            set { _netMarketValueBase = value; }
        }

        private double _netMarketValueLocal;
        public double NetMVLocal
        {
            get { return _netMarketValueLocal; }
            set { _netMarketValueLocal = value; }
        }

        private double _dayPnLBase;
        public double DPnLBase
        {
            get { return _dayPnLBase; }
            set { _dayPnLBase = value; }
        }

        private double _dayPnLocal;
        public double DPnLLocal
        {
            get { return _dayPnLocal; }
            set { _dayPnLocal = value; }
        }

        private double _netExposureBase;
        public double NetExpBase
        {
            get { return _netExposureBase; }
            set { _netExposureBase = value; }
        }

        private double _netExposureLocal;
        public double NetExpLocal
        {
            get { return _netExposureLocal; }
            set { _netExposureLocal = value; }
        }

        private double _costBasisPnLLocal;
        public double CostBPnLLoc
        {
            get { return _costBasisPnLLocal; }
            set { _costBasisPnLLocal = value; }
        }

        private double _costBasisPnLBase;
        public double CostBPnLB
        {
            get { return _costBasisPnLBase; }
            set { _costBasisPnLBase = value; }
        }

        private double _selectedFeedPriceLocal;
        public double FeedPriceL
        {
            get { return _selectedFeedPriceLocal; }
            set { _selectedFeedPriceLocal = value; }
        }

        private double _selectedFeedPriceBase;
        public double FeedPriceB
        {
            get { return _selectedFeedPriceBase; }
            set { _selectedFeedPriceBase = value; }
        }

        private double _currentFxRate;
        public double CurFxRate
        {
            get { return _currentFxRate; }
            set { _currentFxRate = value; }
        }

        private string _uniqueId;
        public string UnqId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        private int _assetId;
        public int AssetId
        {
            get { return _assetId; }
            set { _assetId = value; }
        }

        private double _notionalBase;
        public double NtnlBase
        {
            get { return _notionalBase; }
            set { _notionalBase = value; }
        }

        private double _shortExposureBase;
        public double ShExpBase
        {
            get { return _shortExposureBase; }
            set { _shortExposureBase = value; }
        }

        private double _longExposureBase;
        public double LoExpBase
        {
            get { return _longExposureBase; }
            set { _longExposureBase = value; }
        }

        private double _accrual;
        public double Accrual
        {
            get { return _accrual; }
            set { _accrual = value; }
        }

        private double _currentCash;
        public double CurCash
        {
            get { return _currentCash; }
            set { _currentCash = value; }
        }

        private double _accountNav;
        public double AccNav
        {
            get { return _accountNav; }
            set { _accountNav = value; }
        }

        private double _grossExposureBase;
        public double GrossExpBase
        {
            get { return _grossExposureBase; }
            set { _grossExposureBase = value; }
        }

        private double _grossExposureBaseAccount;
        public double GrossExpBaseAcc
        {
            get { return _grossExposureBaseAccount; }
            set { _grossExposureBaseAccount = value; }
        }

        private double _grossExposureBaseMasterFund;
        public double GrossExpBaseMF
        {
            get { return _grossExposureBaseMasterFund; }
            set { _grossExposureBaseMasterFund = value; }
        }

        private double _betaAdjustedExposureBase;
        public double BetaAdjExpBase
        {
            get { return _betaAdjustedExposureBase; }
            set { _betaAdjustedExposureBase = value; }
        }

        private double _deltaAdjPosition;
        public double DeltaAdjPos
        {
            get { return _deltaAdjPosition; }
            set { _deltaAdjPosition = value; }
        }

        private bool _permissionOverride;
        public bool PermOverride
        {
            get { return _permissionOverride; }
            set { _permissionOverride = value; }
        }

        private int _pricingStatus = 0;
        public int PricingStatus
        {
            get { return _pricingStatus; }
            set { _pricingStatus = value; }
        }

        private double _yesterdayMarketValueBase;
        public double YesterdayMVBase
        {
            get { return _yesterdayMarketValueBase; }
            set { _yesterdayMarketValueBase = value; }
        }

        private double _startOfDayNavAccount;
        public double SODNavAcc
        {
            get { return _startOfDayNavAccount; }
            set { _startOfDayNavAccount = value; }
        }

        private double _startOfDayNavMasterFund;
        public double SODNavMF
        {
            get { return _startOfDayNavMasterFund; }
            set { _startOfDayNavMasterFund = value; }
        }

        private double _todayReturnValueBase;
        public double TdyRtrnValB
        {
            get { return _todayReturnValueBase; }
            set { _todayReturnValueBase = value; }
        }

        public DateTime InsertionDateTime { get; set; } = DateTime.Now;
        #endregion
    }
}
