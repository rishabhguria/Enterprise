#region Using
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Prana.Admin.BLL;
using Prana.Utilities.DateTimeUtilities;
using Prana.Global;
using Infragistics.Win.UltraWinGrid;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;


#endregion

namespace Prana.Admin
{
	/// <summary>
	/// Summary description for Holiday.
	/// </summary>
	public class Holiday : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Holiday : ";
		private const int GRD_HOLIDAY_ID = 0;
		private const int GRD_HOLIDAY_DATE = 1;
		private const int GRD_HOLIDAY_DESC = 2;
		
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label lblSideTagValue;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label59;
		private System.Windows.Forms.DateTimePicker dttHolidayDate;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.TextBox txtHolidayDesc;
		private System.Windows.Forms.Label label1;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdHoliday;
        private System.Windows.Forms.GroupBox updateHoliday;
        private IContainer components;

		public Holiday()
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Holiday));
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("HolidayID", 0);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Date", 1);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn3 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("Description", 2);
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn4 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ExchangeID", 3);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn5 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("AUECExchangeID", 4);
            Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn6 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("StringDate", 5);
            this.btnReset = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.updateHoliday = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.dttHolidayDate = new System.Windows.Forms.DateTimePicker();
            this.label40 = new System.Windows.Forms.Label();
            this.txtHolidayDesc = new System.Windows.Forms.TextBox();
            this.lblSideTagValue = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.grdHoliday = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.updateHoliday.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).BeginInit();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(240)))), ((int)(((byte)(150)))));
            this.btnReset.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnReset.BackgroundImage")));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnReset.Location = new System.Drawing.Point(136, 228);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 4;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btnEdit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnEdit.BackgroundImage")));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnEdit.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnEdit.Location = new System.Drawing.Point(97, 130);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 6;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(202)))));
            this.btnDelete.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDelete.BackgroundImage")));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDelete.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnDelete.Location = new System.Drawing.Point(175, 130);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnClose.Location = new System.Drawing.Point(216, 228);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(204)))), ((int)(((byte)(102)))));
            this.btnSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSave.BackgroundImage")));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.btnSave.Location = new System.Drawing.Point(56, 228);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // updateHoliday
            // 
            this.updateHoliday.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.updateHoliday.Controls.Add(this.label1);
            this.updateHoliday.Controls.Add(this.label59);
            this.updateHoliday.Controls.Add(this.dttHolidayDate);
            this.updateHoliday.Controls.Add(this.label40);
            this.updateHoliday.Controls.Add(this.txtHolidayDesc);
            this.updateHoliday.Controls.Add(this.lblSideTagValue);
            this.updateHoliday.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.updateHoliday.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.updateHoliday.Location = new System.Drawing.Point(38, 154);
            this.updateHoliday.Name = "updateHoliday";
            this.updateHoliday.Size = new System.Drawing.Size(270, 70);
            this.updateHoliday.TabIndex = 16;
            this.updateHoliday.TabStop = false;
            this.updateHoliday.Text = "Add/Update Holiday";
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(80, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 8);
            this.label1.TabIndex = 30;
            this.label1.Text = "*";
            // 
            // label59
            // 
            this.label59.ForeColor = System.Drawing.Color.Red;
            this.label59.Location = new System.Drawing.Point(40, 22);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(8, 8);
            this.label59.TabIndex = 29;
            this.label59.Text = "*";
            // 
            // dttHolidayDate
            // 
            this.dttHolidayDate.CustomFormat = "\'\'";
            this.dttHolidayDate.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dttHolidayDate.Location = new System.Drawing.Point(92, 20);
            this.dttHolidayDate.Name = "dttHolidayDate";
            this.dttHolidayDate.Size = new System.Drawing.Size(158, 21);
            this.dttHolidayDate.TabIndex = 1;
            this.dttHolidayDate.Value = new System.DateTime(2006, 8, 2, 20, 54, 58, 140);
            this.dttHolidayDate.ValueChanged += new System.EventHandler(this.dttHolidayDate_ValueChanged);
            // 
            // label40
            // 
            this.label40.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label40.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label40.Location = new System.Drawing.Point(12, 21);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(32, 18);
            this.label40.TabIndex = 27;
            this.label40.Text = " Date";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHolidayDesc
            // 
            this.txtHolidayDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHolidayDesc.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtHolidayDesc.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtHolidayDesc.Location = new System.Drawing.Point(92, 44);
            this.txtHolidayDesc.MaxLength = 50;
            this.txtHolidayDesc.Name = "txtHolidayDesc";
            this.txtHolidayDesc.Size = new System.Drawing.Size(158, 21);
            this.txtHolidayDesc.TabIndex = 2;
            this.txtHolidayDesc.LostFocus += new System.EventHandler(this.txtHolidayDesc_LostFocus);
            this.txtHolidayDesc.GotFocus += new System.EventHandler(this.txtHolidayDesc_GotFocus);
            // 
            // lblSideTagValue
            // 
            this.lblSideTagValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSideTagValue.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSideTagValue.Location = new System.Drawing.Point(12, 47);
            this.lblSideTagValue.Name = "lblSideTagValue";
            this.lblSideTagValue.Size = new System.Drawing.Size(70, 15);
            this.lblSideTagValue.TabIndex = 8;
            this.lblSideTagValue.Text = " Description";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // grdHoliday
            // 
            this.grdHoliday.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grdHoliday.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            ultraGridColumn1.Header.VisiblePosition = 0;
            ultraGridColumn1.Hidden = true;
            ultraGridColumn1.Width = 211;
            ultraGridColumn2.AutoEdit = false;
            ultraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit;
            appearance1.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellAppearance = appearance1;
            appearance2.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.CellButtonAppearance = appearance2;
            appearance3.FontData.BoldAsString = "False";
            appearance3.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn2.Header.Appearance = appearance3;
            ultraGridColumn2.Header.VisiblePosition = 1;
            ultraGridColumn2.Width = 101;
            ultraGridColumn3.AutoEdit = false;
            appearance4.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellAppearance = appearance4;
            appearance5.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.CellButtonAppearance = appearance5;
            appearance6.FontData.BoldAsString = "False";
            appearance6.TextHAlign = Infragistics.Win.HAlign.Center;
            ultraGridColumn3.Header.Appearance = appearance6;
            ultraGridColumn3.Header.VisiblePosition = 2;
            ultraGridColumn3.Width = 117;
            ultraGridColumn4.Header.VisiblePosition = 3;
            ultraGridColumn4.Hidden = true;
            ultraGridColumn4.Width = 74;
            ultraGridColumn5.Header.VisiblePosition = 4;
            ultraGridColumn5.Hidden = true;
            ultraGridColumn5.Width = 73;
            ultraGridColumn6.Header.Caption = "Date";
            ultraGridColumn6.Header.VisiblePosition = 5;
            ultraGridColumn6.Width = 99;
            ultraGridBand1.Columns.AddRange(new object[] {
            ultraGridColumn1,
            ultraGridColumn2,
            ultraGridColumn3,
            ultraGridColumn4,
            ultraGridColumn5,
            ultraGridColumn6});
            ultraGridBand1.Header.Enabled = false;
            ultraGridBand1.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.grdHoliday.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.grdHoliday.DisplayLayout.GroupByBox.Hidden = true;
            this.grdHoliday.DisplayLayout.MaxColScrollRegions = 1;
            this.grdHoliday.DisplayLayout.MaxRowScrollRegions = 1;
            this.grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
            this.grdHoliday.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdHoliday.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            this.grdHoliday.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdHoliday.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdHoliday.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdHoliday.FlatMode = true;
            this.grdHoliday.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.grdHoliday.Location = new System.Drawing.Point(4, 0);
            this.grdHoliday.Name = "grdHoliday";
            this.grdHoliday.Size = new System.Drawing.Size(338, 128);
            this.grdHoliday.TabIndex = 55;
            // 
            // Holiday
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216)))));
            this.ClientSize = new System.Drawing.Size(344, 255);
            this.Controls.Add(this.grdHoliday);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.updateHoliday);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(352, 282);
            this.Name = "Holiday";
            this.Text = "Holiday";
            this.Load += new System.EventHandler(this.Holiday_Load);
            this.updateHoliday.ResumeLayout(false);
            this.updateHoliday.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdHoliday)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		#region Focus Colors
		private void txtHolidayDesc_GotFocus(object sender, System.EventArgs e)
		{
			txtHolidayDesc.BackColor = Color.LemonChiffon;
		}
		private void txtHolidayDesc_LostFocus(object sender, System.EventArgs e)
		{
			txtHolidayDesc.BackColor = Color.White;
		} 
		#endregion


		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void BindHolidayGrid()
		{
			//this.dataGridTableStyle1.MappingName = "holidays";

			//Fetching the existing assets from the database and binding it to the grid.
			Holidays holidays = ExchangeManager.GetHolidays();

			//Assigning the holiday grid's datasource property to holidays object if it has some values.
			if(holidays.Count != 0)
			{
				//Assigning the grid's datasource to the holidays object.
				grdHoliday.DataSource = holidays;
                
                ColumnsCollection columnsHoliday = grdHoliday.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columnsHoliday)
                {
                    if (column.Key != "Date" && column.Key != "Description")
                    {
                        column.Hidden = true;
                    }
                }
                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Hidden = false;    
                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
                grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Header.VisiblePosition = 1;
                grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Width = 99;
                grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Width = 117;
			}
			else
			{
				Holidays nullHolidays = new Holidays();
				Prana.Admin.BLL.Holiday holiday = new Prana.Admin.BLL.Holiday(int.MinValue,"",Prana.BusinessObjects.DateTimeConstants.MinValue,int.MinValue);
				nullHolidays.Add(holiday);
				grdHoliday.DataSource = nullHolidays;

                ColumnsCollection columns2 = grdHoliday.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns2)
                {
                    if (column.Key != "StringDate" && column.Key != "Description")
                    {
                        column.Hidden = true;
                    }
                }

                //if (!grdHoliday.DisplayLayout.Bands[0].Columns.Exists("StringDate"))
                //{
                //    grdHoliday.DisplayLayout.Bands[0].Columns.Add("StringDate", "Date");
                //    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                //    //grdHoliday.DisplayLayout.Bands[0].Columns["Date"].Header.VisiblePosition = 0;
                //    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                //    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Width = 246;
                //}
                //else
                //{
                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Hidden = false;
                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Header.VisiblePosition = 1;
                    grdHoliday.DisplayLayout.Bands[0].Columns["StringDate"].Width = 99;
                    grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                    grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
                    grdHoliday.DisplayLayout.Bands[0].Columns["Description"].Width = 117;
                //}
			}
			grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
			//grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;

		}

		private void Holiday_Load(object sender, System.EventArgs e)
		{
			BindHolidayGrid();
            this.dttHolidayDate.Value = DateTime.Now;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
                errorProvider1.SetError(btnEdit, "");
                //The checkID is used to store the currenly saved id of the holiday in the database.
				int checkID = SaveHoliday();						
				//Binding grid after saving the a side.
				BindHolidayGrid();
				if(checkID >= 0)
				{
					//Calling refresh form after saving a holiday after checking that whether the holiday
					//is saved successfully or not by checking for the positive value of checkID.
                    MessageBox.Show("Holiday saved.", "Prana Alert", MessageBoxButtons.OK);
					RefreshForm();
                }              
				
				grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
				//grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;

                //dttHolidayDate.CustomFormat = "\'\'";
                //dttHolidayDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                //dttHolidayDate.Value = DateTime.Now;

			}
			
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, ApplicationConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnSave_Click", 
					ApplicationConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private int SaveHoliday()
		{	
			errorProvider1.SetError(txtHolidayDesc, "");
			errorProvider1.SetError(dttHolidayDate, "");
			errorProvider1.SetError(btnSave, "");

			int result = int.MinValue;

			Prana.Admin.BLL.Holiday holiday = new Prana.Admin.BLL.Holiday();
			
			if(dttHolidayDate.Tag != null) 
			{
				//Update
				holiday.HolidayID = int.Parse(dttHolidayDate.Tag.ToString());
			}
			
			//Validation to check for the empty value in Holiday textbox
			if(dttHolidayDate.Text.Trim().Length == 0)
			{
				errorProvider1.SetError(dttHolidayDate, "Provide Value!");
				return result;
			}
			// Check if Description for the new Holiday is given
			else if (txtHolidayDesc.Text.Trim()=="")
			{
				//MessageBox.Show("Please Enter Description");
				errorProvider1.SetError(txtHolidayDesc, "Please Enter Description.");
				txtHolidayDesc.Focus();
			}
			else
			{
				holiday.Date = dttHolidayDate.Value;
				holiday.Description = txtHolidayDesc.Text.Trim();

				//Check if Date already exists in the Holiday List
				Holidays holidays = ((Holidays)grdHoliday.DataSource);
				bool flag = false;
				if(holidays.Count > 0)
				{
					foreach(Prana.Admin.BLL.Holiday checkHoliday in holidays)
					{
						if(checkHoliday.Date.ToShortDateString()== dttHolidayDate.Value.ToShortDateString()) 
						{
							flag = true;
							break;
						}
					}
				}

				if (flag==false || editCase == true)
				{

					//Saving the side data and retrieving the dateid for the newly added date.
					int newDateID = ExchangeManager.SaveHoliday(holiday);
					//Showing the message: side already existing by checking the side id value to -1 
					if(newDateID == -1)
					{
						errorProvider1.SetError(btnSave, "Holiday Already Exists");
                        MessageBox.Show("Date " + dttHolidayDate.Value.ToShortDateString() + " already exists in the List!");
                        btnSave.Focus();                    

					}
						//Showing the message : Holiday data saved
					else
					{
                        //MessageBox.Show("Holiday saved.", "Prana Alert", MessageBoxButtons.OK);
                        dttHolidayDate.Tag = null;
                       
					}
					result = newDateID;
				}
				else
				{
					// Date already exists in the database ... Error Message
					MessageBox.Show("Date " + dttHolidayDate.Value.ToShortDateString() + " already exists in the List!" )  ;
					errorProvider1.SetError(dttHolidayDate, "Holiday with the same date already exists.");
					btnSave.Focus();
				
				}
			}
			
			editCase = false;
			//Returning the newly added date id.
			return result;
		}

		private bool editCase = false;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				errorProvider1.SetError(btnEdit, "");
				//Check for editing the date if the grid has any date.
                string holidayName = grdHoliday.ActiveRow.Cells["Description"].Text.ToString();
                if (holidayName != "")
                {
                    //if (grdHoliday.Rows.Count > 0)
                    //{
                    //Edit: Edit.
                    //Showing the values of the currently selected row to the textboxes in the form by 
                    //the column positions relative to the date and dateID.
                    dttHolidayDate.Text = grdHoliday.ActiveRow.Cells["Date"].Text.ToString();
                    dttHolidayDate.Tag = grdHoliday.ActiveRow.Cells["HolidayID"].Text.ToString();
                    txtHolidayDesc.Text = grdHoliday.ActiveRow.Cells["Description"].Text.ToString();
                    editCase = true;
                    //}
                    //else
                    //{
                    //Showing the message: No Data Available.
                    //errorProvider1.SetError(btnEdit, "No Data Available to edit.");
                    //}
                }
                else
                {
                    errorProvider1.SetError(btnEdit, "No Data Available to edit.");
                }
				grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
