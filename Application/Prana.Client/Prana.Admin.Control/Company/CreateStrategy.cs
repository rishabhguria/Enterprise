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
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
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
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(82, 76);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 26);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtStrategy);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(13, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(319, 70);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Strategy";
			// 
			// txtStrategy
			// 
			this.txtStrategy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtStrategy.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtStrategy.Location = new System.Drawing.Point(142, 18);
			this.txtStrategy.MaxLength = 50;
			this.txtStrategy.Name = "txtStrategy";
			this.txtStrategy.Size = new System.Drawing.Size(148, 21);
			this.txtStrategy.TabIndex = 1;
			this.txtStrategy.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Strategy";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 16);
			this.label2.TabIndex = 0;
			this.label2.Text = "Short Name";
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtShortName.Location = new System.Drawing.Point(142, 46);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(148, 21);
			this.txtShortName.TabIndex = 1;
			this.txtShortName.Text = "";
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(168, 76);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 26);
			this.btnClose.TabIndex = 6;
			this.btnClose.Text = "Close";
			// 
			// statusBar1
			// 
			this.statusBar1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.statusBar1.Location = new System.Drawing.Point(0, 108);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						  this.statusBarPanel1});
			this.statusBar1.Size = new System.Drawing.Size(336, 22);
			this.statusBar1.TabIndex = 8;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(130, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 35;
			this.label3.Text = "*";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(142, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 35;
			this.label4.Text = "*";
			// 
			// CreateStrategy
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label4);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateStrategy";
			this.Size = new System.Drawing.Size(336, 130);
			this.Load += new System.EventHandler(this.CreateStrategy_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
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
