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
	/// Summary description for ThirdPartyForm.
	/// </summary>
	public class ThirdPartyForm : System.Windows.Forms.UserControl
	{
		const string C_COMBO_SELECT = "- Select -";
		
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtWorkTele;
		private System.Windows.Forms.TextBox txtAddress2;
		private System.Windows.Forms.TextBox txtAddress1;
		private System.Windows.Forms.TextBox txtShortName;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtFax;
		private System.Windows.Forms.TextBox txtCellPhone;
		private System.Windows.Forms.TextBox txtContactPerson;
		private System.Windows.Forms.TextBox txtThirdPartyName;
		private Infragistics.Win.UltraWinGrid.UltraCombo cmbThirdPartyType;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ThirdPartyForm()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			BindThirdPartyType();
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn1 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeName", 0);
			Infragistics.Win.UltraWinGrid.UltraGridColumn ultraGridColumn2 = new Infragistics.Win.UltraWinGrid.UltraGridColumn("ThirdPartyTypeID", 1);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmbThirdPartyType = new Infragistics.Win.UltraWinGrid.UltraCombo();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtFax = new System.Windows.Forms.TextBox();
			this.txtWorkTele = new System.Windows.Forms.TextBox();
			this.txtCellPhone = new System.Windows.Forms.TextBox();
			this.txtContactPerson = new System.Windows.Forms.TextBox();
			this.txtAddress2 = new System.Windows.Forms.TextBox();
			this.txtAddress1 = new System.Windows.Forms.TextBox();
			this.txtShortName = new System.Windows.Forms.TextBox();
			this.txtThirdPartyName = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmbThirdPartyType);
			this.groupBox1.Controls.Add(this.txtEmail);
			this.groupBox1.Controls.Add(this.txtFax);
			this.groupBox1.Controls.Add(this.txtWorkTele);
			this.groupBox1.Controls.Add(this.txtCellPhone);
			this.groupBox1.Controls.Add(this.txtContactPerson);
			this.groupBox1.Controls.Add(this.txtAddress2);
			this.groupBox1.Controls.Add(this.txtAddress1);
			this.groupBox1.Controls.Add(this.txtShortName);
			this.groupBox1.Controls.Add(this.txtThirdPartyName);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
			this.groupBox1.Location = new System.Drawing.Point(4, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(374, 256);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Third Party Contact Details";
			// 
			// cmbThirdPartyType
			// 
			this.cmbThirdPartyType.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
			ultraGridColumn1.Header.Caption = "";
			ultraGridColumn1.Header.Enabled = false;
			ultraGridColumn1.Header.VisiblePosition = 0;
			ultraGridColumn1.Width = 200;
			ultraGridColumn2.Header.VisiblePosition = 1;
			ultraGridColumn2.Hidden = true;
			ultraGridBand1.Columns.AddRange(new object[] {
															 ultraGridColumn1,
															 ultraGridColumn2});
			this.cmbThirdPartyType.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			this.cmbThirdPartyType.DisplayMember = "";
			this.cmbThirdPartyType.DropDownStyle = Infragistics.Win.UltraWinGrid.UltraComboStyle.DropDownList;
			this.cmbThirdPartyType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbThirdPartyType.Location = new System.Drawing.Point(150, 38);
			this.cmbThirdPartyType.Name = "cmbThirdPartyType";
			this.cmbThirdPartyType.Size = new System.Drawing.Size(200, 22);
			this.cmbThirdPartyType.TabIndex = 162;
			this.cmbThirdPartyType.ValueMember = "";
			this.cmbThirdPartyType.LostFocus += new System.EventHandler(this.cmbThirdPartyType_LostFocus);
			this.cmbThirdPartyType.GotFocus += new System.EventHandler(this.cmbThirdPartyType_GotFocus);
			// 
			// txtEmail
			// 
			this.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtEmail.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtEmail.Location = new System.Drawing.Point(150, 230);
			this.txtEmail.MaxLength = 50;
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(200, 21);
			this.txtEmail.TabIndex = 170;
			this.txtEmail.Text = "Email";
			this.txtEmail.LostFocus += new System.EventHandler(this.txtEmail_LostFocus);
			this.txtEmail.GotFocus += new System.EventHandler(this.txtEmail_GotFocus);
			// 
			// txtFax
			// 
			this.txtFax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtFax.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtFax.Location = new System.Drawing.Point(150, 206);
			this.txtFax.MaxLength = 50;
			this.txtFax.Name = "txtFax";
			this.txtFax.Size = new System.Drawing.Size(200, 21);
			this.txtFax.TabIndex = 169;
			this.txtFax.Text = "";
			this.txtFax.LostFocus += new System.EventHandler(this.txtFax_LostFocus);
			this.txtFax.GotFocus += new System.EventHandler(this.txtFax_GotFocus);
			// 
			// txtWorkTele
			// 
			this.txtWorkTele.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtWorkTele.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtWorkTele.Location = new System.Drawing.Point(150, 182);
			this.txtWorkTele.MaxLength = 50;
			this.txtWorkTele.Name = "txtWorkTele";
			this.txtWorkTele.Size = new System.Drawing.Size(200, 21);
			this.txtWorkTele.TabIndex = 168;
			this.txtWorkTele.Text = "";
			this.txtWorkTele.LostFocus += new System.EventHandler(this.txtWorkTele_LostFocus);
			this.txtWorkTele.GotFocus += new System.EventHandler(this.txtWorkTele_GotFocus);
			// 
			// txtCellPhone
			// 
			this.txtCellPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtCellPhone.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtCellPhone.Location = new System.Drawing.Point(150, 158);
			this.txtCellPhone.MaxLength = 50;
			this.txtCellPhone.Name = "txtCellPhone";
			this.txtCellPhone.Size = new System.Drawing.Size(200, 21);
			this.txtCellPhone.TabIndex = 167;
			this.txtCellPhone.Text = "";
			this.txtCellPhone.LostFocus += new System.EventHandler(this.txtCellPhone_LostFocus);
			this.txtCellPhone.GotFocus += new System.EventHandler(this.txtCellPhone_GotFocus);
			// 
			// txtContactPerson
			// 
			this.txtContactPerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtContactPerson.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtContactPerson.Location = new System.Drawing.Point(150, 134);
			this.txtContactPerson.MaxLength = 50;
			this.txtContactPerson.Name = "txtContactPerson";
			this.txtContactPerson.Size = new System.Drawing.Size(200, 21);
			this.txtContactPerson.TabIndex = 166;
			this.txtContactPerson.Text = "";
			this.txtContactPerson.LostFocus += new System.EventHandler(this.txtContactPerson_LostFocus);
			this.txtContactPerson.GotFocus += new System.EventHandler(this.txtContactPerson_GotFocus);
			// 
			// txtAddress2
			// 
			this.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtAddress2.Location = new System.Drawing.Point(150, 110);
			this.txtAddress2.MaxLength = 50;
			this.txtAddress2.Name = "txtAddress2";
			this.txtAddress2.Size = new System.Drawing.Size(200, 21);
			this.txtAddress2.TabIndex = 165;
			this.txtAddress2.Text = "";
			this.txtAddress2.LostFocus += new System.EventHandler(this.txtAddress2_LostFocus);
			this.txtAddress2.GotFocus += new System.EventHandler(this.txtAddress2_GotFocus);
			// 
			// txtAddress1
			// 
			this.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAddress1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtAddress1.Location = new System.Drawing.Point(150, 86);
			this.txtAddress1.MaxLength = 50;
			this.txtAddress1.Name = "txtAddress1";
			this.txtAddress1.Size = new System.Drawing.Size(200, 21);
			this.txtAddress1.TabIndex = 164;
			this.txtAddress1.Text = "";
			this.txtAddress1.LostFocus += new System.EventHandler(this.txtAddress1_LostFocus);
			this.txtAddress1.GotFocus += new System.EventHandler(this.txtAddress1_GotFocus);
			// 
			// txtShortName
			// 
			this.txtShortName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtShortName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtShortName.Location = new System.Drawing.Point(150, 62);
			this.txtShortName.MaxLength = 50;
			this.txtShortName.Name = "txtShortName";
			this.txtShortName.Size = new System.Drawing.Size(200, 21);
			this.txtShortName.TabIndex = 163;
			this.txtShortName.Text = "";
			this.txtShortName.LostFocus += new System.EventHandler(this.txtShortName_LostFocus);
			this.txtShortName.GotFocus += new System.EventHandler(this.txtShortName_GotFocus);
			// 
			// txtThirdPartyName
			// 
			this.txtThirdPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtThirdPartyName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtThirdPartyName.Location = new System.Drawing.Point(150, 14);
			this.txtThirdPartyName.MaxLength = 50;
			this.txtThirdPartyName.Name = "txtThirdPartyName";
			this.txtThirdPartyName.Size = new System.Drawing.Size(200, 21);
			this.txtThirdPartyName.TabIndex = 161;
			this.txtThirdPartyName.Text = "";
			this.txtThirdPartyName.LostFocus += new System.EventHandler(this.txtThirdPartyName_LostFocus);
			this.txtThirdPartyName.GotFocus += new System.EventHandler(this.txtThirdPartyName_GotFocus);
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label10.Location = new System.Drawing.Point(10, 234);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(126, 16);
			this.label10.TabIndex = 159;
			this.label10.Text = "Email";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label9.Location = new System.Drawing.Point(10, 210);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(126, 16);
			this.label9.TabIndex = 158;
			this.label9.Text = "Fax #";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(10, 186);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(126, 16);
			this.label8.TabIndex = 157;
			this.label8.Text = "Work TelePhone #";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(10, 162);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(126, 16);
			this.label7.TabIndex = 156;
			this.label7.Text = "Cell Phone #";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label6.Location = new System.Drawing.Point(10, 138);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(126, 16);
			this.label6.TabIndex = 155;
			this.label6.Text = "Contact Person";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label5.Location = new System.Drawing.Point(10, 114);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(126, 16);
			this.label5.TabIndex = 154;
			this.label5.Text = "Address2";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(10, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(126, 16);
			this.label4.TabIndex = 153;
			this.label4.Text = "Address1";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(10, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(126, 16);
			this.label3.TabIndex = 152;
			this.label3.Text = "Short Name";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(10, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 16);
			this.label2.TabIndex = 151;
			this.label2.Text = "Third Party Type";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(10, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(126, 16);
			this.label1.TabIndex = 150;
			this.label1.Text = "Third Party Name";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// ThirdPartyForm
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(236)), ((System.Byte)(233)), ((System.Byte)(216)));
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Verdana", 8.25F);
			this.Name = "ThirdPartyForm";
			this.Size = new System.Drawing.Size(384, 264);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.cmbThirdPartyType)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public ThirdParty ThirdPartyProperty
		{
			get
			{
				ThirdParty thirdParty = new ThirdParty();
				GetThirdPartyDetails(thirdParty);
				return thirdParty;
			}
			set
			{
				SetThirdPartyDetails(value);
			}
		}	 	
		
		public void GetThirdPartyDetails(ThirdParty thirdParty)
		{
			thirdParty.ThirdPartyName = txtThirdPartyName.Text.ToString();
			thirdParty.ThirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
			thirdParty.ShortName = txtShortName.Text.ToString();
			thirdParty.Address1 = txtAddress1.Text.ToString();
			thirdParty.Address2 = txtAddress2.Text.ToString();
			thirdParty.ContactPerson = txtContactPerson.Text.ToString();
			thirdParty.CellPhone = txtCellPhone.Text.ToString();
			thirdParty.WorkTelephone = txtWorkTele.ToString();
			thirdParty.Fax = txtFax.Text.ToString();
			thirdParty.Email = txtEmail.Text.ToString();
		}

		public bool GetThirdPartyDetailsForSave(ThirdParty thirdParty)
		{
			bool result = false;
			errorProvider1.SetError(txtThirdPartyName, "");
			errorProvider1.SetError(cmbThirdPartyType, "");
			errorProvider1.SetError(txtAddress1, "");
			errorProvider1.SetError(txtContactPerson, "");
			errorProvider1.SetError(txtCellPhone, "");
			errorProvider1.SetError(txtEmail, "");
			
			if (txtThirdPartyName.Text == "")
			{
				errorProvider1.SetError(txtThirdPartyName, "Please enter display name!");
				txtThirdPartyName.Focus();
			}
			else if (int.Parse(cmbThirdPartyType.Value.ToString()) == int.MinValue)
			{
				errorProvider1.SetError(cmbThirdPartyType, "Please enter display name!");
				cmbThirdPartyType.Focus();
			}
			else if(txtAddress1.Text == "")
			{
				errorProvider1.SetError(txtAddress1, "Please enter Address1!");
				txtAddress1.Focus();
			}
			else if (txtContactPerson.Text == "")
			{
				errorProvider1.SetError(txtContactPerson, "Please enter Contact person name!");
				txtContactPerson.Focus();
			}
			else if (txtCellPhone.Text == "")
			{
				errorProvider1.SetError(txtCellPhone, "Please enter Cell phone no!");
				txtCellPhone.Focus();
			}
			else if (txtEmail.Text == "")
			{
				errorProvider1.SetError(txtEmail, "Please enter Email!");
				txtEmail.Focus();
			}
			else
			{
				thirdParty.ThirdPartyName = txtThirdPartyName.Text.ToString();
				thirdParty.ThirdPartyTypeID = int.Parse(cmbThirdPartyType.Value.ToString());
				thirdParty.ShortName = txtShortName.Text.ToString();
				thirdParty.Address1 = txtAddress1.Text.ToString();
				thirdParty.Address2 = txtAddress2.Text.ToString();
				thirdParty.ContactPerson = txtContactPerson.Text.ToString();
				thirdParty.CellPhone = txtCellPhone.Text.ToString();
				thirdParty.WorkTelephone = txtWorkTele.Text.ToString();
				thirdParty.Fax = txtFax.Text.ToString();
				thirdParty.Email = txtEmail.Text.ToString();
				return result = true;
			}
			return result;

		}
		public void SetThirdPartyDetails(ThirdParty	thirdParty)
		{
			txtThirdPartyName.Text = thirdParty.ThirdPartyName; 
			cmbThirdPartyType.Value = thirdParty.ThirdPartyTypeID;
			txtShortName.Text = thirdParty.ShortName;
			txtAddress1.Text = thirdParty.Address1; 
			txtAddress2.Text = thirdParty.Address2;
			txtContactPerson.Text = thirdParty.ContactPerson;
			txtCellPhone.Text = thirdParty.CellPhone;
			txtWorkTele.Text = thirdParty.WorkTelephone;
			txtFax.Text = thirdParty.Fax;
			txtEmail.Text = thirdParty.Email;
		}

		public void RefreshThirdPartyDetails()
		{
			txtAddress1.Text = "";
			txtAddress2.Text = "";
			txtCellPhone.Text = "";
			txtContactPerson.Text = "";
			txtEmail.Text = "";
			txtFax.Text = "";
			txtShortName.Text ="";
			txtThirdPartyName.Text = "";
			txtWorkTele.Text = "";
		}
		
		private void BindThirdPartyType()
		{
			ThirdPartyTypes thirdPartyTypes = ThirdPartyManager.GetThirdPartyTypes();
			if (thirdPartyTypes.Count > 0 )
			{
				thirdPartyTypes.Insert(0, new  ThirdPartyType(int.MinValue, C_COMBO_SELECT));
				cmbThirdPartyType.DataSource = thirdPartyTypes;
				cmbThirdPartyType.DisplayMember = "ThirdPartyTypeName";
				cmbThirdPartyType.ValueMember = "ThirdPartyTypeID";
			}
		}

		#region Focus Colors
		private void cmbThirdPartyType_GotFocus(object sender, System.EventArgs e)
		{
			cmbThirdPartyType.BackColor = Color.LemonChiffon;
		}
		private void cmbThirdPartyType_LostFocus(object sender, System.EventArgs e)
		{
			cmbThirdPartyType.BackColor = Color.White;
		}
		private void txtAddress1_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.LemonChiffon;
		}
		private void txtAddress1_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress1.BackColor = Color.White;
		}
		private void txtAddress2_GotFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.LemonChiffon;
		}
		private void txtAddress2_LostFocus(object sender, System.EventArgs e)
		{
			txtAddress2.BackColor = Color.White;
		}
		private void txtCellPhone_GotFocus(object sender, System.EventArgs e)
		{
			txtCellPhone.BackColor = Color.LemonChiffon;
		}
		private void txtCellPhone_LostFocus(object sender, System.EventArgs e)
		{
			txtCellPhone.BackColor = Color.White;
		}
		private void txtContactPerson_GotFocus(object sender, System.EventArgs e)
		{
			txtContactPerson.BackColor = Color.LemonChiffon;
		}
		private void txtContactPerson_LostFocus(object sender, System.EventArgs e)
		{
			txtContactPerson.BackColor = Color.White;
		}
		private void txtEmail_GotFocus(object sender, System.EventArgs e)
		{
			txtEmail.BackColor = Color.LemonChiffon;
		}
		private void txtEmail_LostFocus(object sender, System.EventArgs e)
		{
			txtEmail.BackColor = Color.White;
		}
		private void txtFax_GotFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.LemonChiffon;
		}
		private void txtFax_LostFocus(object sender, System.EventArgs e)
		{
			txtFax.BackColor = Color.White;
		}
		private void txtShortName_GotFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.LemonChiffon;
		}
		private void txtShortName_LostFocus(object sender, System.EventArgs e)
		{
			txtShortName.BackColor = Color.White;
		}
		private void txtThirdPartyName_GotFocus(object sender, System.EventArgs e)
		{
			txtThirdPartyName.BackColor = Color.LemonChiffon;
		}
		private void txtThirdPartyName_LostFocus(object sender, System.EventArgs e)
		{
			txtThirdPartyName.BackColor = Color.White;
		}
		private void txtWorkTele_GotFocus(object sender, System.EventArgs e)
		{
			txtWorkTele.BackColor = Color.LemonChiffon;
		}
		private void txtWorkTele_LostFocus(object sender, System.EventArgs e)
		{
			txtWorkTele.BackColor = Color.White;
		}
		#endregion
	}
}
