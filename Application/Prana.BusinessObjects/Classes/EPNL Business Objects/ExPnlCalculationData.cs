//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Prana.BusinessObjects
//{
//    public class ExPnlCalculationData
//    {
//        private double _longExposure = 0;

//        private double _longNotionalValue = 0;

//        private double _longPnl = 0;

//        private double _netExposure = 0;

//        private double _netNotionalValue = 0;

//        private double _netPnl = 0;

//        private double _percentagePositionLong;

//        private double _percentagePositionShort;

//        private double _positionPNL;

//        private double _realizedPNL = 0;

//        private double _shortExposure = 0;

//        private double _shortNotionalValue = 0;

//        private double _shortPnl = 0;

//        private double _unrealisedPNL = 0;

//        private double _fxRate = 0;

//        private int _sideMultiplier = 1;
//        public double LongExposure
//        {
//            get { return _longExposure; }

//        }

//        public double LongExposureInCompnayBaseCurrency
//        {
//            get { return _longExposure * _fxRate; }
//        }

//        public double LongNotionalValue
//        {
//            get { return _longNotionalValue; }
//        }

//        public double LongNotionalValueInCompnayBaseCurrency
//        {
//            get { return _longNotionalValue * _fxRate; }
//        }

//        public double LongPnL
//        {
//            get { return _longPnl; }
//        }

//        public double LongPnlInCompanyBaseCurrency
//        {
//            get { return _longPnl * _fxRate; }
//        }

//        public double NetExposure
//        {
//            get { return _netExposure; }
//            set
//            {
//                _netExposure = value;

//                if (_sideMultiplier == -1)
//                {
//                    _shortExposure = _netExposure;
//                }
//                else
//                {
//                    _longExposure = _netExposure;
//                }
//            }
//        }

//        public double NetExposureInCompnayBaseCurrency
//        {
//            get { return _netExposure * _fxRate; }
//        }

//        public double NetNotionalValue
//        {
//            get { return _netNotionalValue; }
//            set
//            {
//                _netNotionalValue = value;

//                if (_sideMultiplier == -1)
//                {
//                    _shortNotionalValue = _netExposure;
//                }
//                else
//                {
//                    _longNotionalValue = _netExposure;
//                }
//            }
//        }

//        public double NetNotionalValueInCompnayBaseCurrency
//        {
//            get { return _netNotionalValue * _fxRate; }
//        }

//        public double NetPnL
//        {
//            get { return _netPnl; }
//            set
//            {
//                _netPnl = value;
//                if (_sideMultiplier == -1)
//                {
//                    _shortPnl = _netPnl;
//                }
//                else
//                {
//                    _longPnl = _netPnl;
//                }
//            }
//        }

//        public double NetPnlInCompanyBaseCurrency
//        {
//            get { return _netPnl * _fxRate; }
//        }

//        public double PercentagePositionLong
//        {
//            get { return _percentagePositionLong; }
//            set { _percentagePositionLong = value; }
//        }

//        public double PercentagePositionShort
//        {
//            get { return _percentagePositionShort; }
//            set { _percentagePositionShort = value; }
//        }

//        public double PositionPNL
//        {
//            get { return _positionPNL; }
//            set { _positionPNL = value; }
//        }

//        public double PositionPNLInCompnayBaseCurrency
//        {
//            get { return _positionPNL * _fxRate; }
//        }

//        public double RealizedPNL
//        {
//            get { return _realizedPNL; }
//            set { _realizedPNL = value; }
//        }

//        public double ShortExposure
//        {
//            get { return _shortExposure; }
//        }

//        public double ShortExposureInCompnayBaseCurrency
//        {
//            get { return _shortExposure * _fxRate; }
//        }

//        public double ShortNotionalValue
//        {
//            get { return _shortNotionalValue; }
//        }

//        public double ShortNotionalValueInCompnayBaseCurrency
//        {
//            get { return _shortNotionalValue * _fxRate; }
//        }

//        public double ShortPnL
//        {
//            get { return _shortPnl; }
//        }

//        public double ShortPnlInCompanyBaseCurrency
//        {
//            get { return _shortPnl * _fxRate; }
//        }

//        public double UnrealisedPNL
//        {
//            get { return _unrealisedPNL; }
//            set
//            {
//                _unrealisedPNL = value;
//            }
//        }

//        public double FxRate
//        {
//            get { return _fxRate; }
//            set { _fxRate = value; }
//        }

//        public int SideMultiplier
//        {
//            get { return _sideMultiplier; }

//            //To Do : Remove this setter and make this property readonly when Prana.Constants is created
//            set
//            {
//                _sideMultiplier = value;

//            }
//        }
//    }
//}
