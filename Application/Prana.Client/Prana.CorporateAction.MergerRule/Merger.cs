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

namespace Prana.CorporateAction.MergerRule
{
    public class Merger : ICorporateActionBaseRule
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
                        //row.SetColumnError(columns[CorporateActionConstants.CONST_NewCompanyName], "New Symbol and New Company both can not be empty at the same time.");
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
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_OrigSecQtyRatio].ToString()).Equals(0))
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Qty Ratio cannot be 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Qty Ratio can not be 0.");
                        continue;
                    }
                    if (double.TryParse(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString()).Equals(0))
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Qty Ratio can not be 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Qty Ratio can not be 0.");
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
        public CAPreviewResult PreviewCorporateActions(DataRowCollection caRows, ref TaxlotBaseCollection modifiedTaxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId)
        {
            //This will first remove any entry for the corp action...
            ClearCorporateActionGroups();

            CAPreviewResult caPreviewResult = new CAPreviewResult();
            modifiedTaxlotList = new TaxlotBaseCollection();

            string taxlotsClosedInFutureStr = string.Empty;
            string taxlotsCAAppliedInFutureStr = string.Empty;
            string taxlotsBoxedPositionStr = string.Empty;
            // TODO: When there are no taxlots for a symbol then a return message should go to UI(Obviously in case of Multiple CA's).
            try
            {
                TaxlotsCacheManager.Instance.ClearAll();

                foreach (DataRow corporateActionRow in caRows)
                {
                    TaxlotBaseCollection caModifiedTaxlots = null;
                    string closedTaxlotStr = string.Empty;
                    string origSymbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                    string newSymbol = corporateActionRow[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                    string newCompanyName = corporateActionRow[CorporateActionConstants.CONST_NewCompanyName].ToString();
                    string caID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();

                    GetModifiedTaxlotForMerger(corporateActionRow, ref caModifiedTaxlots, ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, commaSeparatedAccountIds, caPref);

                    if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                    {
                        /// Taxlots are not closed and CA not applied in future hence ready for applying new ca
                        TaxlotsCacheManager.Instance.AddCARow(caID, corporateActionRow);
                        TaxlotsCacheManager.Instance.AddTaxlots(caID, caModifiedTaxlots);
                    }

                    if (caModifiedTaxlots != null && caModifiedTaxlots.Count < 1)
                    {
                        modifiedTaxlotList.Clear();
                        caPreviewResult.NoPositionSymbols = origSymbol;
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

        private void GetModifiedTaxlotForMerger(DataRow corporateActionRow, ref TaxlotBaseCollection caModifiedTaxlots, ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, string commaSeparatedAccountIds, CAPreferences caPref)
        {
            try
            {
                //To roundoff quantity in Corporate Action as fractional shares generated.
                int quantityToRoundoff = int.Parse(ConfigurationManager.AppSettings[CorporateActionConstants.CONST_CAQuantityToRoundOff]);

                caModifiedTaxlots = new TaxlotBaseCollection();
                StringBuilder taxlotsClosedInFuture = new StringBuilder();
                StringBuilder taxlotsCAAppliedInFuture = new StringBuilder();
                StringBuilder taxlotsBoxedPosition = new StringBuilder();

                DateTime date = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                string caID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();
                string origSymbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                string newSymbol = corporateActionRow[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                string origCompanyName = corporateActionRow[CorporateActionConstants.CONST_CompanyName].ToString();
                string newCompanyName = corporateActionRow[CorporateActionConstants.CONST_NewCompanyName].ToString();

                double newSharesReceivedRatio = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_Factor]);

                int cashinLieuValue = Convert.ToInt32(corporateActionRow[CorporateActionConstants.CONST_Cashinlieu].ToString());

                double closingPrice = 0;

                bool isCashinLieuRequired = false;

                bool adjustCashinLieuatAccountLevel = caPref.AdjustFractionalSharesAtAccountPositionLevel;

                if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.CloseAtGivenPrice))
                {
                    closingPrice = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_ClosingPrice]);
                    isCashinLieuRequired = true;
                }
                else if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.CloseAtMarkPrice))
                {
                    closingPrice = 0;
                    // get previous business day
                    DateTime previousBusinessDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(date, -1, 1);

                    // set markprice
                    try
                    {
                        closingPrice = _pricingServicesProxy.InnerChannel.GetMarkPriceForDateAndSymbol(previousBusinessDate, origSymbol) / newSharesReceivedRatio;
                    }
                    catch
                    {
                    }
                    isCashinLieuRequired = true;
                }
                else if (cashinLieuValue.Equals((int)ApplicationConstants.CashInLieu.NoCashInLieu))
                {
                    isCashinLieuRequired = false;
                }

                //Add the UTC Effective Date to the function
                TaxlotBaseCollection taxlotList = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, origSymbol, commaSeparatedAccountIds);
                int openTaxlotCount = taxlotList.Count;

                List<string> positionalMergerTaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(date);
                //check whether taxlot is already closed or not
                CheckTaxlotStatus(ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, taxlotsClosedInFuture, taxlotsCAAppliedInFuture, taxlotsBoxedPosition, date, taxlotList, openTaxlotCount, positionalMergerTaxlotId);

                if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                {
                    // Arrange accountwise taxlots
                    Dictionary<int, List<TaxlotBase>> AccountwisetaxlotBaseDict = ArrangeAccountwiseTaxlotBase(taxlotList);

                    foreach (KeyValuePair<int, List<TaxlotBase>> kvp in AccountwisetaxlotBaseDict)
                    {
                        Dictionary<string, TaxlotBase> groupwiseTaxlotBaseDict = new Dictionary<string, TaxlotBase>();

                        decimal accountTotalQty = 0;

                        List<TaxlotBase> taxlotBaseColl = kvp.Value;

                        int accountTaxlotCount = taxlotBaseColl.Count;

                        for (int i = 0; i < accountTaxlotCount; i++)
                        {
                            TaxlotBase taxlotBase = taxlotBaseColl[i];
                            taxlotBase.CorpActionID = caID;

                            taxlotBase.NewCompanyName = origCompanyName;

                            TaxlotBase taxlotOriginal = taxlotBase.Clone();
                            CARulesHelper.FillText(taxlotOriginal);
                            taxlotOriginal.OldAveragePrice = taxlotOriginal.AvgPrice;
                            double payReceiveOriginal = 0.0;
                            double originalNotionalValue = taxlotOriginal.OpenQty * taxlotOriginal.AvgPrice * taxlotOriginal.AssetMultiplier;

                            if (caPref.UseNetNotional)
                            {
                                payReceiveOriginal = originalNotionalValue + (taxlotOriginal.OpenTotalCommissionandFees * GetSideMultiplier(taxlotOriginal.PositionType));

                                taxlotOriginal.OpenTotalCommissionandFees = 0;
                            }
                            else
                            {
                                payReceiveOriginal = originalNotionalValue;
                            }

                            taxlotOriginal.NotionalValue = payReceiveOriginal;

                            if (payReceiveOriginal != 0 && taxlotOriginal.AssetMultiplier != 0)
                                taxlotOriginal.AvgPrice = payReceiveOriginal / (taxlotOriginal.OpenQty * taxlotOriginal.AssetMultiplier);

                            AllocationGroup allGroupOrig = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotOriginal);
                            //Overriding the new generated taxlotid from the original taxlotid. Since we have to use this generated taxlot with
                            //some changes as the withdrawal taxlot, hence need to generate it from allocation module.
                            allGroupOrig.GroupID = taxlotOriginal.GroupID;
                            allGroupOrig.TaxLots[0].GroupID = taxlotOriginal.GroupID;
                            allGroupOrig.TaxLots[0].TaxLotID = taxlotOriginal.L2TaxlotID;

                            ///Withdrawal for original symbol                                                                                   
                            TaxlotBase taxlotWithdrawal = GenerateNewTaxlotBaseAndAllocationGroup(taxlotBase, corporateActionRow, true);

                            taxlotWithdrawal.CounterPartyID = 0;
                            taxlotWithdrawal.VenueID = 0;

                            double withdrawlNotionalChange = originalNotionalValue;

                            taxlotWithdrawal.OldAveragePrice = withdrawlNotionalChange / (taxlotWithdrawal.OpenQty * taxlotWithdrawal.AssetMultiplier);

                            double openTotalCommissionandFeesWithdrawal = taxlotBase.OpenTotalCommissionandFees;
                            double payReceiveWithdrawal = 0.0;

                            // If  UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Acoordingly
                            if (caPref.UseNetNotional)
                            {
                                payReceiveWithdrawal = withdrawlNotionalChange + (openTotalCommissionandFeesWithdrawal * GetSideMultiplier(taxlotWithdrawal.PositionType));

                                taxlotWithdrawal.OpenTotalCommissionandFees = 0;
                            }
                            else
                            {
                                payReceiveWithdrawal = withdrawlNotionalChange;
                                //commission is negative for taxlotwithdrawal so the cash impact should be 0.
                                //JiraLink: - http://jira.nirvanasolutions.com:8080/browse/PRANA-5072
                                taxlotWithdrawal.OpenTotalCommissionandFees = (openTotalCommissionandFeesWithdrawal * (-1));
                            }
                            taxlotWithdrawal.NotionalValue = payReceiveWithdrawal;

                            if (payReceiveWithdrawal != 0 && taxlotWithdrawal.AssetMultiplier != 0)
                                taxlotWithdrawal.AvgPrice = payReceiveWithdrawal / (taxlotWithdrawal.OpenQty * taxlotWithdrawal.AssetMultiplier);

                            AllocationGroup allGroupWithdrawal = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotWithdrawal);
                            taxlotWithdrawal.L2TaxlotID = allGroupWithdrawal.TaxLots[0].TaxLotID;
                            taxlotWithdrawal.GroupID = allGroupWithdrawal.TaxLots[0].GroupID;
                            //allGroupWithdrawal.TransactionType = TradingTransactionType.StockMG.ToString();
                            AddTransactionSource(allGroupWithdrawal);

                            // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose i.e. pair closing
                            allGroupOrig.TaxLots[0].ClosingWithTaxlotID = taxlotWithdrawal.L2TaxlotID;
                            // add original taxlot in the collection
                            AddGroup(allGroupOrig, ApplicationConstants.CA_ORIGINAL);
                            caModifiedTaxlots.Add(taxlotOriginal);
                            // add withdrawal taxlot in the collection
                            AddGroup(allGroupWithdrawal, ApplicationConstants.CA_WITHDRAWAL);
                            caModifiedTaxlots.Add(taxlotWithdrawal);

                            /// Addition for new symbol                          
                            TaxlotBase taxlotAddition = GenerateNewTaxlotBaseAndAllocationGroup(taxlotBase, corporateActionRow, false);
                            taxlotAddition.CounterPartyID = 0;
                            taxlotAddition.VenueID = 0;
                            taxlotAddition.Symbol = newSymbol;
                            taxlotAddition.NewCompanyName = newCompanyName;

                            double newQty = taxlotAddition.OpenQty * newSharesReceivedRatio;
                            // round off is set as fractional shares go to many decimal places. So fractional shares should be roundoff as per the server app.config entry
                            taxlotAddition.OpenQty = Math.Round(newQty, quantityToRoundoff);
                            taxlotAddition.AllocatedQty = taxlotAddition.OpenQty;
                            taxlotAddition.OldAveragePrice = taxlotAddition.AvgPrice;
                            double payReceiveAddition = 0.0;
                            double openTotalCommissionandFees = taxlotAddition.OpenTotalCommissionandFees;

                            // If  UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Accordingly
                            if (caPref.UseNetNotional)
                            {
                                payReceiveAddition = originalNotionalValue + (openTotalCommissionandFees * GetSideMultiplier(taxlotAddition.PositionType));

                                taxlotAddition.OpenTotalCommissionandFees = 0;
                            }
                            else
                            {
                                payReceiveAddition = originalNotionalValue;
                            }

                            taxlotAddition.OldAveragePrice = originalNotionalValue / (newQty * taxlotAddition.AssetMultiplier);
                            if (payReceiveAddition != 0 && taxlotAddition.AssetMultiplier != 0)
                                taxlotAddition.AvgPrice = payReceiveAddition / (newQty * taxlotAddition.AssetMultiplier);

                            taxlotAddition.NotionalValue = payReceiveAddition;
                            AllocationGroup allGroupAddition = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotAddition);
                            taxlotAddition.L2TaxlotID = allGroupAddition.TaxLots[0].TaxLotID;
                            taxlotAddition.GroupID = allGroupAddition.TaxLots[0].GroupID;
                            //allGroupAddition.TransactionType = TradingTransactionType.StockMG.ToString();
                            AddTransactionSource(allGroupAddition);

                            AddGroup(allGroupAddition, ApplicationConstants.CA_ADDITION);
                            caModifiedTaxlots.Add(taxlotAddition);

                            decimal taxlotQty = 0;
                            decimal integralQty = 0;
                            decimal residualQty = 0;
                            // adjust fractional shares with cash in lieu
                            if (isCashinLieuRequired && !adjustCashinLieuatAccountLevel)
                            {

                                // convert qty value in to decimal for best Precision
                                taxlotQty = Convert.ToDecimal(allGroupAddition.TaxLots[0].TaxLotQty);
                                // integer part of quantity 
                                integralQty = Math.Truncate(taxlotQty);
                                // decimal part if quantity is fractional
                                residualQty = (taxlotQty - integralQty);
                                // if residual qty > 0                           
                                if (residualQty > 0)
                                {
                                    // Generate taxlots if Cash in lieu required
                                    GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotAddition, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);

                                    AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);
                                }
                            }
                            else if (isCashinLieuRequired && adjustCashinLieuatAccountLevel)
                            {
                                accountTotalQty = accountTotalQty + Convert.ToDecimal(taxlotAddition.OpenQty);

                                if (!groupwiseTaxlotBaseDict.ContainsKey(allGroupAddition.GroupID))
                                {
                                    groupwiseTaxlotBaseDict.Add(allGroupAddition.GroupID, taxlotAddition);
                                }

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
                                        //AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);

                                        //AllocationGroup searchedAllocGroup = new AllocationGroup();

                                        //TaxlotBase searchedtaxlotBase = new TaxlotBase();

                                        //decimal searchedtaxlotQty = 0;

                                        //if (residualQty > taxlotQty)
                                        //{
                                        //    // here search and send Allocation Group and TaxlotBase to this method
                                        //    Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(caID);
                                        //    List<AllocationGroup> allocationGroupAddition = allocGroupDict[ApplicationConstants.CA_ADDITION];

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

                                        //    if (searchedtaxlotBase.OpenQty > 0)
                                        //    {
                                        //        // Generate taxlots if Cash in lieu required
                                        //        GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, searchedtaxlotBase, residualQty, ref searchedAllocGroup);
                                        //        AddGroup(searchedAllocGroup, ApplicationConstants.CA_FRACTIONALADDITION);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    // Generate taxlots if Cash in lieu required
                                        //    GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotAddition, residualQty, ref allGroupAddition);
                                        //    AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);
                                        //}
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    caModifiedTaxlots = null;
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
        /// this is used to assign transaction source i.e. origin of the transaction
        /// </summary>
        /// <param name="allocGroup"></param>
        private void AddTransactionSource(AllocationGroup allocGroup)
        {
            try
            {
                allocGroup.TransactionSource = TransactionSource.CAStockMerger;
                allocGroup.TransactionSourceTag = (int)TransactionSource.CAStockMerger;
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
                        taxlotBaseNew.PositionTag = PositionTag.ShortWithdrawal;// "7" short withdrawal 
                        taxlotBaseNew.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;
                        taxlotBaseNew.TransactionType = TradingTransactionType.ShortWithdrawal.ToString();
                    }
                    else
                    {
                        taxlotBaseNew.PositionTag = PositionTag.LongWithdrawal;//"5" long withdrawal
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
                    taxlotBaseNew.OriginalPurchaseDate = taxlotBase.OriginalPurchaseDate;
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
        /// Generate taxlots if Cash in lieu required
        /// </summary>
        /// <param name="corporateActionRow"></param>
        /// <param name="caModifiedTaxlots"></param>   
        /// <param name="splitFactor"></param>
        /// <param name="closingPrice"></param>
        /// <param name="taxlotBase"></param>
        /// <param name="residualQty"></param>
        private void GenerateCashInLieuTaxlots(DataRow corporateActionRow, TaxlotBaseCollection caModifiedTaxlots, double closingPrice, TaxlotBase taxlotBase, decimal residualQty, ref AllocationGroup newGroupAddition, bool adjustCashinLieuatAccountLevel)
        {
            try
            {
                /// Withdrawal of fractional share for new symbol
                TaxlotBase taxlotFractionalShareWithdrawal = taxlotBase.Clone();

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
                CARulesHelper.FillDateInfo(taxlotFractionalShareWithdrawal, corporateActionRow);

                taxlotFractionalShareWithdrawal.ProcessDate = taxlotFractionalShareWithdrawal.AUECLocalDate;
                taxlotFractionalShareWithdrawal.OriginalPurchaseDate = taxlotFractionalShareWithdrawal.AUECLocalDate; //taxlotBase.OriginalPurchaseDate;

                taxlotFractionalShareWithdrawal.AvgPrice = closingPrice;
                taxlotFractionalShareWithdrawal.OldAveragePrice = closingPrice;
                if (taxlotBase.OpenQty > 0)
                    taxlotFractionalShareWithdrawal.OpenTotalCommissionandFees = 0;
                taxlotFractionalShareWithdrawal.OpenQty = Convert.ToDouble(residualQty);
                taxlotFractionalShareWithdrawal.AllocatedQty = taxlotFractionalShareWithdrawal.OpenQty;
                taxlotFractionalShareWithdrawal.NotionalValue = taxlotFractionalShareWithdrawal.OpenQty * closingPrice;

                AllocationGroup allGroupFractionalShareWithdrawal = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotFractionalShareWithdrawal);

                taxlotFractionalShareWithdrawal.L2TaxlotID = allGroupFractionalShareWithdrawal.TaxLots[0].TaxLotID;
                taxlotFractionalShareWithdrawal.GroupID = allGroupFractionalShareWithdrawal.TaxLots[0].GroupID;
                taxlotFractionalShareWithdrawal.CounterPartyID = 0;
                taxlotFractionalShareWithdrawal.VenueID = 0;
                //allGroupFractionalShareWithdrawal.TransactionType = TradingTransactionType.StockMG.ToString();
                AddTransactionSource(allGroupFractionalShareWithdrawal);

                // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                // This is valid only if Fractional Share adjustment in Cash In Lieu is at Taxlot Level
                // At Position Level Fractional share will not be closed, it will be opened and manually closed
                if (!adjustCashinLieuatAccountLevel)
                    newGroupAddition.TaxLots[0].ClosingWithTaxlotID = taxlotFractionalShareWithdrawal.L2TaxlotID;

                AddGroup(allGroupFractionalShareWithdrawal, ApplicationConstants.CA_WITHDRAWAL);
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

        /// <summary>
        /// //check whether taxlot is already closed ot not
        /// </summary>
        /// <param name="taxlotsClosedInFutureStr"></param>
        /// <param name="taxlotsCAAppliedInFutureStr"></param>
        /// <param name="taxlotsClosedInFuture"></param>
        /// <param name="taxlotsCAAppliedInFuture"></param>
        /// <param name="date"></param>
        /// <param name="taxlotList"></param>
        /// <param name="count"></param>
        /// <param name="positionalMergerTaxlotId"></param>
        private void CheckTaxlotStatus(ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, StringBuilder taxlotsClosedInFuture, StringBuilder taxlotsCAAppliedInFuture, StringBuilder taxlotsBoxedPosition, DateTime date, TaxlotBaseCollection taxlotList, int count, List<string> positionalMergerTaxlotId)
        {
            try
            {
                Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();

                for (int i = 0; i < count; i++)
                {
                    TaxlotBase taxlotBase = taxlotList[i];
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
                            dictCheckBoxPositions[taxlotBase.Level1ID] = "Both";
                            taxlotsBoxedPosition.Append(taxlotBase.L2TaxlotID);
                            taxlotsBoxedPosition.Append(",");
                        }
                        else if ((taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_SellShort || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Sell_Closed) && existingValue.Equals(PositionType.Long.ToString()))
                        {
                            dictCheckBoxPositions[taxlotBase.Level1ID] = "Both";
                            taxlotsBoxedPosition.Append(taxlotBase.L2TaxlotID);
                            taxlotsBoxedPosition.Append(",");
                        }
                    }

                    if (_closingServices.CheckClosingStatus(taxlotBase.L2TaxlotID, date))
                    {
                        if (!positionalMergerTaxlotId.Contains(taxlotBase.L2TaxlotID))
                        {
                            taxlotsClosedInFuture.Append(taxlotBase.L2TaxlotID);
                            taxlotsClosedInFuture.Append(",");
                        }
                    }

                    if (_closingServices.CheckCorporateActionStatus(taxlotBase.L2TaxlotID, date) || positionalMergerTaxlotId.Contains(taxlotBase.L2TaxlotID))
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
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private readonly Dictionary<string, Dictionary<string, List<AllocationGroup>>> _corporateActionGroupCollection = new Dictionary<string, Dictionary<string, List<AllocationGroup>>>();
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
        /// For Splits CA Saving
        /// </summary>
        /// <param name="firstCA"></param>
        /// <param name="corporateActionListString"></param>
        /// <param name="updatedTaxlots"></param>
        public bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            // ApplicationConstants.CA_ORIGINAL is used only for closing
            // ApplicationConstants.CA_WITHDRAWAL collection is used to close the taxlots these should be saved in the database as these are new generated
            // ApplicationConstants.CA_ADDITION collection as these are new generated, so only save in the db
            // ApplicationConstants.CA_FRACTIONALADDITION is used only for closing

            string resultStr = string.Empty;
            bool isSuccessful = false;
            ///Here we are not using the passed values but using an internally saved cache. this is specific to Merger only

            try
            {
                TaxlotsCacheManager.Instance.FillCAIDWiseXML(corporateActionListString);
                List<CAOnProcessObjects> caOnProcessObjectList = new List<CAOnProcessObjects>();
                foreach (KeyValuePair<string, TaxlotBaseCollection> keyValue in TaxlotsCacheManager.Instance.GetCAWiseTaxlots())
                {
                    //For Symbol and Company Name Change CA
                    CAOnProcessObjects caOnProcessObject = new CAOnProcessObjects();

                    caOnProcessObject.CorporateActionID = new Guid(keyValue.Key);
                    DataRow dr = TaxlotsCacheManager.Instance.GetCARowByID(keyValue.Key);
                    if (dr != null)
                    {
                        caOnProcessObject.Symbol = dr[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                        caOnProcessObject.NewSymbol = dr[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                        caOnProcessObject.FromDate = DateTime.Parse(dr[CorporateActionConstants.CONST_EffectiveDate].ToString());
                    }
                    caOnProcessObject.CorporateActionListString = TaxlotsCacheManager.Instance.GetCAStrByID(keyValue.Key);

                    Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(keyValue.Key);

                    List<AllocationGroup> allGroupOrig = allocGroupDict[ApplicationConstants.CA_ORIGINAL];
                    List<AllocationGroup> allGroupWithdrawal = allocGroupDict[ApplicationConstants.CA_WITHDRAWAL];
                    List<AllocationGroup> allGroupAddition = allocGroupDict[ApplicationConstants.CA_ADDITION];

                    // fractional shares addition
                    List<AllocationGroup> allGroupFractionalSharesAddition = new List<AllocationGroup>();
                    if (allocGroupDict.ContainsKey(ApplicationConstants.CA_FRACTIONALADDITION))
                    {
                        allGroupFractionalSharesAddition = allocGroupDict[ApplicationConstants.CA_FRACTIONALADDITION];
                    }

                    // set notional change to zero
                    //SetNotionalChangeToZero(allGroupOrig, allGroupWithdrawal, allGroupAddition, allGroupFractionalSharesAddition);

                    List<AllocationGroup> newllocGrs = new List<AllocationGroup>();
                    newllocGrs.AddRange(allGroupWithdrawal);
                    newllocGrs.AddRange(allGroupAddition);

                    ///These taxlots are generated by post Trade Cache Manager, hence these would be sent and saved by posttradecachemanager.
                    caOnProcessObject.NewGeneratedTaxlots = newllocGrs;

                    /// Meaning of buy and sell taxlots has been changed to positional taxlots and closing taxlots hence changed the name
                    List<TaxLot> positionalTaxlots = new List<TaxLot>();
                    List<TaxLot> closingTaxlots = new List<TaxLot>();

                    foreach (AllocationGroup GroupOrig in allGroupOrig)
                    {
                        TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(GroupOrig.TaxLots[0]);
                        taxlotToclose.Update(GroupOrig.TaxLots[0]);
                        //Modifying all dates to the effectivedate, as the original transactions are closed on the effectivedate only
                        taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                        //commented by omshiv, ACA Cleanup
                        //taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        positionalTaxlots.Add(taxlotToclose);
                    }
                    //fractional shares (addition) handling
                    foreach (AllocationGroup GroupOrig in allGroupFractionalSharesAddition)
                    {
                        TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(GroupOrig.TaxLots[0]);
                        taxlotToclose.Update(GroupOrig.TaxLots[0]);
                        //Modifying all dates to the effectivedate, as the original transactions are closed on the effectivedate only
                        taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                        //taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        positionalTaxlots.Add(taxlotToclose);
                    }

                    foreach (AllocationGroup GroupWithdrawal in allGroupWithdrawal)
                    {
                        TaxLot taxlotToclose = DeepCopyHelper.Clone<TaxLot>(GroupWithdrawal.TaxLots[0]);
                        taxlotToclose.Update(GroupWithdrawal.TaxLots[0]);
                        //Modifying all dates to the effectivedate, as the withdrawal transactions are closed on the effectivedate only
                        taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                        taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        // taxlotToclose.ClosingMode = ClosingMode.CorporateAction;
                        // ClosingTaxlots.Add(taxlotToclose);

                        closingTaxlots.Add(taxlotToclose);
                    }

                    /// third parameter says whether closing based on Notional change or normal closing
                    caOnProcessObject.ClosingData = _closingServices.CloseDataforPairedTaxlots(positionalTaxlots, closingTaxlots, false);
                    // set closing status of every taxlot
                    SetClosingStatus(caOnProcessObject.ClosingData);
                    caOnProcessObjectList.Add(caOnProcessObject);
                }

                isSuccessful = DBManager.SaveCAMerger(caOnProcessObjectList, _allocationServices, _closingServices, userID);
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
                    isSuccessful = DBManager.UndoNameChange(requestObject, _closingServices, _allocationServices);

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
