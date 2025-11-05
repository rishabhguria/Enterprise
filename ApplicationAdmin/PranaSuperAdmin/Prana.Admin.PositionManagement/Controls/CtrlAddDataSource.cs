using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Controls;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.Properties;


namespace Nirvana.Admin.PositionManagement.Controls
{
	/// <summary>
	/// Summary description for CreateStrategy.
	/// </summary>
    public partial class CtrlAddDataSource : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.GroupBox grpDataSource;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblShortName;
        private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtFullName;
		private System.Windows.Forms.Label lblIsFullNameEmpty;
		private System.Windows.Forms.Label lblIslShortNameEmpty;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public CtrlAddDataSource()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		

		

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CtrlAddDataSource));
            this.btnAdd = new System.Windows.Forms.Button();
            this.grpDataSource = new System.Windows.Forms.GroupBox();
            this.lblIslShortNameEmpty = new System.Windows.Forms.Label();
            this.lblIsFullNameEmpty = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.lblShortName = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpDataSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.btnAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAdd.BackgroundImage")));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnAdd.Location = new System.Drawing.Point(59, 70);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // grpDataSource
            // 
            this.grpDataSource.Controls.Add(this.lblIslShortNameEmpty);
            this.grpDataSource.Controls.Add(this.lblIsFullNameEmpty);
            this.grpDataSource.Controls.Add(this.txtFullName);
            this.grpDataSource.Controls.Add(this.lblName);
            this.grpDataSource.Controls.Add(this.lblShortName);
            this.grpDataSource.Controls.Add(this.txtShortName);
            this.grpDataSource.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.grpDataSource.Location = new System.Drawing.Point(3, 2);
            this.grpDataSource.Name = "grpDataSource";
            this.grpDataSource.Size = new System.Drawing.Size(266, 66);
            this.grpDataSource.TabIndex = 5;
            this.grpDataSource.TabStop = false;
            this.grpDataSource.Text = "New Data Source";
            this.grpDataSource.Enter += new System.EventHandler(this.grpDataSource_Enter);
            // 
            // lblIslShortNameEmpty
            // 
            this.lblIslShortNameEmpty.ForeColor = System.Drawing.Color.Red;
            this.lblIslShortNameEmpty.Location = new System.Drawing.Point(68, 46);
            this.lblIslShortNameEmpty.Name = "lblIslShortNameEmpty";
            this.lblIslShortNameEmpty.Size = new System.Drawing.Size(12, 8);
            this.lblIslShortNameEmpty.TabIndex = 35;
            this.lblIslShortNameEmpty.Text = "*";
            // 
            // lblIsFullNameEmpty
            // 
            this.lblIsFullNameEmpty.ForeColor = System.Drawing.Color.Red;
            this.lblIsFullNameEmpty.Location = new System.Drawing.Point(63, 23);
            this.lblIsFullNameEmpty.Name = "lblIsFullNameEmpty";
            this.lblIsFullNameEmpty.Size = new System.Drawing.Size(12, 8);
            this.lblIsFullNameEmpty.TabIndex = 35;
            this.lblIsFullNameEmpty.Text = "*";
            // 
            // txtFullName
            // 
            this.txtFullName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFullName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFullName.Location = new System.Drawing.Point(96, 20);
            this.txtFullName.MaxLength = 50;
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(148, 21);
            this.txtFullName.TabIndex = 0;
            this.txtFullName.Enter += new System.EventHandler(this.txtDataSourceFullName_GotFocus);
            this.txtFullName.Leave += new System.EventHandler(this.txtDataSourceFullName_LostFocus);
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblName.Location = new System.Drawing.Point(8, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(56, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Full Name";
            // 
            // lblShortName
            // 
            this.lblShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblShortName.Location = new System.Drawing.Point(8, 46);
            this.lblShortName.Name = "lblShortName";
            this.lblShortName.Size = new System.Drawing.Size(72, 14);
            this.lblShortName.TabIndex = 0;
            this.lblShortName.Text = "Short Name";
            // 
            // txtShortName
            // 
            this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtShortName.Location = new System.Drawing.Point(96, 42);
            this.txtShortName.MaxLength = 50;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(148, 21);
            this.txtShortName.TabIndex = 1;
            this.txtShortName.Enter += new System.EventHandler(this.txtDataSourceShortName_GotFocus);
            this.txtShortName.Leave += new System.EventHandler(this.txtDataSourceShortName_LostFocus);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(139, 70);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CtrlAddDataSource
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpDataSource);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Name = "CtrlAddDataSource";
            this.Size = new System.Drawing.Size(272, 103);
            this.Load += new System.EventHandler(this.CtrlAddDataSource_Load);
            this.grpDataSource.ResumeLayout(false);
            this.grpDataSource.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
        private void txtDataSourceFullName_GotFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.LemonChiffon;
		}
        private void txtDataSourceFullName_LostFocus(object sender, System.EventArgs e)
		{
			txtFullName.BackColor = Color.White;
		}
        private void txtDataSourceShortName_GotFocus(object sender, System.EventArgs e)
		{
            txtShortName.BackColor = Color.LemonChiffon;
		}
        private void txtDataSourceShortName_LostFocus(object sender, System.EventArgs e)
		{
            txtShortName.BackColor = Color.White;
		} 
		#endregion

        /// <summary>
        /// Handles the Click event of the btnAdd control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string shortName = txtShortName.Text.Trim();

            
            if (fullName.Length == 0)
            {
                lblIsFullNameEmpty.Visible = true;
            }
            else
            {
                lblIsFullNameEmpty.Visible = false;
            }
            if (shortName.Length == 0)
            {
                lblIslShortNameEmpty.Visible = true;
            }
            else
            {
                lblIslShortNameEmpty.Visible = false;
            }

            if (!lblIsFullNameEmpty.Visible && !lblIslShortNameEmpty.Visible)
            {
                //Populate new DataSource Object
                DataSourceNameID dataSourceName = new DataSourceNameID();
                dataSourceName.FullName = fullName;
                dataSourceName.ShortName = shortName;

                int isSuccessfull = DataSourceManager.AddDataSource(dataSourceName);

                if (isSuccessfull == 0)
                {
                    MessageBox.Show("Added to DataBase");
                    this.FindForm().Close();
                }
                else if (isSuccessfull == 2627)
                {
                    MessageBox.Show(Constants.C_DUPLICATE_ENTRY);
                }
                else
                {
                    
                    MessageBox.Show(Constants.C_FATAL_ERROR);
                    this.FindForm().Close();
                }               

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
           this.FindForm().Close();
           
            
        }

        public class Utility
        {
            public void CloseForm(Form form)
            {
                form.Close();
            }
        }

        private void CtrlAddDataSource_Load(object sender, EventArgs e)
        {
            lblIsFullNameEmpty.Visible = false;
            lblIslShortNameEmpty.Visible = false;
        }

        private void grpDataSource_Enter(object sender, EventArgs e)
        {

        }

		

		
	
		
	}
}
