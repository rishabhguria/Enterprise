using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.Utilities.DateTimeUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
//using Prana.PostTrade;

namespace Prana.CorporateAction.StockDividendRule
{
    public class StockDividend : ICorporateActionBaseRule
    {
        string _firstTag = string.Empty;
        string _secondTag = string.Empty;
        string _operation = string.Empty;
        string _targetTag = string.Empty;
        string _NewSecQtyRatio = string.Empty;
        string _OrigSecQtyRatio = string.Empty;

        /// <summary>
        /// These indexes are used in _dictGroupWiseQty to keep the information.
        /// </summary>
        const int INDEX_CUMQTY = 0;
        const int INDEX_ALLOCATEDQTY = 1;
        int _hashCode = 0;

        IPostTradeServices _postTradeServicesInstance = null;
        IAllocationServices _allocationServices = null;
        IClosingServices _closingServices = null;
        ProxyBase<IPublishing> _proxyPublishing = null;
        IActivityServices _activityService = null;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        ISecMasterServices _secmasterProxy = null;
        ICashManagementService _cashManagementService = null;

        public void Initialize(IPostTradeServices postTradeServicesInstance, int hashCode, IAllocationServices allocationServices, IClosingServices closingServices, object proxyPublishing, IActivityServices activityService, object pricingService, ISecMasterServices secmasterProxy, ICashManagementService cashManagementService)
        {
            _postTradeServicesInstance = postTradeServicesInstance;
            _hashCode = hashCode;
            _allocationServices = allocationServices;
            _closingServices = closingServices;
            _proxyPublishing = proxyPublishing as ProxyBase<IPublishing>;
            _activityService = activityService;
            _pricingServicesProxy = pricingService as DuplexProxyBase<IPricingService>;
            _secmasterProxy = secmasterProxy;
            _cashManagementService = cashManagementService;
        }

        /// <summary>
        /// It Validates the passed row
        /// </summary>
        /// <returns></returns>
        public void ValidateCAInfo(ref DataRowCollection rows)
        {
            try
            {
                foreach (DataRow row in rows)
                {
                    DataColumnCollection columns = row.Table.Columns;

                    CARulesHelper.ResetRowErrors(row);

                    double tryParseResultDouble = 0;

                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_OrigSymbolTag].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSymbolTag], "Orignal Symbol required.");
                        continue;
                    }

