using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
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
using System.Linq;
using System.Text;

namespace Prana.CorporateAction.SpinOffRule
{
    public class SpinOff : ICorporateActionBaseRule
    {
        #region ICorporateActionBaseRule Members

        int _hashCode = 0;

        IPostTradeServices _postTradeServicesInstance = null;
        IAllocationServices _allocationServices = null;
        IClosingServices _closingServices = null;
        ProxyBase<IPublishing> _proxyPublishing = null;
        IActivityServices _activityService = null;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        ISecMasterServices _secmasterProxy = null;
        ICashManagementService _cashManagementService = null;
        List<Transaction> _nonTradingCashInLieuTransactions = null;
        int _corpActionCashInLieuSubAccountId_Cr = Convert.ToInt16(ConfigurationManager.AppSettings["CorpActionCashInLieuSubAccount_Cr"]);
        int _corpActionCashInLieuSubAccountId_Dr = Convert.ToInt16(ConfigurationManager.AppSettings["CorpActionCashInLieuSubAccount_Dr"]);

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

                    int tryParseResult = 0;
                    double tryParseResultDouble = 0;

                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_OrigSymbolTag].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSymbolTag], "Orignal Symbol required.");
                        continue;
                    }
                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_NewSymbolTag].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSymbolTag], "New Symbol required.");
                        continue;
                    }
                    if (row[CorporateActionConstants.CONST_OrigSymbolTag].ToString().ToUpper().Equals(row[CorporateActionConstants.CONST_NewSymbolTag].ToString().ToUpper()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSymbolTag], "New Symbol should be different than the Orignal Symbol.");
                        continue;
                    }

                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_CompanyName].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_CompanyName], "Old Company Name required.");
                        continue;
                    }
                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_NewCompanyName].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewCompanyName], "New Company Name required.");
                        continue;
                    }

                    if (row[CorporateActionConstants.CONST_CompanyName].ToString().ToUpper().Equals(row[CorporateActionConstants.CONST_NewCompanyName].ToString().ToUpper()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewCompanyName], "New Company Name should be different than the Old Comapany Name.");
                        continue;
                    }

                    if (int.TryParse(row[CorporateActionConstants.CONST_SymbologyID].ToString(), out tryParseResult))
                    {
                        if (int.Parse(row[CorporateActionConstants.CONST_SymbologyID].ToString()) < 0)
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_SymbologyID], "Symbology required.");
                            continue;
                        }
                    }

                    if (double.TryParse(row[CorporateActionConstants.CONST_OrigSecQtyRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_OrigSecQtyRatio].ToString()) <= 0)
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Qty Ratio should be greater than 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Qty Ratio should be greater than 0.");
                        continue;
                    }
                    if (double.TryParse(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString()) <= 0)
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Qty Ratio should be greater than 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Qty Ratio should be greater than 0.");
                        continue;
                    }

                    if (double.TryParse(row[CorporateActionConstants.CONST_ExchangeRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_ExchangeRatio].ToString()) <= 0 || Convert.ToDouble(row[CorporateActionConstants.CONST_ExchangeRatio].ToString()) >= 1)
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_ExchangeRatio], "Adjustment Factor should be greater than 0 and less than 1.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_ExchangeRatio], "Adjustment Factor should be greater than 0 and less than 1.");
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

        /// <summary>
        /// Applies the CA rule and returns the modified taxlots with the corporateActions applied on them
        /// </summary>
        /// <param name="taxlotList"></param>
        /// <param name="corporateActionRow"></param>
        /// <returns></returns>
        public CAPreviewResult PreviewCorporateActions(DataRowCollection caRows, ref TaxlotBaseCollection modifiedTaxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int counterPartyId)
        {
            //This will first remove any entry for the corp action...
            ClearCorporateActionGroups();

            CAPreviewResult caPreviewResult = new CAPreviewResult();
            modifiedTaxlotList = new TaxlotBaseCollection();

            string taxlotsClosedInFutureStr = string.Empty;
            string taxlotsCAAppliedInFutureStr = string.Empty;
            string taxlotsBoxedPositionStr = string.Empty;
            // TODO: When there are no taxlots for a symbol then return a message should go to UI (Obviously in case of Multiple CA's).
            try
            {
                TaxlotsCacheManager.Instance.ClearAll();

                foreach (DataRow corporateActionRow in caRows)
                {
                    TaxlotBaseCollection caModifiedTaxlots = null;

                    CorpActionParameterHelper helper = GetCorpActionParameters(corporateActionRow, caPref, counterPartyId);
                    //Get Open positions as of date for a particular symbol

                    TaxlotBaseCollection openTaxlotsList = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(helper.EffectiveDate, helper.OriginalSymbol, commaSeparatedAccountIds);

                    //check whether taxlot is already closed or not and check boxed positions
                    CheckTaxlotStatus(helper.EffectiveDate, openTaxlotsList, ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr);

                    if (String.IsNullOrWhiteSpace(taxlotsClosedInFutureStr) && String.IsNullOrWhiteSpace(taxlotsCAAppliedInFutureStr) && String.IsNullOrWhiteSpace(taxlotsBoxedPositionStr))
                    {
                        caModifiedTaxlots = GetModifiedTaxlotForSpinoffSecurity(corporateActionRow, openTaxlotsList, helper);
                    }

                    if (String.IsNullOrWhiteSpace(taxlotsClosedInFutureStr) && String.IsNullOrWhiteSpace(taxlotsCAAppliedInFutureStr) && String.IsNullOrWhiteSpace(taxlotsBoxedPositionStr))
                    {
                        /// Taxlots are not closed and CA not applied in future hence ready for applying new ca
                        TaxlotsCacheManager.Instance.AddCARow(helper.CAID, corporateActionRow);
                        TaxlotsCacheManager.Instance.AddTaxlots(helper.CAID, caModifiedTaxlots);
                    }

                    if (caModifiedTaxlots != null && caModifiedTaxlots.Count < 1)
                    {
                        modifiedTaxlotList.Clear();
                        caPreviewResult.NoPositionSymbols = helper.OriginalSymbol;
                        return caPreviewResult;
                    }
                    else
                    {
                        modifiedTaxlotList.AddRange(caModifiedTaxlots);
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

            if (!String.IsNullOrEmpty(taxlotsClosedInFutureStr) || !String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) || !String.IsNullOrEmpty(taxlotsBoxedPositionStr))
            {
                modifiedTaxlotList = null;
                TaxlotsCacheManager.Instance.ClearAll();
            }
            caPreviewResult.ClosingIDs = taxlotsClosedInFutureStr;
            caPreviewResult.CAIDs = taxlotsCAAppliedInFutureStr;
            caPreviewResult.BoxedPositionTaxlotIds = taxlotsBoxedPositionStr;
            return caPreviewResult;

        }

        /// <summary>
        /// Gets the corporate action parameters.
        /// </summary>
        /// <param name="corporateActionRow">The corporate action row.</param>
        /// <param name="caPref">The ca preference.</param>
        /// <param name="counterPartyId">The counter party identifier.</param>
        /// <returns></returns>
        private CorpActionParameterHelper GetCorpActionParameters(DataRow corporateActionRow, CAPreferences caPref, int counterPartyId)
        {
            CorpActionParameterHelper helper = new CorpActionParameterHelper();
            try
            {
                helper.OriginalSymbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                helper.OriginalSymbolCompanyName = corporateActionRow[CorporateActionConstants.CONST_CompanyName].ToString();
                helper.NewSymbol = corporateActionRow[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                helper.NewSymbolCompanyName = corporateActionRow[CorporateActionConstants.CONST_NewCompanyName].ToString();
                helper.CAID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();
                helper.NewSharesReceivedRatio = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_Factor]);
                helper.SpinOffRatio = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_ExchangeRatio]);
                helper.SpinOffRatio = (1 - helper.SpinOffRatio) * 100;
                helper.CashInLieuAt = Convert.ToInt32(corporateActionRow[CorporateActionConstants.CONST_Cashinlieu].ToString());
                helper.EffectiveDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                helper.AdjustCashinLieuatAccountLevel = caPref.AdjustFractionalSharesAtAccountPositionLevel;
                helper.UseNetNotional = caPref.UseNetNotional;
                helper.CounterPartyId = counterPartyId;

                //get currencyId from security services, if information is not available in security master then set it to original symbol currency (for newly generated symbols)
                if (_secmasterProxy != null)
                {
                    List<SecMasterBaseObj> symbolList = _secmasterProxy.GetSecMasterDataForListSync(new List<string> { helper.OriginalSymbol, helper.NewSymbol }, ApplicationConstants.SymbologyCodes.TickerSymbol, 0);

                    SecMasterBaseObj origSymbol = symbolList.FirstOrDefault(x => x.TickerSymbol == helper.OriginalSymbol);
                    helper.OriginalSymbolCurrencyID = origSymbol != null ? origSymbol.CurrencyID : CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                    helper.OriginalSymbolAUECID = origSymbol.AUECID;

                    SecMasterBaseObj newSymbol = symbolList.FirstOrDefault(x => x.TickerSymbol == helper.NewSymbol);
                    helper.NewSymbolCurrencyID = newSymbol != null ? newSymbol.CurrencyID : helper.OriginalSymbolCurrencyID;
                    helper.NewSymbolAUECID = newSymbol.AUECID;
                }

                //To roundoff quantity in Corporate Action as fractional shares generated.
                helper.QuantityToRoundoff = int.Parse(ConfigurationManager.AppSettings[CorporateActionConstants.CONST_CAQuantityToRoundOff]);

                if (helper.CashInLieuAt.Equals((int)ApplicationConstants.CashInLieu.CloseAtGivenPrice))
                {
                    helper.ClosingPrice = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_ClosingPrice]);
                    helper.IsCashInLieuRequired = true;
                }
                else if (helper.CashInLieuAt.Equals((int)ApplicationConstants.CashInLieu.CloseAtMarkPrice))
                {
                    // get previous business day
                    DateTime previousBusinessDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(helper.EffectiveDate, -1, 1);

                    // set markprice
                    try
                    {
                        helper.ClosingPrice = _pricingServicesProxy.InnerChannel.GetMarkPriceForDateAndSymbol(previousBusinessDate, helper.OriginalSymbol) / helper.NewSharesReceivedRatio;
                    }
                    catch
                    {
                    }
                    helper.IsCashInLieuRequired = true;
                }
                else if (helper.CashInLieuAt.Equals((int)ApplicationConstants.CashInLieu.NoCashInLieu))
                {
                    helper.IsCashInLieuRequired = false;
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
            return helper;
        }

        private TaxlotBaseCollection GetModifiedTaxlotForSpinoffSecurity(DataRow corporateActionRow, TaxlotBaseCollection openTaxlotsList, CorpActionParameterHelper helper)
        {
            TaxlotBaseCollection caModifiedTaxlots = new TaxlotBaseCollection();
            try
            {
                // Arrange accountwise taxlots
                Dictionary<int, List<TaxlotBase>> AccountwisetaxlotBaseDict = ArrangeAccountwiseTaxlotBase(openTaxlotsList);

                ClearNonTradingTransaction();

                foreach (KeyValuePair<int, List<TaxlotBase>> kvp in AccountwisetaxlotBaseDict)
                {

                    helper.OriginalSymbolFXRate = GetAccountWiseFXRate(helper.OriginalSymbolCurrencyID, helper.EffectiveDate, kvp.Key, helper.OriginalSymbolAUECID);
                    helper.NewSymbolFXRate = GetAccountWiseFXRate(helper.NewSymbolCurrencyID, helper.EffectiveDate, kvp.Key, helper.NewSymbolAUECID);

                    List<TaxlotBase> taxlotBaseColl = kvp.Value;
                    int accountTaxlotCount = taxlotBaseColl.Count;

                    double sumOfTaxlotQuantities = 0;
                    double sumOfIntegralTaxlotQuantities = 0;
                    List<QuantityData> taxlotQuantities = new List<QuantityData>();

                    for (int i = 0; i < accountTaxlotCount; i++)
                    {
                        TaxlotBase taxlotBase = taxlotBaseColl[i];
                        taxlotBase.CorpActionID = helper.CAID;
                        taxlotBase.NewCompanyName = helper.OriginalSymbolCompanyName;

                        TaxlotBase taxlotOriginal = taxlotBase.Clone();
                        CARulesHelper.FillText(taxlotOriginal);

                        AllocationGroup allGroupOrig = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotOriginal);
                        //Overriding the new generated taxlotid from the original taxlotid. Since we have to use this generated taxlot with
                        //some changes as the withdrawal taxlot, hence need to generate it from allocation module.
                        allGroupOrig.GroupID = taxlotOriginal.GroupID;
                        allGroupOrig.TaxLots[0].GroupID = taxlotOriginal.GroupID;
                        allGroupOrig.TaxLots[0].TaxLotID = taxlotOriginal.L2TaxlotID;

                        // Withdrawal for original symbol
                        TaxlotBase taxlotWithdrawal = GenerateTaxlotBaseForAddtionOrWithdrawal(taxlotBase, corporateActionRow, true);
                        UpdateAttributesWithdrawalTaxlot(taxlotWithdrawal, helper);

                        AllocationGroup allGroupWithdrawal = GetAllocationGroup(taxlotWithdrawal);
                        // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                        allGroupOrig.TaxLots[0].ClosingWithTaxlotID = taxlotWithdrawal.L2TaxlotID;
                        // add original taxlot in the collection
                        AddGroup(allGroupOrig, ApplicationConstants.CA_ORIGINAL);
                        caModifiedTaxlots.Add(taxlotOriginal);
                        // add Withdrawal taxlot in the collection
                        caModifiedTaxlots.Add(taxlotWithdrawal);
                        AddGroup(allGroupWithdrawal, ApplicationConstants.CA_WITHDRAWAL);

                        /// Addition for same symbol with cost adjustment
                        TaxlotBase taxlotAdditionForSameSymbol = GenerateTaxlotBaseForAddtionOrWithdrawal(taxlotBase, corporateActionRow, false);
                        UpdateAttributesForSameSymbolCostAdjustment(taxlotAdditionForSameSymbol, helper);
                        AllocationGroup allGroupAdditionForSameSymbol = GetAllocationGroup(taxlotAdditionForSameSymbol);
                        AddGroup(allGroupAdditionForSameSymbol, ApplicationConstants.CA_ADDITION);
                        caModifiedTaxlots.Add(taxlotAdditionForSameSymbol);

                        /// Addition for new symbol
                        TaxlotBase taxlotAdditionNewSymbol = GenerateTaxlotBaseForAddtionOrWithdrawal(taxlotBase, corporateActionRow, false);
                        UpdateAttributesAdditionForNewSymbol(helper, taxlotAdditionNewSymbol, ref taxlotQuantities, ref sumOfTaxlotQuantities, ref sumOfIntegralTaxlotQuantities);
                        AllocationGroup allGroupAdditionForNewSymbol = GetAllocationGroup(taxlotAdditionNewSymbol);

                        taxlotQuantities[taxlotQuantities.Count - 1].GroupID = allGroupAdditionForNewSymbol.GroupID;
                        taxlotQuantities[taxlotQuantities.Count - 1].TaxLotID = allGroupAdditionForNewSymbol.TaxLots[0].TaxLotID;

                        AddGroup(allGroupAdditionForNewSymbol, ApplicationConstants.CA_ADDITION);
                        caModifiedTaxlots.Add(taxlotAdditionNewSymbol);
                    }

                    ////Generate Cash In Lieu - Non Trading entry        

                    if (helper.IsCashInLieuRequired)
                    {
                        taxlotQuantities = taxlotQuantities.OrderByDescending(t => t.FractionalPart).ThenBy(a => a.IntegralPart).ThenBy(x => x.OriginalPurchaseDate).ThenBy(g => g.AvgPrice).ToList();

                        double roundedTotalTaxlotQuantities = Math.Floor(sumOfTaxlotQuantities);
                        double cashInLieuQuantity = (sumOfTaxlotQuantities - roundedTotalTaxlotQuantities);

                        double amountToBeAdjusted_local = 0.0;
                        double amountToBeAdjusted_base = 0.0;

                        AllocationGroup allocationGroup_CashInLieu = null;

                        for (int counter = 0; counter < taxlotQuantities.Count; counter++)
                        {
                            QuantityData qtyData = taxlotQuantities[counter];
                            if (counter < roundedTotalTaxlotQuantities - sumOfIntegralTaxlotQuantities)
                                qtyData.MoveToCeiling();
                            else
                                qtyData.MoveToFloor();

                            List<AllocationGroup> allocationGroupAddition = GetAllocationGroupsForCA(helper.CAID)[ApplicationConstants.CA_ADDITION];
                            allocationGroup_CashInLieu = allocationGroupAddition.FirstOrDefault(g => g.GroupID == qtyData.GroupID);

                            TaxlotBase taxlotBase = caModifiedTaxlots.FirstOrDefault(t => t.L2TaxlotID == qtyData.TaxLotID);

                            // if qty is greater than 0, update Qty, Price and Notional value
                            if (qtyData.Quantity > 0)
                            {
                                taxlotBase.OpenQty = qtyData.Quantity;
                                taxlotBase.AvgPrice = taxlotBase.NotionalValue / taxlotBase.OpenQty;

                                allocationGroup_CashInLieu.Quantity = qtyData.Quantity;
                                allocationGroup_CashInLieu.CumQty = qtyData.Quantity;
                                allocationGroup_CashInLieu.AllocatedQty = qtyData.Quantity;
                                allocationGroup_CashInLieu.AvgPrice = taxlotBase.AvgPrice;
                                allocationGroup_CashInLieu.TaxLots[0].TaxLotQty = qtyData.Quantity;
                                allocationGroup_CashInLieu.TaxLots[0].AvgPrice = taxlotBase.AvgPrice;
                            }
                            else
                            {
                                // if qty is less than 0, generate a closing (withdrawal) position and transer the cost

                                // Withdrawal for original symbol
                                TaxlotBase taxlotWithdrawal = GenerateTaxlotBaseForAddtionOrWithdrawal(taxlotBase, corporateActionRow, true);
                                UpdateAttributesWithdrawalTaxlot(taxlotWithdrawal, helper);
                                AllocationGroup allGroupWithdrawal = GetAllocationGroup(taxlotWithdrawal);
                                // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                                allocationGroup_CashInLieu.TaxLots[0].ClosingWithTaxlotID = taxlotWithdrawal.L2TaxlotID;
                                // add Withdrawal taxlot in the collection
                                caModifiedTaxlots.Add(taxlotWithdrawal);
                                AddGroup(allGroupWithdrawal, ApplicationConstants.CA_WITHDRAWAL);

                                amountToBeAdjusted_local = amountToBeAdjusted_local + taxlotWithdrawal.NotionalValue;
                                amountToBeAdjusted_base = amountToBeAdjusted_base + taxlotWithdrawal.NotionalValueBase;
                            }
                        }

                        if (amountToBeAdjusted_local > 0)
                        {
                            for (int counter = 0; counter < taxlotQuantities.Count; counter++)
                            {
                                QuantityData qtyData = taxlotQuantities[counter];
                                List<AllocationGroup> allocationGroupAddition = GetAllocationGroupsForCA(helper.CAID)[ApplicationConstants.CA_ADDITION];
                                AllocationGroup allocationGroup = allocationGroupAddition.FirstOrDefault(g => g.GroupID == qtyData.GroupID);

                                if (allocationGroup.TaxLots[0].ClosingWithTaxlotID == null)
                                {
                                    TaxlotBase taxlotBase = caModifiedTaxlots.FirstOrDefault(t => t.L2TaxlotID == qtyData.TaxLotID);

                                    taxlotBase.NotionalValue = taxlotBase.NotionalValue + amountToBeAdjusted_local;
                                    taxlotBase.NotionalValueBase = taxlotBase.NotionalValueBase + amountToBeAdjusted_base;

                                    taxlotBase.AvgPrice = taxlotBase.NotionalValue / taxlotBase.OpenQty;

                                    allocationGroup.AvgPrice = taxlotBase.AvgPrice;
                                    allocationGroup.TaxLots[0].AvgPrice = taxlotBase.AvgPrice;

                                    break;
                                }
                            }
                        }

                        if (cashInLieuQuantity > 0 && allocationGroup_CashInLieu != null)
                        {
                            Transaction nonTradingTranscation = AddNewTransactionForCashInLieu();
                            UpdateNonTradingTransactionForCashInLieu(helper, nonTradingTranscation, cashInLieuQuantity, allocationGroup_CashInLieu);
                        }
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
            return caModifiedTaxlots;
        }

        private void UpdateNonTradingTransactionForCashInLieu(CorpActionParameterHelper helper, Transaction nonTradingTranscation, double cashInLieuQuantity, AllocationGroup allocationGroup_CashInLieu)
        {
            try
            {
                nonTradingTranscation.TransactionEntries[0].AccountID = nonTradingTranscation.TransactionEntries[1].AccountID = allocationGroup_CashInLieu.TaxLots[0].Level1ID;
                nonTradingTranscation.TransactionEntries[0].DR = nonTradingTranscation.TransactionEntries[1].CR = (Decimal)(cashInLieuQuantity * helper.ClosingPrice);
                nonTradingTranscation.TransactionEntries[0].Symbol = nonTradingTranscation.TransactionEntries[1].Symbol = allocationGroup_CashInLieu.TaxLots[0].Symbol;
                nonTradingTranscation.TransactionEntries[0].CurrencyID = nonTradingTranscation.TransactionEntries[1].CurrencyID = allocationGroup_CashInLieu.TaxLots[0].CurrencyID;
                nonTradingTranscation.TransactionEntries[0].FxRate = nonTradingTranscation.TransactionEntries[1].FxRate = allocationGroup_CashInLieu.TaxLots[0].FXRate;
                nonTradingTranscation.TransactionEntries[0].FXConversionMethodOperator = nonTradingTranscation.TransactionEntries[1].FXConversionMethodOperator = allocationGroup_CashInLieu.TaxLots[0].FXConversionMethodOperator;
                nonTradingTranscation.TransactionEntries[0].TransactionDate = nonTradingTranscation.TransactionEntries[1].TransactionDate = allocationGroup_CashInLieu.TaxLots[0].AUECLocalDate;
                nonTradingTranscation.TransactionEntries[0].UserId = nonTradingTranscation.TransactionEntries[1].UserId = allocationGroup_CashInLieu.TaxLots[0].CompanyUserID;
                nonTradingTranscation.TransactionEntries[0].TaxLotId = nonTradingTranscation.TransactionEntries[1].TaxLotId = helper.CAID;
                nonTradingTranscation.TransactionEntries[0].Description = nonTradingTranscation.TransactionEntries[1].Description = "Cash In Lieu";
                if (allocationGroup_CashInLieu.TaxLots[0].OrderSideTagValue.Equals(FIXConstants.SIDE_Buy) || allocationGroup_CashInLieu.TaxLots[0].OrderSideTagValue.Equals(FIXConstants.SIDE_Buy_Open))
                {
                    nonTradingTranscation.TransactionEntries[0].SubAcID = _corpActionCashInLieuSubAccountId_Dr;
                    nonTradingTranscation.TransactionEntries[1].SubAcID = _corpActionCashInLieuSubAccountId_Cr;
                }
                else
                {
                    nonTradingTranscation.TransactionEntries[0].SubAcID = _corpActionCashInLieuSubAccountId_Cr;
                    nonTradingTranscation.TransactionEntries[1].SubAcID = _corpActionCashInLieuSubAccountId_Dr;
                }

                _nonTradingCashInLieuTransactions.Add(nonTradingTranscation);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Create a Transaction for Cash In Lieu
        /// </summary>
        /// <returns></returns>
        private static Transaction AddNewTransactionForCashInLieu()
        {
            Transaction newTranscation = new Transaction();
            try
            {
                TransactionEntry firstTransactionEntry = new TransactionEntry();
                firstTransactionEntry.TransactionEntryID = uIDGenerator.GenerateID();
                firstTransactionEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                firstTransactionEntry.EntryAccountSide = AccountSide.DR;
                firstTransactionEntry.FXConversionMethodOperator = Operator.M.ToString();
                firstTransactionEntry.TransactionSource = CashTransactionType.ManualJournalEntry;

                TransactionEntry SecondTransactionEntry = new TransactionEntry();
                SecondTransactionEntry.TransactionEntryID = uIDGenerator.GenerateID();
                SecondTransactionEntry.TaxLotState = ApplicationConstants.TaxLotState.New;
                SecondTransactionEntry.EntryAccountSide = AccountSide.CR;
                SecondTransactionEntry.FXConversionMethodOperator = Operator.M.ToString();
                SecondTransactionEntry.TransactionSource = CashTransactionType.ManualJournalEntry;

                newTranscation.TransactionID = firstTransactionEntry.TransactionEntryID;
                newTranscation.Add(firstTransactionEntry);
                newTranscation.Add(SecondTransactionEntry);

            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return newTranscation;
        }


        /// <summary>
        /// Gets the account wise fx rate.
        /// </summary>
        /// <param name="currencyId">The currency identifier.</param>
        /// <param name="effectiveDate">The effective date.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <returns></returns>
        private double GetAccountWiseFXRate(int currencyId, DateTime effectiveDate, int accountId, int auecID)
        {
            double fxRate = 0;
            try
            {
                int baseCurrencyId = CachedDataManager.GetInstance.GetCompanyBaseCurrencyID();
                if (currencyId != baseCurrencyId)
                {
                    // get previous business day
                    DateTime previousBusinessDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(effectiveDate, -1, auecID);
                    ConversionRate conversionRate = ForexConverter.GetInstance(CachedDataManager.GetInstance.GetCompanyID()).GetConversionRateFromAndToCurrenciesForAGivenDateAndAccount(currencyId, baseCurrencyId, accountId, previousBusinessDate);
                    if (conversionRate != null)
                    {
                        fxRate = conversionRate.RateValue;
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
            return fxRate;
        }

        private AllocationGroup GetAllocationGroup(TaxlotBase taxlotbase)
        {
            AllocationGroup allocGroup = null;
            try
            {
                allocGroup = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotbase);
                allocGroup.TaxLots[0].PositionTag = taxlotbase.PositionTag;
                taxlotbase.L2TaxlotID = allocGroup.TaxLots[0].TaxLotID;
                taxlotbase.GroupID = allocGroup.TaxLots[0].GroupID;
                allocGroup.SettlementCurrencyID = taxlotbase.SettlementCurrencyID;
                allocGroup.TaxLots[0].SettlementCurrencyID = taxlotbase.SettlementCurrencyID;
                AddTransactionSource(allocGroup);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return allocGroup;
        }

        private void UpdateAttributesWithdrawalTaxlot(TaxlotBase taxlotbase, CorpActionParameterHelper helper)
        {
            try
            {
                taxlotbase.CounterPartyID = helper.CounterPartyId;
                taxlotbase.VenueID = 0;

                if (taxlotbase.OpenQty != 0 && taxlotbase.AssetMultiplier != 0)
                {
                    taxlotbase.AvgPrice = taxlotbase.NotionalValue / (taxlotbase.OpenQty * taxlotbase.AssetMultiplier);
                }
                taxlotbase.OpenTotalCommissionandFees = 0;

                if (helper.OriginalSymbolCurrencyID != helper.NewSymbolCurrencyID)
                {
                    taxlotbase.SettlementCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlotbase.Level1ID);
                    taxlotbase.SettlementCurrency = CachedDataManager.GetInstance.GetCurrencyText(taxlotbase.SettlementCurrencyID);
                }

                CARulesHelper.FillText(taxlotbase);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateAttributesForSameSymbolCostAdjustment(TaxlotBase taxlotbase, CorpActionParameterHelper helper)
        {
            try
            {
                taxlotbase.CounterPartyID = helper.CounterPartyId;
                taxlotbase.VenueID = 0;

                taxlotbase.NewCompanyName = helper.OriginalSymbolCompanyName;
                taxlotbase.OldAveragePrice = taxlotbase.AvgPrice;
                taxlotbase.NotionalValue = taxlotbase.NotionalValue - (taxlotbase.NotionalValue * (helper.SpinOffRatio / 100));
                taxlotbase.NotionalValueBase = taxlotbase.NotionalValueBase - (taxlotbase.NotionalValueBase * (helper.SpinOffRatio / 100));

                if (taxlotbase.OpenQty != 0 && taxlotbase.AssetMultiplier != 0)
                {
                    taxlotbase.AvgPrice = taxlotbase.NotionalValue / (taxlotbase.OpenQty * taxlotbase.AssetMultiplier);
                }

                taxlotbase.OpenTotalCommissionandFees = 0;

                if (helper.OriginalSymbolCurrencyID != helper.NewSymbolCurrencyID)
                {
                    taxlotbase.SettlementCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlotbase.Level1ID);
                    taxlotbase.SettlementCurrency = CachedDataManager.GetInstance.GetCurrencyText(taxlotbase.SettlementCurrencyID);
                }

                CARulesHelper.FillText(taxlotbase);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void UpdateAttributesAdditionForNewSymbol(CorpActionParameterHelper helper, TaxlotBase taxlotAddition, ref List<QuantityData> taxlotQuantities, ref double sumOfTaxlotQuantities, ref double sumOfIntegralTaxlotQuantities)
        {
            try
            {
                taxlotAddition.CounterPartyID = helper.CounterPartyId;
                taxlotAddition.VenueID = 0;
                taxlotAddition.Symbol = helper.NewSymbol;
                taxlotAddition.NewCompanyName = helper.NewSymbolCompanyName;

                taxlotAddition.CurrencyID = helper.NewSymbolCurrencyID;
                taxlotAddition.Currency = CachedDataManager.GetInstance.GetCurrencyText(taxlotAddition.CurrencyID);

                if (helper.OriginalSymbolCurrencyID != helper.NewSymbolCurrencyID)
                {
                    taxlotAddition.SettlementCurrencyID = CachedDataManager.GetInstance.GetBaseCurrencyIDForAccount(taxlotAddition.Level1ID);
                    taxlotAddition.SettlementCurrency = CachedDataManager.GetInstance.GetCurrencyText(taxlotAddition.SettlementCurrencyID);
                }

                double payReceiveForNewAddition = (taxlotAddition.NotionalValue * (helper.SpinOffRatio / 100));

                taxlotAddition.NotionalValueBase = (taxlotAddition.NotionalValueBase * (helper.SpinOffRatio / 100));

                // do nothing if NewSymbolCurrencyID and OriginalSymbolCurrencyID are same
                if (helper.NewSymbolCurrencyID != helper.OriginalSymbolCurrencyID)
                {
                    if (helper.NewSymbolCurrencyID == CachedDataManager.GetInstance.GetCompanyBaseCurrencyID())
                    {
                        payReceiveForNewAddition = taxlotAddition.NotionalValueBase;
                        taxlotAddition.FXRate = 1;
                        taxlotAddition.FXConversionMethodOperator = "M";
                    }
                    else if (helper.NewSymbolFXRate != 0)
                    {
                        payReceiveForNewAddition = taxlotAddition.NotionalValueBase / helper.NewSymbolFXRate;
                        taxlotAddition.FXRate = helper.NewSymbolFXRate;
                        taxlotAddition.FXConversionMethodOperator = "M";
                    }
                    else
                    {
                        payReceiveForNewAddition = 0;
                        taxlotAddition.FXRate = 0;
                        taxlotAddition.FXConversionMethodOperator = "M";
                    }
                }

                taxlotAddition.OpenTotalCommissionandFees = 0;

                taxlotAddition.OriginalQty = taxlotAddition.OpenQty * helper.NewSharesReceivedRatio;

                taxlotAddition.OpenQty = taxlotAddition.OriginalQty;

                if (taxlotAddition.OriginalQty != 0 && taxlotAddition.AssetMultiplier != 0)
                {
                    taxlotAddition.AvgPrice = payReceiveForNewAddition / (taxlotAddition.OriginalQty * taxlotAddition.AssetMultiplier);
                }

                taxlotAddition.OldAveragePrice = (taxlotAddition.NotionalValue * (helper.SpinOffRatio / 100)) / (taxlotAddition.OriginalQty * taxlotAddition.AssetMultiplier);

                taxlotAddition.NotionalChange = payReceiveForNewAddition;
                taxlotAddition.NotionalValue = payReceiveForNewAddition;

                QuantityData qtyData = new QuantityData(taxlotAddition.GroupID, taxlotAddition.L2TaxlotID, taxlotAddition.OriginalPurchaseDate, taxlotAddition.OriginalQty, taxlotAddition.AvgPrice);
                taxlotQuantities.Add(qtyData);

                sumOfTaxlotQuantities += taxlotAddition.OriginalQty;
                sumOfIntegralTaxlotQuantities += qtyData.IntegralPart;

                CARulesHelper.FillText(taxlotAddition);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// this is used to assign transaction source i.e. origin of the transaction
        /// </summary>
        /// <param name="allocGroup"></param>
        private void AddTransactionSource(AllocationGroup allocGroup)
        {
            try
            {
                allocGroup.TransactionSource = TransactionSource.CAStockSpinoff;
                allocGroup.TransactionSourceTag = (int)TransactionSource.CAStockSpinoff;

                allocGroup.TaxLots[0].TransactionSource = TransactionSource.CAStockSpinoff;
                allocGroup.TaxLots[0].TransactionSourceTag = (int)TransactionSource.CAStockSpinoff;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TaxlotBase GenerateTaxlotBaseForAddtionOrWithdrawal(TaxlotBase taxlotBase, DataRow corporateActionRow, bool isWithDrawal)
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
                        taxlotBaseNew.TransactionType = TradingTransactionType.BuytoClose.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongWithdrawal;//"7"; //long cost adjustment
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Sell;
                        taxlotBaseNew.TransactionType = TradingTransactionType.Sell.ToString();
                    }
                }
                else
                {
                    if (taxlotBaseNew.PositionType == PositionType.Short)
                    {
                        taxlotBaseNew.PositionTag = PositionTag.ShortAddition;
                        taxlotBaseNew.TransactionType = TradingTransactionType.SellShort.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongAddition;
                        taxlotBaseNew.TransactionType = TradingTransactionType.Buy.ToString();
                    }
                }

                if (isWithDrawal)
                {
                    taxlotBaseNew.OriginalPurchaseDate = taxlotBaseNew.AUECLocalDate;
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.AUECLocalDate;
                    taxlotBaseNew.SettlementDate = taxlotBaseNew.AUECLocalDate;
                }
                else
                {
                    taxlotBaseNew.OriginalPurchaseDate = taxlotBase.OriginalPurchaseDate;
                    taxlotBaseNew.ProcessDate = taxlotBaseNew.AUECLocalDate;
                    taxlotBaseNew.SettlementDate = taxlotBaseNew.AUECLocalDate;
                }

                //CARulesHelper.FillText(taxlotBaseNew);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return taxlotBaseNew;
        }

        /// <summary>
        /// Arranges the accountwise taxlot base.
        /// </summary>
        /// <param name="taxlotList">The taxlot list.</param>
        /// <returns></returns>
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
                        taxlotBaseDict[taxlotbase.Level1ID].Add(taxlotbase);
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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

            return taxlotBaseDict;
        }

        /// <summary>
        /// //check whether taxlot is already closed or not
        /// </summary>
        /// <param name="taxlotsClosedInFutureStr"></param>
        /// <param name="taxlotsCAAppliedInFutureStr"></param>
        /// <param name="taxlotsClosedInFuture"></param>
        /// <param name="taxlotsCAAppliedInFuture"></param>
        /// <param name="effectiveDate"></param>
        /// <param name="openTaxlotsList"></param>          
        private void CheckTaxlotStatus(DateTime effectiveDate, TaxlotBaseCollection openTaxlotsList, ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr)
        {
            try
            {
                //check if CA is applied in future date
                List<string> positionalCATaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(effectiveDate);

                Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();

                StringBuilder taxlotsClosedInFuture = new StringBuilder();
                StringBuilder taxlotsCAAppliedInFuture = new StringBuilder();
                StringBuilder taxlotsBoxedPosition = new StringBuilder();

                for (int i = 0; i < openTaxlotsList.Count; i++)
                {
                    TaxlotBase taxlotBase = openTaxlotsList[i];
                    // check boxed positions
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
                            dictCheckBoxPositions[taxlotBase.Level1ID] = "Boxed";
                            taxlotsBoxedPosition.Append(taxlotBase.L2TaxlotID);
                            taxlotsBoxedPosition.Append(",");
                        }
                        else if ((taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_SellShort || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed) && existingValue.Equals(PositionType.Long.ToString()))
                        {
                            dictCheckBoxPositions[taxlotBase.Level1ID] = "Boxed";
                            taxlotsBoxedPosition.Append(taxlotBase.L2TaxlotID);
                            taxlotsBoxedPosition.Append(",");
                        }
                    }

                    if (_closingServices.CheckClosingStatus(taxlotBase.L2TaxlotID, effectiveDate))
                    {
                        if (!positionalCATaxlotId.Contains(taxlotBase.L2TaxlotID))
                        {
                            taxlotsClosedInFuture.Append(taxlotBase.L2TaxlotID);
                            taxlotsClosedInFuture.Append(",");
                        }
                    }

                    if (_closingServices.CheckCorporateActionStatus(taxlotBase.L2TaxlotID, effectiveDate) || positionalCATaxlotId.Contains(taxlotBase.L2TaxlotID))
                    {
                        taxlotsCAAppliedInFuture.Append(taxlotBase.L2TaxlotID);
                        taxlotsCAAppliedInFuture.Append(",");
                    }
                }
                taxlotsClosedInFutureStr = taxlotsClosedInFuture.ToString().TrimEnd(new char[] { ',' });
                taxlotsCAAppliedInFutureStr = taxlotsCAAppliedInFuture.ToString().TrimEnd(new char[] { ',' });
                taxlotsBoxedPositionStr = taxlotsBoxedPosition.ToString().TrimEnd(new char[] { ',' });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private Dictionary<string, Dictionary<string, List<AllocationGroup>>> _corporateActionGroupCollection = new Dictionary<string, Dictionary<string, List<AllocationGroup>>>();

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
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void ClearNonTradingTransaction()
        {
            try
            {
                if (_nonTradingCashInLieuTransactions == null)
                {
                    _nonTradingCashInLieuTransactions = new List<Transaction>();
                }
                else
                {
                    _nonTradingCashInLieuTransactions.Clear();
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
        }
        /// <summary>
        /// For Spin-off CA Saving
        /// </summary>
        /// <param name="firstCA"></param>
        /// <param name="corporateActionListString"></param>
        /// <param name="updatedTaxlots"></param>
        public bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            string resultStr = string.Empty;
            bool isSuccessful = false;
            ///Here we are not using the passed values but using an internally saved cache. this is specific to Exchange only
            try
            {
                TaxlotsCacheManager.Instance.FillCAIDWiseXML(corporateActionListString);
                List<CAOnProcessObjects> caOnProcessObjectList = new List<CAOnProcessObjects>();

                Prana.BusinessObjects.PositionManagement.ClosingData closingData_CashInLieuTransactions = null;


                foreach (KeyValuePair<string, TaxlotBaseCollection> keyValue in TaxlotsCacheManager.Instance.GetCAWiseTaxlots())
                {
                    //For Symbol Exchange CA
                    CAOnProcessObjects caOnProcessObject = new CAOnProcessObjects();

                    caOnProcessObject.CorporateActionID = new Guid(keyValue.Key);
                    DataRow dr = TaxlotsCacheManager.Instance.GetCARowByID(keyValue.Key);
                    if (dr != null)
                    {
                        caOnProcessObject.Symbol = dr[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                        caOnProcessObject.NewSymbol = dr[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                    }

                    caOnProcessObject.FromDate = DateTime.Parse(dr[CorporateActionConstants.CONST_EffectiveDate].ToString());
                    caOnProcessObject.CorporateActionListString = TaxlotsCacheManager.Instance.GetCAStrByID(keyValue.Key);

                    Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(keyValue.Key);

                    List<AllocationGroup> allGroupOrig = allocGroupDict[ApplicationConstants.CA_ORIGINAL];
                    List<AllocationGroup> allGroupWithdrawal = allocGroupDict[ApplicationConstants.CA_WITHDRAWAL];
                    List<AllocationGroup> allGroupAddition = allocGroupDict[ApplicationConstants.CA_ADDITION];

                    List<AllocationGroup> newllocGrs = new List<AllocationGroup>();
                    newllocGrs.AddRange(allGroupWithdrawal);
                    newllocGrs.AddRange(allGroupAddition);

                    ///These are taxlots generated by posttradecachemanager, hence these would be sent and saved by posttradecachemanager.
                    caOnProcessObject.NewGeneratedTaxlots = newllocGrs;

                    /// Meaning of buy and sell taxlots has been changed to positional taxlots and closing taxlots hence changed the name
                    List<TaxLot> positionalTaxlots = new List<TaxLot>();
                    List<TaxLot> closingTaxlots = new List<TaxLot>();

                    foreach (AllocationGroup GroupOrig in allGroupOrig)
                    {
                        TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(GroupOrig.TaxLots[0]);
                        taxlotToclose.Update(GroupOrig.TaxLots[0]);
                        //Modifying all dates to the effectivedate, as the original transactions are closed on the effectivedate only
                        //taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                        //taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                        //taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                        //commented by omshiv, ACA Cleanup
                        // taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;
                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        taxlotToclose.ClosingStatus = ClosingStatus.Closed;

                        positionalTaxlots.Add(taxlotToclose);
                    }

                    foreach (AllocationGroup GroupAddition in allGroupAddition)
                    {
                        if (GroupAddition.TaxLots[0].ClosingWithTaxlotID != null)
                        {
                            TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(GroupAddition.TaxLots[0]);
                            taxlotToclose.Update(GroupAddition.TaxLots[0]);
                            //Modifying all dates to the effectivedate, as the original transactions are closed on the effectivedate only
                            //taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                            //taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                            //taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                            //commented by omshiv, ACA Cleanup
                            //taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;
                            //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                            taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                            taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                            taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                            taxlotToclose.ClosingStatus = ClosingStatus.Closed;

                            positionalTaxlots.Add(taxlotToclose);
                        }
                    }

                    foreach (AllocationGroup groupWithdrawal in allGroupWithdrawal)
                    {
                        TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(groupWithdrawal.TaxLots[0]);
                        taxlotToclose.Update(groupWithdrawal.TaxLots[0]);
                        //Modifying all dates to the effectivedate, as the withdrawal transactions are closed on the effectivedate only
                        taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        taxlotToclose.ClosingStatus = ClosingStatus.Closed;

                        closingTaxlots.Add(taxlotToclose);
                    }

                    /// third parameter says whether closing based on Notional change or normal closing
                    caOnProcessObject.ClosingData = _closingServices.CloseDataforPairedTaxlots(positionalTaxlots, closingTaxlots, false);
                    // Set Closing Status Closed for those closed with the CA
                    SetClosingStatus(caOnProcessObject.ClosingData);
                    caOnProcessObjectList.Add(caOnProcessObject);
                }

                isSuccessful = DBManager.SaveCASpinoff(caOnProcessObjectList, _allocationServices, _closingServices, userID, closingData_CashInLieuTransactions, _cashManagementService, _nonTradingCashInLieuTransactions);
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
            return isSuccessful;
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
                ClearNonTradingTransaction();

                taxlots = AllocationDataManager.GetOpenPositionforUndoPreview(caIDsStr);
                _nonTradingCashInLieuTransactions = _cashManagementService.GetTransactionsByCAID(caIDsStr);
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
                    isSuccessful = DBManager.UndoCASpinOff(requestObject, _closingServices, _allocationServices, _cashManagementService, _nonTradingCashInLieuTransactions);
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

        private void RemoveGroups(string corpActionID)
        {
            lock (_corporateActionGroupCollection)
            {
                if (_corporateActionGroupCollection.ContainsKey(corpActionID))
                {
                    _corporateActionGroupCollection.Remove(corpActionID);
                }
            }
        }

        #endregion


        public bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots, bool isSMModificationRequired)
        {
            return false;
        }
    }
}
