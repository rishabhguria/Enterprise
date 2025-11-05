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
	/// Summary description for CounterPartyVenueDetails.
	/// </summary>
	public class CounterPartyVenueDetails : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		private const string FORM_NAME = "Login : ";
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbSymbolConversion;
		private System.Windows.Forms.TextBox txtFixIdentifier;
		private System.Windows.Forms.ComboBox cmbCounterPartyDetailsElectronic;
		private System.Windows.Forms.TextBox txtDisplayName;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.ListBox lstAUEC;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CounterPartyVenueDetails()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			try
			{
				BindIsElectronicID();
				BindSymbolConversions();	
				BindAUEC();	
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbSymbolConversion = new System.Windows.Forms.ComboBox();
			this.txtFixIdentifier = new System.Windows.Forms.TextBox();
			this.cmbCounterPartyDetailsElectronic = new System.Windows.Forms.ComboBox();
			this.txtDisplayName = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lstAUEC = new System.Windows.Forms.ListBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(148, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 14);
			this.label1.TabIndex = 31;
			this.label1.Text = "*";
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(148, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 14);
			this.label2.TabIndex = 32;
			this.label2.Text = "*";
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label30.Location = new System.Drawing.Point(10, 76);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(100, 16);
			this.label30.TabIndex = 18;
			this.label30.Text = "FIX Identifier";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label29.Location = new System.Drawing.Point(10, 50);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(100, 16);
			this.label29.TabIndex = 16;
			this.label29.Text = "Electronic";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label28.Location = new System.Drawing.Point(10, 24);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(100, 16);
			this.label28.TabIndex = 15;
			this.label28.Text = "Display Name";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(6, 366);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "* Required Field";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(10, 100);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 34;
			this.label4.Text = "AUEC";
			// 
			// cmbSymbolConversion
			// 
			this.cmbSymbolConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSymbolConversion.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbSymbolConversion.ItemHeight = 13;
			this.cmbSymbolConversion.Location = new System.Drawing.Point(160, 332);
			this.cmbSymbolConversion.Name = "cmbSymbolConversion";
			this.cmbSymbolConversion.Size = new System.Drawing.Size(284, 21);
			this.cmbSymbolConversion.TabIndex = 27;
			this.cmbSymbolConversion.GotFocus += new System.EventHandler(this.cmbSymbolConversion_GotFocus);
			this.cmbSymbolConversion.LostFocus += new System.EventHandler(this.cmbSymbolConversion_LostFocus);
			// 
			// txtFixIdentifier
			// 
			this.txtFixIdentifier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFixIdentifier.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFixIdentifier.Location = new System.Drawing.Point(160, 70);
			this.txtFixIdentifier.MaxLength = 50;
			this.txtFixIdentifier.Name = "txtFixIdentifier";
			this.txtFixIdentifier.Size = new System.Drawing.Size(284, 21);
			this.txtFixIdentifier.TabIndex = 25;
			this.txtFixIdentifier.Text = "";
			this.txtFixIdentifier.LostFocus += new System.EventHandler(this.txtFixIdentifier_LostFocus);
			this.txtFixIdentifier.GotFocus += new System.EventHandler(this.txtFixIdentifier_GotFocus);
			// 
			// cmbCounterPartyDetailsElectronic
			// 
			this.cmbCounterPartyDetailsElectronic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterPartyDetailsElectronic.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterPartyDetailsElectronic.ItemHeight = 13;
			this.cmbCounterPartyDetailsElectronic.Location = new System.Drawing.Point(160, 44);
			this.cmbCounterPartyDetailsElectronic.Name = "cmbCounterPartyDetailsElectronic";
			this.cmbCounterPartyDetailsElectronic.Size = new System.Drawing.Size(284, 21);
			this.cmbCounterPartyDetailsElectronic.TabIndex = 24;
			this.cmbCounterPartyDetailsElectronic.GotFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_GotFocus);
			this.cmbCounterPartyDetailsElectronic.LostFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_LostFocus);
			// 
			// txtDisplayName
			// 
			this.txtDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtDisplayName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtDisplayName.Location = new System.Drawing.Point(160, 18);
			this.txtDisplayName.MaxLength = 50;
			this.txtDisplayName.Name = "txtDisplayName";
			this.txtDisplayName.Size = new System.Drawing.Size(284, 21);
			this.txtDisplayName.TabIndex = 23;
			this.txtDisplayName.Text = "";
			this.txtDisplayName.LostFocus += new System.EventHandler(this.txtDisplayName_LostFocus);
			this.txtDisplayName.GotFocus += new System.EventHandler(this.txtDisplayName_GotFocus);
			// 
			// label35
			// 
			this.label35.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label35.Location = new System.Drawing.Point(10, 338);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(120, 16);
			this.label35.TabIndex = 22;
			this.label35.Text = "Symbol Conversion";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lstAUEC);
			this.groupBox1.Controls.Add(this.label30);
			this.groupBox1.Controls.Add(this.label29);
			this.groupBox1.Controls.Add(this.label28);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cmbSymbolConversion);
			this.groupBox1.Controls.Add(this.txtFixIdentifier);
			this.groupBox1.Controls.Add(this.cmbCounterPartyDetailsElectronic);
			this.groupBox1.Controls.Add(this.txtDisplayName);
			this.groupBox1.Controls.Add(this.label35);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(2, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(462, 384);
			this.groupBox1.TabIndex = 35;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Counter Party Venue Details";
			// 
			// lstAUEC
			// 
			this.lstAUEC.Location = new System.Drawing.Point(160, 96);
			this.lstAUEC.Name = "lstAUEC";
			this.lstAUEC.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.lstAUEC.Size = new System.Drawing.Size(284, 225);
			this.lstAUEC.TabIndex = 39;
			this.lstAUEC.GotFocus += new System.EventHandler(this.lstAUEC_GotFocus);
			this.lstAUEC.LostFocus += new System.EventHandler(this.lstAUEC_LostFocus);
			// 
			// label7
			// 
			this.label7.ForeColor = System.Drawing.Color.Red;
			this.label7.Location = new System.Drawing.Point(148, 46);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(12, 14);
			this.label7.TabIndex = 38;
			this.label7.Text = "*";
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(148, 96);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 14);
			this.label6.TabIndex = 37;
			this.label6.Text = "*";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(148, 334);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 14);
			this.label5.TabIndex = 36;
			this.label5.Text = "*";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// CounterPartyVenueDetails
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CounterPartyVenueDetails";
			this.Size = new System.Drawing.Size(590, 394);
			this.Load += new System.EventHandler(this.CounterPartyVenueDetails_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public CounterPartyVenue CounterPartyProperty
		{
			get 
			{
				CounterPartyVenue counterPartyVenue = new CounterPartyVenue();
				GetCounterPartyVenueDetails(counterPartyVenue);
				return counterPartyVenue; 
			}
			set 
			{
				SetCounterPartyVenueDetails(value);
			}
		}

		public void GetCounterPartyVenueDetails(CounterPartyVenue counterPartyVenue)
		{
			//counterPartyVenue.CounterPartyVenueID = 1;
			counterPartyVenue.DisplayName = txtDisplayName.Text.Trim();
			counterPartyVenue.IsElectronic = int.Parse(cmbCounterPartyDetailsElectronic.SelectedValue.ToString());
			counterPartyVenue.FixIdentifier = txtFixIdentifier.Text.Trim();
			counterPartyVenue.SymbolConversionID = int.Parse(cmbSymbolConversion.SelectedValue.ToString());

			//return counterPartyVenue;
		}

		public int GetCounterPartyVenueDetailsForSave(CounterPartyVenue counterPartyVenue)
		{
			int result = int.MinValue;
			
			errorProvider1.SetError(txtDisplayName, "");
			errorProvider1.SetError(cmbCounterPartyDetailsElectronic, "");
			errorProvider1.SetError(cmbSymbolConversion, "");
			errorProvider1.SetError(lstAUEC, "");
			errorProvider1.SetError(txtFixIdentifier, "");
			if (txtDisplayName.Text == "")
			{
				errorProvider1.SetError(txtDisplayName, "Please enter display name!");
				txtDisplayName.Focus();
			}
			else if(int.Parse(cmbCounterPartyDetailsElectronic.SelectedValue.ToString()) == int.MinValue)
			{
				errorProvider1.SetError(cmbCounterPartyDetailsElectronic, "Please select Electronic Details!");
				cmbCounterPartyDetailsElectronic.Focus();
			}
			else if (txtFixIdentifier.Text == "")
			{
				errorProvider1.SetError(txtFixIdentifier, "Please enter Fix Identfier name!");
				txtFixIdentifier.Focus();
			}
			else if(int.Parse(lstAUEC.SelectedValue.ToString()) == int.MinValue)
			{
				errorProvider1.SetError(lstAUEC, "Please select AUEC Details while not selecting the -Select- option!");
				lstAUEC.Focus();
			}
			else if(int.Parse(cmbSymbolConversion.SelectedValue.ToString()) == int.MinValue)
			{
				errorProvider1.SetError(cmbSymbolConversion, "Please select Symbol Conversion!");
				cmbSymbolConversion.Focus();
			}
			
			else
			{
				//counterPartyVenue.CounterPartyVenueID = 1;
				counterPartyVenue.DisplayName = txtDisplayName.Text.Trim();
				counterPartyVenue.IsElectronic = int.Parse(cmbCounterPartyDetailsElectronic.SelectedValue.ToString());
				counterPartyVenue.FixIdentifier = txtFixIdentifier.Text.Trim();
				System.Text.StringBuilder valueAUECs = new System.Text.StringBuilder(",");
				for(int i=0, count = lstAUEC.SelectedItems.Count; i<count; i++)
				{
					//valueAUECs.Append(((System.Data.DataRow)lstAUEC.SelectedItems[i])[1].ToString());
					valueAUECs.Append(((System.Data.DataRow)(((System.Data.DataRowView)((lstAUEC.SelectedItems[i]))).Row)).ItemArray[1].ToString());
					valueAUECs.Append(",");
				}
				counterPartyVenue.AUECID = valueAUECs.ToString();
				//counterPartyVenue.AUECID = int.Parse(lstAUEC.SelectedValue.ToString());
				counterPartyVenue.SymbolConversionID = int.Parse(cmbSymbolConversion.SelectedValue.ToString());
				result = 1;
			}			
			return result;
		}

		public void SetCounterPartyVenueDetails(CounterPartyVenue counterPartyVenue)
		{
			if(counterPartyVenue != null)
			{
				txtDisplayName.Text = counterPartyVenue.DisplayName;
				cmbCounterPartyDetailsElectronic.SelectedValue = counterPartyVenue.IsElectronic;
				txtFixIdentifier.Text = counterPartyVenue.FixIdentifier;
				lstAUEC.SelectedValue = counterPartyVenue.AUECID;
				string checkString = counterPartyVenue.AUECID.ToString();
				char[] sep = {','};
				Array a = checkString.Split(sep);
		
				lstAUEC.SelectedIndex = -1;
				if(counterPartyVenue.AUECID.ToString() != "")
				{
					for(int i=1;i<(a.Length-1);i++)
					{
						lstAUEC.SelectedValue = a.GetValue(i);
					}
				}
				else
				{
					lstAUEC.SelectedValue = int.MinValue;
				}

				cmbSymbolConversion.SelectedValue = counterPartyVenue.SymbolConversionID;
			}			
		}

		private void BindIsElectronicID()
		{
			System.Data.DataTable dt = new System.Data.DataTable();
			dt.Columns.Add("Data");
			dt.Columns.Add("Value");
			object[] row = new object[2]; 
			row[0] = C_COMBO_SELECT;
			row[1] = int.MinValue;
			dt.Rows.Add(row);
			row[0] = "Yes";
			row[1] = "1";
			dt.Rows.Add(row);
			row[0] = "No";
			row[1] = "0";
			dt.Rows.Add(row);
			cmbCounterPartyDetailsElectronic.DataSource = dt;
			cmbCounterPartyDetailsElectronic.DisplayMember = "Data";
			cmbCounterPartyDetailsElectronic.ValueMember = "Value";
		}
		
		private void BindSymbolConversions()
		{
			SymbolConversions symbolConversions = SymbolManager.GetSymbolConversions();
			if (symbolConversions.Count > 0 )
			{
				symbolConversions.Insert(0, new SymbolConversion(int.MinValue, C_COMBO_SELECT));
				cmbSymbolConversion.DataSource = symbolConversions;
				cmbSymbolConversion.DisplayMember = "SymbolName";
				cmbSymbolConversion.ValueMember = "SymbolID";
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
					//string Data = auec.Asset.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
					string Data = auec.Asset.Name.ToString() + " : " + auec.UnderLying.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
					int Value = auec.AUECID;
					
					row[0] = Data;
					row[1] = Value;
					dtauec.Rows.Add(row);
				}

				lstAUEC.DataSource = dtauec;
				lstAUEC.DisplayMember = "Data";
				lstAUEC.ValueMember = "Value";
			}
		}

		private StatusBar _statusBar = null;
		public StatusBar ParentStatusBar
		{
			set{_statusBar = value;}
		}
		
		public void Refresh(object sender, System.EventArgs e)
		{
			txtDisplayName.Text = "";
			txtFixIdentifier.Text = "";
		}
		
		private void CounterPartyVenueDetails_Load(object sender, System.EventArgs e)
		{
			try
			{
				BindIsElectronicID();
				BindAUEC();
				BindSymbolConversions();	
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

		

		
		# region Controls Focus Colors
		private void lstAUEC_GotFocus(object sender, System.EventArgs e)
		{
			lstAUEC.BackColor = Color.LemonChiffon;
		}
		private void lstAUEC_LostFocus(object sender, System.EventArgs e)
		{
			lstAUEC.BackColor = Color.White;
		}
		private void cmbCounterPartyDetailsElectronic_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyDetailsElectronic.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyDetailsElectronic_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyDetailsElectronic.BackColor = Color.White;
		}
		
		private void cmbSymbolConversion_GotFocus(object sender, System.EventArgs e)
		{
			cmbSymbolConversion.BackColor = Color.LemonChiffon;
		}
		private void cmbSymbolConversion_LostFocus(object sender, System.EventArgs e)
		{
			cmbSymbolConversion.BackColor = Color.White;
		}
		
		private void txtDisplayName_GotFocus(object sender, System.EventArgs e)
		{
			txtDisplayName.BackColor = Color.LemonChiffon;
		}
		private void txtDisplayName_LostFocus(object sender, System.EventArgs e)
		{
			txtDisplayName.BackColor = Color.White;
		}
		private void txtFixIdentifier_GotFocus(object sender, System.EventArgs e)
		{
			txtFixIdentifier.BackColor = Color.LemonChiffon;
		}
		private void txtFixIdentifier_LostFocus(object sender, System.EventArgs e)
		{
			txtFixIdentifier.BackColor = Color.White;
		}
		#endregion
	}
}