                    if (double.TryParse(row[CorporateActionConstants.CONST_DivRate].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_DivRate].ToString()) <= 0)
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_DivRate], "Dividend Rate required.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_DivRate], "Dividend Rate required.");
                        continue;
                    }

                    //Add current time
                    row[CorporateActionConstants.CONST_EffectiveDate] = CARulesHelper.AddDayStartTimeToDate(Convert.ToDateTime(row[CorporateActionConstants.CONST_EffectiveDate]));
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

        bool _isCashinLieuRequired = false;

        /// <summary>
        /// Applies the CA rule and returns the modified taxlots with the corporateActions applied on them
        /// </summary>
        /// <param name="taxlotList"></param>
        /// <param name="corporateActionRow"></param>
        /// <returns></returns>
        public CAPreviewResult PreviewCorporateActions(DataRowCollection caRows, ref TaxlotBaseCollection taxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId)
        {
            ClearCorporateActionGroups();
            CAPreviewResult caPreviewResult = new CAPreviewResult();
            taxlotList = new TaxlotBaseCollection();

            StringBuilder taxlotsClosedInFuture = new StringBuilder();
            StringBuilder taxlotsCAAppliedInFuture = new StringBuilder();
            StringBuilder taxlotsBoxedPosition = new StringBuilder();

            string taxlotsClosedInFutureStr = string.Empty;
            string taxlotsCAAppliedInFutureStr = string.Empty;
            string taxlotsBoxedPositionStr = string.Empty;

            try
            {
                TaxlotsCacheManager.Instance.ClearAll();
                foreach (DataRow corporateActionRow in caRows)
                {
                    //To roundoff quantity in Corporate Action as fractional shares generated.
                    int quantityToRoundoff = int.Parse(ConfigurationManager.AppSettings[CorporateActionConstants.CONST_CAQuantityToRoundOff]);

                    TaxlotBaseCollection caModifiedTaxlots = null;
                    string symbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                    DateTime date = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_ExDivDate]);
                    // Stock Dividend percentage
                    double stockDividendPercentage = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_DivRate]) / 100;
                    // Stock Dividend Factor
                    double stockDividendFactor = 1 + stockDividendPercentage;
                    DateTime exDivDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_ExDivDate]);
                    DateTime divPayoutDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_DivPayoutDate]);
                    DateTime recordDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_RecordDate]);
                    DateTime divDeclarationDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_DivDeclarationDate]);
                    string caID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();
                    bool saveStockDividendAtZeroPrice = bool.Parse(corporateActionRow[CorporateActionConstants.CONST_SaveStockDividendAtZeroPrice].ToString());

                    string origCompanyName = corporateActionRow[CorporateActionConstants.CONST_CompanyName].ToString();

                    //Add the UTC Effective Date to the function
                    TaxlotBaseCollection caOpenTaxlots = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, symbol, commaSeparatedAccountIds);
                    int cashinLieuValue = Convert.ToInt32(corporateActionRow[CorporateActionConstants.CONST_Cashinlieu].ToString());
                    double closingPrice = 0;
                    bool adjustCashinLieuatAccountLevel = caPref.AdjustFractionalSharesAtAccountPositionLevel;

                    if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.CloseAtGivenPrice))
                    {
                        closingPrice = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_ClosingPrice]);
                        _isCashinLieuRequired = true;
                    }
                    else if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.CloseAtMarkPrice))
                    {
                        closingPrice = 0;
                        // get previous business day
                        DateTime previousBusinessDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(date.Date, -1, 1);
                        // set markprice
                        try
                        {
                            closingPrice = _pricingServicesProxy.InnerChannel.GetMarkPriceForDateAndSymbol(previousBusinessDate, symbol) / stockDividendFactor;
                        }
                        catch
                        {
                        }
                        _isCashinLieuRequired = true;
                    }
                    else if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.NoCashInLieu))
                    {
                        _isCashinLieuRequired = false;
                    }

                    int count = caOpenTaxlots.Count;

                    List<string> positionalNameChangeTaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(date);

                    // checks whether taxlot is already closed or if they have any boxed positions
                    CheckTaxlotStatus(ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, taxlotsClosedInFuture, taxlotsCAAppliedInFuture, taxlotsBoxedPosition, date, caOpenTaxlots, count, positionalNameChangeTaxlotId);

                    if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                    {
                        // Arrange accountwise taxlots
                        Dictionary<int, List<TaxlotBase>> AccountwisetaxlotBaseDict = ArrangeAccountwiseTaxlotBase(caOpenTaxlots);
                        //int totalCount = 0;
                        caModifiedTaxlots = new TaxlotBaseCollection();
                        foreach (KeyValuePair<int, List<TaxlotBase>> kvp in AccountwisetaxlotBaseDict)
                        {
                            Dictionary<string, TaxlotBase> groupwiseTaxlotBaseDict = new Dictionary<string, TaxlotBase>();

                            decimal accountTotalQty = 0;

                            List<TaxlotBase> taxlotBaseColl = kvp.Value;
                            int accountTaxlotCount = taxlotBaseColl.Count;

                            for (int i = 0; i < accountTaxlotCount; i++)
                            {
                                TaxlotBase taxlotBaseAccountWise = taxlotBaseColl[i];

                                taxlotBaseAccountWise.NewCompanyName = origCompanyName;

                                CARulesHelper.FillText(taxlotBaseAccountWise);
                                double avgPrice = taxlotBaseAccountWise.AvgPrice;
                                double avgPriceAfterDividend = avgPrice / stockDividendFactor;
                                // double modifiedNotionalValue = (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier * avgPriceAfterDividend);
                                double modifiedNotionalValue = 0.0;
                                if (saveStockDividendAtZeroPrice)
                                    modifiedNotionalValue = (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier * avgPrice);
                                else
                                    modifiedNotionalValue = (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier * avgPriceAfterDividend);

                                double payReceiveOriginal = 0.0;
                                double taxLotOpenQty = taxlotBaseAccountWise.OpenQty;
                                double openTotalCommissionandFeesAddition = taxlotBaseAccountWise.OpenTotalCommissionandFees * stockDividendPercentage;

                                // decimal qtyAfterDividend = Convert.ToDecimal(taxLotOpenQty * stockDividendPercentage);
                                // round off is set as fractional shares go to many decimal places. So fractional shares should be roundoff as per the server app.config entry
                                decimal qtyAfterDividend = Math.Round(Convert.ToDecimal(taxLotOpenQty * stockDividendPercentage), quantityToRoundoff);

                                if (caPref.UseNetNotional)
                                {
                                    payReceiveOriginal = modifiedNotionalValue + (taxlotBaseAccountWise.OpenTotalCommissionandFees * GetSideMultiplier(taxlotBaseAccountWise.PositionType));
                                    taxlotBaseAccountWise.OpenTotalCommissionandFees = 0;
                                }
                                else
                                {
                                    payReceiveOriginal = modifiedNotionalValue;
                                }

                                taxlotBaseAccountWise.NotionalValue = payReceiveOriginal;
                                taxlotBaseAccountWise.OldAveragePrice = taxlotBaseAccountWise.AvgPrice;

                                taxlotBaseAccountWise.NewTaxlotOpenQty = Convert.ToString(taxLotOpenQty);
                                taxlotBaseAccountWise.CorpActionID = caID;

                                taxlotBaseAccountWise.UTCDate = corporateActionRow[CorporateActionConstants.CONST_ExDivDate].ToString();
                                taxlotBaseAccountWise.AUECDate = taxlotBaseAccountWise.UTCDate;
                                taxlotBaseAccountWise.Dividend = stockDividendFactor;
                                taxlotBaseAccountWise.DivPayoutDate = divPayoutDate.ToShortDateString();
                                taxlotBaseAccountWise.ExDivDate = exDivDate.ToShortDateString();
                                taxlotBaseAccountWise.RecordDate = recordDate.ToShortDateString();
                                taxlotBaseAccountWise.DivDeclarationDate = divDeclarationDate.ToShortDateString();
                                if (payReceiveOriginal != 0 && taxlotBaseAccountWise.OpenQty != 0 && taxlotBaseAccountWise.AssetMultiplier != 0)
                                {
                                    taxlotBaseAccountWise.NewAvgPrice = Convert.ToString(payReceiveOriginal / (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier));
                                }

                                caModifiedTaxlots.Add(taxlotBaseAccountWise);

                                // Ankit Gupta on June 12, 2014: Addition of new symbol for the dividend part
                                TaxlotBase taxlotAddition = GenerateNewTaxlotBaseAndAllocationGroup(taxlotBaseAccountWise, corporateActionRow, false);
                                taxlotAddition.CounterPartyID = 0;
                                taxlotAddition.VenueID = 0;
                                taxlotAddition.UTCDate = corporateActionRow[CorporateActionConstants.CONST_ExDivDate].ToString();
                                taxlotAddition.AUECDate = taxlotAddition.UTCDate;
                                taxlotAddition.AUECLocalDate = Convert.ToDateTime(taxlotAddition.UTCDate);
                                taxlotAddition.ProcessDate = Convert.ToDateTime(taxlotAddition.UTCDate);
                                taxlotAddition.OriginalPurchaseDate = Convert.ToDateTime(taxlotAddition.UTCDate);
                                taxlotAddition.DivPayoutDate = divPayoutDate.ToShortDateString();
                                taxlotAddition.ExDivDate = exDivDate.ToShortDateString();
                                taxlotAddition.RecordDate = recordDate.ToShortDateString();
                                taxlotAddition.DivDeclarationDate = divDeclarationDate.ToShortDateString();
                                taxlotAddition.OpenQty = Convert.ToDouble(qtyAfterDividend);
                                taxlotAddition.OpenTotalCommissionandFees = 0.0;

                                double payReceiveAddition = modifiedNotionalValue * stockDividendPercentage;

                                if (saveStockDividendAtZeroPrice)
                                    taxlotAddition.NotionalValue = 0;
                                else
                                    taxlotAddition.NotionalValue = payReceiveAddition;

                                if (saveStockDividendAtZeroPrice)
                                    taxlotAddition.AvgPrice = 0;
                                else
                                    taxlotAddition.AvgPrice = avgPriceAfterDividend;

                                taxlotAddition.NewTaxlotOpenQty = string.Empty;
                                taxlotAddition.NewAvgPrice = string.Empty;
                                if (saveStockDividendAtZeroPrice)
                                    taxlotAddition.OldAveragePrice = 0;
                                else
                                    taxlotAddition.OldAveragePrice = avgPriceAfterDividend;

                                decimal taxlotQty = 0;
                                decimal integralQty = 0;
                                decimal residualQty = 0;

                                AllocationGroup allGroupAddition = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotAddition);
                                //allGroupAddition.TransactionType = TradingTransactionType.StockDIV.ToString();
                                AddTransactionSource(allGroupAddition);

                                taxlotAddition.L2TaxlotID = allGroupAddition.TaxLots[0].TaxLotID;
                                taxlotAddition.GroupID = allGroupAddition.TaxLots[0].GroupID;

                                // Added for the Cash in Lieu at Account level when the residualQty is greater than the TaxlotQty to be closed with
                                AddGroup(allGroupAddition, ApplicationConstants.CA_ADDITION);
                                caModifiedTaxlots.Add(taxlotAddition);

                                // adjust fractional shares with cash in lieu
                                if (_isCashinLieuRequired && !adjustCashinLieuatAccountLevel)
                                {
                                    taxlotQty = qtyAfterDividend;
                                    // integer part of quantity 
                                    integralQty = Math.Truncate(taxlotQty);
                                    // decimal part if quantity is fractional
                                    residualQty = (taxlotQty - integralQty);
                                    // if residual qty > 0 then proceed
                                    if (residualQty > 0)
                                    {
                                        // Generate taxlots if Cash in lieu required
                                        GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotBaseAccountWise, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);
                                        AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);
                                    }
                                }
                                // If Cash in lieu is required at the Account level
                                else if (_isCashinLieuRequired && adjustCashinLieuatAccountLevel)
                                {
                                    accountTotalQty = accountTotalQty + Convert.ToDecimal(taxlotAddition.OpenQty);

                                    if (!groupwiseTaxlotBaseDict.ContainsKey(allGroupAddition.GroupID))
                                    {
                                        groupwiseTaxlotBaseDict.Add(allGroupAddition.GroupID, taxlotAddition);
                                    }

                                    // Cash in Lieu at Account Level: Try to close data with the last taxlot
                                    if (accountTaxlotCount.Equals(i + 1))
                                    {
                                        taxlotQty = Convert.ToDecimal(allGroupAddition.TaxLots[0].TaxLotQty);
                                        // integer part of total account quantity 
                                        integralQty = Math.Truncate(accountTotalQty);
                                        // decimal part if quantity is fractional
                                        residualQty = (accountTotalQty - integralQty);
                                        // if residual qty > 0                           
                                        if (residualQty > 0)
                                        {

                                            // Generate taxlots if Cash in lieu required
                                            GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotAddition, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);

                                            //TaxlotBase taxlotWithClosing = new TaxlotBase();
                                            //// generate new taxlot with residual qty 
                                            //// example: new generated taxlot qty is 1.5 then this condition will work.
                                            //// If new generated taxlot qty is 0.5 (less than one i.e. fractional) then no need to adjust, just generate Withdrawal taxlot                                                                             

                                            //AllocationGroup searchedAllocGroup = new AllocationGroup();
                                            //TaxlotBase searchedtaxlotBase = new TaxlotBase();
                                            //decimal searchedtaxlotQty = 0;
                                            ////if residual qty is greater that the last laxlot in the collection then search in the collection
                                            //if (residualQty > taxlotQty)
                                            //{
                                            //    // here search and send Allocation Group and TaxlotBase to this method
                                            //    Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(caID);
                                            //    List<AllocationGroup> allocationGroupAddition = allocGroupDict[ApplicationConstants.CA_ADDITION];

                                            //    // Traverse in the List of allocationGroup and check which taxlot has taxlotQty greater than the residualQty
                                            //    foreach (AllocationGroup GroupOrig in allocationGroupAddition)
                                            //    {
                                            //        if (groupwiseTaxlotBaseDict.ContainsKey(GroupOrig.GroupID))
                                            //        {
                                            //            searchedtaxlotQty = Convert.ToDecimal(GroupOrig.TaxLots[0].TaxLotQty);
                                            //            if (searchedtaxlotQty > residualQty)
                                            //            {
                                            //                searchedAllocGroup = GroupOrig;
                                            //                searchedtaxlotBase = groupwiseTaxlotBaseDict[GroupOrig.GroupID];
                                            //                break;
                                            //            }
                                            //        }
                                            //    }
                                            //    // Generate taxlots only if OpenQty of CloseWith Taxlot is greater than Zero
                                            //    if (searchedtaxlotBase.OpenQty > 0)
                                            //    {
                                            //        GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, searchedtaxlotBase, residualQty, ref searchedAllocGroup, adjustCashinLieuatAccountLevel);
                                            //        AddGroup(searchedAllocGroup, ApplicationConstants.CA_FRACTIONALADDITION);
                                            //    }
                                            //    else
                                            //    {
                                            //        caPreviewResult.ErrorMessage = " Cant apply CA: Each taxlot Qty is less than the Total Fractional Quantity.";
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    // Generate taxlots if Cash in lieu required
                                            //    GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotAddition, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);
                                            //    AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);
                                            //}
                                        }
                                    }
                                }
                                //CARulesHelper.FillText(caModifiedTaxlots[totalCount]);
                                //totalCount++;
                            }
                        }
                    }
                    TaxlotsCacheManager.Instance.AddCARow(caID, corporateActionRow);
                    TaxlotsCacheManager.Instance.AddTaxlots(caID, caModifiedTaxlots);
                    if (caModifiedTaxlots != null && caModifiedTaxlots.Count < 1)
                    {
                        caPreviewResult.NoPositionSymbols = symbol;
                        taxlotList.Clear();
                        return caPreviewResult;
                    }
                    taxlotList.AddRange(caModifiedTaxlots);
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

            if (!String.IsNullOrEmpty(taxlotsClosedInFutureStr) || !String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) || !String.IsNullOrEmpty(taxlotsBoxedPositionStr))
            {
                ///Which means that there is some taxlot for which closing is done in the future date, so first user have to unwind then
                ///and only then we will allow applying the corporate action.
                taxlotList = null;
                TaxlotsCacheManager.Instance.ClearAll();
            }
            caPreviewResult.ClosingIDs = taxlotsClosedInFutureStr;
            caPreviewResult.CAIDs = taxlotsCAAppliedInFutureStr;
            caPreviewResult.BoxedPositionTaxlotIds = taxlotsBoxedPositionStr;
            return caPreviewResult;
        }

        private TaxlotBase GenerateNewTaxlotBaseAndAllocationGroup(TaxlotBase taxlotBase, DataRow corporateActionRow, bool isWithDrawal)
        {
            TaxlotBase taxlotBaseNew = taxlotBase.Clone();
            try
            {
                CARulesHelper.FillDateInfo(taxlotBaseNew, corporateActionRow);

                if (isWithDrawal)
                {
                    if (taxlotBaseNew.PositionType == PositionType.Short)
                    {
                        taxlotBaseNew.PositionTag = PositionTag.ShortWithdrawal;
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;

                        taxlotBaseNew.TransactionType = TradingTransactionType.ShortWithdrawal.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongWithdrawal;
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Sell;

                        taxlotBaseNew.TransactionType = TradingTransactionType.LongWithdrawal.ToString();
                    }
                }
                else
                {
                    if (taxlotBaseNew.PositionType == PositionType.Short)
                    {
                        taxlotBaseNew.PositionTag = PositionTag.ShortAddition;//"6"; //short addition

                        taxlotBaseNew.TransactionType = TradingTransactionType.ShortAddition.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongAddition;//"4"; //long addition

                        taxlotBaseNew.TransactionType = TradingTransactionType.LongAddition.ToString();
                    }
                }

                CARulesHelper.FillText(taxlotBaseNew);

                if (isWithDrawal)
                {
                    taxlotBaseNew.OriginalPurchaseDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.OriginalPurchaseDate;
                }
                else
                {
                    taxlotBaseNew.OriginalPurchaseDate = taxlotBase.OriginalPurchaseDate;//Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.AUECLocalDate;
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

            return taxlotBaseNew;
        }

        /// <summary>
        /// this is used to assign transaction source i.e. origin of the transaction
        /// </summary>
        /// <param name="allocGroup"></param>
        private void AddTransactionSource(AllocationGroup allocGroup)
        {
            try
            {
                allocGroup.TransactionSource = TransactionSource.CAStockDividend;
                allocGroup.TransactionSourceTag = (int)TransactionSource.CAStockDividend;
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
        /// checks whether taxlot is already closed or if they have any boxed positions
        /// </summary>
        /// <param name="taxlotsClosedInFutureStr"></param>
        /// <param name="taxlotsCAAppliedInFutureStr"></param>
        /// <param name="taxlotsBoxedPositionStr"></param>
        /// <param name="taxlotsClosedInFuture"></param>
        /// <param name="taxlotsCAAppliedInFuture"></param>
        /// <param name="taxlotsBoxedPosition"></param>
        /// <param name="date"></param>
        /// <param name="caModifiedTaxlots"></param>
        /// <param name="count"></param>
        /// <param name="positionalNameChangeTaxlotId"></param>
        private void CheckTaxlotStatus(ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, StringBuilder taxlotsClosedInFuture, StringBuilder taxlotsCAAppliedInFuture, StringBuilder taxlotsBoxedPosition, DateTime date, TaxlotBaseCollection caModifiedTaxlots, int count, List<string> positionalNameChangeTaxlotId)
        {
            Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();
            for (int i = 0; i < count; i++)
            {
                TaxlotBase taxlotBase = caModifiedTaxlots[i];
                // check boxed positions
                string boxedPositionTaxlotID = CheckBoxedPosition(dictCheckBoxPositions, taxlotBase);
                if (!string.IsNullOrEmpty(boxedPositionTaxlotID))
                {
                    taxlotsBoxedPosition.Append(boxedPositionTaxlotID);
                    taxlotsBoxedPosition.Append(",");
                }

                if (_closingServices.CheckClosingStatus(taxlotBase.L2TaxlotID, date))
                {
                    if (!positionalNameChangeTaxlotId.Contains(taxlotBase.L2TaxlotID))
                    {
                        taxlotsClosedInFuture.Append(taxlotBase.L2TaxlotID);
                        taxlotsClosedInFuture.Append(",");
                    }
                }

                if (_closingServices.CheckCorporateActionStatus(taxlotBase.L2TaxlotID, date) || positionalNameChangeTaxlotId.Contains(taxlotBase.L2TaxlotID))
                {
                    taxlotsCAAppliedInFuture.Append(taxlotBase.L2TaxlotID);
                    taxlotsCAAppliedInFuture.Append(",");
                }
            }

            taxlotsClosedInFutureStr = taxlotsClosedInFuture.ToString().TrimEnd(new char[] { ',' });
            taxlotsCAAppliedInFutureStr = taxlotsCAAppliedInFuture.ToString().TrimEnd(new char[] { ',' });
            taxlotsBoxedPositionStr = taxlotsBoxedPosition.ToString().TrimEnd(new char[] { ',' });

        }

        public void ClearCorporateActionGroups()
        {
            try
            {
                lock (_corporateActionGroupCollection)
                {
                    _corporateActionGroupCollection.Clear();
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
        /// set closing status to closed taxlots
        /// Closing Manager sets closing status of every taxlot whether it is Fully or Partially closed. And this closing status is used by EXP&L server to show data on PM
        /// Closing Manager dose not have Corporate Action cache to check the taxlot closing status. So it is checked here
        /// </summary>
        private void SetClosingStatus(ClosingData closingData)
        {
            try
            {
                foreach (TaxLot txLot in closingData.Taxlots)
                {
                    if (txLot.TaxLotQty == 0)
                        txLot.ClosingStatus = ClosingStatus.Closed;
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

        private int GetSideMultiplier(PositionType positionType)
        {
            int sideMultiplier = 1;
            try
            {
                if (positionType.Equals(PositionType.Short))
                {
                    sideMultiplier = -1;
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
            return sideMultiplier;
        }


        private Dictionary<string, List<AllocationGroup>> GetAllocationGroupsForCA(string caID)
        {
            lock (_corporateActionGroupCollection)
            {
                if (_corporateActionGroupCollection.ContainsKey(caID))
                {
                    return _corporateActionGroupCollection[caID];
                }
            }

            return null;
        }

        private readonly Dictionary<string, Dictionary<string, List<AllocationGroup>>> _corporateActionGroupCollection = new Dictionary<string, Dictionary<string, List<AllocationGroup>>>();
        /// <summary>
        /// Generate taxlots if Cash in lieu required
        /// </summary>
        /// <param name="corporateActionRow"></param>
        /// <param name="caModifiedTaxlots"></param>
        /// <param name="closingPrice"></param>
        /// <param name="taxlotBase"></param>
        /// <param name="residualQty"></param>
        private void GenerateCashInLieuTaxlots(DataRow corporateActionRow, TaxlotBaseCollection caModifiedTaxlots, double closingPrice, TaxlotBase taxlotBase, decimal residualQty, ref AllocationGroup newGroupAddition, bool adjustCashinLieuatAccountLevel)
        {
            try
            {
                /// Withdrawal of fractional share for new symbol
                TaxlotBase taxlotFractionalShareWithdrawal = taxlotBase.Clone();
                taxlotFractionalShareWithdrawal.CounterPartyID = 0;
                taxlotFractionalShareWithdrawal.VenueID = 0;
                if (taxlotFractionalShareWithdrawal.PositionType == PositionType.Short)
                {
                    taxlotFractionalShareWithdrawal.PositionTag = PositionTag.ShortWithdrawalCashInLieu;// "10"; //short withdrawal cash in lieu
                    taxlotFractionalShareWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;

                    taxlotFractionalShareWithdrawal.TransactionType = TradingTransactionType.ShortWithdrawalCashInLieu.ToString();
                }
                else
                {
                    taxlotFractionalShareWithdrawal.PositionTag = PositionTag.LongWithdrawalCashInLieu;//"9"; //long withdrawal cash in lieu
                    taxlotFractionalShareWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Sell;

                    taxlotFractionalShareWithdrawal.TransactionType = TradingTransactionType.LongWithdrawalCashInLieu.ToString();
                }

                CARulesHelper.FillText(taxlotFractionalShareWithdrawal);

                taxlotFractionalShareWithdrawal.UTCDate = corporateActionRow[CorporateActionConstants.CONST_ExDivDate].ToString();
                taxlotFractionalShareWithdrawal.AUECDate = taxlotFractionalShareWithdrawal.UTCDate;
                taxlotFractionalShareWithdrawal.AUECLocalDate = Convert.ToDateTime(taxlotFractionalShareWithdrawal.UTCDate);

                taxlotFractionalShareWithdrawal.ProcessDate = taxlotFractionalShareWithdrawal.AUECLocalDate;
                taxlotFractionalShareWithdrawal.OriginalPurchaseDate = taxlotFractionalShareWithdrawal.AUECLocalDate; //taxlotBase.OriginalPurchaseDate;

                taxlotFractionalShareWithdrawal.AvgPrice = closingPrice;
                taxlotFractionalShareWithdrawal.OldAveragePrice = closingPrice;
                taxlotFractionalShareWithdrawal.NewAvgPrice = string.Empty;
                taxlotFractionalShareWithdrawal.OpenTotalCommissionandFees = 0;
                taxlotFractionalShareWithdrawal.OpenQty = Convert.ToDouble(residualQty);
                taxlotFractionalShareWithdrawal.NewTaxlotOpenQty = string.Empty;

                AllocationGroup allGroupFractionalShareWithdrawal = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotFractionalShareWithdrawal);

                //allGroupFractionalShareWithdrawal.TransactionType = TradingTransactionType.StockDIV.ToString();
                AddTransactionSource(allGroupFractionalShareWithdrawal);

                taxlotFractionalShareWithdrawal.L2TaxlotID = allGroupFractionalShareWithdrawal.TaxLots[0].TaxLotID;
                taxlotFractionalShareWithdrawal.GroupID = allGroupFractionalShareWithdrawal.TaxLots[0].GroupID;

                // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                // This is valid only if Fractional Share adjustment in Cash In Lieu is at Taxlot Level
                // At Position Level Fractional share will not be closed, it will be opened and manually closed
                if (!adjustCashinLieuatAccountLevel)
                    newGroupAddition.TaxLots[0].ClosingWithTaxlotID = taxlotFractionalShareWithdrawal.L2TaxlotID;

                AddGroup(allGroupFractionalShareWithdrawal, ApplicationConstants.CA_FRACTIONALWITHDRAWAL);
                caModifiedTaxlots.Add(taxlotFractionalShareWithdrawal);
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


        private void AddGroup(AllocationGroup group, string positionTag)
        {
            lock (_corporateActionGroupCollection)
            {
                if (_corporateActionGroupCollection.ContainsKey(group.CorpActionID))
                {
                    if (_corporateActionGroupCollection[group.CorpActionID].ContainsKey(positionTag))
                        _corporateActionGroupCollection[group.CorpActionID][positionTag].Add(group);
                    else
                        _corporateActionGroupCollection[group.CorpActionID].Add(positionTag, new List<AllocationGroup>(new AllocationGroup[] { group }));
                }
                else
                {
                    Dictionary<string, List<AllocationGroup>> positionTagGrDict = new Dictionary<string, List<AllocationGroup>>();
                    positionTagGrDict.Add(positionTag, new List<AllocationGroup>(new AllocationGroup[] { group }));
                    _corporateActionGroupCollection.Add(group.CorpActionID, positionTagGrDict);
                }
            }
        }

        private Dictionary<int, List<TaxlotBase>> ArrangeAccountwiseTaxlotBase(TaxlotBaseCollection taxlotList)
        {
            Dictionary<int, List<TaxlotBase>> taxlotBaseDict = new Dictionary<int, List<TaxlotBase>>();

            try
            {
                int count = taxlotList.Count;

                for (int i = 0; i < count; i++)
                {
                    TaxlotBase taxlotbase = taxlotList[i];

                    if (taxlotBaseDict.ContainsKey(taxlotbase.Level1ID))
                    {
                        List<TaxlotBase> baseList = new List<TaxlotBase>();
                        baseList = taxlotBaseDict[taxlotbase.Level1ID];
                        baseList.Add(taxlotbase);

                        taxlotBaseDict[taxlotbase.Level1ID] = baseList;
                    }
                    else
                    {
                        List<TaxlotBase> baseList = new List<TaxlotBase>();
                        baseList.Add(taxlotbase);
                        taxlotBaseDict.Add(taxlotbase.Level1ID, baseList);
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

            return taxlotBaseDict;
        }


        // Returns TaxlotID of a Symbol if it has box positions in the application
        private string CheckBoxedPosition(Dictionary<int, string> dictCheckBoxPositions, TaxlotBase taxlotBase)
        {
            string taxlotID = string.Empty;
            try
            {
                if (!dictCheckBoxPositions.ContainsKey(taxlotBase.Level1ID))
                {
                    if (taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Open || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed)
                    {
                        dictCheckBoxPositions.Add(taxlotBase.Level1ID, PositionType.Long.ToString());
                    }
                    else
                    {
                        dictCheckBoxPositions.Add(taxlotBase.Level1ID, PositionType.Short.ToString());
                    }
                }
                else
                {
                    string existingValue = dictCheckBoxPositions[taxlotBase.Level1ID];
                    if ((taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Open || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed) && existingValue.Equals(PositionType.Short.ToString()))
                    {
                        dictCheckBoxPositions[taxlotBase.Level1ID] = "Both";
                        taxlotID = taxlotBase.L2TaxlotID;
                    }
                    else if ((taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_SellShort || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed) && existingValue.Equals(PositionType.Long.ToString()))
                    {
                        dictCheckBoxPositions[taxlotBase.Level1ID] = "Both";
                        taxlotID = taxlotBase.L2TaxlotID;
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
            return taxlotID;
        }



        /// <summary>
        /// For Stock Dividend CA Saving
        /// </summary>
        /// <param name="corporateActionListString"></param>
        /// <param name="updatedTaxlots"></param>
        public bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            bool isSuccessful = false;
            try
            {
                CAOnProcessObjects caOnProcessObject = new CAOnProcessObjects();
                caOnProcessObject.CorporateActionListString = corporateActionListString;
                TaxlotsCacheManager.Instance.FillCAIDWiseXML(corporateActionListString);
                TaxlotBaseCollection updatedTaxlotsToSave = new TaxlotBaseCollection();
                foreach (TaxlotBase updatedTaxlot in updatedTaxlots)
                {
                    //if (updatedTaxlot.PositionTag != PositionTag.TCloseLongCashInLieu && updatedTaxlot.PositionTag != PositionTag.TCloseShortCashInLieu)
                    if (!string.IsNullOrEmpty(updatedTaxlot.NewTaxlotOpenQty))
                        updatedTaxlotsToSave.Add(updatedTaxlot);
                }
                caOnProcessObject.Taxlots = updatedTaxlotsToSave;
                caOnProcessObject.IsApplied = true;
                List<AllocationGroup> newllocGrs = new List<AllocationGroup>();

                foreach (KeyValuePair<string, TaxlotBaseCollection> keyValue in TaxlotsCacheManager.Instance.GetCAWiseTaxlots())
                {
                    caOnProcessObject.CorporateActionID = new Guid(keyValue.Key);
                    Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(keyValue.Key);

                    List<AllocationGroup> allGroupAddition = allocGroupDict[ApplicationConstants.CA_ADDITION];
                    newllocGrs.AddRange(allGroupAddition);
                    if (_isCashinLieuRequired)
                    {
                        List<AllocationGroup> allGroupFractionalSharesWithdrawal = new List<AllocationGroup>();
                        if (allocGroupDict.ContainsKey(ApplicationConstants.CA_FRACTIONALWITHDRAWAL))
                        {
                            allGroupFractionalSharesWithdrawal = allocGroupDict[ApplicationConstants.CA_FRACTIONALWITHDRAWAL];
                        }

                        DataRow dr = TaxlotsCacheManager.Instance.GetCARowByID(keyValue.Key);
                        // new group will saved by post trade cache manager
                        newllocGrs.AddRange(allGroupFractionalSharesWithdrawal);
                        ///These are taxlots generated by posttradecachemanager, hence these would be sent and saved by posttradecachemanager.

                        caOnProcessObject.FromDate = DateTime.Parse(dr[CorporateActionConstants.CONST_ExDivDate].ToString());
                        caOnProcessObject.CorporateActionListString = TaxlotsCacheManager.Instance.GetCAStrByID(keyValue.Key);

                        // fractional shares addition
                        List<AllocationGroup> allGroupFractionalSharesAddition = new List<AllocationGroup>();
                        if (allocGroupDict.ContainsKey(ApplicationConstants.CA_FRACTIONALADDITION))
                        {
                            allGroupFractionalSharesAddition = allocGroupDict[ApplicationConstants.CA_FRACTIONALADDITION];
                        }

                        List<TaxLot> ClosingTaxlots = new List<TaxLot>();
                        List<TaxLot> fractionalPositionalTaxlots = new List<TaxLot>();
                        List<TaxLot> fractionalClosingTaxlots = new List<TaxLot>();

                        foreach (AllocationGroup fractionalGroupAddition in allGroupFractionalSharesAddition)
                        {
                            TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(fractionalGroupAddition.TaxLots[0]);
                            taxlotToclose.Update(fractionalGroupAddition.TaxLots[0]);
                            //Modifying all dates to the effectivedate, as the original transactions are closed on the effectivedate only
                            taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                            //commented by omshiv, ACA Cleanup
                            //taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;
                            //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                            taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                            taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                            taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                            fractionalPositionalTaxlots.Add(taxlotToclose);
                        }
                        foreach (AllocationGroup fractionalGroupWithdrawal in allGroupFractionalSharesWithdrawal)
                        {
                            TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(fractionalGroupWithdrawal.TaxLots[0]);
                            taxlotToclose.Update(fractionalGroupWithdrawal.TaxLots[0]);
                            //Modifying all dates to the effectivedate, as the withdrawal transactions are closed on the effectivedate only
                            taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                            //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                            taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                            taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                            taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                            ClosingTaxlots.Add(taxlotToclose);

                            fractionalClosingTaxlots.Add(taxlotToclose);
                        }

                        /// Cash in Lieu transactions will be closed as normal closing
                        if (fractionalClosingTaxlots.Count > 0 && fractionalPositionalTaxlots.Count > 0)
                        {
                            caOnProcessObject.ClosingData = _closingServices.CloseDataforPairedTaxlots(fractionalPositionalTaxlots, fractionalClosingTaxlots, false);

                            // Set Closing Status Closed for those closed with the CA
                            SetClosingStatus(caOnProcessObject.ClosingData);
                        }
                    }
                    caOnProcessObject.NewGeneratedTaxlots = newllocGrs;
                }

                isSuccessful = DBManager.SaveCAForStockDividend(caOnProcessObject, _proxyPublishing, _activityService, _allocationServices, _closingServices, userID);
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
            return isSuccessful;
        }

        /// <summary>
        /// BeforeUndoPreview and before ca apply preview
        /// </summary>
        /// <param name="caWiseDates"></param>
        /// <returns></returns>
        public string CheckTaxlotsBeforeUndoPreview(Dictionary<string, DateTime> caWiseDates)
        {
            Dictionary<string, List<string>> caWiseTaxlots = null;
            StringBuilder postCAClosedTaxlots = new StringBuilder();
            try
            {
                string[] caArr = new string[caWiseDates.Count];
                caWiseDates.Keys.CopyTo(caArr, 0);
                string caIDs = String.Join(",", caArr);

                caWiseTaxlots = AllocationDataManager.GetTaxlotIdsBeforeUndoPreview(caIDs);

                if (caWiseTaxlots != null)
                {
                    foreach (KeyValuePair<string, List<string>> ca in caWiseTaxlots)
                    {
                        DateTime caDate = caWiseDates[ca.Key];
                        List<string> positionalNameChangeTaxlotId = AllocationDataManager.GetPositionalCorpActionTaxlotId(caDate, caIDs);

                        foreach (string taxlotID in positionalNameChangeTaxlotId)
                        {
                            if (_closingServices.CheckClosingStatus(taxlotID, caDate))
                            {
                                postCAClosedTaxlots.Append(taxlotID);
                                postCAClosedTaxlots.Append(",");
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
            return postCAClosedTaxlots.ToString().TrimEnd(new char[] { ',' });
        }

        public TaxlotBaseCollection PreviewUndoCorporateActions(string caIDsStr, DataRowCollection caRows)
        {
            TaxlotBaseCollection taxlots = null;
            try
            {
                taxlots = AllocationDataManager.GetOpenPositionforUndoPreview(caIDsStr);
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
            return taxlots;
        }

        public bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots)
        {
            bool isSuccessful = false;
            try
            {
                CAOnProcessObjects requestObject = new CAOnProcessObjects();
                requestObject.CorporateActionIDs = caIDs;
                if (!String.IsNullOrEmpty(caIDs))
                {
                    isSuccessful = DBManager.UndoStockDividend(requestObject, taxlots, _closingServices, _allocationServices, _proxyPublishing, _activityService);
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
            return isSuccessful;
        }

        #region ICorporateActionBaseRule Members


        #endregion


        public bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots, bool isSMModificationRequired)
        {
            return false;
        }
    }
}
