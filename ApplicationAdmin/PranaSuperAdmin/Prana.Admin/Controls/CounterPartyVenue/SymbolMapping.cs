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

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for SymbolMapping.
	/// </summary>
	public class SymbolMapping : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "SymbolMapping : ";
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button btnGrdDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.DataGrid dtgrdSymbolMapping;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		private Nirvana.Admin.BLL.SymbolMappings _symbolMappings = new SymbolMappings();
		public Nirvana.Admin.BLL.SymbolMappings CurrentSymbolMappings
		{
			get 
			{
				return (Nirvana.Admin.BLL.SymbolMappings) dtgrdSymbolMapping.DataSource; 
			}						
		}

		private int _counterPartyVenueID = int.MinValue;
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn3;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn4;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn5;
	
		public int CounterPartyVenueID
		{
			get{return _counterPartyVenueID;}
			set
			{
				_counterPartyVenueID = value;
				BindDataGrid();
			}
		}

		private int _symbolMappingID = int.MinValue;
		public int SymbolMappingID
		{
			get{return _symbolMappingID;}
			set
			{
				_symbolMappingID = value;
				BindDataGrid();
			}
		}

		public SymbolMapping()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			BindDataGrid();

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
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.btnGrdDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.dtgrdSymbolMapping = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn3 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn4 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn5 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.groupBox6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dtgrdSymbolMapping)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.btnGrdDelete);
			this.groupBox6.Controls.Add(this.btnEdit);
			this.groupBox6.Controls.Add(this.btnCreate);
			this.groupBox6.Controls.Add(this.dtgrdSymbolMapping);
			this.groupBox6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox6.Location = new System.Drawing.Point(2, 0);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(446, 248);
			this.groupBox6.TabIndex = 1;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Symbol Mapping";
			// 
			// btnGrdDelete
			// 
			this.btnGrdDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnGrdDelete.Location = new System.Drawing.Point(360, 224);
			this.btnGrdDelete.Name = "btnGrdDelete";
			this.btnGrdDelete.TabIndex = 8;
			this.btnGrdDelete.Text = "Delete";
			this.btnGrdDelete.Click += new System.EventHandler(this.btnGrdDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Location = new System.Drawing.Point(280, 224);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 7;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnCreate
			// 
			this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCreate.Location = new System.Drawing.Point(360, 16);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 6;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// dtgrdSymbolMapping
			// 
			this.dtgrdSymbolMapping.AllowNavigation = false;
			this.dtgrdSymbolMapping.CaptionVisible = false;
			this.dtgrdSymbolMapping.CausesValidation = false;
			this.dtgrdSymbolMapping.DataMember = "";
			this.dtgrdSymbolMapping.FlatMode = true;
			this.dtgrdSymbolMapping.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dtgrdSymbolMapping.Location = new System.Drawing.Point(8, 38);
			this.dtgrdSymbolMapping.Name = "dtgrdSymbolMapping";
			this.dtgrdSymbolMapping.ReadOnly = true;
			this.dtgrdSymbolMapping.Size = new System.Drawing.Size(432, 184);
			this.dtgrdSymbolMapping.TabIndex = 5;
			this.dtgrdSymbolMapping.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
																										   this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.dtgrdSymbolMapping;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
																												  this.dataGridTextBoxColumn1,
																												  this.dataGridTextBoxColumn2,
																												  this.dataGridTextBoxColumn3,
																												  this.dataGridTextBoxColumn4,
																												  this.dataGridTextBoxColumn5});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.MappingName = "symbolMappings";
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.HeaderText = "AUEC ID";
			this.dataGridTextBoxColumn1.MappingName = "AUECID";
			this.dataGridTextBoxColumn1.Width = 75;
			// 
			// dataGridTextBoxColumn2
			// 
			this.dataGridTextBoxColumn2.Format = "";
			this.dataGridTextBoxColumn2.FormatInfo = null;
			this.dataGridTextBoxColumn2.HeaderText = "Counter Party Venue ID";
			this.dataGridTextBoxColumn2.MappingName = "CounterPartyVenueID";
			this.dataGridTextBoxColumn2.Width = 0;
			// 
			// dataGridTextBoxColumn3
			// 
			this.dataGridTextBoxColumn3.Format = "";
			this.dataGridTextBoxColumn3.FormatInfo = null;
			this.dataGridTextBoxColumn3.HeaderText = "Mapped Symbol";
			this.dataGridTextBoxColumn3.MappingName = "MappedSymbol";
			this.dataGridTextBoxColumn3.Width = 75;
			// 
			// dataGridTextBoxColumn4
			// 
			this.dataGridTextBoxColumn4.Format = "";
			this.dataGridTextBoxColumn4.FormatInfo = null;
			this.dataGridTextBoxColumn4.HeaderText = "Mapped Symbol ID";
			this.dataGridTextBoxColumn4.MappingName = "MappedSymbolID";
			this.dataGridTextBoxColumn4.Width = 0;
			// 
			// dataGridTextBoxColumn5
			// 
			this.dataGridTextBoxColumn5.Format = "";
			this.dataGridTextBoxColumn5.FormatInfo = null;
			this.dataGridTextBoxColumn5.HeaderText = "Symbol ID";
			this.dataGridTextBoxColumn5.MappingName = "SymbolID";
			this.dataGridTextBoxColumn5.Width = 75;
			// 
			// SymbolMapping
			// 
			this.Controls.Add(this.groupBox6);
			this.Name = "SymbolMapping";
			this.Size = new System.Drawing.Size(456, 252);
			this.groupBox6.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dtgrdSymbolMapping)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		
		
		public SymbolMapping SymbolMappingProperty
		{
			get 
			{
				SymbolMapping symbolMapping = new SymbolMapping();
				//SymbolMapping symbolMapping = new SymbolMapping();
				GetSymbolMapping(symbolMapping);
				return symbolMapping; 
			}
			set 
			{
					SetSymbolMapping(value);
					
			}
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.SymbolMappings symbolMappings = CounterPartyManager.GetSymbolMapping(_counterPartyVenueID);
				dtgrdSymbolMapping.DataSource = symbolMappings;
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

		public void GetSymbolMapping(SymbolMapping symbolMapping)
		{
			symbolMapping = (SymbolMapping)dtgrdSymbolMapping.DataSource;						
		}

		public void SetSymbolMapping(SymbolMapping symbolMapping)
		{
			dtgrdSymbolMapping.DataSource = symbolMapping;
		}
		
		//instance of CounterPartyVenueSymbolMapping class file.
		private CounterPartyVenueSymbolMapping counterPartyVenueSymbolMapping = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(counterPartyVenueSymbolMapping == null)
			{
				counterPartyVenueSymbolMapping = new CounterPartyVenueSymbolMapping();				
			}
		
			SymbolMappings symbolMappings = new SymbolMappings(); 
			counterPartyVenueSymbolMapping.CurrentSymbolMappings = (Nirvana.Admin.BLL.SymbolMappings) dtgrdSymbolMapping.DataSource;
			counterPartyVenueSymbolMapping.ShowDialog(this.Parent);
			dtgrdSymbolMapping.DataSource = null;
			dtgrdSymbolMapping.Refresh();
			symbolMappings = counterPartyVenueSymbolMapping.CurrentSymbolMappings;
			//dtgrdSymbolMapping.DataSource = counterPartyVenueSymbolMapping.CurrentSymbolMappings;	
			dtgrdSymbolMapping.DataSource = symbolMappings;	
			if(counterPartyVenueSymbolMapping.CurrentSymbolMappings.Count > 0)
			{
				dtgrdSymbolMapping.Select(0);
			}			
		}

		private CounterPartyVenueSymbolMapping counterPartyVenueSymbolMappingEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(counterPartyVenueSymbolMappingEdit == null)
			{
				if(dtgrdSymbolMapping.VisibleRowCount > 0)
				{
					
					counterPartyVenueSymbolMappingEdit = new CounterPartyVenueSymbolMapping();				
					Nirvana.Admin.BLL.SymbolMapping symbolMappingEdit = new Nirvana.Admin.BLL.SymbolMapping();
					//Set object to be edited.				
					symbolMappingEdit.AUECID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 0].ToString());
					symbolMappingEdit.CounterPartyVenueID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 1].ToString());
					symbolMappingEdit.MappedSymbol = dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 2].ToString();
					symbolMappingEdit.MappedSymbolID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 4].ToString());
					symbolMappingEdit.SymbolID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 3].ToString());
					counterPartyVenueSymbolMappingEdit.SymbolMappingEdit = symbolMappingEdit;

					counterPartyVenueSymbolMappingEdit.CurrentSymbolMappings = (Nirvana.Admin.BLL.SymbolMappings) dtgrdSymbolMapping.DataSource;
					counterPartyVenueSymbolMappingEdit.ShowDialog(this.Parent);
					counterPartyVenueSymbolMappingEdit = null;
				}				
			}
		}

		private void btnGrdDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(MessageBox.Show(this, "Do you want to delete this Symbol Mapping?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					int symbolMappingID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 4].ToString());
					if(symbolMappingID != int.MinValue)
					{
						CounterPartyManager.DeleteSymbolMapping(symbolMappingID);	
						BindDataGrid();
					}
					else
					{
						Nirvana.Admin.BLL.SymbolMappings symbolMappings = (SymbolMappings)dtgrdSymbolMapping.DataSource;
						Nirvana.Admin.BLL.SymbolMapping symbolMapping = new Nirvana.Admin.BLL.SymbolMapping();
					
					
						symbolMapping.AUECID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 0].ToString());
						symbolMapping.CounterPartyVenueID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 1].ToString());
						symbolMapping.MappedSymbol = dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 2].ToString();					
						symbolMapping.SymbolID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 3].ToString());
						symbolMapping.MappedSymbolID = int.Parse(dtgrdSymbolMapping[dtgrdSymbolMapping.CurrentCell.RowNumber, 4].ToString());
					
						symbolMappings.RemoveAt(symbolMappings.IndexOf(symbolMapping));
						dtgrdSymbolMapping.DataSource = null;
						dtgrdSymbolMapping.DataSource = symbolMappings;
						dtgrdSymbolMapping.Refresh();
					}				
				}
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
	}
}
