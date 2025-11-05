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
	/// Summary description for GridStrategy.
	/// </summary>
	public class GridStrategy : System.Windows.Forms.UserControl
	{
		private const string FORM_NAME = "GridStrategy : ";
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GridStrategy()
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
			this.grdStrategy = new System.Windows.Forms.DataGrid();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnCreate = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdStrategy)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.grdStrategy);
			this.groupBox1.Location = new System.Drawing.Point(2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(648, 90);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Startegy";
			// 
			// grdStrategy
			// 
			this.grdStrategy.CaptionVisible = false;
			this.grdStrategy.DataMember = "";
			this.grdStrategy.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.grdStrategy.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.grdStrategy.Location = new System.Drawing.Point(6, 11);
			this.grdStrategy.Name = "grdStrategy";
			this.grdStrategy.ReadOnly = true;
			this.grdStrategy.Size = new System.Drawing.Size(640, 78);
			this.grdStrategy.TabIndex = 19;
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(576, 94);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.TabIndex = 25;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(494, 94);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.TabIndex = 24;
			this.btnEdit.Text = "Edit";
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnCreate
			// 
			this.btnCreate.Location = new System.Drawing.Point(412, 94);
			this.btnCreate.Name = "btnCreate";
			this.btnCreate.TabIndex = 27;
			this.btnCreate.Text = "Create";
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			// 
			// GridStrategy
			// 
			this.Controls.Add(this.btnCreate);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.groupBox1);
			this.Name = "GridStrategy";
			this.Size = new System.Drawing.Size(654, 116);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grdStrategy)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.DataGrid grdStrategy;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnCreate;

		private CreateCompanyStrategy createCompanyStrategy = null;
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if(createCompanyStrategy == null)
			{
				createCompanyStrategy = new CreateCompanyStrategy();				
			}
			createCompanyStrategy.ShowDialog(this.Parent);
			grdStrategy.DataSource = null;
			grdStrategy.Refresh();
			grdStrategy.DataSource = createCompanyStrategy.CurrentCompanyStrategies; 
			
			if(createCompanyStrategy.CurrentCompanyStrategies.Count > 0 )
			{
				grdStrategy.Select(0);
			}
			createCompanyStrategy.CurrentCompanyStrategies = null;
			createCompanyStrategy.StrategyEdit = null;
		}

		public Nirvana.Admin.BLL.Strategies CurrentStrategies
		{
			get 
			{
				return (Nirvana.Admin.BLL.Strategies) grdStrategy.DataSource; 
			}						
		}

		private int _companyID = int.MinValue;
		public int CompanyID
		{
			get{return _companyID;}
			set
			{
				_companyID = value;
				BindDataGrid();
			}
		}

		private int _strategyID = int.MinValue;
		public int StrategyID
		{
			get{return _strategyID;}
			set
			{
				_strategyID = value;
				BindDataGrid();
			}
		}

		public Strategy StrategyProperty
		{
			get 
			{
				Strategy strategy = new Strategy();
				GetStrategy(strategy);
				return strategy; 
			}
			set 
			{
				SetStrategy(value);
					
			}
		}

		public void GetStrategy(Strategy strategy)
		{
			strategy = (Strategy)grdStrategy.DataSource;						
		}
		
		public void SetStrategy(Strategy strategy)
		{
			grdStrategy.DataSource = strategy;
		}

		private void BindDataGrid()
		{
			try
			{
				Nirvana.Admin.BLL.Strategies strategies = CompanyManager.GetStrategy(_companyID);
				grdStrategy.DataSource = strategies;
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

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show(this, "Do you want to delete this Strategy?", "Nirvana Alert", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				int strategyID = int.Parse(grdStrategy[grdStrategy.CurrentCell.RowNumber, 0].ToString());
				if(strategyID != int.MinValue)
				{
					
				}
				else
				{
					Nirvana.Admin.BLL.Strategies strategies = (Strategies)grdStrategy.DataSource;
					Nirvana.Admin.BLL.Strategy strategy = new Nirvana.Admin.BLL.Strategy();
					
					
					strategy.StrategyID = int.Parse(grdStrategy[grdStrategy.CurrentCell.RowNumber, 0].ToString());
					strategy.StrategyName = grdStrategy[grdStrategy.CurrentCell.RowNumber, 1].ToString();
					strategy.StrategyShortName = grdStrategy[grdStrategy.CurrentCell.RowNumber, 2].ToString();
					strategy.CompanyID = int.Parse(grdStrategy[grdStrategy.CurrentCell.RowNumber, 3].ToString());
					
					strategies.RemoveAt(strategies.IndexOf(strategy));
					grdStrategy.DataSource = null;
					grdStrategy.DataSource = strategies;
					grdStrategy.Refresh();
				}
		
			}
		}
		private CreateCompanyStrategy createCompanyStrategyEdit = null;
		private void btnEdit_Click(object sender, System.EventArgs e)
		{
			if(createCompanyStrategyEdit == null)
			{
				if(grdStrategy.VisibleRowCount > 0)
				{
					createCompanyStrategyEdit = new CreateCompanyStrategy();
//					Nirvana.Admin.BLL.Strategy strategyEdit = new Nirvana.Admin.BLL.Strategy();
//					strategyEdit.StrategyID = int.Parse(grdStrategy[grdStrategy.CurrentCell.RowNumber, 2].ToString());
//					strategyEdit.StrategyName = grdStrategy[grdStrategy.CurrentCell.RowNumber, 1].ToString();
//					strategyEdit.StrategyShortName = grdStrategy[grdStrategy.CurrentCell.RowNumber, 0].ToString();
//					strategyEdit.CompanyID = int.Parse(grdStrategy[grdStrategy.CurrentCell.RowNumber, 3].ToString());

					createCompanyStrategyEdit.StrategyEdit = (Nirvana.Admin.BLL.Strategy)((Nirvana.Admin.BLL.Strategies) grdStrategy.DataSource)[grdStrategy.CurrentCell.RowNumber];

					createCompanyStrategyEdit.CurrentCompanyStrategies = (Nirvana.Admin.BLL.Strategies) grdStrategy.DataSource;	
					createCompanyStrategyEdit.ShowDialog(this.Parent);
					createCompanyStrategyEdit = null;
				}
			}
		}
	}
}
