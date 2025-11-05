using System;

namespace Prana.PostTrade.Commission
{
    public class CommissionRuleEvents : EventArgs
    {
        bool _groupwise = false;

        public bool GroupWise
        {
            get { return _groupwise; }
            set { _groupwise = value; }
        }

    }
}
