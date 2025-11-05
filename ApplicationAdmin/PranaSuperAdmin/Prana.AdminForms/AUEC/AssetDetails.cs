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
    /// Summary description for AssetDetails.
    /// </summary>
    public class AssetDetails : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "AssetDetails : ";
        #region Private variable

        #region Grid Constants.

        private const int GRD_ASSET_ID = 0;
        private const int GRD_NAME = 1;
        private const int GRD_COMMENT = 2;

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtAsset;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnReset;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdAsset;
        private System.Windows.Forms.Label label4;

        #endregion
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel AssetDetails_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AssetDetails_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AssetDetails_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AssetDetails_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AssetDetails_UltraFormManager_Dock_Area_Bottom;
        private IContainer components;

        public AssetDetails()
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
                if (groupBox1 != null)
                {
                    groupBox1.Dispose();
                }
                if (label1 != null)
                {
                    label1.Dispose();
                }
                if (label2 != null)
                {
                    label2.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (txtAsset != null)
                {
                    txtAsset.Dispose();
                }
                if (txtComment != null)
                {
                    txtComment.Dispose();
                }
                if (btnDelete != null)
                {
                    btnDelete.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (btnReset != null)
                {
                    btnReset.Dispose();
                }
                if (grdAsset != null)
                {
                    grdAsset.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (AssetDetails_Fill_Panel != null)
                {
                    AssetDetails_Fill_Panel.Dispose();
                }
                if (_AssetDetails_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AssetDetails_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_AssetDetails_UltraFormManager_Dock_Area_Left != null)
                {
                    _AssetDetails_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_AssetDetails_UltraFormManager_Dock_Area_Right != null)
                {
                    _AssetDetails_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_AssetDetails_UltraFormManager_Dock_Area_Top != null)
                {
                    _AssetDetails_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetDetails));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AssetID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Comment", 2);
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.txtAsset = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.grdAsset = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.AssetDetails_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._AssetDetails_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AssetDetails_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AssetDetails_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.AssetDetails_Fill_Panel.ClientArea.SuspendLayout();
            this.AssetDetails_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtComment);
            this.groupBox1.Controls.Add(this.txtAsset);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.groupBox1.Location = new System.Drawing.Point(57, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 72);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add/Update Asset ";
            this.groupBox1.Visible = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(56, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 10);
            this.inboxControlStyler1.SetStyleSettings(this.label4, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label4.TabIndex = 17;
            this.label4.Text = "*";
            // 
            // txtComment
            // 
            this.txtComment.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtComment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComment.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtComment.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtComment.Location = new System.Drawing.Point(84, 44);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(130, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtComment, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtComment.TabIndex = 3;
            this.txtComment.GotFocus += new System.EventHandler(this.txtComment_GotFocus);
            this.txtComment.LostFocus += new System.EventHandler(this.txtComment_LostFocus);
            // 
            // txtAsset
            // 
            this.txtAsset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtAsset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAsset.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtAsset.Location = new System.Drawing.Point(84, 22);
            this.txtAsset.MaxLength = 50;
            this.txtAsset.Name = "txtAsset";
            this.txtAsset.Size = new System.Drawing.Size(130, 21);
            this.inboxControlStyler1.SetStyleSettings(this.txtAsset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.txtAsset.TabIndex = 2;
            this.txtAsset.GotFocus += new System.EventHandler(this.txtAsset_GotFocus);
            this.txtAsset.LostFocus += new System.EventHandler(this.txtAsset_LostFocus);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(15, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 18);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 1;
            this.label2.Text = "Comment";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(15, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 15);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(48, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(8, 8);
            this.inboxControlStyler1.SetStyleSettings(this.label3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label3.TabIndex = 7;
            this.label3.Text = "*";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            //this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Text = "Save";
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnSave.Location = new System.Drawing.Point(55, 202);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnSave, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnSave.TabIndex = 2;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            //this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Text = "Close";
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnClose.Location = new System.Drawing.Point(136, 224);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnClose, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnClose.TabIndex = 3;
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
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnDelete.Location = new System.Drawing.Point(174, 102);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnDelete, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnDelete.TabIndex = 5;
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
            this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEdit.Location = new System.Drawing.Point(96, 102);
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
            this.btnReset.Text = "Reset";
            //this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnReset.Location = new System.Drawing.Point(217, 202);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.inboxControlStyler1.SetStyleSettings(this.btnReset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.btnReset.TabIndex = 8;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Visible = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // grdAsset
            // 
            this.grdAsset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdAsset.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 255;
            appearance1.TextHAlignAsString = "Center";
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Center";
            ultraGridColumn2.Header.Appearance = appearance2;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 147;
            appearance3.TextHAlignAsString = "Center";
            ultraGridColumn3.CellAppearance = appearance3;
            appearance4.FontData.BoldAsString = "True";
            appearance4.TextHAlignAsString = "Center";
            ultraGridColumn3.Header.Appearance = appearance4;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 154;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 72;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdAsset.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdAsset.DisplayLayout.GroupByBox.Hidden = true;
            this.grdAsset.DisplayLayout.MaxColScrollRegions = 1;
            this.grdAsset.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdAsset.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdAsset.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdAsset.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdAsset.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdAsset.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdAsset.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand;
            this.grdAsset.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdAsset.Location = new System.Drawing.Point(10, 2);
            this.grdAsset.Name = "grdAsset";
            this.grdAsset.Size = new System.Drawing.Size(322, 216);
            this.grdAsset.TabIndex = 45;
            this.grdAsset.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // AssetDetails_Fill_Panel
            // 
            // 
            // AssetDetails_Fill_Panel.ClientArea
            // 
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.grdAsset);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.btnReset);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.btnEdit);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.btnDelete);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.btnClose);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.btnSave);
            this.AssetDetails_Fill_Panel.ClientArea.Controls.Add(this.groupBox1);
            this.AssetDetails_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.AssetDetails_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssetDetails_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.AssetDetails_Fill_Panel.Name = "AssetDetails_Fill_Panel";
            this.AssetDetails_Fill_Panel.Size = new System.Drawing.Size(344, 251);
            this.AssetDetails_Fill_Panel.TabIndex = 0;
            // 
            // _AssetDetails_UltraFormManager_Dock_Area_Left
            // 
            this._AssetDetails_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssetDetails_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AssetDetails_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AssetDetails_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssetDetails_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AssetDetails_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._AssetDetails_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._AssetDetails_UltraFormManager_Dock_Area_Left.Name = "_AssetDetails_UltraFormManager_Dock_Area_Left";
            this._AssetDetails_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 251);
            // 
            // _AssetDetails_UltraFormManager_Dock_Area_Right
            // 
            this._AssetDetails_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssetDetails_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AssetDetails_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AssetDetails_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssetDetails_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AssetDetails_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._AssetDetails_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(348, 27);
            this._AssetDetails_UltraFormManager_Dock_Area_Right.Name = "_AssetDetails_UltraFormManager_Dock_Area_Right";
            this._AssetDetails_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 251);
            // 
            // _AssetDetails_UltraFormManager_Dock_Area_Top
            // 
            this._AssetDetails_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssetDetails_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AssetDetails_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AssetDetails_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssetDetails_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AssetDetails_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AssetDetails_UltraFormManager_Dock_Area_Top.Name = "_AssetDetails_UltraFormManager_Dock_Area_Top";
            this._AssetDetails_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(352, 27);
            // 
            // _AssetDetails_UltraFormManager_Dock_Area_Bottom
            // 
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 278);
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.Name = "_AssetDetails_UltraFormManager_Dock_Area_Bottom";
            this._AssetDetails_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(352, 4);
            // 
            // AssetDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(352, 282);
            this.Controls.Add(this.AssetDetails_Fill_Panel);
            this.Controls.Add(this._AssetDetails_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AssetDetails_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AssetDetails_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AssetDetails_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "AssetDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Asset";
            this.Load += new System.EventHandler(this.AssetDetails_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdAsset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.AssetDetails_Fill_Panel.ClientArea.ResumeLayout(false);
            this.AssetDetails_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #region Focus Colors
        private void txtAsset_GotFocus(object sender, System.EventArgs e)
        {
            txtAsset.BackColor = Color.LemonChiffon;
        }
        private void txtAsset_LostFocus(object sender, System.EventArgs e)
        {
            txtAsset.BackColor = Color.White;
        }
        private void txtComment_GotFocus(object sender, System.EventArgs e)
        {
            txtComment.BackColor = Color.LemonChiffon;
        }
        private void txtComment_LostFocus(object sender, System.EventArgs e)
        {
            txtComment.BackColor = Color.White;
        }
        #endregion

        /// <summary>
        /// This method closes the AssetDetail Form, associated with the close button click event of the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, System.EventArgs e)
        {
            //			Common.ResetStatusPanel(stbAsset);
            this.Close();
        }

        /// <summary>
        /// This method saves the <see cref="Asset"/> details in DataBase. This method is 
        /// associated with the Save button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                //The checkID is used to store the currenly saved id of the asset in the database.
                int checkID = SaveAssetDetails();
                //Binding grid after saving the an asset detail.
                BindAssetGrid();
                if (checkID >= 0)
                {
                    //Calling refresh form after saving an asset detail after checking that whether the asset
                    //is saved successfully or not by checking for the positive value of checkID.
                    RefreshForm();
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

        //This method blanks the textboxes in the assetdetail form.
        private void RefreshForm()
        {
            txtAsset.Text = "";
            txtComment.Text = "";

            //			int i = 0;
            //			int[] arr;
            //			for(i=0;i<=5;i++)
            //			{
            //				arr[i] = 1;
            //			}

        }

        /// <summary>
        /// This method saves the <see cref="Asset"/> to the database.
        /// </summary>
        /// <returns></returns>
        private int SaveAssetDetails()
        {
            int result = int.MinValue;

            Asset asset = new Asset();

            if (txtAsset.Tag != null)
            {
                //Update
                asset.AssetID = int.Parse(txtAsset.Tag.ToString());
            }
            //Validation to check for the empty value in Asset textbox
            if (txtAsset.Text.Trim().Length == 0)
            {
                errorProvider1.SetError(txtAsset, "Provide Value!");
                return result;
            }
            else
            {
                asset.Name = txtAsset.Text.Trim();
                errorProvider1.SetError(txtAsset, "");
            }
            asset.Comment = txtComment.Text.Trim();

            //Saving the asset data and retrieving the assetid for the newly added asset.
            int newAssetID = AssetManager.SaveAsset(asset);
            //Showing the message at statusbar: asset already existing by checking the asset id value to -1 
            if (newAssetID == -1)
            {
                //				Common.ResetStatusPanel(stbAsset);
                //				Common.SetStatusPanel(stbAsset, "Asset already exists.");
                MessageBox.Show("Asset already exists.", "Prana Alert", MessageBoxButtons.OK);
            }
            //Showing the message at statusbar: Asset data saved
            else
            {
                //				Common.ResetStatusPanel(stbAsset);
                //				Common.SetStatusPanel(stbAsset, "Asset data saved.");
                MessageBox.Show("Asset saved.", "Prana Alert", MessageBoxButtons.OK);
                txtAsset.Tag = null;
            }
            result = newAssetID;
            //Returning the newly added asset id.
            return result;
        }

        /// <summary>
        /// This method calls the BindAssetGrid method on the AssetDetail Form load event to bind the 
        /// Asset Grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssetDetails_Load(object sender, System.EventArgs e)
        {
            //Calling the BindAssetGrid method on the AssetDetail Form load event to bind the Asset Grid.
            BindAssetGrid();
            CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
        }

        /// <summary>
        /// This method binds the Asset Grid.
        /// </summary>
        private void BindAssetGrid()
        {
            //this.dataGridTableStyle1.MappingName = "assets";

            //Fetching the existing assets from the database and binding it to the grid.
            Assets assets = AssetManager.GetAssets();
            //Assigning the grid's datasource to the assets object.

            if (assets.Count <= 0)
            {
                assets.Add(new Asset(int.MinValue, ""));
            }
            grdAsset.DataSource = assets;

        }

        /// <summary>
        /// This method deletes the existing <see cref="Asset"/> from the  Grid and its record from
        /// the DataBase on clicking the delete button in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Check for deleting the asset if the grid has any asset.
                if (grdAsset.Rows.Count > 0)
                {
                    string assetName = grdAsset.ActiveRow.Cells["Name"].Text.ToString();
                    if (assetName != "")
                    {
                        //Asking the user to be sure about deleting the asset.
                        if (MessageBox.Show(this, "Do you want to delete this Asset?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the assetid from the currently selected row in the grid.
                            int assetID = int.Parse(grdAsset.ActiveRow.Cells["AssetID"].Text.ToString());

                            bool chkVarraible = AssetManager.DeleteAsset(assetID);
                            if (!(chkVarraible))
                            {
                                MessageBox.Show(this, "This Asset is referred in Underlying/AUEC. Please remove references first to delete it.", "Asset Delete");
                            }
                            else
                            {
                                //Showing the message at statusbar: Asset deleted.
                                //							Common.ResetStatusPanel(stbAsset);
                                //							Common.SetStatusPanel(stbAsset, "Asset deleted.");
                                //Binding the asset grid.
                                BindAssetGrid();
                            }

                        }
                    }
                    //Binding the asset grid.
                    BindAssetGrid();
                }
                else
                {
                    //Showing the message at statusbar: No Data Available.
                    //					Common.ResetStatusPanel(stbAsset);
                    //					Common.SetStatusPanel(stbAsset, "No Data Available");
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
                    FORM_NAME + "btnSave_Click", null);


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
            try
            {
                //Check for editing the asset if the grid has any asset.
                if (grdAsset.Rows.Count > 0)
                {
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the asset name, assetID and comment.
                    txtAsset.Text = grdAsset.ActiveRow.Cells["Name"].Text.ToString();
                    txtAsset.Tag = grdAsset.ActiveRow.Cells["AssetID"].Text.ToString();
                    txtComment.Text = grdAsset.ActiveRow.Cells["Comment"].Text.ToString();
                }
                else
                {
                    //Showing the message at statusbar: No Data Available.
                    //					Common.ResetStatusPanel(stbAsset);
                    //					Common.SetStatusPanel(stbAsset, "No Data Available!");
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



        private void btnReset_Click(object sender, System.EventArgs e)
        {
            txtAsset.Text = "";
            txtAsset.Tag = null;
            txtComment.Text = "";
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }
    }
}
