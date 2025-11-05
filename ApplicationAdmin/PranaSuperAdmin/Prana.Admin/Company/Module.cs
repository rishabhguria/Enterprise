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
	/// Summary description for Module.
	/// </summary>
	public class Module : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Module : ";
		private const int GRD_MODULE_ID = 0;
		private const int GRD_MODULE = 1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtModule;
		private System.Windows.Forms.Label lblModule;
		private System.Windows.Forms.GroupBox grpModule;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdModule;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Module()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Module));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ModuleID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ModuleName", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, false);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyID", 2);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CompanyModuleID", 3);
			this.btnSave = new System.Windows.Forms.Button();
			this.grpModule = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtModule = new System.Windows.Forms.TextBox();
			this.lblModule = new System.Windows.Forms.Label();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.grdModule = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpModule.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdModule)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnSave.Location = new System.Drawing.Point(24, 210);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 60;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpModule
			// 
			this.grpModule.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpModule.Controls.Add(this.label1);
			this.grpModule.Controls.Add(this.txtModule);
			this.grpModule.Controls.Add(this.lblModule);
			this.grpModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpModule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpModule.Location = new System.Drawing.Point(12, 162);
			this.grpModule.Name = "grpModule";
			this.grpModule.Size = new System.Drawing.Size(255, 46);
			this.grpModule.TabIndex = 58;
			this.grpModule.TabStop = false;
			this.grpModule.Text = "Add/Update Module";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(94, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 8);
			this.label1.TabIndex = 8;
			this.label1.Text = "*";
			// 
			// txtModule
			// 
			this.txtModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtModule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtModule.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtModule.Location = new System.Drawing.Point(108, 22);
			this.txtModule.MaxLength = 50;
			this.txtModule.Name = "txtModule";
			this.txtModule.Size = new System.Drawing.Size(130, 21);
			this.txtModule.TabIndex = 2;
			this.txtModule.Text = "";
			this.txtModule.LostFocus += new System.EventHandler(this.txtModule_LostFocus);
			this.txtModule.GotFocus += new System.EventHandler(this.txtModule_GotFocus);
			// 
			// lblModule
			// 
			this.lblModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblModule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblModule.Location = new System.Drawing.Point(16, 25);
			this.lblModule.Name = "lblModule";
			this.lblModule.Size = new System.Drawing.Size(78, 15);
			this.lblModule.TabIndex = 0;
			this.lblModule.Text = "Module Name";
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnEdit.Location = new System.Drawing.Point(63, 136);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 63;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnReset.Location = new System.Drawing.Point(102, 210);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 64;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnDelete.Location = new System.Drawing.Point(141, 136);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 62;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnClose.Location = new System.Drawing.Point(180, 210);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 61;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// grdModule
			// 
			this.grdModule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdModule.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 211;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Width = 249;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Hidden = true;
			ultraGridColumn3.Width = 68;
			ultraGridColumn4.Header.VisiblePosition = 3;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn4.Width = 69;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3,
															 ultraGridColumn4});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdModule.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdModule.DisplayLayout.GroupByBox.Hidden = true;
			this.grdModule.DisplayLayout.MaxColScrollRegions = 1;
			this.grdModule.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdModule.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdModule.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdModule.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdModule.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdModule.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdModule.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdModule.FlatMode = true;
			this.grdModule.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdModule.Location = new System.Drawing.Point(6, 0);
			this.grdModule.Name = "grdModule";
			this.grdModule.Size = new System.Drawing.Size(270, 134);
			this.grdModule.TabIndex = 65;
			// 
			// Module
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(278, 235);
			this.Controls.Add(this.grdModule);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpModule);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(286, 262);
			this.Name = "Module";
			this.Text = "Module";
			this.Load += new System.EventHandler(this.Module_Load);
			this.grpModule.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdModule)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
		private void txtModule_GotFocus(object sender, System.EventArgs e)
		{
			txtModule.BackColor = Color.LemonChiffon;
		}
		private void txtModule_LostFocus(object sender, System.EventArgs e)
		{
			txtModule.BackColor = Color.White;
		} 
		
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Module_Load(object sender, System.EventArgs e)
		{
			BindModuleGrid();
		}

		private void BindModuleGrid()
		{
			//this.dataGridTableStyle1.MappingName = "modules";

			//Fetching the existing modules from the database and binding it to the grid.
			Modules modules = ModuleManager.GetModules();
			//Assigning the grid's datasource to the modules object.
			grdModule.DataSource = modules;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the Module in the database.
				int checkID = SaveModule();						
				//Binding grid after saving the a Module.
				BindModuleGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a Module after checking that whether the 
					//Module is saved successfully or not by checking for the positive value of 
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
		/// This method saves the <see cref="Module"/> to the database.
		/// </summary>
		/// <returns>ModuleID, saved to the database</returns>
		private int SaveModule()
		{	
			errorProvider1.SetError(txtModule, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.Module module = new Nirvana.Admin.BLL.Module();
			
			if(txtModule.Tag != null) 
			{
				//Update
				module.ModuleID = int.Parse(txtModule.Tag.ToString());
			}
			//Validation to check for the empty value in Module textbox
			if(txtModule.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtModule, "Provide Value!");
				return result;
			}
			else
			{
				module.ModuleName = txtModule.Text.Trim();
				errorProvider1.SetError(txtModule, "");
			}
			
			
			//Saving the Module data and retrieving the moduleid for the newly added Module.
			int newModuleID = ModuleManager.SaveModule(module);
			//Showing the message: Module already existing by checking the Module id value to -1 
			if(newModuleID == -1)
			{
				errorProvider1.SetError(btnSave, "Module Already Exists");
			}
				//Showing the message : Module data saved
			else
			{
				txtModule.Tag = null;
			}
			result = newModuleID;
			//Returning the newly added Module id.
			return result;
		}

		//This method blanks the textboxes in the Module form.
		private void RefreshForm()
		{
			txtModule.Text = "";
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtModule.Text = "";
			txtModule.Tag = null;
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the Module if the grid has any Module.
				if (grdModule.Rows.Count> 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the module and moduleID.
					txtModule.Text = grdModule.ActiveRow.Cells["ModuleName"].Text.ToString();
					txtModule.Tag = grdModule.ActiveRow.Cells["ModuleID"].Text.ToString();
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
				//Check for deleting the module if the grid has any module.
				if(grdModule.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the module.
					if(MessageBox.Show(this, "Do you want to delete this Module?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the moduleid from the currently selected row in the grid.
						int moduleID = int.Parse(grdModule.ActiveRow.Cells["ModuleID"].Text.ToString());				
						
						bool chkVarraible = ModuleManager.DeleteModule(moduleID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "ModuleID is referenced in CompanyModule/User Module.\n Please remove references first to delete it..", "Nirvana Alert");
						}
						else
						{
							BindModuleGrid();
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
	}
}
