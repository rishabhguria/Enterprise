#region Using
using Infragistics.Win;
using Infragistics.Win.UltraWinToolTip;
using Prana.Admin.BLL;
using Prana.BusinessObjects.Compliance;
using Prana.BusinessObjects.Compliance.Permissions;
using Prana.CommonDataCache;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
#endregion

namespace Prana.Admin.Controls
{
    /// <summary>
    /// Summary description for CompanyUserPermissions.
    ///  
    /// We have used checkedListBoxes instead of ListBoxes though the code for the list boxes still persists
    /// and the ListBoxes are invisible. CounterParty Venue List box yet to be changed to checked LIst box.
    /// </summary>
    public class CompanyUserPermissions : System.Windows.Forms.UserControl
    {
        private const string FORM_NAME = "CompanyUserPermissions : ";
        const int POSITION_MANAGEMENT = 1;
        const int ALLOCATION = 3;
        const int COMPLIANCEPRETRADE = 4;
        const int COMPLIANCEPOSTTRADE = 5;
        const int AUDITTRAIL = 6;
        const int TRUE = 1;
        #region private and protected members

        private Infragistics.Win.Misc.UltraGroupBox grpCounterParties;
        private Infragistics.Win.Misc.UltraGroupBox grpApplicationComponent;
        private Infragistics.Win.Misc.UltraGroupBox grpAssetUnderlying;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;

        private int _companyID = int.MinValue;

        #endregion

        private System.Windows.Forms.CheckedListBox checkedlstAssetUnderLying;
        private System.Windows.Forms.CheckedListBox checkedlstTradingAccount;
        private System.Windows.Forms.CheckedListBox checkedlstCounterParties;
        private Infragistics.Win.Misc.UltraGroupBox grpAccounts;
        private System.Windows.Forms.CheckedListBox checkedlstAccounts;
        private Infragistics.Win.Misc.UltraGroupBox grpStrategies;
        private System.Windows.Forms.CheckedListBox checkedlstStrategies;
        private Infragistics.Win.Misc.UltraGroupBox grpAllocationTradingAccounts;
        private System.Windows.Forms.CheckedListBox checkedlistAllocationTradingAccounts;
        private GroupBox grpPM;
        private RadioButton optionReadWritePM;
        private RadioButton optionReadPM;
        private CheckBox chbExportPM;
        private GroupBox grpAllocation;
        private RadioButton rdbReadWriteAllocation;
        private RadioButton rdbReadOnlyAllocation;
        private GroupBox grpRestrictedAllowedSecurities;
        private RadioButton rdbReadWriteRestrictedAllowedSecurities;
        private RadioButton rdbReadOnlyRestrictedAllowedSecurities;
        private CheckBox chbPreTradeEnabled;
        private CheckBox chbOverridePermission;
        private GroupBox gbCompliance;
        private GroupBox gbPreTrade;
        private CheckBox chbApplyToManual;
        private CheckBox chbPowerUser;
        private GroupBox grpCompliancePostTrade;
        private GroupBox grpCompliancePreTrade;
        private CheckedListBox chkListAuditUsers;
        private GroupBox grpAudit;
        private CheckBox chckPostExport;
        private CheckBox chckPostDelete;
        private CheckBox chckPostCreate;
        private CheckBox chckPostImport;
        private CheckBox chckPostRename;
        private CheckBox chckPostEnable;
        private CheckBox chckPreExport;
        private CheckBox chckPreDelete;
        private CheckBox chckPreCreate;
        private CheckBox chckPreImport;
        private CheckBox chckPreRename;
        private CheckBox chckPreEnable;

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private CheckedListBox chkMarketDataTypes;

