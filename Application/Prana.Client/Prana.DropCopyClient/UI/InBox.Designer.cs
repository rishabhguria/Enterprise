namespace Prana.DropCopyClient
{
    partial class InBox
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
            if (headerCheckBox != null)
            {
                headerCheckBox.Dispose();
                headerCheckBox = null;
            }
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            this.pnlLowerBtns = new System.Windows.Forms.Panel();
            this.btnReject = new System.Windows.Forms.Button();
            this.btnAccept = new System.Windows.Forms.Button();
            this.pnlBasket = new System.Windows.Forms.Panel();
            this.cmbBoxTradingAccount = new Infragistics.Win.UltraWinGrid.UltraDropDown();
            this.grdBasket = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlLowerBtns.SuspendLayout();
            this.pnlBasket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBoxTradingAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasket)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlLowerBtns
            // 
            this.pnlLowerBtns.BackColor = System.Drawing.Color.Black;
            this.pnlLowerBtns.Controls.Add(this.btnReject);
            this.pnlLowerBtns.Controls.Add(this.btnAccept);
            this.pnlLowerBtns.Location = new System.Drawing.Point(0, 279);
            this.pnlLowerBtns.Name = "pnlLowerBtns";
            this.pnlLowerBtns.Size = new System.Drawing.Size(788, 30);
            this.pnlLowerBtns.TabIndex = 1;
            // 
            // btnReject
            // 
            this.btnReject.BackColor = System.Drawing.Color.Red;
            this.btnReject.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnReject.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReject.Location = new System.Drawing.Point(701, 3);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(75, 23);
            this.btnReject.TabIndex = 0;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = false;
            this.btnReject.Click += new System.EventHandler(this.btnReject_Click);
            // 
            // btnAccept
            // 
            this.btnAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAccept.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAccept.Location = new System.Drawing.Point(12, 3);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(75, 23);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "Accept";
            this.btnAccept.UseVisualStyleBackColor = false;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // pnlBasket
            // 
            this.pnlBasket.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlBasket.AutoScroll = true;
            this.pnlBasket.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlBasket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBasket.Controls.Add(this.cmbBoxTradingAccount);
            this.pnlBasket.Controls.Add(this.grdBasket);
            this.pnlBasket.Location = new System.Drawing.Point(0, 0);
            this.pnlBasket.MinimumSize = new System.Drawing.Size(500, 200);
            this.pnlBasket.Name = "pnlBasket";
            this.pnlBasket.Size = new System.Drawing.Size(790, 277);
            this.pnlBasket.TabIndex = 96;
            // 
            // cmbBoxTradingAccount
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbBoxTradingAccount.DisplayLayout.Appearance = appearance1;
            this.cmbBoxTradingAccount.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbBoxTradingAccount.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBoxTradingAccount.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBoxTradingAccount.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbBoxTradingAccount.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbBoxTradingAccount.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbBoxTradingAccount.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbBoxTradingAccount.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbBoxTradingAccount.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbBoxTradingAccount.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbBoxTradingAccount.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbBoxTradingAccount.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbBoxTradingAccount.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbBoxTradingAccount.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbBoxTradingAccount.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbBoxTradingAccount.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbBoxTradingAccount.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbBoxTradingAccount.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbBoxTradingAccount.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbBoxTradingAccount.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbBoxTradingAccount.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbBoxTradingAccount.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbBoxTradingAccount.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbBoxTradingAccount.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbBoxTradingAccount.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbBoxTradingAccount.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbBoxTradingAccount.Location = new System.Drawing.Point(3, 177);
            this.cmbBoxTradingAccount.Name = "cmbBoxTradingAccount";
            this.cmbBoxTradingAccount.Size = new System.Drawing.Size(108, 31);
            this.cmbBoxTradingAccount.TabIndex = 16;
            this.cmbBoxTradingAccount.Text = "ultraDropDown1";
            this.cmbBoxTradingAccount.Visible = false;
            // 
            // grdBasket
            // 
            this.grdBasket.DisplayLayout.AddNewBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance13.BackColor = System.Drawing.Color.Black;
            appearance13.BackColor2 = System.Drawing.Color.Black;
            appearance13.BorderColor = System.Drawing.Color.Black;
            appearance13.FontData.BoldAsString = "True";
            appearance13.FontData.Name = "Tahoma";
            appearance13.FontData.SizeInPoints = 8.25F;
            this.grdBasket.DisplayLayout.Appearance = appearance13;
            this.grdBasket.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this.grdBasket.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            appearance14.BackColor = System.Drawing.Color.White;
            this.grdBasket.DisplayLayout.CaptionAppearance = appearance14;
            this.grdBasket.DisplayLayout.ColumnChooserEnabled = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.GroupByBox.Hidden = true;
            this.grdBasket.DisplayLayout.MaxColScrollRegions = 1;
            this.grdBasket.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdBasket.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinBand;
            this.grdBasket.DisplayLayout.Override.AllowColSizing = Infragistics.Win.UltraWinGrid.AllowColSizing.Synchronized;
            this.grdBasket.DisplayLayout.Override.AllowColSwapping = Infragistics.Win.UltraWinGrid.AllowColSwapping.NotAllowed;
            this.grdBasket.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
            this.grdBasket.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdBasket.DisplayLayout.Override.ColumnSizingArea = Infragistics.Win.UltraWinGrid.ColumnSizingArea.CellsOnly;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            appearance15.FontData.BoldAsString = "True";
            appearance15.FontData.Name = "Tahoma";
            appearance15.FontData.SizeInPoints = 8.25F;
            this.grdBasket.DisplayLayout.Override.HeaderAppearance = appearance15;
            this.grdBasket.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdBasket.DisplayLayout.Override.HeaderPlacement = Infragistics.Win.UltraWinGrid.HeaderPlacement.FixedOnTop;
            this.grdBasket.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.XPThemed;
            this.grdBasket.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this.grdBasket.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdBasket.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdBasket.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdBasket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdBasket.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.grdBasket.Location = new System.Drawing.Point(0, 0);
            this.grdBasket.Name = "grdBasket";
            this.grdBasket.Size = new System.Drawing.Size(788, 275);
            this.grdBasket.TabIndex = 15;
            this.grdBasket.UpdateMode = Infragistics.Win.UltraWinGrid.UpdateMode.OnUpdate;
            this.grdBasket.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // InBox
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(788, 309);
            this.Controls.Add(this.pnlBasket);
            this.Controls.Add(this.pnlLowerBtns);
            this.MaximizeBox = false;
            this.Name = "InBox";
            this.Text = "InBox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InBox_FormClosing);
            this.pnlLowerBtns.ResumeLayout(false);
            this.pnlBasket.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBoxTradingAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasket)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLowerBtns;
        private System.Windows.Forms.Button btnReject;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Panel pnlBasket;
        private  Infragistics.Win.UltraWinGrid.UltraGrid grdBasket;
        private Infragistics.Win.UltraWinGrid.UltraDropDown cmbBoxTradingAccount;
        private System.Windows.Forms.Timer timerRefresh;
    }
}

