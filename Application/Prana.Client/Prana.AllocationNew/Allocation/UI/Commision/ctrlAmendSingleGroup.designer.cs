using Prana.Global;
using System;
namespace Prana.AllocationNew
{
    partial class ctrlAmendSingleGroup
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                components.Dispose();

                if(this._sides != null)
                    _sides.Dispose();
            }
            if (cmbFXConvOperator != null)
            {
                this.cmbFXConvOperator.ValueChanged -= new System.EventHandler(this.cmbFXConvOperator_ValueChanged);
                this.cmbFXConvOperator.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
                this.cmbFXConvOperator.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
                this.cmbFXConvOperator.Leave -= new System.EventHandler(this.cmbFXConvOperator_Leave);
                this.cmbFXConvOperator.MouseCaptureChanged -= new System.EventHandler(this.cmb_MouseCaptureChanged);
                this.cmbFXConvOperator.Dispose();
                this.cmbFXConvOperator = null;

            }
            if (cmbSettlFXConvOperator != null)
            {
                this.cmbSettlFXConvOperator.ValueChanged -= new System.EventHandler(this.cmbSettlFXConvOperator_ValueChanged);
                this.cmbSettlFXConvOperator.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
                this.cmbSettlFXConvOperator.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
                this.cmbSettlFXConvOperator.Leave -= new System.EventHandler(this.cmbSettlFXConvOperator_Leave);
                this.cmbSettlFXConvOperator.MouseCaptureChanged -= new System.EventHandler(this.cmb_MouseCaptureChanged);
                this.cmbSettlFXConvOperator.Dispose();
                this.cmbSettlFXConvOperator = null;

            }
            if (cmbVenue != null)
            {
                this.cmbVenue.ValueChanged -= new System.EventHandler(this.cmbVenue_ValueChanged);
                this.cmbVenue.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
                this.cmbVenue.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
                this.cmbVenue.Leave -= new System.EventHandler(this.cmbVenue_Leave);
                this.cmbVenue.MouseCaptureChanged -= new System.EventHandler(this.cmb_MouseCaptureChanged);
                this.cmbVenue.Dispose();
                this.cmbVenue = null;
            }

            if (cmbCounterParty != null)
            {
                this.cmbCounterParty.ValueChanged -= new System.EventHandler(this.cmbCounterParty_ValueChanged);
                this.cmbCounterParty.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
                this.cmbCounterParty.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
                this.cmbCounterParty.Leave -= new System.EventHandler(this.cmbCounterParty_Leave);
                this.cmbCounterParty.MouseCaptureChanged -= new System.EventHandler(this.cmb_MouseCaptureChanged);
                this.cmbCounterParty.Dispose();
                this.cmbCounterParty = null;
            }

            if (cmbSide != null)
            {
                this.cmbSide.ValueChanged -= new System.EventHandler(this.cmbSide_ValueChanged);
                this.cmbSide.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
                this.cmbSide.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
                this.cmbSide.Leave -= new System.EventHandler(this.cmbSide_Leave);
                this.cmbSide.MouseCaptureChanged -= new System.EventHandler(this.cmb_MouseCaptureChanged);
                this.cmbSide.Dispose();
                this.cmbSide = null;
            }

            if (this.dtpSettleDate != null)
            {
                this.dtpSettleDate.AfterCloseUp -= new System.EventHandler(this.dtpSettleDate_AfterCloseUp);
                this.dtpSettleDate.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
                this.dtpSettleDate.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
                this.dtpSettleDate.Leave -= new System.EventHandler(this.dtpSettleDate_Leave);
                this.dtpSettleDate.MouseCaptureChanged -= new System.EventHandler(this.dtp_MouseCaptureChanged);
                this.dtpSettleDate.Dispose();
                this.dtpSettleDate = null;
            }

            if (this.dtpOriginalPurchaseDate != null)
            {
                this.dtpOriginalPurchaseDate.AfterCloseUp -= new System.EventHandler(this.dtpOriginalPurchaseDate_AfterCloseUp);
                this.dtpOriginalPurchaseDate.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
                this.dtpOriginalPurchaseDate.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
                this.dtpOriginalPurchaseDate.Leave -= new System.EventHandler(this.dtpOriginalPurchaseDate_Leave);
                this.dtpOriginalPurchaseDate.MouseCaptureChanged -= new System.EventHandler(this.dtp_MouseCaptureChanged);
                this.dtpOriginalPurchaseDate.Dispose();
                this.dtpOriginalPurchaseDate = null;
            }

            if (this.dtpProcessDate != null)
            {
                this.dtpProcessDate.AfterCloseUp -= new System.EventHandler(this.dtpProcessDate_AfterCloseUp);
                this.dtpProcessDate.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
                this.dtpProcessDate.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
                this.dtpProcessDate.Leave -= new System.EventHandler(this.dtpProcessDate_Leave);
                this.dtpProcessDate.MouseCaptureChanged -= new System.EventHandler(this.dtp_MouseCaptureChanged);
                this.dtpProcessDate.Dispose();
                this.dtpProcessDate = null;
            }

            if (this.txtAttribute1 != null)
            {
                this.txtAttribute1.ValueChanged -= new System.EventHandler(this.txtAttribute1_ValueChanged);
                this.txtAttribute1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute1.Leave -= new System.EventHandler(this.txtAttribute1_Leave);
                this.txtAttribute1.Dispose();
                this.txtAttribute1 = null;
            }

            if (this.txtAttribute2 != null)
            {
                this.txtAttribute2.ValueChanged -= new System.EventHandler(this.txtAttribute2_ValueChanged);
                this.txtAttribute2.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute2.Leave -= new System.EventHandler(this.txtAttribute2_Leave);
                this.txtAttribute2.Dispose();
                this.txtAttribute2 = null;
            }

            if (this.txtAttribute3 != null)
            {
                this.txtAttribute3.ValueChanged -= new System.EventHandler(this.txtAttribute3_ValueChanged);
                this.txtAttribute3.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute3.Leave -= new System.EventHandler(this.txtAttribute3_Leave);
                this.txtAttribute3.Dispose();
                this.txtAttribute3 = null;
            }

            if (this.txtAttribute4 != null)
            {
                this.txtAttribute4.ValueChanged -= new System.EventHandler(this.txtAttribute4_ValueChanged);
                this.txtAttribute4.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute4.Leave -= new System.EventHandler(this.txtAttribute4_Leave);
                this.txtAttribute4.Dispose();
                this.txtAttribute4 = null;
            }

            if (this.txtAttribute5 != null)
            {
                this.txtAttribute5.ValueChanged -= new System.EventHandler(this.txtAttribute5_ValueChanged);
                this.txtAttribute5.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute5.Leave -= new System.EventHandler(this.txtAttribute5_Leave);
                this.txtAttribute5.Dispose();
                this.txtAttribute5 = null;
            }

            if (this.txtAttribute6 != null)
            {
                this.txtAttribute6.ValueChanged -= new System.EventHandler(this.txtAttribute6_ValueChanged);
                this.txtAttribute6.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
                this.txtAttribute6.Leave -= new System.EventHandler(this.txtAttribute6_Leave);
                this.txtAttribute6.Dispose();
                this.txtAttribute6 = null;
            }

            this._securityMaster = null;
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand3 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand4 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand5 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.ultraExpandableGroupBoxTradeAttributes = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel6 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.txtAttribute6 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTradeAttribute6 = new System.Windows.Forms.Label();
            this.txtAttribute1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtAttribute4 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtAttribute5 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtAttribute3 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.txtAttribute2 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.lblTradeAttribute1 = new System.Windows.Forms.Label();
            this.lblTradeAttribute2 = new System.Windows.Forms.Label();
            this.lblTradeAttribute3 = new System.Windows.Forms.Label();
            this.lblTradeAttribute5 = new System.Windows.Forms.Label();
            this.lblTradeAttribute4 = new System.Windows.Forms.Label();
            this.ultraExpandableGrpboxOtherDetails = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel3 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.cmbSettlFXConvOperator = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtSettlCurrFxRate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSettlFXRateOperator = new System.Windows.Forms.Label();
            this.lblSettlFXRate = new System.Windows.Forms.Label();
            this.txtInternalComments = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label19 = new System.Windows.Forms.Label();
            this.cmbFXConvOperator = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbVenue = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbCounterParty = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.txtAccruedInterest = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtFxRate = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtDescription = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.ultraExpandableGrpboxCommissionAndFee = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSoftCommission = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label17 = new System.Windows.Forms.Label();
            this.txtClearingBrokerFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTotalComm = new Infragistics.Win.Misc.UltraLabel();
            this.label21 = new System.Windows.Forms.Label();
            this.txtClearingFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTaxOnCommissions = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtMiscFees = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtOtherBrokerFees = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtStampDuty = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtTransactionLevy = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtSecFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtOccFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtOrfFee = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtCommission = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ultraExpandableGrpboxBasicDetails = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.lblAsset = new Infragistics.Win.Misc.UltraLabel();
            this.lblSymbol = new Infragistics.Win.Misc.UltraLabel();
            this.cmbSide = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.dtpSettleDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.label28 = new System.Windows.Forms.Label();
            this.dtpOriginalPurchaseDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.label27 = new System.Windows.Forms.Label();
            this.dtpProcessDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.label26 = new System.Windows.Forms.Label();
            this.dtpTradeDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.txtAvgPrice = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.txtExecutedQty = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBoxPanel4 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.textBox1 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.textBox3 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.textBox4 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.textBox5 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.textBox6 = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraDropDownButton1 = new Infragistics.Win.Misc.UltraDropDownButton();
            this.ultraDropDownButton2 = new Infragistics.Win.Misc.UltraDropDownButton();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.statusProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtSettlCurrAmt = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblSettlFXRateAmt = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxTradeAttributes)).BeginInit();
            this.ultraExpandableGroupBoxTradeAttributes.SuspendLayout();
            this.ultraExpandableGroupBoxPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxOtherDetails)).BeginInit();
            this.ultraExpandableGrpboxOtherDetails.SuspendLayout();
            this.ultraExpandableGroupBoxPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlFXConvOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettlCurrFxRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInternalComments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFXConvOperator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccruedInterest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFxRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxCommissionAndFee)).BeginInit();
            this.ultraExpandableGrpboxCommissionAndFee.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoftCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClearingBrokerFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClearingFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxOnCommissions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMiscFees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherBrokerFees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStampDuty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionLevy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSecFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOccFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrfFee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxBasicDetails)).BeginInit();
            this.ultraExpandableGrpboxBasicDetails.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSettleDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpOriginalPurchaseDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpProcessDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTradeDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAvgPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExecutedQty)).BeginInit();
            this.ultraExpandableGroupBoxPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettlCurrAmt)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.ultraGroupBox1);
            this.panel1.Controls.Add(this.ultraExpandableGroupBoxTradeAttributes);
            this.panel1.Controls.Add(this.ultraExpandableGrpboxOtherDetails);
            this.panel1.Controls.Add(this.ultraExpandableGrpboxCommissionAndFee);
            this.panel1.Controls.Add(this.ultraExpandableGrpboxBasicDetails);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 344);
            this.panel1.TabIndex = 1;
            // 
            // ultraGroupBox1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraGroupBox1.Appearance = appearance1;
            this.ultraGroupBox1.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rectangular3D;
            this.ultraGroupBox1.Controls.Add(this.btnExit);
            this.ultraGroupBox1.Controls.Add(this.btnApply);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 867);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(223, 43);
            this.ultraGroupBox1.TabIndex = 4;
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnExit.Location = new System.Drawing.Point(112, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(76, 20);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Cancel";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnApply.Location = new System.Drawing.Point(36, 12);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(76, 20);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // ultraExpandableGroupBoxTradeAttributes
            // 
            appearance2.BackColor = System.Drawing.Color.FloralWhite;
            this.ultraExpandableGroupBoxTradeAttributes.Appearance = appearance2;
            this.ultraExpandableGroupBoxTradeAttributes.Controls.Add(this.ultraExpandableGroupBoxPanel6);
            this.ultraExpandableGroupBoxTradeAttributes.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBoxTradeAttributes.Expanded = false;
            this.ultraExpandableGroupBoxTradeAttributes.ExpandedSize = new System.Drawing.Size(223, 171);
            this.ultraExpandableGroupBoxTradeAttributes.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Far;
            appearance3.Cursor = System.Windows.Forms.Cursors.Hand;
            appearance3.FontData.Name = "Tahoma";
            appearance3.FontData.SizeInPoints = 10F;
            this.ultraExpandableGroupBoxTradeAttributes.HeaderAppearance = appearance3;
            this.ultraExpandableGroupBoxTradeAttributes.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.ToggleExpansion;
            this.ultraExpandableGroupBoxTradeAttributes.Location = new System.Drawing.Point(0, 843);
            this.ultraExpandableGroupBoxTradeAttributes.Name = "ultraExpandableGroupBoxTradeAttributes";
            this.ultraExpandableGroupBoxTradeAttributes.Size = new System.Drawing.Size(223, 24);
            this.ultraExpandableGroupBoxTradeAttributes.TabIndex = 3;
            this.ultraExpandableGroupBoxTradeAttributes.Text = "&Trade Attributes";
            this.ultraExpandableGroupBoxTradeAttributes.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGroupBoxTradeAttributes.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel6
            // 
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute6);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute6);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute1);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute4);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute5);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute3);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.txtAttribute2);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute1);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute2);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute3);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute5);
            this.ultraExpandableGroupBoxPanel6.Controls.Add(this.lblTradeAttribute4);
            this.ultraExpandableGroupBoxPanel6.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraExpandableGroupBoxPanel6.Name = "ultraExpandableGroupBoxPanel6";
            this.ultraExpandableGroupBoxPanel6.Size = new System.Drawing.Size(219, 147);
            this.ultraExpandableGroupBoxPanel6.TabIndex = 0;
            this.ultraExpandableGroupBoxPanel6.Visible = false;
            // 
            // txtAttribute6
            // 
            this.txtAttribute6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute6.Location = new System.Drawing.Point(122, 123);
            this.txtAttribute6.MaxLength = 200;
            this.txtAttribute6.Name = "txtAttribute6";
            this.txtAttribute6.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute6.TabIndex = 11;
            this.txtAttribute6.ValueChanged += new System.EventHandler(this.txtAttribute6_ValueChanged);
            this.txtAttribute6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute6.Leave += new System.EventHandler(this.txtAttribute6_Leave);
            // 
            // lblTradeAttribute6
            // 
            this.lblTradeAttribute6.AutoSize = true;
            this.lblTradeAttribute6.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute6.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute6.Location = new System.Drawing.Point(3, 126);
            this.lblTradeAttribute6.Name = "lblTradeAttribute6";
            this.lblTradeAttribute6.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute6.TabIndex = 10;
            this.lblTradeAttribute6.Text = "TradeAttribute6:";
            // 
            // txtAttribute1
            // 
            this.txtAttribute1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute1.Location = new System.Drawing.Point(122, 3);
            this.txtAttribute1.MaxLength = 200;
            this.txtAttribute1.Name = "txtAttribute1";
            this.txtAttribute1.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute1.TabIndex = 1;
            this.txtAttribute1.ValueChanged += new System.EventHandler(this.txtAttribute1_ValueChanged);
            this.txtAttribute1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute1.Leave += new System.EventHandler(this.txtAttribute1_Leave);
            // 
            // txtAttribute4
            // 
            this.txtAttribute4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute4.Location = new System.Drawing.Point(122, 75);
            this.txtAttribute4.MaxLength = 200;
            this.txtAttribute4.Name = "txtAttribute4";
            this.txtAttribute4.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute4.TabIndex = 7;
            this.txtAttribute4.ValueChanged += new System.EventHandler(this.txtAttribute4_ValueChanged);
            this.txtAttribute4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute4.Leave += new System.EventHandler(this.txtAttribute4_Leave);
            // 
            // txtAttribute5
            // 
            this.txtAttribute5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute5.Location = new System.Drawing.Point(122, 99);
            this.txtAttribute5.MaxLength = 200;
            this.txtAttribute5.Name = "txtAttribute5";
            this.txtAttribute5.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute5.TabIndex = 9;
            this.txtAttribute5.ValueChanged += new System.EventHandler(this.txtAttribute5_ValueChanged);
            this.txtAttribute5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute5.Leave += new System.EventHandler(this.txtAttribute5_Leave);
            // 
            // txtAttribute3
            // 
            this.txtAttribute3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute3.Location = new System.Drawing.Point(122, 51);
            this.txtAttribute3.MaxLength = 200;
            this.txtAttribute3.Name = "txtAttribute3";
            this.txtAttribute3.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute3.TabIndex = 5;
            this.txtAttribute3.ValueChanged += new System.EventHandler(this.txtAttribute3_ValueChanged);
            this.txtAttribute3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute3.Leave += new System.EventHandler(this.txtAttribute3_Leave);
            // 
            // txtAttribute2
            // 
            this.txtAttribute2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAttribute2.Location = new System.Drawing.Point(122, 27);
            this.txtAttribute2.MaxLength = 200;
            this.txtAttribute2.Name = "txtAttribute2";
            this.txtAttribute2.Size = new System.Drawing.Size(79, 21);
            this.txtAttribute2.TabIndex = 3;
            this.txtAttribute2.ValueChanged += new System.EventHandler(this.txtAttribute2_ValueChanged);
            this.txtAttribute2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtAttribute2.Leave += new System.EventHandler(this.txtAttribute2_Leave);
            // 
            // lblTradeAttribute1
            // 
            this.lblTradeAttribute1.AutoSize = true;
            this.lblTradeAttribute1.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute1.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute1.Location = new System.Drawing.Point(3, 6);
            this.lblTradeAttribute1.Name = "lblTradeAttribute1";
            this.lblTradeAttribute1.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute1.TabIndex = 0;
            this.lblTradeAttribute1.Text = "TradeAttribute1:";
            // 
            // lblTradeAttribute2
            // 
            this.lblTradeAttribute2.AutoSize = true;
            this.lblTradeAttribute2.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute2.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute2.Location = new System.Drawing.Point(3, 30);
            this.lblTradeAttribute2.Name = "lblTradeAttribute2";
            this.lblTradeAttribute2.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute2.TabIndex = 2;
            this.lblTradeAttribute2.Text = "TradeAttribute2:";
            // 
            // lblTradeAttribute3
            // 
            this.lblTradeAttribute3.AutoSize = true;
            this.lblTradeAttribute3.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute3.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute3.Location = new System.Drawing.Point(3, 54);
            this.lblTradeAttribute3.Name = "lblTradeAttribute3";
            this.lblTradeAttribute3.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute3.TabIndex = 4;
            this.lblTradeAttribute3.Text = "TradeAttribute3:";
            // 
            // lblTradeAttribute5
            // 
            this.lblTradeAttribute5.AutoSize = true;
            this.lblTradeAttribute5.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute5.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute5.Location = new System.Drawing.Point(3, 102);
            this.lblTradeAttribute5.Name = "lblTradeAttribute5";
            this.lblTradeAttribute5.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute5.TabIndex = 8;
            this.lblTradeAttribute5.Text = "TradeAttribute5:";
            // 
            // lblTradeAttribute4
            // 
            this.lblTradeAttribute4.AutoSize = true;
            this.lblTradeAttribute4.BackColor = System.Drawing.Color.Transparent;
            this.lblTradeAttribute4.ForeColor = System.Drawing.Color.Black;
            this.lblTradeAttribute4.Location = new System.Drawing.Point(3, 78);
            this.lblTradeAttribute4.Name = "lblTradeAttribute4";
            this.lblTradeAttribute4.Size = new System.Drawing.Size(83, 13);
            this.lblTradeAttribute4.TabIndex = 6;
            this.lblTradeAttribute4.Text = "TradeAttribute4:";
            // 
            // ultraExpandableGrpboxOtherDetails
            // 
            appearance4.BackColor = System.Drawing.Color.FloralWhite;
            this.ultraExpandableGrpboxOtherDetails.Appearance = appearance4;
            this.ultraExpandableGrpboxOtherDetails.Controls.Add(this.ultraExpandableGroupBoxPanel3);
            this.ultraExpandableGrpboxOtherDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGrpboxOtherDetails.ExpandedSize = new System.Drawing.Size(223, 270);
            this.ultraExpandableGrpboxOtherDetails.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Far;
            appearance53.Cursor = System.Windows.Forms.Cursors.Hand;
            appearance53.FontData.Name = "Tahoma";
            appearance53.FontData.SizeInPoints = 10F;
            this.ultraExpandableGrpboxOtherDetails.HeaderAppearance = appearance53;
            this.ultraExpandableGrpboxOtherDetails.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.ToggleExpansion;
            this.ultraExpandableGrpboxOtherDetails.Location = new System.Drawing.Point(0, 573);
            this.ultraExpandableGrpboxOtherDetails.Name = "ultraExpandableGrpboxOtherDetails";
            this.ultraExpandableGrpboxOtherDetails.Size = new System.Drawing.Size(223, 270);
            this.ultraExpandableGrpboxOtherDetails.TabIndex = 2;
            this.ultraExpandableGrpboxOtherDetails.Text = "&Other Details";
            this.ultraExpandableGrpboxOtherDetails.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGrpboxOtherDetails.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel3
            // 
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtSettlCurrAmt);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.lblSettlFXRateAmt);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.cmbSettlFXConvOperator);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtSettlCurrFxRate);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.lblSettlFXRateOperator);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.lblSettlFXRate);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtInternalComments);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label19);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.cmbFXConvOperator);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.cmbVenue);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.cmbCounterParty);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtAccruedInterest);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtFxRate);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.txtDescription);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label39);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label40);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label41);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label42);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label43);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label44);
            this.ultraExpandableGroupBoxPanel3.Controls.Add(this.label33);
            this.ultraExpandableGroupBoxPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel3.Location = new System.Drawing.Point(2, 22);
            this.ultraExpandableGroupBoxPanel3.Name = "ultraExpandableGroupBoxPanel3";
            this.ultraExpandableGroupBoxPanel3.Size = new System.Drawing.Size(219, 246);
            this.ultraExpandableGroupBoxPanel3.TabIndex = 0;
            // 
            // cmbSettlFXConvOperator
            // 
            this.cmbSettlFXConvOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSettlFXConvOperator.DisplayLayout.Appearance = appearance5;
            ultraGridBand1.ColHeadersVisible = false;
            this.cmbSettlFXConvOperator.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbSettlFXConvOperator.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSettlFXConvOperator.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance6.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance6.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSettlFXConvOperator.DisplayLayout.GroupByBox.Appearance = appearance6;
            appearance7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSettlFXConvOperator.DisplayLayout.GroupByBox.BandLabelAppearance = appearance7;
            this.cmbSettlFXConvOperator.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance8.BackColor2 = System.Drawing.SystemColors.Control;
            appearance8.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance8.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSettlFXConvOperator.DisplayLayout.GroupByBox.PromptAppearance = appearance8;
            this.cmbSettlFXConvOperator.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSettlFXConvOperator.DisplayLayout.MaxRowScrollRegions = 1;
            appearance9.BackColor = System.Drawing.SystemColors.Window;
            appearance9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.ActiveCellAppearance = appearance9;
            appearance10.BackColor = System.Drawing.SystemColors.Highlight;
            appearance10.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.ActiveRowAppearance = appearance10;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.CardAreaAppearance = appearance11;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            appearance12.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.CellAppearance = appearance12;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.CellPadding = 0;
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            appearance13.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance13.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance13.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.GroupByRowAppearance = appearance13;
            appearance14.TextHAlignAsString = "Left";
            this.cmbSettlFXConvOperator.DisplayLayout.Override.HeaderAppearance = appearance14;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance15.BackColor = System.Drawing.SystemColors.Window;
            appearance15.BorderColor = System.Drawing.Color.Silver;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.RowAppearance = appearance15;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSettlFXConvOperator.DisplayLayout.Override.TemplateAddRowAppearance = appearance16;
            this.cmbSettlFXConvOperator.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSettlFXConvOperator.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSettlFXConvOperator.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSettlFXConvOperator.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSettlFXConvOperator.DropDownWidth = 0;
            this.cmbSettlFXConvOperator.Location = new System.Drawing.Point(122, 173);
            this.cmbSettlFXConvOperator.Name = "cmbSettlFXConvOperator";
            this.cmbSettlFXConvOperator.Size = new System.Drawing.Size(79, 22);
            this.cmbSettlFXConvOperator.TabIndex = 17;
            this.cmbSettlFXConvOperator.ValueChanged += new System.EventHandler(this.cmbSettlFXConvOperator_ValueChanged);
            this.cmbSettlFXConvOperator.EnabledChanged += cmbSettlFXConvOperator_EnabledChanged;
            this.cmbSettlFXConvOperator.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
            this.cmbSettlFXConvOperator.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
            this.cmbSettlFXConvOperator.Leave += new System.EventHandler(this.cmbSettlFXConvOperator_Leave);
            this.cmbSettlFXConvOperator.MouseCaptureChanged += new System.EventHandler(this.cmb_MouseCaptureChanged);
            // 
            // txtSettlCurrFxRate
            // 
            this.txtSettlCurrFxRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSettlCurrFxRate.Location = new System.Drawing.Point(122, 149);
            this.txtSettlCurrFxRate.Name = "txtSettlCurrFxRate";
            this.txtSettlCurrFxRate.Size = new System.Drawing.Size(79, 21);
            this.txtSettlCurrFxRate.TabIndex = 15;
            this.txtSettlCurrFxRate.ValueChanged += new System.EventHandler(this.txtSettlFxRate_ValueChanged);
            this.txtSettlCurrFxRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtSettlCurrFxRate.Leave += new System.EventHandler(this.txtSettlFxRate_Leave);
            this.txtSettlCurrFxRate.EnabledChanged += txtSettlCurrFxRate_EnabledChanged;

            // 
            // lblSettlFXRateOperator
            // 
            this.lblSettlFXRateOperator.AutoSize = true;
            this.lblSettlFXRateOperator.BackColor = System.Drawing.Color.Transparent;
            this.lblSettlFXRateOperator.ForeColor = System.Drawing.Color.Black;
            this.lblSettlFXRateOperator.Location = new System.Drawing.Point(6, 174);
            this.lblSettlFXRateOperator.Name = "lblSettlFXRateOperator";
            this.lblSettlFXRateOperator.Size = new System.Drawing.Size(92, 13);
            this.lblSettlFXRateOperator.TabIndex = 16;
            this.lblSettlFXRateOperator.Text = "Settl Fx  Operator:";
            // 
            // lblSettlFXRate
            // 
            this.lblSettlFXRate.AutoSize = true;
            this.lblSettlFXRate.BackColor = System.Drawing.Color.Transparent;
            this.lblSettlFXRate.ForeColor = System.Drawing.Color.Black;
            this.lblSettlFXRate.Location = new System.Drawing.Point(6, 150);
            this.lblSettlFXRate.Name = "lblSettlFXRate";
            this.lblSettlFXRate.Size = new System.Drawing.Size(71, 13);
            this.lblSettlFXRate.TabIndex = 14;
            this.lblSettlFXRate.Text = "Settl Fx Rate:";
            // 
            // txtInternalComments
            // 
            this.txtInternalComments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInternalComments.Location = new System.Drawing.Point(122, 75);
            this.txtInternalComments.Name = "txtInternalComments";
            this.txtInternalComments.Size = new System.Drawing.Size(79, 21);
            this.txtInternalComments.TabIndex = 13;
            this.txtInternalComments.ValueChanged += new System.EventHandler(this.txtInternalComments_ValueChanged);
            this.txtInternalComments.Leave += new System.EventHandler(this.txtInternalComments_Leave);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(3, 78);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(94, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Internal Comments";
            // 
            // cmbFXConvOperator
            // 
            this.cmbFXConvOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbFXConvOperator.DisplayLayout.Appearance = appearance17;
            ultraGridBand2.ColHeadersVisible = false;
            this.cmbFXConvOperator.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbFXConvOperator.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbFXConvOperator.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFXConvOperator.DisplayLayout.GroupByBox.Appearance = appearance18;
            appearance19.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFXConvOperator.DisplayLayout.GroupByBox.BandLabelAppearance = appearance19;
            this.cmbFXConvOperator.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance20.BackColor2 = System.Drawing.SystemColors.Control;
            appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance20.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbFXConvOperator.DisplayLayout.GroupByBox.PromptAppearance = appearance20;
            this.cmbFXConvOperator.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbFXConvOperator.DisplayLayout.MaxRowScrollRegions = 1;
            appearance21.BackColor = System.Drawing.SystemColors.Window;
            appearance21.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbFXConvOperator.DisplayLayout.Override.ActiveCellAppearance = appearance21;
            appearance22.BackColor = System.Drawing.SystemColors.Highlight;
            appearance22.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbFXConvOperator.DisplayLayout.Override.ActiveRowAppearance = appearance22;
            this.cmbFXConvOperator.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbFXConvOperator.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            this.cmbFXConvOperator.DisplayLayout.Override.CardAreaAppearance = appearance23;
            appearance24.BorderColor = System.Drawing.Color.Silver;
            appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbFXConvOperator.DisplayLayout.Override.CellAppearance = appearance24;
            this.cmbFXConvOperator.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbFXConvOperator.DisplayLayout.Override.CellPadding = 0;
            appearance25.BackColor = System.Drawing.SystemColors.Control;
            appearance25.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance25.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance25.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance25.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbFXConvOperator.DisplayLayout.Override.GroupByRowAppearance = appearance25;
            appearance26.TextHAlignAsString = "Left";
            this.cmbFXConvOperator.DisplayLayout.Override.HeaderAppearance = appearance26;
            this.cmbFXConvOperator.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbFXConvOperator.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance27.BackColor = System.Drawing.SystemColors.Window;
            appearance27.BorderColor = System.Drawing.Color.Silver;
            this.cmbFXConvOperator.DisplayLayout.Override.RowAppearance = appearance27;
            this.cmbFXConvOperator.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance28.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbFXConvOperator.DisplayLayout.Override.TemplateAddRowAppearance = appearance28;
            this.cmbFXConvOperator.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbFXConvOperator.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbFXConvOperator.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbFXConvOperator.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbFXConvOperator.DropDownWidth = 0;
            this.cmbFXConvOperator.Location = new System.Drawing.Point(122, 125);
            this.cmbFXConvOperator.Name = "cmbFXConvOperator";
            this.cmbFXConvOperator.Size = new System.Drawing.Size(79, 22);
            this.cmbFXConvOperator.TabIndex = 9;
            this.cmbFXConvOperator.ValueChanged += new System.EventHandler(this.cmbFXConvOperator_ValueChanged);
            this.cmbFXConvOperator.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
            this.cmbFXConvOperator.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
            this.cmbFXConvOperator.Leave += new System.EventHandler(this.cmbFXConvOperator_Leave);
            this.cmbFXConvOperator.MouseCaptureChanged += new System.EventHandler(this.cmb_MouseCaptureChanged);
            // 
            // cmbVenue
            // 
            this.cmbVenue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance29.BackColor = System.Drawing.SystemColors.Window;
            appearance29.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbVenue.DisplayLayout.Appearance = appearance29;
            ultraGridBand3.ColHeadersVisible = false;
            this.cmbVenue.DisplayLayout.BandsSerializer.Add(ultraGridBand3);
            this.cmbVenue.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbVenue.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance30.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance30.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance30.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance30.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.GroupByBox.Appearance = appearance30;
            appearance31.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.BandLabelAppearance = appearance31;
            this.cmbVenue.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance32.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance32.BackColor2 = System.Drawing.SystemColors.Control;
            appearance32.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance32.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbVenue.DisplayLayout.GroupByBox.PromptAppearance = appearance32;
            this.cmbVenue.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbVenue.DisplayLayout.MaxRowScrollRegions = 1;
            appearance33.BackColor = System.Drawing.SystemColors.Window;
            appearance33.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbVenue.DisplayLayout.Override.ActiveCellAppearance = appearance33;
            appearance34.BackColor = System.Drawing.SystemColors.Highlight;
            appearance34.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbVenue.DisplayLayout.Override.ActiveRowAppearance = appearance34;
            this.cmbVenue.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbVenue.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance35.BackColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.CardAreaAppearance = appearance35;
            appearance36.BorderColor = System.Drawing.Color.Silver;
            appearance36.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbVenue.DisplayLayout.Override.CellAppearance = appearance36;
            this.cmbVenue.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbVenue.DisplayLayout.Override.CellPadding = 0;
            appearance37.BackColor = System.Drawing.SystemColors.Control;
            appearance37.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance37.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance37.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance37.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbVenue.DisplayLayout.Override.GroupByRowAppearance = appearance37;
            appearance38.TextHAlignAsString = "Left";
            this.cmbVenue.DisplayLayout.Override.HeaderAppearance = appearance38;
            this.cmbVenue.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbVenue.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance39.BackColor = System.Drawing.SystemColors.Window;
            appearance39.BorderColor = System.Drawing.Color.Silver;
            this.cmbVenue.DisplayLayout.Override.RowAppearance = appearance39;
            this.cmbVenue.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance40.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbVenue.DisplayLayout.Override.TemplateAddRowAppearance = appearance40;
            this.cmbVenue.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbVenue.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbVenue.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbVenue.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbVenue.DropDownWidth = 0;
            this.cmbVenue.Location = new System.Drawing.Point(122, 26);
            this.cmbVenue.Name = "cmbVenue";
            this.cmbVenue.Size = new System.Drawing.Size(79, 22);
            this.cmbVenue.TabIndex = 3;
            this.cmbVenue.ValueChanged += new System.EventHandler(this.cmbVenue_ValueChanged);
            this.cmbVenue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
            this.cmbVenue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
            this.cmbVenue.Leave += new System.EventHandler(this.cmbVenue_Leave);
            this.cmbVenue.MouseCaptureChanged += new System.EventHandler(this.cmb_MouseCaptureChanged);
            // 
            // cmbCounterParty
            // 
            this.cmbCounterParty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance41.BackColor = System.Drawing.SystemColors.Window;
            appearance41.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbCounterParty.DisplayLayout.Appearance = appearance41;
            ultraGridBand4.ColHeadersVisible = false;
            this.cmbCounterParty.DisplayLayout.BandsSerializer.Add(ultraGridBand4);
            this.cmbCounterParty.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbCounterParty.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance42.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance42.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance42.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance42.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.GroupByBox.Appearance = appearance42;
            appearance43.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BandLabelAppearance = appearance43;
            this.cmbCounterParty.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance44.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance44.BackColor2 = System.Drawing.SystemColors.Control;
            appearance44.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance44.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbCounterParty.DisplayLayout.GroupByBox.PromptAppearance = appearance44;
            this.cmbCounterParty.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbCounterParty.DisplayLayout.MaxRowScrollRegions = 1;
            appearance45.BackColor = System.Drawing.SystemColors.Window;
            appearance45.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveCellAppearance = appearance45;
            appearance46.BackColor = System.Drawing.SystemColors.Highlight;
            appearance46.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbCounterParty.DisplayLayout.Override.ActiveRowAppearance = appearance46;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbCounterParty.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance47.BackColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.CardAreaAppearance = appearance47;
            appearance48.BorderColor = System.Drawing.Color.Silver;
            appearance48.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbCounterParty.DisplayLayout.Override.CellAppearance = appearance48;
            this.cmbCounterParty.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbCounterParty.DisplayLayout.Override.CellPadding = 0;
            appearance49.BackColor = System.Drawing.SystemColors.Control;
            appearance49.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance49.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance49.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance49.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbCounterParty.DisplayLayout.Override.GroupByRowAppearance = appearance49;
            appearance50.TextHAlignAsString = "Left";
            this.cmbCounterParty.DisplayLayout.Override.HeaderAppearance = appearance50;
            this.cmbCounterParty.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbCounterParty.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance51.BackColor = System.Drawing.SystemColors.Window;
            appearance51.BorderColor = System.Drawing.Color.Silver;
            this.cmbCounterParty.DisplayLayout.Override.RowAppearance = appearance51;
            this.cmbCounterParty.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance52.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbCounterParty.DisplayLayout.Override.TemplateAddRowAppearance = appearance52;
            this.cmbCounterParty.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbCounterParty.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbCounterParty.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbCounterParty.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbCounterParty.DropDownWidth = 0;
            this.cmbCounterParty.Location = new System.Drawing.Point(122, 1);
            this.cmbCounterParty.Name = "cmbCounterParty";
            this.cmbCounterParty.Size = new System.Drawing.Size(79, 22);
            this.cmbCounterParty.TabIndex = 1;
            this.cmbCounterParty.ValueChanged += new System.EventHandler(this.cmbCounterParty_ValueChanged);
            this.cmbCounterParty.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
            this.cmbCounterParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
            this.cmbCounterParty.Leave += new System.EventHandler(this.cmbCounterParty_Leave);
            this.cmbCounterParty.MouseCaptureChanged += new System.EventHandler(this.cmb_MouseCaptureChanged);
            // 
            // txtAccruedInterest
            // 
            this.txtAccruedInterest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccruedInterest.Location = new System.Drawing.Point(122, 221);
            this.txtAccruedInterest.Name = "txtAccruedInterest";
            this.txtAccruedInterest.Size = new System.Drawing.Size(79, 21);
            this.txtAccruedInterest.TabIndex = 11;
            this.txtAccruedInterest.ValueChanged += new System.EventHandler(this.txtAccruedInterest_ValueChanged);
            this.txtAccruedInterest.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtAccruedInterest.Leave += new System.EventHandler(this.txtAccruedInterest_Leave);
            // 
            // txtFxRate
            // 
            this.txtFxRate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFxRate.Location = new System.Drawing.Point(122, 102);
            this.txtFxRate.Name = "txtFxRate";
            this.txtFxRate.Size = new System.Drawing.Size(79, 21);
            this.txtFxRate.TabIndex = 7;
            this.txtFxRate.ValueChanged += new System.EventHandler(this.txtFxRate_ValueChanged);
            this.txtFxRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtFxRate.Leave += new System.EventHandler(this.txtFxRate_Leave);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(122, 51);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(79, 21);
            this.txtDescription.TabIndex = 5;
            this.txtDescription.ValueChanged += new System.EventHandler(this.txtDescription_ValueChanged);
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.ForeColor = System.Drawing.Color.Black;
            this.label39.Location = new System.Drawing.Point(6, 126);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(93, 13);
            this.label39.TabIndex = 8;
            this.label39.Text = "Fx Conv Operator:";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.BackColor = System.Drawing.Color.Transparent;
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.Location = new System.Drawing.Point(5, 222);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(88, 13);
            this.label40.TabIndex = 10;
            this.label40.Text = "Accrued Interest:";
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.Transparent;
            this.label41.ForeColor = System.Drawing.Color.Black;
            this.label41.Location = new System.Drawing.Point(3, 30);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(41, 13);
            this.label41.TabIndex = 2;
            this.label41.Text = "Venue:";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.BackColor = System.Drawing.Color.Transparent;
            this.label42.ForeColor = System.Drawing.Color.Black;
            this.label42.Location = new System.Drawing.Point(3, 54);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(63, 13);
            this.label42.TabIndex = 4;
            this.label42.Text = "Description:";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.BackColor = System.Drawing.Color.Transparent;
            this.label43.ForeColor = System.Drawing.Color.Black;
            this.label43.Location = new System.Drawing.Point(6, 102);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(47, 13);
            this.label43.TabIndex = 6;
            this.label43.Text = "Fx Rate:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.BackColor = System.Drawing.Color.Transparent;
            this.label44.ForeColor = System.Drawing.Color.Black;
            this.label44.Location = new System.Drawing.Point(3, 6);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(71, 13);
            this.label44.TabIndex = 0;
            this.label44.Text = ApplicationConstants.CONST_BROKER+":";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.ForeColor = System.Drawing.Color.Black;
            this.label33.Location = new System.Drawing.Point(6, 294);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(83, 13);
            this.label33.TabIndex = 2;
            this.label33.Text = "TradeAttribute6:";
            // 
            // ultraExpandableGrpboxCommissionAndFee
            // 
            appearance54.BackColor = System.Drawing.Color.AliceBlue;
            appearance54.BackColor2 = System.Drawing.Color.White;
            this.ultraExpandableGrpboxCommissionAndFee.Appearance = appearance54;
            this.ultraExpandableGrpboxCommissionAndFee.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGrpboxCommissionAndFee.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGrpboxCommissionAndFee.ExpandedSize = new System.Drawing.Size(223, 347);
            this.ultraExpandableGrpboxCommissionAndFee.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Far;
            appearance55.Cursor = System.Windows.Forms.Cursors.Hand;
            appearance55.FontData.Name = "Tahoma";
            appearance55.FontData.SizeInPoints = 10F;
            this.ultraExpandableGrpboxCommissionAndFee.HeaderAppearance = appearance55;
            this.ultraExpandableGrpboxCommissionAndFee.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.ToggleExpansion;
            this.ultraExpandableGrpboxCommissionAndFee.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.TopOnBorder;
            this.ultraExpandableGrpboxCommissionAndFee.Location = new System.Drawing.Point(0, 226);
            this.ultraExpandableGrpboxCommissionAndFee.Name = "ultraExpandableGrpboxCommissionAndFee";
            this.ultraExpandableGrpboxCommissionAndFee.Size = new System.Drawing.Size(223, 347);
            this.ultraExpandableGrpboxCommissionAndFee.TabIndex = 1;
            this.ultraExpandableGrpboxCommissionAndFee.Text = "&Commission and Fees";
            this.ultraExpandableGrpboxCommissionAndFee.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGrpboxCommissionAndFee.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGrpboxCommissionAndFee.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label18);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtSoftCommission);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label17);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtClearingBrokerFee);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label16);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label15);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label14);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.lblTotalComm);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label21);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtClearingFee);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtTaxOnCommissions);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtMiscFees);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtOtherBrokerFees);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtStampDuty);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtTransactionLevy);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtSecFee);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtOccFee);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtOrfFee);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtCommission);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label12);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label11);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label10);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label9);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label8);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label7);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label6);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 22);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(219, 323);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(6, 58);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(87, 13);
            this.label18.TabIndex = 25;
            this.label18.Text = "Soft Commission:";
            // 
            // txtSoftCommission
            // 
            this.txtSoftCommission.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSoftCommission.Location = new System.Drawing.Point(122, 55);
            this.txtSoftCommission.Name = "txtSoftCommission";
            this.txtSoftCommission.Size = new System.Drawing.Size(79, 21);
            this.txtSoftCommission.TabIndex = 24;
            this.txtSoftCommission.TextChanged += new System.EventHandler(this.txtSoftCommission_TextChanged);
            this.txtSoftCommission.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtSoftCommission.Leave += new System.EventHandler(this.txtSoftCommission_Leave);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(7, 106);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(108, 13);
            this.label17.TabIndex = 23;
            this.label17.Text = "Clearing Broker Fees:";
            // 
            // txtClearingBrokerFee
            // 
            this.txtClearingBrokerFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClearingBrokerFee.Location = new System.Drawing.Point(122, 103);
            this.txtClearingBrokerFee.Name = "txtClearingBrokerFee";
            this.txtClearingBrokerFee.Size = new System.Drawing.Size(79, 21);
            this.txtClearingBrokerFee.TabIndex = 22;
            this.txtClearingBrokerFee.TextChanged += new System.EventHandler(this.txtClearingBrokerFee_TextChanged);
            this.txtClearingBrokerFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtClearingBrokerFee.Leave += new System.EventHandler(this.txtClearingBrokerFee_Leave);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(7, 297);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 13);
            this.label16.TabIndex = 20;
            this.label16.Text = "ORF Fee:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(7, 273);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 13);
            this.label15.TabIndex = 18;
            this.label15.Text = "OCC Fee:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(7, 249);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 16;
            this.label14.Text = "SEC Fee:";
            // 
            // lblTotalComm
            // 
            this.lblTotalComm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalComm.Location = new System.Drawing.Point(122, 8);
            this.lblTotalComm.Name = "lblTotalComm";
            this.lblTotalComm.Size = new System.Drawing.Size(79, 18);
            this.lblTotalComm.TabIndex = 17;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(6, 10);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(109, 13);
            this.label21.TabIndex = 0;
            this.label21.Text = "Total Comm And Fee:";
            // 
            // txtClearingFee
            // 
            this.txtClearingFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClearingFee.Location = new System.Drawing.Point(122, 175);
            this.txtClearingFee.Name = "txtClearingFee";
            this.txtClearingFee.Size = new System.Drawing.Size(79, 21);
            this.txtClearingFee.TabIndex = 11;
            this.txtClearingFee.TextChanged += new System.EventHandler(this.txtClearingFee_TextChanged);
            this.txtClearingFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtClearingFee.Leave += new System.EventHandler(this.txtClearingFee_Leave);
            // 
            // txtTaxOnCommissions
            // 
            this.txtTaxOnCommissions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTaxOnCommissions.Location = new System.Drawing.Point(122, 199);
            this.txtTaxOnCommissions.Name = "txtTaxOnCommissions";
            this.txtTaxOnCommissions.Size = new System.Drawing.Size(79, 21);
            this.txtTaxOnCommissions.TabIndex = 13;
            this.txtTaxOnCommissions.TextChanged += new System.EventHandler(this.txtTaxOnCommissions_TextChanged);
            this.txtTaxOnCommissions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtTaxOnCommissions.Leave += new System.EventHandler(this.txtTaxOnCommissions_Leave);
            // 
            // txtMiscFees
            // 
            this.txtMiscFees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMiscFees.Location = new System.Drawing.Point(122, 223);
            this.txtMiscFees.Name = "txtMiscFees";
            this.txtMiscFees.Size = new System.Drawing.Size(79, 21);
            this.txtMiscFees.TabIndex = 15;
            this.txtMiscFees.TextChanged += new System.EventHandler(this.txtMiscFees_TextChanged);
            this.txtMiscFees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtMiscFees.Leave += new System.EventHandler(this.txtMiscFees_Leave);
            // 
            // txtOtherBrokerFees
            // 
            this.txtOtherBrokerFees.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOtherBrokerFees.Location = new System.Drawing.Point(122, 79);
            this.txtOtherBrokerFees.Name = "txtOtherBrokerFees";
            this.txtOtherBrokerFees.Size = new System.Drawing.Size(79, 21);
            this.txtOtherBrokerFees.TabIndex = 5;
            this.txtOtherBrokerFees.TextChanged += new System.EventHandler(this.txtOtherBrokerFees_TextChanged);
            this.txtOtherBrokerFees.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtOtherBrokerFees.Leave += new System.EventHandler(this.txtOtherBrokerFees_Leave);
            // 
            // txtStampDuty
            // 
            this.txtStampDuty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStampDuty.Location = new System.Drawing.Point(122, 127);
            this.txtStampDuty.Name = "txtStampDuty";
            this.txtStampDuty.Size = new System.Drawing.Size(79, 21);
            this.txtStampDuty.TabIndex = 7;
            this.txtStampDuty.TextChanged += new System.EventHandler(this.txtStampDuty_TextChanged);
            this.txtStampDuty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtStampDuty.Leave += new System.EventHandler(this.txtStampDuty_Leave);
            // 
            // txtTransactionLevy
            // 
            this.txtTransactionLevy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTransactionLevy.Location = new System.Drawing.Point(122, 151);
            this.txtTransactionLevy.Name = "txtTransactionLevy";
            this.txtTransactionLevy.Size = new System.Drawing.Size(79, 21);
            this.txtTransactionLevy.TabIndex = 9;
            this.txtTransactionLevy.TextChanged += new System.EventHandler(this.txtTransactionLevy_TextChanged);
            this.txtTransactionLevy.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtTransactionLevy.Leave += new System.EventHandler(this.txtTransactionLevy_Leave);
            // 
            // txtSecFee
            // 
            this.txtSecFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSecFee.Location = new System.Drawing.Point(122, 247);
            this.txtSecFee.Name = "txtSecFee";
            this.txtSecFee.Size = new System.Drawing.Size(79, 21);
            this.txtSecFee.TabIndex = 17;
            this.txtSecFee.TextChanged += new System.EventHandler(this.txtSecFee_TextChanged);
            this.txtSecFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtSecFee.Leave += new System.EventHandler(this.txtSecFee_Leave);
            // 
            // txtOccFee
            // 
            this.txtOccFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOccFee.Location = new System.Drawing.Point(122, 271);
            this.txtOccFee.Name = "txtOccFee";
            this.txtOccFee.Size = new System.Drawing.Size(79, 21);
            this.txtOccFee.TabIndex = 19;
            this.txtOccFee.TextChanged += new System.EventHandler(this.txtOccFee_TextChanged);
            this.txtOccFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtOccFee.Leave += new System.EventHandler(this.txtOccFee_Leave);
            // 
            // txtOrfFee
            // 
            this.txtOrfFee.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOrfFee.Location = new System.Drawing.Point(122, 295);
            this.txtOrfFee.Name = "txtOrfFee";
            this.txtOrfFee.Size = new System.Drawing.Size(79, 21);
            this.txtOrfFee.TabIndex = 21;
            this.txtOrfFee.TextChanged += new System.EventHandler(this.txtOrfFee_TextChanged);
            this.txtOrfFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtOrfFee.Leave += new System.EventHandler(this.txtOrfFee_Leave);
            // 
            // txtCommission
            // 
            this.txtCommission.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommission.Location = new System.Drawing.Point(122, 31);
            this.txtCommission.Name = "txtCommission";
            this.txtCommission.Size = new System.Drawing.Size(79, 21);
            this.txtCommission.TabIndex = 3;
            this.txtCommission.TextChanged += new System.EventHandler(this.txtCommission_TextChanged);
            this.txtCommission.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtCommission.Leave += new System.EventHandler(this.txtCommission_Leave);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(7, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(96, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Other Broker Fees:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(7, 129);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Stamp Duty:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(7, 153);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Transaction Levy:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(7, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "AUEC Fee1:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(7, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Tax on Comm:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(7, 225);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "AUEC Fee2:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(6, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Commission:";
            // 
            // ultraExpandableGrpboxBasicDetails
            // 
            appearance56.BackColor = System.Drawing.Color.AliceBlue;
            appearance56.BackColor2 = System.Drawing.Color.White;
            this.ultraExpandableGrpboxBasicDetails.Appearance = appearance56;
            this.ultraExpandableGrpboxBasicDetails.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGrpboxBasicDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGrpboxBasicDetails.ExpandedSize = new System.Drawing.Size(223, 226);
            this.ultraExpandableGrpboxBasicDetails.ExpansionIndicator = Infragistics.Win.Misc.GroupBoxExpansionIndicator.Far;
            appearance69.Cursor = System.Windows.Forms.Cursors.Hand;
            appearance69.FontData.Name = "Tahoma";
            appearance69.FontData.SizeInPoints = 10F;
            this.ultraExpandableGrpboxBasicDetails.HeaderAppearance = appearance69;
            this.ultraExpandableGrpboxBasicDetails.HeaderClickAction = Infragistics.Win.Misc.GroupBoxHeaderClickAction.ToggleExpansion;
            this.ultraExpandableGrpboxBasicDetails.HeaderPosition = Infragistics.Win.Misc.GroupBoxHeaderPosition.TopOnBorder;
            this.ultraExpandableGrpboxBasicDetails.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGrpboxBasicDetails.Name = "ultraExpandableGrpboxBasicDetails";
            this.ultraExpandableGrpboxBasicDetails.Size = new System.Drawing.Size(223, 226);
            this.ultraExpandableGrpboxBasicDetails.TabIndex = 0;
            this.ultraExpandableGrpboxBasicDetails.Text = "&Basic Details";
            this.ultraExpandableGrpboxBasicDetails.UseHotTracking = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGrpboxBasicDetails.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            this.ultraExpandableGrpboxBasicDetails.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label13);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.lblAsset);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.lblSymbol);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.cmbSide);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.dtpSettleDate);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label28);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.dtpOriginalPurchaseDate);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label27);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.dtpProcessDate);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label26);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.dtpTradeDate);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtAvgPrice);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtExecutedQty);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label5);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label4);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label3);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label2);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label1);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(2, 22);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(219, 202);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Location = new System.Drawing.Point(6, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Asset:";
            // 
            // lblAsset
            // 
            this.lblAsset.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAsset.Location = new System.Drawing.Point(122, 19);
            this.lblAsset.Name = "lblAsset";
            this.lblAsset.Size = new System.Drawing.Size(79, 15);
            this.lblAsset.TabIndex = 3;
            // 
            // lblSymbol
            // 
            this.lblSymbol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSymbol.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSymbol.Location = new System.Drawing.Point(122, 1);
            this.lblSymbol.Name = "lblSymbol";
            this.lblSymbol.Size = new System.Drawing.Size(79, 15);
            this.lblSymbol.TabIndex = 1;
            // 
            // cmbSide
            // 
            this.cmbSide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance57.BackColor = System.Drawing.SystemColors.Window;
            appearance57.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbSide.DisplayLayout.Appearance = appearance57;
            ultraGridBand5.ColHeadersVisible = false;
            this.cmbSide.DisplayLayout.BandsSerializer.Add(ultraGridBand5);
            this.cmbSide.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbSide.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance58.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance58.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance58.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance58.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.GroupByBox.Appearance = appearance58;
            appearance59.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.BandLabelAppearance = appearance59;
            this.cmbSide.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance60.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance60.BackColor2 = System.Drawing.SystemColors.Control;
            appearance60.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance60.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbSide.DisplayLayout.GroupByBox.PromptAppearance = appearance60;
            this.cmbSide.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbSide.DisplayLayout.MaxRowScrollRegions = 1;
            appearance61.BackColor = System.Drawing.SystemColors.Window;
            appearance61.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbSide.DisplayLayout.Override.ActiveCellAppearance = appearance61;
            appearance62.BackColor = System.Drawing.SystemColors.Highlight;
            appearance62.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbSide.DisplayLayout.Override.ActiveRowAppearance = appearance62;
            this.cmbSide.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbSide.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance63.BackColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.CardAreaAppearance = appearance63;
            appearance64.BorderColor = System.Drawing.Color.Silver;
            appearance64.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbSide.DisplayLayout.Override.CellAppearance = appearance64;
            this.cmbSide.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbSide.DisplayLayout.Override.CellPadding = 0;
            appearance65.BackColor = System.Drawing.SystemColors.Control;
            appearance65.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance65.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance65.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance65.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbSide.DisplayLayout.Override.GroupByRowAppearance = appearance65;
            appearance66.TextHAlignAsString = "Left";
            this.cmbSide.DisplayLayout.Override.HeaderAppearance = appearance66;
            this.cmbSide.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbSide.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance67.BackColor = System.Drawing.SystemColors.Window;
            appearance67.BorderColor = System.Drawing.Color.Silver;
            this.cmbSide.DisplayLayout.Override.RowAppearance = appearance67;
            this.cmbSide.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance68.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbSide.DisplayLayout.Override.TemplateAddRowAppearance = appearance68;
            this.cmbSide.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbSide.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbSide.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbSide.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbSide.DropDownWidth = 0;
            this.cmbSide.Location = new System.Drawing.Point(122, 82);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.Size = new System.Drawing.Size(79, 20);
            this.cmbSide.TabIndex = 9;
            this.cmbSide.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbSide.ValueChanged += new System.EventHandler(this.cmbSide_ValueChanged);
            this.cmbSide.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_KeyDown);
            this.cmbSide.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_KeyPress);
            this.cmbSide.Leave += new System.EventHandler(this.cmbSide_Leave);
            this.cmbSide.MouseCaptureChanged += new System.EventHandler(this.cmb_MouseCaptureChanged);
            // 
            // dtpSettleDate
            // 
            this.dtpSettleDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpSettleDate.Location = new System.Drawing.Point(122, 177);
            this.dtpSettleDate.Name = "dtpSettleDate";
            this.dtpSettleDate.Size = new System.Drawing.Size(79, 21);
            this.dtpSettleDate.TabIndex = 18;
            this.dtpSettleDate.AfterCloseUp += new System.EventHandler(this.dtpSettleDate_AfterCloseUp);
            this.dtpSettleDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
            this.dtpSettleDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
            this.dtpSettleDate.Leave += new System.EventHandler(this.dtpSettleDate_Leave);
            this.dtpSettleDate.MouseCaptureChanged += new System.EventHandler(this.dtp_MouseCaptureChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Location = new System.Drawing.Point(6, 179);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(86, 13);
            this.label28.TabIndex = 16;
            this.label28.Text = "Settlement Date:";
            // 
            // dtpOriginalPurchaseDate
            // 
            this.dtpOriginalPurchaseDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpOriginalPurchaseDate.Location = new System.Drawing.Point(122, 153);
            this.dtpOriginalPurchaseDate.Name = "dtpOriginalPurchaseDate";
            this.dtpOriginalPurchaseDate.Size = new System.Drawing.Size(79, 21);
            this.dtpOriginalPurchaseDate.TabIndex = 16;
            this.dtpOriginalPurchaseDate.AfterCloseUp += new System.EventHandler(this.dtpOriginalPurchaseDate_AfterCloseUp);
            this.dtpOriginalPurchaseDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
            this.dtpOriginalPurchaseDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
            this.dtpOriginalPurchaseDate.Leave += new System.EventHandler(this.dtpOriginalPurchaseDate_Leave);
            this.dtpOriginalPurchaseDate.MouseCaptureChanged += new System.EventHandler(this.dtp_MouseCaptureChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(6, 157);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(96, 13);
            this.label27.TabIndex = 14;
            this.label27.Text = "Original Purc Date:";
            // 
            // dtpProcessDate
            // 
            this.dtpProcessDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpProcessDate.Location = new System.Drawing.Point(122, 129);
            this.dtpProcessDate.Name = "dtpProcessDate";
            this.dtpProcessDate.Size = new System.Drawing.Size(79, 21);
            this.dtpProcessDate.TabIndex = 14;
            this.dtpProcessDate.AfterCloseUp += new System.EventHandler(this.dtpProcessDate_AfterCloseUp);
            this.dtpProcessDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
            this.dtpProcessDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
            this.dtpProcessDate.Leave += new System.EventHandler(this.dtpProcessDate_Leave);
            this.dtpProcessDate.MouseCaptureChanged += new System.EventHandler(this.dtp_MouseCaptureChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(6, 133);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(74, 13);
            this.label26.TabIndex = 12;
            this.label26.Text = "Process Date:";
            // 
            // dtpTradeDate
            // 
            this.dtpTradeDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpTradeDate.Location = new System.Drawing.Point(122, 105);
            this.dtpTradeDate.Name = "dtpTradeDate";
            this.dtpTradeDate.Size = new System.Drawing.Size(79, 21);
            this.dtpTradeDate.TabIndex = 11;
            this.dtpTradeDate.ValueChanged += new System.EventHandler(this.dtpTradeDate_ValueChanged);
            this.dtpTradeDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dtp_KeyDown);
            this.dtpTradeDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dtp_KeyPress);
            this.dtpTradeDate.Leave += new System.EventHandler(this.dtpTradeDate_Leave);
            this.dtpTradeDate.MouseCaptureChanged += new System.EventHandler(this.dtp_MouseCaptureChanged);
            // 
            // txtAvgPrice
            // 
            this.txtAvgPrice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAvgPrice.Location = new System.Drawing.Point(122, 59);
            this.txtAvgPrice.Name = "txtAvgPrice";
            this.txtAvgPrice.Size = new System.Drawing.Size(79, 21);
            this.txtAvgPrice.TabIndex = 7;
            this.txtAvgPrice.EnabledChanged += txtAvgPrice_EnabledChanged;
            this.txtAvgPrice.ValueChanged += new System.EventHandler(this.txtAvgPrice_ValueChanged);
            this.txtAvgPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtAvgPrice.Leave += new System.EventHandler(this.txtAvgPrice_Leave);
            // 
            // txtExecutedQty
            // 
            this.txtExecutedQty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecutedQty.Location = new System.Drawing.Point(122, 36);
            this.txtExecutedQty.Name = "txtExecutedQty";
            this.txtExecutedQty.Size = new System.Drawing.Size(79, 21);
            this.txtExecutedQty.TabIndex = 5;
            this.txtExecutedQty.ValueChanged += new System.EventHandler(this.txtExecutedQty_ValueChanged);
            this.txtExecutedQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtExecutedQty.Leave += new System.EventHandler(this.txtExecutedQty_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(6, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Avg Price:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(6, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Side:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(6, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Trade Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Executed Qty:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol:";
            // 
            // ultraExpandableGroupBoxPanel4
            // 
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox3);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox4);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox5);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox6);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.ultraDropDownButton1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.ultraDropDownButton2);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label22);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label23);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label24);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label25);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label35);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label36);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label37);
            this.ultraExpandableGroupBoxPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel4.Location = new System.Drawing.Point(2, 22);
            this.ultraExpandableGroupBoxPanel4.Name = "ultraExpandableGroupBoxPanel4";
            this.ultraExpandableGroupBoxPanel4.Size = new System.Drawing.Size(232, 172);
            this.ultraExpandableGroupBoxPanel4.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 123);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(112, 21);
            this.textBox1.TabIndex = 11;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(120, 99);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(112, 21);
            this.textBox3.TabIndex = 9;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(120, 75);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(112, 21);
            this.textBox4.TabIndex = 7;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(120, 147);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(112, 21);
            this.textBox5.TabIndex = 13;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(120, 51);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(112, 21);
            this.textBox6.TabIndex = 5;
            // 
            // ultraDropDownButton1
            // 
            appearance70.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            appearance70.BorderColor2 = System.Drawing.Color.White;
            appearance70.BorderColor3DBase = System.Drawing.Color.Transparent;
            appearance70.ForeColor = System.Drawing.Color.Black;
            this.ultraDropDownButton1.Appearance = appearance70;
            this.ultraDropDownButton1.Location = new System.Drawing.Point(120, 27);
            this.ultraDropDownButton1.Name = "ultraDropDownButton1";
            this.ultraDropDownButton1.Size = new System.Drawing.Size(112, 20);
            this.ultraDropDownButton1.TabIndex = 3;
            // 
            // ultraDropDownButton2
            // 
            appearance71.BorderColor = System.Drawing.Color.Gray;
            appearance71.BorderColor2 = System.Drawing.Color.White;
            appearance71.ForeColor = System.Drawing.Color.White;
            this.ultraDropDownButton2.Appearance = appearance71;
            this.ultraDropDownButton2.Location = new System.Drawing.Point(120, 3);
            this.ultraDropDownButton2.Name = "ultraDropDownButton2";
            this.ultraDropDownButton2.Size = new System.Drawing.Size(112, 20);
            this.ultraDropDownButton2.TabIndex = 1;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(6, 150);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(73, 13);
            this.label22.TabIndex = 12;
            this.label22.Text = "Proxy Symbol:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(6, 102);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(93, 13);
            this.label23.TabIndex = 8;
            this.label23.Text = "Fx Conv Operator:";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(6, 126);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(88, 13);
            this.label24.TabIndex = 10;
            this.label24.Text = "Accrued Interest:";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.Transparent;
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(6, 30);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 13);
            this.label25.TabIndex = 2;
            this.label25.Text = "Venue:";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.ForeColor = System.Drawing.Color.Black;
            this.label35.Location = new System.Drawing.Point(6, 54);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(63, 13);
            this.label35.TabIndex = 4;
            this.label35.Text = "Description:";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(6, 78);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(47, 13);
            this.label36.TabIndex = 6;
            this.label36.Text = "Fx Rate:";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.ForeColor = System.Drawing.Color.Black;
            this.label37.Location = new System.Drawing.Point(6, 6);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(71, 13);
            this.label37.TabIndex = 0;
            this.label37.Text = "CounterParty:";
            // 
            // statusProvider
            // 
            this.statusProvider.ContainerControl = this;
            // 
            // txtSettlCurrAmt
            // 
            this.txtSettlCurrAmt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSettlCurrAmt.Location = new System.Drawing.Point(122, 197);
            this.txtSettlCurrAmt.Name = "txtSettlCurrAmt";
            this.txtSettlCurrAmt.Size = new System.Drawing.Size(79, 21);
            this.txtSettlCurrAmt.TabIndex = 19;
            this.txtSettlCurrAmt.ValueChanged += new System.EventHandler(this.txtSettlCurrAmt_ValueChanged);
            this.txtSettlCurrAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNumeric_KeyPress);
            this.txtSettlCurrAmt.Leave += new System.EventHandler(this.txtSettlCurrAmt_Leave);
            this.txtSettlCurrAmt.EnabledChanged += txtSettlCurrAmt_EnabledChanged;
            // 
            // lblSettlFXRateAmt
            // 
            this.lblSettlFXRateAmt.AutoSize = true;
            this.lblSettlFXRateAmt.BackColor = System.Drawing.Color.Transparent;
            this.lblSettlFXRateAmt.ForeColor = System.Drawing.Color.Black;
            this.lblSettlFXRateAmt.Location = new System.Drawing.Point(6, 198);
            this.lblSettlFXRateAmt.Name = "lblSettlFXRateAmt";
            this.lblSettlFXRateAmt.Size = new System.Drawing.Size(99, 13);
            this.lblSettlFXRateAmt.TabIndex = 18;
            this.lblSettlFXRateAmt.Text = "Settlement Price";
            // 
            // ctrlAmendSingleGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ctrlAmendSingleGroup";
            this.Size = new System.Drawing.Size(240, 344);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBoxTradeAttributes)).EndInit();
            this.ultraExpandableGroupBoxTradeAttributes.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel6.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAttribute2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxOtherDetails)).EndInit();
            this.ultraExpandableGrpboxOtherDetails.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSettlFXConvOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettlCurrFxRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInternalComments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFXConvOperator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbVenue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCounterParty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAccruedInterest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtFxRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxCommissionAndFee)).EndInit();
            this.ultraExpandableGrpboxCommissionAndFee.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSoftCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClearingBrokerFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtClearingFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTaxOnCommissions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMiscFees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOtherBrokerFees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStampDuty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTransactionLevy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSecFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOccFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOrfFee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGrpboxBasicDetails)).EndInit();
            this.ultraExpandableGrpboxBasicDetails.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbSide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpSettleDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpOriginalPurchaseDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpProcessDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpTradeDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtAvgPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtExecutedQty)).EndInit();
            this.ultraExpandableGroupBoxPanel4.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSettlCurrAmt)).EndInit();
            this.ResumeLayout(false);

        }







        #endregion

        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGrpboxBasicDetails;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtExecutedQty;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAvgPrice;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGrpboxCommissionAndFee;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtClearingFee;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTaxOnCommissions;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtMiscFees;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOtherBrokerFees;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtStampDuty;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtTransactionLevy;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtCommission;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSecFee;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOccFee;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtOrfFee;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtpTradeDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtpSettleDate;
        private System.Windows.Forms.Label label28;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtpOriginalPurchaseDate;
        private System.Windows.Forms.Label label27;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dtpProcessDate;
        private System.Windows.Forms.Label label26;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGrpboxOtherDetails;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel3;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label21;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBoxTradeAttributes;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel6;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox3;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox5;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor textBox6;
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButton1;
        private Infragistics.Win.Misc.UltraDropDownButton ultraDropDownButton2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute6;
        private System.Windows.Forms.Label lblTradeAttribute6;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute5;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute3;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtAttribute2;
        private System.Windows.Forms.Label lblTradeAttribute1;
        private System.Windows.Forms.Label lblTradeAttribute2;
        private System.Windows.Forms.Label lblTradeAttribute3;
        private System.Windows.Forms.Label lblTradeAttribute5;
        private System.Windows.Forms.Label lblTradeAttribute4;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtAccruedInterest;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtFxRate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtDescription;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnApply;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbFXConvOperator;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbVenue;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbCounterParty;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSide;
        private Infragistics.Win.Misc.UltraLabel lblAsset;
        private System.Windows.Forms.Label label13;
        private Infragistics.Win.Misc.UltraLabel lblTotalComm;
        private System.Windows.Forms.ErrorProvider statusProvider;
        internal Infragistics.Win.Misc.UltraLabel lblSymbol;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label17;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtClearingBrokerFee;
        private System.Windows.Forms.Label label18;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSoftCommission;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtInternalComments;
        private System.Windows.Forms.Label label19;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbSettlFXConvOperator;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSettlCurrFxRate;
        private System.Windows.Forms.Label lblSettlFXRateOperator;
        private System.Windows.Forms.Label lblSettlFXRate;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtSettlCurrAmt;
        private System.Windows.Forms.Label lblSettlFXRateAmt;



    }
}
