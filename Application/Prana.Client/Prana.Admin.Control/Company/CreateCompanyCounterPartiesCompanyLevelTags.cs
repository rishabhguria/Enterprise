using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;

using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tracing;

namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyCounterPartiesCompanyLevelTags.
	/// </summary>
	public class CreateCompanyCounterPartiesCompanyLevelTags : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox cmbCounterPartyVenue;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbClearingFirmPrimeBrokers;
		private System.Windows.Forms.ComboBox cmbCounterParty;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtDeliverSubID;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtDeliverCompID;
		private System.Windows.Forms.TextBox txtSenderCompID;
		private System.Windows.Forms.Label lblCounterParty;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyCounterPartiesCompanyLevelTags()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//			
		}

		public void SetData()
		{
			BindClearingFirmPrimeBrokers();
			BindCounterParty();
			BindCounterPartyVenues();
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblCounterParty = new System.Windows.Forms.Label();
			this.txtDeliverCompID = new System.Windows.Forms.TextBox();
			this.txtDeliverSubID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.cmbCounterPartyVenue = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbClearingFirmPrimeBrokers = new System.Windows.Forms.ComboBox();
			this.cmbCounterParty = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtSenderCompID = new System.Windows.Forms.TextBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblCounterParty);
			this.groupBox1.Controls.Add(this.txtDeliverCompID);
			this.groupBox1.Controls.Add(this.txtDeliverSubID);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbClearingFirmPrimeBrokers);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtSenderCompID);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(406, 158);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Company Level Details";
			// 
			// lblCounterParty
			// 
			this.lblCounterParty.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.lblCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.lblCounterParty.Location = new System.Drawing.Point(208, 20);
			this.lblCounterParty.Name = "lblCounterParty";
			this.lblCounterParty.Size = new System.Drawing.Size(168, 21);
			this.lblCounterParty.TabIndex = 13;
			// 
			// txtDeliverCompID
			// 
			this.txtDeliverCompID.Enabled = false;
			this.txtDeliverCompID.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtDeliverCompID.Location = new System.Drawing.Point(206, 86);
			this.txtDeliverCompID.MaxLength = 50;
			this.txtDeliverCompID.Name = "txtDeliverCompID";
			this.txtDeliverCompID.Size = new System.Drawing.Size(174, 21);
			this.txtDeliverCompID.TabIndex = 10;
			this.txtDeliverCompID.Text = "";
			this.txtDeliverCompID.LostFocus += new System.EventHandler(this.txtDeliverCompID_LostFocus);
			this.txtDeliverCompID.GotFocus += new System.EventHandler(this.txtDeliverCompID_GotFocus);
			// 
			// txtDeliverSubID
			// 
			this.txtDeliverSubID.Enabled = false;
			this.txtDeliverSubID.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtDeliverSubID.Location = new System.Drawing.Point(206, 108);
			this.txtDeliverSubID.MaxLength = 50;
			this.txtDeliverSubID.Name = "txtDeliverSubID";
			this.txtDeliverSubID.Size = new System.Drawing.Size(174, 21);
			this.txtDeliverSubID.TabIndex = 11;
			this.txtDeliverSubID.Text = "";
			this.txtDeliverSubID.LostFocus += new System.EventHandler(this.txtDeliverSubID_LostFocus);
			this.txtDeliverSubID.GotFocus += new System.EventHandler(this.txtDeliverSubID_GotFocus);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(8, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "CounterParty";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label8.Location = new System.Drawing.Point(8, 110);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(108, 18);
			this.label8.TabIndex = 5;
			this.label8.Text = "Deliver To Sub ID";
			// 
			// cmbCounterPartyVenue
			// 
			this.cmbCounterPartyVenue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterPartyVenue.Enabled = false;
			this.cmbCounterPartyVenue.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(206, 42);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterPartyVenue.TabIndex = 8;
			this.cmbCounterPartyVenue.GotFocus += new System.EventHandler(this.cmbCounterPartyVenue_GotFocus);
			this.cmbCounterPartyVenue.LostFocus += new System.EventHandler(this.cmbCounterPartyVenue_LostFocus);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(8, 134);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 18);
			this.label9.TabIndex = 6;
			this.label9.Text = "Sender Comp ID";
			// 
			// cmbClearingFirmPrimeBrokers
			// 
			this.cmbClearingFirmPrimeBrokers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbClearingFirmPrimeBrokers.Enabled = false;
			this.cmbClearingFirmPrimeBrokers.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbClearingFirmPrimeBrokers.Location = new System.Drawing.Point(206, 64);
			this.cmbClearingFirmPrimeBrokers.Name = "cmbClearingFirmPrimeBrokers";
			this.cmbClearingFirmPrimeBrokers.Size = new System.Drawing.Size(174, 21);
			this.cmbClearingFirmPrimeBrokers.TabIndex = 9;
			this.cmbClearingFirmPrimeBrokers.GotFocus += new System.EventHandler(this.cmbClearingFirmPrimeBrokers_GotFocus);
			this.cmbClearingFirmPrimeBrokers.LostFocus += new System.EventHandler(this.cmbClearingFirmPrimeBrokers_LostFocus);
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterParty.Location = new System.Drawing.Point(206, 18);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterParty.TabIndex = 7;
			this.cmbCounterParty.Visible = false;
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(8, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(120, 18);
			this.label10.TabIndex = 4;
			this.label10.Text = "Deliver to Comp ID";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(8, 44);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(8, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(158, 18);
			this.label4.TabIndex = 3;
			this.label4.Text = "ClearingFirm/PrimeBroker";
			// 
			// txtSenderCompID
			// 
			this.txtSenderCompID.Enabled = false;
			this.txtSenderCompID.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtSenderCompID.Location = new System.Drawing.Point(206, 130);
			this.txtSenderCompID.MaxLength = 50;
			this.txtSenderCompID.Name = "txtSenderCompID";
			this.txtSenderCompID.Size = new System.Drawing.Size(174, 21);
			this.txtSenderCompID.TabIndex = 12;
			this.txtSenderCompID.Text = "";
			this.txtSenderCompID.LostFocus += new System.EventHandler(this.txtSenderCompID_LostFocus);
			this.txtSenderCompID.GotFocus += new System.EventHandler(this.txtSenderCompID_GotFocus);
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.Enabled = false;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(171, 166);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 13;
			this.btnSave.Text = "Add";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label2
			// 
			this.label2.ForeColor = System.Drawing.Color.Red;
			this.label2.Location = new System.Drawing.Point(198, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 14);
			this.label2.TabIndex = 35;
			this.label2.Text = "*";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(198, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 14);
			this.label5.TabIndex = 36;
			this.label5.Text = "*";
			// 
			// CreateCompanyCounterPartiesCompanyLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateCompanyCounterPartiesCompanyLevelTags";
			this.Size = new System.Drawing.Size(416, 197);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void BindClearingFirmPrimeBrokers()
		{
			cmbClearingFirmPrimeBrokers.DataSource = null;
			ClearingFirmsPrimeBrokers clearingFirmPrimeBrokers = CompanyManager.GetClearingFirmPrimeBroker(_companyID);
//			if (clearingFirmPrimeBrokers.Count > 0 )
//			{
				clearingFirmPrimeBrokers.Insert(0, new ClearingFirmPrimeBroker(int.MinValue, C_COMBO_SELECT));
				cmbClearingFirmPrimeBrokers.DataSource = clearingFirmPrimeBrokers;				
				cmbClearingFirmPrimeBrokers.DisplayMember = "ClearingFirmsPrimeBrokersName";
				cmbClearingFirmPrimeBrokers.ValueMember = "ClearingFirmsPrimeBrokersID";
//			
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbClearingFirmPrimeBrokers.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID.ToString());
			}
		}
			
		private void BindCounterParty()
		{
			
			CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
//			if (counterParties.Count > 0 )
//			{
				counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
				cmbCounterParty.DataSource = counterParties;				
				cmbCounterParty.DisplayMember = "CounterPartyFullName";
				cmbCounterParty.ValueMember = "CounterPartyID";

				//cmbCounterParty.SelectedValue = _companyCounterPartyID;
				//lblCounterParty.Text = _companyCounterPartyID.ToString();
				
				this.cmbCounterParty.SelectedValueChanged += new System.EventHandler(this.cmbCounterParty_SelectedValueChanged);
				this.cmbCounterParty.SelectedIndexChanged += new System.EventHandler(this.cmbCounterParty_SelectedIndexChanged);
//			}
			
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterParty.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());
			}
		}

		private void BindCounterPartyVenues()
		{
			cmbCounterPartyVenue.DataSource = null;
			Venues venues = VenueManager.GetCounterPartyVenues(_companyCounterPartyID);
//			if (venues.Count > 0 )
//			{
				venues.Insert(0, new Venue(int.MinValue, C_COMBO_SELECT));
				cmbCounterPartyVenue.DataSource = venues;				
				cmbCounterPartyVenue.DisplayMember = "VenueName";
				cmbCounterPartyVenue.ValueMember = "VenueID";

//			}
			
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterPartyVenue.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Refresh(sender, e);
		}

		Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail _companyCounterPartyVenueDetailEdit = null;
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail CompanyCounterPartyVenueDetailEdit
		{
			set{_companyCounterPartyVenueDetailEdit = value;}
		}

		public void BindForEdit()
		{
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterParty.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID.ToString());
				cmbCounterPartyVenue.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID.ToString());	
				cmbClearingFirmPrimeBrokers.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID.ToString());
				txtDeliverCompID.Text = _companyCounterPartyVenueDetailEdit.DeliverToCompanyID.ToString();
				txtDeliverSubID.Text = _companyCounterPartyVenueDetailEdit.DeiverToSubID.ToString();
				txtSenderCompID.Text = _companyCounterPartyVenueDetailEdit.SenderCompanyID.ToString();

			}
		}
		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set
			{
				_companyID = value;
				BindClearingFirmPrimeBrokers();
			}
		}
	
		private int _companyCounterPartyID = int.MinValue;
		public int CompanyCounterPartyID
		{
			set
			{
				_companyCounterPartyID = value;
				CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new CompanyCounterPartyVenueDetail(_companyCounterPartyID);
				//lblCounterParty.Text = _companyCounterPartyID.ToString();
				lblCounterParty.Text = companyCounterPartyVenueDetail.CounterPartyFullName;
				BindCounterPartyVenues();
			}
		}

		public event EventHandler SaveData;
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				if(int.Parse(cmbCounterPartyVenue.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbClearingFirmPrimeBrokers.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "Please select Clearing Firm Prime Brokers!");
					cmbClearingFirmPrimeBrokers.Focus();
				}
				else
				{
					//_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = _companyCounterPartyID;
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.ClearingFirmPrimeBrokerID = int.Parse(cmbClearingFirmPrimeBrokers.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.DeliverToCompanyID = txtDeliverCompID.Text.ToString();
					_companyCounterPartyVenueDetailEdit.DeiverToSubID = txtDeliverSubID.Text.ToString();
					_companyCounterPartyVenueDetailEdit.SenderCompanyID = txtSenderCompID.Text.ToString();
					
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");

					Refresh(sender, e);
				}
			}
			else
			{
				errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				if(int.Parse(cmbCounterPartyVenue.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbClearingFirmPrimeBrokers.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbClearingFirmPrimeBrokers, "Please select Clearing Firm Prime Brokers!");
					cmbClearingFirmPrimeBrokers.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();
					
					//companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = _companyCounterPartyID;
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.SelectedValue.ToString());
					companyCounterPartyVenueDetail.ClearingFirmPrimeBrokerID = int.Parse(cmbClearingFirmPrimeBrokers.SelectedValue.ToString());
					companyCounterPartyVenueDetail.DeliverToCompanyID = txtDeliverCompID.Text.ToString();
					companyCounterPartyVenueDetail.DeiverToSubID = txtDeliverSubID.Text.ToString();
					companyCounterPartyVenueDetail.SenderCompanyID = txtSenderCompID.Text.ToString();
					_companyCounterPartyVenueDetails.Add(companyCounterPartyVenueDetail);
					SaveData(_companyCounterPartyVenueDetails, e);
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");

					Refresh(sender, e);
				}
			}
			SaveData(_companyCounterPartyVenueDetails, e);
			//this.Hide();
		}

		

		public void Refresh(object sender, System.EventArgs e)
		{
			cmbCounterParty.SelectedValue = int.MinValue;
			cmbCounterPartyVenue.SelectedValue = int.MinValue;
			cmbClearingFirmPrimeBrokers.SelectedValue = int.MinValue;
			txtDeliverCompID.Text = "";
			txtDeliverSubID.Text = "";
			txtSenderCompID.Text = "";
		}

		private void CreateCompanyCounterPartiesCompanyLevelTags_Load(object sender, System.EventArgs e)
		{
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				BindForEdit();
			}
			else
			{
				Refresh(sender, e);
				//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
			}
		}
		
		private Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails _companyCounterPartyVenueDetails = new  Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails();

		private void cmbCounterParty_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_companyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
			CreateCompanyCounterPartyFundLevelTags createCompanyCounterPartyFundLevelTags = new  CreateCompanyCounterPartyFundLevelTags();
			createCompanyCounterPartyFundLevelTags.CompanyCounterPartyID = _companyCounterPartyID;
			BindCounterPartyVenues();
		}

		private void cmbCounterParty_SelectedValueChanged(object sender, System.EventArgs e)
		{
			_companyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
		}
	
		public Nirvana.Admin.BLL.CompanyCounterPartyVenueDetails CurrentCompanyCounterPartyVenueDetails
		{
			get 
			{
				return _companyCounterPartyVenueDetails; 
			}
			set
			{
				if(value != null)
				{
					_companyCounterPartyVenueDetails = value;
					BindForEdit();
				}
			}			
		}
		#region Focus Colors
		private void txtDeliverCompID_GotFocus(object sender, System.EventArgs e)
		{
			txtDeliverCompID.BackColor = Color.LemonChiffon;
		}
		private void txtDeliverCompID_LostFocus(object sender, System.EventArgs e)
		{
			txtDeliverCompID.BackColor = Color.White;
		}
		private void txtDeliverSubID_GotFocus(object sender, System.EventArgs e)
		{
			txtDeliverSubID.BackColor = Color.LemonChiffon;
		}
		private void txtDeliverSubID_LostFocus(object sender, System.EventArgs e)
		{
			txtDeliverSubID.BackColor = Color.White;
		}
		private void txtSenderCompID_GotFocus(object sender, System.EventArgs e)
		{
			txtSenderCompID.BackColor = Color.LemonChiffon;
		}
		private void txtSenderCompID_LostFocus(object sender, System.EventArgs e)
		{
			txtSenderCompID.BackColor = Color.White;
		}
		private void cmbClearingFirmPrimeBrokers_GotFocus(object sender, System.EventArgs e)
		{
			cmbClearingFirmPrimeBrokers.BackColor = Color.LemonChiffon;
		}
		private void cmbClearingFirmPrimeBrokers_LostFocus(object sender, System.EventArgs e)
		{
			cmbClearingFirmPrimeBrokers.BackColor = Color.White;
		}
		private void cmbCounterPartyVenue_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyVenue_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.White;
		}
		
		#endregion
	}
}
