#region Using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Nirvana.Admin.BLL;
using Nirvana.Admin.Utility;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;
#endregion

namespace Nirvana.Admin
{
	/// <summary>
	/// Summary description for ClearingFirmsPrimeBrokers.
	/// </summary>
	public class ClearingFirmsPrimeBrokers : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "ClearingFirmsPrimeBrokers : ";
		private const int GRD_CLEARINGFIRMPRIMEBROKER_ID = 0;
		private const int GRD_CLEARINGFIRMPRIMEBROKER = 1;
		private const int GRD_CLEARINGFIRMPRIMEBROKER_SHORTNAME = 2;
		
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblClearingFirmsPrimeBrokersSName;
		private System.Windows.Forms.Label lblClearingFirmsPrimeBrokersShortName;
		private System.Windows.Forms.TextBox txtClearingFirmPrimeBrokerShortName;
		private System.Windows.Forms.TextBox txtClearingFirmsPrimeBroker;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdClearingFirmsPrimeBrokers;
		private System.Windows.Forms.GroupBox grpClearingFirmPrimeBokers;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ClearingFirmsPrimeBrokers()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ClearingFirmsPrimeBrokers));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ClearingFirmsPrimeBrokersShortName", 2);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 3);
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grpClearingFirmPrimeBokers = new System.Windows.Forms.GroupBox();
			this.txtClearingFirmPrimeBrokerShortName = new System.Windows.Forms.TextBox();
			this.lblClearingFirmsPrimeBrokersSName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtClearingFirmsPrimeBroker = new System.Windows.Forms.TextBox();
			this.lblClearingFirmsPrimeBrokersShortName = new System.Windows.Forms.Label();
			this.grdClearingFirmsPrimeBrokers = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpClearingFirmPrimeBokers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).BeginInit();
			this.SuspendLayout();
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(79, 134);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 91;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnReset.Location = new System.Drawing.Point(111, 222);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 92;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(157, 134);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 90;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(189, 222);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 89;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(33, 222);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 88;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpClearingFirmPrimeBokers
			// 
			this.grpClearingFirmPrimeBokers.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpClearingFirmPrimeBokers.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.grpClearingFirmPrimeBokers.Controls.Add(this.txtClearingFirmPrimeBrokerShortName);
			this.grpClearingFirmPrimeBokers.Controls.Add(this.lblClearingFirmsPrimeBrokersSName);
			this.grpClearingFirmPrimeBokers.Controls.Add(this.label1);
			this.grpClearingFirmPrimeBokers.Controls.Add(this.txtClearingFirmsPrimeBroker);
			this.grpClearingFirmPrimeBokers.Controls.Add(this.lblClearingFirmsPrimeBrokersShortName);
			this.grpClearingFirmPrimeBokers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpClearingFirmPrimeBokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpClearingFirmPrimeBokers.Location = new System.Drawing.Point(11, 160);
			this.grpClearingFirmPrimeBokers.Name = "grpClearingFirmPrimeBokers";
			this.grpClearingFirmPrimeBokers.Size = new System.Drawing.Size(288, 60);
			this.grpClearingFirmPrimeBokers.TabIndex = 86;
			this.grpClearingFirmPrimeBokers.TabStop = false;
			this.grpClearingFirmPrimeBokers.Text = "Add/Update ClearingFirmsPrimeBrokers";
			this.grpClearingFirmPrimeBokers.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// txtClearingFirmPrimeBrokerShortName
			// 
			this.txtClearingFirmPrimeBrokerShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtClearingFirmPrimeBrokerShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtClearingFirmPrimeBrokerShortName.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtClearingFirmPrimeBrokerShortName.Location = new System.Drawing.Point(114, 38);
			this.txtClearingFirmPrimeBrokerShortName.MaxLength = 50;
			this.txtClearingFirmPrimeBrokerShortName.Name = "txtClearingFirmPrimeBrokerShortName";
			this.txtClearingFirmPrimeBrokerShortName.Size = new System.Drawing.Size(154, 21);
			this.txtClearingFirmPrimeBrokerShortName.TabIndex = 10;
			this.txtClearingFirmPrimeBrokerShortName.Text = "";
			this.txtClearingFirmPrimeBrokerShortName.TextChanged += new System.EventHandler(this.txtClearingFirmPrimeBrokerShortName_TextChanged);
			this.txtClearingFirmPrimeBrokerShortName.LostFocus += new System.EventHandler(this.txtClearingFirmPrimeBrokerShortName_LostFocus);
			this.txtClearingFirmPrimeBrokerShortName.GotFocus += new System.EventHandler(this.txtClearingFirmPrimeBrokerShortName_GotFocus);
			// 
			// lblClearingFirmsPrimeBrokersSName
			// 
			this.lblClearingFirmsPrimeBrokersSName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblClearingFirmsPrimeBrokersSName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblClearingFirmsPrimeBrokersSName.Location = new System.Drawing.Point(16, 41);
			this.lblClearingFirmsPrimeBrokersSName.Name = "lblClearingFirmsPrimeBrokersSName";
			this.lblClearingFirmsPrimeBrokersSName.Size = new System.Drawing.Size(62, 15);
			this.lblClearingFirmsPrimeBrokersSName.TabIndex = 9;
			this.lblClearingFirmsPrimeBrokersSName.Text = "ShortName";
			this.lblClearingFirmsPrimeBrokersSName.Click += new System.EventHandler(this.lblClearingFirmsPrimeBrokersSName_Click);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(78, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 8);
			this.label1.TabIndex = 8;
			this.label1.Text = "*";
			// 
			// txtClearingFirmsPrimeBroker
			// 
			this.txtClearingFirmsPrimeBroker.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtClearingFirmsPrimeBroker.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtClearingFirmsPrimeBroker.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtClearingFirmsPrimeBroker.Location = new System.Drawing.Point(114, 16);
			this.txtClearingFirmsPrimeBroker.MaxLength = 50;
			this.txtClearingFirmsPrimeBroker.Name = "txtClearingFirmsPrimeBroker";
			this.txtClearingFirmsPrimeBroker.Size = new System.Drawing.Size(154, 21);
			this.txtClearingFirmsPrimeBroker.TabIndex = 2;
			this.txtClearingFirmsPrimeBroker.Text = "";
			this.txtClearingFirmsPrimeBroker.TextChanged += new System.EventHandler(this.txtClearingFirmsPrimeBroker_TextChanged);
			this.txtClearingFirmsPrimeBroker.LostFocus += new System.EventHandler(this.txtClearingFirmsPrimeBroker_LostFocus);
			this.txtClearingFirmsPrimeBroker.GotFocus += new System.EventHandler(this.txtClearingFirmsPrimeBroker_GotFocus);
			// 
			// lblClearingFirmsPrimeBrokersShortName
			// 
			this.lblClearingFirmsPrimeBrokersShortName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblClearingFirmsPrimeBrokersShortName.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblClearingFirmsPrimeBrokersShortName.Location = new System.Drawing.Point(16, 20);
			this.lblClearingFirmsPrimeBrokersShortName.Name = "lblClearingFirmsPrimeBrokersShortName";
			this.lblClearingFirmsPrimeBrokersShortName.Size = new System.Drawing.Size(62, 15);
			this.lblClearingFirmsPrimeBrokersShortName.TabIndex = 0;
			this.lblClearingFirmsPrimeBrokersShortName.Text = "Name";
			this.lblClearingFirmsPrimeBrokersShortName.Click += new System.EventHandler(this.lblClearingFirmsPrimeBrokersShortName_Click);
			// 
			// grdClearingFirmsPrimeBrokers
			// 
			this.grdClearingFirmsPrimeBrokers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 229;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.Caption = "Name";
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Width = 114;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.CellAppearance = appearance3;
			appearance4.FontData.BoldAsString = "True";
			appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.Header.Appearance = appearance4;
			ultraGridColumn3.Header.Caption = "ShortName";
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Width = 163;
			ultraGridColumn4.Header.VisiblePosition = 3;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn4.Width = 76;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3,
															 ultraGridColumn4});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.GroupByBox.Hidden = true;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.MaxColScrollRegions = 1;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdClearingFirmsPrimeBrokers.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdClearingFirmsPrimeBrokers.FlatMode = true;
			this.grdClearingFirmsPrimeBrokers.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdClearingFirmsPrimeBrokers.Location = new System.Drawing.Point(8, 2);
			this.grdClearingFirmsPrimeBrokers.Name = "grdClearingFirmsPrimeBrokers";
			this.grdClearingFirmsPrimeBrokers.Size = new System.Drawing.Size(298, 127);
			this.grdClearingFirmsPrimeBrokers.TabIndex = 93;
			// 
			// ClearingFirmsPrimeBrokers
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(310, 247);
			this.Controls.Add(this.grdClearingFirmsPrimeBrokers);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpClearingFirmPrimeBokers);
			this.Controls.Add(this.btnEdit);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MinimumSize = new System.Drawing.Size(318, 274);
			this.Name = "ClearingFirmsPrimeBrokers";
			this.Text = "ClearingFirmsPrimeBrokers";
			this.Load += new System.EventHandler(this.ClearingFirmsPrimeBrokers_Load);
			this.grpClearingFirmPrimeBokers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdClearingFirmsPrimeBrokers)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ClearingFirmsPrimeBrokers_Load(object sender, System.EventArgs e)
		{
			BindClearingFirmPrimeBrokerGrid();
		}

		private void BindClearingFirmPrimeBrokerGrid()
		{
			//this.dataGridTableStyle1.MappingName = "clearingFirmsPrimeBrokers";

			//Fetching the existing clearingFirmPrimeBrokers from the database and binding it to the grid.
			Nirvana.Admin.BLL.ClearingFirmsPrimeBrokers clearingFirmsPrimeBrokers = CompanyManager.GetClearingFirmsPrimeBrokersNew();
			//Assigning the grid's datasource to the symbolIdentifiers object.
			grdClearingFirmsPrimeBrokers.DataSource = clearingFirmsPrimeBrokers;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the ClearingFirmsPrimeBroker in the database.
				int checkID = SaveClearingFirmPrimeBroker();						
				//Binding grid after saving a clearingFirmsPrimeBroker.
				BindClearingFirmPrimeBrokerGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a ClearingFirmsPrimeBroker after checking that whether the 
					//ClearingFirmsPrimeBroker is saved successfully or not by checking for the positive value of 
					//checkID.
					RefreshForm();
				}
			}
			
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnSave_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		/// <summary>
		/// This method saves the <see cref="ClearingFirmPrimeBroker"/> to the database.
		/// </summary>
		/// <returns>ClearingFirmPrimeBroker, saved to the database</returns>
		private int SaveClearingFirmPrimeBroker()
		{	
			errorProvider1.SetError(txtClearingFirmsPrimeBroker, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.ClearingFirmPrimeBroker clearingFirmPrimeBroker = new Nirvana.Admin.BLL.ClearingFirmPrimeBroker();
			
			if(txtClearingFirmsPrimeBroker.Tag != null) 
			{
				//Update
				clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersID = int.Parse(txtClearingFirmsPrimeBroker.Tag.ToString());
			}
			//Validation to check for the empty value in ClearingFirmPrimeBroker textbox
			if(txtClearingFirmsPrimeBroker.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtClearingFirmsPrimeBroker, "Provide Value!");
				return result;
			}
			else
			{
				clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersName = txtClearingFirmsPrimeBroker.Text.Trim();
				errorProvider1.SetError(txtClearingFirmsPrimeBroker, "");
			}
			clearingFirmPrimeBroker.ClearingFirmsPrimeBrokersShortName = txtClearingFirmPrimeBrokerShortName.Text.Trim();
			
			//Saving the ClearingFirmPrimeBroker data and retrieving the ClearingFirmPrimeBroker for the newly added ClearingFirmPrimeBroker.
			int newClearingFirmPrimeBrokerID = CompanyManager.SaveClearingFirmPrimeBrokerNew(clearingFirmPrimeBroker);
			//Showing the message: ClearingFirmPrimeBroker already existing by checking the clearingFirmPrimeBroker id value to -1 
			if(newClearingFirmPrimeBrokerID == -1)
			{
				errorProvider1.SetError(btnSave, "ClearingFirmPrimeBroker Already Exists");
			}
				//Showing the message : ClearingFirmPrimeBroker data saved
			else
			{
				txtClearingFirmsPrimeBroker.Tag = null;
			}
			result = newClearingFirmPrimeBrokerID;
			//Returning the newly added ClearingFirmPrimeBroker id.
			return result;
		}

		//This method blanks the textboxes in the ClearingFirmPrimeBroker form.
		private void RefreshForm()
		{
			txtClearingFirmsPrimeBroker.Text = "";
			txtClearingFirmPrimeBrokerShortName.Text = "";
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBroker.Text = "";
			txtClearingFirmsPrimeBroker.Tag = null;
			txtClearingFirmPrimeBrokerShortName.Text = "";
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the ClearingFirmPrimeBroker if the grid has any ClearingFirmPrimeBroker.
				if (grdClearingFirmsPrimeBrokers.Rows.Count> 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the clearingFirmPrimeBroker and clearingFirmPrimeBrokerID.
					txtClearingFirmsPrimeBroker.Text = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersName"].Text.ToString();
					txtClearingFirmsPrimeBroker.Tag = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersID"].Text.ToString();
					txtClearingFirmPrimeBrokerShortName.Text = grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersShortName"].Text.ToString();
				}
				else
				{
					//Showing the message: No Data Available.
					errorProvider1.SetError(btnEdit, "No Data Available to edit.");
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnEdit_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnEdit_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Check for deleting the clearingFirmPrimeBroker if the grid has any clearingFirmPrimeBroker.
				if(grdClearingFirmsPrimeBrokers.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the clearingFirmPrimeBroker.
					if(MessageBox.Show(this, "Do you want to delete this ClearingFirmPrimeBroker?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the clearingFirmPrimeBrokerid from the currently selected row in the grid.
						int clearingFirmPrimeBrokerID = int.Parse(grdClearingFirmsPrimeBrokers.ActiveRow.Cells["ClearingFirmsPrimeBrokersID"].Text.ToString());				
						
						bool chkVarraible = CompanyManager.DeleteClearingFirmsPrimeBrokers(clearingFirmPrimeBrokerID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "ClearingFirmPrimeBroker is referenced in Company ClearingFirmPrimeBrokers.\n You can not delete it.", "Nirvana Alert");
						}
						else
						{
							BindClearingFirmPrimeBrokerGrid();
						}
							
					}
				}
				else
				{
					//Showing the message: No Data Available.
					errorProvider1.SetError(btnDelete, "No Data Available to delete.");
				}
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, Common.LOG_CATEGORY_EXCEPTION, 1, 1, Severity.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler();
				appMessageExceptionHandler.HandleException(new Exception(Common.ERROR_STATEMENT), Common.POLICY_GLOBAL, System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnDelete_Click", 
					Common.LOG_CATEGORY_UI, 1, 1, Severity.Information, 
					FORM_NAME + "btnSave_Click"); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void lblClearingFirmsPrimeBrokersSName_Click(object sender, System.EventArgs e)
		{
		
		}

		private void txtClearingFirmsPrimeBroker_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void lblClearingFirmsPrimeBrokersShortName_Click(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void txtClearingFirmPrimeBrokerShortName_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		#region Focus Colors
		private void txtClearingFirmsPrimeBroker_GotFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBroker.BackColor = Color.LemonChiffon;
		}
		private void txtClearingFirmsPrimeBroker_LostFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmsPrimeBroker.BackColor = Color.White;
		} 
		private void txtClearingFirmPrimeBrokerShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmPrimeBrokerShortName.BackColor = Color.LemonChiffon;
		}
		private void txtClearingFirmPrimeBrokerShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtClearingFirmPrimeBrokerShortName.BackColor = Color.White;
		} 
		#endregion

	}
}
