using System.Collections.Generic;

namespace Prana.Rebalancer.RebalancerNew.Models
{
    public class ImportModel
    {
        public HashSet<string> CustomGroups { get; set; }

        public HashSet<string> Accounts { get; set; }
    }
}
