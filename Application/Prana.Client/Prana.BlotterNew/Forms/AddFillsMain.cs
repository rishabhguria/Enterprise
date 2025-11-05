using Prana.BusinessObjects;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using System;
using System.ComponentModel;

namespace Prana.Blotter
{
    /// <summary>
    /// Summary description for AddFills.
    /// </summary>
    public class AddFills : System.Windows.Forms.Form
    {
        private Prana.Blotter.AddFillsControl addFills1;
        private IContainer components;
        private Infragistics.Win.Misc.UltraLabel label1;
        private Infragistics.Win.Misc.UltraLabel label2;
        private Infragistics.Win.Misc.UltraLabel label3;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.Misc.UltraLabel lblClOrderID;
        private Infragistics.Win.Misc.UltraLabel lblTargetQty;
        private Infragistics.Win.Misc.UltraLabel lblExecQty;
        private Infragistics.Win.Misc.UltraLabel lblAvgPrice;
        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.Misc.UltraPanel ultraPanel2;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager1;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddFills_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddFills_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddFills_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _AddFills_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
        OrderSingle _order;
        /// <summary>
        /// AddFills 
        /// </summary>
        public AddFills()
        {
            _order = new OrderSingle();
        }
        /// <summary>
        /// SetCommunicationManager 
        /// </summary>
        /// <param name="order"/>
        public AddFills(Prana.BusinessObjects.OrderSingle order)
            : this()
        {
            try
            {
                //
                // Required for Windows Form Designer support
                //
                _order = order;
                InitializeComponent();
                addFills1.ChangeAvgPrice += new EventHandler(addFills1_ChangeAvgPrice);
                addFills1.SendFillsToServer += new EventHandler(addFills1_SendFillsToServer);
                //
                // TODO: Add any constructor code after InitializeComponent call
                //
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
        /// AddFills 
        /// </summary>
        /// <param name="parentClOrderID"/>
        /// <param name="qty"/>
        /// <param name="cumQty"/>
        /// <param name="avgPrice"/>
        public AddFills(string parentClOrderID, double qty, double cumQty, float avgPrice)
            : this()
        {
            try
            {
                //
                // Required for Windows Form Designer support
                //
                _order.ParentClOrderID = parentClOrderID;
                _order.Quantity = qty;
                _order.CumQty = cumQty;
                _order.AvgPrice = avgPrice;
                InitializeComponent();
                addFills1.ChangeAvgPrice += new EventHandler(addFills1_ChangeAvgPrice);
                addFills1.SendFillsToServer += new EventHandler(addFills1_SendFillsToServer);
                //
                // TODO: Add any constructor code after InitializeComponent call
                //
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
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                addFills1.ChangeAvgPrice -= new EventHandler(addFills1_ChangeAvgPrice);
                addFills1.SendFillsToServer -= new EventHandler(addFills1_SendFillsToServer);
                if (components != null)
                {
                    components.Dispose();
                }
                if (addFills1 != null)
                {
                    addFills1.Dispose();
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
                if (lblClOrderID != null)
                {
                    lblClOrderID.Dispose();
                }
                if (lblTargetQty != null)
                {
                    lblTargetQty.Dispose();
                }
                if (lblExecQty != null)
                {
                    lblExecQty.Dispose();
                }
                if (lblAvgPrice != null)
                {
                    lblAvgPrice.Dispose();
                }
                if (ultraPanel1 != null)
                {
                    ultraPanel1.Dispose();
                }
                if (ultraPanel2 != null)
                {
                    ultraPanel2.Dispose();
                }
                if (ultraFormManager1 != null)
                {
                    ultraFormManager1.Dispose();
                }
                if (_AddFills_UltraFormManager_Dock_Area_Left != null)
                {
                    _AddFills_UltraFormManager_Dock_Area_Left.Dispose();
                }
                if (_AddFills_UltraFormManager_Dock_Area_Right != null)
                {
                    _AddFills_UltraFormManager_Dock_Area_Right.Dispose();
                }
                if (_AddFills_UltraFormManager_Dock_Area_Top != null)
                {
                    _AddFills_UltraFormManager_Dock_Area_Top.Dispose();
                }
                if (_AddFills_UltraFormManager_Dock_Area_Bottom != null)
                {
                    _AddFills_UltraFormManager_Dock_Area_Bottom.Dispose();
                }
                if (inboxControlStyler1 != null)
                {
                    inboxControlStyler1.Dispose();
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddFills));
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.lblClOrderID = new Infragistics.Win.Misc.UltraLabel();
            this.lblTargetQty = new Infragistics.Win.Misc.UltraLabel();
            this.lblExecQty = new Infragistics.Win.Misc.UltraLabel();
            this.lblAvgPrice = new Infragistics.Win.Misc.UltraLabel();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraPanel2 = new Infragistics.Win.Misc.UltraPanel();
            this.ultraFormManager1 = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._AddFills_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddFills_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddFills_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._AddFills_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            this.addFills1 = new Prana.Blotter.AddFillsControl();
            this.ultraPanel1.ClientArea.SuspendLayout();
            this.ultraPanel1.SuspendLayout();
            this.ultraPanel2.ClientArea.SuspendLayout();
            this.ultraPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance1.FontData.SizeInPoints = 10F;
            this.label1.Appearance = appearance1;
            this.label1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "ParentClOrderID:";
            this.label1.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance2.FontData.SizeInPoints = 10F;
            this.label2.Appearance = appearance2;
            this.label2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(230, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Target Qty:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance3.FontData.SizeInPoints = 10F;
            this.label3.Appearance = appearance3;
            this.label3.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(380, 3);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Executed Qty:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance4.FontData.SizeInPoints = 10F;
            this.label4.Appearance = appearance4;
            this.label4.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(540, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "AvgPrice:";
            // 
            // lblClOrderID
            // 
            this.lblClOrderID.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance5.FontData.SizeInPoints = 10F;
            this.lblClOrderID.Appearance = appearance5;
            this.lblClOrderID.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClOrderID.Location = new System.Drawing.Point(110, 3);
            this.lblClOrderID.Margin = new System.Windows.Forms.Padding(0);
            this.lblClOrderID.Name = "lblClOrderID";
            this.lblClOrderID.Size = new System.Drawing.Size(120, 20);
            this.lblClOrderID.TabIndex = 5;
            // 
            // lblTargetQty
            // 
            this.lblTargetQty.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance6.FontData.SizeInPoints = 10F;
            this.lblTargetQty.Appearance = appearance6;
            this.lblTargetQty.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTargetQty.Location = new System.Drawing.Point(300, 3);
            this.lblTargetQty.Margin = new System.Windows.Forms.Padding(0);
            this.lblTargetQty.Name = "lblTargetQty";
            this.lblTargetQty.Size = new System.Drawing.Size(80, 20);
            this.lblTargetQty.TabIndex = 6;
            // 
            // lblExecQty
            // 
            this.lblExecQty.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance7.FontData.SizeInPoints = 10F;
            this.lblExecQty.Appearance = appearance7;
            this.lblExecQty.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecQty.Location = new System.Drawing.Point(470, 3);
            this.lblExecQty.Margin = new System.Windows.Forms.Padding(0);
            this.lblExecQty.Name = "lblExecQty";
            this.lblExecQty.Size = new System.Drawing.Size(70, 20);
            this.lblExecQty.TabIndex = 7;
            // 
            // lblAvgPrice
            // 
            this.lblAvgPrice.Anchor = System.Windows.Forms.AnchorStyles.Top;
            appearance8.FontData.SizeInPoints = 10F;
            this.lblAvgPrice.Appearance = appearance8;
            this.lblAvgPrice.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgPrice.Location = new System.Drawing.Point(600, 3);
            this.lblAvgPrice.Margin = new System.Windows.Forms.Padding(0);
            this.lblAvgPrice.Name = "lblAvgPrice";
            this.lblAvgPrice.Size = new System.Drawing.Size(110, 20);
            this.lblAvgPrice.TabIndex = 8;
            // 
            // ultraPanel1
            // 
            // 
            // ultraPanel1.ClientArea
            // 
            this.ultraPanel1.ClientArea.Controls.Add(this.lblClOrderID);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblExecQty);
            this.ultraPanel1.ClientArea.Controls.Add(this.label4);
            this.ultraPanel1.ClientArea.Controls.Add(this.label3);
            this.ultraPanel1.ClientArea.Controls.Add(this.label1);
            this.ultraPanel1.ClientArea.Controls.Add(this.label2);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblTargetQty);
            this.ultraPanel1.ClientArea.Controls.Add(this.lblAvgPrice);
            this.ultraPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraPanel1.Location = new System.Drawing.Point(8, 29);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(634, 25);
            this.ultraPanel1.TabIndex = 9;
            // 
            // ultraPanel2
            // 
            // 
            // ultraPanel2.ClientArea
            // 
            this.ultraPanel2.ClientArea.Controls.Add(this.addFills1);
            this.ultraPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraPanel2.Location = new System.Drawing.Point(8, 54);
            this.ultraPanel2.Name = "ultraPanel2";
            this.ultraPanel2.Size = new System.Drawing.Size(634, 354);
            this.ultraPanel2.TabIndex = 10;
            // 
            // ultraFormManager1
            // 
            this.ultraFormManager1.Form = this;
            // 
            // _AddFills_UltraFormManager_Dock_Area_Left
            // 
            this._AddFills_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddFills_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddFills_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._AddFills_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddFills_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager1;
            this._AddFills_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 8;
            this._AddFills_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 29);
            this._AddFills_UltraFormManager_Dock_Area_Left.Name = "_AddFills_UltraFormManager_Dock_Area_Left";
            this._AddFills_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(8, 379);
            // 
            // _AddFills_UltraFormManager_Dock_Area_Right
            // 
            this._AddFills_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddFills_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddFills_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._AddFills_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddFills_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager1;
            this._AddFills_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 8;
            this._AddFills_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(642, 29);
            this._AddFills_UltraFormManager_Dock_Area_Right.Name = "_AddFills_UltraFormManager_Dock_Area_Right";
            this._AddFills_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(8, 379);
            // 
            // _AddFills_UltraFormManager_Dock_Area_Top
            // 
            this._AddFills_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddFills_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddFills_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._AddFills_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddFills_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager1;
            this._AddFills_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._AddFills_UltraFormManager_Dock_Area_Top.Name = "_AddFills_UltraFormManager_Dock_Area_Top";
            this._AddFills_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(650, 29);
            // 
            // _AddFills_UltraFormManager_Dock_Area_Bottom
            // 
            this._AddFills_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._AddFills_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._AddFills_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._AddFills_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._AddFills_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager1;
            this._AddFills_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 8;
            this._AddFills_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 408);
            this._AddFills_UltraFormManager_Dock_Area_Bottom.Name = "_AddFills_UltraFormManager_Dock_Area_Bottom";
            this._AddFills_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(650, 8);
            // 
            // addFills1
            // 
            this.addFills1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.addFills1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addFills1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.addFills1.Location = new System.Drawing.Point(0, 0);
            this.addFills1.Name = "addFills1";
            this.addFills1.Size = new System.Drawing.Size(634, 354);
            this.addFills1.TabIndex = 0;
            this.addFills1.Load += new System.EventHandler(this.addFills1_Load);
            // 
            // AddFills
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(650, 416);
            this.Controls.Add(this.ultraPanel2);
            this.Controls.Add(this.ultraPanel1);
            this.Controls.Add(this._AddFills_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._AddFills_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._AddFills_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._AddFills_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 200);
            this.Name = "AddFills";
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.Text = "Add/Modify Fills";
            this.Load += new System.EventHandler(this.AddFills_Load);
            this.ultraPanel1.ClientArea.ResumeLayout(false);
            this.ultraPanel1.ResumeLayout(false);
            this.ultraPanel2.ClientArea.ResumeLayout(false);
            this.ultraPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private void addFills1_Load(object sender, System.EventArgs e)
        {
            addFills1.BindData(_order);
            this.lblClOrderID.Text = _order.ParentClOrderID;
            this.lblTargetQty.Text = _order.Quantity.ToString();
            this.lblExecQty.Text = _order.CumQty.ToString();
            this.lblAvgPrice.Text = _order.AvgPrice.ToString();
        }

        private void AddFills_Load(object sender, System.EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(this.FindForm(), CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_BLOTTER_NEW);
                this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
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

        private void addFills1_ChangeAvgPrice(object sender, EventArgs e)
        {
            string[] sumAndPrice = ((string)sender).Split(',');
            this.lblExecQty.Text = sumAndPrice[0];
            this.lblAvgPrice.Text = sumAndPrice[1];
        }
        /// <summary>
        /// SaveManualFills 
        /// </summary>
        public event EventHandler SaveManualFills;
        private void addFills1_SendFillsToServer(object sender, EventArgs e)
        {
            try
            {
                Prana.BusinessObjects.OrderSingle or = (Prana.BusinessObjects.OrderSingle)sender;
                if (SaveManualFills != null)
                {
                    SaveManualFills(or, EventArgs.Empty);
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
    }
}
