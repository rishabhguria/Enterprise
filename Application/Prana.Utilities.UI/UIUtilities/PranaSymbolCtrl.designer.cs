namespace Prana.Utilities.UI.UIUtilities
{
    partial class PranaSymbolCtrl
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
            this.txtSymbol = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSymbol
            // 
            this.txtSymbol.AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None;
            this.txtSymbol.AutoSize = false;
            this.txtSymbol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSymbol.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.txtSymbol.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSymbol.Location = new System.Drawing.Point(0, 0);
            this.txtSymbol.Name = "txtSymbol";
            this.txtSymbol.Size = new System.Drawing.Size(140, 21);
            this.txtSymbol.TabIndex = 0;
            this.txtSymbol.SelectionChangeCommitted += new System.EventHandler(this.txtSymbol_SelectionChangeCommitted);
            this.txtSymbol.SelectionChanged += new System.EventHandler(this.txtSymbol_SelectionChanged);
            this.txtSymbol.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSymbol_KeyUp);
            // 
            // PranaSymbolCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSymbol);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PranaSymbolCtrl";
            this.Size = new System.Drawing.Size(140, 21);
            ((System.ComponentModel.ISupportInitialize)(this.txtSymbol)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraComboEditor txtSymbol;
    }
}
