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
	/// Summary description for TimeInForce.
	/// </summary>
	public class TimeInForce : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "TimeInForce : ";
		private const int GRD_TIMEINFORCE_ID = 0;
		private const int GRD_TIMEINFORCE = 1;
		private const int GRD_TIMEINFORCE_VALUETAG = 2;

		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtTimeInForceTagValue;
		private System.Windows.Forms.TextBox txtTimeInForce;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label lblTimeInForceTagValue;
		private System.Windows.Forms.Label lblTimeInForce;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdTimeInForce;
		private System.Windows.Forms.GroupBox grpTimeInForce;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TimeInForce()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(TimeInForce));
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceID", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("OrderTimeInForce", 1, null, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, false);
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeInForceTagValue", 2);
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("CVAUECID", 3);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Name", 4);
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("TagValue", 5);
			Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.grpTimeInForce = new System.Windows.Forms.GroupBox();
			this.txtTimeInForceTagValue = new System.Windows.Forms.TextBox();
			this.lblTimeInForceTagValue = new System.Windows.Forms.Label();
			this.txtTimeInForce = new System.Windows.Forms.TextBox();
			this.lblTimeInForce = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.grdTimeInForce = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.grpTimeInForce.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdTimeInForce)).BeginInit();
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
			this.btnReset.TabIndex = 43;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(192)), ((System.Byte)(192)), ((System.Byte)(255)));
			this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnEdit.Location = new System.Drawing.Point(96, 134);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 42;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(253)), ((System.Byte)(252)), ((System.Byte)(202)));
			this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.btnDelete.Location = new System.Drawing.Point(174, 134);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 41;
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
			this.btnClose.TabIndex = 40;
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
			this.btnSave.TabIndex = 39;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// grpTimeInForce
			// 
			this.grpTimeInForce.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.grpTimeInForce.Controls.Add(this.txtTimeInForceTagValue);
			this.grpTimeInForce.Controls.Add(this.lblTimeInForceTagValue);
			this.grpTimeInForce.Controls.Add(this.txtTimeInForce);
			this.grpTimeInForce.Controls.Add(this.lblTimeInForce);
			this.grpTimeInForce.Controls.Add(this.label3);
			this.grpTimeInForce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.grpTimeInForce.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
			this.grpTimeInForce.Location = new System.Drawing.Point(38, 160);
			this.grpTimeInForce.Name = "grpTimeInForce";
			this.grpTimeInForce.Size = new System.Drawing.Size(268, 68);
			this.grpTimeInForce.TabIndex = 37;
			this.grpTimeInForce.TabStop = false;
			this.grpTimeInForce.Text = "Add/Update TimeInForce";
			this.grpTimeInForce.Enter += new System.EventHandler(this.groupBox1_Enter);
			// 
			// txtTimeInForceTagValue
			// 
			this.txtTimeInForceTagValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTimeInForceTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTimeInForceTagValue.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtTimeInForceTagValue.Location = new System.Drawing.Point(86, 42);
			this.txtTimeInForceTagValue.MaxLength = 50;
			this.txtTimeInForceTagValue.Name = "txtTimeInForceTagValue";
			this.txtTimeInForceTagValue.Size = new System.Drawing.Size(164, 21);
			this.txtTimeInForceTagValue.TabIndex = 9;
			this.txtTimeInForceTagValue.Text = "";
			this.txtTimeInForceTagValue.LostFocus += new System.EventHandler(this.txtTimeInForceTagValue_LostFocus);
			this.txtTimeInForceTagValue.GotFocus += new System.EventHandler(this.txtTimeInForceTagValue_GotFocus);
			this.txtTimeInForceTagValue.TextChanged += new System.EventHandler(this.txtTimeInForceTagValue_TextChanged);
			// 
			// lblTimeInForceTagValue
			// 
			this.lblTimeInForceTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblTimeInForceTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblTimeInForceTagValue.Location = new System.Drawing.Point(16, 44);
			this.lblTimeInForceTagValue.Name = "lblTimeInForceTagValue";
			this.lblTimeInForceTagValue.Size = new System.Drawing.Size(62, 15);
			this.lblTimeInForceTagValue.TabIndex = 8;
			this.lblTimeInForceTagValue.Text = " TagValue";
			this.lblTimeInForceTagValue.Click += new System.EventHandler(this.lblTimeInForceTagValue_Click);
			// 
			// txtTimeInForce
			// 
			this.txtTimeInForce.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtTimeInForce.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.txtTimeInForce.ForeColor = System.Drawing.SystemColors.WindowText;
			this.txtTimeInForce.Location = new System.Drawing.Point(86, 20);
			this.txtTimeInForce.MaxLength = 50;
			this.txtTimeInForce.Name = "txtTimeInForce";
			this.txtTimeInForce.Size = new System.Drawing.Size(164, 21);
			this.txtTimeInForce.TabIndex = 2;
			this.txtTimeInForce.Text = "";
			this.txtTimeInForce.LostFocus += new System.EventHandler(this.txtTimeInForce_LostFocus);
			this.txtTimeInForce.GotFocus += new System.EventHandler(this.txtTimeInForce_GotFocus);
			this.txtTimeInForce.TextChanged += new System.EventHandler(this.txtTimeInForce_TextChanged);
			// 
			// lblTimeInForce
			// 
			this.lblTimeInForce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.lblTimeInForce.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.lblTimeInForce.Location = new System.Drawing.Point(16, 22);
			this.lblTimeInForce.Name = "lblTimeInForce";
			this.lblTimeInForce.Size = new System.Drawing.Size(34, 15);
			this.lblTimeInForce.TabIndex = 0;
			this.lblTimeInForce.Text = "Name";
			this.lblTimeInForce.Click += new System.EventHandler(this.lblTimeInForce_Click);
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(50, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(14, 8);
			this.label3.TabIndex = 7;
			this.label3.Text = "*";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// grdTimeInForce
			// 
			this.grdTimeInForce.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grdTimeInForce.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Hidden = true;
			ultraGridColumn1.Width = 273;
			appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.CellAppearance = appearance1;
			appearance2.FontData.BoldAsString = "True";
			appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn2.Header.Appearance = appearance2;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Hidden = true;
			ultraGridColumn2.Width = 152;
			appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.CellAppearance = appearance3;
			appearance4.FontData.BoldAsString = "True";
			appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn3.Header.Appearance = appearance4;
			ultraGridColumn3.Header.VisiblePosition = 2;
			ultraGridColumn3.Hidden = true;
			ultraGridColumn3.Width = 327;
			ultraGridColumn4.Header.VisiblePosition = 3;
			ultraGridColumn4.Hidden = true;
			ultraGridColumn4.Width = 74;
			appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn5.CellAppearance = appearance5;
			appearance6.FontData.BoldAsString = "True";
			appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn5.Header.Appearance = appearance6;
			ultraGridColumn5.Header.VisiblePosition = 4;
			ultraGridColumn5.Width = 160;
			appearance7.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn6.CellAppearance = appearance7;
			appearance8.FontData.BoldAsString = "True";
			appearance8.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGridColumn6.Header.Appearance = appearance8;
			ultraGridColumn6.Header.VisiblePosition = 5;
			ultraGridColumn6.Width = 157;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2,
															 ultraGridColumn3,
															 ultraGridColumn4,
															 ultraGridColumn5,
															 ultraGridColumn6});
			ultraGridBand1.Header.Enabled = false;
			ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
			this.grdTimeInForce.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.grdTimeInForce.DisplayLayout.GroupByBox.Hidden = true;
			this.grdTimeInForce.DisplayLayout.MaxColScrollRegions = 1;
			this.grdTimeInForce.DisplayLayout.MaxRowScrollRegions = 1;
			this.grdTimeInForce.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			this.grdTimeInForce.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
			this.grdTimeInForce.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.grdTimeInForce.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
			this.grdTimeInForce.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
			this.grdTimeInForce.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.grdTimeInForce.FlatMode = true;
			this.grdTimeInForce.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.grdTimeInForce.Location = new System.Drawing.Point(4, 0);
			this.grdTimeInForce.Name = "grdTimeInForce";
			this.grdTimeInForce.Size = new System.Drawing.Size(338, 132);
			this.grdTimeInForce.TabIndex = 44;
			// 
			// TimeInForce
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(344, 255);
			this.Controls.Add(this.grdTimeInForce);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.grpTimeInForce);
			this.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(352, 282);
			this.Name = "TimeInForce";
			this.Text = "TimeInForce";
			this.Load += new System.EventHandler(this.TimeInForce_Load);
			this.grpTimeInForce.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdTimeInForce)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Focus Colors
		private void txtTimeInForceTagValue_GotFocus(object sender, System.EventArgs e)
		{
			txtTimeInForceTagValue.BackColor = Color.LemonChiffon;
		}
		private void txtTimeInForceTagValue_LostFocus(object sender, System.EventArgs e)
		{
			txtTimeInForceTagValue.BackColor = Color.White;
		} 
		private void txtTimeInForce_GotFocus(object sender, System.EventArgs e)
		{
			txtTimeInForce.BackColor = Color.LemonChiffon;
		}
		private void txtTimeInForce_LostFocus(object sender, System.EventArgs e)
		{
			txtTimeInForce.BackColor = Color.White;
		} 

		
		#endregion

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void TimeInForce_Load(object sender, System.EventArgs e)
		{
			BindTimeInForceGrid();
		}

		private void BindTimeInForceGrid()
		{
			//this.dataGridTableStyle1.MappingName = "timeInForces";

			//Fetching the existing timeInForces from the database and binding it to the grid.
			TimeInForces timeInForces = OrderManager.GetTimeInForces();
			//Assigning the grid's datasource to the timeInForces object.
			grdTimeInForce.DataSource = timeInForces;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				//The checkID is used to store the currenly saved id of the TimeInForce in the database.
				int checkID = SaveTimeInForce();						
				//Binding grid after saving the a TimeInForce.
				BindTimeInForceGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a TimeInForce after checking that whether the 
					//TimeInForce is saved successfully or not by checking for the positive value of 
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

		//This method blanks the textboxes in the TimeInForce form.
		private void RefreshForm()
		{
			txtTimeInForce.Text = "";
			txtTimeInForceTagValue.Text = "";
		}
		
		/// <summary>
		/// This method saves the <see cref="TimeInForce"/> to the database.
		/// </summary>
		/// <returns>TimeInForceID, saved to the database</returns>
		private int SaveTimeInForce()
		{	
			errorProvider1.SetError(txtTimeInForce, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Nirvana.Admin.BLL.TimeInForce timeInForce = new Nirvana.Admin.BLL.TimeInForce();
			
			if(txtTimeInForce.Tag != null) 
			{
				//Update
				timeInForce.TimeInForceID = int.Parse(txtTimeInForce.Tag.ToString());
			}
			//Validation to check for the empty value in TimeInForce textbox
			if(txtTimeInForce.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(txtTimeInForce, "Provide Value!");
				return result;
			}
			else
			{
				timeInForce.Name = txtTimeInForce.Text.Trim();
				errorProvider1.SetError(txtTimeInForce, "");
			}
			timeInForce.TagValue = txtTimeInForceTagValue.Text;
			
			//Saving the timeInForce data and retrieving the timeInForceid for the newly added timeInForce.
			int newTimeInForceID = OrderManager.SaveTimeInForce(timeInForce);
			//Showing the message: TimeInForce already existing by checking the timeInForce id value to -1 
			if(newTimeInForceID == -1)
			{
				errorProvider1.SetError(btnSave, "timeInForce Already Exists");
			}
				//Showing the message : TimeInForce data saved
			else
			{
				//errorProvider1.SetError(txtTimeInForce, "TimeInForce Saved");
				txtTimeInForce.Tag = null;
			}
			result = newTimeInForceID;
			//Returning the newly added timeInForce id.
			return result;
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			txtTimeInForce.Text = "";
			txtTimeInForce.Tag = null;
			txtTimeInForceTagValue.Text = "";
		}

		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the TimeInForce if the grid has any TimeInForce.
				if (grdTimeInForce.Rows.Count > 0)
				{
					//Edit: Edit.
					//Showing the values of the currently selected row to the textboxes in the form by 
					//the column positions relative to the timeInForce and timeInForceID.
					txtTimeInForce.Text = grdTimeInForce.ActiveRow.Cells["Name"].Text.ToString();
					txtTimeInForce.Tag = grdTimeInForce.ActiveRow.Cells["TimeInForceID"].Text.ToString();
					txtTimeInForceTagValue.Text = grdTimeInForce.ActiveRow.Cells["TagValue"].Text.ToString();
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
				if(grdTimeInForce.Rows.Count > 0)
				{
					//Asking the user to be sure about deleting the timeInForce.
					if(MessageBox.Show(this, "Do you want to delete this Time in Force?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
					{
						//Getting the timeInForceid from the currently selected row in the grid.
						int timeInForceID = int.Parse(grdTimeInForce.ActiveRow.Cells["TimeInForceID"].Text.ToString());				
						
						bool chkVarraible = OrderManager.DeleteTimeInForce(timeInForceID, false);
						if(!(chkVarraible))
						{
							MessageBox.Show(this, "TimeInForce is referenced in CounterpartyVenue.\n Please remove references first to delete it.", "Nirvana Alert");
						}
						else
						{
							BindTimeInForceGrid();
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

		private void lblTimeInForce_Click(object sender, System.EventArgs e)
		{
		
		}

		private void txtTimeInForce_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void lblTimeInForceTagValue_Click(object sender, System.EventArgs e)
		{
		
		}

		private void txtTimeInForceTagValue_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}
	}
}
