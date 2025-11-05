using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using Prana.BusinessObjects.Compliance.Definition;
using Prana.BusinessObjects.Compliance.Delegates;
using Prana.BusinessObjects.Compliance.Enums;
using Prana.BusinessObjects.Compliance.EventArguments;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prana.ComplianceEngine.RuleDefinition.RulePluggins.UserControls
{
    public partial class RuleGroupingControl : UserControl
    {
        public event GroupOperationHandler GroupOperationEvent;

        private Dictionary<String, RuleBase> _ruleList = new Dictionary<String, RuleBase>();
        private Dictionary<String, GroupBase> _groupList = new Dictionary<string, GroupBase>();
        private Dictionary<String, String> _renamedDict = new Dictionary<string, string>();
        private Dictionary<String, GroupBase> _addedDict = new Dictionary<string, GroupBase>();
        private List<String> _deletedList = new List<string>();
        private object _lockerObject = new object();

        public RuleGroupingControl()
        {
            try
            {
                InitializeComponent();
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
                {
                    InitializeContaxtMenu();

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
        /// Initializes context menu for all group operations.
        /// </summary>
        private void InitializeContaxtMenu()
        {
            try
            {
                foreach (GroupOperations operation in Enum.GetValues(typeof(GroupOperations)))
                {
                    if (operation != GroupOperations.None && operation != GroupOperations.Open)
                    {
                        ToolStripItem menuItem = new ToolStripMenuItem();
                        menuItem.Name = operation.ToString();
                        menuItem.Text = SplitCamelCase(operation.ToString());
                        menuItem.Tag = operation.ToString();
                        cntxtMnuGroups.Items.Add(menuItem);
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
        /// Splits string in camel case.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string SplitCamelCase(string input)
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
        /// Assigns selected rules to active groups.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnAssignSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraLstGroup.ActiveItem != null)
                {
                    foreach (UltraListViewItem item in ultraLstUnAssigned.SelectedItems.All)
                    {

                        lock (_lockerObject)
                        {
                            if (_groupList.ContainsKey(ultraLstGroup.ActiveItem.Key) && _ruleList.ContainsKey(item.Key))
                            {
                                _ruleList[item.Key].GroupId = ultraLstGroup.ActiveItem.Key;
                                _groupList[ultraLstGroup.ActiveItem.Key].RuleList.Add(GetRuleBaseItem(item.Key));
                                _ruleList.Remove(item.Key);
                                ultraLstUnAssigned.Items.Remove(item);
                                ultraLstAssignedRules.Items.Add(item);
                                //if (GroupOperationEvent != null)
                                //    GroupOperationEvent(this, new GroupOperationsEventArgs { Group = _groupList[item.Key], Operation = GroupOperations.AddRuleInGroup });
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "No Group Active", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// assigns all rules to active group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnAssignAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (ultraLstGroup.ActiveItem != null)
                {
                    foreach (UltraListViewItem item in ultraLstUnAssigned.Items.All)
                    {

                        lock (_lockerObject)
                        {
                            if (_groupList.ContainsKey(ultraLstGroup.ActiveItem.Key) && _ruleList.ContainsKey(item.Key))
                            {
                                _ruleList[item.Key].GroupId = ultraLstGroup.ActiveItem.Key;
                                _groupList[ultraLstGroup.ActiveItem.Key].RuleList.Add(GetRuleBaseItem(item.Key));
                                _ruleList.Remove(item.Key);
                                ultraLstUnAssigned.Items.Remove(item);
                                ultraLstAssignedRules.Items.Add(item);
                                //if (GroupOperationEvent != null)
                                //    GroupOperationEvent(this, new GroupOperationsEventArgs { Group = _groupList[item.Key], Operation = GroupOperations.AddRuleInGroup });
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "No Group Active", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Unassign rules from Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnUnassignSelected_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraListViewItem item in ultraLstAssignedRules.SelectedItems.All)
                {

                    lock (_lockerObject)
                    {
                        if (_groupList.ContainsKey(ultraLstGroup.ActiveItem.Key))
                        {
                            _ruleList.Add(item.Key, GetRuleBaseItem(item.Key));
                            _ruleList[item.Key].GroupId = "-1";
                            _groupList[ultraLstGroup.ActiveItem.Key].RuleList.Remove(GetRuleBaseItem(item.Key));
                            ultraLstAssignedRules.Items.Remove(item);
                            ultraLstUnAssigned.Items.Add(item);
                            //if (GroupOperationEvent != null)
                            //    GroupOperationEvent(this, new GroupOperationsEventArgs { Group = _groupList[item.Key], Operation = GroupOperations.RemoveRuleFromGroup });
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
        }

        /// <summary>
        /// Unassigns all rules from group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraBtnUnassignAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (UltraListViewItem item in ultraLstAssignedRules.Items.All)
                {
                    lock (_lockerObject)
                    {
                        if (_groupList.ContainsKey(ultraLstGroup.ActiveItem.Key))
                        {
                            _ruleList.Add(item.Key, GetRuleBaseItem(item.Key));
                            _ruleList[item.Key].GroupId = "-1";
                            _groupList[ultraLstGroup.ActiveItem.Key].RuleList.Remove(GetRuleBaseItem(item.Key));
                            ultraLstAssignedRules.Items.Remove(item);
                            ultraLstUnAssigned.Items.Add(item);
                            //if (GroupOperationEvent != null)
                            //    GroupOperationEvent(this, new GroupOperationsEventArgs { Group = _groupList[item.Key], Operation = GroupOperations.RemoveRuleFromGroup });
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
        }

        /// <summary>
        /// Apply context menu operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cntxtMnuGroups_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                GroupOperations operation = (GroupOperations)Enum.Parse(typeof(GroupOperations), e.ClickedItem.Name, true);
                switch (operation)
                {
                    case GroupOperations.AddGroup:
                        AddGroup();
                        break;
                    case GroupOperations.DeleteGroup:
                        DeleteGroup();
                        break;
                    case GroupOperations.RenameGroup:
                        RenameGroup();
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
        /// Renames Group starts editing of node.
        /// </summary>
        private void RenameGroup()
        {
            try
            {
                UltraListViewItem selectedGroup = ultraLstGroup.ActiveItem;
                //selectedGroup.Tag = selectedGroup.Text;
                selectedGroup.BeginEdit();
                //if (GroupOperationEvent != null)
                //   GroupOperationEvent(this, new GroupOperationsEventArgs());
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
        /// Deletes group from list 
        /// if contains rules in group then unassign those rules.
        /// </summary>
        private void DeleteGroup()
        {
            try
            {
                foreach (UltraListViewItem item in ultraLstGroup.SelectedItems)
                {
                    lock (_lockerObject)
                    {
                        foreach (RuleBase rule in _groupList[item.Key].RuleList)
                        {
                            _ruleList.Add(rule.RuleId, rule);
                            _ruleList[rule.RuleId].GroupId = "-1";
                            UltraListViewItem deletedItem = ultraLstAssignedRules.Items[rule.RuleId];
                            ultraLstAssignedRules.Items.Remove(deletedItem);
                            ultraLstUnAssigned.Items.Add(deletedItem);
                        }
                    }
                    _groupList.Remove(item.Key);
                    _deletedList.Add(item.Key);
                    ultraLstGroup.Items.Remove(item);

                    if (GroupOperationEvent != null)
                        GroupOperationEvent(this, new GroupOperationsEventArgs { GroupId = item.Key, Notification = null, Operation = GroupOperations.DeleteGroup });

                    if (ultraLstGroup.Items.Count > 0)
                    {
                        ultraLstGroup.Items[0].Activate();
                        ultraLstGroup.SelectedItems.Add(ultraLstGroup.Items[0]);
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
        /// Add new list item in group list box and starts editing.
        /// </summary>
        private void AddGroup()
        {
            try
            {
                UltraListViewItem item = new UltraListViewItem();
                item.Key = Guid.NewGuid().ToString();
                item.Value = "";
                item.Tag = "";
                item.Appearance.Image = ultraLstGroup.ViewSettingsList.ImageList.Images["Category.ico"];
                ultraLstGroup.Items.Add(item);
                ultraLstGroup.ItemSettings.AllowEdit = DefaultableBoolean.True;
                item.BeginEdit();
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
        /// on double click starts editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            try
            {
                ultraLstGroup.ItemSettings.AllowEdit = DefaultableBoolean.True;
                e.Item.BeginEdit();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_ItemEnteredEditMode(object sender, ItemEnteredEditModeEventArgs e)
        {
            // e.Item.Tag = e.Item.Text;
        }

        /// <summary>
        /// When node edit mode is exited this event is raised 
        /// updates group name in cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_ItemExitedEditMode(object sender, ItemExitedEditModeEventArgs e)
        {

            try
            {
                if (!IsValidGroupName(e.Item))
                    return;

                e.Item.Tag = e.Item.Text.Trim();
                e.Item.Value = e.Item.Text.Trim();
                lock (_lockerObject)
                {
                    if (_groupList.ContainsKey(e.Item.Key))
                    {
                        _groupList[e.Item.Key].GroupName = e.Item.Text.Trim();
                        if (_renamedDict.ContainsKey(e.Item.Key))
                            _renamedDict[e.Item.Key] = e.Item.Text.Trim();
                        else
                            _renamedDict.Add(e.Item.Key, e.Item.Text.Trim());
                    }
                    else
                    {
                        _groupList.Add(e.Item.Key, new GroupBase { GroupId = e.Item.Key, GroupName = e.Item.Text.Trim() });
                        _addedDict.Add(e.Item.Key, new GroupBase { GroupId = e.Item.Key, GroupName = e.Item.Text.Trim() });
                    }
                }
                e.Item.Activate();
                ultraLstGroup.SelectedItems.Add(e.Item);
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
        /// Checks if the rule name is valid, if invalid then do appropriate actions and display message
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool IsValidGroupName(UltraListViewItem item)
        {
            try
            {
                // if no characters were entered after a rename or add new
                if (string.IsNullOrWhiteSpace(item.Text))
                {
                    MessageBox.Show(this, "Group name can not be blank.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // it is a rename(then use the old name) or just a blank new add (then remove the entry)
                    if (_groupList.ContainsKey(item.Key))
                    {
                        item.Value = _groupList[item.Key].GroupName;
                        item.Activate();
                        ultraLstGroup.SelectedItems.Add(item);
                    }
                    else
                    {
                        ultraLstGroup.Items.Remove(item);
                        if (GroupOperationEvent != null)
                            GroupOperationEvent(this, new GroupOperationsEventArgs { GroupId = null, Notification = null, Operation = GroupOperations.DeleteGroup });

                        if (ultraLstGroup.Items.Count > 0)
                        {
                            ultraLstGroup.Items[0].Activate();
                            ultraLstGroup.SelectedItems.Add(ultraLstGroup.Items[0]);
                        }
                    }

                    return false;
                }

                if (_groupList.ToList().FindIndex(G => G.Value.GroupName.Equals(item.Text.Trim(), StringComparison.InvariantCultureIgnoreCase) && G.Key != item.Key) > -1)    // same name already exists
                {
                    MessageBox.Show(this, "A group with the same name already exists.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // it is a rename(then use the old name) or just a blank new add (then remove the entry)
                    if (_groupList.ContainsKey(item.Key))
                    {
                        item.Value = _groupList[item.Key].GroupName;
                        item.Activate();
                        ultraLstGroup.SelectedItems.Add(item);
                    }
                    else
                    {
                        ultraLstGroup.Items.Remove(item);

                        if (GroupOperationEvent != null)
                            GroupOperationEvent(this, new GroupOperationsEventArgs { GroupId = null, Notification = null, Operation = GroupOperations.DeleteGroup });

                        if (ultraLstGroup.Items.Count > 0)
                        {
                            ultraLstGroup.Items[0].Activate();
                            ultraLstGroup.SelectedItems.Add(ultraLstGroup.Items[0]);
                        }
                    }

                    return false;
                }
                return true;
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
        /// When group is activated shows rules in that group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            try
            {
                ultraLstGroup.ItemSettings.AllowEdit = DefaultableBoolean.False;
                if (e.Item != null)
                {
                    AddRuleToAssignedList(e.Item);
                    lock (_lockerObject)
                    {
                        if (GroupOperationEvent != null)
                            GroupOperationEvent(this, new GroupOperationsEventArgs { GroupId = e.Item.Key, Operation = GroupOperations.Open, Notification = (_groupList.ContainsKey(e.Item.Key)) ? _groupList[e.Item.Key].Notification : new NotificationSetting() });

                    }
                }//e.Item.EndEdit(true);
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
        /// Activates selected group
        /// and shows assigned rule list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0 && e.SelectedItems[0] != null)
                {
                    AddRuleToAssignedList(e.SelectedItems[0]);
                    e.SelectedItems[0].Activate();
                    ultraLstGroup.SelectedItems.Add(e.SelectedItems[0]);
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
        /// Add rule in assigned list
        /// </summary>
        /// <param name="ultraListViewItem"></param>
        private void AddRuleToAssignedList(UltraListViewItem ultraListViewItem)
        {
            try
            {
                ultraLstAssignedRules.Items.Clear();
                lock (_lockerObject)
                {
                    if (_groupList.ContainsKey(ultraListViewItem.Key))
                    {
                        foreach (RuleBase rule in _groupList[ultraListViewItem.Key].RuleList)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = rule.RuleId;
                            item.Tag = rule.RuleName;
                            item.Value = rule.RuleName;
                            ultraLstAssignedRules.Items.Add(item);
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
        }

        /// <summary>
        /// On start up and when group is updated by other user then initalize both the cache 
        /// </summary>
        /// <param name="groupList"></param>
        /// <param name="ruleList"></param>
        internal void InitializeCache(List<GroupBase> groupList, List<RuleBase> ruleList)
        {
            try
            {
                lock (_lockerObject)
                {


                    foreach (GroupBase group in groupList)
                    {
                        if (!_groupList.ContainsKey(group.GroupId))
                        {
                            _groupList.Add(group.GroupId, group);
                            AddGroupToList(group);
                        }
                        else
                        {
                            _groupList[group.GroupId] = group;
                        }
                    }

                    foreach (RuleBase rule in ruleList)
                    {
                        if (rule.GroupId == "-1" && rule.Package == RulePackage.PostTrade)
                        {
                            if (!_ruleList.ContainsKey(rule.RuleId))
                                _ruleList.Add(rule.RuleId, rule);
                        }

                    }
                }
                AddRuleToUnAssignedList();
                if (ultraLstGroup.Items.Count > 0)
                {
                    ultraLstGroup.Items[0].Activate();
                    ultraLstGroup.SelectedItems.Add(ultraLstGroup.Items[0]);
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
        /// Add rules to unassigned rules list.
        /// Create list view item and add it to list view
        /// </summary>
        private void AddRuleToUnAssignedList()
        {
            try
            {
                lock (_lockerObject)
                {
                    ultraLstUnAssigned.Items.Clear();
                    foreach (String key in _ruleList.Keys)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Key = _ruleList[key].RuleId;
                        item.Tag = _ruleList[key].RuleName;
                        item.Value = _ruleList[key].RuleName;
                        ultraLstUnAssigned.Items.Add(item);
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
        /// On initialization add group item to group list view control
        /// </summary>
        /// <param name="group"></param>
        private void AddGroupToList(GroupBase group)
        {
            try
            {
                UltraListViewItem item = new UltraListViewItem();
                item.Key = group.GroupId;
                item.Tag = group.GroupName;
                item.Value = group.GroupName;
                ultraLstGroup.Items.Add(item);
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
        /// Returns rule base for rule id
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        private RuleBase GetRuleBaseItem(string ruleId)
        {
            try
            {
                lock (_lockerObject)
                {
                    if (_ruleList.ContainsKey(ruleId))
                        return _ruleList[ruleId];
                    else
                    {
                        foreach (String key in _groupList.Keys)
                        {
                            foreach (RuleBase rule in _groupList[key].RuleList)
                            {
                                if (rule.RuleId == ruleId)
                                {
                                    return rule;
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
            return null;
        }

        /// <summary>
        /// On mouse up if button is right then show context menu
        /// if item is selected then rename and delete options
        /// if not then only add option is visible.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ultraLstGroup_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    cntxtMnuGroups.Visible = false;
                    UltraListViewItem item = ultraLstGroup.ItemFromPoint(e.X, e.Y);
                    if (item == null)
                    {
                        cntxtMnuGroups.Items[GroupOperations.AddGroup.ToString()].Visible = true;
                        cntxtMnuGroups.Items[GroupOperations.DeleteGroup.ToString()].Visible = false;
                        cntxtMnuGroups.Items[GroupOperations.RenameGroup.ToString()].Visible = false;
                        cntxtMnuGroups.Visible = true;
                    }
                    else
                    {
                        cntxtMnuGroups.Items[GroupOperations.AddGroup.ToString()].Visible = false;
                        cntxtMnuGroups.Items[GroupOperations.DeleteGroup.ToString()].Visible = true;
                        cntxtMnuGroups.Items[GroupOperations.RenameGroup.ToString()].Visible = true;
                        cntxtMnuGroups.Visible = true;
                    }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    UltraListViewItem item = ultraLstGroup.ItemFromPoint(e.X, e.Y);
                    if (item != null)
                    {
                        item.Activate();
                        ultraLstGroup.SelectedItems.Add(item);
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
        /// Returns group list cache on save click.
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, GroupBase> GetUpdatedGroupList()
        {
            try
            {
                Dictionary<string, GroupBase> clonedGroupList = new Dictionary<string, GroupBase>();
                lock (_lockerObject)
                {
                    foreach (String key in _groupList.Keys)
                    {
                        clonedGroupList.Add(key, _groupList[key].DeepClone());
                    }

                }

                return clonedGroupList;
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
        /// Returns ruleid list of unassigned rules
        /// </summary>
        /// <returns></returns>
        internal List<String> GetUnAssignedRules()
        {
            try
            {
                List<String> clonedRuleList = new List<String>();
                lock (_lockerObject)
                {
                    foreach (String key in _ruleList.Keys)
                    {
                        clonedRuleList.Add(key);
                    }

                }

                return clonedRuleList;
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
        /// Return all renamed groups details
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, string> GetRenamedGroups()
        {
            try
            {
                Dictionary<string, string> clonedRenamedGroupsList = new Dictionary<string, string>();
                lock (_lockerObject)
                {
                    foreach (String key in _renamedDict.Keys)
                    {
                        clonedRenamedGroupsList.Add(key, _renamedDict[key]);
                    }

                }

                return clonedRenamedGroupsList;
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
        /// returns cache of added groups. on save click
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, GroupBase> GetAddedGroupList()
        {
            try
            {
                Dictionary<string, GroupBase> clonedAddedGroupList = new Dictionary<string, GroupBase>();
                lock (_lockerObject)
                {
                    foreach (String key in _addedDict.Keys)
                    {
                        clonedAddedGroupList.Add(key, _addedDict[key]);
                    }

                }

                return clonedAddedGroupList;
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
        /// returns deleted group list.
        /// </summary>
        /// <returns></returns>
        internal List<string> GetDeletedGroupLIst()
        {
            try
            {
                List<string> clonedDeletedGroupList = new List<string>();

                foreach (String key in _deletedList)
                {
                    clonedDeletedGroupList.Add(key);
                }
                return clonedDeletedGroupList;
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
        /// Returns group id of select group         
        /// </summary>
        /// <returns></returns>
        internal string GetSelectedGroupId()
        {
            try
            {
                if (ultraLstGroup.ActiveItem != null)
                    return ultraLstGroup.ActiveItem.Key;
                else
                    return String.Empty;
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
                return string.Empty;
            }
        }

        /// <summary>
        /// clears rename delete add cache after save completes.
        /// </summary>
        internal void ClearAllCache()
        {
            try
            {

                _deletedList.Clear();
                _renamedDict.Clear();
                _addedDict.Clear();
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
        /// Refresh called when Rules are refreshed in RuleDefination UI
        /// </summary>
        internal void RefreshAll()
        {
            try
            {
                _deletedList.Clear();
                _renamedDict.Clear();
                _addedDict.Clear();
                _groupList.Clear();
                _ruleList.Clear();
                ultraLstAssignedRules.Items.Clear();
                ultraLstGroup.Items.Clear();
                ultraLstAssignedRules.Items.Clear();
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
        /// Selects 1st group after group save complete
        /// </summary>
        /// <returns></returns>
        internal bool SelectGroup(string groupId)
        {
            try
            {
                if (ultraLstGroup.Items.Count > 0)
                {
                    ultraLstGroup.Items[groupId].Activate();
                    ultraLstGroup.SelectedItems.Add(ultraLstGroup.Items[groupId]);
                    return true;
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
        /// UPdates group UI after rule opertaion
        /// Add, rename, delete,import
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ruleOperations"></param>
        /// <param name="oldRuleId"></param>
        /// <returns></returns>
        internal bool UpdateOnRuleOperations(List<RuleBase> list, RuleOperations ruleOperations, String oldRuleId)
        {

            try
            {
                switch (ruleOperations)
                {
                    case RuleOperations.RenameRule:
                        RenameRules(list, oldRuleId);
                        break;
                    case RuleOperations.DeleteRule:
                        DeleteRule(list);
                        break;
                    case RuleOperations.AddRule:
                    case RuleOperations.ImportRule:
                        foreach (RuleBase rule in list)
                        {
                            if (rule.Package == RulePackage.PostTrade)
                            {
                                _ruleList.Add(rule.RuleId, rule);
                            }
                        }
                        AddRuleToUnAssignedList(list);
                        //MessageBox.Show(this, "New Rules Added to Unassigned list after rule operation.", "Nirvana Compliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            return true;
        }

        /// <summary>
        /// delete rule when rule is deleted from rule definition UI
        /// </summary>
        /// <param name="list"></param>
        private void DeleteRule(List<RuleBase> list)
        {
            try
            {
                lock (_lockerObject)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (rule.Package == RulePackage.PostTrade)
                        {
                            if (rule.GroupId == "-1")
                            {
                                _ruleList.Remove(rule.RuleId);
                                ultraLstUnAssigned.Items.Remove(ultraLstUnAssigned.Items[rule.RuleId]);
                            }
                            else
                            {
                                _groupList[rule.GroupId].RuleList.Remove(rule);
                                if (ultraLstGroup.ActiveItem.Key == rule.GroupId)
                                {
                                    ultraLstAssignedRules.Items.Remove(ultraLstAssignedRules.Items[rule.RuleId]);
                                }
                                if (GetSelectedGroupId().Equals(rule.GroupId))
                                    MessageBox.Show(this, "Rule " + rule.RuleName + " Deleted from Group.", "NirvanaCompliance", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        }

        /// <summary>
        /// Renames rule in group list or rule list if reamed from rule definition UI
        /// </summary>
        /// <param name="list"></param>
        /// <param name="oldRuleId"></param>
        private void RenameRules(List<RuleBase> list, string oldRuleId)
        {
            try
            {
                List<RuleBase> unassignedList = new List<RuleBase>();
                lock (_lockerObject)
                {
                    foreach (RuleBase rule in list)
                    {
                        if (rule.Package == RulePackage.PostTrade)
                        {
                            if (rule.GroupId == "-1")
                            {
                                _ruleList.Remove(oldRuleId);
                                _ruleList.Add(rule.RuleId, rule);
                                ultraLstUnAssigned.Items.Remove(ultraLstUnAssigned.Items[oldRuleId]);
                                unassignedList.Add(rule);
                            }
                            else
                            {

                                _groupList[rule.GroupId].RuleList.Remove(rule);
                                _groupList[rule.GroupId].RuleList.Add(rule);
                                if (ultraLstGroup.ActiveItem.Key == rule.GroupId)
                                {
                                    //String oldRuleName = ultraLstAssignedRules.Items[oldRuleId].Text;
                                    AddRuleToAssignedList(ultraLstGroup.ActiveItem);
                                    // MessageBox.Show(this, "Rule " + oldRuleName + " renamed to " + rule.RuleName + ".", "NirvanaCompliance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                //if (GetSelectedGroupId().Equals(rule.GroupId))

                            }
                        }
                    }
                    AddRuleToUnAssignedList(unassignedList);
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
        /// Add list of unassigned rules to List.
        /// </summary>
        /// <param name="list"></param>
        private void AddRuleToUnAssignedList(List<RuleBase> list)
        {
            try
            {
                foreach (RuleBase rule in list)
                {
                    if (rule.Package == RulePackage.PostTrade)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Key = rule.RuleId;
                        item.Tag = rule.RuleName;
                        item.Value = rule.RuleName;
                        ultraLstUnAssigned.Items.Add(item);
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
        /// Close UI
        /// </summary>
        internal void Close()
        {
            try
            {
                _groupList = null;
                _deletedList = null;
                _renamedDict = null;
                _ruleList = null;
                _addedDict = null;
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

        private void RuleGroupingControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
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
                ultraBtnAssignSelected.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnAssignSelected.ForeColor = System.Drawing.Color.White;
                ultraBtnAssignSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnAssignSelected.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnAssignSelected.UseAppStyling = false;
                ultraBtnAssignSelected.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnAssignAll.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnAssignAll.ForeColor = System.Drawing.Color.White;
                ultraBtnAssignAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnAssignAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnAssignAll.UseAppStyling = false;
                ultraBtnAssignAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnUnassignAll.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnUnassignAll.ForeColor = System.Drawing.Color.White;
                ultraBtnUnassignAll.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnUnassignAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnUnassignAll.UseAppStyling = false;
                ultraBtnUnassignAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnUnassignSelected.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnUnassignSelected.ForeColor = System.Drawing.Color.White;
                ultraBtnUnassignSelected.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnUnassignSelected.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnUnassignSelected.UseAppStyling = false;
                ultraBtnUnassignSelected.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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
                Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();

                appearance1.BackColor = System.Drawing.SystemColors.ControlDark;
                this.ultraPnlMain.Appearance = appearance1;

                appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                this.ultraTxtSearchUnassigned.Appearance = appearance3;

                appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                this.ultraTxtSearchAssigned.Appearance = appearance5;

                appearance6.BackColor = System.Drawing.Color.DodgerBlue;
                this.ultraLstGroup.ItemSettings.ActiveAppearance = appearance6;

                appearance8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
                this.ultraTxtSearchGroup.Appearance = appearance8;
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
