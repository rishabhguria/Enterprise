namespace Prana.Utilities.UI.UIUtilities
{
    partial class CustomColumnDialog
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
                if (components != null)
                {
                    components.Dispose();
                }
                if (band != null)
                {
                    band.Dispose();
                }
                if (createdColumn != null)
                {
                    createdColumn.Dispose();
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
            Infragistics.Win.UltraWinEditors.EditorButton editorButton1 = new Infragistics.Win.UltraWinEditors.EditorButton();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraLabelFormula = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTextEditorFormula = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraButtonCancel = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonOk = new Infragistics.Win.Misc.UltraButton();
            this.ultraComboEditorType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTextEditorName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabelSummaries = new Infragistics.Win.Misc.UltraLabel();
            this.cmbsummaries = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.chkAddAll = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorFormula)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsummaries)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabelFormula
            // 
            this.ultraLabelFormula.Location = new System.Drawing.Point(6, 52);
            this.ultraLabelFormula.Name = "ultraLabelFormula";
            this.ultraLabelFormula.Size = new System.Drawing.Size(100, 23);
            this.ultraLabelFormula.TabIndex = 15;
            this.ultraLabelFormula.Text = "Formula (optional)";
            // 
            // ultraTextEditorFormula
            // 
            appearance1.FontData.BoldAsString = "True";
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            editorButton1.Appearance = appearance1;
            editorButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button;
            editorButton1.Text = "...";
            this.ultraTextEditorFormula.ButtonsRight.Add(editorButton1);
            this.ultraTextEditorFormula.Location = new System.Drawing.Point(134, 52);
            this.ultraTextEditorFormula.Name = "ultraTextEditorFormula";
            this.ultraTextEditorFormula.Size = new System.Drawing.Size(144, 21);
            this.ultraTextEditorFormula.TabIndex = 14;
            // 
            // ultraButtonCancel
            // 
            this.ultraButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButtonCancel.Location = new System.Drawing.Point(134, 132);
            this.ultraButtonCancel.Name = "ultraButtonCancel";
            this.ultraButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonCancel.TabIndex = 13;
            this.ultraButtonCancel.Text = "&Cancel";
            this.ultraButtonCancel.Click += new System.EventHandler(this.ultraButtonCancel_Click);
            // 
            // ultraButtonOk
            // 
            this.ultraButtonOk.Location = new System.Drawing.Point(46, 132);
            this.ultraButtonOk.Name = "ultraButtonOk";
            this.ultraButtonOk.Size = new System.Drawing.Size(75, 23);
            this.ultraButtonOk.TabIndex = 12;
            this.ultraButtonOk.Text = "&Ok";
            this.ultraButtonOk.Click += new System.EventHandler(this.ultraButtonOk_Click);
            // 
            // ultraComboEditorType
            // 
            this.ultraComboEditorType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.ultraComboEditorType.Location = new System.Drawing.Point(134, 28);
            this.ultraComboEditorType.Name = "ultraComboEditorType";
            this.ultraComboEditorType.Size = new System.Drawing.Size(144, 21);
            this.ultraComboEditorType.TabIndex = 11;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(6, 28);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 10;
            this.ultraLabel2.Text = "Type";
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(6, 4);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 9;
            this.ultraLabel1.Text = "Name";
            // 
            // ultraTextEditorName
            // 
            this.ultraTextEditorName.Location = new System.Drawing.Point(134, 4);
            this.ultraTextEditorName.MaxLength = 50;
            this.ultraTextEditorName.Name = "ultraTextEditorName";
            this.ultraTextEditorName.Size = new System.Drawing.Size(144, 21);
            this.ultraTextEditorName.TabIndex = 8;
            // 
            // ultraLabelSummaries
            // 
            this.ultraLabelSummaries.Location = new System.Drawing.Point(4, 73);
            this.ultraLabelSummaries.Name = "ultraLabelSummaries";
            this.ultraLabelSummaries.Size = new System.Drawing.Size(117, 23);
            this.ultraLabelSummaries.TabIndex = 17;
            this.ultraLabelSummaries.Text = "Summaries (optional)";
            // 
            // cmbsummaries
            // 
            this.cmbsummaries.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this.cmbsummaries.Location = new System.Drawing.Point(134, 75);
            this.cmbsummaries.Name = "cmbsummaries";
            this.cmbsummaries.Size = new System.Drawing.Size(144, 21);
            this.cmbsummaries.TabIndex = 16;
            // 
            // chkAddAll
            // 
            this.chkAddAll.Location = new System.Drawing.Point(134, 106);
            this.chkAddAll.Name = "chkAddAll";
            this.chkAddAll.Size = new System.Drawing.Size(142, 20);
            this.chkAddAll.TabIndex = 18;
            this.chkAddAll.Text = "Add To All Grids";
            this.chkAddAll.CheckedChanged += new System.EventHandler(this.chkAddAll_CheckedChanged);
            // 
            // CustomColumnDialog
            // 
            this.AcceptButton = this.ultraButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.ultraButtonCancel;
            this.ClientSize = new System.Drawing.Size(282, 167);
            this.Controls.Add(this.chkAddAll);
            this.Controls.Add(this.ultraLabelSummaries);
            this.Controls.Add(this.cmbsummaries);
            this.Controls.Add(this.ultraLabelFormula);
            this.Controls.Add(this.ultraTextEditorFormula);
            this.Controls.Add(this.ultraButtonCancel);
            this.Controls.Add(this.ultraButtonOk);
            this.Controls.Add(this.ultraComboEditorType);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraTextEditorName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CustomColumnDialog";
            this.Text = "New Column";
            this.Load += new System.EventHandler(this.CustomColumnDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorFormula)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraComboEditorType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTextEditorName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbsummaries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabelFormula;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorFormula;
        private Infragistics.Win.Misc.UltraButton ultraButtonCancel;
        private Infragistics.Win.Misc.UltraButton ultraButtonOk;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor ultraComboEditorType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor ultraTextEditorName;
        private Infragistics.Win.Misc.UltraLabel ultraLabelSummaries;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cmbsummaries;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkAddAll;
    }
}