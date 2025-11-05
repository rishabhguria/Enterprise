using Prana.BusinessObjects;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.LogManager;
using Prana.PM.Client.UI.Classes;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Prana.PM.Client.UI.Controls
{
    public partial class CtrlCopyView : UserControl
    {
        string _folderPath = string.Empty;
        string _filePath = string.Empty;
        CompanyUser _loginUser;
        public CtrlCopyView()
        {
            InitializeComponent();
        }

        public void Setup(CompanyUser loginUser)
        {
            try
            {
                _folderPath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + CachedDataManager.GetInstance.LoggedInUser.CompanyUserID.ToString();
                _filePath = _folderPath + @"\CustomViewPreferences.xml";
                _loginUser = loginUser;
                LoadFromUserViews();
                LoadToUserViews();
                LoadDefaultViews();
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

        public void LoadFromUserViews()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using (System.Transactions.TransactionScope updateTransaction = new System.Transactions.TransactionScope())
                    {
                        XDocument xmlDocument = XDocument.Load(_filePath);

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
                                    string removedPrefix = tabname.Value.Replace("CustomView_", "");
                                    dictTabNames.Add(tabname.Value, removedPrefix);
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

        public void LoadToUserViews()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using (System.Transactions.TransactionScope updateTransaction = new System.Transactions.TransactionScope())
                    {
                        XDocument xmlDocument = XDocument.Load(_filePath);

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
                                    string removedPrefix = tabname.Value.Replace("CustomView_", "");
                                    dictTabNames.Add(tabname.Value, removedPrefix);
                                }
                                else
                                {
                                    dictTabNames.Add(tabname.Value, tabname.Value);
                                }
                            }
                        }

                        comboCopyTo.ClearAll();
                        comboCopyTo.AddItemsToTheCheckList(dictTabNames, CheckState.Unchecked);
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

        public void LoadDefaultViews()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using (System.Transactions.TransactionScope updateTransaction = new System.Transactions.TransactionScope())
                    {
                        XDocument xmlDocument = XDocument.Load(_filePath);

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
                                    string removedPrefix = tabname.Value.Replace("CustomView_", "");
                                    dictTabNames.Add(tabname.Value, removedPrefix);
                                }
                                else
                                {
                                    dictTabNames.Add(tabname.Value, tabname.Value);
                                }
                            }
                        }

                        comboDefaultView.Items.Clear();
                        comboDefaultView.DataSource = new BindingSource(dictTabNames, null);
                        comboDefaultView.DisplayMember = "Value";
                        comboDefaultView.ValueMember = "Key";
                        comboDefaultView.Text = PMAppearanceManager.PMAppearance.DefaultSelectedView;
                        if (String.IsNullOrEmpty(comboDefaultView.Text))
                            comboDefaultView.SelectedIndex = 0;
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

        private void CreateBackupFiles(string oldBackupFilePath, string oldBackupFolderPath)
        {
            try
            {
                if (Path.GetFileName(oldBackupFilePath) != null && File.Exists(oldBackupFilePath))
                {
                    string currentFolderPath = oldBackupFolderPath;
                    string newBackupFilePath = Path.Combine(currentFolderPath + "\\CustomViewPreference_Backup_" + DateTime.Now.ToString("yyyyMMdd") + ".xml");
                    File.Copy(oldBackupFilePath, newBackupFilePath, true);
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

        public void CopyViewToTabs()
        {
            try
            {
                if ((comboCopyFrom.SelectedIndex != -1 && comboCopyTo.GetNoOfCheckedItems() > 0))
                {
                    labelStatus.Text = string.Empty;
                    Dictionary<string, string> copyToViews = comboCopyTo.GetSelectedItemsInDictionaryinString();

                    using (System.Transactions.TransactionScope updateTransaction = new System.Transactions.TransactionScope())
                    {
                        XDocument fromXmlDocument = XDocument.Load(_filePath);
                        var fromAllNodes = fromXmlDocument.Descendants("item").ToList();
                        var searchNode = fromAllNodes.FirstOrDefault(i => i.Element("key").Element("string").Value == comboCopyFrom.SelectedItem.DataValue.ToString());
                        if (searchNode == null)
                            return;
                        CreateBackupFiles(_filePath, _folderPath);

                        foreach (KeyValuePair<string, string> kvp in copyToViews)
                        {
                            var replaceNode = fromAllNodes.FirstOrDefault(i => i.Element("key").Element("string").Value == kvp.Key);
                            if (replaceNode == null)
                                continue;
                            if (XNode.DeepEquals(replaceNode, searchNode) && comboCopyFrom.SelectedItem.DataValue.ToString() == kvp.Key)
                            {
                                if (copyToViews.Count == 1)
                                {
                                    MessageBoxInfo msg = new MessageBoxInfo("Cannot copy to same tabs please select different tabs!");
                                    PranaMessageBox.ShowDialog(msg);
                                    return;
                                }
                                continue;
                            }
                            else
                            {
                                XElement selectedColumnSearchNode = searchNode.Descendants("SelectedColumnsCollection").SingleOrDefault();
                                XElement selectedColumReplaceNode = replaceNode.Descendants("SelectedColumnsCollection").SingleOrDefault();
                                XElement selectedDashboardSearchNode = searchNode.Descendants("IsDashboardVisible").SingleOrDefault();
                                XElement selectedDashboardReplaceNode = replaceNode.Descendants("IsDashboardVisible").SingleOrDefault();
                                if (selectedColumnSearchNode == null || selectedColumReplaceNode == null)
                                    return;
                                selectedColumReplaceNode.ReplaceWith(selectedColumnSearchNode);
                                if (selectedDashboardSearchNode == null || selectedDashboardReplaceNode == null)
                                    return;
                                selectedDashboardReplaceNode.ReplaceWith(selectedDashboardSearchNode);
                                var addGroupbyColumn = replaceNode.Descendants("CustomViewPreferences").Single();
                                XElement dynamicFilterColumnReplaceNode = replaceNode.Descendants("FilterDetails").SingleOrDefault();
                                XElement dynamicFilterColumnSearchNode = searchNode.Descendants("FilterDetails").SingleOrDefault();
                                if (!checkboxIncludeFilers.Checked)
                                {
                                    foreach (var columnNode in replaceNode.Descendants("SelectedColumnsCollection").Single().Descendants("Column"))
                                    {
                                        XElement filterConditionColumnNode = columnNode.Descendants("FilterConditionList").SingleOrDefault();
                                        if (filterConditionColumnNode != null)
                                            filterConditionColumnNode.ReplaceWith(XElement.Parse("<FilterConditionList />"));
                                    }
                                    if (dynamicFilterColumnReplaceNode == null)
                                    {
                                        addGroupbyColumn.Add(XElement.Parse("<FilterDetails />"));
                                    }
                                    else
                                    {
                                        dynamicFilterColumnReplaceNode.ReplaceWith(XElement.Parse("<FilterDetails />"));
                                    }
                                }
                                else
                                {
                                    if (dynamicFilterColumnReplaceNode == null)
                                    {
                                        if (dynamicFilterColumnSearchNode == null)
                                        {
                                            addGroupbyColumn.Add(XElement.Parse("<FilterDetails />"));
                                        }
                                        else
                                        {
                                            addGroupbyColumn.Add(dynamicFilterColumnSearchNode);
                                        }
                                    }
                                    else
                                    {
                                        if (dynamicFilterColumnSearchNode == null)
                                        {
                                            dynamicFilterColumnReplaceNode.ReplaceWith(XElement.Parse("<FilterDetails />"));
                                        }
                                        else
                                        {
                                            dynamicFilterColumnReplaceNode.ReplaceWith(dynamicFilterColumnSearchNode);
                                        }
                                    }
                                }
                                int isGroupNodePresentinReplace = replaceNode.Descendants("GroupByColumnsCollection").Count();
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
                        File.WriteAllText(_filePath, fromXmlDocument.ToString());
                        labelStatus.Appearance.ForeColor = System.Drawing.Color.Green;
                        labelStatus.Text = "View(s) successfully updated.Please restart PM.";
                        updateTransaction.Complete();
                    }
                }
                else
                {
                    labelStatus.Appearance.ForeColor = System.Drawing.Color.Red;
                    labelStatus.Text = "Tab(s) not selected";
                }
                CopyDashBoards();
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

        private void CopyDashBoards()
        {
            try
            {
                string path = Application.StartupPath + "\\" + ApplicationConstants.PREFS_FOLDER_NAME + "\\" +
                          _loginUser.CompanyUserID;

                Dictionary<string, string> copyDashboardToViewsDictionary = comboCopyTo.GetSelectedItemsInDictionaryinString();
                string sourceFile = path + "\\" + comboCopyFrom.Value + "Dashboard.xml";
                foreach (string tabNames in copyDashboardToViewsDictionary.Keys)
                {
                    var destFile = path + "\\" + tabNames + "Dashboard.xml";
                    if (File.Exists(sourceFile) && File.Exists(destFile) && sourceFile != destFile)
                        File.Copy(sourceFile, destFile, true);
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void comboCopyFrom_Leave(object sender, EventArgs e)
        {
            try
            {
                labelStatus.Text = string.Empty;
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

        public void SetDefaultView(PMAppearances pmAppearance)
        {
            try
            {
                pmAppearance.DefaultSelectedView = comboDefaultView.SelectedItem.ToString();
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

        private void comboCopyTo_Leave(object sender, EventArgs e)
        {
            try
            {
                labelStatus.Text = string.Empty;
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