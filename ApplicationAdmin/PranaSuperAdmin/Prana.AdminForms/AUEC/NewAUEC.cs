using Prana.Admin.BLL;
using Prana.Global;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;

namespace Prana.Admin
{
    public class NewAUEC : System.Windows.Forms.Form
    {
        private const string FORM_NAME = "NewAUEC : ";

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Infragistics.Win.Misc.UltraButton btnClose;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private System.Windows.Forms.ComboBox cmbExchange;
        private System.Windows.Forms.ComboBox cmbUnderlying;
        private System.Windows.Forms.ComboBox cmbAsset;

        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        private Infragistics.Win.Misc.UltraPanel NewAUEC_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NewAUEC_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NewAUEC_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NewAUEC_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _NewAUEC_UltraFormManager_Dock_Area_Bottom;
        private IContainer components;

        public NewAUEC()
        {
            InitializeComponent();
        }

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
                if (btnClose != null)
                {
                    btnClose.Dispose();
                }
                if (btnSave != null)
                {
                    btnSave.Dispose();
                }
                if (cmbExchange != null)
                {
                    cmbExchange.Dispose();
                }
                if (cmbUnderlying != null)
                {
                    cmbUnderlying.Dispose();
                }
                if (cmbAsset != null)
                {
                    cmbAsset.Dispose();
                }
                if (statusBarPanel1 != null)
                {
                    statusBarPanel1.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (panel1 != null)
                {
                    panel1.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
                }
                if (NewAUEC_Fill_Panel != null)
                {
                    NewAUEC_Fill_Panel.Dispose();
                }
                if (_NewAUEC_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _NewAUEC_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_NewAUEC_UltraFormManager_Dock_Area_Left != null)
                {
                    _NewAUEC_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_NewAUEC_UltraFormManager_Dock_Area_Right != null)
                {
                    _NewAUEC_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_NewAUEC_UltraFormManager_Dock_Area_Top != null)
                {
                    _NewAUEC_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private int _assetID = int.MinValue;
        private int _underlyingID = int.MinValue;
        private int _exchangeID = int.MinValue;
        private bool _isStored = false;

        public int AssetID
        {
            set { _assetID = value; }
            get { return _assetID; }
        }

        public int UnderlyingID
        {
            set { _underlyingID = value; }
            get { return _underlyingID; }
        }

        public int ExchangeID
        {
            set { _exchangeID = value; }
            get { return _exchangeID; }
        }

        public bool IsStored
        {
            get { return _isStored; }
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewAUEC));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbExchange = new System.Windows.Forms.ComboBox();
            this.cmbUnderlying = new System.Windows.Forms.ComboBox();
            this.cmbAsset = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnClose = new Infragistics.Win.Misc.UltraButton();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.NewAUEC_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._NewAUEC_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NewAUEC_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NewAUEC_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.NewAUEC_Fill_Panel.ClientArea.SuspendLayout();
            this.NewAUEC_Fill_Panel.SuspendLayout();
            this.SuspendLayout();

            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmbExchange);
            this.groupBox1.Controls.Add(this.cmbUnderlying);
            this.groupBox1.Controls.Add(this.cmbAsset);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(465, 105);
            this.inboxControlStyler1.SetStyleSettings(this.groupBox1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;

            this.cmbExchange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExchange.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbExchange.Location = new System.Drawing.Point(338, 35);
            this.cmbExchange.Name = "cmbExchange";
            this.cmbExchange.Size = new System.Drawing.Size(121, 21);
            this.inboxControlStyler1.SetStyleSettings(this.cmbExchange, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cmbExchange.TabIndex = 3;

            this.cmbUnderlying.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnderlying.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbUnderlying.Location = new System.Drawing.Point(175, 35);
            this.cmbUnderlying.Name = "cmbUnderlying";
            this.cmbUnderlying.Size = new System.Drawing.Size(121, 21);
            this.inboxControlStyler1.SetStyleSettings(this.cmbUnderlying, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cmbUnderlying.TabIndex = 2;

            this.cmbAsset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAsset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbAsset.Location = new System.Drawing.Point(6, 35);
            this.cmbAsset.Name = "cmbAsset";
            this.cmbAsset.Size = new System.Drawing.Size(121, 21);
            this.inboxControlStyler1.SetStyleSettings(this.cmbAsset, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.cmbAsset.TabIndex = 1;

            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(310, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.inboxControlStyler1.SetStyleSettings(this.label3, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label3.TabIndex = 2;
            this.label3.Text = "Exchange";

            this.label2.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(172, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 1;
            this.label2.Text = "Underlying";

            this.label1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 16);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 0;
            this.label1.Text = "Asset Class";

            this.btnSave.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(142, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Add";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnClose.BackColorInternal = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(223, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 455;

            this.errorProvider1.ContainerControl = this;

            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Location = new System.Drawing.Point(6, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(460, 26);
            this.inboxControlStyler1.SetStyleSettings(this.panel1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.panel1.TabIndex = 4;

            this.ultraFormManager1.Form = this;

            this.NewAUEC_Fill_Panel.ClientArea.Controls.Add(this.groupBox1);
            this.NewAUEC_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.NewAUEC_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.NewAUEC_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NewAUEC_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.NewAUEC_Fill_Panel.Name = "NewAUEC_Fill_Panel";
            this.NewAUEC_Fill_Panel.Size = new System.Drawing.Size(472, 145);
            this.NewAUEC_Fill_Panel.TabIndex = 7;

            this._NewAUEC_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NewAUEC_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NewAUEC_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._NewAUEC_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NewAUEC_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._NewAUEC_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._NewAUEC_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._NewAUEC_UltraFormManager_Dock_Area_Left.Name = "_NewAUEC_UltraFormManager_Dock_Area_Left";
            this._NewAUEC_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(4, 145);

            this._NewAUEC_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NewAUEC_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NewAUEC_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._NewAUEC_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NewAUEC_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._NewAUEC_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._NewAUEC_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(476, 27);
            this._NewAUEC_UltraFormManager_Dock_Area_Right.Name = "_NewAUEC_UltraFormManager_Dock_Area_Right";
            this._NewAUEC_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(4, 145);

            this._NewAUEC_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NewAUEC_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NewAUEC_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._NewAUEC_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NewAUEC_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._NewAUEC_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._NewAUEC_UltraFormManager_Dock_Area_Top.Name = "_NewAUEC_UltraFormManager_Dock_Area_Top";
            this._NewAUEC_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(480, 27);

            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 172);
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.Name = "_NewAUEC_UltraFormManager_Dock_Area_Bottom";
            this._NewAUEC_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(480, 4);

            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(480, 176);
            this.Controls.Add(this.NewAUEC_Fill_Panel);
            this.Controls.Add(this._NewAUEC_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._NewAUEC_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._NewAUEC_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._NewAUEC_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(428, 148);
            this.Name = "NewAUEC";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "AUEC - Add New";
            this.Load += new System.EventHandler(this.NewAUEC_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.NewAUEC_Fill_Panel.ClientArea.ResumeLayout(false);
            this.NewAUEC_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private Prana.Admin.BLL.AUEC _savedAUEC = null;

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Prana.Admin.BLL.AUEC auec = new Prana.Admin.BLL.AUEC();
                auec.AssetID = (int)cmbAsset.SelectedValue;
                auec.UnderlyingID = (int)cmbUnderlying.SelectedValue;
                auec.ExchangeID = (int)cmbExchange.SelectedValue;

                errorProvider1.SetError(cmbAsset, "");
                errorProvider1.SetError(cmbUnderlying, "");
                errorProvider1.SetError(cmbExchange, "");
                if (auec.AssetID == int.MinValue)
                {
                    errorProvider1.SetError(cmbAsset, "Provide Value!");
                }
                else if (auec.UnderlyingID < 1)
                {
                    errorProvider1.SetError(cmbUnderlying, "Provide Value!");
                }
                else if (auec.ExchangeID == int.MinValue)
                {
                    errorProvider1.SetError(cmbExchange, "Provide Value!");
                }
                else
                {
                    _isStored = true;

                    _savedAUEC = auec;
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                Logger.LoggerWrite("btnSave_Click",
                LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                FORM_NAME + "btnSave_Click", null);

            }
        }

        public Prana.Admin.BLL.AUEC SavedAUEC
        {
            get { return _savedAUEC; }
        }

        private void NewAUEC_Load(object sender, System.EventArgs e)
        {
            try
            {
                BindAssetClass();
                BindUnderlyingClass();
                BindExchanges();

                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_TRADING_TICKET);
            }
            catch (Exception ex)
            {
                string formattedInfo = ex.StackTrace.ToString();
                Logger.LoggerWrite(formattedInfo, LoggingConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error,
                FORM_NAME);
                Logger.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), LoggingConstants.POLICY_LOGANDSHOW);
            }
            finally
            {
                Logger.LoggerWrite("NewAUEC_Load",
                LoggingConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                FORM_NAME + "NewAUEC_Load", null);

            }
        }

        private Assets assets = null;

        private void BindAssetClass()
        {
            assets = AssetManager.GetAssets();

            assets.Insert(0, new Asset(int.MinValue, "-Select-"));
            cmbAsset.DataSource = assets;
            cmbAsset.DisplayMember = "Name";
            cmbAsset.ValueMember = "AssetID";
            cmbAsset.SelectedIndex = 0;

            cmbAsset.SelectedValue = _assetID;
        }

        private void BindUnderlyingClass()
        {
            UnderLyings underLyings = AssetManager.GetUnderLyings();

            underLyings.Insert(0, new UnderLying(int.MinValue, "-Select-"));
            cmbUnderlying.DataSource = underLyings;
            cmbUnderlying.DisplayMember = "Name";
            cmbUnderlying.ValueMember = "UnderlyingID";
        }

        private void BindExchanges()
        {
            Exchanges exchanges = ExchangeManager.GetExchanges();

            exchanges.Insert(0, new Exchange(int.MinValue, "-Select-"));

            cmbExchange.DataSource = exchanges;
            cmbExchange.DisplayMember = "DisplayName";
            cmbExchange.ValueMember = "ExchangeID";
        }
    }
}