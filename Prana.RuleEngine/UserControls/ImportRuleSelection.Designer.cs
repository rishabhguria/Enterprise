namespace Prana.RuleEngine.UserControls
{
    partial class ImportRuleSelection
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ulstView = new System.Windows.Forms.ListView();
            this.Key = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OriginalRuleName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Package = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            //((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ulstView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ultraButton2);
            this.splitContainer1.Panel2.Controls.Add(this.ultraButton1);
            this.splitContainer1.Size = new System.Drawing.Size(495, 352);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 0;
            // 
            // ulstView
            // 
            this.ulstView.BackColor = System.Drawing.Color.DimGray;
            this.ulstView.CheckBoxes = true;
            this.ulstView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Key,
            this.RuleName,
            this.OriginalRuleName,
            this.Category,
            this.Package});
            this.ulstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ulstView.ForeColor = System.Drawing.SystemColors.Window;
            this.ulstView.FullRowSelect = true;
            this.ulstView.Location = new System.Drawing.Point(0, 0);
            this.ulstView.Name = "ulstView";
            this.ulstView.Size = new System.Drawing.Size(495, 320);
            this.ulstView.TabIndex = 0;
            this.ulstView.UseCompatibleStateImageBehavior = false;
            this.ulstView.View = System.Windows.Forms.View.Details;
            // 
            // Key
            // 
            this.Key.Tag = "Key";
            this.Key.Text = "";
            this.Key.Width = 20;
            // 
            // RuleName
            // 
            this.RuleName.Tag = "RuleName";
            this.RuleName.Text = "RuleName";
            this.RuleName.Width = 164;
            // 
            // OriginalRuleName
            // 
            this.OriginalRuleName.Tag = "OriginalRuleName";
            this.OriginalRuleName.Text = "OriginalRuleName";
            this.OriginalRuleName.Width = 0;
            // 
            // Category
            // 
            this.Category.Tag = "Category";
            this.Category.Text = "Category";
            this.Category.Width = 162;
            // 
            // Package
            // 
            this.Package.Tag = "Package";
            this.Package.Text = "Package";
            this.Package.Width = 149;
            // 
            // ultraButton2
            // 
            this.ultraButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ultraButton2.Location = new System.Drawing.Point(336, 2);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(75, 23);
            this.ultraButton2.TabIndex = 1;
            this.ultraButton2.Text = "OK";
            this.ultraButton2.Click += new System.EventHandler(this.ultraButton2_Click);
            // 
            // ultraButton1
            // 
            this.ultraButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ultraButton1.Location = new System.Drawing.Point(417, 3);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(75, 23);
            this.ultraButton1.TabIndex = 0;
            this.ultraButton1.Text = "Cancel";
            // 
            // ImportRuleSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 352);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(511, 390);
            this.MinimizeBox = false;
            this.Name = "ImportRuleSelection";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nirvana Compliance - Select rules to import";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            //((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private System.Windows.Forms.ListView ulstView;
        private System.Windows.Forms.ColumnHeader RuleName;
        private System.Windows.Forms.ColumnHeader Category;
        private System.Windows.Forms.ColumnHeader Package;
        private System.Windows.Forms.ColumnHeader OriginalRuleName;
        private System.Windows.Forms.ColumnHeader Key;

    }
}