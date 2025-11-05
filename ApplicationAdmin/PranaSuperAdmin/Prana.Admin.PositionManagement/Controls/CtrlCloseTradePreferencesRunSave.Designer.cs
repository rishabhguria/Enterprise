namespace Nirvana.Admin.PositionManagement.Controls
{
    partial class CtrlCloseTradePreferencesRunSave
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlCloseTradePreferencesRunSave));
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.btnSave = new Infragistics.Win.Misc.UltraButton();
            this.btnRun = new Infragistics.Win.Misc.UltraButton();
            this.ctrlCloseTradePreferences1 = new Nirvana.Admin.PositionManagement.Controls.CtrlCloseTradePreferences();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance1.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.btn_cancel;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.ImageSize = new System.Drawing.Size(75, 23);
            this.btnCancel.Location = new System.Drawing.Point(477, 156);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.ShowFocusRect = false;
            this.btnCancel.ShowOutline = false;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 62;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            this.btnSave.Appearance = appearance2;
            this.btnSave.ImageSize = new System.Drawing.Size(75, 23);
            this.btnSave.Location = new System.Drawing.Point(342, 157);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.ShowFocusRect = false;
            this.btnSave.ShowOutline = false;
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 61;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            appearance3.Image = global::Nirvana.Admin.PositionManagement.Properties.Resources.run;
            this.btnRun.Appearance = appearance3;
            this.btnRun.ImageSize = new System.Drawing.Size(75, 23);
            this.btnRun.Location = new System.Drawing.Point(207, 156);
            this.btnRun.Name = "btnRun";
            this.btnRun.ShowFocusRect = false;
            this.btnRun.ShowOutline = false;
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 60;
            this.btnRun.Text = "Run";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // ctrlCloseTradePreferences1
            // 
            this.ctrlCloseTradePreferences1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlCloseTradePreferences1.ComboDefaultMethodologyEnabled = true;
            this.ctrlCloseTradePreferences1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ctrlCloseTradePreferences1.IsInitialized = false;
            this.ctrlCloseTradePreferences1.Location = new System.Drawing.Point(0, 0);
            this.ctrlCloseTradePreferences1.Name = "ctrlCloseTradePreferences1";
            this.ctrlCloseTradePreferences1.Size = new System.Drawing.Size(756, 153);
            this.ctrlCloseTradePreferences1.TabIndex = 63;
            // 
            // CtrlCloseTradePreferencesRunSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.ctrlCloseTradePreferences1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRun);
            this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "CtrlCloseTradePreferencesRunSave";
            this.Size = new System.Drawing.Size(758, 190);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraButton btnCancel;
        private Infragistics.Win.Misc.UltraButton btnSave;
        private Infragistics.Win.Misc.UltraButton btnRun;
        private CtrlCloseTradePreferences ctrlCloseTradePreferences1;
    }
}
