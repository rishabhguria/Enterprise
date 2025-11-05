namespace Prana.ClientCommon
{
    partial class Options
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.label1 = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.rdBtnRunClosing = new System.Windows.Forms.RadioButton();
            this.rdBtnPreviewData = new System.Windows.Forms.RadioButton();
            this.rdBtnAddUnwindingTemplate = new System.Windows.Forms.RadioButton();
            this.rdbtnEditTemplate = new System.Windows.Forms.RadioButton();
            this.rdBtnDeleteTemplate = new System.Windows.Forms.RadioButton();
            this.rdbtnAddClosingTemplate = new System.Windows.Forms.RadioButton();
            this.inboxControlStyler1 = new Infragistics.Win.AppStyling.Runtime.InboxControlStyler(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(19, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 14);
            this.inboxControlStyler1.SetStyleSettings(this.label1, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label1.TabIndex = 0;
            this.label1.Text = "Please select the options you want to perform:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(38, 360);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(618, 26);
            this.inboxControlStyler1.SetStyleSettings(this.label2, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.label2.TabIndex = 4;
            this.label2.Text = "Note: This Wizard will help you create a custom template for the automated closing and " +
                "unwinding of transactions.\r\nThis is very helpful in case you have a large volume of" +
                " data or complex closing methods.\r\n";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.SteelBlue;
            appearance1.BackColor2 = System.Drawing.Color.White;
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance1.ForeColor = System.Drawing.Color.White;
            appearance1.TextHAlignAsString = "Center";
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ultraLabel2.Location = new System.Drawing.Point(-3, 0);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(853, 20);
            this.ultraLabel2.TabIndex = 16;
            this.ultraLabel2.Text = "Wizard Options";
            // 
            // rdBtnRunClosing
            // 
            this.rdBtnRunClosing.AutoSize = true;
            this.rdBtnRunClosing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnRunClosing.ForeColor = System.Drawing.Color.Black;
            this.rdBtnRunClosing.Location = new System.Drawing.Point(36, 206);
            this.rdBtnRunClosing.Name = "rdBtnRunClosing";
            this.rdBtnRunClosing.Size = new System.Drawing.Size(134, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnRunClosing, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnRunClosing.TabIndex = 117;
            this.rdBtnRunClosing.Text = "Run Closing/Unwinding";
            this.rdBtnRunClosing.UseVisualStyleBackColor = true;
            this.rdBtnRunClosing.CheckedChanged += new System.EventHandler(this.rdBtnRunClosing_CheckedChanged);
            // 
            // rdBtnPreviewData
            // 
            this.rdBtnPreviewData.AutoSize = true;
            this.rdBtnPreviewData.Checked = true;
            this.rdBtnPreviewData.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnPreviewData.ForeColor = System.Drawing.Color.Black;
            this.rdBtnPreviewData.Location = new System.Drawing.Point(36, 69);
            this.rdBtnPreviewData.Name = "rdBtnPreviewData";
            this.rdBtnPreviewData.Size = new System.Drawing.Size(146, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnPreviewData, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnPreviewData.TabIndex = 118;
            this.rdBtnPreviewData.TabStop = true;
            this.rdBtnPreviewData.Text = "Preview Data With Filters";
            this.rdBtnPreviewData.UseVisualStyleBackColor = true;
            this.rdBtnPreviewData.CheckedChanged += new System.EventHandler(this.rdBtnPreviewData_CheckedChanged);
            // 
            // rdBtnAddUnwindingTemplate
            // 
            this.rdBtnAddUnwindingTemplate.AutoSize = true;
            this.rdBtnAddUnwindingTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnAddUnwindingTemplate.ForeColor = System.Drawing.Color.Black;
            this.rdBtnAddUnwindingTemplate.Location = new System.Drawing.Point(36, 125);
            this.rdBtnAddUnwindingTemplate.Name = "rdBtnAddUnwindingTemplate";
            this.rdBtnAddUnwindingTemplate.Size = new System.Drawing.Size(157, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnAddUnwindingTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnAddUnwindingTemplate.TabIndex = 122;
            this.rdBtnAddUnwindingTemplate.Text = "Create Unwinding Template";
            this.rdBtnAddUnwindingTemplate.UseVisualStyleBackColor = true;
            this.rdBtnAddUnwindingTemplate.CheckedChanged += new System.EventHandler(this.rdBtnAddUnwindingTemplate_CheckedChanged);
            // 
            // rdbtnEditTemplate
            // 
            this.rdbtnEditTemplate.AutoSize = true;
            this.rdbtnEditTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbtnEditTemplate.ForeColor = System.Drawing.Color.Black;
            this.rdbtnEditTemplate.Location = new System.Drawing.Point(36, 154);
            this.rdbtnEditTemplate.Name = "rdbtnEditTemplate";
            this.rdbtnEditTemplate.Size = new System.Drawing.Size(95, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdbtnEditTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdbtnEditTemplate.TabIndex = 121;
            this.rdbtnEditTemplate.Text = "Edit Templates";
            this.rdbtnEditTemplate.UseVisualStyleBackColor = true;
            this.rdbtnEditTemplate.CheckedChanged += new System.EventHandler(this.rdbtnEditTemplate_CheckedChanged);
            // 
            // rdBtnDeleteTemplate
            // 
            this.rdBtnDeleteTemplate.AutoSize = true;
            this.rdBtnDeleteTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdBtnDeleteTemplate.ForeColor = System.Drawing.Color.Black;
            this.rdBtnDeleteTemplate.Location = new System.Drawing.Point(36, 181);
            this.rdBtnDeleteTemplate.Name = "rdBtnDeleteTemplate";
            this.rdBtnDeleteTemplate.Size = new System.Drawing.Size(108, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdBtnDeleteTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdBtnDeleteTemplate.TabIndex = 120;
            this.rdBtnDeleteTemplate.Text = "Delete Templates";
            this.rdBtnDeleteTemplate.UseVisualStyleBackColor = true;
            this.rdBtnDeleteTemplate.CheckedChanged += new System.EventHandler(this.rdBtnDeleteTemplate_CheckedChanged);
            // 
            // rdbtnAddClosingTemplate
            // 
            this.rdbtnAddClosingTemplate.AutoSize = true;
            this.rdbtnAddClosingTemplate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbtnAddClosingTemplate.ForeColor = System.Drawing.Color.Black;
            this.rdbtnAddClosingTemplate.Location = new System.Drawing.Point(36, 99);
            this.rdbtnAddClosingTemplate.Name = "rdbtnAddClosingTemplate";
            this.rdbtnAddClosingTemplate.Size = new System.Drawing.Size(142, 17);
            this.inboxControlStyler1.SetStyleSettings(this.rdbtnAddClosingTemplate, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            this.rdbtnAddClosingTemplate.TabIndex = 119;
            this.rdbtnAddClosingTemplate.Text = "Create Closing Template";
            this.rdbtnAddClosingTemplate.UseVisualStyleBackColor = true;
            this.rdbtnAddClosingTemplate.CheckedChanged += new System.EventHandler(this.rdbtnAddClosingTemplate_CheckedChanged);
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.rdBtnAddUnwindingTemplate);
            this.Controls.Add(this.rdbtnEditTemplate);
            this.Controls.Add(this.rdBtnDeleteTemplate);
            this.Controls.Add(this.rdbtnAddClosingTemplate);
            this.Controls.Add(this.rdBtnPreviewData);
            this.Controls.Add(this.rdBtnRunClosing);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Name = "Options";
            this.Size = new System.Drawing.Size(850, 424);
            this.inboxControlStyler1.SetStyleSettings(this, new Infragistics.Win.AppStyling.Runtime.InboxControlStyleSettings(Infragistics.Win.DefaultableBoolean.Default));
            ((System.ComponentModel.ISupportInitialize)(this.inboxControlStyler1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel label1;
        internal Infragistics.Win.Misc.UltraLabel label2;
        internal Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.RadioButton rdBtnRunClosing;
        internal System.Windows.Forms.RadioButton rdBtnPreviewData;
        internal System.Windows.Forms.RadioButton rdBtnAddUnwindingTemplate;
        internal System.Windows.Forms.RadioButton rdbtnEditTemplate;
        internal System.Windows.Forms.RadioButton rdBtnDeleteTemplate;
        internal System.Windows.Forms.RadioButton rdbtnAddClosingTemplate;
        private Infragistics.Win.AppStyling.Runtime.InboxControlStyler inboxControlStyler1;
    }
}
