using System.Collections.Generic;

namespace Prana.TradeManager
{
    public class TradingRuleViolatedData
    {
        /// <summary>
        /// Title Message
        /// </summary>
        private string _titleMessage = string.Empty;
        public string TitleMessage
        {
            get { return _titleMessage; }
            set { _titleMessage = value; }
        }

        /// <summary>
        /// TradingRule Parameter List
        /// </summary>
        private List<TradingRuleViolatedParameter> _tradingRuleViolatedParameter = new List<TradingRuleViolatedParameter>();
        public List<TradingRuleViolatedParameter> TradingRuleViolatedParameter
        {
            get { return _tradingRuleViolatedParameter; }
            set { _tradingRuleViolatedParameter = value; }
        }

        /// <summary>
        /// Allow Expand or not
        /// </summary>
        private bool _allowExpand = true;
        public bool AllowExpand
        {
            get { return _allowExpand; }
            set { _allowExpand = value; }
        }
    }
}
