#region Using

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;
using System.Data;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion
namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for UploadHolidays.
	/// </summary>
	public class UploadHolidays : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCopyHolidays;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.CheckedListBox checkedlstExchange;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UploadHolidays()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UploadHolidays));
			this.checkedlstExchange = new System.Windows.Forms.CheckedListBox();
			this.btnCopyHolidays = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// checkedlstExchange
			// 
			this.checkedlstExchange.CheckOnClick = true;
			this.checkedlstExchange.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.checkedlstExchange.Location = new System.Drawing.Point(35, 2);
			this.checkedlstExchange.Name = "checkedlstExchange";
			this.checkedlstExchange.Size = new System.Drawing.Size(248, 180);
			this.checkedlstExchange.TabIndex = 0;
			// 
			// btnCopyHolidays
			// 
			this.btnCopyHolidays.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnCopyHolidays.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCopyHolidays.BackgroundImage")));
			this.btnCopyHolidays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCopyHolidays.Location = new System.Drawing.Point(82, 186);
			this.btnCopyHolidays.Name = "btnCopyHolidays";
			this.btnCopyHolidays.TabIndex = 1;
			this.btnCopyHolidays.Click += new System.EventHandler(this.btnCopyHolidays_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Location = new System.Drawing.Point(162, 186);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 2;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// UploadHolidays
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(318, 211);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnCopyHolidays);
			this.Controls.Add(this.checkedlstExchange);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(236, 194);
			this.Name = "UploadHolidays";
			this.Text = "UploadHolidays";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		public void BindExchangeList()
		{
			checkedlstExchange.Refresh();
			Exchanges exchanges = new Exchanges();
			exchanges = ExchangeManager.GetExchanges(_exchangeID);
			if(exchanges.Count > 0)
			{
				checkedlstExchange.DataSource = exchanges;
				checkedlstExchange.DisplayMember = "DisplayName";
				checkedlstExchange.ValueMember = "ExchangeID";
			
				checkedlstExchange.SelectedIndex = -1;
			}
			else
			{
				checkedlstExchange.DataSource = null;
			}
		}

		private int _exchangeID = int.MinValue ;

		private void btnCopyHolidays_Click(object sender, System.EventArgs e)
		{
			bool result = false;
			Exchanges exchanges = new Exchanges();
			for(int i = 0, count = checkedlstExchange.Items.Count; i< count; i++)
			{
				if (checkedlstExchange.GetItemChecked(i) == true)
				{
					checkedlstExchange.SetSelected(i, true);
					exchanges.Add((Exchange)checkedlstExchange.SelectedItem);
				}
			}
			result = ExchangeManager.CopyExchangeHolidays(exchanges, _exchangeID);
			if(result == true)
			{
				MessageBox.Show("Holidays copied to the selected exchanges");
				this.Close();
			}
		}
	
		public int ExchangeID
		{
			get 
			{
				return _exchangeID; 
			}
			set
			{
				if(value != int.MinValue)
				{
					_exchangeID = value;
					BindExchangeList();
				}
			}			
		}
	}
}
