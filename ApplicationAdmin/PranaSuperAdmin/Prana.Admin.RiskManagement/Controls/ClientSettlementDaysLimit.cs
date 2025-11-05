using System.Drawing;

namespace Prana.Admin.RiskManagement.Controls
{
    /// <summary>
    /// Summary description for ClientSettlementDaysLimit.
    /// </summary>
    public class uctClientSettlementDaysLimit : System.Windows.Forms.UserControl
    {
        private System.Windows.Forms.Label lblSettlementDaysLimit;
        private System.Windows.Forms.TextBox txtSettlementDaysLimit;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditor1;
        private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo1;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public uctClientSettlementDaysLimit()
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
                if (lblSettlementDaysLimit != null)
                {
                    lblSettlementDaysLimit.Dispose();
                }
                if (txtSettlementDaysLimit != null)
                {
                    txtSettlementDaysLimit.Dispose();
                }
                if (errorProvider1 != null)
                {
                    errorProvider1.Dispose();
                }
                if (ultraComboEditor1 != null)
                {
                    ultraComboEditor1.Dispose();
                }
                if (ultraCombo1 != null)
                {
                    ultraCombo1.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
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
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            this.lblSettlementDaysLimit = new System.Windows.Forms.Label();
            this.txtSettlementDaysLimit = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
            this.ultraComboEditor1 = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraCombo1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSettlementDaysLimit
            // 
            this.lblSettlementDaysLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSettlementDaysLimit.AutoSize = true;
            this.lblSettlementDaysLimit.Location = new System.Drawing.Point(16, 10);
            this.lblSettlementDaysLimit.Name = "lblSettlementDaysLimit";
            this.lblSettlementDaysLimit.Size = new System.Drawing.Size(109, 17);
            this.lblSettlementDaysLimit.TabIndex = 0;
            this.lblSettlementDaysLimit.Text = "Settlement days limit";
            // 
            // txtSettlementDaysLimit
            // 
            this.txtSettlementDaysLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSettlementDaysLimit.Location = new System.Drawing.Point(138, 8);
            this.txtSettlementDaysLimit.Name = "txtSettlementDaysLimit";
            this.txtSettlementDaysLimit.Size = new System.Drawing.Size(138, 20);
            this.txtSettlementDaysLimit.TabIndex = 1;
            this.txtSettlementDaysLimit.Text = "";
            this.txtSettlementDaysLimit.LostFocus += new System.EventHandler(this.txtSettlementDaysLimit_LostFocus);
            this.txtSettlementDaysLimit.GotFocus += new System.EventHandler(this.txtSettlementDaysLimit_GotFocus);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ultraComboEditor1
            // 
            this.ultraComboEditor1.Location = new System.Drawing.Point(82, 52);
            this.ultraComboEditor1.Name = "ultraComboEditor1";
            this.ultraComboEditor1.Size = new System.Drawing.Size(188, 22);
            this.ultraComboEditor1.TabIndex = 2;
            // 
            // ultraCombo1
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraCombo1.Appearance = appearance1;
            this.ultraCombo1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            appearance2.BackColor = System.Drawing.SystemColors.Window;
            appearance2.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.ultraCombo1.DisplayLayout.Appearance = appearance2;
            this.ultraCombo1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraCombo1.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.GroupByBox.Appearance = appearance3;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraCombo1.DisplayLayout.GroupByBox.BandLabelAppearance = appearance4;
            this.ultraCombo1.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance5.BackColor2 = System.Drawing.SystemColors.Control;
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance5.ForeColor = System.Drawing.SystemColors.GrayText;
            this.ultraCombo1.DisplayLayout.GroupByBox.PromptAppearance = appearance5;
            this.ultraCombo1.DisplayLayout.MaxColScrollRegions = 1;
            this.ultraCombo1.DisplayLayout.MaxRowScrollRegions = 1;
            appearance6.BackColor = System.Drawing.SystemColors.Window;
            appearance6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ultraCombo1.DisplayLayout.Override.ActiveCellAppearance = appearance6;
            appearance7.BackColor = System.Drawing.SystemColors.Highlight;
            appearance7.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.ultraCombo1.DisplayLayout.Override.ActiveRowAppearance = appearance7;
            this.ultraCombo1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.ultraCombo1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance8.BackColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BorderColor = System.Drawing.Color.Silver;
            appearance9.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.ultraCombo1.DisplayLayout.Override.CellAppearance = appearance9;
            this.ultraCombo1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.ultraCombo1.DisplayLayout.Override.CellPadding = 0;
            appearance10.BackColor = System.Drawing.SystemColors.Control;
            appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance10.BorderColor = System.Drawing.SystemColors.Window;
            this.ultraCombo1.DisplayLayout.Override.GroupByRowAppearance = appearance10;
            appearance11.TextHAlign = Infragistics.Win.HAlign.Left;
            this.ultraCombo1.DisplayLayout.Override.HeaderAppearance = appearance11;
            this.ultraCombo1.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.ultraCombo1.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance12.BackColor = System.Drawing.SystemColors.Window;
            appearance12.BorderColor = System.Drawing.Color.Silver;
            this.ultraCombo1.DisplayLayout.Override.RowAppearance = appearance12;
            this.ultraCombo1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance13.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ultraCombo1.DisplayLayout.Override.TemplateAddRowAppearance = appearance13;
            this.ultraCombo1.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.ultraCombo1.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.ultraCombo1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.ultraCombo1.DisplayMember = "";
            this.ultraCombo1.Enabled = false;
            this.ultraCombo1.Location = new System.Drawing.Point(110, 98);
            this.ultraCombo1.Name = "ultraCombo1";
            this.ultraCombo1.Size = new System.Drawing.Size(140, 22);
            this.ultraCombo1.TabIndex = 3;
            this.ultraCombo1.Text = "ultraCombo1";
            this.ultraCombo1.ValueMember = "";
            // 
            // uctClientSettlementDaysLimit
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
            this.Controls.Add(this.ultraCombo1);
            this.Controls.Add(this.ultraComboEditor1);
            this.Controls.Add(this.txtSettlementDaysLimit);
            this.Controls.Add(this.lblSettlementDaysLimit);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "uctClientSettlementDaysLimit";
            this.Size = new System.Drawing.Size(292, 132);
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        #region BindALLAUEC
        //		private void BindALLAUEC()
        //		{
        //			AUECs auecs = AUECManager.GetAUEC();
        //
        //			System.Data.DataTable dtauec = new System.Data.DataTable();
        //			dtauec.Columns.Add("Data");
        //			dtauec.Columns.Add("Value");
        //			object[] row = new object[2]; 
        //			row[0] = C_COMBO_SELECT;
        //			row[1] = int.MinValue;
        //			dtauec.Rows.Add(row);
        //
        //			if (auecs.Count > 0 )
        //			{
        //				foreach(AUEC auec in auecs)
        //				{
        //					Currency currency = new Currency();
        //					currency = AUECManager.GetCurrency(auec.Compliance.BaseCurrencyID);
        //				
        //					string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();
        //					int Value = auec.AUECID;
        //					
        //					row[0] = Data;
        //					row[1] = Value;
        //					dtauec.Rows.Add(row);
        //				}
        //
        //				ultraCombo1.DataSource = dtauec;
        //				//auecs.Insert(0, new AUEC(int.MinValue, C_COMBO_SELECT));
        //				ultraCombo1.DisplayMember = "Data";
        //				ultraCombo1.ValueMember = "Value";
        //				this.ultraCombo1.Appearance = CheckBox
        //			}
        //			ColumnsCollection columns5 = ultraCombo1.DisplayLayout.Bands[0].Columns;
        //			foreach (UltraGridColumn column in columns5)
        //			{
        //				if(column.Key != "Data")
        //				{
        //					column.Hidden = true;
        //				}
        //			}
        //			
        //
        //
        //		}


        #endregion

        #region Focus Color
        private void txtSettlementDaysLimit_GotFocus(object sender, System.EventArgs e)
        {
            txtSettlementDaysLimit.BackColor = Color.FromArgb(255, 250, 205);
        }
        private void txtSettlementDaysLimit_LostFocus(object sender, System.EventArgs e)
        {
            txtSettlementDaysLimit.BackColor = Color.White;
        }
        #endregion

    }
}
