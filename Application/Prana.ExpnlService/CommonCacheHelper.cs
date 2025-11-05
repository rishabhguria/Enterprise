using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Data;

namespace Prana.ExpnlService
{
    internal class CommonCacheHelper
    {
        private static Dictionary<int, ConversionRate> _yesterdayFXRates = new Dictionary<int, ConversionRate>();

        private static SelectedFeedPrice _selectedFeedPrice = SelectedFeedPrice.Last;
        internal static SelectedFeedPrice SelectedFeedPrice
        {
            get { return _selectedFeedPrice; }
            set { _selectedFeedPrice = value; }
        }

        private static bool _lastIfZero = false;
        internal static bool LastIfZero
        {
            get { return _lastIfZero; }
            set { _lastIfZero = value; }
        }

        private static bool _lastIfZeroForOptions = false;
        internal static bool LastIfZeroForOptions
        {
            get { return _lastIfZeroForOptions; }
            set { _lastIfZeroForOptions = value; }
        }

        private static double _xpercentOfAvgVolume = 100;
        internal static double XPercentOfAvgVolume
        {
            get { return _xpercentOfAvgVolume; }
            set { _xpercentOfAvgVolume = value; }
        }

        internal static DataTable GetIndicesReturnTable(DataSet indexDS)
        {
            try
            {
                if (indexDS == null || indexDS.Tables == null || indexDS.Tables[0].Rows == null || indexDS.Tables[0].Rows.Count == 0)
                {
                    return null;
                }

                DataTable dtIndicesReturn = new DataTable("IndicesReturn");
                foreach (DataRow row in indexDS.Tables[0].Rows)
                {
                    foreach (string enumValue in Enum.GetNames(typeof(Duration)))
                    {
                        DataColumn col = new DataColumn();
                        col.ColumnName = row["IndexSymbol"].ToString() + "_" + enumValue;
                        col.DataType = typeof(double);
                        col.Caption = row["ShortName"].ToString() + " " + enumValue;
                        dtIndicesReturn.Columns.Add(col);
                    }
                }

                DataRow dr = dtIndicesReturn.NewRow();
                dtIndicesReturn.Rows.Add(dr);

                return dtIndicesReturn;
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

                return null;
            }
        }

