namespace Prana.AdminForms
{
    partial class NewYear
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
                if(_savedcholidays != null)
                {
                    _savedcholidays.Dispose();
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
            this.lblYear = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dtYearPicker = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(32, 17);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(29, 13);
            this.lblYear.TabIndex = 1;
            this.lblYear.Text = "Year";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(93, 45);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dtYearPicker
            // 
            this.dtYearPicker.CustomFormat = "yyyy";
            this.dtYearPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtYearPicker.Location = new System.Drawing.Point(67, 13);
            this.dtYearPicker.Name = "dtYearPicker";
            this.dtYearPicker.ShowUpDown = true;
            this.dtYearPicker.Size = new System.Drawing.Size(89, 20);
            this.dtYearPicker.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 45);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // NewYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(193, 80);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dtYearPicker);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblYear);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(201, 114);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(201, 114);
            this.Name = "NewYear";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Add New Year";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.NewYear_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewYear_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DateTimePicker dtYearPicker;
        private System.Windows.Forms.Button btnAdd;
    }
}