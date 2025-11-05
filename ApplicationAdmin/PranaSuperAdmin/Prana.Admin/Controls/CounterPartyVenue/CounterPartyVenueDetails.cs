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
		
		private System.Windows.Forms.ComboBox cmbSymbolConversion;
		private System.Windows.Forms.TextBox txtFixIdentifier;
		private System.Windows.Forms.TextBox txtDisplayName;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.ComboBox cmbCounterPartyDetailsElectronic;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbAUEC;
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
			this.cmbSymbolConversion = new System.Windows.Forms.ComboBox();
			this.txtFixIdentifier = new System.Windows.Forms.TextBox();
			this.cmbCounterPartyDetailsElectronic = new System.Windows.Forms.ComboBox();
			this.txtDisplayName = new System.Windows.Forms.TextBox();
			this.label35 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbAUEC = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// cmbSymbolConversion
			// 
			this.cmbSymbolConversion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSymbolConversion.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbSymbolConversion.Location = new System.Drawing.Point(154, 104);
			this.cmbSymbolConversion.Name = "cmbSymbolConversion";
			this.cmbSymbolConversion.Size = new System.Drawing.Size(284, 21);
			this.cmbSymbolConversion.TabIndex = 27;
			this.cmbSymbolConversion.GotFocus += new System.EventHandler(this.cmbSymbolConversion_GotFocus);
			this.cmbSymbolConversion.LostFocus += new System.EventHandler(this.cmbSymbolConversion_LostFocus);
			// 
			// txtFixIdentifier
			// 
			this.txtFixIdentifier.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtFixIdentifier.Location = new System.Drawing.Point(154, 58);
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
			this.cmbCounterPartyDetailsElectronic.Location = new System.Drawing.Point(154, 34);
			this.cmbCounterPartyDetailsElectronic.Name = "cmbCounterPartyDetailsElectronic";
			this.cmbCounterPartyDetailsElectronic.Size = new System.Drawing.Size(284, 21);
			this.cmbCounterPartyDetailsElectronic.TabIndex = 24;
			this.cmbCounterPartyDetailsElectronic.GotFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_GotFocus);
			this.cmbCounterPartyDetailsElectronic.LostFocus += new System.EventHandler(this.cmbCounterPartyDetailsElectronic_LostFocus);
			// 
			// txtDisplayName
			// 
			this.txtDisplayName.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtDisplayName.Location = new System.Drawing.Point(154, 10);
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
			this.label35.Location = new System.Drawing.Point(4, 104);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(120, 16);
			this.label35.TabIndex = 22;
			this.label35.Text = "Symbol Conversion";
			// 
			// label30
			// 
			this.label30.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label30.Location = new System.Drawing.Point(4, 56);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(100, 16);
			this.label30.TabIndex = 18;
			this.label30.Text = "FIX Identifier";
			// 
			// label29
			// 
			this.label29.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label29.Location = new System.Drawing.Point(4, 32);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(100, 16);
			this.label29.TabIndex = 16;
			this.label29.Text = "Electronic";
			// 
			// label28
			// 
			this.label28.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label28.Location = new System.Drawing.Point(4, 8);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(100, 16);
			this.label28.TabIndex = 15;
			this.label28.Text = "Display Name";
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.Red;
			this.label1.Location = new System.Drawing.Point(142, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 14);
			this.label1.TabIndex = 31;
			this.label1.Text = "*";
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(142, 62);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 14);
			this.label2.TabIndex = 32;
			this.label2.Text = "*";
			// 
			// label3
			// 
			this.label3.ForeColor = System.Drawing.Color.Red;
			this.label3.Location = new System.Drawing.Point(8, 142);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 14);
			this.label3.TabIndex = 33;
			this.label3.Text = "* Required Field";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(4, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 16);
			this.label4.TabIndex = 34;
			this.label4.Text = "AUEC";
			// 
			// cmbAUEC
			// 
			this.cmbAUEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAUEC.Location = new System.Drawing.Point(154, 82);
			this.cmbAUEC.Name = "cmbAUEC";
			this.cmbAUEC.Size = new System.Drawing.Size(284, 21);
			this.cmbAUEC.TabIndex = 26;
			this.cmbAUEC.GotFocus += new System.EventHandler(this.cmbAUEC_GotFocus);
			this.cmbAUEC.LostFocus += new System.EventHandler(this.cmbAUEC_LostFocus);
			// 
			// CounterPartyVenueDetails
			// 
			this.Controls.Add(this.cmbAUEC);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbSymbolConversion);
			this.Controls.Add(this.txtFixIdentifier);
			this.Controls.Add(this.cmbCounterPartyDetailsElectronic);
			this.Controls.Add(this.txtDisplayName);
			this.Controls.Add(this.label35);
			this.Controls.Add(this.label30);
			this.Controls.Add(this.label29);
			this.Controls.Add(this.label28);
			this.Name = "CounterPartyVenueDetails";
			this.Size = new System.Drawing.Size(448, 160);
			this.Load += new System.EventHandler(this.CounterPartyVenueDetails_Load);
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
			if (txtDisplayName.Text == "")
			{
				_statusBar.Text = "Please enter display name!";
				txtDisplayName.Focus();
			}
			else if(int.Parse(cmbCounterPartyDetailsElectronic.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Electronic Details!";
				cmbCounterPartyDetailsElectronic.Focus();
			}
			else if (txtFixIdentifier.Text == "")
			{
				_statusBar.Text = "Please enter Fix Identfier name!";
				txtFixIdentifier.Focus();
			}
			else if(int.Parse(cmbAUEC.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select AUEC Details!";
				cmbAUEC.Focus();
			}
			else if(int.Parse(cmbSymbolConversion.SelectedValue.ToString()) == int.MinValue)
			{
				_statusBar.Text = "Please select Symbol Conversion!";
				cmbSymbolConversion.Focus();
			}
			
			else
			{
				//counterPartyVenue.CounterPartyVenueID = 1;
				counterPartyVenue.DisplayName = txtDisplayName.Text.Trim();
				counterPartyVenue.IsElectronic = int.Parse(cmbCounterPartyDetailsElectronic.SelectedValue.ToString());
				counterPartyVenue.FixIdentifier = txtFixIdentifier.Text.Trim();
				counterPartyVenue.AUECID = int.Parse(cmbAUEC.SelectedValue.ToString());
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
				cmbAUEC.SelectedValue = counterPartyVenue.AUECID;
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
				foreach(Nirvana.Admin.BLL.AUEC auec in auecs)
				{
					//string Data = auec.Asset.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
					string Data = auec.Asset.Name.ToString() + " : " + auec.UnderLying.Name.ToString() + " : " + auec.Exchange.Name.ToString() + " : " + auec.Currency.CurrencyName.ToString();
					int Value = auec.AUECID;
					
					row[0] = Data;
					row[1] = Value;
					dtauec.Rows.Add(row);
				}

				cmbAUEC.DataSource = dtauec;
				cmbAUEC.DisplayMember = "Data";
				cmbAUEC.ValueMember = "Value";
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
		private void cmbAUEC_GotFocus(object sender, System.EventArgs e)
		{
			cmbAUEC.BackColor = Color.LemonChiffon;
		}
		private void cmbAUEC_LostFocus(object sender, System.EventArgs e)
		{
			cmbAUEC.BackColor = Color.White;
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
