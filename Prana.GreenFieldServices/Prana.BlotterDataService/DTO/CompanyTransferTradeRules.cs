using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.BlotterDataService.DTO
{
    internal class CompanyTransferTradeRules
    {

        public class TradingTicket
        {
            public UserTradingTicketUiPrefs UserTradingTicketUiPrefs { get; set; }
            public CompanyTradingTicketUiPrefs CompanyTradingTicketUiPrefs { get; set; }
            public TradingTicketRulesPrefs TradingTicketRulesPrefs { get; set; }
            public List<string> RestrictedAllowedSecuritiesList { get; set; }
            public bool IsTickerSymbologySecuritiesList { get; set; }
            public CpwiseCommissionBasis CpwiseCommissionBasis { get; set; }
            public QuickTTPrefs QuickTTPrefs { get; set; }
            public List<object> QTTFieldPreference { get; set; }
            public TTGeneralPrefs TTGeneralPrefs { get; set; }
            public bool DollarAmountPermission { get; set; }
            public ConfirmationPopUpPrefs ConfirmationPopUpPrefs { get; set; }
            public PriceSymbolValidationData PriceSymbolValidationData { get; set; }
            public bool IsTTPrefInitialized { get; set; }
        }

        public class CompanyTradingTicketUiPrefs
        {
            public List<DefAssetSide> DefAssetSides { get; set; }
            public string DefTTControlsMapping { get; set; }
            public List<object> listTTControlsMapping { get; set; }
            public object Broker { get; set; }
            public object Venue { get; set; }
            public int? OrderType { get; set; }
            public int? TimeInForce { get; set; }
            public int? HandlingInstruction { get; set; }
            public object ExecutionInstruction { get; set; }
            public int? TradingAccount { get; set; }
            public int? Strategy { get; set; }
            public int? Account { get; set; }
            public bool? IsSettlementCurrencyBase { get; set; }
            public bool? IsShowTargetQTY { get; set; }
            public double Quantity { get; set; }
            public double IncrementOnQty { get; set; }
            public double IncrementOnStop { get; set; }
            public double IncrementOnLimit { get; set; }
            public int QuantityType { get; set; }
            public bool IsUseRoundLots { get; set; }
        }

        public class ConfirmationPopUpPrefs
        {
            public bool ISNewOrder { get; set; }
            public bool ISCXL { get; set; }
            public bool ISCXLReplace { get; set; }
            public bool IsManualOrder { get; set; }
            public int CompanyUserID { get; set; }
        }

        public class CpwiseCommissionBasis
        {
            public object DictCounterPartyWiseCommissionBasis { get; set; }
            public object DictCounterPartyWiseSoftCommissionBasis { get; set; }
            public object DictCounterPartyWiseExecutionInstructions { get; set; }
            public object DictCounterPartyWiseExecutionVenue { get; set; }
            public object DictCounterPartyWiseExecutionAlgoType { get; set; }
            public int UserID { get; set; }
        }

        public class DefAssetSide
        {
            public int Asset { get; set; }
            public object OrderSide { get; set; }
        }

        public class PriceSymbolValidationData
        {
            public bool RiskCtrlCheck { get; set; }
            public double RiskValue { get; set; }
            public bool ValidateSymbolCheck { get; set; }
            public int CompanyUserID { get; set; }
            public bool LimitPriceCheck { get; set; }
            public bool SetExecutedQtytoZero { get; set; }
        }

        public class QuickTTPrefs
        {
            public List<string> InstanceNames { get; set; }
            public List<string> InstanceForeColors { get; set; }
            public List<string> InstanceBackColors { get; set; }
            public List<int> HotButtonQuantities { get; set; }
            public bool UseAccountForLinking { get; set; }
            public bool UseVenueForLinking { get; set; }
        }

        public class TradingTicketRulesPrefs
        {
            public bool IsOversellTradingRule { get; set; }
            public bool IsOverbuyTradingRule { get; set; }
            public bool IsUnallocatedTradeAlert { get; set; }
            public bool IsFatFingerTradingRule { get; set; }
            public bool IsDuplicateTradeAlert { get; set; }
            public bool IsPendingNewTradeAlert { get; set; }
            public bool IsInMarketIncluded { get; set; }
            public bool IsSharesOutstandingRule { get; set; }
            public double DefineFatFingerValue { get; set; }
            public int DuplicateTradeAlertTime { get; set; }
            public int PendingNewOrderAlertTime { get; set; }
            public int FatFingerAccountOrMasterFund { get; set; }
            public int IsAbsoluteAmountOrDefinePercent { get; set; }
            public int SharesOutstandingAccOrMF { get; set; }
            public double SharesOutstandingValue { get; set; }
        }

        public class TTGeneralPrefs
        {
            public int DefaultSymbology { get; set; }
            public int DefaultOptionType { get; set; }
            public bool IsShowOptionDetails { get; set; }
            public bool IsSaveChecked { get; set; }
            public bool CleanDetailsAfterTrade { get; set; }
            public string DefaultInternalComments { get; set; }
            public string DefaultBrokerComments { get; set; }
            public bool IsUseCustodianAsExecutingBroker { get; set; }
        }

        public class UserTradingTicketUiPrefs
        {
            public List<DefAssetSide> DefAssetSides { get; set; }
            public string DefTTControlsMapping { get; set; }
            public List<object> listTTControlsMapping { get; set; }
            public int? Broker { get; set; }
            public object Venue { get; set; }
            public object OrderType { get; set; }
            public object TimeInForce { get; set; }
            public object HandlingInstruction { get; set; }
            public int? ExecutionInstruction { get; set; }
            public object TradingAccount { get; set; }
            public object Strategy { get; set; }
            public int? Account { get; set; }
            public bool? IsSettlementCurrencyBase { get; set; }
            public object IsShowTargetQTY { get; set; }
            public double Quantity { get; set; }
            public double IncrementOnQty { get; set; }
            public double IncrementOnStop { get; set; }
            public double IncrementOnLimit { get; set; }
            public int QuantityType { get; set; }
            public bool IsUseRoundLots { get; set; }
        }
    }
}
