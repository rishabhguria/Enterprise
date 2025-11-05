using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using System;
using System.Collections.Generic;

namespace Prana.BusinessObjects.Compliance.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRuleHandler : IDisposable
    {
        /// <summary>
        /// Event raised when rules are loaded.
        /// </summary>
        event RuleLoadedHandler RuleLoaded;

        /// <summary>
        /// method to get all rules from the server.
        /// </summary>
        void GetAllRulesAsync();

        /// <summary>
        /// Method for operations to be applied on rules.
        /// </summary>
        /// <param name="rulePackage"></param>
        /// <param name="ruleName"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="ruleId"></param>
        /// <param name="oldRuleName"></param>
        /// <param name="tag"></param>
        void OperationOnRule(RulePackage rulePackage, string ruleName, RuleOperations ruleOperations, String ruleId, String oldRuleName, Object tag);

        /// <summary>
        /// Event raised when Rule operation is completed.
        /// </summary>
        event RuleOperationHandler RuleOperation;
        /// <summary>
        ///  Method to export rules.
        /// </summary>
        /// <param name="ruleList"></param>
        /// <param name="importExportPath"></param>
        void ExportRule(List<RuleBase> ruleList, string importExportPath);

        //event RuleImportExportHandler ExportComplete;

        /// <summary>
        /// Method to import rules.
        /// </summary>
        /// <param name="importDefinition"></param>
        void ImportRule(ImportDefinition importDefinition);

    }
}
