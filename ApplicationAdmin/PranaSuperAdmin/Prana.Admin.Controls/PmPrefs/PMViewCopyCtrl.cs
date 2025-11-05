using Prana.Admin.BLL;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Prana.Admin.Controls.PmPrefs
{
    public partial class PMViewCopyCtrl : UserControl
    {
        public string _isPasswordChanged = "";
        public string _isUsernameChanged = "";
        public string _isClientDBChanged = "";
        public string _isMachineIPChanged = "";

        public PMViewCopyCtrl()
        {
            InitializeComponent();
            if (!(DesignMode || CustomThemeHelper.IsDesignMode()))
            {
                FetchData();
            }
        }

        private void ClearStatus()
        {
            try
            {
                msgStatusBar.Text = string.Empty;
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

        private void FetchData()
        {
            try
            {
                Users allUsers = new Users();
                Dictionary<int, string> dictUser = new Dictionary<int, string>();
                allUsers = UserManager.GetAllUsers();

                foreach (User user in allUsers)
                    if (!dictUser.ContainsKey(user.UserID))
                        dictUser.Add(user.UserID, user.LoginName);

                comboFromUsername.DataSource = null;
                comboFromUsername.DataSource = new BindingSource(dictUser, null);
                comboFromUsername.DisplayMember = "Value";
                comboFromUsername.ValueMember = "Key";
                comboToUsername.DataSource = null;
                comboToUsername.DataSource = new BindingSource(dictUser, null);
                comboToUsername.DisplayMember = "Value";
                comboToUsername.ValueMember = "Key";

            }
            catch (Exception ex)
            {
                ClearTabs();
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void LoadFromUserViews(string userFromId)
        {
            try
            {
                string filePath = txtFolderBrowser.Text.Trim() + "\\" + userFromId + "\\" + "CustomViewPreferences.xml";
                if (File.Exists(filePath))
                {
                    using (TransactionScope updateTransaction = new TransactionScope())
                    {
                        XDocument xmlDocument = XDocument.Load(filePath);

                        var allNodes = xmlDocument.Descendants("item").ToList();
                        var allTabNames = allNodes.Descendants("string");
                        Dictionary<string, string> dictTabNames = new Dictionary<string, string>();

                        foreach (XElement tabname in allTabNames)
                        {
                            if (tabname.Value == "Default")
                            {
                                continue;
                            }
                            else
                            {
                                if (tabname.Value.StartsWith("CustomView_"))
                                {
                                    string removedFundPrefix = tabname.Value.Replace("CustomView_", "");
                                    dictTabNames.Add(tabname.Value, removedFundPrefix);
                                }
                                else if (tabname.Value.Equals("Main"))
                                {
                                    dictTabNames.Add(tabname.Value, "Main");
                                }
                                else
                                {
                                    dictTabNames.Add(tabname.Value, tabname.Value);
                                }
                            }
                        }

                        comboCopyFrom.Items.Clear();
                        comboCopyFrom.DataSource = new BindingSource(dictTabNames, null);
                        comboCopyFrom.DisplayMember = "Value";
                        comboCopyFrom.ValueMember = "Key";
                        updateTransaction.Complete();
                    }
                }
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

        private void LoadToUserViews(string userToId, bool isCopyForAllUsers, out bool customViewExists)
        {
            try
            {
                comboCopyTo.ClearAll();
                string filePath = txtFolderBrowser.Text.Trim() + "\\" + userToId + "\\" + "CustomViewPreferences.xml";
                customViewExists = false;
                if (File.Exists(filePath))
                {
                    comboCopyTo.Enabled = true;
                    customViewExists = true;
                    using (TransactionScope updateTransaction = new TransactionScope())
                    {
                        XDocument xmlDocument = XDocument.Load(filePath);

                        var allNodes = xmlDocument.Descendants("item").ToList();
                        var allTabNames = allNodes.Descendants("string");
                        Dictionary<string, string> dictTabNames = new Dictionary<string, string>();

                        foreach (XElement tabname in allTabNames)
                        {
                            if (tabname.Value == "Default")
                            {
                                continue;
                            }
                            else
                            {
                                if (tabname.Value.StartsWith("CustomView_"))
                                {
                                    string removedFundPrefix = tabname.Value.Replace("CustomView_", "");
                                    dictTabNames.Add(tabname.Value, removedFundPrefix);
                                }
                                else if (tabname.Value.Equals("Main"))
                                {
                                    dictTabNames.Add(tabname.Value, "Main");
                                }
                                else
                                {
                                    dictTabNames.Add(tabname.Value, tabname.Value);
                                }
                            }
                        }


                        comboCopyTo.AddItemsToTheCheckList(dictTabNames, CheckState.Unchecked);
                        updateTransaction.Complete();
                    }
                }
                else
                {
                    comboCopyTo.Enabled = isCopyForAllUsers;
                }
            }
            catch (Exception ex)
            {
                customViewExists = false;
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void PMViewCopy(string fromTabKey, Dictionary<string, string> copyToViews, string userToID)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    string fromFilePath = txtFolderBrowser.Text.Trim() + "\\" + comboFromUsername.SelectedItem.DataValue.ToString() + "\\" + "CustomViewPreferences.xml";
                    XDocument fromXmlDocument = XDocument.Load(fromFilePath);

                    string toFilePath = txtFolderBrowser.Text.Trim() + "\\" + userToID + "\\" + "CustomViewPreferences.xml";
                    CreateBackupFiles(toFilePath, userToID);
                    XDocument toXmlDocument = XDocument.Load(toFilePath);

                    var fromAllNodes = fromXmlDocument.Descendants("item").ToList();
                    var searchNode = fromAllNodes.First(i => i.Element("key").Element("string").Value == fromTabKey);
                    var toAllNodes = toXmlDocument.Descendants("item").ToList();

                    foreach (KeyValuePair<string, string> kvp in copyToViews)
                    {
                        var replaceNode = toAllNodes.First(i => i.Element("key").Element("string").Value == kvp.Key);

                        if (XNode.DeepEquals(replaceNode, searchNode) && comboFromUsername.SelectedItem.DataValue.ToString() == userToID)
                        {
                            continue;
                        }
                        else
                        {
                            replaceNode.Descendants("SelectedColumnsCollection").Single().ReplaceWith(searchNode.Descendants("SelectedColumnsCollection").Single());
                            replaceNode.Descendants("IsDashboardVisible").SingleOrDefault().ReplaceWith(searchNode.Descendants("IsDashboardVisible").SingleOrDefault());

                            if (!checkboxIncludeFilers.Checked)
                            {
                                foreach (var columnNode in replaceNode.Descendants("SelectedColumnsCollection").Single().Descendants("Column"))
                                {
                                    columnNode.Descendants("FilterConditionList").Single().ReplaceWith(XElement.Parse("<FilterConditionList />"));
                                }

                                replaceNode.Descendants("FilterDetails").Single().ReplaceWith(XElement.Parse("<FilterDetails />"));
                            }
                            else
                            {
                                replaceNode.Descendants("FilterDetails").Single().ReplaceWith(searchNode.Descendants("FilterDetails").Single());
                            }
                            int isGroupNodePresentinReplace = replaceNode.Descendants("GroupByColumnsCollection").Count();
                            var addGroupbyColumn = replaceNode.Descendants("CustomViewPreferences").Single();
                            if (checkboxIncludeGrouping.Checked)
                            {
                                int isGroupNodePresentinSearch = searchNode.Descendants("GroupByColumnsCollection").Count();

                                if (isGroupNodePresentinReplace == 0)
                                {
                                    if (isGroupNodePresentinSearch == 0)
                                        addGroupbyColumn.Add(XElement.Parse("<GroupByColumnsCollection />"));
                                    else
                                        addGroupbyColumn.Add(searchNode.Descendants("GroupByColumnsCollection").Single());
                                }
                                else
                                {
                                    if (isGroupNodePresentinSearch == 0)
                                        replaceNode.Descendants("GroupByColumnsCollection").Single().ReplaceWith(XElement.Parse("<GroupByColumnsCollection />"));
                                    else
                                        replaceNode.Descendants("GroupByColumnsCollection").Single().ReplaceWith(searchNode.Descendants("GroupByColumnsCollection").Single());
                                }
                            }
                            else
                            {
                                if (isGroupNodePresentinReplace == 0)
                                    addGroupbyColumn.Add(XElement.Parse("<GroupByColumnsCollection />"));
                                else
                                    replaceNode.Descendants("GroupByColumnsCollection").Single().ReplaceWith(XElement.Parse("<GroupByColumnsCollection />"));
                            }
                        }
                    }

                    File.WriteAllText(toFilePath, toXmlDocument.ToString());
                    updateTransaction.Complete();
                }
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

        private void CreateBackupFiles(string oldBackupFilePath, string userToID)
        {
            try
            {
                if (Path.GetFileName(oldBackupFilePath) != null && File.Exists(oldBackupFilePath))
                {
                    string currentFolderPath = txtFolderBrowser.Text.Trim() + "\\" + userToID;
                    string newBackupFilePath = Path.Combine(currentFolderPath + "\\CustomViewPreference_Backup_" + DateTime.Now.ToString("yyyyMMdd") + ".xml");
                    File.Copy(oldBackupFilePath, newBackupFilePath, true);
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

        private void comboFromUser_Leave(object sender, EventArgs e)
        {
            try
            {
                comboCopyFrom.Items.Clear();
                ClearStatus();
                if ((!string.IsNullOrEmpty(comboFromUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                {
                    LoadFromUserViews(comboFromUsername.SelectedItem.DataValue.ToString());
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

        private void comboToUser_Leave(object sender, EventArgs e)
        {
            try
            {
                ComboToUserRefresh();

            }
            catch (Exception ex)
            {

                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void ComboToUserRefresh()
        {
            try
            {
                comboCopyTo.ClearAll();
                ClearStatus();
                if ((!string.IsNullOrEmpty(comboToUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                {
                    bool isCustomView;
                    LoadToUserViews(comboToUsername.SelectedItem.DataValue.ToString(), false, out isCustomView);
                }
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

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatus();
                if (preferenceBrowser.ShowDialog() == DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(preferenceBrowser.SelectedPath.ToString().Trim()))
                    {
                        txtFolderBrowser.Text = preferenceBrowser.SelectedPath.ToString();
                        if ((!string.IsNullOrEmpty(comboFromUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                        {
                            LoadFromUserViews(comboFromUsername.SelectedItem.DataValue.ToString());
                        }
                        if ((!string.IsNullOrEmpty(comboToUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                        {
                            bool isCustomView;
                            LoadToUserViews(comboToUsername.SelectedItem.DataValue.ToString(), false, out isCustomView);
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

        private void ClearTabs()
        {
            try
            {
                comboCopyFrom.Items.Clear();
                comboCopyTo.ClearAll();
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

        private void txtFolderBrowser_Leave(object sender, EventArgs e)
        {
            try
            {
                txtFolderBrowser.Text = txtFolderBrowser.Text.Trim();
                ClearStatus();
                if (!Directory.Exists(txtFolderBrowser.Text))
                    ClearTabs();
                if ((!string.IsNullOrEmpty(comboFromUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                {
                    LoadFromUserViews(comboFromUsername.SelectedItem.DataValue.ToString());
                }
                if ((!string.IsNullOrEmpty(comboToUsername.Text)) && !string.IsNullOrEmpty(txtFolderBrowser.Text.Trim()))
                {
                    bool isCustomView;
                    LoadToUserViews(comboToUsername.SelectedItem.DataValue.ToString(), false, out isCustomView);
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

        private void buttonRun_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatus();

                if (comboCopyFrom.SelectedItem == null || (!(checkboxCopyViewsForAllUsers.Checked) && comboToUsername.SelectedItem == null))
                {
                    msgStatusBar.Text = "Please select all values";
                    return;
                }

                string s = comboCopyFrom.SelectedItem.DataValue.ToString();
                if (checkboxCopyViewsForAllUsers.Checked)
                {
                    foreach (var item in comboToUsername.Items)
                    {
                        if (checkboxExcludeCurrentUser.Checked && comboFromUsername.SelectedItem.DataValue.ToString() == item.DataValue.ToString())
                        {
                            continue;
                        }
                        bool isCustomView;
                        LoadToUserViews(item.DataValue.ToString(), true, out isCustomView);
                        if (comboCopyTo.GetNoOfTotalItems() != 0)
                        {
                            comboCopyTo.SelectUnselectAll(CheckState.Checked);
                        }
                        Dictionary<string, string> copyToViews = comboCopyTo.GetSelectedItemsInDictionaryinString();

                        if (checkboxCreateNewTabIfNotExists.Checked)
                        {
                            if (!(copyToViews.ContainsKey(comboCopyFrom.SelectedItem.DataValue.ToString())))
                            {
                                if (copyToViews.Count == 0)
                                {
                                    PMViewCreate(comboCopyFrom.SelectedItem.DataValue.ToString(), item.DataValue.ToString());
                                    CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), item.DataValue.ToString(), comboCopyFrom.SelectedItem.DataValue.ToString());
                                    ComboToUserRefresh();
                                }
                                else
                                {
                                    PMViewCreateAndCopy(comboCopyFrom.SelectedItem.DataValue.ToString(), item.DataValue.ToString());
                                    CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), item.DataValue.ToString(), comboCopyFrom.SelectedItem.DataValue.ToString());
                                }

                            }
                        }

                        if (isCustomView)
                        {
                            PMViewCopy(comboCopyFrom.SelectedItem.DataValue.ToString(), copyToViews, item.DataValue.ToString());

                            foreach (var tabs in copyToViews)
                            {
                                CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), item.DataValue.ToString(), tabs.Key);
                            }
                        }

                    }

                    comboToUsername.Text = null;
                    comboCopyTo.ClearAll();
                    msgStatusBar.Text = "File(s) successfully updated at " + DateTime.Now.ToLongTimeString();
                }
                else
                {
                    if (checkboxExcludeCurrentUser.Checked && comboFromUsername.SelectedItem.DataValue.ToString() == comboToUsername.SelectedItem.DataValue.ToString())
                    {
                        return;
                    }
                    if ((comboCopyFrom.SelectedIndex != -1 && comboCopyTo.GetNoOfCheckedItems() > 0))
                    {
                        Dictionary<string, string> copyToViews = comboCopyTo.GetSelectedItemsInDictionaryinString();
                        if (checkboxCreateNewTabIfNotExists.Checked)
                        {
                            if (!(copyToViews.ContainsKey(comboCopyFrom.SelectedItem.DataValue.ToString())))
                            {
                                PMViewCreateAndCopy(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString());
                                CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString(), comboCopyFrom.SelectedItem.DataValue.ToString());
                                ComboToUserRefresh();
                            }
                        }
                        PMViewCopy(comboCopyFrom.SelectedItem.DataValue.ToString(), copyToViews, comboToUsername.SelectedItem.DataValue.ToString());
                        CopyDashboard(comboCopyFrom.SelectedItem.DataValue.ToString(), copyToViews, comboToUsername.SelectedItem.DataValue.ToString());
                        if (copyToViews.Count > 0)
                            msgStatusBar.Text = "File(s) successfully updated at " + DateTime.Now.ToLongTimeString();
                    }
                    else
                    {
                        if (comboCopyFrom.SelectedIndex != -1 && comboCopyTo.GetNoOfCheckedItems() <= 0)
                        {
                            if (!comboCopyTo.Enabled)
                            {
                                if (checkboxCreateNewTabIfNotExists.Checked)
                                {
                                    PMViewCreate(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString());
                                    CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString(), comboCopyFrom.SelectedItem.DataValue.ToString());
                                    ComboToUserRefresh();
                                    msgStatusBar.Text = "File(s) successfully updated at " + DateTime.Now.ToLongTimeString();
                                }
                                else
                                {
                                    msgStatusBar.Text = "Please select All values";
                                }
                            }
                            else
                            {
                                Dictionary<string, string> copyToViews = comboCopyTo.GetAllItemsInDictionaryinString();
                                if (copyToViews.ContainsKey(comboCopyFrom.SelectedItem.DataValue.ToString()))
                                {
                                    msgStatusBar.Text = "A view with the same name exists";
                                    return;
                                }
                                PMViewCreateAndCopy(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString());
                                CopyDashboardForAView(comboCopyFrom.SelectedItem.DataValue.ToString(), comboToUsername.SelectedItem.DataValue.ToString(), comboCopyFrom.SelectedItem.DataValue.ToString());
                                ComboToUserRefresh();
                                msgStatusBar.Text = "File(s) successfully updated at " + DateTime.Now.ToLongTimeString();
                            }
                        }
                        else
                        {
                            msgStatusBar.Text = "Please select tab name(s).";
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

        private void CopyDashboard(string fromTabKey, Dictionary<string, string> copyToViews, string userToID)
        {
            using (TransactionScope updateTransaction = new TransactionScope())
            {
                foreach (KeyValuePair<string, string> kvp in copyToViews)
                {
                    CopyDashboardForAView(fromTabKey, userToID, kvp.Key);
                }
                updateTransaction.Complete();
            }
        }

        private void CopyDashboardForAView(string fromTabKey, string userToID, string toViewKey)
        {
            string fromFilePath = txtFolderBrowser.Text.Trim() + "\\" + comboFromUsername.SelectedItem.DataValue + "\\" + fromTabKey + "Dashboard.xml";
            XDocument fromXmlDocument = XDocument.Load(fromFilePath);
            string toFilePath = txtFolderBrowser.Text.Trim() + "\\" + userToID + "\\" + toViewKey + "Dashboard.xml";
            CreateDashboardBackupFiles(toFilePath, userToID, toViewKey);
            File.WriteAllText(toFilePath, fromXmlDocument.ToString());
        }

        private void CreateDashboardBackupFiles(string oldBackupFilePath, string userToID, string dashboardKey)
        {
            try
            {
                if (Path.GetFileName(oldBackupFilePath) != null && File.Exists(oldBackupFilePath))
                {
                    string currentFolderPath = txtFolderBrowser.Text.Trim() + "\\" + userToID;
                    string newBackupFilePath = Path.Combine(currentFolderPath + "\\" + dashboardKey + "Dashboard_Backup_" + DateTime.Now.ToString("yyyyMMdd") + ".xml");
                    File.Copy(oldBackupFilePath, newBackupFilePath, true);
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

        private void PMViewCreateAndCopy(string fromTabKey, string userToID)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    string fromFilePath = txtFolderBrowser.Text.Trim() + "\\" + comboFromUsername.SelectedItem.DataValue + "\\" + "CustomViewPreferences.xml";
                    XDocument fromXmlDocument = XDocument.Load(fromFilePath);

                    string toFilePath = txtFolderBrowser.Text.Trim() + "\\" + userToID + "\\" + "CustomViewPreferences.xml";
                    CreateBackupFiles(toFilePath, userToID);
                    XDocument toXmlDocument = XDocument.Load(toFilePath);

                    var fromAllNodes = fromXmlDocument.Descendants("item").ToList();
                    var searchNode = fromAllNodes.First(i => i.Element("key").Element("string").Value == fromTabKey);

                    var toLastNode = toXmlDocument.Descendants("item").ToList().Last();
                    if (toLastNode.Parent != null)
                    {
                        toLastNode.Parent.Add(searchNode);
                        File.WriteAllText(toFilePath, toXmlDocument.ToString());
                    }

                    CreateNodeInXml(fromTabKey, toFilePath, searchNode);
                    updateTransaction.Complete();
                }
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

        private void PMViewCreate(string fromTabKey, string userToID)
        {
            try
            {
                using (TransactionScope updateTransaction = new TransactionScope())
                {
                    string fromFilePath = txtFolderBrowser.Text.Trim() + "\\" + comboFromUsername.SelectedItem.DataValue + "\\" + "CustomViewPreferences.xml";
                    XDocument fromXmlDocument = XDocument.Load(fromFilePath);

                    string toFilePath = txtFolderBrowser.Text.Trim() + "\\" + userToID + "\\" + "CustomViewPreferences.xml";
                    string toFolderPath = txtFolderBrowser.Text.Trim() + "\\" + userToID;
                    var fromAllNodes = fromXmlDocument.Descendants("item").ToList();
                    var searchNode = fromAllNodes.First(i => i.Element("key").Element("string").Value == fromTabKey);

                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNode rootNode = xmlDoc.CreateElement("CustomViewPreferencesList");
                    xmlDoc.AppendChild(rootNode);
                    rootNode.AppendChild(ToXmlNode(searchNode, xmlDoc));
                    if (!Directory.Exists(toFolderPath))
                    {
                        Directory.CreateDirectory(toFolderPath);
                    }
                    xmlDoc.Save(toFilePath);
                    CreateNodeInXml(fromTabKey, toFilePath, searchNode);
                    updateTransaction.Complete();
                }
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

        private void CreateNodeInXml(string fromTabKey, string toFilePath, XElement searchNode)
        {
            try
            {
                XDocument toXmlDocument = XDocument.Load(toFilePath);
                var toAllNodes = toXmlDocument.Descendants("item").ToList();

                var replaceNode = toAllNodes.First(i => i.Element("key").Element("string").Value == fromTabKey);
                if (!checkboxIncludeFilers.Checked)
                {
                    foreach (var columnNode in replaceNode.Descendants("SelectedColumnsCollection").Single().Descendants("Column"))
                    {
                        columnNode.Descendants("FilterConditionList")
                            .Single()
                            .ReplaceWith(XElement.Parse("<FilterConditionList />"));
                    }
                    replaceNode.Descendants("FilterDetails").Single().ReplaceWith(XElement.Parse("<FilterDetails />"));
                }
                else
                {
                    replaceNode.Descendants("FilterDetails").Single().ReplaceWith(searchNode.Descendants("FilterDetails").Single());
                }

                int isGroupNodePresentinReplace = replaceNode.Descendants("GroupByColumnsCollection").Count();
                var addGroupbyColumn = replaceNode.Descendants("CustomViewPreferences").Single();
                if (checkboxIncludeGrouping.Checked)
                {
                    int isGroupNodePresentinSearch = searchNode.Descendants("GroupByColumnsCollection").Count();
                    if (isGroupNodePresentinReplace == 0)
                    {
                        addGroupbyColumn.Add(isGroupNodePresentinSearch == 0
                            ? XElement.Parse("<GroupByColumnsCollection />")
                            : searchNode.Descendants("GroupByColumnsCollection").Single());
                    }
                    else
                    {
                        if (isGroupNodePresentinSearch == 0)
                            replaceNode.Descendants("GroupByColumnsCollection")
                                .Single()
                                .ReplaceWith(XElement.Parse("<GroupByColumnsCollection />"));
                        else
                            replaceNode.Descendants("GroupByColumnsCollection")
                                .Single()
                                .ReplaceWith(searchNode.Descendants("GroupByColumnsCollection").Single());
                    }
                }
                else
                {
                    if (isGroupNodePresentinReplace == 0)
                        addGroupbyColumn.Add(XElement.Parse("<GroupByColumnsCollection />"));
                    else
                        replaceNode.Descendants("GroupByColumnsCollection")
                            .Single()
                            .ReplaceWith(XElement.Parse("<GroupByColumnsCollection />"));
                }

                File.WriteAllText(toFilePath, toXmlDocument.ToString());
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

        private void checkboxCopyViewsForAllUsers_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkboxCopyViewsForAllUsers.Checked)
                {
                    lblCopyTo.Visible = false;
                    lblToUsername.Visible = false;
                    comboCopyTo.Visible = false;
                    comboToUsername.Visible = false;
                    comboToUsername.Text = null;
                    comboCopyTo.ClearAll();
                    checkboxExcludeCurrentUser.Visible = true;
                }
                else
                {
                    lblCopyTo.Visible = true;
                    lblToUsername.Visible = true;
                    comboCopyTo.Visible = true;
                    comboToUsername.Visible = true;
                    checkboxExcludeCurrentUser.Visible = false;
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

        public XmlNode ToXmlNode(XElement element, XmlDocument xmlDoc = null)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                if (xmlDoc == null) xmlDoc = new XmlDocument();
                return xmlDoc.ReadNode(xmlReader);
            }
        }
    }
}