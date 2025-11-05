using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Nirvana.Admin.BLL;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CreateCompanyStrategy.
	/// </summary>
	public class CreateCompanyStrategy : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtStrategy;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.StatusBarPanel statusBarPanel1;
		private System.Windows.Forms.StatusBar stbCreateCompanyStrategy;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyStrategy()
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
			this.btnSave = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtStrategy = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.stbCreateCompanyStrategy = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
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
			this.btnSave.Location = new System.Drawing.Point(83, 78);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(72, 24);
			this.btnSave.TabIndex = 9;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnClose.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnClose.Location = new System.Drawing.Point(163, 78);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(72, 24);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtStrategy);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(11, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(319, 70);
			this.groupBox1.TabIndex = 8;
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
			this.txtStrategy.LostFocus += new System.EventHandler(this.txtStrategy_LostFocus);
			this.txtStrategy.GotFocus += new System.EventHandler(this.txtStrategy_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(8, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(98, 16);
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
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// stbCreateCompanyStrategy
			// 
			this.stbCreateCompanyStrategy.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbCreateCompanyStrategy.Location = new System.Drawing.Point(0, 107);
			this.stbCreateCompanyStrategy.Name = "stbCreateCompanyStrategy";
			this.stbCreateCompanyStrategy.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																										this.statusBarPanel1});
			this.stbCreateCompanyStrategy.ShowPanels = true;
			this.stbCreateCompanyStrategy.Size = new System.Drawing.Size(330, 22);
			this.stbCreateCompanyStrategy.TabIndex = 11;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarPanel1.Text = "statusBarPanel1";
			this.statusBarPanel1.Width = 314;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(130, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(12, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "*";
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(140, 50);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 33;
			this.label4.Text = "*";
			// 
			// CreateCompanyStrategy
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(330, 129);
			this.ControlBox = false;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.stbCreateCompanyStrategy);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "CreateCompanyStrategy";
			this.Text = "CreateCompanyStrategy";
			this.Load += new System.EventHandler(this.CreateCompanyStrategy_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		Nirvana.Admin.BLL.Strategy _strategyEdit = null;

		public Nirvana.Admin.BLL.Strategy StrategyEdit
		{
			set{_strategyEdit = value;}
		}

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
				errorProvider1.SetError(txtStrategy, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtStrategy.Text.Trim() == "")
				{
					errorProvider1.SetError(txtStrategy, "Please enter Strategy Name!");
					txtStrategy.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					_strategyEdit.StrategyName = txtStrategy.Text.ToString();
					_strategyEdit.StrategyShortName = txtShortName.Text.ToString();

					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
				}
			}
			else
			{
				errorProvider1.SetError(txtStrategy, "");
				errorProvider1.SetError(txtShortName, "");
				if(txtStrategy.Text.Trim() == "")
				{
					errorProvider1.SetError(txtStrategy, "Please enter Strategy Name!");
					txtStrategy.Focus();
				}
				else if(txtShortName.Text.Trim() == "")
				{
					errorProvider1.SetError(txtShortName, "Please enter Short Name!");
					txtShortName.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.Strategy strategy = new Nirvana.Admin.BLL.Strategy();
						
					strategy.StrategyName = txtStrategy.Text.ToString();
					strategy.StrategyShortName = txtShortName.Text.ToString();
					_strategies.Add(strategy);		
					Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
					this.Hide();
				}
			}
			
		}

		
		public void Refresh(object sender, System.EventArgs e)
		{
			txtStrategy.Text = "";
			txtShortName.Text = "";
		}

		private void CreateCompanyStrategy_Load(object sender, System.EventArgs e)
		{
			if(_strategyEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
			}
		}

		private Nirvana.Admin.BLL.Strategies _strategies = new Strategies();

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}
	
		public Strategies CurrentCompanyStrategies
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
		#region Focus Colors
		private void txtStrategy_GotFocus(object sender, System.EventArgs e)
		{
			txtStrategy.BackColor = Color.LemonChiffon;
		}
		private void txtStrategy_LostFocus(object sender, System.EventArgs e)
		{
			txtStrategy.BackColor = Color.White;
		}
		private void txtShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.LemonChiffon;
		}
		private void txtShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.White;
		}
		#endregion
	}
}
