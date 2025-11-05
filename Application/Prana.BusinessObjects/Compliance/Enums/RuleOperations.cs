using Prana.BusinessObjects.AppConstants;

namespace Prana.BusinessObjects.Compliance.Enums
{
    /// <summary>
    /// Enum for all operations to be applied on rules.
    /// </summary>
    public enum RuleOperations
    {
        [EnumDescriptionAttribute("Add Rule")]
        AddRule,

        [EnumDescriptionAttribute("Rename Rule")]
        RenameRule,

        [EnumDescriptionAttribute("Delete Rule")]
        DeleteRule,

        [EnumDescriptionAttribute("Enable Rule")]
        EnableRule,

        [EnumDescriptionAttribute("Disable Rule")]
        DisableRule,

        [EnumDescriptionAttribute("Import Rule")]
        ImportRule,

        [EnumDescriptionAttribute("Export Rule")]
        ExportRule,

        [EnumDescriptionAttribute("Export All Rules")]
        ExportAllRules,

        [EnumDescriptionAttribute("Enable All Rules")]
        EnableAllRules,

        [EnumDescriptionAttribute("Disable All Rules")]
        DisableAllRules,

        [EnumDescriptionAttribute("Load Rules")]
        LoadRules,

        [EnumDescriptionAttribute("Save Rule")]
        Build,

        [EnumDescriptionAttribute("Unidentified")]
        None

    }
}
