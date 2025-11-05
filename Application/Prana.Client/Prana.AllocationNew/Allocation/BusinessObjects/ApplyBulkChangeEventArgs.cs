using Prana.Allocation.Common.Definitions;
using Prana.Allocation.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prana.AllocationNew.Allocation.BusinessObjects
{
    public class ApplyBulkChangeEventArgs : EventArgs
    {
        /// <summary>
        ///AllocationRule Object
        /// </summary>
        public AllocationRule Rule { get; set; }

        /// <summary>
        /// allocationBase is updated on the basis of this value.
        /// </summary>
        public bool allocationBaseChecked { get; set; }
        /// <summary>
        /// matchingRule is updated on the basis of this value.
        /// </summary>
        public bool matchingRuleChecked { get; set; }
        /// <summary>
        /// preferencedAccount is updated on the basis of this value.
        /// </summary>
        public bool preferencedAccountChecked { get; set; }
        /// <summary>
        /// matchPortfolioPostion is updated on the basis of this value.
        /// </summary>
        public bool matchPortfolioPostionChecked { get; set; }

        /// <summary>
        /// Apply bulk operation to default rule also.
        /// </summary>
        public bool ApplyOnDefaultRule { get; set; }

        /// <summary>
        /// Apply bulk operation to selected Pref on All
        /// </summary>
        public bool ApplyOnSelectedPref { get; set; }

        /// <summary>
        /// List of preferences on which bulk change to be applied.
        /// </summary>
        public List<int> PreferenceList { get; set; }
    }
}
