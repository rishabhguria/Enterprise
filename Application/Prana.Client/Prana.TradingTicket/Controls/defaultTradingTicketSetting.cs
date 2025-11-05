using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Global;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Prana.Utilities.UIUtilities;
using System.EnterpriseServices;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging;
using Prana.ClientCommon;
using Prana.CommonDataCache;
using System.Collections.Generic;

namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for DefaultTradingTicketSetting.
    /// </summary>
    public class DefaultTradingTicketSetting : System.Windows.Forms.UserControl
    {
        #region Windows Members

        private Infragistics.Win.Misc.UltraLabel lblDispQty;
        private Infragistics.Win.Misc.UltraLabel lblIncStopPrice;
        private Infragistics.Win.Misc.UltraLabel lblIncrementQty;
        private Infragistics.Win.Misc.UltraLabel lblQuantity;
        private Infragistics.Win.Misc.UltraLabel lblIncPriceLimit;
        private Infragistics.Win.Misc.UltraLabel lblIncrPegOff;
        private Infragistics.Win.Misc.UltraLabel lblIncrDiscOff;
        private Infragistics.Win.Misc.UltraGroupBox grpbxCPVenue;
        private Infragistics.Win.Misc.UltraLabel label5;
        private Infragistics.Win.Misc.UltraLabel lblVenue;
        private Infragistics.Win.Misc.UltraLabel lblHandInst;
        private Infragistics.Win.Misc.UltraLabel lblOrderType;
        private Infragistics.Win.Misc.UltraLabel lblCounterParty;
        private Infragistics.Win.Misc.UltraLabel lblExecInst;
        private Infragistics.Win.Misc.UltraLabel lblAccount;
        private Infragistics.Win.Misc.UltraLabel lblTIF;
        private Infragistics.Win.Misc.UltraLabel lblStrategy;
        private Infragistics.Win.Misc.UltraLabel lblTradAcc;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenue;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbOrderType;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbExecutionInstruction;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbHandlingInstruction;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbTIF;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAccount;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbStrategy;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
        private Infragistics.Win.Misc.UltraButton btnAddCVSettings;

        #endregion
        
        private IContainer components;
        
        #region User Defined Private Members
        private CompanyUser _LoginUser;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Infragistics.Win.Misc.UltraGroupBox grpbxNumerics;
        private Spinner txtQuantity;
        private Spinner txtDisplayQuantity;
        private Spinner txtQuantityIncrement;
        private Spinner txtPriceLimitIncrement;
        private Spinner txtDiscrOffset;
        private Spinner txtPegOffset;
        private Spinner txtStopPriceIncrement;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.Misc.UltraLabel label4;
        private PreferencesUniversalSettingsCollection _preferencesUniversalSettingsCollection;
        private Infragistics.Win.Misc.UltraGroupBox grpBxDefaultSelections;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClearingFirm;
        private Infragistics.Win.Misc.UltraLabel lblClearingFirm;
        private PreferencesUniversalSettings _preferenceUniversalSetting = null;//new PreferencesUniversalSettings(); 
        #endregion

        private int _assetID;
        private UltraCombo cmbOrderSide;
        private Infragistics.Win.Misc.UltraLabel labelOrderSide;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoSetDefaultCV;
        // private Infragistics.Win.Misc.UltraLabel lblTradAcc;
        private UltraCombo cmbTradingAccount;
        private UltraCombo cmbSettlCurrency;
        private Infragistics.Win.Misc.UltraLabel lblSettlCurrency;
        private static bool _hasChanges = false;
        private CheckBox defaultQuantityValueChkBox;

        public int AssetID
        {
            get { return _assetID; }
            set 
            {
                _assetID = value; 

            }
        }

        private int _underlyingID;

        public int UnderlyingID
        {
            get { return _underlyingID; }
            set { _underlyingID = value; }
        }
	
        public DefaultTradingTicketSetting()
        {

        }

        //Added new parameter DataTable accountValueCollection for accounts and preferences
        public DefaultTradingTicketSetting(int assetID, int underlyingID, Prana.BusinessObjects.CompanyUser user, PreferencesUniversalSettingsCollection preferencesUniversalSettingsCollection, DataTable accountValueCollection)
        {
            try
            {
                this.AccountValueCollection = accountValueCollection;
                // This call is required by the Windows.Forms Form Designer.
                _preferencesUniversalSettingsCollection = preferencesUniversalSettingsCollection;
                InitializeComponent();
                
                AssetID = assetID;
                UnderlyingID = underlyingID;
                _LoginUser = user;
                BindCVTradingAccount();

                BindUserCounterParty();
                SetPreferences(_preferencesUniversalSettingsCollection.GetDefaultPref(_assetID, _underlyingID));             
                uoSetDefaultCV.ValueChanged += new EventHandler(uoSetDefaultCV_ValueChanged);
                int baseAssetID = Mapper.GetBaseAsset(_assetID);
                switch (baseAssetID)
                {
                    case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Option:
                        HideOrShowControlsForAssets();
                        ShiftControlsUpOrDown();
                        lblClearingFirm.Text = "CMTA";
                        break;
                    case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Future:
                    
                        HideOrShowControlsForAssets();
                        ShiftControlsUpOrDown();
                        lblClearingFirm.Text = "GiveUp";
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        void uoSetDefaultCV_ValueChanged(object sender, EventArgs e)
        {
            try
            {
            if (uoSetDefaultCV.CheckedIndex==1)
                {
                    _preferencesUniversalSettingsCollection.RemoveDefaultCV(_assetID.ToString(), _underlyingID.ToString());
                    if (_preferenceUniversalSetting != null)
                    {
                        _preferenceUniversalSetting.IsDefaultCV = false;
                    }
                }
                else
                {
                    if (_preferenceUniversalSetting != null)
                    {
                        _preferenceUniversalSetting.IsDefaultCV = true;
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        //private void SetCMTAorGiveUpCombo(int assetID, int underlyingID)
        //{
        //    switch (assetID)
        //        {
        //        case FIXConstants.SECURITYTYPE_Options:
        //            lblClearingFirm.Text = "CMTA";
                    

        //            default:
        //                break;
        //        }
          



        //}


        

        public void RestoreDefaultPrefs(PreferencesUniversalSettingsCollection preferencesUniversalSettingsCollection)
        {
            _preferencesUniversalSettingsCollection = preferencesUniversalSettingsCollection;
            cmbVenue.Value = int.MinValue;
            PreferencesUniversalSettings prefSetting = preferencesUniversalSettingsCollection.GetDefaultPref(_assetID, _underlyingID);
            SetPreferences(prefSetting);           
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn13 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HandlingInstructionID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn14 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderHandlingInstruction", 1);
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand6 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn15 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn16 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTimeInForce", 1);
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand7 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn17 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn18 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance75 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance76 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance77 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance78 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance79 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance80 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance81 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance82 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance83 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance84 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance85 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand8 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn19 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn20 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StrategyID", 1);
            Infragistics.Win.Appearance appearance86 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance87 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance88 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance89 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance90 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance91 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance92 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance93 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance94 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance95 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance96 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance97 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand9 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance98 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance99 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance100 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance101 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance102 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance103 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance104 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance105 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance106 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance107 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance108 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance109 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance110 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance111 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance112 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance113 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance114 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance115 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance116 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance117 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance118 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance119 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance120 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand10 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AccountID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance121 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand11 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn21 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn22 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TradingAccountID", 1);
            Infragistics.Win.Appearance appearance122 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance123 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance124 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance125 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance126 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance127 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance128 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance129 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance130 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance131 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance132 = new Infragistics.Win.Appearance();
            this.grpbxNumerics = new Infragistics.Win.Misc.UltraGroupBox();
            this.defaultQuantityValueChkBox = new System.Windows.Forms.CheckBox();
            this.txtStopPriceIncrement = new Prana.Utilities.UIUtilities.Spinner();
            this.txtPegOffset = new Prana.Utilities.UIUtilities.Spinner();
            this.txtDiscrOffset = new Prana.Utilities.UIUtilities.Spinner();
            this.txtPriceLimitIncrement = new Prana.Utilities.UIUtilities.Spinner();
            this.txtQuantityIncrement = new Prana.Utilities.UIUtilities.Spinner();
            this.txtDisplayQuantity = new Prana.Utilities.UIUtilities.Spinner();
            this.lblDispQty = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncStopPrice = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncrementQty = new Infragistics.Win.Misc.UltraLabel();
            this.lblQuantity = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncPriceLimit = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncrPegOff = new Infragistics.Win.Misc.UltraLabel();
            this.lblIncrDiscOff = new Infragistics.Win.Misc.UltraLabel();
            this.txtQuantity = new Prana.Utilities.UIUtilities.Spinner();
            this.grpbxCPVenue = new Infragistics.Win.Misc.UltraGroupBox();
            this.label5 = new Infragistics.Win.Misc.UltraLabel();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.uoSetDefaultCV = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblVenue = new Infragistics.Win.Misc.UltraLabel();
            this.lblCounterParty = new Infragistics.Win.Misc.UltraLabel();
            this.lblHandInst = new Infragistics.Win.Misc.UltraLabel();
            this.lblOrderType = new Infragistics.Win.Misc.UltraLabel();
            this.lblExecInst = new Infragistics.Win.Misc.UltraLabel();
            this.lblAccount = new Infragistics.Win.Misc.UltraLabel();
            this.lblTIF = new Infragistics.Win.Misc.UltraLabel();
            this.lblStrategy = new Infragistics.Win.Misc.UltraLabel();
            this.lblTradAcc = new Infragistics.Win.Misc.UltraLabel();
            this.cmbOrderType = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbExecutionInstruction = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbHandlingInstruction = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbTIF = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbStrategy = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnAddCVSettings = new Infragistics.Win.Misc.UltraButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.grpBxDefaultSelections = new Infragistics.Win.Misc.UltraGroupBox();
            this.labelOrderSide = new Infragistics.Win.Misc.UltraLabel();
            this.cmbOrderSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbClearingFirm = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblClearingFirm = new Infragistics.Win.Misc.UltraLabel();
            this.cmbTradingAccount = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbSettlCurrency = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblSettlCurrency = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grpbxNumerics)).BeginInit();
            this.grpbxNumerics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpbxCPVenue)).BeginInit();
            this.grpbxCPVenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoSetDefaultCV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxDefaultSelections)).BeginInit();
            this.grpBxDefaultSelections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlCurrency)).BeginInit();
            this.SuspendLayout();
            // 
            // grpbxNumerics
            // 
            this.grpbxNumerics.Controls.Add(this.defaultQuantityValueChkBox);
            this.grpbxNumerics.Controls.Add(this.txtStopPriceIncrement);
            this.grpbxNumerics.Controls.Add(this.txtPegOffset);
            this.grpbxNumerics.Controls.Add(this.txtDiscrOffset);
            this.grpbxNumerics.Controls.Add(this.txtPriceLimitIncrement);
            this.grpbxNumerics.Controls.Add(this.txtQuantityIncrement);
            this.grpbxNumerics.Controls.Add(this.txtDisplayQuantity);
            this.grpbxNumerics.Controls.Add(this.lblDispQty);
            this.grpbxNumerics.Controls.Add(this.lblIncStopPrice);
            this.grpbxNumerics.Controls.Add(this.lblIncrementQty);
            this.grpbxNumerics.Controls.Add(this.lblQuantity);
            this.grpbxNumerics.Controls.Add(this.lblIncPriceLimit);
            this.grpbxNumerics.Controls.Add(this.lblIncrPegOff);
            this.grpbxNumerics.Controls.Add(this.lblIncrDiscOff);
            this.grpbxNumerics.Controls.Add(this.txtQuantity);
            this.grpbxNumerics.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpbxNumerics.Location = new System.Drawing.Point(288, 48);
            this.grpbxNumerics.Name = "grpbxNumerics";
            this.grpbxNumerics.Size = new System.Drawing.Size(270, 251);
            this.grpbxNumerics.TabIndex = 8;
            this.grpbxNumerics.Text = "Numerics";
            // 
            // defaultQuantityValueChkBox
            // 
            this.defaultQuantityValueChkBox.AutoSize = true;
            this.defaultQuantityValueChkBox.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.defaultQuantityValueChkBox.Location = new System.Drawing.Point(6, 228);
            this.defaultQuantityValueChkBox.Name = "defaultQuantityValueChkBox";
            this.defaultQuantityValueChkBox.Size = new System.Drawing.Size(173, 17);
            this.defaultQuantityValueChkBox.TabIndex = 8;
            this.defaultQuantityValueChkBox.Text = "Set Default Quantity Zero";
            this.defaultQuantityValueChkBox.UseVisualStyleBackColor = true;
            this.defaultQuantityValueChkBox.CheckedChanged += new System.EventHandler(this.defaultQuantityValueChkBox_CheckedChanged);
            // 
            // txtStopPriceIncrement
            // 
            this.txtStopPriceIncrement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtStopPriceIncrement.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtStopPriceIncrement.DecimalPoints = 2147483647;
            this.txtStopPriceIncrement.Increment = 1D;
            this.txtStopPriceIncrement.Location = new System.Drawing.Point(180, 140);
            this.txtStopPriceIncrement.MaxValue = 99999D;
            this.txtStopPriceIncrement.MinValue = 0D;
            this.txtStopPriceIncrement.Name = "txtStopPriceIncrement";
            this.txtStopPriceIncrement.Size = new System.Drawing.Size(66, 18);
            this.txtStopPriceIncrement.TabIndex = 5;
            this.txtStopPriceIncrement.Value = 0.25D;
            this.txtStopPriceIncrement.ValueChanged += Control_ValueChanged;
            this.txtStopPriceIncrement.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // txtPegOffset
            // 
            this.txtPegOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtPegOffset.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtPegOffset.DecimalPoints = 2147483647;
            this.txtPegOffset.Increment = 1D;
            this.txtPegOffset.Location = new System.Drawing.Point(180, 170);
            this.txtPegOffset.MaxValue = 99999D;
            this.txtPegOffset.MinValue = 0D;
            this.txtPegOffset.Name = "txtPegOffset";
            this.txtPegOffset.Size = new System.Drawing.Size(66, 18);
            this.txtPegOffset.TabIndex = 6;
            this.txtPegOffset.Value = 0.25D;
            this.txtPegOffset.ValueChanged += Control_ValueChanged;
            this.txtPegOffset.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // txtDiscrOffset
            // 
            this.txtDiscrOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtDiscrOffset.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtDiscrOffset.DecimalPoints = 2147483647;
            this.txtDiscrOffset.Increment = 1D;
            this.txtDiscrOffset.Location = new System.Drawing.Point(180, 200);
            this.txtDiscrOffset.MaxValue = 99999D;
            this.txtDiscrOffset.MinValue = 0D;
            this.txtDiscrOffset.Name = "txtDiscrOffset";
            this.txtDiscrOffset.Size = new System.Drawing.Size(66, 18);
            this.txtDiscrOffset.TabIndex = 7;
            this.txtDiscrOffset.Value = 0.25D;
            this.txtDiscrOffset.ValueChanged += Control_ValueChanged;
            this.txtDiscrOffset.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // txtPriceLimitIncrement
            // 
            this.txtPriceLimitIncrement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtPriceLimitIncrement.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtPriceLimitIncrement.DecimalPoints = 2147483647;
            this.txtPriceLimitIncrement.Increment = 1D;
            this.txtPriceLimitIncrement.Location = new System.Drawing.Point(180, 110);
            this.txtPriceLimitIncrement.MaxValue = 99999D;
            this.txtPriceLimitIncrement.MinValue = 0D;
            this.txtPriceLimitIncrement.Name = "txtPriceLimitIncrement";
            this.txtPriceLimitIncrement.Size = new System.Drawing.Size(66, 18);
            this.txtPriceLimitIncrement.TabIndex = 4;
            this.txtPriceLimitIncrement.Value = 0.25D;
            this.txtPriceLimitIncrement.ValueChanged += Control_ValueChanged;
            this.txtPriceLimitIncrement.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // txtQuantityIncrement
            // 
            this.txtQuantityIncrement.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtQuantityIncrement.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtQuantityIncrement.DecimalPoints = 2147483647;
            this.txtQuantityIncrement.Increment = 0.0001D;
            this.txtQuantityIncrement.Location = new System.Drawing.Point(180, 80);
            this.txtQuantityIncrement.MaxValue = 99999D;
            this.txtQuantityIncrement.MinValue = 0.0001D;
            this.txtQuantityIncrement.Name = "txtQuantityIncrement";
            this.txtQuantityIncrement.Size = new System.Drawing.Size(66, 18);
            this.txtQuantityIncrement.TabIndex = 3;
            this.txtQuantityIncrement.Value = 1D;
            this.txtQuantityIncrement.ValueChanged += Control_ValueChanged;
            this.txtQuantityIncrement.Validated += new System.EventHandler(this.spinner_Validated);

            // 
            // txtDisplayQuantity
            // 
            this.txtDisplayQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtDisplayQuantity.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtDisplayQuantity.DecimalPoints = 2147483647;
            this.txtDisplayQuantity.Increment = 0.0001D;
            this.txtDisplayQuantity.Location = new System.Drawing.Point(180, 50);
            this.txtDisplayQuantity.MaxValue = 99999D;
            this.txtDisplayQuantity.MinValue = 0.0001D;
            this.txtDisplayQuantity.Name = "txtDisplayQuantity";
            this.txtDisplayQuantity.Size = new System.Drawing.Size(66, 18);
            this.txtDisplayQuantity.TabIndex = 2;
            this.txtDisplayQuantity.Value = 1D;
            this.txtDisplayQuantity.ValueChanged += Control_ValueChanged;
            this.txtDisplayQuantity.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // lblDispQty
            // 
            this.lblDispQty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDispQty.Location = new System.Drawing.Point(6, 48);
            this.lblDispQty.Name = "lblDispQty";
            this.lblDispQty.Size = new System.Drawing.Size(148, 22);
            this.lblDispQty.TabIndex = 1;
            this.lblDispQty.Text = "Display Quantity";
            // 
            // lblIncStopPrice
            // 
            this.lblIncStopPrice.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncStopPrice.Location = new System.Drawing.Point(6, 138);
            this.lblIncStopPrice.Name = "lblIncStopPrice";
            this.lblIncStopPrice.Size = new System.Drawing.Size(148, 22);
            this.lblIncStopPrice.TabIndex = 4;
            this.lblIncStopPrice.Text = "Increment On Stop Price";
            // 
            // lblIncrementQty
            // 
            this.lblIncrementQty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncrementQty.Location = new System.Drawing.Point(6, 78);
            this.lblIncrementQty.Name = "lblIncrementQty";
            this.lblIncrementQty.Size = new System.Drawing.Size(148, 22);
            this.lblIncrementQty.TabIndex = 2;
            this.lblIncrementQty.Text = "Increment On Quantity";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuantity.Location = new System.Drawing.Point(6, 22);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(148, 22);
            this.lblQuantity.TabIndex = 0;
            this.lblQuantity.Text = "Quantity";
            // 
            // lblIncPriceLimit
            // 
            this.lblIncPriceLimit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncPriceLimit.Location = new System.Drawing.Point(6, 108);
            this.lblIncPriceLimit.Name = "lblIncPriceLimit";
            this.lblIncPriceLimit.Size = new System.Drawing.Size(148, 22);
            this.lblIncPriceLimit.TabIndex = 3;
            this.lblIncPriceLimit.Text = "Increment On Price Limit";
            // 
            // lblIncrPegOff
            // 
            this.lblIncrPegOff.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncrPegOff.Location = new System.Drawing.Point(6, 168);
            this.lblIncrPegOff.Name = "lblIncrPegOff";
            this.lblIncrPegOff.Size = new System.Drawing.Size(148, 22);
            this.lblIncrPegOff.TabIndex = 3;
            this.lblIncrPegOff.Text = "Increment On Peg Offset";
            // 
            // lblIncrDiscOff
            // 
            this.lblIncrDiscOff.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncrDiscOff.Location = new System.Drawing.Point(6, 198);
            this.lblIncrDiscOff.Name = "lblIncrDiscOff";
            this.lblIncrDiscOff.Size = new System.Drawing.Size(148, 22);
            this.lblIncrDiscOff.TabIndex = 4;
            this.lblIncrDiscOff.Text = "Increment On Discr Offset";
            // 
            // txtQuantity
            // 
            this.txtQuantity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.txtQuantity.DataType = Prana.Utilities.UIUtilities.DataTypes.Numeric;
            this.txtQuantity.DecimalPoints = 2147483647;
            this.txtQuantity.Increment = 0.0001D;
            this.txtQuantity.Location = new System.Drawing.Point(180, 22);
            this.txtQuantity.MaxValue = 99999D;
            this.txtQuantity.MinValue = 0.0001D;
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(66, 18);
            this.txtQuantity.TabIndex = 1;
            this.txtQuantity.Value = 1D;
            this.txtQuantity.ValueChanged += Control_ValueChanged;
            this.txtQuantity.Validated += new System.EventHandler(this.spinner_Validated);
            // 
            // grpbxCPVenue
            // 
            this.grpbxCPVenue.Controls.Add(this.label5);
            this.grpbxCPVenue.Controls.Add(this.cmbCounterParty);
            this.grpbxCPVenue.Controls.Add(this.uoSetDefaultCV);
            this.grpbxCPVenue.Controls.Add(this.cmbVenue);
            this.grpbxCPVenue.Controls.Add(this.lblVenue);
            this.grpbxCPVenue.Controls.Add(this.lblCounterParty);
            this.grpbxCPVenue.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpbxCPVenue.Location = new System.Drawing.Point(6, 2);
            this.grpbxCPVenue.Name = "grpbxCPVenue";
            this.grpbxCPVenue.Size = new System.Drawing.Size(552, 46);
            this.grpbxCPVenue.TabIndex = 3;
            this.grpbxCPVenue.Text = "Broker - Venue";
            // 
            // uoSetDefaultCV
            // 
            this.uoSetDefaultCV.CheckedIndex = 0;
            valueListItem1.DataValue = "Yes";
            valueListItem1.DisplayText = "Yes";
            valueListItem2.DataValue = "No";
            valueListItem2.DisplayText = "No";
            this.uoSetDefaultCV.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.uoSetDefaultCV.Location = new System.Drawing.Point(461, 18);
            this.uoSetDefaultCV.Name = "uoSetDefaultCV";
            this.uoSetDefaultCV.Size = new System.Drawing.Size(85, 20);
            this.uoSetDefaultCV.TabIndex = 33;
            this.uoSetDefaultCV.Text = "Yes";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(329, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 15);
            this.label5.TabIndex = 32;
            this.label5.Text = "Set this as Default CV";
            // 
            // cmbCounterParty
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance1;
            this.cmbCounterParty.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridBand1.GroupHeadersVisible = false;
            ultraGridBand1.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand1.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.DropDownWidth = 0;
            this.cmbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCounterParty.Location = new System.Drawing.Point(86, 18);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(90, 21);
            this.cmbCounterParty.TabIndex = 1;
            this.cmbCounterParty.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            // 
            // cmbVenue
            // 
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance13;
            this.cmbVenue.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridBand2.GroupHeadersVisible = false;
            ultraGridBand2.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand2.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.DropDownWidth = 0;
            this.cmbVenue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVenue.Location = new System.Drawing.Point(233, 16);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(90, 24);
            this.cmbVenue.TabIndex = 2;
            this.cmbVenue.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbVenue.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbVenue_InitializeLayout);
            this.cmbVenue.ValueChanged += new System.EventHandler(this.cmbVenue_ValueChanged);
            // 
            // lblVenue
            // 
            this.lblVenue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVenue.Location = new System.Drawing.Point(181, 18);
            this.lblVenue.Name = "lblVenue";
            this.lblVenue.Size = new System.Drawing.Size(46, 20);
            this.lblVenue.TabIndex = 1;
            this.lblVenue.Text = "Venue";
            // 
            // lblCounterParty
            // 
            this.lblCounterParty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCounterParty.Location = new System.Drawing.Point(6, 18);
            this.lblCounterParty.Name = "lblCounterParty";
            this.lblCounterParty.Size = new System.Drawing.Size(80, 20);
            this.lblCounterParty.TabIndex = 0;
            this.lblCounterParty.Text = "Broker";
            // 
            // lblHandInst
            // 
            this.lblHandInst.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHandInst.Location = new System.Drawing.Point(6, 86);
            this.lblHandInst.Name = "lblHandInst";
            this.lblHandInst.Size = new System.Drawing.Size(118, 18);
            this.lblHandInst.TabIndex = 4;
            this.lblHandInst.Text = "Handling Instr.";
            // 
            // lblOrderType
            // 
            this.lblOrderType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrderType.Location = new System.Drawing.Point(6, 20);
            this.lblOrderType.Name = "lblOrderType";
            this.lblOrderType.Size = new System.Drawing.Size(101, 18);
            this.lblOrderType.TabIndex = 2;
            this.lblOrderType.Text = "Order Type";
            // 
            // lblExecInst
            // 
            this.lblExecInst.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecInst.Location = new System.Drawing.Point(6, 108);
            this.lblExecInst.Name = "lblExecInst";
            this.lblExecInst.Size = new System.Drawing.Size(118, 18);
            this.lblExecInst.TabIndex = 3;
            this.lblExecInst.Text = "Execution Instr.";
            // 
            // lblAccount
            // 
            this.lblAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(6, 152);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(101, 18);
            this.lblAccount.TabIndex = 2;
            this.lblAccount.Text = "Account";
            // 
            // lblTIF
            // 
            this.lblTIF.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTIF.Location = new System.Drawing.Point(6, 42);
            this.lblTIF.Name = "lblTIF";
            this.lblTIF.Size = new System.Drawing.Size(80, 18);
            this.lblTIF.TabIndex = 0;
            this.lblTIF.Text = "TIF";
            // 
            // lblStrategy
            // 
            this.lblStrategy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStrategy.Location = new System.Drawing.Point(6, 130);
            this.lblStrategy.Name = "lblStrategy";
            this.lblStrategy.Size = new System.Drawing.Size(118, 18);
            this.lblStrategy.TabIndex = 3;
            this.lblStrategy.Text = "Strategy";
            // 
            // lblTradAcc
            // 
            this.lblTradAcc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTradAcc.Location = new System.Drawing.Point(6, 64);
            this.lblTradAcc.Name = "lblTradAcc";
            this.lblTradAcc.Size = new System.Drawing.Size(100, 18);
            this.lblTradAcc.TabIndex = 13;
            this.lblTradAcc.Text = "Trading Account";
            // 
            // cmbOrderType
            // 
            this.cmbOrderType.AutoSize = false;
            appearance25.BackColor = System.Drawing.SystemColors.Window;
            appearance25.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderType.DisplayLayout.Appearance = appearance25;
            this.cmbOrderType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand3.ColHeadersVisible = false;
            ultraGridBand3.GroupHeadersVisible = false;
            ultraGridBand3.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand3.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbOrderType.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance26.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance26.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.GroupByBox.Appearance = appearance26;
            appearance27.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.BandLabelAppearance = appearance27;
            this.cmbOrderType.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance28.BackColor2 = System.Drawing.SystemColors.Control;
            appearance28.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance28.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderType.DisplayLayout.GroupByBox.PromptAppearance = appearance28;
            this.cmbOrderType.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderType.DisplayLayout.MaxRowScrollRegions = 1;
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderType.DisplayLayout.Override.ActiveCellAppearance = appearance29;
            appearance30.BackColor = System.Drawing.SystemColors.Highlight;
            appearance30.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderType.DisplayLayout.Override.ActiveRowAppearance = appearance30;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderType.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance31.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.CardAreaAppearance = appearance31;
            appearance32.BorderColor = System.Drawing.Color.Silver;
            appearance32.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderType.DisplayLayout.Override.CellAppearance = appearance32;
            this.cmbOrderType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderType.DisplayLayout.Override.CellPadding = 0;
            appearance33.BackColor = System.Drawing.SystemColors.Control;
            appearance33.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance33.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance33.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance33.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderType.DisplayLayout.Override.GroupByRowAppearance = appearance33;
            appearance34.TextHAlignAsString = "Left";
            this.cmbOrderType.DisplayLayout.Override.HeaderAppearance = appearance34;
            this.cmbOrderType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderType.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            appearance35.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderType.DisplayLayout.Override.RowAppearance = appearance35;
            this.cmbOrderType.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance36.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderType.DisplayLayout.Override.TemplateAddRowAppearance = appearance36;
            this.cmbOrderType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderType.DropDownWidth = 0;
            this.cmbOrderType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOrderType.Location = new System.Drawing.Point(130, 20);
            this.cmbOrderType.Name = "cmbOrderType";
            this.cmbOrderType.Size = new System.Drawing.Size(106, 18);
            this.cmbOrderType.TabIndex = 1;
            this.cmbOrderType.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOrderType.ValueChanged += Control_ValueChanged;
            // 
            // cmbExecutionInstruction
            // 
            this.cmbExecutionInstruction.AutoSize = false;
            appearance37.BackColor = System.Drawing.SystemColors.Window;
            appearance37.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbExecutionInstruction.DisplayLayout.Appearance = appearance37;
            this.cmbExecutionInstruction.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand4.ColHeadersVisible = false;
            ultraGridBand4.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand4.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecutionInstruction.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbExecutionInstruction.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecutionInstruction.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance38.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance38.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance38.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance38.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.Appearance = appearance38;
            appearance39.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.BandLabelAppearance = appearance39;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance40.BackColor2 = System.Drawing.SystemColors.Control;
            appearance40.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance40.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbExecutionInstruction.DisplayLayout.GroupByBox.PromptAppearance = appearance40;
            this.cmbExecutionInstruction.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbExecutionInstruction.DisplayLayout.MaxRowScrollRegions = 1;
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbExecutionInstruction.DisplayLayout.Override.ActiveCellAppearance = appearance41;
            appearance42.BackColor = System.Drawing.SystemColors.Highlight;
            appearance42.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbExecutionInstruction.DisplayLayout.Override.ActiveRowAppearance = appearance42;
            this.cmbExecutionInstruction.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbExecutionInstruction.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance43.BackColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.Override.CardAreaAppearance = appearance43;
            appearance44.BorderColor = System.Drawing.Color.Silver;
            appearance44.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellAppearance = appearance44;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbExecutionInstruction.DisplayLayout.Override.CellPadding = 0;
            appearance45.BackColor = System.Drawing.SystemColors.Control;
            appearance45.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance45.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance45.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance45.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbExecutionInstruction.DisplayLayout.Override.GroupByRowAppearance = appearance45;
            appearance46.TextHAlignAsString = "Left";
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderAppearance = appearance46;
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbExecutionInstruction.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            appearance47.BorderColor = System.Drawing.Color.Silver;
            this.cmbExecutionInstruction.DisplayLayout.Override.RowAppearance = appearance47;
            this.cmbExecutionInstruction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance48.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbExecutionInstruction.DisplayLayout.Override.TemplateAddRowAppearance = appearance48;
            this.cmbExecutionInstruction.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbExecutionInstruction.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbExecutionInstruction.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbExecutionInstruction.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbExecutionInstruction.DropDownWidth = 0;
            this.cmbExecutionInstruction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbExecutionInstruction.Location = new System.Drawing.Point(130, 108);
            this.cmbExecutionInstruction.Name = "cmbExecutionInstruction";
            this.cmbExecutionInstruction.Size = new System.Drawing.Size(106, 18);
            this.cmbExecutionInstruction.TabIndex = 5;
            this.cmbExecutionInstruction.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbExecutionInstruction.ValueChanged += Control_ValueChanged;
            // 
            // cmbHandlingInstruction
            // 
            this.cmbHandlingInstruction.AutoSize = false;
            appearance49.BackColor = System.Drawing.SystemColors.Window;
            appearance49.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbHandlingInstruction.DisplayLayout.Appearance = appearance49;
            this.cmbHandlingInstruction.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand5.ColHeadersVisible = false;
            ultraGridColumn13.Header.VisiblePosition = 0;
            ultraGridColumn13.Hidden = true;
            ultraGridColumn14.Header.VisiblePosition = 1;
            ultraGridColumn14.Width = 104;
            ultraGridBand5.Columns.AddRange(new object[] {
            ultraGridColumn13,
            ultraGridColumn14});
            ultraGridBand5.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand5.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHandlingInstruction.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbHandlingInstruction.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHandlingInstruction.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance50.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance50.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance50.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance50.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.Appearance = appearance50;
            appearance51.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.BandLabelAppearance = appearance51;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance52.BackColor2 = System.Drawing.SystemColors.Control;
            appearance52.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance52.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbHandlingInstruction.DisplayLayout.GroupByBox.PromptAppearance = appearance52;
            this.cmbHandlingInstruction.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbHandlingInstruction.DisplayLayout.MaxRowScrollRegions = 1;
            appearance53.BackColor = System.Drawing.SystemColors.Window;
            appearance53.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbHandlingInstruction.DisplayLayout.Override.ActiveCellAppearance = appearance53;
            appearance54.BackColor = System.Drawing.SystemColors.Highlight;
            appearance54.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbHandlingInstruction.DisplayLayout.Override.ActiveRowAppearance = appearance54;
            this.cmbHandlingInstruction.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbHandlingInstruction.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance55.BackColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.Override.CardAreaAppearance = appearance55;
            appearance56.BorderColor = System.Drawing.Color.Silver;
            appearance56.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellAppearance = appearance56;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbHandlingInstruction.DisplayLayout.Override.CellPadding = 0;
            appearance57.BackColor = System.Drawing.SystemColors.Control;
            appearance57.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance57.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance57.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance57.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbHandlingInstruction.DisplayLayout.Override.GroupByRowAppearance = appearance57;
            appearance58.TextHAlignAsString = "Left";
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderAppearance = appearance58;
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbHandlingInstruction.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance59.BackColor = System.Drawing.SystemColors.Window;
            appearance59.BorderColor = System.Drawing.Color.Silver;
            this.cmbHandlingInstruction.DisplayLayout.Override.RowAppearance = appearance59;
            this.cmbHandlingInstruction.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance60.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbHandlingInstruction.DisplayLayout.Override.TemplateAddRowAppearance = appearance60;
            this.cmbHandlingInstruction.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbHandlingInstruction.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbHandlingInstruction.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbHandlingInstruction.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbHandlingInstruction.DropDownWidth = 0;
            this.cmbHandlingInstruction.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbHandlingInstruction.Location = new System.Drawing.Point(130, 86);
            this.cmbHandlingInstruction.Name = "cmbHandlingInstruction";
            this.cmbHandlingInstruction.Size = new System.Drawing.Size(106, 18);
            this.cmbHandlingInstruction.TabIndex = 4;
            this.cmbHandlingInstruction.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbHandlingInstruction.ValueChanged += Control_ValueChanged;
            // 
            // cmbTIF
            // 
            this.cmbTIF.AutoSize = false;
            appearance61.BackColor = System.Drawing.SystemColors.Window;
            appearance61.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTIF.DisplayLayout.Appearance = appearance61;
            this.cmbTIF.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand6.ColHeadersVisible = false;
            ultraGridColumn15.Header.VisiblePosition = 0;
            ultraGridColumn15.Hidden = true;
            ultraGridColumn16.Header.VisiblePosition = 1;
            ultraGridColumn16.Width = 104;
            ultraGridBand6.Columns.AddRange(new object[] {
            ultraGridColumn15,
            ultraGridColumn16});
            ultraGridBand6.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand6.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.DisplayLayout.BandsSerializer.Add(ultraGridBand6);
            this.cmbTIF.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance62.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance62.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance62.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance62.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.GroupByBox.Appearance = appearance62;
            appearance63.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.BandLabelAppearance = appearance63;
            this.cmbTIF.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance64.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance64.BackColor2 = System.Drawing.SystemColors.Control;
            appearance64.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance64.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTIF.DisplayLayout.GroupByBox.PromptAppearance = appearance64;
            this.cmbTIF.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTIF.DisplayLayout.MaxRowScrollRegions = 1;
            appearance65.BackColor = System.Drawing.SystemColors.Window;
            appearance65.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTIF.DisplayLayout.Override.ActiveCellAppearance = appearance65;
            appearance66.BackColor = System.Drawing.SystemColors.Highlight;
            appearance66.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTIF.DisplayLayout.Override.ActiveRowAppearance = appearance66;
            this.cmbTIF.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTIF.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.CardAreaAppearance = appearance67;
            appearance68.BorderColor = System.Drawing.Color.Silver;
            appearance68.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTIF.DisplayLayout.Override.CellAppearance = appearance68;
            this.cmbTIF.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTIF.DisplayLayout.Override.CellPadding = 0;
            appearance69.BackColor = System.Drawing.SystemColors.Control;
            appearance69.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance69.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance69.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance69.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTIF.DisplayLayout.Override.GroupByRowAppearance = appearance69;
            appearance70.TextHAlignAsString = "Left";
            this.cmbTIF.DisplayLayout.Override.HeaderAppearance = appearance70;
            this.cmbTIF.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTIF.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance71.BackColor = System.Drawing.SystemColors.Window;
            appearance71.BorderColor = System.Drawing.Color.Silver;
            this.cmbTIF.DisplayLayout.Override.RowAppearance = appearance71;
            this.cmbTIF.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance72.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTIF.DisplayLayout.Override.TemplateAddRowAppearance = appearance72;
            this.cmbTIF.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTIF.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTIF.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTIF.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTIF.DropDownWidth = 0;
            this.cmbTIF.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTIF.Location = new System.Drawing.Point(130, 42);
            this.cmbTIF.Name = "cmbTIF";
            this.cmbTIF.Size = new System.Drawing.Size(106, 18);
            this.cmbTIF.TabIndex = 2;
            this.cmbTIF.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTIF.ValueChanged += Control_ValueChanged;
            // 
            // cmbAccount
            // 
            this.cmbAccount.AutoSize = false;
            appearance73.BackColor = System.Drawing.SystemColors.Window;
            appearance73.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbAccount.DisplayLayout.Appearance = appearance73;
            this.cmbAccount.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand7.ColHeadersVisible = false;
            ultraGridColumn17.Header.VisiblePosition = 0;
            ultraGridColumn17.Hidden = true;
            ultraGridColumn18.Header.VisiblePosition = 1;
            ultraGridColumn18.Width = 104;
            ultraGridBand7.Columns.AddRange(new object[] {
            ultraGridColumn17,
            ultraGridColumn18});
            ultraGridBand7.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand7.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand7);
            this.cmbAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance74.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance74.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance74.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance74.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccount.DisplayLayout.GroupByBox.Appearance = appearance74;
            appearance75.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance75;
            this.cmbAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance76.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance76.BackColor2 = System.Drawing.SystemColors.Control;
            appearance76.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance76.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance76;
            this.cmbAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance77.BackColor = System.Drawing.SystemColors.Window;
            appearance77.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbAccount.DisplayLayout.Override.ActiveCellAppearance = appearance77;
            appearance78.BackColor = System.Drawing.SystemColors.Highlight;
            appearance78.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbAccount.DisplayLayout.Override.ActiveRowAppearance = appearance78;
            this.cmbAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance79.BackColor = System.Drawing.SystemColors.Window;
            this.cmbAccount.DisplayLayout.Override.CardAreaAppearance = appearance79;
            appearance80.BorderColor = System.Drawing.Color.Silver;
            appearance80.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbAccount.DisplayLayout.Override.CellAppearance = appearance80;
            this.cmbAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbAccount.DisplayLayout.Override.CellPadding = 0;
            appearance81.BackColor = System.Drawing.SystemColors.Control;
            appearance81.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance81.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance81.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance81.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbAccount.DisplayLayout.Override.GroupByRowAppearance = appearance81;
            appearance82.TextHAlignAsString = "Left";
            this.cmbAccount.DisplayLayout.Override.HeaderAppearance = appearance82;
            this.cmbAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance83.BackColor = System.Drawing.SystemColors.Window;
            appearance83.BorderColor = System.Drawing.Color.Silver;
            this.cmbAccount.DisplayLayout.Override.RowAppearance = appearance83;
            this.cmbAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance84.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance84;
            this.cmbAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAccount.DropDownWidth = 0;
            this.cmbAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccount.Location = new System.Drawing.Point(130, 152);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.Size = new System.Drawing.Size(106, 18);
            this.cmbAccount.TabIndex = 7;
            this.cmbAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAccount.ValueChanged += new System.EventHandler(this.cmbAccount_ValueChanged);
            // 
            // cmbStrategy
            // 
            this.cmbStrategy.AutoSize = false;
            appearance85.BackColor = System.Drawing.SystemColors.Window;
            appearance85.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbStrategy.DisplayLayout.Appearance = appearance85;
            this.cmbStrategy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand8.ColHeadersVisible = false;
            ultraGridColumn19.Header.VisiblePosition = 0;
            ultraGridColumn19.Width = 104;
            ultraGridColumn20.Header.VisiblePosition = 1;
            ultraGridColumn20.Hidden = true;
            ultraGridBand8.Columns.AddRange(new object[] {
            ultraGridColumn19,
            ultraGridColumn20});
            ultraGridBand8.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand8.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStrategy.DisplayLayout.BandsSerializer.Add(ultraGridBand8);
            this.cmbStrategy.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStrategy.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance86.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance86.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance86.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance86.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.GroupByBox.Appearance = appearance86;
            appearance87.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.BandLabelAppearance = appearance87;
            this.cmbStrategy.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance88.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance88.BackColor2 = System.Drawing.SystemColors.Control;
            appearance88.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance88.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbStrategy.DisplayLayout.GroupByBox.PromptAppearance = appearance88;
            this.cmbStrategy.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbStrategy.DisplayLayout.MaxRowScrollRegions = 1;
            appearance89.BackColor = System.Drawing.SystemColors.Window;
            appearance89.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbStrategy.DisplayLayout.Override.ActiveCellAppearance = appearance89;
            appearance90.BackColor = System.Drawing.SystemColors.Highlight;
            appearance90.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbStrategy.DisplayLayout.Override.ActiveRowAppearance = appearance90;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbStrategy.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance91.BackColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.CardAreaAppearance = appearance91;
            appearance92.BorderColor = System.Drawing.Color.Silver;
            appearance92.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbStrategy.DisplayLayout.Override.CellAppearance = appearance92;
            this.cmbStrategy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbStrategy.DisplayLayout.Override.CellPadding = 0;
            appearance93.BackColor = System.Drawing.SystemColors.Control;
            appearance93.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance93.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance93.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance93.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbStrategy.DisplayLayout.Override.GroupByRowAppearance = appearance93;
            appearance94.TextHAlignAsString = "Left";
            this.cmbStrategy.DisplayLayout.Override.HeaderAppearance = appearance94;
            this.cmbStrategy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbStrategy.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance95.BackColor = System.Drawing.SystemColors.Window;
            appearance95.BorderColor = System.Drawing.Color.Silver;
            this.cmbStrategy.DisplayLayout.Override.RowAppearance = appearance95;
            this.cmbStrategy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance96.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbStrategy.DisplayLayout.Override.TemplateAddRowAppearance = appearance96;
            this.cmbStrategy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbStrategy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbStrategy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbStrategy.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbStrategy.DropDownWidth = 0;
            this.cmbStrategy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStrategy.Location = new System.Drawing.Point(130, 130);
            this.cmbStrategy.Name = "cmbStrategy";
            this.cmbStrategy.Size = new System.Drawing.Size(106, 18);
            this.cmbStrategy.TabIndex = 6;
            this.cmbStrategy.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbStrategy.ValueChanged += Control_ValueChanged;
            // 
            // btnAddCVSettings
            // 
            this.btnAddCVSettings.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnAddCVSettings.Location = new System.Drawing.Point(239, 327);
            this.btnAddCVSettings.Name = "btnAddCVSettings";
            this.btnAddCVSettings.Size = new System.Drawing.Size(102, 23);
            this.btnAddCVSettings.TabIndex = 9;
            this.btnAddCVSettings.Text = "Add CV Settings";
            this.btnAddCVSettings.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.btnAddCVSettings.Visible = false;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(8, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(8, 9);
            this.label2.TabIndex = 41;
            this.label2.Text = "*";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(390, 23);
            this.label4.TabIndex = 44;
            this.label4.Text = "These changes will be reflected when the user reopens Trading Ticket ";
            // 
            // grpBxDefaultSelections
            // 
            this.grpBxDefaultSelections.Controls.Add(this.cmbSettlCurrency);
            this.grpBxDefaultSelections.Controls.Add(this.labelOrderSide);
            this.grpBxDefaultSelections.Controls.Add(this.lblSettlCurrency);
            this.grpBxDefaultSelections.Controls.Add(this.cmbOrderSide);
            this.grpBxDefaultSelections.Controls.Add(this.cmbClearingFirm);
            this.grpBxDefaultSelections.Controls.Add(this.lblClearingFirm);
            this.grpBxDefaultSelections.Controls.Add(this.cmbOrderType);
            this.grpBxDefaultSelections.Controls.Add(this.cmbStrategy);
            this.grpBxDefaultSelections.Controls.Add(this.cmbAccount);
            this.grpBxDefaultSelections.Controls.Add(this.lblStrategy);
            this.grpBxDefaultSelections.Controls.Add(this.lblAccount);
            this.grpBxDefaultSelections.Controls.Add(this.lblTradAcc);
            this.grpBxDefaultSelections.Controls.Add(this.cmbTradingAccount);
            this.grpBxDefaultSelections.Controls.Add(this.lblOrderType);
            this.grpBxDefaultSelections.Controls.Add(this.cmbTIF);
            this.grpBxDefaultSelections.Controls.Add(this.cmbExecutionInstruction);
            this.grpBxDefaultSelections.Controls.Add(this.lblTIF);
            this.grpBxDefaultSelections.Controls.Add(this.lblExecInst);
            this.grpBxDefaultSelections.Controls.Add(this.cmbHandlingInstruction);
            this.grpBxDefaultSelections.Controls.Add(this.lblHandInst);
            this.grpBxDefaultSelections.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBxDefaultSelections.Location = new System.Drawing.Point(6, 48);
            this.grpBxDefaultSelections.Name = "grpBxDefaultSelections";
            this.grpBxDefaultSelections.Size = new System.Drawing.Size(276, 251);
            this.grpBxDefaultSelections.TabIndex = 9;
            this.grpBxDefaultSelections.Text = "Default Selections";
            // 
            // labelOrderSide
            // 
            this.labelOrderSide.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.labelOrderSide.Location = new System.Drawing.Point(6, 175);
            this.labelOrderSide.Name = "labelOrderSide";
            this.labelOrderSide.Size = new System.Drawing.Size(80, 18);
            this.labelOrderSide.TabIndex = 12;
            this.labelOrderSide.Text = "OrderSide";
            // 
            // cmbOrderSide
            // 
            this.cmbOrderSide.AutoSize = false;
            appearance109.BackColor = System.Drawing.SystemColors.Window;
            appearance109.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbOrderSide.DisplayLayout.Appearance = appearance109;
            this.cmbOrderSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbOrderSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance110.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance110.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance110.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance110.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.GroupByBox.Appearance = appearance110;
            appearance111.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance111;
            this.cmbOrderSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance112.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance112.BackColor2 = System.Drawing.SystemColors.Control;
            appearance112.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance112.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbOrderSide.DisplayLayout.GroupByBox.PromptAppearance = appearance112;
            this.cmbOrderSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbOrderSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance113.BackColor = System.Drawing.SystemColors.Window;
            appearance113.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbOrderSide.DisplayLayout.Override.ActiveCellAppearance = appearance113;
            appearance114.BackColor = System.Drawing.SystemColors.Highlight;
            appearance114.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbOrderSide.DisplayLayout.Override.ActiveRowAppearance = appearance114;
            this.cmbOrderSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbOrderSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance115.BackColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.Override.CardAreaAppearance = appearance115;
            appearance116.BorderColor = System.Drawing.Color.Silver;
            appearance116.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbOrderSide.DisplayLayout.Override.CellAppearance = appearance116;
            this.cmbOrderSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbOrderSide.DisplayLayout.Override.CellPadding = 0;
            appearance117.BackColor = System.Drawing.SystemColors.Control;
            appearance117.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance117.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance117.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance117.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbOrderSide.DisplayLayout.Override.GroupByRowAppearance = appearance117;
            appearance118.TextHAlignAsString = "Left";
            this.cmbOrderSide.DisplayLayout.Override.HeaderAppearance = appearance118;
            this.cmbOrderSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbOrderSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance119.BackColor = System.Drawing.SystemColors.Window;
            appearance119.BorderColor = System.Drawing.Color.Silver;
            this.cmbOrderSide.DisplayLayout.Override.RowAppearance = appearance119;
            this.cmbOrderSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance120.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbOrderSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance120;
            this.cmbOrderSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbOrderSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbOrderSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbOrderSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbOrderSide.DropDownWidth = 0;
            this.cmbOrderSide.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOrderSide.Location = new System.Drawing.Point(130, 175);
            this.cmbOrderSide.Name = "cmbOrderSide";
            this.cmbOrderSide.Size = new System.Drawing.Size(106, 18);
            this.cmbOrderSide.TabIndex = 11;
            this.cmbOrderSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbOrderSide.ValueChanged += Control_ValueChanged;
            // 
            // cmbClearingFirm
            // 
            this.cmbClearingFirm.AutoSize = false;
            this.cmbClearingFirm.DisplayLayout.Appearance = appearance109;
            this.cmbClearingFirm.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand10.ColHeadersVisible = false;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 1;
            ultraGridColumn4.Width = 104;
            ultraGridBand10.Columns.AddRange(new object[] {
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand10.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand10.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirm.DisplayLayout.BandsSerializer.Add(ultraGridBand10);
            this.cmbClearingFirm.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirm.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.cmbClearingFirm.DisplayLayout.GroupByBox.Appearance = appearance110;
            this.cmbClearingFirm.DisplayLayout.GroupByBox.BandLabelAppearance = appearance111;
            this.cmbClearingFirm.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirm.DisplayLayout.GroupByBox.PromptAppearance = appearance112;
            this.cmbClearingFirm.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClearingFirm.DisplayLayout.MaxRowScrollRegions = 1;
            this.cmbClearingFirm.DisplayLayout.Override.ActiveCellAppearance = appearance113;
            this.cmbClearingFirm.DisplayLayout.Override.ActiveRowAppearance = appearance114;
            this.cmbClearingFirm.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirm.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClearingFirm.DisplayLayout.Override.CardAreaAppearance = appearance115;
            this.cmbClearingFirm.DisplayLayout.Override.CellAppearance = appearance116;
            this.cmbClearingFirm.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClearingFirm.DisplayLayout.Override.CellPadding = 0;
            this.cmbClearingFirm.DisplayLayout.Override.GroupByRowAppearance = appearance117;
            this.cmbClearingFirm.DisplayLayout.Override.HeaderAppearance = appearance118;
            this.cmbClearingFirm.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClearingFirm.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            this.cmbClearingFirm.DisplayLayout.Override.RowAppearance = appearance119;
            this.cmbClearingFirm.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.cmbClearingFirm.DisplayLayout.Override.TemplateAddRowAppearance = appearance120;
            this.cmbClearingFirm.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClearingFirm.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClearingFirm.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClearingFirm.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClearingFirm.DropDownWidth = 0;
            this.cmbClearingFirm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbClearingFirm.Location = new System.Drawing.Point(130, 221);
            this.cmbClearingFirm.Name = "cmbClearingFirm";
            this.cmbClearingFirm.Size = new System.Drawing.Size(106, 18);
            this.cmbClearingFirm.TabIndex = 9;
            this.cmbClearingFirm.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClearingFirm.Visible = false;
            this.cmbClearingFirm.ValueChanged += Control_ValueChanged;
            // 
            // lblClearingFirm
            // 
            this.lblClearingFirm.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClearingFirm.Location = new System.Drawing.Point(6, 221);
            this.lblClearingFirm.Name = "lblClearingFirm";
            this.lblClearingFirm.Size = new System.Drawing.Size(118, 18);
            this.lblClearingFirm.TabIndex = 8;
            this.lblClearingFirm.Text = "Clearing Firm";
            this.lblClearingFirm.Visible = false;
            // 
            // cmbTradingAccount
            // 
            this.cmbTradingAccount.AutoSize = false;
            appearance121.BackColor = System.Drawing.SystemColors.Window;
            appearance121.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbTradingAccount.DisplayLayout.Appearance = appearance121;
            this.cmbTradingAccount.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand11.ColHeadersVisible = false;
            ultraGridColumn21.Header.VisiblePosition = 0;
            ultraGridColumn21.Width = 104;
            ultraGridColumn22.Header.VisiblePosition = 1;
            ultraGridColumn22.Hidden = true;
            ultraGridBand11.Columns.AddRange(new object[] {
            ultraGridColumn21,
            ultraGridColumn22});
            ultraGridBand11.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand11.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTradingAccount.DisplayLayout.BandsSerializer.Add(ultraGridBand11);
            this.cmbTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance122.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance122.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance122.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance122.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance122;
            appearance123.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance123;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance124.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance124.BackColor2 = System.Drawing.SystemColors.Control;
            appearance124.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance124.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance124;
            this.cmbTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance125.BackColor = System.Drawing.SystemColors.Window;
            appearance125.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance125;
            appearance126.BackColor = System.Drawing.SystemColors.Highlight;
            appearance126.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance126;
            this.cmbTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance127.BackColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance127;
            appearance128.BorderColor = System.Drawing.Color.Silver;
            appearance128.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbTradingAccount.DisplayLayout.Override.CellAppearance = appearance128;
            this.cmbTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbTradingAccount.DisplayLayout.Override.CellPadding = 0;
            appearance129.BackColor = System.Drawing.SystemColors.Control;
            appearance129.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance129.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance129.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance129.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance129;
            appearance130.TextHAlignAsString = "Left";
            this.cmbTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance130;
            this.cmbTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance131.BackColor = System.Drawing.SystemColors.Window;
            appearance131.BorderColor = System.Drawing.Color.Silver;
            this.cmbTradingAccount.DisplayLayout.Override.RowAppearance = appearance131;
            this.cmbTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance132.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance132;
            this.cmbTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbTradingAccount.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbTradingAccount.DropDownWidth = 0;
            this.cmbTradingAccount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTradingAccount.Location = new System.Drawing.Point(130, 64);
            this.cmbTradingAccount.Name = "cmbTradingAccount";
            this.cmbTradingAccount.Size = new System.Drawing.Size(106, 18);
            this.cmbTradingAccount.TabIndex = 3;
            this.cmbTradingAccount.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbTradingAccount.ValueChanged += Control_ValueChanged;
            // 
            // cmbSettlCurrency
            // 
            this.cmbSettlCurrency.AutoSize = false;
            appearance97.BackColor = System.Drawing.SystemColors.Window;
            appearance97.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSettlCurrency.DisplayLayout.Appearance = appearance97;
            this.cmbSettlCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridBand9.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 104;
            ultraGridBand9.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2});
            ultraGridBand9.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            ultraGridBand9.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSettlCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand9);
            this.cmbSettlCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSettlCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance98.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance98.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance98.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance98.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSettlCurrency.DisplayLayout.GroupByBox.Appearance = appearance98;
            appearance99.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSettlCurrency.DisplayLayout.GroupByBox.BandLabelAppearance = appearance99;
            this.cmbSettlCurrency.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance100.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance100.BackColor2 = System.Drawing.SystemColors.Control;
            appearance100.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance100.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSettlCurrency.DisplayLayout.GroupByBox.PromptAppearance = appearance100;
            this.cmbSettlCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSettlCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance101.BackColor = System.Drawing.SystemColors.Window;
            appearance101.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSettlCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance101;
            appearance102.BackColor = System.Drawing.SystemColors.Highlight;
            appearance102.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSettlCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance102;
            this.cmbSettlCurrency.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSettlCurrency.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance103.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSettlCurrency.DisplayLayout.Override.CardAreaAppearance = appearance103;
            appearance104.BorderColor = System.Drawing.Color.Silver;
            appearance104.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSettlCurrency.DisplayLayout.Override.CellAppearance = appearance104;
            this.cmbSettlCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSettlCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance105.BackColor = System.Drawing.SystemColors.Control;
            appearance105.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance105.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance105.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance105.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSettlCurrency.DisplayLayout.Override.GroupByRowAppearance = appearance105;
            appearance106.TextHAlignAsString = "Left";
            this.cmbSettlCurrency.DisplayLayout.Override.HeaderAppearance = appearance106;
            this.cmbSettlCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSettlCurrency.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance107.BackColor = System.Drawing.SystemColors.Window;
            appearance107.BorderColor = System.Drawing.Color.Silver;
            this.cmbSettlCurrency.DisplayLayout.Override.RowAppearance = appearance107;
            this.cmbSettlCurrency.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance108.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSettlCurrency.DisplayLayout.Override.TemplateAddRowAppearance = appearance108;
            this.cmbSettlCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSettlCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSettlCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSettlCurrency.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSettlCurrency.DropDownWidth = 0;
            this.cmbSettlCurrency.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSettlCurrency.Location = new System.Drawing.Point(130, 198);
            this.cmbSettlCurrency.Name = "cmbSettlCurrency";
            this.cmbSettlCurrency.Size = new System.Drawing.Size(106, 18);
            this.cmbSettlCurrency.TabIndex = 15;
            this.cmbSettlCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;            
            this.cmbSettlCurrency.ValueChanged += Control_ValueChanged;
            // 
            // lblSettlCurrency
            // 
            this.lblSettlCurrency.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettlCurrency.Location = new System.Drawing.Point(6, 198);
            this.lblSettlCurrency.Name = "lblSettlCurrency";
            this.lblSettlCurrency.Size = new System.Drawing.Size(118, 18);
            this.lblSettlCurrency.TabIndex = 14;
            this.lblSettlCurrency.Text = "Settlement Currency";
            // 
            // DefaultTradingTicketSetting
            // 
            this.Controls.Add(this.label4);
            this.Controls.Add(this.grpBxDefaultSelections);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddCVSettings);
            this.Controls.Add(this.grpbxNumerics);
            this.Controls.Add(this.grpbxCPVenue);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DefaultTradingTicketSetting";
            this.Size = new System.Drawing.Size(574, 394);
            this.Load += new System.EventHandler(this.DefaultTradingTicketSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grpbxNumerics)).EndInit();
            this.grpbxNumerics.ResumeLayout(false);
            this.grpbxNumerics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpbxCPVenue)).EndInit();
            this.grpbxCPVenue.ResumeLayout(false);
            this.grpbxCPVenue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoSetDefaultCV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbExecutionInstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbHandlingInstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTIF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbStrategy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpBxDefaultSelections)).EndInit();
            this.grpBxDefaultSelections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOrderSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClearingFirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbTradingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlCurrency)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region Combo Binding

        private void BindAccount()
        {
            try
            {
                //DataTable accountsAndAllocationDefaults = CachedDataManager.GetInstance.GetAccountsAndAllocationRules();
                //Setting values from account value property instead of accounts and allocation defaults.
                //Account value property contains user account and allocation operation preference.
                DataTable accountsAndAllocationDefaults = AccountValueCollection.Clone();
                foreach (DataRow dr in AccountValueCollection.Rows)
                {
                    if (!dr[OrderFields.PROPERTY_LEVEL1NAME].ToString().StartsWith("*Custom#_") && !dr[OrderFields.PROPERTY_LEVEL1NAME].ToString().StartsWith("*WorkArea#_") && !dr[OrderFields.PROPERTY_LEVEL1NAME].ToString().StartsWith("*PST#_"))
                    {
                        DataRow drNew = accountsAndAllocationDefaults.NewRow();
                        for (int i = 0; i < dr.Table.Columns.Count; i++)
                        {
                            drNew[i] = dr[i];
                        }
                        accountsAndAllocationDefaults.Rows.Add(drNew);
                    }
                }

                cmbAccount.DataSource = null;
                cmbAccount.DataSource = accountsAndAllocationDefaults;
                cmbAccount.DisplayMember = OrderFields.PROPERTY_LEVEL1NAME;
                cmbAccount.ValueMember = OrderFields.PROPERTY_LEVEL1ID;
                foreach (UltraGridColumn column in cmbAccount.DisplayLayout.Bands[0].Columns)
                {
                    if (column.Header.Caption.ToString() != OrderFields.PROPERTY_LEVEL1NAME)
                    {
                        column.Hidden = true;
                    }
                }
                this.cmbAccount.Rows.Band.ColHeadersVisible = false;
                this.cmbAccount.Value = int.MinValue;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        private void BindStrategy()
        {
            try
            {
                StrategyCollection strategies = new StrategyCollection();
                strategies = ClientsCommonDataManager.GetStrategies(_LoginUser.CompanyUserID);
                strategies.Insert(0, new Prana.BusinessObjects.Strategy(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbStrategy.DataSource = null;
                cmbStrategy.DataSource = strategies;
                cmbStrategy.DisplayMember = "Name";
                cmbStrategy.ValueMember = "StrategyID";
                cmbStrategy.Value = int.MinValue;
                ColumnsCollection columns = cmbStrategy.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                cmbStrategy.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }

        }
        private void BindUserCounterParty()
        {
            try
            {
                CounterPartyCollection counterParties = new CounterPartyCollection();

                counterParties = ClientsCommonDataManager.GetCounterPartiesByAUIDAndUserID(_LoginUser.CompanyUserID, _assetID, _underlyingID);
                counterParties.Insert(0, new CounterParty(int.MinValue,  ApplicationConstants.C_COMBO_SELECT));
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
                VenueCollection venues = new VenueCollection();
                venues = ClientsCommonDataManager.GetVenuesByAUIDCounterPartyAndUserID(_LoginUser.CompanyUserID, counterPartyID, _assetID, _underlyingID);
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
                cmbVenue.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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

                tradingAccounts = ClientsCommonDataManager.GetTradingAccounts(_LoginUser.CompanyUserID);
                tradingAccounts.Insert(0, new Prana.BusinessObjects.TradingAccount(int.MinValue, ApplicationConstants.C_COMBO_SELECT));
                cmbTradingAccount.DataSource = null;
                cmbTradingAccount.DataSource = tradingAccounts;

                cmbTradingAccount.DisplayMember = "Name";
                cmbTradingAccount.ValueMember = "TradingAccountID";
                cmbTradingAccount.Value = int.MinValue;
                cmbTradingAccount.DataBind();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        //private void BindCVStrategy()
        //{
        //    try
        //    {
        //        // might be changed after we make changes in the Client part. Should be actually taken from 
        //        // company counter party details tables ... as is the case with account and broker ID
        //        StrategyCollection strategies = new StrategyCollection();
        //        strategies = ClientsCommonDataManager.GetStrategies(_LoginUser.CompanyUserID);
        //        cmbStrategy.DataSource = null;
        //        cmbStrategy.DataSource = strategies;
        //        cmbStrategy.DisplayMember = "Name";
        //        cmbStrategy.ValueMember = "StrategyID";
        //        cmbStrategy.Rows.Band.ColHeadersVisible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }
        //}
        //private void BindCVAccount(int counterPartyID, int venueID)
        //{
            // Here we are selecting the FUnds corresponding to the counterPartyVenue that is selected corresponding to the user
            // where as in the user defaults we show the ones corresponding to the user
            //			//This accounts is a subset of wat we get in the UserDefaults
            //			AccountCollection accounts = new AccountCollection();
            //			accounts = ClientsCommonDataManager.GetAccounts(counterPartyID, venueID);
            //			cmbAccount.DataSource = accounts;
            //			cmbAccount.DisplayMember = "Name";
            //			cmbAccount.ValueMember = "AccountID";
        //}
        //private void BindCompanyBorrower()
        //{
        //    try
        //    {

        //        // Here we are selecting the Brokers corresponding to the counterPartyVenue that is selected corresponding to the user
        //        // where as in the user defaults we show the ones corresponding to the user
        //        //This Brokers is a subset of wat we get in the UserDefaults
        //        //			BrokerCollection brokers = new BrokerCollection();
        //        //brokers = ClientsCommonDataManager.GetBrokers(counterPartyID, venueID);	
        //        //			cmbCompanyBorrower.DataSource = brokers;
        //        //			cmbCompanyBorrower.DisplayMember = "Name";
        //        //			cmbCompanyBorrower.ValueMember = "BrokerID";
        //        int companyID = ClientsCommonDataManager.GetCompanyID(_LoginUser.CompanyUserID);
        //        Prana.Admin.BLL.Companies companies = CompanyManager.GetCompanyBorrowers(companyID);
        //        companies.Insert(0, new Prana.Admin.BLL.Company(int.MinValue, Common.C_COMBO_SELECT, Common.C_COMBO_SELECT));
        //        //cmbCompanyBorrower.DataSource = companies;
        //        //cmbCompanyBorrower.DisplayMember = "BorrowerName";
        //        //cmbCompanyBorrower.ValueMember = "CompanyBorrowerID";
        //        //cmbCompanyBorrower.Value = int.MinValue;
        //        //ColumnsCollection columns = cmbCompanyBorrower.DisplayLayout.Bands[0].Columns;
        //        foreach (UltraGridColumn column in columns)
        //        {

        //            if (column.Key != "BorrowerName")
        //            {
        //                column.Hidden = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, Common.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        private void BindOrderType(int counterPartyID, int venueID)
        {
            try
            {

                OrderTypes orderTypes = new OrderTypes();
                orderTypes = ClientsCommonDataManager.GetOrderTypesByAUCVID(_assetID, _underlyingID, counterPartyID, venueID);
                orderTypes.Insert(0, new OrderType(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue.ToString()));
                cmbOrderType.DataSource = null;
                cmbOrderType.DataSource = orderTypes;
                cmbOrderType.DisplayMember = "Type";
                cmbOrderType.ValueMember = "TagValue";
                cmbOrderType.Value = int.MinValue;

                ColumnsCollection columns = cmbOrderType.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Type")
                    {
                        column.Hidden = true;
                    }
                }
                cmbOrderType.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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

                executionInstrucitons = ClientsCommonDataManager.GetExecutionInstructionByAUCVID(_assetID, _underlyingID, counterPartyID, venueID);
                executionInstrucitons.Insert(0, new ExecutionInstruction(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue.ToString()));
                cmbExecutionInstruction.DataSource = null;
                cmbExecutionInstruction.DataSource = executionInstrucitons;
                cmbExecutionInstruction.DisplayMember = "ExecutionInstructions";
                cmbExecutionInstruction.ValueMember = "TagValue";
                cmbExecutionInstruction.Value = int.MinValue;
                ColumnsCollection columns = cmbExecutionInstruction.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "ExecutionInstructions")
                    {
                        column.Hidden = true;
                    }
                }
                cmbExecutionInstruction.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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

                HandlingInstructions handlingInstrucitons = new HandlingInstructions();
                handlingInstrucitons = ClientsCommonDataManager.GetHandlingInstructionByAUCVID(_assetID, _underlyingID, counterPartyID, venueID);
                handlingInstrucitons.Insert(0, new HandlingInstruction(int.MinValue, ApplicationConstants.C_COMBO_SELECT,int.MinValue.ToString()));
                cmbHandlingInstruction.DataSource = null;
                cmbHandlingInstruction.DataSource = handlingInstrucitons;
                cmbHandlingInstruction.DisplayMember = "Name";
                cmbHandlingInstruction.ValueMember = "TagValue";
                cmbHandlingInstruction.Value = int.MinValue;
                ColumnsCollection columns = cmbHandlingInstruction.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                cmbHandlingInstruction.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
                TimeInForces timeInForces = new TimeInForces();

                timeInForces = ClientsCommonDataManager.GetTIFByAUCVID(_assetID, _underlyingID, counterPartyID, venueID);
                timeInForces.Insert(0, new TimeInForce(int.MinValue, ApplicationConstants.C_COMBO_SELECT,int.MinValue.ToString()));
                cmbTIF.DataSource = null;
                cmbTIF.DataSource = timeInForces;
                cmbTIF.DisplayMember = "Name";
                cmbTIF.ValueMember = "TagValue";
                cmbTIF.Value = int.MinValue;
                ColumnsCollection columns = cmbTIF.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                cmbTIF.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        // Atul Dislay (29/01/2015)
        // Added To Bind The Default Trade Side
        private void BindDefaultTradeSide(int counterPartyID, int venueID)
        {
            try
            {
                Sides sides = new Sides();
                sides = ClientsCommonDataManager.GetOrderSidesByCVAUEC(_assetID, _underlyingID, counterPartyID, venueID);
                sides.Insert(0, new Side(int.MinValue, ApplicationConstants.C_COMBO_SELECT, int.MinValue.ToString()));
                cmbOrderSide.DataSource = null;
                cmbOrderSide.DataSource = sides;
                cmbOrderSide.DisplayMember = "Name";
                cmbOrderSide.ValueMember = "TagValue";
                cmbOrderSide.Value = int.MinValue;
                ColumnsCollection columns = cmbOrderSide.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                cmbOrderSide.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
       
        private void BindSettlCurrencyCombo()
        {
            try
            {
                Dictionary<int, string> dictCurrencies = CachedDataManager.GetInstance.GetAllCurrencies();
                #region Get  Currency DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                dt.Columns.Add("TagValue");
                DataRow drSELECT = dt.NewRow();
                drSELECT["Name"] = ApplicationConstants.C_COMBO_SELECT;
                drSELECT["TagValue"] = int.MinValue;
                dt.Rows.Add(drSELECT);
                foreach (KeyValuePair<int, string> item in dictCurrencies)
                {
                    DataRow dr = dt.NewRow();
                    dr["Name"] = item.Value;
                    dr["TagValue"] = item.Key;
                    dt.Rows.Add(dr);
                }
                #endregion

                cmbSettlCurrency.DataSource = null;
                cmbSettlCurrency.DataSource = dt;
                cmbSettlCurrency.DisplayMember = "Name";
                cmbSettlCurrency.ValueMember = "TagValue";
                cmbSettlCurrency.Value = int.MinValue;
                ColumnsCollection columns = cmbSettlCurrency.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    if (column.Key != "Name")
                    {
                        column.Hidden = true;
                    }
                }
                cmbSettlCurrency.Rows.Band.ColHeadersVisible = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        #region commented for CMTA and GIVEUP

        private void BindCMTA(int counterPartyID, int venueID)
        {
            int CPVenueID = TradingTicketManager.GetCompanyCPVenueIDfromCPIDVenueID(counterPartyID, venueID, _LoginUser.CompanyID);
            
            CompanyCVCMTAIdentifiers cvCMTAIdentifiers = TradingTicketManager.GetCompanyCVCMTAIdentifiers(CPVenueID);
                        
            cvCMTAIdentifiers.Insert(0, new CompanyCVCMTAIdentifier(int.MinValue, Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            cmbClearingFirm.DataSource = null;
            cmbClearingFirm.DataSource = cvCMTAIdentifiers;
            cmbClearingFirm.DataBind();
            cmbClearingFirm.DisplayMember = "CMTAIdentifier";
            //Modified by sandeep
            // cmbCMTA.ValueMember = "CompanyCounterPartyVenueIdentifierID";
            cmbClearingFirm.ValueMember = "CompanyCVCMTAIdentifierID";
            cmbClearingFirm.Value = int.MinValue;
            ColumnsCollection columns = cmbClearingFirm.DisplayLayout.Bands[0].Columns;
            foreach (UltraGridColumn column in columns)
            {
                if (column.Key != "CMTAIdentifier")
                {
                    column.Hidden = true;
                }
                else
                {
                    column.Hidden = false;
                }
            }
            cmbClearingFirm.SelectedRow = cmbClearingFirm.Rows[0];
            cmbClearingFirm.Rows.Band.ColHeadersVisible = false;
        }
                
        private void BindGiveUp(int counterPartyID, int venueID)
        {
            int CPVenueID = TradingTicketManager.GetCompanyCPVenueIDfromCPIDVenueID(counterPartyID, venueID, _LoginUser.CompanyID);
            CompanyCVGiveUpIdentifiers cvGiveUpIdentifiers = TradingTicketManager.GetCompanyCVGiveUpIdentifiers(CPVenueID);

            cmbClearingFirm.DisplayMember = "GiveUpIdentifier";
            cmbClearingFirm.ValueMember = "CompanyCVGiveUpIdentifierID";

            cvGiveUpIdentifiers.Insert(0, new CompanyCVGiveUpIdentifier(int.MinValue, Prana.Global.ApplicationConstants.C_COMBO_SELECT));
            cmbClearingFirm.DataSource = null;
            cmbClearingFirm.DataSource = cvGiveUpIdentifiers;
            cmbClearingFirm.DataBind();
            //http://www.infragistics.com/community/forums/t/35172.aspx
            cmbClearingFirm.Value = int.MinValue;
            ColumnsCollection collection = cmbClearingFirm.DisplayLayout.Bands[0].Columns;

            foreach (UltraGridColumn column in collection)
            {
                if (column.Header.Caption.ToString() != "GiveUpIdentifier")
                {
                    column.Hidden = true;
                }
                else
                {
                    column.Hidden = false;
                }
            }
            cmbClearingFirm.SelectedRow = cmbClearingFirm.Rows[0];
            cmbClearingFirm.Rows.Band.ColHeadersVisible = false;
        }
        #endregion
        #endregion

        #region ValueChanged Events
        private void cmbCounterParty_ValueChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (cmbCounterParty.Value != null)
                {
                    int counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                    BindVenue(counterPartyID);
                    SetHasChanges();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
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
                int counterPartyID = Int32.MinValue;
                if (cmbCounterParty.Value != null)
                    counterPartyID = Convert.ToInt32(cmbCounterParty.Value.ToString());
                if (cmbVenue.Value != null)
                {
                    int venueID = int.Parse(cmbVenue.Value.ToString());
                    BindHandlingInstr(counterPartyID, venueID);
                    BindOrderType(counterPartyID, venueID);
                    BindTIF(counterPartyID, venueID);
                    BindExecInstr(counterPartyID, venueID);
                    //BindCompanyBorrower();
                    BindAccount();
                    BindStrategy();
                    //Atul Dislay (29/01/2015)
                    //BInd Default TradeSide
                    BindDefaultTradeSide(counterPartyID, venueID);
                    BindSettlCurrencyCombo();
                    _preferenceUniversalSetting = _preferencesUniversalSettingsCollection.GetPref(_assetID, _underlyingID, counterPartyID, venueID);
                    if (_preferenceUniversalSetting != null)
                    {
                        #region Setting OF Combos on Basis of Saved CV's
                        // cmbVenue.Value = int.Parse(_preferenceUniversalSetting.VenueID);
                        //cmbCompanyBorrower.Value = int.Parse(_preferenceUniversalSetting.BorrowerFirmID);
                        cmbTradingAccount.Value = int.Parse(_preferenceUniversalSetting.TradingAccountID);
                        if (cmbTradingAccount.Value == null)
                        {
                            cmbTradingAccount.Value = int.MinValue;
                        }
                        cmbExecutionInstruction.Value = _preferenceUniversalSetting.ExecutionInstructionID;
                        if (cmbExecutionInstruction.Value == null)
                        {
                            cmbExecutionInstruction.Value = ApplicationConstants.C_COMBO_SELECT;
                        }
                        cmbHandlingInstruction.Value = _preferenceUniversalSetting.HandlingInstructionID;
                        if (cmbHandlingInstruction.Value == null)
                        {
                            cmbHandlingInstruction.Value = ApplicationConstants.C_COMBO_SELECT;
                        }
                        cmbAccount.Value = int.Parse(_preferenceUniversalSetting.AccountID);
                        //TODO: the following code has been added as a patch. To be removed.
                        //Checking if null as if permission for account or strategy is removed then combo value is set to null.
                        if (cmbAccount.Value == null)
                        {
                            cmbAccount.Value = int.MinValue;
                        }
                        if (!string.IsNullOrEmpty(_preferenceUniversalSetting.StrategyID))
                        {
                            cmbStrategy.Value = int.Parse(_preferenceUniversalSetting.StrategyID);
                        }
                        if (cmbStrategy.Value == null)
                        {
                            cmbStrategy.Value = int.MinValue;
                        }
                        cmbOrderType.Value = _preferenceUniversalSetting.OrderTypeID;
                        if (cmbOrderType.Value == null)
                        {
                            cmbOrderType.Value = ApplicationConstants.C_COMBO_SELECT;
                        }
                        cmbTIF.Value = _preferenceUniversalSetting.TIF;
                        if (cmbTIF.Value == null)
                        {
                            cmbTIF.Value = ApplicationConstants.C_COMBO_SELECT;
                        }
                        cmbOrderSide.Value = _preferenceUniversalSetting.OrderSide;
                        if (cmbOrderSide.Value == null)
                        {
                            cmbOrderSide.Value = int.MinValue;
                        }
                        cmbSettlCurrency.Value = _preferenceUniversalSetting.SettlCurrency;
                        if (cmbSettlCurrency.Value == null)
                        {
                            cmbSettlCurrency.Value = int.MinValue;
                        }
                        if (cmbAccount.Value == null)
                        {
                            cmbAccount.Value = int.MinValue;
                        }
                        if (cmbOrderType.Value == null)
                        {
                            cmbOrderType.Value = int.MinValue;
                        }
                        txtQuantity.Value = double.Parse(_preferenceUniversalSetting.Quantity);
                        txtDisplayQuantity.Value = double.Parse(_preferenceUniversalSetting.DisplayQuantity);
                        txtQuantityIncrement.Value = double.Parse(_preferenceUniversalSetting.QuantityIncrement);
                        txtPriceLimitIncrement.Value = double.Parse(_preferenceUniversalSetting.PriceLimitIncrement.ToString());
                        txtStopPriceIncrement.Value = double.Parse(_preferenceUniversalSetting.StopPriceIncrement);
                        txtPegOffset.Value = double.Parse(_preferenceUniversalSetting.PegOffset);
                        txtDiscrOffset.Value = double.Parse(_preferenceUniversalSetting.DiscrOffset);
                        defaultQuantityValueChkBox.Checked = _preferenceUniversalSetting.IsQuantityDefaultValueChecked;
                        if (_preferenceUniversalSetting.IsDefaultCV)
                        {
                            uoSetDefaultCV.CheckedIndex = 0;

                        }
                        else
                        {
                            uoSetDefaultCV.CheckedIndex = 1;
                        }
                        #endregion
                    }
                    int baseAssetID = Mapper.GetBaseAsset(_assetID);

                    switch (baseAssetID)
                    {
                        case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Option:
                            BindCMTA(counterPartyID, venueID);
                            if (_preferenceUniversalSetting != null)
                            {
                                cmbClearingFirm.Value = _preferenceUniversalSetting.CMTAID;
                            }
                            break;
                        case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Future:

                            BindGiveUp(counterPartyID, venueID);
                            if (_preferenceUniversalSetting != null)
                            {
                                cmbClearingFirm.Value = _preferenceUniversalSetting.GiveUpID;
                            }
                            break;
                        default:
                            break;
                    }

                }
                SetHasChanges();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void HideOrShowControlsForAssets()
        {
            lblDispQty.Visible = false;
            txtDisplayQuantity.Visible = false;

            lblIncrDiscOff.Visible = false;
            txtDiscrOffset.Visible = false;

            lblIncrPegOff.Visible = false;
            txtPegOffset.Visible = false;
            lblClearingFirm.Visible = true;
            cmbClearingFirm.Visible = true;
        }

        private void ShiftControlsUpOrDown()
        {
            lblIncrementQty.Top-= 23;
            lblIncPriceLimit.Top -= 23;
            lblIncStopPrice.Top -= 23;
            txtQuantityIncrement.Top -= 23;
            txtPriceLimitIncrement.Top -= 23;
            txtStopPriceIncrement.Top -= 23;
        }

        void cmbAccount_ValueChanged(object sender, EventArgs e)
        {
            if (cmbAccount.Value != null)
            {
                if (cmbAccount.SelectedRow != null)
                {
                    DataTable dt = ((DataTable)cmbAccount.DataSource);
                    if (Convert.ToBoolean(dt.Rows[cmbAccount.SelectedRow.Index][OrderFields.PROPERTY_ISDEFAULTALLLOCATIONRULE].ToString()))
                    {
                        cmbStrategy.Value = int.MinValue;
                        cmbStrategy.Enabled = false;
                    }
                    else
                    {
                        cmbStrategy.Enabled = true;
                    }
                }
            }
            SetHasChanges();
        }

        #endregion

        //private void AddReplacePreference(PreferencesUniversalSettings _preference)
        //{
            
        //    _preferencesUniversalSettingsCollection.Add(_preference);
            
        //}
        public PreferencesUniversalSettingsCollection GetPreferencesUniversalSettingsCollection()
        {
            return _preferencesUniversalSettingsCollection;
        }
        

        #region Preferences
        private PreferencesUniversalSettings GetPreferences(Object Sender)
        {
            Infragistics.Win.UltraWinTabControl.UltraTab ticket = (Infragistics.Win.UltraWinTabControl.UltraTab)Sender;
            DefaultTradingTicketSetting TTSettings = (DefaultTradingTicketSetting)(ticket.TabPage.Controls[0]);
            TradingTktPrefs.DeleteAllTradingTicketPreferences(_LoginUser.CompanyUserID, TTSettings.AssetID, TTSettings.UnderlyingID, int.Parse(TTSettings.cmbCounterParty.Value.ToString()), int.Parse(TTSettings.cmbVenue.Value.ToString()));
            _preferenceUniversalSetting = _preferencesUniversalSettingsCollection.GetPref(TTSettings.AssetID, TTSettings.UnderlyingID, int.Parse(TTSettings.cmbCounterParty.Value.ToString()), int.Parse(TTSettings.cmbVenue.Value.ToString()));
            if (_preferenceUniversalSetting == null)
            {
                //_preferenceUniversalSetting = new PreferencesUniversalSettings(_assetID, _underlyingID, int.Parse(cmbCounterParty.Value.ToString()), int.Parse(cmbVenue.Value.ToString()));
                _preferenceUniversalSetting = new PreferencesUniversalSettings(TTSettings.AssetID, TTSettings.UnderlyingID, int.Parse(TTSettings.cmbCounterParty.Value.ToString()), int.Parse(TTSettings.cmbVenue.Value.ToString()));
                _preferencesUniversalSettingsCollection.Add(_preferenceUniversalSetting);
            }


            #region Commented Code

            //			errorProvider.SetError(txtStopPriceIncrement,"");
            //			#region Error Provider
            //			ClearErrorProvider();
            //			string ErrorMessage="Please Enter a Numeric Value";
            //			if(!RegularExpressionValidation.IsInteger(txtQuantity.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtQuantity,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtDisplayQuantity.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtDisplayQuantity,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtQuantityIncrement.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtQuantityIncrement,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtPriceLimitIncrement.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtPriceLimitIncrement,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtStopPriceIncrement.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtStopPriceIncrement,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtPegOffset.Value.ToString()))
            //			{
            //					errorProvider.SetError(txtPegOffset,ErrorMessage);
            //					return null;
            //			}
            //			if(!RegularExpressionValidation.IsInteger(txtDiscrOffset.Value.ToString()))
            //			{
            //				errorProvider.SetError(txtDiscrOffset,ErrorMessage);
            //					return null;
            //			}
            //			
            //			
            //		
            //
            //
            //			#endregion


            //			if(Convert.ToInt32(cmbCounterParty.Value)==int.MinValue )
            //			{
            //					return null;
            //			}
            //			if(Convert.ToInt32(cmbVenue.Value)==int.MinValue )
            //			{
            //					return null;
            //			}
            //			if(cmbHandlingInstruction.Value.ToString()==Common.C_COMBO_SELECT )
            //				return null;
            //			if(Convert.ToInt32(cmbStrategy.Value)==int.MinValue )
            //				return null;
            //			if(Convert.ToInt32(cmbCompanyBorrower.Value)==int.MinValue )
            //				return null;
            //			if(cmbExecutionInstruction.Value.ToString()==Common.C_COMBO_SELECT )
            //				return null;
            //			if(Convert.ToInt32(cmbFund.Value)==int.MinValue )
            //				return null;
            //			if(cmbOrderType.Value.ToString()==Common.C_COMBO_SELECT )
            //				return null;
            //			if(cmbTIF.Value.ToString()==Common.C_COMBO_SELECT )
            //				return null;
            //			if(Convert.ToInt32(cmbTradingAccount.Value)==int.MinValue )
            //				return null;
            #endregion
            try
            {              
                _preferenceUniversalSetting.AssetID = TTSettings.AssetID.ToString();
                _preferenceUniversalSetting.UnderlyingID = TTSettings.UnderlyingID.ToString();
                _preferenceUniversalSetting.CounterPartyID = TTSettings.cmbCounterParty.Value.ToString();
                _preferenceUniversalSetting.VenueID = TTSettings.cmbVenue.Value.ToString();
                _preferenceUniversalSetting.HandlingInstructionID = TTSettings.cmbHandlingInstruction.Value.ToString();
                if (cmbStrategy.Value != null)
                {
                    _preferenceUniversalSetting.StrategyID = TTSettings.cmbStrategy.Value.ToString();
                }
                _preferenceUniversalSetting.ExecutionInstructionID = TTSettings.cmbExecutionInstruction.Value.ToString();
                _preferenceUniversalSetting.AccountID = TTSettings.cmbAccount.Value.ToString();
                _preferenceUniversalSetting.OrderTypeID = TTSettings.cmbOrderType.Value.ToString();
                _preferenceUniversalSetting.TIF = TTSettings.cmbTIF.Value.ToString();
                _preferenceUniversalSetting.TradingAccountID = TTSettings.cmbTradingAccount.Value.ToString();
                _preferenceUniversalSetting.Quantity = TTSettings.txtQuantity.Value.ToString();
                _preferenceUniversalSetting.DisplayQuantity = TTSettings.txtDisplayQuantity.Value.ToString();
                _preferenceUniversalSetting.QuantityIncrement = TTSettings.txtQuantityIncrement.Value.ToString();
                _preferenceUniversalSetting.PriceLimitIncrement = TTSettings.txtPriceLimitIncrement.Value.ToString();
                _preferenceUniversalSetting.StopPriceIncrement = TTSettings.txtStopPriceIncrement.Value.ToString();
                _preferenceUniversalSetting.PegOffset = TTSettings.txtPegOffset.Value.ToString();
                _preferenceUniversalSetting.DiscrOffset = TTSettings.txtDiscrOffset.Value.ToString();
                _preferenceUniversalSetting.OrderSide = TTSettings.cmbOrderSide.Value.ToString();
                _preferenceUniversalSetting.SettlCurrency = TTSettings.cmbSettlCurrency.Value.ToString();
                _preferenceUniversalSetting.IsDefaultCV = TTSettings.uoSetDefaultCV.CheckedIndex == 0 ? true : false;
                _preferenceUniversalSetting.CompanyUserID = _LoginUser.CompanyUserID;
                _preferenceUniversalSetting.IsQuantityDefaultValueChecked = TTSettings.defaultQuantityValueChkBox.Checked;
                int baseAssetID = Mapper.GetBaseAsset(TTSettings._assetID);
                switch (baseAssetID)
                {
                    case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Equity:
                        
                        break;
                    case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Option:
                        _preferenceUniversalSetting.CMTAID = (int)TTSettings.cmbClearingFirm.Value;
                        break;
                    case (int)Prana.BusinessObjects.AppConstants.AssetCategory.Future:
                        _preferenceUniversalSetting.GiveUpID = (int)TTSettings.cmbClearingFirm.Value;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return _preferenceUniversalSetting;
        }

        //private void ClearErrorProvider()
        //{
        //    try
        //    {
        //        errorProvider.SetError(txtDiscrOffset, "");
        //        errorProvider.SetError(txtDisplayQuantity, "");
        //        errorProvider.SetError(txtPegOffset, "");
        //        errorProvider.SetError(txtPriceLimitIncrement, "");
        //        errorProvider.SetError(txtQuantity, "");
        //        errorProvider.SetError(txtQuantityIncrement, "");
        //        errorProvider.SetError(txtStopPriceIncrement, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
        //        if (rethrow)
        //        {
        //            throw;
        //        }
        //    }

        //}
        private void SetPreferences(PreferencesUniversalSettings preferences)
        {
            try
            {
                _preferenceUniversalSetting = preferences;
                if (_preferenceUniversalSetting != null)
                {
                    cmbCounterParty.Value = Convert.ToInt32(preferences.CounterPartyID);
                    BindVenue(Convert.ToInt32(preferences.CounterPartyID));
                    cmbVenue.Value = Convert.ToInt32(preferences.VenueID);
                }
                else
                {
                    int counterPartyID = int.Parse(cmbCounterParty.Value.ToString());
                    int venueID = int.Parse(cmbVenue.Value.ToString());
                    BindHandlingInstr(counterPartyID, venueID);
                    BindOrderType(counterPartyID, venueID);
                    BindTIF(counterPartyID, venueID);
                    BindExecInstr(counterPartyID, venueID);
                    BindAccount();
                    BindStrategy();
                    BindDefaultTradeSide(counterPartyID, venueID);
                    BindSettlCurrencyCombo();
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        #endregion



        public  void TicketPreferenceControl_RefreshUserControl(object sender, EventArgs e)
        {
            try
            {
                PreferencesUniversalSettings prefs = GetPreferences(sender);
                TradingTktPrefs.SavePrefs(prefs);
                _hasChanges = false;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void DefaultTradingTicketSetting_Load(object sender, EventArgs e)
        {
            try
            {
                _hasChanges = false;
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_PREFERENCES);
                if (!CustomThemeHelper.ApplyTheme)
                {
                    this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
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
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
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
                btnAddCVSettings.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddCVSettings.ForeColor = System.Drawing.Color.White;
                btnAddCVSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddCVSettings.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddCVSettings.UseAppStyling = false;
                btnAddCVSettings.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void cmbVenue_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                SetHasChanges();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        void Control_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (_hasChanges == false)
                    SetHasChanges();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetHasChanges()
        {
            try
            {
                if (TicketPreferenceControl.IsTabsCreated())
                {
                    _hasChanges = true;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        public static bool GetHasChanges()
        {
            try
            {
                return _hasChanges;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }

        public static bool SetDonotSave()
        {
            try
            {
                _hasChanges = false; ;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return false;
        }
        public static string GetAUCVKey(object Sender)
        {
            try
            {
                Infragistics.Win.UltraWinTabControl.UltraTab ticket = (Infragistics.Win.UltraWinTabControl.UltraTab)Sender;
                DefaultTradingTicketSetting TTSettings = (DefaultTradingTicketSetting)(ticket.TabPage.Controls[0]);
                return "A" + TTSettings.AssetID.ToString() + "U" + TTSettings.UnderlyingID.ToString() + "C" + TTSettings.cmbCounterParty.Value.ToString() + "V" + TTSettings.cmbVenue.Value.ToString();
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets or sets the account value collection.
        /// Contains CashAccounts and allocation preferences
        /// </summary>
        /// <value>The account value collection.</value>
        public DataTable AccountValueCollection { get; set; }

        private void spinner_Validated(object sender, EventArgs e)
        {
            if (((Spinner)sender).Value == 0D)
            {
                ((Spinner)sender).Value = 0.0001D;
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the defaultQuantityValueChkBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void defaultQuantityValueChkBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtQuantity.Enabled = ((sender as CheckBox) == null || !(sender as CheckBox).Checked);
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }
    }
}
