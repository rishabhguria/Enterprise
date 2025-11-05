using Prana.BusinessObjects;
namespace Prana.TradingTicket
{
    /// <summary>
    /// Summary description for ConfirmationPopUp.
    /// </summary>
    public class ConfirmationPopUpUserControl : System.Windows.Forms.UserControl
    {
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private Infragistics.Win.Misc.UltraLabel label4;
        private Infragistics.Win.Misc.UltraLabel label3;
        private Infragistics.Win.Misc.UltraLabel label25;
        private Infragistics.Win.Misc.UltraLabel otherPopTitleLabel;
        private Infragistics.Win.Misc.UltraLabel manualOrderPrifLabel;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoNewOrder;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoCXL;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoCXLReplace;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet uoManualOrderPrif;
        private ConfirmationPopUp _confirmationPopUp;

        public ConfirmationPopUpUserControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();


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
                if (uoManualOrderPrif != null)
                {
                    uoManualOrderPrif.Dispose();
                }
                if (uoCXLReplace != null)
                {
                    uoCXLReplace.Dispose();
                }
                if (uoCXL != null)
                {
                    uoCXL.Dispose();
                }
                if (uoNewOrder != null)
                {
                    uoNewOrder.Dispose();
                }
                if (manualOrderPrifLabel != null)
                {
                    manualOrderPrifLabel.Dispose();
                }
                if (otherPopTitleLabel != null)
                {
                    otherPopTitleLabel.Dispose();
                }
                if (label25 != null)
                {
                    label25.Dispose();
                }
                if (label3 != null)
                {
                    label3.Dispose();
                }
                if (label4 != null)
                {
                    label4.Dispose();
                }
                if (ultraGroupBox1 != null)
                {
                    ultraGroupBox1.Dispose();
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.label4 = new Infragistics.Win.Misc.UltraLabel();
            this.label3 = new Infragistics.Win.Misc.UltraLabel();
            this.label25 = new Infragistics.Win.Misc.UltraLabel();
            this.otherPopTitleLabel = new Infragistics.Win.Misc.UltraLabel();
            this.manualOrderPrifLabel = new Infragistics.Win.Misc.UltraLabel();
            this.uoNewOrder = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.uoCXL = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.uoCXLReplace = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.uoManualOrderPrif = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uoNewOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoCXL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoCXLReplace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoManualOrderPrif)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this.uoCXLReplace);
            this.ultraGroupBox1.Controls.Add(this.uoCXL);
            this.ultraGroupBox1.Controls.Add(this.uoNewOrder);
            this.ultraGroupBox1.Controls.Add(this.label4);
            this.ultraGroupBox1.Controls.Add(this.label3);
            this.ultraGroupBox1.Controls.Add(this.label25);
            this.ultraGroupBox1.Controls.Add(this.otherPopTitleLabel);
            this.ultraGroupBox1.Controls.Add(this.manualOrderPrifLabel);
            this.ultraGroupBox1.Controls.Add(this.uoManualOrderPrif);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ultraGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(496, 160);
            this.ultraGroupBox1.TabIndex = 32;
            this.ultraGroupBox1.Text = "Confirmation Pop ups";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 16);
            this.label4.TabIndex = 30;
            this.label4.Text = "CXL/Replace";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 16);
            this.label3.TabIndex = 29;
            this.label3.Text = "CXL";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(6, 29);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(78, 16);
            this.label25.TabIndex = 5;
            this.label25.Text = "New Order";
            // 
            // otherPopTitleLabel
            // 
            this.otherPopTitleLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.otherPopTitleLabel.Location = new System.Drawing.Point(6, 112);
            this.otherPopTitleLabel.Name = "label25";
            this.otherPopTitleLabel.Size = new System.Drawing.Size(200, 16);
            this.otherPopTitleLabel.TabIndex = 5;
            this.otherPopTitleLabel.Text = "Other pop-ups preferences";
            // 
            // manualOrderPrifLabel
            // 
            this.manualOrderPrifLabel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.manualOrderPrifLabel.Location = new System.Drawing.Point(6, 137);
            this.manualOrderPrifLabel.Name = "label25";
            this.manualOrderPrifLabel.Size = new System.Drawing.Size(200, 16);
            this.manualOrderPrifLabel.TabIndex = 5;
            this.manualOrderPrifLabel.Text = "Confirm/Edit Fills for Manual Orders";
            // 
            // uoNewOrder
            // 
            this.uoNewOrder.CheckedIndex = 0;
            valueListItem1.DataValue = "Yes";
            valueListItem1.DisplayText = "Yes";
            valueListItem2.DataValue = "No";
            valueListItem2.DisplayText = "No";
            this.uoNewOrder.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.uoNewOrder.Location = new System.Drawing.Point(100, 29);
            this.uoNewOrder.Name = "uoNewOrder";
            this.uoNewOrder.Size = new System.Drawing.Size(96, 22);
            this.uoNewOrder.TabIndex = 33;
            this.uoNewOrder.Text = "Yes";
            // 
            // uoCXL
            // 
            this.uoCXL.CheckedIndex = 0;
            valueListItem3.DataValue = "Yes";
            valueListItem3.DisplayText = "Yes";
            valueListItem4.DataValue = "No";
            valueListItem4.DisplayText = "No";
            this.uoCXL.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.uoCXL.Location = new System.Drawing.Point(98, 57);
            this.uoCXL.Name = "uoCXL";
            this.uoCXL.Size = new System.Drawing.Size(96, 22);
            this.uoCXL.TabIndex = 34;
            this.uoCXL.Text = "Yes";
            // 
            // uoCXLReplace
            // 
            this.uoCXLReplace.CheckedIndex = 0;
            valueListItem5.DataValue = "Yes";
            valueListItem5.DisplayText = "Yes";
            valueListItem6.DataValue = "No";
            valueListItem6.DisplayText = "No";
            this.uoCXLReplace.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6});
            this.uoCXLReplace.Location = new System.Drawing.Point(98, 84);
            this.uoCXLReplace.Name = "uoCXLReplace";
            this.uoCXLReplace.Size = new System.Drawing.Size(96, 25);
            this.uoCXLReplace.TabIndex = 35;
            this.uoCXLReplace.Text = "Yes";
            // 
            // uoManualOrderPrif
            // 
            this.uoManualOrderPrif.CheckedIndex = 0;
            valueListItem7.DataValue = "Yes";
            valueListItem7.DisplayText = "Yes";
            valueListItem8.DataValue = "No";
            valueListItem8.DisplayText = "No";
            this.uoManualOrderPrif.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem7,
            valueListItem8});
            this.uoManualOrderPrif.Location = new System.Drawing.Point(210, 137);
            this.uoManualOrderPrif.Name = "uoManualOrderPrif";
            this.uoManualOrderPrif.Size = new System.Drawing.Size(96, 25);
            this.uoManualOrderPrif.TabIndex = 35;
            this.uoManualOrderPrif.Text = "Yes";
            // 
            // ConfirmationPopUpUserControl
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "ConfirmationPopUpUserControl";
            this.Size = new System.Drawing.Size(496, 170);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uoNewOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoCXL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoCXLReplace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uoManualOrderPrif)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        public void SetConfirmation(ConfirmationPopUp confirmationPopUp)
        {
            _confirmationPopUp = confirmationPopUp;
            if (_confirmationPopUp.ISNewOrder)
            {
                uoNewOrder.CheckedIndex = 0;
            }
            else
            {
                uoNewOrder.CheckedIndex = 1;
            }
            if (_confirmationPopUp.ISCXL)
            {
                uoCXL.CheckedIndex = 0;
            }
            else
            {
                uoCXL.CheckedIndex = 1;
            }
            if (_confirmationPopUp.ISCXLReplace)
            {
                uoCXLReplace.CheckedIndex = 0;

            }
            else
            {
                uoCXLReplace.CheckedIndex = 1;
            }
            if (_confirmationPopUp.IsManualOrder)
                uoManualOrderPrif.CheckedIndex = 0;
            else
                uoManualOrderPrif.CheckedIndex = 1;
        }
        public ConfirmationPopUp GetConfirmation()
        {
            _confirmationPopUp = new ConfirmationPopUp();
            _confirmationPopUp.ISNewOrder = uoNewOrder.CheckedIndex == 0 ? true : false;
            _confirmationPopUp.ISCXL = uoCXL.CheckedIndex == 0 ? true : false;
            _confirmationPopUp.ISCXLReplace = uoCXLReplace.CheckedIndex == 0 ? true : false;
            _confirmationPopUp.IsManualOrder = uoManualOrderPrif.CheckedIndex == 0 ? true : false;
            TradeManager.TradeManager.GetNewValidationPopUpSettings(_confirmationPopUp);
            return _confirmationPopUp;
        }
    }
}
