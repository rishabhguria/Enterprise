using Prana.BusinessObjects.Compliance.Enums;
using Prana.LogManager;
using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    /// <summary>
    /// Event args for opertaions when raised from rule navigator.
    /// 
    /// </summary>
    public class OperationsOnRuleEventArgs : EventArgs
    {
        //public List<RuleBase> RuleList { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RulePackage PackageName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public RuleCategory Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String RuleName { get; set; }
        public int Level { get; set; }
        public String RuleId { get; set; }
        public RuleOperations OperationType { get; set; }
        public String OldRuleName { get; set; }

        public OperationsOnRuleEventArgs()
        {
            try
            {
                //this.RuleList = new List<RuleBase>();
                this.RuleName = String.Empty;
                this.PackageName = RulePackage.None;
                this.Category = RuleCategory.None;
                this.Level = -1;
                this.RuleId = String.Empty;
                this.OperationType = RuleOperations.None;
                this.OldRuleName = String.Empty;
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
    }
}
