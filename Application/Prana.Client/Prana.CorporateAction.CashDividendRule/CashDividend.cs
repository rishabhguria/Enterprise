using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PubSubService.Interfaces;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
//using Prana.PostTrade;

namespace Prana.CorporateAction.CashDividendRule
{
    public class CashDividend : ICorporateActionBaseRule
    {

        IPostTradeServices _postTradeServicesInstance = null;
        IAllocationServices _allocationServices = null;
        IClosingServices _closingServices = null;
        IActivityServices _activityService = null;
        int _hashCode = 0;
        ProxyBase<IPublishing> _proxyPublishing = null;
        DuplexProxyBase<IPricingService> _pricingServicesProxy = null;
        ISecMasterServices _secmasterProxy = null;
        ICashManagementService _cashManagementService = null;

        #region ICorporateActionBaseRule Members

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
                            row.SetColumnError(columns[CorporateActionConstants.CONST_DivRate], "Dividend Rate should be greater than zero.");
                            continue;
                        }
                    }

                    else
                    {
                        row.SetColumnError(columns[CorporateActionConstants.CONST_DivRate], "Dividend Rate required.");
                        continue;
                    }

                    /// We will pick up all positions less than the ex div date and apply the divideds.
                    /// May be we can decrement this in Preview function, it creates problem when CA between date range are fetched in Historical tabs -Abhilash
                    row[CorporateActionConstants.CONST_EffectiveDate] = CARulesHelper.AddDayStartTimeToDate(Convert.ToDateTime(row[CorporateActionConstants.CONST_ExDivDate]));
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

        public CAPreviewResult PreviewCorporateActions(DataRowCollection caRows, ref TaxlotBaseCollection taxlotList, string commaSeparatedAccountIds, CAPreferences caPref, int brokerId)
        {
            CAPreviewResult caPreviewResult = new CAPreviewResult();
            StringBuilder postCAClosedTaxlots = new StringBuilder();
            string taxlotsBoxedPositionStr = string.Empty;
            taxlotList = new TaxlotBaseCollection();

            try
            {
                foreach (DataRow corporateActionRow in caRows)
                {
                    TaxlotBaseCollection caModifiedTaxlots = null;
                    string symbol = corporateActionRow[CorporateActionConstants.CONST_OrigSymbolTag].ToString();
                    DateTime exDivDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_ExDivDate]);
                    DateTime divPayoutDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_DivPayoutDate]);
                    DateTime recordDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_RecordDate]);
                    DateTime divDeclarationDate = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_DivDeclarationDate]);

                    string origCompanyName = corporateActionRow[CorporateActionConstants.CONST_CompanyName].ToString();

                    double divRate = Convert.ToDouble(corporateActionRow[CorporateActionConstants.CONST_DivRate]);

                    /// We will pick up all positions less than the ex div date and apply the divideds. Already assigned to effectivedate in validaterow function
                    /// Moved substraction of date from ValidaeRow() function
                    DateTime date = Convert.ToDateTime(corporateActionRow[CorporateActionConstants.CONST_EffectiveDate]);

                    //Add the UTC Effective Date to the function
                    caModifiedTaxlots = AllocationDataManager.GetAllAccountOpenPositionsForDateAndSymbol(date, symbol, commaSeparatedAccountIds);
                    if (caModifiedTaxlots.Count < 1)
                    {
                        caPreviewResult.NoPositionSymbols = symbol;
                        taxlotList.Clear();
                        return caPreviewResult;
                    }
                    // check box position fundwise
                    taxlotsBoxedPositionStr = CheckBoxedPosition(caModifiedTaxlots);

                    if (String.IsNullOrEmpty(taxlotsBoxedPositionStr))
                    {
                        foreach (TaxlotBase taxlot in caModifiedTaxlots)
                        {
                            /// A issue was raised that we should let the user apply dividend even if taxlots closed in future date. Hence commented
                            /// Similarly we should also not check if the corporate action has been applied in future
                            /// The reason is that dividend works on the open qty at the time of applying on a particular date, and just
                            /// change the cash flow (dividend)
                            //if (_closingServices.CheckClosingStatus(taxlot.L2TaxlotID, date))
                            //{
                            //    postCAClosedTaxlots.Append(taxlot.L2TaxlotID);
                            //    postCAClosedTaxlots.Append(",");
                            //}
                            //else
                            //{

                            double sideMultiplier = 1;
                            if (taxlot.PositionType == PositionType.Short)
                            {
                                sideMultiplier = -1;
                            }

                            taxlot.NewCompanyName = origCompanyName;
                            taxlot.Dividend = taxlot.OpenQty * divRate * sideMultiplier;
                            taxlot.DivPayoutDate = divPayoutDate.ToShortDateString();
                            taxlot.ExDivDate = exDivDate.ToShortDateString();
                            taxlot.RecordDate = recordDate.ToShortDateString();
                            taxlot.DivDeclarationDate = divDeclarationDate.ToShortDateString();
                            taxlot.CorpActionID = corporateActionRow[CorporateActionConstants.CONST_CorporateActionId].ToString();
                            taxlot.Description = corporateActionRow[CorporateActionConstants.CONST_Description].ToString();
                            double payReceiveOriginal = 0.0;
                            double originalNotionalValue = taxlot.OpenQty * taxlot.AvgPrice * taxlot.AssetMultiplier;

                            // If  UseNetNotional is set true then Set the Commission to Zero and update the Notional Value Acoordingly
                            if (caPref.UseNetNotional)
                            {
                                payReceiveOriginal = originalNotionalValue + (taxlot.OpenTotalCommissionandFees * GetSideMultiplier(taxlot.PositionType));

                                taxlot.OpenTotalCommissionandFees = 0;
                            }
                            else
                            {
                                payReceiveOriginal = originalNotionalValue;
                            }
                            if (payReceiveOriginal != 0 && taxlot.AssetMultiplier != 0)
                                taxlot.AvgPrice = payReceiveOriginal / (taxlot.OpenQty * taxlot.AssetMultiplier);

                            taxlot.OldAveragePrice = taxlot.AvgPrice;
                            taxlot.NotionalValue = taxlot.OpenQty * taxlot.AvgPrice * taxlot.AssetMultiplier;

                            CARulesHelper.FillText(taxlot);
                        }
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

            string postCAClosedTaxlotsStr = postCAClosedTaxlots.ToString().TrimEnd(new char[1] { ',' });

            if (!String.IsNullOrEmpty(postCAClosedTaxlotsStr) || !String.IsNullOrEmpty(taxlotsBoxedPositionStr))
            {
                ///Which means that there is some taxlot for which closing is done in the future date, so first user have to unwind then
                ///and only then we will allow applying the corporate action.
                taxlotList = null;
            }
            caPreviewResult.ClosingIDs = postCAClosedTaxlotsStr;
            caPreviewResult.BoxedPositionTaxlotIds = taxlotsBoxedPositionStr;
            return caPreviewResult;
        }

        /// <summary>
        /// check fund wise box position. Both Long and Short positions are not allowed in the same fund
        /// </summary>
        /// <param name="caModifiedTaxlots"></param>
        /// <returns></returns>
        private string CheckBoxedPosition(TaxlotBaseCollection caModifiedTaxlots)
        {
            StringBuilder strBldrTaxlotsBoxedPosition = new StringBuilder();
            string strTaxlotsBoxedPosition = string.Empty;
            Dictionary<int, string> dictCheckBoxPositions = new Dictionary<int, string>();
            string taxlotID = string.Empty;

            try
            {
                for (int i = 0; i < caModifiedTaxlots.Count; i++)
                {
                    TaxlotBase taxlotBase = caModifiedTaxlots[i];

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

                    if (!string.IsNullOrEmpty(taxlotID))
                    {
                        strBldrTaxlotsBoxedPosition.Append(taxlotID);
                        strBldrTaxlotsBoxedPosition.Append(",");
                    }
                }

                strTaxlotsBoxedPosition = strBldrTaxlotsBoxedPosition.ToString().TrimEnd(new char[] { ',' });
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return strTaxlotsBoxedPosition;
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

        public bool SaveCorporateAction(string corporateActionListString, TaxlotBaseCollection updatedTaxlots, int userID)
        {
            bool isSuccessful = false;
            try
            {
                CAOnProcessObjects caOnProcessObject = new CAOnProcessObjects();
                caOnProcessObject.CorporateActionListString = corporateActionListString;

                caOnProcessObject.Taxlots = updatedTaxlots;
                caOnProcessObject.IsApplied = true;
                isSuccessful = DBManager.SaveCAForCashDividend(caOnProcessObject, _activityService);
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

        public bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots, bool isSMModificationRequired)
        {
            bool isSuccessful = false;
            try
            {
                StringBuilder taxlotIds = new StringBuilder();
                CAOnProcessObjects requestObject = new CAOnProcessObjects();
                requestObject.CorporateActionIDs = caIDs;
                foreach (TaxlotBase taxlot in taxlots)
                {
                    taxlotIds.Append(taxlot.L2TaxlotID + ",");
                }
                if (!String.IsNullOrEmpty(caIDs))
                {
                    isSuccessful = DBManager.UndoCashDividend(requestObject, _activityService, taxlotIds.ToString(), isSMModificationRequired);
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
        #endregion



        public bool UndoCorporateActions(string caIDs, TaxlotBaseCollection taxlots)
        {
            return false;
        }
    }
}
