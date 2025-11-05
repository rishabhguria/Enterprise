namespace Prana
{
    partial class UsrCtrlConnectionStatus
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
            this.lblCpName = new Infragistics.Win.Misc.UltraLabel();
            this.SuspendLayout();
            // 
            // lblCpName
            // 
            this.lblCpName.Location = new System.Drawing.Point(0, 0);
            this.lblCpName.Name = "lblCpName";
            this.lblCpName.Size = new System.Drawing.Size(80, 17);
            this.lblCpName.TabIndex = 15;
            this.lblCpName.Text = "CounterParty";
            this.lblCpName.UseAppStyling = false;
            // 
            // UsrCtrlConnectionStatus
            // 
            //this.AutoSize = true;
            //this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientArea.Controls.Add(this.lblCpName);
            this.ForeColor = System.Drawing.Color.Red;
            this.Name = "UsrCtrlConnectionStatus";
            this.Size = new System.Drawing.Size(83, 17);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel lblCpName;
    }
}
