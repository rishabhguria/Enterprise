using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class RuleOperationEventArgs : EventArgs
    {
        /// <summary>
        /// Category of rule on which operation to be performed.
        /// </summary>
        public RuleCategory Category { get; set; }

        /// <summary>
        /// Operation to be performed on rule.
        /// </summary>
        public RuleOperations OperationType { get; set; }

        /// <summary>
        /// Contains value when operation is Rename rule
        /// It contains either ruleId or rule name.
        /// In other cases it is empty.
        /// </summary>
        public string OldValue { get; set; }
        //private List<RuleBase> ruleList;

        /// <summary>
        /// Tells if operation is raised by current user or other user.
        /// </summary>
        public Boolean IsOperationFromDifferentClient { get; set; }

        /// <summary>
        /// Tells if operation is successful or not.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// stores if Error message in case of failure.
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// specifies default values.
        /// </summary>
        public RuleOperationEventArgs()
        {
            //Setting false as default value
            IsOperationFromDifferentClient = false;
            this.RuleList = new List<RuleBase>();
            this.FailedRuleList = new List<RuleBase>();
        }
        //public List<RuleBase> RuleList
        //{
        //    get { return this.ruleList; }
        //}

        /// <summary>
        /// List of rules for Operation.
        /// </summary>
        public List<RuleBase> RuleList { get; set; }

        /// <summary>
        /// List of rules for which operation is failed.
        /// </summary>
        public List<RuleBase> FailedRuleList { get; set; }
    }
}