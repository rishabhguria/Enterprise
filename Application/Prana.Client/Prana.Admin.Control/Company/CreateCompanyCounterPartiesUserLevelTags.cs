using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Nirvana.Admin.BLL;
namespace Nirvana.Admin.Controls
{
	/// <summary>
	/// Summary description for CompanyCounterPartiesUserLevelTags.
	/// </summary>
	public class CreateCompanyCounterPartiesUserLevelTags : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbCounterPartyVenue;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox cmbCounterParty;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbCMTAGiveUp;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtSubID;
		private System.Windows.Forms.ComboBox cmbUsers;
		private System.Windows.Forms.Label lblCounterParty;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateCompanyCounterPartiesUserLevelTags()
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
			BindCounterPartyVenues();
			BindCounterParty();
			BindUsers();
			BindIdentifiers();
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
			this.txtSubID = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbCounterPartyVenue = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cmbUsers = new System.Windows.Forms.ComboBox();
			this.cmbCounterParty = new System.Windows.Forms.ComboBox();
			this.cmbCMTAGiveUp = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblCounterParty);
			this.groupBox1.Controls.Add(this.txtSubID);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cmbCounterPartyVenue);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.cmbUsers);
			this.groupBox1.Controls.Add(this.cmbCounterParty);
			this.groupBox1.Controls.Add(this.cmbCMTAGiveUp);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(390, 130);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "User Level Details";
			// 
			// lblCounterParty
			// 
			this.lblCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.lblCounterParty.Location = new System.Drawing.Point(188, 20);
			this.lblCounterParty.Name = "lblCounterParty";
			this.lblCounterParty.Size = new System.Drawing.Size(168, 21);
			this.lblCounterParty.TabIndex = 14;
			// 
			// txtSubID
			// 
			this.txtSubID.Enabled = false;
			this.txtSubID.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.txtSubID.Location = new System.Drawing.Point(186, 82);
			this.txtSubID.MaxLength = 50;
			this.txtSubID.Name = "txtSubID";
			this.txtSubID.Size = new System.Drawing.Size(174, 21);
			this.txtSubID.TabIndex = 10;
			this.txtSubID.Text = "";
			this.txtSubID.LostFocus += new System.EventHandler(this.txtSubID_LostFocus);
			this.txtSubID.GotFocus += new System.EventHandler(this.txtSubID_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 18);
			this.label1.TabIndex = 0;
			this.label1.Text = "CounterParty";
			// 
			// cmbCounterPartyVenue
			// 
			this.cmbCounterPartyVenue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterPartyVenue.Enabled = false;
			this.cmbCounterPartyVenue.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterPartyVenue.Location = new System.Drawing.Point(186, 40);
			this.cmbCounterPartyVenue.Name = "cmbCounterPartyVenue";
			this.cmbCounterPartyVenue.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterPartyVenue.TabIndex = 8;
			this.cmbCounterPartyVenue.GotFocus += new System.EventHandler(this.cmbCounterPartyVenue_GotFocus);
			this.cmbCounterPartyVenue.LostFocus += new System.EventHandler(this.cmbCounterPartyVenue_LostFocus);
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label9.Location = new System.Drawing.Point(6, 106);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(100, 18);
			this.label9.TabIndex = 6;
			this.label9.Text = "CMTA/Give Up";
			// 
			// cmbUsers
			// 
			this.cmbUsers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbUsers.Enabled = false;
			this.cmbUsers.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbUsers.Location = new System.Drawing.Point(186, 62);
			this.cmbUsers.Name = "cmbUsers";
			this.cmbUsers.Size = new System.Drawing.Size(174, 21);
			this.cmbUsers.TabIndex = 9;
			this.cmbUsers.GotFocus += new System.EventHandler(this.cmbUsers_GotFocus);
			this.cmbUsers.LostFocus += new System.EventHandler(this.cmbUsers_LostFocus);
			// 
			// cmbCounterParty
			// 
			this.cmbCounterParty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCounterParty.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCounterParty.Location = new System.Drawing.Point(186, 20);
			this.cmbCounterParty.Name = "cmbCounterParty";
			this.cmbCounterParty.Size = new System.Drawing.Size(174, 21);
			this.cmbCounterParty.TabIndex = 7;
			this.cmbCounterParty.Visible = false;
			// 
			// cmbCMTAGiveUp
			// 
			this.cmbCMTAGiveUp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCMTAGiveUp.Enabled = false;
			this.cmbCMTAGiveUp.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.cmbCMTAGiveUp.Location = new System.Drawing.Point(186, 104);
			this.cmbCMTAGiveUp.Name = "cmbCMTAGiveUp";
			this.cmbCMTAGiveUp.Size = new System.Drawing.Size(174, 21);
			this.cmbCMTAGiveUp.TabIndex = 11;
			this.cmbCMTAGiveUp.GotFocus += new System.EventHandler(this.cmbCMTAGiveUp_GotFocus);
			this.cmbCMTAGiveUp.LostFocus += new System.EventHandler(this.cmbCMTAGiveUp_LostFocus);
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label10.Location = new System.Drawing.Point(6, 86);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(126, 18);
			this.label10.TabIndex = 4;
			this.label10.Text = "On Behalf of Sub ID";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label3.Location = new System.Drawing.Point(6, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(118, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "CounterPartyVenue";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.label4.Location = new System.Drawing.Point(6, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(138, 18);
			this.label4.TabIndex = 3;
			this.label4.Text = "Users";
			// 
			// btnSave
			// 
			this.btnSave.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(204)), ((System.Byte)(102)));
			this.btnSave.Enabled = false;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Location = new System.Drawing.Point(163, 138);
			this.btnSave.Name = "btnSave";
			this.btnSave.TabIndex = 12;
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
			this.label2.Location = new System.Drawing.Point(178, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 14);
			this.label2.TabIndex = 35;
			this.label2.Text = "*";
			// 
			// label5
			// 
			this.label5.ForeColor = System.Drawing.Color.Red;
			this.label5.Location = new System.Drawing.Point(178, 68);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(12, 14);
			this.label5.TabIndex = 36;
			this.label5.Text = "*";
			// 
			// label6
			// 
			this.label6.ForeColor = System.Drawing.Color.Red;
			this.label6.Location = new System.Drawing.Point(178, 108);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(12, 14);
			this.label6.TabIndex = 37;
			this.label6.Text = "*";
			// 
			// CreateCompanyCounterPartiesUserLevelTags
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "CreateCompanyCounterPartiesUserLevelTags";
			this.Size = new System.Drawing.Size(400, 170);
			this.Load += new System.EventHandler(this.CreateCompanyCounterPartiesUserLevelTags_Load);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void BindCounterParty()
		{
			
			CounterParties counterParties = CounterPartyManager.GetCompanyCounterParties(_companyID);
//			if (counterParties.Count > 0 )
//			{
				counterParties.Insert(0, new CounterParty(int.MinValue, C_COMBO_SELECT));
				cmbCounterParty.DataSource = counterParties;				
				cmbCounterParty.DisplayMember = "CounterPartyFullName";
				cmbCounterParty.ValueMember = "CounterPartyID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCounterParty.SelectedValue = _companyCounterPartyVenueDetailEdit.CompanyCounterPartyID;
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
				cmbCounterPartyVenue.SelectedValue = _companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID;
			}
		}
		
		private void BindUsers()
		{
			cmbUsers.DataSource = null;
			Users users = UserManager.GetUsers(_companyID);
//			if (users.Count > 0 )
//			{
				users.Insert(0, new User(int.MinValue, C_COMBO_SELECT));
				cmbUsers.DataSource = users;				
				cmbUsers.DisplayMember = "FirstName";
				cmbUsers.ValueMember = "UserID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbUsers.SelectedValue = _companyCounterPartyVenueDetailEdit.UserID;
			}
		}
		
		private void BindIdentifiers()
		{
			Identifiers identifiers = AUECManager.GetIdentifiers();
//			if (identifiers.Count > 0 )
//			{
				identifiers.Insert(0, new Identifier(int.MinValue, C_COMBO_SELECT));
				cmbCMTAGiveUp.DataSource = identifiers;				
				cmbCMTAGiveUp.DisplayMember = "IdentifierName";
				cmbCMTAGiveUp.ValueMember = "IdentifierID";
//			}
			
			if(_companyCounterPartyVenueDetailEdit != null)
			{
				cmbCMTAGiveUp.SelectedValue = _companyCounterPartyVenueDetailEdit.CMTAGiveUp;
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
				cmbUsers.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.UserID.ToString());
				txtSubID.Text = _companyCounterPartyVenueDetailEdit.OnBehalfOfSubID.ToString();
				cmbCMTAGiveUp.SelectedValue = int.Parse(_companyCounterPartyVenueDetailEdit.CMTAGiveUp.ToString());
			}
		}

		private int _companyID = int.MinValue;
		public int CompanyID
		{
			set
			{
				_companyID = value;
				BindIdentifiers();
				BindUsers();
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
				errorProvider1.SetError(cmbCMTAGiveUp, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				errorProvider1.SetError(cmbUsers, "");
				if(int.Parse(cmbCounterPartyVenue.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbUsers.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbUsers, "Please select User!");
					cmbUsers.Focus();
				}
				else if(int.Parse(cmbCMTAGiveUp.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCMTAGiveUp, "Please select CMTA/GiveUp!");
					cmbCMTAGiveUp.Focus();
				}
				else
				{
					//_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyID = _companyCounterPartyID;
					_companyCounterPartyVenueDetailEdit.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.UserID = int.Parse(cmbUsers.SelectedValue.ToString());
					_companyCounterPartyVenueDetailEdit.OnBehalfOfSubID = txtSubID.Text.ToString();
					_companyCounterPartyVenueDetailEdit.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.Text.ToString());
										
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
				}
			}
			else
			{
				errorProvider1.SetError(cmbCMTAGiveUp, "");
				errorProvider1.SetError(cmbCounterPartyVenue, "");
				errorProvider1.SetError(cmbUsers, "");
				if(int.Parse(cmbCounterPartyVenue.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCounterPartyVenue, "Please select Counter Party Venue!");
					cmbCounterPartyVenue.Focus();
				}
				else if(int.Parse(cmbUsers.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbUsers, "Please select User!");
					cmbUsers.Focus();
				}
				else if(int.Parse(cmbCMTAGiveUp.SelectedValue.ToString()) == int.MinValue)
				{
					errorProvider1.SetError(cmbCMTAGiveUp, "Please select CMTA/GiveUp!");
					cmbCMTAGiveUp.Focus();
				}
				else
				{
					Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail companyCounterPartyVenueDetail = new Nirvana.Admin.BLL.CompanyCounterPartyVenueDetail();

					
					//companyCounterPartyVenueDetail.CompanyCounterPartyID = int.Parse(cmbCounterParty.SelectedValue.ToString());
					companyCounterPartyVenueDetail.CompanyCounterPartyID = _companyCounterPartyID;
					companyCounterPartyVenueDetail.CompanyCounterPartyVenueID = int.Parse(cmbCounterPartyVenue.SelectedValue.ToString());
					companyCounterPartyVenueDetail.UserID = int.Parse(cmbUsers.SelectedValue.ToString());
					companyCounterPartyVenueDetail.OnBehalfOfSubID = txtSubID.Text.ToString();
					companyCounterPartyVenueDetail.CMTAGiveUp = int.Parse(cmbCMTAGiveUp.SelectedValue.ToString());
					_companyCounterPartyVenueDetails.Add(companyCounterPartyVenueDetail);
					
					//Nirvana.Admin.Utility.Common.ResetStatusPanel(stbCreateCompanyStrategy);
					//Nirvana.Admin.Utility.Common.SetStatusPanel(stbCreateCompanyStrategy, "Stored!");
				}
			}
			SaveData(_companyCounterPartyVenueDetails, e);
			//this.Hide();
		}

		public void Refresh(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.SelectedValue = int.MinValue;
			cmbCounterParty.SelectedValue = int.MinValue;
			cmbCounterPartyVenue.SelectedValue = int.MinValue;
			cmbUsers.Text = "";
			txtSubID.Text = "";
		}

		private void CreateCompanyCounterPartiesUserLevelTags_Load(object sender, System.EventArgs e)
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
				}
			}			
		}
		#region Focus Colors
		private void cmbCMTAGiveUp_GotFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.LemonChiffon;
		}
		private void cmbCMTAGiveUp_LostFocus(object sender, System.EventArgs e)
		{
			cmbCMTAGiveUp.BackColor = Color.White;
		}
		private void cmbCounterPartyVenue_GotFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.LemonChiffon;
		}
		private void cmbCounterPartyVenue_LostFocus(object sender, System.EventArgs e)
		{
			cmbCounterPartyVenue.BackColor = Color.White;
		}
		private void cmbUsers_GotFocus(object sender, System.EventArgs e)
		{
			cmbUsers.BackColor = Color.LemonChiffon;
		}
		private void cmbUsers_LostFocus(object sender, System.EventArgs e)
		{
			cmbUsers.BackColor = Color.White;
		}
		private void txtSubID_GotFocus(object sender, System.EventArgs e)
		{
			txtSubID.BackColor = Color.LemonChiffon;
		}
		private void txtSubID_LostFocus(object sender, System.EventArgs e)
		{
			txtSubID.BackColor = Color.White;
		}
		
		#endregion
	}
}
