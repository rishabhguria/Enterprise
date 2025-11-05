using Prana.BusinessLogic.Symbol;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.FIX;
using Prana.BusinessObjects.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using Prana.SecurityMasterNew;
using System;
using System.Collections.Generic;

namespace Prana.CounterPartyRules
{
    class OptionTickerConversionRule : DllBaseRule
    {
        string _underlyingSymbolTag = string.Empty;
        string _conditionString = string.Empty;
        string _symbology = string.Empty;
        string _symbolTag = string.Empty;
        string _rootSymbolTag = string.Empty;
        string _rootSymbolStage = string.Empty;

        public override void ApplyRule(Prana.BusinessObjects.FIX.PranaMessage msg)
        {
            string underlyingSymbol = string.Empty;
            string monthYear = string.Empty;
            string monthStr = string.Empty;

            int day = int.MinValue;
            int month = int.MinValue;
            int year = int.MinValue;
            DateTime expiryDate = DateTimeConstants.MinValue;

            string nonStandardExpirySuffix = string.Empty;
            int callOrPut = int.MinValue;
            string strikePrice = string.Empty;

            string symbolTagValue = string.Empty;
            //if invalid maturity date is recieved from FIX
            bool isValidExpiryDate = false;

            try
            {
                if (!msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSecurityType))
                {
                    //Its definitely not an option trade so get out of the rule
                    return;
                }

                if (!string.IsNullOrEmpty(_conditionString))
                {
                    string[] str = _conditionString.Split(new char[] { '=' });
                    if (str.Length == 2 && str[1].Trim().ToUpper().Equals(msg.FIXMessage.ExternalInformation[FIXConstants.TagSecurityType].Value.Trim().ToUpper()))
                    {
                        if (msg.FIXMessage.ExternalInformation.ContainsKey(_underlyingSymbolTag))
                        {
                            underlyingSymbol = msg.FIXMessage.ExternalInformation[_underlyingSymbolTag].Value;
                            if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagUnderlyingSymbol))
                            {
                                msg.FIXMessage.ExternalInformation[FIXConstants.TagUnderlyingSymbol].Value = underlyingSymbol;
                            }
                            else
                            {
                                msg.FIXMessage.ExternalInformation.AddField(FIXConstants.TagUnderlyingSymbol, underlyingSymbol);
                            }

                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityDay))
                        {
                            day = int.Parse(msg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityDay].Value);
                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityMonthYear))
                        {
                            monthYear = msg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityMonthYear].Value;
                            monthStr = monthYear.Trim().Substring(4, 2);
                            month = int.Parse(monthStr);
                            year = int.Parse(monthYear.Trim().Substring(0, 4));
                        }
                        if (year >= DateTime.MinValue.Year && year <= DateTime.MaxValue.Year && month >= DateTime.MinValue.Month && month <= DateTime.MaxValue.Month && day >= DateTime.MinValue.Day && day <= DateTime.MaxValue.Day)
                        {
                            isValidExpiryDate = true;
                        }
                        if (isValidExpiryDate)
                        {
                            expiryDate = new DateTime(year, month, day);
                        }
                        else
                        {
                            monthYear = "180001"; //set to minimum value
                            monthStr = "01";
                            month = 1;
                            day = 1;  //set to minimum value
                        }

                        ///Following the eSignal, whenever the expiration is friday, it returs the date for saturday, hence added 1 in the date.
                        DateTime standardExpiry = Prana.Utilities.DateTimeUtilities.DateTimeHelper.GetNthWeekDay(3, DayOfWeek.Friday, year, month).Add(TimeSpan.FromDays(1));
                        ///If Option don't expire on standard expiry then this is nonstandard flex option
                        ///The symbol is like - O:MSFT 10B25.00D31
                        if (expiryDate != standardExpiry)
                        {
                            nonStandardExpirySuffix = "D" + day.ToString();
                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagPutOrCall))
                        {
                            callOrPut = int.Parse(msg.FIXMessage.ExternalInformation[FIXConstants.TagPutOrCall].Value);
                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagStrikePrice))
                        {
                            strikePrice = Convert.ToDouble(msg.FIXMessage.ExternalInformation[FIXConstants.TagStrikePrice].Value).ToString("f2");
                        }

                        if (!string.IsNullOrEmpty(_symbolTag))
                        {
                            if (msg.FIXMessage.ExternalInformation.ContainsKey(_symbolTag))
                            {
                                symbolTagValue = msg.FIXMessage.ExternalInformation[_symbolTag].Value;
                            }
                            else
                            {
                                symbolTagValue = msg.FIXMessage.InternalInformation[_symbolTag].Value;
                            }
                        }

                        ///If we just received the root symbol then let's build the IDCO symbol directly
                        if (!String.IsNullOrEmpty(_rootSymbolTag) && !String.IsNullOrEmpty(_rootSymbolStage))
                        {
                            string rootSymbol = string.Empty;

                            if (msg.FIXMessage.ExternalInformation.ContainsKey(_rootSymbolTag))
                            {
                                rootSymbol = msg.FIXMessage.ExternalInformation[_rootSymbolTag].Value;
                                /// In the pre consolidation phase we will get OSI/IDCO roots : rdq for SPY underlying
                                /// Creating IDCO symbol here.
                                if (_rootSymbolStage.Trim().ToUpper().Equals("PRE"))
                                {
                                    string rootWithSpaceStr = rootSymbol.PadRight(6, ' ');
                                    // set to minimum value ie 01/01/1800
                                    string yearMonthDateStr = "000101";
                                    if (isValidExpiryDate)
                                    {
                                        yearMonthDateStr = monthYear.Substring(2, 2) + monthStr + day.ToString().PadLeft(2, '0');
                                    }

                                    string callOrPutStr = string.Empty;
                                    switch (callOrPut.ToString())
                                    {
                                        case "0":
                                            callOrPutStr = "P";
                                            break;
                                        case "1":
                                            callOrPutStr = "C";
                                            break;
                                        default:
                                            callOrPutStr = "C";
                                            break;
                                    }
                                    string[] strikePriceArr = strikePrice.Split('.');
                                    string strikeStr = string.Empty;
                                    if (strikePriceArr.Length == 1)
                                    {
                                        strikeStr = strikePriceArr[0].Trim().PadLeft(5, '0') + "000";
                                    }
                                    else if (strikePriceArr.Length == 2)
                                    {
                                        strikeStr = strikePriceArr[0].Trim().PadLeft(5, '0') + strikePriceArr[1].Trim().PadRight(3, '0');
                                    }

                                    _symbology = ((int)ApplicationConstants.SymbologyCodes.IDCOOptionSymbol).ToString();
                                    symbolTagValue = rootWithSpaceStr + yearMonthDateStr + callOrPutStr + strikeStr + "U";

                                }
                                /// In the post consolidation phase we will get Underlying root OIH3 for OIH underlying.
                                else if (_rootSymbolStage.Trim().ToUpper().Equals("POST"))
                                {
                                    underlyingSymbol = rootSymbol;
                                }
                            }
                        }

                        //Either we have recived directly the osi/idco symbol or we just received rootSymbol, in that
                        //case we are building the idco symbol
                        if (!String.IsNullOrEmpty(_symbology) && !String.IsNullOrEmpty(symbolTagValue))
                        {
                            SetOCCSymbologyDetails(msg, _symbology, symbolTagValue);
                        }
                        else if (!String.IsNullOrEmpty(underlyingSymbol) && !String.IsNullOrEmpty(monthYear) && !String.IsNullOrEmpty(strikePrice) && callOrPut != int.MinValue)
                        {
                            string optionCallPutMonthCodes = ConfigurationHelper.Instance.GetValueBySectionAndKey(ConfigurationHelper.SECTION_OptionCallPutMonthCodes, callOrPut + monthStr);


                            string tickerSymbol = "O:" + underlyingSymbol + " " + monthYear.Substring(2, 2) + optionCallPutMonthCodes + strikePrice + nonStandardExpirySuffix;
                            int symbology = 0;
                            if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_Symbology))
                            {
                                symbology = Convert.ToInt32(msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Symbology].Value);
                            }
                            SecMasterRequestObj underLyingSymbolReqObj = new SecMasterRequestObj();
                            underLyingSymbolReqObj.AddData(underlyingSymbol, (ApplicationConstants.SymbologyCodes)symbology);
                            underLyingSymbolReqObj.HashCode = GetHashCode();
                            if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_CompanyUserID))
                                underLyingSymbolReqObj.UserID = msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_CompanyUserID].Value;

                            List<SecMasterBaseObj> secMasterObjList = SecMasterDataManager.GetSecMasterDataFromDB_XML(underLyingSymbolReqObj);

                            if (secMasterObjList.Count > 0)
                            {
                                SecMasterBaseObj underLyingSecMasterBaseObj = secMasterObjList[0];
                                OptionDetail optionDetail = new OptionDetail();
                                optionDetail.Symbology = (ApplicationConstants.SymbologyCodes)symbology;
                                optionDetail.AssetCategory = (AssetCategory)underLyingSecMasterBaseObj.AssetID;
                                optionDetail.UnderlyingSymbol = underlyingSymbol;
                                optionDetail.AUECID = underLyingSecMasterBaseObj.AUECID;
                                optionDetail.ExpirationDate = expiryDate;
                                optionDetail.OptionType = (OptionType)callOrPut;
                                optionDetail.StrikePrice = Convert.ToDouble(strikePrice);
                                OptionSymbolGenerator.GetOptionSymbol(optionDetail);
                                tickerSymbol = optionDetail.Symbol;
                            }

                            if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_TickerSymbol))
                            {
                                msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value = tickerSymbol;
                            }
                            else
                            {
                                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_TickerSymbol, tickerSymbol);
                            }

                            if (!msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_Symbology))
                            {
                                int tickerSymbology = Convert.ToInt32(ApplicationConstants.SymbologyCodes.TickerSymbol);
                                msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Symbology, tickerSymbology.ToString());
                            }

                            if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                            {
                                msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = tickerSymbol;
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
        }

        private void SetOCCSymbologyDetails(Prana.BusinessObjects.FIX.PranaMessage msg, string symbology, string symbolTagValue)
        {
            try
            {
                if (!msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_Symbology))
                {
                    msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_Symbology, symbology);
                }

                ApplicationConstants.SymbologyCodes code = (ApplicationConstants.SymbologyCodes)Convert.ToInt32(symbology);
                switch (code)
                {
                    case ApplicationConstants.SymbologyCodes.OSIOptionSymbol:
                        string transformedIDCOSymbol = symbolTagValue + 'U';

                        //msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = transformedIDCOSymbol;
                        msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Symbology].Value = ((int)(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol)).ToString();
                        if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_OSIOptionSymbol))
                        {
                            msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_TickerSymbol].Value = string.Empty;
                        }
                        else
                        {
                            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_IDCOOptionSymbol, transformedIDCOSymbol);
                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                        {
                            msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = transformedIDCOSymbol;
                        }
                        break;
                    case ApplicationConstants.SymbologyCodes.IDCOOptionSymbol:

                        //msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = symbolTagValue;
                        msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_Symbology].Value = ((int)(ApplicationConstants.SymbologyCodes.IDCOOptionSymbol)).ToString();
                        if (msg.FIXMessage.InternalInformation.ContainsKey(CustomFIXConstants.CUST_TAG_IDCOOptionSymbol))
                        {
                            msg.FIXMessage.InternalInformation[CustomFIXConstants.CUST_TAG_IDCOOptionSymbol].Value = symbolTagValue;
                        }
                        else
                        {
                            msg.FIXMessage.InternalInformation.AddField(CustomFIXConstants.CUST_TAG_IDCOOptionSymbol, symbolTagValue);
                        }

                        if (msg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                        {
                            msg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value = symbolTagValue;
                        }
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
        }

        public override bool CreateRules(System.Xml.XmlNode xmlNodeItem)
        {
            try
            {
                if (xmlNodeItem.Attributes["ConditionString"] != null)
                    _conditionString = xmlNodeItem.Attributes["ConditionString"].Value;
                if (xmlNodeItem.Attributes["UnderlyingSymbolTag"] != null)
                    _underlyingSymbolTag = xmlNodeItem.Attributes["UnderlyingSymbolTag"].Value;

                if (xmlNodeItem.Attributes["Symbology"] != null)
                    _symbology = xmlNodeItem.Attributes["Symbology"].Value;
                if (xmlNodeItem.Attributes["SymbolTag"] != null)
                    _symbolTag = xmlNodeItem.Attributes["SymbolTag"].Value;

                if (xmlNodeItem.Attributes["RootSymbolTag"] != null)
                    _rootSymbolTag = xmlNodeItem.Attributes["RootSymbolTag"].Value;
                if (xmlNodeItem.Attributes["RootSymbolStage"] != null)
                    _rootSymbolStage = xmlNodeItem.Attributes["RootSymbolStage"].Value;
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
            return true;
        }

        public override void ApplyRule(PranaMessage msg, List<RepeatingMessageFieldCollection> repeatingMessageFields, string repeatingTag)
        {
            return;
        }
    }
}
