namespace Prana.AmqpPlugin
{
    partial class AlertForm
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
                if (_dtListViewSource != null)
                {
                    _dtListViewSource.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlertForm));
            this.spBase = new System.Windows.Forms.SplitContainer();
            this.spAlert = new System.Windows.Forms.SplitContainer();
            this.lstViewAlert = new System.Windows.Forms.ListView();
            this.clHeadeDefault = new System.Windows.Forms.ColumnHeader();
            this.ulblDescription = new Infragistics.Win.Misc.UltraLabel();
            this.ulblRuleName = new Infragistics.Win.Misc.UltraLabel();
            this.ubtnDismiss = new Infragistics.Win.Misc.UltraButton();
            this.ubtnDismissAll = new Infragistics.Win.Misc.UltraButton();
            this.spBase.Panel1.SuspendLayout();
            this.spBase.Panel2.SuspendLayout();
            this.spBase.SuspendLayout();
            this.spAlert.Panel1.SuspendLayout();
            this.spAlert.Panel2.SuspendLayout();
            this.spAlert.SuspendLayout();
            this.SuspendLayout();
            // 
            // spBase
            // 
            this.spBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spBase.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spBase.IsSplitterFixed = true;
            this.spBase.Location = new System.Drawing.Point(0, 0);
            this.spBase.Name = "spBase";
            this.spBase.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spBase.Panel1
            // 
            this.spBase.Panel1.Controls.Add(this.spAlert);
            // 
            // spBase.Panel2
            // 
            this.spBase.Panel2.Controls.Add(this.ubtnDismiss);
            this.spBase.Panel2.Controls.Add(this.ubtnDismissAll);
            this.spBase.Size = new System.Drawing.Size(421, 364);
            this.spBase.SplitterDistance = 326;
            this.spBase.TabIndex = 1;
            // 
            // spAlert
            // 
            this.spAlert.BackColor = System.Drawing.Color.Silver;
            this.spAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spAlert.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.spAlert.Location = new System.Drawing.Point(0, 0);
            this.spAlert.Name = "spAlert";
            this.spAlert.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spAlert.Panel1
            // 
            this.spAlert.Panel1.Controls.Add(this.lstViewAlert);
            // 
            // spAlert.Panel2
            // 
            this.spAlert.Panel2.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.spAlert.Panel2.AutoScroll = true;
            this.spAlert.Panel2.Controls.Add(this.ulblDescription);
            this.spAlert.Panel2.Controls.Add(this.ulblRuleName);
            this.spAlert.Size = new System.Drawing.Size(421, 326);
            this.spAlert.SplitterDistance = 200;
            this.spAlert.SplitterWidth = 8;
            this.spAlert.TabIndex = 0;
            // 
            // lstViewAlert
            // 
            this.lstViewAlert.BackColor = System.Drawing.Color.Black;
            this.lstViewAlert.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clHeadeDefault});
            this.lstViewAlert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstViewAlert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lstViewAlert.FullRowSelect = true;
            this.lstViewAlert.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstViewAlert.HideSelection = false;
            this.lstViewAlert.Location = new System.Drawing.Point(0, 0);
            this.lstViewAlert.Name = "lstViewAlert";
            this.lstViewAlert.ShowItemToolTips = true;
            this.lstViewAlert.Size = new System.Drawing.Size(421, 200);
            this.lstViewAlert.TabIndex = 0;
            this.lstViewAlert.UseCompatibleStateImageBehavior = false;
            this.lstViewAlert.View = System.Windows.Forms.View.Details;
            this.lstViewAlert.SelectedIndexChanged += new System.EventHandler(this.lstViewAlert_SelectedIndexChanged);
            // 
            // clHeadeDefault
            // 
            this.clHeadeDefault.Text = "Alert";
            this.clHeadeDefault.Width = 417;
            // 
            // ulblDescription
            // 
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Top";
            this.ulblDescription.Appearance = appearance1;
            this.ulblDescription.AutoSize = true;
            this.ulblDescription.Location = new System.Drawing.Point(23, 21);
            this.ulblDescription.MaximumSize = new System.Drawing.Size(380, 0);
            this.ulblDescription.Name = "ulblDescription";
            this.ulblDescription.Size = new System.Drawing.Size(0, 0);
            this.ulblDescription.TabIndex = 1;
            // 
            // ulblRuleName
            // 
            this.ulblRuleName.AutoSize = true;
            this.ulblRuleName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ulblRuleName.Location = new System.Drawing.Point(12, 3);
            this.ulblRuleName.Name = "ulblRuleName";
            this.ulblRuleName.Size = new System.Drawing.Size(0, 0);
            this.ulblRuleName.TabIndex = 0;
            // 
            // ubtnDismiss
            // 
            this.ubtnDismiss.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.ubtnDismiss.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ubtnDismiss.Location = new System.Drawing.Point(123, 8);
            this.ubtnDismiss.Name = "ubtnDismiss";
            this.ubtnDismiss.Size = new System.Drawing.Size(75, 23);
            this.ubtnDismiss.TabIndex = 1;
            this.ubtnDismiss.Text = "Dismiss";
            this.ubtnDismiss.Click += new System.EventHandler(this.ubtnDismiss_Click);
            // 
            // ubtnDismissAll
            // 
            this.ubtnDismissAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Flat;
            this.ubtnDismissAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ubtnDismissAll.Location = new System.Drawing.Point(204, 8);
            this.ubtnDismissAll.Name = "ubtnDismissAll";
            this.ubtnDismissAll.Size = new System.Drawing.Size(93, 23);
            this.ubtnDismissAll.TabIndex = 0;
            this.ubtnDismissAll.Text = "Dismiss All";
            this.ubtnDismissAll.Click += new System.EventHandler(this.ubtnDismissAll_Click);
            // 
            // AlertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(421, 364);
            this.Controls.Add(this.spBase);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AlertForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nirvana Alerts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlertForm_FormClosing);
            this.spBase.Panel1.ResumeLayout(false);
            this.spBase.Panel2.ResumeLayout(false);
            this.spBase.ResumeLayout(false);
            this.spAlert.Panel1.ResumeLayout(false);
            this.spAlert.Panel2.ResumeLayout(false);
            this.spAlert.Panel2.PerformLayout();
            this.spAlert.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spBase;
        private System.Windows.Forms.SplitContainer spAlert;
        private Infragistics.Win.Misc.UltraLabel ulblDescription;
        private Infragistics.Win.Misc.UltraLabel ulblRuleName;
        private Infragistics.Win.Misc.UltraButton ubtnDismiss;
        private Infragistics.Win.Misc.UltraButton ubtnDismissAll;
        private System.Windows.Forms.ListView lstViewAlert;
        private System.Windows.Forms.ColumnHeader clHeadeDefault;

    }
}