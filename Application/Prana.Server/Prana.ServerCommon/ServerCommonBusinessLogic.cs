using Prana.BusinessLogic;
using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.DateTimeUtilities;
using System;

namespace Prana.ServerCommon
{
    public class ServerCommonBusinessLogic
    {
        /*
         * Never used variable. error after applying microsoft managed rules.
         */
        //static ITradeQueueProcessor _errorQueue;
        // static ITradeQueueProcessor _comOutQueue;

        public static void Initlise()
        {
        }

        public static void SetOrderStatus(PranaMessage pranaMessage)
        {
            try
            {
                double qty = double.Parse(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagOrderQty].Value);
                double cumQty = 0;
                if (pranaMessage.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCumQty) && pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty] != null)
                    cumQty = double.Parse(pranaMessage.FIXMessage.ExternalInformation[FIXConstants.TagCumQty].Value);
                if (qty - cumQty == 0)
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_Filled);
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_Filled);
                }
                else if (qty - cumQty == qty)
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_New);
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_New);
                }
                else if (((qty - cumQty) > 0) && ((qty - cumQty) < qty))
                {
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagOrdStatus, FIXConstants.ORDSTATUS_PartiallyFilled);
                    pranaMessage.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExecType, FIXConstants.EXECTYPE_PartiallyFilled);
                }
                else
                {
                    // Incase Executed Qty is greater than Order Qty
                }
            }
            #region Catch
            //Never used variable ex error after managed rules.
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
            #endregion
        }
       
        public static void SetDateDetails(PranaMessage pranaMsg)
        {
            try
            {
                int assetID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AssetID].Value);
                int auecID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                string orderSideTagValue = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value.ToString();
                string transactionTime = string.Empty;
                string cutOffTime = string.Empty;
                bool isCutOffUsed = false;

                DateTime auecLocalDate = DateTimeConstants.MinValue;
                DateTime processDate = DateTimeConstants.MinValue;
                DateTime origPurchaseDate = DateTimeConstants.MinValue;

                int auecSettlementPeriod = CachedDataManager.GetInstance.GetAUECSettlementPeriod(auecID, orderSideTagValue);

                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTransactTime))
                    transactionTime = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagTransactTime].Value;
                else
                {
                    transactionTime = DateTimeConstants.GetCurrentTimeInFixFormat();
                    pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagTransactTime, transactionTime);
                }
                if (!transactionTime.Contains("/"))
                    transactionTime = DateTime.ParseExact(transactionTime, DateTimeConstants.NirvanaDateTimeFormat, null).ToString("MM/dd/yyyy HH:mm:ss tt");
                string msgtype = "";
                if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_PranaMsgType))
                    msgtype = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_PranaMsgType].Value.ToString();
                if (!string.IsNullOrEmpty(msgtype) && (Convert.ToInt32(msgtype) == (int)(OrderFields.PranaMsgTypes.ORDManual) || Convert.ToInt32(msgtype) == (int)(OrderFields.PranaMsgTypes.ORDManualSub)))
                {
                    auecLocalDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(transactionTime), CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AUECLocalDate, auecLocalDate.ToString());
                    processDate = auecLocalDate;
                }
                else
                {
                    auecLocalDate = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(transactionTime), CachedDataManager.GetInstance.GetAUECTimeZone(auecID));
                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_AUECLocalDate, auecLocalDate.ToString());
                    if (Convert.ToDateTime(transactionTime).Date != auecLocalDate.Date)
                    {
                        Logger.LoggerWrite("[TRADEDATE-MISMATCH] TransactionTime Is= " + transactionTime.ToString() + " And AUEC ID Is= " + auecID.ToString() + " And AUECLOCALDATE Is= " + auecLocalDate.ToString(), LoggingConstants.CATEGORY_FLAT_FILE_ClientMessages);
                    }

                    // Process date logic for futures
                    try
                    {
                        // by default initialize processdate by aueclocaldate
                        processDate = auecLocalDate;
                        if (assetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.Future)
                        {
                            string symbol = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value.ToString();

                            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CutOffTime))
                            {
                                cutOffTime = pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CutOffTime].Value;
                            }
                            //isCutOffUsed
                            if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IsCutOffTimeUsed))
                            {
                                isCutOffUsed = Convert.ToBoolean(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IsCutOffTimeUsed].Value);
                            }
                            if (isCutOffUsed)
                            {
                                if (String.IsNullOrEmpty(cutOffTime))
                                {
                                    throw new Exception("Cut-Off time is not available for the symbol : " + symbol + ". Please save cutoff time and correct the ProcessDate and OriginalPurchaseDate.");
                                }
                                else
                                {
                                    // This array would contain hour, min, sec
                                    string[] Arr = new string[10];
                                    string[] cutOffArr = new string[10];
                                    if (cutOffTime.Contains("/"))
                                    {
                                        Arr = cutOffTime.Split(new char[] { ' ' });
                                        cutOffArr = Arr[1].Split(new char[] { ':' });
                                    }
                                    else
                                    {
                                        cutOffArr = cutOffTime.Split(new char[] { ':' });
                                    }
                                    if (cutOffArr.Length == 3)
                                    {
                                        DateTime aueclocalCutOffDateTime = new DateTime(auecLocalDate.Year, auecLocalDate.Month, auecLocalDate.Day, Convert.ToInt32(cutOffArr[0]), Convert.ToInt32(cutOffArr[1]), Convert.ToInt32(cutOffArr[2])); //auecLocalDate.Date + cutoff    
                                        //If the trade date is greater than the cut-off time, then process date would be of next business day
                                        if (auecLocalDate >= aueclocalCutOffDateTime)
                                        {
                                            processDate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(auecLocalDate, 1, auecID);
                                            //By Kashish G.,PRANA-20469
                                            if (Convert.ToBoolean(ConfigurationHelper.Instance.GetAppSettingValueByKey("UpdateTradeDateBasedOnCutOffTime")))
                                            {
                                                pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECLocalDate].Value = processDate.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Cut-Off time for the symbol : " + symbol + " is not in correct format (HH:MM:SS). Please correct the CutOffTime, ProcessDate and OriginalPurchaseDate.");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                    }
                }

                origPurchaseDate = processDate;
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_ProcessDate, processDate.ToString());
                pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_OriginalPurchaseDate, origPurchaseDate.ToString());
                if (assetID != (int)Prana.BusinessObjects.AppConstants.AssetCategory.FXForward)
                {
                    DateTime settlementdate = DateTimeConstants.MinValue;
                    if (auecSettlementPeriod == 0)
                    {
                        settlementdate = processDate;
                    }
                    else
                    {
                        settlementdate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(processDate, auecSettlementPeriod, auecID, true);
                    }
                    if (pranaMsg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_DaysToSettlementFixedIncome) && (assetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.FixedIncome || assetID == (int)Prana.BusinessObjects.AppConstants.AssetCategory.ConvertibleBond))
                    {
                        int daysToSettlementFixedIncome = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_DaysToSettlementFixedIncome].Value);
                        if (daysToSettlementFixedIncome > 0)
                        {
                            // Kuldeep A.: For Bonds, there was not any handling for holidays so modified it to adjust with holidays.
                            settlementdate = BusinessDayCalculator.GetInstance().AdjustBusinessDaysForAUEC(processDate, daysToSettlementFixedIncome, auecID, true);
                        }
                        else
                        {
                            // Kuldeep A.: In case user do not provides DaysToSettlement in SymbolLookUp then settlement date will be min value (01-01-1800), so
                            // copied process date into settlement date in this case as a preventive step.
                            settlementdate = processDate;
                        }
                    }
                    pranaMsg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_SettlementDate, settlementdate.ToString());
                }
                pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagSendingTime, DateTimeConstants.GetCurrentTimeInFixFormat());
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
        }

        public static void SetExpiryDateDetails(PranaMessage pranaMsg)
        {
            try
            {
                string expiryTime = string.Empty;
                int auecID = int.Parse(pranaMsg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_AUECID].Value);
                DateTime date = DateTime.Now;
                if (pranaMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExpireTime))
                {
                    expiryTime = pranaMsg.FIXMessage.ExternalInformation[FIXConstants.TagExpireTime].Value;
                    if (!expiryTime.Contains("/"))
                        expiryTime = DateTime.ParseExact(expiryTime, DateTimeConstants.NirvanaDateTimeFormat, null).ToString("MM/dd/yyyy HH:mm:ss tt");
                    if(!DateTime.TryParse(expiryTime, out date))
                        expiryTime=string.Empty;
                    else
                        expiryTime = Prana.BusinessObjects.TimeZoneInfo.ConvertUtcTimeToLocalTime(Convert.ToDateTime(expiryTime), CachedDataManager.GetInstance.GetAUECTimeZone(auecID)).ToString();
                    pranaMsg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagExpireTime, expiryTime);
                }
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
            }

        }
    }
}
