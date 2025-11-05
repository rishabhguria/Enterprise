using Infragistics.Win.UltraWinTree;
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.ReconciliationNew;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace Prana.Tools
{
    public partial class ctrlReconTemplate : UserControl
    {
        //const string TYPE_POSITION = "Position";
        //const string TYPE_TRANSACTION = "Transaction";




        public bool _isUnSavedChanges = false;
        //string _previousText = string.Empty;
        //bool isRootNode = false;
        //bool isPreferencesUpdated = false;
        //ctrlReconReportLayout ctrlReconReportLayout1 = new ctrlReconReportLayout();


        private const string TAB_MATCHINGRULES = "Tab_MatchingRules";
        private const string TAB_XSLTMAPPING = "Tab_XSLTMapping";
        private const string TAB_RECONFILTERS = "Tab_ReconFilters";
        private const string TAB_GRIDLAYOUT = "Tab_GridLayOut";
        private const string TAB_ReconReportLayout = "Tab_ReconReportLayout";
        private const string TAB_GeneralInfo = "Tab_GeneralInformation";

        private const string TemplatePositionDefault = "POS_REC_DEFAULT";
        private const string TemplateTransactionDefault = "TRN_REC_DEFAULT";
        BackgroundWorker _bgRunReconcile = null;
        public ctrlReconTemplate()
        {
            try
            {
                InitializeComponent();

                if (!CustomThemeHelper.IsDesignMode())
                {
                    _bgRunReconcile = new BackgroundWorker();
                    _bgRunReconcile.DoWork += new DoWorkEventHandler(_bgRunReconcile_DoWork);
                    _bgRunReconcile.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_bgRunReconcile_RunWorkerCompleted);
                    //show group box for all except ch user
                    //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare)
                    //{
                    //splitContainer2.Panel2.Controls.Remove(groupBox2);
                    splitContainer2.Panel2Collapsed = true;
                    //}
                    //else
                    //{
                    //    //splitContainer3.Panel2.Controls.Remove(groupBox2);
                    //    splitContainer3.Panel2Collapsed = true;
                    //}
                    splitContainer3.Panel2Collapsed = true;

                    Application.EnableVisualStyles();
                    // treeViewTemplates.ExpandAll();
                    CheckForPreferencesUpdated();

                }
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


        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        public void InitializeReconTemplates()
        {
            try
            {
                //ctrlReconFilters1.InitializeReconFiltersTab();
                _isUnSavedChanges = false;
                //  ctrlReconFilters1.GetReconFilters();
                ctrlReconGroupByColumns1.InitializeReconGroupByColumns();
                ctrlReconGeneralInfo1.InitializeControls();
                // ctrlMatchingRules1.InitializeMatchingRulesTab();
                BindTree();
                BindOtherTree();


                ReconPrefManager.prefsSaved += new EventHandler(ReconPrefManager_prefsSaved);
                ctrlReconShowCAGeneratedTrades1.SaveCAGenerateTrade += ctrlReconShowCAGeneratedTrades1_SaveCAGenerateTrade;
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



        /// <summary>
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReconPrefManager_prefsSaved(object sender, EventArgs e)
        {
            try
            {
                ctrlReconFilters1.IsUnsavedChanges = false;
                _isUnSavedChanges = false;
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

        private void btnAddTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                string templateName = string.Empty;
                string reconType = string.Empty;
                string clientID = string.Empty;
                //check that template name blank or not
                if (string.IsNullOrEmpty(tbtemplateName.Text))
                {
                    MessageBox.Show("Please enter template name", "Recon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!treeViewTemplates.ActiveNode.Text.Equals(treeViewTemplates.ActiveNode.RootNode.Text))
                {
                    _isUnSavedChanges = true;
                    //if selected node is a recon type
                    if (ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.ActiveNode.Text))
                    {
                        reconType = treeViewTemplates.ActiveNode.Text;
                        clientID = treeViewTemplates.ActiveNode.RootNode.Key;
                    }
                    else
                    {
                        clientID = treeViewTemplates.ActiveNode.RootNode.Key;
                        reconType = ReconPrefManager.ReconPreferences.GetTemplates(treeViewTemplates.ActiveNode.Key).ReconType.ToString();
                    }

                    templateName = string.Concat(reconType, "_", tbtemplateName.Text);

                    //check that template does not exist
                    string templateKey = clientID + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName;
                    if (!ReconPrefManager.ReconPreferences.CheckTemplateExists(templateKey))
                    {
                        UltraTreeNode newNode = new UltraTreeNode();
                        treeViewTemplates.Nodes[clientID].Nodes[clientID + Seperators.SEPERATOR_6 + reconType].Nodes.Add(newNode);
                        ReconPrefManager.AddDefaultTemplate(Convert.ToInt32(clientID), reconType, templateName);
                        tbtemplateName.Text = string.Empty;
                        newNode.Text = templateName;
                        newNode.Key = clientID + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName;
                        newNode.Selected = true;
                        treeViewTemplates.ActiveNode = newNode;
                        if (!string.IsNullOrWhiteSpace(reconType))
                        {
                            treeViewTemplates.Nodes[clientID].ExpandAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Template Name Already Exists", "Recon", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Please select recon type/template", "Recon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //  LoadBlankTemplate();
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

        private void btnDeleteTemplates_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewTemplates.SelectedNodes.Count > 0)
                {
                    if (!ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Key))
                    {
                        if (treeViewTemplates.ActiveNode.Key != treeViewTemplates.ActiveNode.RootNode.Key && treeViewTemplates.ActiveNode.Parent.Key != treeViewTemplates.ActiveNode.RootNode.Key)
                        {
                            string templateName = treeViewTemplates.ActiveNode.Text;
                            string key = treeViewTemplates.ActiveNode.Key;
                            UltraTreeNode node = treeViewTemplates.ActiveNode;
                            DialogResult dialog = MessageBox.Show("Do you want to delete " + templateName + " Template", "Recon Templates", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dialog.ToString().Equals(DialogResult.Yes.ToString()))
                            {
                                _isUnSavedChanges = true;
                                node.RootNode.Selected = true;
                                String rootNode = treeViewTemplates.ActiveNode.RootNode.Key;
                                String ParentNode = treeViewTemplates.ActiveNode.Parent.Key;
                                treeViewTemplates.Nodes[rootNode].Nodes[ParentNode].Nodes.Remove(node);
                                ReconPrefManager.ReconPreferences.DeleteTemplates(key);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select a template to delete", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Please select a template to delete", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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





        private void treeViewTemplates_AfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
        {
            try
            {
                //Narendra Kumar Jangir 2012/08/20
                //Jira Issue fixed-exception on click of space on template name
                if ((treeViewTemplates.SelectedNodes.Count > 0) && (!ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Text)) && ((!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text))))
                {
                    if (e.NewSelections.Count > 0)
                    {
                        //UltraTreeNode node = (UltraTreeNode)e.NewSelections.All[0];
                        // Dictionary<string, ReconTemplate> dictTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;
                        //if (dictTemplates.ContainsKey(node.Key))
                        //{
                        //    ReconTemplate temp = dictTemplates[node.Key];
                        LoadDataForSelectedTab();
                        //}
                        //else
                        //{
                        //    InitializeSelectedTab();
                        //}
                    }
                }
                //else
                //{
                //    InitializeSelectedTab();
                //}
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
        public void LoadDataForSelectedTab()
        {
            try
            {
                if (treeViewTemplates.SelectedNodes != null && treeViewTemplates.SelectedNodes.Count > 0)
                {
                    string TemplateName = treeViewTemplates.SelectedNodes[0].Text;
                    if (treeViewTemplates.SelectedNodes[0].Parent != null)
                    {
                        string reconType = treeViewTemplates.SelectedNodes[0].Parent.Text;
                        int clientID;
                        //added by amit on 15.04.2015
                        //http://jira.nirvanasolutions.com:8080/browse/CHMW-3396
                        if (treeViewTemplates.SelectedNodes[0].RootNode != null)
                        {
                            if (int.TryParse(treeViewTemplates.SelectedNodes[0].RootNode.Key, out clientID))
                            {
                                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName);

                                if (template != null)
                                {
                                    //ultraTabReconTemplate.Refresh();

                                    if (ultraTabReconTemplate.SelectedTab != null)
                                    {
                                        string selectedTabKey = ultraTabReconTemplate.SelectedTab.Key;
                                        switch (selectedTabKey)
                                        {
                                            case TAB_MATCHINGRULES:
                                                ctrlMatchingRules1.LoadMatchingRules(template);
                                                //ctrlMatchingRules1.InitializeMatchingRulesTab();
                                                break;
                                            case TAB_GeneralInfo:
                                                ctrlReconGeneralInfo1.LoadData(template);
                                                //ctrlMatchingRules1.InitializeMatchingRulesTab();
                                                break;
                                            case TAB_RECONFILTERS:
                                                ctrlReconFilters1.LoadReconFilters(template);
                                                ctrlReconGroupByColumns1.LoadGroupCriteria(template);

                                                //Modified By: Pranay Deep
                                                //http://jira.nirvanasolutions.com:8080/browse/PRANA-11772
                                                // [Recon]Hide IsShowReconpreferences for position, taxlot and PNL
                                                if (((template.ReconType).ToString()).Equals((ReconType.Transaction).ToString()))
                                                {
                                                    ctrlReconShowCAGeneratedTrades1.Visible = true;
                                                    ctrlReconShowCAGeneratedTrades1.LoadShowCAGeneratedTrades(template);
                                                }
                                                else
                                                {
                                                    ctrlReconShowCAGeneratedTrades1.Visible = false;
                                                }

                                                // ctrlReconXSLTMapping1.InitializeXSLTMappingTab();
                                                break;
                                            case TAB_XSLTMAPPING:
                                                ctrlReconXSLTMapping1.LoadXSLTMapping(template);
                                                break;

                                            case TAB_GRIDLAYOUT:
                                                ctrlMasterColumns1.LoadMasterColumns(template);
                                                break;
                                            case TAB_ReconReportLayout:
                                                ctrlReconReportLayout1.LoadReconReportLayout(template);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
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

        /// <summary>
        /// This method is used for fetching the template from tree view
        /// and later this template is passed in the method "UpdateShowCAGeneratedTrades(template)"
        /// in the control ctrlReconShowCAGeneratedTrades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctrlReconShowCAGeneratedTrades1_SaveCAGenerateTrade(object sender, EventArgs<bool> e)
        {
            try
            {
                if (treeViewTemplates.SelectedNodes != null && treeViewTemplates.SelectedNodes.Count > 0)
                {
                    string TemplateName = treeViewTemplates.SelectedNodes[0].Text;
                    if (treeViewTemplates.SelectedNodes[0].Parent != null)
                    {
                        string reconType = treeViewTemplates.SelectedNodes[0].Parent.Text;
                        int clientID;
                        if (treeViewTemplates.SelectedNodes[0].RootNode != null)
                        {
                            if (int.TryParse(treeViewTemplates.SelectedNodes[0].RootNode.Key, out clientID))
                            {
                                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + TemplateName);
                                template.IsShowCAGeneratedTrades = e.Value;

                                ctrlReconShowCAGeneratedTrades1.UpdateShowCAGeneratedTrades(template);
                            }
                        }
                    }
                }
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
        //private void SetListView(ReconTemplate template)
        //{
        //    try
        //    {
        //        listViewAvailableColumns.Items.Clear();
        //        //listViewAvailableColumns.Clear();
        //       // listViewSelectedColumns.Items.Clear();
        //        //listViewAvailableColumns.View = View.Details;

        //        listViewAvailableColumns.View = View.Details;
        //        listViewAvailableColumns.Dock = DockStyle.None;
        //        //listViewAvailableColumns.HeaderStyle = ColumnHeaderStyle.None;
        //      //  listViewAvailableColumns.Groups.Add("NirvanaColumns", "NirvanaColumns");
        //        listViewAvailableColumns.Groups.Add("NirvanaColumns1", "NirvanaColumns1");
        //        listViewAvailableColumns.Groups.Add("NirvanaColumns2", "NirvanaColumns2");
        //        //listViewAvailableColumns.Columns.Add("NirvanaColumns",100);
        //        //listView1.Groups.Add("Group1", "Group1");
        //        //listView1.Items[0].Group = listView1.Groups[0];
        //        ListViewItem item = new ListViewItem("item1", listViewAvailableColumns.Groups[0]);
        //        item.
        //        item.Group=listViewAvailableColumns.Groups[0];
        //        listViewAvailableColumns.Items.Add(item);
        //        //listViewAvailableColumns.Groups[0].Items.Insert(0, item);
        //        ListViewItem item1 = new ListViewItem("item2", listViewAvailableColumns.Groups[0]);
        //        item1.Group = listViewAvailableColumns.Groups[0];
        //        listViewAvailableColumns.Items.Add(item1);
        //        //listViewAvailableColumns.Groups[0].Items.Insert(1, item1);
        //        //listViewAvailableColumns.ShowGroups = true;
        //        //item.Group=li
        //        //ListViewItem item1 = new ListViewItem("item2", listViewAvailableColumns.Groups[0]);
        //        //listViewAvailableColumns.Items.Insert(0,item);
        //        //listViewAvailableColumns.Groups[0].Items.Insert(0, item);
        //        ////listViewAvailableColumns.Items.Insert(1,item1);
        //        //listViewAvailableColumns.Groups[0].Items.Insert(1, item1);

        //        //listViewSelectedColumns.Groups.Add("NirvanaColumns1", "NirvanaColumns");
        //        //int i = 0;
        //        //List<string> matchingrules = new List<string>();
        //        //foreach (DataRow MatchingRuleRow in template.DsMatchingRules.Tables["Parameter"].Rows)
        //        //{
        //        //    if ((bool)MatchingRuleRow[2] == true)
        //        //    {
        //        //        matchingrules.Add((string)MatchingRuleRow[0]);
        //        //    }
        //        //}
        //        //foreach (DataRow NirvanaRow in dtNirvanaMasterColumns.Rows)
        //        //{
        //        //    if (matchingrules.Contains((string)NirvanaRow[0]))
        //        //    {
        //        //        //UltraListViewItem item=ultraListView2.Items.Add((string)NirvanaRow[ReconConstants.COLUMN_Name],);
        //        //    }
        //        //    else
        //        //    {
        //        //        //ListViewItem item = new ListViewItem((string)NirvanaRow[ReconConstants.COLUMN_Name],listViewAvailableColumns.Groups[0]);
        //        //        //listViewAvailableColumns.Items.Add(item);
        //        //        //listViewAvailableColumns.Groups[0].Items.Insert(i, item);
        //        //        //i++;
        //        //    }
        //        //}
        //        //this.ultraListView1.EndUpdate();
        //    }
        //    catch (Exception ex)
        //    {

        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //private void InitializeSelectedTab()
        //{
        //    try
        //    {
        //        if (ultraTabReconTemplate.SelectedTab != null)
        //        {
        //            string selectedTabKey = ultraTabReconTemplate.SelectedTab.Key;
        //            switch (selectedTabKey)
        //            {
        //                case TAB_MATCHINGRULES:
        //                  //  ctrlMatchingRules1.InitializeMatchingRulesTabForSelectedTemplate();
        //                    break;
        //                case TAB_RECONFILTERS:
        //                    //ctrlReconFilters1.LoadBlankReconFilters();
        //                    ctrlReconFilters1.InitializeReconFiltersTab();
        //                    break;
        //                case TAB_XSLTMAPPING:
        //                    ctrlReconXSLTMapping1.InitializeXSLTMappingTabForSelectedTemplate();
        //                    break;

        //      case TAB_MASTERCOLUMS:
        //                    ctrlMasterColumns1.InitializeMasterColumnsTabForSelectedTemplate();
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }

        //    }
        //}
        public void UpdateDataForSelectedTab()
        {
            try
            {
                //First condition checks that a node is selected or not
                //second condition checks that selected node is not a recon type
                //third condition checks that selected node is not a client node
                if (treeViewTemplates.SelectedNodes.Count > 0 && !ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Key) && (!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text)))
                {
                    string TemplateKey = treeViewTemplates.SelectedNodes[0].Key;
                    ReconTemplate reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(TemplateKey);

                    if (ultraTabReconTemplate.SelectedTab != null && reconTemplate != null)
                    {
                        string selectedTabKey = ultraTabReconTemplate.SelectedTab.Key;
                        switch (selectedTabKey)
                        {
                            case TAB_MATCHINGRULES:
                                if (ctrlMatchingRules1.IsUnsavedChanges())
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                ctrlMatchingRules1.UpdateMatchingRules(reconTemplate);
                                break;
                            case TAB_GeneralInfo:

                                if (ctrlReconGeneralInfo1.IsUnsavedChanges())
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                ctrlReconGeneralInfo1.UpdateData(reconTemplate);
                                break;
                            case TAB_RECONFILTERS:
                                ctrlReconFilters1.UpdateReconFilters(reconTemplate);
                                //New CH tab should be synchronized with nirvana preferences
                                ctrlReconGeneralInfo1.UpdateDataForAssetFilters(reconTemplate);
                                if (ctrlReconFilters1.IsUnsavedChanges)
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                if (ctrlReconGroupByColumns1.IsGroupingChanged(reconTemplate))
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }

                                ctrlReconGroupByColumns1.UpdateGroupCriteria(reconTemplate);
                                break;
                            case TAB_XSLTMAPPING:
                                if (ctrlReconXSLTMapping1.IsUnsavedChanges())
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                ctrlReconXSLTMapping1.UpdateXSLTMapping(reconTemplate);
                                break;

                            case TAB_GRIDLAYOUT:
                                if (ctrlMasterColumns1.IsUnsavedChanges())
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                ctrlMasterColumns1.UpdateMasterColumns(reconTemplate);
                                break;
                            case TAB_ReconReportLayout:
                                if (ctrlReconReportLayout1.IsUnsavedChanges())
                                {
                                    _isUnSavedChanges = true;
                                    reconTemplate.IsDirtyForSaving = true;
                                }
                                ctrlReconReportLayout1.UpdateReconReportLayout(reconTemplate);
                                break;
                        }
                        ReconPrefManager.ReconPreferences.UpdateTemplates(treeViewTemplates.SelectedNodes[0].Key, reconTemplate);
                    }
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


        private void treeViewTemplates_BeforeSelect(object sender, Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)
        {
            try
            {
                //First condition checks that a node is selected or not
                //second condition checks that selected node is not a recon type
                //third condition checks that selected node is not a client node
                if (treeViewTemplates.SelectedNodes.Count > 0 && !ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Text) && ((!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text))))
                {
                    //if (!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text))
                    //{
                    //string TemplateName = treeViewTemplates.SelectedNodes[0].Text;
                    //string reconType = treeViewTemplates.SelectedNodes[0].Parent.Text;
                    //}
                    //if (treeViewTemplates.SelectedNodes[0].RootNode.Key.Equals(ReconType.Position.ToString()))
                    //{
                    //    reconType = ReconType.Position.ToString();
                    //}
                    //else
                    //{
                    //    if (treeViewTemplates.SelectedNodes[0].RootNode.Key.Equals(ReconType.Transaction.ToString()))
                    //    {
                    //        reconType = ReconType.Transaction.ToString();
                    //    }
                    //}
                    //ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(TemplateName);
                    UpdateDataForSelectedTab();
                }
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


        #region PrivateFunctions
        //add template tree nodes
        private void BindTree()
        {
            try
            {
                //Added Null Checkes
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3487
                Dictionary<string, ReconTemplate> dictTemplates = new Dictionary<string, ReconTemplate>();
                if (ReconPrefManager.ReconPreferences != null && ReconPrefManager.ReconPreferences.DictReconTemplates != null)
                {
                    dictTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;
                }

                //To clear the tree of any node before binding it afresh.0
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-161
                treeViewTemplates.Nodes.Clear();

                // To bind all clients of selected user
                Dictionary<int, string> dictClients = new Dictionary<int, string>();
                foreach (KeyValuePair<int, List<int>> clients in CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts())
                {
                    if (CachedDataManager.GetUserPermittedCompanyList().ContainsKey(clients.Key))
                    {
                        if (!dictClients.ContainsKey(clients.Key))
                        {
                            dictClients.Add(clients.Key, CachedDataManager.GetUserPermittedCompanyList()[clients.Key]);
                        }
                    }
                }

                foreach (KeyValuePair<int, string> kvp in dictClients)
                {
                    treeViewTemplates.Nodes.Add(kvp.Key.ToString(), kvp.Value);
                    foreach (string reconType in ReconPrefManager.ReconPreferences.getRootTemplates())
                    {
                        //add root node (e.g. Transaction or Position or PNL)
                        UltraTreeNode treeNodeRoot = new UltraTreeNode(kvp.Key.ToString() + Seperators.SEPERATOR_6 + reconType, reconType);
                        treeViewTemplates.Nodes[kvp.Key.ToString()].Nodes.Add(treeNodeRoot);
                        treeNodeRoot.ExpandAll();
                    }
                }

                if (dictTemplates.Count > 0)
                {
                    foreach (KeyValuePair<string, ReconTemplate> kvp in dictTemplates)
                    {
                        string templateName = ReconUtilities.GetTemplateNameFromTemplateKey(kvp.Key);
                        if (ReconPrefManager.ReconPreferences.getRootTemplates().Contains(kvp.Value.ReconType.ToString()))
                        {

                            string reconType = kvp.Value.ReconType.ToString();
                            //updates the sorted column in the template
                            kvp.Value.SortingColumnOrder = ReconPrefManager.ReconPreferences.getSortedColumnsWithoutDataSet(reconType);
                            int clientID = kvp.Value.ClientID;
                            //add child node
                            UltraTreeNode treeNode1 = new UltraTreeNode(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName, templateName);
                            if (treeNode1 != null && treeViewTemplates.Nodes.Exists(clientID.ToString()) && treeViewTemplates.Nodes[clientID.ToString()].Nodes.Exists(clientID.ToString() + Seperators.SEPERATOR_6 + reconType) && !treeViewTemplates.Nodes[clientID.ToString()].Nodes[clientID.ToString() + Seperators.SEPERATOR_6 + reconType].Nodes.Exists(treeNode1.Key))
                            {
                                treeViewTemplates.Nodes[clientID.ToString()].Nodes[clientID.ToString() + Seperators.SEPERATOR_6 + reconType].Nodes.Add(treeNode1);
                            }
                            //treeNodeRoot.Selected = true;
                            //treeViewTemplates.ActiveNode = treeNodeRoot;
                            //treeNodeRoot.ExpandAll();
                            //treeNodeRoot.RootNode.Nodes[0].Selected = true;
                            //treeViewTemplates.ActiveNode = treeNodeRoot.RootNode.Nodes[0];
                        }
                        //string key = kvp.Key;
                        //if (kvp.Value.ReconType.Equals(ReconType.Position))
                        //{
                        //    UltraTreeNode treeNode = new UltraTreeNode(key, key);
                        //    treeViewTemplates.Nodes[ReconType.Position.ToString()].Nodes.Add(treeNode);
                        //}
                        //else
                        //{
                        //    UltraTreeNode treeNode = new UltraTreeNode(key, key);
                        //    treeViewTemplates.Nodes[ReconType.Transaction.ToString()].Nodes.Add(treeNode);
                        //}
                    }

                    #region Comments
                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1482
                    // [Recon] - Add Default templates in Company if if it don't have any templates in it
                    // dictClients.Keys provides the client ID of all the clients, 
                    // Here, linq is used to get list of distinct client IDs whose templates already exists.
                    // Therefore, 'except' is used, to pass only those client IDs as arguments, whose templates does not exists.
                    #endregion

                    AddDefaultNodes(dictClients.Keys.ToList<int>().Except(dictTemplates.Select(x => x.Value.ClientID).Distinct()).ToList<int>());
                    treeViewTemplates.ExpandAll();
                }
                else
                {
                    // Only those clients needs to be displayed which have a account mapping associated with them.
                    AddDefaultNodes(CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts().Keys.ToList());
                }



                //treeNodePosition.Selected = true;

                //treeViewTemplates.ActiveNode = treeNodePosition;
                //treeNodePosition.ExpandAll();
                //treeNodeTransaction.ExpandAll();
                //treeNodePosition.RootNode.Nodes[0].Selected = true;
                //treeViewTemplates.ActiveNode = treeNodePosition.RootNode.Nodes[0];

                //do no allow editing on F2 button press on ultratree nodes
                treeViewTemplates.Override.LabelEdit = Infragistics.Win.DefaultableBoolean.False;

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
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        private void BindOtherTree()
        {
            try
            {
                Dictionary<string, ReconTemplate> dictTemplates = ReconPrefManager.ReconPreferences.DictReconTemplates;

                //To clear the tree of any node before binding it afresh.0
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-161
                treeViewTemplatesOtherGroup.Nodes.Clear();
                Dictionary<int, string> dictClients = CachedDataManager.GetUserPermittedCompanyList();


                foreach (KeyValuePair<int, string> kvp in dictClients)
                {
                    treeViewTemplatesOtherGroup.Nodes.Add(kvp.Key.ToString(), kvp.Value);
                    foreach (string reconType in ReconPrefManager.ReconPreferences.getRootTemplates())
                    {
                        //add root node (e.g. Transaction or Position or PNL)
                        UltraTreeNode treeNodeRoot = new UltraTreeNode(kvp.Key.ToString() + Seperators.SEPERATOR_6 + reconType, reconType);
                        treeViewTemplatesOtherGroup.Nodes[kvp.Key.ToString()].Nodes.Add(treeNodeRoot);
                        treeNodeRoot.ExpandAll();
                    }
                }

                if (dictTemplates.Count > 0)
                {
                    foreach (KeyValuePair<string, ReconTemplate> kvp in dictTemplates)
                    {
                        string templateName = ReconUtilities.GetTemplateNameFromTemplateKey(kvp.Key);
                        if (ReconPrefManager.ReconPreferences.getRootTemplates().Contains(kvp.Value.ReconType.ToString()))
                        {

                            string reconType = kvp.Value.ReconType.ToString();
                            //updates the sorted column in the template
                            kvp.Value.SortingColumnOrder = ReconPrefManager.ReconPreferences.getSortedColumnsWithoutDataSet(reconType);
                            int clientID = kvp.Value.ClientID;
                            //add child node
                            UltraTreeNode treeNode1 = new UltraTreeNode(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName, templateName);
                            if (treeViewTemplatesOtherGroup.Nodes.Exists(clientID.ToString()) && treeViewTemplatesOtherGroup.Nodes[clientID.ToString()].Nodes.Exists(clientID.ToString() + Seperators.SEPERATOR_6 + reconType) && !treeViewTemplatesOtherGroup.Nodes[clientID.ToString()].Nodes[clientID.ToString() + Seperators.SEPERATOR_6 + reconType].Nodes.Exists(treeNode1.Key))
                            {
                                treeViewTemplatesOtherGroup.Nodes[clientID.ToString()].Nodes[clientID.ToString() + Seperators.SEPERATOR_6 + reconType].Nodes.Add(treeNode1);
                            }
                            //treeNodeRoot.Selected = true;
                            //treeViewTemplates.ActiveNode = treeNodeRoot;
                            //treeNodeRoot.ExpandAll();
                            //treeNodeRoot.RootNode.Nodes[0].Selected = true;
                            //treeViewTemplates.ActiveNode = treeNodeRoot.RootNode.Nodes[0];
                        }
                        //string key = kvp.Key;
                        //if (kvp.Value.ReconType.Equals(ReconType.Position))
                        //{
                        //    UltraTreeNode treeNode = new UltraTreeNode(key, key);
                        //    treeViewTemplates.Nodes[ReconType.Position.ToString()].Nodes.Add(treeNode);
                        //}
                        //else
                        //{
                        //    UltraTreeNode treeNode = new UltraTreeNode(key, key);
                        //    treeViewTemplates.Nodes[ReconType.Transaction.ToString()].Nodes.Add(treeNode);
                        //}
                    }
                    treeViewTemplatesOtherGroup.ExpandAll();
                }
                else
                {
                    AddDefaultNodes(CachedDataManagerRecon.GetInstance.GetAllCompanyAccounts().Keys.ToList());
                }



                //treeNodePosition.Selected = true;

                //treeViewTemplates.ActiveNode = treeNodePosition;
                //treeNodePosition.ExpandAll();
                //treeNodeTransaction.ExpandAll();
                //treeNodePosition.RootNode.Nodes[0].Selected = true;
                //treeViewTemplates.ActiveNode = treeNodePosition.RootNode.Nodes[0];

                //do no allow editing on F2 button press on ultratree nodes
                treeViewTemplatesOtherGroup.Override.LabelEdit = Infragistics.Win.DefaultableBoolean.False;

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
        // Modified by Ankit Gupta on 23 Sep, 2014
        // http://jira.nirvanasolutions.com:8080/browse/CHMW-1482
        /// <summary>
        ///modified by: sachin mishra 28 jan 2015
        ///Instead of LOGANDSHOW I have replaced to LOGANDTHROW
        /// </summary>
        /// <param name="lstClients"></param>
        private void AddDefaultNodes(List<int> lstClients)
        {
            try
            {
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-161

                foreach (int clientID in lstClients)
                {
                    foreach (string reconType in ReconPrefManager.ReconPreferences.getRootTemplates())
                    {
                        //TODO: Iterate for each client for which user have permission
                        UltraTreeNode treeNodeDefault = new UltraTreeNode(clientID.ToString() + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + reconType + "_DEFAULT", reconType + "_DEFAULT");
                        treeViewTemplates.Nodes[clientID.ToString()].Nodes[clientID.ToString() + Seperators.SEPERATOR_6 + reconType].Nodes.Add(treeNodeDefault);
                        ReconPrefManager.AddDefaultTemplate(clientID, reconType, reconType + "_DEFAULT");
                    }
                }
                //  treeViewTemplates.ExpandAll();
                //UltraTreeNode treeNodePositionDefault = new UltraTreeNode(TemplatePositionDefault, TemplatePositionDefault);
                //treeViewTemplates.Nodes[ReconType.Position.ToString()].Nodes.Add(treeNodePositionDefault);
                ////treeNodePositionDefault.Selected = true;
                ////treeViewTemplates.ActiveNode = treeNodePositionDefault;
                //UltraTreeNode treeNodeTransactionDefault = new UltraTreeNode(TemplateTransactionDefault, TemplateTransactionDefault);
                //treeViewTemplates.Nodes[ReconType.Transaction.ToString()].Nodes.Add(treeNodeTransactionDefault);

                //ReconPrefManager.AddDefaultTemplate(ReconType.Position.ToString(), TemplatePositionDefault);
                //ReconPrefManager.AddDefaultTemplate(ReconType.Transaction.ToString(), TemplateTransactionDefault);
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


        private void ultraTabReconTemplate_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                //First condition checks that a node is selected or not
                //second condition checks that selected node is not a recon type
                //third condition checks that selected node is not a client node
                if (treeViewTemplates.SelectedNodes.Count > 0 && (!ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Key)) && (!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text)))
                {

                    LoadDataForSelectedTab();
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

        private void ultraTabReconTemplate_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            try
            {
                //First condition checks that a node is selected or not
                //second condition checks that selected node is not a recon type
                //third condition checks that selected node is not a client node
                if (treeViewTemplates.SelectedNodes.Count > 0 && (!ReconPrefManager.ReconPreferences.getRootTemplates().Contains(treeViewTemplates.SelectedNodes[0].Text)) && (!treeViewTemplates.SelectedNodes[0].Text.Equals(treeViewTemplates.SelectedNodes[0].RootNode.Text)))
                {
                    //string key = ultraTabReconTemplate.SelectedTab.Key;
                    UpdateDataForSelectedTab();
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
        /// modified by: sachin mishra,28 jan 2015
        /// purpose: Add try catch block in leftover methods in Project (JIRA-CHMW-2408)
        /// </summary>
        /// <returns></returns>
        public bool IsUnsavedChanges()
        {
            //if (!treeViewTemplates.SelectedNodes[0].Key.Equals(ReconType.Transaction.ToString()) && !treeViewTemplates.SelectedNodes[0].Key.Equals(ReconType.Position.ToString()))
            //{
            //    string TemplateName = treeViewTemplates.SelectedNodes[0].Key;
            //    ReconTemplate reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(TemplateName);
            //    ctrlReconFilters1.UpdateReconFilters(reconTemplate);
            //    if (ctrlReconFilters1.IsUnsavedChanges)
            //    {
            //        _isUnSavedChanges = true;
            //    }

            //}
            bool isUnsavedChanges = _isUnSavedChanges;
            try
            {
                _isUnSavedChanges = false;
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
            return isUnsavedChanges;
        }

        /// <summary>
        /// checks if the new node text is null then no node is added
        /// if the new node label exist then an prompt is givem
        /// else new template is added in prefrences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewTemplates_AfterLabelEdit(object sender, NodeEventArgs e)
        {
            try
            {
                string templateName = string.Empty;
                string reconType = string.Empty;
                string clientID = string.Empty;
                treeViewTemplates.ActiveNode.Text = treeViewTemplates.ActiveNode.Text.Trim();
                //check that template name blank or not
                if (string.IsNullOrEmpty(treeViewTemplates.ActiveNode.Text))
                {
                    MessageBox.Show("Template name cannot be empty or blank spaces.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    treeViewTemplates.ActiveNode.Remove();
                    return;
                }
                if (treeViewTemplates.ActiveNode.Text.Contains('~'))
                {
                    MessageBox.Show("The character '~' is not valid in this field.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    treeViewTemplates.ActiveNode.Remove();
                    return;
                }
                //http://jira.nirvanasolutions.com:8080/browse/PRANA-4590
                if (Enum.GetNames(typeof(ReconType)).ToList().Contains(treeViewTemplates.ActiveNode.Text, StringComparer.InvariantCultureIgnoreCase))
                {
                    MessageBox.Show("Template name cannot be same as recon type", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    treeViewTemplates.ActiveNode.Remove();
                    return;
                }
                reconType = treeViewTemplates.ActiveNode.Parent.Text;
                clientID = treeViewTemplates.ActiveNode.RootNode.Key;
                templateName = string.Concat(treeViewTemplates.ActiveNode.Text);
                //check that template does not exist
                string templateKey = clientID + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName;
                if (!ReconPrefManager.ReconPreferences.CheckTemplateExists(templateKey))
                {
                    _isUnSavedChanges = true;
                    UltraTreeNode newNode = treeViewTemplates.ActiveNode;

                    //TODO: here client name is hard coded
                    //Here client id 0 is hardcoded
                    ReconPrefManager.AddDefaultTemplate(Convert.ToInt32(clientID), reconType, templateName);

                    newNode.Text = templateName;
                    newNode.Key = clientID + Seperators.SEPERATOR_6 + reconType + Seperators.SEPERATOR_6 + templateName;
                    newNode.Selected = true;
                    treeViewTemplates.ActiveNode = newNode;
                    if (!string.IsNullOrWhiteSpace(reconType))
                    {
                        treeViewTemplates.Nodes[clientID].ExpandAll();
                    }
                }
                else
                {
                    MessageBox.Show("Template Name Already Exists", "Recon", MessageBoxButtons.OK);
                    treeViewTemplates.ActiveNode.Remove();
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

        //private void treeViewTemplates_BeforeLabelEdit(object sender, CancelableNodeEventArgs e)
        //{
        //    try
        //    {
        //        //_previousText = e.TreeNode.Text;
        //        //if (ReconPrefManager.ReconPreferences.getRootTemplates().Contains(e.TreeNode.Text))
        //        //    isRootNode = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Invoke our policy that is responsible for making sure no secure information
        //        // gets out of our layer.
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        /// <summary>
        /// sets the active node on right mouse click &
        /// set the button on context menu that are to be visible for Recon Type,Template And Client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewTemplates_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //make the tree node active when right click to the node
                if (e.Button == MouseButtons.Right)
                {
                    UltraTreeNode selectedNode = treeViewTemplates.GetNodeFromPoint(e.Location);

                    // http://jira.nirvanasolutions.com:8080/browse/CHMW-1257
                    //Modified By: Ankit Gupta, 22 aug 2014
                    //Purpose: If no account group permission is given, right click menu or Context menu must not be displayed, that is no action
                    // should be taken if the user right clicks on treeViewTemplates.
                    if (selectedNode == null)
                    {
                        treeViewTemplates.ContextMenuStrip = null;
                        return;
                    }
                    treeViewTemplates.ContextMenuStrip = this.mnuTemplateSaveAs;
                    treeViewTemplates.ActiveNode = selectedNode;
                    treeViewTemplates.ActiveNode.Selected = true;
                    //if (treeViewTemplates.ActiveNode.Key.Equals(treeViewTemplates.ActiveNode.RootNode.Key) &&treeViewTemplates.ActiveNode.Parent==null)
                    //{
                    //    SendKeys.Send("{ESC}");
                    //}
                    Point pt = treeViewTemplates.PointToScreen(e.Location);
                    if (selectedNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && !treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                    {


                        mnuTemplateSaveAs.Items[0].Visible = true;
                        mnuTemplateSaveAs.Items[1].Visible = true;
                        mnuTemplateSaveAs.Items[2].Visible = true;
                        mnuTemplateSaveAs.Items[3].Visible = false;
                        mnuTemplateSaveAs.Items[4].Visible = true;
                        // clear previous sub menu items from copy to menu
                        (mnuTemplateSaveAs.Items[4] as ToolStripMenuItem).DropDownItems.Clear();
                        // add new sub menu items, that is all other company names except the one that is currently active
                        // and bind the click event.
                        //CHMW-2783
                        //if (treeViewTemplates.Nodes.Count == 1 || CachedDataManager.GetPranaReleaseType() != PranaReleaseViewType.CHMiddleWare)
                        if (treeViewTemplates.Nodes.Count == 1)
                        {
                            mnuTemplateSaveAs.Items[4].Visible = false;
                        }
                        else
                        {
                            mnuTemplateSaveAs.Items[4].Visible = true;
                            foreach (UltraTreeNode node in treeViewTemplates.Nodes)
                            {
                                if (!(node.RootNode.Key.Equals(selectedNode.RootNode.Key)))
                                {
                                    ToolStripMenuItem temp = new ToolStripMenuItem();
                                    temp.Name = "tempToolStripMenuItem";
                                    temp.Text = node.RootNode.Text;
                                    temp.Click += new System.EventHandler(this.TempToolStripMenuItem_Click);
                                    copyToToolStripMenuItem.DropDownItems.Add(temp);
                                }
                            }
                        }
                        //CHMW-2783
                        //if (CachedDataManager.GetPranaReleaseType() != PranaReleaseViewType.CHMiddleWare)
                        //{
                        //    mnuTemplateSaveAs.Items[5].Visible = true;
                        //}
                        //else

                        //Added by Pranay Deep Purpose : Jira-PRANA-10493
                        //http://jira.nirvanasolutions.com:8080/browse/PRANA-10493

                        if (this.FindForm().Name == "ReconPrefForm")
                        {
                            mnuTemplateSaveAs.Items[5].Visible = true;

                        }
                        mnuTemplateSaveAs.Show(pt);
                    }
                    else if (selectedNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                    {
                        mnuTemplateSaveAs.Items[0].Visible = false;
                        mnuTemplateSaveAs.Items[1].Visible = true;
                        mnuTemplateSaveAs.Items[2].Visible = false;
                        mnuTemplateSaveAs.Items[3].Visible = true;
                        mnuTemplateSaveAs.Items[4].Visible = false;
                        mnuTemplateSaveAs.Items[5].Visible = false;
                        mnuTemplateSaveAs.Show(pt);
                    }
                    else if (selectedNode != null && treeViewTemplates.ActiveNode.IsRootLevelNode)
                    {
                        mnuTemplateSaveAs.Items[0].Visible = false;
                        mnuTemplateSaveAs.Items[1].Visible = true;
                        mnuTemplateSaveAs.Items[2].Visible = false;
                        mnuTemplateSaveAs.Items[3].Visible = false;
                        mnuTemplateSaveAs.Items[4].Visible = false;
                        mnuTemplateSaveAs.Items[5].Visible = false;
                        mnuTemplateSaveAs.Show(pt);
                    }
                    else
                    {
                        treeViewTemplates.ContextMenuStrip = null;
                    }
                    //else
                    //{
                    //    Point pt = treeViewTemplates.PointToScreen(e.Location);
                    //    mnuTemplateSaveAs.Show(pt);
                    //}
                }
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

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraTreeNode newNode = new UltraTreeNode();
                string templateName = treeViewTemplates.ActiveNode.Text;
                string templateKey = treeViewTemplates.ActiveNode.Key;
                //string reconType = string.Empty;
                newNode.Text = templateName + "-copy";
                newNode.Key = templateKey + "-copy";
                if (treeViewTemplates.ActiveNode.Key != treeViewTemplates.ActiveNode.RootNode.Key && treeViewTemplates.ActiveNode.Parent.Key != treeViewTemplates.ActiveNode.RootNode.Key)
                {
                    //check template exists
                    if (!ReconPrefManager.ReconPreferences.CheckTemplateExists(newNode.Key))
                    {
                        _isUnSavedChanges = true;
                        treeViewTemplates.Nodes[treeViewTemplates.ActiveNode.RootNode.Key].Nodes[treeViewTemplates.ActiveNode.Parent.Key].Nodes.Add(newNode);
                        //recon type is same as that of root node
                        //reconType = treeViewTemplates.ActiveNode.Parent.Key;
                        ReconPrefManager.copyTemplate(templateKey, newNode.Key);
                        newNode.Selected = true;
                        treeViewTemplates.ActiveNode = newNode;
                    }
                    else
                    {
                        MessageBox.Show("Template Name Already Exists", "Recon", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Parent/Root Node cannot be copied as a Template", "Recon", MessageBoxButtons.OK);
                }
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


        private void UpdateTemplate(string templateKey)
        {
            try
            {

                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(templateKey);
                DataSet dsMastercolumnsExisting = template.DsMasterColumns;
                //Fetch custom columns which have formula expression not null
                DataRow[] CustomColumns = dsMastercolumnsExisting.Tables[0].Select(string.Format(ReconConstants.COLUMN_FormulaExpression + " <> ''"));
                DataSet dsMastercolumnsUpdated = new DataSet();

                #region Master Columns

                DataSet dsOldMasterColumns = template.DsMasterColumns.Copy();
                //path for the mastercolumns xml
                string xmlMasterColumnsPath = ReconPrefManager.ReconPreferences.XmlRulePath + "//MasterColumns.xml";
                string xmlMasterColumnsSchema = ReconPrefManager.ReconPreferences.XmlRulePath + "//MasterColumns.xsd";
                DataSet dsMasterColumnsDefault = new DataSet();
                dsMasterColumnsDefault.ReadXmlSchema(xmlMasterColumnsSchema);
                dsMasterColumnsDefault.ReadXml(xmlMasterColumnsPath);
                //updates the sorted column in the template
                template.SortingColumnOrder = ReconPrefManager.ReconPreferences.getSortedColumnsWithoutDataSet(template.ReconType.ToString());

                //Load master columns which have datasouce PB
                DataTable dtPBMasterColumns = ReconPrefManager.GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, DataSourceType.PrimeBroker);
                dtPBMasterColumns.TableName = ReconConstants.TABLENAME_PBGridMasterColumns;
                //Load master columns which have datasouce Nirvana
                DataTable dtNirvanaMasterColumns = ReconPrefManager.GetFilteredMasterColumns(dsMasterColumnsDefault, template.ReconType, DataSourceType.Nirvana);
                dtNirvanaMasterColumns.TableName = ReconConstants.TABLENAME_NirvanaGridColumns;
                //Add nirvana and pb tables to the dataset
                dsMastercolumnsUpdated.Tables.Add(dtNirvanaMasterColumns);
                dsMastercolumnsUpdated.Tables.Add(dtPBMasterColumns);
                //adds editable column to template for the recon type from MasterColumns.Xml File
                template.EditableColumns.Clear();

                foreach (DataRow item in dtNirvanaMasterColumns.Rows)
                {
                    if (item[ReconConstants.COLUMN_ISEditable].Equals(true))
                    {
                        template.EditableColumns.Add(item[ReconConstants.COLUMN_Name].ToString()); ;
                    }
                }
                //save checked preferences of master columns
                List<string> CheckedNirvanaColumns = new List<string>();
                List<string> CheckedPBColumns = new List<string>();
                //add selected columns for NirvanaTable
                foreach (DataRow NirvanaRow in template.DsMasterColumns.Tables[0].Rows)
                {
                    if (NirvanaRow[ReconConstants.COLUMN_ISSelected].Equals(true))
                    {
                        CheckedNirvanaColumns.Add(NirvanaRow[ReconConstants.COLUMN_Name].ToString());
                    }
                }
                //add selected columns for PBTable
                foreach (DataRow PBRow in template.DsMasterColumns.Tables[1].Rows)
                {
                    if (PBRow[ReconConstants.COLUMN_ISSelected].Equals(true))
                    {
                        CheckedPBColumns.Add(PBRow[ReconConstants.COLUMN_Name].ToString());
                    }
                }
                //add new default preferences to the dsmastercolumn dataset
                template.DsMasterColumns = dsMastercolumnsUpdated.Copy();
                //add custom columns row array to the new dataset tables 
                if (CustomColumns.Length > 0)
                {
                    foreach (DataRow row in CustomColumns)
                    {
                        template.DsMasterColumns.Tables[0].ImportRow(row);
                        template.DsMasterColumns.Tables[1].ImportRow(row);
                    }
                }
                //CHMW-2783
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare && template.DsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                if (template.DsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                {
                    foreach (DataRow customColumnRow in template.DsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows)
                    {
                        DataRow NewRow1 = template.DsMasterColumns.Tables[0].NewRow();
                        DataRow NewRow2 = template.DsMasterColumns.Tables[1].NewRow();
                        if (ReconUtilities.ValueExistInDataSet(dsOldMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, ReconConstants.COLUMN_Name, customColumnRow[ReconConstants.COLUMN_Name].ToString()) != null)
                        {
                            NewRow1.ItemArray = ReconUtilities.ValueExistInDataSet(dsOldMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, ReconConstants.COLUMN_Name, customColumnRow[ReconConstants.COLUMN_Name].ToString()).ItemArray;
                            NewRow2.ItemArray = ReconUtilities.ValueExistInDataSet(dsOldMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, ReconConstants.COLUMN_Name, customColumnRow[ReconConstants.COLUMN_Name].ToString()).ItemArray;

                            if (NewRow1 != null)
                            {
                                template.DsMasterColumns.Tables[0].Rows.Add(NewRow1);
                            }
                            if (NewRow2 != null)
                            {
                                template.DsMasterColumns.Tables[1].Rows.Add(NewRow2);
                            }
                        }
                    }
                }


                //save checked preferences of master columns
                List<string> NirvanaColumns = new List<string>();
                List<string> PBColumns = new List<string>();
                List<string> CommonColumns = new List<string>();
                //update checked new master columns
                foreach (DataRow NirvanaRow in template.DsMasterColumns.Tables[0].Rows)
                {
                    if (CheckedNirvanaColumns.Contains(NirvanaRow[ReconConstants.COLUMN_Name].ToString()))
                    {
                        NirvanaRow[ReconConstants.COLUMN_ISSelected] = true;
                    }
                    else
                    {
                        NirvanaRow[ReconConstants.COLUMN_ISSelected] = false;
                    }
                    if ((!NirvanaColumns.Contains(NirvanaRow["Name"].ToString()))
                     && (NirvanaRow["GroupType"].ToString().Equals(((int)ColumnGroupType.Nirvana).ToString())
                    || NirvanaRow["GroupType"].ToString().Equals(((int)ColumnGroupType.Both).ToString())))
                    {
                        NirvanaColumns.Add(NirvanaRow[ReconConstants.COLUMN_Name].ToString());
                    }
                }
                //add selected columns for PBTable
                foreach (DataRow PBRow in template.DsMasterColumns.Tables[1].Rows)
                {
                    if (CheckedPBColumns.Contains(PBRow[ReconConstants.COLUMN_Name].ToString()))
                    {
                        PBRow[ReconConstants.COLUMN_ISSelected] = true;
                    }
                    else
                    {
                        PBRow[ReconConstants.COLUMN_ISSelected] = false;
                    }
                    if ((!PBColumns.Contains(PBRow[ReconConstants.COLUMN_Name].ToString()))
                        && (PBRow[ReconConstants.COLUMN_GroupType].ToString().Equals(((int)ColumnGroupType.PrimeBroker).ToString())
                    || PBRow[ReconConstants.COLUMN_GroupType].ToString().Equals(((int)ColumnGroupType.Both).ToString())))
                    {
                        PBColumns.Add(PBRow[ReconConstants.COLUMN_Name].ToString());
                    }
                }

                //add selected columns for CommonColumn from any table
                foreach (DataRow CommonColumnsRow in template.DsMasterColumns.Tables[0].Rows)
                {
                    if (CheckedPBColumns.Contains(CommonColumnsRow[ReconConstants.COLUMN_Name].ToString()))
                    {
                        CommonColumnsRow[ReconConstants.COLUMN_ISSelected] = true;
                    }
                    else
                    {
                        CommonColumnsRow[ReconConstants.COLUMN_ISSelected] = false;
                    }
                    if ((!CommonColumns.Contains(CommonColumnsRow[ReconConstants.COLUMN_Name].ToString()))
                        && CommonColumnsRow[ReconConstants.COLUMN_GroupType].ToString().Equals(((int)ColumnGroupType.Common).ToString()))
                    {
                        CommonColumns.Add(CommonColumnsRow[ReconConstants.COLUMN_Name].ToString());
                    }
                }


                //load master columns UI
                ctrlMasterColumns1.LoadMasterColumns(template);

                #endregion

                #region Matching Rules

                DataSet dsMatchingRulesDefault = new DataSet();
                string MatchingRuleSchema = ReconPrefManager.ReconPreferences.XmlRulePath + "//XmlMatchingRule.xsd";
                string xmlMatchingRulePath = ReconPrefManager.ReconPreferences.XmlRulePath + "//XmlMatchingRule.xml";
                //Load matching rules
                dsMatchingRulesDefault.ReadXmlSchema(MatchingRuleSchema);
                dsMatchingRulesDefault.ReadXml(xmlMatchingRulePath);
                ReconPrefManager.RemoveRowsBasedOnTemplateReconType(template.ReconType, dsMatchingRulesDefault);
                DataTable dtOldMatchinRules = template.DsMatchingRules.Tables[1].Copy();
                //add spname from xmls

                DataRow[] result = dsMatchingRulesDefault.Tables["Rule"].Select("Name='" + template.ReconType + "'");
                foreach (DataRow row in result)
                {
                    template.SpName = row["SP"].ToString();
                }
                template.DsMatchingRules.Tables[1].Clear();
                template.DsMatchingRules.Tables[0].Merge(dsMatchingRulesDefault.Tables[0]);
                template.DsMatchingRules.Tables[1].Merge(dsMatchingRulesDefault.Tables[1]);
                DataSet ds = new DataSet();
                ds.Tables.Add(dtOldMatchinRules);

                foreach (DataRow NewRow in template.DsMatchingRules.Tables[ReconConstants.MatchingRulesRuleTableName].Rows)
                {
                    NewRow[ReconConstants.COLUMN_ISIncluded] = false;


                    DataRow OldRow = ReconUtilities.ValueExistInDataSet(ds, ReconConstants.MatchingRulesRuleTableName, ReconConstants.COLUMN_Name, NewRow[ReconConstants.COLUMN_Name].ToString());
                    if (OldRow != null)
                    {
                        //CHMW-2331	[Recon] Quantity mismatch not coming on position recon while quantity is mismatching
                        string Rule_ID = NewRow["Rule_ID"].ToString();
                        NewRow.ItemArray = OldRow.ItemArray;
                        NewRow["Rule_ID"] = Rule_ID;
                    }
                }
                //CHMW-2783
                //if (CachedDataManager.GetPranaReleaseType() == PranaReleaseViewType.CHMiddleWare && template.DsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                if (template.DsCustomColumns.Tables.Contains(ReconConstants.CustomColumnsTableName))
                {
                    foreach (DataRow customColumnRow in template.DsCustomColumns.Tables[ReconConstants.CustomColumnsTableName].Rows)
                    {
                        DataRow NewRow = template.DsMatchingRules.Tables[ReconConstants.MatchingRulesRuleTableName].NewRow();
                        if (ReconUtilities.ValueExistInDataSet(dsOldMasterColumns, ReconConstants.MasterColumnsNirvanaTableName, ReconConstants.COLUMN_Name, customColumnRow[ReconConstants.COLUMN_Name].ToString()) != null)
                        {
                            DataRow dr = ReconUtilities.ValueExistInDataSet(ds, dtOldMatchinRules.TableName, ReconConstants.COLUMN_Name, customColumnRow[ReconConstants.COLUMN_Name].ToString());
                            if (dr != null)
                            {
                                NewRow.ItemArray = dr.ItemArray;
                                if (NewRow != null)
                                {
                                    template.DsMatchingRules.Tables[ReconConstants.MatchingRulesRuleTableName].Rows.Add(NewRow);
                                }
                            }
                        }
                    }
                }

                //CHMW-2262 [Recon][Improvement] - Add diff column automatically if column is of numeric type.
                //Used to update  numeric columns list to add new columns in dictionary
                template.DsMatchingRules = template.DsMatchingRules;

                //template.DsMatchingRules.Tables[1].Merge(dsMatchingRulesDefault.Tables[1]);
                //DataTable NewTable = template.DsMatchingRules.Tables[1].DefaultView.ToTable(true);
                //template.DsMatchingRules.Tables[1].Clear();
                //template.DsMatchingRules.Tables[1].Merge(NewTable);
                ctrlMatchingRules1.LoadMatchingRules(template);
                ctrlReconGeneralInfo1.LoadMatchingRules(template);
                ctrlReconGroupByColumns1.LoadGroupCriteria(template);

                #endregion

                #region xslt path
                //if (string.IsNullOrEmpty(template.XsltPath))
                //{
                //    foreach (XsltSetup xsltsetup in template.XsltMappingList)
                //    {
                //        template.XsltPath = xsltsetup.XsltName;
                //    }
                //}
                #endregion

                #region Exception Report Columns
                //moved to recon utilities
                ReconUtilities.UpdateExceptionReportLayout(template, NirvanaColumns, PBColumns, CommonColumns);
                ctrlReconReportLayout1.LoadReconReportLayout(template);

                #endregion

                //For New Grouping Logic for PNL
                //get grouping summary for the PNL
                template.DictGroupingSummary = ReconPrefManager.GetGroupingSummary(template.DsMasterColumns, template.ReconType);

                //unsaved changes flag set to true
                _isUnSavedChanges = true;
                //flag true to save the changes
                template.IsDirtyForSaving = true;
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



        /// <summary>
        /// Update the teplate/templates from the latest recon File
        /// As per the selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int clientID = Convert.ToInt32(treeViewTemplates.ActiveNode.RootNode.Key);
                string templateName = treeViewTemplates.ActiveNode.Text;
                //update template by latest recon files
                if (!string.IsNullOrWhiteSpace(templateName) && (!treeViewTemplates.ActiveNode.IsRootLevelNode) && (!treeViewTemplates.ActiveNode.Parent.IsRootLevelNode))
                {
                    UpdateTemplate(treeViewTemplates.ActiveNode.Key);
                }
                //create default template for any recon type which dont have default template
                else if (!string.IsNullOrWhiteSpace(templateName) && (!treeViewTemplates.ActiveNode.IsRootLevelNode) && (treeViewTemplates.ActiveNode.Parent.IsRootLevelNode) && (treeViewTemplates.ActiveNode.Nodes.Count == 0))
                {
                    //if (ReconPrefManager.ReconPreferences.GetListOfTemplates(templateName, clientID).Count == 0)
                    if (treeViewTemplates.ActiveNode.Nodes.Count == 0)
                    {
                        UltraTreeNode newNode = new UltraTreeNode();
                        templateName += "_DEFAULT";
                        treeViewTemplates.Nodes[treeViewTemplates.ActiveNode.RootNode.Key].Nodes[treeViewTemplates.ActiveNode.Key].Nodes.Add(newNode);
                        string reconType = treeViewTemplates.ActiveNode.Text;

                        ReconPrefManager.AddDefaultTemplate(clientID, reconType, templateName);
                        //tbtemplateName.Text = string.Empty;
                        newNode.Text = templateName;
                        newNode.Key = ReconUtilities.GetTemplateKeyFromParameters(newNode.RootNode.Key, newNode.Parent.Text, templateName);
                        newNode.Selected = true;
                        treeViewTemplates.ActiveNode = newNode;
                        //_isUnSavedChanges = true;
                        if (!string.IsNullOrWhiteSpace(reconType))
                        {
                            treeViewTemplates.Nodes[treeViewTemplates.ActiveNode.RootNode.Key].ExpandAll();
                        }
                    }
                }
                //update all the templates for recon type
                else if (!string.IsNullOrWhiteSpace(templateName) && (!treeViewTemplates.ActiveNode.IsRootLevelNode) && (treeViewTemplates.ActiveNode.Nodes.Count > 0))
                {
                    foreach (UltraTreeNode childNode in treeViewTemplates.ActiveNode.Nodes)
                    {
                        UpdateTemplate(childNode.Key);
                        treeViewTemplates.Nodes[treeViewTemplates.ActiveNode.RootNode.Key].Nodes[treeViewTemplates.ActiveNode.Key].ExpandAll();
                    }
                }
                //if the selected node is root level node
                else if (!string.IsNullOrWhiteSpace(templateName) && (treeViewTemplates.ActiveNode.IsRootLevelNode) && (treeViewTemplates.ActiveNode.Nodes.Count > 0))
                {
                    foreach (UltraTreeNode reconTypeNode in treeViewTemplates.ActiveNode.Nodes)
                    {
                        //create default template for any recon type which dont have default template
                        if (reconTypeNode.Nodes.Count == 0)
                        {
                            if (ReconPrefManager.ReconPreferences.GetListOfTemplates(templateName, clientID).Count == 0)
                            {
                                UltraTreeNode newNode = new UltraTreeNode();
                                templateName = reconTypeNode.Text + "_DEFAULT";
                                treeViewTemplates.Nodes[reconTypeNode.RootNode.Key].Nodes[reconTypeNode.Key].Nodes.Add(newNode);


                                ReconPrefManager.AddDefaultTemplate(clientID, reconTypeNode.Text, templateName);
                                //tbtemplateName.Text = string.Empty;
                                newNode.Text = templateName;
                                newNode.Key = ReconUtilities.GetTemplateKeyFromParameters(newNode.RootNode.Key, newNode.Parent.Text, templateName);
                                newNode.Selected = true;
                                treeViewTemplates.ActiveNode = newNode;
                                //if (!reconTypeNode.Key.Equals(string.Empty))
                                //{
                                //    treeViewTemplates.Nodes[clientID.ToString()].Nodes[reconTypeNode.Key].ExpandAll();
                                //}
                            }
                        }
                        //update all the templates for recon type
                        else if (reconTypeNode.Nodes.Count > 0)
                        {
                            foreach (UltraTreeNode childNode in reconTypeNode.Nodes)
                            {
                                UpdateTemplate(childNode.Key);
                            }
                        }
                    }
                    treeViewTemplates.Nodes[clientID.ToString()].ExpandAll();
                }
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
        /// <summary>
        /// This method checks that recon is set according to the latest preferences
        /// </summary>
        private void CheckForPreferencesUpdated()
        {
            try
            {
                if (ReconPrefManager.ReconPreferences != null && ReconPrefManager.ReconPreferences.DictReconTemplates.Count > 0)
                {
                    //DataSet dsUpdatePreferences = new DataSet();
                    //bool isRequiredToUpdateReconPreferences = false;
                    //load xml CheckToUpdateReconPreferences with its schema
                    //string xmlUpdatePreferencesPath = ReconPrefManager.xmlRulePath + "//CheckToUpdateReconPreferences.xml";
                    //string xmlUpdatePreferencesSchema = ReconPrefManager.xmlRulePath + "//CheckToUpdateReconPreferences.xsd";
                    //if (File.Exists(xmlUpdatePreferencesPath) && File.Exists(xmlUpdatePreferencesSchema))
                    //{
                    //    dsUpdatePreferences.ReadXmlSchema(xmlUpdatePreferencesSchema);
                    //    dsUpdatePreferences.ReadXml(xmlUpdatePreferencesPath);
                    //    if (dsUpdatePreferences != null && dsUpdatePreferences.Tables[0] != null)
                    //    {
                    //        foreach (DataRow row in dsUpdatePreferences.Tables[0].Rows)
                    //        {
                    //            bool.TryParse(row["IsUpdateReconPreferences"].ToString(), out isRequiredToUpdateReconPreferences);
                    //            row["IsUpdateReconPreferences"] = !isRequiredToUpdateReconPreferences;
                    //        }
                    //    }
                    //    if (isRequiredToUpdateReconPreferences)
                    //Code already exist in ReconPrefManager
                    if (ReconPrefManager.IsTemplatesToBeUpdated())
                    {
                        foreach (KeyValuePair<string, ReconTemplate> kvp in ReconPrefManager.ReconPreferences.DictReconTemplates)
                        {
                            UpdateTemplate(kvp.Key);
                            //_isPreferencesUpdated = true;
                        }
                    }
                    //    if (isPreferencesUpdated)
                    //    {
                    //        //save updated preferences
                    //        ReconPrefManager.SavePreferences(ReconPrefManager.ReconPreferences);
                    //        //write xml CheckToUpdateReconPreferences.xml after updating field IsUpdateReconPreferences from true to false so that preference not update everytime
                    //        using (XmlTextWriter writer = new XmlTextWriter(xmlUpdatePreferencesPath, Encoding.UTF8))
                    //        {
                    //            writer.Formatting = Formatting.Indented;
                    //            XmlSerializer serializer;
                    //            serializer = new XmlSerializer(typeof(DataSet));
                    //            serializer.Serialize(writer, dsUpdatePreferences);
                    //            writer.Flush();
                    //            writer.Close();
                    //        }
                    //    }
                    //}
                }
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

        /// <summary>
        /// get template name
        /// </summary>
        internal string GetTemplateName()
        {
            string templateName = string.Empty;
            try
            { //First condition checks if node is selected or not
                //second condition checks that selected node is not a rootnode(client)
                //third condition checks that selected node is not a recontype node
                if (treeViewTemplates.ActiveNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && !treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                {
                    templateName = treeViewTemplates.ActiveNode.Text;
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
            return templateName;
        }
        /// <summary>
        /// get template key
        /// </summary>
        internal string GetTemplateKey()
        {
            string templateKey = string.Empty;
            try
            {
                //First condition checks if node is selected or not
                //second condition checks that selected node is not a rootnode(client)
                //third condition checks that selected node is not a recontype node
                if (treeViewTemplates.ActiveNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && !treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                {
                    templateKey = treeViewTemplates.SelectedNodes[0].Key;
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
            return templateKey;
        }
        /// <summary>
        /// Set template name
        /// </summary>
        /// <returns></returns>
        internal void SetTemplateName(string previousTemplateName, string newTemplateName)
        {
            try
            {
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-4590                
                if (!string.IsNullOrEmpty(CachedDataManagerRecon.GetExecutionDashboardFilePath(GetTemplateKey())))
                {
                    MessageBox.Show("Template in use, cannot be modified.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                if (Enum.GetNames(typeof(ReconType)).ToList().Contains(newTemplateName, StringComparer.InvariantCultureIgnoreCase))
                {
                    MessageBox.Show("Template name cannot be same as recon type", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                string newTemplateKey = treeViewTemplates.ActiveNode.Parent.Key + Seperators.SEPERATOR_6 + newTemplateName;
                //check if node with same name already exist
                if (treeViewTemplates.ActiveNode.Parent.Nodes.Exists(newTemplateKey))
                {
                    MessageBox.Show("Template with the same name already exists, Please enter a different name", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                _isUnSavedChanges = true;
                //previous text and new text are not same
                treeViewTemplates.ActiveNode.Key = newTemplateKey;
                string previousTemplateKey = treeViewTemplates.ActiveNode.Parent.Key + Seperators.SEPERATOR_6 + previousTemplateName;
                treeViewTemplates.ActiveNode.Text = newTemplateName;
                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(previousTemplateKey);

                if (template == null)
                {
                    return;
                }
                ReconPrefManager.ReconPreferences.DeleteTemplates(previousTemplateKey);
                template.IsDirtyForSaving = true;
                template.TemplateKey = newTemplateKey;
                template.TemplateName = newTemplateName;
                ReconPrefManager.ReconPreferences.UpdateTemplates(treeViewTemplates.ActiveNode.Key, template);
                string layoutFilePath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + ReconConstants.RECONLAYOUT + @"\" + previousTemplateKey;
                string newLayoutFilePath = Application.StartupPath + @"\" + ApplicationConstants.PREFS_FOLDER_NAME + @"\" + ReconConstants.RECONLAYOUT + @"\" + newTemplateKey;

                if (File.Exists(layoutFilePath))
                {
                    File.Copy(layoutFilePath, newLayoutFilePath);
                    File.Delete(layoutFilePath);
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
        /// <summary>
        /// Get recon type for the selected node
        /// </summary>
        /// <returns></returns>
        internal string GetReconType()
        {
            string reconType = string.Empty;
            try
            {
                //First condition checks if node is selected or not
                //second condition checks that selected node is not a rootnode(client)
                //third condition checks that selected node is not a recontype node
                if (treeViewTemplates.ActiveNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && !treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                {
                    reconType = treeViewTemplates.ActiveNode.Parent.Text;
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
            return reconType;
        }
        /// <summary>
        /// Get ClientID for the selected node
        /// </summary>
        /// <returns></returns>
        internal string GetClientID()
        {
            string ClientID = string.Empty;
            try
            {
                //First condition checks if node is selected or not
                //second condition checks that selected node is not a rootnode(client)
                //third condition checks that selected node is not a recontype node               
                if (treeViewTemplates.ActiveNode != null && !treeViewTemplates.ActiveNode.IsRootLevelNode && !treeViewTemplates.ActiveNode.Parent.IsRootLevelNode)
                {
                    ClientID = treeViewTemplates.ActiveNode.RootNode.Key;
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
            return ClientID;
        }
        /// <summary>
        /// View and hide recon pref tabs based on the preferences given in the xml file
        /// </summary>
        private void SetUPViewForReconUI()
        {
            try
            {

                //PranaReleaseViewType uiView = CachedDataManager.GetPranaReleaseType();
                if (this.FindForm() != null)
                {
                    switch (this.FindForm().Name)
                    {
                        case "ReconPrefForm":
                            ultraTabReconTemplate.Tabs[TAB_GeneralInfo].Visible = false;
                            ultraTabReconTemplate.Tabs[TAB_MATCHINGRULES].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_XSLTMAPPING].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_RECONFILTERS].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_GRIDLAYOUT].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_ReconReportLayout].Visible = true;
                            break;
                        case "frmReconCancelAmend":
                            ultraTabReconTemplate.Tabs[TAB_GeneralInfo].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_MATCHINGRULES].Visible = false;
                            ultraTabReconTemplate.Tabs[TAB_XSLTMAPPING].Visible = false;
                            ultraTabReconTemplate.Tabs[TAB_RECONFILTERS].Visible = false;
                            ultraTabReconTemplate.Tabs[TAB_GRIDLAYOUT].Visible = false;
                            ultraTabReconTemplate.Tabs[TAB_ReconReportLayout].Visible = true;
                            ctrlReconReportLayout1.groupBox1.Visible = false;
                            break;
                        default:
                            ultraTabReconTemplate.Tabs[TAB_GeneralInfo].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_MATCHINGRULES].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_XSLTMAPPING].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_RECONFILTERS].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_GRIDLAYOUT].Visible = true;
                            ultraTabReconTemplate.Tabs[TAB_ReconReportLayout].Visible = true;
                            break;
                    }
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


        internal void SaveTemplate()
        {
            try
            {
                //UpdateDataForSelectedTab();
                ReconPreferences reconPref = ReconPrefManager.ReconPreferences;
                //CHMW-3160	[Recon] Amendments are not saving if a column is under different group
                if (!ReconPrefManager.SavePreferences(reconPref))
                {
                    MessageBox.Show("Preference Cannot be saved now. Please try again later.", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _isUnSavedChanges = true;
                }
                else
                {
                    //modified by amit on 16.03.2015
                    //http://jira.nirvanasolutions.com:8080/browse/CHMW-3396
                    ReconUtilities.ReconPreferenceSaved();
                    MessageBox.Show("Sucessfully Saved and Applied !   ", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        internal void RunRecon(ReconParameters reconParameters)
        {
            try
            {
                reconParameters.TemplateKey = GetTemplateKey();
                ReconTemplate template = ReconPrefManager.ReconPreferences.GetTemplates(reconParameters.TemplateKey);
                if (template != null)
                {
                    reconParameters.FormatName = template.FormatName;
                    reconParameters.ReconDateType = template.ReconDateType;
                    reconParameters.DTRunDate = DateTime.Now;

                    //Logger.LoggerWrite("Temlate name:" + templateName);
                    //string reconType = GetReconType();
                    //Logger.LoggerWrite("Recon type:" + reconType);
                    //string clientID = GetClientID();
                    //object[] arguments = new object[1];
                    //arguments[0] = reconParameters;
                    //arguments[1] = reconType;
                    //arguments[2] = clientID;
                    //arguments[3] = reconDateRange[0];
                    //arguments[4] = reconDateRange[1];
                    //arguments[5] = template.FormatName;
                    //arguments[6] = template.ReconDateType.ToString();
                    //arguments[7] = DateTime.Now.ToString(ApplicationConstants.DateFormat);

                    //btnRunRecon.Enabled = false;
                    _bgRunReconcile.RunWorkerAsync(reconParameters);
                }
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

        private void _bgRunReconcile_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //object arguments = e.Argument as object[];
                ReconParameters reconParameters = (ReconParameters)e.Argument;
                //string reconType = arguments[1].ToString();
                //string clientID = arguments[2].ToString();
                //string fromDate = arguments[3].ToString();
                //string toDate = arguments[4].ToString();
                //string formatName = arguments[5].ToString();
                //string runByDate = arguments[6].ToString();
                //string runDate = arguments[7].ToString();
                if (!(string.IsNullOrEmpty(reconParameters.TemplateKey) || string.IsNullOrEmpty(reconParameters.ReconType) || string.IsNullOrEmpty(reconParameters.ClientID)))
                {
                    //Commenting the code as it was creating duplicate xmls http://jira.nirvanasolutions.com:8080/browse/CHMW-477
                    //TaskInfo info = new TaskInfo() { TaskId = 2, TaskName = "Recon", AssemblyName = "Prana.Tools.dll", QClassName = "Prana.Tools.ctrlAdHocRecon" };
                    //code here is implemented in ReconManager                    
                    reconParameters.IsReconReportToBeGenerated = true;
                    ReconManager.ExecuteTask(reconParameters); //Recon Report is to be generated
                }
                else
                {
                    MessageBox.Show("Please select a template", "Reconciliation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void _bgRunReconcile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //object[] results = e.Result as object[];
                //DataSet ds = results[0] as DataSet;
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally
            {
                //btnRunRecon.Enabled = true;
            }
        }
        /// <summary>
        /// deletes the selected node
        /// code of delete button already implemented is used
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CachedDataManagerRecon.GetExecutionDashboardFilePath(GetTemplateKey())))
                {
                    MessageBox.Show("Template in use, cannot be modified.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                btnDeleteTemplates_Click(null, null);
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
        /// <summary>
        /// add a new node with no name\
        /// user will add name and node will be added in after Laebel edit event of tree view template
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraTreeNode newNode = new UltraTreeNode();
                newNode.Text = string.Empty;
                treeViewTemplates.ActiveNode.Nodes.Add(newNode);
                treeViewTemplates.ActiveNode = newNode;
                treeViewTemplates.ActiveNode.BeginEdit();
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
        private void treeViewTemplatesOtherGroup_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    treeViewTemplatesOtherGroup.ContextMenuStrip = this.mnuTemplateSaveAs;
                    UltraTreeNode selectedNode = treeViewTemplatesOtherGroup.GetNodeFromPoint(e.Location);
                    treeViewTemplatesOtherGroup.ActiveNode = selectedNode;
                    treeViewTemplatesOtherGroup.ActiveNode.Selected = true;
                    Point pt = treeViewTemplatesOtherGroup.PointToScreen(e.Location);
                    if (selectedNode != null && !treeViewTemplatesOtherGroup.ActiveNode.IsRootLevelNode && !treeViewTemplatesOtherGroup.ActiveNode.Parent.IsRootLevelNode)
                    {
                        mnuTemplateSaveAs.Items[0].Visible = false;
                        mnuTemplateSaveAs.Items[1].Visible = false;
                        mnuTemplateSaveAs.Items[2].Visible = false;
                        mnuTemplateSaveAs.Items[3].Visible = false;
                        mnuTemplateSaveAs.Items[4].Visible = true;
                        copyToToolStripMenuItem.DropDownItems.Clear();
                        foreach (UltraTreeNode node in treeViewTemplates.Nodes)
                        {
                            if (node.IsRootLevelNode)
                            {
                                ToolStripMenuItem newClient = new ToolStripMenuItem();
                                newClient.Text = node.Text;
                                copyToToolStripMenuItem.DropDownItems.Add(newClient);
                                newClient.Click += new System.EventHandler(this.newClientCopytoToolStripMenuItem_Click);
                            }
                        }
                        mnuTemplateSaveAs.Show(pt);
                    }
                    else
                    {
                        treeViewTemplatesOtherGroup.ContextMenuStrip = null;
                    }
                }
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

        private void newClientCopytoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraTreeNode newNode = new UltraTreeNode();
                // newNode = treeViewTemplatesOtherGroup.ActiveNode;
                newNode.Text = treeViewTemplatesOtherGroup.ActiveNode.Text + "_new";

                string templateName = treeViewTemplatesOtherGroup.ActiveNode.Text;
                string reconType = treeViewTemplatesOtherGroup.ActiveNode.Parent.Text;
                //TODO : HArdCoded here reconype key and client id is to be changed 
                string reconTypeKey = treeViewTemplatesOtherGroup.ActiveNode.Parent.Key;
                string clientID = "5";
                string templateKey = ReconUtilities.GetTemplateKeyFromParameters(clientID, reconType, templateName);
                newNode.Key = templateKey + "_new";
                if (!treeViewTemplates.Nodes[clientID].Nodes[reconTypeKey].Nodes.Exists(newNode.Key))
                {

                    _isUnSavedChanges = true;
                    treeViewTemplates.Nodes[clientID].Nodes[treeViewTemplatesOtherGroup.ActiveNode.Parent.Key].Nodes.Add(newNode);
                    // ReconPrefManager.copyTemplate(templateKey, newNode.Key);
                    ReconTemplate templateToCopy = ReconPrefManager.ReconPreferences.GetTemplates(treeViewTemplatesOtherGroup.ActiveNode.Key);
                    //int hashcodeold = templateToCopy.GetHashCode();
                    ReconTemplate newTemplate = new ReconTemplate();
                    newTemplate = (ReconTemplate)(DeepCopyHelper.Clone(templateToCopy));
                    newTemplate.IsDirtyForSaving = true;
                    ReconPrefManager.ReconPreferences.UpdateTemplates(newNode.Key, newTemplate);
                    newNode.Selected = true;
                    treeViewTemplates.ActiveNode = newNode;
                }
                else
                {
                    MessageBox.Show("Template Name Already Exists", "Recon", MessageBoxButtons.OK);
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

        private void treeViewTemplatesOtherGroup_BeforeSelect(object sender, Infragistics.Win.UltraWinTree.BeforeSelectEventArgs e)
        {
            try
            {
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

        //private void treeViewTemplatesOtherGroup_BeforeLabelEdit(object sender, Infragistics.Win.UltraWinTree.CancelableNodeEventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        //private void treeViewTemplatesOtherGroup_AfterSelect(object sender, Infragistics.Win.UltraWinTree.SelectEventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}

        private void ctrlReconTemplate_Load(object sender, System.EventArgs e)
        {
            try
            {
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
                    this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
                    this.splitContainer3.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
                    this.splitContainer4.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
                    //this.splitContainer5.BackColor = System.Drawing.Color.FromArgb(209, 210, 212);
                }
                if (!CustomThemeHelper.IsDesignMode())
                {
                    if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                    {
                        SetButtonsColor();
                    }
                    SetUPViewForReconUI();

                }
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

        private void SetButtonsColor()
        {
            try
            {
                btnDeleteTemplates.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnDeleteTemplates.ForeColor = System.Drawing.Color.White;
                btnDeleteTemplates.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnDeleteTemplates.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnDeleteTemplates.UseAppStyling = false;
                btnDeleteTemplates.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnAddTemplates.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddTemplates.ForeColor = System.Drawing.Color.White;
                btnAddTemplates.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddTemplates.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddTemplates.UseAppStyling = false;
                btnAddTemplates.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
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

        //private void treeViewTemplatesOtherGroup_AfterLabelEdit(object sender, Infragistics.Win.UltraWinTree.NodeEventArgs e)
        //{
        //    try
        //    {
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        /// <summary>
        /// checks weather a node selected in tree view is template or not
        /// </summary>
        /// <returns></returns>
        internal bool isTemplateSelected()
        {
            string templateKey = GetTemplateKey();
            //Logger.LoggerWrite("Temlate name:" + templateName);
            string reconType = GetReconType();
            //Logger.LoggerWrite("Recon type:" + reconType);
            string clientID = GetClientID();
            if (!(string.IsNullOrEmpty(templateKey) || string.IsNullOrEmpty(reconType) || string.IsNullOrEmpty(clientID)))
            {
                return true;
            }
            return false;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //get template name
                //null or empty value if there is any issue
                string prevTemplateName = GetTemplateName();
                if (!string.IsNullOrEmpty(prevTemplateName))
                {
                    //inputs new value
                    string newTemplateName = Microsoft.VisualBasic.Interaction.InputBox("Enter New Name:", "Reconciliation Template Rename", prevTemplateName).Trim();
                    //chck if new name is Equal to previous name or user pressed cancle button
                    if (!string.IsNullOrEmpty(newTemplateName))
                    {
                        if (!newTemplateName.Contains('~'))
                        {
                            //changes the template name
                            SetTemplateName(prevTemplateName, newTemplateName);
                        }
                        else
                        {
                            MessageBox.Show("The character '~' is not calid in this field.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Template name cannot be empty or blank spaces.", "Recon Templates", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
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

        /// <summary>
        /// Sub menu of copy To menu item is dynamically generated at run time, therefore this method is named as TempToolStripMenuItem_Click.
        /// This method is called when a sub menu of copy to menu is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void TempToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UltraTreeNode newNode = new UltraTreeNode();
                string templateName = treeViewTemplates.ActiveNode.Text;
                string templateKey = treeViewTemplates.ActiveNode.Key;
                newNode.Text = templateName + "-copy";
                if (treeViewTemplates.ActiveNode.Key != treeViewTemplates.ActiveNode.RootNode.Key && treeViewTemplates.ActiveNode.Parent.Key != treeViewTemplates.ActiveNode.RootNode.Key)
                {
                    _isUnSavedChanges = true;
                    // get company id from sender, as 'sender' returned by method parameters contains only company name.
                    int ClientID = CachedDataManager.GetCompanyID(sender.ToString());
                    if (ClientID != -1)
                    {
                        UltraTreeNode node = treeViewTemplates.Nodes[ClientID.ToString()];
                        string templatekey = ReconUtilities.GetTemplateKeyFromParameters(node.Key, treeViewTemplates.ActiveNode.Parent.Text, newNode.Text);
                        newNode.Key = templatekey;
                        //check if template already exists
                        if (!ReconPrefManager.ReconPreferences.CheckTemplateExists(newNode.Key))
                        {
                            // add new tree node
                            treeViewTemplates.Nodes[node.Key].Nodes[node.Key + "~" + treeViewTemplates.ActiveNode.Parent.Text].Nodes.Add(newNode);
                            // copy the template
                            ReconPrefManager.copyTemplate(templateKey, newNode.Key);
                            newNode.Selected = true;
                            treeViewTemplates.ActiveNode = newNode;
                        }
                        else
                        {
                            MessageBox.Show("Cannot copy, as template with same name already exists.", "Recon", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Parent/Root Node cannot be copied as a Template", "Recon", MessageBoxButtons.OK);
                }
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
    }
    #endregion
}

