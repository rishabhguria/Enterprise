using Infragistics.Win;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Prana.TradeManager
{
    /// <summary>
    /// Summary description for NewOrderPrompt.
    /// </summary>
    public class PromptWindow : System.Windows.Forms.Form
    {
        private bool _bTrade = false;
        private decimal _cumQty = decimal.Zero;
        private decimal _sharesPresent = decimal.Zero;
        private Infragistics.Win.Misc.UltraButton btnPlace;
        private Infragistics.Win.Misc.UltraButton btnEdit;
        private Infragistics.Win.Misc.UltraLabel lblMsg;
        private Prana.Utilities.UI.UIUtilities.PranaNumericUpDown nmrcTargetQuantity;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.Misc.UltraPanel PromptWindow_Fill_Panel;
        private Infragistics.Win.Misc.UltraPanel MsgPanel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PromptWindow_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PromptWindow_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PromptWindow_UltraFormManager_Dock_Area_Top;
        private IContainer components;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _PromptWindow_UltraFormManager_Dock_Area_Bottom;

        public Infragistics.Win.Misc.UltraButton BtnContinue
        {
            get { return btnPlace; }
        }
        public Infragistics.Win.Misc.UltraButton BtnEdit
        {
            get { return btnEdit; }
        }

        public PromptWindow(string msg, string windowName)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            lblMsg.Text = msg;
            this.Text = windowName;
            //this.Parent=(System.Windows.Forms.Control )parent;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void SetButtonAttributes()
        {
            BtnContinue.Text = "Commit && Send";
            BtnEdit.Text = "Review";
            this.btnPlace.Location = new System.Drawing.Point(71, 50);
            this.btnPlace.Size = new System.Drawing.Size(98, 23);
            this.btnEdit.Location = new System.Drawing.Point(175, 50);
            this.btnEdit.Size = new System.Drawing.Size(98, 23);
            this.MinimumSize = new System.Drawing.Size(365, 120);
            this.ClientSize = new System.Drawing.Size(365, 120);
        }
        /// <summary>
        /// Set Dynamic Window Size for GTC GTD orders
        /// </summary>
        public void SetDynamicWindowSize()
        {
            this.ClientSize = new System.Drawing.Size(400, 180);
            this.MinimumSize = new System.Drawing.Size(400, 180);
            this.btnEdit.Location = new System.Drawing.Point(182, 95);
            this.btnPlace.Location = new System.Drawing.Point(92, 95);
            this.MaximumSize = new System.Drawing.Size(400, 180);
            this.lblMsg.Size = new System.Drawing.Size(350, 90);
            this.MsgPanel.Size = new System.Drawing.Size(350, 90);
        }
        public PromptWindow(string msg, string windowName, double orderQuantity, double tgtQuantity)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            lblMsg.Text = msg;
            nmrcTargetQuantity.Visible = true;
            nmrcTargetQuantity.Maximum = Convert.ToDecimal(orderQuantity);
            nmrcTargetQuantity.Value = Convert.ToDecimal(tgtQuantity);
            this.Text = windowName;
            //this.Parent=(System.Windows.Forms.Control )parent;

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
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
                if (_PromptWindow_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _PromptWindow_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (_PromptWindow_UltraFormManager_Dock_Area_Top != null)
                {
                    _PromptWindow_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_PromptWindow_UltraFormManager_Dock_Area_Left != null)
                {
                    _PromptWindow_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_PromptWindow_UltraFormManager_Dock_Area_Right != null)
                {
                    _PromptWindow_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (btnPlace != null)
                {
                    btnPlace.Dispose();
                }
                if (btnEdit != null)
                {
                    btnEdit.Dispose();
                }
                if (lblMsg != null)
                {
                    lblMsg.Dispose();
                }
                if (nmrcTargetQuantity != null)
                {
                    nmrcTargetQuantity.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (PromptWindow_Fill_Panel != null)
                {
                    PromptWindow_Fill_Panel.Dispose();
                }
                if (MsgPanel != null)
                {
                    MsgPanel.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.nmrcTargetQuantity = new Prana.Utilities.UI.UIUtilities.PranaNumericUpDown();
            this.lblMsg = new Infragistics.Win.Misc.UltraLabel();
            this.btnPlace = new Infragistics.Win.Misc.UltraButton();
            this.btnEdit = new Infragistics.Win.Misc.UltraButton();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this.PromptWindow_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.MsgPanel = new Infragistics.Win.Misc.UltraPanel();
            this._PromptWindow_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PromptWindow_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PromptWindow_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.nmrcTargetQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            this.PromptWindow_Fill_Panel.ClientArea.SuspendLayout();
            this.MsgPanel.ClientArea.SuspendLayout();
            this.PromptWindow_Fill_Panel.SuspendLayout();
            this.MsgPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // nmrcTargetQuantity
            // 
            this.nmrcTargetQuantity.AllowThousandSeperator = true;
            this.nmrcTargetQuantity.Location = new System.Drawing.Point(100, 53);
            this.nmrcTargetQuantity.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nmrcTargetQuantity.Name = "nmrcTargetQuantity";
            this.nmrcTargetQuantity.ShowCommaSeperatorOnEditing = true;
            this.nmrcTargetQuantity.Size = new System.Drawing.Size(117, 20);
            this.nmrcTargetQuantity.TabIndex = 8;
            this.nmrcTargetQuantity.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmrcTargetQuantity.Visible = false;
            // 
            // lblMsg
            // 
            this.lblMsg.Enabled = false;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(0, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(332, 61);
            this.lblMsg.TabIndex = 0;
            // 
            // btnPlace
            // 
            this.btnPlace.Location = new System.Drawing.Point(82, 82);
            this.btnPlace.Name = "btnPlace";
            this.btnPlace.Size = new System.Drawing.Size(75, 23);
            this.btnPlace.TabIndex = 1;
            this.btnPlace.Text = "Continue";
            this.btnPlace.Click += new System.EventHandler(this.btnPlace_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(172, 82);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit Order";
            this.btnEdit.Click += new System.EventHandler(this.btnDontPlace_Click);
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // PromptWindow_Fill_Panel
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.PromptWindow_Fill_Panel.Appearance = appearance1;
            // 
            // PromptWindow_Fill_Panel.ClientArea
            // 
            this.PromptWindow_Fill_Panel.ClientArea.Controls.Add(this.btnEdit);
            this.PromptWindow_Fill_Panel.ClientArea.Controls.Add(this.btnPlace);
            this.PromptWindow_Fill_Panel.ClientArea.Controls.Add(this.nmrcTargetQuantity);
            this.PromptWindow_Fill_Panel.ClientArea.Controls.Add(this.MsgPanel);
            this.PromptWindow_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PromptWindow_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PromptWindow_Fill_Panel.Location = new System.Drawing.Point(8, 32);
            this.PromptWindow_Fill_Panel.Name = "PromptWindow_Fill_Panel";
            this.PromptWindow_Fill_Panel.Size = new System.Drawing.Size(344, 110);
            this.PromptWindow_Fill_Panel.TabIndex = 0;
            // 
            // MsgPanel
            // 
            this.MsgPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.MsgPanel.Appearance = appearance1;
            // 
            // MsgPanel.ClientArea
            // 
            this.MsgPanel.ClientArea.Controls.Add(this.lblMsg);
            this.MsgPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.MsgPanel.Location = new System.Drawing.Point(8, 8);
            this.MsgPanel.Name = "MsgPanel";
            this.MsgPanel.Size = new System.Drawing.Size(332, 61);
            this.MsgPanel.TabIndex = 0;
            // 
            // _PromptWindow_UltraFormManager_Dock_Area_Left
            // 
            this._PromptWindow_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PromptWindow_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PromptWindow_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._PromptWindow_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PromptWindow_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._PromptWindow_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._PromptWindow_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 32);
            this._PromptWindow_UltraFormManager_Dock_Area_Left.Name = "_PromptWindow_UltraFormManager_Dock_Area_Left";
            this._PromptWindow_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 110);
            // 
            // _PromptWindow_UltraFormManager_Dock_Area_Right
            // 
            this._PromptWindow_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PromptWindow_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PromptWindow_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._PromptWindow_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PromptWindow_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._PromptWindow_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._PromptWindow_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(352, 32);
            this._PromptWindow_UltraFormManager_Dock_Area_Right.Name = "_PromptWindow_UltraFormManager_Dock_Area_Right";
            this._PromptWindow_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 110);
            // 
            // _PromptWindow_UltraFormManager_Dock_Area_Top
            // 
            this._PromptWindow_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PromptWindow_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PromptWindow_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._PromptWindow_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PromptWindow_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._PromptWindow_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._PromptWindow_UltraFormManager_Dock_Area_Top.Name = "_PromptWindow_UltraFormManager_Dock_Area_Top";
            this._PromptWindow_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(360, 32);
            // 
            // _PromptWindow_UltraFormManager_Dock_Area_Bottom
            // 
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 142);
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.Name = "_PromptWindow_UltraFormManager_Dock_Area_Bottom";
            this._PromptWindow_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(360, 8);
            // 
            // PromptWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(360, 150);
            this.Controls.Add(this.PromptWindow_Fill_Panel);
            this.Controls.Add(this._PromptWindow_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._PromptWindow_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._PromptWindow_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._PromptWindow_UltraFormManager_Dock_Area_Bottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(360, 150);
            this.Name = "PromptWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NewOrderPrompt";
            this.Load += new System.EventHandler(this.PromptWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmrcTargetQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            this.PromptWindow_Fill_Panel.ClientArea.ResumeLayout(false);
            this.PromptWindow_Fill_Panel.ResumeLayout(false);
            this.MsgPanel.ClientArea.ResumeLayout(false);
            this.MsgPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void btnPlace_Click(object sender, System.EventArgs e)
        {
            _bTrade = true;
            if (isPopUsedForShareOutstanding)
                _sharesPresent = nmrcTargetQuantity.Value;
            else
                _cumQty = nmrcTargetQuantity.Value;
            this.Close();
        }

        private void btnDontPlace_Click(object sender, System.EventArgs e)
        {
            _bTrade = false;
            this.Close();

        }
        public bool ShouldTrade
        {
            get { return _bTrade; }
        }

        public decimal SharesPresent
        {
            get { return _sharesPresent; }
        }

        public decimal CumQty
        {
            get { return _cumQty; }
        }

        private void PromptWindow_Load(object sender, EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_WATCHLIST);
                SetButtonsColor();
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

        /// <summary>
        /// Sets the color of the buttons.
        /// </summary>
        private void SetButtonsColor()
        {
            try
            {
                BtnEdit.ButtonStyle = UIElementButtonStyle.Button3D;
                BtnEdit.BackColor = Color.FromArgb(140, 5, 5);
                BtnEdit.ForeColor = Color.White;
                BtnEdit.UseAppStyling = false;
                BtnEdit.UseOsThemes = DefaultableBoolean.False;

                BtnContinue.ButtonStyle = UIElementButtonStyle.Button3D;
                BtnContinue.BackColor = Color.FromArgb(104, 156, 46);
                BtnContinue.ForeColor = Color.White;
                BtnContinue.UseAppStyling = false;
                BtnContinue.UseOsThemes = DefaultableBoolean.False;
                if (BtnEdit.Text == "Review")
                {
                    BtnEdit.BackColor = Color.FromArgb(72, 99, 160);
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
        /// Sets the properties for fat finger.
        /// </summary>
        public void SetPropertiesForTradingRules()
        {
            try
            {
                this.lblMsg.Location = new System.Drawing.Point(0, 0);
                this.lblMsg.Size = new System.Drawing.Size(332, 71);
                this.lblMsg.AutoSize = true;
                this.PromptWindow_Fill_Panel.Size = new System.Drawing.Size(344, 121);
                this.ClientSize = new System.Drawing.Size(360, 160);
                this.MinimumSize = new System.Drawing.Size(360, 160);
                this.MsgPanel.Location = new System.Drawing.Point(8, 8);
                this.MsgPanel.Size = new System.Drawing.Size(344, 78);
                this.btnEdit.Location = new System.Drawing.Point(172, 92);
                this.btnPlace.Location = new System.Drawing.Point(82, 92);
                this.MsgPanel.AutoScroll = true;
                lblMsg.WrapText = true;
                lblMsg.MaximumSize = new System.Drawing.Size(332, 0);
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
        /// Sets the properties for shares Outstanding.
        /// </summary>
        bool isPopUsedForShareOutstanding = false;
        public void SetPropertiesForSharesOutstanding()
        {
            try
            {
                this.lblMsg.Location = new System.Drawing.Point(0, 0);
                this.lblMsg.Size = new System.Drawing.Size(332, 71);
                this.lblMsg.AutoSize = true;
                this.PromptWindow_Fill_Panel.Size = new System.Drawing.Size(344, 121);
                this.ClientSize = new System.Drawing.Size(360, 160);
                this.MinimumSize = new System.Drawing.Size(360, 160);
                this.MsgPanel.Location = new System.Drawing.Point(8, 8);
                this.MsgPanel.Size = new System.Drawing.Size(344, 78);
                this.btnEdit.Location = new System.Drawing.Point(172, 92);
                this.btnPlace.Location = new System.Drawing.Point(82, 92);
                this.btnPlace.Text = "Yes";
                this.btnEdit.Text = "No";
                this.MsgPanel.AutoScroll = true;
                lblMsg.WrapText = true;
                lblMsg.MaximumSize = new System.Drawing.Size(332, 0);
                this.nmrcTargetQuantity.Visible = true;
                this.nmrcTargetQuantity.Value = 0;
                isPopUsedForShareOutstanding = true;
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
        /// Sets the properties for shares Outstanding.
        /// </summary>
        public void SetPropertiesForRestrictedSymbol()
        {
            try
            {
                this.lblMsg.Location = new System.Drawing.Point(0, 0);
                this.lblMsg.Size = new System.Drawing.Size(332, 71);
                this.lblMsg.AutoSize = true;
                this.PromptWindow_Fill_Panel.Size = new System.Drawing.Size(344, 121);
                this.ClientSize = new System.Drawing.Size(360, 160);
                this.MinimumSize = new System.Drawing.Size(360, 160);
                this.MsgPanel.Location = new System.Drawing.Point(8, 8);
                this.MsgPanel.Size = new System.Drawing.Size(344, 78);
                this.btnEdit.Location = new System.Drawing.Point(172, 92);
                this.btnPlace.Location = new System.Drawing.Point(82, 92);
                this.MsgPanel.AutoScroll = true;
                lblMsg.WrapText = true;
                lblMsg.MaximumSize = new System.Drawing.Size(332, 0);
                this.btnPlace.Text = "Yes";
                this.btnEdit.Text = "No";
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
    }
}
