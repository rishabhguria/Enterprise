using Prana.BusinessObjects;
using Prana.BusinessObjects.FIX;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.PreTrade;
using Prana.Utilities.MiscUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;

namespace Prana.DropCopyProcessor_PostTrade
{
    public static class StuckTradeMailManager
    {
        static int emailIntervalForStuckTrades = 0;
        static System.Threading.Timer _timer = null;
        static bool _isTimerOn = false;
        static BackgroundWorker _bgStuckTradeTimer = null;

        static string[] mailRecipients;
        static string mailSender = string.Empty;
        static string mailSenderName = string.Empty;
        static string mailerPassword = string.Empty;
        static int mailPort = 0;
        static bool enableSSL = false;
        static string mailHost = string.Empty;
        static bool authenticationRequired = false;
        static string mailSubject = string.Empty;
        static string mailHeader = string.Empty;
        static string mailFooter = string.Empty;

        static int _maxMailsForStuckTrades = 0;
        static int _noOfTimesMailSent = 0;

        static IPreTradeService _preTradeService = new PreTradeService();

        public static void CheckAndProcessStuckTrades()
        {
            try
            {
                InitializeVariables();
                _noOfTimesMailSent = 0;
                if (_bgStuckTradeTimer == null)
                {
                    _bgStuckTradeTimer = new BackgroundWorker();
                }
                if (_isTimerOn == false)
                {
                    _isTimerOn = true;
                    _bgStuckTradeTimer.DoWork += new DoWorkEventHandler(_bgStuckTradeTimer_DoWork);
                    _bgStuckTradeTimer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgStuckTradeTimer_RunWorkerCompleted);
                    _bgStuckTradeTimer.RunWorkerAsync();
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

        private static void InitializeVariables()
        {
            try
            {
                emailIntervalForStuckTrades = CommonDataCache.CachedDataManager.GetInstance.EmailIntervalForStuckTrades;

                mailRecipients = ConfigurationManager.AppSettings["MailReceiverAddress"].Split(',');
                mailSender = ConfigurationManager.AppSettings["MailSenderAddress"];
                mailSenderName = ConfigurationManager.AppSettings["MailSenderName"];
                mailerPassword = ConfigurationManager.AppSettings["MailSenderPassword"];
                mailPort = int.Parse(ConfigurationManager.AppSettings["MailPort"]);
                enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"]);
                mailHost = ConfigurationManager.AppSettings["MailServer"];
                mailSubject = ConfigurationManager.AppSettings["MailSubject"];
                mailHeader = ConfigurationManager.AppSettings["MailHeader"];
                mailFooter = ConfigurationManager.AppSettings["MailFooter"];
                authenticationRequired = bool.Parse(ConfigurationManager.AppSettings["AuthenticationRequired"]);

                _maxMailsForStuckTrades = int.Parse(ConfigurationManager.AppSettings["MaxMailsForStuckTrades"]);
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
        private static void _bgStuckTradeTimer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                _bgStuckTradeTimer.DoWork -= new DoWorkEventHandler(_bgStuckTradeTimer_DoWork);
                _bgStuckTradeTimer.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(_bgStuckTradeTimer_RunWorkerCompleted);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        static void _bgStuckTradeTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                StartTimerToSendStuckTradesEmail();
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void StartTimerToSendStuckTradesEmail()
        {
            try
            {
                if (_timer == null)
                {
                    _timer = new System.Threading.Timer(Timerhit);
                    GetStuckTradesAndSendMail(true);
                }
                // I think First mail shoul be go instantly 
                _timer.Change(emailIntervalForStuckTrades, emailIntervalForStuckTrades);
                _isTimerOn = true;
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
        private static void StopTimerToSendStuckTradesEmail()
        {
            try
            {
                if (_timer != null)
                {
                    _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                }
                _isTimerOn = false;
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
        public static void Timerhit(object state)
        {
            try
            {
                GetStuckTradesAndSendMail(false);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private static void GetStuckTradesAndSendMail(bool isFirstTime)
        {
            try
            {
                List<PranaMessage> StuckTrades = new List<PranaMessage>();
                List<PranaMessage> complianceStuckTrades = new List<PranaMessage>();
                if (_noOfTimesMailSent < _maxMailsForStuckTrades)
                {
                    StuckTrades = PranaDropCopyProcessor_PostTrade.GetInstance.GetCachedErrorOrders();
                    if (CommonDataCache.ComplianceCacheManager.GetPreComplianceModuleEnabled())
                        complianceStuckTrades = _preTradeService.GetComplianceCachedErrorOrders();

                    if (StuckTrades.Count != 0 || complianceStuckTrades.Count != 0)
                    {
                        StringBuilder mailBody = GetStuckTradesInStringForm(StuckTrades, complianceStuckTrades);
                        _noOfTimesMailSent++;
                        EmailsHelper.MailSend(mailSubject + " " + DateTime.Now.ToString(), mailBody.ToString(), mailSender, mailSenderName, mailerPassword, mailRecipients, mailPort, mailHost, enableSSL, authenticationRequired);
                    }
                }
                if (StuckTrades.Count == 0 && complianceStuckTrades.Count == 0 && isFirstTime == false)
                {
                    StopTimerToSendStuckTradesEmail();
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
        private static StringBuilder GetStuckTradesInStringForm(List<PranaMessage> StuckTrades, List<PranaMessage> complianceStuckTrades)
        {
            StringBuilder stuckTrades = new StringBuilder();
            List<string> stuckTradesFIX = new List<string>();
            List<string> stuckTradesCompliance = new List<string>();

            try
            {
                stuckTrades.AppendLine("-------------------------------------------------------------------------------------------");
                stuckTrades.AppendLine("Total No. OF " + mailHeader + " " + DateTime.Now.ToString() + ":" + (StuckTrades.Count + complianceStuckTrades.Count).ToString());
                stuckTrades.AppendLine("-------------------------------------------------------------------------------------------");
                stuckTrades.AppendLine();
                stuckTrades.AppendLine("Stuck FIX Trades:");
                if (StuckTrades.Count == 0)
                    stuckTrades.Append("Not Available");
                stuckTrades.AppendLine();

                foreach (PranaMessage pranaMessage in StuckTrades)
                {
                    AppendStuckTradeInformation(pranaMessage, ref stuckTrades, ref stuckTradesFIX);
                }
                stuckTrades.AppendLine();
                stuckTrades.AppendLine("Stuck Compliance Trades:");
                if (complianceStuckTrades.Count == 0)
                    stuckTrades.Append("Not Available");
                stuckTrades.AppendLine();

                foreach (PranaMessage kvp in complianceStuckTrades)
                {
                    AppendStuckTradeInformation(kvp, ref stuckTrades, ref stuckTradesCompliance);
                }

                stuckTrades.AppendLine("-------------------------------------------------------------------------------------------");
                stuckTrades.AppendLine(mailFooter);
                stuckTrades.AppendLine("-------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return stuckTrades;
        }

        private static void AppendStuckTradeInformation(PranaMessage pranaMessage, ref StringBuilder stuckTrades, ref List<string> stuckTrade)
        {
            try
            {
                string symbol = string.Empty;
                string counterParty = string.Empty;
                string assetID = string.Empty;
                string currencyID = string.Empty;
                string exDestination = string.Empty;
                string onBehalfOfCompID = string.Empty;
                string idSourceValues = string.Empty;
                string targetCompID = string.Empty;
                string side = string.Empty;

                //For Option Asset
                string maturityMonthYear = string.Empty;
                string putOrCall = string.Empty;
                string strikePrice = string.Empty;
                string maturityDay = string.Empty;
                string contractMultiplier = string.Empty;
                //

                PranaMessage prMsg = pranaMessage;

                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSymbol))
                    symbol = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagSymbol].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExecBroker))
                    counterParty = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagExecBroker].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagCurrency))
                    currencyID = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagCurrency].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagExDestination))
                    exDestination = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagExDestination].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagOnBehalfOfCompID))
                    onBehalfOfCompID = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagOnBehalfOfCompID].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagTargetCompID))
                    targetCompID = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagTargetCompID].Value;
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSide))
                {
                    switch (prMsg.FIXMessage.ExternalInformation[FIXConstants.TagSide].Value)
                    {
                        case FIXConstants.SIDE_Buy:
                            side = "Buy";
                            break;
                        case FIXConstants.SIDE_Sell:
                            side = "Sell";
                            break;
                        case FIXConstants.SIDE_BuyMinus:
                            side = "Buy Minus";
                            break;
                        case FIXConstants.SIDE_SellPlus:
                            side = "Sell Plus";
                            break;
                        case FIXConstants.SIDE_SellShort:
                            side = "Sell Short";
                            break;
                        case FIXConstants.SIDE_SellShortExempt:
                            side = "Sell Short Exempt";
                            break;
                        case FIXConstants.SIDE_Cross:
                            side = "Cross";
                            break;
                        case FIXConstants.SIDE_CrossShort:
                            side = "Cross Short";
                            break;
                        case FIXConstants.SIDE_Buy_Open:
                            side = "Buy to Open";
                            break;
                        case FIXConstants.SIDE_Buy_Closed:
                            side = "Buy to Close";
                            break;
                        case FIXConstants.SIDE_Sell_Open:
                            side = "Sell to Open";
                            break;
                        case FIXConstants.SIDE_Sell_Closed:
                            side = "Sell to Close";
                            break;
                        case FIXConstants.SIDE_Buy_Cover:
                            side = "Buy to Cover";
                            break;
                    }
                }

                //Security Type
                if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSecurityType))
                    assetID = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagSecurityType].Value;
                //For Security type : Option -<Below are the mandatory tags>
                //Start
                if (assetID == FIXConstants.SECURITYTYPE_Options)
                {
                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityMonthYear))
                        maturityMonthYear = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityMonthYear].Value;
                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagPutOrCall))
                    {
                        int value = Convert.ToInt32(prMsg.FIXMessage.ExternalInformation[FIXConstants.TagPutOrCall].Value);
                        switch (value)
                        {
                            case FIXConstants.Underlying_Put:
                                putOrCall = "Put";
                                break;
                            case FIXConstants.Underlying_Call:
                                putOrCall = "Call";
                                break;
                        }
                    }
                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagStrikePrice))
                        strikePrice = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagStrikePrice].Value;
                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagMaturityDay))
                        maturityDay = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagMaturityDay].Value;
                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagContractMultiplier))
                        contractMultiplier = prMsg.FIXMessage.ExternalInformation[FIXConstants.TagContractMultiplier].Value;

                    if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagIDSource))
                    {
                        switch (prMsg.FIXMessage.ExternalInformation[FIXConstants.TagIDSource].Value)
                        {
                            case FIXConstants.SECURITYIDSOURCE_Cusip:
                                idSourceValues = "Cusip[TAG 22]: ";
                                break;
                            case FIXConstants.SECURITYIDSOURCE_Sedol:
                                idSourceValues = "Sedol[TAG 22]: ";
                                break;
                            case FIXConstants.SECURITYIDSOURCE_Isin:
                                idSourceValues = "Isin[TAG 22]: ";
                                break;
                            case FIXConstants.SECURITYIDSOURCE_Bloomberg:
                                idSourceValues = "Bloomberg[TAG 22]: ";
                                break;
                        }
                        if (prMsg.FIXMessage.ExternalInformation.ContainsKey(FIXConstants.TagSecurityID))
                            idSourceValues += prMsg.FIXMessage.ExternalInformation[FIXConstants.TagSecurityID].Value;
                    }
                }
                //End
                if (!stuckTrade.Contains(symbol))
                {
                    stuckTrade.Add(symbol);
                    stuckTrades.AppendLine("Symbol[TAG 55]:" + symbol);
                    stuckTrades.AppendLine("Side[TAG 54]:" + side);
                    stuckTrades.AppendLine("Asset Category[TAG 167]:" + assetID);

                    //Checking assetID is Option
                    if (assetID == FIXConstants.SECURITYTYPE_Options)
                    {
                        if (idSourceValues != string.Empty)
                            stuckTrades.AppendLine(idSourceValues);
                        if (strikePrice != string.Empty)
                            stuckTrades.AppendLine("Strike Price[TAG 202]:" + strikePrice);
                        if (putOrCall != string.Empty)
                            stuckTrades.AppendLine("Put Or Call[TAG 201]:" + putOrCall);
                        if (maturityDay != string.Empty)
                            stuckTrades.AppendLine("Maturity Day[TAG 205]:" + maturityDay);
                        if (maturityMonthYear != string.Empty)
                            stuckTrades.AppendLine("Maturity Month Year[TAG 200]:" + maturityMonthYear);
                        if (contractMultiplier != string.Empty)
                            stuckTrades.AppendLine("Contract Multiplier[TAG 231]:" + contractMultiplier);
                    }
                    //
                    stuckTrades.AppendLine("Counter party[TAG 76,115,100,15]:" + counterParty + "," + onBehalfOfCompID + "," + exDestination + "," + currencyID);
                    stuckTrades.AppendLine("Target Company ID[TAG 56]:" + targetCompID);
                    stuckTrades.AppendLine("-------------------------------------------------------------------------------------------");
                    stuckTrades.AppendLine();
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
    }
}