        private IDictionary<string, IList<string>> _marketDataTypeModules;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComEdORRulePermission;
        private UltraToolTipManager ultraToolTipManager1;
        private GroupBox groupBoxComplianceCheck;
        private CheckBox checkBoxStaging;
        private CheckBox checkBoxTrading;
        private CheckBox auec_selectallbox;
        private CheckBox appcomp_selectallbox;
        private CheckBox trading_selectallbox;
        private CheckBox account_selectallbox;
        private CheckBox strategy_selectallbox;
        private CheckBox brokervenue_selectallbox;
        private CheckedListBox checkedlstApplicationComponent;
        private CheckBox auditlist_selectallbox;
        private CheckBox livefeed_selectallbox;
        private Label lblSeperatorPM;
        private IContainer components;

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public CompanyUserPermissions()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            if (!DesignMode)
            {
                //
                //Below code will not interfere the design window. So Manual event wiring is done here.
                //
                brokervenue_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstCounterParties); };
                checkedlstCounterParties.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, brokervenue_selectallbox); };
                checkedlstCounterParties.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, brokervenue_selectallbox); };

                appcomp_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstApplicationComponent); };
                checkedlstApplicationComponent.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, appcomp_selectallbox); };
                checkedlstApplicationComponent.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, appcomp_selectallbox); };

                auec_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstAssetUnderLying); };
                checkedlstAssetUnderLying.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, auec_selectallbox); };
                checkedlstAssetUnderLying.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, auec_selectallbox); };

                trading_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstTradingAccount); };
                checkedlstTradingAccount.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, trading_selectallbox); };
                checkedlstTradingAccount.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, trading_selectallbox); };

                account_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstAccounts); };
                checkedlstAccounts.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, account_selectallbox); };
                checkedlstAccounts.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, account_selectallbox); };

                strategy_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, checkedlstStrategies); };
                checkedlstStrategies.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, strategy_selectallbox); };
                checkedlstStrategies.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, strategy_selectallbox); };

                auditlist_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, chkListAuditUsers); };
                chkListAuditUsers.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, auditlist_selectallbox); };
                chkListAuditUsers.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, auditlist_selectallbox); };

                livefeed_selectallbox.Click += delegate (object sender, EventArgs e) { selectallbox_Click(sender, e, chkMarketDataTypes); };
                chkMarketDataTypes.SelectedValueChanged += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, livefeed_selectallbox); };
                chkMarketDataTypes.DoubleClick += delegate (object sender, EventArgs e) { checkedlistbox_SelectedValueChanged(sender, e, livefeed_selectallbox); };

            }
            // TODO: Add any initialization after the InitializeComponent call

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (grpCounterParties != null)
                {
                    grpCounterParties.Dispose();
                }
                if (grpApplicationComponent != null)
                {
                    grpApplicationComponent.Dispose();
                }
                if (grpAssetUnderlying != null)
                {
                    grpAssetUnderlying.Dispose();
                }
                if (ultraGroupBox1 != null)
                {
                    ultraGroupBox1.Dispose();
                }
                if (checkedlstAssetUnderLying != null)
                {
                    checkedlstAssetUnderLying.Dispose();
                }
                if (checkedlstTradingAccount != null)
                {
                    checkedlstTradingAccount.Dispose();
                }
                if (checkedlstCounterParties != null)
                {
                    checkedlstCounterParties.Dispose();
                }
                if (grpAccounts != null)
                {
                    grpAccounts.Dispose();
                }
                if (checkedlstAccounts != null)
                {
                    checkedlstAccounts.Dispose();
                }
                if (grpStrategies != null)
                {
                    grpStrategies.Dispose();
                }
                if (checkedlstStrategies != null)
                {
                    checkedlstStrategies.Dispose();
                }
                if (grpAllocationTradingAccounts != null)
                {
                    grpAllocationTradingAccounts.Dispose();
                }
                if (checkedlistAllocationTradingAccounts != null)
                {
                    checkedlistAllocationTradingAccounts.Dispose();
                }
                if (grpPM != null)
                {
                    grpPM.Dispose();
                }
                if (optionReadWritePM != null)
                {
                    optionReadWritePM.Dispose();
                }
                if (optionReadPM != null)
                {
                    optionReadPM.Dispose();
                }
                if (chbExportPM != null)
                {
                    chbExportPM.Dispose();
                }
                if (grpAllocation != null)
                {
                    grpAllocation.Dispose();
                }
                if (rdbReadWriteAllocation != null)
                {
                    rdbReadWriteAllocation.Dispose();
                }
                if (rdbReadOnlyAllocation != null)
                {
                    rdbReadOnlyAllocation.Dispose();
                }
                if (grpRestrictedAllowedSecurities != null)
                {
                    grpRestrictedAllowedSecurities.Dispose();
                }
                if (rdbReadWriteRestrictedAllowedSecurities != null)
                {
                    rdbReadWriteRestrictedAllowedSecurities.Dispose();
                }
                if (chbPreTradeEnabled != null)
                {
                    chbPreTradeEnabled.Dispose();
                }
                if (chbOverridePermission != null)
                {
                    chbOverridePermission.Dispose();
                }
                if (gbCompliance != null)
                {
                    gbCompliance.Dispose();
                }
                if (gbPreTrade != null)
                {
                    gbPreTrade.Dispose();
                }
                if (chbApplyToManual != null)
                {
                    chbApplyToManual.Dispose();
                }
                if (chbPowerUser != null)
                {
                    chbPowerUser.Dispose();
                }
                if (grpCompliancePostTrade != null)
                {
                    grpCompliancePostTrade.Dispose();
                }
                if (grpCompliancePreTrade != null)
                {
                    grpCompliancePreTrade.Dispose();
                }
                if (chkListAuditUsers != null)
                {
                    chkListAuditUsers.Dispose();
                }
                if (grpAudit != null)
                {
                    grpAudit.Dispose();
                }
                if (chckPostExport != null)
                {
                    chckPostExport.Dispose();
                }
                if (chckPostDelete != null)
                {
                    chckPostDelete.Dispose();
                }
                if (chckPostCreate != null)
                {
                    chckPostCreate.Dispose();
                }
                if (chckPostImport != null)
                {
                    chckPostImport.Dispose();
                }
                if (chckPostRename != null)
                {
                    chckPostRename.Dispose();
                }
                if (chckPostEnable != null)
                {
                    chckPostEnable.Dispose();
                }
                if (chckPreImport != null)
                {
                    chckPreImport.Dispose();
                }
                if (chckPreDelete != null)
                {
                    chckPreDelete.Dispose();
                }
                if (chckPreCreate != null)
                {
                    chckPreCreate.Dispose();
                }
                if (chckPreImport != null)
                {
                    chckPreImport.Dispose();
                }
                if (chckPreRename != null)
                {
                    chckPreRename.Dispose();
                }
                if (chckPreEnable != null)
                {
                    chckPreEnable.Dispose();
                }
                if (ultraGroupBox2 != null)
                {
                    ultraGroupBox2.Dispose();
                }
                if (chkMarketDataTypes != null)
                {
                    chkMarketDataTypes.Dispose();
                }
                if (ultraComEdORRulePermission != null)
                {
                    ultraComEdORRulePermission.Dispose();
                }
                if (ultraToolTipManager1 != null)
                {
                    ultraToolTipManager1.Dispose();
                }
                if (groupBoxComplianceCheck != null)
                {
                    groupBoxComplianceCheck.Dispose();
                }
                if (checkBoxStaging != null)
                {
                    checkBoxStaging.Dispose();
                }
                if (checkBoxTrading != null)
                {
                    checkBoxTrading.Dispose();
                }
                if (auec_selectallbox != null)
                {
                    auec_selectallbox.Dispose();
                }
                if (appcomp_selectallbox != null)
                {
                    appcomp_selectallbox.Dispose();
                }
                if (trading_selectallbox != null)
                {
                    trading_selectallbox.Dispose();
                }
                if (account_selectallbox != null)
                {
                    account_selectallbox.Dispose();
                }
                if (strategy_selectallbox != null)
                {
                    strategy_selectallbox.Dispose();
                }
                if (brokervenue_selectallbox != null)
                {
                    brokervenue_selectallbox.Dispose();
                }
                if (checkedlstApplicationComponent != null)
                {
                    checkedlstApplicationComponent.Dispose();
                }
                if (auditlist_selectallbox != null)
                {
                    auditlist_selectallbox.Dispose();
                }
                if (livefeed_selectallbox != null)
                {
                    livefeed_selectallbox.Dispose();
                }
                if (lblSeperatorPM != null)
                {
                    lblSeperatorPM.Dispose();
                }
                if (chckPreExport != null)
                {
                    chckPreExport.Dispose();
                }
                if (rdbReadOnlyRestrictedAllowedSecurities != null)
                {
                    rdbReadOnlyRestrictedAllowedSecurities.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Check Compliance", Infragistics.Win.ToolTipImage.Default, null, Infragistics.Win.DefaultableBoolean.Default);
            this.grpCounterParties = new Infragistics.Win.Misc.UltraGroupBox();
            this.brokervenue_selectallbox = new System.Windows.Forms.CheckBox();
            this.checkedlstCounterParties = new System.Windows.Forms.CheckedListBox();
            this.grpApplicationComponent = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkedlstApplicationComponent = new System.Windows.Forms.CheckedListBox();
            this.appcomp_selectallbox = new System.Windows.Forms.CheckBox();
            this.grpAssetUnderlying = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkedlstAssetUnderLying = new System.Windows.Forms.CheckedListBox();
            this.auec_selectallbox = new System.Windows.Forms.CheckBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkedlstTradingAccount = new System.Windows.Forms.CheckedListBox();
            this.trading_selectallbox = new System.Windows.Forms.CheckBox();
            this.grpAccounts = new Infragistics.Win.Misc.UltraGroupBox();
            this.account_selectallbox = new System.Windows.Forms.CheckBox();
            this.checkedlstAccounts = new System.Windows.Forms.CheckedListBox();
            this.grpAllocationTradingAccounts = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkedlistAllocationTradingAccounts = new System.Windows.Forms.CheckedListBox();
            this.grpStrategies = new Infragistics.Win.Misc.UltraGroupBox();
            this.checkedlstStrategies = new System.Windows.Forms.CheckedListBox();
            this.strategy_selectallbox = new System.Windows.Forms.CheckBox();
            this.grpPM = new System.Windows.Forms.GroupBox();
            this.lblSeperatorPM = new System.Windows.Forms.Label();
            this.optionReadWritePM = new System.Windows.Forms.RadioButton();
            this.optionReadPM = new System.Windows.Forms.RadioButton();
            this.chbExportPM = new System.Windows.Forms.CheckBox();
            this.grpAllocation = new System.Windows.Forms.GroupBox();
            this.rdbReadWriteAllocation = new System.Windows.Forms.RadioButton();
            this.rdbReadOnlyAllocation = new System.Windows.Forms.RadioButton();
            this.grpRestrictedAllowedSecurities = new System.Windows.Forms.GroupBox();
            this.rdbReadWriteRestrictedAllowedSecurities = new System.Windows.Forms.RadioButton();
            this.rdbReadOnlyRestrictedAllowedSecurities = new System.Windows.Forms.RadioButton();
            this.chbOverridePermission = new System.Windows.Forms.CheckBox();
            this.chbPreTradeEnabled = new System.Windows.Forms.CheckBox();
            this.gbCompliance = new System.Windows.Forms.GroupBox();
            this.chbPowerUser = new System.Windows.Forms.CheckBox();
            this.grpCompliancePostTrade = new System.Windows.Forms.GroupBox();
            this.chckPostExport = new System.Windows.Forms.CheckBox();
            this.chckPostDelete = new System.Windows.Forms.CheckBox();
            this.chckPostCreate = new System.Windows.Forms.CheckBox();
            this.chckPostImport = new System.Windows.Forms.CheckBox();
            this.chckPostRename = new System.Windows.Forms.CheckBox();
            this.chckPostEnable = new System.Windows.Forms.CheckBox();
            this.grpCompliancePreTrade = new System.Windows.Forms.GroupBox();
            this.chckPreExport = new System.Windows.Forms.CheckBox();
            this.chckPreDelete = new System.Windows.Forms.CheckBox();
            this.chckPreCreate = new System.Windows.Forms.CheckBox();
            this.chckPreImport = new System.Windows.Forms.CheckBox();
            this.chckPreRename = new System.Windows.Forms.CheckBox();
            this.chckPreEnable = new System.Windows.Forms.CheckBox();
            this.gbPreTrade = new System.Windows.Forms.GroupBox();
            this.groupBoxComplianceCheck = new System.Windows.Forms.GroupBox();
            this.checkBoxStaging = new System.Windows.Forms.CheckBox();
            this.checkBoxTrading = new System.Windows.Forms.CheckBox();
            this.ultraComEdORRulePermission = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chbApplyToManual = new System.Windows.Forms.CheckBox();
            this.chkListAuditUsers = new System.Windows.Forms.CheckedListBox();
            this.grpAudit = new System.Windows.Forms.GroupBox();
            this.auditlist_selectallbox = new System.Windows.Forms.CheckBox();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.livefeed_selectallbox = new System.Windows.Forms.CheckBox();
            this.chkMarketDataTypes = new System.Windows.Forms.CheckedListBox();
            this.ultraToolTipManager1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grpCounterParties)).BeginInit();
            this.grpCounterParties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpApplicationComponent)).BeginInit();
            this.grpApplicationComponent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAssetUnderlying)).BeginInit();
            this.grpAssetUnderlying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAccounts)).BeginInit();
            this.grpAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpAllocationTradingAccounts)).BeginInit();
            this.grpAllocationTradingAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpStrategies)).BeginInit();
            this.grpStrategies.SuspendLayout();
            this.grpPM.SuspendLayout();
            this.grpAllocation.SuspendLayout();
            this.grpRestrictedAllowedSecurities.SuspendLayout();
            this.gbCompliance.SuspendLayout();
            this.grpCompliancePostTrade.SuspendLayout();
            this.grpCompliancePreTrade.SuspendLayout();
            this.gbPreTrade.SuspendLayout();
            this.groupBoxComplianceCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComEdORRulePermission)).BeginInit();
            this.grpAudit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCounterParties
            // 
            this.grpCounterParties.Controls.Add(this.brokervenue_selectallbox);
            this.grpCounterParties.Controls.Add(this.checkedlstCounterParties);
            this.grpCounterParties.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCounterParties.ForeColor = System.Drawing.Color.White;
            this.grpCounterParties.Location = new System.Drawing.Point(3, 5);
            this.grpCounterParties.Name = "grpCounterParties";
            this.grpCounterParties.Size = new System.Drawing.Size(242, 170);
            this.grpCounterParties.TabIndex = 0;
            // 
            // brokervenue_selectallbox
            // 
            this.brokervenue_selectallbox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.brokervenue_selectallbox.ForeColor = System.Drawing.Color.Black;
            this.brokervenue_selectallbox.Location = new System.Drawing.Point(10, 9);
            this.brokervenue_selectallbox.Name = "brokervenue_selectallbox";
            this.brokervenue_selectallbox.Size = new System.Drawing.Size(112, 18);
            this.brokervenue_selectallbox.TabIndex = 11;
            this.brokervenue_selectallbox.Text = "BrokerVenues";
            // 
            // checkedlstCounterParties
            // 
            this.checkedlstCounterParties.CheckOnClick = true;
            this.checkedlstCounterParties.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstCounterParties.Location = new System.Drawing.Point(6, 30);
            this.checkedlstCounterParties.Name = "checkedlstCounterParties";
            this.checkedlstCounterParties.Size = new System.Drawing.Size(228, 132);
            this.checkedlstCounterParties.TabIndex = 0;
            // 
            // grpApplicationComponent
            // 
            this.grpApplicationComponent.Controls.Add(this.checkedlstApplicationComponent);
            this.grpApplicationComponent.Controls.Add(this.appcomp_selectallbox);
            this.grpApplicationComponent.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpApplicationComponent.Location = new System.Drawing.Point(251, 3);
            this.grpApplicationComponent.Name = "grpApplicationComponent";
            this.grpApplicationComponent.Size = new System.Drawing.Size(246, 172);
            this.grpApplicationComponent.TabIndex = 1;
            // 
            // checkedlstApplicationComponent
            // 
            this.checkedlstApplicationComponent.CheckOnClick = true;
            this.checkedlstApplicationComponent.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstApplicationComponent.Location = new System.Drawing.Point(0, 32);
            this.checkedlstApplicationComponent.Name = "checkedlstApplicationComponent";
            this.checkedlstApplicationComponent.Size = new System.Drawing.Size(227, 132);
            this.checkedlstApplicationComponent.TabIndex = 0;
            // appcomp_selectallbox
            // 
            this.appcomp_selectallbox.Location = new System.Drawing.Point(3, 5);
            this.appcomp_selectallbox.Name = "appcomp_selectallbox";
            this.appcomp_selectallbox.Size = new System.Drawing.Size(226, 24);
            this.appcomp_selectallbox.TabIndex = 0;
            this.appcomp_selectallbox.Text = "Application Components";
            // grpAssetUnderlying
            // 
            this.grpAssetUnderlying.Controls.Add(this.checkedlstAssetUnderLying);
            this.grpAssetUnderlying.Controls.Add(this.auec_selectallbox);
            this.grpAssetUnderlying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAssetUnderlying.HeaderBorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grpAssetUnderlying.Location = new System.Drawing.Point(0, 193);
            this.grpAssetUnderlying.Name = "grpAssetUnderlying";
            this.grpAssetUnderlying.Size = new System.Drawing.Size(246, 186);
            this.grpAssetUnderlying.TabIndex = 2;
            // 
            // checkedlstAssetUnderLying
            // 
            this.checkedlstAssetUnderLying.CheckOnClick = true;
            this.checkedlstAssetUnderLying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstAssetUnderLying.HorizontalExtent = 500;
            this.checkedlstAssetUnderLying.HorizontalScrollbar = true;
            this.checkedlstAssetUnderLying.Location = new System.Drawing.Point(10, 32);
            this.checkedlstAssetUnderLying.Name = "checkedlstAssetUnderLying";
            this.checkedlstAssetUnderLying.Size = new System.Drawing.Size(228, 148);
            this.checkedlstAssetUnderLying.TabIndex = 0;
            // 
            // auec_selectallbox
            // 
            this.auec_selectallbox.Location = new System.Drawing.Point(13, 9);
            this.auec_selectallbox.Name = "auec_selectallbox";
            this.auec_selectallbox.Size = new System.Drawing.Size(225, 24);
            this.auec_selectallbox.TabIndex = 1;
            this.auec_selectallbox.Text = "AUEC";
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.checkedlstTradingAccount);
            this.ultraGroupBox1.Controls.Add(this.trading_selectallbox);
            this.ultraGroupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGroupBox1.Location = new System.Drawing.Point(256, 193);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(246, 186);
            this.ultraGroupBox1.TabIndex = 3;
            // 
            // checkedlstTradingAccount
            // 
            this.checkedlstTradingAccount.CheckOnClick = true;
            this.checkedlstTradingAccount.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstTradingAccount.Location = new System.Drawing.Point(0, 32);
            this.checkedlstTradingAccount.Name = "checkedlstTradingAccount";
            this.checkedlstTradingAccount.Size = new System.Drawing.Size(232, 148);
            this.checkedlstTradingAccount.TabIndex = 0;
            // 
            // trading_selectallbox
            // 
            this.trading_selectallbox.Location = new System.Drawing.Point(3, 9);
            this.trading_selectallbox.Name = "trading_selectallbox";
            this.trading_selectallbox.Size = new System.Drawing.Size(226, 21);
            this.trading_selectallbox.TabIndex = 0;
            this.trading_selectallbox.Text = "Trading Accounts";
            this.trading_selectallbox.UseMnemonic = false;
            // 
            // grpAccounts
            // 
            this.grpAccounts.Controls.Add(this.account_selectallbox);
            this.grpAccounts.Controls.Add(this.checkedlstAccounts);
            this.grpAccounts.Controls.Add(this.grpAllocationTradingAccounts);
            this.grpAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAccounts.Location = new System.Drawing.Point(3, 389);
            this.grpAccounts.Name = "grpAccounts";
            this.grpAccounts.Size = new System.Drawing.Size(246, 167);
            this.grpAccounts.TabIndex = 4;
            // 
            // account_selectallbox
            // 
            this.account_selectallbox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.account_selectallbox.Location = new System.Drawing.Point(9, 6);
            this.account_selectallbox.Name = "account_selectallbox";
            this.account_selectallbox.Size = new System.Drawing.Size(227, 22);
            this.account_selectallbox.TabIndex = 4;
            this.account_selectallbox.Text = "Accounts";
            this.account_selectallbox.UseVisualStyleBackColor = true;
            // 
            // checkedlstAccounts
            // 
            this.checkedlstAccounts.CheckOnClick = true;
            this.checkedlstAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstAccounts.Location = new System.Drawing.Point(7, 29);
            this.checkedlstAccounts.Name = "checkedlstAccounts";
            this.checkedlstAccounts.Size = new System.Drawing.Size(232, 132);
            this.checkedlstAccounts.TabIndex = 0;
            // 
            // grpAllocationTradingAccounts
            // 
            this.grpAllocationTradingAccounts.Controls.Add(this.checkedlistAllocationTradingAccounts);
            this.grpAllocationTradingAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAllocationTradingAccounts.Location = new System.Drawing.Point(241, 0);
            this.grpAllocationTradingAccounts.Name = "grpAllocationTradingAccounts";
            this.grpAllocationTradingAccounts.Size = new System.Drawing.Size(23, 16);
            this.grpAllocationTradingAccounts.TabIndex = 3;
            this.grpAllocationTradingAccounts.Text = "Allocation Trading Accounts";
            this.grpAllocationTradingAccounts.Visible = false;
            // 
            // checkedlistAllocationTradingAccounts
            // 
            this.checkedlistAllocationTradingAccounts.CheckOnClick = true;
            this.checkedlistAllocationTradingAccounts.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlistAllocationTradingAccounts.Location = new System.Drawing.Point(12, 18);
            this.checkedlistAllocationTradingAccounts.Name = "checkedlistAllocationTradingAccounts";
            this.checkedlistAllocationTradingAccounts.Size = new System.Drawing.Size(232, 132);
            this.checkedlistAllocationTradingAccounts.TabIndex = 4;
            // 
            // grpStrategies
            // 
            this.grpStrategies.Controls.Add(this.checkedlstStrategies);
            this.grpStrategies.Controls.Add(this.strategy_selectallbox);
            this.grpStrategies.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpStrategies.Location = new System.Drawing.Point(254, 388);
            this.grpStrategies.Name = "grpStrategies";
            this.grpStrategies.Size = new System.Drawing.Size(251, 168);
            this.grpStrategies.TabIndex = 5;
            // 
            // checkedlstStrategies
            // 
            this.checkedlstStrategies.CheckOnClick = true;
            this.checkedlstStrategies.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.checkedlstStrategies.Location = new System.Drawing.Point(6, 30);
            this.checkedlstStrategies.Name = "checkedlstStrategies";
            this.checkedlstStrategies.Size = new System.Drawing.Size(231, 132);
            this.checkedlstStrategies.TabIndex = 0;
            // 
            // strategy_selectallbox
            // 
            this.strategy_selectallbox.Location = new System.Drawing.Point(6, 12);
            this.strategy_selectallbox.Name = "strategy_selectallbox";
            this.strategy_selectallbox.Size = new System.Drawing.Size(87, 17);
            this.strategy_selectallbox.TabIndex = 1;
            this.strategy_selectallbox.Text = "Strategies";
            this.strategy_selectallbox.UseVisualStyleBackColor = true;
            // 
            // grpPM
            // 
            this.grpPM.Controls.Add(this.lblSeperatorPM);
            this.grpPM.Controls.Add(this.optionReadWritePM);
            this.grpPM.Controls.Add(this.optionReadPM);
            this.grpPM.Controls.Add(this.chbExportPM);
            this.grpPM.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpPM.Location = new System.Drawing.Point(508, 5);
            this.grpPM.Name = "grpPM";
            this.grpPM.Size = new System.Drawing.Size(141, 98);
            this.grpPM.TabIndex = 7;
            this.grpPM.TabStop = false;
            this.grpPM.Text = "PM";
            this.grpPM.Visible = false;
            // 
            // lblSeperatorPM
            // 
            this.lblSeperatorPM.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeperatorPM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeperatorPM.Location = new System.Drawing.Point(0, 70);
            this.lblSeperatorPM.Name = "lblSeperatorPM";
            this.lblSeperatorPM.Size = new System.Drawing.Size(141, 3);
            this.lblSeperatorPM.TabIndex = 6;
            // 
            // optionReadWritePM
            // 
            this.optionReadWritePM.AutoSize = true;
            this.optionReadWritePM.Location = new System.Drawing.Point(16, 43);
            this.optionReadWritePM.Name = "optionReadWritePM";
            this.optionReadWritePM.Size = new System.Drawing.Size(91, 17);
            this.optionReadWritePM.TabIndex = 1;
            this.optionReadWritePM.TabStop = true;
            this.optionReadWritePM.Text = "Read/Write";
            this.optionReadWritePM.UseVisualStyleBackColor = true;
            // 
            // optionReadPM
            // 
            this.optionReadPM.AutoSize = true;
            this.optionReadPM.Checked = true;
            this.optionReadPM.Location = new System.Drawing.Point(17, 20);
            this.optionReadPM.Name = "optionReadPM";
            this.optionReadPM.Size = new System.Drawing.Size(54, 17);
            this.optionReadPM.TabIndex = 0;
            this.optionReadPM.TabStop = true;
            this.optionReadPM.Text = "Read";
            this.optionReadPM.UseVisualStyleBackColor = true;
            // 
            // chbExportPM
            // 
            this.chbExportPM.AutoSize = true;
            this.chbExportPM.Location = new System.Drawing.Point(16, 75);
            this.chbExportPM.Name = "chbExportPM";
            this.chbExportPM.Size = new System.Drawing.Size(63, 17);
            this.chbExportPM.TabIndex = 5;
            this.chbExportPM.Text = "Export";
            this.chbExportPM.UseVisualStyleBackColor = true;
            // 
            // grpAllocation
            // 
            this.grpAllocation.Controls.Add(this.rdbReadWriteAllocation);
            this.grpAllocation.Controls.Add(this.rdbReadOnlyAllocation);
            this.grpAllocation.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpAllocation.Location = new System.Drawing.Point(508, 109);
            this.grpAllocation.Name = "grpAllocation";
            this.grpAllocation.Size = new System.Drawing.Size(141, 66);
            this.grpAllocation.TabIndex = 8;
            this.grpAllocation.TabStop = false;
            this.grpAllocation.Text = "Allocation";
            // 
            // rdbReadWriteAllocation
            // 
            this.rdbReadWriteAllocation.AutoSize = true;
            this.rdbReadWriteAllocation.Location = new System.Drawing.Point(16, 43);
            this.rdbReadWriteAllocation.Name = "rdbReadWriteAllocation";
            this.rdbReadWriteAllocation.Size = new System.Drawing.Size(91, 17);
            this.rdbReadWriteAllocation.TabIndex = 1;
            this.rdbReadWriteAllocation.Text = "Read/Write";
            this.rdbReadWriteAllocation.UseVisualStyleBackColor = true;
            // 
            // rdbReadOnlyAllocation
            // 
            this.rdbReadOnlyAllocation.AutoSize = true;
            this.rdbReadOnlyAllocation.Checked = true;
            this.rdbReadOnlyAllocation.Location = new System.Drawing.Point(17, 20);
            this.rdbReadOnlyAllocation.Name = "rdbReadOnlyAllocation";
            this.rdbReadOnlyAllocation.Size = new System.Drawing.Size(54, 17);
            this.rdbReadOnlyAllocation.TabIndex = 0;
            this.rdbReadOnlyAllocation.TabStop = true;
            this.rdbReadOnlyAllocation.Text = "Read";
            this.rdbReadOnlyAllocation.UseVisualStyleBackColor = true;
            // 
            // grpRestrictedAllowedSecurities
            // 
            this.grpRestrictedAllowedSecurities.Controls.Add(this.rdbReadWriteRestrictedAllowedSecurities);
            this.grpRestrictedAllowedSecurities.Controls.Add(this.rdbReadOnlyRestrictedAllowedSecurities);
            this.grpRestrictedAllowedSecurities.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpRestrictedAllowedSecurities.Location = new System.Drawing.Point(508, 181);
            this.grpRestrictedAllowedSecurities.Name = "grpRestrictedAllowedSecurities";
            this.grpRestrictedAllowedSecurities.Size = new System.Drawing.Size(141, 86);
            this.grpRestrictedAllowedSecurities.TabIndex = 10;
            this.grpRestrictedAllowedSecurities.TabStop = false;
            this.grpRestrictedAllowedSecurities.Text = "Restricted/Allowed Securities List";
            // 
            // rdbReadWriteRestrictedAllowedSecurities
            // 
            this.rdbReadWriteRestrictedAllowedSecurities.AutoSize = true;
            this.rdbReadWriteRestrictedAllowedSecurities.Location = new System.Drawing.Point(16, 63);
            this.rdbReadWriteRestrictedAllowedSecurities.Name = "rdbReadWriteRestrictedAllowedSecurities";
            this.rdbReadWriteRestrictedAllowedSecurities.Size = new System.Drawing.Size(91, 17);
            this.rdbReadWriteRestrictedAllowedSecurities.TabIndex = 1;
            this.rdbReadWriteRestrictedAllowedSecurities.Text = "Read/Write";
            this.rdbReadWriteRestrictedAllowedSecurities.UseVisualStyleBackColor = true;
            // 
            // rdbReadOnlyRestrictedAllowedSecurities
            // 
            this.rdbReadOnlyRestrictedAllowedSecurities.AutoSize = true;
            this.rdbReadOnlyRestrictedAllowedSecurities.Checked = true;
            this.rdbReadOnlyRestrictedAllowedSecurities.Location = new System.Drawing.Point(17, 40);
            this.rdbReadOnlyRestrictedAllowedSecurities.Name = "rdbReadOnlyRestrictedAllowedSecurities";
            this.rdbReadOnlyRestrictedAllowedSecurities.Size = new System.Drawing.Size(54, 17);
            this.rdbReadOnlyRestrictedAllowedSecurities.TabIndex = 0;
            this.rdbReadOnlyRestrictedAllowedSecurities.TabStop = true;
            this.rdbReadOnlyRestrictedAllowedSecurities.Text = "Read";
            this.rdbReadOnlyRestrictedAllowedSecurities.UseVisualStyleBackColor = true;
            // 
            // chbOverridePermission
            // 
            this.chbOverridePermission.AutoSize = true;
            this.chbOverridePermission.Location = new System.Drawing.Point(14, 65);
            this.chbOverridePermission.Name = "chbOverridePermission";
            this.chbOverridePermission.Size = new System.Drawing.Size(126, 30);
            this.chbOverridePermission.TabIndex = 2;
            this.chbOverridePermission.Text = "All Rule Override  \r\nPermission";
            this.chbOverridePermission.UseVisualStyleBackColor = true;
            this.chbOverridePermission.Click += new System.EventHandler(this.chbOverridePermission_CheckedChanged);
            // 
            // chbPreTradeEnabled
            // 
            this.chbPreTradeEnabled.AutoSize = true;
            this.chbPreTradeEnabled.Location = new System.Drawing.Point(5, 17);
            this.chbPreTradeEnabled.Name = "chbPreTradeEnabled";
            this.chbPreTradeEnabled.Size = new System.Drawing.Size(118, 17);
            this.chbPreTradeEnabled.TabIndex = 0;
            this.chbPreTradeEnabled.Text = "Pre Trade Check";
            this.chbPreTradeEnabled.UseVisualStyleBackColor = true;
            this.chbPreTradeEnabled.Click += new System.EventHandler(this.chbPreTradeEnabled_CheckedChanged);
            // 
            // gbCompliance
            // 
            this.gbCompliance.Controls.Add(this.chbPowerUser);
            this.gbCompliance.Controls.Add(this.grpCompliancePostTrade);
            this.gbCompliance.Controls.Add(this.grpCompliancePreTrade);
            this.gbCompliance.Controls.Add(this.gbPreTrade);
            this.gbCompliance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCompliance.Location = new System.Drawing.Point(497, 175);
            this.gbCompliance.Name = "gbCompliance";
            this.gbCompliance.Size = new System.Drawing.Size(160, 538);
            this.gbCompliance.TabIndex = 9;
            this.gbCompliance.TabStop = false;
            this.gbCompliance.Text = "Compliance";
            // 
            // chbPowerUser
            // 
            this.chbPowerUser.AutoSize = true;
            this.chbPowerUser.Location = new System.Drawing.Point(11, 27);
            this.chbPowerUser.Name = "chbPowerUser";
            this.chbPowerUser.Size = new System.Drawing.Size(90, 17);
            this.chbPowerUser.TabIndex = 0;
            this.chbPowerUser.Text = "Power User";
            this.chbPowerUser.UseVisualStyleBackColor = true;
            // 
            // grpCompliancePostTrade
            // 
            this.grpCompliancePostTrade.Controls.Add(this.chckPostExport);
            this.grpCompliancePostTrade.Controls.Add(this.chckPostDelete);
            this.grpCompliancePostTrade.Controls.Add(this.chckPostCreate);
            this.grpCompliancePostTrade.Controls.Add(this.chckPostImport);
            this.grpCompliancePostTrade.Controls.Add(this.chckPostRename);
            this.grpCompliancePostTrade.Controls.Add(this.chckPostEnable);
            this.grpCompliancePostTrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompliancePostTrade.Location = new System.Drawing.Point(8, 394);
            this.grpCompliancePostTrade.Name = "grpCompliancePostTrade";
            this.grpCompliancePostTrade.Size = new System.Drawing.Size(146, 137);
            this.grpCompliancePostTrade.TabIndex = 3;
            this.grpCompliancePostTrade.TabStop = false;
            this.grpCompliancePostTrade.Text = "Post-Trade";
            // 
            // chckPostExport
            // 
            this.chckPostExport.AutoSize = true;
            this.chckPostExport.Location = new System.Drawing.Point(6, 114);
            this.chckPostExport.Name = "chckPostExport";
            this.chckPostExport.Size = new System.Drawing.Size(63, 17);
            this.chckPostExport.TabIndex = 5;
            this.chckPostExport.Text = "Export";
            this.chckPostExport.UseVisualStyleBackColor = true;
            // 
            // chckPostDelete
            // 
            this.chckPostDelete.AutoSize = true;
            this.chckPostDelete.Location = new System.Drawing.Point(6, 95);
            this.chckPostDelete.Name = "chckPostDelete";
            this.chckPostDelete.Size = new System.Drawing.Size(63, 17);
            this.chckPostDelete.TabIndex = 4;
            this.chckPostDelete.Text = "Delete";
            this.chckPostDelete.UseVisualStyleBackColor = true;
            // 
            // chckPostCreate
            // 
            this.chckPostCreate.AutoSize = true;
            this.chckPostCreate.Location = new System.Drawing.Point(6, 76);
            this.chckPostCreate.Name = "chckPostCreate";
            this.chckPostCreate.Size = new System.Drawing.Size(111, 17);
            this.chckPostCreate.TabIndex = 3;
            this.chckPostCreate.Text = "Create/Update";
            this.chckPostCreate.UseVisualStyleBackColor = true;
            // 
            // chckPostImport
            // 
            this.chckPostImport.AutoSize = true;
            this.chckPostImport.Location = new System.Drawing.Point(6, 56);
            this.chckPostImport.Name = "chckPostImport";
            this.chckPostImport.Size = new System.Drawing.Size(66, 17);
            this.chckPostImport.TabIndex = 2;
            this.chckPostImport.Text = "Import";
            this.chckPostImport.UseVisualStyleBackColor = true;
            // 
            // chckPostRename
            // 
            this.chckPostRename.AutoSize = true;
            this.chckPostRename.Location = new System.Drawing.Point(6, 37);
            this.chckPostRename.Name = "chckPostRename";
            this.chckPostRename.Size = new System.Drawing.Size(73, 17);
            this.chckPostRename.TabIndex = 1;
            this.chckPostRename.Text = "Rename";
            this.chckPostRename.UseVisualStyleBackColor = true;
            // 
            // chckPostEnable
            // 
            this.chckPostEnable.AutoSize = true;
            this.chckPostEnable.Location = new System.Drawing.Point(6, 18);
            this.chckPostEnable.Name = "chckPostEnable";
            this.chckPostEnable.Size = new System.Drawing.Size(110, 17);
            this.chckPostEnable.TabIndex = 0;
            this.chckPostEnable.Text = "Enable/Disable";
            this.chckPostEnable.UseVisualStyleBackColor = true;
            // 
            // grpCompliancePreTrade
            // 
            this.grpCompliancePreTrade.Controls.Add(this.chckPreExport);
            this.grpCompliancePreTrade.Controls.Add(this.chckPreDelete);
            this.grpCompliancePreTrade.Controls.Add(this.chckPreCreate);
            this.grpCompliancePreTrade.Controls.Add(this.chckPreImport);
            this.grpCompliancePreTrade.Controls.Add(this.chckPreRename);
            this.grpCompliancePreTrade.Controls.Add(this.chckPreEnable);
            this.grpCompliancePreTrade.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpCompliancePreTrade.Location = new System.Drawing.Point(8, 50);
            this.grpCompliancePreTrade.Name = "grpCompliancePreTrade";
            this.grpCompliancePreTrade.Size = new System.Drawing.Size(146, 139);
            this.grpCompliancePreTrade.TabIndex = 1;
            this.grpCompliancePreTrade.TabStop = false;
            this.grpCompliancePreTrade.Text = "Pre-Trade";
            // 
            // chckPreExport
            // 
            this.chckPreExport.AutoSize = true;
            this.chckPreExport.Location = new System.Drawing.Point(6, 116);
            this.chckPreExport.Name = "chckPreExport";
            this.chckPreExport.Size = new System.Drawing.Size(63, 17);
            this.chckPreExport.TabIndex = 5;
            this.chckPreExport.Text = "Export";
            this.chckPreExport.UseVisualStyleBackColor = true;
            // 
            // chckPreDelete
            // 
            this.chckPreDelete.AutoSize = true;
            this.chckPreDelete.Location = new System.Drawing.Point(6, 97);
            this.chckPreDelete.Name = "chckPreDelete";
            this.chckPreDelete.Size = new System.Drawing.Size(63, 17);
            this.chckPreDelete.TabIndex = 4;
            this.chckPreDelete.Text = "Delete";
            this.chckPreDelete.UseVisualStyleBackColor = true;
            // 
            // chckPreCreate
            // 
            this.chckPreCreate.AutoSize = true;
            this.chckPreCreate.Location = new System.Drawing.Point(6, 78);
            this.chckPreCreate.Name = "chckPreCreate";
            this.chckPreCreate.Size = new System.Drawing.Size(111, 17);
            this.chckPreCreate.TabIndex = 3;
            this.chckPreCreate.Text = "Create/Update";
            this.chckPreCreate.UseVisualStyleBackColor = true;
            // 
            // chckPreImport
            // 
            this.chckPreImport.AutoSize = true;
            this.chckPreImport.Location = new System.Drawing.Point(6, 58);
            this.chckPreImport.Name = "chckPreImport";
            this.chckPreImport.Size = new System.Drawing.Size(66, 17);
            this.chckPreImport.TabIndex = 2;
            this.chckPreImport.Text = "Import";
            this.chckPreImport.UseVisualStyleBackColor = true;
            // 
            // chckPreRename
            // 
            this.chckPreRename.AutoSize = true;
            this.chckPreRename.Location = new System.Drawing.Point(6, 38);
            this.chckPreRename.Name = "chckPreRename";
            this.chckPreRename.Size = new System.Drawing.Size(73, 17);
            this.chckPreRename.TabIndex = 1;
            this.chckPreRename.Text = "Rename";
            this.chckPreRename.UseVisualStyleBackColor = true;
            // 
            // chckPreEnable
            // 
            this.chckPreEnable.AutoSize = true;
            this.chckPreEnable.Location = new System.Drawing.Point(6, 18);
            this.chckPreEnable.Name = "chckPreEnable";
            this.chckPreEnable.Size = new System.Drawing.Size(110, 17);
            this.chckPreEnable.TabIndex = 0;
            this.chckPreEnable.Text = "Enable/Disable";
            this.chckPreEnable.UseVisualStyleBackColor = true;
            // 
            // gbPreTrade
            // 
            this.gbPreTrade.Controls.Add(this.groupBoxComplianceCheck);
            this.gbPreTrade.Controls.Add(this.ultraComEdORRulePermission);
            this.gbPreTrade.Controls.Add(this.chbApplyToManual);
            this.gbPreTrade.Controls.Add(this.chbOverridePermission);
            this.gbPreTrade.Controls.Add(this.chbPreTradeEnabled);
            this.gbPreTrade.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPreTrade.Location = new System.Drawing.Point(8, 189);
            this.gbPreTrade.Name = "gbPreTrade";
            this.gbPreTrade.Size = new System.Drawing.Size(146, 199);
            this.gbPreTrade.TabIndex = 2;
            this.gbPreTrade.TabStop = false;
            // 
            // groupBoxComplianceCheck
            // 
            this.groupBoxComplianceCheck.Controls.Add(this.checkBoxStaging);
            this.groupBoxComplianceCheck.Controls.Add(this.checkBoxTrading);
            this.groupBoxComplianceCheck.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBoxComplianceCheck.Location = new System.Drawing.Point(3, 128);
            this.groupBoxComplianceCheck.Name = "groupBoxComplianceCheck";
            this.groupBoxComplianceCheck.Size = new System.Drawing.Size(143, 64);
            this.groupBoxComplianceCheck.TabIndex = 4;
            this.groupBoxComplianceCheck.TabStop = false;
            this.groupBoxComplianceCheck.Text = "Check Compliance On";
            ultraToolTipInfo1.ToolTipText = "Check Compliance";
            this.ultraToolTipManager1.SetUltraToolTip(this.groupBoxComplianceCheck, ultraToolTipInfo1);
            // 
            // checkBoxStaging
            // 
            this.checkBoxStaging.AutoSize = true;
            this.checkBoxStaging.Location = new System.Drawing.Point(14, 37);
            this.checkBoxStaging.Name = "checkBoxStaging";
            this.checkBoxStaging.Size = new System.Drawing.Size(69, 17);
            this.checkBoxStaging.TabIndex = 1;
            this.checkBoxStaging.Text = "Staging";
            this.checkBoxStaging.UseVisualStyleBackColor = true;
            // 
            // checkBoxTrading
            // 
            this.checkBoxTrading.AutoSize = true;
            this.checkBoxTrading.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.checkBoxTrading.Location = new System.Drawing.Point(14, 18);
            this.checkBoxTrading.Name = "checkBoxTrading";
            this.checkBoxTrading.Size = new System.Drawing.Size(75, 18);
            this.checkBoxTrading.TabIndex = 0;
            this.checkBoxTrading.Text = "Trading";
            this.checkBoxTrading.UseVisualStyleBackColor = true;
            // 
            // ultraComEdORRulePermission
            // 
            this.ultraComEdORRulePermission.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
            this.ultraComEdORRulePermission.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.CheckedItems;
            this.ultraComEdORRulePermission.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;
            this.ultraComEdORRulePermission.CheckedListSettings.ListSeparator = ", ";
            this.ultraComEdORRulePermission.DropDownListWidth = -1;
            this.ultraComEdORRulePermission.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraComEdORRulePermission.Location = new System.Drawing.Point(13, 101);
            this.ultraComEdORRulePermission.Name = "ultraComEdORRulePermission";
            this.ultraComEdORRulePermission.Size = new System.Drawing.Size(106, 22);
            this.ultraComEdORRulePermission.TabIndex = 3;
            this.ultraComEdORRulePermission.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultraComEdORRulePermission_MouseEnterElement);
            // 
            // chbApplyToManual
            // 
            this.chbApplyToManual.AutoSize = true;
            this.chbApplyToManual.Location = new System.Drawing.Point(14, 42);
            this.chbApplyToManual.Name = "chbApplyToManual";
            this.chbApplyToManual.Size = new System.Drawing.Size(103, 17);
            this.chbApplyToManual.TabIndex = 1;
            this.chbApplyToManual.Text = "Manual Trade";
            this.chbApplyToManual.UseVisualStyleBackColor = true;
            // 
            // chkListAuditUsers
            // 
            this.chkListAuditUsers.FormattingEnabled = true;
            this.chkListAuditUsers.Location = new System.Drawing.Point(3, 34);
            this.chkListAuditUsers.Name = "chkListAuditUsers";
            this.chkListAuditUsers.Size = new System.Drawing.Size(232, 84);
            this.chkListAuditUsers.TabIndex = 0;
            // 
            // grpAudit
            // 
            this.grpAudit.Controls.Add(this.auditlist_selectallbox);
            this.grpAudit.Controls.Add(this.chkListAuditUsers);
            this.grpAudit.Location = new System.Drawing.Point(4, 556);
            this.grpAudit.Name = "grpAudit";
            this.grpAudit.Size = new System.Drawing.Size(244, 125);
            this.grpAudit.TabIndex = 6;
            this.grpAudit.TabStop = false;
            // 
            // auditlist_selectallbox
            // 
            this.auditlist_selectallbox.AutoSize = true;
            this.auditlist_selectallbox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.auditlist_selectallbox.Location = new System.Drawing.Point(5, 12);
            this.auditlist_selectallbox.Name = "auditlist_selectallbox";
            this.auditlist_selectallbox.Size = new System.Drawing.Size(212, 17);
            this.auditlist_selectallbox.TabIndex = 1;
            this.auditlist_selectallbox.Text = "Audit Users To Ommit In Log";
            this.auditlist_selectallbox.UseVisualStyleBackColor = true;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.livefeed_selectallbox);
            this.ultraGroupBox2.Controls.Add(this.chkMarketDataTypes);
            this.ultraGroupBox2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.ultraGroupBox2.Location = new System.Drawing.Point(251, 562);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(246, 119);
            this.ultraGroupBox2.TabIndex = 10;
            // 
            // livefeed_selectallbox
            // 
            this.livefeed_selectallbox.AutoSize = true;
            this.livefeed_selectallbox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.livefeed_selectallbox.Location = new System.Drawing.Point(9, 6);
            this.livefeed_selectallbox.Name = "livefeed_selectallbox";
            this.livefeed_selectallbox.Size = new System.Drawing.Size(146, 17);
            this.livefeed_selectallbox.TabIndex = 1;
            this.livefeed_selectallbox.Text = "Live Feed Data Types";
            this.livefeed_selectallbox.UseVisualStyleBackColor = true;
            // 
            // chkMarketDataTypes
            // 
            this.chkMarketDataTypes.CheckOnClick = true;
            this.chkMarketDataTypes.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chkMarketDataTypes.Location = new System.Drawing.Point(6, 28);
            this.chkMarketDataTypes.Name = "chkMarketDataTypes";
            this.chkMarketDataTypes.Size = new System.Drawing.Size(232, 84);
            this.chkMarketDataTypes.TabIndex = 0;
            //
            //ultraToolTipManager1
            // 
            this.ultraToolTipManager1.ContainingControl = this;
            // 
            // CompanyUserPermissions
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.grpAudit);
            this.Controls.Add(this.gbCompliance);
            this.Controls.Add(this.grpRestrictedAllowedSecurities);
            this.Controls.Add(this.grpAllocation);
            this.Controls.Add(this.grpPM);
            this.Controls.Add(this.grpStrategies);
            this.Controls.Add(this.grpAccounts);
            this.Controls.Add(this.grpCounterParties);
            this.Controls.Add(this.grpAssetUnderlying);
            this.Controls.Add(this.grpApplicationComponent);
            this.Controls.Add(this.ultraGroupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CompanyUserPermissions";
            this.Size = new System.Drawing.Size(660, 686);
            this.Load += new System.EventHandler(this.CompanyUserPermissions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpCounterParties)).EndInit();
            this.grpCounterParties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpApplicationComponent)).EndInit();
            this.grpApplicationComponent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAssetUnderlying)).EndInit();
            this.grpAssetUnderlying.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAccounts)).EndInit();
            this.grpAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpAllocationTradingAccounts)).EndInit();
            this.grpAllocationTradingAccounts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpStrategies)).EndInit();
            this.grpStrategies.ResumeLayout(false);
            this.grpPM.ResumeLayout(false);
            this.grpPM.PerformLayout();
            this.grpRestrictedAllowedSecurities.ResumeLayout(false);
            this.grpRestrictedAllowedSecurities.PerformLayout();
            this.grpAllocation.ResumeLayout(false);
            this.grpAllocation.PerformLayout();
            this.gbCompliance.ResumeLayout(false);
            this.gbCompliance.PerformLayout();
            this.grpCompliancePostTrade.ResumeLayout(false);
            this.grpCompliancePostTrade.PerformLayout();
            this.grpCompliancePreTrade.ResumeLayout(false);
            this.grpCompliancePreTrade.PerformLayout();
            this.gbPreTrade.ResumeLayout(false);
            this.gbPreTrade.PerformLayout();
            this.groupBoxComplianceCheck.ResumeLayout(false);
            this.groupBoxComplianceCheck.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComEdORRulePermission)).EndInit();
            this.grpAudit.ResumeLayout(false);
            this.grpAudit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion

        /// <summary>
        /// Company User Permission Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CompanyUserPermissions_Load(object sender, System.EventArgs e)
        {
            //Enabling/Disabling ComboBox. If OverRidden check Box checked then OverRiddenRulePermission ComboBox Disable otherwise Enable.
            try
            {
                if (chbPreTradeEnabled.Checked == true)
                {
                    if (chbOverridePermission.Checked == true)
                        ultraComEdORRulePermission.Enabled = false;
                    else
                        ultraComEdORRulePermission.Enabled = true;

                    //Enable the checkBoxTrading, checkBoxStaging, chbApplyToManual, chbOverridePermission If chbPreTradeEnabled checked
                    checkBoxTrading.Enabled = true;
                    checkBoxStaging.Enabled = true;
                    chbApplyToManual.Enabled = true;
                    chbOverridePermission.Enabled = true;
                }
                else
                {
                    ultraComEdORRulePermission.Enabled = false;

                    //Diable the checkBoxTrading, checkBoxStaging, chbApplyToManual, chbOverridePermission If chbPreTradeEnabled Unchecked
                    checkBoxTrading.Enabled = false;
                    checkBoxStaging.Enabled = false;
                    chbApplyToManual.Enabled = false;
                    chbOverridePermission.Enabled = false;
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

        #region Private methods Code Commented (not in use)

        //		private void BindCounterParties()
        //		{
        //			CounterParties counterParties = CounterPartyManager.GetCounterPartiesForCompanies();
        //			if(counterParties.Count > 0)
        //			{
        //				lstCounterParties.DataSource = counterParties;
        //				lstCounterParties.DisplayMember = "CounterPartyFullName";
        //				lstCounterParties.ValueMember = "CounterPartyID";
        //			}
        //		}		
        //
        //		private void BindComponents()
        //		{
        //			Modules modules = ModuleManager.GetModules();
        //			if(modules.Count > 0)
        //			{
        //				lstApplicationComponent.DataSource = modules;
        //				lstApplicationComponent.DisplayMember = "ModuleName";
        //				lstApplicationComponent.ValueMember = "ModuleID";
        //			}			
        //		}
        //
        //		private void BindAssetUnderlying()
        //		{
        //			UnderLyings underLyings = AssetManager.GetUnderLyings();
        //			if(underLyings.Count > 0)
        //			{
        //				System.Data.DataTable dt = new System.Data.DataTable();
        //				dt.Columns.Add("Data");
        //				dt.Columns.Add("Value");
        //				foreach(UnderLying underLying in underLyings)
        //				{
        //					object[] row = new object[2]; 
        //					row[0] = underLying.Asset + " : " + underLying.Name;
        //					row[1] = underLying.UnderlyingID;
        //					dt.Rows.Add(row);
        //				}
        //				lstAssetUnderLying.DataSource = dt; //underLyings;
        //				lstAssetUnderLying.DisplayMember = "Data";			
        //				lstAssetUnderLying.ValueMember = "Value";
        //			}
        //		}
        //
        //		
        //
        //		private void BindTradingAccount()
        //		{			
        //			TradingAccounts tradingAccounts = CompanyManager.GetTradingAccounts();
        //			if(tradingAccounts.Count > 0)
        //			{
        //				lstTradingAccount.DataSource = tradingAccounts;
        //				lstTradingAccount.DisplayMember = "TradingAccountName";
        //				lstTradingAccount.ValueMember = "TradingAccountsID";
        //			}
        //
        //		}		

        #endregion

        #region Public Properties Code Commented (not in use)
        //		public CounterParties CounterParties
        //		{
        //			get
        //			{
        //				return GetCounterParties();
        //			}
        //			set
        //			{
        //				SetCounterParties(value);
        //			}
        //		}

        //		public TradingAccounts TradingAccounts
        //		{
        //			get
        //			{
        //				return GetTradingAccounts();
        //			}
        //			set
        //			{
        //				SetTradingAccounts(value);
        //			}
        //		}
        //
        //		public Modules Modules
        //		{
        //			get
        //			{
        //				return GetModules();
        //			}
        //			set
        //			{
        //				SetModules(value);
        //			}
        //		}
        #endregion

        public void SetupControl(int companyID, int companyUserID, Modules companyUserModules)
        {
            _companyID = companyID;
            BindData(_companyID, companyUserID, companyUserModules);
        }

        public void BindData(int companyID, int companyUserID, Modules companyUserModules)
        {
            _marketDataTypeModules = null;

            SetCheckedCounterParties(companyID, companyUserID);
            //Modified By Faisal Shah
            //Dated 20/06/14
            SetAssetsUnderLyings(companyID, companyUserID);
            SetCheckedAssetsUnderLyings(companyID, companyUserID);
            // Modified by Ankit Gupta on 08 Oct, 2014.
            // http://jira.nirvanasolutions.com:8080/browse/CHMW-1494
            // Strategies List needs to be displayed for both release types.
            Dictionary<int, String> companyIDs = new Dictionary<int, string>();
            companyIDs = CompanyManager.GetUserPermittedCompanyList(companyUserID);
            SetCheckedStrategies(companyID, companyUserID, companyIDs);
            SetCheckedTradingAccounts(companyID, companyUserID, companyIDs);


            SetTradingAccounts(companyID, companyUserID);
            //  SetCheckedTradingAccounts(companyID, companyUserID);
            SetModules(companyID, companyUserID);

            SetCheckedModules(companyID, companyUserID, companyUserModules);


            SetCheckedAccounts(companyID, companyUserID);

            SetCheckedAllocationTradingAccounts(companyID, companyUserID);
            SetPreTradeCheckPowerUserAndOverridePermission(companyID, companyUserID);
            SetAuditTrailCheckedUsersToIgnore(companyID, companyUserID);
            SetMarketDataTypes(companyUserID);
            SetUserSecuritiesListPermission(companyUserID);

        }

        /// <summary>
        ///Checks the radio box for Restricted/Allowed securities list
        /// </summary>
        /// <param name="companyID"></param>
        private void SetUserSecuritiesListPermission(int companyUserID)
        {
            try
            {
                int readWritePermissionID = CompanyManager.GetUserSecuritiesListPermission(companyUserID);
                if (readWritePermissionID == 0)
                {
                    rdbReadOnlyRestrictedAllowedSecurities.Checked = true;
                }
                else
                {
                    rdbReadWriteRestrictedAllowedSecurities.Checked = true;
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
        /// ticks the user names in the list box for which the db aready has the ignored users
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        private void SetAuditTrailCheckedUsersToIgnore(int companyID, int companyUserID)
        {
            try
            {
                string ignoredUserIds = CompanyManager.GetIgnoredUserForAuditTrail(companyUserID, companyID);
                if (!String.IsNullOrEmpty(ignoredUserIds))
                {
                    foreach (string id in ignoredUserIds.Split(','))
                    {
                        foreach (User us in UserManager.GetUsers(companyID))
                        {
                            if (us.UserID == int.Parse(id))
                            {
                                chkListAuditUsers.SetItemChecked(chkListAuditUsers.Items.IndexOf(us.LoginName), true);
                            }
                        }
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
            checkedlistbox_SelectedValueChanged(this.chkListAuditUsers, null, this.auditlist_selectallbox);
        }
        #region CounterPartyVenues Get Set into the listboxes (Not Changed to Checked LIst box)
        //		public CounterPartyVenues GetCounterPartyVenues()
        //		{
        //			Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
        //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
        //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
        //			{
        //				System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)lstCounterParties.SelectedItems[i]).Row;
        //				int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
        //				int venueID = int.Parse(selectedRow["VenueID"].ToString());
        //				CounterParty cp = new CounterParty(counterPartyID, "");
        //				if(!counterParties.Contains(cp))
        //				{
        //					counterParties.Add(cp);
        //				}
        //				if(venueID > 0)
        //				{
        //					CounterPartyVenue cpv = new CounterPartyVenue();
        //					cpv.CounterPartyID = counterPartyID;
        //					cpv.VenueID = venueID;
        //					counterPartyVenues.Add(cpv);
        //				}
        //			}
        //			
        //			//			if(counterParties.Count > 0)
        //			//			{
        //			//				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
        //			//			}
        //			
        //			return counterPartyVenues;
        //
        //			
        //			
        //			#region Oldcode
        //			//			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
        //			//			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
        //			//			{
        //			//				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
        //			//			}
        //			//			return counterPartyVenues;
        //			#endregion
        //		}

        //		public CounterParties GetCounterParties()
        //		{
        //			Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
        //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
        //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
        //			{
        //				System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)lstCounterParties.SelectedItems[i]).Row;
        //				int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
        //				int venueID = int.Parse(selectedRow["VenueID"].ToString());
        //				CounterParty cp = new CounterParty(counterPartyID, "");
        //				if(!counterParties.Contains(cp))
        //				{
        //					counterParties.Add(cp);
        //				}
        //				if(venueID > 0)
        //				{
        //					CounterPartyVenue cpv = new CounterPartyVenue();
        //					cpv.CounterPartyID = counterPartyID;
        //					cpv.VenueID = venueID;
        //					counterPartyVenues.Add(cpv);
        //				}
        //			}

        //			if(counterParties.Count > 0)
        //			{
        //				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
        //			}

        //			return counterParties;



        #region Oldcode
        //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
        //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
        //			{
        //				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
        //			}
        //			return counterPartyVenues;
        #endregion
        //		}
        public CounterPartyVenues GetCheckedCounterPartyVenues()
        {
            Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            //next line needs change
            for (int i = 0, count = checkedlstCounterParties.CheckedItems.Count; i < count; i++)
            {
                System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCounterParties.CheckedItems[i]).Row;
                int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
                int venueID = int.Parse(selectedRow["VenueID"].ToString());

                CounterParty cp = new CounterParty(counterPartyID, "");
                if (!counterParties.Contains(cp))
                {
                    counterParties.Add(cp);
                }
                if (venueID > 0)
                {
                    CounterPartyVenue cpv = new CounterPartyVenue();
                    cpv.CounterPartyID = counterPartyID;
                    cpv.VenueID = venueID;
                    counterPartyVenues.Add(cpv);
                }
            }

            //			if(counterParties.Count > 0)
            //			{
            //				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
            //			}


            return counterPartyVenues;



            #region Oldcode
            //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
            //			{
            //				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
            //			}
            //			return counterPartyVenues;
            #endregion
        }

        public CounterParties GetCheckedCounterParties()
        {
            Prana.Admin.BLL.CounterParties counterParties = new CounterParties();
            CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            for (int i = 0, count = checkedlstCounterParties.CheckedItems.Count; i < count; i++)
            {
                System.Data.DataRow selectedRow = (System.Data.DataRow)((System.Data.DataRowView)checkedlstCounterParties.CheckedItems[i]).Row;
                int counterPartyID = int.Parse(selectedRow["CounterPartyID"].ToString());
                int venueID = int.Parse(selectedRow["VenueID"].ToString());
                CounterParty cp = new CounterParty(counterPartyID, "");
                if (!counterParties.Contains(cp))
                {
                    counterParties.Add(cp);
                }
                if (venueID > 0)
                {
                    CounterPartyVenue cpv = new CounterPartyVenue();
                    cpv.CounterPartyID = counterPartyID;
                    cpv.VenueID = venueID;
                    counterPartyVenues.Add(cpv);
                }
            }

            //			if(counterParties.Count > 0)
            //			{
            //				CounterPartyManager.SaveCompanyUserCounterParties(company.CompanyID, counterParties, userID);
            //			}

            return counterParties;



            #region Oldcode
            //			CounterPartyVenues counterPartyVenues = new CounterPartyVenues();
            //			for(int i=0, count = lstCounterParties.SelectedItems.Count; i<count; i++)
            //			{
            //				counterPartyVenues.Add((CounterPartyVenue)lstCounterParties.SelectedItems[i]);
            //			}
            //			return counterPartyVenues;
            #endregion
        }

        //		public void SetCounterParties(int companyID, int companyUserID)
        //		{
        //			lstCounterParties.Refresh();
        //			CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);
        //			CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
        //			
        //			CounterParties counterParties = CounterPartyManager.GetCompanyUserCounterParties(companyID, companyUserID);
        //			CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
        //		
        //			System.Data.DataTable tempDataTable = new System.Data.DataTable();
        //			tempDataTable.Columns.Add("DisplayData");
        //			tempDataTable.Columns.Add("CounterPartyID");
        //			tempDataTable.Columns.Add("VenueID");				
        //			foreach(CounterParty counterParty in counterParties)
        //			{
        //				System.Data.DataRow row = tempDataTable.NewRow();
        //				row["DisplayData"] = counterParty.CounterPartyFullName;
        //				row["CounterPartyID"] = counterParty.CounterPartyID;
        //				row["VenueID"] = int.MinValue;					
        //				tempDataTable.Rows.Add(row);
        //				foreach(Venue venue in counterParty.Venues)
        //				{
        //					if(CounterPartyManager.CheckExistingUserCounterPartyVenue(counterParty.CounterPartyID, venue.VenueID, companyID) == true)
        //					{
        //						row = tempDataTable.NewRow();
        //						row["DisplayData"] = "      " + venue.VenueName;
        //						row["CounterPartyID"] = counterParty.CounterPartyID;
        //						row["VenueID"] = venue.VenueID;						
        //						tempDataTable.Rows.Add(row);
        //					}
        //				}
        //			}
        //
        //			if(counterParties.Count > 0)
        //			{
        //				lstCounterParties.DataSource = null;
        //				lstCounterParties.Items.Clear();
        //				lstCounterParties.DataSource = tempDataTable;// counterParties;
        //				lstCounterParties.DisplayMember = "DisplayData";//"CounterPartyFullName";
        //				lstCounterParties.ValueMember = "CounterPartyID";//"CounterPartyID";						
        //			}
        //			else
        //			{
        //				lstCounterParties.DataSource = null;
        //			}
        //
        //			
        //			CounterParties userCounterParties = CounterPartyManager.GetCompanyUserCounterParties(companyID, companyUserID);
        //			CounterPartyVenues userCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
        //			
        //			lstCounterParties.SelectedIndex = -1;
        //			foreach(CounterParty userCounterParty in userCounterParties)
        //			{
        //				lstCounterParties.SelectedValue = userCounterParty.CounterPartyID;	
        //				
        //			}				
        //
        //			System.Data.DataTable dt = (System.Data.DataTable) lstCounterParties.DataSource;
        //			int rowIndex = 0;
        //			counterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
        //						if (dt.Rows.Count > 0)
        //						{
        //			foreach(CounterPartyVenue counterPartyVenue in counterPartyVenues)
        //			{
        //				rowIndex = 0;
        //				foreach(System.Data.DataRow row in dt.Rows)
        //				{
        //					if(int.Parse(row["CounterPartyID"].ToString()) == counterPartyVenue.CounterPartyID && int.Parse(row["VenueID"].ToString()) == counterPartyVenue.VenueID)
        //					{
        //						lstCounterParties.SelectedIndex = rowIndex;
        //					}
        //					rowIndex++;
        //				}
        //			}
        //						}
        //
        //
        //						if(companyUserID != int.MinValue)
        //						{
        //							CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
        //							foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
        //							{
        //								lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
        //							}
        //						}
        //
        //
        //			
        //			#region Old Code
        //						CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
        //						
        //						if(counterPartyVenues.Count > 0)
        //						{
        //							lstCounterParties.DataSource = counterPartyVenues;
        //							lstCounterParties.DisplayMember = "DisplayName";
        //							lstCounterParties.ValueMember = "CounterPartyVenueID";
        //							lstCounterParties.SelectedIndex = -1;
        //									
        //							if(companyUserID != int.MinValue)
        //							{
        //								CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
        //								foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
        //								{
        //									lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
        //								}
        //							}
        //						}
        //			#endregion
        //		}

        public void SetCheckedCounterParties(int companyID, int companyUserID)
        {
            //Have to use only the counterPartyVenues object as it has now all the info required including the counterparty.
            checkedlstCounterParties.Refresh();
            CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(companyID);
            CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
            BusinessObjects.Venue exAssignVenue = CachedDataManager.GetInstance.GetExerciseAssignVenue();

            System.Data.DataTable tempDataTable = new System.Data.DataTable();
            tempDataTable.Columns.Add("DisplayData");
            tempDataTable.Columns.Add("CounterPartyID");
            tempDataTable.Columns.Add("VenueID");
            foreach (CounterParty counterParty in counterParties)
            {
                System.Data.DataRow row = tempDataTable.NewRow();
                row["DisplayData"] = counterParty.CounterPartyFullName;
                row["CounterPartyID"] = counterParty.CounterPartyID;
                row["VenueID"] = int.MinValue;
                //row["CompanyCounterPartyCVID"] = int.MinValue;	
                tempDataTable.Rows.Add(row);
                foreach (Venue venue in counterParty.Venues)
                {
                    if ((exAssignVenue == null || venue.VenueID != exAssignVenue.VenueID) && CounterPartyManager.CheckExistingUserCounterPartyVenue(counterParty.CounterPartyID, venue.VenueID, companyID) == true)
                    {
                        row = tempDataTable.NewRow();
                        row["DisplayData"] = "      " + venue.VenueName;
                        row["CounterPartyID"] = counterParty.CounterPartyID;
                        row["VenueID"] = venue.VenueID;
                        tempDataTable.Rows.Add(row);
                    }
                }
            }

            if (counterParties.Count > 0)
            {
                checkedlstCounterParties.DataSource = null;
                checkedlstCounterParties.Items.Clear();
                checkedlstCounterParties.DataSource = tempDataTable;// counterParties;
                checkedlstCounterParties.DisplayMember = "DisplayData";//"CounterPartyFullName";
                checkedlstCounterParties.ValueMember = "CounterPartyID";//"CounterPartyID";						
            }
            else
            {
                checkedlstCounterParties.DataSource = null;
            }
            //Now T_CompanyCounterParties dropped.
            //CounterParties userCounterParties = CounterPartyManager.GetCompanyUserCounterParties(companyID, companyUserID);
            CounterPartyVenues userCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);

            checkedlstCounterParties.SelectedIndex = -1;
            //			for ( int j = 0; j < checkedlstCounterParties.Items.Count ; j++)
            //			{
            //				foreach(CounterParty userCounterParty in userCounterParties)
            //				{
            //					if ( ((((System.Data.DataRowView) checkedlstCounterParties.Items[j]).Row).ItemArray[1].ToString() == userCounterParty.CounterPartyID.ToString() )&& 
            //						((int.Parse((((System.Data.DataRowView) checkedlstCounterParties.Items[j]).Row).ItemArray[2].ToString()) < 0)))
            //					{
            //						checkedlstCounterParties.SetItemChecked(j, true);
            //					}
            //				}
            //			}

            for (int j = 0; j < checkedlstCounterParties.Items.Count; j++)
            {
                foreach (CounterPartyVenue counterPartyVenue in userCounterPartyVenues)
                {
                    if (((((System.Data.DataRowView)checkedlstCounterParties.Items[j]).Row).ItemArray[1].ToString() == counterPartyVenue.CounterPartyID.ToString()) &&
                        ((int.Parse((((System.Data.DataRowView)checkedlstCounterParties.Items[j]).Row).ItemArray[2].ToString()) < 0)))
                    {
                        checkedlstCounterParties.SetItemChecked(j, true);
                    }
                }
            }


            System.Data.DataTable dt = (System.Data.DataTable)checkedlstCounterParties.DataSource;

            int rowIndex = 0;
            userCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            foreach (CounterPartyVenue counterPartyVenue in userCounterPartyVenues)
            {
                rowIndex = 0;

                if (checkedlstCounterParties.Items.Count > 0)
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        if (int.Parse(row["CounterPartyID"].ToString()) == counterPartyVenue.CounterPartyID && int.Parse(row["VenueID"].ToString()) == counterPartyVenue.VenueID)
                        {
                            checkedlstCounterParties.SetItemChecked(rowIndex, true);
                        }
                        rowIndex++;
                    }
                }
                checkedlistbox_SelectedValueChanged(this.checkedlstCounterParties, null, this.brokervenue_selectallbox);
            }
            #region Old Code
            //			}`


            //			if(companyUserID != int.MinValue)
            //			{
            //				CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            //				foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
            //				{
            //					lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
            //				}
            //			}




            //			CounterPartyVenues counterPartyVenues = CounterPartyManager.GetCompanyCounterPartyVeneus(companyID);
            //			
            //			if(counterPartyVenues.Count > 0)
            //			{
            //				lstCounterParties.DataSource = counterPartyVenues;
            //				lstCounterParties.DisplayMember = "DisplayName";
            //				lstCounterParties.ValueMember = "CounterPartyVenueID";
            //				lstCounterParties.SelectedIndex = -1;
            //						
            //				if(companyUserID != int.MinValue)
            //				{
            //					CounterPartyVenues companyUserCounterPartyVenues = CounterPartyManager.GetCounterPartyVenuesForUser(companyUserID);
            //					foreach(CounterPartyVenue counterPartyVenue  in companyUserCounterPartyVenues)
            //					{
            //						lstCounterParties.SelectedValue = counterPartyVenue.CounterPartyVenueID;
            //					}
            //				}
            //			}
            #endregion
        }
        #endregion

        #region Trading Account Get Set
        /// <summary>
        /// This function is used to Get TradingTicket setting for a user. 
        /// We have changed the ListBox to a checkedListBox hence this is no more in use. 
        /// The ListBox is invisible and not yet deleted 
        /// </summary>
        /// <returns></returns>
        public TradingAccounts GetTradingAccounts()
        {
            TradingAccounts tradingAccounts = new TradingAccounts();
            //			for(int i=0, count = lstTradingAccount.SelectedItems.Count; i<count; i++)
            //			{
            //				tradingAccounts.Add((TradingAccount)lstTradingAccount.SelectedItems[i]);
            //			}
            return tradingAccounts;
        }
        /// <summary>
        ///  This function is used to Set TradingTicket setting for a user. 
        ///  We have changed the ListBox to a checkedListBox hence this is no more in use. 
        ///  The ListBox is invisible and not yet deleted
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetTradingAccounts(int companyID, int companyUserID)
        {
            //			lstTradingAccount.Refresh();
            //			TradingAccounts tradingaccounts = CompanyManager.GetTradingAccountsForCompany(companyID);
            //			if(tradingaccounts.Count > 0)
            //			{
            //				lstTradingAccount.DataSource = tradingaccounts;
            //				lstTradingAccount.DisplayMember = "TradingAccountName";
            //				lstTradingAccount.ValueMember = "TradingAccountsID";
            //			
            //				lstTradingAccount.SelectedIndex = -1;
            //				if(companyUserID != int.MinValue)
            //				{
            //					TradingAccounts companyUserTradingaccounts = CompanyManager.GetTradingAccountsForUser(companyUserID);
            //					foreach(TradingAccount tradingAccount in companyUserTradingaccounts)
            //					{
            //						lstTradingAccount.SelectedValue = tradingAccount.TradingAccountsID;
            //					}
            //				}
            //			}
            //			else
            //			{
            //				lstTradingAccount.DataSource = null;
            //			}
        }
        /// <summary>
        /// To Get Data from the CheckedListBox 
        /// </summary>
        /// <returns></returns>

        public TradingAccounts GetCheckedTradingAccounts()
        {
            TradingAccounts tradingAccounts = new TradingAccounts();
            try
            {
                //if (_releaseType != PranaReleaseViewType.CHMiddleWare)
                //{

                for (int i = 0, count = checkedlstTradingAccount.Items.Count; i < count; i++)
                {
                    if (checkedlstTradingAccount.GetItemChecked(i) == true)
                    {
                        checkedlstTradingAccount.SetSelected(i, true);
                        tradingAccounts.Add((TradingAccount)checkedlstTradingAccount.SelectedItem);
                    }
                }
                //}
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
            return tradingAccounts;
        }

        /// <summary>
        /// TO Set data to the CheckedList Box
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetCheckedTradingAccounts(int companyID, int companyUserID, Dictionary<int, String> companyIDs)
        {
            //Here we have used a foreach loop in foreach ... because i was unable 
            //to check the checkboxes in one go(As we have done with the selections in the case
            // case of a List box) Here we have the extra burden of checking each checkbox
            // corresponding to which the permission is set. which can be done only by sending 
            // index of each checkbox that is to be checked
            TradingAccounts tradingaccounts = new TradingAccounts();
            tradingaccounts = CompanyManager.GetTradingAccountsForCompany(companyID);

            if (tradingaccounts.Count > 0)
            {
                checkedlstTradingAccount.DataSource = tradingaccounts;
                checkedlstTradingAccount.DisplayMember = "TradingAccountName";
                checkedlstTradingAccount.ValueMember = "TradingAccountsID";
                checkedlstTradingAccount.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    TradingAccounts companyUserTradingaccounts = CompanyManager.GetTradingAccountsForUser(companyUserID);

                    for (int j = 0; j < tradingaccounts.Count; j++)
                    {
                        checkedlstTradingAccount.SetItemChecked(j, false);
                    }

                    // this is the new working code for checking the selected items
                    for (int j = 0; j < checkedlstTradingAccount.Items.Count; j++)
                    {
                        foreach (TradingAccount userTradingAccount in companyUserTradingaccounts)
                        {
                            if (((TradingAccount)checkedlstTradingAccount.Items[j]).TradingAccountsID == userTradingAccount.TradingAccountsID)
                            {
                                checkedlstTradingAccount.SetItemChecked(j, true);
                            }
                        }
                    }

                }
                else
                {
                    for (int j = 0; j < tradingaccounts.Count; j++)
                    {
                        checkedlstTradingAccount.SetItemChecked(j, false);
                    }
                }
            }
            else
            {
                checkedlstTradingAccount.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstTradingAccount, null, this.trading_selectallbox);
        }
        #endregion

        #region Modules Get Set
        /// <summary>
        /// This function is used to Get Module setting for a user. We have changed the ListBox to a checkedListBox hence this is no more in use. The ListBox is invisible and not yet deleted
        /// </summary>
        /// <returns></returns>
        public Modules GetModules()
        {
            Modules modules = new Modules();
            //			for(int i=0, count = lstApplicationComponent.SelectedItems.Count; i<count; i++)
            //			{				
            //				modules.Add((Module)lstApplicationComponent.SelectedItems[i]);
            //			}
            return modules;
        }

        /// <summary>
        /// This function is used to Set Module settings from the database into the List Box.
        ///  We have changed the ListBox to a checkedListBox hence this is no more in use. 
        ///  The ListBox is invisible and not yet deleted
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetModules(int companyID, int companyUserID)
        {
            //			lstApplicationComponent.Refresh();
            //			Modules modules = new Modules();
            //			modules = ModuleManager.GetModulesForCompany(companyID);
            //			if(modules.Count > 0)
            //			{
            //				lstApplicationComponent.DataSource = modules;
            //				lstApplicationComponent.DisplayMember = "ModuleName";
            //				lstApplicationComponent.ValueMember = "CompanyModuleID";
            //			
            //				lstApplicationComponent.SelectedIndex = -1;
            //				if(companyUserID != int.MinValue)
            //				{
            //					Modules userModules = ModuleManager.GetModulesForCompanyUser(companyUserID);
            //					foreach(Module module in userModules)
            //					{
            //						lstApplicationComponent.SelectedValue = module.ModuleID;
            //					}
            //				}
            //			}
            //			else
            //			{
            //				lstApplicationComponent.DataSource = null;
            //			}
        }
        /// <summary>
        /// Get Settings from a Checkedlist Box
        /// </summary>
        /// <returns></returns>
        public Modules GetCheckedModules()
        {
            int readWriteID = 0;
            bool exportCheck;
            Modules modules = new Modules();
            for (int i = 0, count = checkedlstApplicationComponent.CheckedItems.Count; i < count; i++)
            {
                //checkedlstApplicationComponent.SetSelected(i, true);
                Prana.Admin.BLL.Module module = new Prana.Admin.BLL.Module();
                if (((Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i]).ModuleID == POSITION_MANAGEMENT)
                {
                    bool abc = optionReadWritePM.Checked;
                    if (optionReadPM.Checked == true)
                    {
                        //break;
                        readWriteID = 0;
                    }
                    else
                    {
                        readWriteID = 1;
                    }
                    // Check if export is checked in grpPM
                    if (chbExportPM.Checked == true)
                    {
                        exportCheck = true;
                    }
                    else
                    {
                        exportCheck = false;
                    }

                    module = (Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i];
                    module.ReadWriteID = readWriteID;
                    module.IsShowExport = exportCheck;
                    modules.Add(module);
                }
                else if (((Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i]).ModuleID == ALLOCATION)
                {
                    if (rdbReadOnlyAllocation.Checked == true)
                    {
                        readWriteID = 0;
                    }
                    else
                    {
                        readWriteID = 1;
                    }
                    module = (Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i];
                    module.ReadWriteID = readWriteID;
                    modules.Add(module);
                }
                //Set read write for module as 1 as handled in seperate table
                else if (((Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i]).ModuleID == COMPLIANCEPRETRADE)
                {

                    //if (rdbPreRead.Checked == true)
                    //{
                    //    readWriteID = 0;
                    //}
                    //else
                    //{
                    //    readWriteID = 1;
                    //}
                    module = (Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i];
                    module.ReadWriteID = 1;
                    modules.Add(module);
                }


                //Set read write for module as 1 as handled in seperate table
                else if (((Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i]).ModuleID == COMPLIANCEPOSTTRADE)
                {
                    //if (rdbPostRead.Checked == true)
                    //{
                    //    readWriteID = 0;
                    //}
                    //else
                    //{
                    //    readWriteID = 1;
                    //}
                    module = (Prana.Admin.BLL.Module)checkedlstApplicationComponent.CheckedItems[i];
                    module.ReadWriteID = 1;
                    modules.Add(module);
                }
                else
                {
                    modules.Add((Module)checkedlstApplicationComponent.CheckedItems[i]);
                }


            }
            return modules;
        }
        /// <summary>
        /// Set Data to a checkedListBox
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        bool _pmReadWritePermission = false;
        // bool _compliancePreReadWritePermission = false;
        // bool _compliancePostReadWritePermission = false;
        public void SetCheckedModules(int companyID, int companyUserID, Modules companyUserModules)
        {
            //			lstApplicationComponent.Refresh();
            Modules modules = new Modules();
            modules = ModuleManager.GetModulesForCompany(companyID);
            if (modules.Count > 0)
            {
                checkedlstApplicationComponent.DataSource = modules;
                checkedlstApplicationComponent.DisplayMember = "ModuleName";
                checkedlstApplicationComponent.ValueMember = "CompanyModuleID";
                checkedlstApplicationComponent.SelectedIndex = -1;



                if (companyUserID != int.MinValue)
                {
                    // Modules companyUserModules = ModuleManager.GetModulesForCompanyUser(companyUserID);
                    int i = 0;

                    //Here we have used a foreach loop in foreach ... because i was unable 
                    //to check the checkboxes in one go(As we have done with the selections in the case
                    // case of a List box) Here we have the extra burden of checking each checkbox
                    // corresponding to which the permission is set. which can be done only by sending 
                    // index of each checkbox that is to be checked
                    grpPM.Visible = false;
                    grpPM.Enabled = false;
                    grpAllocation.Visible = false;
                    grpAllocation.Enabled = false;
                    grpCompliancePostTrade.Visible = false;
                    grpCompliancePostTrade.Enabled = false;
                    grpCompliancePreTrade.Visible = false;
                    grpCompliancePreTrade.Enabled = false;
                    gbPreTrade.Enabled = false;
                    gbPreTrade.Visible = false;
                    _pmReadWritePermission = false;
                    gbCompliance.Visible = false;
                    grpAudit.Visible = false;
                    for (int j = 0; j < modules.Count; j++)
                    {
                        checkedlstApplicationComponent.SetItemChecked(j, false);
                    }


                    if (companyUserModules.Count > 0)
                    {
                        foreach (Module module in modules)
                        {
                            if (module.ModuleID == POSITION_MANAGEMENT)
                            {
                                grpPM.Visible = true;
                            }

                            if (module.ModuleID == ALLOCATION)
                            {
                                grpAllocation.Visible = true;
                            }
                            // compare module id to module if then visibilty of grpCompliancePreTrade is true
                            if (module.ModuleID == COMPLIANCEPRETRADE)
                            {
                                //gbCompliance.Visible = true;
                                //gbPreTrade.Visible = true;
                                //grpCompliancePreTrade.Enabled=true;
                            }

                            // compare module id to module if then visibilty of  grpCompliancePostTrade is true
                            if (module.ModuleID == COMPLIANCEPOSTTRADE)
                            {
                                //gbCompliance.Visible = true;
                                //grpCompliancePostTrade.Enabled = true;
                            }
                            if (module.ModuleID == AUDITTRAIL)
                            {
                                grpAudit.Visible = true;
                                Users tempUsers = UserManager.GetUsers(companyID);
                                List<string> loginNames = new List<string>();
                                foreach (User user in tempUsers)
                                {
                                    loginNames.Add(user.LoginName);
                                }
                                chkListAuditUsers.Items.Clear();
                                chkListAuditUsers.Items.AddRange(loginNames.ToArray());
                                chkListAuditUsers.CheckOnClick = true;

                            }
                            foreach (Module userModule in companyUserModules)
                            {
                                if (module.CompanyModuleID == userModule.CompanyModuleID)
                                {
                                    checkedlstApplicationComponent.SelectedValue = module.CompanyModuleID;
                                    checkedlstApplicationComponent.SetSelected(i, true);
                                    checkedlstApplicationComponent.SetItemChecked(i, true);

                                    if (module.ModuleID == POSITION_MANAGEMENT)
                                    {
                                        grpPM.Visible = true;
                                        if (module.ReadWriteID == 1)
                                        {
                                            grpPM.Enabled = true;
                                            _pmReadWritePermission = true;
                                            if (userModule.ReadWriteID == TRUE)
                                            {
                                                optionReadWritePM.Checked = true;
                                            }
                                            else
                                            {
                                                optionReadPM.Checked = true;
                                            }
                                        }
                                        else
                                        {
                                            grpPM.Enabled = false;
                                            optionReadPM.Checked = true;
                                            _pmReadWritePermission = false;
                                        }
                                        this.chbExportPM.Checked = userModule.IsShowExport;
                                    }
                                    // add new module COMPLIANCEPRETRADE and set permission of read-write for Compliance alerting

                                    if (module.ModuleID == COMPLIANCEPRETRADE)
                                    {
                                        grpCompliancePreTrade.Visible = true;
                                        if (module.ReadWriteID == 1)
                                        {
                                            grpCompliancePreTrade.Enabled = true;
                                            gbPreTrade.Enabled = true;
                                            //_compliancePreReadWritePermission = true;
                                            if (userModule.ReadWriteID == TRUE)
                                            {
                                                //rdbPreReadWrite.Checked = true;
                                            }
                                            else
                                            {
                                                //rdbPreRead.Checked = true;
                                            }
                                        }
                                        else
                                        {
                                            grpCompliancePreTrade.Enabled = false;
                                            // rdbPreRead.Checked = true;
                                            //_compliancePreReadWritePermission = false;
                                        }
                                    }

                                    // add new module COMPLIANCEPOSTTRADE and set permission of read-write for Compliance alerting


                                    if (module.ModuleID == COMPLIANCEPOSTTRADE)
                                    {
                                        grpCompliancePostTrade.Visible = true;
                                        if (module.ReadWriteID == 1)
                                        {
                                            grpCompliancePostTrade.Enabled = true;
                                            //_compliancePostReadWritePermission = true;
                                            if (userModule.ReadWriteID == TRUE)
                                            {
                                                //rdbPostReadWrite.Checked = true;
                                            }
                                            else
                                            {
                                                // rdbPostRead.Checked = true;
                                            }
                                        }
                                        else
                                        {
                                            grpCompliancePostTrade.Enabled = false;
                                            // rdbPostRead.Checked = true;
                                            //_compliancePostReadWritePermission = false;
                                        }
                                    }


                                    if (module.ModuleID == ALLOCATION)
                                    {
                                        grpAllocation.Visible = true;
                                        grpAllocation.Enabled = true;
                                        if (userModule.ReadWriteID == TRUE)
                                        {
                                            rdbReadWriteAllocation.Checked = true;
                                        }
                                        else
                                        {
                                            rdbReadOnlyAllocation.Checked = true;
                                        }
                                    }

                                }
                            }
                            i++;
                        }
                    }

                }
            }
            else
            {
                checkedlstApplicationComponent.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstApplicationComponent, null, this.appcomp_selectallbox);
        }

        #endregion

        #region Accounts Get Set
        public void SetCheckedAccounts(int companyID, int companyUserID)
        {
            Accounts companyAccounts = new Accounts();
            companyAccounts = CompanyManager.GetAccount(companyID);
            if (companyAccounts.Count > 0)
            {
                checkedlstAccounts.DataSource = companyAccounts;
                checkedlstAccounts.DisplayMember = "AccountName";
                //checkedlstAccounts.ValueMember = "AccountID";
                checkedlstAccounts.ValueMember = "CompanyAccountID";
                checkedlstAccounts.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    Accounts companyUserAccounts = CompanyManager.GetAccountsForCompanyUser(companyUserID);
                    int i = 0;

                    //Here we have used a foreach loop in foreach ... because i was unable 
                    //to check the checkboxes in one go(As we have done with the selections in the case
                    // case of a List box) Here we have the extra burden of checking each checkbox
                    // corresponding to which the permission is set. which can be done only by sending 
                    // index of each checkbox that is to be checked

                    for (int j = 0; j < companyAccounts.Count; j++)
                    {
                        checkedlstAccounts.SetItemChecked(j, false);
                    }

                    foreach (Account companyAccount in companyAccounts)
                    {
                        foreach (Account companyUserAccount in companyUserAccounts)
                        {
                            //if (companyAccount.AccountID == companyUserAccount.CompanyAccountID)
                            if (companyAccount.CompanyAccountID == companyUserAccount.CompanyAccountID)
                            {
                                checkedlstAccounts.SelectedValue = companyAccount.CompanyAccountID;
                                checkedlstAccounts.SetSelected(i, true);
                                checkedlstAccounts.SetItemChecked(i, true);
                            }
                        }
                        i++;
                    }
                }
            }
            else
            {
                checkedlstAccounts.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstAccounts, null, this.account_selectallbox);
        }

        public Accounts GetCheckedAccounts()
        {
            Accounts companyUserAccounts = new Accounts();
            for (int i = 0, count = checkedlstAccounts.Items.Count; i < count; i++)
            {
                if (checkedlstAccounts.GetItemChecked(i) == true)
                {
                    checkedlstAccounts.SetSelected(i, true);
                    companyUserAccounts.Add((Account)checkedlstAccounts.SelectedItem);
                }
            }
            return companyUserAccounts;
        }
        #endregion

        #region Strategies Get Set
        public void SetCheckedStrategies(int companyID, int companyUserID, Dictionary<int, String> companyIDs)
        {
            Strategies companyStrategies = new Strategies();
            companyStrategies = CompanyManager.GetStrategy(companyID);

            if (companyStrategies.Count > 0)
            {
                checkedlstStrategies.DataSource = companyStrategies;
                checkedlstStrategies.DisplayMember = "StrategyName";
                checkedlstStrategies.ValueMember = "StrategyID";
                checkedlstStrategies.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    Strategies companyUserStrategies = CompanyManager.GetStrategiesForCompanyUser(companyUserID);
                    int i = 0;

                    //Here we have used a foreach loop in foreach ... because i was unable 
                    //to check the checkboxes in one go(As we have done with the selections in the case
                    // case of a List box) Here we have the extra burden of checking each checkbox
                    // corresponding to which the permission is set. which can be done only by sending 
                    // index of each checkbox that is to be checked

                    for (int j = 0; j < companyStrategies.Count; j++)
                    {
                        checkedlstStrategies.SetItemChecked(j, false);
                    }

                    foreach (Strategy companyStrategy in companyStrategies)
                    {
                        foreach (Strategy companyUserStrategy in companyUserStrategies)
                        {
                            if (companyStrategy.StrategyID == companyUserStrategy.CompanyStrategyID)
                            {
                                checkedlstStrategies.SelectedValue = companyStrategy.CompanyStrategyID;
                                checkedlstStrategies.SetSelected(i, true);
                                checkedlstStrategies.SetItemChecked(i, true);
                            }
                        }
                        i++;
                    }
                }
            }
            else
            {
                checkedlstStrategies.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstStrategies, null, this.strategy_selectallbox);
        }

        public Strategies GetCheckedStrategies()
        {
            Strategies companyUserStrategies = new Strategies();
            for (int i = 0, count = checkedlstStrategies.Items.Count; i < count; i++)
            {
                if (checkedlstStrategies.GetItemChecked(i) == true)
                {
                    checkedlstStrategies.SetSelected(i, true);
                    companyUserStrategies.Add((Strategy)checkedlstStrategies.SelectedItem);
                }
            }
            return companyUserStrategies;
        }
        #endregion

        #region Asset Underlying Get Set
        /// <summary>
        ///  This function is used to Get Underlyings setting for a user. 
        ///  We have changed the ListBox to a checkedListBox hence this is no more in use. 
        ///  The ListBox is invisible and not yet deleted
        /// </summary>
        /// <returns></returns>
        public UnderLyings GetUnderlyings()
        {
            UnderLyings underLyings = new UnderLyings();
            //			for(int i=0, count = lstAssetUnderLying.SelectedItems.Count; i<count; i++)
            //			{
            //				underLyings.Add(new UnderLying(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
            //			}
            return underLyings;
        }
        /// <summary>
        ///  This function is used to Get Assets setting for a user. 
        ///  We have changed the ListBox to a checkedListBox hence this is no more in use. 
        ///  The ListBox is invisible and not yet deleted
        /// </summary>
        /// <returns></returns>
        public Assets GetAssets()
        {
            Assets assets = new Assets();
            //			for(int i=0, count = lstAssetUnderLying.SelectedItems.Count; i<count; i++)
            //			{
            //				assets.Add(new Asset(int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[1].ToString()), ((System.Data.DataRow)(((System.Data.DataRowView)((lstAssetUnderLying.SelectedItems[i]))).Row)).ItemArray[0].ToString()));
            //			}	
            return assets;
        }
        /// <summary>
        /// This is the function in use now. To Get User setting from the CheckedLIstBox
        /// 
        /// </summary>
        /// <returns></returns>
        public UnderLyings GetCheckedUnderlyings()
        {
            UnderLyings underLyings = new UnderLyings();
            for (int i = 0, count = checkedlstAssetUnderLying.Items.Count; i < count; i++)
            {
                if (checkedlstAssetUnderLying.GetItemChecked(i) == true)
                {
                    checkedlstAssetUnderLying.SetSelected(i, true);
                    underLyings.Add(new UnderLying(int.Parse(((System.Data.DataRow)((System.Data.DataRowView)
                        (checkedlstAssetUnderLying.SelectedItem)).Row).ItemArray[1].ToString()), (((System.Data.DataRow)((System.Data.DataRowView)
                        (checkedlstAssetUnderLying.SelectedItem)).Row).ItemArray[1].ToString())));
                }
            }
            return underLyings;
        }

        /// <summary>
        /// This is the function in use now. To Get User setting from the CheckedLIstBox
        /// 
        /// </summary>
        /// <returns></returns>
        public AUECs GetCheckedAssets()
        {
            AUECs companyUserAUECs = new AUECs();
            // int companyAUECID = int.MinValue;
            //for(int i=0, count = checkedlstAssetUnderLying.Items.Count; i<count; i++)
            for (int i = 0, count = checkedlstAssetUnderLying.CheckedItems.Count; i < count; i++)
            {
                //if (checkedlstAssetUnderLying.GetItemChecked(i) == true)
                //if (checkedlstAssetUnderLying.CheckedItems[i] == true)
                //{
                AUEC companyUserAUEC = new AUEC();
                //checkedlstAssetUnderLying.SetSelected(i, true);
                //					assets.Add(new Asset(int.Parse(((System.Data.DataRow)((System.Data.DataRowView)
                //						(checkedlstAssetUnderLying.SelectedItem)).Row).ItemArray[1].ToString()),(((System.Data.DataRow)((System.Data.DataRowView)
                //						(checkedlstAssetUnderLying.SelectedItem)).Row).ItemArray[1].ToString())  ));
                //string itemText = string.Empty;
                //itemText = checkedlstAssetUnderLying.GetItemText(checkedlstAssetUnderLying.Items[i]);
                //MessageBox.Show("Item: " + itemText);
                //companyAUECID = int.Parse(((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAssetUnderLying.CheckedItems[i]))).Row)).ItemArray[1].ToString());
                //companyUserAUEC.CompanyAUECID = companyAUECID;
                System.Data.DataRow selectedRow = ((System.Data.DataRow)(((System.Data.DataRowView)((checkedlstAssetUnderLying.CheckedItems[i]))).Row));
                companyUserAUEC.CompanyAUECID = int.Parse(selectedRow["value"].ToString());
                companyUserAUECs.Add(companyUserAUEC);

                //}
            }
            return companyUserAUECs;
        }

        public void refreshAuecValues()
        {
            try
            {
                for (int i = 0, count = checkedlstAssetUnderLying.Items.Count; i < count; i++)
                {
                    checkedlstAssetUnderLying.SetItemChecked(i, true);
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
        ///  This function is used to Set Assets setting for a user from the Database. 
        ///  We have changed the ListBox to a checkedListBox hence this is no more in use. 
        ///  The ListBox is invisible and not yet deleted
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetAssetsUnderLyings(int companyID, int companyUserID)
        {
            //			lstAssetUnderLying.Refresh();
            //			
            //			UnderLyings underLyings = AssetManager.GetCompanyUnderLyings(companyID);
            //			if(underLyings.Count > 0)
            //			{
            //				System.Data.DataTable dt = new System.Data.DataTable();
            //				dt.Columns.Add("Data");
            //				dt.Columns.Add("Value");
            //				foreach(UnderLying underLying in underLyings)
            //				{
            //					object[] row = new object[2]; 
            //					row[0] = underLying.Asset + " : " + underLying.Name;
            //					row[1] = underLying.UnderlyingID;
            //					dt.Rows.Add(row);
            //				}
            //				lstAssetUnderLying.DataSource = dt; //underLyings;
            //				lstAssetUnderLying.DisplayMember = "Data";			
            //				lstAssetUnderLying.ValueMember = "Value";				
            //			
            //				lstAssetUnderLying.SelectedIndex = -1;
            //				if(companyUserID != int.MinValue)
            //				{
            //					UnderLyings companyUserUnderLyings = AssetManager.GetCompanyUserUnderLyings(companyUserID);
            //					foreach (UnderLying underLying in companyUserUnderLyings)
            //					{
            //						lstAssetUnderLying.SelectedValue = underLying.UnderlyingID;
            //					}
            //				}
            //			}
            //			else
            //			{
            //				lstAssetUnderLying.DataSource = null;
            //			}
        }
        /// <summary>
        /// To set Data into the Checked List Box
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetCheckedAssetsUnderLyings(int companyID, int companyUserID)
        {
            //Here we have used a foreach loop in foreach ... because i was unable 
            //to check the checkboxes in one go(As we have done with the selections in the case
            // case of a List box) Here we have the extra burden of checking each checkbox
            // corresponding to which the permission is set. which can be done only by sending 
            // index of each checkbox that is to be checked
            checkedlstAssetUnderLying.Refresh();

            //			UnderLyings underLyings = AssetManager.GetCompanyUnderLyings(companyID);
            //			if(underLyings.Count > 0)
            //			{
            //				System.Data.DataTable dt = new System.Data.DataTable();
            //				dt.Columns.Add("Data");
            //				dt.Columns.Add("Value");
            //				foreach(UnderLying underLying in underLyings)
            //				{
            //					object[] row = new object[2]; 
            //					row[0] = underLying.Asset + " : " + underLying.Name;
            //					row[1] = underLying.UnderlyingID;
            //					dt.Rows.Add(row);
            //				}
            //
            //
            //
            //				checkedlstAssetUnderLying.DataSource = dt;
            //				checkedlstAssetUnderLying.DisplayMember = "Data";			
            //				checkedlstAssetUnderLying.ValueMember = "Value";				
            //				checkedlstAssetUnderLying.SelectedIndex = -1;

            AUECs companyAUECs = CompanyManager.GetCompanyAUECs(companyID);

            System.Data.DataTable dtauec = new System.Data.DataTable();
            dtauec.Columns.Add("Data");
            dtauec.Columns.Add("Value");
            object[] rowAUEC = new object[2];
            if (companyAUECs.Count > 0)
            {
                foreach (Prana.Admin.BLL.AUEC auec in companyAUECs)
                {
                    //					Exchange exchange = auec.Exchange;
                    //Currency currency = new Currency();
                    string Data = "";
                    //					if (exchange != null)
                    //					{
                    //currency = AUECManager.GetCurrency(auec.Exchange.Currency);
                    //currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
                    //SK 2061009 removed Compliance class
                    //Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencySymbol.ToString();
                    Data = auec.AUECString;
                    //
                    //					}
                    //					else
                    //					{
                    //						Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString(); //+ "/" + auec.Exchange.DisplayName.ToString();
                    //					}
                    int Value = auec.CompanyAUECID;

                    rowAUEC[0] = Data;
                    rowAUEC[1] = Value;
                    dtauec.Rows.Add(rowAUEC);
                }
                checkedlstAssetUnderLying.DataSource = dtauec;
                checkedlstAssetUnderLying.DisplayMember = "Data";
                checkedlstAssetUnderLying.ValueMember = "Value";

                checkedlstAssetUnderLying.ClearSelected();
                checkedlstAssetUnderLying.SelectedItems.Clear();
                if (companyUserID != int.MinValue)
                {
                    AUECs companyUserAUECs = CompanyManager.GetCompanyUserAUECs(companyUserID);
                    int i = 0;
                    //Here we have used a foreach loop in foreach ... because i was unable 
                    //to check the checkboxes in one go(As we have done with the selections in the case
                    // case of a List box) Here we have the extra burden of checking each checkbox
                    // corresponding to which the permission is set. which can be done only by sending 
                    // index of each checkbox that is to be checked

                    for (int j = 0; j < companyAUECs.Count; j++)
                    {
                        checkedlstAssetUnderLying.SetItemChecked(j, false);
                    }
                    if (companyUserAUECs.Count > 0)
                    {
                        foreach (AUEC companyAUEC in companyAUECs)
                        {
                            foreach (AUEC companyUserAUEC in companyUserAUECs)
                            {
                                if (companyAUEC.CompanyAUECID == companyUserAUEC.CompanyAUECID)
                                {
                                    checkedlstAssetUnderLying.SelectedValue = companyUserAUEC.CompanyAUECID;
                                    checkedlstAssetUnderLying.SetSelected(i, true);
                                    checkedlstAssetUnderLying.SetItemChecked(i, true);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            else
            {
                checkedlstAssetUnderLying.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.checkedlstAssetUnderLying, null, this.auec_selectallbox);
        }
        #endregion

        /// <summary>
        /// save preTradeCheck, override  permission, Power User,Enable, update, delete etc.  Check box  on UI and taking value  from data Base
        /// </summary>
        /// <param name="companyID">It is 6</param>
        /// <param name="companyUserID">Id of Select User</param>

        public void SetPreTradeCheckPowerUserAndOverridePermission(int companyID, int companyUserID)
        {
            try
            {
                CompliancePermissions dict = CompanyManager.GetPermmissionForUser(companyUserID, companyID);
                if (dict != null)
                {
                    chbPowerUser.Checked = dict.IsPowerUser;

                    chbOverridePermission.Checked = dict.RuleCheckPermission.IsOverridePermission;
                    chbPreTradeEnabled.Checked = dict.RuleCheckPermission.IsPreTradeEnabled;
                    chbApplyToManual.Checked = dict.RuleCheckPermission.IsApplyToManual;
                    checkBoxTrading.Checked = dict.RuleCheckPermission.IsTrading;
                    checkBoxStaging.Checked = dict.RuleCheckPermission.IsStaging;

                    chckPostCreate.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsCreate;
                    chckPostDelete.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsDelete;
                    chckPostEnable.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsEnable;
                    chckPostExport.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsExport;
                    chckPostImport.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsImport;
                    chckPostRename.Checked = dict.complianceUIPermissions[RuleType.PostTrade].IsRename;

                    chckPreCreate.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsCreate;
                    chckPreDelete.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsDelete;
                    chckPreEnable.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsEnable;
                    chckPreExport.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsExport;
                    chckPreImport.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsImport;
                    chckPreRename.Checked = dict.complianceUIPermissions[RuleType.PreTrade].IsRename;

                    ultraComEdORRulePermission.DataSource = null;
                    ultraComEdORRulePermission.DataSource = dict.RuleLevelPermission;
                    ultraComEdORRulePermission.DisplayMember = "RuleName";
                    ultraComEdORRulePermission.ValueMember = "RuleId";

                    //Cheking checkbox of Rule OverRidden Permission in ComboBox, If user have individual Rule Permission
                    foreach (ValueListItem item in ultraComEdORRulePermission.Items)
                    {
                        item.CheckState = (item.ListObject as RuleLevelPermission).OverridePermission ? CheckState.Checked : CheckState.Unchecked;
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

        #region AllocationTradingAccounts Get Set
        /// <summary>
        /// TO Set data to the CheckedList Box
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        public void SetCheckedAllocationTradingAccounts(int companyID, int companyUserID)
        {
            //Here we have used a foreach loop in foreach ... because i was unable 
            //to check the checkboxes in one go(As we have done with the selections in the case
            // case of a List box) Here we have the extra burden of checking each checkbox
            // corresponding to which the permission is set. which can be done only by sending 
            // index of each checkbox that is to be checked

            TradingAccounts tradingaccounts = CompanyManager.GetTradingAccountsForCompany(companyID);
            if (tradingaccounts.Count > 0)
            {
                checkedlistAllocationTradingAccounts.DataSource = tradingaccounts;
                checkedlistAllocationTradingAccounts.DisplayMember = "TradingAccountName";
                checkedlistAllocationTradingAccounts.ValueMember = "TradingAccountsID"; //In actual it is the CompanyTradingAccountID
                checkedlistAllocationTradingAccounts.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    TradingAccounts companyUserAllocationTradingAccounts = CompanyManager.GetAllocationTradingAccountsForUser(companyUserID);

                    //Here we have used a foreach loop in foreach ... because i was unable 
                    //to check the checkboxes in one go(As we have done with the selections in the case
                    // case of a List box) Here we have the extra burden of checking each checkbox
                    // corresponding to which the permission is set. which can be done only by sending 
                    // index of each checkbox that is to be checked

                    // this is the new working code for checking the selected items
                    for (int j = 0; j < checkedlistAllocationTradingAccounts.Items.Count; j++)
                    {
                        foreach (TradingAccount userAllocationTradingAccount in companyUserAllocationTradingAccounts)
                        {
                            if (((TradingAccount)checkedlistAllocationTradingAccounts.Items[j]).TradingAccountsID == userAllocationTradingAccount.TradingAccountsID)
                            {
                                checkedlistAllocationTradingAccounts.SetItemChecked(j, true);
                            }
                        }
                    }

                }
            }
            else
            {
                checkedlistAllocationTradingAccounts.DataSource = null;
            }
        }

        /// <summary>
        /// gets the checked users from the chkListAuditUsers and converts them to a comma separated userids string
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public string GetCheckedAuditTradeUserIDs(int companyID)
        {
            StringBuilder auditTrailIgnoredUsersChecked = new StringBuilder();
            Users users = UserManager.GetUsers(companyID);
            try
            {
                for (int i = 0; i < chkListAuditUsers.Items.Count; i++)
                {
                    if (chkListAuditUsers.GetItemChecked(i) == true)
                    {
                        foreach (User us in users)
                        {
                            if (String.Compare(us.LoginName, chkListAuditUsers.Items[i].ToString(), true) == 0)
                            {
                                auditTrailIgnoredUsersChecked.Append(us.UserID);
                                auditTrailIgnoredUsersChecked.Append(',');
                            }
                        }
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
                return null;
            }
            return auditTrailIgnoredUsersChecked.ToString().Trim(',');
        }

        public TradingAccounts GetCheckedAllocationTradingAccounts()
        {
            TradingAccounts companyUserAllocationTradingAccounts = new TradingAccounts();
            for (int i = 0, count = checkedlistAllocationTradingAccounts.Items.Count; i < count; i++)
            {
                if (checkedlistAllocationTradingAccounts.GetItemChecked(i) == true)
                {
                    checkedlistAllocationTradingAccounts.SetSelected(i, true);
                    companyUserAllocationTradingAccounts.Add((TradingAccount)checkedlistAllocationTradingAccounts.SelectedItem);
                }
            }
            return companyUserAllocationTradingAccounts;
        }
        #endregion

        #region Save
        /// <summary>
        /// To Save Data from the ListBoxes into the Database. IT has been modified to pick data from the check
        /// boxes instead of the listboxes
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public bool Save(int companyUserID, int companyID)
        {
            bool result = false;
            try
            {
                if (CounterPartyManager.SaveCounterPartyVenuesForUser(companyID, companyUserID, GetCheckedCounterPartyVenues())
                    && CompanyManager.SaveTradingAccountsForUser(companyUserID, GetCheckedTradingAccounts())
                    && CompanyManager.SaveCompanyModulesForUser(companyUserID, GetCheckedModules())
                    //save override  and pre trade permission
                    // && CompanyManager.SaveCompliancePermissions( GetCompliancePermissions(companyID, companyUserID))
                    //&& CompanyManager.SaveOverridePowerUserAndPreTradeCheckPermission(companyID, companyUserID, chbPreTradeEnabled.Checked, chbOverridePermission.Checked, chbPowerUser.Checked, chbApplyToManual.Checked)
                    //&& CompanyManager.SaveCompanyUnderlyingsForUser(companyUserID, GetCheckedUnderlyings())
                    && CompanyManager.SaveCompanyAUECsForUser(companyUserID, GetCheckedAssets())
                    && CompanyManager.SaveCompanyAccountsForUser(companyUserID, GetCheckedAccounts())
                    && CompanyManager.SaveCompanyStrategiesForUser(companyUserID, GetCheckedStrategies())
                    && CompanyManager.SaveAllocationTradingAccountsForUser(companyUserID, GetCheckedAllocationTradingAccounts())
                    && CompanyManager.SaveAuditTrailIgnoredUsers(companyID, companyUserID, GetCheckedAuditTradeUserIDs(companyID))
                    && CompanyManager.SaveMarketDataTypesForUser(companyUserID, GetCheckedMarketDataTypes())
                    && CompanyManager.SaveAssetPermissionForUser(companyUserID)
                    && CompanyManager.SaveUsersSecuritiesListPermission(companyUserID, GetUserSecuritiesListPermission())
                    && CompanyManager.SaveHotKeyPreferences(companyUserID))
                //   && CompanyManager.SaveOverRiddenRulePermission(companyUserID, GetCheckedOverRiddenRule())
                {
                    result = true;
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
            return result;
        }

        /// <summary>
        /// Gets the user securities list permission.
        /// </summary>
        /// <returns></returns>
        private int GetUserSecuritiesListPermission()
        {
            int readWritePermissionID = 0;
            try
            {
                if (rdbReadOnlyRestrictedAllowedSecurities.Checked == true)
                {
                    readWritePermissionID = 0;
                }
                else
                {
                    readWritePermissionID = 1;
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

            return readWritePermissionID;
        }

        /// <summary>
        /// Getting Checked OverRidden Rule Items
        /// </summary>
        /// <returns></returns>
        private List<string> GetCheckedOverRiddenRule()
        {
            try
            {
                List<string> rulePermission = new List<string>();
                if (!chbOverridePermission.Checked)
                {
                    if (ultraComEdORRulePermission.CheckedItems.Count > 0)
                    {
                        foreach (ValueListItem item in ultraComEdORRulePermission.Items)
                        {
                            if (item.CheckState == CheckState.Checked)
                            {
                                rulePermission.Add((item.ListObject as RuleLevelPermission).RuleId);
                            }
                        }
                    }
                }
                return rulePermission;
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
                return null;
            }

        }

        /// <summary>
        /// Creates compliance perrmission object for user id
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="companyUserID"></param>
        /// <returns></returns>
        private CompliancePermissions GetCompliancePermissions(int companyID, int companyUserID)
        {
            try
            {
                CompliancePermissions compliancePermissions = new CompliancePermissions();
                compliancePermissions.CompanyId = companyID;
                compliancePermissions.CompanyUserId = companyUserID;
                compliancePermissions.IsPowerUser = chbPowerUser.Checked;

                compliancePermissions.complianceUIPermissions.Add(RuleType.PostTrade, new ComplianceUIPermissions());
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsCreate = chckPostCreate.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsExport = chckPostExport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsRename = chckPostRename.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsEnable = chckPostEnable.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsImport = chckPostImport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PostTrade].IsDelete = chckPostDelete.Checked;

                compliancePermissions.complianceUIPermissions.Add(RuleType.PreTrade, new ComplianceUIPermissions());
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsCreate = chckPreCreate.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsExport = chckPreExport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsRename = chckPreRename.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsEnable = chckPreEnable.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsImport = chckPreImport.Checked;
                compliancePermissions.complianceUIPermissions[RuleType.PreTrade].IsDelete = chckPreDelete.Checked;

                compliancePermissions.RuleCheckPermission.IsApplyToManual = chbApplyToManual.Checked;
                compliancePermissions.RuleCheckPermission.IsOverridePermission = chbOverridePermission.Checked;
                compliancePermissions.RuleCheckPermission.IsPreTradeEnabled = chbPreTradeEnabled.Checked;
                compliancePermissions.RuleCheckPermission.IsTrading = checkBoxTrading.Checked;
                compliancePermissions.RuleCheckPermission.IsStaging = checkBoxStaging.Checked;

                return compliancePermissions;
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
                return null;
            }
        }


        #endregion


        private void checkedlstApplicationComponent_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Modules modules = new Modules();
            CheckState chkPM = e.NewValue;

            checkedlstApplicationComponent.SetSelected(e.Index, true);

            if (((Prana.Admin.BLL.Module)(((System.Windows.Forms.ListBox)(sender)).SelectedItem)) != null)
            {
                if (((Prana.Admin.BLL.Module)(((System.Windows.Forms.ListBox)(sender)).SelectedItem)).ModuleID == POSITION_MANAGEMENT)
                {
                    if (_pmReadWritePermission)
                    {
                        if (chkPM == CheckState.Checked)
                        {
                            grpPM.Enabled = true;
                            //optionReadPM.Checked = true;
                        }
                        else
                        {
                            grpPM.Enabled = false;
                        }
                    }
                    else
                    {
                        grpPM.Enabled = false;
                    }
                }
                if (((Prana.Admin.BLL.Module)(((System.Windows.Forms.ListBox)(sender)).SelectedItem)).ModuleID == ALLOCATION)
                {

                    if (chkPM == CheckState.Checked)
                    {
                        grpAllocation.Enabled = true;
                    }
                    else
                    {
                        grpAllocation.Enabled = false;
                    }

                }
                if (_marketDataTypeModules != null && chkPM == CheckState.Checked)
                {
                    updateMarketDataType((Prana.Admin.BLL.Module)((System.Windows.Forms.ListBox)sender).SelectedItem);
                }
            }

        }

        private void chbPreTradeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chbPreTradeEnabled.Checked == true)
            {
                if (chbOverridePermission.Checked == true)
                {
                    ultraComEdORRulePermission.Enabled = false;
                }
                else
                {
                    ultraComEdORRulePermission.Enabled = true;
                }
                chbApplyToManual.Enabled = true;
                chbOverridePermission.Enabled = true;
                //Enable the checkBoxTrading, checkBoxStaging, If chbPreTradeEnabled checked
                checkBoxTrading.Enabled = true;
                checkBoxStaging.Enabled = true;
            }
            else
            {
                chbOverridePermission.Checked = false;
                chbApplyToManual.Checked = false;
                chbApplyToManual.Enabled = false;
                chbOverridePermission.Enabled = false;
                ultraComEdORRulePermission.Enabled = false;
                //Diable the checkBoxTrading, checkBoxStaging, If chbPreTradeEnabled Unchecked
                checkBoxTrading.Enabled = false;
                checkBoxStaging.Enabled = false;

            }
        }
        /// <summary>
        /// Added By Faisal Shah
        /// Dated 20/06/14
        /// Needed to show only some controls on UI for CH Release
        /// </summary>
        public void SetControlsForCHRelease()
        {
            try
            {
                this.Controls.Remove(this.grpAudit);
                this.Controls.Remove(this.gbCompliance);
                this.Controls.Remove(this.grpAllocation);
                this.Controls.Remove(this.grpPM);
                this.Controls.Remove(this.grpAccounts);
                this.Controls.Remove(this.grpApplicationComponent);
                //this.Controls.Remove(this.ultraGroupBox1);
                this.Controls.Remove(this.ultraGroupBox2);

                // Code Added & Modified by Ankit Gupta on 8th Oct, 2014.
                // http://jira.nirvanasolutions.com:8080/browse/CHMW-1494
                // Need to add Strategy Mapping UI to implement permissioning on Strategies
                //this.Controls.Remove(this.grpStrategies);

                this.grpAssetUnderlying.Location = new System.Drawing.Point(05, 2);
                this.grpAssetUnderlying.Size = new System.Drawing.Size(270, 300);
                this.grpAssetUnderlying.HeaderAppearance.ForeColor = System.Drawing.Color.White;
                this.checkedlstAssetUnderLying.Size = new System.Drawing.Size(256, 285);

                this.grpCounterParties.Location = new System.Drawing.Point(290, 2);
                this.grpCounterParties.Size = new System.Drawing.Size(270, 300);
                this.grpCounterParties.HeaderAppearance.ForeColor = System.Drawing.Color.White;
                this.checkedlstCounterParties.Size = new System.Drawing.Size(256, 285);

                this.grpStrategies.Location = new System.Drawing.Point(05, 322);
                this.grpStrategies.Size = new System.Drawing.Size(270, 200);
                this.grpStrategies.HeaderAppearance.ForeColor = System.Drawing.Color.White;
                this.checkedlstStrategies.Size = new System.Drawing.Size(256, 185);

                this.ultraGroupBox1.Location = new System.Drawing.Point(290, 322);
                this.ultraGroupBox1.Size = new System.Drawing.Size(270, 200);
                this.ultraGroupBox1.HeaderAppearance.ForeColor = System.Drawing.Color.White;
                this.checkedlstTradingAccount.Size = new System.Drawing.Size(256, 185);

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

        public void SetMarketDataTypes(int companyUserID)
        {
            IList<MarketDataType> types = CompanyManager.GetMarketDataTypes();
            if (types.Count > 0)
            {
                chkMarketDataTypes.DataSource = types;
                chkMarketDataTypes.DisplayMember = "MarketDataTypeName";
                chkMarketDataTypes.ValueMember = "MarketDataTypeID";
                chkMarketDataTypes.SelectedIndex = -1;
                if (companyUserID != int.MinValue)
                {
                    IList<MarketDataType> userTypes = CompanyManager.GetMarketDataTypesForUser(companyUserID);

                    for (int j = 0; j < types.Count; j++)
                    {
                        chkMarketDataTypes.SetItemChecked(j, false);
                    }

                    for (int j = 0; j < chkMarketDataTypes.Items.Count; j++)
                    {
                        foreach (MarketDataType userTradingAccount in userTypes)
                        {
                            if (((MarketDataType)chkMarketDataTypes.Items[j]).MarketDataTypeID == userTradingAccount.MarketDataTypeID)
                            {
                                chkMarketDataTypes.SetItemChecked(j, true);
                            }
                        }
                    }

                }
            }
            else
            {
                chkMarketDataTypes.DataSource = null;
            }
            checkedlistbox_SelectedValueChanged(this.chkMarketDataTypes, null, this.livefeed_selectallbox);
            _marketDataTypeModules = CompanyManager.GetMarketDataModues();
        }

        public IList<MarketDataType> GetCheckedMarketDataTypes()
        {
            IList<MarketDataType> checkedTypes = new List<MarketDataType>();
            try
            {
                for (int i = 0, count = chkMarketDataTypes.Items.Count; i < count; i++)
                {
                    if (chkMarketDataTypes.GetItemChecked(i) == true)
                    {
                        chkMarketDataTypes.SetSelected(i, true);
                        checkedTypes.Add((MarketDataType)chkMarketDataTypes.SelectedItem);
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
            return checkedTypes;
        }

        private void chkMarketDataTypes_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckState chkstate = e.NewValue;

            chkMarketDataTypes.SetSelected(e.Index, true);
            try
            {
                if (((((System.Windows.Forms.ListBox)(sender)).SelectedItem)) != null)
                {
                    MarketDataType selected = ((MarketDataType)(((System.Windows.Forms.ListBox)(sender)).SelectedItem));
                    string marketDataType = selected.MarketDataTypeName;
                    if (_marketDataTypeModules != null && _marketDataTypeModules.ContainsKey(marketDataType))
                    {
                        IList<string> modules = _marketDataTypeModules[marketDataType];
                        for (int i = 0; i < checkedlstApplicationComponent.Items.Count; i++)
                        {
                            Module module = (Module)checkedlstApplicationComponent.Items[i];
                            if (module != null && modules.Contains(module.ModuleName))
                            {

                                checkedlstApplicationComponent.SetItemCheckState(i, chkstate);
                            }
                        }
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

        private void updateMarketDataType(Module module)
        {
            try
            {
                foreach (string mdType in _marketDataTypeModules.Keys)
                {
                    if (_marketDataTypeModules[mdType].Contains(module.ModuleName))
                    {
                        for (int i = 0; i < chkMarketDataTypes.Items.Count; i++)
                        {
                            MarketDataType mdt = (MarketDataType)chkMarketDataTypes.Items[i];
                            if (mdt.MarketDataTypeName == mdType)
                            {
                                this.chkMarketDataTypes.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.chkMarketDataTypes_ItemCheck);
                                chkMarketDataTypes.SetItemChecked(i, true);
                                this.chkMarketDataTypes.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chkMarketDataTypes_ItemCheck);
                                break;
                            }
                        }
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

        /// <summary>
        /// OverRidden CheckBox Checked Change, If OverRidden Check box checked then OverRidden Rule ComboBox Disable otherwise Enable
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">EverntArgs</param>
        private void chbOverridePermission_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chbOverridePermission.Checked)
                {
                    ultraComEdORRulePermission.Enabled = false;
                }
                else
                {
                    ultraComEdORRulePermission.Enabled = true;
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
        /// UltraCombo Editor RulePermission Mouse Enter Element Event to show tool tip
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">UIElementEventArgs</param>
        private void ultraComEdORRulePermission_MouseEnterElement(object sender, UIElementEventArgs e)
        {
            try
            {
                UltraToolTipInfo toolTipInfo = this.ultraToolTipManager1.GetUltraToolTip(ultraComEdORRulePermission);
                toolTipInfo.ToolTipText = ultraComEdORRulePermission.Text;
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

        private void selectallbox_Click(object sender, EventArgs e, CheckedListBox selectedcheckedlistbox)
        {
            CheckBox selectedcheckbox = sender as CheckBox;
            for (int i = 0; i < selectedcheckedlistbox.Items.Count; i++)
                selectedcheckedlistbox.SetItemChecked(i, selectedcheckbox.Checked);
        }

        private void checkedlistbox_SelectedValueChanged(object sender, EventArgs e, CheckBox currentselectallbox)
        {
            CheckedListBox selectedcheckedlistbox = sender as CheckedListBox;
            if (selectedcheckedlistbox.CheckedItems.Count == 0)
            {
                currentselectallbox.Checked = false;
                currentselectallbox.CheckState = CheckState.Unchecked;
            }
            else if (selectedcheckedlistbox.CheckedItems.Count < selectedcheckedlistbox.Items.Count)
                currentselectallbox.CheckState = CheckState.Indeterminate;
            else
            {
                currentselectallbox.Checked = true;
                currentselectallbox.CheckState = CheckState.Checked;
            }
        }

    }
}
