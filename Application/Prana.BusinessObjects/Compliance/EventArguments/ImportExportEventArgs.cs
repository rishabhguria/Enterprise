using Prana.BusinessObjects.Compliance.Enums;
using System;

namespace Prana.BusinessObjects.Compliance.EventArguments
{
    public class ImportExportEventArgs : EventArgs
    {
        public RulePackage PackageName { get; set; }
        public RuleCategory Category { get; set; }
        public String RuleId { get; set; }

        public ImportExportEventArgs(RulePackage packageName, RuleCategory category, String ruleId)
        {
            this.PackageName = packageName;
            this.Category = category;
            this.RuleId = ruleId;
        }

        public ImportExportEventArgs()
        {
            this.PackageName = RulePackage.None;
            this.Category = RuleCategory.None;
            this.RuleId = String.Empty;
        }
    }
}