//				grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, ApplicationConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnEdit_Click", 
					ApplicationConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnEdit_Click", null); 
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				//Check for deleting the holiday if the grid has any holiday to delete.
				if(grdHoliday.Rows.Count > 0)
				{
                    string holidayDescription = grdHoliday.ActiveRow.Cells["Description"].Text.ToString();
                    if (holidayDescription != "")
                    {
                        //Asking the user to be sure about deleting the holiday.
                        if (MessageBox.Show(this, "Do you want to delete this Holiday?", "Prana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            //Getting the holidayid from the currently selected row in the grid.
                            int holidayID = int.Parse(grdHoliday.ActiveRow.Cells["HolidayID"].Text.ToString());

                            bool chkVarriable = GeneralManager.DeleteMasterHoliday(holidayID);
                            if (!(chkVarriable))
                            {
                                MessageBox.Show(this, "Holiday is referenced in Exchange/AUEC.\n Please remove references first to delete it.", "Prana Alert");
                            }
                            else
                            {
                                BindHolidayGrid();
                            }
                        }
                    }
					
				}
				else
				{
					//Showing the message: No Data Available.
					errorProvider1.SetError(btnDelete, "No Data Available to delete.");
				}
				grdHoliday.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
				//grdHoliday.DisplayLayout.Bands[0].Columns["Date"].CellActivation = Activation.NoEdit;
			}
				#region Catch
			catch(Exception ex)
			{
				string formattedInfo = ex.StackTrace.ToString();
				Logger.Write(formattedInfo, ApplicationConstants.LOG_CATEGORY_EXCEPTION, 1, 1, System.Diagnostics.TraceEventType.Error, 
					FORM_NAME);
				AppMessageExceptionHandler appMessageExceptionHandler = new AppMessageExceptionHandler(null);
				appMessageExceptionHandler.HandleException(new Exception(ErrorStatements.ERROR_STATEMENT), System.Guid.NewGuid());
			}
				#endregion
			finally
			{
				#region LogEntry

				LogEntry logEntry = new LogEntry("btnDelete_Click", 
					ApplicationConstants.LOG_CATEGORY_UI, 1, 1, System.Diagnostics.TraceEventType.Information,
                    FORM_NAME + "btnSave_Click", null);
				Logger.Write(logEntry); 

				#endregion
			}
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			dttHolidayDate.Text = "";
			dttHolidayDate.Tag = null;
			txtHolidayDesc.Text = "";
		}

		//This method blanks the textboxes in the Holiday form.
		private void RefreshForm()
		{
			dttHolidayDate.Text = "";
			txtHolidayDesc.Text = "";
		}

		private void dttHolidayDate_ValueChanged(object sender, System.EventArgs e)
		{
			dttHolidayDate.Format = DateTimePickerFormat.Short;
		}
	}
}
