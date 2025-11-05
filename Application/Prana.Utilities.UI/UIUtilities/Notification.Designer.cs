namespace Prana.Utilities.UI.UIUtilities
{
    partial class Notification
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
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.btnClose = new System.Windows.Forms.Button();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.LblTitle = new Infragistics.Win.Misc.UltraLabel();
            this.txtbxDescription = new System.Windows.Forms.TextBox();
            this.notifyIcon = new Infragistics.Win.Misc.UltraDesktopAlert(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.notifyIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(320, 151);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ultraLabel1
            // 
            appearance1.ForeColor = System.Drawing.Color.Teal;
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Font = new System.Drawing.Font("Times New Roman", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel1.Location = new System.Drawing.Point(16, 11);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(282, 23);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "Nirvana Compliance -  Alert";
            // 
            // LblTitle
            // 
            appearance2.ForeColor = System.Drawing.Color.Maroon;
            this.LblTitle.Appearance = appearance2;
            this.LblTitle.Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblTitle.Location = new System.Drawing.Point(16, 40);
            this.LblTitle.Name = "LblTitle";
            this.LblTitle.Size = new System.Drawing.Size(379, 23);
            this.LblTitle.TabIndex = 2;
            // 
            // txtbxDescription
            // 
            this.txtbxDescription.BackColor = System.Drawing.Color.DarkGray;
            this.txtbxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbxDescription.Enabled = false;
            this.txtbxDescription.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbxDescription.Location = new System.Drawing.Point(16, 69);
            this.txtbxDescription.Multiline = true;
            this.txtbxDescription.Name = "txtbxDescription";
            this.txtbxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtbxDescription.Size = new System.Drawing.Size(379, 76);
            this.txtbxDescription.TabIndex = 6;
            // 
            // notifyIcon
            // 
            this.notifyIcon.AllowMove = Infragistics.Win.DefaultableBoolean.True;
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.BackColor2 = System.Drawing.Color.White;
            appearance3.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.BorderColor = System.Drawing.Color.Black;
            appearance3.BorderColor2 = System.Drawing.Color.Black;
            this.notifyIcon.Appearance = appearance3;
            this.notifyIcon.AutoClose = Infragistics.Win.DefaultableBoolean.True;
            this.notifyIcon.AutoCloseDelay = 5000;
            appearance4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            appearance4.FontData.SizeInPoints = 10F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            this.notifyIcon.CaptionAppearance = appearance4;
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.White;
            this.notifyIcon.CaptionAreaAppearance = appearance7;
            this.notifyIcon.CloseButtonVisible = Infragistics.Win.DefaultableBoolean.True;
            this.notifyIcon.DropDownButtonVisible = Infragistics.Win.DefaultableBoolean.False;
            this.notifyIcon.FixedSize = new System.Drawing.Size(396, 0);
            appearance6.BackColor = System.Drawing.Color.LightGray;
            appearance6.BackColor2 = System.Drawing.Color.LightGray;
            appearance6.BackColorAlpha = Infragistics.Win.Alpha.Transparent;
            appearance6.ForeColor = System.Drawing.Color.Red;
            appearance6.ForeColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.notifyIcon.GripAreaAppearance = appearance6;
            this.notifyIcon.MultipleWindowDisplayStyle = Infragistics.Win.Misc.MultipleWindowDisplayStyle.Tiled;
            this.notifyIcon.Style = Infragistics.Win.Misc.DesktopAlertStyle.Office2007;
            appearance5.FontData.SizeInPoints = 9F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            this.notifyIcon.TextAppearance = appearance5;
            this.notifyIcon.TreatCaptionAsLink = Infragistics.Win.DefaultableBoolean.False;
            this.notifyIcon.TreatFooterTextAsLink = Infragistics.Win.DefaultableBoolean.False;
            this.notifyIcon.TreatTextAsLink = Infragistics.Win.DefaultableBoolean.False;
            this.notifyIcon.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // Notification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.txtbxDescription);
            this.Controls.Add(this.LblTitle);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Location = new System.Drawing.Point(1000, 600);
            this.Name = "Notification";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Notification";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.notifyIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel LblTitle;
        private System.Windows.Forms.TextBox txtbxDescription;
        private Infragistics.Win.Misc.UltraDesktopAlert notifyIcon;
    }
}