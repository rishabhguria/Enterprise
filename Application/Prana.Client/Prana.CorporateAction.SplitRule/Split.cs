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

namespace Prana.CorporateAction.SplitRule
{
    public class Split : ICorporateActionBaseRule
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
        //public event PreviewCAResponseHandler PreviewCAResponse;
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
                    if (double.TryParse(row[CorporateActionConstants.CONST_OrigSecQtyRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_OrigSecQtyRatio].ToString()).Equals(0))
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Ratio cannot be 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSecQtyRatio], "Orignal Security Ratio can not be 0.");
                        continue;
                    }
                    if (double.TryParse(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_NewSecQtyRatio].ToString()).Equals(0))
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Ratio can not be 0.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSecQtyRatio], "New Security Ratio can not be 0.");
                        continue;
                    }
                    if (double.TryParse(row[CorporateActionConstants.CONST_Factor].ToString(), out tryParseResultDouble))
                    {
                        if (Convert.ToDouble(row[CorporateActionConstants.CONST_Factor].ToString()).Equals(1))
                        {
                            row.SetColumnError(columns[CorporateActionConstants.CONST_Factor], "Factor should not be 1.");
                            continue;
                        }
                    }
                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_Factor], "Factor should not be 1.");
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

        //A variable to check if Cash in Lieu required
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
                    DateTime date = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                    string caID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();

                    //Add the UTC Effective Date to the function
                    caModifiedTaxlots = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, symbol, commaSeparatedAccountIds);

                    double splitFactor = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_Factor]);

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
                        DateTime previousBusinessDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(date, -1, 1);
                        // set markprice
                        try
                        {
                            closingPrice = _pricingServicesProxy.InnerChannel.GetMarkPriceForDateAndSymbol(previousBusinessDate, symbol) / splitFactor;
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

                    int count = caModifiedTaxlots.Count;

                    List<string> positionalNameChangeTaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(date);

                    Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();

                    CheckTaxlotStatus(ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, taxlotsClosedInFuture, taxlotsCAAppliedInFuture, taxlotsBoxedPosition, date, caModifiedTaxlots, count, positionalNameChangeTaxlotId);

                    if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                    {
                        // Arrange taxlots accountwise
                        Dictionary<int, List<TaxlotBase>> AccountwisetaxlotBaseDict = ArrangeAccountwiseTaxlotBase(caModifiedTaxlots);

                        // A variable that keeps the count of all the caModifiedTaxlots in the looping
                        int totalCount = 0;

                        foreach (KeyValuePair<int, List<TaxlotBase>> kvp in AccountwisetaxlotBaseDict)
                        {
                            Dictionary<string, TaxlotBase> groupwiseTaxlotBaseDict = new Dictionary<string, TaxlotBase>();

                            decimal accountTotalQty = 0;

                            List<TaxlotBase> taxlotBaseColl = kvp.Value;
                            int accountTaxlotCount = taxlotBaseColl.Count;

                            for (int i = 0; i < accountTaxlotCount; i++)
                            {
                                TaxlotBase taxlotBaseAccountWise = taxlotBaseColl[i];

                                double originalNotionalValue = (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier * taxlotBaseAccountWise.AvgPrice);

                                double payReceiveOriginal = 0.0;
                                double taxLotOpenQty = taxlotBaseAccountWise.OpenQty;

                                // decimal qtyAfterSplit = Convert.ToDecimal(taxLotOpenQty * splitFactor);
                                // round off is set as fractional shares go to many decimal places. So fractional shares should be roundoff as per the server app.config entry
                                decimal qtyAfterSplit = Math.Round(Convert.ToDecimal(taxLotOpenQty * splitFactor), quantityToRoundoff);

                                taxlotBaseAccountWise.OldAveragePrice = taxlotBaseAccountWise.AvgPrice;
                                // Ankit Gupta on May 12, 2014:
                                // If UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Acoordingly
                                if (caPref.UseNetNotional)
                                {
                                    payReceiveOriginal = originalNotionalValue + (taxlotBaseAccountWise.OpenTotalCommissionandFees * GetSideMultiplier(taxlotBaseAccountWise.PositionType));
                                    taxlotBaseAccountWise.OpenTotalCommissionandFees = 0;
                                }
                                else
                                {
                                    payReceiveOriginal = originalNotionalValue;
                                }

                                taxlotBaseAccountWise.NotionalValue = payReceiveOriginal;
                                double avgPrice = 0;

                                if (payReceiveOriginal != 0 && taxlotBaseAccountWise.OpenQty != 0 && taxlotBaseAccountWise.AssetMultiplier != 0)
                                {
                                    avgPrice = payReceiveOriginal / (taxlotBaseAccountWise.OpenQty * taxlotBaseAccountWise.AssetMultiplier);
                                }

                                double avgPriceAfterSplit = avgPrice / splitFactor;

                                taxlotBaseAccountWise.NewTaxlotOpenQty = Convert.ToString(qtyAfterSplit);
                                taxlotBaseAccountWise.NewAvgPrice = Convert.ToString(avgPriceAfterSplit);
                                //taxlotBaseAccountWise.AvgPrice = avgPrice;
                                taxlotBaseAccountWise.AvgPrice = avgPriceAfterSplit;

                                taxlotBaseAccountWise.CorpActionID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();

                                taxlotBaseAccountWise.UTCDate = corporateActionRow[CorporateActionConstants.CONST_EffectiveDate].ToString();
                                taxlotBaseAccountWise.AUECDate = taxlotBaseAccountWise.UTCDate;

                                // taxlotQty: Qty of the taxlot
                                decimal taxlotQty = 0;
                                // integralQty: Integral part of taxlotQty 
                                decimal integralQty = 0;
                                // integralQty: Fractional part of taxlotQty 
                                decimal residualQty = 0;

                                AllocationGroup allGroupAddition = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotBaseAccountWise);
                                allGroupAddition.TaxLots[0].TaxLotQty = Convert.ToDouble(qtyAfterSplit);
                                //allGroupAddition.TaxLots[0].AvgPrice = avgPrice;
                                allGroupAddition.TaxLots[0].AvgPrice = avgPriceAfterSplit;
                                allGroupAddition.TaxLots[0].TaxLotID = taxlotBaseAccountWise.L2TaxlotID;

                                allGroupAddition.TaxLots[0].GroupID = taxlotBaseAccountWise.GroupID;
                                // Added for the Cash in Lieu at Account level when the residualQty is greater than the TaxlotQty to be closed with
                                AddGroup(allGroupAddition, ApplicationConstants.CA_ADDITION);

                                // adjust fractional shares with cash in lieu
                                if (_isCashinLieuRequired && !adjustCashinLieuatAccountLevel)
                                {
                                    taxlotQty = qtyAfterSplit;
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
                                    accountTotalQty = accountTotalQty + Convert.ToDecimal(taxlotBaseAccountWise.NewTaxlotOpenQty);

                                    if (!groupwiseTaxlotBaseDict.ContainsKey(allGroupAddition.GroupID))
                                    {
                                        groupwiseTaxlotBaseDict.Add(allGroupAddition.GroupID, taxlotBaseAccountWise);
                                    }

                                    // Cash in Lieu at Account Level: Try to close data with the last taxlot
                                    if (accountTaxlotCount.Equals(i + 1))
                                    {
                                        //taxlotQty = Convert.ToDecimal(allGroupAddition.TaxLots[0].TaxLotQty);
                                        // integer part of total account quantity 
                                        integralQty = Math.Truncate(accountTotalQty);
                                        // decimal part if quantity is fractional
                                        residualQty = (accountTotalQty - integralQty);
                                        // if residual qty > 0                           
                                        if (residualQty > 0)
                                        {
                                            // Generate taxlots if Cash in lieu required
                                            GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotBaseAccountWise, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);

                                            //TaxlotBase taxlotWithClosing = new TaxlotBase();
                                            //// generate new taxlot with residual qty 
                                            //// example say new generated taxlot qty is 1.5 then this condition will work.
                                            //// If new generated taxlot qty is 0.5 (less than one i.e. fractional) then no need to adjust, just generate Withdrawal taxlot                                                                                          
                                            //AllocationGroup searchedAllocGroup = new AllocationGroup();
                                            //TaxlotBase searchedtaxlotBase = new TaxlotBase();
                                            //decimal searchedtaxlotQty = 0;
                                            ////if residual qty is greater that the last laxlot's Qty then search in the collection
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
                                            //    // TODO: Display appropriate message
                                            //    else
                                            //    {
                                            //        caPreviewResult.ErrorMessage = " Cant apply CA: Each taxlot Qty is less than the Total Fractional Quantity.";
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    // Generate taxlots if Cash in lieu required
                                            //    GenerateCashInLieuTaxlots(corporateActionRow, caModifiedTaxlots, closingPrice, taxlotBaseAccountWise, residualQty, ref allGroupAddition, adjustCashinLieuatAccountLevel);
                                            //    AddGroup(allGroupAddition, ApplicationConstants.CA_FRACTIONALADDITION);
                                            //}
                                        }
                                    }
                                }
                                CARulesHelper.FillText(caModifiedTaxlots[totalCount]);
                                totalCount++;
                            }
                        }
                    }
                    TaxlotsCacheManager.Instance.AddCARow(caID, corporateActionRow);
                    TaxlotsCacheManager.Instance.AddTaxlots(caID, caModifiedTaxlots);
                    if (caModifiedTaxlots.Count < 1)
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
                //taxlotFractionalShareWithdrawal.Symbol = newSymbol;

                if (taxlotFractionalShareWithdrawal.PositionType == PositionType.Short)
                {
                    taxlotFractionalShareWithdrawal.PositionTag = PositionTag.ShortWithdrawalCashInLieu;// "10"; //short withdrawal cash in lieu
                    taxlotFractionalShareWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;

                    taxlotFractionalShareWithdrawal.TransactionType = PositionTag.ShortWithdrawalCashInLieu.ToString();
                }
                else
                {
                    taxlotFractionalShareWithdrawal.PositionTag = PositionTag.LongWithdrawalCashInLieu;//"9"; //long withdrawal cash in lieu
                    taxlotFractionalShareWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Sell;

                    taxlotFractionalShareWithdrawal.TransactionType = PositionTag.LongWithdrawalCashInLieu.ToString();
                }

                CARulesHelper.FillText(taxlotFractionalShareWithdrawal);
                CARulesHelper.FillDateInfo(taxlotFractionalShareWithdrawal, corporateActionRow);

                taxlotFractionalShareWithdrawal.ProcessDate = taxlotFractionalShareWithdrawal.AUECLocalDate;
                taxlotFractionalShareWithdrawal.OriginalPurchaseDate = taxlotFractionalShareWithdrawal.AUECLocalDate; //taxlotBase.OriginalPurchaseDate;

                taxlotFractionalShareWithdrawal.AvgPrice = closingPrice;
                taxlotFractionalShareWithdrawal.OldAveragePrice = closingPrice;
                taxlotFractionalShareWithdrawal.NewAvgPrice = closingPrice.ToString();
                taxlotFractionalShareWithdrawal.OpenTotalCommissionandFees = 0;
                //taxlotFractionalShareWithdrawal.OpenTotalCommissionandFees = (taxlotBase.OpenTotalCommissionandFees * Convert.ToDouble(residualQty)) / Convert.ToDouble(taxlotBase.NewTaxlotOpenQty);
                taxlotFractionalShareWithdrawal.OpenQty = Convert.ToDouble(residualQty);
                taxlotFractionalShareWithdrawal.NewTaxlotOpenQty = residualQty.ToString();

                AllocationGroup allGroupFractionalShareWithdrawal = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotFractionalShareWithdrawal);
                //allGroupFractionalShareWithdrawal.TransactionType = TradingTransactionType.StockSPLIT.ToString();
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

        /// <summary>
        /// this is used to assign transaction source i.e. origin of the transaction
        /// </summary>
        /// <param name="allocGroup"></param>
        private void AddTransactionSource(AllocationGroup allocGroup)
        {
            try
            {
                allocGroup.TransactionSource = TransactionSource.CAStockSplit;
                allocGroup.TransactionSourceTag = (int)TransactionSource.CAStockSplit;
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
        /// Save CA data for Split
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
                    if (updatedTaxlot.PositionTag != PositionTag.LongWithdrawalCashInLieu && updatedTaxlot.PositionTag != PositionTag.ShortWithdrawalCashInLieu)
                        updatedTaxlotsToSave.Add(updatedTaxlot);
                }
                caOnProcessObject.Taxlots = updatedTaxlotsToSave;
                caOnProcessObject.IsApplied = true;
                List<AllocationGroup> newllocGrs = new List<AllocationGroup>();
                if (_isCashinLieuRequired)
                {
                    foreach (KeyValuePair<string, TaxlotBaseCollection> keyValue in TaxlotsCacheManager.Instance.GetCAWiseTaxlots())
                    {
                        caOnProcessObject.CorporateActionID = new Guid(keyValue.Key);
                        Dictionary<string, List<AllocationGroup>> allocGroupDict = GetAllocationGroupsForCA(keyValue.Key);
                        List<AllocationGroup> allGroupFractionalSharesWithdrawal = new List<AllocationGroup>();
                        if (allocGroupDict.ContainsKey(ApplicationConstants.CA_FRACTIONALWITHDRAWAL))
                        {
                            allGroupFractionalSharesWithdrawal = allocGroupDict[ApplicationConstants.CA_FRACTIONALWITHDRAWAL];
                        }

                        DataRow dr = TaxlotsCacheManager.Instance.GetCARowByID(keyValue.Key);
                        // new group will be saved by post trade cache manager
                        newllocGrs.AddRange(allGroupFractionalSharesWithdrawal);
                        ///These are taxlots generated by posttradecachemanager, hence these would be sent and saved by posttradecachemanager.
                        caOnProcessObject.NewGeneratedTaxlots = newllocGrs;
                        caOnProcessObject.FromDate = DateTime.Parse(dr[CorporateActionConstants.CONST_EffectiveDate].ToString());
                        caOnProcessObject.CorporateActionListString = TaxlotsCacheManager.Instance.GetCAStrByID(keyValue.Key);

                        // Fractional shares addition
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
                            //taxlotToclose.AUECLocalDate = caOnProcessObject.FromDate.Date;
                            taxlotToclose.AUECModifiedDate = caOnProcessObject.FromDate.Date;
                            //taxlotToclose.ProcessDate = caOnProcessObject.FromDate.Date;
                            //taxlotToclose.OriginalPurchaseDate = caOnProcessObject.FromDate.Date;
                            //commented by omshiv, ACA Cleanup
                            //taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;

                            taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                            taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                            taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                            //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

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

                            taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                            taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                            taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                            //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;
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
                }

                isSuccessful = DBManager.SaveCAForSplits(caOnProcessObject, _proxyPublishing, _allocationServices, _closingServices, userID);
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
                        foreach (string taxlotID in ca.Value)
                        {
                            if (_closingServices.CheckClosingStatus(taxlotID, caWiseDates[ca.Key]))
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
                    isSuccessful = DBManager.UndoSplit(requestObject, taxlots, _closingServices, _allocationServices, _proxyPublishing);
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
