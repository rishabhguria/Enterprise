using System;
using System.Collections.Generic;
using System.Text;
using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects
{
    [Serializable()]


    public class ExposurePnlCacheItemGlobal
    {
        /// <summary>
        ///   Some Fields in an order and Global NAV dependent and Global NAV may vary in case client donot have permissions for all the accounts.
        ///    Thus we need to calculate Global NAV dependent separately for different clients.
        /// Also Cloumns like Position Side Global and Long Short Exposure Global which are dependent on Symbol level Summary are kept in ExposurePnlCacheItemGlobal
        /// </summary>
        public ExposurePnlCacheItemGlobal()
        {
            _id = string.Empty;
            _percentagePositionLong = 0.0;
            _percentagePositionShort = 0.0;
            _percentAssetGlobal = 0.0;
            _percentNetExposureGlobal = 0.0;
            _percentGrossExposureGlobal = 0.0;
            _pNLContributionPercentageGlobal = 0.0;
            _exposureBPInBaseCurrency = 0.0;
            _positionSideExposureInPortfolio = string.Empty;
            _positionSideMVInPortfolio = string.Empty;
            _liquidationCostInPortfolio = "0.0";
            _longExposurePortfolio = 0.0;
            _shortExposurePortfolio = 0.0;
            _navValueGlobal = 0.0;
            _percentNetExpGlobalGrossMvAndCash = 0.0;
            _startOfDayNAVGlobal = 0.0;
            _dayReturnGlobal = 0.0;
            _underlyingGrossExposureGlobalInBaseCurrency = 0.0;
            _percentUnderlyingGrossExposureGlobalInBaseCurrency = 0.0;
            _percentNetNotionalGlobal = 0.0;
            _positionSideExposureUnderlyingInPortfolio = string.Empty;
            _betaAdjGrossExposureUnderlyingGlobal = 0.0;
            _betaAdjGrossExposureUnderlyingGlobalInBaseCurrency = 0.0;
            _percentBetaAdjGrossExposureUnderlyingGlobalInBaseCurrency = 0.0;
            _underlyingGrossExposureGlobal = 0.0;
            _percentExposureFirmInBaseCurrency = 0.0;
        }

        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private double _percentagePositionLong;
        public double PercentagePositionLong
        {
            get { return _percentagePositionLong; }
            set { _percentagePositionLong = value; }
        }

        private double _percentagePositionShort;
        public double PercentagePositionShort
        {
            get { return _percentagePositionShort; }
            set { _percentagePositionShort = value; }
        }

        private double _percentAssetGlobal;
        public double PercentAssetGlobal
        {
            get { return _percentAssetGlobal; }
            set { _percentAssetGlobal = value; }
        }

        private double _percentNetExposureGlobal;
        public double PercentNetExposureGlobal
        {
            get { return _percentNetExposureGlobal; }
            set { _percentNetExposureGlobal = value; }
        }

        private double _percentGrossExposureGlobal;
        public double PercentGrossExposureGlobal
        {
            get { return _percentGrossExposureGlobal; }
            set { _percentGrossExposureGlobal = value; }
        }

        private double _pNLContributionPercentageGlobal;
        public double PNLContributionPercentageGlobal
        {
            get { return _pNLContributionPercentageGlobal; }
            set { _pNLContributionPercentageGlobal = value; }
        }

        private double _exposureBPInBaseCurrency;
        public double ExposureBPInBaseCurrency
        {
            get { return _exposureBPInBaseCurrency; }
            set { _exposureBPInBaseCurrency = value; }
        }

        private string _positionSideMVInPortfolio;
        public string PositionSideMVInPortfolio
        {
            get { return _positionSideMVInPortfolio; }
            set { _positionSideMVInPortfolio = value; }
        }

        private string _positionSideExposureInPortfolio;
        public string PositionSideExposureInPortfolio
        {
            get { return _positionSideExposureInPortfolio; }
            set { _positionSideExposureInPortfolio = value; }
        }

        private string _liquidationCostInPortfolio;
        public string LiquidationCostInPortfolio
        {
            get { return _liquidationCostInPortfolio; }
            set { _liquidationCostInPortfolio = value; }

        }

        private double _longExposurePortfolio;
        public double LongExposurePortfolio
        {
            get { return _longExposurePortfolio; }
            set
            {
                _longExposurePortfolio = value;
            }
        }

        private double _shortExposurePortfolio;
        public double ShortExposurePortfolio
        {
            get { return _shortExposurePortfolio; }
            set
            {
                _shortExposurePortfolio = value;
            }
        }

        private double _navValueGlobal;
        public double NAVValueGlobal
        {
            get { return _navValueGlobal; }
            set
            {
                _navValueGlobal = value;
            }
        }


        private double _startOfDayNAVGlobal;
        public double StartOfDayNAVGlobal
        {
            get { return _startOfDayNAVGlobal; }
            set { _startOfDayNAVGlobal = value; }
        }

        private double _dayReturnGlobal;
        public double DayReturnGlobal
        {
            get { return _dayReturnGlobal; }
            set { _dayReturnGlobal = value; }
        }



        private double _percentNetExpGlobalGrossMvAndCash;
        public double PercentNetExpGlobalGrossMvAndCash
        {

            get { return _percentNetExpGlobalGrossMvAndCash; }
            set { _percentNetExpGlobalGrossMvAndCash = value; }

        }

        private double _underlyingGrossExposureGlobal;
        public double UnderlyingGrossExposureGlobal
        {
            get { return _underlyingGrossExposureGlobal; }
            set { _underlyingGrossExposureGlobal = value; }
        }

        private double _underlyingGrossExposureGlobalInBaseCurrency;
        public double UnderlyingGrossExposureGlobalInBaseCurrency
        {
            get { return _underlyingGrossExposureGlobalInBaseCurrency; }
            set { _underlyingGrossExposureGlobalInBaseCurrency = value; }
        }

        private double _percentUnderlyingGrossExposureGlobalInBaseCurrency;
        public double PercentUnderlyingGrossExposureGlobalInBaseCurrency
        {
            get { return _percentUnderlyingGrossExposureGlobalInBaseCurrency; }
            set { _percentUnderlyingGrossExposureGlobalInBaseCurrency = value; }
        }
        private double _percentNetNotionalGlobal;
        public double PercentNetNotionalGlobal
        {
            get { return _percentNetNotionalGlobal; }
            set { _percentNetNotionalGlobal = value; }
        }
        private string _potentialRisk;
        public string PotentialRisk
        {
            get { return _potentialRisk; }
            set { _potentialRisk = value; }
        }
        private double _trueExposure;
        public double TrueExposure
        {
            get { return _trueExposure; }
            set { _trueExposure = value; }
        }
        private string _positionSideExposureUnderlyingInPortfolio;
        public string PositionSideExposureUnderlyingInPortfolio
        {
            get { return _positionSideExposureUnderlyingInPortfolio; }
            set { _positionSideExposureUnderlyingInPortfolio = value; }
        }

        private double _betaAdjGrossExposureUnderlyingGlobal;
        public double BetaAdjGrossExposureUnderlyingGlobal
        {
            get { return _betaAdjGrossExposureUnderlyingGlobal; }
            set { _betaAdjGrossExposureUnderlyingGlobal = value; }
        }

        private double _betaAdjGrossExposureUnderlyingGlobalInBaseCurrency;
        public double BetaAdjGrossExposureUnderlyingGlobalInBaseCurrency
        {
            get { return _betaAdjGrossExposureUnderlyingGlobalInBaseCurrency; }
            set { _betaAdjGrossExposureUnderlyingGlobalInBaseCurrency = value; }
        }
     
        private double _percentBetaAdjGrossExposureUnderlyingGlobalInBaseCurrency;
        public double PercentBetaAdjGrossExposureUnderlyingGlobalInBaseCurrency
        {
            get { return _percentBetaAdjGrossExposureUnderlyingGlobalInBaseCurrency; }
            set { _percentBetaAdjGrossExposureUnderlyingGlobalInBaseCurrency = value; }
        }
      
        private double _percentExposureFirmInBaseCurrency;
        public double PercentExposureFirmInBaseCurrency
        {
            get { return _percentExposureFirmInBaseCurrency; }
            set { _percentExposureFirmInBaseCurrency = value; }
        }
    }
}
