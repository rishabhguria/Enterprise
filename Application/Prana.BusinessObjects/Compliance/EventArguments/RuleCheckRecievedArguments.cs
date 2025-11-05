using Prana.BusinessObjects.FIX;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class RuleCheckRecievedArguments : EventArgs
    {
        public bool isPassed { get; set; }
        public bool isOverriden { get; set; }
        public Dictionary<String, PranaMessage> orders { get; set; }
        public bool isSimulation { get; set; }
        public bool isCancelOrder { get; set; }

        public RuleCheckRecievedArguments(Dictionary<String, PranaMessage> orders, bool isPassed, bool isOverriden, bool isSimulation = false, bool isCancelOrder = false)
        {
            this.orders = orders;
            this.isPassed = isPassed;
            this.isOverriden = isOverriden;
            this.isSimulation = isSimulation;
            this.isCancelOrder = isCancelOrder;
        }

    }
}
