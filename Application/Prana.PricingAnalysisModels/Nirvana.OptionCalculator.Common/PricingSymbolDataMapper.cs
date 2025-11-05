using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Prana.OptionCalculator.Common
{
    public class PricingSymbolDataMapper
    {
        static string _mappingFilePath = Path.Combine(Path.Combine(Application.StartupPath, "Xmls"), "PricingColumnMapping.xml");
        static SerializableDictionary<string, string> _dictColumnMapping;
        public static void ParseDataRowToSymbolData(SymbolData symbolData, DataRow row, List<string> template, Prana.Global.ApplicationConstants.SymbologyCodes symbology, Dictionary<string, string> dict = null)
        {
            try
            {
                lock (_locker)
                {
                    LoadDictionary();
                    string format = "yyyy-MM-dd HH:mm:ss";
                    foreach (string field in template)
                    {
                        if ((row != null && row.Table.Columns.Contains(field) && row[field] != DBNull.Value) ||
                            dict != null)
                        {
                            string value = string.Empty;
                            if (dict == null)
                                value = row[field].ToString().Trim().ToUpper();
                            else
                                value = dict[field].ToString().Trim().ToUpper();
                            string propertyName = field;
                            if (_dictColumnMapping.ContainsKey(field))
                            {
                                propertyName = _dictColumnMapping[field].ToUpper();
                            }
                            switch (propertyName.ToUpper())
                            {
                                case "ANNUALDIVIDEND":
                                    double AnnualDividend = 0;
                                    if (double.TryParse(value, out AnnualDividend))
                                    {
                                        symbolData.AnnualDividend = AnnualDividend;
                                    }
                                    break;
                                case "ASK":
                                    double Ask = 0;
                                    if (double.TryParse(value, out Ask))
                                    {
                                        symbolData.Ask = Ask;
                                    }
                                    break;
                                case "ASKEXCHANGE":
                                    symbolData.AskExchange = value;
                                    break;
                                case "ASKSIZE":
                                    long AskSize = 0;
                                    if (long.TryParse(value, out AskSize))
                                    {
                                        symbolData.AskSize = AskSize;
                                    }
                                    break;
                                case "AUECID":
                                    int AUECID = 0;
                                    if (int.TryParse(value, out AUECID))
                                    {
                                        symbolData.AUECID = AUECID;
                                    }
                                    break;
                                case "AVERAGEVOLUME20DAY":
                                    double AverageVolume20Day = 0;
                                    if (double.TryParse(value, out AverageVolume20Day))
                                    {
                                        symbolData.AverageVolume20Day = AverageVolume20Day;
                                    }
                                    break;
                                case "AVGVOLUME":
                                case "AVERAGEVOLUME":
                                    double AvgVolume = 0;
                                    if (double.TryParse(value, out AvgVolume))
                                    {
                                        symbolData.AvgVolume = AvgVolume;
                                    }
                                    break;
                                case "BBGID":
                                    symbolData.BBGID = value;
                                    break;
                                case "BETA_5YRMONTHLY":
                                    double Beta_5yrMonthly = 0;
                                    if (double.TryParse(value, out Beta_5yrMonthly))
                                    {
                                        symbolData.Beta_5yrMonthly = Beta_5yrMonthly;
                                    }
                                    break;
                                case "BID":
                                    double Bid = 0;
                                    if (double.TryParse(value, out Bid))
                                    {
                                        symbolData.Bid = Bid;
                                    }
                                    break;
                                case "BIDEXCHANGE":

                                    symbolData.BidExchange = value;

                                    break;
                                case "BIDSIZE":
                                    long BidSize = 0;
                                    if (long.TryParse(value, out BidSize))
                                    {
                                        symbolData.BidSize = BidSize;
                                    }
                                    break;
                                case "BLOOMBERGSYMBOL":
                                    symbolData.BloombergSymbol = value;
                                    break;
                                case "CATEGORYCODE":
                                    AssetCategory CategoryCode = 0;
                                    if (AssetCategory.TryParse(value, out CategoryCode))
                                    {
                                        symbolData.CategoryCode = CategoryCode;
                                    }
                                    break;
                                case "CFICODE":
                                    symbolData.CFICode = value;
                                    break;
                                case "CHANGE":
                                    double Change = 0;
                                    if (double.TryParse(value, out Change))
                                    {
                                        symbolData.Change = Change;
                                    }
                                    break;
                                case "CONVERSIONMETHOD":
                                    Operator ConversionMethod = 0;
                                    if (Enum.TryParse<Operator>(value, true, out ConversionMethod))
                                    {
                                        symbolData.ConversionMethod = ConversionMethod;
                                    }
                                    break;
                                case "CURENCYCODE":
                                    symbolData.CurencyCode = value;
                                    break;
                                case "CUSIPNO":
                                    symbolData.CusipNo = value;
                                    break;
                                case "DAYSTOEXPIRATION":
                                    int DaysToExpiration = 0;
                                    if (int.TryParse(value, out DaysToExpiration))
                                    {
                                        symbolData.DaysToExpiration = DaysToExpiration;
                                    }
                                    break;
                                case "DELAYED":
                                    bool Delayed = true;
                                    if (bool.TryParse(value, out Delayed))
                                    {
                                        symbolData.PricingStatus = Delayed ? PricingStatus.Delayed : PricingStatus.RealTime;
                                    }
                                    break;
                                case "DELTA":
                                    double Delta = 0;
                                    if (double.TryParse(value, out Delta))
                                    {
                                        symbolData.Delta = Delta;
                                    }
                                    break;
                                case "DELTASOURCE":
                                    DeltaSource deltaSource = 0;
                                    if (Enum.TryParse<DeltaSource>(value, true, out deltaSource))
                                    {
                                        symbolData.DeltaSource = deltaSource;
                                    }
                                    break;
                                case "DIVDISTRIBUTIONDATE":
                                    DateTime DivDistributionDate;
                                    if (DateTime.TryParse(value, out DivDistributionDate))
                                    {
                                        symbolData.DivDistributionDate = DivDistributionDate;
                                    }
                                    break;
                                case "DIVIDEND":
                                    double Dividend = 0;
                                    if (double.TryParse(value, out Dividend))
                                    {
                                        symbolData.Dividend = Dividend;
                                    }
                                    break;
                                case "DIVIDENDAMTRATE":
                                    float DividendAmtRate = 0;
                                    if (float.TryParse(value, out DividendAmtRate))
                                    {
                                        symbolData.DividendAmtRate = DividendAmtRate;
                                    }
                                    break;
                                case "DIVIDENDINTERVAL":
                                    long DividendInterval = 0;
                                    if (long.TryParse(value, out DividendInterval))
                                    {
                                        symbolData.DividendInterval = DividendInterval;
                                    }
                                    break;
                                case "DIVIDENDYIELD":
                                    double DividendYield = 0;
                                    if (double.TryParse(value, out DividendYield))
                                    {
                                        symbolData.DividendYield = DividendYield;
                                    }
                                    break;
                                case "EXCHANGEID":
                                    int ExchangeID = 0;
                                    if (int.TryParse(value, out ExchangeID))
                                    {
                                        symbolData.ExchangeID = ExchangeID;
                                    }
                                    break;
                                case "EXPIRATIONDATE":
                                    DateTime ExpirationDate;
                                    if (DateTime.TryParse(value, out ExpirationDate))
                                    {
                                        symbolData.ExpirationDate = ExpirationDate;
                                    }
                                    break;
                                case "FINALDIVIDENDYIELD":
                                    double FinalDividendYield = 0;
                                    if (double.TryParse(value, out FinalDividendYield))
                                    {
                                        symbolData.FinalDividendYield = FinalDividendYield;
                                    }
                                    break;
                                case "STOCKBORROWCOST":
                                    double StockBorrowCost = 0;
                                    if (double.TryParse(value, out StockBorrowCost))
                                    {
                                        symbolData.StockBorrowCost = StockBorrowCost;
                                    }
                                    break;
                                case "FINALIMPLIEDVOL":
                                    double FinalImpliedVol = 0;
                                    if (double.TryParse(value, out FinalImpliedVol))
                                    {
                                        symbolData.FinalImpliedVol = FinalImpliedVol;
                                    }
                                    break;
                                case "FINALINTERESTRATE":
                                    double FinalInterestRate = 0;
                                    if (double.TryParse(value, out FinalInterestRate))
                                    {
                                        symbolData.FinalInterestRate = FinalInterestRate;
                                    }
                                    break;
                                case "FORWARDPOINTS":
                                    double ForwardPoints = 0;
                                    if (double.TryParse(value, out ForwardPoints))
                                    {
                                        symbolData.ForwardPoints = ForwardPoints;
                                    }
                                    break;
                                case "FULLCOMPANYNAME":
                                    symbolData.FullCompanyName = value;
                                    break;
                                case "GAMMA":
                                    double Gamma = 0;
                                    if (double.TryParse(value, out Gamma))
                                    {
                                        symbolData.Gamma = Gamma;
                                    }
                                    break;
                                case "GAPOPEN":
                                    double GapOpen = 0;
                                    if (double.TryParse(value, out GapOpen))
                                    {
                                        symbolData.GapOpen = GapOpen;
                                    }
                                    break;
                                case "HIGH":
                                    double High = 0;
                                    if (double.TryParse(value, out High))
                                    {
                                        symbolData.High = High;
                                    }
                                    break;
                                case "HISTORICALVOLATILITY":
                                    break;
                                case "IDCOOPTIONSYMBOL":
                                    symbolData.IDCOOptionSymbol = value;
                                    break;
                                case "IMID":
                                    double iMid = 0;
                                    if (double.TryParse(value, out iMid))
                                    {
                                        symbolData.iMid = iMid;
                                    }
                                    break;
                                case "IMPLIEDVOL":
                                    double ImpliedVol = 0;
                                    if (double.TryParse(value, out ImpliedVol))
                                    {
                                        symbolData.ImpliedVol = ImpliedVol;
                                    }
                                    break;
                                case "INTERESTRATE":
                                    double InterestRate = 0;
                                    if (double.TryParse(value, out InterestRate))
                                    {
                                        symbolData.InterestRate = InterestRate;
                                    }
                                    break;
                                case "ISCHANGEDTOHIGHERCURRENCY":
                                    bool IsChangedToHigherCurrency = true;
                                    if (bool.TryParse(value, out IsChangedToHigherCurrency))
                                    {
                                        symbolData.IsChangedToHigherCurrency = IsChangedToHigherCurrency;
                                    }
                                    break;
                                case "ISIN":
                                    symbolData.ISIN = value;
                                    break;
                                case "LASTPRICE":
                                    double LastPrice = 0;
                                    if (double.TryParse(value, out LastPrice))
                                    {
                                        symbolData.LastPrice = LastPrice;
                                    }
                                    break;
                                case "LASTTICK":
                                    symbolData.LastTick = value;
                                    break;
                                case "LISTEDEXCHANGE":
                                    symbolData.ListedExchange = value;
                                    break;
                                case "LOW":
                                    double Low = 0;
                                    if (double.TryParse(value, out Low))
                                    {
                                        symbolData.Low = Low;
                                    }
                                    break;
                                case "MARKETCAPITALIZATION":
                                    double MarketCapitalization = 0;
                                    if (double.TryParse(value, out MarketCapitalization))
                                    {
                                        symbolData.MarketCapitalization = MarketCapitalization;
                                    }
                                    break;
                                case "MARKPRICE":
                                case "YESTERDAYCLOSINGMARK":
                                    double MarkPrice = 0;
                                    if (double.TryParse(value, out MarkPrice))
                                    {
                                        symbolData.MarkPrice = MarkPrice;
                                    }
                                    break;
                                case "MARKPRICESTR":
                                    symbolData.MarkPriceStr = value;
                                    break;
                                case "MID":
                                    double Mid = 0;
                                    if (double.TryParse(value, out Mid))
                                    {
                                        symbolData.Mid = Mid;
                                    }
                                    break;
                                case "OPEN":
                                    double Open = 0;
                                    if (double.TryParse(value, out Open))
                                    {
                                        symbolData.Open = Open;
                                    }
                                    break;
                                case "OPENINTEREST":
                                    double OpenInterest = 0;
                                    if (double.TryParse(value, out OpenInterest))
                                    {
                                        symbolData.OpenInterest = OpenInterest;
                                    }
                                    break;
                                case "OPRASYMBOL":
                                    symbolData.OpraSymbol = value;
                                    break;
                                case "OSIOptionSymbol":
                                    symbolData.OSIOptionSymbol = value;
                                    break;
                                case "PCTCHANGE":
                                    double PctChange = 0;
                                    if (double.TryParse(value, out PctChange))
                                    {
                                        symbolData.PctChange = PctChange;
                                    }
                                    break;
                                case "PREFERENCEDPRICE":
                                    double PreferencedPrice = 0;
                                    if (double.TryParse(value, out PreferencedPrice))
                                    {
                                        symbolData.PreferencedPrice = PreferencedPrice;
                                    }
                                    break;
                                case "PREVIOUS":
                                    double Previous = 0;
                                    if (double.TryParse(value, out Previous))
                                    {
                                        symbolData.Previous = Previous;
                                    }
                                    break;
                                case "PRICINGPROVIDER":
                                    MarketDataProvider PricingProvider = 0;
                                    if (Enum.TryParse<MarketDataProvider>(value, true, out PricingProvider))
                                    {
                                        symbolData.MarketDataProvider = PricingProvider;
                                    }
                                    break;
                                case "PRICINGSOURCE":
                                    switch (value)
                                    {
                                        case "BBGDLWS":
                                            symbolData.PricingSource = PricingSource.BloombergDLWS;
                                            break;
                                        case "BLOOMBERG":
                                            symbolData.PricingSource = PricingSource.ImportFileBloomberg;
                                            break;
                                        case "CUSTOM":
                                            symbolData.PricingSource = PricingSource.ImportFileCustom;
                                            break;
                                        default:
                                            symbolData.PricingSource = PricingSource.Gateway;
                                            break;
                                    }
                                    break;
                                case "PUTORCALL":
                                    OptionType PutOrCall;
                                    if (Enum.TryParse<OptionType>(value, true, out PutOrCall))
                                    {
                                        symbolData.PutOrCall = PutOrCall;
                                    }
                                    break;
                                case "REQUESTEDSYMBOLOGY":
                                    ApplicationConstants.SymbologyCodes RequestedSymbology;
                                    if (Enum.TryParse<ApplicationConstants.SymbologyCodes>(value, true, out RequestedSymbology))
                                    {
                                        symbolData.RequestedSymbology = RequestedSymbology;
                                    }
                                    break;
                                case "REUTERSYMBOL":
                                    symbolData.ReuterSymbol = value;
                                    break;
                                case "RHO":
                                    double Rho = 0;
                                    if (double.TryParse(value, out Rho))
                                    {
                                        symbolData.Rho = Rho;
                                    }
                                    break;
                                case "SEDOLSYMBOL":
                                    symbolData.SedolSymbol = value;
                                    break;
                                case "SELECTEDFEEDPRICE":
                                    double SelectedFeedPrice = 0;
                                    if (double.TryParse(value, out SelectedFeedPrice))
                                    {
                                        symbolData.SelectedFeedPrice = SelectedFeedPrice;
                                    }
                                    break;
                                case "SHARESOUTSTANDING":
                                    long SharesOutstanding = 0;
                                    if (long.TryParse(value, out SharesOutstanding))
                                    {
                                        symbolData.SharesOutstanding = SharesOutstanding;
                                    }
                                    break;
                                case "SPREAD":
                                    double Spread = 0;
                                    if (double.TryParse(value, out Spread))
                                    {
                                        symbolData.Spread = Spread;
                                    }
                                    break;
                                case "STRIKEPRICE":
                                    double StrikePrice = 0;
                                    if (double.TryParse(value, out StrikePrice))
                                    {
                                        symbolData.StrikePrice = StrikePrice;
                                    }
                                    break;
                                case "SYMBOL":
                                    switch (symbology)
                                    {
                                        case ApplicationConstants.SymbologyCodes.TickerSymbol:
                                            symbolData.Symbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.ReutersSymbol:
                                            symbolData.ReuterSymbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.ISINSymbol:
                                            symbolData.ISIN = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.SEDOLSymbol:
                                            symbolData.SedolSymbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.CUSIPSymbol:
                                            symbolData.CusipNo = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.BloombergSymbol:
                                            symbolData.BloombergSymbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                                            symbolData.OSIOptionSymbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:
                                            symbolData.IDCOOptionSymbol = value;
                                            break;
                                        case ApplicationConstants.SymbologyCodes.OPRAOptionSymbol:
                                            symbolData.OpraSymbol = value;
                                            break;
                                        default:
                                            symbolData.Symbol = value;
                                            break;
                                    }
                                    break;
                                case "THEORETICALPRICE":
                                    double TheoreticalPrice;
                                    if (double.TryParse(value, out TheoreticalPrice))
                                    {
                                        symbolData.TheoreticalPrice = TheoreticalPrice;
                                    }
                                    break;
                                case "THETA":
                                    double Theta;
                                    if (double.TryParse(value, out Theta))
                                    {
                                        symbolData.Theta = Theta;
                                    }
                                    break;
                                case "TOTALVOLUME":
                                    long TotalVolume;
                                    if (long.TryParse(value, out TotalVolume))
                                    {
                                        symbolData.TotalVolume = TotalVolume;
                                    }
                                    break;
                                case "TRADEVOLUME":
                                    long TradeVolume;
                                    if (long.TryParse(value, out TradeVolume))
                                    {
                                        symbolData.TradeVolume = TradeVolume;
                                    }
                                    break;
                                case "UNDERLYINGCATEGORY":
                                    Underlying UnderlyingCategory;
                                    if (Enum.TryParse<Underlying>(value, true, out UnderlyingCategory))
                                    {
                                        symbolData.UnderlyingCategory = UnderlyingCategory;
                                    }
                                    break;
                                case "UNDERLYINGSYMBOL":
                                    symbolData.UnderlyingSymbol = value;
                                    break;
                                case "UPDATETIME":
                                    DateTime UpdateTime = DateTimeConstants.MinValue;
                                    if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out UpdateTime))
                                    {
                                        symbolData.UpdateTime = UpdateTime;
                                    }
                                    break;
                                case "VEGA":
                                    double Vega = 0;
                                    if (double.TryParse(value, out Vega))
                                    {
                                        symbolData.Vega = Vega;
                                    }
                                    break;
                                case "VOLUME10DAVG":
                                    double Volume10DAvg = 0;
                                    if (double.TryParse(value, out Volume10DAvg))
                                    {
                                        symbolData.Volume10DAvg = Volume10DAvg;
                                    }
                                    break;
                                case "VWAP":
                                    double VWAP = 0;
                                    if (double.TryParse(value, out VWAP))
                                    {
                                        symbolData.VWAP = VWAP;
                                    }
                                    break;
                                case "XDIVIDENDDATE":
                                    DateTime XDividendDate = DateTimeConstants.MinValue;
                                    if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out XDividendDate))
                                    {
                                        symbolData.XDividendDate = XDividendDate;
                                    }
                                    break;
                            }
                        }
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
        private readonly static object _locker = new object();
        /// <summary>
        /// 
        /// </summary>
        private static void LoadDictionary()
        {
            try
            {
                if (_dictColumnMapping == null)
                {
                    if (File.Exists(_mappingFilePath))
                    {
                        if (Prana.Utilities.IO.File.IsFileOpen(_mappingFilePath))
                        {
                            using (FileStream fs = File.OpenRead(_mappingFilePath))
                            {
                                XmlSerializer reconFiltersSerializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                                _dictColumnMapping = (SerializableDictionary<string, string>)reconFiltersSerializer.Deserialize(fs);
                            }
                        }
                    }
                    else
                    {
                        _dictColumnMapping = GetPropertiesNameOfSymbolData();
                        WriteColumnMappingXml();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static SerializableDictionary<string, string> GetPropertiesNameOfSymbolData()
        {
            SerializableDictionary<string, string> dictProperty = new SerializableDictionary<string, string>();
            try
            {
                foreach (var prop in (new SymbolData()).GetType().GetProperties())
                {
                    dictProperty.Add(prop.Name, prop.Name);
                }
                dictProperty.Add("YESTERDAYCLOSINGMARK", "MarkPrice");
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
            return dictProperty;
        }
        /// <summary>
        /// 
        /// </summary>
        private static void WriteColumnMappingXml()
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(_mappingFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_mappingFilePath));
                }
                if (Prana.Utilities.IO.File.IsFileOpen(_mappingFilePath))
                {
                    using (XmlTextWriter writer = new XmlTextWriter(_mappingFilePath, Encoding.UTF8))
                    {
                        writer.Formatting = Formatting.Indented;
                        XmlSerializer serializer;
                        serializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
                        serializer.Serialize(writer, _dictColumnMapping);
                        writer.Flush();
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
    }
}
