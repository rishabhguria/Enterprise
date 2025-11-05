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
	/// Summary description for CounterPartyVenueSymbolMapping.
	/// </summary>
	public class CounterPartyVenueSymbolMapping : System.Windows.Forms.Form
	{
		private const string FORM_NAME = "Login : ";
		const string C_COMBO_SELECT = "- Select -";

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtMappedSymbol;
		private System.Windows.Forms.ComboBox cmbSymbol;
		private System.Windows.Forms.ComboBox cmbAUEC;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.StatusBar stbSymbolMapping;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;

		Nirvana.Admin.BLL.SymbolMapping _symbolMappingEdit = null;

		public Nirvana.Admin.BLL.SymbolMapping SymbolMappingEdit
		{
			set{_symbolMappingEdit = value;}
		}

		public CounterPartyVenueSymbolMapping()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			try
			{
				BindAUEC();
				BindSymbol();
				Refresh();
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
			this.label4 = new System.Windows.Forms.Label();
			this.txtMappedSymbol = new System.Windows.Forms.TextBox();
			this.cmbSymbol = new System.Windows.Forms.ComboBox();
			this.cmbAUEC = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.stbSymbolMapping = new System.Windows.Forms.StatusBar();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtMappedSymbol);
			this.groupBox1.Controls.Add(this.cmbSymbol);
			this.groupBox1.Controls.Add(this.cmbAUEC);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.groupBox1.Location = new System.Drawing.Point(8, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(474, 104);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			// 
			// label4
			// 
			this.label4.ForeColor = System.Drawing.Color.Red;
			this.label4.Location = new System.Drawing.Point(126, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 14);
			this.label4.TabIndex = 33;
			this.label4.Text = "*";
			// 
			// txtMappedSymbol
			// 
			this.txtMappedSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtMappedSymbol.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtMappedSymbol.Location = new System.Drawing.Point(139, 71);
			this.txtMappedSymbol.MaxLength = 50;
			this.txtMappedSymbol.Name = "txtMappedSymbol";
			this.txtMappedSymbol.Size = new System.Drawing.Size(304, 21);
			this.txtMappedSymbol.TabIndex = 11;
			this.txtMappedSymbol.Text = "";
			this.txtMappedSymbol.LostFocus += new System.EventHandler(this.txtMappedSymbol_LostFocus);
			this.txtMappedSymbol.GotFocus += new System.EventHandler(this.txtMappedSymbol_GotFocus);
			// 
			// cmbSymbol
			// 
			this.cmbSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSymbol.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbSymbol.Location = new System.Drawing.Point(139, 47);
			this.cmbSymbol.Name = "cmbSymbol";
			this.cmbSymbol.Size = new System.Drawing.Size(304, 21);
			this.cmbSymbol.TabIndex = 10;
			this.cmbSymbol.GotFocus += new System.EventHandler(this.cmbSymbol_GotFocus);
			this.cmbSymbol.LostFocus += new System.EventHandler(this.cmbSymbol_LostFocus);
			// 
			// cmbAUEC
			// 
			this.cmbAUEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAUEC.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbAUEC.Location = new System.Drawing.Point(139, 23);
			this.cmbAUEC.Name = "cmbAUEC";
			this.cmbAUEC.Size = new System.Drawing.Size(304, 21);
			this.cmbAUEC.TabIndex = 9;
			this.cmbAUEC.GotFocus += new System.EventHandler(this.cmbAUEC_GotFocus);
			this.cmbAUEC.LostFocus += new System.EventHandler(this.cmbAUEC_LostFocus);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(5, 76);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 16);
			this.label3.TabIndex = 8;
			this.label3.Text = "Mapped Symbol";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label2.Location = new System.Drawing.Point(5, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 16);
			this.label2.TabIndex = 7;
			this.label2.Text = "Symbol";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(5, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(95, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "AUEC";
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnSave.Location = new System.Drawing.Point(168, 110);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(255)), ((System.Byte)(51)), ((System.Byte)(51)));
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.btnCancel.Location = new System.Drawing.Point(250, 110);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Close";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// stbSymbolMapping
			// 
			this.stbSymbolMapping.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.stbSymbolMapping.Location = new System.Drawing.Point(0, 137);
			this.stbSymbolMapping.Name = "stbSymbolMapping";
			this.stbSymbolMapping.Size = new System.Drawing.Size(488, 22);
			this.stbSymbolMapping.TabIndex = 9;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(134, 50);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 14);
			this.label5.TabIndex = 33;
			this.label5.Text = "*";
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(134, 74);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 14);
			this.label6.TabIndex = 34;
			this.label6.Text = "*";
			// 
			// CounterPartyVenueSymbolMapping
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.ClientSize = new System.Drawing.Size(488, 159);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.stbSymbolMapping);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CounterPartyVenueSymbolMapping";
			this.Text = "Symbol Mapping";
			this.Load += new System.EventHandler(this.CounterPartyVenueSymbolMapping_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

//		private void RefreshWindow()
//		{
//			txtMappedSymbol.Text = "";
//		}
		private void BindSymbol()
		{
			Symbols symbols = SymbolManager.GetSymbols();
			if (symbols.Count > 0 )
			{
				symbols.Insert(0, new Symbol(int.MinValue, "-Select-"));
				cmbSymbol.DataSource = symbols;
				cmbSymbol.DisplayMember = "CompanySymbol";
				cmbSymbol.ValueMember = "SymbolID";
			}
			if(_symbolMappingEdit != null)
			{
				cmbSymbol.SelectedValue = _symbolMappingEdit.SymbolID;
			}
		}

		private void BindAUEC()
		{
			AUECs auecs = AUECManager.GetAUEC();

			System.Data.DataTable dtauec = new System.Data.DataTable();
			dtauec.Columns.Add("Data");
			dtauec.Columns.Add("Value");
			object[] row = new object[2]; 
			row[0] = C_COMBO_SELECT;
			row[1] = int.MinValue;
			dtauec.Rows.Add(row);

			if (auecs.Count > 0 )
			{
				foreach(AUEC auec in auecs)
				{
					string Data = auec.Asset.Name.ToString() + " : " + auec.UnderLying.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
					int Value = auec.AUECID;
					
					row[0] = Data;
					row[1] = Value;
					dtauec.Rows.Add(row);
				}

				cmbAUEC.DataSource = dtauec;
				//auecs.Insert(0, new AUEC(int.MinValue, C_COMBO_SELECT));
				cmbAUEC.DisplayMember = "Data";
				cmbAUEC.ValueMember = "Value";
			}

			if(_symbolMappingEdit != null)
			{
				cmbAUEC.SelectedValue = _symbolMappingEdit.AUECID;
				txtMappedSymbol.Text = _symbolMappingEdit.MappedSymbol;
			}
		}

		private StatusBar _statusBar = null;
		public StatusBar ParentStatusBar
		{
			set{_statusBar = value;}
		}
		
		public void Refresh(object sender, System.EventArgs e)
		{
			txtMappedSymbol.Text = "";
		}

		private void CounterPartyVenueSymbolMapping_Load(object sender, System.EventArgs e)
		{
			BindAUEC();
			BindSymbol();
			if(_symbolMappingEdit != null)
			{
			}
			else
			{
				Refresh(sender, e);
				stbSymbolMapping.Text = "";
			}
		}

		# region Controls Focus Colors
		private void cmbAUEC_GotFocus(object sender, System.EventArgs e)
		{
			cmbAUEC.BackColor = Color.LemonChiffon;
		}
		private void cmbAUEC_LostFocus(object sender, System.EventArgs e)
		{
			cmbAUEC.BackColor = Color.White;
		}
		private void cmbSymbol_GotFocus(object sender, System.EventArgs e)
		{
			cmbSymbol.BackColor = Color.LemonChiffon;
		}
		private void cmbSymbol_LostFocus(object sender, System.EventArgs e)
		{
			cmbSymbol.BackColor = Color.White;
		}
		private void txtMappedSymbol_GotFocus(object sender, System.EventArgs e)
		{
			txtMappedSymbol.BackColor = Color.LemonChiffon;
		}
		private void txtMappedSymbol_LostFocus(object sender, System.EventArgs e)
		{
			txtMappedSymbol.BackColor = Color.White;
		}
		#endregion

		
		private Nirvana.Admin.BLL.SymbolMappings _symbolMappings = new SymbolMappings();
		public SymbolMappings CurrentSymbolMappings
		{
			get 
			{
				return _symbolMappings; 
			}
			set
			{
				if(value != null)
				{
					_symbolMappings = value;
				}
			}			
		}
	
		
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(_symbolMappingEdit != null)
				{
					if(int.Parse(cmbAUEC.SelectedValue.ToString()) == int.MinValue)
					{
						errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
						cmbAUEC.Focus();
					}
					else if(int.Parse(cmbSymbol.SelectedValue.ToString()) == int.MinValue)
					{
						errorProvider1.SetError(cmbSymbol, "Please select Symbol !");
						cmbSymbol.Focus();
					}
					else if(txtMappedSymbol.Text.Trim() == "")
					{
						errorProvider1.SetError(txtMappedSymbol, "Please enter Mapped Symbol Name!");
						txtMappedSymbol.Focus();
					}
					else
					{
						int index = _symbolMappings.IndexOf(_symbolMappingEdit);
						
						//if (index == int.MinValue)
						//{
						((Nirvana.Admin.BLL.SymbolMapping)_symbolMappings[index]).AUECID = int.Parse(cmbAUEC.SelectedValue.ToString());
						((Nirvana.Admin.BLL.SymbolMapping)_symbolMappings[index]).AUEC = cmbAUEC.SelectedText.ToString();
						((Nirvana.Admin.BLL.SymbolMapping)_symbolMappings[index]).SymbolID = int.Parse(cmbSymbol.SelectedValue.ToString());
						((Nirvana.Admin.BLL.SymbolMapping)_symbolMappings[index]).SymbolName = cmbSymbol.SelectedText.ToString();
						((Nirvana.Admin.BLL.SymbolMapping)_symbolMappings[index]).MappedSymbol = txtMappedSymbol.Text.ToString();
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSymbolMapping);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbSymbolMapping, "Stored!");
					}
				}
				else
				{
					errorProvider1.SetError(txtMappedSymbol, "");
					errorProvider1.SetError(cmbAUEC, "");
					errorProvider1.SetError(cmbSymbol, "");
					if(int.Parse(cmbAUEC.SelectedValue.ToString()) == int.MinValue)
					{
						errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
						cmbAUEC.Focus();
					}
					else if(int.Parse(cmbSymbol.SelectedValue.ToString()) == int.MinValue)
					{
						errorProvider1.SetError(cmbSymbol, "Please select Symbol !");
						cmbSymbol.Focus();
					}
					else if(txtMappedSymbol.Text.Trim() == "")
					{
						errorProvider1.SetError(txtMappedSymbol, "Please enter Mapped Symbol Name!");
						txtMappedSymbol.Focus();
					}
					else
					{
						Nirvana.Admin.BLL.SymbolMapping symbolMapping = new Nirvana.Admin.BLL.SymbolMapping();
						
						symbolMapping.AUECID = int.Parse(cmbAUEC.SelectedValue.ToString());
						symbolMapping.AUEC = cmbAUEC.Text.ToString();
						symbolMapping.SymbolID = int.Parse(cmbSymbol.SelectedValue.ToString());
						symbolMapping.SymbolName = cmbSymbol.Text.ToString();
						symbolMapping.MappedSymbol = txtMappedSymbol.Text.ToString();
						_symbolMappings.Add(symbolMapping);		
						Nirvana.Admin.Utility.Common.ResetStatusPanel(stbSymbolMapping);
						Nirvana.Admin.Utility.Common.SetStatusPanel(stbSymbolMapping, "Stored!");
						this.Hide();
					}
				}
				
				//}
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

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

	}
}
