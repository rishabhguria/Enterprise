using Infragistics.Win;
namespace Prana.Admin.Controls
{
    partial class ctrlRevaluationPreference
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
            if (disposing)
            {
                if(components != null)
                   components.Dispose();

                if(_dtSource != null)
                    _dtSource.Dispose();
            }
            if(_vlOperationMode!=null)
            {
                _vlOperationMode.Dispose();
            }
            if (_vlOperationModeDropDown != null)
            {
                _vlOperationModeDropDown.Dispose();
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
            this.grdRevaluationPref = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.dailyProcessDaysNumericEditor = new System.Windows.Forms.NumericUpDown();
            this.lblTNTextboxMessage = new Infragistics.Win.Misc.UltraLabel();
            this.lblTN = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdRevaluationPref)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dailyProcessDaysNumericEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // grdRevaluationPref
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdRevaluationPref.DisplayLayout.Appearance = appearance1;
            this.grdRevaluationPref.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdRevaluationPref.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdRevaluationPref.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdRevaluationPref.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdRevaluationPref.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdRevaluationPref.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdRevaluationPref.DisplayLayout.MaxColScrollRegions = 1;
            this.grdRevaluationPref.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdRevaluationPref.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdRevaluationPref.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdRevaluationPref.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdRevaluationPref.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdRevaluationPref.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdRevaluationPref.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdRevaluationPref.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdRevaluationPref.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdRevaluationPref.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdRevaluationPref.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdRevaluationPref.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdRevaluationPref.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdRevaluationPref.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdRevaluationPref.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdRevaluationPref.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdRevaluationPref.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdRevaluationPref.SyncWithCurrencyManager = false;
            this.grdRevaluationPref.DisplayLayout.Override.RowSelectors = DefaultableBoolean.False;
            this.grdRevaluationPref.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None;
            this.grdRevaluationPref.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdRevaluationPref.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdRevaluationPref.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdRevaluationPref.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.Horizontal;
            this.grdRevaluationPref.Location = new System.Drawing.Point(3, 3);
            this.grdRevaluationPref.Name = "grdRevaluationPref";
            this.grdRevaluationPref.Size = new System.Drawing.Size(510, 146);
            this.grdRevaluationPref.TabIndex = 0;
            this.grdRevaluationPref.Text = "ultraGrid1";
            this.grdRevaluationPref.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdRevaluationPref_InitializeLayout);
            this.grdRevaluationPref.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdRevaluationPref_InitializeRow);
            this.grdRevaluationPref.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdRevaluationPref_CellChange);
            this.grdRevaluationPref.BeforeSortChange += new Infragistics.Win.UltraWinGrid.BeforeSortChangeEventHandler(this.grdRevaluationPref_BeforeSortChange);
            // 
            // numericUpDown1
            // 
            this.dailyProcessDaysNumericEditor.Location = new System.Drawing.Point(245, 188);
            this.dailyProcessDaysNumericEditor.Name = "numericUpDown1";
            this.dailyProcessDaysNumericEditor.Size = new System.Drawing.Size(54, 20);
            this.dailyProcessDaysNumericEditor.TabIndex = 2;
            // 
            // lblTNTextboxMessage
            // 
           this.lblTNTextboxMessage.Location = new System.Drawing.Point(-2, 163);
            this.lblTNTextboxMessage.Name = "lblTNTextboxMessage";
            this.lblTNTextboxMessage.Size = new System.Drawing.Size(523, 23);
            this.lblTNTextboxMessage.TabIndex = 3;
            this.lblTNTextboxMessage.AutoSize = true;
            this.lblTNTextboxMessage.Text = "The revaluation process started for back dated enteries for the account in daily " +
    "process mode will run till";
            // 
            // lblTN
            // 
            this.lblTN.Location = new System.Drawing.Point(224, 191);
            this.lblTN.Name = "lblTN";
            this.lblTN.Size = new System.Drawing.Size(20, 18);
            this.lblTN.TabIndex = 4;
            this.lblTN.Text = "T - ";
            // 
            // ctrlRevaluationPreference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTN);
            this.Controls.Add(this.lblTNTextboxMessage);
            this.Controls.Add(this.dailyProcessDaysNumericEditor);
            this.Controls.Add(this.grdRevaluationPref);
            this.Name = "ctrlRevaluationPreference";
            this.Size = new System.Drawing.Size(516, 218);
            this.Load += new System.EventHandler(this.CtrlRevaluationPreference_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdRevaluationPref)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dailyProcessDaysNumericEditor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinGrid.UltraGrid grdRevaluationPref;
        private System.Windows.Forms.NumericUpDown dailyProcessDaysNumericEditor;
        private Infragistics.Win.Misc.UltraLabel lblTNTextboxMessage;
        private Infragistics.Win.Misc.UltraLabel lblTN;
    }
}
