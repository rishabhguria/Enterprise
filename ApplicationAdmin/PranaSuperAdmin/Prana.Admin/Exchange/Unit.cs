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
	/// Summary description for Unit.
	/// </summary>
	public class Unit : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Unit : ";
		private const int GRD_Unit_ID = 0;
		private const int GRD_Unit = 1;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblUnit;
		private System.Windows.Forms.TextBox txtUnit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox grpUnit;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdUnit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Unit()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Unit));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("UnitName", 1);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			this.grpUnit = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtUnit = new System.Windows.Forms.TextBox();
			this.lblUnit = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grdUnit = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpUnit.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdUnit)).BeginInit();
			this.SuspendLayout();
			// 
			// grpUnit
			// 
			this.grpUnit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpUnit.Controls.Add(this.label1);
			this.grpUnit.Controls.Add(this.txtUnit);
			this.grpUnit.Controls.Add(this.lblUnit);
			this.grpUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpUnit.Location = new System.Drawing.Point(52, 186);
			this.grpUnit.Name = "grpUnit";
			this.grpUnit.Size = new System.Drawing.Size(240, 44);
			this.grpUnit.TabIndex = 30;
			this.grpUnit.TabStop = false;
			this.grpUnit.Text = "Add/Update Unit";
			this.grpUnit.Enter += new System.EventHandler(this.grpUnit_Enter);
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(78, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 8);
			this.label1.TabIndex = 8;
			this.label1.Text = "*";
			// 
			// txtUnit
			// 
			this.txtUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtUnit.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtUnit.Location = new System.Drawing.Point(92, 20);
			this.txtUnit.MaxLength = 50;
			this.txtUnit.Name = "txtUnit";
			this.txtUnit.Size = new System.Drawing.Size(130, 21);
			this.txtUnit.TabIndex = 2;
			this.txtUnit.Text = "";
			this.txtUnit.LostFocus += new System.EventHandler(this.txtUnit_LostFocus);
			this.txtUnit.GotFocus += new System.EventHandler(this.txtUnit_GotFocus);
			// 
			// lblUnit
			// 
			this.lblUnit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblUnit.Location = new System.Drawing.Point(16, 22);
			this.lblUnit.Name = "lblUnit";
			this.lblUnit.Size = new System.Drawing.Size(64, 15);
			this.lblUnit.TabIndex = 0;
			this.lblUnit.Text = "Unit Name";
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnReset.Location = new System.Drawing.Point(135, 230);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 36;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnEdit.Location = new System.Drawing.Point(101, 160);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 35;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnDelete.Location = new System.Drawing.Point(179, 160);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 34;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnClose.Location = new System.Drawing.Point(213, 230);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 33;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnSave.Location = new System.Drawing.Point(57, 230);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 32;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grdUnit
			// 
			this.grdUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdUnit.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 211;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Width = 311;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdUnit.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdUnit.DisplayLayout.GroupByBox.Hidden = true;
			this.grdUnit.DisplayLayout.MaxColScrollRegions = 1;
			this.grdUnit.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdUnit.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdUnit.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdUnit.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdUnit.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdUnit.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdUnit.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdUnit.FlatMode = true;
			this.grdUnit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdUnit.Location = new System.Drawing.Point(6, 0);
			this.grdUnit.Name = "grdUnit";
			this.grdUnit.Size = new System.Drawing.Size(332, 158);
			this.grdUnit.TabIndex = 53;
			// 
			// Unit
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(344, 255);
			this.Controls.Add(this.grdUnit);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpUnit);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(352, 282);
			this.Name = "Unit";
			this.Text = "Unit";
			this.Load += new System.EventHandler(this.Unit_Load);
			this.grpUnit.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdUnit)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
		private void  txtUnit_GotFocus(object sender, System.EventArgs e)
		{
			 txtUnit.BackColor = Color.LemonChiffon;
		}
		private void  txtUnit_LostFocus(object sender, System.EventArgs e)
		{
			 txtUnit.BackColor = Color.White;
		} 
		
		#endregion

		private void BindUnitsGrid()
		{
			//this.dataGridTableStyle1.MappingName = "units";

			//Fetching the existing units from the database and binding it to the grid.
			Units units = AUECManager.GetUnits();
			//Assigning the grid's datasource to the units object.
			grdUnit.DataSource = units;
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Unit_Load(object sender, System.EventArgs e)
		{
			BindUnitsGrid();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the Unit in the database.
				int checkID = SaveUnit();						
				//Binding grid after saving the a Unit.
				BindUnitsGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a Unit after checking that whether the 
					//Unit is saved successfully or not by checking for the positive value of 
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
		/// This method saves the <see cref="Unit"/> to the database.
		/// </summary>
		/// <returns>UnitID, saved to the database</returns>
		private int SaveUnit()
		{	
			errorProvider1.SetError(txtUnit, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.Unit unit = new Nirvana.Admin.BLL.Unit();
			
			if(txtUnit.Tag != null) 
			{
				//Update
				unit.UnitID = int.Parse(txtUnit.Tag.ToString());
			}
			//Validation to check for the empty value in Unit textbox
			if(txtUnit.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtUnit, "Provide Value!");
				return result;
			}
			else
			{
				unit.UnitName = txtUnit.Text.Trim();
				errorProvider1.SetError(txtUnit, "");
			}
			
			
			//Saving the unit data and retrieving the unitid for the newly added unit.
			int newUnitID = AUECManager.SaveUnit(unit);
			//Showing the message: Unit already existing by checking the unit id value to -1 
			if(newUnitID == -1)
			{
				errorProvider1.SetError(btnSave, "Unit Already Exists");
			}
				//Showing the message : Unit data saved
			else
			{
				txtUnit.Tag = null;
			}
			result = newUnitID;
			//Returning the newly added unit id.
			return result;
		}
		//This method blanks the textboxes in the Unit form.
		private void RefreshForm()
		{
			txtUnit.Text = "";
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtUnit.Text = "";
			txtUnit.Tag = null;
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the Unit if the grid has any Unit.
				if (grdUnit.Rows.Count> 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the unit and unitID.
					txtUnit.Text = grdUnit.ActiveRow.Cells["UnitName"].Text.ToString();
					txtUnit.Tag = grdUnit.ActiveRow.Cells["UnitID"].Text.ToString();
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
				//Check for deleting the unit if the grid has any unit.
				if(grdUnit.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the unit.
					if(MessageBox.Show(this, "Do you want to delete this Unit?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the unitid from the currently selected row in the grid.
						int unitID = int.Parse(grdUnit.ActiveRow.Cells["UnitID"].Text.ToString());				
						
						bool chkVarraible = AUECManager.DeleteUnit(unitID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "Unit is referenced in Exchange/AUEC Exchange.\n Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							BindUnitsGrid();
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

		private void grpUnit_Enter(object sender, System.EventArgs e)
		{
		
		}

	}
}
