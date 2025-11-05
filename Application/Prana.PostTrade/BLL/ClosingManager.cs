using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.Constants;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PostTrade.BLL;
using Prana.PostTrade.BLL.CostAdjustment;
using Prana.PostTrade.BusinessObjects;
using Prana.PostTrade.Commission;
using Prana.PubSubService.Interfaces;
using Prana.Utilities;
using Prana.Utilities.MiscUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Prana.PostTrade
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class ClosingManager : IClosingServices, IDisposable
    {
        private ClosingPreferences _preferences = null;
        private DateTime _toClosingDate = DateTime.Now.Date;
        private DateTime _fromClosingDate = DateTime.Now.Date;
        private DateTime _toTradeDate = DateTime.Now.Date;
        private DateTime _fromTradeDate = DateTime.Now.Date;
        private BufferBlock<MessageData> dataBuffer;
        private DateTime _ACALatestCalculationDate = DateTimeConstants.MinValue;
        private Dictionary<string, TaxlotClosingInfo> _closingInfoCache = new Dictionary<string, TaxlotClosingInfo>();
        private readonly object _closingInfoCacheLock = new object();
        Dictionary<string, Tuple<DateTime, int>> _TaxLotIdToCADateDict = new Dictionary<string, Tuple<DateTime, int>>();
        private readonly object _TaxLotIdToCADateDictLock = new object();

        private List<string> _TaxLotIdToCostAdjustDateDict = new List<string>();
        private readonly object _TaxLotIdToCostAdjustDateDictLock = new object();
        #region singleton instance
        private static readonly object _lockerObject = new object();
        private Dictionary<int, DateTime> _dictUserWiseDates = new Dictionary<int, DateTime>();

        public ClosingManager()
        {
            try
            {
                Logger.LoggerWrite("Starting service", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                CreatePublishingProxy();
                RefreshClosingData();
                CostAdjustmentManager.GetInstance().Initialize();
                dataBuffer = new BufferBlock<MessageData>();
                System.Threading.Tasks.Task.Factory.StartNew(() => ConsumeBufferMessageAsync(dataBuffer)).ConfigureAwait(false);
                Logger.LoggerWrite("Started service", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }

        public void RefreshClosingData()
        {
            try
            {
                Logger.LoggerWrite("Refreshing Closing Data", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                GetPreferencesFromDB();
                GetTaxLotsLatestClosingDatesFromDB(true);
                GetTaxLotsLatestCADatesFromDB();
                GetTaxLotsLatestCostAdjustmentDatesFromDB();
                Logger.LoggerWrite("Refreshing Closing Data - Done", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }

        static ProxyBase<IPublishing> _proxy;
        private void CreatePublishingProxy()
        {
            try
            {
                Logger.LoggerWrite("Creating Publishing Proxy.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                _proxy = new ProxyBase<IPublishing>("TradePublishingEndpointAddress");
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }
        #endregion

        #region Variables
        IPranaPositionServices _positionServices = null;
        public IPranaPositionServices positionServices
        {
            set
            {
                _positionServices = value;
                CloseTradesDataManager.positionServices = value;
            }
        }

        private IActivityServices _activityServices;
        const string COL_TradeDate = "TradeDate";
        const string PositionalSide_Long = "Long";
        const string PositionalSide_Short = "Short";
        bool _isSameAccount = true;
        bool _isSameSymbol = true;
        bool _isSameStrategy = true;
        bool _isSameSettlementCurrency = false;
        const string COL_AveragePrice = "AveragePrice";

        #endregion

        public void SetActicityServices(IActivityServices activityServices)
        {
            _activityServices = activityServices;
        }

        #region Private Methods
        /// <summary>
        /// Get Symbol Account Open Qty
        /// </summary>
        /// <param name="Todate"></param>
        /// <param name="symbol"></param>
        /// <param name="accountID"></param>
        /// <returns></returns>
        private double GetSymbolAccountOpenQty(DateTime Todate, string symbol, string accountID)
        {
            try
            {
                string ToAllAUECDatesString = string.Empty;
                ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(Todate);
                Dictionary<string, double> dictSymbolPos = CloseTradesDataManager.GetSymbolAccountOpenQty(ToAllAUECDatesString, symbol, accountID);
                //return only quantity , if long then +ve , if short then -
                if (dictSymbolPos != null && dictSymbolPos.ContainsKey(symbol))
                {
                    return dictSymbolPos[symbol];
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return 0;
        }

        private Dictionary<PositionTag, double> GetSymbolAccountOpenQtyByPositionTag(DateTime Todate, string symbol, string accountID)
        {
            string ToAllAUECDatesString = string.Empty;
            ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(Todate);
            return CloseTradesDataManager.GetSymbolAccountOpenQtyByPositionTag(ToAllAUECDatesString, symbol, accountID);
        }

        public ClosingData CloseDataforPairedTaxlots(List<TaxLot> positionalTaxLots, List<TaxLot> TaxLotsToClose, bool isNotionalChangeClosing)
        {
            Logger.LoggerWrite("Closing Data for Paired Taxlots", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"PositionTaxlots", positionalTaxLots},
                {"TaxLotsToClose", TaxLotsToClose},
                {"isNotionalChangeClosing", isNotionalChangeClosing}
            });

            ClosingData closedData = new ClosingData();
            ClosingData currentClosingData = new ClosingData();
            try
            {
                foreach (TaxLot positionalTaxlot in positionalTaxLots)
                {
                    TaxLot closingTaxlot = TaxLotsToClose.FirstOrDefault(x => positionalTaxlot.ClosingWithTaxlotID != null && x.TaxLotID.Equals(positionalTaxlot.ClosingWithTaxlotID));
                    if (closingTaxlot != null)
                    {
                        positionalTaxlot.ClosingMode = ClosingMode.CorporateAction;
                        closingTaxlot.ClosingMode = ClosingMode.CorporateAction;
                        if (isNotionalChangeClosing)
                        {
                            currentClosingData = CloseTaxlotforNotionalChange(positionalTaxlot, closingTaxlot);
                        }
                        else
                        {
                            // set closing taxlot closingdate on the basis of preferences
                            SetClosingDateBasedOnPreferences(closingTaxlot);
                            // set positional taxlot closingdate on the basis of preferences
                            SetClosingDateBasedOnPreferences(positionalTaxlot);
                            currentClosingData = CloseTaxLot(positionalTaxlot, closingTaxlot, GetPositionTag(positionalTaxlot.OrderSideTagValue), PostTradeEnums.CloseTradeAlogrithm.FIFO, false, false,false);
                        }
                        UpdateClosingData(closedData, currentClosingData, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            return closedData;
        }

        /// <summary>
        /// This is used to close positions on the basis of notional changes, This method is used for Cost Adjustment Closing
        /// </summary>
        /// <param name="positionalTaxLots">buy side taxlots to close</param>
        /// <param name="TaxLotsToClose">sell side taxlots to close</param>
        /// <param name="isNotionalChangeClosing">Normal/Notional change closing</param>
        /// <param name="mode">closing mode</param>
        /// <returns></returns>
        public ClosingData CloseDataforPairedTaxlotsOnMode(List<TaxLot> positionalTaxLots, List<TaxLot> TaxLotsToClose, bool isNotionalChangeClosing, ClosingMode mode)
        {
            Logger.LoggerWrite("Closing Data for Paired Taxlots On Mode: " + mode, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"PositionTaxlots", positionalTaxLots},
                {"TaxLotsToClose", TaxLotsToClose},
                {"isNotionalChangeClosing", isNotionalChangeClosing},
                {"Mode", mode}
            });

            ClosingData closedData = new ClosingData();
            ClosingData currentClosingData = new ClosingData();
            try
            {
                foreach (TaxLot positionalTaxlot in positionalTaxLots)
                {
                    foreach (TaxLot closingTaxlot in TaxLotsToClose)
                    {
                        if (positionalTaxlot.ClosingWithTaxlotID != null && (positionalTaxlot.ClosingWithTaxlotID.Equals(closingTaxlot.TaxLotID)))
                        {
                            if (isNotionalChangeClosing)
                            {
                                positionalTaxlot.ClosingMode = mode;
                                closingTaxlot.ClosingMode = mode;

                                currentClosingData = CloseTaxlotforNotionalChange(positionalTaxlot, closingTaxlot);
                            }
                            else
                            {
                                // set closing taxlot closingdate on the basis of preferences
                                SetClosingDateBasedOnPreferences(closingTaxlot);
                                // set positional taxlot closingdate on the basis of preferences
                                SetClosingDateBasedOnPreferences(positionalTaxlot);
                                positionalTaxlot.ClosingMode = mode;
                                closingTaxlot.ClosingMode = mode;
                                currentClosingData = CloseTaxLot(positionalTaxlot, closingTaxlot, GetPositionTag(positionalTaxlot.OrderSideTagValue), PostTradeEnums.CloseTradeAlogrithm.FIFO, false, false, false);
                            }
                            UpdateClosingData(closedData, currentClosingData, true);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;

            }
            return closedData;
        }

        /// <summary>
        /// Close taxlots on the basis of Notional changes, by Sandeep Singh Feb 5, 2014
        /// </summary>
        /// <param name="positionalTaxLot"></param>
        /// <param name="taxlotToClose"></param>
        /// <returns></returns>
        private ClosingData CloseTaxlotforNotionalChange(TaxLot positionalTaxLot, TaxLot taxlotToClose)
        {
            ClosingData closedData = new ClosingData();
            try
            {
                Position currentPosition = null;

                // set closing taxlot closingdate on the basis of preferences
                SetClosingDateBasedOnPreferences(taxlotToClose);
                // set positional taxlot closingdate on the basis of preferences
                SetClosingDateBasedOnPreferences(positionalTaxLot);
                currentPosition = GetPosition(positionalTaxLot.TaxLotID);
                ////skip closing date logic
                //positionalTaxLot.ClosingDate = positionalTaxLot.OriginalPurchaseDate;
                //taxlotToClose.ClosingDate = taxlotToClose.OriginalPurchaseDate;             

                //need to check long short logic 
                switch (positionalTaxLot.OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                        currentPosition.PositionalTag = PositionTag.Long;
                        currentPosition.ClosingPositionTag = PositionTag.Short;
                        currentPosition.PositionSide = PositionalSide_Long;
                        break;

                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        currentPosition.PositionalTag = PositionTag.Short;
                        currentPosition.ClosingPositionTag = PositionTag.Long;
                        currentPosition.PositionSide = PositionalSide_Short;
                        break;
                }

                currentPosition.ClosingAlgo = (int)(PostTradeEnums.CloseTradeAlogrithm.FIFO);
                currentPosition.IsNDF = positionalTaxLot.IsNDF;
                currentPosition.LeadCurrencyID = positionalTaxLot.LeadCurrencyID;
                currentPosition.VsCurrencyID = positionalTaxLot.VsCurrencyID;
                currentPosition.FxRate = positionalTaxLot.FXRate;
                currentPosition.IsCurrencySettle = positionalTaxLot.IsCurrencySettle;

                if (positionalTaxLot.TaxLotClosingId != null && positionalTaxLot.TaxLotClosingId != Guid.Empty.ToString())
                {
                    currentPosition.TaxLotClosingId = positionalTaxLot.TaxLotClosingId;
                }
                else
                {
                    currentPosition.TaxLotClosingId = Guid.NewGuid().ToString();
                    positionalTaxLot.TaxLotClosingId = currentPosition.TaxLotClosingId;
                    taxlotToClose.TaxLotClosingId = currentPosition.TaxLotClosingId;
                }
                currentPosition.ClosingID = taxlotToClose.TaxLotID;
                currentPosition.OpenAveragePrice = positionalTaxLot.AvgPrice;
                currentPosition.ClosedAveragePrice = taxlotToClose.AvgPrice;

                currentPosition.Side = positionalTaxLot.OrderSide;
                currentPosition.ClosingSide = taxlotToClose.OrderSide;
                currentPosition.Currency = CachedDataManager.GetInstance.GetCurrencyText(positionalTaxLot.CurrencyID);
                currentPosition.Exchange = CachedDataManager.GetInstance.GetExchangeText(CachedDataManager.GetInstance.GetExchangeIdFromAUECId(positionalTaxLot.AUECID));
                currentPosition.UnderlyingName = positionalTaxLot.UnderlyingName;
                currentPosition.AssetCategoryValue = (AssetCategory)CachedDataManager.GetInstance.GetAssetIdByAUECId(positionalTaxLot.AUECID);
                currentPosition.Symbol = positionalTaxLot.Symbol;
                currentPosition.AccountValue.ID = positionalTaxLot.Level1ID;
                currentPosition.AccountValue.FullName = positionalTaxLot.Level1Name;
                currentPosition.AccountValue.ShortName = positionalTaxLot.Level1Name;
                currentPosition.Strategy = positionalTaxLot.Level2Name;
                currentPosition.TimeOfSaveUTC = DateTime.UtcNow;
                currentPosition.ClosingMode = ClosingMode.CorporateAction;

                positionalTaxLot.TimeOfSaveUTC = currentPosition.TimeOfSaveUTC;
                taxlotToClose.TimeOfSaveUTC = currentPosition.TimeOfSaveUTC;

                if (positionalTaxLot.ProcessDate > taxlotToClose.ProcessDate)
                {
                    currentPosition.TradeDate = taxlotToClose.ProcessDate;
                    currentPosition.ClosingTradeDate = positionalTaxLot.ProcessDate;
                }
                else
                {
                    currentPosition.TradeDate = positionalTaxLot.ProcessDate;
                    currentPosition.ClosingTradeDate = taxlotToClose.ProcessDate;
                }

                positionalTaxLot.AUECModifiedDate = currentPosition.ClosingTradeDate;
                taxlotToClose.AUECModifiedDate = currentPosition.ClosingTradeDate;

                currentPosition.Multiplier = positionalTaxLot.ContractMultiplier;
                if (positionalTaxLot.SwapParameters != null)
                {
                    currentPosition.IsPositionSwapped = true;
                    currentPosition.PositionSwapParameters = positionalTaxLot.SwapParameters.Clone();
                }
                if (taxlotToClose.SwapParameters != null)
                {
                    currentPosition.IsClosedSwapped = true;
                    currentPosition.ClosedSwapParameters = taxlotToClose.SwapParameters.Clone();
                }

                #region notional change

                if (positionalTaxLot.NotionalChange > 0)
                {
                    double closingQuantity;
                    if (positionalTaxLot.TaxLotQty > taxlotToClose.TaxLotQty)
                        closingQuantity = taxlotToClose.TaxLotQty;
                    else
                        closingQuantity = positionalTaxLot.TaxLotQty;
                    currentPosition.ClosedQty = closingQuantity;
                    taxlotToClose.TaxLotQty -= closingQuantity;

                    double closingNotionalChange;
                    if (positionalTaxLot.NotionalChange > taxlotToClose.NotionalChange)
                        closingNotionalChange = taxlotToClose.NotionalChange;
                    else
                        closingNotionalChange = positionalTaxLot.NotionalChange;
                    //update Notional Change
                    currentPosition.NotionalChange = closingNotionalChange;
                    //Buy i.e. opening trades
                    positionalTaxLot.NotionalChange -= taxlotToClose.NotionalChange;
                    //Sell i.e. closing trades
                    taxlotToClose.NotionalChange -= closingNotionalChange;
                    // update commissions
                    double closingCommissionPositionalTaxLot = positionalTaxLot.OpenTotalCommissionandFees;
                    double closingCommissionClosingTaxLot = taxlotToClose.OpenTotalCommissionandFees;
                    // opening trade commission
                    // open commission will be reduced by closing trade commission, closing trade commission is coming in -ve
                    //positionalTaxLot.OpenTotalCommissionandFees += closingCommissionClosingTaxLot;
                    // there is no commission cosideration while closing Notional Change, we have considered here Net Notional
                    //positionalTaxLot.OpenTotalCommissionandFees = 0;
                    // Sandeep Singh_09DEC2014: As commission for withdrawal taxlot is always negative, so closingCommissionClosingTaxLot is added here
                    // i.e. (- closingCommissionClosingTaxLot) is coming with negative sign
                    // positionalTaxLot.OpenTotalCommissionandFees = positionalTaxLot.OpenTotalCommissionandFees - closingCommissionClosingTaxLot;
                    positionalTaxLot.OpenTotalCommissionandFees = positionalTaxLot.OpenTotalCommissionandFees + closingCommissionClosingTaxLot;
                    positionalTaxLot.ClosedTotalCommissionandFees = closingCommissionClosingTaxLot;// closingCommissionClosingTaxLot * (-1);
                    // closing trade commission                   
                    taxlotToClose.OpenTotalCommissionandFees -= closingCommissionClosingTaxLot;
                    taxlotToClose.ClosedTotalCommissionandFees = closingCommissionClosingTaxLot;
                    // average price will be updated when spin-off CA applied
                    if (positionalTaxLot.TaxLotQty != 0)
                    {
                        positionalTaxLot.AvgPrice = positionalTaxLot.NotionalChange / positionalTaxLot.TaxLotQty;
                    }

                    currentPosition.PositionTotalCommissionandFees = closingCommissionClosingTaxLot;
                    currentPosition.ClosingTotalCommissionandFees = closingCommissionClosingTaxLot;
                }

                #endregion

                closedData.Taxlots.Add(taxlotToClose);
                closedData.Taxlots.Add(positionalTaxLot);
                closedData.ClosedPositions.Add(currentPosition);

                closedData.UnSavedTaxlots.Add((TaxLot)(taxlotToClose.Clone()));
                closedData.UnSavedTaxlots.Add((TaxLot)(positionalTaxLot.Clone()));

                positionalTaxLot.TaxLotClosingId = Guid.Empty.ToString();
                taxlotToClose.TaxLotClosingId = Guid.Empty.ToString();

                ////prevent to add taxlots to closingInfoCache when taxlots are populated from CloseTradeInformation UI
                //if (!positionalTaxLot.ClosingMode.Equals(ClosingMode.CorporateAction))
                //{
                //    AddToClosingInfoCache(positionalTaxLot, TaxLotToClose);
                //}                
            }
            catch (Exception)
            {
                throw;
            }
            return closedData;
        }

        private ClosingData CloseTaxLot(TaxLot positionalTaxLot, TaxLot TaxLotToClose, PositionTag positionType, PostTradeEnums.CloseTradeAlogrithm algorithm, bool isVirtualClosingPopulate, bool isOverrideWithUserClosing, bool IsCopyOpeningTradeAttributes)
        {
            ClosingData closedData = new ClosingData();
            try
            {
                Position currentPosition = null;

                currentPosition = GetPosition(positionalTaxLot.TaxLotID);

                currentPosition.ClosingAlgo = (int)algorithm;
                currentPosition.IsNDF = positionalTaxLot.IsNDF;
                currentPosition.LeadCurrencyID = positionalTaxLot.LeadCurrencyID;
                currentPosition.VsCurrencyID = positionalTaxLot.VsCurrencyID;
                currentPosition.FxRate = positionalTaxLot.FXRate;
                currentPosition.IsCurrencySettle = positionalTaxLot.IsCurrencySettle;
                double closingQuantity;
                if (positionalTaxLot.TaxLotClosingId != null && positionalTaxLot.TaxLotClosingId != Guid.Empty.ToString())
                {
                    currentPosition.TaxLotClosingId = positionalTaxLot.TaxLotClosingId;
                }
                else
                {
                    currentPosition.TaxLotClosingId = Guid.NewGuid().ToString();
                    positionalTaxLot.TaxLotClosingId = currentPosition.TaxLotClosingId;
                    TaxLotToClose.TaxLotClosingId = currentPosition.TaxLotClosingId;
                }
                currentPosition.ClosingID = TaxLotToClose.TaxLotID;


                //currentPosition.OpenAveragePrice = positionalTaxLot.AvgPrice;
                //currentPosition.ClosedAveragePrice = TaxLotToClose.AvgPrice;
                currentPosition.OpenAveragePrice = positionalTaxLot.AvgPrice;

                currentPosition.ClosedAveragePrice = TaxLotToClose.AvgPrice;
                currentPosition.PositionalTag = positionType;
                if (positionType == PositionTag.Long)
                {
                    currentPosition.ClosingPositionTag = PositionTag.Short;
                    if (positionalTaxLot.ClosingDate.Date <= TaxLotToClose.ClosingDate.Date)
                    {
                        currentPosition.PositionSide = PositionalSide_Long;
                    }
                    else
                    {
                        currentPosition.PositionSide = PositionalSide_Short;
                    }
                }
                else
                {
                    currentPosition.ClosingPositionTag = PositionTag.Long;
                    if (positionalTaxLot.ClosingDate.Date <= TaxLotToClose.ClosingDate.Date)
                    {
                        currentPosition.PositionSide = PositionalSide_Short;
                    }
                    else
                    {
                        currentPosition.PositionSide = PositionalSide_Long;
                    }
                }
                currentPosition.Side = positionalTaxLot.OrderSide;
                currentPosition.ClosingSide = TaxLotToClose.OrderSide;
                currentPosition.Currency = CachedDataManager.GetInstance.GetCurrencyText(positionalTaxLot.CurrencyID);
                currentPosition.Exchange = CachedDataManager.GetInstance.GetExchangeText(CachedDataManager.GetInstance.GetExchangeIdFromAUECId(positionalTaxLot.AUECID));
                currentPosition.UnderlyingName = positionalTaxLot.UnderlyingName;
                currentPosition.CurrencyID = positionalTaxLot.CurrencyID;
                currentPosition.AssetCategoryValue = (AssetCategory)CachedDataManager.GetInstance.GetAssetIdByAUECId(positionalTaxLot.AUECID);
                currentPosition.Symbol = positionalTaxLot.Symbol;
                //currentPosition.AccountValue = positionalTaxLot.AccountValue;
                currentPosition.AccountValue.ID = positionalTaxLot.Level1ID;
                currentPosition.AccountValue.FullName = positionalTaxLot.Level1Name;
                currentPosition.AccountValue.ShortName = positionalTaxLot.Level1Name;
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-10513
                currentPosition.Strategy = TaxLotToClose.Level2Name;
                currentPosition.TimeOfSaveUTC = DateTime.UtcNow;
                if (positionalTaxLot.ClosingMode.Equals(ClosingMode.None))
                {
                    currentPosition.ClosingMode = ClosingMode.Offset;
                }
                else
                {
                    currentPosition.ClosingMode = positionalTaxLot.ClosingMode;
                    currentPosition.IsExpired_Settled = true;
                }

                positionalTaxLot.TimeOfSaveUTC = currentPosition.TimeOfSaveUTC;
                TaxLotToClose.TimeOfSaveUTC = currentPosition.TimeOfSaveUTC;
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-3429
                if (positionalTaxLot.ClosingDate > TaxLotToClose.ClosingDate)
                {
                    currentPosition.TradeDate = TaxLotToClose.ClosingDate;
                    currentPosition.ClosingTradeDate = positionalTaxLot.ClosingDate;

                }
                else
                {
                    currentPosition.TradeDate = positionalTaxLot.ClosingDate;
                    currentPosition.ClosingTradeDate = TaxLotToClose.ClosingDate;
                }

                positionalTaxLot.AUECModifiedDate = currentPosition.ClosingTradeDate;
                TaxLotToClose.AUECModifiedDate = currentPosition.ClosingTradeDate;

                currentPosition.Multiplier = positionalTaxLot.ContractMultiplier;
                if (positionalTaxLot.SwapParameters != null)
                {
                    currentPosition.IsPositionSwapped = true;
                    currentPosition.PositionSwapParameters = positionalTaxLot.SwapParameters.Clone();
                }
                if (TaxLotToClose.SwapParameters != null)
                {
                    currentPosition.IsClosedSwapped = true;
                    currentPosition.ClosedSwapParameters = TaxLotToClose.SwapParameters.Clone();
                }
                //Narendra Kumar Jangir May 27 2013
                //closing is done on the basis of user defined close quantities
                //TODO: refractor commision calculation

                //Narendra 2014 Aug 2014
                //Here we are handling ACA fractional closing using TaxLotToClose field
                //in secondary sort field we are filling eligible aca quantity in TaxLotToClose field and here on the basis of configuration and algo we are saving data.
                if (isOverrideWithUserClosing && positionalTaxLot.TaxLotQtyToClose > 0)
                {
                    //in case if sell is allocated to strategies s1(500) and s2(500) having total quantity 1000 
                    //and buy have no strategy allocated, in that case closing quantity will be TaxLotQty of closing taxlot otherwise TaxLotQtyToClose of positional taxlot
                    if (positionalTaxLot.TaxLotQtyToClose > TaxLotToClose.TaxLotQty)
                        closingQuantity = TaxLotToClose.TaxLotQty;
                    else
                        closingQuantity = positionalTaxLot.TaxLotQtyToClose;
                    currentPosition.ClosedQty = closingQuantity;
                    positionalTaxLot.TaxLotQtyToClose -= closingQuantity;
                    TaxLotToClose.TaxLotQtyToClose -= closingQuantity;
                    CalculateCommission(ref positionalTaxLot, ref TaxLotToClose, ref currentPosition, closingQuantity, _preferences.QtyRoundoffDigits);

                }
                else if (positionalTaxLot.TaxLotQty > TaxLotToClose.TaxLotQty)
                {
                    closingQuantity = TaxLotToClose.TaxLotQty;
                    currentPosition.ClosedQty = closingQuantity;
                    //TaxLotQtyToClose is used to populate the closed quantity from CloseTradeAllocationForm
                    positionalTaxLot.TaxLotQtyToClose += closingQuantity;
                    TaxLotToClose.TaxLotQtyToClose += closingQuantity;
                    # region old code for calculate commission
                    ////to adjust notional value for swap
                    //if (positionalTaxLot.ISSwap)
                    //{
                    //    currentPosition.PositionSwapParameters.NotionalValue = positionalTaxLot.SwapParameters.NotionalValue * closingQuantity / positionalTaxLot.TaxLotQty;
                    //}
                    //double closingcommissionpositionalTaxLot = positionalTaxLot.OpenTotalCommissionandFees * closingQuantity / positionalTaxLot.TaxLotQty;

                    //double closingcommissionclosingTaxLot = TaxLotToClose.OpenTotalCommissionandFees;

                    //positionalTaxLot.TaxLotQty -= closingQuantity;
                    //positionalTaxLot.OpenTotalCommissionandFees -= closingcommissionpositionalTaxLot;
                    //TaxLotToClose.TaxLotQty = 0;
                    //TaxLotToClose.OpenTotalCommissionandFees = 0;
                    //currentPosition.PositionTotalCommissionandFees = closingcommissionpositionalTaxLot;
                    //currentPosition.ClosingTotalCommissionandFees = closingcommissionclosingTaxLot;

                    //positionalTaxLot.ClosedTotalCommissionandFees = closingcommissionpositionalTaxLot;
                    //TaxLotToClose.ClosedTotalCommissionandFees = closingcommissionclosingTaxLot;

                    //_openTaxLots.Remove(TaxLotToClose.TaxLotID);
                    #endregion

                    CalculateCommission(ref positionalTaxLot, ref TaxLotToClose, ref currentPosition, closingQuantity, _preferences.QtyRoundoffDigits);
                }
                else if (positionalTaxLot.TaxLotQty < TaxLotToClose.TaxLotQty)
                {
                    closingQuantity = positionalTaxLot.TaxLotQty;
                    currentPosition.ClosedQty = closingQuantity;
                    //TaxLotQtyToClose is used to populate the closed quantity from CloseTradeAllocationForm
                    positionalTaxLot.TaxLotQtyToClose += closingQuantity;
                    TaxLotToClose.TaxLotQtyToClose += closingQuantity;
                    #region old code for calculate commission
                    ////to adjust notional value for swap
                    //if (TaxLotToClose.ISSwap)
                    //{
                    //    currentPosition.ClosedSwapParameters.NotionalValue = TaxLotToClose.SwapParameters.NotionalValue * closingQuantity / TaxLotToClose.TaxLotQty;
                    //}
                    //double closingCommission = ((TaxLotToClose.OpenTotalCommissionandFees / TaxLotToClose.TaxLotQty) * closingQuantity);

                    //double closingcommissionPositionTaxLot = positionalTaxLot.OpenTotalCommissionandFees;

                    //TaxLotToClose.TaxLotQty -= closingQuantity;
                    //TaxLotToClose.OpenTotalCommissionandFees = TaxLotToClose.OpenTotalCommissionandFees - closingCommission;
                    //positionalTaxLot.TaxLotQty = 0;
                    //positionalTaxLot.OpenTotalCommissionandFees = 0;
                    //currentPosition.PositionTotalCommissionandFees = closingcommissionPositionTaxLot;
                    //currentPosition.ClosingTotalCommissionandFees = closingCommission;

                    //positionalTaxLot.ClosedTotalCommissionandFees = closingcommissionPositionTaxLot;
                    //TaxLotToClose.ClosedTotalCommissionandFees = closingCommission;
                    #endregion

                    CalculateCommission(ref positionalTaxLot, ref TaxLotToClose, ref currentPosition, closingQuantity, _preferences.QtyRoundoffDigits);
                }
                else
                {
                    /// Added 11 April 08, Resolved ashish closing bug
                    closingQuantity = TaxLotToClose.TaxLotQty;
                    currentPosition.ClosedQty = closingQuantity;
                    //TaxLotQtyToClose is used to populate the closed quantity from CloseTradeAllocationForm
                    positionalTaxLot.TaxLotQtyToClose += closingQuantity;
                    TaxLotToClose.TaxLotQtyToClose += closingQuantity;
                    #region old code to calculate commission
                    //double closingcommissionPositionTaxLot = positionalTaxLot.OpenTotalCommissionandFees;


                    //double closingcommissionclosingTaxLot = TaxLotToClose.OpenTotalCommissionandFees;



                    //TaxLotToClose.OpenTotalCommissionandFees = 0;

                    //positionalTaxLot.TaxLotQty = 0;
                    //positionalTaxLot.OpenTotalCommissionandFees = 0;

                    //currentPosition.PositionTotalCommissionandFees = closingcommissionPositionTaxLot;
                    //currentPosition.ClosingTotalCommissionandFees = closingcommissionclosingTaxLot;

                    //positionalTaxLot.ClosedTotalCommissionandFees = closingcommissionPositionTaxLot;
                    //TaxLotToClose.ClosedTotalCommissionandFees = closingcommissionclosingTaxLot;

                    /////_openTaxLots.Remove(TaxLotToClose.TaxLotID);
                    ////_openTaxLots.Remove(positionalTaxLot.TaxLotID);
                    ////_openTaxLots.Remove(TaxLotToClose);
                    ////_openTaxLots.Remove(positionalTaxLot);
                    #endregion

                    CalculateCommission(ref positionalTaxLot, ref TaxLotToClose, ref currentPosition, closingQuantity, _preferences.QtyRoundoffDigits);
                }

                //if taxlot closing algo info available then fill the details
                if (_closingAlgoInfo.ContainsKey(currentPosition.ID + "," + currentPosition.ClosingID))
                {
                    currentPosition.ClosingAlgo = _closingAlgoInfo[currentPosition.ID + "," + currentPosition.ClosingID];
                    _closingAlgoInfo.Remove(currentPosition.ID + "," + currentPosition.ClosingID);
                }

                if (positionalTaxLot.IsManualyExerciseAssign != null)
                    currentPosition.IsManualyExerciseAssign = positionalTaxLot.IsManualyExerciseAssign;

                if (IsCopyOpeningTradeAttributes)
                {
                    TaxLotToClose.TradeAttribute1 = positionalTaxLot.TradeAttribute1;
                    TaxLotToClose.TradeAttribute2 = positionalTaxLot.TradeAttribute2;
                    TaxLotToClose.TradeAttribute3 = positionalTaxLot.TradeAttribute3;
                    TaxLotToClose.TradeAttribute4 = positionalTaxLot.TradeAttribute4;
                    TaxLotToClose.TradeAttribute5 = positionalTaxLot.TradeAttribute5;
                    TaxLotToClose.TradeAttribute6 = positionalTaxLot.TradeAttribute6;
                    TaxLotToClose.SetTradeAttribute(positionalTaxLot.GetTradeAttributesAsDict());

                    currentPosition.TradeAttribute1 = positionalTaxLot.TradeAttribute1;
                    currentPosition.TradeAttribute2 = positionalTaxLot.TradeAttribute2;
                    currentPosition.TradeAttribute3 = positionalTaxLot.TradeAttribute3;
                    currentPosition.TradeAttribute4 = positionalTaxLot.TradeAttribute4;
                    currentPosition.TradeAttribute5 = positionalTaxLot.TradeAttribute5;
                    currentPosition.TradeAttribute6 = positionalTaxLot.TradeAttribute6;
                    currentPosition.SetTradeAttribute(positionalTaxLot.GetTradeAttributesAsDict());
                }

                TaxLotToClose.AdditionalTradeAttributes = TaxLotToClose.GetTradeAttributesAsJson();
                positionalTaxLot.AdditionalTradeAttributes = positionalTaxLot.GetTradeAttributesAsJson();
                currentPosition.IsCopyTradeAttrbsPrefUsed = IsCopyOpeningTradeAttributes;

                closedData.Taxlots.Add(TaxLotToClose);
                closedData.Taxlots.Add(positionalTaxLot);
                closedData.ClosedPositions.Add(currentPosition);

                closedData.UnSavedTaxlots.Add((TaxLot)(TaxLotToClose.Clone()));
                closedData.UnSavedTaxlots.Add((TaxLot)(positionalTaxLot.Clone()));
                //_unSaveAllocatedTadeList.Add((TaxLot)(TaxLotToClose.Clone()));
                //_unSaveAllocatedTadeList.Add((TaxLot)positionalTaxLot.Clone());

                positionalTaxLot.TaxLotClosingId = Guid.Empty.ToString();
                TaxLotToClose.TaxLotClosingId = Guid.Empty.ToString();

                //// AddToCloseDict(TaxLotToClose.TaxLotID, TaxLotToClose.AUECModifiedDate.ToString());
                ////prevent to add taxlots to closingInfoCache when taxlots are populated from CloseTradeInformation UI
                //if (!positionalTaxLot.ClosingMode.Equals(ClosingMode.CorporateAction) && !isVirtualClosingPopulate)
                //{
                //    AddToClosingInfoCache(positionalTaxLot, TaxLotToClose, algorithm);
                //}

                Prana.BusinessLogic.Calculations.SetAveragePriceRealizedPNL(currentPosition);

            }
            catch (Exception)
            {
                throw;
            }
            return closedData;
        }
        /// <summary>
        /// Narendra Kumar Jangir June 05 2013
        /// This method is refractored to calculate commission for closing position and closed taxlots
        /// </summary>
        /// <param name="positionalTaxLot"></param>
        /// <param name="TaxLotToClose"></param>
        /// <param name="currentPosition"></param>
        /// <param name="closingQuantity"></param>
        private static void CalculateCommission(ref TaxLot positionalTaxLot, ref TaxLot TaxLotToClose, ref Position currentPosition, double closingQuantity, int roundOffDigits)
        {
            try
            {
                //to adjust notional value for swap
                if (positionalTaxLot.ISSwap)
                {
                    currentPosition.PositionSwapParameters.NotionalValue = positionalTaxLot.SwapParameters.NotionalValue * closingQuantity / positionalTaxLot.TaxLotQty;
                }
                //to adjust notional value for swap
                if (TaxLotToClose.ISSwap)
                {
                    currentPosition.ClosedSwapParameters.NotionalValue = TaxLotToClose.SwapParameters.NotionalValue * closingQuantity / TaxLotToClose.TaxLotQty;
                }
                double closingcommissionpositionalTaxLot = 0;
                double closingcommissionclosingTaxLot = 0;

                if (positionalTaxLot.TaxLotQty != 0)
                {
                    closingcommissionpositionalTaxLot = positionalTaxLot.OpenTotalCommissionandFees * closingQuantity / positionalTaxLot.TaxLotQty;
                }
                if (TaxLotToClose.TaxLotQty != 0)
                {
                    closingcommissionclosingTaxLot = TaxLotToClose.OpenTotalCommissionandFees * closingQuantity / TaxLotToClose.TaxLotQty;
                }
                double positionalTaxlotQty = positionalTaxLot.TaxLotQty - closingQuantity;
                positionalTaxLot.TaxLotQty = positionalTaxlotQty;

                double closingTaxlotQty = TaxLotToClose.TaxLotQty - closingQuantity;
                TaxLotToClose.TaxLotQty = closingTaxlotQty;

                #region handle fractional closing
                //TODO - Need to clean up when we convert data type from double to decimal - omshiv
                double roundOffValue = Math.Pow((1.0 / 10.0), roundOffDigits);
                //set taxlot qty to zero if taxlot have remaining qty between 0 and 1 / 10^roundOffDigits 
                if (positionalTaxLot.TaxLotQty > roundOffValue * -1 && positionalTaxLot.TaxLotQty < roundOffValue) //
                {
                    positionalTaxLot.TaxLotQty = 0;
                }
                if (TaxLotToClose.TaxLotQty > roundOffValue * -1 && TaxLotToClose.TaxLotQty < roundOffValue) // && 
                {
                    TaxLotToClose.TaxLotQty = 0;
                }
                if (currentPosition.ClosedQty > roundOffValue * -1 && currentPosition.ClosedQty < roundOffValue)
                {
                    currentPosition.ClosedQty = 0;
                }
                #endregion

                positionalTaxLot.OpenTotalCommissionandFees -= closingcommissionpositionalTaxLot;
                TaxLotToClose.OpenTotalCommissionandFees -= closingcommissionclosingTaxLot;

                currentPosition.PositionTotalCommissionandFees = closingcommissionpositionalTaxLot;
                currentPosition.ClosingTotalCommissionandFees = closingcommissionclosingTaxLot;

                positionalTaxLot.ClosedTotalCommissionandFees = closingcommissionpositionalTaxLot;
                TaxLotToClose.ClosedTotalCommissionandFees = closingcommissionclosingTaxLot;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Get Sorted Dates Based On Closing Date
        /// </summary>
        /// <param name="listOfTaxLots"></param>
        /// <returns></returns>
        private List<DateTime> GetSortedDatesBasedOnClosingDate(List<TaxLot> listOfTaxLots)
        {
            List<DateTime> SortedDates = new List<DateTime>();
            try
            {

                foreach (TaxLot item in listOfTaxLots)
                {
                    // Here we are setting the closing Date Based On Preference saved in the server app settings xml. 

                    SetClosingDateBasedOnPreferences(item);

                    if (!SortedDates.Contains(item.ClosingDate.Date))
                    {
                        SortedDates.Add(item.ClosingDate.Date);
                    }
                }

                SortedDates.Sort(delegate (DateTime date1, DateTime date2) { return date1.CompareTo(date2); });
            }
            catch (Exception)
            {
                throw;
            }

            return SortedDates;
        }

        //Reference not found
        private List<DateSymbol> GetALLClosingDates(List<TaxLot> buyTaxlots, List<TaxLot> sellTaxlots)
        {
            List<DateSymbol> lstSymbols = new List<DateSymbol>();
            Dictionary<string, Dictionary<DateTime, DateTime>> dictSymbols = new Dictionary<string, Dictionary<DateTime, DateTime>>();
            try
            {
                List<TaxLot> transactions_BTC = buyTaxlots.FindAll(ClosingTransaction);
                List<TaxLot> transactionSell_SelltoClose = sellTaxlots.FindAll(ClosingTransaction);

                foreach (TaxLot taxlot in transactions_BTC)
                {
                    if (!dictSymbols.ContainsKey(taxlot.Symbol))
                    {
                        Dictionary<DateTime, DateTime> dictDates = new Dictionary<DateTime, DateTime>();
                        dictDates.Add(taxlot.ClosingDate.Date, taxlot.ClosingDate.Date);
                        dictSymbols.Add(taxlot.Symbol, dictDates);
                    }
                    else
                    {
                        if (!dictSymbols[taxlot.Symbol].ContainsKey(taxlot.ClosingDate.Date))
                        {
                            dictSymbols[taxlot.Symbol].Add(taxlot.ClosingDate.Date, taxlot.ClosingDate.Date);
                        }
                    }
                }

                foreach (TaxLot taxlot in transactionSell_SelltoClose)
                {
                    if (!dictSymbols.ContainsKey(taxlot.Symbol))
                    {
                        Dictionary<DateTime, DateTime> dictDates = new Dictionary<DateTime, DateTime>();
                        dictDates.Add(taxlot.ClosingDate.Date, taxlot.ClosingDate.Date);
                        dictSymbols.Add(taxlot.Symbol, dictDates);
                    }
                    else
                    {
                        if (!dictSymbols[taxlot.Symbol].ContainsKey(taxlot.ClosingDate.Date))
                        {
                            dictSymbols[taxlot.Symbol].Add(taxlot.ClosingDate.Date, taxlot.ClosingDate.Date);
                        }
                    }
                }


                foreach (KeyValuePair<string, Dictionary<DateTime, DateTime>> kp in dictSymbols)
                {
                    Dictionary<DateTime, DateTime> dictDates = kp.Value;

                    foreach (DateTime date in dictDates.Keys)
                    {
                        DateSymbol dateSymbol = new DateSymbol();
                        dateSymbol.Symbol = kp.Key.ToString();
                        dateSymbol.FromDate = date;
                        dateSymbol.ToDate = date;
                        lstSymbols.Add(dateSymbol);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return lstSymbols;
        }

        /// <summary>
        /// check for, Is Closing Transaction
        /// </summary>
        /// <param name="taxlot"></param>
        /// <returns></returns>
        private bool ClosingTransaction(TaxLot taxlot)
        {
            SetClosingDateBasedOnPreferences(taxlot);
            if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///  creates a new  position object and return back.
        /// </summary>
        /// <param name="positionTaxLotID"></param>
        /// <returns></returns>
        private Position GetPosition(string startTaxLotTaxLotID)
        {
            Position position = new Position();


            position.ID = startTaxLotTaxLotID;
            position.IsClosingSaved = false;

            return position;
        }

        /// <summary>
        /// Checks the close rules and returns the error string or empty string. If empty string is returned
        /// then TaxLots are successfully closed
        /// </summary>
        /// <param name="positionalTaxlot">The target allocated trade.</param>
        /// <param name="closingTaxlot">The dragged allocated trade.</param>
        /// <returns></returns>
        private string CheckCloseRules(TaxLot positionalTaxlot, TaxLot closingTaxlot, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, bool isMatchStrategy, List<string> VirtualUnwidedTaxlots)
        {
            ///TODO : Need to integrate with some rules engine.
            //string dateMessage = "Closing TaxLot date should be greater than or equal to Positional TaxLots";

            try
            {
                if (_isSameAccount)
                {
                    //Same account rule
                    if (positionalTaxlot.Level1ID != closingTaxlot.Level1ID)
                    {
                        return "TaxLot/Position pair to close must belong to the same account.";
                    }
                }


                if (_isSameSymbol)
                {
                    //Same symbol rule
                    if (positionalTaxlot.Symbol.ToUpper().Trim() != closingTaxlot.Symbol.ToUpper().Trim())
                    {
                        return "TaxLot/Position pair to close must belong to same symbol.";
                    }
                }
                //if (_isSwapAndEquity)
                //{
                //    if (!(positionalTaxlot.AssetName.Equals(closingTaxlot.AssetName) && (positionalTaxlot.ISSwap.Equals(closingTaxlot.ISSwap))))
                //    {
                //        return "TaxLot/Position pair to close must belong to same Asset Class.";
                //    }
                //}
                //Narendra Kumar Jangir, 2013 May 28
                //strategy will not be matched if closing is populated from new CloseTradeInformation UI
                if (isMatchStrategy && _isSameStrategy)// Level2Allocation 
                {
                    //Same symbol rule
                    if (positionalTaxlot.Level2ID != closingTaxlot.Level2ID)
                    {
                        return "TaxLot/Position pair to close must belong to same Strategy.";
                    }
                }

                // Same settlement currency rule otherwise cause cash differences after closing.
                if (_isSameSettlementCurrency)
                {
                    if (positionalTaxlot.SettlementCurrencyID != closingTaxlot.SettlementCurrencyID)
                    {
                        return "TaxLot/Position pair to close must belong to the same settlement currency.";
                    }
                }

                ///Side specific rules


                //Opposite side rule
                if (positionalTaxlot.OrderSideTagValue.Equals(closingTaxlot.OrderSideTagValue))
                {
                    return "TaxLot/Position pair to close must belong to opposite sides.";
                }

                ///Buy to cover and Buy cannot be closed
                if ((positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) && closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed)) || (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) && closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy)))
                {
                    return "TaxLot/Position pair to close must belong to opposite sides.";
                }

                ///Sell and SellShort cannot be closed
                if ((positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) && closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell)) || (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) && closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort)))
                {
                    return "TaxLot/Position pair to close must belong to opposite sides.";
                }
                bool isCheckClosingDate = false;
                bool.TryParse(ConfigurationHelper.Instance.GetAppSettingValueByKey("IsCheckClosingDate"), out isCheckClosingDate);
                if (isCheckClosingDate && (!IsShortWithBuyAndBuyToCover || !IsSellWithBuyToClose))
                {
                    if (positionalTaxlot.ClosingDate.Date > closingTaxlot.ClosingDate.Date)
                    {
                        return "Opening taxlot date should be less than from closing taxlot date.";
                    }
                }
                //if (IsShortWithBuyAndBuyToCover || IsSellWithBuyToClose)
                //{
                if (IsShortWithBuyAndBuyToCover)
                {
                    if (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                    {
                        switch (closingTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Buy_Cover:
                                {

                                    break;

                                }
                            default:
                                return "Sell Short can only be closed with 'Buy' and 'Buy To Close'.";
                        }

                    }
                    else if (closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                    {
                        switch (positionalTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Buy_Cover:
                                {
                                    break;
                                    // return string.Empty;

                                }
                            default:
                                return "Sell Short can only be closed with 'Buy' and 'Buy To Close'.";

                        }

                    }
                }
                else
                {
                    if (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                    {
                        switch (closingTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                }

                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Sell:
                            default:
                                return "Sell Short can only be closed with 'Buy To Close'.";
                        }

                    }
                    else if (closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                    {
                        switch (positionalTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                    //return string.Empty;

                                }
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Sell:
                            default:
                                return "Sell Short can only be closed with 'Buy To Close'.";
                        }
                    }
                }

                if (IsSellWithBuyToClose)
                {
                    if (closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        switch (positionalTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                }

                            default:
                                return "Sell  can only be closed with 'Buy To Close and Buy'.";
                        }
                    }

                    else if (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        switch (closingTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                }

                            default:
                                return "Sell  can only be closed with 'Buy To Close and Buy'.";
                        }
                    }
                }
                else
                {
                    if (closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || closingTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        switch (positionalTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                }

                            case FIXConstants.SIDE_Buy_Closed:
                            default:
                                return "Sell  can only be closed with Buy'.";
                        }

                    }

                    else if (positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || positionalTaxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        switch (closingTaxlot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                                {
                                    break;
                                    //return string.Empty;
                                }
                            case FIXConstants.SIDE_Buy_Closed:
                            default:
                                return "Sell  can only be closed with Buy.";
                        }
                    }

                    //  }
                    //else if(!IsShortWithBuyAndBuyToCover || !IsSellWithBuyToClose)
                    //{


                    ////if (!IsSellWithBuyToClose)
                    //    //{
                    //    else 
                }


                StringBuilder alreadyClosedError = new StringBuilder();
                StringBuilder CorporateActionError = new StringBuilder();


                //if (!_preferences.IsCurrentDateClosing)
                //{
                lock (_closingInfoCacheLock)
                {
                    if (VirtualUnwidedTaxlots == null || !VirtualUnwidedTaxlots.Contains(positionalTaxlot.TaxLotID))
                    {
                        if (_closingInfoCache.ContainsKey(positionalTaxlot.TaxLotID) && _closingInfoCache[positionalTaxlot.TaxLotID].AUECMaxModifiedDate.Date > closingTaxlot.ClosingDate.Date && _closingInfoCache[positionalTaxlot.TaxLotID].AUECMaxModifiedDate.Date > positionalTaxlot.ClosingDate.Date)
                        {
                            alreadyClosedError.Append(positionalTaxlot.Symbol);
                            alreadyClosedError.Append(" ( ");
                            alreadyClosedError.Append("TaxLotId : ");
                            alreadyClosedError.Append(positionalTaxlot.TaxLotID.ToString());
                            alreadyClosedError.Append(" )");
                            alreadyClosedError.Append(" is closed on AUEC date : ");
                            alreadyClosedError.Append(_closingInfoCache[positionalTaxlot.TaxLotID].AUECMaxModifiedDate);
                            alreadyClosedError.Append(".");
                        }

                        if (_closingInfoCache.ContainsKey(closingTaxlot.TaxLotID) && _closingInfoCache[closingTaxlot.TaxLotID].AUECMaxModifiedDate.Date > positionalTaxlot.ClosingDate.Date && _closingInfoCache[closingTaxlot.TaxLotID].AUECMaxModifiedDate.Date > closingTaxlot.ClosingDate.Date)
                        {
                            alreadyClosedError.Append(Environment.NewLine);
                            alreadyClosedError.Append(closingTaxlot.Symbol);
                            alreadyClosedError.Append(" ( ");
                            alreadyClosedError.Append("TaxLotId : ");
                            alreadyClosedError.Append(" )");
                            alreadyClosedError.Append(closingTaxlot.TaxLotID.ToString());
                            alreadyClosedError.Append(" is closed on AUEC date : ");
                            alreadyClosedError.Append(_closingInfoCache[closingTaxlot.TaxLotID].AUECMaxModifiedDate);
                            alreadyClosedError.Append(".");
                        }
                    }
                }
                lock (_TaxLotIdToCADateDictLock)
                {
                    if (_TaxLotIdToCADateDict.ContainsKey(positionalTaxlot.TaxLotID) && _TaxLotIdToCADateDict[positionalTaxlot.TaxLotID].Item1.Date > closingTaxlot.ClosingDate.Date)
                    {
                        CorporateActionError.Append(positionalTaxlot.Symbol);
                        CorporateActionError.Append(" ( ");
                        CorporateActionError.Append("TaxLotId : ");
                        CorporateActionError.Append(positionalTaxlot.TaxLotID.ToString());
                        CorporateActionError.Append(" )");
                        CorporateActionError.Append(" has Corporate action  on AUEC date : ");
                        CorporateActionError.Append(_TaxLotIdToCADateDict[positionalTaxlot.TaxLotID].Item1.Date);
                        CorporateActionError.Append(".");
                    }
                    if (alreadyClosedError.ToString() != string.Empty)
                    {
                        alreadyClosedError.Append(Environment.NewLine);
                        alreadyClosedError.Append("Please unwind closed TaxLots before attempting to close in the back date.");
                        return alreadyClosedError.ToString();
                    }
                    // As we have made a check while applying the corporate action that there should be no box positions in the same account, so there is no chance of having opening 
                    // and closing trades before applying the CA. Now fractional share that are genrated from Corporate action will be closed from closing UI i.e. user will have this option to close from closing UI, 
                    // closing trade will be available on closing UI and will be closed by user.                  

                    //if (_TaxLotIdToCADateDict.ContainsKey(draggedTaxLot.TaxLotID) && _TaxLotIdToCADateDict[draggedTaxLot.TaxLotID].Date > targetTaxLot.ClosingDate.Date)
                    //{
                    //    CorporateActionError.Append(Environment.NewLine);
                    //    CorporateActionError.Append(targetTaxLot.Symbol);
                    //    CorporateActionError.Append(" ( ");
                    //    CorporateActionError.Append("TaxLotId : ");
                    //    CorporateActionError.Append(" )");
                    //    CorporateActionError.Append(draggedTaxLot.TaxLotID.ToString());
                    //    CorporateActionError.Append(" has Corporate action  on AUEC date : ");
                    //    CorporateActionError.Append(_TaxLotIdToCADateDict[draggedTaxLot.TaxLotID].Date);
                    //    CorporateActionError.Append(".");
                    //}

                    if (CorporateActionError.ToString() != string.Empty)
                    {
                        CorporateActionError.Append(Environment.NewLine);
                        CorporateActionError.Append("Please Undo Corporate Action  before attempting to close in the back date.");
                        return CorporateActionError.ToString();
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            //}

            return string.Empty;
        }

        /// <summary>
        /// Get Eligible Closing TaxLot New
        /// </summary>
        /// <param name="openTaxLotsAndPositions"></param>
        /// <param name="sideTaxLotID"></param>
        /// <param name="AvgPx"></param>
        /// <param name="IsShortWithBuyAndBuyToCover"></param>
        /// <param name="IsSellWithBuyToClose"></param>
        /// <param name="isSameAvgPx"></param>
        /// <param name="closingDate"></param>
        /// <param name="isOverrideWithUserClosing"></param>
        /// <param name="isBTAX"></param>
        /// <returns></returns>
        private TaxLot GetEligibleClosingTaxLotNew(List<TaxLot> openTaxLotsAndPositions, TaxLot closeTaxLot, bool isSamePxAvgPx, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, bool isOverrideWithUserClosing)
        {
            try
            {
                double AvgPx = closeTaxLot.AvgPrice;
                string sideTaxLotID = closeTaxLot.OrderSideTagValue;
                DateTime closingDate = closeTaxLot.ClosingDate;

                foreach (TaxLot taxLot in openTaxLotsAndPositions)
                {
                    //to do closing for close order UI
                    if ((isOverrideWithUserClosing && taxLot.TaxLotQtyToClose == 0) || (isOverrideWithUserClosing && closeTaxLot.TaxLotQtyToClose == 0))
                    {
                        continue;
                    }

                    //sideTaxLotID.Equals(FIXConstants.SIDE_Buy) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Open)
                    if (sideTaxLotID.Equals(FIXConstants.SIDE_Sell) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        //(TaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || TaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open)))//&& TaxLot.TradeDate.Date >= tradeDate.Date)
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot; //= TaxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                    //sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Cover)
                    else if (sideTaxLotID.Equals(FIXConstants.SIDE_SellShort) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open))
                    {
                        //(taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover)))// && TaxLot.TradeDate.Date <= tradeDate.Date)
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot; //= TaxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                    //To handle case of closing side BuyToClose from close order UI
                    //In normal closing work flow BuyToClose taxlots are buy grid side but in close order ui BuyToClose taxlots are sell side(closing side)
                    //Narendra Kumar Jangir 2013 June 07

                    //sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open) || sideTaxLotID.Equals(FIXConstants.SIDE_SellShort)
                    else if (sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Closed))
                    {
                        //TaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed)
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open)) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort))//&& TaxLot.TradeDate.Date >= tradeDate.Date)
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot; //= TaxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return new TaxLot();
        }

        /// <summary>
        /// Return taxlot in case of forcible side closing
        /// e.g sell with buy to close and sell short with buy
        /// </summary>
        /// <param name="openTaxLotsAndPositions"></param>
        /// <param name="closeTaxLot"></param>
        /// <param name="IsShortWithBuyAndBuyToCover"></param>
        /// <param name="IsSellWithBuyToClose"></param>
        /// <param name="isSameAvgPx"></param>
        /// <param name="isOverrideWithUserClosing"></param>
        /// <returns></returns>
        private TaxLot GetEligibleTaxLotWithForceSide(List<TaxLot> openTaxLotsAndPositions, TaxLot closeTaxLot, bool isSamePxAvgPx, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, bool isOverrideWithUserClosing)
        {
            try
            {
                double AvgPx = closeTaxLot.AvgPrice;
                string sideTaxLotID = closeTaxLot.OrderSideTagValue;
                DateTime closingDate = closeTaxLot.ClosingDate;

                foreach (TaxLot taxLot in openTaxLotsAndPositions)
                {
                    //to do closing for close order UI
                    if (isOverrideWithUserClosing && taxLot.TaxLotQtyToClose == 0)
                    {
                        continue;
                    }
                    //sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Cover)
                    if (sideTaxLotID.Equals(FIXConstants.SIDE_Sell) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        //(taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover)))// && TaxLot.TradeDate.Date >= tradeDate.Date)
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot; //= TaxLot;
                            }
                            else
                                return taxLot;
                        }
                    }

                    //for corporate action, the positional taxlot is sometimes sell side taxlot

                    //sideTaxLotID.Equals(FIXConstants.SIDE_Sell) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_SellShort) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open)
                    if (sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Cover) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open))// && TaxLot.TradeDate.Date >= tradeDate.Date)
                    {
                        //(taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy))
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open)))
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                    //sideTaxLotID.Equals(FIXConstants.SIDE_Buy) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Open)
                    if (sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open) || sideTaxLotID.Equals(FIXConstants.SIDE_SellShort) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell))
                    {
                        //(taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort))
                        if ((taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open)))
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                    //sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Open) || sideTaxLotID.Equals(FIXConstants.SIDE_SellShort) || (sideTaxLotID.Equals(FIXConstants.SIDE_Sell_Closed) || sideTaxLotID.Equals(FIXConstants.SIDE_Sell))
                    if (sideTaxLotID.Equals(FIXConstants.SIDE_Buy) || sideTaxLotID.Equals(FIXConstants.SIDE_Buy_Open))
                    {
                        //taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open)
                        if (taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed) || taxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell))
                        {
                            if (isSamePxAvgPx)
                            {
                                if (AvgPx.Equals(taxLot.AvgPrice) && closingDate.Date.Equals(taxLot.ClosingDate.Date))
                                    return taxLot; //= TaxLot;
                            }
                            else
                                return taxLot;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new TaxLot();
        }


        /// <summary>
        /// Update Closing Data
        /// </summary>
        /// <param name="UpdatedClosedData"></param>
        /// <param name="CurrentClosingData"></param>
        /// <param name="maintainLatestModifiedState"></param>
        private void UpdateClosingData(ClosingData UpdatedClosedData, ClosingData CurrentClosingData, bool maintainLatestModifiedState)
        {
            try
            {
                foreach (TaxLot TaxLot in CurrentClosingData.Taxlots)
                {
                    // if maintainLatestModifiedState is true then only the latest modified state will be returned for a particular taxlot ID
                    // as the closing data will not be filtered as is the case with the client side call.
                    if (maintainLatestModifiedState)
                    {
                        if (!UpdatedClosedData.UpdatedTaxlots.ContainsKey(TaxLot.TaxLotID))
                        {
                            UpdatedClosedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
                            UpdatedClosedData.Taxlots.Add(TaxLot);
                        }
                    }
                    // contains all the modified states for a particular taxlotID.
                    else
                    {
                        UpdatedClosedData.Taxlots.Add((TaxLot)TaxLot.Clone());
                    }
                }
                UpdatedClosedData.ErrorMsg.Append(CurrentClosingData.ErrorMsg.ToString());
                //UpdatedClosedData.ErrorMsg.Append(Environment.NewLine);
                UpdatedClosedData.ClosedPositions.AddRange(CurrentClosingData.ClosedPositions);
                UpdatedClosedData.UnSavedTaxlots.AddRange(CurrentClosingData.UnSavedTaxlots);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Remove Zero Open Qty From List
        /// </summary>
        /// <param name="listOfTaxLots"></param>
        private void RemoveZeroOpenQtyFromList(List<TaxLot> listOfTaxLots)
        {
            try
            {

                if (listOfTaxLots.Count > 0)
                {
                    TaxLot[] TaxLotsClone = new TaxLot[listOfTaxLots.Count];
                    listOfTaxLots.CopyTo(TaxLotsClone, 0);

                    foreach (TaxLot TaxLot in TaxLotsClone)
                    {
                        if (TaxLot.TaxLotQty == 0)
                        {
                            listOfTaxLots.Remove(TaxLot);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get Open Positions
        /// </summary>
        /// <param name="sortedTaxLots">Allocated Trade.</param>
        private List<TaxLot> GetOpenPositionsFromDB(DateTime CloseTradeDate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string customConditions)
        {
            List<TaxLot> OpenTaxLots = new List<TaxLot>();
            try
            {
                string ToAllAUECDatesString = string.Empty;

                if (IsCurrentDateClosing)
                {
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                }
                else
                {
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(CloseTradeDate);
                }

                OpenTaxLots = CloseTradesDataManager.GetOpenPositions(ToAllAUECDatesString, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, customConditions);
            }
            catch (Exception)
            {
                throw;
            }
            return OpenTaxLots;
        }


        /// <summary>
        /// Get Open Positions for a Symbol from database
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <returns></returns>
        private List<TaxLot> GetOpenPositionsFromDBForASymbol(string symbol, string CommaSeparatedAccountIds, string orderSideTagValue)
        {
            List<TaxLot> OpenTaxLots = new List<TaxLot>();
            try
            {

                string ToAllAUECDatesString = string.Empty;

                //if (IsCurrentDateClosing)
                //{
                //    ToAllAUECDatesString = TimeZoneHelper.GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                //}
                //else
                //{
                //    ToAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(CloseTradeDate);
                //}

                //current date
                ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);

                OpenTaxLots = CloseTradesDataManager.GetOpenPositionsForASymbol(symbol, ToAllAUECDatesString, CommaSeparatedAccountIds, orderSideTagValue);
                //CreateOpenTaxLotDictionary(OpenTaxLots);

            }
            catch (Exception)
            {
                throw;
            }

            return OpenTaxLots;

        }

        //Reference not found
        /// <summary>
        /// Get Closing Status For TaxlotID
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private ClosingStatus GetClosingStatusForTaxlotID(string TaxlotID)
        {
            ClosingStatus taxlotClosingStatus = ClosingStatus.Open;
            try
            {
                if (_closingInfoCache.ContainsKey(TaxlotID))
                {
                    taxlotClosingStatus = _closingInfoCache[TaxlotID].ClosingStatus;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return taxlotClosingStatus;
        }

        //private void CreateOpenTaxLotDictionary(List<TaxLot> openTaxLots)
        //{
        //    _updatedOpenTaxLots.Clear();
        //    foreach (TaxLot trade in openTaxLots)
        //    {
        //        if(!_updatedOpenTaxLots.ContainsKey(trade.TaxLotID))
        //        _updatedOpenTaxLots.Add(trade.TaxLotID, trade);
        //    }

        //}

        /// <summary>
        /// Get Net Positions For TaxlotIds
        /// </summary>
        /// <param name="CommaSeparatedTaxlotIds"></param>
        /// <returns></returns>
        public List<Position> GetNetPositionsForTaxlotIds(string CommaSeparatedTaxlotIds)
        {
            Logger.LoggerWrite(string.Format("Getting Net Positions For TaxlotIds: {0}", CommaSeparatedTaxlotIds), LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"CommaSeparatedTaxlotIds", CommaSeparatedTaxlotIds}
            });

            List<Position> netPositions = null;
            try
            {
                netPositions = CloseTradesDataManager.GetNetPositionsForTaxlotIds(CommaSeparatedTaxlotIds);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;

            }
            return netPositions;
        }

        /// <summary>
        /// Get Net Position
        /// </summary>
        /// <param name="sortedTaxLots">Allocated Trade.</param>
        public List<Position> GetNetPositionsFromDB(DateTime FromDate, DateTime CloseTradeDate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string customConditions, PostTradeEnums.DateType closingDateType = PostTradeEnums.DateType.ProcessDate)
        {
            Logger.LoggerWrite("Getting Net Positions From DB, CloseTradeDate: " + CloseTradeDate + "; AccountIds:" + CommaSeparatedAccountIds, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"CurrentMethod", MethodBase.GetCurrentMethod()},
                {"FromDate", FromDate},
                {"CloseTradeDate", CloseTradeDate},
                {"IsCurrentDateClosing", IsCurrentDateClosing},
                {"CommaSeparatedAccountIds", CommaSeparatedAccountIds},
                {"CommaSeparatedAssetIds", CommaSeparatedAssetIds},
                {"commaSeparatedSymbols", commaSeparatedSymbols},
                {"customConditions", customConditions},
            });

            List<Position> netPositions = null;
            //List<Position> Netpositions = new List<Position>();
            try
            {

                string ToAllAUECDatesString = string.Empty;
                string FromAllAUECDatesString = string.Empty;

                if (IsCurrentDateClosing)
                {
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                    FromAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                }
                else
                {
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(CloseTradeDate);
                    FromAllAUECDatesString = TimeZoneHelper.GetInstance().GetSameDateInUseAUECStr(FromDate);
                }

                netPositions = CloseTradesDataManager.GetNetPositionsList(ToAllAUECDatesString, FromAllAUECDatesString, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, customConditions, closingDateType);

                //CreateNetPositionsDicitonary(Netpositions);


            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            return netPositions;
        }

        /// <summary>
        /// Get Net Positions From DB For A Symbol 
        /// </summary>
        /// <param name="IsCurrentDateClosing"></param>
        /// <param name="CommaSeparatedAccountIds"></param>
        /// <param name="Symbol"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<Position> GetNetPositionsFromDBForASymbol(bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string @Symbol, string groupID)
        {
            List<Position> netPositions = null;
            try
            {
                string ToAllAUECDatesString = string.Empty;
                string FromAllAUECDatesString = string.Empty;

                if (IsCurrentDateClosing)
                {
                    ToAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                    FromAllAUECDatesString = TimeZoneHelper.GetInstance().GetAllAUECDateInUseAUECStr(DateTime.UtcNow);
                }
                else
                {
                    //ToAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(CloseTradeDate);
                    //FromAllAUECDatesString = TimeZoneHelper.GetSameDateInUseAUECStr(FromDate);
                }

                netPositions = CloseTradesDataManager.GetNetPositionsListForASymbol(ToAllAUECDatesString, FromAllAUECDatesString, CommaSeparatedAccountIds, @Symbol, groupID);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            return netPositions;
        }

        private Dictionary<string, int> _closingAlgoInfo = new Dictionary<string, int>();

        #region Closing and CorporateAction Checks

        //Unused Cache
        //  private Dictionary<string, Dictionary<string, string>> _exercisedUnderlyingTaxLots;

        /// <summary>
        /// Here we are keeping the first closing date corresponding to account TaxLotid.
        /// </summary>
        /// 
        private void GetTaxLotsLatestClosingDatesFromDB(bool isUpdateOpenTaxlots)
        {
            Logger.LoggerWrite("Getting TaxLots Latest Closing Dates From DB, isUpdateOpenTaxlots: " + isUpdateOpenTaxlots, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            try
            {

                var closingInfoCacheNew = CloseTradesDataManager.GetTaxlotsLatestClosingDates(_toClosingDate, _fromClosingDate, _toTradeDate, _fromTradeDate, isUpdateOpenTaxlots);


                lock (_closingInfoCacheLock)
                {
                    if (isUpdateOpenTaxlots == true)
                    {
                        if (_closingInfoCache != null && _closingInfoCache.Count > 0)
                        {
                            _closingInfoCache.Clear();
                        }
                        _closingInfoCache = closingInfoCacheNew;
                        Logger.LoggerWrite("Refreshed whole closing cache ", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                    }
                    else
                    {

                        foreach (KeyValuePair<string, TaxlotClosingInfo> kp in closingInfoCacheNew)
                        {
                            if (_closingInfoCache.ContainsKey(kp.Key))
                            {

                                _closingInfoCache[kp.Key] = kp.Value;
                            }
                            else
                            {
                                _closingInfoCache.Add(kp.Key, kp.Value);
                            }
                            Logger.LoggerWrite("Setting Closing status for taxlotId: " + kp.Key + " = " + kp.Value.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                        }
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }


        private delegate void TaxlottoCloseCacheHandler(bool isUpdateOpenTaxlots);
        public void UpdateTradeDates(string FromAuecDatesString, string ToAuecDatesString)
        {
            try
            {
                DateTime fromTradeDate = Convert.ToDateTime(FromAuecDatesString).Date;
                DateTime toTradeDate = Convert.ToDateTime(ToAuecDatesString).Date;
                if (fromTradeDate.Date >= _fromTradeDate.Date && toTradeDate.Date <= _toTradeDate.Date)
                {
                    return;
                }
                else
                {
                    _fromTradeDate = fromTradeDate.Date;
                    _toTradeDate = toTradeDate.Date;
                    Logger.LoggerWrite("Updating Trade Dates, FromAuecDatesString: " + FromAuecDatesString + ", ToAuecDatesString:" + ToAuecDatesString, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                    GetTaxLotsLatestClosingDatesFromDB(false);

                }


                //TaxlottoCloseCacheHandler closingInfoCacheHandler = new TaxlottoCloseCacheHandler(GetTaxLotsLatestClosingDatesFromDB);
                //closingInfoCacheHandler.BeginInvoke(false, null, null);

            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

        }

        //why this method??
        public void GetExcercisedGroupIDsFromDB(string FromAuecDatesString, string ToAuecDatesString, int userID)
        {

            try
            {
                //_fromTradeDate = Convert.ToDateTime(FromAuecDatesString);
                //_toTradeDate = Convert.ToDateTime(ToAuecDatesString);
                //string fromdateStringMax = string.Empty;
                //DateTime date = Convert.ToDateTime(FromAuecDatesString);

                //if (_exercisedUnderlyingTaxLots != null && _exercisedUnderlyingTaxLots.Count > 0)
                //{
                //    _exercisedUnderlyingTaxLots.Clear();
                //}
                //if (_dictUserWiseDates.ContainsKey(userID))
                //{
                //    _dictUserWiseDates[userID] = date;
                //}
                //else
                //{
                //    _dictUserWiseDates.Add(userID, date);
                //}

                //List<DateTime> listDates = new List<DateTime>();
                //listDates.AddRange(_dictUserWiseDates.Values);
                //listDates.Sort(delegate(DateTime t1, DateTime t2) { return (t1.CompareTo(t2) * (-1)); });

                //if (listDates.Count > 0)
                //{
                //    fromdateStringMax = listDates[0].ToString();
                //}

                //_exercisedUnderlyingTaxLots = ClientsCommonDataManager.GetExcerciseGroupIDs(fromdateStringMax);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);



                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Here we are keeping the first closing date corresponding to account TaxLotid.
        /// </summary>
        private void GetTaxLotsLatestCADatesFromDB()
        {
            try
            {
                lock (_TaxLotIdToCADateDictLock)
                {
                    if (_TaxLotIdToCADateDict != null && _TaxLotIdToCADateDict.Count > 0)
                    {
                        _TaxLotIdToCADateDict.Clear();
                    }

                    _TaxLotIdToCADateDict = WindsorContainerManager.GetTaxlotsLatestCADates();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Closes the tax lots automatically.  [Sugandh Jain 02-Jan-2007]
        /// </summary>
        /// <param name="buyTaxLotsAndPositions">The buy tax lots and positions.</param>
        /// <param name="sellTaxLotsAndPositions">The sell tax lots and positions.</param>
        /// <param name="IsShortWithBuyAndBuyToCover">checkbox to close short with buy and buy to close.</param>
        /// <param name="IsSellWithBuyToClose">checkbox to close sell with buy to close.</param>
        /// <param name="algorithm">Algorithm selected for the closing.</param>
        /// <param name="isSameAvgPx">bool flag that identifies that method is called for same avg price or not.</param>
        private ClosingData CloseTaxLotsAutomaticallyNew(List<TaxLot> openTaxLotsAndPositions, List<TaxLot> closeTaxLotsAndPositions, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, PostTradeEnums.CloseTradeAlogrithm algorithm, bool isSamePxAvgPx, bool isVirtualClosingPopulate, bool isMatchStrategy, bool isOverrideWithUserClosing, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, PostTradeEnums.ClosingField closingField, bool IsCopyOpeningTradeAttributes)
        {
            ClosingData ClosedData = new ClosingData();
            ClosingData closingData = null;
            try
            {
                //We are copying closeTaxLotsAndPositions to an array because we also need to remove the positions from this collection and 
                //looping won't be correct if we are removing from the same collection on which we are looping.

                TaxLot[] TaxLotArr = new TaxLot[closeTaxLotsAndPositions.Count];
                closeTaxLotsAndPositions.CopyTo(TaxLotArr, 0);
                for (int counter = 0; counter < TaxLotArr.Length; counter++)
                {
                    //Narendra Kumar jangir May 31 2013
                    //if isOverrideWithUserClosing is true i.e. closing is done on the basis of user defined quantity than
                    //while condition will be based on TaxLotQtyToClose rather than TaxLotQty
                    //because it may stuck in case of partial closing where TaxLotQtyToClose is zero but TaxLotQty is always greater than zero
                    TaxLot closeTaxLot = TaxLotArr[counter];
                    //TODO: Need to analyse close order closing logic

                    double BuyTaxLotQty = 0;
                    //while ((isOverrideWithUserClosing ? closeTaxLot.TaxLotQtyToClose : closeTaxLot.TaxLotQty) > 0)

                    //secondary sort for algo if required
                    AlgoBase algo = AlgoFactory.GetAlgo(algorithm);
                    algo.SecondarySort(closeTaxLot, ref openTaxLotsAndPositions, _preferences, secondarySortCriteria, closingField);

                    while (closeTaxLot.TaxLotQty > 0)
                    {
                        bool isOpeningTaxLotSmaller = false;

                        TaxLot matchingEligibleTaxLot = GetEligibleClosingTaxLotNew(openTaxLotsAndPositions, closeTaxLot, isSamePxAvgPx, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, isOverrideWithUserClosing);

                        //double BuyTaxLotQtyToClose = matchingEligibleTaxLot.TaxLotQty;
                        //isOverrideWithUserClosing is true than closing will be done on the basis of TaxLotQtyToClose rather than TaxLotQty
                        //if (isOverrideWithUserClosing)
                        //BuyTaxLotQtyToClose = closeTaxLot.TaxLotQtyToClose;
                        if ((matchingEligibleTaxLot.TaxLotID.Length == 0) && IsShortWithBuyAndBuyToCover && IsSellWithBuyToClose)
                        {
                            matchingEligibleTaxLot = GetEligibleTaxLotWithForceSide(openTaxLotsAndPositions, closeTaxLot, isSamePxAvgPx, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, isOverrideWithUserClosing);
                        }

                        BuyTaxLotQty = matchingEligibleTaxLot.TaxLotQty;
                        if (matchingEligibleTaxLot.TaxLotID.Length > 0)
                        {
                            isOpeningTaxLotSmaller = false;
                            //TaxLotQty of sell is equal to TaxLotQty and TaxLotQtyToClose of buy than remove both taxlots
                            //TODO: BuyTaxLotQtyToClose to buyTaxLot.TaxLotQtyToClose

                            if (matchingEligibleTaxLot.TaxLotQty < closeTaxLot.TaxLotQty)
                            {
                                isOpeningTaxLotSmaller = true;
                            }
                            closingData = ClosePosition(matchingEligibleTaxLot, closeTaxLot, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, algorithm, isVirtualClosingPopulate, isMatchStrategy, isOverrideWithUserClosing, null, IsCopyOpeningTradeAttributes);
                            //quantity of opening taxlot is similrar to closing taxlot then closing taxlot and opening taxlot are completely closed 
                            if ((closeTaxLot.TaxLotQty == 0) && (matchingEligibleTaxLot.TaxLotQty == 0) || closingData.IsError)
                            {
                                openTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                                closeTaxLotsAndPositions.Remove(closeTaxLot);
                            }
                            //quantity of opening taxlot is smaller than closing taxlot and opening taxlot is completely closed 
                            else if ((isOpeningTaxLotSmaller && matchingEligibleTaxLot.TaxLotQty == 0) || closingData.IsError)
                            {
                                openTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                            }
                            //quantity of closing taxlot is smaller than opening taxlot and closing taxlot is completely closed 
                            else if ((!isOpeningTaxLotSmaller && closeTaxLot.TaxLotQty == 0) || closingData.IsError)
                            {
                                closeTaxLotsAndPositions.Remove(closeTaxLot);
                            }
                            #region commented closing for SellShort or SellToOpen
                            // i.e. this is hit,when the tax lot on sell grid is Sell Short.

                            //else
                            //{
                            //    isOpeningTaxLotSmaller = false;
                            //    //if (closeTaxLot.TaxLotQty.Equals(matchingEligibleTaxLot.TaxLotQty) && BuyTaxLotQtyToClose.Equals(closeTaxLot.TaxLotQty))
                            //    //{
                            //    //    closingData = ClosePosition(matchingEligibleTaxLot, closeTaxLot, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, algorithm, isVirtualClosingPopulate,isMatchStrategy,isOverrideWithUserClosing);
                            //    //    openTaxLotsAndPositions.Remove(closeTaxLot);
                            //    //    closeTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                            //    //    // break;
                            //    //}
                            //    //else
                            //    //{
                            //    if (closeTaxLot.TaxLotQty < matchingEligibleTaxLot.TaxLotQty)
                            //    {
                            //        isOpeningTaxLotSmaller = true;
                            //    }
                            //    closingData = ClosePosition(closeTaxLot, matchingEligibleTaxLot, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, algorithm, isVirtualClosingPopulate, isMatchStrategy, isOverrideWithUserClosing);
                            //    //quantity of opening taxlot is similrar to closing taxlot then closing taxlot and opening taxlot are completely closed 
                            //    if ((closeTaxLot.TaxLotQty == 0) && (matchingEligibleTaxLot.TaxLotQty == 0) || closingData.IsError)
                            //    {
                            //        openTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                            //        closeTaxLotsAndPositions.Remove(closeTaxLot);
                            //    }
                            //    if ((isOpeningTaxLotSmaller && closeTaxLot.TaxLotQty == 0) || closingData.IsError)
                            //    {
                            //        closeTaxLotsAndPositions.Remove(closeTaxLot);
                            //    }
                            //    if ((!isOpeningTaxLotSmaller && matchingEligibleTaxLot.TaxLotQty == 0) || closingData.IsError)
                            //    {
                            //        openTaxLotsAndPositions.Remove(matchingEligibleTaxLot);
                            //    }
                            //}
                            #endregion
                        }
                        else
                        {
                            break;
                        }

                        UpdateClosingData(ClosedData, closingData, true);

                    }
                    RemoveZeroOpenQtyFromList(openTaxLotsAndPositions);
                    RemoveZeroOpenQtyFromList(closeTaxLotsAndPositions);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ClosedData;
        }

        #endregion
        #endregion

        #region Public
        #region Expiration

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TaxLotTaxLotID"></param>
        /// <param name="auecTaxLotID"></param>
        /// <returns></returns>
        public string ArePositionElligibleToExpire(string TaxLotTaxLotID, int auecTaxLotID, bool IsCurrentDateClosing, DateTime CloseTradeDate)
        {
            Logger.LoggerWrite("Checking for Position Eligible to Expire, TaxLotTaxLotID: " + TaxLotTaxLotID + ", CloseTradeDate: " + CloseTradeDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"TaxLotTaxLotID", TaxLotTaxLotID},
                {"auecTaxLotID", auecTaxLotID},
                {"IsCurrentDateClosing", IsCurrentDateClosing},
                {"CloseTradeDate", CloseTradeDate}
            });

            DateTime selectedFilterDate = DateTimeConstants.MinValue;
            //Passed TaxLotTaxLotID.AUECTaxLotID because after some rules checking it would be sure that auecid for those 2 TaxLots are same.

            //if (_preferences.IsCurrentDateClosing)
            //{
            //    selectedFilterDate = TimeZoneHelper.GetAUECLocalDateFromUTC(auecTaxLotID, DateTime.UtcNow);
            //}
            StringBuilder alreadyClosedError = new StringBuilder();
            try
            {
                if (IsCurrentDateClosing)
                {
                    selectedFilterDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(DateTime.UtcNow, CachedDataManager.GetInstance.GetAUECTimeZone(auecTaxLotID));
                }
                ////else
                ////{
                ////    selectedFilterDate = _preferences.CloseTradeDate;
                ////}
                else
                {
                    selectedFilterDate = CloseTradeDate;
                }


                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache.ContainsKey(TaxLotTaxLotID) && _closingInfoCache[TaxLotTaxLotID].AUECMaxModifiedDate.Date > selectedFilterDate.Date)
                    {
                        alreadyClosedError.Append("TaxLotId : ");
                        alreadyClosedError.Append(TaxLotTaxLotID);
                        alreadyClosedError.Append(" is partially Expired or Expired on AUEC date : ");
                        alreadyClosedError.Append(_closingInfoCache[TaxLotTaxLotID].AUECMaxModifiedDate);
                        alreadyClosedError.Append(".");

                        alreadyClosedError.Append(Environment.NewLine);
                        alreadyClosedError.Append("Please unwind closed TaxLots before attempting to close in the back date.");

                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            if (!string.IsNullOrEmpty(alreadyClosedError.ToString()))
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ApplicationConstants.CONST_CLOSING_SERVICE + alreadyClosedError.ToString(), LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);

            return alreadyClosedError.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TaxLotTaxLotID"></param>
        /// <param name="OTCPositionDate"></param>
        /// <returns></returns>
        public StringBuilder ArePositionElligibleToExercise(string TaxLotTaxLotID, DateTime OTCPositionDate)
        {
            Logger.LoggerWrite("Checking for Position Eligible to Exercise, TaxLotTaxLotID: " + TaxLotTaxLotID + ", OTCPositionDate: " + OTCPositionDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"TaxLotTaxLotID", TaxLotTaxLotID},
                {"OTCPositionDate", OTCPositionDate}
            });

            StringBuilder alreadyClosedError = new StringBuilder();
            try
            {
                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache.ContainsKey(TaxLotTaxLotID))
                    {
                        if (_closingInfoCache[TaxLotTaxLotID].AUECMaxModifiedDate.Date > OTCPositionDate.Date)
                        {

                            alreadyClosedError.Append("TaxLotId : ");
                            alreadyClosedError.Append(TaxLotTaxLotID);
                            alreadyClosedError.Append(" is partially Assigned on AUEC date : ");
                            alreadyClosedError.Append(_closingInfoCache[TaxLotTaxLotID].AUECMaxModifiedDate);
                            alreadyClosedError.Append(".");

                            alreadyClosedError.Append(Environment.NewLine);
                            alreadyClosedError.Append("Please unwind closed TaxLots before attempting to close in the back date.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return alreadyClosedError;
        }

        #endregion

        #region Closing and CorporateAction Checks
        ///
        /// <summary>
        ///Closes the taxlots on Manual or Preset Basis
        ///Manual Mode: Refers to the UI mode in which user selects the Algo for closing.
        ///Preset Basis: Closing based on preferences saved in the Database.
        ///isVirtualClosingPopulate: This bool variable stop saving and publishing of closed data and populate virtual closing 
        /// </summary>
        /// <param name="buyTaxLotsAndPositions"></param>
        /// <param name="sellTaxLotsAndPositions"></param>
        /// <param name="algorithm"></param>
        /// <param name="IsShortWithBuyAndBuyToCover"></param>
        /// <param name="IsSellWithBuyToClose"></param>
        /// <param name="isManual">
        /// Determines whether the mode is manual or Preset
        /// </param>
        /// <param name="isDragDrop">
        /// Determines whether the closing is done by drag drop.
        /// </param>
        /// <param name="isFromServer"></param>
        /// <param name="secondarySort"></param>
        /// <param name="isVirtualClosingPopulate">
        /// isVirtualClosingPopulate: This bool variable stop saving and publishing of closed data and populate virtual closing
        /// </param>
        /// <param name="isOverrideWithUserClosing">
        /// isOverrideWithUserClosing : On the basis of this bool variable(true) closing is done based on user defined quantity(TaxLotQuantityToClose)
        /// </param>
        /// <param name="isMatchStrategy">
        /// isMatchStrategy: in Deafault closing methododly closing is done by including strategy match(isMatchStrategy=true), from Close Order UI strategy match can be ignored(isMatchStrategy=false) based on checkbox.
        /// </param>
        /// <returns></returns>
        public ClosingData AutomaticClosingOnManualOrPresetBasis(ClosingParameters closingParams)
        {
            Logger.LoggerWrite("Starting Automatic Closing On Manual Or Preset Basis.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"closingParams", closingParams}
            });

            ClosingData UpdatedClosingData = new ClosingData();


            //Checking the buy and sell taxlots are updated or not.
            VerifyClosingParamsData(closingParams, UpdatedClosingData);

            if (UpdatedClosingData.IsError)
                return UpdatedClosingData;


            try
            {

                bool returnLatestModifiedTaxlots = false;
                if (closingParams.IsFromServer)
                {
                    returnLatestModifiedTaxlots = true;
                }
                //closing is done manually by drag and drop
                if (closingParams.IsDragDrop)
                {
                    // ClosingData ClosedData = new ClosingData();

                    if (closingParams.BuyTaxLotsAndPositions.Count > 0 && closingParams.SellTaxLotsAndPositions.Count > 0)
                    {

                        for (int i = 0; i < closingParams.BuyTaxLotsAndPositions.Count; i++)
                        {
                            if (i < closingParams.BuyTaxLotsAndPositions.Count && i < closingParams.SellTaxLotsAndPositions.Count)
                            {

                                SetClosingDateBasedOnPreferences(closingParams.BuyTaxLotsAndPositions[i]);
                                SetClosingDateBasedOnPreferences(closingParams.SellTaxLotsAndPositions[i]);
                                //ClosingData closedDataCurrent = ClosePosition(closingParams);
                                ClosingData closedDataCurrent = ClosePosition(closingParams.BuyTaxLotsAndPositions[i], closingParams.SellTaxLotsAndPositions[i], closingParams.IsShortWithBuyAndBuyToCover, closingParams.IsSellWithBuyToClose, closingParams.Algorithm, closingParams.IsVirtualClosingPopulate, closingParams.IsMatchStrategy, closingParams.IsOverrideWithUserClosing, closingParams.VirtualUnwidedTaxlots, closingParams.IsCopyOpeningTradeAttributes);
                                if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                                {
                                    foreach (var closedPosition in closedDataCurrent.ClosedPositions)
                                    {
                                        if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closedPosition.ClosingTradeDate))
                                        {
                                            UpdatedClosingData.IsError = true;
                                            UpdatedClosingData.IsNavLockFailed = true;
                                            UpdatedClosingData.ErrorMsg = new StringBuilder("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                                                            + "). Please reach out to your Support Team for further assistance");
                                            return UpdatedClosingData;
                                        }
                                    } 
                                }
                                UpdateClosingData(UpdatedClosingData, closedDataCurrent, returnLatestModifiedTaxlots);
                            }
                        }
                    }
                    //   return ClosedData;
                }
                //Narendra Kumar jangir May 31 2013
                //Closing is done on the basis of close quantity filled by user on the Close Order UI
                else if (closingParams.IsOverrideWithUserClosing)
                {
                    //validation to check that for a particular account, sum of positional taxlots quantity is less than sum of closing taxlots quantity
                    UpdatedClosingData = ValiadateClosing(closingParams.BuyTaxLotsAndPositions, closingParams.SellTaxLotsAndPositions);
                    if (!UpdatedClosingData.IsError)
                    {
                        //closing algo modified to fifo as closed Quantity is already populated based on algo, so default algo is FIFO
                        ClosingData closingData = AutomaticClosing(closingParams.BuyTaxLotsAndPositions, closingParams.SellTaxLotsAndPositions, closingParams.Algorithm, closingParams.IsShortWithBuyAndBuyToCover, closingParams.IsSellWithBuyToClose, returnLatestModifiedTaxlots, false, PostTradeEnums.SecondarySortCriteria.None, closingParams.IsVirtualClosingPopulate, closingParams.IsMatchStrategy, closingParams.IsOverrideWithUserClosing, closingParams.ClosingField, closingParams.IsCopyOpeningTradeAttributes);
                        if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                        {
                            foreach (var closedPosition in closingData.ClosedPositions)
                            {
                                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closedPosition.ClosingTradeDate))
                                {
                                    UpdatedClosingData.IsError = true;
                                    UpdatedClosingData.IsNavLockFailed = true;
                                    UpdatedClosingData.ErrorMsg = new StringBuilder("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                                                    + "). Please reach out to your Support Team for further assistance");
                                    return UpdatedClosingData;
                                }
                            }
                        }
                        UpdateClosingData(UpdatedClosingData, closingData, returnLatestModifiedTaxlots);
                    }
                }
                else
                {
                    #region commented by omshiv, ACA cleanup, need discussion here
                    //List<string> eligibleAccountlist = new List<string>();
                    //foreach (KeyValuePair<string, DataRow> kp in _preferences.ClosingMethodology.DictAccountingMethods)
                    //{
                    //    if (Convert.ToBoolean(kp.Value["IsACA"].ToString()))
                    //    {
                    //        string[] splitData = kp.Key.Split('_');
                    //        if (splitData.Length > 0)
                    //        {
                    //            eligibleAccountlist.Add(splitData[0]);
                    //        }
                    //    }
                    //}
                    //if (eligibleAccountlist.Count > 0)
                    //{
                    //    if (!_isACACalculationRunning)
                    //    {
                    //        
                    // The primary layer is for ACA irrespective of mode of closing and algo.
                    //        List<TaxLot> EligibleBuyTaxlots = ACAManager.GetACATaxlotsForClosing(buyTaxLotsAndPositions, eligibleAccountlist);
                    //        List<TaxLot> EligibleSellTaxlots = ACAManager.GetACATaxlotsForClosing(sellTaxLotsAndPositions, eligibleAccountlist);

                    //        if (EligibleBuyTaxlots.Count > 0 && EligibleSellTaxlots.Count > 0)
                    //        {
                    //            List<DateSymbol> lstSymbols = GetALLClosingDates(EligibleBuyTaxlots, EligibleSellTaxlots);
                    //            lstSymbols.Sort(delegate(DateSymbol t1, DateSymbol t2) { return t1.FromDate.CompareTo(t2.FromDate); });
                    //            if (lstSymbols.Count > 0)
                    //            {
                    //                DateSymbol dateMax = lstSymbols[lstSymbols.Count - 1];
                    //                if (dateMax.FromDate > _ACALatestCalculationDate)
                    //                {
                    //                    UpdatedClosingData.ErrorMsg.Append("ACA data has not been calculated for ");
                    //                    UpdatedClosingData.ErrorMsg.Append(dateMax.FromDate.ToString());
                    //                    UpdatedClosingData.ErrorMsg.Append(".");
                    //                    UpdatedClosingData.ErrorMsg.Append("Please Click SaveACA button before running closing.");
                    //                    // return UpdatedClosingData;
                    //                }
                    //            }

                    //            ACAManager.CreateACADateWiseDictionary(lstSymbols);
                    //            ClosingData closingData = AutomaticClosing(EligibleBuyTaxlots, EligibleSellTaxlots, PostTradeEnums.CloseTradeAlogrithm.ACA, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, returnLatestModifiedTaxlots, false, PostTradeEnums.SecondarySortCriteria.None, isVirtualClosingPopulate, isMatchStrategy, isOverrideWithUserClosing);
                    //            UpdateClosingData(UpdatedClosingData, closingData, returnLatestModifiedTaxlots);
                    //        } 
                    //        #endregion
                    //    }
                    //    else
                    //    {
                    //        UpdatedClosingData.ErrorMsg.Append("ACA Calculation is running in background");
                    //        UpdatedClosingData.ErrorMsg.Append(".");
                    //        UpdatedClosingData.ErrorMsg.Append("Please wait for few seconds and again run closing.");
                    //        // return UpdatedClosingData;
                    //    }
                    //}

                    #endregion

                    if (closingParams.BuyTaxLotsAndPositions.Count > 0 && closingParams.SellTaxLotsAndPositions.Count > 0)
                    {
                        //romoved ACAMode check- omshiv
                        //For ACA closing we need to close proportional fractional quantity which will be filled by by ACA algorithm in secondary sort criteria for each opening taxlot depending on proportion of that taxlot in total open quantity of that account+symbol.
                        if (closingParams.Algorithm.Equals(PostTradeEnums.CloseTradeAlogrithm.ACA)) //_preferences.AcaMode.Equals(PostTradeEnums.ACAMode.FractionalClosing) && 
                        {
                            closingParams.IsOverrideWithUserClosing = true;
                        }
                        if (closingParams.IsManual)
                        {
                            //PostTradeEnums.SecondarySortCriteria secondarySort = (PostTradeEnums.SecondarySortCriteria)(_preferences.SecondarySort);
                            ClosingData closeData = AutomaticClosing(closingParams.BuyTaxLotsAndPositions, closingParams.SellTaxLotsAndPositions, closingParams.Algorithm, closingParams.IsShortWithBuyAndBuyToCover, closingParams.IsSellWithBuyToClose, returnLatestModifiedTaxlots, false, closingParams.SecondarySort, closingParams.IsVirtualClosingPopulate, closingParams.IsMatchStrategy, closingParams.IsOverrideWithUserClosing, closingParams.ClosingField, closingParams.IsCopyOpeningTradeAttributes);
                            if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                            {
                                foreach (var closedPosition in closeData.ClosedPositions)
                                {
                                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closedPosition.ClosingTradeDate))
                                    {
                                        UpdatedClosingData.IsError = true;
                                        UpdatedClosingData.IsNavLockFailed = true;
                                        UpdatedClosingData.ErrorMsg = new StringBuilder("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                                                        + "). Please reach out to your Support Team for further assistance");
                                        return UpdatedClosingData;
                                    }
                                } 
                            }
                            UpdateClosingData(UpdatedClosingData, closeData, returnLatestModifiedTaxlots);
                            // return UpdatedClosingData;
                        }
                        else
                        {
                            closingParams.IsShortWithBuyAndBuyToCover = _preferences.ClosingMethodology.IsShortWithBuyandBTC;
                            closingParams.IsSellWithBuyToClose = _preferences.ClosingMethodology.IsSellWithBTC;

                            Dictionary<string, List<TaxLot>> _dictbuyTrades = new Dictionary<string, List<TaxLot>>();
                            //bool IsShortWithBuyAndBuyToCover = _preferences.IsShortWithBuyandBTC;
                            Dictionary<string, List<TaxLot>> _dictsellTrades = new Dictionary<string, List<TaxLot>>();

                            List<int> _accountList = new List<int>();
                            List<int> _assetList = new List<int>();

                            foreach (TaxLot TaxLot in closingParams.BuyTaxLotsAndPositions)
                            {
                                int assetTaxLotID = CachedDataManager.GetInstance.GetAssetIdByAUECId(TaxLot.AUECID);
                                int AccountTaxLotID = TaxLot.Level1ID;
                                string key = assetTaxLotID.ToString() + AccountTaxLotID.ToString();
                                if (!_accountList.Contains(AccountTaxLotID))
                                {
                                    _accountList.Add(AccountTaxLotID);
                                }
                                if (!_assetList.Contains(assetTaxLotID))
                                {
                                    _assetList.Add(assetTaxLotID);
                                }
                                if (_dictbuyTrades.ContainsKey(key))
                                {
                                    _dictbuyTrades[key].Add(TaxLot);
                                }
                                else
                                {
                                    List<TaxLot> TaxLotsList = new List<TaxLot>();
                                    TaxLotsList.Add(TaxLot);
                                    _dictbuyTrades.Add(key, TaxLotsList);
                                }
                            }
                            foreach (TaxLot TaxLot in closingParams.SellTaxLotsAndPositions)
                            {
                                int assetTaxLotID = CachedDataManager.GetInstance.GetAssetIdByAUECId(TaxLot.AUECID);
                                int AccountTaxLotID = TaxLot.Level1ID;
                                string key = assetTaxLotID.ToString() + AccountTaxLotID.ToString();

                                if (_dictsellTrades.ContainsKey(key))
                                {
                                    _dictsellTrades[key].Add(TaxLot);
                                }
                                else
                                {
                                    List<TaxLot> TaxLotsList = new List<TaxLot>();
                                    TaxLotsList.Add(TaxLot);
                                    _dictsellTrades.Add(key, TaxLotsList);
                                }
                            }
                            foreach (int accountTaxLotID in _accountList)
                            {
                                foreach (int assetTaxLotID in _assetList)
                                {
                                    string key = assetTaxLotID.ToString() + accountTaxLotID.ToString();
                                    //int presetAlgorithm = GetMethodologybyAccountID(accountTaxLotID, assetTaxLotID, AccountingMethods.Tables[0]);
                                    PostTradeEnums.CloseTradeAlogrithm presetAlgorithm = (PostTradeEnums.CloseTradeAlogrithm)GetMethodologybyAccountID(accountTaxLotID, assetTaxLotID);
                                    PostTradeEnums.SecondarySortCriteria secondarySortCriteria = GetSecondarySortCriteria(accountTaxLotID, assetTaxLotID);
                                    PostTradeEnums.ClosingField closingField = GetClosingField(accountTaxLotID, assetTaxLotID);
                                    if (_dictbuyTrades.ContainsKey(key) && _dictsellTrades.ContainsKey(key))
                                    {
                                        if (presetAlgorithm.Equals(PostTradeEnums.CloseTradeAlogrithm.ACA))
                                        {
                                            closingParams.IsOverrideWithUserClosing = true;
                                        }
                                        else
                                        {
                                            closingParams.IsOverrideWithUserClosing = false;
                                        }
                                        ClosingData ClosedData = AutomaticClosing(_dictbuyTrades[key], _dictsellTrades[key], (PostTradeEnums.CloseTradeAlogrithm)presetAlgorithm, closingParams.IsShortWithBuyAndBuyToCover, closingParams.IsSellWithBuyToClose, returnLatestModifiedTaxlots, false, secondarySortCriteria, closingParams.IsVirtualClosingPopulate, closingParams.IsMatchStrategy, closingParams.IsOverrideWithUserClosing, closingField, closingParams.IsCopyOpeningTradeAttributes);
                                        if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                                        {
                                            foreach (var closedPosition in ClosedData.ClosedPositions)
                                            {
                                                if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closedPosition.ClosingTradeDate))
                                                {
                                                    UpdatedClosingData.IsError = true;
                                                    UpdatedClosingData.IsNavLockFailed = true;
                                                    UpdatedClosingData.ErrorMsg = new StringBuilder("The date you’ve chosen for this action precedes your NAV Lock date (" + CachedDataManager.GetInstance.NAVLockDate.Value.ToShortDateString()
                                                                                    + "). Please reach out to your Support Team for further assistance");
                                                    return UpdatedClosingData;
                                                }
                                            }
                                        }
                                        UpdateClosingData(UpdatedClosingData, ClosedData, returnLatestModifiedTaxlots);
                                    }
                                }
                            }

                        }
                    }

                }
                //stop saving and publishing of closing data when populate is done from new closing ui.
                //isVirtualClosingPopulate is a bool variable(true: closing is from new allocation; false: closing is not from new allocation)
                if (UpdatedClosingData.UnSavedTaxlots.Count > 0 && !closingParams.IsFromServer && !closingParams.IsVirtualClosingPopulate)
                {
                    //save closed data in the database
                    int rowsAffected = SaveCloseTradesData(UpdatedClosingData);

                    if (rowsAffected > 0)
                    {
                        UpdatedClosingData.IsDataClosed = true;
                    }
                }
                //}
                //else
                //{
                //    UpdatedClosingData.IsError = true;
                //    UpdatedClosingData.ErrorMsg = UpdatedClosingData.ErrorMsg.Append("Invalid Seconadry Sort Criteria, Vaild Criteria are None and Order sequence");
                //}
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

            return UpdatedClosingData;
        }

        /// <summary>
        /// Verify Closing Params Data
        /// </summary>
        /// <param name="closingParams"></param>
        /// <param name="UpdatedClosingData"></param>
        private void VerifyClosingParamsData(ClosingParameters closingParams, ClosingData UpdatedClosingData)
        {
            lock (_closingInfoCacheLock)
            {
                foreach (var taxlot in closingParams.BuyTaxLotsAndPositions)
                {
                    //Due to problem in differences in last decimal place like 1.2323111 and 1.232311, so we are comparing the two decimals only.after that it will show mismatch   
                    if (_closingInfoCache.ContainsKey(taxlot.TaxLotID) && Math.Round(_closingInfoCache[taxlot.TaxLotID].OpenQuantity, 2) != Math.Round(taxlot.TaxLotQty, 2))
                    {
                        UpdatedClosingData.IsError = true;
                        UpdatedClosingData.ErrorMsg = new StringBuilder("Please refresh the data.");
                        Logger.LoggerWrite("MisMatched on VerifyClosingParamsData, for taxlot " + taxlot.TaxLotID + ", closingInfoCache QTY= " + _closingInfoCache[taxlot.TaxLotID].OpenQuantity + ",taxlot QTY = " + taxlot.TaxLotQty, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

                        return;
                    }

                }

                foreach (var taxlot in closingParams.SellTaxLotsAndPositions)
                {
                    if (_closingInfoCache.ContainsKey(taxlot.TaxLotID) && Math.Round(_closingInfoCache[taxlot.TaxLotID].OpenQuantity, 2) != Math.Round(taxlot.TaxLotQty, 2))
                    {
                        UpdatedClosingData.IsError = true;
                        UpdatedClosingData.ErrorMsg = new StringBuilder("Please refresh the data.");
                        Logger.LoggerWrite("MisMatched on VerifyClosingParamsData, for taxlot " + taxlot.TaxLotID + ", closingInfoCache QTY= " + _closingInfoCache[taxlot.TaxLotID].OpenQuantity + ",taxlot QTY = " + taxlot.TaxLotQty, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

                        return;
                    }

                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="closingTemplates"></param>
        /// <returns></returns>

        public ClosingData AutomaticClosingBasedOnTemplates(List<ClosingTemplate> closingTemplates)
        {
            Logger.LoggerWrite("Automatic Closing Based On Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"closingTemplates", closingTemplates}
            });

            ClosingData closingData = new ClosingData();
            StringBuilder errorMessage = new StringBuilder();
            ProgressInfo info = null;
            List<string> navLockFailedTemplates = new List<string>();

            //cloning the preferences object as this preference object corresponds to central closing preferences set from closing preference module...
            ClosingPreferences preferences = DeepCopyHelper.Clone<ClosingPreferences>(_preferences);

            try
            {
                int i = 0;
                foreach (ClosingTemplate template in closingTemplates)
                {
                    //updating the preference object based on closing methodology set for each template...
                    _preferences.ClosingMethodology = template.ClosingMeth;

                    #region Filters

                    #region Publish Progress

                    info = new ProgressInfo();
                    info.HeaderText = "Processing request for Closing Template " + template.TemplateName + "...";
                    if (template.UseCurrentDate)
                    {
                        info.ProgressStatus = "Fetching open positions till " + DateTime.UtcNow.Date + " based on Applied filters...";
                    }
                    else
                    {
                        info.ProgressStatus = "Fetching open positions till " + template.ToDate + " based on Applied filters...";
                    }
                    PublishProgress(info);

                    #endregion

                    Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                    string CommaSeparatedAccountIDs = template.GetCommaSeparatedAccounts(dictMasterFundSubAccountAssociation);
                    string CommaSeparatedAssetIDs = template.GetCommaSeparatedAssets();
                    string commaSeparatedSymbols = template.GetCommaSeparatedSymbols();
                    string CustomFilterCondition = SqlParser.GetDynamicConditionQuerry(template.DictCustomConditions);

                    #endregion

                    #region DATA FETCHING

                    List<TaxLot> openTaxlots = GetOpenPositionsFromDB(template.ToDate, template.UseCurrentDate, CommaSeparatedAccountIDs, CommaSeparatedAssetIDs, commaSeparatedSymbols, CustomFilterCondition);

                    List<TaxLot> BuyTaxlots = new List<TaxLot>();
                    List<TaxLot> SellTaxlots = new List<TaxLot>();

                    foreach (TaxLot taxlot in openTaxlots)
                    {
                        if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                        {
                            BuyTaxlots.Add(taxlot);
                        }
                        else
                        {
                            SellTaxlots.Add(taxlot);
                        }
                    }

                    #endregion

                    #region Closing

                    #region Publish Progress
                    info = new ProgressInfo();
                    info.HeaderText = "Processing request for Closing Template " + template.TemplateName + "...";
                    info.ProgressStatus = "Running Closing...";
                    PublishProgress(info);
                    #endregion

                    ClosingParameters closingParams = new ClosingParameters();
                    closingParams.BuyTaxLotsAndPositions = BuyTaxlots;
                    closingParams.SellTaxLotsAndPositions = SellTaxlots;
                    closingParams.Algorithm = template.ClosingMeth.ClosingAlgo;
                    closingParams.IsShortWithBuyAndBuyToCover = template.ClosingMeth.IsShortWithBuyandBTC;
                    closingParams.IsSellWithBuyToClose = template.ClosingMeth.IsSellWithBTC;
                    closingParams.IsManual = false;
                    closingParams.IsDragDrop = false;
                    closingParams.IsFromServer = false;
                    closingParams.SecondarySort = template.ClosingMeth.SecondarySort;
                    closingParams.IsVirtualClosingPopulate = false;
                    closingParams.IsOverrideWithUserClosing = false;
                    closingParams.IsMatchStrategy = !template.ClosingMeth.IsAutoCloseStrategy;
                    closingParams.ClosingField = template.ClosingMeth.ClosingField;
                    ClosingData closedData = AutomaticClosingOnManualOrPresetBasis(closingParams);
                    #endregion

                    #region Publish Progress
                    info = new ProgressInfo();
                    info.HeaderText = "Processing request for Closing Template " + template.TemplateName + "...";

                    if (closedData.IsNavLockFailed)
                    {
                        closingData.IsNavLockFailed = true;
                        info.ProgressStatus = "Nav Lock Validation Failed";
                        navLockFailedTemplates.Add(template.TemplateName);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(closedData.ErrorMsg.ToString()))
                        {
                            if (closedData.ClosedPositions.Count > 0)
                            {
                                info.ProgressStatus = "Data Closed Successfully...";
                            }
                            else
                            {
                                info.ProgressStatus = "Nothing to Close...";
                            }
                        }
                        else
                        {
                            info.ProgressStatus = "Some errors occurred while closing. For more details view the error log after the operation is complete...";

                            errorMessage.Append(Environment.NewLine);
                            errorMessage.Append("Erros in ");
                            errorMessage.Append(template.TemplateName);
                            errorMessage.Append("................");
                            errorMessage.Append(Environment.NewLine);
                            errorMessage.Append(closedData.ErrorMsg.ToString());
                        }
                    }

                    i++;
                    if (i == closingTemplates.Count)
                    {
                        info.IsTaskCompleted = true;
                    }
                    PublishProgress(info);
                    #endregion

                }

                _preferences = preferences;
                if (closingData.IsNavLockFailed)
                {
                    closingData.NavLockFailedTemplates = string.Join(",", navLockFailedTemplates);
                }
                closingData.ErrorMsg = errorMessage;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return closingData;
        }


        private void PublishProgress(ProgressInfo info)
        {
            try
            {

                MessageData data = new MessageData();
                List<ProgressInfo> listProgress = new List<ProgressInfo>();
                listProgress.Add(info);
                data.EventData = listProgress;
                data.TopicName = Topics.Topic_ClosingStatus;
                BufferMessage(dataBuffer, data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Add To Closing Info Cache
        /// </summary>
        /// <param name="PositionalTaxlot"></param>
        /// <param name="ClosingTaxlot"></param>
        /// <param name="algorithm"></param>
        private void AddToClosingInfoCache(TaxLot PositionalTaxlot, TaxLot ClosingTaxlot, PostTradeEnums.CloseTradeAlogrithm algorithm)
        {
            // positional taxlot added
            try
            {
                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache.ContainsKey(PositionalTaxlot.TaxLotID))
                    {
                        TaxlotClosingInfo info = _closingInfoCache[PositionalTaxlot.TaxLotID];
                        info.ListClosingId.Add(ClosingTaxlot.TaxLotID);
                        info.AUECMaxModifiedDate = PositionalTaxlot.AUECModifiedDate;
                        info.OpenQuantity = PositionalTaxlot.TaxLotQty;
                        //Modified By : Manvendra Jira : PRANA-10341
                        if (info.ClosingAlgo != (int)algorithm)
                        {
                            info.ClosingAlgo = (int)PostTradeEnums.CloseTradeAlogrithm.Multiple;
                        }
                        else
                        {
                            info.ClosingAlgo = (int)algorithm;
                        }
                        if (PositionalTaxlot.TaxLotQty > 0)
                        {
                            info.ClosingStatus = ClosingStatus.PartiallyClosed;
                        }
                        else
                            info.ClosingStatus = ClosingStatus.Closed;
                        if (PositionalTaxlot.IsManualyExerciseAssign != null)
                            info.IsManualyExerciseAssign = PositionalTaxlot.IsManualyExerciseAssign;
                        Logger.LoggerWrite("AddToClosingInfoCache, TaxlotID: " + PositionalTaxlot.TaxLotID + " = " + info.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                    }
                    else
                    {
                        TaxlotClosingInfo infonew = new TaxlotClosingInfo();
                        infonew.AUECMaxModifiedDate = PositionalTaxlot.AUECModifiedDate;
                        infonew.ClosingAlgo = (int)algorithm;
                        infonew.OpenQuantity = PositionalTaxlot.TaxLotQty;
                        infonew.TaxLotID = PositionalTaxlot.TaxLotID;
                        infonew.ListClosingId.Add(ClosingTaxlot.TaxLotID);
                        if (PositionalTaxlot.TaxLotQty > 0)
                        {
                            infonew.ClosingStatus = ClosingStatus.PartiallyClosed;
                        }
                        else

                            infonew.ClosingStatus = ClosingStatus.Closed;
                        if (PositionalTaxlot.IsManualyExerciseAssign != null)
                            infonew.IsManualyExerciseAssign = PositionalTaxlot.IsManualyExerciseAssign;
                        _closingInfoCache.Add(PositionalTaxlot.TaxLotID, infonew);

                        Logger.LoggerWrite("AddToClosingInfoCache, TaxlotID: " + PositionalTaxlot.TaxLotID + " = " + infonew.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                    }

                    //Add Closing taxlot.
                    if (_closingInfoCache.ContainsKey(ClosingTaxlot.TaxLotID))
                    {
                        TaxlotClosingInfo infoClosingTaxlot = _closingInfoCache[ClosingTaxlot.TaxLotID];
                        infoClosingTaxlot.ListClosingId.Add(PositionalTaxlot.TaxLotID);
                        infoClosingTaxlot.AUECMaxModifiedDate = ClosingTaxlot.AUECModifiedDate;
                        infoClosingTaxlot.OpenQuantity = ClosingTaxlot.TaxLotQty;
                        if (infoClosingTaxlot.ClosingAlgo != (int)algorithm)
                        {
                            infoClosingTaxlot.ClosingAlgo = (int)PostTradeEnums.CloseTradeAlogrithm.Multiple;
                        }
                        else
                        {
                            infoClosingTaxlot.ClosingAlgo = (int)algorithm;
                        }
                        if (ClosingTaxlot.TaxLotQty > 0)
                        {
                            infoClosingTaxlot.ClosingStatus = ClosingStatus.PartiallyClosed;
                        }
                        else
                            infoClosingTaxlot.ClosingStatus = ClosingStatus.Closed;
                        if (ClosingTaxlot.IsManualyExerciseAssign != null)
                        {
                            infoClosingTaxlot.IsManualyExerciseAssign = ClosingTaxlot.IsManualyExerciseAssign;
                        }

                        Logger.LoggerWrite("AddToClosingInfoCache, TaxlotID: " + ClosingTaxlot.TaxLotID + " = " + infoClosingTaxlot.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                            {"Current Method", MethodBase.GetCurrentMethod()}
                        });

                    }
                    else
                    {
                        TaxlotClosingInfo infonewClosingTaxlot = new TaxlotClosingInfo();
                        infonewClosingTaxlot.AUECMaxModifiedDate = ClosingTaxlot.AUECModifiedDate;
                        infonewClosingTaxlot.ClosingAlgo = (int)algorithm;
                        infonewClosingTaxlot.OpenQuantity = ClosingTaxlot.TaxLotQty;
                        infonewClosingTaxlot.TaxLotID = ClosingTaxlot.TaxLotID;
                        infonewClosingTaxlot.ListClosingId.Add(PositionalTaxlot.TaxLotID);
                        if (ClosingTaxlot.TaxLotQty > 0)
                        {
                            infonewClosingTaxlot.ClosingStatus = ClosingStatus.PartiallyClosed;
                        }
                        else
                            infonewClosingTaxlot.ClosingStatus = ClosingStatus.Closed;
                        if (ClosingTaxlot.IsManualyExerciseAssign != null)
                            infonewClosingTaxlot.IsManualyExerciseAssign = ClosingTaxlot.IsManualyExerciseAssign;
                        _closingInfoCache.Add(ClosingTaxlot.TaxLotID, infonewClosingTaxlot);

                        Logger.LoggerWrite("AddToClosingInfoCache, TaxlotID: " + ClosingTaxlot.TaxLotID + " = " + infonewClosingTaxlot.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                            {"Current Method", MethodBase.GetCurrentMethod()}
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }
        /// <summary>
        /// Gets all positoins from DataBase
        /// </summary>
        public async Task<ClosingData> GetAllClosingData(DateTime FromDate, DateTime Todate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string CommaSeparatedCustomConditions)
        {
            Logger.LoggerWrite("Getting All closing Data, from date: " + FromDate + ", To Date: " + Todate + ", for Accounts: " + CommaSeparatedAccountIds, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"CurrentMethod", MethodBase.GetCurrentMethod()},
                {"FromDate", FromDate},
                {"Todate", Todate},
                {"IsCurrentDateClosing", IsCurrentDateClosing},
                {"CommaSeparatedAccountIds", CommaSeparatedAccountIds},
                {"commaSeparatedSymbols", commaSeparatedSymbols},
                {"CommaSeparatedCustomConditions", CommaSeparatedCustomConditions}
            });

            ClosingData closingData = new ClosingData();

            try
            {

                if (FromDate.Date >= _fromClosingDate.Date && Todate.Date <= _toClosingDate.Date)
                {

                }
                else
                {
                    _toClosingDate = Todate.Date;
                    _fromClosingDate = FromDate.Date;
                    GetTaxLotsLatestClosingDatesFromDB(false);
                }
                closingData.Taxlots = await System.Threading.Tasks.Task.Run(() => GetOpenPositionsFromDB(Todate, IsCurrentDateClosing, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, CommaSeparatedCustomConditions));

                closingData.ClosedPositions = await System.Threading.Tasks.Task.Run(() => GetNetPositionsFromDB(FromDate, Todate, IsCurrentDateClosing, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, CommaSeparatedCustomConditions, GetPreferences().DateType));
            }

            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }


            //GetTaxLotsLatestClosingDatesFromDB();
            //GetTaxLotsLatestCADatesFromDB();
            return closingData;
        }

        /// <summary>
        /// Get open and closed positoins for a symbol from DataBase
        /// </summary>
        public ClosingData GetClosingDataForASymbol(string symbol, string CommaSeparatedAccountIds, string orderSideTagValue, string groupID)
        {
            Logger.LoggerWrite(string.Format("Getting closing Data for symbol: {0}, groupID: {1}, ", symbol, groupID), LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"symbol", symbol},
                {"CommaSeparatedAccountIds", CommaSeparatedAccountIds},
                {"orderSideTagValue", orderSideTagValue},
                {"groupID", groupID}
            });

            ClosingData closingData = new ClosingData();
            try
            {
                //if (FromDate.Date >= _fromClosingDate.Date && Todate.Date <= _toClosingDate.Date)
                //{

                //}
                //else
                //{
                //    _toClosingDate = Todate.Date;
                //    _fromClosingDate = FromDate.Date;
                //    TaxlottoCloseCacheHandler closingInfoDictHandler = new TaxlottoCloseCacheHandler(GetTaxLotsLatestClosingDatesFromDB);
                //    closingInfoDictHandler.BeginInvoke(false, null, null);
                //}
                closingData.TaxlotsToPopulate = GetOpenPositionsFromDBForASymbol(symbol, CommaSeparatedAccountIds, orderSideTagValue);

                closingData.ClosedPositions = GetNetPositionsFromDBForASymbol(true, CommaSeparatedAccountIds, symbol, groupID);

            }

            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }


            //GetTaxLotsLatestClosingDatesFromDB();
            //GetTaxLotsLatestCADatesFromDB();
            return closingData;
        }


        /// <summary>
        /// Set Closing Date Based On Preferences
        /// </summary>
        /// <param name="taxlot"></param>
        private void SetClosingDateBasedOnPreferences(TaxLot taxlot)
        {
            try
            {

                switch (_preferences.DateType)
                {
                    case PostTradeEnums.DateType.AuecLocalDate:
                        taxlot.ClosingDate = taxlot.AUECLocalDate;
                        break;
                    case PostTradeEnums.DateType.ProcessDate:
                        taxlot.ClosingDate = taxlot.ProcessDate;
                        break;
                    case PostTradeEnums.DateType.OriginalPurchaseDate:
                        taxlot.ClosingDate = taxlot.OriginalPurchaseDate;
                        break;
                    default:
                        taxlot.ClosingDate = taxlot.OriginalPurchaseDate;
                        break;
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


        ///// <summary>
        ///// Gets All updated ClosingDates and CA Dates 
        ///// </summary>
        //public void RefreshClosingData()
        //{
        //    try
        //    {
        //        //GetTaxLotsLatestClosingDatesFromDB();

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDTHROW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        public void RefreshCADataonNewThread()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(delegate
                {
                    RefreshCAData();
                });
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Refresh CA Data
        /// </summary>
        public void RefreshCAData()
        {
            try
            {
                Logger.LoggerWrite("Refreshing CA Data", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                GetTaxLotsLatestCADatesFromDB();
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Unwind Positions
        /// </summary>
        /// <param name="TaxlotClosingIDWithClosingDate"></param>
        public void UnwindPositions(string TaxlotClosingIDWithClosingDate)
        {
            Logger.LoggerWrite("UnWinding Positions, TaxlotClosingIDWithClosingDate:" + TaxlotClosingIDWithClosingDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"TaxlotClosingIDWithClosingDate", TaxlotClosingIDWithClosingDate}
            });
            ClosingData closedData = new ClosingData();
            try
            {
                closedData.PositionsToUnwind = TaxlotClosingIDWithClosingDate;
                PublishClosingData(closedData);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }
        private void DeterminePositionalandClosingTaxlots(TaxLot targetTaxLot, TaxLot draggedTaxLot, out TaxLot positionalTaxlot, out TaxLot closingTaxlot)
        {
            DateTime targetTaxlotDate = targetTaxLot.ClosingDate.Date;
            DateTime draggedTaxLotDate = draggedTaxLot.ClosingDate.Date;

            positionalTaxlot = targetTaxLot;
            closingTaxlot = draggedTaxLot;

            if (targetTaxlotDate.Equals(draggedTaxLotDate))
            {
                // if the dates are equal then we need to consider the orderSide for determining the positional and closing Taxlots...
                switch (targetTaxLot.OrderSideTagValue)
                {


                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:

                        switch (draggedTaxLot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell_Open:


                                positionalTaxlot = draggedTaxLot;
                                closingTaxlot = targetTaxLot;
                                break;


                            default:
                                break;
                        }
                        break;

                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:

                        switch (draggedTaxLot.OrderSideTagValue)
                        {
                            case FIXConstants.SIDE_Buy_Closed:
                            case FIXConstants.SIDE_Buy_Cover:
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Closed:


                                positionalTaxlot = targetTaxLot;
                                closingTaxlot = draggedTaxLot;
                                break;

                            default:
                                break;
                        }
                        break;

                }
            }
            else
            {
                if (targetTaxlotDate > draggedTaxLotDate)
                {
                    positionalTaxlot = draggedTaxLot;
                    closingTaxlot = targetTaxLot;
                }
            }

        }

        /// <summary>
        /// Closes the position.
        /// </summary>
        /// <param name="targetTaxLot">The target allocated trade.</param>
        /// <param name="draggedTaxLot">The dragged allocated trade.</param>
        private ClosingData ClosePosition(TaxLot targetTaxLot, TaxLot draggedTaxLot, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, PostTradeEnums.CloseTradeAlogrithm algorithm, bool isVirtualClosingPopulate, bool isMatchStrategy, bool isOverrideWithUserClosing, List<string> VirtualUnwidedTaxlots, bool IsCopyOpeningTradeAttributes)
        {
            //DateTime targetTaxlotDate = targetTaxLot.ClosingDate.Date;
            //DateTime draggedTaxLotDate = draggedTaxLot.ClosingDate.Date;
            TaxLot positionalTaxlot = targetTaxLot;
            TaxLot closingTaxlot = draggedTaxLot;
            if (algorithm != PostTradeEnums.CloseTradeAlogrithm.NONE)
            {
                DeterminePositionalandClosingTaxlots(targetTaxLot, draggedTaxLot, out positionalTaxlot, out closingTaxlot);
            }
            ClosingData ClosedData = null;
            try
            {
                //bool retVal = true;

                //Narendra Kumar Jangir, 2013 May 28
                //strategy will not be matched if closing is populated from new CloseTradeInformation UI
                string rulesCheckString = CheckCloseRules(positionalTaxlot, closingTaxlot, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, isMatchStrategy, VirtualUnwidedTaxlots);
                if (rulesCheckString != string.Empty)
                {
                    ClosedData = new ClosingData();
                    ClosedData.IsError = true;
                    ClosedData.ErrorMsg.Append(rulesCheckString);
                    ClosedData.ErrorMsg.Append(Environment.NewLine);
                    return ClosedData;
                }

                ///new method is created to get the position tag and following code is commented
                ///Sandeep Singh on Feb 5, 2014
                #region Commented by Sandeep
                //switch (positionalTaxlot.OrderSideTagValue)
                //{
                //    case FIXConstants.SIDE_Buy:
                //    case FIXConstants.SIDE_Buy_Open:
                //    case FIXConstants.SIDE_Buy_Closed:
                //    case FIXConstants.SIDE_Buy_Cover:

                //        ClosedData = CloseTaxLot(positionalTaxlot, closingTaxlot, PositionTag.Long, algorithm, isVirtualClosingPopulate, isOverrideWithUserClosing);
                //        break;

                //    case FIXConstants.SIDE_SellShort:
                //    case FIXConstants.SIDE_Sell_Open:
                //    case FIXConstants.SIDE_Sell:
                //    case FIXConstants.SIDE_Sell_Closed:

                //        ClosedData = CloseTaxLot(positionalTaxlot, closingTaxlot, PositionTag.Short, algorithm, isVirtualClosingPopulate, isOverrideWithUserClosing);

                //        break;
                //}
                #endregion Commented by Sandeep

                PositionTag positionTag = GetPositionTag(positionalTaxlot.OrderSideTagValue);
                ClosedData = CloseTaxLot(positionalTaxlot, closingTaxlot, positionTag, algorithm, isVirtualClosingPopulate, isOverrideWithUserClosing, IsCopyOpeningTradeAttributes);

                #region commented
                //        switch (targetTaxLot.OrderSideTagValue)
                //        {
                //            case FIXConstants.SIDE_Buy:
                //            case FIXConstants.SIDE_Buy_Open:

                //                if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                //                {
                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                }
                //                else if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                //                {
                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                }

                //                break;

                //            case FIXConstants.SIDE_Buy_Closed:
                //            case FIXConstants.SIDE_Buy_Cover:
                //                ///Only short sell allowed 
                //                if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                //                {
                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                }
                //                else if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_SellShort) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Open))
                //                {
                //                    if (targetTaxlotDate >= draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                }

                //                break;

                //            case FIXConstants.SIDE_SellShort:
                //            case FIXConstants.SIDE_Sell_Open:
                //                if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                //                {
                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                }
                //                else if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                //                {
                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                    {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Long, algorithm);
                //                    }
                //                    else
                //                    {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Short, algorithm);
                //                    }

                //                }
                //                break;

                //            case FIXConstants.SIDE_Sell:
                //            case FIXConstants.SIDE_Sell_Closed:
                //                ///Only buy can be dragged here
                //                if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                //                {
                //                    if (targetTaxlotDate >= draggedTaxLotDate)
                //                {
                //                    ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Long, algorithm);
                //                }
                //                else
                //                {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Short, algorithm);
                //                }
                //                }
                //                else if (draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || draggedTaxLot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                //                {

                //                    if (targetTaxlotDate > draggedTaxLotDate)
                //                {
                //                        ClosedData = CloseTaxLot(draggedTaxLot, targetTaxLot, PositionTag.Long, algorithm);
                //                }
                //                else
                //                {
                //                        ClosedData = CloseTaxLot(targetTaxLot, draggedTaxLot, PositionTag.Short, algorithm);
                //                    }
                //                }
                //                break;


                //        }

                //return retVal;
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            return ClosedData;
        }

        /// <summary>
        /// Get position tag i.e. long/short on the basis of order side tag value
        /// Sandeep Singh on Feb 5, 2014
        /// </summary>
        /// <param name="OrderSideTagValue"></param>
        /// <returns></returns>
        private PositionTag GetPositionTag(string OrderSideTagValue)
        {
            PositionTag positionTag = PositionTag.None;
            try
            {
                switch (OrderSideTagValue)
                {
                    case FIXConstants.SIDE_Buy:
                    case FIXConstants.SIDE_Buy_Open:
                    case FIXConstants.SIDE_Buy_Closed:
                    case FIXConstants.SIDE_Buy_Cover:
                        positionTag = PositionTag.Long;
                        break;
                    case FIXConstants.SIDE_SellShort:
                    case FIXConstants.SIDE_Sell_Open:
                    case FIXConstants.SIDE_Sell:
                    case FIXConstants.SIDE_Sell_Closed:
                        positionTag = PositionTag.Short;
                        break;
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
                //return false;
            }
            return positionTag;
        }
        private bool CheckGroupClosingStatus(AllocationGroup Group)
        {

            if (Group.TaxLots == null)// if unallocated no need to check 
                return true;

            bool isNotPartiallyOrFullyClosed = true;
            try
            {
                if (_closingInfoCache == null) // no closed TaxLots in cache
                    return true;
                foreach (TaxLot TaxLot in Group.TaxLots)
                {
                    if (_closingInfoCache.ContainsKey(TaxLot.TaxLotID))
                    {
                        isNotPartiallyOrFullyClosed = false;
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
            return isNotPartiallyOrFullyClosed;

        }

        /// <summary>
        /// This method returns falls if unallocated taxlot on user1 is already closed on user2 
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-2238
        /// </summary>
        /// <param name="taxlotIDs">List of taxlotIDs which are unallocated</param>
        /// <returns></returns>
        public bool CheckTaxlotClosingStatus(List<string> taxlotIDs)
        {
            if (taxlotIDs == null)// if unallocated no need to check 
                return false;

            try
            {
                foreach (string taxlotID in taxlotIDs)
                {
                    if (_closingInfoCache.ContainsKey(taxlotID))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        // Check if corporate action is applied or Underlying in case of exercise or physical is closed

        public Dictionary<string, StatusInfo> ArePositionEligibletoUnwind(Dictionary<string, DateTime> taxlotID)
        {
            Logger.LoggerWrite("Checking for Position Eligible to Unwind", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            StringBuilder taxlotClosingIDWithClosingDate = new StringBuilder();

            Dictionary<string, StatusInfo> taxlotsWithClosedUnderlyingorCA = new Dictionary<string, StatusInfo>();
            try
            {
                lock (_closingInfoCacheLock)
                {
                    foreach (KeyValuePair<string, DateTime> kp in taxlotID)
                    {
                        taxlotClosingIDWithClosingDate.Append(kp.Key);
                        // comma separated
                        taxlotClosingIDWithClosingDate.Append(Seperators.SEPERATOR_8);
                        // _ separated
                        taxlotClosingIDWithClosingDate.Append(kp.Value);
                        taxlotClosingIDWithClosingDate.Append(Seperators.SEPERATOR_13);


                        bool isCAapplied = CheckCorporateActionStatus(kp.Key, kp.Value);
                        if (isCAapplied.Equals(true))
                        {
                            if (taxlotsWithClosedUnderlyingorCA.ContainsKey(kp.Key))
                            {
                                taxlotsWithClosedUnderlyingorCA[kp.Key].Status = PostTradeEnums.Status.CorporateAction;
                            }
                            else
                            {
                                StatusInfo info = new StatusInfo();
                                info.Status = PostTradeEnums.Status.CorporateAction;
                                taxlotsWithClosedUnderlyingorCA.Add(kp.Key, info);
                            }
                        }

                        // If cost adjustment has been applied on taxlotID, then closing should not be unwinded
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7515
                        bool isCostAdjustmentApplied = CheckCostAdjustmentStatus(kp.Key);
                        if (isCostAdjustmentApplied.Equals(true))
                        {
                            if (taxlotsWithClosedUnderlyingorCA.ContainsKey(kp.Key))
                            {
                                taxlotsWithClosedUnderlyingorCA[kp.Key].Status = PostTradeEnums.Status.CostBasisAdjustment;
                            }
                            else
                            {
                                StatusInfo info = new StatusInfo();
                                info.Status = PostTradeEnums.Status.CostBasisAdjustment;
                                taxlotsWithClosedUnderlyingorCA.Add(kp.Key, info);
                            }
                        }

                        if (_closingInfoCache.ContainsKey(kp.Key))
                        {
                            if (_closingInfoCache[kp.Key].DictExercisedUnderlying.Keys.Count > 0)
                            {
                                foreach (string underylingId in _closingInfoCache[kp.Key].DictExercisedUnderlying.Keys)
                                {
                                    if (_closingInfoCache.ContainsKey(underylingId))
                                    {
                                        if (taxlotsWithClosedUnderlyingorCA.ContainsKey(kp.Key))
                                        {
                                            taxlotsWithClosedUnderlyingorCA[kp.Key].ExercisedUnderlying.Add(underylingId, PostTradeEnums.Status.Closed);
                                        }
                                        else
                                        {
                                            StatusInfo infonew = new StatusInfo();
                                            PostTradeEnums.Status status = PostTradeEnums.Status.Closed;
                                            infonew.ExercisedUnderlying.Add(underylingId, status);
                                            taxlotsWithClosedUnderlyingorCA.Add(kp.Key, infonew);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }


            return taxlotsWithClosedUnderlyingorCA;


        }


        /// <summary>
        /// Check Group Corporate Action Status
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        private bool CheckGroupCorporateActionStatus(AllocationGroup Group)
        {
            //RefreshCAData();
            if (Group.TaxLots == null)// if unallocated no need to check 
                return true;
            lock (_TaxLotIdToCADateDictLock)
            {
                if (_TaxLotIdToCADateDict == null)
                    return true;
                bool isCANotApplied = true;
                try
                {
                    foreach (TaxLot TaxLot in Group.TaxLots)
                    {
                        if (_TaxLotIdToCADateDict.ContainsKey(TaxLot.TaxLotID))
                        {
                            isCANotApplied = false;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return isCANotApplied;
            }
        }

        /// <summary>
        /// check if group status is cost adjustment or not
        /// </summary>
        /// <param name="Group">The allocation group</param>
        /// <returns>true if group status is cost adjustment, false otherwise</returns>
        private bool CheckGroupCostAdjustmentStatus(AllocationGroup Group)
        {
            if (Group.TaxLots == null)// if unallocated no need to check 
                return true;
            lock (_TaxLotIdToCostAdjustDateDictLock)
            {
                if (_TaxLotIdToCostAdjustDateDict == null)
                    return true;
                bool isCostAdjustmentNotApplied = true;
                try
                {
                    foreach (TaxLot TaxLot in Group.TaxLots)
                    {
                        if (_TaxLotIdToCostAdjustDateDict.Contains(TaxLot.TaxLotID))
                        {
                            isCostAdjustmentNotApplied = false;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return isCostAdjustmentNotApplied;
            }
        }

        /// <summary>
        /// check that corporate action is not applied on another user
        /// </summary>
        /// <param name="Group"></param>
        /// <returns></returns>
        private bool CheckTaxlotCorporateActionStatus(List<string> taxlotIDs)
        {
            //RefreshCAData();
            if (taxlotIDs == null)// if unallocated no need to check 
                return false;
            lock (_TaxLotIdToCADateDictLock)
            {
                if (_TaxLotIdToCADateDict == null)
                    return false;
                bool isCANotApplied = false;
                try
                {
                    foreach (string taxlotID in taxlotIDs)
                    {
                        if (_TaxLotIdToCADateDict.ContainsKey(taxlotID))
                        {
                            isCANotApplied = true;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                return isCANotApplied;
            }
        }

        /// <summary>
        /// Check Closing Status
        /// </summary>
        /// <param name="TaxLotID"></param>
        /// <param name="caEffectiveDate"></param>
        /// <returns></returns>
        public bool CheckClosingStatus(string TaxLotID, DateTime caEffectiveDate)
        {
            Logger.LoggerWrite("Checking Closing Status TaxLotID: " + TaxLotID + ", caEffectiveDate: " + caEffectiveDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"TaxLotID", TaxLotID},
                {"caEffectiveDate", caEffectiveDate}
            });

            bool isClosed = false;
            lock (_closingInfoCacheLock)
            {
                if (_closingInfoCache != null)
                {
                    if (_closingInfoCache.ContainsKey(TaxLotID) && _closingInfoCache[TaxLotID].AUECMaxModifiedDate >= caEffectiveDate.Date)
                    {
                        isClosed = true;
                    }
                }

                else
                {
                    ///Don't have closing info hence 
                    isClosed = true;
                }
            }
            Logger.LoggerWrite("CheckClosingStatus, Closing Status for TaxLotID: " + TaxLotID + " is " + isClosed, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            return isClosed;
        }

        /// <summary>
        /// Check Corporate Action Status
        /// </summary>
        /// <param name="taxLotID"></param>
        /// <param name="dateToCheck"></param>
        /// <returns></returns>
        public bool CheckCorporateActionStatus(string taxLotID, DateTime dateToCheck)
        {


            //TO DO: Instead of making call to DB everytime , we can update the cache at rumtime whenever a CA or closing is applied or unapplied          
            //RefreshCAData();
            bool isCAApplied = false;
            lock (_TaxLotIdToCADateDictLock)
            {
                if (_TaxLotIdToCADateDict.ContainsKey(taxLotID))
                {
                    if (_TaxLotIdToCADateDict[taxLotID].Item1.Date > dateToCheck.Date)
                    {
                        isCAApplied = true;
                    }
                }

                return isCAApplied;
            }
        }
        public bool CheckIsExercised(AllocationGroup group)
        {


            if (group.TaxLots == null)// if unallocated no need to check 
                return true;

            bool isNotExercise = true;
            lock (_closingInfoCacheLock)
            {
                foreach (TaxLot TaxLot in group.TaxLots)
                {
                    if (_closingInfoCache.ContainsKey(TaxLot.TaxLotID))
                    {
                        if (_closingInfoCache[TaxLot.TaxLotID].DictExercisedUnderlying.Keys.Count > 0 || _closingInfoCache[TaxLot.TaxLotID].IsManualyExerciseAssign != null)
                        {
                            isNotExercise = false;
                            return isNotExercise;
                        }
                    }
                }
            }
            return isNotExercise;
        }
        /// <summary>
        /// check that taxlot is not exercised at another user
        /// </summary>
        /// <param name="taxlotIDs"></param>
        /// <returns></returns>
        public bool CheckIsTaxlotExercised(List<string> taxlotIDs)
        {

            if (taxlotIDs == null)// if unallocated no need to check 
                return false;

            bool isNotExercise = false;
            lock (_closingInfoCacheLock)
            {
                foreach (string taxlotID in taxlotIDs)
                {
                    if (_closingInfoCache.ContainsKey(taxlotID))
                    {
                        if (_closingInfoCache[taxlotID].DictExercisedUnderlying.Keys.Count > 0)
                        {
                            isNotExercise = true;
                            return isNotExercise;
                        }
                    }
                }
            }
            return isNotExercise;
        }


        public bool CheckExercisedOrPhysicalGenerated(AllocationGroup group)
        {

            if (group.TaxLots == null)// if unallocated no need to check 
                return true;

            bool isNotExercise = true;
            bool? _isManualyExceriseAssign = null;
            try
            {
                lock (_closingInfoCacheLock)
                {
                    List<TaxlotClosingInfo> list = new List<TaxlotClosingInfo>(_closingInfoCache.Values);

                    foreach (TaxLot taxlot in group.TaxLots)
                    {

                        if (list.Exists(delegate (TaxlotClosingInfo info)
                         {
                             if (info.DictExercisedUnderlying.ContainsKey(taxlot.TaxLotID))
                             {
                                 _isManualyExceriseAssign = info.IsManualyExerciseAssign;
                                 return true;
                             }
                             else
                             {
                                 return false;
                             }
                         }))
                        {
                            if (_isManualyExceriseAssign != null)
                                taxlot.IsManualyExerciseAssign = _isManualyExceriseAssign;
                            isNotExercise = false;
                            return isNotExercise;

                        }
                        if (group.GroupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually))
                        {
                            isNotExercise = false;
                            return isNotExercise;
                        }
                    }
                    if (group.TaxLots != null && group.TaxLots.Count == 0 && group.GroupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually))
                        group.GroupStatus = Prana.BusinessObjects.PostTradeEnums.Status.None;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isNotExercise;
        }

        //Reference not found
        /// <summary>
        /// check that taxlot is not exercised for another user
        /// </summary>
        /// <param name="taxlotIDs"></param>
        /// <returns></returns>
        public bool CheckTaxlotExercisedOrPhysicalGenerated(List<string> taxlotIDs)
        {
            if (taxlotIDs == null)// if unallocated no need to check 
                return false;

            bool isNotExercise = false;
            try
            {
                lock (_closingInfoCacheLock)
                {
                    List<TaxlotClosingInfo> list = new List<TaxlotClosingInfo>(_closingInfoCache.Values);

                    foreach (string taxlotID in taxlotIDs)
                    {

                        if (list.Exists(delegate (TaxlotClosingInfo info)
                         {
                             if (info.DictExercisedUnderlying.ContainsKey(taxlotID))
                             {
                                 return false;
                             }
                             else
                             {
                                 return true;
                             }
                         }))
                        {

                            isNotExercise = true;
                            return isNotExercise;

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return isNotExercise;
        }

        //public TaxLot AddTaxLot(TaxLot TaxLot)
        //{
        //    TaxLot newTaxLot = TaxLot.Clone();

        //    newTaxLot.TaxLotID = AllocationTaxLotIDGenerator.GenerateGroupTaxLotID() + newTaxLot.AccountValue.TaxLotID.ToString();
        //    newTaxLot.OpenQty = 0;
        //    newTaxLot.ParentRowPk = TaxLot.TaxLotPk;
        //    return newTaxLot;
        //}
        //public void UndoSplit(string TaxLotTaxLotID)
        //{
        //    Int64 parentRowPk = 0;
        //    foreach (TaxLot TaxLot in new List<TaxLot>(_openTaxLots))
        //    {
        //        if (TaxLot.TaxLotID == TaxLotTaxLotID)
        //        {
        //            parentRowPk = TaxLot.ParentRowPk;
        //            _openTaxLots.Remove(TaxLot);
        //        }
        //    }
        //    CloseTradesDataManager.UndoSplitTaxLot(TaxLotTaxLotID);
        //    TaxLotsList TaxLotList = CloseTradesDataManager.GetPosition(parentRowPk);
        //    if (TaxLotList.Count > 0)
        //    {
        //        if (_openTaxLots.Contains(TaxLotList[0]))
        //        {
        //            _openTaxLots.Remove(TaxLotList[0]);
        //            _openTaxLots.Add(TaxLotList[0]);
        //        }
        //        else
        //        {
        //            _openTaxLots.Add(TaxLotList[0]);
        //        }
        //    }
        //}


        /// <summary>
        /// Gets the order side tag value for physical settlement new.
        /// </summary>
        /// <param name="asset">The asset.</param>
        /// <param name="sidetagValue">The sidetag value.</param>
        /// <param name="putOrCall">The put or call.</param>
        /// <param name="settleDate">The settle date.</param>
        /// <param name="underlyingSymbol">The underlying symbol.</param>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="isCurrentDayClosing">if set to <c>true</c> [is current day closing].</param>
        /// <param name="underlyingQty">The underlying qty.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, PhysicalSettlementDto>> GetOrderSideTagValueForPhysicalSettlementGroup(List<TaxLot> taxlots, bool isCurrentDayClosing)
        {
            Dictionary<string, Dictionary<string, PhysicalSettlementDto>> dictTaxlotSideTagValueQty = new Dictionary<string, Dictionary<string, PhysicalSettlementDto>>();
            if (taxlots != null && taxlots.Count > 0)
            {
                TaxLot taxlot = taxlots[0];
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(taxlot.AssetCategoryValue);
                Dictionary<PositionTag, double> dictPositionTagValueQty = null;

                dictPositionTagValueQty = GetSymbolAccountOpenQtyByPositionTag(DateTime.Now, taxlot.UnderlyingSymbol, taxlot.Level1ID.ToString());
                double underlyingPosShort = 0;
                double underlyingPosLong = 0;
                if (dictPositionTagValueQty != null && dictPositionTagValueQty.ContainsKey(PositionTag.Short))
                {
                    underlyingPosShort = dictPositionTagValueQty[PositionTag.Short] > 0 ? dictPositionTagValueQty[PositionTag.Short] : 0;
                }
                if (dictPositionTagValueQty != null && dictPositionTagValueQty.ContainsKey(PositionTag.Long))
                {
                    underlyingPosLong = dictPositionTagValueQty[PositionTag.Long] > 0 ? dictPositionTagValueQty[PositionTag.Long] : 0;
                }

                if (taxlots.Count > 1)
                {
                    taxlots = ExerciseSideManager.OrdertaxlotsBasedOnAlgo(ref taxlots, _preferences.ClosingMethodology);
                }

                foreach (TaxLot tax in taxlots)
                {
                    int putOrCall = tax.PutOrCall;
                    string sidetagValue = tax.OrderSideTagValue;
                    double underlyingQty = tax.SettledQty * tax.ContractMultiplier;
                    switch (baseAssetCategory)
                    {
                        case AssetCategory.Option:
                            switch (putOrCall)
                            {
                                case FIXConstants.Underlying_Call:
                                    switch (sidetagValue)
                                    {
                                        case FIXConstants.SIDE_Buy:
                                        case FIXConstants.SIDE_Buy_Open:
                                        case FIXConstants.SIDE_Buy_Closed:
                                            if (underlyingPosShort > 0)
                                            {
                                                if (underlyingPosShort >= underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleDto = new PhysicalSettlementDto();
                                                    settleDto.Quantity = underlyingQty;
                                                    settleDto.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy_Closed, settleDto);
                                                    underlyingPosShort -= underlyingQty;
                                                }
                                                else
                                                {
                                                    PhysicalSettlementDto settleDtoBuyClosed = new PhysicalSettlementDto();
                                                    settleDtoBuyClosed.Quantity = underlyingPosShort;
                                                    settleDtoBuyClosed.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy_Closed, settleDtoBuyClosed);

                                                    PhysicalSettlementDto settleDtoBuy = new PhysicalSettlementDto();
                                                    settleDtoBuy.Quantity = underlyingQty - underlyingPosShort;
                                                    settleDtoBuy.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy, settleDtoBuy);
                                                    underlyingPosShort = 0;
                                                }
                                            }
                                            else
                                            {
                                                PhysicalSettlementDto settleDto = new PhysicalSettlementDto();
                                                settleDto.Quantity = underlyingQty;
                                                settleDto.TransactionType = TradingTransactionType.Exercise.ToString();
                                                AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy, settleDto);
                                            }
                                            break;
                                        case FIXConstants.SIDE_Sell:
                                        case FIXConstants.SIDE_Sell_Open:
                                        case FIXConstants.SIDE_SellShort:
                                        case FIXConstants.SIDE_Sell_Closed:

                                            if (underlyingPosLong > 0)
                                            {
                                                if (underlyingPosLong >= underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleDtoSell = new PhysicalSettlementDto();
                                                    settleDtoSell.Quantity = underlyingQty;
                                                    settleDtoSell.TransactionType = TradingTransactionType.Assignment.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleDtoSell);
                                                    underlyingPosLong -= underlyingQty;
                                                }
                                                else
                                                {
                                                    PhysicalSettlementDto settleDtoSell = new PhysicalSettlementDto();
                                                    settleDtoSell.Quantity = underlyingPosLong;
                                                    settleDtoSell.TransactionType = TradingTransactionType.Assignment.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleDtoSell);

                                                    PhysicalSettlementDto settleDtoSellShort = new PhysicalSettlementDto();
                                                    settleDtoSellShort.Quantity = underlyingQty - underlyingPosLong;
                                                    settleDtoSellShort.TransactionType = TradingTransactionType.Assignment.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleDtoSellShort);
                                                    underlyingPosLong = 0;

                                                }
                                            }
                                            else
                                            {
                                                PhysicalSettlementDto settleDtoSellShort = new PhysicalSettlementDto();
                                                settleDtoSellShort.Quantity = underlyingQty;
                                                settleDtoSellShort.TransactionType = TradingTransactionType.Assignment.ToString();
                                                AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleDtoSellShort);
                                            }

                                            break;
                                    }
                                    break;
                                case FIXConstants.Underlying_Put:
                                    switch (sidetagValue)
                                    {
                                        case FIXConstants.SIDE_Buy:
                                        case FIXConstants.SIDE_Buy_Open:
                                        case FIXConstants.SIDE_Buy_Closed:

                                            if (underlyingPosLong > 0)
                                            {
                                                if (underlyingPosLong >= underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                                    settleSell.Quantity = underlyingQty;
                                                    settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);
                                                    underlyingPosLong -= underlyingQty;
                                                }
                                                else if (underlyingPosLong < underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                                    settleSell.Quantity = underlyingPosLong;
                                                    settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);

                                                    PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                                    settleSellShort.Quantity = underlyingQty - underlyingPosLong;
                                                    settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                                    underlyingPosLong = 0;
                                                }
                                            }
                                            else
                                            {
                                                PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                                settleSellShort.Quantity = underlyingQty;
                                                settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                                AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                            }

                                            break;
                                        case FIXConstants.SIDE_Sell:
                                        case FIXConstants.SIDE_Sell_Open:
                                        case FIXConstants.SIDE_SellShort:
                                        case FIXConstants.SIDE_Sell_Closed:


                                            if (underlyingPosShort > 0)
                                            {
                                                if (underlyingPosShort >= underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleDto = new PhysicalSettlementDto();
                                                    settleDto.Quantity = underlyingQty;
                                                    settleDto.TransactionType = TradingTransactionType.Assignment.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy_Closed, settleDto);
                                                    underlyingPosShort -= underlyingQty;
                                                }
                                                else if (underlyingPosShort < underlyingQty)
                                                {
                                                    PhysicalSettlementDto settleDtoBuyClosed = new PhysicalSettlementDto();
                                                    settleDtoBuyClosed.Quantity = underlyingPosShort;
                                                    settleDtoBuyClosed.TransactionType = TradingTransactionType.Assignment.ToString();
                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy_Closed, settleDtoBuyClosed);

                                                    PhysicalSettlementDto settleDtoBuy = new PhysicalSettlementDto();
                                                    settleDtoBuy.Quantity = underlyingQty - underlyingPosShort;
                                                    settleDtoBuy.TransactionType = TradingTransactionType.Assignment.ToString();

                                                    AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy, settleDtoBuy);
                                                    underlyingPosShort = 0;
                                                }
                                            }
                                            else
                                            {
                                                PhysicalSettlementDto settleDto = new PhysicalSettlementDto();
                                                settleDto.Quantity = underlyingQty;
                                                settleDto.TransactionType = TradingTransactionType.Assignment.ToString();
                                                AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Buy, settleDto);
                                            }

                                            break;
                                    }
                                    break;
                            }
                            break;
                        case AssetCategory.Future:
                            switch (sidetagValue)
                            {
                                case FIXConstants.SIDE_Buy:
                                case FIXConstants.SIDE_Buy_Open:
                                case FIXConstants.SIDE_Buy_Closed:
                                    if (underlyingPosLong > 0)
                                    {
                                        if (underlyingPosLong >= underlyingQty)
                                        {
                                            PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                            settleSell.Quantity = underlyingQty;
                                            settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);
                                            underlyingPosLong -= underlyingQty;
                                        }
                                        else if (underlyingPosLong < underlyingQty)
                                        {
                                            PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                            settleSell.Quantity = underlyingPosLong;
                                            settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);

                                            PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                            settleSellShort.Quantity = underlyingQty - underlyingPosLong;
                                            settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                            underlyingPosLong = 0;
                                        }
                                    }
                                    else
                                    {
                                        PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                        settleSellShort.Quantity = underlyingQty;
                                        settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                        AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                    }
                                    break;
                                case FIXConstants.SIDE_Sell:
                                case FIXConstants.SIDE_Sell_Open:
                                case FIXConstants.SIDE_SellShort:
                                case FIXConstants.SIDE_Sell_Closed:

                                    if (underlyingPosLong > 0)
                                    {
                                        if (underlyingPosLong >= underlyingQty)
                                        {
                                            PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                            settleSell.Quantity = underlyingQty;
                                            settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);
                                            underlyingPosLong -= underlyingQty;
                                        }
                                        else if (underlyingPosLong < underlyingQty)
                                        {
                                            PhysicalSettlementDto settleSell = new PhysicalSettlementDto();
                                            settleSell.Quantity = underlyingPosLong;
                                            settleSell.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_Sell, settleSell);

                                            PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                            settleSellShort.Quantity = underlyingQty - underlyingPosLong;
                                            settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                            AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                            underlyingPosLong = 0;
                                        }
                                    }
                                    else
                                    {
                                        PhysicalSettlementDto settleSellShort = new PhysicalSettlementDto();
                                        settleSellShort.Quantity = underlyingQty;
                                        settleSellShort.TransactionType = TradingTransactionType.Exercise.ToString();
                                        AddSettlementForTaxlotAndSide(dictTaxlotSideTagValueQty, tax.TaxLotID, FIXConstants.SIDE_SellShort, settleSellShort);
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            return dictTaxlotSideTagValueQty;
        }

        /// <summary>
        /// Adds the settlement for taxlot and side.
        /// </summary>
        /// <param name="dictTaxlotSideTagValueQty">The dictionary taxlot side tag value qty.</param>
        /// <param name="taxlotId">The taxlot identifier.</param>
        /// <param name="sideTagValue">The side tag value.</param>
        /// <param name="settleDto">The settle dto.</param>
        private void AddSettlementForTaxlotAndSide(Dictionary<string, Dictionary<string, PhysicalSettlementDto>> dictTaxlotSideTagValueQty, string taxlotId, string sideTagValue, PhysicalSettlementDto settleDto)
        {
            if (!dictTaxlotSideTagValueQty.ContainsKey(taxlotId))
                dictTaxlotSideTagValueQty.Add(taxlotId, new Dictionary<string, PhysicalSettlementDto>());

            dictTaxlotSideTagValueQty[taxlotId].Add(sideTagValue, settleDto);
        }


        public string GetOrderSideTagValueForPhysicalSettlement(AssetCategory asset, string sidetagValue, double underlyingPosQty, int putOrCall, DateTime settleDate, string underlyingSymbol, string accountID, bool isCurrentDayClosing, ref string transactionType)
        {
            Logger.LoggerWrite(ApplicationConstants.CONST_CLOSING_SERVICE, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, string.Empty, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"asset", asset},
                {"sidetagValue", sidetagValue},
                {"underlyingPosQty", underlyingPosQty},
                {"putOrCall", putOrCall},
                {"settleDate", settleDate},
                {"underlyingSymbol", underlyingSymbol},
                {"accountID", accountID},
                {"isCurrentDayClosing", isCurrentDayClosing},
                {"transactionType", transactionType}
            });
            string sideTagValue = string.Empty;

            try
            {
                AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(asset);
                if (!isCurrentDayClosing)
                {
                    underlyingPosQty = GetSymbolAccountOpenQty(settleDate, underlyingSymbol, accountID);
                }
                switch (baseAssetCategory)
                {

                    case AssetCategory.Option:
                        switch (putOrCall)
                        {
                            case FIXConstants.Underlying_Call:
                                switch (sidetagValue)
                                {
                                    case FIXConstants.SIDE_Buy:
                                    case FIXConstants.SIDE_Buy_Open:
                                    case FIXConstants.SIDE_Buy_Closed:
                                        if (underlyingPosQty >= 0)
                                        {
                                            sideTagValue = FIXConstants.SIDE_Buy;
                                            transactionType = TradingTransactionType.Exercise.ToString();
                                        }
                                        else
                                        {
                                            sideTagValue = FIXConstants.SIDE_Buy_Closed;
                                            transactionType = TradingTransactionType.Exercise.ToString();
                                        }

                                        break;
                                    case FIXConstants.SIDE_Sell:
                                    case FIXConstants.SIDE_Sell_Open:
                                    case FIXConstants.SIDE_SellShort:
                                    case FIXConstants.SIDE_Sell_Closed:
                                        // sideTagValue = FIXConstants.SIDE_Sell;
                                        if (underlyingPosQty > 0)
                                        {
                                            sideTagValue = FIXConstants.SIDE_Sell;
                                            transactionType = TradingTransactionType.Assignment.ToString();
                                        }
                                        else
                                        {
                                            sideTagValue = FIXConstants.SIDE_SellShort;
                                            transactionType = TradingTransactionType.Assignment.ToString();
                                        }

                                        break;
                                }
                                break;
                            case FIXConstants.Underlying_Put:
                                switch (sidetagValue)
                                {
                                    case FIXConstants.SIDE_Buy:
                                    case FIXConstants.SIDE_Buy_Open:
                                    case FIXConstants.SIDE_Buy_Closed:
                                        //sideTagValue = FIXConstants.SIDE_SellShort;
                                        if (underlyingPosQty > 0)
                                        {
                                            sideTagValue = FIXConstants.SIDE_Sell;
                                            transactionType = TradingTransactionType.Exercise.ToString();
                                        }
                                        else
                                        {
                                            sideTagValue = FIXConstants.SIDE_SellShort;
                                            transactionType = TradingTransactionType.Exercise.ToString();
                                        }
                                        break;
                                    case FIXConstants.SIDE_Sell:
                                    case FIXConstants.SIDE_Sell_Open:
                                    case FIXConstants.SIDE_SellShort:
                                    case FIXConstants.SIDE_Sell_Closed:

                                        //sideTagValue = FIXConstants.SIDE_Buy;
                                        if (underlyingPosQty >= 0)
                                        {
                                            sideTagValue = FIXConstants.SIDE_Buy;
                                            transactionType = TradingTransactionType.Assignment.ToString();
                                        }
                                        else
                                        {
                                            sideTagValue = FIXConstants.SIDE_Buy_Closed;
                                            transactionType = TradingTransactionType.Assignment.ToString();
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
                    case AssetCategory.Future:
                        switch (sidetagValue)
                        {
                            case FIXConstants.SIDE_Buy:
                            case FIXConstants.SIDE_Buy_Open:
                            case FIXConstants.SIDE_Buy_Closed:

                                sideTagValue = FIXConstants.SIDE_Buy;


                                break;
                            case FIXConstants.SIDE_Sell:
                            case FIXConstants.SIDE_Sell_Open:
                            case FIXConstants.SIDE_SellShort:
                            case FIXConstants.SIDE_Sell_Closed:

                                sideTagValue = FIXConstants.SIDE_Sell;

                                break;
                        }
                        break;
                    default:
                        sideTagValue = FIXConstants.SIDE_Buy;

                        break;

                }

            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            return sideTagValue;
        }

        #region commentedCreatePositionForPhysicalSettlement
        //public OTCPosition CreatePositionForPhysicalSettlement(OTCPosition otcPos, AssetCategory asset, string sidetagValue, int putOrCall)
        //{
        //    try
        //    {
        //        AssetCategory baseAssetCategory = Mapper.GetBaseAssetCategory(asset);
        //        switch (baseAssetCategory)
        //        {
        //            case AssetCategory.Option:
        //                switch (putOrCall)
        //                {
        //                    case FIXConstants.Underlying_Call:
        //                        switch (sidetagValue)
        //                        {
        //                            case FIXConstants.SIDE_Buy:
        //                            case FIXConstants.SIDE_Buy_Open:
        //                            case FIXConstants.SIDE_Buy_Closed:
        //                                otcPos.SideTagValue = FIXConstants.SIDE_Buy;

        //                                break;
        //                            case FIXConstants.SIDE_Sell:
        //                            case FIXConstants.SIDE_Sell_Open:
        //                            case FIXConstants.SIDE_SellShort:
        //                            case FIXConstants.SIDE_Sell_Closed:
        //                                otcPos.SideTagValue = FIXConstants.SIDE_Sell;
        //                                break;
        //                        }
        //                        break;
        //                    case FIXConstants.Underlying_Put:
        //                        switch (sidetagValue)
        //                        {
        //                            case FIXConstants.SIDE_Buy:
        //                            case FIXConstants.SIDE_Buy_Open:
        //                            case FIXConstants.SIDE_Buy_Closed:
        //                                otcPos.SideTagValue = FIXConstants.SIDE_SellShort;
        //                                break;
        //                            case FIXConstants.SIDE_Sell:
        //                            case FIXConstants.SIDE_Sell_Open:
        //                            case FIXConstants.SIDE_SellShort:
        //                            case FIXConstants.SIDE_Sell_Closed:

        //                                otcPos.SideTagValue = FIXConstants.SIDE_Buy;
        //                                break;
        //                        }
        //                        break;
        //                }
        //                break;
        //            default:
        //                otcPos.SideTagValue = FIXConstants.SIDE_Buy;

        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return otcPos;
        //}
        //public bool IsUnsavedData()
        //{
        //    if (_unSaveAllocatedTadeList.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}



        #endregion

        /// <summary>
        /// Automatic Closing
        /// </summary>
        /// <param name="openingTaxLots"></param>
        /// <param name="closingTaxLots"></param>
        /// <param name="algorithm"></param>
        /// <param name="IsShortWithBuyAndBuyToCover"></param>
        /// <param name="IsSellWithBuyToClose"></param>
        /// <param name="isLatestStateRequired"></param>
        /// <param name="isVirtualClosing"></param>
        /// <param name="secondarySortCriteria"></param>
        /// <param name="isVirtualClosingPopulate"></param>
        /// <param name="isMatchStrategy"></param>
        /// <param name="isOverrideWithUserClosing"></param>
        /// <param name="closingField"></param>
        /// <returns></returns>
        private ClosingData AutomaticClosing(List<TaxLot> openingTaxLots, List<TaxLot> closingTaxLots, PostTradeEnums.CloseTradeAlogrithm algorithm, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose, bool isLatestStateRequired, bool isVirtualClosing, PostTradeEnums.SecondarySortCriteria secondarySortCriteria, bool isVirtualClosingPopulate, bool isMatchStrategy, bool isOverrideWithUserClosing, PostTradeEnums.ClosingField closingField, bool IsCopyOpeningTradeAttributes)
        {
            Logger.LoggerWrite(ApplicationConstants.CONST_CLOSING_SERVICE, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, string.Empty, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"openingTaxLots", openingTaxLots},
                {"closingTaxLots", closingTaxLots},
                {"algorithm", algorithm},
                {"IsShortWithBuyAndBuyToCover", IsShortWithBuyAndBuyToCover},
                {"IsSellWithBuyToClose", IsSellWithBuyToClose},
                {"isLatestStateRequired", isLatestStateRequired},
                {"isVirtualClosing", isVirtualClosing},
                {"secondarySortCriteria", secondarySortCriteria},
                {"isVirtualClosingPopulate", isVirtualClosingPopulate},
                {"isMatchStrategy", isMatchStrategy},
                {"isOverrideWithUserClosing", isOverrideWithUserClosing},
                {"closingField", closingField}
            });

            ClosingData UpdatedClosingData = new ClosingData();
            string validationErrorMsg = string.Empty;

            try
            {
                if (ValiadateClosingCriteria(algorithm, secondarySortCriteria, out validationErrorMsg))
                {

                    GenericBucket genericBucket = new GenericBucket();
                    //buckets are generated based on account and strategy match, strategy match can be ignored if isMatchStrategy is false (closing from Close Order UI)
                    genericBucket.CreateGenericBucket(openingTaxLots, closingTaxLots, isMatchStrategy);

                    List<BuySellBucket> bucketOpenTaxLots = genericBucket.GetOpenTaxlotsforClosing();

                    AlgoBase algo = AlgoFactory.GetAlgo(algorithm);
                    if (algo != null)
                    {

                        foreach (BuySellBucket bucket in bucketOpenTaxLots)
                        {
                            List<TaxLot> BuyTaxLotsAndPositions = bucket.Buytaxlotsandpositions;

                            List<TaxLot> SellTaxLotsAndPositions = bucket.Selltaxlotsandpositions;

                            List<DateTime> sellTaxLotDates = GetSortedDatesBasedOnClosingDate(SellTaxLotsAndPositions);

                            List<DateTime> buyTaxLotDates = GetSortedDatesBasedOnClosingDate(BuyTaxLotsAndPositions);

                            PranaSortedList actionDatesList = new PranaSortedList();

                            // action dates are those dates on which any trading activity has happened  
                            IList<DateTime> actionDates = actionDatesList.getAllActionDates(buyTaxLotDates, sellTaxLotDates);

                            foreach (DateTime actiondate in actionDates)
                            {
                                // since closing is done for each date we have to get the eligible open TaxLots having date less than equal to the action date by iterating for each action date 

                                List<TaxLot> sellTaxLotForClosing = algo.GetTaxLotsByDate(actiondate.Date, SellTaxLotsAndPositions);
                                List<TaxLot> buyTaxLotForClosing = algo.GetTaxLotsByDate(actiondate.Date, BuyTaxLotsAndPositions);
                                //get taxlots long and short
                                //taxlots with side buy, sell, BuyToOpen, SellToClose are long taxlots
                                //taxlots with remaining sides are short taxlots
                                List<List<TaxLot>> sellTaxLotsLongAndShort = algo.GetSellClosingTaxLots(sellTaxLotForClosing);
                                List<List<TaxLot>> buyTaxLotsLongAndShort = algo.GetBuyClosingTaxLots(buyTaxLotForClosing);
                                for (int i = 0; i < sellTaxLotsLongAndShort.Count; i++)
                                {
                                    //at index 0 there will be long taxlots
                                    //at index 1 there will be short taxlots
                                    ClosingData ClosedData = null;
                                    List<TaxLot> longAndShortBuyTaxlots = new List<TaxLot>();
                                    List<TaxLot> longAndShortSellTaxlots = new List<TaxLot>();

                                    if (i > 0)
                                    {
                                        //append remainig long taxlots to short taxlots bucket so that closing with force side(Sell with BuyToClose and SellShort with Buy) can be done
                                        buyTaxLotsLongAndShort[i].AddRange(buyTaxLotsLongAndShort[i - 1]);
                                        sellTaxLotsLongAndShort[i].AddRange(sellTaxLotsLongAndShort[i - 1]);
                                        //assign sell taxlots to buy bucket and and buy taxlots to sell bucket in case of short taxlots
                                        longAndShortBuyTaxlots = sellTaxLotsLongAndShort[i];
                                        longAndShortSellTaxlots = buyTaxLotsLongAndShort[i];
                                    }
                                    else
                                    {
                                        longAndShortBuyTaxlots = buyTaxLotsLongAndShort[i];
                                        longAndShortSellTaxlots = sellTaxLotsLongAndShort[i];
                                    }

                                    if (!(longAndShortSellTaxlots.Count == 0 || longAndShortBuyTaxlots.Count == 0))// if either of the buy or sell TaxLots bucket is zero then we simply move to the next action date 
                                    {
                                        // here we are sorting the eligible TaxLots according to the algo 
                                        if (i > 0)
                                        {
                                            algo.Sort(ref longAndShortSellTaxlots, ref longAndShortBuyTaxlots, secondarySortCriteria, closingField);
                                        }
                                        else
                                        {
                                            algo.Sort(ref longAndShortBuyTaxlots, ref longAndShortSellTaxlots, secondarySortCriteria, closingField);
                                        }
                                        //this would be executed only if same avg price option is selected
                                        //closing for same avg price taxlots
                                        if ((secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC) || secondarySortCriteria.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC)))
                                        {
                                            //first close trades which have same avg price
                                            //flag at the end represends that closing should be done for same avg price
                                            ClosedData = CloseTaxLotsAutomaticallyNew(longAndShortBuyTaxlots, longAndShortSellTaxlots, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, algorithm, true, isVirtualClosingPopulate, isMatchStrategy, isOverrideWithUserClosing, secondarySortCriteria, closingField, IsCopyOpeningTradeAttributes);
                                            RemoveZeroOpenQtyFromList(BuyTaxLotsAndPositions);
                                            RemoveZeroOpenQtyFromList(SellTaxLotsAndPositions);
                                            RemoveZeroOpenQtyFromList(buyTaxLotsLongAndShort[i]);
                                            RemoveZeroOpenQtyFromList(sellTaxLotsLongAndShort[i]);
                                            UpdateClosingData(UpdatedClosingData, ClosedData, true);
                                        }
                                        //closing for the remaining taxlots
                                        ClosedData = CloseTaxLotsAutomaticallyNew(longAndShortBuyTaxlots, longAndShortSellTaxlots, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, algorithm, false, isVirtualClosingPopulate, isMatchStrategy, isOverrideWithUserClosing, secondarySortCriteria, closingField, IsCopyOpeningTradeAttributes);
                                        RemoveZeroOpenQtyFromList(BuyTaxLotsAndPositions);
                                        RemoveZeroOpenQtyFromList(SellTaxLotsAndPositions);

                                        //for algo sorting it may be possible that modified list is returned and reference of list may change
                                        //so remove zero quantity taxlots externally
                                        RemoveZeroOpenQtyFromList(buyTaxLotsLongAndShort[i]);
                                        RemoveZeroOpenQtyFromList(sellTaxLotsLongAndShort[i]);
                                        //here closing data with same avg price and remaining closing data will be appended
                                        UpdateClosingData(UpdatedClosingData, ClosedData, true);

                                    }
                                    if (isVirtualClosing)
                                    {
                                        //for each actionDate no closing transaction should remain open after closing otherwise it will counted as a confilcting transaction...
                                        List<TaxLot> listConflictingtaxlots = GetConflictedTaxlots(longAndShortBuyTaxlots, longAndShortSellTaxlots);
                                        UpdatedClosingData.ConflictedTaxlots.AddRange(listConflictingtaxlots);

                                        RemoveZeroOpenQtyFromList(openingTaxLots);
                                        RemoveZeroOpenQtyFromList(closingTaxLots);
                                        RemoveZeroOpenQtyFromList(buyTaxLotsLongAndShort[i]);
                                        RemoveZeroOpenQtyFromList(sellTaxLotsLongAndShort[i]);
                                        ////removing conflicted taxlot reference from the main list
                                        foreach (TaxLot taxlot in listConflictingtaxlots)
                                        {
                                            if (BuyTaxLotsAndPositions.Contains(taxlot))
                                            {
                                                BuyTaxLotsAndPositions.Remove(taxlot);
                                                openingTaxLots.Remove(taxlot);
                                            }
                                            if (SellTaxLotsAndPositions.Contains(taxlot))
                                            {
                                                SellTaxLotsAndPositions.Remove(taxlot);
                                                closingTaxLots.Remove(taxlot);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        UpdatedClosingData.IsError = true;
                        if (algorithm.Equals(PostTradeEnums.CloseTradeAlogrithm.FIFO) || algorithm.Equals(PostTradeEnums.CloseTradeAlogrithm.LIFO))
                            UpdatedClosingData.ErrorMsg = UpdatedClosingData.ErrorMsg.Append("Invalid Seconadry Sort Criteria " + secondarySortCriteria.ToString() + " for algorithm " + algorithm.ToString() + " \nVaild Criterias are AvgPxASC, AvgPxDESC, None and OrderSequence\n");
                        else
                            UpdatedClosingData.ErrorMsg = UpdatedClosingData.ErrorMsg.Append("Invalid Seconadry Sort Criteria " + secondarySortCriteria.ToString() + " for algorithm " + algorithm.ToString() + " \nVaild Criterias are None, OrderSequenceASC, OrderSequenceDESC\n");
                    }
                }
                else
                {
                    UpdatedClosingData.IsError = true;
                    UpdatedClosingData.ErrorMsg = new StringBuilder(validationErrorMsg);
                }

            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            return UpdatedClosingData;
        }


        /// <summary>
        /// Get Conflicted Taxlots
        /// </summary>
        /// <param name="BuyTaxlots"></param>
        /// <param name="SellTaxlots"></param>
        /// <returns></returns>
        private List<TaxLot> GetConflictedTaxlots(List<TaxLot> BuyTaxlots, List<TaxLot> SellTaxlots)
        {
            List<TaxLot> buyTaxlotsCopy = new List<TaxLot>();
            buyTaxlotsCopy.AddRange(BuyTaxlots);

            List<TaxLot> sellTaxlotsCopy = new List<TaxLot>();
            sellTaxlotsCopy.AddRange(SellTaxlots);

            List<TaxLot> ConflictedTaxlots = new List<TaxLot>();
            try
            {
                foreach (TaxLot taxlot in buyTaxlotsCopy)
                {
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                    {
                        ConflictedTaxlots.Add(taxlot);
                        BuyTaxlots.Remove(taxlot);
                    }
                }
                foreach (TaxLot taxlot in sellTaxlotsCopy)
                {
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        ConflictedTaxlots.Add(taxlot);
                        SellTaxlots.Remove(taxlot);
                    }
                }
                //checking for any conflicting pair of opening sides
                if (BuyTaxlots.Count > 0 && SellTaxlots.Count > 0)
                {
                    ConflictedTaxlots.AddRange(BuyTaxlots);
                    ConflictedTaxlots.AddRange(SellTaxlots);
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
            return ConflictedTaxlots;
        }


        //private List<DateTime> GetSortedDates(List<TaxLot> SellTaxLots)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        /// <summary>
        /// Get TaxLots By Date
        /// </summary>
        /// <param name="date"></param>
        /// <param name="listOftaxlots"></param>
        /// <returns></returns>
        private List<TaxLot> GetTaxLotsByDate(DateTime date, List<TaxLot> listOftaxlots)
        {
            List<TaxLot> taxlotList = new List<TaxLot>();

            try
            {
                foreach (TaxLot sortedItem in listOftaxlots)
                {
                    if (sortedItem.ClosingDate.Date <= date)
                    {
                        taxlotList.Add(sortedItem);
                    }
                    else
                    {
                        break;
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
            return taxlotList;
        }

        /// <summary>
        /// Run Virtual Closing
        /// </summary>
        /// <param name="Taxlots"></param>
        /// <param name="toDate"></param>
        /// <param name="algorithm"></param>
        /// <param name="IsShortWithBuyAndBuyToCover"></param>
        /// <param name="IsSellWithBuyToClose"></param>
        /// <returns></returns>
        public List<TaxLot> RunVirtualClosing(List<TaxLot> Taxlots, DateTime toDate, PostTradeEnums.CloseTradeAlogrithm algorithm, bool IsShortWithBuyAndBuyToCover, bool IsSellWithBuyToClose)
        {
            try
            {
                Logger.LoggerWrite("Running Virtual Closing for taxlots: " + string.Join("; ", Taxlots.Select(x => x.TaxLotID).ToArray()), LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

                Dictionary<string, TaxlotClosingInfo> closingInfoCacheCopy = _closingInfoCache;

                _closingInfoCache = new Dictionary<string, TaxlotClosingInfo>();

                List<TaxLot> OpenTaxlots = GetOpenPositionsFromDB(toDate, false, string.Empty, string.Empty, string.Empty, string.Empty);

                Dictionary<string, TaxLot> dictionaryTaxlots = new Dictionary<string, TaxLot>();

                foreach (TaxLot taxlot in OpenTaxlots)
                {
                    dictionaryTaxlots.Add(taxlot.TaxLotID, taxlot);
                }
                foreach (TaxLot taxlot in Taxlots)
                {
                    if (!(dictionaryTaxlots.ContainsKey(taxlot.TaxLotID)))
                        OpenTaxlots.Add(taxlot);
                }
                List<TaxLot> BuyTaxlots = new List<TaxLot>();

                List<TaxLot> SellTaxlots = new List<TaxLot>();

                foreach (TaxLot taxlot in OpenTaxlots)
                {
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                    {
                        BuyTaxlots.Add(taxlot);
                    }
                    else
                    {
                        SellTaxlots.Add(taxlot);
                    }
                }

                ClosingData closingData = AutomaticClosing(BuyTaxlots, SellTaxlots, algorithm, IsShortWithBuyAndBuyToCover, IsSellWithBuyToClose, true, true, PostTradeEnums.SecondarySortCriteria.None, false, true, false, PostTradeEnums.ClosingField.Default, false);

                _closingInfoCache = closingInfoCacheCopy;

                foreach (TaxLot taxlot in BuyTaxlots)
                {
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Closed) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Cover))
                    {
                        if (!closingData.ConflictedTaxlots.Contains(taxlot))
                        {
                            closingData.ConflictedTaxlots.Add(taxlot);
                        }
                    }
                }
                foreach (TaxLot taxlot in SellTaxlots)
                {
                    if (taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell) || taxlot.OrderSideTagValue.Equals(FIXConstants.SIDE_Sell_Closed))
                    {
                        if (!closingData.ConflictedTaxlots.Contains(taxlot))
                        {
                            closingData.ConflictedTaxlots.Add(taxlot);
                        }
                    }
                }
                return closingData.ConflictedTaxlots;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        /// <summary>
        /// Get Preferences FromDB
        /// </summary>
        private void GetPreferencesFromDB()
        {
            try
            {
                _preferences = ClosingPrefDataManager.GetPreferences();

            }
            catch (Exception)
            {
                throw;

            }
        }

        /// <summary>
        /// Get Preferences
        /// </summary>
        /// <returns></returns>
        public ClosingPreferences GetPreferences()
        {
            try
            {
                Logger.LoggerWrite("Getting closing preferences.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                return _preferences;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

        }



        #region ACA Cleanup
        //public List<string> GetACAAccounts()
        //{
        //    List<string> eligibleAccountlist = new List<string>();
        //    try
        //    {
        //        foreach (KeyValuePair<string, DataRow> kp in _preferences.ClosingMethodology.DictAccountingMethods)
        //        {
        //            if (Convert.ToBoolean(kp.Value["IsACA"].ToString()))
        //            {
        //                string[] splitData = kp.Key.Split('_');
        //                if (splitData.Length > 0)
        //                {
        //                    eligibleAccountlist.Add(splitData[0]);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //    return eligibleAccountlist;
        //} 
        #endregion

        /// <summary>
        /// Save closing data directly to the database and update in cache
        /// </summary>
        /// <param name="closedData"></param>
        /// <returns></returns>
        public int SaveCloseTradesDataToDB(ClosingData closedData)
        {
            try
            {
                List<Position> netPositions = closedData.ClosedPositions;
                List<TaxLot> UnsavedTaxlots = closedData.UnSavedTaxlots;

                int rowsAffected = 0;
                if (UnsavedTaxlots.Count > 0)
                {

                    rowsAffected = CloseTradesDataManager.SaveCloseTradesData(netPositions, UnsavedTaxlots);

                    if (rowsAffected > 0)
                    {
                        // update closing cache
                        UpdateClosingCache(netPositions, UnsavedTaxlots);

                        // publish data
                        PublishClosingData(closedData);
                    }
                }



                return rowsAffected;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
        }

        private void UpdateClosingCache(List<Position> netPositions, List<TaxLot> UnsavedTaxlots)
        {
            try
            {
                Dictionary<string, Dictionary<string, TaxLot>> dictTaxlots = new Dictionary<string, Dictionary<string, TaxLot>>();
                UnsavedTaxlots.ForEach(taxlot =>
                {
                    if (taxlot.TaxLotClosingId != null)
                    {
                        if (!dictTaxlots.ContainsKey(taxlot.TaxLotID))
                        {
                            Dictionary<string, TaxLot> taxlot1 = new Dictionary<string, TaxLot>();
                            taxlot1.Add(taxlot.TaxLotClosingId, taxlot);
                            dictTaxlots.Add(taxlot.TaxLotID, taxlot1);
                        }
                        else
                        {
                            dictTaxlots[taxlot.TaxLotID].Add(taxlot.TaxLotClosingId, taxlot);
                        }
                    }
                });

                foreach (Position pos in netPositions)
                {
                    if (!pos.IsClosingSaved)
                    {
                        //netPositionList.Add(pos);
                        pos.IsClosingSaved = true;
                    }

                    //prevent to add taxlots to closingInfoCache when taxlots are populated from CloseTradeInformation UI or from CorporateAction
                    if (!pos.ClosingMode.Equals(ClosingMode.CorporateAction))
                    {
                        //add to closing info cache
                        AddToClosingInfoCache(dictTaxlots[pos.ID][pos.TaxLotClosingId], dictTaxlots[pos.ClosingID][pos.TaxLotClosingId], (PostTradeEnums.CloseTradeAlogrithm)pos.ClosingAlgo);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Save Closed Trades Data
        /// </summary>
        /// <param name="closedData"></param>
        /// <returns></returns>
        public int SaveCloseTradesData(ClosingData closedData)
        {
            try
            {
                Logger.LoggerWrite("Saving Close Trades Data", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                 {"Current Method", MethodBase.GetCurrentMethod()},
                 {"closedData", closedData}
                });

                List<Position> netPositions = closedData.ClosedPositions;
                List<TaxLot> UnsavedTaxlots = closedData.UnSavedTaxlots;
                // to get the latest closing date
                //NetPositionList netPositionList = new NetPositionList();
                //List<Position> netPositionList = new List<Position>();
                int rowsAffected = 0;
                if (UnsavedTaxlots.Count > 0)
                {

                    rowsAffected = CloseTradesDataManager.SaveCloseTradesData(netPositions, UnsavedTaxlots);
                    if (rowsAffected != netPositions.Count)
                    {
                        closedData.UnSavedTaxlots = closedData.UnSavedTaxlots.GetRange(0, rowsAffected * 2);
                        closedData.ClosedPositions = closedData.ClosedPositions.GetRange(0, rowsAffected);
                        closedData.IsPartial = true;
                    }

                    if (rowsAffected > 0)
                    {
                        // update closing cache
                        UpdateClosingCache(netPositions, UnsavedTaxlots);

                        // publish data
                        PublishClosingData(closedData);
                    }
                }



                return rowsAffected;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

        }


        /// <summary>
        /// Fetch Updated Taxlots From DB
        /// </summary>
        /// <param name="TaxLotIDList"></param>
        /// <param name="groupIDs"></param>
        /// <returns></returns>
        public ClosingData FetchUpdatedTaxlotsFromDB(string TaxLotIDList, List<string> groupIDs)
        {

            ClosingData ClosedData = new ClosingData();
            try
            {
                List<TaxLot> openTaxLot = CloseTradesDataManager.GetPosition(TaxLotIDList);
                foreach (TaxLot TaxLot in openTaxLot)
                {
                    if (ClosedData.UpdatedTaxlots.ContainsKey(TaxLot.TaxLotID))
                    {
                        TaxLot Olditem = ClosedData.UpdatedTaxlots[TaxLot.TaxLotID];
                        ClosedData.Taxlots.Remove(Olditem);

                        ClosedData.UpdatedTaxlots.Remove(TaxLot.TaxLotID);
                        ClosedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
                        ClosedData.Taxlots.Add(TaxLot);
                    }
                    else
                    {
                        ClosedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
                        ClosedData.Taxlots.Add(TaxLot);
                    }
                }

                GetTaxLotsLatestClosingDatesFromDB(true);
                PublishClosingData(ClosedData);
                //if (openTaxLot.Count>0 && groupIDs.Count>0)
                //{
                //    RemoveFromExercisedTaxlotsDictionary(openTaxLot,groupIDs);
                //}
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            return ClosedData;
        }



        private void PublishClosingData(ClosingData ClosedData)
        {
            try
            {
                List<TaxLot> lsTaxlots = new List<TaxLot>();
                if (ClosedData.UnSavedTaxlots.Count > 0)
                {
                    lsTaxlots = ClosedData.UnSavedTaxlots;
                }
                else
                {
                    lsTaxlots = ClosedData.Taxlots;
                }
                if (lsTaxlots.Count > 0)
                {
                    foreach (TaxLot taxlot in lsTaxlots)
                    {
                        // if a taxlot is closed with corporate action, its closing mode should be none.
                        // because after publishing, when we close it from closing UI, closing module corrupts data.
                        // so closing mode has been updated

                        // SetTaxlotClosingStatus funcation is used to set AUECModifiedDate and closing status
                        // this is done on the basis of taxlotid which is in closing cache. 
                        // 1. For CA generated transaction, AUECModifiedDate is set from CA only, so no need to set here. 
                        // Example a trade MSFT Buy 100@10 on 10/10/2014, Now it is partially closed of 10 qty on 10/14/2014 and added in the closing cache.
                        // Next time assume on 11/06/2014 we apply Corporate action, CA generated transactions are not added in the closing cache,
                        // but closing cache already contains this taxlotID as it was closed previously, so updates its AUECLocalDate on which it was previously closed i.e. 10/14/2014
                        // It creates problem while publishing the data

                        if (taxlot.ClosingMode.Equals(ClosingMode.CorporateAction))
                        {
                            taxlot.ClosingMode = ClosingMode.None;
                        }
                        else
                        {
                            SetTaxlotClosingStatus(taxlot);
                        }
                    }

                    List<List<TaxLot>> closeddata = ChunkingManager.CreateChunks(lsTaxlots, 1000);
                    foreach (List<TaxLot> eventData in closeddata)
                    {
                        MessageData data = new MessageData();
                        data.EventData = eventData;
                        data.TopicName = Topics.Topic_Closing;
                        BufferMessage(dataBuffer, data);
                    }

                }
                if (ClosedData.ClosedPositions.Count > 0)
                {
                    List<List<Position>> closeddPosition = ChunkingManager.CreateChunks(ClosedData.ClosedPositions, 1000);
                    foreach (List<Position> eventData in closeddPosition)
                    {
                        MessageData data = new MessageData();
                        data.EventData = eventData;
                        data.TopicName = Topics.Topic_Closing_NetPositions;
                        BufferMessage(dataBuffer, data);
                    }
                }

                if (ClosedData.PositionsToUnwind.Length > 0)
                {
                    MessageData data = new MessageData();
                    List<string> posTounwind = new List<string>();
                    posTounwind.Add(ClosedData.PositionsToUnwind);
                    data.EventData = posTounwind;
                    data.TopicName = Topics.Topic_UnwindPositions;
                    BufferMessage(dataBuffer, data);
                }
                if ((_proxy != null) && (lsTaxlots.Count > 0 || ClosedData.ClosedPositions.Count > 0 || ClosedData.PositionsToUnwind.Length > 0))
                {
                    BufferMessage(dataBuffer, null);
                }

                if (_activityServices != null)
                    _activityServices.GenerateCashActivity(ClosedData.ClosedPositions, CashTransactionType.Closing);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Consume Buffer Message Async
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        async System.Threading.Tasks.Task<MessageData> ConsumeBufferMessageAsync(IReceivableSourceBlock<MessageData> source)
        {
            try
            {
                while (await source.OutputAvailableAsync())
                {
                    MessageData message;
                    while (source.TryReceive(out message))
                    {
                        if (message == null)
                        {
                            _proxy.InnerChannel.Publish(null, Topics.Topic_ClosingCompleted);
                            Logger.LoggerWrite("Publish Message null with topic Topics.Topic_ClosingCompleted.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                        }
                        else
                        {
                            _proxy.InnerChannel.Publish(message, message.TopicName);
                            Logger.LoggerWrite("Publish Message: " + message + ", with topic: " + message.TopicName, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            return null;
        }

        /// <summary>
        /// Buffer Message
        /// </summary>
        /// <param name="target"></param>
        /// <param name="message"></param>
        void BufferMessage(ITargetBlock<MessageData> target, MessageData message)
        {
            try
            {
                target.Post(message);
            }
            catch (Exception)
            {

                throw;

            }
        }

        /// <summary>
        /// UnWind Closing From Database
        /// </summary>
        /// <param name="TaxlotClosingID"></param>
        /// <param name="isBasedOnTemplates"></param>
        /// <returns></returns>
        public List<string> UnWindClosingFromDatabase(string TaxlotClosingID, bool isBasedOnTemplates)
        {
            Logger.LoggerWrite("UnWinding Closing From Database, TaxlotClosingID:" + TaxlotClosingID + ", isBasedOnTemplates:" + isBasedOnTemplates, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            List<string> listGroupIDs = new List<string>();

            try
            {
                listGroupIDs = CloseTradesDataManager.UnWindClosingForTaxlots(TaxlotClosingID.ToString(), isBasedOnTemplates);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            Logger.LoggerWrite("UnWinding Closing From Database completed, TaxlotClosingID:" + TaxlotClosingID, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            return listGroupIDs;

        }

        /// <summary>
        /// Unwind Closing Based On Templates
        /// </summary>
        /// <param name="closingTemplates"></param>
        /// <returns></returns>
        public ClosingData UnwindClosingBasedOnTemplates(List<ClosingTemplate> closingTemplates)
        {
            Logger.LoggerWrite("Unwinding Closing Based On Templates", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"closingTemplates", closingTemplates}
            });

            int i = 0;
            StringBuilder errorMessage = new StringBuilder();
            ClosingData closingData = new ClosingData();
            List<string> navLockFailedTemplates = new List<string>();
            try
            {
                ProgressInfo info = null;
                bool isProcessingNextTemplate = false;
                foreach (ClosingTemplate template in closingTemplates)
                {
                    i++;
                    isProcessingNextTemplate = true;
                    #region Filters

                    #region Publish Progress

                    info = new ProgressInfo();
                    info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                    if (template.UseCurrentDate)
                    {
                        info.ProgressStatus = "Fetching Closed Positions from" + DateTime.UtcNow.Date + " based on applied filters...";
                    }
                    else
                    {
                        info.ProgressStatus = "Fetching Closed Positions from" + template.FromDate + " based on applied filters...";
                    }
                    PublishProgress(info);

                    #endregion

                    Dictionary<int, List<int>> dictMasterFundSubAccountAssociation = CachedDataManager.GetInstance.GetMasterFundSubAccountAssociation();
                    string CommaSeparatedAccountIDs = template.GetCommaSeparatedAccounts(dictMasterFundSubAccountAssociation);
                    string CommaSeparatedAssetIDs = template.GetCommaSeparatedAssets();
                    string commaSeparatedSymbols = template.GetCommaSeparatedSymbols();

                    string CustomFilterCondition = SqlParser.GetDynamicConditionQuerry(template.DictCustomConditions);

                    #endregion

                    #region DATA FETCHING

                    List<Position> closedPositions = GetNetPositionsFromDB(template.FromDate, DateTime.UtcNow, template.UseCurrentDate, CommaSeparatedAccountIDs, CommaSeparatedAssetIDs, commaSeparatedSymbols, CustomFilterCondition, GetPreferences().DateType);

                    #endregion

                    #region UNWINDING CHECKS

                    #region Publish Progress

                    info = new ProgressInfo();
                    info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                    info.ProgressStatus = "Performing unwinding feasibility checks...";
                    PublishProgress(info);

                    #endregion
                    Dictionary<string, DateTime> dictionaryTaxlotIDs = new Dictionary<string, DateTime>();
                    foreach (Position position in closedPositions)
                    {
                        //add positionaltaxlotid
                        if (!dictionaryTaxlotIDs.ContainsKey(position.ID))
                        {
                            dictionaryTaxlotIDs.Add(position.ID, position.ClosingTradeDate);
                        }
                        //add closingtaxlotID...
                        if (!dictionaryTaxlotIDs.ContainsKey(position.ClosingID))
                        {
                            dictionaryTaxlotIDs.Add(position.ClosingID, position.ClosingTradeDate);
                        }
                    }

                    Dictionary<string, StatusInfo> positionStatusDict = ArePositionEligibletoUnwind(dictionaryTaxlotIDs);

                    StringBuilder commaSeparatedTaxlotClosingID = new StringBuilder();
                    List<string> taxlotidList = new List<string>();
                    StringBuilder taxlotId = new StringBuilder();
                    List<DateTime> closingDates = new List<DateTime>();

                    foreach (Position position in closedPositions)
                    {
                        if (!((positionStatusDict.ContainsKey(position.ID)) || (positionStatusDict.ContainsKey(position.ClosingID))))
                        {
                            commaSeparatedTaxlotClosingID.Append(position.TaxLotClosingId.ToString());
                            commaSeparatedTaxlotClosingID.Append(",");

                            if (!taxlotidList.Contains(position.ID.ToString()))
                            {
                                taxlotId.Append(position.ID.ToString());
                                taxlotId.Append(",");
                            }
                            if (!taxlotidList.Contains(position.ClosingID.ToString()))
                            {
                                taxlotId.Append(position.ClosingID.ToString());
                                taxlotId.Append(",");
                            }
                            closingDates.Add(position.ClosingTradeDate);
                        }
                        else
                        {
                            if (positionStatusDict.ContainsKey(position.ID) || positionStatusDict.ContainsKey(position.ClosingID))
                            {
                                if ((positionStatusDict.ContainsKey(position.ID) && positionStatusDict[position.ID].Status.Equals(PostTradeEnums.Status.CorporateAction)) || (positionStatusDict.ContainsKey(position.ClosingID) && positionStatusDict[position.ClosingID].Status.Equals(PostTradeEnums.Status.CorporateAction)))
                                {
                                    if (isProcessingNextTemplate)
                                    {
                                        isProcessingNextTemplate = false;
                                        errorMessage.Append(Environment.NewLine);
                                        errorMessage.Append("Errors in ");
                                        errorMessage.Append(template.TemplateName);
                                        errorMessage.Append("................");
                                        errorMessage.Append(Environment.NewLine);
                                    }
                                    errorMessage.Append(position.Symbol);
                                    errorMessage.Append(" (");
                                    errorMessage.Append("TaxlotID : ");
                                    errorMessage.Append(position.ID.ToString());
                                    errorMessage.Append(" )");
                                    errorMessage.Append(",");
                                    errorMessage.Append(position.ClosingID.ToString());
                                    errorMessage.Append(" has corporate action on future date,First undo Corporate action then unwind");
                                    errorMessage.Append(Environment.NewLine);
                                }
                            }
                            if (positionStatusDict.ContainsKey(position.ID))
                            {
                                foreach (KeyValuePair<string, PostTradeEnums.Status> kp in positionStatusDict[position.ID].ExercisedUnderlying)
                                {
                                    if (isProcessingNextTemplate)
                                    {
                                        isProcessingNextTemplate = false;
                                        errorMessage.Append(Environment.NewLine);
                                        errorMessage.Append("Errors in ");
                                        errorMessage.Append(template.TemplateName);
                                        errorMessage.Append("................");
                                        errorMessage.Append(Environment.NewLine);
                                    }
                                    errorMessage.Append("Exercised Underlying IDs : ");
                                    if (positionStatusDict[position.ID].ExercisedUnderlying.Keys.Count > 0)
                                    {
                                        foreach (string key in positionStatusDict[position.ID].ExercisedUnderlying.Keys)
                                        {
                                            string id = key;
                                            errorMessage.Append(id);
                                            errorMessage.Append(" , ");
                                        }
                                        errorMessage.Append("generated by TaxlotID : ");
                                        errorMessage.Append(position.ID.ToString());
                                        errorMessage.Append("(");
                                        errorMessage.Append(position.Symbol);
                                        errorMessage.Append(" )");
                                        errorMessage.Append("  is closed, First unwind the underlying to continue");
                                        errorMessage.Append(Environment.NewLine);
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region UNWINDING

                    if (string.IsNullOrEmpty(errorMessage.ToString()))
                    {
                        if (!string.IsNullOrEmpty(commaSeparatedTaxlotClosingID.ToString()))
                        {
                            #region Publish Progress

                            info = new ProgressInfo();
                            info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                            info.ProgressStatus = "Unwinding data...";
                            PublishProgress(info);

                            #endregion

                            bool isNavLockValidationFailed = false;
                            if (CachedDataManager.GetInstance.NAVLockDate.HasValue)
                            {
                                foreach (var closingDate in closingDates)
                                {
                                    if (!CachedDataManager.GetInstance.ValidateNAVLockDate(closingDate))
                                    {
                                        isNavLockValidationFailed = true;
                                        navLockFailedTemplates.Add(template.TemplateName);
                                        break;
                                    }
                                }
                            }

                            if (!isNavLockValidationFailed)
                            {
                                List<string> groupIDsToDelete = UnWindClosingFromDatabase(commaSeparatedTaxlotClosingID.ToString(), true);
                                UnwindPositions(commaSeparatedTaxlotClosingID.ToString());
                                FetchUpdatedTaxlotsFromDB(taxlotId.ToString(), groupIDsToDelete);
                            }

                            #region Publish Progress

                            info = new ProgressInfo();
                            info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                            info.ProgressStatus = isNavLockValidationFailed ? "NavLock Validation Failed" : "Data Unwinded Successfully";
                            if (i == closingTemplates.Count)
                            {
                                info.IsTaskCompleted = true;
                            }
                            PublishProgress(info);

                            #endregion
                        }
                        else
                        {
                            #region Publish Progress

                            info = new ProgressInfo();
                            info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                            info.ProgressStatus = "Nothing to Unwind...";
                            if (i == closingTemplates.Count)
                            {
                                info.IsTaskCompleted = true;
                            }
                            PublishProgress(info);

                            #endregion
                            //nothing to unwind...
                        }
                    }
                    else
                    {
                        #region Publish Progress

                        info = new ProgressInfo();
                        info.HeaderText = "Processing request for Unwinding Template " + template.TemplateName + "...";
                        info.ProgressStatus = "Some errors occured while unwinding data.For more details please view error log after the operation is complete.";
                        if (i == closingTemplates.Count)
                        {
                            info.IsTaskCompleted = true;
                        }
                        PublishProgress(info);

                        #endregion
                        //do logging here and send back error message to client...
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

            if (!string.IsNullOrEmpty(errorMessage.ToString()))
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ApplicationConstants.CONST_CLOSING_SERVICE + errorMessage.ToString(), LoggingConstants.CATEGORY_WARNING, 1, 1, System.Diagnostics.TraceEventType.Warning);

            if(navLockFailedTemplates.Count > 0)
            {
                closingData.IsNavLockFailed = true;
                closingData.NavLockFailedTemplates = string.Join(",", navLockFailedTemplates);
            }
            closingData.ErrorMsg = errorMessage;

            return closingData;
        }

        /// <summary>
        /// Save Preferences
        /// </summary>
        /// <param name="ClosingPreferences"></param>
        public void SavePreferences(ClosingPreferences ClosingPreferences)
        {
            Logger.LoggerWrite("Saving closing Preferences.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            try
            {
                ClosingPrefDataManager.SavePreferences(ClosingPreferences);
                UpdatePreferences(ClosingPreferences);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }

        }


        private void UpdatePreferences(ClosingPreferences ClosingPreferences)
        {
            try
            {
                _preferences.ClosingMethodology.AccountingMethodsTable = ClosingPreferences.ClosingMethodology.AccountingMethodsTable;
                _preferences.ClosingMethodology.GlobalClosingMethodology = ClosingPreferences.ClosingMethodology.GlobalClosingMethodology;
                _preferences.ClosingMethodology.ClosingAlgo = ClosingPreferences.ClosingMethodology.ClosingAlgo;
                _preferences.ClosingMethodology.SecondarySort = ClosingPreferences.ClosingMethodology.SecondarySort;
                _preferences.ClosingMethodology.IsShortWithBuyandBTC = ClosingPreferences.ClosingMethodology.IsShortWithBuyandBTC;
                _preferences.ClosingMethodology.IsSellWithBTC = ClosingPreferences.ClosingMethodology.IsSellWithBTC;
                _preferences.ClosingMethodology.OverrideGlobal = ClosingPreferences.ClosingMethodology.OverrideGlobal;
                _preferences.IsFetchDataAutomatically = ClosingPreferences.IsFetchDataAutomatically;
                _preferences.AutoOptExerciseValue = ClosingPreferences.AutoOptExerciseValue;

                _preferences.ClosingMethodology.LongTermTaxRate = ClosingPreferences.ClosingMethodology.LongTermTaxRate;
                _preferences.ClosingMethodology.ShortTermTaxRate = ClosingPreferences.ClosingMethodology.ShortTermTaxRate;

                _preferences.QtyRoundoffDigits = ClosingPreferences.QtyRoundoffDigits;
                _preferences.PriceRoundOffDigits = ClosingPreferences.PriceRoundOffDigits;
                _preferences.ClosingMethodology.IsAutoCloseStrategy = ClosingPreferences.ClosingMethodology.IsAutoCloseStrategy;
                _preferences.ClosingMethodology.ClosingField = ClosingPreferences.ClosingMethodology.ClosingField;
                _preferences.ClosingMethodology.SplitunderlyingBasedOnPosition = ClosingPreferences.ClosingMethodology.SplitunderlyingBasedOnPosition;
                _preferences.CopyOpeningTradeAttributes = ClosingPreferences.CopyOpeningTradeAttributes;
            }
            catch (Exception)
            {

                throw;

            }


        }

        /// <summary>
        /// Checks the group status.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public Dictionary<string, PostTradeEnums.Status> GetGroupStatus(List<AllocationGroup> groups)
        {
            Logger.LoggerWrite("Get Group Status", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            Dictionary<string, PostTradeEnums.Status> groupStatusDictionary = new Dictionary<string, PostTradeEnums.Status>();
            try
            {
                groups.ForEach(group =>
                        {
                            var groupStatus = CheckGroupStatus(group);
                            groupStatusDictionary.Add(group.GroupID, groupStatus);
                            Logger.LoggerWrite("Group Status for groupID : " + group.GroupID + " = " + groupStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                        });
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw new FaultException<PranaAppException>(new PranaAppException(ex), ex.Message);
            }
            return groupStatusDictionary;
        }

        public PostTradeEnums.Status CheckGroupStatus(AllocationGroup Group)
        {
            // Sandeep_05 DEC 2014: Sequence of checking group status changed. 
            // Now first of all corporate action has to be checked because if CA is applied then no field has to edit from Edit Trade UI.
            // assume a trade is partially closed and then CA is applied, in this case this function was returning status closed and user was able to change trade from Edit Trade UI.

            try
            {
                if (!(CheckGroupCorporateActionStatus(Group)))
                {
                    return PostTradeEnums.Status.CorporateAction;
                }
                else if (!(CheckIsExercised(Group)))
                {
                    return PostTradeEnums.Status.IsExercised;
                }
                else if (!(CheckExercisedOrPhysicalGenerated(Group)))
                {
                    if (Group.GroupStatus.Equals(PostTradeEnums.Status.ExerciseAssignManually) || (Group.TaxLots.Count > 0 && Group.TaxLots[0].IsManualyExerciseAssign != null && Group.TaxLots[0].IsManualyExerciseAssign == true))
                    {
                        return PostTradeEnums.Status.ExerciseAssignManually;
                    }
                    return PostTradeEnums.Status.Exercise;
                }
                else if (!(CheckGroupClosingStatus(Group)))
                {
                    // if group is closed then if its closed through cost adjustment, then group status is set to CostBasisAdjustment
                    // http://jira.nirvanasolutions.com:8080/browse/PRANA-7030
                    if (!(CheckGroupCostAdjustmentStatus(Group)))
                        return PostTradeEnums.Status.CostBasisAdjustment;
                    else
                        return PostTradeEnums.Status.Closed;
                }
                //Modified by Disha, fetched status of group for cost adjusted taxlots: http://jira.nirvanasolutions.com:8080/browse/PRANA-6754
                else if (!(CheckGroupCostAdjustmentStatus(Group)))
                {
                    return PostTradeEnums.Status.CostBasisAdjustment;
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }

            return PostTradeEnums.Status.None;
        }

        //Reference not found for this method
        /// <summary>
        /// This method is implemened to check taxlot status in multi user scenario
        /// </summary>
        /// <param name="taxlotIDs"></param>
        /// <returns></returns>
        public bool CheckTaxlotStatus(List<string> taxlotIDs)
        {

            if ((CheckTaxlotClosingStatus(taxlotIDs)))
            {
                return true;
            }
            else if ((CheckIsTaxlotExercised(taxlotIDs)))
            {
                return true;
            }
            //else if ((CheckTaxlotExercisedOrPhysicalGenerated(taxlotIDs)))
            //{
            //    return true;
            //}
            else if ((CheckTaxlotCorporateActionStatus(taxlotIDs)))
            {
                return true;
            }


            return false;

        }

        /// <summary>
        /// Get Methodology by AccountID
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="assetID"></param>
        /// <returns></returns>
        private int GetMethodologybyAccountID(int accountID, int assetID)
        {
            try
            {
                string assetName = CachedDataManager.GetInstance.GetAssetText(assetID);
                return _preferences.ClosingMethodology.GetClosingAlgoForAccountAndAsset(accountID.ToString(), assetName);

            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return int.MinValue;
        }


        private PostTradeEnums.SecondarySortCriteria GetSecondarySortCriteria(int accountID, int assetID)
        {
            try
            {
                string assetName = CachedDataManager.GetInstance.GetAssetText(assetID);
                return _preferences.ClosingMethodology.GetSecondarySortingCriteriaForAccountAndAsset(accountID.ToString(), assetName);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return PostTradeEnums.SecondarySortCriteria.None;
        }

        private PostTradeEnums.ClosingField GetClosingField(int accountID, int assetID)
        {
            try
            {
                string assetName = CachedDataManager.GetInstance.GetAssetText(assetID);
                return _preferences.ClosingMethodology.GetClosingFieldForAccountAndAsset(accountID.ToString(), assetName);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return PostTradeEnums.ClosingField.Default;
        }

        #region commentedSaveACADataintoDB
        //public bool SaveACADataIntoDB()
        //{
        //    bool isDataSaved = false;
        //    try
        //    {
        //        DateTime toDate = DateTime.UtcNow;
        //        string ToAllAUECDatesStringTransactions = TimeZoneHelper.GetAllAUECDateInUseAUECStr(toDate);
        //        List<TaxLot> Transactions = _positionServices.GetOpenPositionsOrTransactions(ToAllAUECDatesStringTransactions, true);

        //        ACAManager acaManager = new ACAManager();
        //        List<TaxLot> eligibleTaxlots= acaManager.GetEligibleTaxlotsForACA(Transactions, _preferences.DictAccountingMethods);

        //        if (eligibleTaxlots.Count > 0)
        //        {

        //            int rowsAffected = ACAManager.CalculateAndSaveACAData(taxlotsnew);

        //            if (rowsAffected > 0)
        //            {
        //                isDataSaved = true;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            PranaAppException theFault = new PranaAppException(ex);

        //            throw new FaultException<PranaAppException>(theFault, ex.Message);
        //        }
        //    }
        //    return isDataSaved;
        //}
        #endregion

        /// <summary>
        /// Update Exercised Taxlots Dictionary
        /// </summary>
        /// <param name="parentTaxlotID"></param>
        /// <param name="UnderlyingID"></param>
        /// <param name="parentTaxlottoCloseId"></param>
        public void UpdateExercisedTaxlotsDictionary(string parentTaxlotID, string UnderlyingID, string parentTaxlottoCloseId)
        {
            Logger.LoggerWrite(ApplicationConstants.CONST_CLOSING_SERVICE, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, string.Empty, new Dictionary<string, object>(){
                {"Current Method", MethodBase.GetCurrentMethod()},
                {"parentTaxlotID", parentTaxlotID},
                {"UnderlyingID", UnderlyingID},
                {"parentTaxlottoCloseId", parentTaxlottoCloseId}
            });

            try
            {
                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache != null)
                    {
                        if (_closingInfoCache.ContainsKey(parentTaxlotID))
                        {
                            if (!string.IsNullOrEmpty(UnderlyingID) && !string.IsNullOrEmpty(parentTaxlottoCloseId))
                            {
                                _closingInfoCache[parentTaxlotID].DictExercisedUnderlying.Add(UnderlyingID, parentTaxlottoCloseId);
                            }
                        }
                        else
                        {
                            TaxlotClosingInfo info = new TaxlotClosingInfo();
                            if (!string.IsNullOrEmpty(UnderlyingID) && !string.IsNullOrEmpty(parentTaxlottoCloseId))
                            {
                                info.DictExercisedUnderlying.Add(UnderlyingID, parentTaxlottoCloseId);
                            }
                            _closingInfoCache.Add(parentTaxlotID, info);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }
        #region commented UpdateExercisedTaxlotsDictionary
        //public void UpdateExercisedTaxlotsDictionary(string parentTaxlotID, string groupID, string ClosingGroupID)
        //{
        //    try
        //    {
        //        if (_exercisedUnderlyingTaxLots != null)
        //        {
        //            if (!_exercisedUnderlyingTaxLots.ContainsKey(parentTaxlotID))
        //            {
        //                Dictionary<string, string> underlyingGroupIDs = new Dictionary<string, string>();
        //                underlyingGroupIDs.Add(groupID, ClosingGroupID);
        //                _exercisedUnderlyingTaxLots.Add(parentTaxlotID, underlyingGroupIDs);
        //            }
        //            else
        //            {
        //                _exercisedUnderlyingTaxLots[parentTaxlotID].Add(groupID, ClosingGroupID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {

        //            throw;
        //        }
        //    }
        //}
        #endregion

        #region commented RemoveFromExercisedTaxlotsDictionary()
        //private void RemoveFromExercisedTaxlotsDictionary(List<TaxLot> taxlots, List<string> groupIDs)
        //{
        //    try
        //    {
        //        if (_exercisedUnderlyingTaxLots != null)
        //        {
        //            foreach (TaxLot taxlot in taxlots)
        //            {
        //                if (_exercisedUnderlyingTaxLots.ContainsKey(taxlot.TaxLotID))
        //                {
        //                    Dictionary<string, string> dictunderlyingIDs = _exercisedUnderlyingTaxLots[taxlot.TaxLotID];


        //                    List<string> groupIDsNew = new List<string>();
        //                    groupIDsNew.AddRange(groupIDs);
        //                    foreach (string groupID in groupIDsNew)
        //                    {
        //                        if (dictunderlyingIDs.ContainsKey(groupID))
        //                        {
        //                            dictunderlyingIDs.Remove(groupID);
        //                            groupIDs.Remove(groupID);
        //                            _exercisedUnderlyingTaxLots[taxlot.TaxLotID].Remove(groupID);

        //                        }
        //                        if (dictunderlyingIDs.Count == 0)
        //                        {
        //                            _exercisedUnderlyingTaxLots.Remove(taxlot.TaxLotID);
        //                            break;
        //                        }
        //                   }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {

        //            throw;
        //        }
        //    }

        //}
        #endregion

        /// <summary>
        /// Get IDs To UnwindFromDB
        /// </summary>
        /// <param name="dictSymbols"></param>
        /// <returns></returns>
        public List<string> GetIDsToUnwindFromDB(Dictionary<string, DateTime> dictSymbols)
        {
            List<string> listIDs = new List<string>();
            try
            {
                listIDs = CloseTradesDataManager.GetIDsToUnwind(dictSymbols);
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
            return listIDs;
        }


        /// <summary>
        /// GetExercisedUnderlyingGroupIDs
        /// </summary>
        /// <param name="ParentGroup"></param>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> GetExercisedUnderlyingGroupIDs(AllocationGroup ParentGroup)
        {

            Dictionary<string, Dictionary<string, string>> dictTaxlotWiseGroupID = new Dictionary<string, Dictionary<string, string>>();
            try
            {


                //foreach (TaxLot taxlot in ParentGroup.TaxLots)
                //{
                //    if (_exercisedUnderlyingTaxLots.ContainsKey(taxlot.TaxLotID))
                //    {
                //        dictTaxlotWiseGroupID.Add(taxlot.TaxLotID, _exercisedUnderlyingTaxLots[taxlot.TaxLotID]);
                //    }
                //}
                //return dictTaxlotWiseGroupID; 
                lock (_closingInfoCacheLock)
                {
                    foreach (TaxLot taxlot in ParentGroup.TaxLots)
                    {
                        if (_closingInfoCache.ContainsKey(taxlot.TaxLotID))

                            dictTaxlotWiseGroupID.Add(taxlot.TaxLotID, _closingInfoCache[taxlot.TaxLotID].DictExercisedUnderlying);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            return dictTaxlotWiseGroupID;
        }



        #region commented by omshiv, ACA cleanup
        //public int CalculateAndSaveACAData()
        //{
        //    int rowsaffected = 0;
        //    try
        //    {
        //        List<string> eligibleAccountlist = new List<string>();
        //        foreach (KeyValuePair<string, DataRow> kp in _preferences.ClosingMethodology.DictAccountingMethods)
        //        {
        //            if (Convert.ToBoolean(kp.Value["IsACA"].ToString()))
        //            {
        //                string[] splitData = kp.Key.Split('_');
        //                if (splitData.Length > 0)
        //                {
        //                    eligibleAccountlist.Add(splitData[0]);
        //                }
        //            }
        //        }
        //        if (eligibleAccountlist.Count > 0)
        //        {
        //            DateTime fromDate = DateTime.UtcNow.Date;
        //            if (_ACALatestCalculationDate != DateTime.MinValue)
        //            {
        //                fromDate = _ACALatestCalculationDate;
        //            }
        //            rowsaffected = ACAManager.CalculateAndSaveACA(fromDate, eligibleAccountlist);
        //            _ACALatestCalculationDate = fromDate.Date;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {

        //            throw;
        //        }
        //    }
        //    return rowsaffected;
        //} 

        //public void CalculateAndSaveACADataForSymbol(Dictionary<string, DateTime> dictSymbols)
        //{
        //    try
        //    {
        //        if (dictSymbols.Count > 0)
        //        {
        //            _isACACalculationRunning = true;
        //            List<string> eligibleAccountlist = new List<string>();
        //            foreach (KeyValuePair<string, DataRow> kp in _preferences.ClosingMethodology.DictAccountingMethods)
        //            {
        //                if (Convert.ToBoolean(kp.Value["IsACA"].ToString()))
        //                {
        //                    string[] splitData = kp.Key.Split('_');
        //                    if (splitData.Length > 0)
        //                    {
        //                        eligibleAccountlist.Add(splitData[0]);
        //                    }
        //                }
        //            }
        //            if (eligibleAccountlist.Count > 0)
        //            {
        //                if (!_ACALatestCalculationDate.Equals(DateTime.MinValue))
        //                {
        //                    ACAManager.CalculateACADataForSymbol(dictSymbols, eligibleAccountlist, _ACALatestCalculationDate);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = EnterpriseLibraryManager.HandleException(ex, EnterpriseLibraryConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {

        //            throw;
        //        }
        //    }
        //    finally
        //    {
        //        _isACACalculationRunning = false;
        //    }
        //} 
        #endregion

        /// <summary>
        /// To set closing status for the taxlots being published on PM so that if the taxlots are already closed, they donot pop up on PM.
        /// </summary>
        /// <param name="taxlot"></param>
        public void SetTaxlotClosingStatus(TaxLot taxlot)
        {

            try
            {

                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache.ContainsKey(taxlot.TaxLotID))
                    {
                        taxlot.ClosingStatus = _closingInfoCache[taxlot.TaxLotID].ClosingStatus;
                        //taxlot.AUECModifiedDate = _closingInfoCache[taxlot.TaxLotID].AUECMaxModifiedDate;
                        taxlot.ClosingAlgo = _closingInfoCache[taxlot.TaxLotID].ClosingAlgo;
                    }
                }
                if (taxlot.Level1Name == string.Empty && taxlot.Level1ID != int.MinValue)
                {
                    taxlot.Level1Name = CachedDataManager.GetInstance.GetAccountText(taxlot.Level1ID);
                }
                List<string> taxlotIds = new List<string> { taxlot.TaxLotID };

                if (CheckTaxlotCorporateActionStatus(taxlotIds))
                {
                    //if (_TaxLotIdToCADateDict[taxlot.TaxLotID].Item2.Equals(1))
                    //{
                    //    taxlot.ClosingStatus = ClosingStatus.Closed;
                    //}
                    //else
                    //{
                    //    taxlot.ClosingStatus = ClosingStatus.Open;
                    //}
                    taxlot.ClosingMode = ClosingMode.CorporateAction;
                    taxlot.ClosingDate = _TaxLotIdToCADateDict[taxlot.TaxLotID].Item1.Date;
                }

                Logger.LoggerWrite("Getting Taxlot Closing status for TaxLotID: " + taxlot.TaxLotID + " is " + taxlot.ClosingStatus, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Get closing status and max modified date
        /// </summary>
        /// <param name="taxlot"></param>
        public void GetTaxlotClosingStatusWithMaxModifiedDate(TaxLot taxlot, out ClosingStatus ClosingStatus, out DateTime AUECModifiedDate)
        {
            ClosingStatus = ClosingStatus.Open;
            AUECModifiedDate = DateTimeConstants.MinValue;
            try
            {

                lock (_closingInfoCacheLock)
                {
                    if (_closingInfoCache.ContainsKey(taxlot.TaxLotID))
                    {
                        ClosingStatus = _closingInfoCache[taxlot.TaxLotID].ClosingStatus;
                        AUECModifiedDate = _closingInfoCache[taxlot.TaxLotID].AUECMaxModifiedDate;
                    }
                    Logger.LoggerWrite("Get Taxlot Closing Status With Max Modified Date for TaxLotID: " + taxlot.TaxLotID + " is " + ClosingStatus + ", AUECModifiedDate:" + AUECModifiedDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {

                    throw;
                }
            }
        }
        //Narendra Kumar Jangir, May 28 2013

        #region validate closing for close trade information
        /// <summary>
        /// Validate available quantity of opening taxlot against the available quantity of closing taxlot
        /// </summary>
        /// <param name="buyTaxLots"></param>
        /// <param name="sellTaxLots"></param>
        /// <returns></returns>
        private ClosingData ValiadateClosing(List<TaxLot> buyTaxLots, List<TaxLot> sellTaxLots)
        {
            ClosingData ClosingData = new ClosingData();
            try
            {
                Dictionary<int, double> dictBuyAccountIdWithQty = new Dictionary<int, double>();
                foreach (TaxLot taxLot in buyTaxLots)
                {
                    if (!dictBuyAccountIdWithQty.ContainsKey(taxLot.Level1ID))
                    {
                        dictBuyAccountIdWithQty.Add(taxLot.Level1ID, taxLot.TaxLotQtyToClose);
                    }
                    else
                    {
                        dictBuyAccountIdWithQty[taxLot.Level1ID] += taxLot.TaxLotQtyToClose;
                    }
                }
                Dictionary<int, double> dictSellAccountIdWithQty = new Dictionary<int, double>(); List<TaxLot> listTaxlots = new List<TaxLot>();
                foreach (TaxLot taxLot in sellTaxLots)
                {
                    if (!dictSellAccountIdWithQty.ContainsKey(taxLot.Level1ID))
                    {
                        dictSellAccountIdWithQty.Add(taxLot.Level1ID, taxLot.TaxLotQty);
                    }
                    else
                    {
                        dictSellAccountIdWithQty[taxLot.Level1ID] += taxLot.TaxLotQty;
                    }
                }

                foreach (KeyValuePair<int, double> kvp in dictSellAccountIdWithQty)
                {
                    if (dictBuyAccountIdWithQty.ContainsKey(kvp.Key) && dictBuyAccountIdWithQty[kvp.Key] > dictSellAccountIdWithQty[kvp.Key])
                    {
                        ClosingData.IsError = true;

                        ClosingData.ErrorMsg.Append("Closing taxlots have less Quantity for Account: " + CachedDataManager.GetInstance.GetAccountText(kvp.Key));
                        ClosingData.ErrorMsg.Append(Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {

                    throw;
                }
            }
            return ClosingData;
        }

        #endregion
        #endregion

        #region Valiadate Closing Criteria
        /// <summary>
        /// Narendra Kumar Jangir, June 27 2013
        /// 
        /// for all algos corresponding valid secondary sort criteria are following
        /// 
        /// FIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// LIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// MIFO: AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
        /// HIFO: None
        /// HIHO: None
        /// LOWCOST: None
        /// ETM: None
        /// BTAX: None
        /// TAXADV: None
        /// ACA: None
        /// </summary>
        /// <param name="algorithm">Selected algorithm for the closing</param>
        ///  <param name="secondarySort">Secondary sort criteria for the closing</param>
        private bool ValiadateClosingCriteria(PostTradeEnums.CloseTradeAlogrithm algorithm, PostTradeEnums.SecondarySortCriteria secondarySort, out string ErrorMessage)
        {
            string errorMessage = string.Empty;
            bool isValidate = true;
            try
            {
                switch (algorithm)
                {
                    case PostTradeEnums.CloseTradeAlogrithm.NONE:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                            isValidate = false;

                        break;
                    //for FIFO and LOFO, valid secondary sort criteria are AvgPxASC,AvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
                    case PostTradeEnums.CloseTradeAlogrithm.FIFO:
                    case PostTradeEnums.CloseTradeAlogrithm.LIFO:

                        if (secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxASC) || secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.SamePxAvgPxDESC))
                        {
                            errorMessage = "For selected algo SamePxAvgPxASC /SamePxAvgPxDESC is not a valid secondary sort criteria.";
                            isValidate = false;
                        }
                        break;

                    //for MFIFO valid secondary sort criteria are AvgPxASC,AvgPxDESC,SamePxAvgPxASC,SamePxAvgPxDESC,OrderSequenceASC,OrderSequenceDESC and None
                    case PostTradeEnums.CloseTradeAlogrithm.MFIFO:

                        break;
                    //for HIFO valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.HIFO:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "for HIFO valid secondary sort criteria is None";
                            isValidate = false;
                        }

                        break;
                    //for PRESET algo secondary sort criteria would be validated during saving secondary sort for an algo.
                    case PostTradeEnums.CloseTradeAlogrithm.PRESET:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                            isValidate = false;
                        break;
                    //for ETM and HCST valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.ETM:
                    case PostTradeEnums.CloseTradeAlogrithm.HCST:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "For selected closing algorithm, valid secondary sort criteria is None.";
                            isValidate = false;
                        }
                        // isValidate = false;
                        break;

                    //for LOWCOST valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.LOWCOST:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "for LOWCOST valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for ACA valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.ACA:
                        if (!(secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None) || secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.OrderSequenceASC)))
                        {
                            errorMessage = "for ACA valid secondary sort criteria are OrderSequenceASC and None";
                            isValidate = false;
                        }
                        break;
                    //for HIHO valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.HIHO:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "for HIHO valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for BTAX valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.BTAX:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "for BTAX valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    //for TAXADV valid secondary sort criteria is None
                    case PostTradeEnums.CloseTradeAlogrithm.TAXADV:
                        if (!secondarySort.Equals(PostTradeEnums.SecondarySortCriteria.None))
                        {
                            errorMessage = "for TAXADV valid secondary sort criteria is None";
                            isValidate = false;
                        }
                        break;
                    case PostTradeEnums.CloseTradeAlogrithm.MANUAL:
                        //Narendra Kumar Jangir, 2013 Oct 02
                        //http://jira.nirvanasolutions.com:8080/browse/SS-80
                        //whenever user changes closed quantity from close order UI than closing algorithm will be MANUAL.
                        break;
                    default:
                        break;
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
            ErrorMessage = errorMessage;
            return isValidate;
        }
        #endregion


        public int SaveClosedData(ClosingData closedData)
        {
            Logger.LoggerWrite("Saving Closed Data.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE, new Dictionary<string, object>(){
                {"Current Method",MethodBase.GetCurrentMethod()},
                {"Closed Data",closedData}
            });

            int rowsAffected = 0;
            try
            {
                rowsAffected = SaveCloseTradesDataToDB(closedData);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
            return rowsAffected;
        }


        /// <summary>
        /// Get PNL for a account and symbol
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public void GetPNLForSymbol(string accountName, string symbol, DateTime startDate, DateTime endDate, out Double accountRealizedPNL, out Double accountUnRealizedPNL, out Double SymbolRealizedPNL, out Double SymbolUnRealizedPNL)
        {
            accountRealizedPNL = 0;
            accountUnRealizedPNL = 0;
            SymbolRealizedPNL = 0;
            SymbolUnRealizedPNL = 0;
            try
            {
                CloseTradesDataManager.GetPNLForSymbol(accountName, symbol, startDate, endDate, out accountRealizedPNL, out accountUnRealizedPNL, out SymbolRealizedPNL, out SymbolUnRealizedPNL);
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }

        public void UpdatePreferencesFromDB()
        {
            try
            {
                Logger.LoggerWrite("Updating Preferences From DB", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                GetPreferencesFromDB();
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }

        /// <summary>
        /// Fetch Open positions from database after virtual closing and fill in closed data
        /// </summary>
        /// <param name="TaxlotClosingID"></param>
        /// <param name="taxlotIDList"></param>
        /// <param name="TaxlotClosingIDWithClosingDate"></param>
        /// <returns></returns>
        public ClosingData VirtualUnWindClosing(string TaxlotClosingID, string taxlotIDList, string TaxlotClosingIDWithClosingDate)
        {
            Logger.LoggerWrite("Virtual UnWind Closing, TaxlotClosingID: " + TaxlotClosingID + ", taxlotIDList: " + taxlotIDList + ", TaxlotClosingIDWithClosingDate: " + TaxlotClosingIDWithClosingDate, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            ClosingData ClosedData = new ClosingData();
            try
            {
                DataSet productsDataSet = CloseTradesDataManager.FetchOpenPositionsPostVirtualUnwinding(TaxlotClosingID);

                List<TaxLot> openTaxLot = CloseTradesDataManager.FillOpenPositions(productsDataSet);
                foreach (TaxLot TaxLot in openTaxLot)
                {
                    if (ClosedData.UpdatedTaxlots.ContainsKey(TaxLot.TaxLotID))
                    {
                        TaxLot Olditem = ClosedData.UpdatedTaxlots[TaxLot.TaxLotID];
                        ClosedData.Taxlots.Remove(Olditem);
                        ClosedData.UpdatedTaxlots.Remove(TaxLot.TaxLotID);
                        ClosedData.UnSavedTaxlots.Remove(Olditem);

                        ClosedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
                        ClosedData.Taxlots.Add(TaxLot);
                        ClosedData.UnSavedTaxlots.Add(TaxLot);
                    }
                    else
                    {
                        ClosedData.UpdatedTaxlots.Add(TaxLot.TaxLotID, TaxLot);
                        ClosedData.Taxlots.Add(TaxLot);
                        ClosedData.UnSavedTaxlots.Add(TaxLot);
                    }
                }
                ClosedData.PositionsToUnwind = TaxlotClosingID;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return ClosedData;
        }
        /// <summary>
        /// Returns Closing Info Cache to Client
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TaxlotClosingInfo> GetClosingStatusInfo(DateTime toDate, DateTime fromDate, DateTime toTradeDate, DateTime fromTradeDate)
        {

            Dictionary<string, TaxlotClosingInfo> closingInfoDict = new Dictionary<string, TaxlotClosingInfo>();
            try
            {
                closingInfoDict = CloseTradesDataManager.GetTaxlotsLatestClosingDates(_toClosingDate, _fromClosingDate, _toTradeDate, _fromTradeDate, true);

            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
            return closingInfoDict;
        }

        #region IClosingServices Members

        /// <summary>
        /// Update closing info cache
        /// </summary>
        /// <param name="dictionary"></param>
        public void UpdateClosingAlgoInfoCache(Dictionary<string, int> dictionary)
        {
            Logger.LoggerWrite("Updating Closing Algo Info Cache.", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
            try
            {
                foreach (KeyValuePair<string, int> item in dictionary)
                {
                    if (!_closingAlgoInfo.ContainsKey(item.Key))
                    {
                        _closingAlgoInfo.Add(item.Key, item.Value);
                        Logger.LoggerWrite("Updating Closing Algo Info Cache, key: " + item.Key + " = " + item.Value, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_preferences != null)
                    _preferences.Dispose();
                _proxy.Dispose();
            }
        }

        #endregion

        #region IClosingServices Members Cost Adjustment


        /// <summary>
        /// Gets all open taxlots.
        /// </summary>
        /// <param name="FromDate">From date.</param>
        /// <param name="CloseTradeDate">The close trade date.</param>
        /// <param name="IsCurrentDateClosing">if set to <c>true</c> [is current date closing].</param>
        /// <param name="CommaSeparatedAccountIds">The comma separated account ids.</param>
        /// <param name="CommaSeparatedAssetIds">The comma separated asset ids.</param>
        /// <param name="commaSeparatedSymbols">The comma separated symbols.</param>
        /// <param name="CommaSeparatedCustomConditions">The comma separated custom conditions.</param>
        /// <returns>CostAdjustmentResult.</returns>
        public async Task<CostAdjustmentResult> GetAllOpenTaxlots(DateTime FromDate, DateTime CloseTradeDate, bool IsCurrentDateClosing, string CommaSeparatedAccountIds, string CommaSeparatedAssetIds, string commaSeparatedSymbols, string CommaSeparatedCustomConditions)
        {
            try
            {
                ClosingData data = new ClosingData();
                List<CostAdjustmentTaxlot> taxlotList = new List<CostAdjustmentTaxlot>();
                data = await System.Threading.Tasks.Task.Run(() => GetAllClosingData(FromDate, CloseTradeDate, IsCurrentDateClosing, CommaSeparatedAccountIds, CommaSeparatedAssetIds, commaSeparatedSymbols, CommaSeparatedCustomConditions));
                foreach (TaxLot taxlot in data.Taxlots)
                {
                    //TODO: Need to get data from DB on basis of fromdate
                    if (taxlot.AUECLocalDate.Date >= FromDate.Date && taxlot.ClosingStatus != ClosingStatus.Closed)
                    {
                        taxlotList.Add(CostAdjustmentManager.GetInstance().GetTaxlot(taxlot));
                    }
                }
                CostAdjustmentResult result = new CostAdjustmentResult();
                result.AdjustedTaxlots = taxlotList;
                return result;
            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// object for locking
        /// </summary>
        private static object _lockerObj = new object();

        /// <summary>
        /// Gets all open CostAdjustmentTaxlots
        /// </summary>
        /// <param name="taxlotIDList">list of Taxlot ids</param>
        /// <returns>list of CostAdjustmentTaxlots</returns>
        public List<CostAdjustmentTaxlot> GetAllOpenCostAdjustmentTaxlots(List<string> taxlotIDList)
        {
            try
            {
                // Added lock so that other thread wait till execution of this method is not completed for one thread, PRANA-9002
                lock (_lockerObj)
                {
                    List<TaxLot> taxlotList = new List<TaxLot>();
                    String csv = String.Join(",", taxlotIDList);
                    taxlotList = CloseTradesDataManager.GetPosition(csv);

                    List<CostAdjustmentTaxlot> result = new List<CostAdjustmentTaxlot>();
                    result.AddRange(taxlotList.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Adjusts the cost.
        /// </summary>
        /// <param name="parameterList">The parameter.</param>
        /// <param name="originalList">List of allocation groups</param>
        /// <returns>CostAdjustmentResult.</returns>
        public CostAdjustmentResult AdjustCost(List<CostAdjustmentParameter> parameterList, List<AllocationGroup> originalList)
        {
            try
            {

                List<TaxLot> taxlotList = new List<TaxLot>();
                List<string> taxlotIDs = new List<string>();
                // Fetched TaxlotIds for taxlots in each parameter in parameterList
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
                parameterList.ForEach(x => x.Taxlots.ForEach(y => taxlotIDs.Add(y.TaxlotId)));
                String csv = String.Join(",", taxlotIDs.ToArray());
                taxlotList = CloseTradesDataManager.GetPosition(csv);
                //TODO: Need to get these fields while getting data.
                foreach (TaxLot taxlot in taxlotList)
                {
                    foreach (AllocationGroup group in originalList)
                    {
                        if (group.TaxLots.FirstOrDefault(x => x.TaxLotID == taxlot.TaxLotID) != null)
                        {
                            taxlot.OrderTypeTagValue = group.OrderTypeTagValue;
                            taxlot.CounterPartyID = group.CounterPartyID;
                            taxlot.VenueID = group.VenueID;
                            taxlot.CompanyUserID = group.CompanyUserID;
                            taxlot.TradingAccountID = group.TradingAccountID;
                            // Updated feilds related to settlement
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7026
                            taxlot.SettlementCurrencyID = group.SettlementCurrencyID;
                            // updated Quantity and IsPreAllocated value
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7234
                            taxlot.Quantity = group.Quantity;
                            taxlot.IsPreAllocated = group.IsPreAllocated;
                            break;
                        }
                    }
                }
                CostAdjustmentResult result = CostAdjustmentManager.GetInstance().AdjustCost(taxlotList, parameterList);
                return result;
            }
            catch (Exception ex)
            {
                LogAndDisplayOnInformationReporter.GetInstance.WriteAndDisplayOnInformationReporter(ex, LoggingConstants.CATEGORY_FLAT_FILE_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, ApplicationConstants.CONST_CLOSING_SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Get taxlotIDs for cost adjustment taxlots
        /// </summary>
        public void GetTaxLotsLatestCostAdjustmentDatesFromDB()
        {
            try
            {
                Logger.LoggerWrite("Getting TaxLots Latest Cost Adjustment Dates From DB", LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

                lock (_TaxLotIdToCostAdjustDateDictLock)
                {
                    if (_TaxLotIdToCostAdjustDateDict != null && _TaxLotIdToCostAdjustDateDict.Count > 0)
                    {
                        _TaxLotIdToCostAdjustDateDict.Clear();
                    }

                    _TaxLotIdToCostAdjustDateDict = CostAdjustmentManager.GetTaxlotsLatestCostAdjustmentTaxlotIdsFromDB();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);



                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks if cost adjustment is applied on taxlot or not
        /// </summary>
        /// <param name="taxLotID">taxlot ID</param>
        /// <returns>true if cost adjustment is applied, false otherwise</returns>
        private bool CheckCostAdjustmentStatus(string taxLotID)
        {
            bool isCostAdjustmentApplied = false;
            try
            {
                lock (_TaxLotIdToCostAdjustDateDictLock)
                {
                    if (_TaxLotIdToCostAdjustDateDict.Contains(taxLotID))
                    {
                        isCostAdjustmentApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return isCostAdjustmentApplied;
        }

        #endregion


        // Added By : Manvendra Prajapati
        // Jira : http://jira.nirvanasolutions.com:8080/browse/PRANA-8746
        /// <summary>
        /// Return closing date type, on the bases of server side closing date type preferences
        /// </summary>
        public PostTradeEnums.DateType GetClosingDateType()
        {
            PostTradeEnums.DateType dateType = PostTradeEnums.DateType.OriginalPurchaseDate;
            try
            {
                if (ConfigurationManager.AppSettings["ClosingDate"] != null)
                {
                    dateType = (PostTradeEnums.DateType)Convert.ToInt32(ConfigurationManager.AppSettings["ClosingDate"].ToString());
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dateType;
        }

        /// <summary>
        /// Get future date closed data
        /// </summary>
        /// <param name="taxlotClosingIds"></param>
        /// <returns></returns>
        public Dictionary<string, StatusInfo> GetFutureDateClosingInfo(string taxlotClosingIds)
        {
            Logger.LoggerWrite("Get Future Date Closing Info, taxlotClosingIds:" + taxlotClosingIds, LoggingConstants.CATEGORY_INFORMATION, 1, 1, System.Diagnostics.TraceEventType.Information, ApplicationConstants.CONST_CLOSING_SERVICE);

            Dictionary<string, StatusInfo> dictFutureDateClosedInfo = new Dictionary<string, StatusInfo>();
            try
            {
                dictFutureDateClosedInfo = CloseTradesDataManager.CheckClosingForFutureDate(taxlotClosingIds);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
            return dictFutureDateClosedInfo;
        }

        /// <summary>
        /// Gets the closing transaction exceptions.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="accountIDs">The account i ds.</param>
        /// <returns></returns>
        public List<Position> GetClosingTransactionExceptions(DateTime fromDate, DateTime toDate, string accountIDs)
        {
            List<Position> netPositions = new List<Position>();
            try
            {
                netPositions = CloseTradesDataManager.GetClosingTransactionExceptions(fromDate, toDate, accountIDs);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return netPositions;
        }

        /// <summary>
        /// Calculates the other fees for BasicOrderInfo.
        /// </summary>
        /// <param name="taxlots"></param>
        /// <returns></returns>
        public BasicOrderInfo CalculateOtherFeesForBasicOrderInfo(BasicOrderInfo basicOrderInfo)
        {
            BasicOrderInfo calculatedBasicOrderInfo = null;
            try
            {
                calculatedBasicOrderInfo = Prana.Global.Utilities.DeepCopyHelper.Clone(basicOrderInfo);
                CommissionCalculator.GetInstance.CalculationForBasicOrderInfo(calculatedBasicOrderInfo);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
            return calculatedBasicOrderInfo;
        }

        #region IServiceOnDemandStatus Members
        public async System.Threading.Tasks.Task<bool> HealthCheck()
        {
            // Awaiting for a completed task to make function asynchronous
            await System.Threading.Tasks.Task.CompletedTask;

            return true;
        }
        #endregion
    }
}
