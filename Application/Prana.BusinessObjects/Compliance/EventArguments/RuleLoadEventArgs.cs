using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    /// <summary>
    /// Event agrs for load rule event
    /// </summary>
    public class RuleLoadEventArgs : EventArgs
    {
        public RuleCategory category { get; set; }

        /// <summary>
        /// Tells if operation is successful or not.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// stores if Error message in case of failure.
        /// </summary>
        public String ErrorMessage { get; set; }

        public List<GroupBase> GroupList { get; set; }
        //private List<RuleBase> ruleList;

        public RuleLoadEventArgs(List<RuleBase> ruleList, RuleCategory category)
        {
            this.RuleList = ruleList;
            this.category = category;
        }

        public RuleLoadEventArgs(List<RuleBase> ruleList, RuleCategory category, List<GroupBase> groupList)
        {
            this.GroupList = groupList;
            this.RuleList = ruleList;
            this.category = category;
        }

        public RuleLoadEventArgs()
        { }
        //public List<RuleBase> RuleList
        //{
        //    get { return this.ruleList; }
        //}
        public List<RuleBase> RuleList { get; set; }
    }
}
