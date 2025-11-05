#region Using
using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion

namespace Prana.Admin
{
    /// <summary>
    /// Summary description for Underlying.
    /// </summary>
    public class UnderlyingDetails : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "UnderlyingDetails : ";
        #region Private variable

        private System.Windows.Forms.TextBox txtUnderlying;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnDelete;

        #endregion
        private IContainer components;

        private const int GRD_UNDERLYING_ID = 0;
        private const int GRD_ASSET = 1;
        private const int GRD_UNDERLYING_NAME = 2;
        private const int GRD_COMMENT = 3;
        private const int GRD_ASSET_ID = 4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox grpUnderlying;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdUnderLying;
        private System.Windows.Forms.Label lblcomment;
        private System.Windows.Forms.Label lblname;
        private System.Windows.Forms.Label lblasset;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbAsset;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel UnderlyingDetails_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UnderlyingDetails_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UnderlyingDetails_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UnderlyingDetails_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _UnderlyingDetails_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ErrorProvider errorProvider1;

        public UnderlyingDetails()
        {
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
                if (txtUnderlying != null)
                {
                    txtUnderlying.Dispose();
                }
                if (txtComment != null)
                {
                    txtComment.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (label5 != null)
                {
                    label5.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnReset != null)
                {
                    btnReset.Dispose();
                }
                if (grpUnderlying != null)
                {
                    grpUnderlying.Dispose();
                }
                if (grdUnderLying != null)
                {
                    grdUnderLying.Dispose();
                }
                if (lblcomment != null)
                {
                    lblcomment.Dispose();
                }
                if (lblname != null)
                {
                    lblname.Dispose();
                }
                if (lblasset != null)
                {
                    lblasset.Dispose();
                }
                if (cmbAsset != null)
                {
                    cmbAsset.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (UnderlyingDetails_Fill_Panel != null)
                {
                    UnderlyingDetails_Fill_Panel.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_UnderlyingDetails_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_UnderlyingDetails_UltraFormManager_Dock_Area_Left != null)
                {
                    _UnderlyingDetails_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_UnderlyingDetails_UltraFormManager_Dock_Area_Right != null)
                {
                    _UnderlyingDetails_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_UnderlyingDetails_UltraFormManager_Dock_Area_Top != null)
                {
                    _UnderlyingDetails_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssetID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnderlyingDetails));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnderlyingID", 0);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Asset", 1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Underlying", 2);
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 3);
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment", 4);
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssetID", 5);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn11 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 6);
            this.grpUnderlying = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.lblcomment = new System.Windows.Forms.Label();
            this.txtUnderlying = new System.Windows.Forms.TextBox();
            this.lblname = new System.Windows.Forms.Label();
            this.lblasset = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbAsset = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grdUnderLying = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.UnderlyingDetails_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.grpUnderlying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnderLying)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.UnderlyingDetails_Fill_Panel.ClientArea.SuspendLayout();
            this.UnderlyingDetails_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpUnderlying
            // 
            this.grpUnderlying.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.grpUnderlying.Controls.Add(this.label4);
            this.grpUnderlying.Controls.Add(this.txtComment);
            this.grpUnderlying.Controls.Add(this.lblcomment);
            this.grpUnderlying.Controls.Add(this.txtUnderlying);
            this.grpUnderlying.Controls.Add(this.lblname);
            this.grpUnderlying.Controls.Add(this.lblasset);
            this.grpUnderlying.Controls.Add(this.label5);
            this.grpUnderlying.Controls.Add(this.cmbAsset);
            this.grpUnderlying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpUnderlying.Location = new System.Drawing.Point(26, 141);
            this.grpUnderlying.Name = "grpUnderlying";
            this.grpUnderlying.Size = new System.Drawing.Size(300, 88);
            this.inboxControlStyler1.SetStyleSettings(this.grpUnderlying, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.grpUnderlying.TabIndex = 1;
            this.grpUnderlying.TabStop = false;
            this.grpUnderlying.Text = "Add/Update Underlying";
            this.grpUnderlying.Visible = false;
            this.grpUnderlying.Enter += new System.EventHandler(this.grpUnderlying_Enter);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(58, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label4.TabIndex = 8;
            this.label4.Text = "*";
            // 
            // txtComment
            // 
            this.txtComment.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComment.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtComment.Location = new System.Drawing.Point(104, 62);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(178, 23);
            this.inboxControlStyler1.SetStyleSettings(this.txtComment, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtComment.TabIndex = 3;
            this.txtComment.TextChanged += new System.EventHandler(this.txtComment_TextChanged);
            this.txtComment.GotFocus += new System.EventHandler(this.txtComment_GotFocus);
            this.txtComment.LostFocus += new System.EventHandler(this.txtComment_LostFocus);
            // 
            // lblcomment
            // 
            this.lblcomment.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcomment.Location = new System.Drawing.Point(16, 65);
            this.lblcomment.Name = "lblcomment";
            this.lblcomment.Size = new System.Drawing.Size(54, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblcomment, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblcomment.TabIndex = 4;
            this.lblcomment.Text = "Comment";
            this.lblcomment.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtUnderlying
            // 
            this.txtUnderlying.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUnderlying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUnderlying.Location = new System.Drawing.Point(104, 40);
            this.txtUnderlying.MaxLength = 50;
            this.txtUnderlying.Name = "txtUnderlying";
            this.txtUnderlying.Size = new System.Drawing.Size(178, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtUnderlying, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtUnderlying.TabIndex = 2;
            this.txtUnderlying.TextChanged += new System.EventHandler(this.txtUnderlying_TextChanged);
            this.txtUnderlying.GotFocus += new System.EventHandler(this.txtUnderlying_GotFocus);
            this.txtUnderlying.LostFocus += new System.EventHandler(this.txtUnderlying_LostFocus);
            // 
            // lblname
            // 
            this.lblname.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblname.Location = new System.Drawing.Point(16, 42);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(62, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblname, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblname.TabIndex = 2;
            this.lblname.Text = "UnderLying";
            this.lblname.Click += new System.EventHandler(this.label2_Click);
            // 
            // lblasset
            // 
            this.lblasset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblasset.Location = new System.Drawing.Point(16, 20);
            this.lblasset.Name = "lblasset";
            this.lblasset.Size = new System.Drawing.Size(42, 16);
            this.inboxControlStyler1.SetStyleSettings(this.lblasset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.lblasset.TabIndex = 1;
            this.lblasset.Text = " Asset";
            this.lblasset.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(78, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 6);
            this.inboxControlStyler1.SetStyleSettings(this.label5, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label5.TabIndex = 8;
            this.label5.Text = "*";
            // 
            // cmbAsset
            // 
            this.cmbAsset.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            this.cmbAsset.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbAsset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbAsset.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbAsset.DropDownWidth = 0;
            this.cmbAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAsset.Location = new System.Drawing.Point(104, 18);
            this.cmbAsset.Name = "cmbAsset";
            this.cmbAsset.Size = new System.Drawing.Size(178, 21);
            this.cmbAsset.TabIndex = 1;
            this.cmbAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbAsset.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.cmbAsset_InitializeLayout);
            this.cmbAsset.GotFocus += new System.EventHandler(this.cmbAsset_GotFocus);
            this.cmbAsset.LostFocus += new System.EventHandler(this.cmbAsset_LostFocus);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            //this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(64, 231);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnSave, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnSave.TabIndex = 4;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            //this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(146, 253);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnClose, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnClose.TabIndex = 6;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            //this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.Text = "Delete";
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(181, 115);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnDelete, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnDelete.TabIndex = 8;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            //this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.Text = "Edit";
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(103, 115);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnEdit, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnEdit.TabIndex = 7;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            //this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.Text = "Reset";
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Location = new System.Drawing.Point(233, 232);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnReset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnReset.TabIndex = 5;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // grdUnderLying
            // 
            this.grdUnderLying.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.grdUnderLying.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            appearance1.FontData.BoldAsString = "True";
            ultraGridColumn5.Header.Appearance = appearance1;
            ultraGridColumn5.Header.VisiblePosition = 0;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn5.Width = 363;
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn6.CellAppearance = appearance2;
            appearance3.FontData.BoldAsString = "True";
            ultraGridColumn6.Header.Appearance = appearance3;
            ultraGridColumn6.Header.VisiblePosition = 1;
            ultraGridColumn6.Width = 80;
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn7.CellAppearance = appearance4;
            appearance5.FontData.BoldAsString = "True";
            ultraGridColumn7.Header.Appearance = appearance5;
            ultraGridColumn7.Header.Caption = "UnderLying";
            ultraGridColumn7.Header.VisiblePosition = 2;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn7.RowLayoutColumnInfo.AllowCellSizing = Infragistics.Win.UltraWinGrid.RowLayoutSizing.Vertical;
            ultraGridColumn7.Width = 108;
            appearance6.TextHAlignAsString = "Center";
            ultraGridColumn8.CellAppearance = appearance6;
            appearance7.FontData.BoldAsString = "True";
            appearance7.TextHAlignAsString = "Center";
            ultraGridColumn8.Header.Appearance = appearance7;
            ultraGridColumn8.Header.Caption = "UnderLying";
            ultraGridColumn8.Header.VisiblePosition = 3;
            ultraGridColumn8.Width = 141;
            appearance8.TextHAlignAsString = "Center";
            ultraGridColumn9.CellAppearance = appearance8;
            appearance9.FontData.BoldAsString = "True";
            appearance9.TextHAlignAsString = "Center";
            ultraGridColumn9.Header.Appearance = appearance9;
            ultraGridColumn9.Header.VisiblePosition = 4;
            ultraGridColumn9.Width = 112;
            ultraGridColumn10.Header.VisiblePosition = 5;
            ultraGridColumn10.Hidden = true;
            ultraGridColumn10.Width = 72;
            ultraGridColumn11.Header.VisiblePosition = 6;
            ultraGridColumn11.Hidden = true;
            ultraGridColumn11.Width = 73;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn5,
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10,
            ultraGridColumn11});
            ultraGridBand2.Header.Enabled = false;
            ultraGridBand2.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdUnderLying.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.grdUnderLying.DisplayLayout.GroupByBox.Hidden = true;
            this.grdUnderLying.DisplayLayout.MaxColScrollRegions = 1;
            this.grdUnderLying.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdUnderLying.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdUnderLying.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdUnderLying.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdUnderLying.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdUnderLying.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdUnderLying.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdUnderLying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdUnderLying.Location = new System.Drawing.Point(3, 2);
            this.grdUnderLying.Name = "grdUnderLying";
            this.grdUnderLying.Size = new System.Drawing.Size(354, 245);
            this.grdUnderLying.TabIndex = 53;
            this.grdUnderLying.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.grdUnderLying.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdUnderLying_InitializeLayout);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // UnderlyingDetails_Fill_Panel
            // 
            // 
            // UnderlyingDetails_Fill_Panel.ClientArea
            // 
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.grdUnderLying);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.btnReset);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.btnEdit);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.btnDelete);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.btnClose);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.UnderlyingDetails_Fill_Panel.ClientArea.Controls.Add(this.grpUnderlying);
            this.UnderlyingDetails_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.UnderlyingDetails_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnderlyingDetails_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.UnderlyingDetails_Fill_Panel.Name = "UnderlyingDetails_Fill_Panel";
            this.UnderlyingDetails_Fill_Panel.Size = new System.Drawing.Size(359, 280);
            this.UnderlyingDetails_Fill_Panel.TabIndex = 0;
            // 
            // _UnderlyingDetails_UltraFormManager_Dock_Area_Left
            // 
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.Name = "_UnderlyingDetails_UltraFormManager_Dock_Area_Left";
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 280);
            // 
            // _UnderlyingDetails_UltraFormManager_Dock_Area_Right
            // 
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(363, 27);
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.Name = "_UnderlyingDetails_UltraFormManager_Dock_Area_Right";
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 280);
            // 
            // _UnderlyingDetails_UltraFormManager_Dock_Area_Top
            // 
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.Name = "_UnderlyingDetails_UltraFormManager_Dock_Area_Top";
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(367, 27);
            // 
            // _UnderlyingDetails_UltraFormManager_Dock_Area_Bottom
            // 
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 307);
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.Name = "_UnderlyingDetails_UltraFormManager_Dock_Area_Bottom";
            this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(367, 4);
            // 
            // UnderlyingDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(367, 311);
            this.Controls.Add(this.UnderlyingDetails_Fill_Panel);
            this.Controls.Add(this._UnderlyingDetails_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._UnderlyingDetails_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._UnderlyingDetails_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._UnderlyingDetails_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(367, 311);
            this.Name = "UnderlyingDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Underlying";
            this.Load += new System.EventHandler(this.UnderlyingDetails_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UnderlyingDetails_Paint);
            this.grpUnderlying.ResumeLayout(false);
            this.grpUnderlying.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdUnderLying)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.UnderlyingDetails_Fill_Panel.ClientArea.ResumeLayout(false);
            this.UnderlyingDetails_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtComment_GotFocus(object sender, System.EventArgs e)
        {
            txtComment.BackColor = Color.LemonChiffon;
        }
        private void txtComment_LostFocus(object sender, System.EventArgs e)
        {
            txtComment.BackColor = Color.White;
        }
        private void txtUnderlying_GotFocus(object sender, System.EventArgs e)
        {
            txtUnderlying.BackColor = Color.LemonChiffon;
        }
        private void txtUnderlying_LostFocus(object sender, System.EventArgs e)
        {
            txtUnderlying.BackColor = Color.White;
        }
        private void cmbAsset_GotFocus(object sender, System.EventArgs e)
        {
            cmbAsset.Appearance.BackColor = Color.LemonChiffon;
        }
        private void cmbAsset_LostFocus(object sender, System.EventArgs e)
        {
            cmbAsset.Appearance.BackColor = Color.White;
        }

        #endregion


        /// <summary>
        /// This method saves the UnderLying. Also it applies validation on the 
        /// required fields by checking for the unselected values and some other validation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private int SaveUnderlying()
        {
            int result = int.MinValue;
            if (int.Parse(cmbAsset.Value.ToString()) == int.MinValue)
            {
                errorProvider1.SetError(cmbAsset, "Provide Value!");
                cmbAsset.Focus();
            }
            else if (txtUnderlying.Text.Trim() == "")
            {
                errorProvider1.SetError(txtUnderlying, "Provide Value!");
                txtUnderlying.Focus();
                errorProvider1.SetError(cmbAsset, "");
            }
            else
            {
                UnderLying underLying = new UnderLying();

                if (txtUnderlying.Tag != null)
                {
                    //Update
                    underLying.UnderlyingID = int.Parse(txtUnderlying.Tag.ToString());
                }
                underLying.AssetID = int.Parse(cmbAsset.Value.ToString());
                underLying.Name = txtUnderlying.Text.Trim();
                underLying.Comment = txtComment.Text.Trim();

                //SaveUnderLying method saves the underLying in the database.
                int newUnderLyingID = AssetManager.SaveUnderLying(underLying);
                if (newUnderLyingID == -1)
                {
                    //					Common.ResetStatusPanel(stbUnderLying);
                    //					Common.SetStatusPanel(stbUnderLying, "UnderLying already exists.");
                    MessageBox.Show("Underlying under the selected asset already exists.", "Prana Alert", MessageBoxButtons.OK);
                }
                else
                {
                    //					Common.ResetStatusPanel(stbUnderLying);
                    //					Common.SetStatusPanel(stbUnderLying, "UnderLying Saved.");
                    MessageBox.Show("Underlying saved.", "Prana Alert", MessageBoxButtons.OK);
                }

                txtUnderlying.Tag = null;

                result = newUnderLyingID;

                errorProvider1.SetError(cmbAsset, "");
                errorProvider1.SetError(txtUnderlying, "");
                return result;
            }
            return result;
        }

        /// <summary>
        /// This method saves the UnderLying details in the database by calling the SaveUnderLying method on
        /// the click event of Save Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                int checkID = SaveUnderlying();
                BindUnderLyingGrid();
                //To refresh the form as soon as the underlying is saved by calling the RefreshForm method.
                if (checkID >= 0)
                {
                    RefreshForm();
                }
            }
            catch (Exception ex)
            {
                #region Catch
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                    FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
                #endregion
            }
            finally
            {
                #region LogEntry

                Logger.LoggerWrite("button1_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "button1_Click", null);


                #endregion
            }
        }

        /// <summary>
        /// Blanks the textboxes and unselects any valid value in combobox
        /// </summary>
        private void RefreshForm()
        {

            cmbAsset.Value = int.MinValue;
            txtUnderlying.Text = "";
            txtComment.Text = "";
        }

        /// <summary>
        /// Various Bind methods are called on the on Load event of the UnderlyingDetails form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnderlyingDetails_Load(object sender, System.EventArgs e)
        {
            BindAssets();
            BindUnderLyingGrid();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        /// <summary>
        /// This method binds the underlying grid control by assigning the 
        /// underLyings object to its datasource property.
        /// </summary>
        private void BindUnderLyingGrid()
        {
            //this.dataGridTableStyle1.MappingName = "underLyings";

            //This method fetches the existing underlyings from the database.
            UnderLyings underLyings = AssetManager.GetUnderLyings();

            if (underLyings.Count <= 0)
            {
                underLyings.Add(new UnderLying(int.MinValue, "", int.MinValue));
            }
            grdUnderLying.DataSource = underLyings;
        }

        /// <summary>
        /// This method binds the existing <see cref="Assets"/> in the ComboBox control by assigning the 
        /// assets object to its datasource property.
        /// </summary>
        private void BindAssets()
        {
            //GetAssets method fetches the existing assets from the database.
            Assets assets = AssetManager.GetAssets();
            //Inserting the - Select - option in the Combo Box at the top.
            assets.Insert(0, new Asset(int.MinValue, "-Select-"));
            cmbAsset.DataSource = null;
            cmbAsset.DataSource = assets;
            cmbAsset.DisplayMember = "Name";
            cmbAsset.ValueMember = "AssetID";
            cmbAsset.Value = int.MinValue;
        }

        /// <summary>
        /// This method closes the form when clicked the close button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            //			Common.ResetStatusPanel(stbUnderLying);
            this.Close();
        }

        /// <summary>
        /// This method deletes the existing <see cref="UnderLying"/> from the Grid and its record from
        /// the DataBase on clicking the delete button in the form. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Check for deleting the underlying if the grid has any underlying.	
                if (grdUnderLying.Rows.Count > 0)
                {
                    string underLyingName = grdUnderLying.ActiveRow.Cells["Name"].Text.ToString();
                    if (underLyingName != "")
                    {
                        if (MessageBox.Show(this, "Do you want to delete this UnderLying?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the underlyingid from the currently selected row in the grid.
                            int underlyingID = int.Parse(grdUnderLying.ActiveRow.Cells["UnderlyingID"].Text.ToString());
                            bool chkVarraible = AssetManager.DeleteUnderlying(underlyingID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "This Underlying is referred in AUEC/Broker Venue Master/Company. Please remove references first to delete it.", "Underlying Delete");
                            }
                            else
                            {
                                //							Common.ResetStatusPanel(stbUnderLying);
                                //							Common.SetStatusPanel(stbUnderLying, "Underlying Deleted.!");
                                //Binding the UnderLying grid.
                                BindUnderLyingGrid();
                            }
                        }
                    }
                }
                else
                {
                    //					Common.ResetStatusPanel(stbUnderLying);
                    //					Common.SetStatusPanel(stbUnderLying, "No Data Available!");
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

        /// <summary>
        /// Method to edit the existing asset in the grid on clicking the edit button in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, System.EventArgs e)
        {
            //Edit: Edit.				
            try
            {
                //Check for editing the UnderLying if the grid has any underLying.
                if (grdUnderLying.Rows.Count > 0)
                {
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //					//the column positions relative to the underlyingname, underyingID, comment and assetID.
                    txtUnderlying.Text = grdUnderLying.ActiveRow.Cells["Name"].Text.ToString();
                    txtUnderlying.Tag = grdUnderLying.ActiveRow.Cells["UnderlyingID"].Text.ToString();
                    txtComment.Text = grdUnderLying.ActiveRow.Cells["Comment"].Text.ToString();
                    cmbAsset.Value = int.Parse(grdUnderLying.ActiveRow.Cells["AssetID"].Text.ToString());

                    //					Common.ResetStatusPanel(stbUnderLying);
                    //					Common.SetStatusPanel(stbUnderLying, "Edit Underlying.");
                }
                else
                {
                    //					Common.ResetStatusPanel(stbUnderLying);
                    //					Common.SetStatusPanel(stbUnderLying, "No Data Available");
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

                Logger.LoggerWrite("btnEdit_Click",
                    LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnEdit_Click", null);


                #endregion
            }
        }

        private void UnderlyingDetails_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            //			BindAssets();
            //			BindUnderLyingGrid();
        }


        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtUnderlying.Text = "";
            txtUnderlying.Tag = null;
            cmbAsset.Value = int.MinValue;
            txtComment.Text = "";
        }

        private void txtComment_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void grpUnderlying_Enter(object sender, System.EventArgs e)
        {

        }

        private void cmbAsset_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void label2_Click(object sender, System.EventArgs e)
        {

        }

        private void txtUnderlying_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void grdUnderLying_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }

        private void cmbAsset_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {

        }
    }
}
