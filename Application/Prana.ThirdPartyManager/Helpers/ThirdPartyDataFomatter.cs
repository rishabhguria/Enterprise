using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.XMLUtilities;
using System;
using System.Data;
using System.IO;

namespace Prana.ThirdPartyManager.Helper
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class ThirdPartyDataFomatter
    {
        public ThirdPartyDataFomatter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyDataFomatter"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="thirdParty">The third party.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="date">The date.</param>
        /// <remarks></remarks>
        public ThirdPartyDataFomatter(ThirdPartyCommon common, ThirdPartyFlatFileDetailCollection items, string thirdParty, int companyId, DateTime date)
        {
            FormatData(common, items, thirdParty, companyId, date);
        }
        /// <summary>
        /// Gets the forex rate.
        /// </summary>
        /// <param name="companyId">The company id.</param>
        /// <param name="currencyId">The currency id.</param>
        /// <param name="date">The date.</param>
        /// <param name="companyAccountID">The account.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static double GetForexRate(int companyId, int currencyId, DateTime date, int companyAccountID)
        {
            try
            {
                // Added by Suraj http://jira.nirvanasolutions.com:8080/browse/PRANA-7826
                ConversionRate conversionRate =
                    ForexConverter.GetInstance(companyId).GetConversionRateForCurrencyToBaseCurrency(currencyId, date, companyAccountID);
                return conversionRate.RateValue;

                //Prana.BusinessObjects.ConversionRate conversionRate = 
                //    Prana.CommonDataCache.ForexConverter.GetInstance(companyId).GetConversionRateForCurrencyToBaseCurrency(currencyId, date);
                //return conversionRate.RateValue;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Gets the side multiplier.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static int GetSideMultiplier(ThirdPartyFlatFileDetail item)
        {
            int sideMultiplier = -1;
            try
            {
                if (CachedDataManager.GetInstance.IsLongSide(item.SideTag))
                {
                    sideMultiplier = 1;
                }
                return sideMultiplier;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        /// <summary>
        /// Gets the cache values.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static bool GetCacheValues(ThirdPartyFlatFileDetail item)
        {
            try
            {
                item.AccountName = CachedDataManager.GetInstance.GetAccountText(item.CompanyAccountID);
                item.CounterParty = CachedDataManager.GetInstance.GetCounterPartyText(item.CounterPartyID);
                item.VenueName = CachedDataManager.GetInstance.GetVenueText(item.VenueID);
                item.Exchange = CachedDataManager.GetInstance.GetExchangeText(item.ExchangeID);
                item.Asset = CachedDataManager.GetInstance.GetAssetText(item.AssetID);
                item.UnderLying = CachedDataManager.GetInstance.GetUnderLyingText(item.UnderLyingID);
                item.Strategy = CachedDataManager.GetInstance.GetStrategyText(item.StrategyID);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Formats the data.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="thirdParty">The third party.</param>
        /// <param name="companyId">The company id.</param>
        /// <param name="date">The date.</param>
        /// <remarks></remarks>
        public void FormatData(ThirdPartyCommon thirdPartyCommon, ThirdPartyFlatFileDetailCollection items, string thirdParty, int companyId, DateTime date)
        {
            bool isCacheValid = true;
            //double forexValid = 1.0;

            if(items == null)
            {
                return;
            }

            #region Format Data
            //for each row of data obtained for the selected account details, further details are filled .
            foreach (ThirdPartyFlatFileDetail item in items)
            {

                if (thirdPartyCommon.PbUniqueIDDict.ContainsKey(item.PBUniqueID))
                {
                    item.AllocationSeqNo = thirdPartyCommon.PbUniqueIDDict[item.PBUniqueID];
                    item.AllocationSeqNo++;
                    thirdPartyCommon.PbUniqueIDDict[item.PBUniqueID] = item.AllocationSeqNo;
                }
                else
                {
                    item.AllocationSeqNo = 0;
                    thirdPartyCommon.PbUniqueIDDict.Add(item.PBUniqueID, item.AllocationSeqNo);
                }

                if (isCacheValid)
                    isCacheValid = GetCacheValues(item);

                item.GrossAmount = Math.Round(item.AllocatedQty * item.AveragePrice * item.AssetMultiplier, 6);

                // by default the securityIDType is "Ticker" (and therefore, hardcoded).
                item.SecurityIDType = "Ticker";
                item.ThirdParty = thirdParty;





                item.TradeDate = Ext.ShortDateStr(item.TradeDate);
                item.ExpirationDate = Ext.ShortDateStr(item.ExpirationDate);
                item.SettlementDate = Ext.ShortDateStr(item.SettlementDate);
                item.ProcessDate = Ext.ShortDateStr(item.ProcessDate);
                item.OriginalPurchaseDate = Ext.ShortDateStr(item.OriginalPurchaseDate);
                item.IssueDate = Ext.ShortDateStr(item.IssueDate);
                item.FirstCouponDate = Ext.ShortDateStr(item.FirstCouponDate);
                item.FirstResetDate = Ext.ShortDateStr(item.FirstResetDate);
                item.RerateDateBusDayAdjusted1 = Ext.ShortDateStr(item.RerateDateBusDayAdjusted1);
                item.RerateDateBusDayAdjusted2 = Ext.ShortDateStr(item.RerateDateBusDayAdjusted2);
                //Commented by Suraj as it is not required http://jira.nirvanasolutions.com:8080/browse/PRANA-7826
                //if (forexValid != 0)
                //    forexValid = GetForexRate(companyId, item.CurrencyID, date, item.CompanyAccountID);

                item.ForexRate = GetForexRate(companyId, item.CurrencyID, date, item.CompanyAccountID);

                if (item.CommissionCharged == double.MinValue)
                {
                    item.CommissionCharged = 0;
                }
                if (item.SoftCommissionCharged == double.MinValue)
                {
                    item.SoftCommissionCharged = 0;
                }
                if (item.OtherBrokerFee == double.MinValue)
                {
                    item.OtherBrokerFee = 0;
                }
                if (item.ClearingBrokerFee == double.MinValue)
                {
                    item.ClearingBrokerFee = 0;
                }

                item.NetAmount = Math.Round(((item.GrossAmount) +
                    (item.CommissionCharged +
                        item.SoftCommissionCharged +
                        item.OtherBrokerFee +
                        item.ClearingBrokerFee +
                        item.StampDuty +
                        item.TransactionLevy +
                        item.ClearingFee +
                        item.TaxOnCommissions +
                        item.MiscFees +
                        item.SecFee +
                        item.OccFee +
                        item.OrfFee
                    ) * GetSideMultiplier(item)), 6);

                item.CompanyID = companyId;

                if (!thirdPartyCommon.TaxLotWithStateDict.ContainsKey(item.EntityID))
                {
                    thirdPartyCommon.TaxLotWithStateDict.Add(item.EntityID, item.TaxLotState);
                }

                if (item.TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore) &&
                    (!thirdPartyCommon.TaxLotIgnoreStateDict.ContainsKey(item.EntityID)))
                {
                    thirdPartyCommon.TaxLotIgnoreStateDict.Add(item.EntityID, item.TaxLotState);
                }

                if (item.TaxLotState.Equals(Prana.BusinessObjects.AppConstants.PranaTaxLotState.Ignore) &&
                    item.FromDeleted.ToLower().Equals("yes") && (!thirdPartyCommon.DeletedToIgnoreDict.ContainsKey(item.EntityID)))
                {
                    thirdPartyCommon.DeletedToIgnoreDict.Add(item.EntityID, "yes");
                }

            }
            #endregion
        }


        public DataSet FormatData(DataSet ds, Prana.BusinessObjects.ThirdPartyFileFormat thirdPartyFileFormat, ThirdPartyBatch batch, string spName)
        {
            try
            {
                DataSet _dsXML = new DataSet();
                string mappedfilePath = string.Empty;

                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    dt = GeneralUtilities.ArrangeTable(dt, "ThirdPartyFlatFileDetail");

                    string xsltPath = thirdPartyFileFormat.PranaToThirdParty;
                    string xsltName = xsltPath.Substring(xsltPath.LastIndexOf("\\") + 1);

                    string xsltStartUpPath;
                    if (batch.TransmissionType == ((int)ApplicationConstants.TransmissionType.FIX).ToString())
                    {
                        if(spName.StartsWith("P_"))
                            xsltStartUpPath = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), spName.Substring(2) + ".xslt");
                        else
                            xsltStartUpPath = String.Format(@"{0}\{1}", Directory.GetCurrentDirectory(), spName + ".xslt");
                    }
                    else
                        xsltStartUpPath = String.Format(@"{0}\{1}\{2}\{3}",
                        Directory.GetCurrentDirectory(), ApplicationConstants.MAPPING_FILE_DIRECTORY, ApplicationConstants.MappingFileType.ThirdPartyXSLT, xsltName);

                    string strSerializedXML = Directory.GetCurrentDirectory() + @"\SerializedThirdPartyFlatFileXml.xml";
                    dt.WriteXml(strSerializedXML);
                    string mappedxml = Directory.GetCurrentDirectory() + @"\ConvertedThirdPartyNew.xml";
                    
                    if (new FileInfo(xsltStartUpPath).Exists)
                    {
                        mappedfilePath = XMLUtilities.GetTransformed(strSerializedXML, mappedxml, xsltStartUpPath);
                    }

                    if (!mappedfilePath.Equals(""))
                    {
                        _dsXML.ReadXml(mappedfilePath);
                        if (_dsXML.Tables.Count <= 0)
                        {
                            return null;
                        }

                        GeneralUtilities.ReArrangeTable(_dsXML.Tables[0]);
                        return _dsXML;

                    }
                    else
                    {
                        return null;
                    }
                }
                return _dsXML;
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGONLY);
                return null;
            }
        }

    }
}
