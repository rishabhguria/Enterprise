using Prana.BusinessObjects.Compliance.Enums;
using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    /// <summary>
    /// Event args containig details of selected node
    /// </summary>
    public class SelectedNodeEventArgs : EventArgs
    {
        public RulePackage PackageName { get; set; }
        public RuleCategory Category { get; set; }
        public String RuleName { get; set; }
        public int Level { get; set; }
        public String RuleId { get; set; }
        // SelectedNodeEventArgs() { }
    }
}
