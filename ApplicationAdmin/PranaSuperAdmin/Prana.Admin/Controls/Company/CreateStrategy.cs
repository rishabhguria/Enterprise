using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Nirvana.Admin.BLL;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CreateStrategy.
	/// </summary>
	public class CreateStrategy : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TextBox txtStrategy;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateStrategy()
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

		Nirvana.Admin.BLL.Strategy _strategyEdit = null;

		public Nirvana.Admin.BLL.Strategy StrategyEdit
		{
			set{_strategyEdit = value;}
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSave = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtStrategy = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(65, 80);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 26);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtStrategy);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Location = new System.Drawing.Point(3, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(312, 70);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Strategy";
			// 
			// txtStrategy
			// 
			this.txtStrategy.Location = new System.Drawing.Point(148, 16);
			this.txtStrategy.MaxLength = 50;
			this.txtStrategy.Name = "txtStrategy";
			this.txtStrategy.Size = new System.Drawing.Size(148, 20);
			this.txtStrategy.TabIndex = 1;
			this.txtStrategy.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(14, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(124, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Strategy";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(14, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtShortName
			// 
			this.txtShortName.Location = new System.Drawing.Point(148, 44);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 20);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// btnClose
			// 
			this.btnClose.Location = new System.Drawing.Point(151, 80);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 26);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Close";
			// 
			// CreateStrategy
			// 
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Name = "CreateStrategy";
			this.Size = new System.Drawing.Size(320, 108);
			this.Load += new System.EventHandler(this.CreateStrategy_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void BindForEdit()
		{
			if(_strategyEdit != null)
			{
				txtStrategy.Text = _strategyEdit.StrategyName;
				txtShortName.Text = _strategyEdit.StrategyShortName;
			}
		}
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set{_companyID = value;}
		}	
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_strategyEdit != null)
			{
				int index = _strategies.IndexOf(_strategyEdit);
						
				((Nirvana.Admin.BLL.Strategy)_strategies[index]).StrategyName = txtStrategy.Text.ToString();
				((Nirvana.Admin.BLL.Strategy)_strategies[index]).StrategyShortName = txtShortName.Text.ToString();
				
				//stbStrategy.Text = "Stored !";
				
			}
			else
			{
				if(txtStrategy.Text.Trim() == "")
				{
					//stbStrategy Fund.Text = "Please enter Strategy Name!";
					txtStrategy.Focus();
				}
				if(txtShortName.Text.Trim() == "")
				{
					//stbStrategy.Text = "Please enter Short Name!";
					txtShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.Strategy strategy = new Nirvana.Admin.BLL.Strategy();
						
					strategy.StrategyName = txtStrategy.Text.ToString();
					strategy.StrategyShortName = txtShortName.Text.ToString();
					_strategies.Add(strategy);		
					//stbFund.Text = "Stored !";
				}
			}
		}
		
		public void Refresh(object sender, System.EventArgs e)
		{
			txtStrategy.Text = "";
			txtShortName.Text = "";
		}

		private void CreateStrategy_Load(object sender, System.EventArgs e)
		{
			if(_strategyEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				//stbFunds.Text = "";
			}
		}		

		private Nirvana.Admin.BLL.Strategies _strategies = new Strategies();
		public Strategies CurrentStrategies
		{
			get 
			{
				return _strategies; 
			}
			set
			{
				if(value != null)
				{
					_strategies = value;
				}
			}			
		}
	}
}
