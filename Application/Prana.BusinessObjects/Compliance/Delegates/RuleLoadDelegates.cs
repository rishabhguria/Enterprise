using Prana.BusinessObjects.Compliance.EventArguments;
using System;

namespace Prana.BusinessObjects.Compliance.Delegates
{
    /// <summary>
    /// delegate for Rules loaded.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void RuleLoadedHandler(Object sender, RuleLoadEventArgs e);

    /// <summary>
    /// For opening rules.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RuleOpenHandler(Object sender, SelectedNodeEventArgs e);

    /// <summary>
    /// For operations on rule Event raised from rule navigator to rule definition main.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void OperationsOnRuleHandler(Object sender, OperationsOnRuleEventArgs e);

    /// <summary>
    /// Event raised from handler to rule manager when operation completed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RuleOperationHandler(Object sender, RuleOperationEventArgs e);

    /// <summary>
    /// For Import Export operations of rule.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RuleImportExportHandler(Object sender, ImportExportEventArgs e);

    /// <summary>
    /// For updating Status bar.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void UpdateStatusBarHandler(Object sender, StatusBarEventArgs e);

    /// <summary>
    /// Raised when operation is done by other user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RuleOperationFromDifferentClientHandler(object sender, RuleOperationEventArgs e);

    /// <summary>
    ///  Event raised when guvnor completes save rule.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void RuleBrowerSaveCompleteHandle(object sender, BrowserLoadCompletedEventArgs e);

}
