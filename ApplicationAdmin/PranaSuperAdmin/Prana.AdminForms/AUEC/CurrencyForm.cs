using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin
{

    /// <summary>
    /// Add Delete and Save Currencies in the database
    /// </summary>
    public class CurrencyForm : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "CurrencyForm: ";
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdCurrency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrency;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSymbol;
        private System.Windows.Forms.Button btnReset;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel CurrencyForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CurrencyForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CurrencyForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CurrencyForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _CurrencyForm_UltraFormManager_Dock_Area_Bottom;
        private IContainer components;

        public CurrencyForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (grdCurrency != null)
                {
                    grdCurrency.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (txtCurrency != null)
                {
                    txtCurrency.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (txtSymbol != null)
                {
                    txtSymbol.Dispose();
                }
                if (btnReset != null)
                {
                    btnReset.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (CurrencyForm_Fill_Panel != null)
                {
                    CurrencyForm_Fill_Panel.Dispose();
                }
                if (_CurrencyForm_UltraFormManager_Dock_Area_Left != null)
                {
                    _CurrencyForm_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_CurrencyForm_UltraFormManager_Dock_Area_Right != null)
                {
                    _CurrencyForm_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_CurrencyForm_UltraFormManager_Dock_Area_Top != null)
                {
                    _CurrencyForm_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_CurrencyForm_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _CurrencyForm_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance("CurrencyAdd");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 0);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 1);
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 3);
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 4);
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CurrencyForm));
            this.grdCurrency = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurrency = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSymbol = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnReset = new System.Windows.Forms.Button();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.CurrencyForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._CurrencyForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CurrencyForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CurrencyForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrency)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.CurrencyForm_Fill_Panel.ClientArea.SuspendLayout();
            this.CurrencyForm_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // grdCurrency
            // 
            this.grdCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.None;
            this.grdCurrency.DisplayLayout.AddNewBox.ButtonAppearance = appearance1;
            this.grdCurrency.DisplayLayout.AddNewBox.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.grdCurrency.DisplayLayout.AddNewBox.Prompt = "Add new Row";
            appearance2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdCurrency.DisplayLayout.Appearance = appearance2;
            this.grdCurrency.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            ultraGridColumn1.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn1.CellAppearance = appearance3;
            ultraGridColumn1.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
            appearance4.BackColor = System.Drawing.SystemColors.Control;
            appearance4.BackColor2 = System.Drawing.SystemColors.ActiveBorder;
            appearance4.FontData.BoldAsString = "True";
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn1.Header.Appearance = appearance4;
            ultraGridColumn1.Header.VisiblePosition = 2;
            ultraGridColumn1.MinWidth = 100;
            ultraGridColumn1.ProportionalResize = true;
            ultraGridColumn1.Width = 157;
            ultraGridColumn2.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance5.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance5;
            ultraGridColumn2.GroupByMode = Infragistics.Win.UltraWinGrid.GroupByMode.Value;
            appearance6.BackColor = System.Drawing.SystemColors.Control;
            appearance6.FontData.BoldAsString = "True";
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance6;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn2.MaxWidth = 10;
            ultraGridColumn2.MinWidth = 5;
            ultraGridColumn2.Width = 10;
            ultraGridColumn3.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            ultraGridColumn3.AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False;
            appearance7.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance7;
            ultraGridColumn3.CellDisplayStyle = Infragistics.Win.UltraWinGrid.CellDisplayStyle.PlainText;
            appearance8.BackColor = System.Drawing.SystemColors.Control;
            appearance8.BackColor2 = System.Drawing.SystemColors.ActiveBorder;
            appearance8.FontData.BoldAsString = "True";
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance8;
            ultraGridColumn3.Header.VisiblePosition = 0;
            ultraGridColumn3.MinWidth = 100;
            ultraGridColumn3.ProportionalResize = true;
            ultraGridColumn3.Width = 137;
            appearance9.TextHAlignAsString = "Center";
            ultraGridColumn4.CellAppearance = appearance9;
            appearance10.FontData.BoldAsString = "True";
            ultraGridColumn4.Header.Appearance = appearance10;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            appearance11.TextHAlignAsString = "Center";
            ultraGridColumn5.CellAppearance = appearance11;
            appearance12.FontData.BoldAsString = "True";
            ultraGridColumn5.Header.Appearance = appearance12;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            appearance13.BackColor = System.Drawing.SystemColors.Control;
            appearance13.ForeColor = System.Drawing.SystemColors.Control;
            ultraGridBand1.Header.Appearance = appearance13;
            appearance14.BackColor = System.Drawing.Color.LemonChiffon;
            ultraGridBand1.Override.ActiveCellAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.LemonChiffon;
            ultraGridBand1.Override.ActiveRowAppearance = appearance15;
            ultraGridBand1.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Free;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdCurrency.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdCurrency.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdCurrency.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            this.grdCurrency.DisplayLayout.GroupByBox.Hidden = true;
            this.grdCurrency.DisplayLayout.MaxColScrollRegions = 1;
            this.grdCurrency.DisplayLayout.MaxRowScrollRegions = 1;
            appearance16.BackColor = System.Drawing.SystemColors.Highlight;
            appearance16.ForeColor = System.Drawing.Color.Black;
            this.grdCurrency.DisplayLayout.Override.ActiveCellAppearance = appearance16;
            appearance17.BackColor = System.Drawing.SystemColors.Highlight;
            appearance17.ForeColor = System.Drawing.Color.Black;
            this.grdCurrency.DisplayLayout.Override.ActiveRowAppearance = appearance17;
            this.grdCurrency.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdCurrency.DisplayLayout.Override.CellPadding = 0;
            appearance18.TextHAlignAsString = "Left";
            this.grdCurrency.DisplayLayout.Override.HeaderAppearance = appearance18;
            this.grdCurrency.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdCurrency.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdCurrency.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdCurrency.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdCurrency.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdCurrency.Location = new System.Drawing.Point(6, 2);
            this.grdCurrency.Name = "grdCurrency";
            this.grdCurrency.RowUpdateCancelAction = Infragistics.Win.UltraWinGrid.RowUpdateCancelAction.RetainDataAndActivation;
            this.grdCurrency.Size = new System.Drawing.Size(332, 90);
            this.grdCurrency.TabIndex = 1;
            this.grdCurrency.TabStop = false;
            this.grdCurrency.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            //this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(209, 200);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnClose, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnClose.TabIndex = 5;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            //this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Location = new System.Drawing.Point(53, 200);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnSave, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            //this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.Text = "Edit";
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Location = new System.Drawing.Point(94, 96);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(76, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnEdit, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnEdit.TabIndex = 4;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            //this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.Text = "Delete";
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Location = new System.Drawing.Point(174, 96);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(76, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnDelete, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnDelete.TabIndex = 5;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 18);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 6;
            this.label1.Text = "Currency Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtCurrency
            // 
            this.txtCurrency.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCurrency.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtCurrency.Location = new System.Drawing.Point(136, 46);
            this.txtCurrency.MaxLength = 50;
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.Size = new System.Drawing.Size(134, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtCurrency, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtCurrency.TabIndex = 2;
            this.txtCurrency.GotFocus += new System.EventHandler(this.txtCurrency_GotFocus);
            this.txtCurrency.LostFocus += new System.EventHandler(this.txtCurrency_LostFocus);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSymbol);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCurrency);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(28, 122);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(288, 74);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Edit Currency";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(12, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 18);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 15;
            this.label2.Text = "Currency Symbol";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSymbol
            // 
            this.txtSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtSymbol.Location = new System.Drawing.Point(136, 22);
            this.txtSymbol.MaxLength = 4;
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(134, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtSymbol, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtSymbol.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(118, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 10);
            this.inboxControlStyler1.SetStyleSettings(this.label4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label4.TabIndex = 16;
            this.label4.Text = "*";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            //this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.Text = "Reset";
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Location = new System.Drawing.Point(131, 200);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnReset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnReset.TabIndex = 4;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // CurrencyForm_Fill_Panel
            // 
            // 
            // CurrencyForm_Fill_Panel.ClientArea
            // 
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.btnReset);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.groupBox1);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.btnDelete);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.btnEdit);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.btnClose);
            this.CurrencyForm_Fill_Panel.ClientArea.Controls.Add(this.grdCurrency);
            this.CurrencyForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.CurrencyForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CurrencyForm_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.CurrencyForm_Fill_Panel.Name = "CurrencyForm_Fill_Panel";
            this.CurrencyForm_Fill_Panel.Size = new System.Drawing.Size(344, 251);
            this.CurrencyForm_Fill_Panel.TabIndex = 0;
            // 
            // _CurrencyForm_UltraFormManager_Dock_Area_Left
            // 
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.Name = "_CurrencyForm_UltraFormManager_Dock_Area_Left";
            this._CurrencyForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 251);
            // 
            // _CurrencyForm_UltraFormManager_Dock_Area_Right
            // 
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(348, 27);
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.Name = "_CurrencyForm_UltraFormManager_Dock_Area_Right";
            this._CurrencyForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 251);
            // 
            // _CurrencyForm_UltraFormManager_Dock_Area_Top
            // 
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.Name = "_CurrencyForm_UltraFormManager_Dock_Area_Top";
            this._CurrencyForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(352, 27);
            // 
            // _CurrencyForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 278);
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.Name = "_CurrencyForm_UltraFormManager_Dock_Area_Bottom";
            this._CurrencyForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(352, 4);
            // 
            // CurrencyForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(352, 282);
            this.Controls.Add(this.CurrencyForm_Fill_Panel);
            this.Controls.Add(this._CurrencyForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._CurrencyForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._CurrencyForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._CurrencyForm_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "CurrencyForm";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Currency";
            this.Load += new System.EventHandler(this.CurrencyForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdCurrency)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.CurrencyForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.CurrencyForm_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// This method binds the existing <see cref="Currencies"/> in the ComboBox control by assigning the 
        /// currencies object to its datasource property.
        /// </summary>
        private void BindCurrencyGrid()
        {
            // This method fetches the existing currencies from the database.
            Prana.Admin.BLL.Currencies currencies = AUECManager.GetCurrencies();

            if (currencies.Count <= 0)
            {
                currencies.Add(new Currency(int.MinValue, "", ""));
            }
            this.grdCurrency.DataSource = currencies;
        }

        /// <summary>
        /// This method closes the form when clicked the close button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            //			Common.ResetStatusPanel(statusBar1);

            this.Close();
        }

        /// <summary>
        /// Various BindCurrencyGrid method is called on the on Load event of the CurrencyForm form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrencyForm_Load(object sender, System.EventArgs e)
        {
            BindCurrencyGrid();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        /// <summary>
        /// This method saves the currency in the database. Also it applies validation on the required
        /// field by checking for the null values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Currencies currencies = ((Currencies)grdCurrency.DataSource);
                errorProvider1.SetError(txtSymbol, "");
                if ((txtSymbol.Text.Trim() == ""))
                {
                    errorProvider1.SetError(txtSymbol, "Provide Symbol!");
                    txtSymbol.Focus();
                    //					Common.ResetStatusPanel(statusBar1);
                    //					Common.SetStatusPanel(statusBar1, "Enter Currency Symbol");
                }
                else
                {
                    bool flag = false;
                    if (_checkEdit == false)
                    {
                        foreach (Currency currency in currencies)
                        {
                            if (currency.CurrencySymbol.ToString().Trim().ToUpper() == txtSymbol.Text.ToString().Trim().ToUpper())
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag == false)
                    {
                        Currency currency1 = new Currency(_currencyID, txtCurrency.Text, txtSymbol.Text);
                        int result = AUECManager.SaveCurrency(currency1);
                        if (result < 0)
                        {
                            MessageBox.Show("Currency Symbol already exists in the database");
                        }
                        else
                        {
                            AUECManager.SaveSMCurrencyDetails(currency1, result);
                            MessageBox.Show("Currency saved !");
                        }
                        BindCurrencyGrid();
                        RefreshForm();
                    }
                    else
                    {
                        //						Common.ResetStatusPanel(statusBar1);
                        //						Common.SetStatusPanel(statusBar1, "Currency already exists");
                        //Currency already exists in the database ... Error MessageBox
                        MessageBox.Show("Currency Symbol already exists in the database");
                    }
                    _checkEdit = false;
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnSave_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);


                #endregion
            }
        }
        /// <summary>
        /// This method blanks the data in textboxes if any.
        /// </summary>
        private void RefreshForm()
        {
            txtCurrency.Text = "";
            txtSymbol.Text = "";
            _currencyID = int.MinValue;
        }


        public int _currencyID = int.MinValue;
        public bool _checkEdit = false;
        private void btnEdit_Click(object sender, System.EventArgs e)
        {   //Edit Currencies in the Database and Save them
            if (grdCurrency.Rows.Count > 0)
            {
                txtSymbol.Text = grdCurrency.ActiveRow.Cells["CurrencySymbol"].Value.ToString();
                txtCurrency.Text = grdCurrency.ActiveRow.Cells["CurrencyName"].Value.ToString();
                _currencyID = int.Parse(grdCurrency.ActiveRow.Cells["CurencyID"].Value.ToString());
                _checkEdit = true;
            }
        }


        /// <summary>
        /// This method deletes the selected currency from the grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            //Delete Currency
            try
            {
                if (grdCurrency.Rows.Count > 0)
                {
                    string currencySymbol = grdCurrency.ActiveRow.Cells["CurrencySymbol"].Text.ToString();
                    if (currencySymbol != "")
                    {
                        if (MessageBox.Show(this, "Do you want to delete this Currency?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int id = int.Parse(grdCurrency.ActiveRow.Cells["CurencyId"].Value.ToString());
                            if (!(AUECManager.DeleteCurrency(id)))
                            {
                                MessageBox.Show("This Currency is referred in AUEC/CounterpartyVenue/Client. Please remove references first to delete it.", "Currency Delete");
                            }
                            else
                            {
                                AUECManager.DeleteSM_AECCS(id, 2);
                                BindCurrencyGrid();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Data Available!");
                }
            }
            #region Catch
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            #endregion
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("btnDelete_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnDelete_Click", null);


                #endregion
            }
        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        #region Focus Colors
        private void txtCurrency_GotFocus(object sender, System.EventArgs e)
        {
            txtCurrency.BackColor = Color.LemonChiffon;
        }
        private void txtCurrency_LostFocus(object sender, System.EventArgs e)
        {
            txtCurrency.BackColor = Color.White;
        }

        private void txtSymbol_GotFocus(object sender, System.EventArgs e)
        {
            txtSymbol.BackColor = Color.LemonChiffon;
        }
        private void txtSymbol_LostFocus(object sender, System.EventArgs e)
        {
            txtSymbol.BackColor = Color.White;
        }
        #endregion

        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtCurrency.Text = "";
            txtSymbol.Text = "";
            _currencyID = int.MinValue;
        }

        private void statusBar1_PanelClick(object sender, System.Windows.Forms.StatusBarPanelClickEventArgs e)
        {

        }

    }
}
