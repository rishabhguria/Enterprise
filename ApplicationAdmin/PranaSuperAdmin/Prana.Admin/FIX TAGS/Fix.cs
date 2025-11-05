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
	/// Summary description for Fix.
	/// </summary>
	public class Fix : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Fix : ";
		private const int GRD_FIX_ID = 0;
		private const int GRD_FIX = 1;
		
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFix;
		private System.Windows.Forms.Label lblFix;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.GroupBox grpFix;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdFix;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Fix()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Fix));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("FixVersion", 1);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grpFix = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFix = new System.Windows.Forms.TextBox();
			this.lblFix = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grdFix = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpFix.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdFix)).BeginInit();
			this.SuspendLayout();
			// 
			// btnReset
			// 
			this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnReset.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(200)), ((System.Byte)(240)), ((System.Byte)(150)));
			this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnReset.Location = new System.Drawing.Point(135, 236);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 50;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnEdit.Location = new System.Drawing.Point(98, 166);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 49;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnDelete.Location = new System.Drawing.Point(175, 166);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 48;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnClose.Location = new System.Drawing.Point(213, 236);
			this.btnClose.Name = "btnClose";
			this.btnClose.TabIndex = 47;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnSave.Location = new System.Drawing.Point(57, 236);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 46;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpFix
			// 
			this.grpFix.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpFix.Controls.Add(this.label1);
			this.grpFix.Controls.Add(this.txtFix);
			this.grpFix.Controls.Add(this.lblFix);
			this.grpFix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpFix.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpFix.Location = new System.Drawing.Point(43, 192);
			this.grpFix.Name = "grpFix";
			this.grpFix.Size = new System.Drawing.Size(258, 44);
			this.grpFix.TabIndex = 44;
			this.grpFix.TabStop = false;
			this.grpFix.Text = "Add/Update Fix";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(72, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(8, 8);
			this.label1.TabIndex = 8;
			this.label1.Text = "*";
			// 
			// txtFix
			// 
			this.txtFix.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFix.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtFix.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtFix.Location = new System.Drawing.Point(86, 18);
			this.txtFix.MaxLength = 50;
			this.txtFix.Name = "txtFix";
			this.txtFix.Size = new System.Drawing.Size(156, 21);
			this.txtFix.TabIndex = 2;
			this.txtFix.Text = "";
			this.txtFix.LostFocus += new System.EventHandler(this.txtFix_LostFocus);
			this.txtFix.GotFocus += new System.EventHandler(this.txtFix_GotFocus);
			// 
			// lblFix
			// 
			this.lblFix.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblFix.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblFix.Location = new System.Drawing.Point(14, 21);
			this.lblFix.Name = "lblFix";
			this.lblFix.Size = new System.Drawing.Size(59, 15);
			this.lblFix.TabIndex = 0;
			this.lblFix.Text = "Fix Version";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grdFix
			// 
			this.grdFix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdFix.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 200;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Width = 309;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdFix.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdFix.DisplayLayout.GroupByBox.Hidden = true;
			this.grdFix.DisplayLayout.MaxColScrollRegions = 1;
			this.grdFix.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdFix.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdFix.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdFix.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdFix.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdFix.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdFix.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdFix.FlatMode = true;
			this.grdFix.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdFix.Location = new System.Drawing.Point(8, 2);
			this.grdFix.Name = "grdFix";
			this.grdFix.Size = new System.Drawing.Size(330, 162);
			this.grdFix.TabIndex = 52;
			// 
			// Fix
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(344, 261);
			this.Controls.Add(this.grdFix);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpFix);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(352, 282);
			this.Name = "Fix";
			this.Text = "Fix";
			this.Load += new System.EventHandler(this.Fix_Load);
			this.grpFix.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdFix)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
		#region Focus Colors
		private void txtFix_GotFocus(object sender, System.EventArgs e)
		{
			txtFix.BackColor = Color.LemonChiffon;
		}
		private void txtFix_LostFocus(object sender, System.EventArgs e)
		{
			txtFix.BackColor = Color.White;
		} 
		
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the Fix in the database.
				int checkID = SaveFix();						
				//Binding grid after saving the a Fix.
				BindFixGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a Fix after checking that whether the 
					//Fix is saved successfully or not by checking for the positive value of 
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

		private void Fix_Load(object sender, System.EventArgs e)
		{
			BindFixGrid();
		}

		private void BindFixGrid()
		{
			//this.dataGridTableStyle1.MappingName = "fixs";

			//Fetching the existing fixs from the database and binding it to the grid.
			Fixs fixs = Fixmanager.GetFixs();
			//Assigning the grid's datasource to the fixs object.
			grdFix.DataSource = fixs;
		}
		
		/// <summary>
		/// This method saves the <see cref="Fix"/> to the database.
		/// </summary>
		/// <returns>FixID, saved to the database</returns>
		private int SaveFix()
		{	
			errorProvider1.SetError(txtFix, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.Fix fix = new Nirvana.Admin.BLL.Fix();
			
			if(txtFix.Tag != null) 
			{
				//Update
				fix.FixID = int.Parse(txtFix.Tag.ToString());
			}
			//Validation to check for the empty value in Fix textbox
			if(txtFix.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtFix, "Provide Value!");
				return result;
			}
			else
			{
				fix.FixVersion = txtFix.Text.Trim();
				errorProvider1.SetError(txtFix, "");
			}
			
			
			//Saving the Fix data and retrieving the fixid for the newly added Fix.
			int newFixID = Fixmanager.SaveFix(fix);
			//Showing the message: Fix already existing by checking the fix id value to -1 
			if(newFixID == -1)
			{
				errorProvider1.SetError(btnSave, "Fix Already Exists");
			}
				//Showing the message : Fix data saved
			else
			{
				txtFix.Tag = null;
			}
			result = newFixID;
			//Returning the newly added Fix id.
			return result;
		}

		//This method blanks the textboxes in the Fix form.
		private void RefreshForm()
		{
			txtFix.Text = "";
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtFix.Text = "";
			txtFix.Tag = null;
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the Fix if the grid has any Fix.
				if (grdFix.Rows.Count > 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the fix and fixID.
					txtFix.Text = grdFix.ActiveRow.Cells["FixVersion"].Text.ToString();
					txtFix.Tag =  grdFix.ActiveRow.Cells["FixID"].Text.ToString();
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
				//Check for deleting the identifier if the grid has any identifier.
				if(grdFix.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the fix.
					if(MessageBox.Show(this, "Do you want to delete this Fix?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the fixid from the currently selected row in the grid.
						int fixID = int.Parse( grdFix.ActiveRow.Cells["FixID"].Value.ToString());				
						
						bool chkVarraible = Fixmanager.DeleteFix(fixID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "Fix is referenced in CounterpartyVenue.\n Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							BindFixGrid();
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