        internal static ConversionRate GetYesterdayFXRateFromCurrency(int fromCurrencyID)
        {
            try
            {
                lock (_fxDictLockerobj)
                {
                    if (_yesterdayFXRates.ContainsKey(fromCurrencyID))
                    {
                        return _yesterdayFXRates[fromCurrencyID];
                    }
                    else
                    {
                        return null;
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

                return null;
            }
        }

        private static object _fxDictLockerobj = new object();

        internal static void LoadYesterdayFXRates(string yesterdayAUECString)
        {
            try
            {
                Dictionary<int, ConversionRate> tempFXRateDict = DatabaseManager.GetInstance().GetYesterdayFXRates(yesterdayAUECString);

                lock (_fxDictLockerobj)
                {
                    _yesterdayFXRates = tempFXRateDict;
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

        internal static EPnlOrder GetEPnlOrderFromTaxlot(TaxLot taxLot)
        {
            try
            {
                EPnlOrder epOrder;
                AssetCategory origAssetCategory = (AssetCategory)taxLot.AssetID;
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(origAssetCategory);

                switch (origAssetCategory)
                {
                    case AssetCategory.FXForward:
                        epOrder = new EPnLOrderFXForward();
                        ((EPnLOrderFXForward)epOrder).LeadCurrencyID = taxLot.LeadCurrencyID;
                        ((EPnLOrderFXForward)epOrder).VsCurrencyID = taxLot.VsCurrencyID;
                        ((EPnLOrderFuture)epOrder).ExpirationDate = taxLot.ExpirationDate.ToShortDateString();
                        ((EPnLOrderFXForward)epOrder).ExpirationDate = taxLot.ExpirationDate.ToShortDateString();
                        ((EPnLOrderFXForward)epOrder).LeveragedFactor = taxLot.UnderlyingDelta;
                        ((EPnLOrderFXForward)epOrder).CounterCurrencyID = taxLot.CurrencyID == taxLot.LeadCurrencyID ? taxLot.VsCurrencyID : taxLot.LeadCurrencyID;
                        break;

                    default:
                        switch (baseAssetCategory)
                        {
                            case AssetCategory.Equity:
                                if (taxLot.SwapParameters == null)
                                {
                                    epOrder = new EPnLOrderEquity();
                                }
                                else
                                {
                                    epOrder = new EPnLOrderEquitySwap();
                                    ((EPnLOrderEquitySwap)epOrder).SwapParameters = taxLot.SwapParameters.Clone();
                                    epOrder.IsSwapped = taxLot.ISSwap;
                                }
                                epOrder.LeveragedFactor = taxLot.UnderlyingDelta;
                                if (epOrder.LeveragedFactor.Equals(0D))
                                {
                                    string message = "Leverage Factor zero for Equity taxlot details :" + Environment.NewLine + LogExtensions.ToLoggerString(taxLot);
                                    Logger.LoggerWrite(message + Environment.StackTrace, LoggingConstants.CATEGORY_GENERAL);
                                }
                                break;

                            case AssetCategory.Option:
                                epOrder = new EPnLOrderOption();

                                switch (taxLot.PutOrCall)
                                {
                                    case (int)Prana.BusinessObjects.AppConstants.OptionType.CALL:
                                        ((EPnLOrderOption)epOrder).ContractType = Prana.BusinessObjects.AppConstants.OptionType.CALL.ToString().ToUpper();
                                        break;

                                    case (int)Prana.BusinessObjects.AppConstants.OptionType.PUT:
                                        ((EPnLOrderOption)epOrder).ContractType = Prana.BusinessObjects.AppConstants.OptionType.PUT.ToString().ToUpper();
                                        break;

                                    default:

                                        break;
                                }
                                ((EPnLOrderOption)epOrder).LeveragedFactor = taxLot.UnderlyingDelta;
                                ((EPnLOrderOption)epOrder).ExpirationDate = taxLot.ExpirationDate.ToShortDateString();
                                ((EPnLOrderOption)epOrder).StrikePrice = taxLot.StrikePrice;
                                ((EPnLOrderOption)epOrder).IsCurrencyFuture = taxLot.IsCurrencyFuture;
                                break;

                            case AssetCategory.Future:

                                epOrder = new EPnLOrderFuture();
                                ((EPnLOrderFuture)epOrder).ExpirationDate = taxLot.ExpirationDate.ToShortDateString();
                                ((EPnLOrderFuture)epOrder).LeveragedFactor = taxLot.UnderlyingDelta;
                                ((EPnLOrderFuture)epOrder).IsCurrencyFuture = taxLot.IsCurrencyFuture;
                                break;

                            case AssetCategory.FX:

                                epOrder = new EPnLOrderFX();
                                ((EPnLOrderFX)epOrder).LeadCurrencyID = taxLot.LeadCurrencyID;
                                ((EPnLOrderFX)epOrder).VsCurrencyID = taxLot.VsCurrencyID;
                                ((EPnLOrderFX)epOrder).LeveragedFactor = taxLot.UnderlyingDelta;
                                ((EPnLOrderFX)epOrder).CounterCurrencyID = taxLot.CurrencyID == taxLot.LeadCurrencyID ? taxLot.VsCurrencyID : taxLot.LeadCurrencyID;
                                break;

                            case AssetCategory.FixedIncome:
                            case AssetCategory.ConvertibleBond:
                                epOrder = new EPnLOrderFixedIncome();
                                ((EPnLOrderFixedIncome)epOrder).AccruedInterest = taxLot.AccruedInterest;
                                ((EPnLOrderFixedIncome)epOrder).ExpirationDate = taxLot.ExpirationDate.ToShortDateString();
                                ((EPnLOrderFixedIncome)epOrder).LeveragedFactor = taxLot.UnderlyingDelta;
                                break;

                            default:
                                epOrder = new EPnLOrderEquity();
                                break;
                        }
                        break;
                }
                epOrder.ProxySymbol = taxLot.ProxySymbol;
                epOrder.IsinSymbol = taxLot.ISINSymbol;
                epOrder.IdcoSymbol = taxLot.IDCOSymbol;
                epOrder.OsiSymbol = taxLot.OSISymbol;
                epOrder.SedolSymbol = taxLot.SEDOLSymbol;
                epOrder.CusipSymbol = taxLot.CusipSymbol;
                epOrder.BloombergSymbol = taxLot.BloombergSymbol;
                epOrder.FactSetSymbol = taxLot.FactSetSymbol;
                epOrder.ActivSymbol = taxLot.ActivSymbol;
                //Checking if CompanyUserName available then set user name. //PRANA-37936
                if (!string.IsNullOrWhiteSpace(taxLot.CompanyUserName))
                {
                    epOrder.UserName = taxLot.CompanyUserName;
                }
                else
                {
                    epOrder.UserName = CachedDataManager.GetInstance.GetUserText(taxLot.CompanyUserID);
                }
                epOrder.CounterPartyName = CachedDataManager.GetInstance.GetCounterPartyText(taxLot.CounterPartyID);

                epOrder.CurrencyID = taxLot.CurrencyID;
                //epOrder.AssetID = taxLot.AssetID;
                epOrder.AUECID = taxLot.AUECID;
                epOrder.Asset = (AssetCategory)taxLot.AssetID;
                epOrder.AssetName = epOrder.Asset.ToString();

                epOrder.AvgPrice = taxLot.AvgPrice;
                //we are using parentCLorderID as indentifier for Trades :o
                epOrder.ID = taxLot.TaxLotID;
                //epOrder.CompanyUserID = taxLot.CompanyUserID;

                if (taxLot.FXRate != 0.0)
                {
                    //by default if the fxrate is 0 or 1, so it does not matter whether we divide or multiply
                    if (String.IsNullOrEmpty(taxLot.FXConversionMethodOperator.Trim()))
                    {
                        taxLot.FXConversionMethodOperator = "M";
                    }

                    if (taxLot.FXConversionMethodOperator.Equals("D"))
                    {
                        epOrder.FXConversionMethodOnTradeDate = Operator.M;
                        epOrder.FXRateOnTradeDate = 1 / taxLot.FXRate;
                        epOrder.FxRate = 1 / taxLot.FXRate;
                    }
                    else
                    {
                        epOrder.FXConversionMethodOnTradeDate = Operator.M;
                        epOrder.FXRateOnTradeDate = taxLot.FXRate;
                        epOrder.FxRate = taxLot.FXRate;
                    }
                    epOrder.IsFXRateSavedWithTrade = true;
                }
                else
                {
                    epOrder.IsFXRateSavedWithTrade = false;
                }

                //epOrder.CumQty = taxLot.CumQty;
                //epOrder.Quantity = taxLot.Quantity;

                if (taxLot.Level1ID == int.MinValue || taxLot.Level1ID == 0)
                {
                    // following code required as unallocated tab has key (account) -1
                    epOrder.Level1ID = -1;
                }
                else
                {
                    epOrder.Level1ID = taxLot.Level1ID;
                }

                epOrder.Quantity = taxLot.TaxLotQty;
                epOrder.OrderSideTagValue = taxLot.OrderSideTagValue;
                epOrder.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(epOrder.OrderSideTagValue);

                epOrder.Symbol = taxLot.Symbol.Trim().ToUpper();

                epOrder.Level2ID = taxLot.Level2ID;

                epOrder.TransactionDate = taxLot.ProcessDate;
                epOrder.AUECLocalDate = taxLot.AUECLocalDate;

                if (!(string.IsNullOrEmpty(taxLot.UnderlyingSymbol)))
                {
                    epOrder.UnderlyingSymbol = taxLot.UnderlyingSymbol.Trim().ToUpper();
                }
                epOrder.Description = taxLot.Description;
                epOrder.InternalComments = taxLot.InternalComments;
                epOrder.ExchangeID = taxLot.ExchangeID;
                epOrder.ExchangeName = taxLot.ExchangeName;
                epOrder.Multiplier = taxLot.ContractMultiplier;
                epOrder.FullSecurityName = taxLot.CompanyName;
                Prana.BusinessLogic.Calculations.SetLongShort(epOrder);
                epOrder.SideMultiplier = Prana.BusinessLogic.Calculations.GetSideMultilpier(epOrder.OrderSideTagValue);
                epOrder.UnderlyingID = taxLot.UnderlyingID;
                epOrder.UnderlyingName = CachedDataManager.GetInstance.GetAssetText(epOrder.UnderlyingID);
                epOrder.SettlementDate = taxLot.SettlementDate;
                epOrder.SharesOutstanding = taxLot.SharesOutstanding;
                epOrder.TradeAttribute1 = taxLot.TradeAttribute1;
                epOrder.TradeAttribute2 = taxLot.TradeAttribute2;
                epOrder.TradeAttribute3 = taxLot.TradeAttribute3;
                epOrder.TradeAttribute4 = taxLot.TradeAttribute4;
                epOrder.TradeAttribute5 = taxLot.TradeAttribute5;
                epOrder.TradeAttribute6 = taxLot.TradeAttribute6;
                epOrder.ProxySymbol = taxLot.ProxySymbol;
                epOrder.TransactionType = taxLot.TransactionType;
                epOrder.ReutersSymbol = taxLot.ReutersSymbol;
                epOrder.OrderTypeTagValue = taxLot.OrderTypeTagValue;

                #region Compliance Section

                try
                {
                    epOrder.CounterPartyId = taxLot.CounterPartyID;
                    //in case of undefined venue it is selected -1 by default
                    if (taxLot.VenueID != int.MinValue)
                    {
                        epOrder.VenueId = taxLot.VenueID;
                        epOrder.Venue = CommonDataCache.CachedDataManager.GetInstance.GetVenueText(epOrder.VenueId);
                    }
                    else
                        epOrder.VenueId = -1;

                    if (epOrder.OrderTypeTagValue != string.Empty)
                    {
                        epOrder.OrderType = TagDatabaseManager.GetInstance.GetOrderTypeText(epOrder.OrderTypeTagValue);
                    }
                }
                catch (Exception ex)
                {
                    Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                }

                #endregion Compliance Section

                #region Add UDA Symbol Data Section

                epOrder.UDAAsset = taxLot.UDAAsset;
                epOrder.UDASecurityType = taxLot.SecurityTypeName;
                epOrder.UDASector = taxLot.SectorName;
                epOrder.UDASubSector = taxLot.SubSectorName;
                epOrder.UDACountry = taxLot.CountryName;

                #endregion Add UDA Symbol Data Section

                Calculations.SetDefaultPositionSideExposureBoxed(epOrder);

                #region Dyanmic-UDA

                epOrder.Analyst = taxLot.Analyst;
                epOrder.CountryOfRisk = taxLot.CountryOfRisk;
                epOrder.CustomUDA1 = taxLot.CustomUDA1;
                epOrder.CustomUDA2 = taxLot.CustomUDA2;
                epOrder.CustomUDA3 = taxLot.CustomUDA3;
                epOrder.CustomUDA4 = taxLot.CustomUDA4;
                epOrder.CustomUDA5 = taxLot.CustomUDA5;
                epOrder.CustomUDA6 = taxLot.CustomUDA6;
                epOrder.CustomUDA7 = taxLot.CustomUDA7;
                epOrder.Issuer = taxLot.Issuer;
                epOrder.LiquidTag = taxLot.LiquidTag;
                epOrder.MarketCap = taxLot.MarketCap;
                epOrder.Region = taxLot.Region;
                epOrder.RiskCurrency = taxLot.RiskCurrency;
                epOrder.UcitsEligibleTag = taxLot.UcitsEligibleTag;

                #endregion

                epOrder.FxRateDisplay = taxLot.FXRate;
                epOrder.CustomUDA8 = taxLot.CustomUDA8;
                epOrder.CustomUDA9 = taxLot.CustomUDA9;
                epOrder.CustomUDA10 = taxLot.CustomUDA10;
                epOrder.CustomUDA11 = taxLot.CustomUDA11;
                epOrder.CustomUDA12 = taxLot.CustomUDA12;
                epOrder.BloombergSymbolWithExchangeCode = taxLot.BloombergSymbolWithExchangeCode;

                epOrder.TradeAttribute7 = taxLot.TradeAttribute7;
                epOrder.TradeAttribute8 = taxLot.TradeAttribute8;
                epOrder.TradeAttribute9 = taxLot.TradeAttribute9;
                epOrder.TradeAttribute10 = taxLot.TradeAttribute10;
                epOrder.TradeAttribute11 = taxLot.TradeAttribute11;
                epOrder.TradeAttribute12 = taxLot.TradeAttribute12;
                epOrder.TradeAttribute13 = taxLot.TradeAttribute13;
                epOrder.TradeAttribute14 = taxLot.TradeAttribute14;
                epOrder.TradeAttribute15 = taxLot.TradeAttribute15;
                epOrder.TradeAttribute16 = taxLot.TradeAttribute16;
                epOrder.TradeAttribute17 = taxLot.TradeAttribute17;
                epOrder.TradeAttribute18 = taxLot.TradeAttribute18;
                epOrder.TradeAttribute19 = taxLot.TradeAttribute19;
                epOrder.TradeAttribute20 = taxLot.TradeAttribute20;
                epOrder.TradeAttribute21 = taxLot.TradeAttribute21;
                epOrder.TradeAttribute22 = taxLot.TradeAttribute22;
                epOrder.TradeAttribute23 = taxLot.TradeAttribute23;
                epOrder.TradeAttribute24 = taxLot.TradeAttribute24;
                epOrder.TradeAttribute25 = taxLot.TradeAttribute25;
                epOrder.TradeAttribute26 = taxLot.TradeAttribute26;
                epOrder.TradeAttribute27 = taxLot.TradeAttribute27;
                epOrder.TradeAttribute28 = taxLot.TradeAttribute28;
                epOrder.TradeAttribute29 = taxLot.TradeAttribute29;
                epOrder.TradeAttribute30 = taxLot.TradeAttribute30;
                epOrder.TradeAttribute31 = taxLot.TradeAttribute31;
                epOrder.TradeAttribute32 = taxLot.TradeAttribute32;
                epOrder.TradeAttribute33 = taxLot.TradeAttribute33;
                epOrder.TradeAttribute34 = taxLot.TradeAttribute34;
                epOrder.TradeAttribute35 = taxLot.TradeAttribute35;
                epOrder.TradeAttribute36 = taxLot.TradeAttribute36;
                epOrder.TradeAttribute37 = taxLot.TradeAttribute37;
                epOrder.TradeAttribute38 = taxLot.TradeAttribute38;
                epOrder.TradeAttribute39 = taxLot.TradeAttribute39;
                epOrder.TradeAttribute40 = taxLot.TradeAttribute40;
                epOrder.TradeAttribute41 = taxLot.TradeAttribute41;
                epOrder.TradeAttribute42 = taxLot.TradeAttribute42;
                epOrder.TradeAttribute43 = taxLot.TradeAttribute43;
                epOrder.TradeAttribute44 = taxLot.TradeAttribute44;
                epOrder.TradeAttribute45 = taxLot.TradeAttribute45;

                return epOrder;
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

                return null;
            }
        }
    }
}