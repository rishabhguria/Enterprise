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
	/// Summary description for ContractListingType.
	/// </summary>
	public class ContractListingType : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "ContractListingType : ";
		private const int GRD_CONTRACTLISTING_TYPE_ID = 0;
		private const int GRD_CONTRACTLISTING_TYPE = 1;
		
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblCurrencyType;
		private System.Windows.Forms.TextBox txtContractListingType;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdContractListingType;
		private System.Windows.Forms.GroupBox grpContractlistingtype;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ContractListingType()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ContractListingType));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContractListingTypeID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ContractListingTypeName", 1);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Type", 2, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grpContractlistingtype = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtContractListingType = new System.Windows.Forms.TextBox();
			this.lblCurrencyType = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grdContractListingType = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpContractlistingtype.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdContractListingType)).BeginInit();
			this.SuspendLayout();
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnEdit.Location = new System.Drawing.Point(96, 156);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 98;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnReset.Location = new System.Drawing.Point(137, 230);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 99;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnDelete.Location = new System.Drawing.Point(174, 156);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 97;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(215, 230);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 96;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(59, 230);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 95;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpContractlistingtype
			// 
			this.grpContractlistingtype.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpContractlistingtype.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.grpContractlistingtype.Controls.Add(this.label1);
			this.grpContractlistingtype.Controls.Add(this.txtContractListingType);
			this.grpContractlistingtype.Controls.Add(this.lblCurrencyType);
			this.grpContractlistingtype.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpContractlistingtype.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpContractlistingtype.Location = new System.Drawing.Point(60, 184);
			this.grpContractlistingtype.Name = "grpContractlistingtype";
			this.grpContractlistingtype.Size = new System.Drawing.Size(229, 42);
			this.grpContractlistingtype.TabIndex = 93;
			this.grpContractlistingtype.TabStop = false;
			this.grpContractlistingtype.Text = "Add/Update ContractListingType";
			this.grpContractlistingtype.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(46, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 8);
			this.label1.TabIndex = 8;
			this.label1.Text = "*";
			// 
			// txtContractListingType
			// 
			this.txtContractListingType.BackColor = System.Drawing.Color.White;
			this.txtContractListingType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContractListingType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtContractListingType.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtContractListingType.Location = new System.Drawing.Point(58, 20);
			this.txtContractListingType.MaxLength = 20;
			this.txtContractListingType.Name = "txtContractListingType";
			this.txtContractListingType.Size = new System.Drawing.Size(154, 21);
			this.txtContractListingType.TabIndex = 2;
			this.txtContractListingType.Text = "";
			this.txtContractListingType.LostFocus += new System.EventHandler(this.txtContractListingType_LostFocus);
			this.txtContractListingType.GotFocus += new System.EventHandler(this.txtContractListingType_GotFocus);
			this.txtContractListingType.TextChanged += new System.EventHandler(this.txtContractListingType_TextChanged);
			// 
			// lblCurrencyType
			// 
			this.lblCurrencyType.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.lblCurrencyType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblCurrencyType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblCurrencyType.Location = new System.Drawing.Point(14, 22);
			this.lblCurrencyType.Name = "lblCurrencyType";
			this.lblCurrencyType.Size = new System.Drawing.Size(31, 15);
			this.lblCurrencyType.TabIndex = 0;
			this.lblCurrencyType.Text = "Type";
			this.lblCurrencyType.Click += new System.EventHandler(this.lblCurrencyType_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grdContractListingType
			// 
			this.grdContractListingType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdContractListingType.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 211;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Hidden = true;
			ultraGridColumn2.Width = 287;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.CellAppearance = appearance3;
			appearance4.FontData.BoldAsString = "True";
			appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.Header.Appearance = appearance4;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Width = 309;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdContractListingType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdContractListingType.DisplayLayout.GroupByBox.Hidden = true;
			this.grdContractListingType.DisplayLayout.MaxColScrollRegions = 1;
			this.grdContractListingType.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdContractListingType.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdContractListingType.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdContractListingType.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdContractListingType.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdContractListingType.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdContractListingType.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdContractListingType.FlatMode = true;
			this.grdContractListingType.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdContractListingType.Location = new System.Drawing.Point(7, 0);
			this.grdContractListingType.Name = "grdContractListingType";
			this.grdContractListingType.Size = new System.Drawing.Size(330, 152);
			this.grdContractListingType.TabIndex = 100;
			// 
			// ContractListingType
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(344, 255);
			this.Controls.Add(this.grdContractListingType);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpContractlistingtype);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.MinimumSize = new System.Drawing.Size(352, 282);
			this.Name = "ContractListingType";
			this.Text = "ContractListingType";
			this.Load += new System.EventHandler(this.ContractListingType_Load);
			this.grpContractlistingtype.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdContractListingType)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
		private void txtContractListingType_GotFocus(object sender, System.EventArgs e)
		{
			txtContractListingType.BackColor = Color.LemonChiffon;
		}
		private void txtContractListingType_LostFocus(object sender, System.EventArgs e)
		{
			txtContractListingType.BackColor = Color.White;
		} 
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Binds the grid with all the Contract Listing Types in the database.
		/// </summary>
		private void BindContractListingTypeGrid()
		{
			//this.dataGridTableStyle1.MappingName = "contractListingTypes";

			//Fetching the existing contractListingTypes from the database and binding it to the grid.
			ContractListingTypes contractListingTypes = FutureManager.GetContractListingTypes();
			//Assigning the grid's datasource to the contractListingTypes object.
			grdContractListingType.DataSource = contractListingTypes;
		}

		private void ContractListingType_Load(object sender, System.EventArgs e)
		{
			BindContractListingTypeGrid();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currently saved id of the ContractListingType in the database.
				int checkID = SaveContractListingType();						
				//Binding grid after saving a ContractListingType.
				BindContractListingTypeGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a ContractListingType after checking that whether the 
					//ContractListingType is saved successfully or not by checking for the positive value of 
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
		/// This method saves the <see cref="ContractListingType"/> to the database.
		/// </summary>
		/// <returns>ContractListingType, saved to the database</returns>
		private int SaveContractListingType()
		{	
			errorProvider1.SetError(txtContractListingType, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.ContractListingType contractListingType = new Nirvana.Admin.BLL.ContractListingType();
			
			if(txtContractListingType.Tag != null) 
			{
				//Update
				contractListingType.ContractListingTypeID = int.Parse(txtContractListingType.Tag.ToString());
			}
			//Validation to check for the empty value in ContractListingType textbox
			if(txtContractListingType.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtContractListingType, "Provide Value!");
				return result;
			}
			else
			{
				contractListingType.Type = txtContractListingType.Text.Trim();
				errorProvider1.SetError(txtContractListingType, "");
			}
						
			//Saving the ContractListingType data and retrieving the ContractListingType for the newly added ContractListingType.
			int newContractListingTypeID = FutureManager.SaveContractListingType(contractListingType);
			//Showing the message: ContractListingType already existing by checking the newContractListingType id value to -1 
			if(newContractListingTypeID == -1)
			{
				errorProvider1.SetError(btnSave, "ContractListingType Already Exists");
			}
				//Showing the message : ContractListingType data saved
			else
			{
				txtContractListingType.Tag = null;
			}
			result = newContractListingTypeID;
			//Returning the newly added ContractListingType id.
			return result;
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the ContractListingType if the grid has any ContractListingType.
				if (grdContractListingType.Rows.Count > 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the contractListingType and currencyTypID.
					txtContractListingType.Text = grdContractListingType.ActiveRow.Cells["Type"].Text.ToString();
					txtContractListingType.Tag = grdContractListingType.ActiveRow.Cells["ContractListingTypeID"].Text.ToString();
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

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtContractListingType.Text = "";
			txtContractListingType.Tag = null;
		}

		//This method blanks the textboxes in the ContractListingType form.
		private void RefreshForm()
		{
			txtContractListingType.Text = "";
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Check for deleting the CurrencyType if the grid has any contractListingType.
				if(grdContractListingType.Rows.Count> 0)
				{
					//Asking the user to be sure about deleting the contractListingType.
					if(MessageBox.Show(this, "Do you want to delete this ContractListingType?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the contractListingTypeid from the currently selected row in the grid.
						int contractListingTypeID = int.Parse(grdContractListingType.ActiveRow.Cells["ContractListingTypeID"].Text.ToString());				
						
						bool chkVarraible = FutureManager.DeleteContractListingType(contractListingTypeID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "ContractListingType is referenced in ContractListingType.\n Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							BindContractListingTypeGrid();
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

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void lblCurrencyType_Click(object sender, System.EventArgs e)
		{
		
		}

		private void txtContractListingType_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}
