using System;

namespace Prana.BusinessObjects.Compliance.Constants
{
    /// <summary>
    /// Tags for the tree node while operation is going on
    /// </summary>
    public class RuleNavigatorConstants
    {
        public const String NEW_TREE_NODE = "PendingNew";
        public const String RENAME_TREE_NODE = "PendingRename";
        public const String ENABLE_TREE_NODE = "PendingEnable";
        public const String DELETE_TREE_NODE = "PendingDelete";
        public const String DISABLE_TREE_NODE = "PendingDisable";

        public const String CATEGORY_ICO = "Category.ico";
        public const String ENABLE_ICO = "Enable.png";
        public const String DISABLE_ICO = "Disable.png";

        public const String SUCCESSFUL = "Successful";
        public const String UNSUCCESSFUL = "Un-Successful";
        public const String COMPLIANCE_LOG = "ComplianceUserActionLog";
    }
}
