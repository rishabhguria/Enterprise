using Prana.PM.BLL;
//using Prana.PM.Common;
using Prana.PM.Admin.UI.Controls;
using Prana.PM.Admin.UI.Forms;

namespace Prana.PM.Admin.UI.Forms
{
    partial class AddDataSource
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddDataSource));
            this.ctrlAddDataSource1 = new Prana.PM.Admin.UI.Controls.CtrlAddDataSource();
            this.SuspendLayout();
            // 
            // ctrlAddDataSource1
            // 
            this.ctrlAddDataSource1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlAddDataSource1.DataMember = "";
            this.ctrlAddDataSource1.DataSource = typeof(Prana.BusinessObjects.PositionManagement.DataSourceNameID);
            this.ctrlAddDataSource1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ctrlAddDataSource1.Location = new System.Drawing.Point(0, 0);
            this.ctrlAddDataSource1.MinimumSize = new System.Drawing.Size(272, 204);
            this.ctrlAddDataSource1.Name = "ctrlAddDataSource1";
            this.ctrlAddDataSource1.Size = new System.Drawing.Size(281, 204);
            this.ctrlAddDataSource1.TabIndex = 0;
            // 
            // AddDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 208);
            this.Controls.Add(this.ctrlAddDataSource1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddDataSource";
            this.Text = "Add Data Source";
            this.Load += new System.EventHandler(this.AddDataSource_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Prana.PM.Admin.UI.Controls.CtrlAddDataSource ctrlAddDataSource1;
    }
}