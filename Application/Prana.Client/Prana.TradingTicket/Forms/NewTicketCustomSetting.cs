using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.CommonDatabaseAccess;
using Prana.CommonDataCache;
using Prana.Global;
using Prana.Global.Utilities;
using Prana.LogManager;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.StringUtilities;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for NewTicketSetting.
    /// </summary>
    public class NewTicketCustomSetting : System.Windows.Forms.Form
    {
        #region Private Variables
        private const string FORM_NAME = "NewTicketCustomSetting : ";
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAsset;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUnderLying;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenue;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbStrategy;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUserAccount;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbHandlingInstruction;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUserTradingAccount;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbOrderType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTIF;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExecutionInstruction;
        private IContainer components;
        private Infragistics.Win.UltraWinEditors.UltraColorPicker cmbButtonColor;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSide;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAgencyPrincipal;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbShortExempt;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClientAccount;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClientTrader;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClientCompany;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClearingFirmID;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtName;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbPNP;

        #endregion

        private CompanyUser _loginUser;
        private ArrayList _existingNames;

        #region Properties
        private TradingTicketSettings _actionButtonDefinition = new TradingTicketSettings();
        public TradingTicketSettings ActionButton
        {
            get { return _actionButtonDefinition; }
            set { _actionButtonDefinition = value; }
        }
        private int _auecID;
        public int AUECID
        {
            get { return _auecID; }
            set
            {
                _auecID = value;
                _actionButtonDefinition.AUECID = _auecID;
            }
        }

        private int _assetID;
        public int AssetID
        {
            get { return _assetID; }
            set
            {
                _assetID = value;
                _actionButtonDefinition.AssetID = _assetID;
            }
        }

        private int _underLyingID;
        public int UnderLyingID
        {
            get { return _underLyingID; }
            set
            {
                _underLyingID = value;
                _actionButtonDefinition.UnderLyingID = _underLyingID;

            }
        }

        private string _buttonPosition;
        public string ButtonPosition
        {
            get { return _buttonPosition; }
            set
            {
                _buttonPosition = value;
                _actionButtonDefinition.ButtonPosition = _buttonPosition;
            }
        }

        private string _selectedButtonName = string.Empty;

        public string SelectedButtonName
        {
            get { return _selectedButtonName; }
            set { _selectedButtonName = value; }
        }

        #endregion

        public NewTicketCustomSetting(CompanyUser user, ArrayList existingNames, int auecID, int assetID, int underlyingID, string buttonPosition)
        {
            _existingNames = existingNames;
            _loginUser = user;
            AUECID = auecID;
            AssetID = assetID;
            UnderLyingID = underlyingID;
            ButtonPosition = buttonPosition;
            InitializeComponent();
            BindCombos();
            SetDefaultValues();
            DisableControls();
        }
        #region Initinal Load Methods

        private void BindCombos()
        {
            BindAssetsCombo();
            BindUserCounterParty();
            BindSideCombo();
            BindAccount();
            BindCompanyClients();
            BindCVTradingAccount();
            BindStrategy();
            BindYesNo();
            BindCompanyClearingFirm();
            BindLimitPrice();
        }

        private void SetDefaultValues()
        {
            cmbAsset.Value = _assetID;
            cmbUnderLying.Value = _underLyingID;
        }
        private void DisableControls()
        {
            cmbAsset.Enabled = false;
            cmbUnderLying.Enabled = false;
        }
        #endregion

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
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (lblHotCold != null)
                {
                    lblHotCold.Dispose();
                }
                if (btnHot != null)
                {
                    btnHot.Dispose();
                }
                if (btnCold != null)
                {
                    btnCold.Dispose();
                }
                if (groupBoxHOtCold != null)
                {
                    groupBoxHOtCold.Dispose();
                }
                if (label13 != null)
                {
                    label13.Dispose();
                }
                if (label37 != null)
                {
                    label37.Dispose();
                }
                if (label48 != null)
                {
                    label48.Dispose();
                }
                if (label47 != null)
                {
                    label47.Dispose();
                }
                if (label46 != null)
                {
                    label46.Dispose();
                }
                if (label45 != null)
                {
                    label45.Dispose();
                }
                if (label42 != null)
                {
                    label42.Dispose();
                }
                if (label44 != null)
                {
                    label44.Dispose();
                }
                if (label43 != null)
                {
                    label43.Dispose();
                }
                if (label41 != null)
                {
                    label41.Dispose();
                }
                if (label40 != null)
                {
                    label40.Dispose();
                }
                if (label35 != null)
                {
                    label35.Dispose();
                }
                if (label34 != null)
                {
                    label34.Dispose();
                }
                if (txtQuantity != null)
                {
                    txtQuantity.Dispose();
                }
                if (label38 != null)
                {
                    label38.Dispose();
                }
                if (txtPegOffSet != null)
                {
                    txtPegOffSet.Dispose();
                }
                if (txtDisplayQuantity != null)
                {
                    txtDisplayQuantity.Dispose();
                }
                if (txtRandom != null)
                {
                    txtRandom.Dispose();
                }
                if (txtDiscrOffSet != null)
                {
                    txtDiscrOffSet.Dispose();
                }
                if (lblLimitOffset != null)
                {
                    lblLimitOffset.Dispose();
                }
                if (txtLimitOffset != null)
                {
                    txtLimitOffset.Dispose();
                }
                if (label33 != null)
                {
                    label33.Dispose();
                }
                if (cmbLimitPrice != null)
                {
                    cmbLimitPrice.Dispose();
                }
                if (errorProvider != null)
                {
                    errorProvider.Dispose();
                }
                if (cmbPNP != null)
                {
                    cmbPNP.Dispose();
                }
                if (txtDescription != null)
                {
                    txtDescription.Dispose();
                }
                if (txtName != null)
                {
                    txtName.Dispose();
                }
                if (cmbClearingFirmID != null)
                {
                    cmbClearingFirmID.Dispose();
                }
                if (cmbClientCompany != null)
                {
                    cmbClientCompany.Dispose();
                }
                if (cmbClientTrader != null)
                {
                    cmbClientTrader.Dispose();
                }
                if (cmbClientAccount != null)
                {
                    cmbClientAccount.Dispose();
                }
                if (cmbShortExempt != null)
                {
                    cmbShortExempt.Dispose();
                }
                if (cmbAgencyPrincipal != null)
                {
                    cmbAgencyPrincipal.Dispose();
                }
                if (cmbSide != null)
                {
                    cmbSide.Dispose();
                }
                if (cmbButtonColor != null)
                {
                    cmbButtonColor.Dispose();
                }
                if (cmbExecutionInstruction != null)
                {
                    cmbExecutionInstruction.Dispose();
                }
                if (cmbTIF != null)
                {
                    cmbTIF.Dispose();
                }
                if (cmbOrderType != null)
                {
                    cmbOrderType.Dispose();
                }
                if (cmbUserTradingAccount != null)
                {
                    cmbUserTradingAccount.Dispose();
                }
                if (cmbHandlingInstruction != null)
                {
                    cmbHandlingInstruction.Dispose();
                }
                if (cmbUserAccount != null)
                {
                    cmbUserAccount.Dispose();
                }
                if (cmbStrategy != null)
                {
                    cmbStrategy.Dispose();
                }
                if (cmbVenue != null)
                {
                    cmbVenue.Dispose();
                }
                if (cmbCounterParty != null)
                {
                    cmbCounterParty.Dispose();
                }
                if (cmbUnderLying != null)
                {
                    cmbUnderLying.Dispose();
                }
                if (cmbAsset != null)
                {
                    cmbAsset.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (label32 != null)
                {
                    label32.Dispose();
                }
                if (label31 != null)
                {
                    label31.Dispose();
                }
                if (label30 != null)
                {
                    label30.Dispose();
                }
                if (label29 != null)
                {
                    label29.Dispose();
                }
                if (label28 != null)
                {
                    label28.Dispose();
                }
                if (label27 != null)
                {
                    label27.Dispose();
                }
                if (label26 != null)
                {
                    label26.Dispose();
                }
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (label24 != null)
                {
                    label24.Dispose();
                }
                if (label23 != null)
                {
                    label23.Dispose();
                }
                if (label22 != null)
                {
                    label22.Dispose();
                }
                if (label21 != null)
                {
                    label21.Dispose();
                }
                if (label20 != null)
                {
                    label20.Dispose();
                }
                if (ultraGroupBox3 != null)
                {
                    ultraGroupBox3.Dispose();
                }
                if (ultraGroupBox2 != null)
                {
                    ultraGroupBox2.Dispose();
                }
                if (label19 != null)
                {
                    label19.Dispose();
                }
                if (label18 != null)
                {
                    label18.Dispose();
                }
                if (label17 != null)
                {
                    label17.Dispose();
                }
                if (label16 != null)
                {
                    label16.Dispose();
                }
                if (label15 != null)
                {
                    label15.Dispose();
                }
                if (label14 != null)
                {
                    label14.Dispose();
                }
                if (label12 != null)
                {
                    label12.Dispose();
                }
                if (label11 != null)
                {
                    label11.Dispose();
                }
                if (label10 != null)
                {
                    label10.Dispose();
                }
                if (label9 != null)
                {
                    label9.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label8 != null)
                {
                    label8.Dispose();
                }
                if (label6 != null)
                {
                    label6.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (ultraGroupBox1 != null)
                {
                    ultraGroupBox1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.ErrorProvider errorProvider;
        private UltraCombo cmbLimitPrice;
        private Label label33;
        private Spinner txtLimitOffset;
        private Label lblLimitOffset;
        private Spinner txtDiscrOffSet;
        private Spinner txtRandom;
        private Spinner txtDisplayQuantity;
        private Spinner txtPegOffSet;
        private Label label38;
        protected Spinner txtQuantity;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label13;
        private GroupBox groupBoxHOtCold;
        private RadioButton btnCold;
        private RadioButton btnHot;
        private Label lblHotCold;
        private Label label1;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance721 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand61 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn229 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderSide", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn230 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SideID", 1);
            Infragistics.Win.Appearance appearance722 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance723 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance724 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance725 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance726 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance727 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance728 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance729 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance730 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance731 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance732 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance733 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand62 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn231 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn232 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance734 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance735 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance736 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance737 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance738 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance739 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance740 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance741 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance742 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance743 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance744 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance745 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand63 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn233 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssetID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn234 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn235 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn236 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment", 3);
            Infragistics.Win.Appearance appearance746 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance747 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance748 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance749 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance750 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance751 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance752 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance753 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance754 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance755 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance756 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance757 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand64 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn237 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssetID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn238 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn239 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnderLyingID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn240 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Asset", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn241 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 4, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn242 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment", 5);
            Infragistics.Win.Appearance appearance758 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance759 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance760 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance761 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance762 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance763 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance764 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance765 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance766 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance767 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance768 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance769 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand65 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance770 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance771 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance772 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance773 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance774 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance775 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance776 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance777 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance778 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance779 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance780 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance781 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand66 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn243 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn244 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("VenueID", 1);
            Infragistics.Win.Appearance appearance782 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance783 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance784 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance785 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance786 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance787 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance788 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance789 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance790 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance791 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance792 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance793 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand67 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn245 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTypesID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn246 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance794 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance795 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance796 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance797 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance798 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance799 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance800 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance801 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance802 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance803 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance804 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance805 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand68 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn247 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn248 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTimeInForce", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance806 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance807 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance808 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance809 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance810 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance811 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance812 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance813 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance814 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance815 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance816 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance817 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand69 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn249 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn250 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructions", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance818 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance819 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance820 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance821 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance822 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance823 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance824 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance825 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance826 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance827 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance828 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance829 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand70 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn251 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn252 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHandlingInstruction", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance830 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance831 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance832 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance833 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance834 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance835 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance836 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance837 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance838 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance839 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance840 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance841 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand71 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn253 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn254 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance842 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance843 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance844 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance845 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance846 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance847 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance848 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance849 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance850 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance851 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance852 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance853 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand72 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn255 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn256 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance854 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance855 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance856 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance857 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance858 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance859 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance860 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance861 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance862 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance863 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance864 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance865 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand73 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn257 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn258 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance866 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance867 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance868 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance869 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance870 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance871 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance872 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance873 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance874 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance875 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance876 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance877 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand74 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn259 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn260 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value", 1);
            Infragistics.Win.Appearance appearance878 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance879 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance880 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance881 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance882 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance883 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance884 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance885 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance886 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance887 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance888 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance889 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand75 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn261 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data2", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn262 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value2", 1);
            Infragistics.Win.Appearance appearance890 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance891 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance892 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance893 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance894 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance895 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance896 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance897 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance898 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance899 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance900 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance901 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand76 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn263 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Data1", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn264 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Value1", 1);
            Infragistics.Win.Appearance appearance902 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance903 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance904 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance905 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance906 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance907 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance908 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance909 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance910 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance911 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance912 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance913 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand77 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn265 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountShortName", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn266 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn267 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientAccountName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn268 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientID", 3);
            Infragistics.Win.Appearance appearance914 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance915 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance916 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance917 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance918 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance919 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance920 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance921 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance922 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance923 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance924 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance925 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand78 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn269 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ShortName", 0, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn270 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TraderID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn271 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FirstName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn272 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("LastName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn273 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Title", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn274 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("EMail", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn275 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneWork", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn276 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneCell", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn277 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Pager", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn278 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TelephoneHome", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn279 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn280 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 11);
            Infragistics.Win.Appearance appearance926 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance927 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance928 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance929 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance930 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance931 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance932 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance933 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance934 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance935 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance936 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance937 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand79 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn281 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyClientID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn282 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn283 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactCell", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn284 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactCell", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn285 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTitle", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn286 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTitle", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn287 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactFirstName", 6);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn288 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactFirstName", 7);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn289 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 8);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn290 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactEMail", 9);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn291 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactEMail", 10);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn292 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactTelephone", 11);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn293 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactTelephone", 12);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn294 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Telephone", 13);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn295 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address1", 14);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn296 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Address2", 15);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn297 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Fax", 16);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn298 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("SecondaryContactLastName", 17);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn299 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("PrimaryContactLastName", 18);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn300 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyTypeID", 19);
            Infragistics.Win.Appearance appearance938 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance939 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance940 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance941 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance942 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance943 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance944 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance945 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance946 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance947 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance948 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance949 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand80 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn301 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn302 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn303 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersShortName", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn304 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            Infragistics.Win.Appearance appearance950 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance951 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance952 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance953 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance954 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance955 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance956 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance957 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance958 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance959 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance960 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTicketCustomSetting));
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.cmbButtonColor = new Infragistics.Win.UltraWinEditors.UltraColorPicker();
            this.txtName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label37 = new System.Windows.Forms.Label();
            this.cmbSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtQuantity = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.label38 = new System.Windows.Forms.Label();
            this.lblLimitOffset = new System.Windows.Forms.Label();
            this.txtLimitOffset = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.cmbLimitPrice = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label33 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbAsset = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbUnderLying = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label12 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbOrderType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbTIF = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbExecutionInstruction = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbHandlingInstruction = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbUserTradingAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label19 = new System.Windows.Forms.Label();
            this.cmbUserAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.cmbStrategy = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.txtRandom = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.txtDisplayQuantity = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.txtDiscrOffSet = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.txtPegOffSet = new Prana.Utilities.UI.UIUtilities.Spinner();
            this.label28 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.cmbPNP = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label32 = new System.Windows.Forms.Label();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this.cmbAgencyPrincipal = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label20 = new System.Windows.Forms.Label();
            this.cmbShortExempt = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.cmbClientAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label23 = new System.Windows.Forms.Label();
            this.cmbClientTrader = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbClientCompany = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.label27 = new System.Windows.Forms.Label();
            this.cmbClearingFirmID = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label13 = new System.Windows.Forms.Label();
            this.groupBoxHOtCold = new System.Windows.Forms.GroupBox();
            this.btnCold = new System.Windows.Forms.RadioButton();
            this.btnHot = new System.Windows.Forms.RadioButton();
            this.lblHotCold = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbButtonColor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLimitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnderLying)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserTradingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPNP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAgencyPrincipal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortExempt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientTrader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientCompany)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirmID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBoxHOtCold.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.groupBoxHOtCold);
            this.ultraGroupBox1.Controls.Add(this.label34);
            this.ultraGroupBox1.Controls.Add(this.label35);
            this.ultraGroupBox1.Controls.Add(this.cmbButtonColor);
            this.ultraGroupBox1.Controls.Add(this.txtName);
            this.ultraGroupBox1.Controls.Add(this.label6);
            this.ultraGroupBox1.Controls.Add(this.label3);
            this.ultraGroupBox1.Controls.Add(this.label2);
            this.ultraGroupBox1.Controls.Add(this.txtDescription);
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 3);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(266, 133);
            this.ultraGroupBox1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGroupBox1.TabIndex = 1;
            this.ultraGroupBox1.Text = "Create New";
            // 
            // label34
            // 
            this.label34.ForeColor = System.Drawing.Color.Red;
            this.label34.Location = new System.Drawing.Point(98, 46);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(10, 10);
            this.label34.TabIndex = 12;
            this.label34.Text = "*";
            // 
            // label35
            // 
            this.label35.ForeColor = System.Drawing.Color.Red;
            this.label35.Location = new System.Drawing.Point(98, 20);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(10, 10);
            this.label35.TabIndex = 13;
            this.label35.Text = "*";
            // 
            // cmbButtonColor
            // 
            this.cmbButtonColor.Color = System.Drawing.Color.Empty;
            this.cmbButtonColor.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbButtonColor.Location = new System.Drawing.Point(113, 73);
            this.cmbButtonColor.Name = "cmbButtonColor";
            this.cmbButtonColor.Size = new System.Drawing.Size(142, 20);
            this.cmbButtonColor.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtName.Location = new System.Drawing.Point(114, 20);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(142, 20);
            this.txtName.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(5, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "Button Color";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name";
            // 
            // txtDescription
            // 
            this.txtDescription.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.txtDescription.Location = new System.Drawing.Point(114, 48);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(142, 20);
            this.txtDescription.TabIndex = 1;
            // 
            // label37
            // 
            this.label37.ForeColor = System.Drawing.Color.Red;
            this.label37.Location = new System.Drawing.Point(103, 102);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(10, 10);
            this.label37.TabIndex = 23;
            this.label37.Text = "*";
            // 
            // cmbSide
            // 
            this.cmbSide.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance721.BackColor = System.Drawing.SystemColors.Window;
            appearance721.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSide.DisplayLayout.Appearance = appearance721;
            ultraGridBand61.ColHeadersVisible = false;
            ultraGridColumn229.Header.VisiblePosition = 0;
            ultraGridColumn230.Header.VisiblePosition = 1;
            ultraGridColumn230.Hidden = true;
            ultraGridBand61.Columns.AddRange(new object[] {
            ultraGridColumn229,
            ultraGridColumn230});
            this.cmbSide.DisplayLayout.BandsSerializer.Add(ultraGridBand61);
            this.cmbSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance722.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance722.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance722.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance722.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.GroupByBox.Appearance = appearance722;
            appearance723.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance723;
            this.cmbSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance724.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance724.BackColor2 = System.Drawing.SystemColors.Control;
            appearance724.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance724.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.PromptAppearance = appearance724;
            this.cmbSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance725.BackColor = System.Drawing.SystemColors.Window;
            appearance725.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSide.DisplayLayout.Override.ActiveCellAppearance = appearance725;
            appearance726.BackColor = System.Drawing.SystemColors.Highlight;
            appearance726.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSide.DisplayLayout.Override.ActiveRowAppearance = appearance726;
            this.cmbSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance727.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.CardAreaAppearance = appearance727;
            appearance728.BorderColor = System.Drawing.Color.Silver;
            appearance728.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSide.DisplayLayout.Override.CellAppearance = appearance728;
            this.cmbSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSide.DisplayLayout.Override.CellPadding = 0;
            appearance729.BackColor = System.Drawing.SystemColors.Control;
            appearance729.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance729.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance729.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance729.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.GroupByRowAppearance = appearance729;
            appearance730.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbSide.DisplayLayout.Override.HeaderAppearance = appearance730;
            this.cmbSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance731.BackColor = System.Drawing.SystemColors.Window;
            appearance731.BorderColor = System.Drawing.Color.Silver;
            this.cmbSide.DisplayLayout.Override.RowAppearance = appearance731;
            this.cmbSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance732.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance732;
            this.cmbSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSide.DisplayMember = "";
            this.cmbSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSide.Location = new System.Drawing.Point(122, 102);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(142, 21);
            this.cmbSide.TabIndex = 4;
            this.cmbSide.ValueMember = "";
            this.cmbSide.ValueChanged += new System.EventHandler(this.cmbSide_ValueChanged);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "Side";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtQuantity);
            this.groupBox1.Controls.Add(this.label38);
            this.groupBox1.Controls.Add(this.lblLimitOffset);
            this.groupBox1.Controls.Add(this.txtLimitOffset);
            this.groupBox1.Controls.Add(this.cmbLimitPrice);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.label48);
            this.groupBox1.Controls.Add(this.label37);
            this.groupBox1.Controls.Add(this.label47);
            this.groupBox1.Controls.Add(this.label46);
            this.groupBox1.Controls.Add(this.cmbSide);
            this.groupBox1.Controls.Add(this.label45);
            this.groupBox1.Controls.Add(this.label44);
            this.groupBox1.Controls.Add(this.label43);
            this.groupBox1.Controls.Add(this.label42);
            this.groupBox1.Controls.Add(this.label41);
            this.groupBox1.Controls.Add(this.label40);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmbAsset);
            this.groupBox1.Controls.Add(this.cmbUnderLying);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbCounterParty);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cmbVenue);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cmbOrderType);
            this.groupBox1.Controls.Add(this.cmbTIF);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.cmbExecutionInstruction);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.cmbHandlingInstruction);
            this.groupBox1.Controls.Add(this.cmbUserTradingAccount);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.cmbUserAccount);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label24);
            this.groupBox1.Controls.Add(this.cmbStrategy);
            this.groupBox1.Location = new System.Drawing.Point(267, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(278, 370);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // txtQuantity
            // 
            this.txtQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtQuantity.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.PositiveInteger;
            this.txtQuantity.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtQuantity.Increment = 1;
            this.txtQuantity.Location = new System.Drawing.Point(122, 125);
            this.txtQuantity.MaxValue = 99999999;
            this.txtQuantity.MinValue = 1;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(141, 20);
            this.txtQuantity.TabIndex = 5;
            this.txtQuantity.Value = 1;
            // 
            // label38
            // 
            this.label38.ForeColor = System.Drawing.Color.Red;
            this.label38.Location = new System.Drawing.Point(103, 291);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(10, 10);
            this.label38.TabIndex = 34;
            this.label38.Text = "*";
            // 
            // lblLimitOffset
            // 
            this.lblLimitOffset.Location = new System.Drawing.Point(7, 197);
            this.lblLimitOffset.Name = "lblLimitOffset";
            this.lblLimitOffset.Size = new System.Drawing.Size(74, 12);
            this.lblLimitOffset.TabIndex = 33;
            this.lblLimitOffset.Text = "Limit Offset";
            // 
            // txtLimitOffset
            // 
            this.txtLimitOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtLimitOffset.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.txtLimitOffset.Enabled = false;
            this.txtLimitOffset.Increment = 0.01;
            this.txtLimitOffset.Location = new System.Drawing.Point(179, 197);
            this.txtLimitOffset.MaxValue = 999999999;
            this.txtLimitOffset.MinValue = -999999999;
            this.txtLimitOffset.Name = "txtLimitOffset";
            this.txtLimitOffset.Size = new System.Drawing.Size(86, 20);
            this.txtLimitOffset.TabIndex = 8;
            this.txtLimitOffset.Value = 0;
            // 
            // cmbLimitPrice
            // 
            this.cmbLimitPrice.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance733.BackColor = System.Drawing.SystemColors.Window;
            appearance733.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbLimitPrice.DisplayLayout.Appearance = appearance733;
            ultraGridBand62.ColHeadersVisible = false;
            ultraGridColumn231.Header.VisiblePosition = 0;
            ultraGridColumn231.Hidden = true;
            ultraGridColumn232.Header.VisiblePosition = 1;
            ultraGridBand62.Columns.AddRange(new object[] {
            ultraGridColumn231,
            ultraGridColumn232});
            ultraGridBand62.GroupHeadersVisible = false;
            this.cmbLimitPrice.DisplayLayout.BandsSerializer.Add(ultraGridBand62);
            this.cmbLimitPrice.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbLimitPrice.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance734.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance734.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance734.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance734.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLimitPrice.DisplayLayout.GroupByBox.Appearance = appearance734;
            appearance735.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLimitPrice.DisplayLayout.GroupByBox.BandLabelAppearance = appearance735;
            this.cmbLimitPrice.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance736.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance736.BackColor2 = System.Drawing.SystemColors.Control;
            appearance736.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance736.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbLimitPrice.DisplayLayout.GroupByBox.PromptAppearance = appearance736;
            this.cmbLimitPrice.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbLimitPrice.DisplayLayout.MaxRowScrollRegions = 1;
            appearance737.BackColor = System.Drawing.SystemColors.Window;
            appearance737.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbLimitPrice.DisplayLayout.Override.ActiveCellAppearance = appearance737;
            appearance738.BackColor = System.Drawing.SystemColors.Highlight;
            appearance738.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbLimitPrice.DisplayLayout.Override.ActiveRowAppearance = appearance738;
            this.cmbLimitPrice.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbLimitPrice.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance739.BackColor = System.Drawing.SystemColors.Window;
            this.cmbLimitPrice.DisplayLayout.Override.CardAreaAppearance = appearance739;
            appearance740.BorderColor = System.Drawing.Color.Silver;
            appearance740.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbLimitPrice.DisplayLayout.Override.CellAppearance = appearance740;
            this.cmbLimitPrice.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbLimitPrice.DisplayLayout.Override.CellPadding = 0;
            appearance741.BackColor = System.Drawing.SystemColors.Control;
            appearance741.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance741.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance741.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance741.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbLimitPrice.DisplayLayout.Override.GroupByRowAppearance = appearance741;
            appearance742.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbLimitPrice.DisplayLayout.Override.HeaderAppearance = appearance742;
            this.cmbLimitPrice.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbLimitPrice.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance743.BackColor = System.Drawing.SystemColors.Window;
            appearance743.BorderColor = System.Drawing.Color.Silver;
            this.cmbLimitPrice.DisplayLayout.Override.RowAppearance = appearance743;
            this.cmbLimitPrice.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance744.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbLimitPrice.DisplayLayout.Override.TemplateAddRowAppearance = appearance744;
            this.cmbLimitPrice.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbLimitPrice.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbLimitPrice.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbLimitPrice.DisplayMember = "";
            this.cmbLimitPrice.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbLimitPrice.Enabled = false;
            this.cmbLimitPrice.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbLimitPrice.Location = new System.Drawing.Point(122, 170);
            this.cmbLimitPrice.Name = "cmbLimitPrice";
            this.cmbLimitPrice.Size = new System.Drawing.Size(142, 21);
            this.cmbLimitPrice.TabIndex = 7;
            this.cmbLimitPrice.ValueMember = "";
            // 
            // label33
            // 
            this.label33.Location = new System.Drawing.Point(6, 174);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(74, 12);
            this.label33.TabIndex = 30;
            this.label33.Text = "Limit Price";
            // 
            // label48
            // 
            this.label48.ForeColor = System.Drawing.Color.Red;
            this.label48.Location = new System.Drawing.Point(103, 10);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(10, 10);
            this.label48.TabIndex = 29;
            this.label48.Text = "*";
            // 
            // label47
            // 
            this.label47.ForeColor = System.Drawing.Color.Red;
            this.label47.Location = new System.Drawing.Point(103, 33);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(10, 10);
            this.label47.TabIndex = 28;
            this.label47.Text = "*";
            // 
            // label46
            // 
            this.label46.ForeColor = System.Drawing.Color.Red;
            this.label46.Location = new System.Drawing.Point(103, 56);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(10, 10);
            this.label46.TabIndex = 27;
            this.label46.Text = "*";
            // 
            // label45
            // 
            this.label45.ForeColor = System.Drawing.Color.Red;
            this.label45.Location = new System.Drawing.Point(103, 79);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(10, 10);
            this.label45.TabIndex = 26;
            this.label45.Text = "*";
            // 
            // label44
            // 
            this.label44.ForeColor = System.Drawing.Color.Red;
            this.label44.Location = new System.Drawing.Point(103, 125);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(10, 10);
            this.label44.TabIndex = 25;
            this.label44.Text = "*";
            // 
            // label43
            // 
            this.label43.ForeColor = System.Drawing.Color.Red;
            this.label43.Location = new System.Drawing.Point(103, 147);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(10, 10);
            this.label43.TabIndex = 24;
            this.label43.Text = "*";
            // 
            // label42
            // 
            this.label42.ForeColor = System.Drawing.Color.Red;
            this.label42.Location = new System.Drawing.Point(103, 222);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(10, 10);
            this.label42.TabIndex = 23;
            this.label42.Text = "*";
            // 
            // label41
            // 
            this.label41.ForeColor = System.Drawing.Color.Red;
            this.label41.Location = new System.Drawing.Point(103, 245);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(10, 10);
            this.label41.TabIndex = 22;
            this.label41.Text = "*";
            // 
            // label40
            // 
            this.label40.ForeColor = System.Drawing.Color.Red;
            this.label40.Location = new System.Drawing.Point(103, 268);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(10, 10);
            this.label40.TabIndex = 21;
            this.label40.Text = "*";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "Asset";
            // 
            // cmbAsset
            // 
            this.cmbAsset.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance745.BackColor = System.Drawing.SystemColors.Window;
            appearance745.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAsset.DisplayLayout.Appearance = appearance745;
            ultraGridBand63.ColHeadersVisible = false;
            ultraGridColumn233.Header.VisiblePosition = 0;
            ultraGridColumn233.Hidden = true;
            ultraGridColumn234.Header.VisiblePosition = 1;
            ultraGridColumn235.Header.VisiblePosition = 2;
            ultraGridColumn235.Hidden = true;
            ultraGridColumn236.Header.VisiblePosition = 3;
            ultraGridColumn236.Hidden = true;
            ultraGridBand63.Columns.AddRange(new object[] {
            ultraGridColumn233,
            ultraGridColumn234,
            ultraGridColumn235,
            ultraGridColumn236});
            this.cmbAsset.DisplayLayout.BandsSerializer.Add(ultraGridBand63);
            this.cmbAsset.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAsset.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance746.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance746.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance746.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance746.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.GroupByBox.Appearance = appearance746;
            appearance747.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAsset.DisplayLayout.GroupByBox.BandLabelAppearance = appearance747;
            this.cmbAsset.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance748.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance748.BackColor2 = System.Drawing.SystemColors.Control;
            appearance748.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance748.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAsset.DisplayLayout.GroupByBox.PromptAppearance = appearance748;
            this.cmbAsset.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAsset.DisplayLayout.MaxRowScrollRegions = 1;
            appearance749.BackColor = System.Drawing.SystemColors.Window;
            appearance749.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAsset.DisplayLayout.Override.ActiveCellAppearance = appearance749;
            appearance750.BackColor = System.Drawing.SystemColors.Highlight;
            appearance750.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAsset.DisplayLayout.Override.ActiveRowAppearance = appearance750;
            this.cmbAsset.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAsset.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance751.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.Override.CardAreaAppearance = appearance751;
            appearance752.BorderColor = System.Drawing.Color.Silver;
            appearance752.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAsset.DisplayLayout.Override.CellAppearance = appearance752;
            this.cmbAsset.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAsset.DisplayLayout.Override.CellPadding = 0;
            appearance753.BackColor = System.Drawing.SystemColors.Control;
            appearance753.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance753.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance753.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance753.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAsset.DisplayLayout.Override.GroupByRowAppearance = appearance753;
            appearance754.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAsset.DisplayLayout.Override.HeaderAppearance = appearance754;
            this.cmbAsset.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAsset.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance755.BackColor = System.Drawing.SystemColors.Window;
            appearance755.BorderColor = System.Drawing.Color.Silver;
            this.cmbAsset.DisplayLayout.Override.RowAppearance = appearance755;
            this.cmbAsset.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance756.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAsset.DisplayLayout.Override.TemplateAddRowAppearance = appearance756;
            this.cmbAsset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAsset.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAsset.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAsset.DisplayMember = "";
            this.cmbAsset.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAsset.Location = new System.Drawing.Point(122, 10);
            this.cmbAsset.Name = "cmbAsset";
            this.cmbAsset.Size = new System.Drawing.Size(142, 21);
            this.cmbAsset.TabIndex = 0;
            this.cmbAsset.ValueMember = "";
            this.cmbAsset.ValueChanged += new System.EventHandler(this.cmbAsset_ValueChanged);
            // 
            // cmbUnderLying
            // 
            this.cmbUnderLying.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance757.BackColor = System.Drawing.SystemColors.Window;
            appearance757.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUnderLying.DisplayLayout.Appearance = appearance757;
            ultraGridBand64.ColHeadersVisible = false;
            ultraGridColumn237.Header.VisiblePosition = 0;
            ultraGridColumn237.Hidden = true;
            ultraGridColumn238.Header.VisiblePosition = 1;
            ultraGridColumn238.Hidden = true;
            ultraGridColumn239.Header.VisiblePosition = 2;
            ultraGridColumn239.Hidden = true;
            ultraGridColumn240.Header.VisiblePosition = 3;
            ultraGridColumn240.Hidden = true;
            ultraGridColumn241.Header.VisiblePosition = 4;
            ultraGridColumn242.Header.VisiblePosition = 5;
            ultraGridColumn242.Hidden = true;
            ultraGridBand64.Columns.AddRange(new object[] {
            ultraGridColumn237,
            ultraGridColumn238,
            ultraGridColumn239,
            ultraGridColumn240,
            ultraGridColumn241,
            ultraGridColumn242});
            ultraGridBand64.GroupHeadersVisible = false;
            this.cmbUnderLying.DisplayLayout.BandsSerializer.Add(ultraGridBand64);
            this.cmbUnderLying.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUnderLying.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance758.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance758.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance758.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance758.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.GroupByBox.Appearance = appearance758;
            appearance759.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUnderLying.DisplayLayout.GroupByBox.BandLabelAppearance = appearance759;
            this.cmbUnderLying.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance760.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance760.BackColor2 = System.Drawing.SystemColors.Control;
            appearance760.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance760.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUnderLying.DisplayLayout.GroupByBox.PromptAppearance = appearance760;
            this.cmbUnderLying.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUnderLying.DisplayLayout.MaxRowScrollRegions = 1;
            appearance761.BackColor = System.Drawing.SystemColors.Window;
            appearance761.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUnderLying.DisplayLayout.Override.ActiveCellAppearance = appearance761;
            appearance762.BackColor = System.Drawing.SystemColors.Highlight;
            appearance762.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUnderLying.DisplayLayout.Override.ActiveRowAppearance = appearance762;
            this.cmbUnderLying.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUnderLying.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance763.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.Override.CardAreaAppearance = appearance763;
            appearance764.BorderColor = System.Drawing.Color.Silver;
            appearance764.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUnderLying.DisplayLayout.Override.CellAppearance = appearance764;
            this.cmbUnderLying.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUnderLying.DisplayLayout.Override.CellPadding = 0;
            appearance765.BackColor = System.Drawing.SystemColors.Control;
            appearance765.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance765.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance765.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance765.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUnderLying.DisplayLayout.Override.GroupByRowAppearance = appearance765;
            appearance766.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbUnderLying.DisplayLayout.Override.HeaderAppearance = appearance766;
            this.cmbUnderLying.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUnderLying.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance767.BackColor = System.Drawing.SystemColors.Window;
            appearance767.BorderColor = System.Drawing.Color.Silver;
            this.cmbUnderLying.DisplayLayout.Override.RowAppearance = appearance767;
            this.cmbUnderLying.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance768.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUnderLying.DisplayLayout.Override.TemplateAddRowAppearance = appearance768;
            this.cmbUnderLying.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUnderLying.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUnderLying.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUnderLying.DisplayMember = "";
            this.cmbUnderLying.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUnderLying.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUnderLying.Location = new System.Drawing.Point(122, 33);
            this.cmbUnderLying.Name = "cmbUnderLying";
            this.cmbUnderLying.Size = new System.Drawing.Size(142, 21);
            this.cmbUnderLying.TabIndex = 1;
            this.cmbUnderLying.ValueMember = "";
            this.cmbUnderLying.ValueChanged += new System.EventHandler(this.cmbUnderLying_ValueChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(6, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 14);
            this.label10.TabIndex = 3;
            this.label10.Text = "Underlying";
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance769.BackColor = System.Drawing.SystemColors.Window;
            appearance769.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance769;
            ultraGridBand65.ColHeadersVisible = false;
            ultraGridBand65.GroupHeadersVisible = false;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand65);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance770.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance770.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance770.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance770.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance770;
            appearance771.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance771;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance772.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance772.BackColor2 = System.Drawing.SystemColors.Control;
            appearance772.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance772.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance772;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance773.BackColor = System.Drawing.SystemColors.Window;
            appearance773.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance773;
            appearance774.BackColor = System.Drawing.SystemColors.Highlight;
            appearance774.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance774;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance775.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance775;
            appearance776.BorderColor = System.Drawing.Color.Silver;
            appearance776.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance776;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance777.BackColor = System.Drawing.SystemColors.Control;
            appearance777.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance777.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance777.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance777.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance777;
            appearance778.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance778;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance779.BackColor = System.Drawing.SystemColors.Window;
            appearance779.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance779;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance780.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance780;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DisplayMember = "";
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.Location = new System.Drawing.Point(122, 56);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(142, 21);
            this.cmbCounterParty.TabIndex = 2;
            this.cmbCounterParty.ValueMember = "";
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(6, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 14);
            this.label11.TabIndex = 3;
            this.label11.Text = "Counterparty";
            // 
            // cmbVenue
            // 
            this.cmbVenue.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance781.BackColor = System.Drawing.SystemColors.Window;
            appearance781.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance781;
            ultraGridBand66.ColHeadersVisible = false;
            ultraGridColumn243.Header.VisiblePosition = 0;
            ultraGridColumn244.Header.VisiblePosition = 1;
            ultraGridColumn244.Hidden = true;
            ultraGridBand66.Columns.AddRange(new object[] {
            ultraGridColumn243,
            ultraGridColumn244});
            ultraGridBand66.GroupHeadersVisible = false;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand66);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance782.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance782.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance782.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance782.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance782;
            appearance783.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance783;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance784.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance784.BackColor2 = System.Drawing.SystemColors.Control;
            appearance784.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance784.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance784;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance785.BackColor = System.Drawing.SystemColors.Window;
            appearance785.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance785;
            appearance786.BackColor = System.Drawing.SystemColors.Highlight;
            appearance786.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance786;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance787.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance787;
            appearance788.BorderColor = System.Drawing.Color.Silver;
            appearance788.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance788;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance789.BackColor = System.Drawing.SystemColors.Control;
            appearance789.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance789.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance789.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance789.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance789;
            appearance790.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance790;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance791.BackColor = System.Drawing.SystemColors.Window;
            appearance791.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance791;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance792.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance792;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DisplayMember = "";
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenue.Location = new System.Drawing.Point(122, 79);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(142, 21);
            this.cmbVenue.TabIndex = 3;
            this.cmbVenue.ValueMember = "";
            this.cmbVenue.ValueChanged += new System.EventHandler(this.cmbVenue_ValueChanged);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(6, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 12);
            this.label12.TabIndex = 3;
            this.label12.Text = "Venue";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(6, 127);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 14);
            this.label14.TabIndex = 3;
            this.label14.Text = "Quantity";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(6, 151);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 12);
            this.label15.TabIndex = 3;
            this.label15.Text = "Order Type";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance793.BackColor = System.Drawing.SystemColors.Window;
            appearance793.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderType.DisplayLayout.Appearance = appearance793;
            ultraGridBand67.ColHeadersVisible = false;
            ultraGridColumn245.Header.VisiblePosition = 0;
            ultraGridColumn245.Hidden = true;
            ultraGridColumn246.Header.VisiblePosition = 1;
            ultraGridBand67.Columns.AddRange(new object[] {
            ultraGridColumn245,
            ultraGridColumn246});
            ultraGridBand67.GroupHeadersVisible = false;
            this.cmbOrderType.DisplayLayout.BandsSerializer.Add(ultraGridBand67);
            this.cmbOrderType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance794.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance794.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance794.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance794.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.GroupByBox.Appearance = appearance794;
            appearance795.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance795;
            this.cmbOrderType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance796.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance796.BackColor2 = System.Drawing.SystemColors.Control;
            appearance796.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance796.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.PromptAppearance = appearance796;
            this.cmbOrderType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance797.BackColor = System.Drawing.SystemColors.Window;
            appearance797.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderType.DisplayLayout.Override.ActiveCellAppearance = appearance797;
            appearance798.BackColor = System.Drawing.SystemColors.Highlight;
            appearance798.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderType.DisplayLayout.Override.ActiveRowAppearance = appearance798;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance799.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.CardAreaAppearance = appearance799;
            appearance800.BorderColor = System.Drawing.Color.Silver;
            appearance800.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderType.DisplayLayout.Override.CellAppearance = appearance800;
            this.cmbOrderType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderType.DisplayLayout.Override.CellPadding = 0;
            appearance801.BackColor = System.Drawing.SystemColors.Control;
            appearance801.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance801.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance801.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance801.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.GroupByRowAppearance = appearance801;
            appearance802.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbOrderType.DisplayLayout.Override.HeaderAppearance = appearance802;
            this.cmbOrderType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance803.BackColor = System.Drawing.SystemColors.Window;
            appearance803.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderType.DisplayLayout.Override.RowAppearance = appearance803;
            this.cmbOrderType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance804.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderType.DisplayLayout.Override.TemplateAddRowAppearance = appearance804;
            this.cmbOrderType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderType.DisplayMember = "";
            this.cmbOrderType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOrderType.Location = new System.Drawing.Point(122, 147);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(142, 21);
            this.cmbOrderType.TabIndex = 6;
            this.cmbOrderType.ValueMember = "";
            this.cmbOrderType.ValueChanged += new System.EventHandler(this.cmbOrderType_ValueChanged);
            // 
            // cmbTIF
            // 
            this.cmbTIF.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance805.BackColor = System.Drawing.SystemColors.Window;
            appearance805.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTIF.DisplayLayout.Appearance = appearance805;
            ultraGridBand68.ColHeadersVisible = false;
            ultraGridColumn247.Header.VisiblePosition = 0;
            ultraGridColumn247.Hidden = true;
            ultraGridColumn248.Header.VisiblePosition = 1;
            ultraGridBand68.Columns.AddRange(new object[] {
            ultraGridColumn247,
            ultraGridColumn248});
            ultraGridBand68.GroupHeadersVisible = false;
            this.cmbTIF.DisplayLayout.BandsSerializer.Add(ultraGridBand68);
            this.cmbTIF.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance806.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance806.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance806.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance806.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.GroupByBox.Appearance = appearance806;
            appearance807.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.BandLabelAppearance = appearance807;
            this.cmbTIF.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance808.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance808.BackColor2 = System.Drawing.SystemColors.Control;
            appearance808.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance808.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.PromptAppearance = appearance808;
            this.cmbTIF.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTIF.DisplayLayout.MaxRowScrollRegions = 1;
            appearance809.BackColor = System.Drawing.SystemColors.Window;
            appearance809.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTIF.DisplayLayout.Override.ActiveCellAppearance = appearance809;
            appearance810.BackColor = System.Drawing.SystemColors.Highlight;
            appearance810.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTIF.DisplayLayout.Override.ActiveRowAppearance = appearance810;
            this.cmbTIF.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbTIF.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance811.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.CardAreaAppearance = appearance811;
            appearance812.BorderColor = System.Drawing.Color.Silver;
            appearance812.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTIF.DisplayLayout.Override.CellAppearance = appearance812;
            this.cmbTIF.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTIF.DisplayLayout.Override.CellPadding = 0;
            appearance813.BackColor = System.Drawing.SystemColors.Control;
            appearance813.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance813.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance813.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance813.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.GroupByRowAppearance = appearance813;
            appearance814.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbTIF.DisplayLayout.Override.HeaderAppearance = appearance814;
            this.cmbTIF.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTIF.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance815.BackColor = System.Drawing.SystemColors.Window;
            appearance815.BorderColor = System.Drawing.Color.Silver;
            this.cmbTIF.DisplayLayout.Override.RowAppearance = appearance815;
            this.cmbTIF.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance816.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTIF.DisplayLayout.Override.TemplateAddRowAppearance = appearance816;
            this.cmbTIF.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTIF.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTIF.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTIF.DisplayMember = "";
            this.cmbTIF.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTIF.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTIF.Location = new System.Drawing.Point(122, 222);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new System.Drawing.Size(142, 21);
            this.cmbTIF.TabIndex = 9;
            this.cmbTIF.ValueMember = "";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(6, 225);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 14);
            this.label16.TabIndex = 3;
            this.label16.Text = "Time In Force";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(6, 249);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 14);
            this.label17.TabIndex = 3;
            this.label17.Text = "Exec. Inst.";
            // 
            // cmbExecutionInstruction
            // 
            this.cmbExecutionInstruction.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance817.BackColor = System.Drawing.SystemColors.Window;
            appearance817.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbExecutionInstruction.DisplayLayout.Appearance = appearance817;
            ultraGridBand69.ColHeadersVisible = false;
            ultraGridColumn249.Header.VisiblePosition = 0;
            ultraGridColumn249.Hidden = true;
            ultraGridColumn250.Header.VisiblePosition = 1;
            ultraGridBand69.Columns.AddRange(new object[] {
            ultraGridColumn249,
            ultraGridColumn250});
            ultraGridBand69.GroupHeadersVisible = false;
            this.cmbExecutionInstruction.DisplayLayout.BandsSerializer.Add(ultraGridBand69);
            this.cmbExecutionInstruction.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecutionInstruction.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance818.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance818.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance818.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance818.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.Appearance = appearance818;
            appearance819.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.BandLabelAppearance = appearance819;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance820.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance820.BackColor2 = System.Drawing.SystemColors.Control;
            appearance820.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance820.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.PromptAppearance = appearance820;
            this.cmbExecutionInstruction.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbExecutionInstruction.DisplayLayout.MaxRowScrollRegions = 1;
            appearance821.BackColor = System.Drawing.SystemColors.Window;
            appearance821.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExecutionInstruction.DisplayLayout.Override.ActiveCellAppearance = appearance821;
            appearance822.BackColor = System.Drawing.SystemColors.Highlight;
            appearance822.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbExecutionInstruction.DisplayLayout.Override.ActiveRowAppearance = appearance822;
            this.cmbExecutionInstruction.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbExecutionInstruction.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance823.BackColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.Override.CardAreaAppearance = appearance823;
            appearance824.BorderColor = System.Drawing.Color.Silver;
            appearance824.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellAppearance = appearance824;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellPadding = 0;
            appearance825.BackColor = System.Drawing.SystemColors.Control;
            appearance825.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance825.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance825.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance825.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.Override.GroupByRowAppearance = appearance825;
            appearance826.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderAppearance = appearance826;
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance827.BackColor = System.Drawing.SystemColors.Window;
            appearance827.BorderColor = System.Drawing.Color.Silver;
            this.cmbExecutionInstruction.DisplayLayout.Override.RowAppearance = appearance827;
            this.cmbExecutionInstruction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance828.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbExecutionInstruction.DisplayLayout.Override.TemplateAddRowAppearance = appearance828;
            this.cmbExecutionInstruction.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExecutionInstruction.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbExecutionInstruction.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbExecutionInstruction.DisplayMember = "";
            this.cmbExecutionInstruction.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExecutionInstruction.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbExecutionInstruction.Location = new System.Drawing.Point(122, 245);
            this.cmbExecutionInstruction.Name = "cmbExecutionInstruction";
            this.cmbExecutionInstruction.Size = new System.Drawing.Size(142, 21);
            this.cmbExecutionInstruction.TabIndex = 10;
            this.cmbExecutionInstruction.ValueMember = "";
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(6, 273);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(64, 14);
            this.label18.TabIndex = 3;
            this.label18.Text = "Hand. Inst.";
            // 
            // cmbHandlingInstruction
            // 
            this.cmbHandlingInstruction.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance829.BackColor = System.Drawing.SystemColors.Window;
            appearance829.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbHandlingInstruction.DisplayLayout.Appearance = appearance829;
            ultraGridBand70.ColHeadersVisible = false;
            ultraGridColumn251.Header.VisiblePosition = 0;
            ultraGridColumn251.Hidden = true;
            ultraGridColumn252.Header.VisiblePosition = 1;
            ultraGridBand70.Columns.AddRange(new object[] {
            ultraGridColumn251,
            ultraGridColumn252});
            ultraGridBand70.GroupHeadersVisible = false;
            this.cmbHandlingInstruction.DisplayLayout.BandsSerializer.Add(ultraGridBand70);
            this.cmbHandlingInstruction.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHandlingInstruction.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance830.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance830.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance830.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance830.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.Appearance = appearance830;
            appearance831.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.BandLabelAppearance = appearance831;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance832.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance832.BackColor2 = System.Drawing.SystemColors.Control;
            appearance832.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance832.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.PromptAppearance = appearance832;
            this.cmbHandlingInstruction.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbHandlingInstruction.DisplayLayout.MaxRowScrollRegions = 1;
            appearance833.BackColor = System.Drawing.SystemColors.Window;
            appearance833.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbHandlingInstruction.DisplayLayout.Override.ActiveCellAppearance = appearance833;
            appearance834.BackColor = System.Drawing.SystemColors.Highlight;
            appearance834.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbHandlingInstruction.DisplayLayout.Override.ActiveRowAppearance = appearance834;
            this.cmbHandlingInstruction.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbHandlingInstruction.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance835.BackColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.Override.CardAreaAppearance = appearance835;
            appearance836.BorderColor = System.Drawing.Color.Silver;
            appearance836.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellAppearance = appearance836;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellPadding = 0;
            appearance837.BackColor = System.Drawing.SystemColors.Control;
            appearance837.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance837.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance837.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance837.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.Override.GroupByRowAppearance = appearance837;
            appearance838.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderAppearance = appearance838;
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance839.BackColor = System.Drawing.SystemColors.Window;
            appearance839.BorderColor = System.Drawing.Color.Silver;
            this.cmbHandlingInstruction.DisplayLayout.Override.RowAppearance = appearance839;
            this.cmbHandlingInstruction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance840.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbHandlingInstruction.DisplayLayout.Override.TemplateAddRowAppearance = appearance840;
            this.cmbHandlingInstruction.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbHandlingInstruction.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbHandlingInstruction.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbHandlingInstruction.DisplayMember = "";
            this.cmbHandlingInstruction.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbHandlingInstruction.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbHandlingInstruction.Location = new System.Drawing.Point(122, 268);
            this.cmbHandlingInstruction.Name = "cmbHandlingInstruction";
            this.cmbHandlingInstruction.Size = new System.Drawing.Size(142, 21);
            this.cmbHandlingInstruction.TabIndex = 11;
            this.cmbHandlingInstruction.ValueMember = "";
            // 
            // cmbUserTradingAccount
            // 
            this.cmbUserTradingAccount.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance841.BackColor = System.Drawing.SystemColors.Window;
            appearance841.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUserTradingAccount.DisplayLayout.Appearance = appearance841;
            ultraGridBand71.ColHeadersVisible = false;
            ultraGridColumn253.Header.VisiblePosition = 0;
            ultraGridColumn253.Hidden = true;
            ultraGridColumn254.Header.VisiblePosition = 1;
            ultraGridBand71.Columns.AddRange(new object[] {
            ultraGridColumn253,
            ultraGridColumn254});
            ultraGridBand71.GroupHeadersVisible = false;
            this.cmbUserTradingAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand71);
            this.cmbUserTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUserTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance842.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance842.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance842.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance842.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance842;
            appearance843.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance843;
            this.cmbUserTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance844.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance844.BackColor2 = System.Drawing.SystemColors.Control;
            appearance844.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance844.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance844;
            this.cmbUserTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUserTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance845.BackColor = System.Drawing.SystemColors.Window;
            appearance845.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUserTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance845;
            appearance846.BackColor = System.Drawing.SystemColors.Highlight;
            appearance846.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUserTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance846;
            this.cmbUserTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUserTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance847.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUserTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance847;
            appearance848.BorderColor = System.Drawing.Color.Silver;
            appearance848.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUserTradingAccount.DisplayLayout.Override.CellAppearance = appearance848;
            this.cmbUserTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUserTradingAccount.DisplayLayout.Override.CellPadding = 0;
            appearance849.BackColor = System.Drawing.SystemColors.Control;
            appearance849.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance849.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance849.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance849.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance849;
            appearance850.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbUserTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance850;
            this.cmbUserTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUserTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance851.BackColor = System.Drawing.SystemColors.Window;
            appearance851.BorderColor = System.Drawing.Color.Silver;
            this.cmbUserTradingAccount.DisplayLayout.Override.RowAppearance = appearance851;
            this.cmbUserTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance852.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUserTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance852;
            this.cmbUserTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUserTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUserTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUserTradingAccount.DisplayMember = "";
            this.cmbUserTradingAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUserTradingAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUserTradingAccount.Location = new System.Drawing.Point(122, 291);
            this.cmbUserTradingAccount.Name = "cmbUserTradingAccount";
            this.cmbUserTradingAccount.Size = new System.Drawing.Size(142, 21);
            this.cmbUserTradingAccount.TabIndex = 12;
            this.cmbUserTradingAccount.ValueMember = "";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(6, 297);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(84, 12);
            this.label19.TabIndex = 3;
            this.label19.Text = "Trading A/C";
            // 
            // cmbUserAccount
            // 
            this.cmbUserAccount.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance853.BackColor = System.Drawing.SystemColors.Window;
            appearance853.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUserAccount.DisplayLayout.Appearance = appearance853;
            ultraGridBand72.ColHeadersVisible = false;
            ultraGridColumn255.Header.VisiblePosition = 0;
            ultraGridColumn255.Hidden = true;
            ultraGridColumn256.Header.VisiblePosition = 1;
            ultraGridBand72.Columns.AddRange(new object[] {
            ultraGridColumn255,
            ultraGridColumn256});
            ultraGridBand72.GroupHeadersVisible = false;
            this.cmbUserAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand72);
            this.cmbUserAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUserAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance854.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance854.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance854.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance854.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserAccount.DisplayLayout.GroupByBox.Appearance = appearance854;
            appearance855.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance855;
            this.cmbUserAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance856.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance856.BackColor2 = System.Drawing.SystemColors.Control;
            appearance856.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance856.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance856;
            this.cmbUserAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUserAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance857.BackColor = System.Drawing.SystemColors.Window;
            appearance857.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUserAccount.DisplayLayout.Override.ActiveCellAppearance = appearance857;
            appearance858.BackColor = System.Drawing.SystemColors.Highlight;
            appearance858.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUserAccount.DisplayLayout.Override.ActiveRowAppearance = appearance858;
            this.cmbUserAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUserAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance859.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUserAccount.DisplayLayout.Override.CardAreaAppearance = appearance859;
            appearance860.BorderColor = System.Drawing.Color.Silver;
            appearance860.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUserAccount.DisplayLayout.Override.CellAppearance = appearance860;
            this.cmbUserAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUserAccount.DisplayLayout.Override.CellPadding = 0;
            appearance861.BackColor = System.Drawing.SystemColors.Control;
            appearance861.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance861.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance861.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance861.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserAccount.DisplayLayout.Override.GroupByRowAppearance = appearance861;
            appearance862.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbUserAccount.DisplayLayout.Override.HeaderAppearance = appearance862;
            this.cmbUserAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUserAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance863.BackColor = System.Drawing.SystemColors.Window;
            appearance863.BorderColor = System.Drawing.Color.Silver;
            this.cmbUserAccount.DisplayLayout.Override.RowAppearance = appearance863;
            this.cmbUserAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance864.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUserAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance864;
            this.cmbUserAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUserAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUserAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUserAccount.DisplayMember = "";
            this.cmbUserAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUserAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUserAccount.Location = new System.Drawing.Point(122, 314);
            this.cmbUserAccount.Name = "cmbUserAccount";
            this.cmbUserAccount.Size = new System.Drawing.Size(142, 21);
            this.cmbUserAccount.TabIndex = 13;
            this.cmbUserAccount.ValueMember = "";
            // 
            // label26
            // 
            this.label26.Location = new System.Drawing.Point(6, 314);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(36, 12);
            this.label26.TabIndex = 3;
            this.label26.Text = "Account";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(6, 341);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(56, 12);
            this.label24.TabIndex = 3;
            this.label24.Text = "Strategy";
            // 
            // cmbStrategy
            // 
            this.cmbStrategy.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance865.BackColor = System.Drawing.SystemColors.Window;
            appearance865.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStrategy.DisplayLayout.Appearance = appearance865;
            ultraGridBand73.ColHeadersVisible = false;
            ultraGridColumn257.Header.VisiblePosition = 0;
            ultraGridColumn257.Hidden = true;
            ultraGridColumn258.Header.VisiblePosition = 1;
            ultraGridBand73.Columns.AddRange(new object[] {
            ultraGridColumn257,
            ultraGridColumn258});
            ultraGridBand73.GroupHeadersVisible = false;
            this.cmbStrategy.DisplayLayout.BandsSerializer.Add(ultraGridBand73);
            this.cmbStrategy.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStrategy.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance866.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance866.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance866.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance866.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.GroupByBox.Appearance = appearance866;
            appearance867.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.BandLabelAppearance = appearance867;
            this.cmbStrategy.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance868.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance868.BackColor2 = System.Drawing.SystemColors.Control;
            appearance868.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance868.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.PromptAppearance = appearance868;
            this.cmbStrategy.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStrategy.DisplayLayout.MaxRowScrollRegions = 1;
            appearance869.BackColor = System.Drawing.SystemColors.Window;
            appearance869.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStrategy.DisplayLayout.Override.ActiveCellAppearance = appearance869;
            appearance870.BackColor = System.Drawing.SystemColors.Highlight;
            appearance870.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStrategy.DisplayLayout.Override.ActiveRowAppearance = appearance870;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance871.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.CardAreaAppearance = appearance871;
            appearance872.BorderColor = System.Drawing.Color.Silver;
            appearance872.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStrategy.DisplayLayout.Override.CellAppearance = appearance872;
            this.cmbStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStrategy.DisplayLayout.Override.CellPadding = 0;
            appearance873.BackColor = System.Drawing.SystemColors.Control;
            appearance873.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance873.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance873.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance873.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.GroupByRowAppearance = appearance873;
            appearance874.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbStrategy.DisplayLayout.Override.HeaderAppearance = appearance874;
            this.cmbStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStrategy.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance875.BackColor = System.Drawing.SystemColors.Window;
            appearance875.BorderColor = System.Drawing.Color.Silver;
            this.cmbStrategy.DisplayLayout.Override.RowAppearance = appearance875;
            this.cmbStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance876.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStrategy.DisplayLayout.Override.TemplateAddRowAppearance = appearance876;
            this.cmbStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStrategy.DisplayMember = "";
            this.cmbStrategy.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbStrategy.Location = new System.Drawing.Point(122, 337);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.Size = new System.Drawing.Size(142, 21);
            this.cmbStrategy.TabIndex = 14;
            this.cmbStrategy.ValueMember = "";
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this.txtRandom);
            this.ultraGroupBox2.Controls.Add(this.txtDisplayQuantity);
            this.ultraGroupBox2.Controls.Add(this.txtDiscrOffSet);
            this.ultraGroupBox2.Controls.Add(this.txtPegOffSet);
            this.ultraGroupBox2.Controls.Add(this.label28);
            this.ultraGroupBox2.Controls.Add(this.label29);
            this.ultraGroupBox2.Controls.Add(this.label30);
            this.ultraGroupBox2.Controls.Add(this.label31);
            this.ultraGroupBox2.Controls.Add(this.cmbPNP);
            this.ultraGroupBox2.Controls.Add(this.label32);
            this.ultraGroupBox2.Location = new System.Drawing.Point(268, 372);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(278, 136);
            this.ultraGroupBox2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGroupBox2.TabIndex = 4;
            // 
            // txtRandom
            // 
            this.txtRandom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtRandom.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.PositiveInteger;
            this.txtRandom.Increment = 1;
            this.txtRandom.Location = new System.Drawing.Point(121, 82);
            this.txtRandom.MaxValue = 99999;
            this.txtRandom.MinValue = 0;
            this.txtRandom.Name = "txtRandom";
            this.txtRandom.Size = new System.Drawing.Size(142, 20);
            this.txtRandom.TabIndex = 3;
            this.txtRandom.Value = 0;
            // 
            // txtDisplayQuantity
            // 
            this.txtDisplayQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtDisplayQuantity.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.PositiveInteger;
            this.txtDisplayQuantity.Increment = 1;
            this.txtDisplayQuantity.Location = new System.Drawing.Point(121, 56);
            this.txtDisplayQuantity.MaxValue = 99999;
            this.txtDisplayQuantity.MinValue = 0;
            this.txtDisplayQuantity.Name = "txtDisplayQuantity";
            this.txtDisplayQuantity.Size = new System.Drawing.Size(142, 20);
            this.txtDisplayQuantity.TabIndex = 2;
            this.txtDisplayQuantity.Value = 0;
            // 
            // txtDiscrOffSet
            // 
            this.txtDiscrOffSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtDiscrOffSet.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.txtDiscrOffSet.Increment = 0.01;
            this.txtDiscrOffSet.Location = new System.Drawing.Point(121, 30);
            this.txtDiscrOffSet.MaxValue = 99999;
            this.txtDiscrOffSet.MinValue = -99999;
            this.txtDiscrOffSet.Name = "txtDiscrOffSet";
            this.txtDiscrOffSet.Size = new System.Drawing.Size(142, 20);
            this.txtDiscrOffSet.TabIndex = 1;
            this.txtDiscrOffSet.Value = 0;
            // 
            // txtPegOffSet
            // 
            this.txtPegOffSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtPegOffSet.DataType = Prana.Utilities.UI.UIUtilities.DataTypes.Numeric;
            this.txtPegOffSet.Increment = 0.01;
            this.txtPegOffSet.Location = new System.Drawing.Point(121, 5);
            this.txtPegOffSet.MaxValue = 99999;
            this.txtPegOffSet.MinValue = 0;
            this.txtPegOffSet.Name = "txtPegOffSet";
            this.txtPegOffSet.Size = new System.Drawing.Size(142, 20);
            this.txtPegOffSet.TabIndex = 0;
            this.txtPegOffSet.Value = 0;
            // 
            // label28
            // 
            this.label28.Location = new System.Drawing.Point(6, 86);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(106, 14);
            this.label28.TabIndex = 3;
            this.label28.Text = "Random Reserver";
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(6, 34);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(76, 12);
            this.label29.TabIndex = 3;
            this.label29.Text = "Discr. Offset";
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(6, 8);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(76, 14);
            this.label30.TabIndex = 3;
            this.label30.Text = "Peg Offset";
            // 
            // label31
            // 
            this.label31.Location = new System.Drawing.Point(6, 110);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(32, 14);
            this.label31.TabIndex = 3;
            this.label31.Text = "PNP";
            // 
            // cmbPNP
            // 
            this.cmbPNP.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance877.BackColor = System.Drawing.SystemColors.Window;
            appearance877.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbPNP.DisplayLayout.Appearance = appearance877;
            ultraGridBand74.ColHeadersVisible = false;
            ultraGridColumn259.Header.VisiblePosition = 0;
            ultraGridColumn260.Header.VisiblePosition = 1;
            ultraGridColumn260.Hidden = true;
            ultraGridBand74.Columns.AddRange(new object[] {
            ultraGridColumn259,
            ultraGridColumn260});
            ultraGridBand74.GroupHeadersVisible = false;
            this.cmbPNP.DisplayLayout.BandsSerializer.Add(ultraGridBand74);
            this.cmbPNP.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbPNP.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance878.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance878.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance878.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance878.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbPNP.DisplayLayout.GroupByBox.Appearance = appearance878;
            appearance879.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbPNP.DisplayLayout.GroupByBox.BandLabelAppearance = appearance879;
            this.cmbPNP.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance880.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance880.BackColor2 = System.Drawing.SystemColors.Control;
            appearance880.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance880.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbPNP.DisplayLayout.GroupByBox.PromptAppearance = appearance880;
            this.cmbPNP.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbPNP.DisplayLayout.MaxRowScrollRegions = 1;
            appearance881.BackColor = System.Drawing.SystemColors.Window;
            appearance881.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbPNP.DisplayLayout.Override.ActiveCellAppearance = appearance881;
            appearance882.BackColor = System.Drawing.SystemColors.Highlight;
            appearance882.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbPNP.DisplayLayout.Override.ActiveRowAppearance = appearance882;
            this.cmbPNP.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbPNP.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance883.BackColor = System.Drawing.SystemColors.Window;
            this.cmbPNP.DisplayLayout.Override.CardAreaAppearance = appearance883;
            appearance884.BorderColor = System.Drawing.Color.Silver;
            appearance884.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbPNP.DisplayLayout.Override.CellAppearance = appearance884;
            this.cmbPNP.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbPNP.DisplayLayout.Override.CellPadding = 0;
            appearance885.BackColor = System.Drawing.SystemColors.Control;
            appearance885.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance885.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance885.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance885.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbPNP.DisplayLayout.Override.GroupByRowAppearance = appearance885;
            appearance886.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbPNP.DisplayLayout.Override.HeaderAppearance = appearance886;
            this.cmbPNP.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbPNP.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance887.BackColor = System.Drawing.SystemColors.Window;
            appearance887.BorderColor = System.Drawing.Color.Silver;
            this.cmbPNP.DisplayLayout.Override.RowAppearance = appearance887;
            this.cmbPNP.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance888.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbPNP.DisplayLayout.Override.TemplateAddRowAppearance = appearance888;
            this.cmbPNP.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbPNP.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbPNP.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbPNP.DisplayMember = "Data";
            this.cmbPNP.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbPNP.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbPNP.Location = new System.Drawing.Point(120, 108);
            this.cmbPNP.Name = "cmbPNP";
            this.cmbPNP.Size = new System.Drawing.Size(142, 21);
            this.cmbPNP.TabIndex = 4;
            this.cmbPNP.ValueMember = "Value";
            // 
            // label32
            // 
            this.label32.Location = new System.Drawing.Point(6, 60);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(76, 14);
            this.label32.TabIndex = 3;
            this.label32.Text = "Display Qty.";
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this.cmbAgencyPrincipal);
            this.ultraGroupBox3.Controls.Add(this.label20);
            this.ultraGroupBox3.Controls.Add(this.cmbShortExempt);
            this.ultraGroupBox3.Controls.Add(this.label21);
            this.ultraGroupBox3.Controls.Add(this.label22);
            this.ultraGroupBox3.Controls.Add(this.cmbClientAccount);
            this.ultraGroupBox3.Controls.Add(this.label23);
            this.ultraGroupBox3.Controls.Add(this.cmbClientTrader);
            this.ultraGroupBox3.Controls.Add(this.label25);
            this.ultraGroupBox3.Controls.Add(this.cmbClientCompany);
            this.ultraGroupBox3.Controls.Add(this.label27);
            this.ultraGroupBox3.Controls.Add(this.cmbClearingFirmID);
            this.ultraGroupBox3.Location = new System.Drawing.Point(2, 138);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(264, 156);
            this.ultraGroupBox3.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGroupBox3.TabIndex = 2;
            // 
            // cmbAgencyPrincipal
            // 
            this.cmbAgencyPrincipal.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance889.BackColor = System.Drawing.SystemColors.Window;
            appearance889.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAgencyPrincipal.DisplayLayout.Appearance = appearance889;
            ultraGridBand75.ColHeadersVisible = false;
            ultraGridColumn261.Header.VisiblePosition = 0;
            ultraGridColumn262.Header.VisiblePosition = 1;
            ultraGridColumn262.Hidden = true;
            ultraGridBand75.Columns.AddRange(new object[] {
            ultraGridColumn261,
            ultraGridColumn262});
            ultraGridBand75.GroupHeadersVisible = false;
            this.cmbAgencyPrincipal.DisplayLayout.BandsSerializer.Add(ultraGridBand75);
            this.cmbAgencyPrincipal.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAgencyPrincipal.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance890.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance890.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance890.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance890.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAgencyPrincipal.DisplayLayout.GroupByBox.Appearance = appearance890;
            appearance891.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAgencyPrincipal.DisplayLayout.GroupByBox.BandLabelAppearance = appearance891;
            this.cmbAgencyPrincipal.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance892.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance892.BackColor2 = System.Drawing.SystemColors.Control;
            appearance892.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance892.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAgencyPrincipal.DisplayLayout.GroupByBox.PromptAppearance = appearance892;
            this.cmbAgencyPrincipal.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAgencyPrincipal.DisplayLayout.MaxRowScrollRegions = 1;
            appearance893.BackColor = System.Drawing.SystemColors.Window;
            appearance893.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAgencyPrincipal.DisplayLayout.Override.ActiveCellAppearance = appearance893;
            appearance894.BackColor = System.Drawing.SystemColors.Highlight;
            appearance894.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAgencyPrincipal.DisplayLayout.Override.ActiveRowAppearance = appearance894;
            this.cmbAgencyPrincipal.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbAgencyPrincipal.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance895.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAgencyPrincipal.DisplayLayout.Override.CardAreaAppearance = appearance895;
            appearance896.BorderColor = System.Drawing.Color.Silver;
            appearance896.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAgencyPrincipal.DisplayLayout.Override.CellAppearance = appearance896;
            this.cmbAgencyPrincipal.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAgencyPrincipal.DisplayLayout.Override.CellPadding = 0;
            appearance897.BackColor = System.Drawing.SystemColors.Control;
            appearance897.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance897.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance897.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance897.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAgencyPrincipal.DisplayLayout.Override.GroupByRowAppearance = appearance897;
            appearance898.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbAgencyPrincipal.DisplayLayout.Override.HeaderAppearance = appearance898;
            this.cmbAgencyPrincipal.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAgencyPrincipal.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance899.BackColor = System.Drawing.SystemColors.Window;
            appearance899.BorderColor = System.Drawing.Color.Silver;
            this.cmbAgencyPrincipal.DisplayLayout.Override.RowAppearance = appearance899;
            this.cmbAgencyPrincipal.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance900.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAgencyPrincipal.DisplayLayout.Override.TemplateAddRowAppearance = appearance900;
            this.cmbAgencyPrincipal.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAgencyPrincipal.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAgencyPrincipal.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAgencyPrincipal.DisplayMember = "Data2";
            this.cmbAgencyPrincipal.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAgencyPrincipal.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAgencyPrincipal.Location = new System.Drawing.Point(122, 78);
            this.cmbAgencyPrincipal.Name = "cmbAgencyPrincipal";
            this.cmbAgencyPrincipal.Size = new System.Drawing.Size(134, 21);
            this.cmbAgencyPrincipal.TabIndex = 3;
            this.cmbAgencyPrincipal.ValueMember = "Value2";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(6, 80);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(100, 14);
            this.label20.TabIndex = 3;
            this.label20.Text = "Agency Principal";
            // 
            // cmbShortExempt
            // 
            this.cmbShortExempt.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance901.BackColor = System.Drawing.SystemColors.Window;
            appearance901.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbShortExempt.DisplayLayout.Appearance = appearance901;
            ultraGridBand76.ColHeadersVisible = false;
            ultraGridColumn263.Header.VisiblePosition = 0;
            ultraGridColumn264.Header.VisiblePosition = 1;
            ultraGridColumn264.Hidden = true;
            ultraGridBand76.Columns.AddRange(new object[] {
            ultraGridColumn263,
            ultraGridColumn264});
            ultraGridBand76.GroupHeadersVisible = false;
            this.cmbShortExempt.DisplayLayout.BandsSerializer.Add(ultraGridBand76);
            this.cmbShortExempt.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbShortExempt.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance902.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance902.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance902.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance902.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbShortExempt.DisplayLayout.GroupByBox.Appearance = appearance902;
            appearance903.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbShortExempt.DisplayLayout.GroupByBox.BandLabelAppearance = appearance903;
            this.cmbShortExempt.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance904.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance904.BackColor2 = System.Drawing.SystemColors.Control;
            appearance904.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance904.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbShortExempt.DisplayLayout.GroupByBox.PromptAppearance = appearance904;
            this.cmbShortExempt.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbShortExempt.DisplayLayout.MaxRowScrollRegions = 1;
            appearance905.BackColor = System.Drawing.SystemColors.Window;
            appearance905.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbShortExempt.DisplayLayout.Override.ActiveCellAppearance = appearance905;
            appearance906.BackColor = System.Drawing.SystemColors.Highlight;
            appearance906.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbShortExempt.DisplayLayout.Override.ActiveRowAppearance = appearance906;
            this.cmbShortExempt.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbShortExempt.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance907.BackColor = System.Drawing.SystemColors.Window;
            this.cmbShortExempt.DisplayLayout.Override.CardAreaAppearance = appearance907;
            appearance908.BorderColor = System.Drawing.Color.Silver;
            appearance908.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbShortExempt.DisplayLayout.Override.CellAppearance = appearance908;
            this.cmbShortExempt.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbShortExempt.DisplayLayout.Override.CellPadding = 0;
            appearance909.BackColor = System.Drawing.SystemColors.Control;
            appearance909.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance909.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance909.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance909.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbShortExempt.DisplayLayout.Override.GroupByRowAppearance = appearance909;
            appearance910.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbShortExempt.DisplayLayout.Override.HeaderAppearance = appearance910;
            this.cmbShortExempt.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbShortExempt.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance911.BackColor = System.Drawing.SystemColors.Window;
            appearance911.BorderColor = System.Drawing.Color.Silver;
            this.cmbShortExempt.DisplayLayout.Override.RowAppearance = appearance911;
            this.cmbShortExempt.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance912.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbShortExempt.DisplayLayout.Override.TemplateAddRowAppearance = appearance912;
            this.cmbShortExempt.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbShortExempt.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbShortExempt.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbShortExempt.DisplayMember = "Data1";
            this.cmbShortExempt.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbShortExempt.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbShortExempt.Location = new System.Drawing.Point(122, 102);
            this.cmbShortExempt.Name = "cmbShortExempt";
            this.cmbShortExempt.Size = new System.Drawing.Size(134, 21);
            this.cmbShortExempt.TabIndex = 4;
            this.cmbShortExempt.ValueMember = "Value1";
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(6, 106);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(84, 12);
            this.label21.TabIndex = 3;
            this.label21.Text = "Short Exempt";
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point(6, 56);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(72, 14);
            this.label22.TabIndex = 3;
            this.label22.Text = "Client Account";
            // 
            // cmbClientAccount
            // 
            this.cmbClientAccount.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance913.BackColor = System.Drawing.SystemColors.Window;
            appearance913.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClientAccount.DisplayLayout.Appearance = appearance913;
            ultraGridBand77.ColHeadersVisible = false;
            ultraGridColumn265.Header.VisiblePosition = 0;
            ultraGridColumn265.Hidden = true;
            ultraGridColumn266.Header.VisiblePosition = 1;
            ultraGridColumn266.Hidden = true;
            ultraGridColumn267.Header.VisiblePosition = 2;
            ultraGridColumn268.Header.VisiblePosition = 3;
            ultraGridColumn268.Hidden = true;
            ultraGridBand77.Columns.AddRange(new object[] {
            ultraGridColumn265,
            ultraGridColumn266,
            ultraGridColumn267,
            ultraGridColumn268});
            this.cmbClientAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand77);
            this.cmbClientAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClientAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance914.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance914.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance914.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance914.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientAccount.DisplayLayout.GroupByBox.Appearance = appearance914;
            appearance915.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance915;
            this.cmbClientAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance916.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance916.BackColor2 = System.Drawing.SystemColors.Control;
            appearance916.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance916.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance916;
            this.cmbClientAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClientAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance917.BackColor = System.Drawing.SystemColors.Window;
            appearance917.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClientAccount.DisplayLayout.Override.ActiveCellAppearance = appearance917;
            appearance918.BackColor = System.Drawing.SystemColors.Highlight;
            appearance918.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClientAccount.DisplayLayout.Override.ActiveRowAppearance = appearance918;
            this.cmbClientAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClientAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance919.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClientAccount.DisplayLayout.Override.CardAreaAppearance = appearance919;
            appearance920.BorderColor = System.Drawing.Color.Silver;
            appearance920.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClientAccount.DisplayLayout.Override.CellAppearance = appearance920;
            this.cmbClientAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClientAccount.DisplayLayout.Override.CellPadding = 0;
            appearance921.BackColor = System.Drawing.SystemColors.Control;
            appearance921.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance921.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance921.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance921.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientAccount.DisplayLayout.Override.GroupByRowAppearance = appearance921;
            appearance922.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbClientAccount.DisplayLayout.Override.HeaderAppearance = appearance922;
            this.cmbClientAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClientAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance923.BackColor = System.Drawing.SystemColors.Window;
            appearance923.BorderColor = System.Drawing.Color.Silver;
            this.cmbClientAccount.DisplayLayout.Override.RowAppearance = appearance923;
            this.cmbClientAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance924.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClientAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance924;
            this.cmbClientAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClientAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClientAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClientAccount.DisplayMember = "";
            this.cmbClientAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClientAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClientAccount.Location = new System.Drawing.Point(122, 54);
            this.cmbClientAccount.Name = "cmbClientAccount";
            this.cmbClientAccount.Size = new System.Drawing.Size(134, 21);
            this.cmbClientAccount.TabIndex = 2;
            this.cmbClientAccount.ValueMember = "";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(6, 33);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(82, 14);
            this.label23.TabIndex = 3;
            this.label23.Text = "Client Trader";
            // 
            // cmbClientTrader
            // 
            this.cmbClientTrader.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance925.BackColor = System.Drawing.SystemColors.Window;
            appearance925.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClientTrader.DisplayLayout.Appearance = appearance925;
            ultraGridBand78.ColHeadersVisible = false;
            ultraGridColumn269.Header.VisiblePosition = 0;
            ultraGridColumn270.Header.VisiblePosition = 1;
            ultraGridColumn270.Hidden = true;
            ultraGridColumn271.Header.VisiblePosition = 2;
            ultraGridColumn271.Hidden = true;
            ultraGridColumn272.Header.VisiblePosition = 3;
            ultraGridColumn272.Hidden = true;
            ultraGridColumn273.Header.VisiblePosition = 4;
            ultraGridColumn273.Hidden = true;
            ultraGridColumn274.Header.VisiblePosition = 5;
            ultraGridColumn274.Hidden = true;
            ultraGridColumn275.Header.VisiblePosition = 6;
            ultraGridColumn275.Hidden = true;
            ultraGridColumn276.Header.VisiblePosition = 7;
            ultraGridColumn276.Hidden = true;
            ultraGridColumn277.Header.VisiblePosition = 8;
            ultraGridColumn277.Hidden = true;
            ultraGridColumn278.Header.VisiblePosition = 9;
            ultraGridColumn278.Hidden = true;
            ultraGridColumn279.Header.VisiblePosition = 10;
            ultraGridColumn279.Hidden = true;
            ultraGridColumn280.Header.VisiblePosition = 11;
            ultraGridColumn280.Hidden = true;
            ultraGridBand78.Columns.AddRange(new object[] {
            ultraGridColumn269,
            ultraGridColumn270,
            ultraGridColumn271,
            ultraGridColumn272,
            ultraGridColumn273,
            ultraGridColumn274,
            ultraGridColumn275,
            ultraGridColumn276,
            ultraGridColumn277,
            ultraGridColumn278,
            ultraGridColumn279,
            ultraGridColumn280});
            ultraGridBand78.GroupHeadersVisible = false;
            this.cmbClientTrader.DisplayLayout.BandsSerializer.Add(ultraGridBand78);
            this.cmbClientTrader.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClientTrader.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance926.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance926.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance926.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance926.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientTrader.DisplayLayout.GroupByBox.Appearance = appearance926;
            appearance927.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientTrader.DisplayLayout.GroupByBox.BandLabelAppearance = appearance927;
            this.cmbClientTrader.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance928.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance928.BackColor2 = System.Drawing.SystemColors.Control;
            appearance928.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance928.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientTrader.DisplayLayout.GroupByBox.PromptAppearance = appearance928;
            this.cmbClientTrader.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClientTrader.DisplayLayout.MaxRowScrollRegions = 1;
            appearance929.BackColor = System.Drawing.SystemColors.Window;
            appearance929.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClientTrader.DisplayLayout.Override.ActiveCellAppearance = appearance929;
            appearance930.BackColor = System.Drawing.SystemColors.Highlight;
            appearance930.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClientTrader.DisplayLayout.Override.ActiveRowAppearance = appearance930;
            this.cmbClientTrader.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClientTrader.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance931.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClientTrader.DisplayLayout.Override.CardAreaAppearance = appearance931;
            appearance932.BorderColor = System.Drawing.Color.Silver;
            appearance932.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClientTrader.DisplayLayout.Override.CellAppearance = appearance932;
            this.cmbClientTrader.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClientTrader.DisplayLayout.Override.CellPadding = 0;
            appearance933.BackColor = System.Drawing.SystemColors.Control;
            appearance933.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance933.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance933.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance933.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientTrader.DisplayLayout.Override.GroupByRowAppearance = appearance933;
            appearance934.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbClientTrader.DisplayLayout.Override.HeaderAppearance = appearance934;
            this.cmbClientTrader.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClientTrader.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance935.BackColor = System.Drawing.SystemColors.Window;
            appearance935.BorderColor = System.Drawing.Color.Silver;
            this.cmbClientTrader.DisplayLayout.Override.RowAppearance = appearance935;
            this.cmbClientTrader.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance936.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClientTrader.DisplayLayout.Override.TemplateAddRowAppearance = appearance936;
            this.cmbClientTrader.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClientTrader.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClientTrader.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClientTrader.DisplayMember = "";
            this.cmbClientTrader.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClientTrader.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClientTrader.Location = new System.Drawing.Point(122, 30);
            this.cmbClientTrader.Name = "cmbClientTrader";
            this.cmbClientTrader.Size = new System.Drawing.Size(134, 21);
            this.cmbClientTrader.TabIndex = 1;
            this.cmbClientTrader.ValueMember = "";
            // 
            // label25
            // 
            this.label25.Location = new System.Drawing.Point(6, 9);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(100, 14);
            this.label25.TabIndex = 3;
            this.label25.Text = "Client Company";
            // 
            // cmbClientCompany
            // 
            this.cmbClientCompany.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance937.BackColor = System.Drawing.SystemColors.Window;
            appearance937.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClientCompany.DisplayLayout.Appearance = appearance937;
            ultraGridBand79.ColHeadersVisible = false;
            ultraGridColumn281.Header.VisiblePosition = 0;
            ultraGridColumn281.Hidden = true;
            ultraGridColumn282.Header.VisiblePosition = 1;
            ultraGridColumn283.Header.VisiblePosition = 2;
            ultraGridColumn283.Hidden = true;
            ultraGridColumn284.Header.VisiblePosition = 3;
            ultraGridColumn284.Hidden = true;
            ultraGridColumn285.Header.VisiblePosition = 4;
            ultraGridColumn285.Hidden = true;
            ultraGridColumn286.Header.VisiblePosition = 5;
            ultraGridColumn286.Hidden = true;
            ultraGridColumn287.Header.VisiblePosition = 6;
            ultraGridColumn287.Hidden = true;
            ultraGridColumn288.Header.VisiblePosition = 7;
            ultraGridColumn288.Hidden = true;
            ultraGridColumn289.Header.VisiblePosition = 8;
            ultraGridColumn289.Hidden = true;
            ultraGridColumn290.Header.VisiblePosition = 9;
            ultraGridColumn290.Hidden = true;
            ultraGridColumn291.Header.VisiblePosition = 10;
            ultraGridColumn291.Hidden = true;
            ultraGridColumn292.Header.VisiblePosition = 11;
            ultraGridColumn292.Hidden = true;
            ultraGridColumn293.Header.VisiblePosition = 12;
            ultraGridColumn293.Hidden = true;
            ultraGridColumn294.Header.VisiblePosition = 13;
            ultraGridColumn294.Hidden = true;
            ultraGridColumn295.Header.VisiblePosition = 14;
            ultraGridColumn295.Hidden = true;
            ultraGridColumn296.Header.VisiblePosition = 15;
            ultraGridColumn296.Hidden = true;
            ultraGridColumn297.Header.VisiblePosition = 16;
            ultraGridColumn297.Hidden = true;
            ultraGridColumn298.Header.VisiblePosition = 17;
            ultraGridColumn298.Hidden = true;
            ultraGridColumn299.Header.VisiblePosition = 18;
            ultraGridColumn299.Hidden = true;
            ultraGridColumn300.Header.VisiblePosition = 19;
            ultraGridColumn300.Hidden = true;
            ultraGridBand79.Columns.AddRange(new object[] {
            ultraGridColumn281,
            ultraGridColumn282,
            ultraGridColumn283,
            ultraGridColumn284,
            ultraGridColumn285,
            ultraGridColumn286,
            ultraGridColumn287,
            ultraGridColumn288,
            ultraGridColumn289,
            ultraGridColumn290,
            ultraGridColumn291,
            ultraGridColumn292,
            ultraGridColumn293,
            ultraGridColumn294,
            ultraGridColumn295,
            ultraGridColumn296,
            ultraGridColumn297,
            ultraGridColumn298,
            ultraGridColumn299,
            ultraGridColumn300});
            this.cmbClientCompany.DisplayLayout.BandsSerializer.Add(ultraGridBand79);
            this.cmbClientCompany.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClientCompany.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance938.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance938.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance938.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance938.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientCompany.DisplayLayout.GroupByBox.Appearance = appearance938;
            appearance939.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientCompany.DisplayLayout.GroupByBox.BandLabelAppearance = appearance939;
            this.cmbClientCompany.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance940.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance940.BackColor2 = System.Drawing.SystemColors.Control;
            appearance940.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance940.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientCompany.DisplayLayout.GroupByBox.PromptAppearance = appearance940;
            this.cmbClientCompany.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClientCompany.DisplayLayout.MaxRowScrollRegions = 1;
            appearance941.BackColor = System.Drawing.SystemColors.Window;
            appearance941.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClientCompany.DisplayLayout.Override.ActiveCellAppearance = appearance941;
            appearance942.BackColor = System.Drawing.SystemColors.Highlight;
            appearance942.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClientCompany.DisplayLayout.Override.ActiveRowAppearance = appearance942;
            this.cmbClientCompany.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClientCompany.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance943.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClientCompany.DisplayLayout.Override.CardAreaAppearance = appearance943;
            appearance944.BorderColor = System.Drawing.Color.Silver;
            appearance944.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClientCompany.DisplayLayout.Override.CellAppearance = appearance944;
            this.cmbClientCompany.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClientCompany.DisplayLayout.Override.CellPadding = 0;
            appearance945.BackColor = System.Drawing.SystemColors.Control;
            appearance945.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance945.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance945.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance945.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientCompany.DisplayLayout.Override.GroupByRowAppearance = appearance945;
            appearance946.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbClientCompany.DisplayLayout.Override.HeaderAppearance = appearance946;
            this.cmbClientCompany.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClientCompany.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance947.BackColor = System.Drawing.SystemColors.Window;
            appearance947.BorderColor = System.Drawing.Color.Silver;
            this.cmbClientCompany.DisplayLayout.Override.RowAppearance = appearance947;
            this.cmbClientCompany.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance948.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClientCompany.DisplayLayout.Override.TemplateAddRowAppearance = appearance948;
            this.cmbClientCompany.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClientCompany.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClientCompany.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClientCompany.DisplayMember = "";
            this.cmbClientCompany.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClientCompany.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClientCompany.Location = new System.Drawing.Point(122, 6);
            this.cmbClientCompany.Name = "cmbClientCompany";
            this.cmbClientCompany.Size = new System.Drawing.Size(134, 21);
            this.cmbClientCompany.TabIndex = 0;
            this.cmbClientCompany.ValueMember = "";
            this.cmbClientCompany.ValueChanged += new System.EventHandler(this.cmbClientCompany_ValueChanged);
            // 
            // label27
            // 
            this.label27.Location = new System.Drawing.Point(6, 128);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(100, 14);
            this.label27.TabIndex = 3;
            this.label27.Text = "Clearing Firm ID";
            // 
            // cmbClearingFirmID
            // 
            this.cmbClearingFirmID.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance949.BackColor = System.Drawing.SystemColors.Window;
            appearance949.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClearingFirmID.DisplayLayout.Appearance = appearance949;
            ultraGridBand80.ColHeadersVisible = false;
            ultraGridColumn301.Header.VisiblePosition = 0;
            ultraGridColumn301.Hidden = true;
            ultraGridColumn302.Header.VisiblePosition = 1;
            ultraGridColumn303.Header.VisiblePosition = 2;
            ultraGridColumn303.Hidden = true;
            ultraGridColumn304.Header.VisiblePosition = 3;
            ultraGridColumn304.Hidden = true;
            ultraGridBand80.Columns.AddRange(new object[] {
            ultraGridColumn301,
            ultraGridColumn302,
            ultraGridColumn303,
            ultraGridColumn304});
            this.cmbClearingFirmID.DisplayLayout.BandsSerializer.Add(ultraGridBand80);
            this.cmbClearingFirmID.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirmID.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance950.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance950.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance950.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance950.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmID.DisplayLayout.GroupByBox.Appearance = appearance950;
            appearance951.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClearingFirmID.DisplayLayout.GroupByBox.BandLabelAppearance = appearance951;
            this.cmbClearingFirmID.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance952.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance952.BackColor2 = System.Drawing.SystemColors.Control;
            appearance952.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance952.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClearingFirmID.DisplayLayout.GroupByBox.PromptAppearance = appearance952;
            this.cmbClearingFirmID.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClearingFirmID.DisplayLayout.MaxRowScrollRegions = 1;
            appearance953.BackColor = System.Drawing.SystemColors.Window;
            appearance953.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClearingFirmID.DisplayLayout.Override.ActiveCellAppearance = appearance953;
            appearance954.BackColor = System.Drawing.SystemColors.Highlight;
            appearance954.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClearingFirmID.DisplayLayout.Override.ActiveRowAppearance = appearance954;
            this.cmbClearingFirmID.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClearingFirmID.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance955.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmID.DisplayLayout.Override.CardAreaAppearance = appearance955;
            appearance956.BorderColor = System.Drawing.Color.Silver;
            appearance956.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClearingFirmID.DisplayLayout.Override.CellAppearance = appearance956;
            this.cmbClearingFirmID.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClearingFirmID.DisplayLayout.Override.CellPadding = 0;
            appearance957.BackColor = System.Drawing.SystemColors.Control;
            appearance957.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance957.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance957.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance957.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClearingFirmID.DisplayLayout.Override.GroupByRowAppearance = appearance957;
            appearance958.TextHAlign = Infragistics.Win.HAlign.Left;
            this.cmbClearingFirmID.DisplayLayout.Override.HeaderAppearance = appearance958;
            this.cmbClearingFirmID.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClearingFirmID.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance959.BackColor = System.Drawing.SystemColors.Window;
            appearance959.BorderColor = System.Drawing.Color.Silver;
            this.cmbClearingFirmID.DisplayLayout.Override.RowAppearance = appearance959;
            this.cmbClearingFirmID.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance960.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClearingFirmID.DisplayLayout.Override.TemplateAddRowAppearance = appearance960;
            this.cmbClearingFirmID.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClearingFirmID.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClearingFirmID.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClearingFirmID.DisplayMember = "";
            this.cmbClearingFirmID.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClearingFirmID.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClearingFirmID.Location = new System.Drawing.Point(122, 126);
            this.cmbClearingFirmID.Name = "cmbClearingFirmID";
            this.cmbClearingFirmID.Size = new System.Drawing.Size(134, 21);
            this.cmbClearingFirmID.TabIndex = 5;
            this.cmbClearingFirmID.ValueMember = "";
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(195, 511);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(276, 511);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label13
            // 
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(5, 494);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 14);
            this.label13.TabIndex = 35;
            this.label13.Text = "*Required Fields";
            // 
            // groupBoxDefaultCV
            // 
            this.groupBoxHOtCold.Controls.Add(this.label1);
            this.groupBoxHOtCold.Controls.Add(this.btnCold);
            this.groupBoxHOtCold.Controls.Add(this.btnHot);
            this.groupBoxHOtCold.Controls.Add(this.lblHotCold);
            this.groupBoxHOtCold.Location = new System.Drawing.Point(4, 97);
            this.groupBoxHOtCold.Name = "groupBoxDefaultCV";
            this.groupBoxHOtCold.Size = new System.Drawing.Size(257, 27);
            this.groupBoxHOtCold.TabIndex = 36;
            this.groupBoxHOtCold.TabStop = false;
            // 
            // btnCold
            // 
            this.btnCold.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCold.Location = new System.Drawing.Point(189, 9);
            this.btnCold.Name = "btnCold";
            this.btnCold.Size = new System.Drawing.Size(55, 16);
            this.btnCold.TabIndex = 23;
            this.btnCold.Text = "Cold";
            // 
            // btnHot
            // 
            this.btnHot.Checked = true;
            this.btnHot.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnHot.Location = new System.Drawing.Point(118, 9);
            this.btnHot.Name = "btnHot";
            this.btnHot.Size = new System.Drawing.Size(47, 16);
            this.btnHot.TabIndex = 22;
            this.btnHot.TabStop = true;
            this.btnHot.Text = "Hot";
            // 
            // lblHotCold
            // 
            this.lblHotCold.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblHotCold.Location = new System.Drawing.Point(6, 9);
            this.lblHotCold.Name = "lblHotCold";
            this.lblHotCold.Size = new System.Drawing.Size(77, 15);
            this.lblHotCold.TabIndex = 32;
            this.lblHotCold.Text = "Hot/Cold";
            this.lblHotCold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(94, -2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(10, 7);
            this.label1.TabIndex = 37;
            this.label1.Text = "*";
            // 
            // NewTicketCustomSetting
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(548, 541);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox1);
            this.Controls.Add(this.ultraGroupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.MaximizeBox = false;
            this.Name = "NewTicketCustomSetting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Action Button";
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbButtonColor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbLimitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnderLying)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserTradingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPNP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAgencyPrincipal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbShortExempt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientTrader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientCompany)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirmID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBoxHOtCold.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Bind PNP, AgencyPrincipal, ShortExempt (HardCoded using DataTables)
        /// <summary>
        /// Bind Combo PNP(Yes/No), ShortExempt(Yes/No), AgencyPrincipal(Agency/Principal/None)
        /// </summary>
        private void BindYesNo()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dt1 = new System.Data.DataTable();
            System.Data.DataTable dt2 = new System.Data.DataTable();
            //System.Data.DataTable dt3 = new System.Data.DataTable();


            dt.Columns.Add("Data");
            dt.Columns.Add("Value");
            dt.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
            dt.Rows.Add(new object[2] { "Yes", 1 });
            dt.Rows.Add(new object[2] { "No", 0 });
            cmbPNP.DataSource = null;
            cmbPNP.DataSource = dt;
            cmbPNP.DisplayMember = "Data";
            cmbPNP.ValueMember = "Value";
            cmbPNP.Value = int.MinValue;

            dt1.Columns.Add("Data1");
            dt1.Columns.Add("Value1");
            dt1.Rows.Add(new object[2] { "Yes", 1 });
            dt1.Rows.Add(new object[2] { "No", 0 });
            dt1.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
            cmbShortExempt.DataSource = null;
            cmbShortExempt.DataSource = dt1;
            cmbShortExempt.DisplayMember = "Data1";
            cmbShortExempt.ValueMember = "Value1";
            cmbShortExempt.Value = int.MinValue;


            dt2.Columns.Add("Data2");
            dt2.Columns.Add("Value2");
            dt2.Rows.Add(new object[2] { "Agency", 1 });
            dt2.Rows.Add(new object[2] { "Principal", 0 });
            dt2.Rows.Add(new object[2] { ApplicationConstants.C_COMBO_SELECT, int.MinValue });
            cmbAgencyPrincipal.DataSource = null;
            cmbAgencyPrincipal.DataSource = dt2;
            cmbAgencyPrincipal.DisplayMember = "Data2";
            cmbAgencyPrincipal.ValueMember = "Value2";
            cmbAgencyPrincipal.Value = int.MinValue;

            //dt3.Columns.Add("Data3");
            //dt3.Columns.Add("Value3");
            //dt3.Rows.Add(new object[2] {"Montage",0});
            //dt3.Rows.Add(new object[2] {"Ticket",1});
            //dt3.Rows.Add(new object[2] {"FastTicket",2});
            //dt3.Rows.Add(new object[2] {ApplicationConstants.C_COMBO_SELECT, int.MinValue});
            //cmbDefaultTicket.DataSource = dt3;
            //cmbDefaultTicket.DisplayMember = "Data3";
            //cmbDefaultTicket.ValueMember = "Value3";
            //cmbDefaultTicket.Value=int.MinValue;
        }
        #endregion

        #region Btn Close and Save

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
                //TODO: this.Dispose(true);
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

            }

        }
        /// <summary>
        /// Save details of the form to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {

                if (!GetTradingTicketData())
                {
                    return;
                }

                this.DialogResult = DialogResult.None;
                if (_actionButtonDefinition != null)
                {
                    //_actionButtonDefinition.Display = _toDisplay;
                    //IF not null update old to new. 

                    // Also return Dialogresult.OK
                    this.DialogResult = DialogResult.OK;

                    //this.Close();
                    //call this.Dispose
                }
                else
                {
                    this.DialogResult = DialogResult.None;
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
            finally
            {

            }

        }
        #endregion

        /// <summary>
        /// Refreshes the Error Providers to null
        /// </summary>
        public void RefreshForm()
        {

            errorProvider.SetError(cmbSide, "");
            errorProvider.SetError(cmbAsset, "");
            errorProvider.SetError(cmbUnderLying, "");
            errorProvider.SetError(cmbCounterParty, "");
            errorProvider.SetError(cmbVenue, "");
            errorProvider.SetError(cmbOrderType, "");
            errorProvider.SetError(cmbExecutionInstruction, "");
            errorProvider.SetError(cmbHandlingInstruction, "");
            errorProvider.SetError(cmbTIF, "");
            errorProvider.SetError(cmbUserTradingAccount, "");
            errorProvider.SetError(cmbHandlingInstruction, "");
            //errorProvider.SetError(txtActionButtonDisplayName, "");
            //errorProvider.SetError(cmbActionButton, "");
            //errorProvider.SetError(cmbDefaultTicket, "");
            errorProvider.SetError(cmbClientCompany, "");
            errorProvider.SetError(cmbClientAccount, "");
            errorProvider.SetError(cmbClientTrader, "");
            errorProvider.SetError(cmbClearingFirmID, "");
            errorProvider.SetError(cmbShortExempt, "");
            errorProvider.SetError(cmbAgencyPrincipal, "");
            errorProvider.SetError(txtName, "");
            errorProvider.SetError(txtDescription, "");
            //errorProvider.SetError(txtActionButtonDisplayName, "");		
            errorProvider.SetError(txtDiscrOffSet, "");
            errorProvider.SetError(txtPegOffSet, "");
            errorProvider.SetError(txtDisplayQuantity, "");
            errorProvider.SetError(txtQuantity, "");
            errorProvider.SetError(txtRandom, "");
            errorProvider.SetError(cmbUserTradingAccount, "");
            //errorProvider.SetError(cmbDisplayPosition, "");
            errorProvider.SetError(cmbLimitPrice, "");
        }


        #region Property ID of New Custom Settings Grid Call SetTradingTicket
        public string _ID = string.Empty;
        /// <summary>
        /// ID refers to the TradingTicketSettingID. Needed when modifications are to be made in a ticketSetting.
        /// For a particular Ticket SettingID the property gets the object from the database and sets it into
        /// the Form. After making neccessary modifications the object is saved back to the same TicketSettingsID
        /// </summary>
        public string ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                //TODO : check whether commenting is fine
                TradingTicketSettings tradingTicketIDSettings = new TradingTicketSettings();
                tradingTicketIDSettings = TradingTicketManager.GetInstance().GetTradingTicketIDSettings(_ID);
                SetTradingTicket(tradingTicketIDSettings);

            }
        }

        #endregion

        #region Value Changed Events

        private void cmbSide_ValueChanged(object sender, System.EventArgs e)
        {
            if (cmbSide.Value == null)
                return;
            //if ( (cmbSide.Value.ToString().Equals("2")) || (cmbSide.Value.ToString() > 3 && int.Parse(cmbSide.Value.ToString()) < 7 ))
            //{
            //    this.cmbClientCompany.Enabled = true;
            //    this.cmbClientAccount.Enabled= true;
            //    this.cmbClientTrader.Enabled= true;
            //    this.cmbAgencyPrincipal.Enabled= true;
            //    this.cmbShortExempt.Enabled = true;
            //    this.cmbClearingFirmID.Enabled= true;
            //}
            //else
            //{
            this.cmbClientCompany.Enabled = false;
            this.cmbClientAccount.Enabled = false;
            this.cmbClientTrader.Enabled = false;
            this.cmbAgencyPrincipal.Enabled = false;
            this.cmbShortExempt.Enabled = false;
            this.cmbClearingFirmID.Enabled = false;
            //			}
        }
        private void cmbAsset_ValueChanged(object sender, System.EventArgs e)
        {
            int assetID = Convert.ToInt32(cmbAsset.Value);
            BindUnderlyingCombo(assetID);
        }

        private void cmbUnderLying_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbUnderLying.Value != null)
                {
                    BindUserCounterParty();
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
        private void cmbCounterParty_ValueChanged(object sender, System.EventArgs e)
        {//Combo Venue can only be bound if CounterParty is selected

            // change to be made in this... suppose venue is selected and this value is changed
            // then binding function should be called for other combos as well ...(( i.e when 
            // value is not null for venue and counterparty is changed ....)) 
            try
            {
                if (cmbCounterParty.Value == null)
                    return;
                int counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                BindVenue(counterPartyID);
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
        private void cmbVenue_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {

                if (cmbCounterParty.Value == null || cmbVenue.Value == null)
                    return;
                BindHandlingInstr(int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                BindSideCombo();
                BindOrderType(int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                BindTIF(int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                BindExecInstr(int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
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

        private void cmbOrderType_ValueChanged(object sender, EventArgs e)
        {
            if (cmbOrderType.Value == null)
            {
                return;
            }
            if (!cmbOrderType.Value.Equals(ApplicationConstants.C_COMBO_SELECT))
            {
                if (IfLimitPriceRequired()) //"4"for Stop Limit
                {
                    cmbLimitPrice.Enabled = true;
                    txtLimitOffset.Enabled = true;
                }
                else
                {
                    cmbLimitPrice.Value = int.MinValue;
                    cmbLimitPrice.Enabled = false;
                    txtLimitOffset.Value = 0.0;
                    txtLimitOffset.Enabled = false;
                }

                if (cmbOrderType.Value.ToString() == FIXConstants.ORDTYPE_Limit)
                {
                    SetDefaultValuesForOrderType();
                }
            }
        }
        /// <summary>
        /// Bind Combos dependent of Company Client Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbClientCompany_ValueChanged(object sender, System.EventArgs e)
        {
            // The following Combos can only be bound if Company Client is selected
            BindClientAccounts(int.Parse(cmbClientCompany.Value.ToString()));

            BindCompanyClientTraders(int.Parse(cmbClientCompany.Value.ToString()));
        }
        #endregion

        #region GetTradingTicketData from the form

        public bool GetTradingTicketData()
        {

            RefreshForm();

            #region Error Provider
            if (txtName.Text.ToString().Trim() == "")
            {
                errorProvider.SetError(txtName, "Please Enter  Name!");
                txtName.Focus();
                return false;

            }
            else if (txtName.Text.ToString().Trim() == "Define")
            {
                errorProvider.SetError(txtName, "This corresponds to undefined Buttons! Please Enter a different name.");
                txtName.Focus();
                return false;

            }
            else if (DisplayNameOverlaps())
            {
                errorProvider.SetError(txtName, "This name already exists! Please Enter a different name.");
                txtName.Focus();
                return false;

            }
            else if (txtDescription.Text.ToString().Trim() == "")
            {
                errorProvider.SetError(txtDescription, "Please Enter Description!");
                txtDescription.Focus();
                return false;

            }


            else if (cmbSide.Value.Equals(ApplicationConstants.C_COMBO_SELECT))
            {
                errorProvider.SetError(cmbSide, "Please select Side Type!");
                cmbSide.Focus();
                return false;
            }
            else if (Convert.ToInt32(cmbAsset.Value) == int.MinValue)
            {
                errorProvider.SetError(cmbAsset, "Please select Asset !");
                cmbAsset.Focus();
                return false;
            }
            else if (Convert.ToInt32(cmbUnderLying.Value) == int.MinValue)
            {
                errorProvider.SetError(cmbUnderLying, "Please select UnderLying!");
                cmbUnderLying.Focus();
                return false;
            }

            else if (Convert.ToInt32(cmbCounterParty.Value) == int.MinValue)
            {
                errorProvider.SetError(cmbCounterParty, "Please select CounterParty!");
                cmbCounterParty.Focus();
                return false;
            }

            else if (Convert.ToInt32(cmbVenue.Value) == int.MinValue)
            {
                errorProvider.SetError(cmbVenue, "Please select Venue !");
                cmbVenue.Focus();
                return false;
            }
            else if (cmbOrderType.Value.ToString() == ApplicationConstants.C_COMBO_SELECT)
            {
                errorProvider.SetError(cmbOrderType, "Please select OrderType!");
                cmbOrderType.Focus();
                return false;
            }
            else if (!RegularExpressionValidation.IsInteger(txtQuantity.Value.ToString()))
            {
                errorProvider.SetError(txtQuantity, "Please Enter Numeric Value!");
                txtQuantity.Focus();
                return false;
            }
            else if (cmbTIF.Value.ToString() == ApplicationConstants.C_COMBO_SELECT)
            {
                errorProvider.SetError(cmbTIF, "Please select Time In Force!");
                cmbTIF.Focus();
                return false;
            }

            else if (cmbExecutionInstruction.Value.ToString() == ApplicationConstants.C_COMBO_SELECT)
            {
                errorProvider.SetError(cmbExecutionInstruction, "Please select ExecutionInstruction!");
                cmbExecutionInstruction.Focus();
                return false;
            }
            else if (cmbHandlingInstruction.Value.ToString() == ApplicationConstants.C_COMBO_SELECT)
            {
                errorProvider.SetError(cmbHandlingInstruction, "Please select Handling Instruction!");
                cmbHandlingInstruction.Focus();
                return false;
            }
            else if (txtPegOffSet.Text.Trim() != string.Empty && !RegularExpressionValidation.IsPositiveNumber(txtPegOffSet.Text.Trim()))
            {

                errorProvider.SetError(txtPegOffSet, "Please Enter Positive Number!");
                txtPegOffSet.Focus();
                return false;

            }
            else if (txtDiscrOffSet.Text.Trim() != string.Empty && !RegularExpressionValidation.IsPositiveNumber(txtDiscrOffSet.Text.Trim()))
            {

                errorProvider.SetError(txtDiscrOffSet, "Please Enter Positive Number!");
                txtDiscrOffSet.Focus();
                return false;

            }
            else if (txtDisplayQuantity.Text.Trim() != string.Empty && !RegularExpressionValidation.IsPositiveInteger(txtDisplayQuantity.Text.Trim()))
            {

                errorProvider.SetError(txtDisplayQuantity, "Please Enter Positive Integer!");
                txtDisplayQuantity.Focus();
                return false;

            }
            else if (txtRandom.Text.Trim() != string.Empty && !RegularExpressionValidation.IsPositiveInteger(txtRandom.Text.Trim()))
            {

                errorProvider.SetError(txtRandom, "Please Enter Positive Integer!");
                txtRandom.Focus();
                return false;

            }
            else if (cmbUserTradingAccount.Value.Equals(int.MinValue))
            {
                errorProvider.SetError(cmbUserTradingAccount, "Please select Trading Account!");
                cmbUserTradingAccount.Focus();
                return false;
            }
            //else if (cmbDisplayPosition.Value.Equals(int.MinValue))
            //{
            //    errorProvider.SetError(cmbDisplayPosition, "Please select Display Position!");
            //    cmbDisplayPosition.Focus();
            //    return null;
            //}
            else if (IfLimitPriceRequired() && cmbLimitPrice.Value.Equals(int.MinValue))//"4"for Stop Limit)
            {
                errorProvider.SetError(cmbLimitPrice, "Please select Limit Price!");
                cmbLimitPrice.Focus();
                return false;
            }

            else
            {
                //if (checkTicketSetting.Checked == true)
                //{
                //    if (Convert.ToInt32(cmbDefaultTicket.Value) == int.MinValue)
                //    {
                //        errorProvider.SetError(cmbDefaultTicket, "Please Enter Default TicketType!");
                //        cmbDefaultTicket.Focus();
                //        return null;
                //    }
                //    else
                //        tradingTicketSettings.DefaultTicketID = int.Parse(cmbDefaultTicket.Value.ToString());
                //}

                //if (checkActionButton.Checked == true)
                //{
                //    if (txtActionButtonDisplayName.Text.ToString().Trim() == "")
                //    {
                //        errorProvider.SetError(txtActionButtonDisplayName, "Please Enter  Name!");
                //        txtActionButtonDisplayName.Focus();
                //        return null;

                //    }

                //    else
                //    {
                //        tradingTicketSettings.ButtonColor = cmbButtonColor.Color.R + "," + cmbButtonColor.Color.G + "," + cmbButtonColor.Color.B;
                //        tradingTicketSettings.DisplayName = txtActionButtonDisplayName.Text.ToString();
                //    }
                //}
                _actionButtonDefinition.ButtonColor = string.Empty;
                string color = cmbButtonColor.Color.R.ToString() + "," + cmbButtonColor.Color.G.ToString() + "," + cmbButtonColor.Color.B.ToString();
                _actionButtonDefinition.ButtonColor = color;

                if (ID != string.Empty)
                    _actionButtonDefinition.TicketSettingsID = _ID;
                else
                    _actionButtonDefinition.TicketSettingsID = System.Guid.NewGuid().ToString();


                //if (checkTicketSetting.Checked == true)
                //{
                //    if (checkActionButton.Checked == true)
                //        tradingTicketSettings.SettingType = 3;
                //    else
                //        tradingTicketSettings.SettingType = 1;
                //}
                //else
                //{
                //    if (checkActionButton.Checked == true)
                //        tradingTicketSettings.SettingType = 2;
                //    else
                //        MessageBox.Show("Please Select the Setting Type");
                //}

                #region cmbClearingFirmID,cmbShortExempt
                if ((cmbSide.Value.ToString() == FIXConstants.SIDE_Sell) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellPlus) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShort) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShortExempt))
                {
                    #region commented code
                    //if(Convert.ToInt32( cmbClearingFirmID.Value)  == int.MinValue )
                    //{
                    //    //errorProvider.SetError(cmbClearingFirmID, "Please select Company Clearing Firm");
                    //    //cmbClearingFirmID.Focus();
                    //    //return null;
                    //}
                    ////else if( Convert.ToInt32(cmbShortExempt.Value ) == int.MinValue )
                    ////{
                    ////    errorProvider.SetError(cmbShortExempt, "Please select Yes/No !");
                    ////    cmbShortExempt.Focus();
                    ////    return null;
                    ////}
                    //else 
                    //{						
                    //    //tradingTicketSettings.ShortExempt= int.Parse(cmbShortExempt.Value.ToString());
                    //    //tradingTicketSettings.ClearingFirmID= int.Parse(cmbClearingFirmID.Value.ToString());
                    //}				
                    #endregion
                    //tradingTicketSettings.Principal = int.Parse(cmbAgencyPrincipal.Value.ToString());
                    //tradingTicketSettings.ClientCompanyID = int.Parse(cmbClientCompany.Value.ToString());
                    //tradingTicketSettings.ClientAccountID = int.Parse(cmbClientAccount.Value.ToString());
                    //tradingTicketSettings.ClientTraderID = int.Parse(cmbClientTrader.Value.ToString());

                }

                else
                {
                    //tradingTicketSettings.ClientTraderID = int.MinValue;
                    //tradingTicketSettings.ClientAccountID = int.MinValue;
                    //tradingTicketSettings.Principal = int.MinValue;
                    //tradingTicketSettings.ShortExempt = int.MinValue;
                    //tradingTicketSettings.ClearingFirmID = int.MinValue;
                    //tradingTicketSettings.ClientCompanyID = int.MinValue;
                }
                #endregion
            }
            #endregion

            #region value Setting
            _actionButtonDefinition.AssetID = AssetID;
            _actionButtonDefinition.UnderLyingID = UnderLyingID;
            _actionButtonDefinition.AUECID = AUECID;
            _actionButtonDefinition.ButtonPosition = ButtonPosition;
            if (btnHot.Checked)
            {
                _actionButtonDefinition.IsHotButton = true;
            }
            else
            {
                _actionButtonDefinition.IsHotButton = false;
            }

            _actionButtonDefinition.Name = txtName.Text.ToString().Trim();
            _actionButtonDefinition.Description = txtDescription.Text.ToString().Trim();
            _actionButtonDefinition.SideID = cmbSide.Value.ToString();

            _actionButtonDefinition.CounterpartyID = int.Parse(cmbCounterParty.Value.ToString());
            _actionButtonDefinition.VenueID = int.Parse(cmbVenue.Value.ToString());
            _actionButtonDefinition.Quantity = int.Parse(txtQuantity.Value.ToString());
            _actionButtonDefinition.OrderTypeID = cmbOrderType.Value.ToString();

            _actionButtonDefinition.TIF = int.Parse(cmbTIF.Value.ToString());
            _actionButtonDefinition.ExecutionInstructionID = cmbExecutionInstruction.Value.ToString();
            _actionButtonDefinition.HandlingInstructionID = cmbHandlingInstruction.Value.ToString();
            _actionButtonDefinition.AccountID = int.Parse(cmbUserAccount.Value.ToString());
            _actionButtonDefinition.StrategyID = int.Parse(cmbStrategy.Value.ToString());
            _actionButtonDefinition.TradingAccountID = int.Parse(cmbUserTradingAccount.Value.ToString());
            _actionButtonDefinition.CompanyUserID = _loginUser.CompanyUserID;
            //if (txtPegOffSet.Text.Trim() != string.Empty)
            //{
            //    tradingTicketSettings.Peg = double.Parse(txtPegOffSet.Text.ToString());
            //}
            //else
            //{
            //    tradingTicketSettings.Peg = int.MinValue;
            //}
            _actionButtonDefinition.Peg = txtPegOffSet.Value;
            _actionButtonDefinition.DiscreationOffset = txtDiscrOffSet.Value;
            _actionButtonDefinition.DisplayQuantity = (int)txtDisplayQuantity.Value;
            _actionButtonDefinition.Random = (int)txtRandom.Value;

            _actionButtonDefinition.PNP = int.Parse(cmbPNP.Value.ToString());

            // added for 2.0.1.4
            //tradingTicketSettings.DisplayPosition = int.Parse(cmbDisplayPosition.Value.ToString());
            if (cmbLimitPrice.Value != null)
                _actionButtonDefinition.LimitType = int.Parse(cmbLimitPrice.Value.ToString());
            //if(txtLimitOffset.Value!=null)
            _actionButtonDefinition.LimitOffset = double.Parse(txtLimitOffset.Value.ToString());
            //GetOpenCloseField(tradingTicketSettings);
            //tradingTicketSettings.ButtonColor = cmbButtonColor.Value;
            #endregion
            //_actionButtonDefinition = tradingTicketSettings;
            return true;
        }

        public TradingTicketSettings GetTradingTicket()
        {
            return _actionButtonDefinition;
        }

        #endregion

        #region SetTradingTicketForm with values from Parent Form
        public bool SetTradingTicket(TradingTicketSettings _tradingTicket)
        {
            bool result = false;
            _actionButtonDefinition = _tradingTicket;
            if (_existingNames.Count > 0)
            {
                _selectedButtonName = _tradingTicket.Name;
                _existingNames.Remove(_tradingTicket.Name);
            }
            _ID = _actionButtonDefinition.TicketSettingsID;
            AUECID = _actionButtonDefinition.AUECID;
            ButtonPosition = _actionButtonDefinition.ButtonPosition;

            txtName.Value = _actionButtonDefinition.Name;
            txtDescription.Value = _actionButtonDefinition.Description;
            string s = _actionButtonDefinition.ButtonColor;
            string[] array = s.Split(',');
            cmbButtonColor.Value = Color.FromArgb(int.Parse(array[0]), int.Parse(array[1]), int.Parse(array[2]));//	_tradingTicket.ButtonColor;
            if (_actionButtonDefinition.IsHotButton)
            {
                btnHot.Checked = true;
                btnCold.Checked = false;
            }
            else
            {
                btnHot.Checked = false;
                btnCold.Checked = true;
            }

            cmbAsset.Value = _actionButtonDefinition.AssetID;

            BindUnderlyingCombo(_actionButtonDefinition.AssetID);
            cmbUnderLying.Value = _actionButtonDefinition.UnderLyingID;

            BindUserCounterParty();
            cmbCounterParty.Value = _actionButtonDefinition.CounterpartyID;

            BindVenue(_actionButtonDefinition.CounterpartyID);
            cmbVenue.Value = _actionButtonDefinition.VenueID;

            BindSideCombo();
            cmbSide.Value = _actionButtonDefinition.SideID;

            txtQuantity.Value = _actionButtonDefinition.Quantity;

            BindOrderType(_actionButtonDefinition.CounterpartyID, _actionButtonDefinition.VenueID);
            cmbOrderType.Value = _actionButtonDefinition.OrderTypeID;

            if (IfLimitPriceRequired())
            {
                cmbLimitPrice.Value = _actionButtonDefinition.LimitType;
                txtLimitOffset.Value = _actionButtonDefinition.LimitOffset;
            }

            BindTIF(_actionButtonDefinition.CounterpartyID, _actionButtonDefinition.VenueID);
            cmbTIF.Value = _actionButtonDefinition.TIF;

            BindHandlingInstr(_actionButtonDefinition.CounterpartyID, _actionButtonDefinition.VenueID);
            cmbHandlingInstruction.Value = _actionButtonDefinition.HandlingInstructionID;

            BindExecInstr(_actionButtonDefinition.CounterpartyID, _actionButtonDefinition.VenueID);
            cmbExecutionInstruction.Value = _actionButtonDefinition.ExecutionInstructionID;

            cmbUserTradingAccount.Value = _actionButtonDefinition.TradingAccountID;
            cmbStrategy.Value = _actionButtonDefinition.StrategyID;
            cmbUserAccount.Value = _actionButtonDefinition.AccountID;
            txtPegOffSet.Value = _actionButtonDefinition.Peg;
            txtDiscrOffSet.Value = _actionButtonDefinition.DiscreationOffset;
            txtDisplayQuantity.Value = _actionButtonDefinition.DisplayQuantity;
            txtRandom.Value = _actionButtonDefinition.Random;
            cmbPNP.Value = _actionButtonDefinition.PNP;

            //SetComboOrderSide(_tradingTicket);
            if ((cmbSide.Value.ToString() == FIXConstants.SIDE_Sell) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellPlus) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShort) ||
                    (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShortExempt))
            {
                //cmbClientCompany.Value = _tradingTicket.ClientCompanyID;				
                //cmbClientTrader.Value = _tradingTicket.ClientTraderID;
                //cmbClientAccount.Value = _tradingTicket.ClientAccountID;
                //cmbAgencyPrincipal.Value = _tradingTicket.Principal;


                //cmbAgencyPrincipal.Value = _tradingTicket.Principal;
                //cmbShortExempt.Value = _tradingTicket.ShortExempt;
                //cmbClearingFirmID.Value = _tradingTicket.ClearingFirmID;
            }


            // NO NEED TO CHECK FOR VALUES AS NOW SPINNER HAS BEEN PLACED AND THE 
            // DEFAULT VALUE WILL BE 0.0 MINIMUM


            result = true;
            return result;
        }
        #endregion


        #region Private Methods
        private void SetDefaultValuesForOrderType()
        {
            if ((cmbSide.Value.ToString() == FIXConstants.SIDE_Buy) ||
                   (cmbSide.Value.ToString() == FIXConstants.SIDE_BuyMinus) ||
                   (cmbSide.Value.ToString() == FIXConstants.SIDE_Buy_Closed) ||
                   (cmbSide.Value.ToString() == FIXConstants.SIDE_Buy_Open))
            {
                cmbLimitPrice.Value = (int)Prana.BusinessObjects.Enumerators.TradingTicketEnums.LimitType.Ask;
            }
            else if ((cmbSide.Value.ToString() == FIXConstants.SIDE_Sell) ||
                        (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShort) ||
                        (cmbSide.Value.ToString() == FIXConstants.SIDE_Sell_Closed) ||
                        (cmbSide.Value.ToString() == FIXConstants.SIDE_Sell_Open) ||
                        (cmbSide.Value.ToString() == FIXConstants.SIDE_SellShortExempt))
            {
                cmbLimitPrice.Value = (int)Prana.BusinessObjects.Enumerators.TradingTicketEnums.LimitType.Bid;
            }
            else
            {
                cmbLimitPrice.Value = (int)Prana.BusinessObjects.Enumerators.TradingTicketEnums.LimitType.Last;
            }
        }
        private bool IfLimitPriceRequired()
        {
            if ((cmbOrderType.Value != null) && (cmbOrderType.Value.ToString() == FIXConstants.ORDTYPE_Limit  //"2"for Limit
               || cmbOrderType.Value.ToString() == FIXConstants.ORDTYPE_Pegged//"P"for pegged
               || cmbOrderType.Value.ToString() == FIXConstants.ORDTYPE_Stoplimit)) //"4"for Stop Limit
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool DisplayNameOverlaps()
        {
            for (int count = 0; count < _existingNames.Count; count++)
            {
                if (txtName.Text.Trim().ToString() == _existingNames[count].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        #endregion


        #region Bind Combos

        private void BindAssetsCombo()
        {
            Assets assets = new Assets();
            assets = WindsorContainerManager.GetCompanyUserAssets(_loginUser.CompanyUserID);
            assets.Insert(0, new Asset(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbAsset.DataSource = null;
            cmbAsset.DataSource = assets;
            cmbAsset.DisplayMember = "Name";
            cmbAsset.ValueMember = "AssetID";
            cmbAsset.Value = int.MinValue;

            ColumnsCollection columns = cmbAsset.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {

                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
            }
        }
        private void BindUnderlyingCombo(int AssetID)
        {
            UnderLyings underLyings = new UnderLyings();
            underLyings = WindsorContainerManager.GetUnderLyingsByAssetAndUserID(_loginUser.CompanyUserID, AssetID);
            underLyings.Insert(0, new UnderLying(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbUnderLying.DataSource = null;
            cmbUnderLying.DataSource = underLyings;
            cmbUnderLying.DisplayMember = "Name";
            cmbUnderLying.ValueMember = "UnderLyingID";
            cmbUnderLying.Value = int.MinValue;
            ColumnsCollection columns = cmbUnderLying.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {

                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
            }
        }
        private void BindUserCounterParty()
        {
            try
            {
                CounterPartyCollection counterParties = new CounterPartyCollection();

                counterParties = WindsorContainerManager.GetCounterPartiesByAUIDAndUserID(_loginUser.CompanyUserID, int.Parse(cmbAsset.Value.ToString()), int.Parse(cmbUnderLying.Value.ToString()));
                counterParties.Insert(0, new CounterParty(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbCounterParty.DataSource = null;
                cmbCounterParty.DataSource = counterParties;
                cmbCounterParty.DisplayMember = "Name";
                cmbCounterParty.ValueMember = "CounterPartyID";
                cmbCounterParty.DataBind();
                cmbCounterParty.Value = int.MinValue;
                ColumnsCollection columns = cmbCounterParty.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
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
        private void BindVenue(int counterPartyID)
        {
            try
            {
                if (cmbAsset.Value == null || cmbUnderLying.Value == null)
                    return;
                VenueCollection venues = new VenueCollection();
                int assetID = Convert.ToInt32(cmbAsset.Value);
                int underlyingID = Convert.ToInt32(cmbUnderLying.Value);
                venues = WindsorContainerManager.GetVenuesByAUIDCounterPartyAndUserID(_loginUser.CompanyUserID, counterPartyID, assetID, underlyingID);
                venues.Insert(0, new Prana.BusinessObjects.Venue(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbVenue.DataSource = null;
                cmbVenue.DataSource = venues;
                cmbVenue.DisplayMember = "Name";
                cmbVenue.ValueMember = "VenueID";
                cmbVenue.Value = int.MinValue;
                ColumnsCollection columns = cmbVenue.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
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
        private void BindCompanyClients()
        {

            ClientCollection clients = new ClientCollection();
            clients = WindsorContainerManager.GetClients(_loginUser.CompanyID);
            clients.Insert(0, new Prana.BusinessObjects.Client(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbClientCompany.DataSource = null;
            cmbClientCompany.DataSource = clients;
            cmbClientCompany.DisplayMember = "ClientName";
            cmbClientCompany.ValueMember = "ClientID";
            cmbClientCompany.Value = int.MinValue;
            ColumnsCollection columns = cmbClientCompany.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {

                if (column.Key != "ClientName")
                {
                    column.Hidden = true;
                }
            }

        }
        private void BindClientAccounts(int companyClientID)
        {
            ClientAccounts accounts = new ClientAccounts();
            accounts = WindsorContainerManager.GetCompanyClientAccounts(companyClientID);

            accounts.Insert(0, new ClientAccount(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbClientAccount.DataSource = null;
            cmbClientAccount.DataSource = accounts;
            cmbClientAccount.DisplayMember = "CompanyClientAccountName";
            cmbClientAccount.ValueMember = "CompanyClientAccountID";
            cmbClientAccount.Value = int.MinValue;
        }
        private void BindCompanyClearingFirm()
        {
            ClearingFirmsPrimeBrokers clearingFirms = new ClearingFirmsPrimeBrokers();
            clearingFirms = WindsorContainerManager.GetClearingFirmsPrimeBrokers();
            clearingFirms.Insert(0, new ClearingFirmPrimeBroker(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
            cmbClearingFirmID.DataSource = null;
            cmbClearingFirmID.DataSource = clearingFirms;
            cmbClearingFirmID.DisplayMember = "ClearingFirmsPrimeBrokersName";

            cmbClearingFirmID.ValueMember = "ClearingFirmsPrimeBrokersID";
            cmbClearingFirmID.Value = int.MinValue;
        }
        private void BindCompanyClientTraders(int companyClientID)
        {
            Traders traders = new Traders();
            traders = WindsorContainerManager.GetTraders(companyClientID);
            traders.Insert(0, new Trader(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            cmbClientTrader.DataSource = null;
            cmbClientTrader.DataSource = traders;
            cmbClientTrader.DisplayMember = "ShortName";
            cmbClientTrader.ValueMember = "TraderID";
            cmbClientTrader.Value = int.MinValue;
        }
        private void BindAccount()
        {
            try
            {
                AccountCollection accounts = new AccountCollection();
                accounts = WindsorContainerManager.GetAccounts(_loginUser.CompanyUserID);
                accounts.Insert(0, new Prana.BusinessObjects.Account(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbUserAccount.DataSource = accounts;
                cmbUserAccount.DisplayMember = "Name";
                cmbUserAccount.ValueMember = "AccountID";
                cmbUserAccount.Value = int.MinValue;
                foreach (UltraGridColumn column in cmbUserAccount.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Header.Caption != "Name")
                    {
                        column.Hidden = true;
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
        private void BindOrderType(int counterPartyID, int venueID)
        {
            try
            {
                if (cmbAsset.Value == null || cmbUnderLying.Value == null)
                    return;
                int assetID = Convert.ToInt32(cmbAsset.Value);
                int underlyingID = Convert.ToInt32(cmbUnderLying.Value);
                OrderTypes orderTypes = new OrderTypes();
                orderTypes = WindsorContainerManager.GetOrderTypesByAUCVID(assetID, underlyingID, counterPartyID, venueID);
                orderTypes.Insert(0, new OrderType(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
                cmbOrderType.DataSource = null;
                cmbOrderType.DataSource = orderTypes;
                cmbOrderType.DisplayMember = "Type";
                cmbOrderType.ValueMember = "TagValue";
                cmbOrderType.Value = ApplicationConstants.C_COMBO_SELECT;


                ColumnsCollection columns = cmbOrderType.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {

                    if (column.Key != "Type")
                    {
                        column.Hidden = true;
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
        private void BindHandlingInstr(int counterPartyID, int venueID)
        {
            try
            {
                if (cmbAsset.Value == null || cmbUnderLying.Value == null)
                    return;
                int assetID = Convert.ToInt32(cmbAsset.Value);
                int underlyingID = Convert.ToInt32(cmbUnderLying.Value);
                HandlingInstructions handlingInstrucitons = new HandlingInstructions();
                handlingInstrucitons = WindsorContainerManager.GetHandlingInstructionByAUCVID(assetID, underlyingID, counterPartyID, venueID);
                handlingInstrucitons.Insert(0, new HandlingInstruction(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
                cmbHandlingInstruction.DataSource = null;
                cmbHandlingInstruction.DataSource = handlingInstrucitons;
                cmbHandlingInstruction.DisplayMember = "Name";
                cmbHandlingInstruction.ValueMember = "TagValue";
                cmbHandlingInstruction.Value = ApplicationConstants.C_COMBO_SELECT;
                ColumnsCollection columns = cmbHandlingInstruction.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {



                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
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
        private void BindTIF(int counterPartyID, int venueID)
        {
            try
            {
                if (cmbAsset.Value == null || cmbUnderLying.Value == null)
                    return;
                TimeInForces timeInForces = new TimeInForces();
                int assetID = Convert.ToInt32(cmbAsset.Value);
                int underlyingID = Convert.ToInt32(cmbUnderLying.Value);
                timeInForces = WindsorContainerManager.GetTIFByAUCVID(assetID, underlyingID, counterPartyID, venueID);
                timeInForces.Insert(0, new TimeInForce(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
                cmbTIF.DataSource = null;
                cmbTIF.DataSource = timeInForces;
                cmbTIF.DisplayMember = "Name";
                cmbTIF.ValueMember = "TagValue";
                cmbTIF.Value = ApplicationConstants.C_COMBO_SELECT;
                ColumnsCollection columns = cmbTIF.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {


                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
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
        private void BindSideCombo()
        {

            Sides sides = new Sides();
            sides = WindsorContainerManager.GetOrderSidesByCVAUEC(int.Parse(cmbAsset.Value.ToString()), int.Parse(cmbUnderLying.Value.ToString()), int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
            sides.Insert(0, new Side(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
            cmbSide.DataSource = null;
            cmbSide.DataSource = sides;
            cmbSide.DisplayMember = "Name";
            cmbSide.ValueMember = "TagValue";
            cmbSide.Value = ApplicationConstants.C_COMBO_SELECT;
            ColumnsCollection columns = cmbSide.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {

                if (column.Key != "Name")
                {
                    column.Hidden = true;
                }
            }

        }
        private void BindStrategy()
        {
            try
            {
                StrategyCollection strategies = new StrategyCollection();
                strategies = WindsorContainerManager.GetStrategies(_loginUser.CompanyUserID);
                strategies.Insert(0, new Prana.BusinessObjects.Strategy(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbStrategy.DataSource = null;
                cmbStrategy.DataSource = strategies;
                cmbStrategy.DisplayMember = "Name";
                cmbStrategy.ValueMember = "StrategyID";
                cmbStrategy.Value = int.MinValue;
                foreach (UltraGridColumn column in cmbStrategy.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Header.Caption != "Name")
                    {
                        column.Hidden = true;
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
        private void BindCVTradingAccount()
        {
            try
            {
                // might be changed after we make changes in the Client part. Should be actually taken from 
                // company counter party details tables ... as is the case with account and broker ID
                TradingAccountCollection tradingAccounts = new TradingAccountCollection();
                tradingAccounts = WindsorContainerManager.GetTradingAccounts(_loginUser.CompanyUserID);
                tradingAccounts.Insert(0, new Prana.BusinessObjects.TradingAccount(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbUserTradingAccount.DataSource = null;
                cmbUserTradingAccount.DataSource = tradingAccounts;

                cmbUserTradingAccount.DisplayMember = "Name";
                cmbUserTradingAccount.ValueMember = "TradingAccountID";
                cmbUserTradingAccount.Value = int.MinValue;
                cmbUserTradingAccount.DataBind();
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
        private void BindExecInstr(int counterPartyID, int venueID)
        {
            try
            {
                ExecutionInstructions executionInstrucitons = new ExecutionInstructions();
                if (cmbAsset.Value == null || cmbUnderLying.Value == null)
                    return;
                int assetID = Convert.ToInt32(cmbAsset.Value);
                int underlyingID = Convert.ToInt32(cmbUnderLying.Value);

                executionInstrucitons = WindsorContainerManager.GetExecutionInstructionByAUCVID(assetID, underlyingID, counterPartyID, venueID);
                executionInstrucitons.Insert(0, new ExecutionInstruction(int.MinValue, ApplicationConstants.C_COMBO_SELECT, ApplicationConstants.C_COMBO_SELECT));
                cmbExecutionInstruction.DataSource = null;
                cmbExecutionInstruction.DataSource = executionInstrucitons;
                cmbExecutionInstruction.DisplayMember = "ExecutionInstructions";
                cmbExecutionInstruction.ValueMember = "TagValue";
                cmbExecutionInstruction.Value = ApplicationConstants.C_COMBO_SELECT;
                ColumnsCollection columns = cmbExecutionInstruction.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {



                    if (column.Key != "ExecutionInstructions")
                    {
                        column.Hidden = true;
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
        private void BindLimitPrice()
        {
            cmbLimitPrice.DataSource = null;
            cmbLimitPrice.DataSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(Prana.BusinessObjects.Enumerators.TradingTicketEnums.LimitType));
            cmbLimitPrice.ValueMember = "Value";
            cmbLimitPrice.DisplayMember = "DisplayText";
            foreach (UltraGridColumn column in cmbLimitPrice.DisplayLayout.Bands[0].Columns)
            {
                if (column.Key != "DisplayText")
                {
                    column.Hidden = true;
                }
            }
            cmbLimitPrice.DataBind();
            cmbLimitPrice.Value = int.MinValue;
        }


        #endregion

    }
}
