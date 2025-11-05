namespace Prana.Admin.Controls.Company
{
    partial class UserSetup
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn7 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn8 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn9 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn10 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 4);
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
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand2 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencySymbol", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurencyID", 1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyTypeID", 2);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CurrencyName", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CounterPartyVenueID", 4);
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            this.lblCompanyUser = new System.Windows.Forms.Label();
            this.cmbClientName = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.cmbUserRole = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.lblUserRole = new System.Windows.Forms.Label();
            this.userPermission1 = new Prana.Admin.Controls.Company.UserPermission();
            this.userSetupDetails1 = new Prana.Admin.Controls.Company.UserSetupDetails();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserRole)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCompanyUser
            // 
            this.lblCompanyUser.AutoSize = true;
            this.lblCompanyUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompanyUser.Location = new System.Drawing.Point(12, 16);
            this.lblCompanyUser.Name = "lblCompanyUser";
            this.lblCompanyUser.Size = new System.Drawing.Size(83, 13);
            this.lblCompanyUser.TabIndex = 2;
            this.lblCompanyUser.Text = "Client Name :";
            this.lblCompanyUser.Visible = false;
            // 
            // cmbClientName
            // 
            this.cmbClientName.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbClientName.DisplayLayout.Appearance = appearance1;
            this.cmbClientName.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand1.ColHeadersVisible = false;
            ultraGridColumn6.Header.VisiblePosition = 0;
            ultraGridColumn7.Header.VisiblePosition = 1;
            ultraGridColumn7.Hidden = true;
            ultraGridColumn8.Header.VisiblePosition = 2;
            ultraGridColumn8.Hidden = true;
            ultraGridColumn9.Header.VisiblePosition = 3;
            ultraGridColumn9.Hidden = true;
            ultraGridColumn10.Header.VisiblePosition = 4;
            ultraGridColumn10.Hidden = true;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn6,
            ultraGridColumn7,
            ultraGridColumn8,
            ultraGridColumn9,
            ultraGridColumn10});
            this.cmbClientName.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.cmbClientName.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClientName.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientName.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientName.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.cmbClientName.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbClientName.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbClientName.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.cmbClientName.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbClientName.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbClientName.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbClientName.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.cmbClientName.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbClientName.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.cmbClientName.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbClientName.DisplayLayout.Override.CellAppearance = appearance8;
            this.cmbClientName.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbClientName.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbClientName.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.cmbClientName.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.cmbClientName.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbClientName.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.cmbClientName.DisplayLayout.Override.RowAppearance = appearance11;
            this.cmbClientName.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbClientName.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.cmbClientName.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbClientName.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbClientName.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbClientName.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbClientName.DropDownWidth = 0;
            this.cmbClientName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbClientName.Location = new System.Drawing.Point(131, 16);
            this.cmbClientName.MaxDropDownItems = 12;
            this.cmbClientName.Name = "cmbClientName";
            this.cmbClientName.Size = new System.Drawing.Size(135, 21);
            this.cmbClientName.TabIndex = 10;
            this.cmbClientName.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbClientName.Visible = false;
            // 
            // cmbUserRole
            // 
            this.cmbUserRole.DisplayLayout.AddNewBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance13.BackColor = System.Drawing.SystemColors.Window;
            appearance13.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.cmbUserRole.DisplayLayout.Appearance = appearance13;
            this.cmbUserRole.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ExtendLastColumn;
            ultraGridBand2.ColHeadersVisible = false;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Hidden = true;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Hidden = true;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridBand2.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5});
            this.cmbUserRole.DisplayLayout.BandsSerializer.Add(ultraGridBand2);
            this.cmbUserRole.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUserRole.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance14.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance14.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance14.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance14.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserRole.DisplayLayout.GroupByBox.Appearance = appearance14;
            appearance15.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserRole.DisplayLayout.GroupByBox.BandLabelAppearance = appearance15;
            this.cmbUserRole.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.cmbUserRole.DisplayLayout.GroupByBox.ButtonConnectorColor = System.Drawing.Color.LemonChiffon;
            appearance16.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance16.BackColor2 = System.Drawing.SystemColors.Control;
            appearance16.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance16.ForeColor = System.Drawing.SystemColors.GrayText;
            this.cmbUserRole.DisplayLayout.GroupByBox.PromptAppearance = appearance16;
            this.cmbUserRole.DisplayLayout.MaxColScrollRegions = 1;
            this.cmbUserRole.DisplayLayout.MaxRowScrollRegions = 1;
            appearance17.BackColor = System.Drawing.SystemColors.Window;
            appearance17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbUserRole.DisplayLayout.Override.ActiveCellAppearance = appearance17;
            appearance18.BackColor = System.Drawing.SystemColors.Highlight;
            appearance18.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.cmbUserRole.DisplayLayout.Override.ActiveRowAppearance = appearance18;
            this.cmbUserRole.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.cmbUserRole.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance19.BackColor = System.Drawing.SystemColors.Window;
            this.cmbUserRole.DisplayLayout.Override.CardAreaAppearance = appearance19;
            appearance20.BorderColor = System.Drawing.Color.Silver;
            appearance20.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.cmbUserRole.DisplayLayout.Override.CellAppearance = appearance20;
            this.cmbUserRole.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.cmbUserRole.DisplayLayout.Override.CellPadding = 0;
            appearance21.BackColor = System.Drawing.SystemColors.Control;
            appearance21.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance21.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance21.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance21.BorderColor = System.Drawing.SystemColors.Window;
            this.cmbUserRole.DisplayLayout.Override.GroupByRowAppearance = appearance21;
            appearance22.TextHAlignAsString = "Left";
            this.cmbUserRole.DisplayLayout.Override.HeaderAppearance = appearance22;
            this.cmbUserRole.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.cmbUserRole.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance23.BackColor = System.Drawing.SystemColors.Window;
            appearance23.BorderColor = System.Drawing.Color.Silver;
            this.cmbUserRole.DisplayLayout.Override.RowAppearance = appearance23;
            this.cmbUserRole.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance24.BackColor = System.Drawing.SystemColors.ControlLight;
            this.cmbUserRole.DisplayLayout.Override.TemplateAddRowAppearance = appearance24;
            this.cmbUserRole.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.cmbUserRole.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.cmbUserRole.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.cmbUserRole.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
            this.cmbUserRole.DropDownWidth = 0;
            this.cmbUserRole.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cmbUserRole.Location = new System.Drawing.Point(436, 16);
            this.cmbUserRole.MaxDropDownItems = 12;
            this.cmbUserRole.Name = "cmbUserRole";
            this.cmbUserRole.ReadOnly = true;
            this.cmbUserRole.Size = new System.Drawing.Size(135, 21);
            this.cmbUserRole.TabIndex = 3;
            this.cmbUserRole.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            this.cmbUserRole.Visible = false;
            // 
            // lblUserRole
            // 
            this.lblUserRole.AutoSize = true;
            this.lblUserRole.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserRole.Location = new System.Drawing.Point(305, 16);
            this.lblUserRole.Name = "lblUserRole";
            this.lblUserRole.Size = new System.Drawing.Size(41, 13);
            this.lblUserRole.TabIndex = 2;
            this.lblUserRole.Text = "Role :";
            this.lblUserRole.Visible = false;
            // 
            // userPermission1
            // 
            this.userPermission1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.userPermission1.Location = new System.Drawing.Point(8, 246);
            this.userPermission1.MinimumSize = new System.Drawing.Size(589, 270);
            this.userPermission1.Name = "userPermission1";
            this.userPermission1.Size = new System.Drawing.Size(589, 270);
            this.userPermission1.TabIndex = 1;
            // 
            // userSetupDetails1
            // 
            this.userSetupDetails1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.userSetupDetails1.Location = new System.Drawing.Point(8, 43);
            this.userSetupDetails1.MinimumSize = new System.Drawing.Size(589, 200);
            this.userSetupDetails1.Name = "userSetupDetails1";
            this.userSetupDetails1.Size = new System.Drawing.Size(610, 200);
            this.userSetupDetails1.TabIndex = 0;
            // 
            // UserSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.cmbUserRole);
            this.Controls.Add(this.cmbClientName);
            this.Controls.Add(this.lblUserRole);
            this.Controls.Add(this.lblCompanyUser);
            this.Controls.Add(this.userPermission1);
            this.Controls.Add(this.userSetupDetails1);
            this.MinimumSize = new System.Drawing.Size(607, 530);
            this.Name = "UserSetup";
            this.Size = new System.Drawing.Size(607, 530);
            ((System.ComponentModel.ISupportInitialize)(this.cmbClientName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUserRole)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UserSetupDetails userSetupDetails1;
        private UserPermission userPermission1;
        private System.Windows.Forms.Label lblCompanyUser;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbClientName;
        private Infragistics.Win.UltraWinGrid.UltraCombo cmbUserRole;
        private System.Windows.Forms.Label lblUserRole;
    }
}
