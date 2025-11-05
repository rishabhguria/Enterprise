namespace Prana.Preferences
{
    partial class GeneralPreferenceControl
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
            this.lblShowServiceIcons = new Infragistics.Win.Misc.UltraLabel();
            this.chbIsShowServiceIcons = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblShowServiceIcons
            // 
            this.lblShowServiceIcons.Location = new System.Drawing.Point(102, 95);
            this.lblShowServiceIcons.Name = "lblShowServiceIcons";
            this.lblShowServiceIcons.Size = new System.Drawing.Size(200, 23);
            this.lblShowServiceIcons.TabIndex = 4;
            this.lblShowServiceIcons.Text = "Show Service icons";
            // 
            // chbIsShowServiceIcons
            // 
            this.chbIsShowServiceIcons.AutoSize = true;
            this.chbIsShowServiceIcons.Location = new System.Drawing.Point(77, 95);
            this.chbIsShowServiceIcons.Name = "chbIsShowServiceIcons";
            this.chbIsShowServiceIcons.Size = new System.Drawing.Size(15, 14);
            this.chbIsShowServiceIcons.TabIndex = 2;
            this.chbIsShowServiceIcons.UseVisualStyleBackColor = true;
            this.chbIsShowServiceIcons.Checked = false;
            this.chbIsShowServiceIcons.CheckedChanged += new System.EventHandler(this.chbIsShowPMToolbar_CheckedChanged);
            // 
            // PMPreferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chbIsShowServiceIcons);
            this.Controls.Add(this.lblShowServiceIcons);
            this.Name = "GeneralPreferenceControl";
            this.Size = new System.Drawing.Size(557, 273);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.Misc.UltraLabel lblShowServiceIcons;
        private System.Windows.Forms.CheckBox chbIsShowServiceIcons;
    }
}
