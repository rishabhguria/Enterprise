using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.Properties;
namespace Nirvana.Admin.PositionManagement.Forms
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
            this.ctrlAddDataSource1 = new Nirvana.Admin.PositionManagement.Controls.CtrlAddDataSource();
            this.SuspendLayout();
            // 
            // ctrlAddDataSource1
            // 
            this.ctrlAddDataSource1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ctrlAddDataSource1.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ctrlAddDataSource1.Location = new System.Drawing.Point(8, 12);
            this.ctrlAddDataSource1.Name = "ctrlAddDataSource1";
            this.ctrlAddDataSource1.Size = new System.Drawing.Size(272, 103);
            this.ctrlAddDataSource1.TabIndex = 0;
            // 
            // AddDataSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 110);
            this.Controls.Add(this.ctrlAddDataSource1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddDataSource";
            this.Text = "Add Data Source";
            this.ResumeLayout(false);

        }

        #endregion

        private Nirvana.Admin.PositionManagement.Controls.CtrlAddDataSource ctrlAddDataSource1;
    }
}