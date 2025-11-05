#region Using

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

#endregion

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for ClientFunds.
	/// </summary>
	public class ClientFunds : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "ClientFunds : ";
		#region Private and Protected Members

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdFunds;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGridTableStyle tblStyle;
		private System.Windows.Forms.DataGridTextBoxColumn colFundName;
		private System.Windows.Forms.DataGridTextBoxColumn colFundShortName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private int _companyID = int.MinValue;

		#endregion

		public ClientFunds()
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grdFunds = new System.Windows.Forms.DataGrid();
			this.tblStyle = new System.Windows.Forms.DataGridTableStyle();
			this.colFundName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.colFundShortName = new System.Windows.Forms.DataGridTextBoxColumn();
			this.btnCreate = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdFunds)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.grdFunds);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox1.Location = new System.Drawing.Point(6, 26);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(384, 326);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Funds";
			// 
			// grdFunds
			// 
			this.grdFunds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdFunds.CaptionVisible = false;
			this.grdFunds.DataMember = "";
			this.grdFunds.FlatMode = true;
			this.grdFunds.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdFunds.Location = new System.Drawing.Point(6, 16);
			this.grdFunds.Name = "grdFunds";
			this.grdFunds.Size = new System.Drawing.Size(374, 303);
			this.grdFunds.TabIndex = 0;
			this.grdFunds.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																								 this.tblStyle});
			// 
			// tblStyle
			// 
			this.tblStyle.DataGrid = this.grdFunds;
			this.tblStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																									   this.colFundName,
																									   this.colFundShortName});
			this.tblStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.tblStyle.MappingName = "";
			// 
			// colFundName
			// 
			this.colFundName.Format = "";
			this.colFundName.FormatInfo = null;
			this.colFundName.HeaderText = "Fund Name";
			this.colFundName.MappingName = "FundName";
			this.colFundName.ReadOnly = true;
			this.colFundName.Width = 75;
			// 
			// colFundShortName
			// 
			this.colFundShortName.Format = "";
			this.colFundShortName.FormatInfo = null;
			this.colFundShortName.HeaderText = "Short Name";
			this.colFundShortName.MappingName = "FundShortName";
			this.colFundShortName.Width = 75;
			// 
			// btnCreate
			// 
			this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnCreate.Location = new System.Drawing.Point(302, 0);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.Size = new System.Drawing.Size(88, 24);
			this.btnCreate.TabIndex = 1;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// ClientFunds
			// 
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.groupBox1);
			this.Name = "ClientFunds";
			this.Size = new System.Drawing.Size(398, 368);
			this.Load += new System.EventHandler(this.ClientFunds_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdFunds)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public int CompanyID
		{
			set
			{
				_companyID = value;
				BindFundGrid();
			}
		}

		private void ClientFunds_Load(object sender, System.EventArgs e)
		{
			try
			{
				BindFundGrid();
			}
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnLogin_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnLogin_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void BindFundGrid()
		{
//			Nirvana.Admin.BLL.ClientFunds	clientFunds = ClientFundManager.GetClientFunds(_companyID);
//			grdFunds.DataSource = clientFunds;
		}

		CreateClientFund frmCreateClientFund = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(frmCreateClientFund == null)
			{
				frmCreateClientFund = new CreateClientFund();
				//frmCreateClientFund.MdiParent = this.ParentForm;
			}
			frmCreateClientFund.ShowDialog(this.ParentForm);
		}
	}
}
