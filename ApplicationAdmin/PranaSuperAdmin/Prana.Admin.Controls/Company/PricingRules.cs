using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.AuditManager.Definitions.Constants;
using Prana.AuditManager.Definitions.Interface;
using Prana.LogManager;
using Prana.Utilities.UI;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Prana.Admin.Controls.Company
{
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.PricingRuleCreated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.PricingRuleUpdated, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.PricingRuleApproved, ShowAuditUI = true)]
    [AuditManager.Attributes.Audit(AuditManager.Definitions.Enum.AuditAction.PricingRuleDeleted, ShowAuditUI = true)]

    public partial class PricingRules : UserControl, IAuditSource
    {
        /// <summary>
        /// Constructor for control initialization
        /// </summary>
        [AuditManager.Attributes.AuditSourceConstAttri]
        public PricingRules()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Applying Black Gray Theme
        /// </summary>
        public void ApplyTheme()
        {
            try
            {
                this.ugPriceRules.DisplayLayout.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(46)))), ((int)(((byte)(49)))));
                this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
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

        //ValueList _vlAccounts = new ValueList();
        //ValueList _vlAsset=new ValueList();
        //ValueList _vlExchange = new ValueList();
        public int _companyID = 0;
        public bool _isSaveRequired = false;
        public bool _isValidData = false;
        ValueList _vlPricingDataType = new ValueList();
        ValueList _vlSource = new ValueList();
        ValueList _vlSecondarySource = new ValueList();
        ValueList _vlRuleType = new ValueList();
        ValueList _vlRuleTypeByTime = new ValueList();
        ValueList _vlPricingPolicies = new ValueList();
        List<int> _deletedPricingRules = new List<int>();
        ConcurrentDictionary<int, int> dictActionStatus = new ConcurrentDictionary<int, int>();

        /// <summary>
        /// Initialize the value lists for the grid cells
        /// </summary>
        public void InitializeValueLists()
        {
            try
            {
                Dictionary<int, string> dicPricingDatatype = PricingRuleManager.GetPricingDataTypes();
                Dictionary<int, string> dicSource = PricingRuleManager.GetSources();
                Dictionary<int, string> dicSecondarySource = PricingRuleManager.GetSecondarySource();
                Dictionary<int, string> dicPricingPolicies = PricingRuleManager.GetPricingPolicyList();

                _vlPricingDataType.ValueListItems.Clear();
                _vlSecondarySource.ValueListItems.Clear();
                _vlSource.ValueListItems.Clear();
                _vlPricingPolicies.ValueListItems.Clear();

                _vlPricingDataType.ValueListItems.Add(-1, "-Select-");
                _vlSource.ValueListItems.Add(-1, "-Select-");
                _vlSecondarySource.ValueListItems.Add(-1, "-Select-");
                _vlPricingPolicies.ValueListItems.Add(-1, "-Select-");


                foreach (int datatypeID in dicPricingDatatype.Keys)
                {
                    _vlPricingDataType.ValueListItems.Add(datatypeID, dicPricingDatatype[datatypeID]);
                }
                foreach (int sourceID in dicSource.Keys)
                {
                    _vlSource.ValueListItems.Add(sourceID, dicSource[sourceID]);
                }
                foreach (int secSourceID in dicSecondarySource.Keys)
                {
                    _vlSecondarySource.ValueListItems.Add(secSourceID, dicSecondarySource[secSourceID]);
                }
                foreach (int pricingPolicyID in dicPricingPolicies.Keys)
                {
                    _vlPricingPolicies.ValueListItems.Add(pricingPolicyID, dicPricingPolicies[pricingPolicyID]);
                }

                // Value list for Pricing rule Type
                _vlRuleType.ValueListItems.Clear();
                _vlRuleType.ValueListItems.Add(-1, "-Select-");
                _vlRuleType.ValueListItems.Add(1, "Mark Price");
                _vlRuleType.ValueListItems.Add(2, "SM Batch");


                // Value list for Pricing rule Type based on time 
                _vlRuleTypeByTime.ValueListItems.Clear();
                _vlRuleTypeByTime.ValueListItems.Add(-1, "-Select-");
                // _vlRuleTypeByTime.ValueListItems.Add(2, "Current");
                _vlRuleTypeByTime.ValueListItems.Add(1, "Historical");
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
        /// Initialize the control data
        /// </summary>
        public void InitializeData(int companyID)
        {
            try
            {
                this._companyID = companyID;
                PricingRuleManager._companyID = _companyID;
                ucbAccount.DataSource = PricingRuleManager.GetAccounts();
                ucbAccount.DisplayLayout.Bands[0].Columns["FundID"].Hidden = true;
                ucbAsset.DataSource = PricingRuleManager.GetAssets();
                ucbAsset.DisplayLayout.Bands[0].Columns["AssetID"].Hidden = true;
                //ucbExchange.DataSource = PricingRuleManager.GetExchanges();
                PricingRuleManager.GetAssetExchanges();
                //ucbExchange.DisplayLayout.Bands[0].Columns["ExchangeID"].Hidden = true;
                InitializeValueLists();
                ugPriceRules.DataSource = PricingRuleManager.GetPricingDetails();
                SetExchangeEditorControl();


                //Modified by omshiv, Added creation filter for modify grid cell UI component
                //http://www.infragistics.com/community/forums/p/89599/442509.aspx#442509
                List<String> columnList = new List<string>() { "AssetClass", "Exchange", "Account" };
                ugPriceRules.CreationFilter = new CustomCreationFilter(columnList);

                CtrlPricingPolicy.PricingPolicyEvent += CtrlPricingPolicy_PricingPolicyEvent;

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
        public delegate void InvokeDelegate(object sender, EventArgs e);

        //EventHandler UIMarshler;
        void CtrlPricingPolicy_PricingPolicyEvent(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    InvokeDelegate invokeDelegate = new InvokeDelegate(CtrlPricingPolicy_PricingPolicyEvent);
                    this.Invoke(invokeDelegate, new object[] { sender, e });
                    return;
                }
                else
                {
                    Dictionary<int, string> dicPricingPolicies = PricingRuleManager.GetPricingPolicyList();
                    _vlPricingPolicies.ValueListItems.Clear();
                    _vlPricingPolicies.ValueListItems.Add(-1, "-Select-");
                    foreach (int pricingPolicyID in dicPricingPolicies.Keys)
                    {
                        _vlPricingPolicies.ValueListItems.Add(pricingPolicyID, dicPricingPolicies[pricingPolicyID]);
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
        /// Initialize the layout of the Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugPriceRules_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                UltraGridBand band = e.Layout.Bands[0];
                band.Override.AllowRowFiltering = DefaultableBoolean.True;
                if (!band.Columns.Exists("DeleteButton"))
                {
                    UltraGridColumn colDelete = band.Columns.Add("DeleteButton");
                    colDelete.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
                    colDelete.Width = 20;
                    colDelete.Header.Caption = "";
                    colDelete.Header.VisiblePosition = 0;
                    colDelete.ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                }

                if (ucbAccount.DataSource != null)
                {
                    if (!ucbAccount.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn cbAccount = ucbAccount.DisplayLayout.Bands[0].Columns.Add();
                        cbAccount.Key = "Selected";
                        cbAccount.Header.Caption = string.Empty;
                        cbAccount.Width = 25;
                        cbAccount.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        cbAccount.DataType = typeof(bool);
                        cbAccount.Header.VisiblePosition = 1;
                    }
                    ucbAccount.CheckedListSettings.CheckStateMember = "Selected";
                    ucbAccount.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    ucbAccount.CheckedListSettings.ListSeparator = " , ";
                    ucbAccount.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    ucbAccount.DisplayMember = "AccountName";
                    ucbAccount.ValueMember = "AccountID";
                    ucbAccount.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                }
                //ucbAccount.Hide();


                if (!band.Columns.Exists("IsPricingPolicy"))
                {
                    UltraGridColumn isPricingPolicy = band.Columns.Add("IsPricingPolicy");
                    isPricingPolicy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
                    isPricingPolicy.Width = 80;
                    isPricingPolicy.Header.Caption = "Is Pricing Policy";
                    isPricingPolicy.Header.VisiblePosition = 3;

                }

                UltraGridColumn colPricingPolicy = band.Columns["PricingPolicy"];
                colPricingPolicy.Header.Caption = "Pricing Policy";
                colPricingPolicy.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colPricingPolicy.NullText = "-Select-";
                colPricingPolicy.ValueList = _vlPricingPolicies;


                UltraGridColumn colAccount = band.Columns["Account"];
                colAccount.Header.Caption = "Account";
                ucbAccount.NullText = "-Select-";
                colAccount.EditorComponent = ucbAccount;
                //colAccount.ValueList = _vlAccounts;

                if (!ucbAsset.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                {
                    UltraGridColumn cbAsset = ucbAsset.DisplayLayout.Bands[0].Columns.Add();
                    cbAsset.Key = "Selected";
                    cbAsset.Header.Caption = string.Empty;
                    cbAsset.Width = 25;
                    cbAsset.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                    cbAsset.DataType = typeof(bool);
                    cbAsset.Header.VisiblePosition = 1;
                }
                ucbAsset.CheckedListSettings.CheckStateMember = "Selected";
                ucbAsset.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                ucbAsset.CheckedListSettings.ListSeparator = " , ";
                ucbAsset.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                ucbAsset.DisplayMember = "AssetName";
                ucbAsset.ValueMember = "AssetID";
                ucbAsset.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                //ucbAsset.Hide();

                UltraGridColumn colAsset = band.Columns["AssetClass"];
                colAsset.Header.Caption = "Asset Class";
                ucbAsset.NullText = "-Select-";
                colAsset.EditorComponent = ucbAsset;


                UltraGridColumn colExchange = band.Columns["Exchange"];
                colExchange.Header.Caption = "Exchange";
                ucbExchange.NullText = "-Select-";
                colExchange.EditorComponent = ucbExchange;

                UltraGridColumn colPricing = band.Columns["Pricing"];
                colPricing.Header.Caption = "Pricing";
                colPricing.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colPricing.ValueList = _vlPricingDataType;

                UltraGridColumn colSource = band.Columns["Source"];
                colSource.Header.Caption = "Source";
                colSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSource.ValueList = _vlSource;

                UltraGridColumn colSecSource = band.Columns["SecondarySource"];
                colSecSource.Header.Caption = "Secondary Source";
                colSecSource.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colSecSource.ValueList = _vlSecondarySource;


                UltraGridColumn colRuleType = band.Columns["RuleType"];
                colRuleType.Header.Caption = "Rule Type";
                colRuleType.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colRuleType.NullText = "-Select-";
                colRuleType.ValueList = _vlRuleType;


                UltraGridColumn colRuleTypeByTime = band.Columns["RuleTypeByTime"];
                colRuleTypeByTime.Header.Caption = "Pricing Time";
                colRuleTypeByTime.NullText = "-Select-";
                colRuleTypeByTime.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                colRuleTypeByTime.ValueList = _vlRuleTypeByTime;

                ugPriceRules.DisplayLayout.Bands[0].Columns["RuleID"].Hidden = true;

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
        /// Handle the click of the delete button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugPriceRules_ClickCellButton(object sender, CellEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "DeleteButton")
                {
                    if (!string.IsNullOrEmpty(row.Cells["Account"].Value.ToString()) && !string.IsNullOrEmpty(row.Cells["AssetClass"].Value.ToString()) && !string.IsNullOrEmpty(row.Cells["Exchange"].Value.ToString()))
                    {
                        DialogResult dr = MessageBox.Show("Do you want to delete the selected price rule", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }

                        int pricingRuleID;
                        bool isParsed = int.TryParse(row.Cells["RuleID"].Value.ToString(), out pricingRuleID);
                        if (isParsed && !_deletedPricingRules.Contains(pricingRuleID))
                        {
                            _deletedPricingRules.Add(pricingRuleID);
                            dictActionStatus.TryAdd(pricingRuleID, 2);
                        }
                        _isSaveRequired = true;

                    }
                    e.Cell.Row.Delete(false);
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
        /// Add new rows to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ugPriceRules.DisplayLayout.Bands[0].AddNew();
                ugPriceRules.ActiveRow.Cells["Pricing"].Value = -1;
                ugPriceRules.ActiveRow.Cells["Source"].Value = -1;
                ugPriceRules.ActiveRow.Cells["PricingPolicy"].Value = -1;
                ugPriceRules.ActiveRow.Cells["SecondarySource"].Value = -1;
                ugPriceRules.ActiveRow.Cells["IsPricingPolicy"].Value = false;
                //ugPriceRules.ActiveRow.Cells[].Column.EditorComponent
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
        /// Function to approve pricing rule
        /// </summary>
        /// <param name="sender">button Approve</param>
        /// <param name="e">e</param>
        public int uBtnApprove_Click()
        {
            int i = 0;
            try
            {
                bool isValidSecondarySource = true;
                // Purpose : To check approval for Blank pricing rules.
                if (HasEmpty())
                {
                    MessageBox.Show("Blank pricing rules cannot be approved. \nFill in all the details", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
                if (_companyID != int.MinValue)
                {
                    List<List<int>> priceList = CreatePricingList(out isValidSecondarySource);
                    if (!isValidSecondarySource)
                    {
                        MessageBox.Show("For asset class other than FX, select secondary source as bloomberg. Details could not be saved.", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 0;
                    }
                    DataSet ds = PricingRuleManager.CreatePricingDataSet(priceList);
                    // Purpose : To check approval for duplicate pricing rules.
                    if (ds.DataSetName == "Duplicate")
                    {
                        MessageBox.Show("Duplicate rules cannot be approved.", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return 0;
                    }
                    else if (priceList.Count > 0)
                    {
                        AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditPricingRuleData(_companyID, priceList, 0), AuditManager.Definitions.Enum.AuditAction.PricingRuleApproved);
                        GetPricingRuleID(Convert.ToInt32(_companyID.ToString() + priceList[0][1].ToString()));
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

        private void ucbAccount_AfterCloseUp(object sender, EventArgs e)
        {
            //if (ucbAccount.Rows.Count == ucbAccount.CheckedRows.Count)
            //{
            //    ugPriceRules.ActiveCell.Value = "All";
            //}
        }

        /// <summary>
        /// Check if grid has empty rows or empty cells
        /// </summary>
        /// <returns>True if there are empty cells</returns>
        public bool HasEmpty()
        {
            for (int i = ugPriceRules.Rows.Count - 1; i >= 0; i--)
            {
                UltraGridRow ugRow = ugPriceRules.Rows[i];
                if (string.IsNullOrEmpty(ugRow.Cells["Account"].Text) && string.IsNullOrEmpty(ugRow.Cells["AssetClass"].Text) && string.IsNullOrEmpty(ugRow.Cells["Exchange"].Text)
                    && ugRow.Cells["Pricing"].Text == "-Select-" && ugRow.Cells["Source"].Text == "-Select-" && ugRow.Cells["SecondarySource"].Text == "-Select-" && ugRow.Cells["RuleType"].Text == "-Select-" && ugRow.Cells["RuleTypeByTime"].Text == "-Select-")
                {
                    ugRow.Delete(false);
                    continue;
                }
                if (Convert.ToBoolean(ugRow.Cells["IsPricingPolicy"].Text))
                {
                    if (string.IsNullOrEmpty(ugRow.Cells["Account"].Text) || ugRow.Cells["PricingPolicy"].Text == "-Select-" || ugRow.Cells["Source"].Text == "-Select-" || ugRow.Cells["SecondarySource"].Text == "-Select-" || ugRow.Cells["RuleType"].Text == "-Select-" || ugRow.Cells["RuleTypeByTime"].Text == "-Select-")
                    {
                        return true;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(ugRow.Cells["Account"].Text) || string.IsNullOrEmpty(ugRow.Cells["AssetClass"].Text) || string.IsNullOrEmpty(ugRow.Cells["Exchange"].Text)
                        || ugRow.Cells["Pricing"].Text == "-Select-" || ugRow.Cells["Source"].Text == "-Select-" || ugRow.Cells["SecondarySource"].Text == "-Select-" || ugRow.Cells["RuleType"].Text == "-Select-" || ugRow.Cells["RuleTypeByTime"].Text == "-Select-")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Save the pricing details in the Database
        /// </summary>
        public int SavePricingDetails()
        {
            try
            {
                bool isValidSecondarySource = true;
                dictActionStatus.Clear();

                if (!_isSaveRequired)
                {
                    return 1;
                }
                if (HasEmpty())
                {
                    MessageBox.Show("Blank pricing rules cannot be inserted. \nFill in all the details", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isValidData = false;
                    return 0;
                }
                ugPriceRules.UpdateData();

                List<List<int>> priceList = CreatePricingList(out isValidSecondarySource);
                if (!isValidSecondarySource)
                {
                    MessageBox.Show("For asset class other than FX, select secondary source as bloomberg. Details could not be saved.", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _isValidData = false;
                    return 0;
                }
                bool isSaved = PricingRuleManager.SavePricingRules(priceList, _deletedPricingRules);
                if (!isSaved)
                {
                    _isValidData = false;
                    MessageBox.Show("Duplicate rules cannot be inserted. Details could not be saved.", "Pricing Rule", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    _isSaveRequired = false;

                    // Audit for deletion
                    if (_deletedPricingRules.Count > 0)
                    {
                        foreach (int ruleID in _deletedPricingRules)
                        {
                            AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditPricingRuleData(_companyID, null, ruleID), AuditManager.Definitions.Enum.AuditAction.PricingRuleDeleted);
                        }
                    }
                    // Audit for creation and updation
                    foreach (int ruleID in PricingRuleManager.GetCompanywisePricingRuleID(priceList, _companyID))
                    {
                        if (dictActionStatus.ContainsKey(ruleID))
                        {
                            int action = dictActionStatus[ruleID];
                            switch (action)
                            {
                                case 1:
                                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditPricingRuleData(_companyID, null, ruleID), AuditManager.Definitions.Enum.AuditAction.PricingRuleCreated);
                                    break;
                                case 3:
                                    AuditManager.BLL.AuditHandler.GetInstance().AuditDataForGivenInstance(this, AuditPricingRuleData(_companyID, null, ruleID), AuditManager.Definitions.Enum.AuditAction.PricingRuleUpdated);
                                    break;
                            }
                        }
                    }
                    _deletedPricingRules.Clear();
                    return 1;
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
            return 0;
        }

        /// <summary>
        /// Function to Get Pricing rules details dictionary
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        private Dictionary<String, List<String>> AuditPricingRuleData(int _companyID, List<List<int>> priceList = null, int ruleID = 0)
        {
            Dictionary<String, List<String>> auditDataForPricingRule = new Dictionary<string, List<string>>();
            try
            {
                auditDataForPricingRule.Add(CustomAuditSourceConstants.AuditSourceTypePricingRule, new List<string>());
                auditDataForPricingRule[CustomAuditSourceConstants.AuditSourceTypePricingRule].Add(_companyID.ToString());

                if (priceList != null)
                {
                    foreach (int id in PricingRuleManager.GetCompanywisePricingRuleID(priceList, _companyID))
                    {
                        // To save composite key (companyID + pricingRuleID) as audit dimension value for unique ID.
                        auditDataForPricingRule[CustomAuditSourceConstants.AuditSourceTypePricingRule].Add(_companyID.ToString() + id.ToString());
                    }
                }
                else
                {
                    // To save composite key (companyID + pricingRuleID) as audit dimension value for unique ID.
                    auditDataForPricingRule[CustomAuditSourceConstants.AuditSourceTypePricingRule].Add(_companyID.ToString() + ruleID.ToString());
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
            return auditDataForPricingRule;
        }

        /// <summary>
        /// Create the List of pricing Details from grid
        /// </summary>
        /// <returns>The list of lists with each element list having the details of one rule</returns>
        public List<List<int>> CreatePricingList(out bool isValidSecondarySource)
        {
            isValidSecondarySource = true;
            List<List<int>> pricingDetails = new List<List<int>>();
            try
            {

                DataTable dt = (DataTable)ugPriceRules.DataSource;
                foreach (DataRow dRow in dt.Rows)
                {
                    int ruleId;
                    int.TryParse(dRow["RuleID"].ToString(), out ruleId);
                    if (dRow.RowState != DataRowState.Unchanged && !_deletedPricingRules.Contains(ruleId))
                    {
                        if (dRow[0] == DBNull.Value)
                        {
                            try
                            {
                                dRow[0] = Convert.ToInt32(dt.Rows[dt.Rows.IndexOf(dRow) - 1][0]) + 1;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                dRow[0] = 1;
                            }
                            // For newly inserted pricing rule
                            dictActionStatus.TryAdd(Convert.ToInt32(dRow[0]), 1);
                        }
                        bool IsPricingPolicy = (Boolean)(dRow["IsPricingPolicy"]);

                        List<object> listAccount = (List<object>)dRow["Account"];
                        foreach (int accountID in listAccount)
                        {
                            if (!IsPricingPolicy)
                            {

                                List<object> listAsset = (List<object>)dRow["AssetClass"];
                                foreach (int assetID in listAsset)
                                {
                                    // Purpose : validate secondarySource should be Bloomberg for asset class other than FX.
                                    // ToDo : Need to remove this check secondary source implemented for all fields other than FX.
                                    if ((assetID != 5 && (int)dRow["SecondarySource"] != 4))
                                    {
                                        if (assetID == 8)
                                        {
                                            if ((int)dRow["SecondarySource"] == 1 || (int)dRow["SecondarySource"] == 2 || (int)dRow["SecondarySource"] == 3)
                                                isValidSecondarySource = false;
                                        }
                                        else
                                        {
                                            isValidSecondarySource = false;
                                        }
                                    }
                                    List<object> listExchange = (List<object>)dRow["Exchange"];
                                    foreach (int exchangeID in listExchange)
                                    {
                                        int[] values = { (int)dRow[0], accountID, 0, -1, assetID, exchangeID, (int)dRow[6], (int)dRow[7], (int)dRow[8], (int)dRow[9], (int)dRow[10] };
                                        List<int> priceRecord = new List<int>(values);
                                        pricingDetails.Add(priceRecord);
                                    }
                                }
                            }
                            else
                            {
                                int[] values = { (int)dRow[0], accountID, 1, (int)dRow[3], int.MinValue, int.MinValue, (int)dRow[6], (int)dRow[7], (int)dRow[8], (int)dRow[9], (int)dRow[10] };
                                List<int> priceRecord = new List<int>(values);
                                pricingDetails.Add(priceRecord);

                            }
                        }
                        // For updated pricing rule
                        dictActionStatus.TryAdd(Convert.ToInt32(dRow[0]), 3);
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
            return pricingDetails;
        }

        /// <summary>
        /// To find currently active row
        /// </summary>
        private void ugPriceRules_AfterRowActivate(object sender, EventArgs e)
        {
            try
            {
                if (this.ugPriceRules.ActiveRow != null)
                {
                    UltraGridRow activeRow = this.ugPriceRules.ActiveRow;
                    if (activeRow.Cells["RuleID"].Value != DBNull.Value)
                    {
                        int ruleID = Convert.ToInt32(activeRow.Cells["RuleID"].Value);
                        GetPricingRuleID(Convert.ToInt32(_companyID.ToString() + ruleID.ToString()));
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

        [AuditManager.Attributes.AuditRefreshMethAttri(AuditManager.Attributes.AuditMehodType.Arguments, 0)]
        public void GetPricingRuleID(int CompanyPricingRuleID) { }

        /// <summary>
        /// Set the flag to indicate that the data is to be saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ugPriceRules_CellChange(object sender, CellEventArgs e)
        {
            try
            {
                if (!_isSaveRequired)
                {
                    _isSaveRequired = true;
                }
                if (e.Cell.Column.Key == "AssetClass")
                {
                    UltraGridColumn gridColumn = e.Cell.Column;
                    String ColumnText = e.Cell.Text;
                    EmbeddableEditorBase editor = e.Cell.EditorResolved;
                    object changedValue = editor.IsValid ? editor.Value : editor.CurrentEditText;
                    UltraGridRow actRow = ugPriceRules.ActiveRow;
                    List<object> assets = (List<object>)changedValue;
                    ugPriceRules.ActiveRow.Cells["Exchange"].Value = DBNull.Value;
                    ugPriceRules.ActiveRow.Cells["Exchange"].EditorComponent = EditorComponentForCell(assets);
                }

                UltraGridRow row = e.Cell.Row;
                if (e.Cell.Column.Key == "IsPricingPolicy")
                {
                    if (bool.Parse(e.Cell.Text) == true)
                    {
                        row.Cells["Exchange"].Activation = Activation.Disabled;
                        row.Cells["AssetClass"].Activation = Activation.Disabled;
                        row.Cells["Pricing"].Activation = Activation.Disabled;
                        row.Cells["PricingPolicy"].Activation = Activation.AllowEdit;
                    }
                    else
                    {
                        row.Cells["Exchange"].Activation = Activation.AllowEdit;
                        row.Cells["AssetClass"].Activation = Activation.AllowEdit;
                        row.Cells["Pricing"].Activation = Activation.AllowEdit;
                        row.Cells["PricingPolicy"].Activation = Activation.Disabled;
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
        /// added by bharat raturi, 03 jun 2014
        /// </summary>
        /// <param name="assets">List of asset IDs</param>
        /// <returns>Ultracombo for cell</returns>
        private UltraCombo EditorComponentForCell(List<object> assets)
        {
            UltraCombo cmbCell = new UltraCombo();
            try
            {
                cmbCell.DataSource = PricingRuleManager.GetCurrentAssetExchanges(assets);
                if (cmbCell.DataSource != null)
                {
                    if (!cmbCell.DisplayLayout.Bands[0].Columns.Exists("Selected"))
                    {
                        UltraGridColumn colSelectExchange = cmbCell.DisplayLayout.Bands[0].Columns.Add();
                        colSelectExchange.Key = "Selected";
                        colSelectExchange.Header.Caption = string.Empty;
                        colSelectExchange.Width = 25;
                        colSelectExchange.Header.CheckBoxVisibility = HeaderCheckBoxVisibility.Always;
                        colSelectExchange.DataType = typeof(bool);
                        colSelectExchange.DefaultCellValue = false;
                        colSelectExchange.Header.VisiblePosition = 1;
                    }
                    cmbCell.CheckedListSettings.CheckStateMember = "Selected";
                    cmbCell.CheckedListSettings.EditorValueSource = EditorWithComboValueSource.CheckedItems;
                    cmbCell.CheckedListSettings.ListSeparator = " , ";
                    cmbCell.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
                    cmbCell.DisplayMember = "ExchangeName";
                    cmbCell.ValueMember = "ExchangeID";
                    cmbCell.NullText = "-Select-";
                    cmbCell.DisplayLayout.Bands[0].Columns[1].Header.Caption = "Select All";
                    cmbCell.DisplayLayout.Bands[0].Columns[0].Hidden = true;
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
            return cmbCell;
        }

        private void SetExchangeEditorControl()
        {
            try
            {
                foreach (UltraGridRow row in ugPriceRules.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row.Cells["AssetClass"].Value.ToString()))
                    {
                        List<object> clients = (row.Cells["AssetClass"].Value) as List<object>;
                        row.Cells["Exchange"].EditorComponent = EditorComponentForCell(clients);
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
        private void btnPricingPolicy_Click(object sender, EventArgs e)
        {
            try
            {

                Form frmPricingPolicy = new Form();
                frmPricingPolicy.Size = new System.Drawing.Size(765, 540);
                frmPricingPolicy.ShowIcon = false;
                frmPricingPolicy.Text = "Pricing Policy";
                //added by amit on 07.04.2015
                //http://jira.nirvanasolutions.com:8080/browse/CHMW-3280
                frmPricingPolicy.FormBorderStyle = FormBorderStyle.FixedSingle;
                frmPricingPolicy.MaximumSize = frmPricingPolicy.MinimumSize = new System.Drawing.Size(765, 540);
                CtrlPricingPolicy ctrlPricingPolicy1 = new CtrlPricingPolicy();
                frmPricingPolicy.Controls.Add(ctrlPricingPolicy1);
                CustomThemeHelper.SetThemeAtDynamicForm(frmPricingPolicy, ctrlPricingPolicy1);
                frmPricingPolicy.Load += frmPricingPolicy_Load;

                frmPricingPolicy.Owner = this.FindForm();
                frmPricingPolicy.ShowDialog();


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



        private void frmPricingPolicy_Load(object sender, EventArgs e)
        {
            try
            {

                CustomThemeHelper.SetThemeProperties((sender as Form).FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
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

        //private void ugPriceRules_CellChange(object sender, CellEventArgs e)
        //{
        //    try
        //    {
        //        UltraGridRow row = e.Cell.Row;
        //        if (e.Cell.Column.Key == "IsPricingPolicy")
        //        {
        //            if (bool.Parse(e.Cell.Text) == true)
        //            {
        //                row.Cells["Exchange"].Activation = Activation.Disabled;
        //                row.Cells["AssetClass"].Activation = Activation.Disabled;
        //                row.Cells["Pricing"].Activation = Activation.Disabled;
        //                row.Cells["PricingPolicy"].Activation = Activation.AllowEdit;
        //            }
        //            else
        //            {
        //                row.Cells["Exchange"].Activation = Activation.AllowEdit;
        //                row.Cells["AssetClass"].Activation = Activation.AllowEdit;
        //                row.Cells["Pricing"].Activation = Activation.AllowEdit;
        //                row.Cells["PricingPolicy"].Activation = Activation.Disabled;
        //            }
        //        }
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

        private void ugPriceRules_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            try
            {
                UltraGridRow row = e.Row;
                //e.row.Column.Key
                if (row.Cells.Exists("IsPricingPolicy"))
                {
                    if (row.Cells["IsPricingPolicy"].Value != System.DBNull.Value)
                    {
                        if (Convert.ToBoolean(row.Cells["IsPricingPolicy"].Value) == true)
                        {
                            row.Cells["Exchange"].Activation = Activation.Disabled;
                            row.Cells["AssetClass"].Activation = Activation.Disabled;
                            row.Cells["Pricing"].Activation = Activation.Disabled;
                            row.Cells["PricingPolicy"].Activation = Activation.AllowEdit;
                        }
                        else
                        {
                            row.Cells["Exchange"].Activation = Activation.AllowEdit;
                            row.Cells["AssetClass"].Activation = Activation.AllowEdit;
                            row.Cells["Pricing"].Activation = Activation.AllowEdit;
                            row.Cells["PricingPolicy"].Activation = Activation.Disabled;
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
    }
}
