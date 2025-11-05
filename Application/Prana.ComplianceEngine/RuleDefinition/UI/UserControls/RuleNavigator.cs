using Infragistics.Documents.Excel;
using Infragistics.Win;
using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects.Compliance.Constants;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.CommonDataCache;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.UI.UserControls
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RuleNavigator : UserControl
    {

        public event RuleOpenHandler OpenSelectedRule;
        public event OperationsOnRuleHandler OperateOnRule;

        // UltraToolTipInfo _toolTip = new UltraToolTipInfo();


        public RuleNavigator()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeContextMenu();
                }


            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add all Operations from Enum to Context Menu.
        /// </summary>
        private void InitializeContextMenu()
        {
            try
            {

                foreach (RuleOperations operation in Enum.GetValues(typeof(RuleOperations)))
                {
                    if (operation != RuleOperations.None && operation != RuleOperations.LoadRules && operation != RuleOperations.Build)
                    {
                        ToolStripItem menuItem = new ToolStripMenuItem();
                        menuItem.Name = operation.ToString();
                        menuItem.Text = SplitCamelCase(operation.ToString());
                        menuItem.Tag = operation.ToString();
                        contextMenuTree.Items.Add(menuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Expands Rule Tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnExpand_Click(object sender, EventArgs e)
        {
            try
            {
                ultraRuleTree.ExpandAll();
                MessageBox.Show("ExpandAll");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Collapse all tree node.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnCollapse_Click(object sender, EventArgs e)
        {
            try
            {
                ultraRuleTree.CollapseAll();
                MessageBox.Show("CollapseAll");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Loads all rule from esper and rule mediator.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ultraRuleTree.Refresh();
                MessageBox.Show("Refresh");
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add package and category nodes to Tree. 
        /// For package node key tag text all are package name.
        /// For Category node key is packageName_categoryName.
        /// </summary>
        /// <param name="packageName">if null then its root node</param>
        /// <param name="category">if null then its a package node</param>
        /// <param name="rule">null in case of package and category node.</param>
        internal void AddTreeNode(RulePackage packageName, RuleCategory category, RuleBase rule)
        {
            try
            {
                UltraTreeNode node = new UltraTreeNode();
                if (category.Equals(RuleCategory.None) && rule == null)
                {
                    UltraTreeNode rootNode = ultraRuleTree.GetNodeByKey("ComplianceRules");
                    node.Key = packageName.ToString();
                    node.Tag = packageName.ToString();
                    node.Text = SplitCamelCase(packageName.ToString());
                    node.Visible = GetCategoryNodePermission(packageName);
                    node.LeftImages.Add(this.complianceImageList.Images[RuleNavigatorConstants.CATEGORY_ICO]);
                    rootNode.Nodes.Add(node);
                }
                else if (!category.Equals(RuleCategory.None) && rule == null)
                {
                    UltraTreeNode rootNode = ultraRuleTree.GetNodeByKey(packageName.ToString());
                    node.Key = packageName.ToString() + "_" + category.ToString();
                    node.Tag = category.ToString();
                    node.Text = SplitCamelCase(category.ToString());
                    node.LeftImages.Add(this.complianceImageList.Images[RuleNavigatorConstants.CATEGORY_ICO]);

                    rootNode.Nodes.Add(node);
                }
                else
                {
                    UltraTreeNode rootNode = ultraRuleTree.GetNodeByKey(packageName.ToString() + "_" + category.ToString());
                    node.Key = rule.RuleId;
                    node.Tag = rule.RuleId;
                    node.Text = rule.RuleName;
                    rootNode.Nodes.Add(node);
                }
                UpdateTreeCount();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets visibility of node according to permissions.
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        private bool GetCategoryNodePermission(RulePackage package)
        {
            try
            {
                if (package.Equals(RulePackage.PostTrade))
                {
                    if (ComplianceCacheManager.GetPostComplianceModuleEnabled() && ComplianceCacheManager.GetPostTradeModuleEnabledForUser(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                        return true;
                    else
                        return false;
                }
                else if (package.Equals(RulePackage.PreTrade))
                {
                    if (ComplianceCacheManager.GetPreComplianceModuleEnabled() && ComplianceCacheManager.GetPreTradeModuleEnabledForUser(CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Splits package Name, items in context menu in camel case 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string SplitCamelCase(string input)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets package name category from the selected Node. Open rule on left click and set context menu on right click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleTree_MouseUp(object sender, MouseEventArgs e)
        {

            try
            {
                contextMenuTree.Visible = false;
                UltraTreeNode node = ultraRuleTree.GetNodeFromPoint(e.X, e.Y);
                RulePackage packageName = RulePackage.None;
                RuleCategory category = RuleCategory.None;
                String ruleName = null;
                if (node != null && !node.IsEditing)
                {
                    node.Selected = true;
                    if (!node.IsRootLevelNode)
                    {
                        if (node.Level == 1)
                        {
                            packageName = (RulePackage)Enum.Parse(typeof(RulePackage), node.Key);
                            category = RuleCategory.None;
                            ruleName = null;
                        }
                        else if (node.Level == 2)
                        {
                            packageName = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Key);
                            category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Tag.ToString());
                            ruleName = null;

                        }
                        else
                        {
                            packageName = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Parent.Key);
                            category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Parent.Tag.ToString());
                            ruleName = node.Text;

                        }

                    }
                    else
                    {
                        packageName = RulePackage.None;
                        category = RuleCategory.None;
                        ruleName = null;

                    }

                    if (e.Button == MouseButtons.Left)
                    {
                        OpenRule(packageName, category, ruleName, node.Level, node.Key);

                    }
                    else if (e.Button == MouseButtons.Right)
                    {

                        SetNodeContextMenu(packageName, category, node.Level, node.Tag.ToString());
                    }
                    //contextMenuTree.Show(ultraRuleTree, e.Location);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Raises Open rule Event with for the seleted tree node.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="category"></param>
        /// <param name="ruleName"></param>
        /// <param name="level">decides whether it is package Node category node or rule node.</param>
        /// <param name="ruleId"></param>
        private void OpenRule(RulePackage packageName, RuleCategory category, String ruleName, int level, String ruleId)
        {
            try
            {
                SelectedNodeEventArgs args = new SelectedNodeEventArgs();

                args.Category = category;
                args.PackageName = packageName;
                args.RuleName = ruleName;
                args.Level = level;
                args.RuleId = ruleId;

                if (OpenSelectedRule != null)
                    OpenSelectedRule(this, args);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Return all selected nodes of tree.
        /// </summary>
        /// <returns></returns>
        private SelectedNodesCollection GetSelectedNode()
        {
            try
            {
                return ultraRuleTree.SelectedNodes;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Sets the node context menu.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="category">The category.</param>
        /// <param name="level">The level.</param>
        /// <param name="enableState">State of the enable.</param>
        private void SetNodeContextMenu(RulePackage packageName, RuleCategory category, int level, String enableState)
        {

            try
            {
                int userId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                if (level == 0)
                {
                    contextMenuTree.Visible = false;
                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Visible = false;


                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Enabled = false;

                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Enabled = false;
                }
                else if (level == 1)
                {
                    contextMenuTree.Visible = true;
                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.DisableAllRules);
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.EnableAllRules);
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.ExportAllRules);
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Visible = false;

                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Enabled = ComplianceCacheManager.GetExportPermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Enabled = false;
                }
                else if (level == 2)
                {
                    contextMenuTree.Visible = true;
                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.DisableAllRules);
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.EnableAllRules);
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Visible = ShouldMenuItemBeAvailable(packageName, category, RuleOperations.ExportAllRules);
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Visible = true;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Visible = false;


                    contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Enabled = ComplianceCacheManager.GetExportPermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Enabled = ComplianceCacheManager.GetImportPermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Enabled = false;

                    if (category.Equals(RuleCategory.UserDefined))
                    {
                        contextMenuTree.Items[RuleOperations.AddRule.ToString()].Visible = true;
                        contextMenuTree.Items[RuleOperations.AddRule.ToString()].Enabled = ComplianceCacheManager.GetCreatePermission(packageName.ToString(), userId);
                    }
                    else
                    {
                        contextMenuTree.Items[RuleOperations.AddRule.ToString()].Visible = false;
                        contextMenuTree.Items[RuleOperations.AddRule.ToString()].Enabled = false;
                    }
                }
                else if (level == 3)
                {
                    contextMenuTree.Visible = true;
                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Visible = false;

                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Visible = false;

                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Visible = false;

                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Visible = true;
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Visible = false;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Visible = true;

                    contextMenuTree.Items[RuleOperations.AddRule.ToString()].Enabled = false;

                    contextMenuTree.Items[RuleOperations.DisableAllRules.ToString()].Enabled = false;

                    contextMenuTree.Items[RuleOperations.EnableAllRules.ToString()].Enabled = false;

                    contextMenuTree.Items[RuleOperations.ExportAllRules.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.ExportRule.ToString()].Enabled = ComplianceCacheManager.GetExportPermission(packageName.ToString(), userId);
                    contextMenuTree.Items[RuleOperations.ImportRule.ToString()].Enabled = false;
                    contextMenuTree.Items[RuleOperations.RenameRule.ToString()].Enabled = ComplianceCacheManager.GetRenamePermission(packageName.ToString(), userId);



                    if (enableState == "Enabled")
                    {
                        contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Visible = true;
                        contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                        contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Visible = false;
                        contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Enabled = false;
                    }

                    else if (enableState == "Disabled")
                    {
                        contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Visible = false;
                        contextMenuTree.Items[RuleOperations.DisableRule.ToString()].Enabled = false;
                        contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Visible = true;
                        contextMenuTree.Items[RuleOperations.EnableRule.ToString()].Enabled = ComplianceCacheManager.GetEnablePermission(packageName.ToString(), userId);
                    }



                    if (category.Equals(RuleCategory.UserDefined))
                    {
                        contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Visible = true;
                        contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Enabled = ComplianceCacheManager.GetDeletePermission(packageName.ToString(), userId);
                    }
                    else
                    {
                        contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Visible = false;
                        contextMenuTree.Items[RuleOperations.DeleteRule.ToString()].Enabled = false;
                    }

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Apply operation with respect to the context menu item clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuTree_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                RuleOperations operation = (RuleOperations)Enum.Parse(typeof(RuleOperations), e.ClickedItem.Name, true);
                switch (operation)
                {
                    case RuleOperations.AddRule:
                        AddUserDefinedRules();
                        break;
                    case RuleOperations.DeleteRule:
                        DeleteRule();
                        break;
                    case RuleOperations.DisableAllRules:
                        if ((ultraRuleToolBar.Toolbars["Operation"].ToolbarsManager.Tools["TextSearch"] as Infragistics.Win.UltraWinToolbars.TextBoxTool).Text != String.Empty)
                        {
                            DialogResult dr = MessageBox.Show(this, "All rules will be Disabled, not the filtered one. Do you want to continue?", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                EnableDisableAllRules(true);
                            }
                        }
                        else
                            EnableDisableAllRules(true);
                        break;
                    case RuleOperations.DisableRule:
                        EnableDisableAllRules(true);
                        break;
                    case RuleOperations.EnableAllRules:
                        if ((ultraRuleToolBar.Toolbars["Operation"].ToolbarsManager.Tools["TextSearch"] as Infragistics.Win.UltraWinToolbars.TextBoxTool).Text != String.Empty)
                        {
                            DialogResult dr = MessageBox.Show(this, "All rules will be Enabled, not the filtered one. Do you want to continue?", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                EnableDisableAllRules(false);
                            }
                        }
                        else
                            EnableDisableAllRules(false);
                        break;
                    case RuleOperations.EnableRule:
                        EnableDisableAllRules(false);
                        break;
                    case RuleOperations.ExportAllRules:
                    case RuleOperations.ExportRule:
                        RaiseExportRuleEvent();
                        break;
                    case RuleOperations.ImportRule:
                        RaiseImportRuleEvent();
                        break;
                    case RuleOperations.RenameRule:
                        RenameRule();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RaiseImportRuleEvent()
        {
            try
            {
                RulePackage package = RulePackage.None;
                RuleCategory category = RuleCategory.None;
                //String ruleId = String.Empty;

                SelectedNodesCollection coll = GetSelectedNode();
                foreach (UltraTreeNode node in coll)
                {
                    if (node.Level == 1)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Key, true);
                        category = RuleCategory.None;
                        //  ruleId = String.Empty;
                    }
                    if (node.Level == 2)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Tag.ToString(), true);
                        //ruleId = String.Empty;
                    }


                    OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();
                    args.Category = category;
                    args.PackageName = package;
                    args.RuleName = String.Empty;
                    args.Level = -1;
                    args.RuleId = String.Empty;
                    args.OperationType = RuleOperations.ImportRule;

                    if (OperateOnRule != null)
                        OperateOnRule(this, args);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Raises Export Event for exporting rule
        /// Contains 3 properties in event argument
        /// package name, category,ruleName
        /// If selected node is package node then category and rule name is none and empty respectively
        /// else is category node then rule name is empty
        /// else if rule node then all parameters contains value.
        /// Rule definition main is listening to this event.
        /// </summary>
        private void RaiseExportRuleEvent()
        {
            try
            {
                RulePackage package = RulePackage.None;
                RuleCategory category = RuleCategory.None;
                String ruleId = String.Empty;

                SelectedNodesCollection coll = GetSelectedNode();
                foreach (UltraTreeNode node in coll)
                {
                    if (node.Level == 1)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Key, true);
                        category = RuleCategory.None;
                        ruleId = String.Empty;
                    }
                    if (node.Level == 2)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Tag.ToString(), true);
                        ruleId = String.Empty;
                    }
                    if (node.Level == 3)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Parent.Tag.ToString(), true);
                        ruleId = node.Key;
                    }

                    OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();
                    args.Category = category;
                    args.PackageName = package;
                    args.RuleName = String.Empty;
                    args.Level = -1;
                    args.RuleId = ruleId;
                    args.OperationType = RuleOperations.ExportRule;

                    if (OperateOnRule != null)
                        OperateOnRule(this, args);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Send Request for enabling and disabling rules.
        /// set Tag of node as PendingEnable or PendingDisable.
        /// </summary>
        /// <param name="isDisable"></param>
        private void EnableDisableAllRules(bool isDisable)
        {
            try
            {

                // List<RuleBase> ruleList = new List<RuleBase>();
                SelectedNodesCollection nodeCollection = GetSelectedNode();
                RulePackage package = RulePackage.None;
                RuleCategory category = RuleCategory.None; ;
                String ruleName = "";
                foreach (UltraTreeNode node in nodeCollection)
                {
                    if (node.Level == 1)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Key, true);
                        category = RuleCategory.None;
                        ruleName = String.Empty;
                    }
                    else if (node.Level == 2)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Tag.ToString(), true);
                        ruleName = String.Empty;
                    }
                    else if (node.Level == 3)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Parent.Tag.ToString(), true);
                        ruleName = node.Text;
                        if (isDisable)
                        {
                            node.Tag = RuleNavigatorConstants.DISABLE_TREE_NODE;

                        }
                        else
                        {
                            node.Tag = RuleNavigatorConstants.ENABLE_TREE_NODE;

                        }
                    }

                    OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();
                    if (isDisable)
                    {

                        args.OperationType = RuleOperations.DisableRule;
                    }
                    else
                    {

                        args.OperationType = RuleOperations.EnableRule;
                    }

                    args.Category = category;
                    args.PackageName = package;
                    args.RuleName = ruleName;
                    args.Level = node.Level;
                    args.RuleId = node.Key;

                    if (OperateOnRule != null)
                        OperateOnRule(this, args);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        /// <summary>
        /// Change Tag to RuleId_RuleOldName, key to PendingRename.
        /// </summary>
        private void RenameRule()
        {
            try
            {
                SelectedNodesCollection nodeCollection = GetSelectedNode();
                UltraTreeNode node;
                if (nodeCollection.Count == 1)
                {
                    node = nodeCollection[0];
                    node.Tag = node.Key + "_" + node.Tag + "_" + node.Text;
                    node.Key = RuleNavigatorConstants.RENAME_TREE_NODE;
                    ultraRuleTree.GetNodeByKey(RuleNavigatorConstants.RENAME_TREE_NODE).BeginEdit();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Can delete multiple rules. Raises OperateOnRule Event with operation type Delete
        /// </summary>
        private void DeleteRule()
        {
            try
            {
                SelectedNodesCollection nodeCollection = GetSelectedNode();

                StringBuilder rulesToDelete = new StringBuilder();
                rulesToDelete.Append("Do you want to Delete rule: ");
                for (int i = 0; i < nodeCollection.Count; i++)
                {
                    UltraTreeNode node = nodeCollection[i];
                    rulesToDelete.Append(node.Text);
                    if ((nodeCollection.Count - i) > 1)
                        rulesToDelete.Append(", ");
                }
                DialogResult result = MessageBox.Show(this, rulesToDelete.ToString(), "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (UltraTreeNode node in nodeCollection)
                    {
                        node.Tag = RuleNavigatorConstants.DELETE_TREE_NODE;
                        OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();

                        args.Category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Parent.Tag.ToString(), true);
                        args.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Parent.Key, true);
                        args.RuleName = node.Text;
                        args.Level = node.Level;
                        args.RuleId = node.Key;
                        args.OperationType = RuleOperations.DeleteRule;
                        if (OperateOnRule != null)
                            OperateOnRule(this, args);
                    }
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Add new tree node in tree in editable mode.
        /// </summary>
        private void AddUserDefinedRules()
        {
            try
            {
                SelectedNodesCollection nodeCollection = GetSelectedNode();
                UltraTreeNode parentNode;//=new UltraTreeNode();
                if (nodeCollection.Count == 1)
                {
                    parentNode = nodeCollection[0];
                    UltraTreeNode node = new UltraTreeNode();
                    node.Key = RuleNavigatorConstants.NEW_TREE_NODE;
                    node.Tag = RuleNavigatorConstants.NEW_TREE_NODE;
                    node.Text = GetRuleName("NewRule", (RulePackage)Enum.Parse(typeof(RulePackage), parentNode.Parent.Key));
                    parentNode.Nodes.Add(node);
                    ultraRuleTree.GetNodeByKey(RuleNavigatorConstants.NEW_TREE_NODE).BeginEdit();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        long _newRuleCounter = 1000;

        /// <summary>
        /// Returns rule name when new user defined rule is added.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="rulePackage"></param>
        /// <returns></returns>
        private string GetRuleName(String ruleName, RulePackage rulePackage)
        {
            try
            {
                if (RuleExist(ruleName, rulePackage, false))
                {
                    String newRuleName = "";
                    bool newRuleNameFound = false;
                    while (!newRuleNameFound)
                    {
                        newRuleName = ruleName + "_" + _newRuleCounter++;
                        if (RuleExist(newRuleName, rulePackage, false))
                            newRuleNameFound = false;
                        else
                            newRuleNameFound = true;
                    }
                    return newRuleName;
                }
                else
                    return ruleName;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets rule Id for all selected rules
        /// </summary>
        /// <returns></returns>
        internal List<String> GetRuleId()
        {
            try
            {
                List<String> ruleIdList = new List<string>();
                //String ruleId = (-1).ToString();
                SelectedNodesCollection nodes = GetSelectedNode();
                if (nodes.Count >= 1)
                {
                    foreach (UltraTreeNode node in nodes)
                    {
                        if (node.Level >= 3)
                            ruleIdList.Add(node.Key);
                    }
                }
                return ruleIdList;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Raised in case of rename and create rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleTree_AfterLabelEdit(object sender, NodeEventArgs e)
        {
            try
            {
                UltraTreeNode newNode = e.TreeNode;
                e.TreeNode.EndEdit(true);
                if (!String.IsNullOrEmpty(newNode.Text))
                {
                    if (ValidateRuleName(newNode.Text))
                    {
                        if (!RuleExist(newNode.Text, (RulePackage)Enum.Parse(typeof(RulePackage), newNode.Parent.Parent.Key), true))
                        {

                            if (newNode.Key == RuleNavigatorConstants.NEW_TREE_NODE)
                            {
                                DialogResult result = MessageBox.Show(this, "Do you want to create rule " + newNode.Text + "?", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                // e.TreeNode.Key = newNode.Text;
                                if (DialogResult.Yes == result)
                                {
                                    OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();

                                    args.Category = (RuleCategory)Enum.Parse(typeof(RuleCategory), newNode.Parent.Tag.ToString(), true);
                                    args.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), newNode.Parent.Parent.Key, true);
                                    args.RuleName = newNode.Text;
                                    args.Level = newNode.Level;
                                    args.OperationType = RuleOperations.AddRule;
                                    if (OperateOnRule != null)
                                        OperateOnRule(this, args);
                                }
                                else
                                {
                                    e.TreeNode.Remove();
                                }
                            }
                            else if (newNode.Key == RuleNavigatorConstants.RENAME_TREE_NODE)
                            {
                                String[] oldRuleDetails = newNode.Tag.ToString().Split('_');

                                StringBuilder ruleName = new StringBuilder();
                                for (int i = 2; i < oldRuleDetails.Length; i++)
                                {
                                    ruleName.Append(oldRuleDetails[i]);
                                    ruleName.Append("_");
                                }
                                String oldRuleName = ruleName.Remove(ruleName.Length - 1, 1).ToString();
                                String ruleId = oldRuleDetails[0];
                                if (oldRuleName == newNode.Text)
                                {
                                    newNode.Text = oldRuleName;
                                    newNode.Key = ruleId;
                                    newNode.Tag = ruleId;
                                }
                                else
                                {
                                    DialogResult result = MessageBox.Show(this, "Do you want to rename rule " + oldRuleName + " to " + newNode.Text + "?", "Nirvana Compliance", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    // e.TreeNode.Key = newNode.Text;
                                    if (DialogResult.Yes == result)
                                    {
                                        OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();

                                        args.Category = (RuleCategory)Enum.Parse(typeof(RuleCategory), newNode.Parent.Tag.ToString(), true);
                                        args.PackageName = (RulePackage)Enum.Parse(typeof(RulePackage), newNode.Parent.Parent.Key, true);
                                        args.RuleName = newNode.Text;
                                        args.Level = newNode.Level;
                                        args.RuleId = ruleId;
                                        args.OldRuleName = oldRuleName;
                                        args.OperationType = RuleOperations.RenameRule;
                                        if (OperateOnRule != null)
                                            OperateOnRule(this, args);
                                    }
                                    else
                                    {
                                        newNode.Text = oldRuleName;
                                        newNode.Key = ruleId;
                                        newNode.Tag = ruleId;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "Rule name already exists.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            if (newNode.Key.ToString().Equals(RuleNavigatorConstants.NEW_TREE_NODE))
                            {
                                e.TreeNode.Remove();
                                //AddUserDefinedRules();
                            }
                            else if (newNode.Key.ToString().Equals(RuleNavigatorConstants.RENAME_TREE_NODE))
                            {
                                String[] oldRuleDetails = newNode.Tag.ToString().Split('_');
                                StringBuilder ruleName = new StringBuilder();
                                for (int i = 2; i < oldRuleDetails.Length; i++)
                                {
                                    ruleName.Append(oldRuleDetails[i]);
                                    ruleName.Append("_");
                                }
                                e.TreeNode.Text = ruleName.Remove(ruleName.Length - 1, 1).ToString();
                                e.TreeNode.Key = oldRuleDetails[0];
                                e.TreeNode.Tag = oldRuleDetails[1];
                                e.TreeNode.Selected = true;
                                // RenameRule();
                            }
                            //e.TreeNode.Selected = true;
                            // e.TreeNode.BeginEdit();
                            //newNode.EndEdit(false);
                            //newNode.BeginEdit();
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Rule name can only have alphanumeric characters, space and @ _ . - & $ % special characters and can not start/end with special characters.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (newNode.Key.ToString().Equals(RuleNavigatorConstants.NEW_TREE_NODE))
                        {
                            e.TreeNode.Remove();
                            // AddUserDefinedRules();
                        }
                        else if (newNode.Key.ToString().Equals(RuleNavigatorConstants.RENAME_TREE_NODE))
                        {
                            String[] oldRuleDetails = newNode.Tag.ToString().Split('_');
                            StringBuilder ruleName = new StringBuilder();
                            for (int i = 2; i < oldRuleDetails.Length; i++)
                            {
                                ruleName.Append(oldRuleDetails[i]);
                                ruleName.Append("_");
                            }
                            e.TreeNode.Text = ruleName.Remove(ruleName.Length - 1, 1).ToString();
                            e.TreeNode.Key = oldRuleDetails[0];
                            e.TreeNode.Tag = oldRuleDetails[1];
                            e.TreeNode.Selected = true;
                            //RenameRule();
                        }
                        //newNode.EndEdit(false);
                        //newNode.BeginEdit();
                    }

                }
                else
                {
                    if (newNode.Key.ToString().Equals(RuleNavigatorConstants.NEW_TREE_NODE))
                    {
                        e.TreeNode.Remove();
                    }
                    else if (newNode.Key.ToString().Equals(RuleNavigatorConstants.RENAME_TREE_NODE))
                    {
                        String[] oldRuleDetails = newNode.Tag.ToString().Split('_');

                        StringBuilder ruleName = new StringBuilder();
                        for (int i = 2; i < oldRuleDetails.Length; i++)
                        {
                            ruleName.Append(oldRuleDetails[i]);
                            ruleName.Append("_");
                        }

                        e.TreeNode.Text = ruleName.Remove(ruleName.Length - 1, 1).ToString();
                        e.TreeNode.Key = oldRuleDetails[0];
                        e.TreeNode.Tag = oldRuleDetails[1];
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks if rule name already exists or not
        /// </summary>
        /// <param name="ruleName"></param>
        /// <param name="rulePackage"></param>
        /// <returns></returns>
        private bool RuleExist(string ruleName, RulePackage rulePackage, bool isLabelEdit)
        {
            try
            {
                UltraTreeNode rootNode = ultraRuleTree.Nodes[0];
                int count = 0;
                if (rootNode.Nodes.Exists(rulePackage.ToString()))
                {
                    UltraTreeNode packageNode = ultraRuleTree.Nodes[0].Nodes[rulePackage.ToString()];
                    foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                    {
                        if (packageNode.Nodes.Exists(rulePackage.ToString() + "_" + category.ToString()))
                        {
                            UltraTreeNode categoryNode = packageNode.Nodes[rulePackage.ToString() + "_" + category.ToString()];

                            foreach (UltraTreeNode node in categoryNode.Nodes)
                            {
                                if (ruleName == node.Text)
                                    count++;
                                if (isLabelEdit)
                                {
                                    if (count == 2)
                                        return true;
                                }
                                else
                                {
                                    if (count == 1)
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Validate rule name against regular expression.
        /// </summary>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        private bool ValidateRuleName(string ruleName)
        {
            try
            {
                Regex regex = new Regex(@"^[a-zA-Z0-9][-@\$%&_.a-zA-Z0-9 ]*[a-zA-Z0-9]$");

                if (regex.IsMatch(ruleName))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Updates tree after operation completes.
        /// </summary>
        /// <param name="ruleList"></param>
        /// <param name="operationType"></param>
        internal void UpdateTreeNode(List<RuleBase> ruleList, List<RuleBase> failedRuleList, RuleOperations operationType)
        {
            try
            {

                switch (operationType)
                {
                    case RuleOperations.AddRule:
                        UltraTreeNode node = ultraRuleTree.GetNodeByKey(RuleNavigatorConstants.NEW_TREE_NODE);
                        if (node != null)
                        {
                            //node = ultraRuleTree.Nodes[RuleNavigatorConstants.NEW_TREE_NODE];
                            node.Key = ruleList[0].RuleId;
                            node.Tag = GetRuleTag(ruleList[0]);
                            node.Selected = true;
                            node.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(ruleList[0])]);
                            OpenRule(ruleList[0].Package, ruleList[0].Category, ruleList[0].RuleName, node.Level, node.Key);
                            Logger.LoggerWrite(node.Text + " Rule Added by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                        }
                        else
                        {
                            AddTreeNode(ruleList);
                        }

                        break;
                    case RuleOperations.DeleteRule:
                        foreach (RuleBase rule in ruleList)
                        {
                            UltraTreeNode deleteNode = ultraRuleTree.GetNodeByKey(rule.RuleId);
                            UltraTreeNode parentNode = deleteNode.Parent;
                            if (deleteNode != null)
                            {
                                deleteNode.Remove();
                                Logger.LoggerWrite(deleteNode.Text + " Rule Deleted by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                            }
                            parentNode.Selected = true;
                        }
                        break;
                    case RuleOperations.RenameRule:
                        UltraTreeNode renameNode = ultraRuleTree.GetNodeByKey(RuleNavigatorConstants.RENAME_TREE_NODE);
                        String[] oldRuleDetails = renameNode.Tag.ToString().Split('_');

                        StringBuilder ruleName = new StringBuilder();
                        for (int i = 2; i < oldRuleDetails.Length; i++)
                        {
                            ruleName.Append(oldRuleDetails[i]);
                            ruleName.Append("_");
                        }
                        String oldRuleName = ruleName.Remove(ruleName.Length - 1, 1).ToString();
                        if (renameNode != null)
                        {
                            renameNode.Text = ruleList[0].RuleName;
                            renameNode.Key = ruleList[0].RuleId;
                            renameNode.Tag = GetRuleTag(ruleList[0]);
                            renameNode.Selected = true;
                            renameNode.LeftImages.Clear();
                            renameNode.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(ruleList[0])]);
                            Logger.LoggerWrite("\"" + oldRuleName + "\" Rule Renamed to \"" + renameNode.Text + "\" by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                        }
                        OpenRule(ruleList[0].Package, ruleList[0].Category, ruleList[0].RuleName, 3, ruleList[0].RuleId);
                        break;
                    case RuleOperations.EnableRule:
                        foreach (RuleBase rule in ruleList)
                        {
                            UltraTreeNode enableNode = ultraRuleTree.GetNodeByKey(rule.RuleId);
                            if (enableNode != null)
                            {
                                enableNode.Tag = GetRuleTag(rule);
                                enableNode.LeftImages.Clear();
                                enableNode.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(rule)]);
                                Logger.LoggerWrite(enableNode.Text + " Rule Enabled by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");

                            }
                        }
                        foreach (RuleBase rule in failedRuleList)
                        {
                            UltraTreeNode enableNode = ultraRuleTree.GetNodeByKey(rule.RuleId);
                            if (enableNode != null)
                            {
                                enableNode.Tag = GetRuleTag(rule);
                                enableNode.LeftImages.Clear();
                                enableNode.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(rule)]);
                                Logger.LoggerWrite(enableNode.Text + " Rule Enabled failed by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");

                            }
                        }

                        break;
                    case RuleOperations.DisableRule:
                        foreach (RuleBase rule in ruleList)
                        {
                            UltraTreeNode disableNode = ultraRuleTree.GetNodeByKey(rule.RuleId);
                            if (disableNode != null)
                            {
                                disableNode.Tag = GetRuleTag(rule);
                                disableNode.LeftImages.Clear();
                                disableNode.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(rule)]);
                                Logger.LoggerWrite(disableNode.Text + " Rule Disabled by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                            }
                        }
                        break;
                    case RuleOperations.ImportRule:
                        if (ruleList.Count > 0)
                        {
                            AddTreeNode(ruleList);
                            string str = string.Join(", ", from item in ruleList select item.RuleName);
                            Logger.LoggerWrite(str + " Rules(Count-" + ruleList.Count + ") Imported by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                            MessageBox.Show(this, ruleList.Count + " rules imported.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (failedRuleList.Count > 0)
                        {
                            string str = string.Join(", ", from item in failedRuleList select item.RuleName);
                            Logger.LoggerWrite(str + " Rules(Count-" + failedRuleList.Count + ") Import failed by user " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                        }
                        break;
                    case RuleOperations.Build:
                        foreach (RuleBase rule in failedRuleList)
                        {
                            UltraTreeNode disableNode = ultraRuleTree.GetNodeByKey(rule.RuleId);
                            if (disableNode != null)
                            {
                                disableNode.Tag = GetRuleTag(rule);
                                disableNode.LeftImages.Clear();
                                disableNode.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(rule)]);
                                Logger.LoggerWrite(disableNode.Text + " Rule Disabled as build failed for the rule while saving by user: " + CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.ShortName, LoggingConstants.LOG_CATEGORY_UI, 1, 1, TraceEventType.Information, "RuleOperation");
                            }
                        }
                        break;
                }
                UpdateTreeCount();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds count of number of rules to tree nodes.
        /// </summary>
        private void UpdateTreeCount()
        {
            try
            {
                foreach (UltraTreeNode node in ultraRuleTree.Nodes)
                {
                    int countRoot = 0;
                    foreach (UltraTreeNode packageNode in node.Nodes)
                    {
                        if (packageNode.Visible)
                        {
                            int countPackage = 0;
                            foreach (UltraTreeNode categoryNode in packageNode.Nodes)
                            {
                                if (categoryNode.Level == 2)
                                {
                                    //Adds count to Category node
                                    //Number of rules in Category
                                    categoryNode.Text = SplitCamelCase(categoryNode.Tag.ToString()) + " (" + categoryNode.Nodes.Count + ")";
                                    countPackage += categoryNode.Nodes.Count;
                                }
                            }
                            if (packageNode.Level == 1)
                            {
                                //Adds count to package node
                                //Number of rules in package
                                packageNode.Text = SplitCamelCase(packageNode.Tag.ToString()) + " (" + countPackage + ")";
                                countRoot += countPackage;
                            }
                        }
                    }
                    //Adds count to compliance rules (root node)
                    node.Text = SplitCamelCase(node.Tag.ToString()) + " (" + countRoot + ")";

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Overload for adding tree node to tree. add list of rules on tree.
        /// </summary>
        /// <param name="list"></param>
        internal void AddTreeNode(List<RuleBase> list)
        {
            try
            {
                foreach (RuleBase rule in list)
                {
                    UltraTreeNode node = new UltraTreeNode();
                    if (rule.Package != RulePackage.None && rule.Category != RuleCategory.None)
                    {
                        UltraTreeNode rootNode = ultraRuleTree.GetNodeByKey(rule.Package.ToString() + "_" + rule.Category.ToString());
                        node.Key = rule.RuleId;
                        node.Tag = GetRuleTag(rule);
                        node.Text = rule.RuleName;
                        node.LeftImages.Add(this.complianceImageList.Images[GetNodeImageKey(rule)]);

                        // _toolTip.ToolTipText = node.Tag.ToString();
                        if (!rootNode.Nodes.Exists(node.Key))
                            rootNode.Nodes.Add(node);
                    }
                }
                UpdateTreeCount();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets enable disable image for rule.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private string GetNodeImageKey(RuleBase rule)
        {
            try
            {
                if (rule.Enabled)
                    return RuleNavigatorConstants.ENABLE_ICO;
                else
                    return RuleNavigatorConstants.DISABLE_ICO;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Sets Tag of the tree node according to rule enable state.
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        private string GetRuleTag(RuleBase rule)
        {
            try
            {
                if (rule.Enabled)
                    return "Enabled";
                else
                    return "Disabled";
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return String.Empty;
            }
        }

        /// <summary>
        /// Sets tooltip text for treenode on mouse hover.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleTree_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            //try
            //{
            //    if (e.Element is NodeTextUIElement)
            //    {
            //        Point pt = new Point(e.Element.Rect.X, e.Element.Rect.Y);
            //        UltraTreeNode node = this.ultraRuleTree.GetNodeFromPoint(pt);
            //        if (node != null)
            //        {
            //            if (!node.IsEditing)
            //            {
            //                UltraToolTipInfo tipInfo = new UltraToolTipInfo(node.Tag.ToString(), ToolTipImage.Default, node.Text, DefaultableBoolean.True);
            //                this.ultraToolTipManager1.SetUltraToolTip(this.ultraRuleTree, tipInfo);
            //                this.ultraToolTipManager1.ShowToolTip(this.ultraRuleTree);
            //                this.ultraToolTipManager1.GetUltraToolTip(this.ultraRuleTree);
            //            }
            //        }
            //    }
            //    else
            //        this.ultraToolTipManager1.HideToolTip();

            //}
            //catch (Exception ex)
            //{
            //    // Invoke our policy that is responsible for making sure no secure information
            //    // gets out of our layer.
            //    bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}
        }

        /// <summary>
        /// Searches in rule tree on value change of text box. 
        /// show only node which contains text in textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraTextSearch_ValueChanged(object sender, EventArgs e)
        {
            try
            {

                foreach (RulePackage packageName in Enum.GetValues(typeof(RulePackage)))
                {
                    if (packageName.Equals(RulePackage.None))
                        continue;
                    else
                    {
                        foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                        {
                            if (category.Equals(RuleCategory.None))
                                continue;
                            else
                            {
                                foreach (UltraTreeNode node in ultraRuleTree.GetNodeByKey(packageName.ToString() + "_" + category.ToString()).Nodes)
                                {
                                    if (node != null)
                                    {
                                        if (ultraTextSearch.Text.Length > 0)
                                        {
                                            if (node.Text.ToLower().Contains(ultraTextSearch.Text.ToLower()))
                                                node.Visible = true;
                                            else
                                                node.Visible = false;
                                        }
                                        else
                                            node.Visible = true;
                                    }
                                }
                            }
                        }
                    }
                }
                UpdateTreeCount();
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Event raised when tool in toolbar is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleToolBar_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Expand":    // ButtonTool
                        ultraRuleTree.ExpandAll();
                        //MessageBox.Show("ExpandAll");
                        break;

                    case "Collapse":    // ButtonTool
                        ultraRuleTree.CollapseAll();
                        //MessageBox.Show("ExpandAll");
                        break;

                    case "Refresh":    // ButtonTool
                        ultraRuleTree.Nodes[0].Nodes.Clear();
                        OperationsOnRuleEventArgs args = new OperationsOnRuleEventArgs();
                        args.OperationType = RuleOperations.LoadRules;
                        if (OperateOnRule != null)
                            OperateOnRule(this, args);
                        break;

                    case "SortA-Z":    // ButtonTool
                        ultraRuleTree.Override.Sort = SortType.Ascending;
                        ultraRuleTree.RefreshSort();
                        break;

                    case "SortZ-A":    // ButtonTool
                        ultraRuleTree.Override.Sort = SortType.Descending;
                        ultraRuleTree.RefreshSort();
                        break;



                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Event raised tree node is selected.
        /// Either it is with mouse or keyboard.
        /// scrolling rule tree with keys will open rule.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleTree_AfterSelect(object sender, SelectEventArgs e)
        {
            try
            {
                RulePackage package = RulePackage.None;
                RuleCategory category = RuleCategory.None;
                String ruleId = String.Empty;

                SelectedNodesCollection nodeCollection = e.NewSelections;
                foreach (UltraTreeNode node in nodeCollection)
                {
                    if (node.Level == 1)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Key, true);
                        category = RuleCategory.None;
                        ruleId = String.Empty;
                    }
                    if (node.Level == 2)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Tag.ToString(), true);
                        ruleId = String.Empty;
                    }
                    if (node.Level == 3)
                    {
                        package = (RulePackage)Enum.Parse(typeof(RulePackage), node.Parent.Parent.Key, true);
                        category = (RuleCategory)Enum.Parse(typeof(RuleCategory), node.Parent.Tag.ToString(), true);
                        ruleId = node.Key;
                    }
                    OpenRule(package, category, node.Text, node.Level, ruleId);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            //OpenRule(
        }


        /// <summary>
        /// Returns true if rule modified by other user is selected by current user for operation.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        internal Dictionary<String, bool> GetIfRuleSelected(List<RuleBase> list)
        {

            try
            {
                Dictionary<String, bool> selectedRules = new Dictionary<string, bool>();
                SelectedNodesCollection nodeCollection = GetSelectedNode();
                foreach (UltraTreeNode node in nodeCollection)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (node.Key == rule.RuleId)
                            if (!selectedRules.ContainsKey(rule.RuleId))
                                selectedRules.Add(rule.RuleId, true);
                    }
                }
                return selectedRules;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Reloads tree when rule operation is done by different client.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="oldValue"></param>
        internal void ReloadTree(List<RuleBase> list, RuleOperations ruleOperations, string oldValue, string reseletionRequiredRuleId)
        {
            try
            {
                switch (ruleOperations)
                {
                    case RuleOperations.AddRule:
                        AddTreeNode(list);
                        break;
                    case RuleOperations.DeleteRule:
                        foreach (RuleBase rule in list)
                        {
                            if (rule != null)
                            {
                                UltraTreeNode node = ultraRuleTree.GetNodeByKey(rule.RuleId);
                                node.Remove();
                            }
                        }
                        UpdateTreeCount();
                        break;
                    case RuleOperations.RenameRule:
                        foreach (RuleBase rule in list)
                        {
                            UltraTreeNode node = ultraRuleTree.GetNodeByKey(oldValue);
                            node.Remove();
                        }
                        AddTreeNode(list);
                        break;
                    case RuleOperations.EnableRule:
                    case RuleOperations.DisableRule:
                    case RuleOperations.Build:
                        foreach (RuleBase rule in list)
                        {
                            if (rule != null)
                            {
                                UltraTreeNode node = ultraRuleTree.GetNodeByKey(rule.RuleId);
                                node.Remove();
                            }
                        }
                        AddTreeNode(list);
                        break;
                    case RuleOperations.ImportRule:
                        AddTreeNode(list);
                        break;
                }

                if (reseletionRequiredRuleId != string.Empty)
                {
                    UltraTreeNode node = ultraRuleTree.GetNodeByKey(reseletionRequiredRuleId);
                    if (node != null)
                        ultraRuleTree.GetNodeByKey(reseletionRequiredRuleId).Selected = true;
                    else
                        ultraRuleTree.Nodes["ComplianceRules"].Selected = true;

                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }

        /// <summary>
        /// Get if rule is selected based on old rule id
        /// </summary>
        /// <param name="oldRuleId"></param>
        /// <returns>true if rule is open</returns>
        internal bool GetIfRuleSelectedFromOldValue(string oldRuleId)
        {
            try
            {
                SelectedNodesCollection nodeCollection = GetSelectedNode();
                foreach (UltraTreeNode node in nodeCollection)
                {
                    if (node.Key == oldRuleId)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
                return false;
            }
        }

        /// <summary>
        /// Searches in rule tree on value change of text box. 
        /// show only node which contains text in text toolbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraRuleToolBar_ToolValueChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            try
            {
                if (e.Tool.Key.Equals("TextSearch"))
                {
                    String searchText = (e.Tool as Infragistics.Win.UltraWinToolbars.TextBoxTool).Text;
                    foreach (RulePackage packageName in Enum.GetValues(typeof(RulePackage)))
                    {
                        if (packageName.Equals(RulePackage.None))
                            continue;
                        else
                        {
                            foreach (RuleCategory category in Enum.GetValues(typeof(RuleCategory)))
                            {
                                if (category.Equals(RuleCategory.None))
                                    continue;
                                else
                                {
                                    foreach (UltraTreeNode node in ultraRuleTree.GetNodeByKey(packageName.ToString() + "_" + category.ToString()).Nodes)
                                    {
                                        if (node != null)
                                        {
                                            if (searchText.Length > 0)
                                            {
                                                if (node.Text.ToLower().Contains(searchText.ToLower()))
                                                    node.Visible = true;
                                                else
                                                    node.Visible = false;
                                            }
                                            else
                                                node.Visible = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    UpdateTreeCount();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Checks if the perticular contex menu option should be available or not for the current node
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="category"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        private Boolean ShouldMenuItemBeAvailable(RulePackage packageName, RuleCategory category, RuleOperations operation)
        {
            try
            {
                // first of all get the node
                UltraTreeNode node = new UltraTreeNode();
                if (category.Equals(RuleCategory.None))
                {
                    node = ultraRuleTree.GetNodeByKey(packageName.ToString());
                }
                else if (!category.Equals(RuleCategory.None))
                {
                    node = ultraRuleTree.GetNodeByKey(packageName.ToString() + "_" + category.ToString());
                }

                if (node == null)
                    return false;

                switch (operation)
                {
                    case RuleOperations.DisableAllRules:
                        if (node.Level == 1)
                        {
                            foreach (UltraTreeNode N in node.Nodes)
                            {
                                foreach (UltraTreeNode x in N.Nodes)
                                {
                                    if (x.Tag.Equals("Enabled"))
                                        return true;
                                }
                            }
                        }
                        else if (node.Level == 2)
                        {
                            foreach (UltraTreeNode x in node.Nodes)
                            {
                                if (x.Tag.Equals("Enabled"))
                                    return true;
                            }
                        }
                        break;
                    case RuleOperations.EnableAllRules:
                        if (node.Level == 1)
                        {
                            foreach (UltraTreeNode N in node.Nodes)
                            {
                                foreach (UltraTreeNode x in N.Nodes)
                                {
                                    if (x.Tag.Equals("Disabled"))
                                        return true;
                                }
                            }
                        }
                        else if (node.Level == 2)
                        {
                            foreach (UltraTreeNode x in node.Nodes)
                            {
                                if (x.Tag.Equals("Disabled"))
                                    return true;
                            }
                        }
                        break;
                    case RuleOperations.ExportAllRules:
                        if (node.Level == 1)
                        {
                            if ((node.GetNodeCount(true) - node.GetNodeCount(false)) > 0)
                                return true;
                        }
                        else if (node.Level == 2)
                        {
                            if (node.GetNodeCount(true) > 0)
                                return true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        private void RuleNavigator_Load(object sender, EventArgs e)
        {
            try
            {
                if (!CustomThemeHelper.ApplyTheme)
                {
                    SetAppearanceWithoutTheme();
                }
                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                ultraBtnRefresh.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnRefresh.ForeColor = System.Drawing.Color.White;
                ultraBtnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnRefresh.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnRefresh.UseAppStyling = false;
                ultraBtnRefresh.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnCollapse.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnCollapse.ForeColor = System.Drawing.Color.White;
                ultraBtnCollapse.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnCollapse.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnCollapse.UseAppStyling = false;
                ultraBtnCollapse.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnExpand.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnExpand.ForeColor = System.Drawing.Color.White;
                ultraBtnExpand.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnExpand.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnExpand.UseAppStyling = false;
                ultraBtnExpand.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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


        private void SetAppearanceWithoutTheme()
        {
            try
            {
                Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
                // Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();


                appearance1.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlOperation.Appearance = appearance1;

                appearance2.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlMain.Appearance = appearance2;

                appearance3.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlTree.Appearance = appearance3;

                appearance4.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraRuleTree.Appearance = appearance4;

                appearance5.BackColor = System.Drawing.SystemColors.ScrollBar;
                this.ultraToolTipManager1.Appearance = appearance5;

                appearance6.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraRuleToolBar.Appearance = appearance6;
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

        public void ExportUltraTreeToExcel(string gridName, string filePath)
        {
            if (ultraRuleTree == null || string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            try
            {
                // Create Excel workbook and worksheet
                Workbook workbook = new Workbook(WorkbookFormat.Excel97To2003);
                Worksheet worksheet = workbook.Worksheets.Add("TreeExport");

                int currentRow = 0;
                // Export all nodes recursively
                foreach (UltraTreeNode node in ultraRuleTree.Nodes)
                {
                    ExportNodeRecursive(node, worksheet, ref currentRow, 0);
                }
                workbook.Save(filePath);
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

        private void ExportNodeRecursive(UltraTreeNode node, Worksheet worksheet, ref int row, int indentLevel)
        {
            worksheet.Rows[row].Cells[indentLevel].Value = node.Text;

            // Export node's cells if available (for multi-column nodes)
            for (int i = 0; i < node.Cells.Count; i++)
            {
                worksheet.Rows[row].Cells[indentLevel + i].Value = node.Cells[i].Text;
            }

            row++;

            foreach (UltraTreeNode childNode in node.Nodes)
            {
                ExportNodeRecursive(childNode, worksheet, ref row, indentLevel + 1);
            }
        }
    }
}
