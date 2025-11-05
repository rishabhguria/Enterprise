namespace Prana.Utilities.UI.UIUtilities
{
    partial class MultiSelectDropDown
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

            if (DropDownEditorButton1 != null)
                DropDownEditorButton1.Dispose();
            if (MultiSelectEditor != null)
                MultiSelectEditor.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DropDownEditorButton1 = new Infragistics.Win.UltraWinEditors.DropDownEditorButton();
            this.checkedMultipleItems = new System.Windows.Forms.CheckedListBox();
            this.ultraPanel1 = new Infragistics.Win.Misc.UltraPanel();
            this.MultiSelectEditor = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MultiSelectEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedMultipleItems
            // 
            this.checkedMultipleItems.BackColor = System.Drawing.SystemColors.Control;
            this.checkedMultipleItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.checkedMultipleItems.CheckOnClick = true;
            this.checkedMultipleItems.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedMultipleItems.ForeColor = System.Drawing.Color.Black;
            this.checkedMultipleItems.Location = new System.Drawing.Point(6, 43);
            this.checkedMultipleItems.Name = "checkedMultipleItems";
            this.checkedMultipleItems.Size = new System.Drawing.Size(200, 98);
            this.checkedMultipleItems.TabIndex = 0;
            this.checkedMultipleItems.ThreeDCheckBoxes = true;
            this.checkedMultipleItems.Visible = false;
            this.checkedMultipleItems.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedMultipleItems_ItemCheck);
            this.checkedMultipleItems.Click += new System.EventHandler(this.checkedMultipleItems_Click);
            // 
            // ultraPanel1
            // 
            this.ultraPanel1.Location = new System.Drawing.Point(0, -78);
            this.ultraPanel1.Name = "ultraPanel1";
            this.ultraPanel1.Size = new System.Drawing.Size(210, 30);
            this.ultraPanel1.TabIndex = 0;
            // 
            // MultiSelectEditor
            // 
            DropDownEditorButton1.Control = this.checkedMultipleItems;
            this.DropDownEditorButton1.AfterCloseUp += new Infragistics.Win.UltraWinEditors.EditorButtonEventHandler(DropDownEditorButton1_AfterCloseUp);
            this.MultiSelectEditor.ButtonsRight.Add(DropDownEditorButton1);
            this.MultiSelectEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MultiSelectEditor.Location = new System.Drawing.Point(0, 0);
            this.MultiSelectEditor.Name = "MultiSelectEditor";
            this.MultiSelectEditor.ReadOnly = true;
            this.MultiSelectEditor.Size = new System.Drawing.Size(146, 21);
            this.MultiSelectEditor.TabIndex = 1;
            this.MultiSelectEditor.Text = "DropDownEditorButton1";
            // 
            // MultiSelectDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MultiSelectEditor);
            this.Controls.Add(this.ultraPanel1);
            this.Name = "MultiSelectDropDown";
            this.Size = new System.Drawing.Size(146, 21);
            this.ultraPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MultiSelectEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel ultraPanel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor MultiSelectEditor;
        private Infragistics.Win.UltraWinEditors.DropDownEditorButton DropDownEditorButton1;
        private System.Windows.Forms.CheckedListBox checkedMultipleItems;
    }
}
