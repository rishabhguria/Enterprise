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
	/// Summary description for ExecutionInstruction.
	/// </summary>
	public class ExecutionInstruction : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "ExecutionInstruction : ";
		private const int GRD_EXECUTIONINSTRUCTION_ID = 0;
		private const int GRD_EXECUTIONINSTRUCTION = 1;
		private const int GRD_EXECUTIONINSTRUCTION_VALUETAG = 2;
		
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtExecutionInstruction;
		private System.Windows.Forms.TextBox txtExecutionInstructionTagValue;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label lblExecutionInstructionTagValue;
		private System.Windows.Forms.Label lblExecutionInstruction;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdExecutionInstructions;
		private System.Windows.Forms.GroupBox grpExecutionInstruction;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ExecutionInstruction()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExecutionInstruction));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructions", 1);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExecutionInstructionsTagValue", 2);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 3);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TagValue", 4);
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grpExecutionInstruction = new System.Windows.Forms.GroupBox();
			this.txtExecutionInstructionTagValue = new System.Windows.Forms.TextBox();
			this.lblExecutionInstructionTagValue = new System.Windows.Forms.Label();
			this.txtExecutionInstruction = new System.Windows.Forms.TextBox();
			this.lblExecutionInstruction = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grdExecutionInstructions = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpExecutionInstruction.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdExecutionInstructions)).BeginInit();
			this.SuspendLayout();
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
			this.btnReset.TabIndex = 29;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnEdit.Location = new System.Drawing.Point(96, 136);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 28;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnDelete.Location = new System.Drawing.Point(174, 136);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 27;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
			this.btnClose.TabIndex = 26;
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
			this.btnSave.TabIndex = 25;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpExecutionInstruction
			// 
			this.grpExecutionInstruction.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpExecutionInstruction.Controls.Add(this.txtExecutionInstructionTagValue);
			this.grpExecutionInstruction.Controls.Add(this.lblExecutionInstructionTagValue);
			this.grpExecutionInstruction.Controls.Add(this.txtExecutionInstruction);
			this.grpExecutionInstruction.Controls.Add(this.lblExecutionInstruction);
			this.grpExecutionInstruction.Controls.Add(this.label3);
			this.grpExecutionInstruction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpExecutionInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpExecutionInstruction.Location = new System.Drawing.Point(10, 162);
			this.grpExecutionInstruction.Name = "grpExecutionInstruction";
			this.grpExecutionInstruction.Size = new System.Drawing.Size(325, 67);
			this.grpExecutionInstruction.TabIndex = 23;
			this.grpExecutionInstruction.TabStop = false;
			this.grpExecutionInstruction.Text = "Add/Update ExecutionInstruction";
			// 
			// txtExecutionInstructionTagValue
			// 
			this.txtExecutionInstructionTagValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtExecutionInstructionTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtExecutionInstructionTagValue.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtExecutionInstructionTagValue.Location = new System.Drawing.Point(144, 42);
			this.txtExecutionInstructionTagValue.MaxLength = 50;
			this.txtExecutionInstructionTagValue.Name = "txtExecutionInstructionTagValue";
			this.txtExecutionInstructionTagValue.Size = new System.Drawing.Size(164, 21);
			this.txtExecutionInstructionTagValue.TabIndex = 9;
			this.txtExecutionInstructionTagValue.Text = "";
			this.txtExecutionInstructionTagValue.LostFocus += new System.EventHandler(this.txtExecutionInstructionTagValue_LostFocus);
			this.txtExecutionInstructionTagValue.GotFocus += new System.EventHandler(this.txtExecutionInstructionTagValue_GotFocus);
			// 
			// lblExecutionInstructionTagValue
			// 
			this.lblExecutionInstructionTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblExecutionInstructionTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblExecutionInstructionTagValue.Location = new System.Drawing.Point(12, 45);
			this.lblExecutionInstructionTagValue.Name = "lblExecutionInstructionTagValue";
			this.lblExecutionInstructionTagValue.Size = new System.Drawing.Size(52, 15);
			this.lblExecutionInstructionTagValue.TabIndex = 8;
			this.lblExecutionInstructionTagValue.Text = "TagValue";
			// 
			// txtExecutionInstruction
			// 
			this.txtExecutionInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtExecutionInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtExecutionInstruction.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtExecutionInstruction.Location = new System.Drawing.Point(144, 20);
			this.txtExecutionInstruction.MaxLength = 50;
			this.txtExecutionInstruction.Name = "txtExecutionInstruction";
			this.txtExecutionInstruction.Size = new System.Drawing.Size(164, 21);
			this.txtExecutionInstruction.TabIndex = 2;
			this.txtExecutionInstruction.Text = "";
			this.txtExecutionInstruction.LostFocus += new System.EventHandler(this.txtExecutionInstruction_LostFocus);
			this.txtExecutionInstruction.GotFocus += new System.EventHandler(this.txtExecutionInstruction_GotFocus);
			// 
			// lblExecutionInstruction
			// 
			this.lblExecutionInstruction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblExecutionInstruction.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblExecutionInstruction.Location = new System.Drawing.Point(14, 22);
			this.lblExecutionInstruction.Name = "lblExecutionInstruction";
			this.lblExecutionInstruction.Size = new System.Drawing.Size(115, 15);
			this.lblExecutionInstruction.TabIndex = 0;
			this.lblExecutionInstruction.Text = "Execution Instructions";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(128, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 10);
			this.label3.TabIndex = 7;
			this.label3.Text = "*";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grdExecutionInstructions
			// 
			this.grdExecutionInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdExecutionInstructions.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Width = 159;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.CellAppearance = appearance3;
			appearance4.FontData.BoldAsString = "True";
			appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.Header.Appearance = appearance4;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Hidden = true;
			ultraGridColumn3.Width = 178;
			ultraGridColumn4.Header.VisiblePosition = 3;
			ultraGridColumn4.Hidden = true;
			appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn5.CellAppearance = appearance5;
			appearance6.FontData.BoldAsString = "True";
			appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn5.Header.Appearance = appearance6;
			ultraGridColumn5.Header.VisiblePosition = 4;
			ultraGridColumn5.Width = 153;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3,
															 ultraGridColumn4,
															 ultraGridColumn5});
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdExecutionInstructions.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdExecutionInstructions.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdExecutionInstructions.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdExecutionInstructions.FlatMode = true;
			this.grdExecutionInstructions.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdExecutionInstructions.Location = new System.Drawing.Point(5, 0);
			this.grdExecutionInstructions.Name = "grdExecutionInstructions";
			this.grdExecutionInstructions.Size = new System.Drawing.Size(333, 134);
			this.grdExecutionInstructions.TabIndex = 31;
			this.grdExecutionInstructions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdExecutionInstructions1_InitializeLayout);
			// 
			// ExecutionInstruction
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(344, 255);
			this.Controls.Add(this.grdExecutionInstructions);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpExecutionInstruction);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(352, 282);
			this.Name = "ExecutionInstruction";
			this.Text = "ExecutionInstruction";
			this.Load += new System.EventHandler(this.ExecutionInstruction_Load);
			this.grpExecutionInstruction.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdExecutionInstructions)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
		private void txtExecutionInstruction_GotFocus(object sender, System.EventArgs e)
		{
			txtExecutionInstruction.BackColor = Color.LemonChiffon;
		}
		private void txtExecutionInstruction_LostFocus(object sender, System.EventArgs e)
		{
			txtExecutionInstruction.BackColor = Color.White;
		} 
		private void txtExecutionInstructionTagValue_GotFocus(object sender, System.EventArgs e)
		{
			txtExecutionInstructionTagValue.BackColor = Color.LemonChiffon;
		}
		private void txtExecutionInstructionTagValue_LostFocus(object sender, System.EventArgs e)
		{
			txtExecutionInstructionTagValue.BackColor = Color.White;
		} 

		#endregion
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ExecutionInstruction_Load(object sender, System.EventArgs e)
		{
			BindExecutionInstructionsGrid();
		}

		private void BindExecutionInstructionsGrid()
		{
			//this.dataGridTableStyle1.MappingName = "executionInstructions";

			//Fetching the existing executionInstructions from the database and binding it to the grid.
			ExecutionInstructions executionInstructions = OrderManager.GetExecutionInstructions();
			//Assigning the grid's datasource to the executionInstructions object.
			grdExecutionInstructions.DataSource = executionInstructions;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the ExecutionInstruction in the database.
				int checkID = SaveExecutionInstruction();						
				//Binding grid after saving the a ExecutionInstruction.
				BindExecutionInstructionsGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a ExecutionInstruction after checking that whether the 
					//ExecutionInstruction is saved successfully or not by checking for the positive value of 
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

		//This method blanks the textboxes in the ExecutionInstruction form.
		private void RefreshForm()
		{
			txtExecutionInstruction.Text = "";
			txtExecutionInstructionTagValue.Text = "";
		}

		/// <summary>
		/// This method saves the <see cref="ExecutionInstruction"/> to the database.
		/// </summary>
		/// <returns>ExecutionInstructionID, saved to the database</returns>
		private int SaveExecutionInstruction()
		{	
			errorProvider1.SetError(txtExecutionInstruction, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.ExecutionInstruction executionInstruction = new Nirvana.Admin.BLL.ExecutionInstruction();
			
			if(txtExecutionInstruction.Tag != null) 
			{
				//Update
				executionInstruction.ExecutionInstructionsID = int.Parse(txtExecutionInstruction.Tag.ToString());
			}
			//Validation to check for the empty value in ExecutionInstruction textbox
			if(txtExecutionInstruction.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtExecutionInstruction, "Provide Value!");
				return result;
			}
			else
			{
				executionInstruction.ExecutionInstructions = txtExecutionInstruction.Text.Trim();
				errorProvider1.SetError(txtExecutionInstruction, "");
			}
			executionInstruction.TagValue = txtExecutionInstructionTagValue.Text;
			
			//Saving the executionInstruction data and retrieving the executionInstructionid for the newly added executionInstruction.
			int newExecutionInstructionID = OrderManager.SaveExecutionInstruction(executionInstruction);
			//Showing the message: ExecutionInstruction already existing by checking the executionInstruction id value to -1 
			if(newExecutionInstructionID == -1)
			{
				errorProvider1.SetError(btnSave, "ExecutionInstruction Already Exists");
			}
				//Showing the message : ExecutionInstruction data saved
			else
			{
				//errorProvider1.SetError(txtExecutionInstruction, "ExecutionInstruction Saved");
				txtExecutionInstruction.Tag = null;
			}
			result = newExecutionInstructionID;
			//Returning the newly added executionInstruction id.
			return result;
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtExecutionInstruction.Text = "";
			txtExecutionInstruction.Tag = null;
			txtExecutionInstructionTagValue.Text = "";
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the ExecutionInstruction if the grid has any ExecutionInstruction.
				if (grdExecutionInstructions.Rows.Count > 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the executionInstruction and executionInstructionID.
					txtExecutionInstruction.Text = grdExecutionInstructions.ActiveRow.Cells["ExecutionInstructions" ].Text.ToString();
					txtExecutionInstruction.Tag =   grdExecutionInstructions.ActiveRow.Cells["ExecutionInstructionsID"].Text.ToString();
					txtExecutionInstructionTagValue.Text =  grdExecutionInstructions.ActiveRow.Cells["TagValue"].Text.ToString();
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
				//Check for deleting the executionInstruction if the grid has any executionInstruction.
				if(grdExecutionInstructions.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the executionInstruction.
					if(MessageBox.Show(this, "Do you want to delete this ExecutionInstruction?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the executionInstructionid from the currently selected row in the grid.
						int executionInstructionID = int.Parse(grdExecutionInstructions.ActiveRow.Cells["ExecutionInstructionsID"].Value.ToString());			
						
						bool chkVarraible = OrderManager.DeleteExecutionInstruction(executionInstructionID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "ExecutionInstruction is referred in CounterpartyVenue.\n Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							BindExecutionInstructionsGrid();
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

		private void grdExecutionInstructions1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
		
		}

		private void label3_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
