//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using Prana.Global;
//using Prana.Logging;
//using Prana.BusinessObjects.LiveFeed;

//namespace Prana.SecurityMasterNew
//{
//    public class SecMasterCoreObject
//    {

//        string _tickerSymbol = string.Empty;
//        double _dividend = double.Epsilon;
//        double _peRatio = double.Epsilon;
//        double _dailyVolume = double.Epsilon;
//        DateTime _date = DateTime.UtcNow;
//        string _sectorInformation = string.Empty;

//        string _longName = string.Empty;
//        decimal _roundLot = 1;

//        #region Properties


//        public string TikcerSymbol
//        {
//            get { return _tickerSymbol; }
//            set { _tickerSymbol = value; }

//        }
//        public double Dividend
//        {
//            get { return _dividend; }
//            set { _dividend = value; }

//        }
//        public double DailyVolume
//        {
//            get { return _dailyVolume; }
//            set { _dailyVolume = value; }

//        }
//        public double PERatio
//        {
//            get { return _peRatio; }
//            set { _peRatio = value; }
//        }
//        public DateTime Date
//        {
//            get { return _date; }
//            set { _date = value; }
//        }

//        public string CompanyName
//        {
//            get { return _longName; }
//            set { _longName = value; }
//        }
//        public decimal RoundLot
//        {
//            get { return _roundLot; }
//            set { _roundLot = value; }
//        }
//        public string SectorInformation
//        {
//            get { return _sectorInformation; }
//            set { _sectorInformation = value; }

//        }
//        #endregion

//        public void FillData(object[] row, int offset, DateTime dateTime)
//        {

//            if (offset < 0)
//            {
//                offset = 0;
//            }
//            if (row != null)
//            {
//                int TickerSymbol = offset + 0;
//                int RoundLot = offset + 1;
//                int LongName = offset + 2;

//                int DailyVolume = offset + 3;
//                int PeRatio = offset + 4;
//                int Dividend = offset + 5;
//                int SectorInformation = offset + 6;

//                try
//                {
//                    // order.AllocatedQty = int.Parse(row[AllocatedQty].ToString());
//                    if (row[TickerSymbol] != System.DBNull.Value)
//                    {
//                        _tickerSymbol = row[TickerSymbol].ToString();
//                    }
//                    if (row[RoundLot] != System.DBNull.Value)
//                    {
//                        _roundLot = Convert.ToDecimal(row[RoundLot]);
//                    }
//                    if (row[LongName] != System.DBNull.Value)
//                    {
//                        _longName = row[LongName].ToString();
//                    }
//                    if (row[DailyVolume] != System.DBNull.Value)
//                    {
//                        _dailyVolume = float.Parse(row[DailyVolume].ToString());
//                    }

//                    if (row[PeRatio] != System.DBNull.Value)
//                    {
//                        _peRatio = Convert.ToDouble(row[PeRatio].ToString());
//                    }
//                    if (row[Dividend] != System.DBNull.Value)
//                    {
//                        _dividend = Convert.ToDouble(row[Dividend].ToString());
//                    }
//                    if (row[SectorInformation] != System.DBNull.Value)
//                    {
//                        _sectorInformation = row[SectorInformation].ToString();
//                    }
//                }
//                catch (Exception ex)
//                {
//                    bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);


//                    if (rethrow)
//                    {
//                        throw;
//                    }

//                }
//            }
//        }

//        public void FillData(Level1Data level1Data)
//        {
//            if (level1Data.FullCompanyName != null)
//            {
//                _longName = level1Data.FullCompanyName;
//            }
//            _tickerSymbol = level1Data.Symbol;
//        }
//        public override string ToString()
//        {
//            return " CompanyName=" + _longName + " DailyVolume=" + _dailyVolume.ToString() +
//                " Dividend=" + _dividend.ToString() + " PE Ratio=" + _peRatio.ToString() + " RoundLot=" + _roundLot.ToString()+"SectorInformation="+_sectorInformation.ToString();
//        }
//        public void UpDateData(object secMasterObj1)
//        {
//            SecMasterCoreObject secMasterObj = (SecMasterCoreObject)secMasterObj1;
//            //if (_underlyingID == int.MinValue)
//            //{
//            //    _underlyingID = secMasterObj.UnderLyingID;
//            //}
//            //if (_longName == string.Empty)
//            //{
//            //    _longName = secMasterObj.LongName;
//            //}
//            //if (_assetID == int.MinValue)
//            //{
//            //    _assetID = secMasterObj.AssetID;
//            //}
//            //if (_auecID == int.MinValue)
//            //{
//            //    _auecID = secMasterObj.AuecID;
//            //}
//            _tickerSymbol = secMasterObj.TikcerSymbol;
//            if (_longName == string.Empty)
//            {
//                _longName = secMasterObj.CompanyName;
//            }
//        }

//    }
//}
