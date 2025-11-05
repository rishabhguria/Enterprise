//Created by:Bharat Raturi, Date: 02/13/2014
//Purpose: User defined control to show the Strategy to master strategy mapping
using Infragistics.Win;
using Infragistics.Win.UltraWinListView;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.StrategyCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.StrategyUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.StrategyDeleted, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterStrategyCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterStrategyUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.MasterStrategyDeleted, ShowAuditUI = false)]
    public partial class MasterStrategyMapping : UserControl, IAuditSource
    {
        /// <summary>
        /// Constructor to initialize MasterStrategyMapping 
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public MasterStrategyMapping()
        {
            InitializeComponent();
        }
        void MasterStrategyMapping_Load(object sender, System.EventArgs e)
        {
            grdStrategyList.grdStrategyRowActivated += grdStrategyList_grdStrategyRowActiated;
        }

        void grdStrategyList_grdStrategyRowActiated(object sender, int e)
        {
            RefreshStrategyAuditFor(e);
        }

        void grdStrategyList_strategyDeleted(object sender, int e)
        {
            AuditStrategyDeletion(e);
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.grdStrategyList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.grdStrategyList.ForeColor = System.Drawing.Color.White;
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
                this.ForeColor = System.Drawing.Color.White;
                this.UBtnUnSelectAssignedStrategies.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllUnSelectAssignedStrategies.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnSelectUnassignedStrategies.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.uBtnAllSelectUnassignedStrategies.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));

                this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox1.ForeColor = System.Drawing.Color.White;

                this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox2.ForeColor = System.Drawing.Color.White;

                this.groupBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.groupBox4.ForeColor = System.Drawing.Color.White;

                this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
                this.groupBox3.ForeColor = System.Drawing.Color.White;
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

        public Strategies strategies = new Strategies();
        /// <summary>
        /// old master Strategy name needed at rename master Strategy 
        /// </summary>
        String _oldMasterStrategyName;

        /// <summary>
        /// Variable to hold the strategy ID
        /// </summary>
        //int _tempID;

        /// <summary>
        /// set true or false is item in exited Edit Mode
        /// </summary>
        bool _isItemExistedEditMode = false;

        /// <summary>
        /// change master Strategy in dictionary on basis of _ismasterStrategyId 1 for rename, 0 for add and 2 for delete 
        /// </summary>
        int _isMasterStrategyId = -1;

        /// <summary>
        /// ID of the current company
        /// </summary>
        public int _companyID = 0;

        //added by: Bharat raturi, 23 apr 2014
        //purpose: flag variable to check if many to many mapping is allowed
        public bool _isManyToManyMapping = false;

        /// <summary>
        /// create object of text box at run time
        /// </summary>
        private TextBox txtRenameAdd = new TextBox();

        /// <summary>
        /// initialized data at load time
        /// </summary>
        public void InitializeControl(int companyID, bool isManyToManyMapping)
        {
            try
            {
                this._companyID = companyID;
                RefreshMasterStrategyAuditFor(int.MinValue);
                RefreshStrategyAuditFor(int.MinValue);
                grdStrategyList.SetupControl(_companyID);
                _isManyToManyMapping = isManyToManyMapping;
                chkBoxManyToMany.Checked = _isManyToManyMapping;
                //initialize textbox
                InitTextBoxes();
                StrategyMasterStrategyMappingManager.InitializeData(companyID);
                grdStrategyList.SetupControl(_companyID);
                listMasterStrategy.ItemSettings.AllowEdit = DefaultableBoolean.True;
                BindMasterStrategy();
                //modified by: Bharat raturi, 23 apr 2014
                //purpose: Bind unmapped strategies according to the chosen mapping style
                //bind Unmapped Strategy to control
                //BindUnmappedStrategies();
                if (chkBoxManyToMany.Checked)
                {
                    BindUnmappedStrategies();
                }
                else
                {
                    BindUnMappedStrategiesForOneToMany();
                }
                listMasterStrategy.ItemSettings.HideSelection = false;

                if (listMasterStrategy.SelectedItems.Count == 1)
                {
                    uBtnAllUnSelectAssignedStrategies.Enabled = true;
                    uBtnAllSelectUnassignedStrategies.Enabled = true;
                    uBtnSelectUnassignedStrategies.Enabled = true;
                    UBtnUnSelectAssignedStrategies.Enabled = true;
                }
                else
                {
                    uBtnAllUnSelectAssignedStrategies.Enabled = false;
                    uBtnAllSelectUnassignedStrategies.Enabled = false;
                    uBtnSelectUnassignedStrategies.Enabled = false;
                    UBtnUnSelectAssignedStrategies.Enabled = false;
                    //uLstViewUnAssignedStrategies.Items.Clear();
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
        /// initialized  text boxs at run time 
        /// </summary>
        private void InitTextBoxes()
        {
            try
            {
                this.listMasterStrategy.Controls.Add(txtRenameAdd);
                txtRenameAdd.Visible = false;
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
        /// show all masterStrategys in List box named  listMasterStrategy
        /// </summary>
        public void BindMasterStrategy()
        {
            try
            {
                listMasterStrategy.Items.Clear();
                List<String> getMasterStrategyNames = new List<string>();

                //Get all masterStrategy name From _masterStrategyCollection and add intl list then show in list box
                getMasterStrategyNames = StrategyMasterStrategyMappingManager.GetAllMasterStrategyName();
                for (int i = 0; i < getMasterStrategyNames.Count; i++)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = getMasterStrategyNames[i];
                    //item.Key = strategyName;
                    item.Value = getMasterStrategyNames[i];
                    listMasterStrategy.Items.Add(item);
                }
                // set index 0 at start level
                //if (listMasterStrategy.SelectedItems.Count >= 1)
                //{
                if (listMasterStrategy.Items.Count > 0)
                {
                    listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[0]);//[0];
                }

                //}
                //else
                //{ }
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
        /// Show all Un mapped strategys in ultra list view 
        /// </summary>
        public void BindUnmappedStrategies()
        {
            try
            {
                if (listMasterStrategy.Items.Count > 0)
                {
                    List<String> unMappedStrategyNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategies(listMasterStrategy.SelectedItems[0].Text);
                    if (uLstViewUnAssignedStrategies.Items.Count > 0)
                    {
                        uLstViewUnAssignedStrategies.Items.Clear();
                    }
                    foreach (String strategyName in unMappedStrategyNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = strategyName;
                        //item.Key = strategyName;
                        item.Value = strategyName;
                        uLstViewUnAssignedStrategies.Items.Add(item);
                    }
                }
                else
                {
                    List<String> unMappedStrategyNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategies(string.Empty);
                    if (uLstViewUnAssignedStrategies.Items.Count > 0)
                    {
                        uLstViewUnAssignedStrategies.Items.Clear();
                    }
                    foreach (String strategyName in unMappedStrategyNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = strategyName;
                        //item.Key = strategyName;
                        item.Value = strategyName;
                        uLstViewUnAssignedStrategies.Items.Add(item);
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
        /// Show mapped Strategys in ultra list view 
        /// </summary>
        /// <param name="strategyNames">List of mappedStrategy Name for A selected MasterStrategy Name </param>
        public void BindMappedStrategies(List<String> strategyNames)
        {
            try
            {
                if (strategyNames == null)
                    uLstViewAssignedStrategies.Items.Clear();
                else
                {
                    uLstViewAssignedStrategies.Items.Clear();
                    foreach (String strategyName in strategyNames)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = strategyName;
                        //item.Key = strategyName;
                        item.Value = strategyName;
                        uLstViewAssignedStrategies.Items.Add(item);
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
        /// add Strategy names in list to be unassigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to unassigned  of all to be un assigned</param>
        /// <returns>List of toBeUnAssignedStrategys</returns>
        private List<String> GetToBeUnAssignedStrategy(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeUnAssignedStrategies = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = uLstViewAssignedStrategies.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedStrategies.Add(uLstViewAssignedStrategies.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewAssignedStrategies.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeUnAssignedStrategies.Add(uLstViewAssignedStrategies.Items[i].Text);
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
            return toBeUnAssignedStrategies;
        }

        /// <summary>
        /// add Strategy names in list to be assigned 
        /// </summary>
        /// <param name="isOnlySelectedRequired">return true of false only selected Require to assigned  of all to be  assigned</param>
        /// <returns>List of toBeAssignedStrategys</returns>
        private List<string> GetToBeAssignedStrategies(bool isOnlySelectedRequired)
        {
            int count;
            List<string> toBeAssignedStrategies = new List<string>();
            try
            {
                if (isOnlySelectedRequired)
                {
                    count = uLstViewUnAssignedStrategies.SelectedItems.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedStrategies.Add(uLstViewUnAssignedStrategies.SelectedItems[i].Text);
                    }
                }
                else
                {
                    count = uLstViewUnAssignedStrategies.Items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        toBeAssignedStrategies.Add(uLstViewUnAssignedStrategies.Items[i].Text);
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
            return toBeAssignedStrategies;
        }

        /// <summary>
        /// Get the strategies from the grid control
        /// </summary>
        /// <returns>The collection of strategies</returns>
        public Strategies GetStrategies()
        {
            try
            {
                strategies = grdStrategyList.CurrentStrategies;
                return strategies;
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
        /// save all changes in data base and clean back up of changes
        /// </summary>
        /// <param name="sender">button save</param>
        /// <param name="e">e</param>
        public int uBtnSave_Click(int companyID)
        {
            int i = 0;
            try
            {
                if (StrategyMasterStrategyMappingManager.isBackUpOn || (StrategyMasterStrategyMappingManager.isBackUpMasterStrategyOn))
                {
                    StrategyMasterStrategyMappingManager.CleanBackUp();
                    StrategyMasterStrategyMappingManager.CleanBackUpMasterStrategy();

                    i = StrategyMasterStrategyMappingManager.SaveMapping(companyID);

                    // Audit for deletion
                    AuditMasterStrategyData(StrategyMasterStrategyMappingManager.GetStatusForAuditDel());
                    // Audit for saving and updation
                    AuditMasterStrategyData(StrategyMasterStrategyMappingManager.GetStatusForAudit());
                    if (i == -11)
                    {
                        MessageBox.Show("There is already a Master Strategy with the same name but in Inactive State", "Prana Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
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
            return i;
        }

        /// <summary>
        /// cancle all changes apply on Strategy or masterStrategy
        /// </summary>
        /// <param name="sender">button cancel</param>
        /// <param name="e">e</param>
        public bool uBtnCancel_Click()
        {
            bool r = false;
            try
            {
                // if Change is exist before savethen ask to revert the changes or not
                if ((StrategyMasterStrategyMappingManager.isBackUpOn) || (StrategyMasterStrategyMappingManager.isBackUpMasterStrategyOn))
                {
                    //if copyof dictionary _strategyMasterStrategyMapping is exist then give message of want revert or not if yes delete copy of dictionay _strategyMasterStrategyMapping()
                    // if no then action is required 
                    DialogResult result = MessageBox.Show("Do you want to save changes of Strategy?", "Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Cancel)
                        r = true;
                    if (result == DialogResult.Yes)
                    {
                        uBtnSave_Click(_companyID);
                    }
                    else if (result == DialogResult.No)
                    {
                        StrategyMasterStrategyMappingManager.RestoreBackUp();
                        StrategyMasterStrategyMappingManager.RestoreBackUpMasterStrategy();
                        BindMasterStrategy();
                        //BindUnmappedStrategies();
                        if (chkBoxManyToMany.Checked)
                        {
                            BindUnmappedStrategies();
                        }
                        else
                        {
                            BindUnMappedStrategiesForOneToMany();
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
            return r;
        }

        /// <summary>
        /// search items from typed key in text box and show itmes in lasterStrategy Box and set sellected index 0
        /// </summary>
        public void AddSearchedItemMasterStrategy()
        {
            try
            {
                List<String> searchingList = StrategyMasterStrategyMappingManager.GetAllMasterStrategyName();
                List<String> result = StrategyMasterStrategyMappingManager.SearchForKeyword(uTxtMasterStrategy.Text, searchingList);
                listMasterStrategy.Items.Clear();
                uLstViewAssignedStrategies.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem();
                        item.Tag = item.Key = foundItem;
                        item.Value = foundItem;
                        listMasterStrategy.Items.Add(item);
                    }
                    listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[0]);
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
        /// texbox  for contain search in unassigned strategy list
        /// </summary>
        /// <param name="sender">text box</param>
        /// <param name="e"></param>
        private void uTxtUnassignedStrategies_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtUnassignedStrategies.Text.ToUpper()) && uTxtUnassignedStrategies.Text.ToUpper() != "SEARCH")
                    AddSearchedItemUnassignedStrategy();
                else
                {
                    uLstViewUnAssignedStrategies.Items.Clear();
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
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
        /// show only contain searched items in list
        /// </summary>
        public void AddSearchedItemUnassignedStrategy()
        {
            List<String> searchingList = new List<string>();
            try
            {
                if (listMasterStrategy.SelectedItems.Count > 0 && !String.IsNullOrEmpty(listMasterStrategy.SelectedItems[0].Text.ToString()))
                {
                    searchingList = StrategyMasterStrategyMappingManager.GetUnmappedStrategies(listMasterStrategy.SelectedItems[0].Text);//new List<string>();
                }
                else
                {
                    searchingList = uLstViewUnAssignedStrategies.Items.Cast<UltraListViewItem>().Select(x => x.Text).ToList();
                }
                List<String> result = StrategyMasterStrategyMappingManager.SearchForKeyword(uTxtUnassignedStrategies.Text, searchingList);
                uLstViewUnAssignedStrategies.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        uLstViewUnAssignedStrategies.Items.Add(item);
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
        /// Show only Searched item in Assigned list view for a selected masterStrategy name
        /// </summary>
        public void AddSearchedItemAssignedStrategy()
        {
            string masterStrategyName;
            try
            {
                if (listMasterStrategy.SelectedItems.Count >= 1)
                {
                    masterStrategyName = listMasterStrategy.SelectedItems[0].Text;
                }
                else
                {
                    masterStrategyName = null;
                }
                List<String> searchingList = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                List<String> result = StrategyMasterStrategyMappingManager.SearchForKeyword(uTxtAssignedStrategies.Text, searchingList);
                uLstViewAssignedStrategies.Items.Clear();
                if (result.Count > 0)
                {
                    foreach (String foundItem in result)
                    {
                        UltraListViewItem item = new UltraListViewItem(foundItem);
                        item.Key = foundItem;
                        item.Tag = foundItem;
                        item.Value = foundItem;
                        uLstViewAssignedStrategies.Items.Add(item);
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
        /// contain search in Unassigned strategys list and show searched items in Unassigned list view
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">e</param>
        private void uTxtUnassignedStrategies_Click(object sender, EventArgs e)
        {
            try
            {
                if (uTxtUnassignedStrategies.Text.Trim().ToLower() == "search")
                {
                    uTxtUnassignedStrategies.SelectAll();
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
        /// event enable conterxt menu depending upon selection if multiple selected then only  delete  enable if 1 selected then  then add rename and delete enabled 
        /// </summary>
        /// <param name="sender">list view </param>
        /// <param name="e">e</param>
        private void listMasterStrategy_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    int indexSeleted = 0;
                    if (listMasterStrategy.Items.Count > 0)
                        indexSeleted = e.Y / listMasterStrategy.Items[0].UIElement.Rect.Height;
                    if (indexSeleted < listMasterStrategy.Items.Count)
                        if (!listMasterStrategy.Items[indexSeleted].IsSelected)
                        {
                            listMasterStrategy.SelectedItems.Clear();
                            listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[indexSeleted]);
                        }


                    if (listMasterStrategy.SelectedItems.Count > 1)
                    {
                        addMasterStrategyToolStripMenuItem.Enabled = false;
                        renameMasterStrategyToolStripMenuItem.Enabled = false;
                        deleteMasterStrategyToolStripMenuItem.Enabled = true;
                    }
                    else if (listMasterStrategy.SelectedItems.Count == 1)
                    {

                        addMasterStrategyToolStripMenuItem.Enabled = true;
                        renameMasterStrategyToolStripMenuItem.Enabled = true;
                        deleteMasterStrategyToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        addMasterStrategyToolStripMenuItem.Enabled = true;
                        renameMasterStrategyToolStripMenuItem.Enabled = false;
                        deleteMasterStrategyToolStripMenuItem.Enabled = false;
                    }
                }
                else if (e.Button == MouseButtons.Left && (!_isItemExistedEditMode))
                {
                    int indexSeleted = 0;
                    if (listMasterStrategy.Items.Count > 0)
                    {
                        indexSeleted = e.Y / listMasterStrategy.Items[0].UIElement.Rect.Height;
                        if (indexSeleted > listMasterStrategy.Items.Count)
                            listMasterStrategy.SelectedItems.Clear();
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
        /// To be unassign selected  Strategys on button click and show selected strategys in unmapped list view
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void UBtnUnSelectAssignedStrategies_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listMasterStrategy.SelectedItems[0].Index;
                if (index >= 0)
                {
                    string masterStrategyName = listMasterStrategy.Items[index].Text;
                    List<string> unSelectStrategies = GetToBeUnAssignedStrategy(true);
                    StrategyMasterStrategyMappingManager.UnassignStrategies(masterStrategyName, unSelectStrategies);
                    List<String> strategyNames = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                    uLstViewAssignedStrategies.Items.Clear();
                    BindMappedStrategies(strategyNames);
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
                    }

                    listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[index]);
                    //Contain search to assigned key in UnassignedStrategys list view
                    uTxtUnassignedStrategies_TextChanged(uTxtUnassignedStrategies, null);
                    //Contain search to assigned key in Assigned Strategys List view
                    uTxtAssignedStrategies_TextChanged(uTxtAssignedStrategies, null);
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
        /// To be unassign all  Strategys from assigned list view on button click and show all strategys in unmapped list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectAssignedStrategies_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listMasterStrategy.SelectedItems[0].Index;// FocusedItem.Index;
                if (listMasterStrategy.SelectedItems.Count >= 0)
                {
                    string masterStrategyName = listMasterStrategy.SelectedItems[0].Text;
                    List<string> unSelectStrategies = GetToBeUnAssignedStrategy(false);
                    StrategyMasterStrategyMappingManager.UnassignStrategies(masterStrategyName, unSelectStrategies);
                    List<String> strategyNames = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                    uLstViewAssignedStrategies.Items.Clear();
                    BindMappedStrategies(strategyNames);
                    BindUnmappedStrategies();
                    //Contain search to assigned key in UnassignedStrategys list view
                    uTxtUnassignedStrategies_TextChanged(uTxtUnassignedStrategies, null);
                    //Contain search to assigned key in Assigned Strategys List view
                    uTxtAssignedStrategies_TextChanged(uTxtAssignedStrategies, null);
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
        /// Slelected unassigned strategys assign in selected master Strategy
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">null</param>
        private void uBtnSelectUnassignedStrategies_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listMasterStrategy.SelectedItems[0].Index;
                if (index >= 0)
                {
                    string masterStrategyName = listMasterStrategy.Items[index].Text;
                    List<string> assignedStrategies = GetToBeAssignedStrategies(true);
                    StrategyMasterStrategyMappingManager.AssignStrategies(masterStrategyName, assignedStrategies);
                    List<String> StrategyNames = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                    uLstViewUnAssignedStrategies.Items.Clear();
                    BindMappedStrategies(StrategyNames);
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
                    }
                    listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[index]);
                    //Contain search to assigned key in UnassignedStrategys list view
                    uTxtUnassignedStrategies_TextChanged(uTxtUnassignedStrategies, null);
                    //Contain search to assigned key in Assigned Strategys List view
                    uTxtAssignedStrategies_TextChanged(uTxtAssignedStrategies, null);
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
        /// all assigned strategy assign in unassinged strategies
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e"></param>
        private void uBtnAllUnSelectUnassignedStrategies_Click(object sender, EventArgs e)
        {
            try
            {
                int index = listMasterStrategy.SelectedItems[0].Index;
                if (index >= 0)
                {
                    string masterStrategyName = listMasterStrategy.Items[index].Text;
                    List<string> assignedStrategies = GetToBeAssignedStrategies(false);
                    StrategyMasterStrategyMappingManager.AssignStrategies(masterStrategyName, assignedStrategies);
                    List<String> StrategyNames = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                    uLstViewUnAssignedStrategies.Items.Clear();
                    BindMappedStrategies(StrategyNames);
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
                    }
                    //Contain search to assigned key in UnassignedStrategies list view
                    uTxtUnassignedStrategies_TextChanged(uTxtUnassignedStrategies, null);
                    //Contain search to assigned key in Assigned Strategies List view
                    uTxtAssignedStrategies_TextChanged(uTxtAssignedStrategies, null);
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
        /// bind associated strategy on Seleted master Master Strategy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listMasterStrategy_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count == 1)
                {
                    BindMappedStrategies(StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(e.SelectedItems[0].Text));
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();

                    // For audit trail
                    RefreshMasterStrategyAuditFor(StrategyMasterStrategyMappingManager.GetMasterStrategyIdByName(e.SelectedItems[0].Text));

                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
                    }
                }
                else
                {
                    BindMappedStrategies(null);
                }
                if (listMasterStrategy.SelectedItems.Count == 1)
                {
                    uBtnAllUnSelectAssignedStrategies.Enabled = true;
                    uBtnAllSelectUnassignedStrategies.Enabled = true;
                    uBtnSelectUnassignedStrategies.Enabled = true;
                    UBtnUnSelectAssignedStrategies.Enabled = true;
                }
                else
                {
                    uBtnAllUnSelectAssignedStrategies.Enabled = false;
                    uBtnAllSelectUnassignedStrategies.Enabled = false;
                    uBtnSelectUnassignedStrategies.Enabled = false;
                    UBtnUnSelectAssignedStrategies.Enabled = false;
                    uLstViewUnAssignedStrategies.Items.Clear();
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
        /// Get the master fund into edit mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listMasterStrategy_ItemActivated(object sender, ItemActivatedEventArgs e)
        {
            try
            {
                listMasterStrategy.ItemSettings.AllowEdit = DefaultableBoolean.False;
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
        /// allow edit on double click
        /// </summary>
        /// <param name="sender">ultra list view</param>
        /// <param name="e">e</param>
        private void listMasterStrategy_ItemDoubleClick(object sender, ItemDoubleClickEventArgs e)
        {
            try
            {
                listMasterStrategy.ItemSettings.AllowEdit = DefaultableBoolean.True;
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
        /// get old name of master strategy before changed 
        /// </summary>
        /// <param name="sender">ultralist view</param>
        /// <param name="e"></param>
        private void listMasterStrategy_ItemEnteringEditMode(object sender, ItemEnteringEditModeEventArgs e)
        {
            try
            {
                _oldMasterStrategyName = e.Item.Text;
                //Modified by: sachin mishra solution of JIRA no - CHMW-2283 date-19/jan/2015
                e.Item.SelectedAppearance.ForeColor = System.Drawing.Color.Black;
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
        /// add or rename or delete from dictionary and list view when edit mode disable
        /// </summary>
        /// <param name="sender">listMasterStrategy</param>
        /// <param name="e">e</param>
        private void listMasterStrategy_ItemExitedEditMode(object sender, ItemExitedEditModeEventArgs e)
        {
            _isItemExistedEditMode = true;
            bool isExist = false;
            string newName = e.Item.Text;
            try
            {
                Regex r = new Regex("^[a-zA-Z0-9_]+$");
                if (!r.IsMatch(e.Item.Text))
                {
                    BindMasterStrategy();
                    //modified by: Bharat raturi, 23 apr 2014
                    //purpose: Bind unmapped strategies according to the chosen mapping style
                    //bind Unmapped Strategy to control
                    //BindUnmappedStrategies();
                    if (chkBoxManyToMany.Checked)
                    {
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        BindUnMappedStrategiesForOneToMany();
                    }
                }
                else
                {
                    if (e.Item.Text != _oldMasterStrategyName && !String.IsNullOrEmpty(e.Item.Text))
                    {
                        isExist = StrategyMasterStrategyMappingManager.IsMasterStrategyNameExist(e.Item.Text);
                        if (isExist)
                        {
                            MessageBox.Show("Master Strategy Already Exists!", "Master Strategy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            listMasterStrategy.SelectedItems.Clear();
                            listMasterStrategy.Items[_oldMasterStrategyName].Value = _oldMasterStrategyName;
                            listMasterStrategy.SelectedItems.Clear();
                            listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[_oldMasterStrategyName]);
                        }
                        else
                        {
                            listMasterStrategy.Items[_oldMasterStrategyName].Key = e.Item.Text;
                            listMasterStrategy.Items[e.Item.Text].Tag = e.Item.Text;
                            listMasterStrategy.Items[e.Item.Text].Value = e.Item.Text;
                            StrategyMasterStrategyMappingManager.ManageMasterStrategy(1, e.Item.Text, _oldMasterStrategyName);
                        }

                    }
                    else if (String.IsNullOrEmpty(e.Item.Text))
                    {
                        BindMasterStrategy();
                        BindUnmappedStrategies();
                    }
                    else
                    {
                        listMasterStrategy.SelectedItems.Clear();
                        listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[listMasterStrategy.Items.Count - 1]);
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
        /// contain search in masterStrategy list  to a typed key
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">e</param>
        private void uTxtMasterStrategy_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(uTxtMasterStrategy.Text.ToUpper()) && uTxtMasterStrategy.Text.ToUpper() != "SEARCH")
                    AddSearchedItemMasterStrategy();
                else
                {
                    listMasterStrategy.Items.Clear();
                    BindMasterStrategy();
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
        /// contain search in assigned strategys list and show searched items in assigned list view
        /// </summary>
        /// <param name="sender">textbox</param>
        /// <param name="e">e</param>
        private void uTxtAssignedStrategies_TextChanged(object sender, EventArgs e)
        {
            string masterStrategyName;
            try
            {
                if (!String.IsNullOrEmpty(uTxtAssignedStrategies.Text.ToUpper()) && uTxtAssignedStrategies.Text.ToUpper() != "SEARCH")
                    AddSearchedItemAssignedStrategy();
                else
                {
                    uLstViewAssignedStrategies.Items.Clear();
                    if (listMasterStrategy.SelectedItems.Count >= 1)
                    {
                        masterStrategyName = listMasterStrategy.SelectedItems[0].Text;
                    }
                    else
                    {
                        masterStrategyName = null;
                    }
                    List<String> StrategyNames = StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(masterStrategyName);
                    if (StrategyNames != null)
                    {
                        foreach (String foundItem in StrategyNames)
                        {
                            UltraListViewItem item = new UltraListViewItem();
                            item.Key = foundItem;
                            item.Tag = foundItem;
                            item.Value = foundItem;
                            uLstViewAssignedStrategies.Items.Add(item);
                        }
                    }
                    else
                    {
                        uLstViewAssignedStrategies.Items.Clear();
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
        /// add new master Strategy on click on add new master Strategy tool strip menu item 
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">e</param>
        private void addMasterStrategyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String runtimeMasterStrategyName = StrategyMasterStrategyMappingManager.GetRuntimeMasterStrategyName();
                UltraListViewItem item = new UltraListViewItem(runtimeMasterStrategyName);

                item.Tag = item.Key = runtimeMasterStrategyName;
                item.Value = runtimeMasterStrategyName;
                StrategyMasterStrategyMappingManager.ManageMasterStrategy(0, runtimeMasterStrategyName);
                listMasterStrategy.Items.Add(item);
                listMasterStrategy.SelectedItems.Clear();
                listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[listMasterStrategy.Items.Count - 1]);
                listMasterStrategy.Items[runtimeMasterStrategyName].BeginEdit();

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
        /// rename master Strategy name on click on rename master Strategy tool strip menu item 
        /// </summary>
        /// <param name="sender">rename menu item</param>
        /// <param name="e">e</param>
        private void renameMasterStrategyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (listMasterStrategy.SelectedItems.Count == 1)
                {
                    listMasterStrategy.ItemSettings.AllowEdit = DefaultableBoolean.True;
                    listMasterStrategy.SelectedItems[0].BeginEdit();
                    _isMasterStrategyId = 1;
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
        /// delete Seleted master Strategy on click on delete master Strategy tool strip master Strategy
        /// </summary>
        /// <param name="sender">delete menu item</param>
        /// <param name="e">e</param>
        private void deleteMasterStrategyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //int idx = 0;
                //modified by: Bharat Raturi, 05 may 2014
                //purpose: prevent the master strategy from getting deleted if it has assoicated strategies
                //Boolean doNotAskAgain = false;
                for (int i = 0; i <= listMasterStrategy.SelectedItems.Count - 1; i++)
                {
                    string name = listMasterStrategy.SelectedItems[i].Text;
                    //if (!doNotAskAgain && StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(name).Count > 0)
                    if (StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(name).Count > 0)
                    {
                        //doNotAskAgain = true;

                        //DialogResult dr = MessageBox.Show("Master Strategy " + name + " has some strategies associated with it.\n Do you want to delete this master strategy?", "Warning", MessageBoxButtons.YesNo);
                        MessageBox.Show("Master Strategy " + name + " has some strategies associated with it.\n It cannot be deleted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        //if (dr == DialogResult.No)
                        //{
                        //    break;
                        //}
                    }
                    _isMasterStrategyId = 2;
                    StrategyMasterStrategyMappingManager.ManageMasterStrategy(_isMasterStrategyId, null, name);
                }
                listMasterStrategy.Items.Clear();
                BindMasterStrategy();
                //modified by: Bharat raturi, 23 apr 2014
                //purpose: Bind unmapped strategies according to the chosen mapping style
                //bind Unmapped Strategy to control
                //BindUnmappedStrategies();
                if (chkBoxManyToMany.Checked)
                {
                    BindUnmappedStrategies();
                }
                else
                {
                    BindUnMappedStrategiesForOneToMany();
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
        /// Load the new added strategy to the list of strategies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdStrategyList_Leave(object sender, EventArgs e)
        {
            try
            {
                int strategyID = -1;
                Strategies strategyList = grdStrategyList.CurrentStrategies;
                StrategyMasterStrategyMappingManager._strategyCollection.Clear();
                foreach (Strategy strategy in strategyList)
                {
                    if (StrategyMasterStrategyMappingManager._strategyCollection.ContainsKey(strategy.StrategyID) && strategy.StrategyID == -1)
                    {
                        strategy.StrategyID = --strategyID;
                    }
                    StrategyMasterStrategyMappingManager._strategyCollection.Add(strategy.StrategyID, strategy.StrategyName);
                }
                if (listMasterStrategy.SelectedItems.Count > 0)
                {
                    String m_StrName = listMasterStrategy.SelectedItems[0].Text;
                    BindMappedStrategies(StrategyMasterStrategyMappingManager.GetStrategyNamesForMasterStrategy(m_StrName));
                }
                //modified by: Bharat raturi, 23 apr 2014
                //purpose: Bind unmapped strategies according to the chosen mapping style
                //bind Unmapped Strategy to control
                //BindUnmappedStrategies();
                if (chkBoxManyToMany.Checked)
                {
                    BindUnmappedStrategies();
                }
                else
                {
                    BindUnMappedStrategiesForOneToMany();
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
        /// Show all Un mapped accounts in ultra list view 
        /// </summary>
        public void BindUnMappedStrategiesForOneToMany()
        {
            try
            {
                List<String> unMappedAccountNames = new List<string>();
                if (listMasterStrategy.SelectedItems.Count > 0)
                {
                    if (chkBoxManyToMany.Checked)
                    {
                        unMappedAccountNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategies(listMasterStrategy.SelectedItems[0].Text);
                    }
                    else
                    {
                        unMappedAccountNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategiesForOnetoMany();
                    }
                }
                else
                {
                    if (chkBoxManyToMany.Checked)
                    {
                        unMappedAccountNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategies(string.Empty);
                    }
                    else
                    {
                        unMappedAccountNames = StrategyMasterStrategyMappingManager.GetUnmappedStrategiesForOnetoMany();
                    }
                }
                uLstViewUnAssignedStrategies.Items.Clear();
                foreach (String accountName in unMappedAccountNames)
                {
                    UltraListViewItem item = new UltraListViewItem();
                    item.Tag = item.Key = accountName;
                    //item.Key = accountName;
                    item.Value = accountName;
                    if (!uLstViewAssignedStrategies.Items.Contains(item))
                    {
                        uLstViewUnAssignedStrategies.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                //Invoke our policy that is responsible for making sure no secure information
                //gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        //added by: Bharat raturi, 23 apr 2014
        //purpose: activate many to many or one to many mapping after validating the current mapping
        //bind Unmapped Strategy to control
        /// <summary>
        /// On Check change Check what mapping style should be activated
        /// along with this check if one to many mapping can be activated if the check box was unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBoxManyToMany_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBoxManyToMany.Checked)
            {
                _isManyToManyMapping = true;
            }
            else
            {
                _isManyToManyMapping = false;
            }
            if (!chkBoxManyToMany.Checked && StrategyMasterStrategyMappingManager.IsManyToManyMapping())
            {
                MessageBox.Show("Some Strategies are associated with more than one master strategies.\nRemove that mapping in order to proceed.", "Master Strategy Mapping Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _isManyToManyMapping = true;
                chkBoxManyToMany.Checked = true;
            }
            else if (!chkBoxManyToMany.Checked && !StrategyMasterStrategyMappingManager.IsManyToManyMapping())
            {
                BindUnMappedStrategiesForOneToMany();
                _isManyToManyMapping = false;
                return;
            }
            BindUnmappedStrategies();
        }

        /// <summary>
        /// returns the value of variable _isManyToManyMapping
        /// </summary>
        /// <returns>Return true if the many to many mapping is allowed</returns>
        public bool GetIsManyToManymapping()
        {
            return _isManyToManyMapping;
        }

        /// <summary>
        /// Function to get Master stragey ID for audit
        /// </summary>
        /// <param name="SelectedMasterStrategyID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void RefreshMasterStrategyAuditFor(int SelectedMasterStrategyID) { }

        /// <summary>
        /// Function to get Master stragey ID for audit
        /// </summary>
        /// <param name="SelectedMasterStrategyID"></param>
        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void RefreshStrategyAuditFor(int SelectedStrategyID) { }

        /// <summary>
        /// Function to Get dictionary for details of Master Strategy
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> GetMasterStrategyAuditData(int _companyID, int masterStrategyID)
        {
            Dictionary<String, List<String>> auditDataForMasterStrategy = new Dictionary<string, List<string>>();
            try
            {
                auditDataForMasterStrategy.Add(CustomAuditSourceConstants.AuditSourceTypeMasterStrategy, new List<string>());
                auditDataForMasterStrategy[CustomAuditSourceConstants.AuditSourceTypeMasterStrategy].Add(_companyID.ToString());
                auditDataForMasterStrategy[CustomAuditSourceConstants.AuditSourceTypeMasterStrategy].Add(masterStrategyID.ToString());
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
            return auditDataForMasterStrategy;
        }

        /// <summary>
        /// Function to trace audit details of Master Strategy
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private void AuditMasterStrategyData(Dictionary<int, int> dictAuditStatus)
        {
            try
            {
                if (dictAuditStatus != null)
                {
                    foreach (KeyValuePair<int, int> status in dictAuditStatus)
                    {
                        if (status.Value == 1)
                        {
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, GetMasterStrategyAuditData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterStrategyCreated);
                        }
                        else if (status.Value == 2)
                        {
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, GetMasterStrategyAuditData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterStrategyDeleted);
                        }
                        else if (status.Value == 3)
                        {
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, GetMasterStrategyAuditData(_companyID, status.Key), AuditManager.Definitions.Enum.AuditAction.MasterStrategyUpdated);
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
        /// To save audit for strategy deletion
        /// </summary>
        /// <param name="strategyID"></param>
        public void AuditStrategyDeletion(int strategyID)
        {
            try
            {
                AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, GetStrategyDeletionAuditData(_companyID, strategyID), AuditManager.Definitions.Enum.AuditAction.StrategyDeleted);
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
        /// To get dictionary for strategy deletion audit
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> GetStrategyDeletionAuditData(int companyID, int strategyID)
        {
            Dictionary<String, List<String>> auditDataForStrategyDeletion = new Dictionary<string, List<string>>();
            try
            {
                auditDataForStrategyDeletion.Add(CustomAuditSourceConstants.AuditSourceTypeStrategy, new List<string>());
                auditDataForStrategyDeletion[CustomAuditSourceConstants.AuditSourceTypeStrategy].Add(companyID.ToString());
                auditDataForStrategyDeletion[CustomAuditSourceConstants.AuditSourceTypeStrategy].Add(strategyID.ToString());
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
            return auditDataForStrategyDeletion;
        }

        /// <summary>
        /// To unselect UnAssigned Master Strategies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uLstViewAssignedStrategies_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    uLstViewUnAssignedStrategies.SelectedItems.Clear();
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
        /// To unselect Assigned Master Strategies
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uLstViewUnAssignedStrategies_ItemSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItems.Count > 0)
                {
                    uLstViewAssignedStrategies.SelectedItems.Clear();
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

        string _selectedItemInListMasterStrategy = string.Empty;

        /// <summary>
        /// Added by Ankit Gupta on 07 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1548
        ///  After saving Master strategy focus should be on strategy which is currently saved.
        ///  This method sets the value of string '_selectedItemInListMasterStrategy' equal to the key of the item that is selected in listMasterStrategy,
        ///  while adding or renaming.
        /// </summary>
        public void GetSelectedItemFromList()
        {
            try
            {
                if (listMasterStrategy.Items.Count > 0)
                {
                    _selectedItemInListMasterStrategy = listMasterStrategy.SelectedItems.First.Key;
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

        /// <summary>
        /// Added by Ankit Gupta on 07 Oct, 2014.
        /// http://jira.nirvanasolutions.com:8080/browse/CHMW-1548
        /// After saving Master strategy focus should be on strategy which is currently saved.
        /// This method retains the original selection in listMasterStrategy, with the help of '_selectedItemInListMasterStrategy'.
        /// </summary>
        public void SetSelectedItemInList()
        {
            try
            {
                if (listMasterStrategy.Items.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(_selectedItemInListMasterStrategy) && listMasterStrategy.Items.Exists(_selectedItemInListMasterStrategy))
                    {
                        listMasterStrategy.SelectedItems.Clear();
                        listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[_selectedItemInListMasterStrategy]);
                        _selectedItemInListMasterStrategy = string.Empty;
                    }
                    else
                    {
                        listMasterStrategy.SelectedItems.Clear();
                        listMasterStrategy.SelectedItems.Add(listMasterStrategy.Items[0]);
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
    }
}
