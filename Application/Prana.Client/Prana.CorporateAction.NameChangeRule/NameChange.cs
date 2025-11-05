using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.PositionManagement;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
//using Prana.PostTrade;
using System.Data;
using System.Text;

namespace Prana.CorporateAction.NameChangeRule
{
    class NameChange : ICorporateActionBaseRule
    {
        string _OrigSymbolTag = string.Empty;
        string _FinalSymbolTag = string.Empty;
        string _operation = string.Empty;
        string _targetTag = string.Empty;
        string _symbologyCode = string.Empty;

        IPostTradeServices _postTradeServicesInstance = null;
        IAllocationServices _allocationServices = null;
        IClosingServices _closingServices = null;
        ProxyBase<IPublishing> _proxyPublishing = null;
        IActivityServices _activityService = null;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        ISecMasterServices _secmasterProxy = null;
        ICashManagementService _cashManagementService = null;

        int _hashCode = 0;
        //public event PreviewCAResponseHandler PreviewCAResponse;
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
                    //double tryParseResultDouble = 0;

                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_OrigSymbolTag].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_OrigSymbolTag], "Orignal Symbol required.");
                        continue;
                    }
                    if (String.IsNullOrEmpty(row[CorporateActionConstants.CONST_NewSymbolTag].ToString()) && String.IsNullOrEmpty(row[CorporateActionConstants.CONST_NewCompanyName].ToString()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSymbolTag], "New Symbol and New Company both can not be empty at the same time.");
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewCompanyName], "New Symbol and New Company both can not be empty at the same time.");
                        continue;
                    }

                    if (row[CorporateActionConstants.CONST_OrigSymbolTag].ToString().ToUpper().Equals(row[CorporateActionConstants.CONST_NewSymbolTag].ToString().ToUpper()))
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_NewSymbolTag], "New Symbol should be different than the Orig Symbol.");
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

            // TODO: When there are no taxlots for a symbol then a return message shold go to UI(Obviously in case of Multiple CA's).
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

                    if (String.IsNullOrEmpty(newSymbol)) // FOR COMPANY NAME CHANGE 
                    {
                        GetModifiedTaxlotForCompanyNameChange(corporateActionRow, ref caModifiedTaxlots, ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, commaSeparatedAccountIds, caPref);
                        if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr))
                        {
                            /// Taxlots are not closed hence ready for applying ca
                            TaxlotsCacheManager.Instance.AddCARow(caID, corporateActionRow);
                            TaxlotsCacheManager.Instance.AddTaxlots(caID, new TaxlotBaseCollection());
                        }
                    }
                    else
                    {
                        GetModifiedTaxlotForNameChangeNew(corporateActionRow, ref caModifiedTaxlots, ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, commaSeparatedAccountIds, caPref);
                        if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                        {
                            /// Taxlots are not closed and CA not applied in future hence ready for applying new ca
                            TaxlotsCacheManager.Instance.AddCARow(caID, corporateActionRow);
                            TaxlotsCacheManager.Instance.AddTaxlots(caID, caModifiedTaxlots);
                        }
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

        private void GetModifiedTaxlotForCompanyNameChange(DataRow corporateActionRow, ref TaxlotBaseCollection caModifiedTaxlots, ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, string commaSeparatedAccountIds, CAPreferences caPref)
        {
            caModifiedTaxlots = new TaxlotBaseCollection();
            StringBuilder taxlotsClosedInFuture = new StringBuilder();
            StringBuilder taxlotsCAAppliedInFuture = new StringBuilder();
            StringBuilder taxlotsBoxedPosition = new StringBuilder();
            Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();
            try
            {
                string origSymbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                string newComapanyName = corporateActionRow[CorporateActionConstants.CONST_NewCompanyName].ToString();
                string caID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();
                DateTime date = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);

                TaxlotBaseCollection taxlotList = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, origSymbol, commaSeparatedAccountIds);

                List<string> positionalNameChangeTaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(date);
                for (int i = 0; i < taxlotList.Count; i++)
                {
                    TaxlotBase taxlotBase = taxlotList[i];
                    if (!dictCheckBoxPositions.ContainsKey(taxlotBase.Level1ID))
                    {
                        if (taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Open || taxlotBase.OrderSideTagValue == FIXConstants.SIDE_Buy_Closed)
                            dictCheckBoxPositions.Add(taxlotBase.Level1ID, PositionType.Long.ToString());
                        else
                            dictCheckBoxPositions.Add(taxlotBase.Level1ID, PositionType.Short.ToString());

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

                    if (_closingServices.CheckClosingStatus(taxlotList[i].L2TaxlotID, date))
                    {
                        if (!positionalNameChangeTaxlotId.Contains(taxlotList[i].L2TaxlotID))
                        {
                            taxlotsClosedInFuture.Append(taxlotList[i].L2TaxlotID);
                            taxlotsClosedInFuture.Append(",");
                        }
                    }

                    if (_closingServices.CheckCorporateActionStatus(taxlotList[i].L2TaxlotID, date) || positionalNameChangeTaxlotId.Contains(taxlotList[i].L2TaxlotID))
                    {
                        taxlotsCAAppliedInFuture.Append(taxlotList[i].L2TaxlotID);
                        taxlotsCAAppliedInFuture.Append(",");
                    }
                }

                taxlotsClosedInFutureStr = taxlotsClosedInFuture.ToString().TrimEnd(new char[] { ',' });
                taxlotsCAAppliedInFutureStr = taxlotsCAAppliedInFuture.ToString().TrimEnd(new char[] { ',' });
                taxlotsBoxedPositionStr = taxlotsBoxedPosition.ToString().TrimEnd(new char[] { ',' });

                if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                {
                    TaxlotBase taxlotBase = null;
                    for (int i = 0; i < taxlotList.Count; i++)
                    {
                        taxlotBase = taxlotList[i];
                        if (taxlotBase != null)
                        {
                            taxlotBase.CorpActionID = caID;
                            taxlotBase.NewCompanyName = newComapanyName;
                            taxlotBase.OldAveragePrice = taxlotBase.AvgPrice;
                            CARulesHelper.FillDateInfo(taxlotBase, corporateActionRow);
                            CARulesHelper.FillText(taxlotBase);

                            caModifiedTaxlots.Add(taxlotBase);
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

            if (!String.IsNullOrEmpty(taxlotsClosedInFutureStr) || !String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr))
            {
                ///Which means that there is some taxlot for which closing/corporate action is done in the future date, so first user have to unwind then
                ///and only then we will let the user apply the corporate action.
                caModifiedTaxlots = null;
            }
        }

        private void GetModifiedTaxlotForNameChangeNew(DataRow corporateActionRow, ref TaxlotBaseCollection caModifiedTaxlots, ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, string commaSeparatedAccountIds, CAPreferences caPref)
        {
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

            //Add the UTC Effective Date to the function
            TaxlotBaseCollection taxlotList = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, origSymbol, commaSeparatedAccountIds);
            int count = taxlotList.Count;
            List<string> positionalNameChangeTaxlotId = AllocationDataManager.GetFuturePositionalCorpActionTaxlotId(date);

            //check whether taxlot is already closed or not and boxed positions
            CheckTaxlotStatus(ref taxlotsClosedInFutureStr, ref taxlotsCAAppliedInFutureStr, ref taxlotsBoxedPositionStr, taxlotsClosedInFuture, taxlotsCAAppliedInFuture, taxlotsBoxedPosition, date, taxlotList, count, positionalNameChangeTaxlotId);

            if (String.IsNullOrEmpty(taxlotsClosedInFutureStr) && String.IsNullOrEmpty(taxlotsCAAppliedInFutureStr) && String.IsNullOrEmpty(taxlotsBoxedPositionStr))
            {
                for (int i = 0; i < count; i++)
                {
                    TaxlotBase taxlotBase = taxlotList[i];
                    taxlotBase.CorpActionID = caID;

                    if (String.IsNullOrEmpty(newCompanyName))
                    {
                        taxlotBase.NewCompanyName = origCompanyName;
                    }
                    else
                    {
                        taxlotBase.NewCompanyName = newCompanyName;
                    }

                    double originalNotionalValue = taxlotBase.OpenQty * taxlotBase.AvgPrice * taxlotBase.AssetMultiplier;

                    taxlotBase.OldAveragePrice = taxlotBase.AvgPrice;

                    TaxlotBase taxlotOriginal = taxlotBase.Clone();

                    //Added to set company name, PRANA-13022
                    taxlotOriginal.NewCompanyName = origCompanyName;
                    // handle Net Notional
                    double payReceiveOriginal = 0.0;

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

                    CARulesHelper.FillText(taxlotOriginal);

                    AllocationGroup allGroupOrig = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotOriginal);
                    //Overriding the new generated taxlotid from the original taxlotid. Since we have to use this generated taxlot with
                    //some changes as the withdrawal taxlot, hence need to generate it from allocation module.
                    allGroupOrig.GroupID = taxlotOriginal.GroupID;
                    allGroupOrig.TaxLots[0].GroupID = taxlotOriginal.GroupID;
                    allGroupOrig.TaxLots[0].TaxLotID = taxlotOriginal.L2TaxlotID;

                    ///Withdrawal for original symbol
                    TaxlotBase taxlotWithdrawal = taxlotBase.Clone();

                    taxlotWithdrawal.CounterPartyID = 0;
                    taxlotWithdrawal.VenueID = 0;

                    //Added to set company name, PRANA-13022
                    taxlotWithdrawal.NewCompanyName = origCompanyName;

                    CARulesHelper.FillDateInfo(taxlotWithdrawal, corporateActionRow);
                    if (taxlotWithdrawal.PositionType == PositionType.Short)
                    {
                        taxlotWithdrawal.PositionTag = PositionTag.ShortWithdrawal;// "7"; //short withdrawal 
                        taxlotWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Buy_Closed;

                        taxlotWithdrawal.TransactionType = TradingTransactionType.ShortWithdrawal.ToString();
                    }
                    else
                    {
                        taxlotWithdrawal.PositionTag = PositionTag.LongWithdrawal;//"5"; //long withdrawal
                        taxlotWithdrawal.OrderSideTagValue = FIXConstants.SIDE_Sell;

                        taxlotWithdrawal.TransactionType = TradingTransactionType.LongWithdrawal.ToString();
                    }

                    double payReceiveWithdrawal = 0.0;

                    // If  UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Acoordingly
                    if (caPref.UseNetNotional)
                    {
                        payReceiveWithdrawal = originalNotionalValue + (taxlotBase.OpenTotalCommissionandFees * GetSideMultiplier(taxlotWithdrawal.PositionType));
                        taxlotWithdrawal.OpenTotalCommissionandFees = 0;
                    }
                    else
                    {
                        payReceiveWithdrawal = originalNotionalValue;
                        taxlotWithdrawal.OpenTotalCommissionandFees = (taxlotBase.OpenTotalCommissionandFees * (-1));
                    }

                    taxlotWithdrawal.NotionalValue = payReceiveWithdrawal;
                    taxlotWithdrawal.AvgPrice = payReceiveWithdrawal / (taxlotWithdrawal.OpenQty * taxlotWithdrawal.AssetMultiplier);

                    CARulesHelper.FillText(taxlotWithdrawal);
                    //taxlotWithdrawal.ProcessDate = taxlotWithdrawal.AUECLocalDate;
                    //taxlotWithdrawal.OriginalPurchaseDate = taxlotOriginal.OriginalPurchaseDate;

                    taxlotWithdrawal.OriginalPurchaseDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);
                    taxlotWithdrawal.ProcessDate = taxlotWithdrawal.OriginalPurchaseDate;
                    //taxlotWithdrawal.OpenTotalCommissionandFees = taxlotBase.OpenTotalCommissionandFees; //Negative commission for withdrawal taxlot;//(taxlotBase.OpenTotalCommissionandFees * (-1)); //Negative commission for withdrawal taxlot

                    AllocationGroup allGroupWithdrawal = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotWithdrawal);
                    //allGroupWithdrawal.TransactionType = TradingTransactionType.StockNC.ToString();
                    AddTransactionSource(allGroupWithdrawal);

                    taxlotWithdrawal.L2TaxlotID = allGroupWithdrawal.TaxLots[0].TaxLotID;
                    taxlotWithdrawal.GroupID = allGroupWithdrawal.TaxLots[0].GroupID;

                    // adding closing taxlot's taxlotid with the opening taxlotID for closing purpose
                    allGroupOrig.TaxLots[0].ClosingWithTaxlotID = taxlotWithdrawal.L2TaxlotID;

                    // add original taxlot in the collection
                    AddGroup(allGroupOrig, ApplicationConstants.CA_ORIGINAL);
                    caModifiedTaxlots.Add(taxlotOriginal);
                    // add withdrawal taxlot in the collection
                    AddGroup(allGroupWithdrawal, ApplicationConstants.CA_WITHDRAWAL);
                    caModifiedTaxlots.Add(taxlotWithdrawal);

                    /// Addition for new symbol
                    TaxlotBase taxlotAddition = taxlotBase.Clone();

                    taxlotAddition.CounterPartyID = 0;
                    taxlotAddition.VenueID = 0;

                    taxlotAddition.Symbol = newSymbol;
                    CARulesHelper.FillText(taxlotAddition);
                    CARulesHelper.FillDateInfo(taxlotAddition, corporateActionRow);
                    if (taxlotAddition.PositionType == PositionType.Short)
                    {
                        taxlotAddition.PositionTag = PositionTag.ShortAddition;//"6"; //short addition
                        taxlotAddition.TransactionType = TradingTransactionType.ShortAddition.ToString();
                    }
                    else
                    {
                        taxlotAddition.PositionTag = PositionTag.LongAddition;//"4"; //long addition
                        taxlotAddition.TransactionType = TradingTransactionType.LongAddition.ToString();
                    }
                    taxlotAddition.ProcessDate = taxlotAddition.AUECLocalDate;
                    // Initially all dates have same values i.e. Trade, Process and Original Purchase date
                    // but if Name change is applied more than once on the same symbol then this line will not be validated
                    // taxlotAddition.OriginalPurchaseDate = taxlotOriginal.ProcessDate;
                    // PRANA-6506: Original purchase date not updates correctly for new trades when we apply any CA on the security which already generated from Corporate action                   
                    taxlotAddition.OriginalPurchaseDate = taxlotOriginal.OriginalPurchaseDate;

                    double payReceiveAddition = 0.0;
                    // If  UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Accordingly
                    if (caPref.UseNetNotional)
                    {
                        payReceiveAddition = originalNotionalValue + (taxlotBase.OpenTotalCommissionandFees * GetSideMultiplier(taxlotAddition.PositionType));
                        taxlotAddition.OpenTotalCommissionandFees = 0;
                    }
                    else
                    {
                        taxlotAddition.OpenTotalCommissionandFees = taxlotBase.OpenTotalCommissionandFees;
                        payReceiveAddition = originalNotionalValue;
                    }

                    if (payReceiveAddition != 0 && taxlotAddition.AssetMultiplier != 0)
                        taxlotAddition.AvgPrice = payReceiveAddition / (taxlotAddition.OpenQty * taxlotAddition.AssetMultiplier);

                    taxlotAddition.NotionalValue = payReceiveAddition;

                    AllocationGroup allGroupAddition = _allocationServices.CreateAllcationGroupFromTaxlotBase(taxlotAddition);
                    //allGroupAddition.TransactionType = TradingTransactionType.StockNC.ToString();//CachedData.GetInstance().vlTransactionType.
                    AddTransactionSource(allGroupAddition);

                    AddGroup(allGroupAddition, ApplicationConstants.CA_ADDITION);
                    taxlotAddition.L2TaxlotID = allGroupAddition.TaxLots[0].TaxLotID;
                    taxlotAddition.GroupID = allGroupAddition.TaxLots[0].GroupID;
                    caModifiedTaxlots.Add(taxlotAddition);
                }
            }
            else
            {
                caModifiedTaxlots = null;
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
                allocGroup.TransactionSource = TransactionSource.CAStockNameChange;
                allocGroup.TransactionSourceTag = (int)TransactionSource.CAStockNameChange;
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

        private void CheckTaxlotStatus(ref string taxlotsClosedInFutureStr, ref string taxlotsCAAppliedInFutureStr, ref string taxlotsBoxedPositionStr, StringBuilder taxlotsClosedInFuture, StringBuilder taxlotsCAAppliedInFuture, StringBuilder taxlotsBoxedPosition, DateTime date, TaxlotBaseCollection taxlotList, int count, List<string> positionalNameChangeTaxlotId)
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

                    // check closing status
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
        /// <summary>
        /// Save CA and positions in the secmaster DB
        /// </summary>
        /// <param name="firstCA"></param>
        /// <param name="corporateActionListString"></param>
        /// <param name="updatedTaxlots"></param>
        public bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            string resultStr = string.Empty;
            bool isSuccessful = false;
            ///Here we are not using the passed values but using an internally saved cache. this is specific to namechange only

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

                    List<AllocationGroup> newllocGrs = new List<AllocationGroup>();
                    newllocGrs.AddRange(allGroupWithdrawal);
                    newllocGrs.AddRange(allGroupAddition);

                    ///These are taxlots generated by posttradecachemanager, hence these would be sent and saved by posttradecachemanager.
                    caOnProcessObject.NewGeneratedTaxlots = newllocGrs;
                    // not in use
                    //List<TaxLot> PositionalTaxlots = new List<TaxLot>();
                    //List<TaxLot> ClosingTaxlots = new List<TaxLot>();

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
                        // taxlotToclose.ACAData.ACAAvgPrice = taxlotToclose.AvgPrice;
                        // it is set in closing
                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

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
                        // it is set in closing
                        //taxlotToclose.ClosingMode = ClosingMode.CorporateAction;

                        taxlotToclose.OrderSide = TagDatabaseManager.GetInstance.GetOrderSideText(taxlotToclose.OrderSideTagValue);
                        taxlotToclose.AssetName = CachedDataManager.GetInstance.GetAssetText(taxlotToclose.AssetID);
                        taxlotToclose.AssetCategoryValue = (AssetCategory)taxlotToclose.AssetID;

                        //ClosingTaxlots.Add(taxlotToclose);

                        closingTaxlots.Add(taxlotToclose);

                    }
                    /// third parameter says whether closing based on Notional change or normal closing
                    caOnProcessObject.ClosingData = _closingServices.CloseDataforPairedTaxlots(positionalTaxlots, closingTaxlots, false);
                    // set closing status of every taxlot
                    SetClosingStatus(caOnProcessObject.ClosingData);
                    caOnProcessObjectList.Add(caOnProcessObject);
                }

                isSuccessful = DBManager.SaveCAWithSymbolAndCompanyNameChange(caOnProcessObjectList, _allocationServices, _closingServices, userID);
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
                // Taxlots for Name Change
                taxlots = AllocationDataManager.GetOpenPositionforUndoPreview(caIDsStr);

                // Taxlots for Company Name Change
                foreach (DataRow row in caRows)
                {
                    string newSymbol = row[CorporateActionConstants.CONST_NewSymbolTag].ToString();
                    string origSymbol = row[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                    string origCompanyName = row[CorporateActionConstants.CONST_CompanyName].ToString();
                    DateTime date = Convert.ToDateTime(row[CorporateActionConstants.CONST_EffectiveDate]);
                    if (String.IsNullOrEmpty(newSymbol))
                    {
                        TaxlotBaseCollection companyNameChangeTaxlots = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, origSymbol, string.Empty);
                        foreach (TaxlotBase txlt in companyNameChangeTaxlots)
                        {
                            txlt.NewCompanyName = origCompanyName;
                            taxlots.Add(txlt);
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

        #region moved code


        //private string GetCorporateActionXML(string caID)
        //{
        //    XmlSaveHandler xmlSaveMgr = new XmlSaveHandler();
        //    AllocationGroupCollection groups = null;
        //    lock (_corporateActionGroupCollection)
        //    {
        //        if (_corporateActionGroupCollection.ContainsKey(caID))
        //        {
        //            groups = _corporateActionGroupCollection[caID];
        //        }
        //        if (groups != null)
        //        {
        //            foreach (AllocationGroup group in groups)
        //            {
        //                xmlSaveMgr.CreateXmls(group);
        //            }
        //        }

        //    }
        //    return xmlSaveMgr.GetAndClearCorporateActionXML();
        //}

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
