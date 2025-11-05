using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.MiscUtilities;
using Prana.PM.BLL;
//using Prana.PM.Common;
using Prana.PM.DAL;
using Prana.BusinessObjects.PositionManagement;

namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlCompanyDetails : UserControl
    {
        private Company _companyDetails = new Company();
        private const string SELECTVALUE = "--select--";

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlCompanyDetails"/> class.
        /// </summary>
        public CtrlCompanyDetails()
        {
            InitializeComponent();
        }

        

        /// <summary>
        /// Handles the Click event of the btnUploadLogo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnUploadLogo_Click(object sender, EventArgs e)
        {
            Stream myStream = null;

              openFileDialog1.InitialDirectory = "c:\\" ;
              openFileDialog1.Filter = "jpg|*.jpg|bmp|*.bmp|gif|*.gif|image files (*.gif; *.jpg; *.bmp)|*.gif; *.jpg; *.bmp"; 
              openFileDialog1.FilterIndex = 2 ;
              openFileDialog1.RestoreDirectory = true ;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //string filePath = openFileDialog1.ge
                            string fileName = openFileDialog1.FileName;

                            if (!String.IsNullOrEmpty(fileName))
                            {
                                txtfilePath.Text = fileName;
                            }
                            
                            // Insert code to read the stream here.
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
          }

          /// <summary>
          /// Clears the control.
          /// </summary>
        public void ClearControl()
        {
            //PopulateCompanyDetails(0);
        }

        private bool _isInitialized = false;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

          /// <summary>
          /// Populates the company details.
          /// </summary>
          /// <param name="companyID">The company ID</param>
        public void PopulateCompanyDetails(Prana.BusinessObjects.PositionManagement.CompanyNameID companyNameID)
        {

            int companyID = companyNameID.ID;

            //if (!int.Equals(companyID, 0))
            // {
            _companyDetails = CompanyManager.GetCompanyDetailsForID(companyID);
            _companyDetails.AdminUser.ConfirmPassword = "";

            if (!_isInitialized)
            {
                SetUpControlBindings(companyNameID.ID);
                _isInitialized = true;
            }


            this.bindingSourceUserList.DataSource = CompanyManager.GetCompanyUserList(companyID);
            this.bindingSourceCompanyDetails.DataSource = _companyDetails;

            this._companyDetails.BeginEdit();
            this._companyDetails.AdminUser.BeginEdit();

            //  }
        }

          /// <summary>
          /// Sets the up control bindings.
          /// </summary>
          /// <param name="companyID">The company ID.</param>
        private void SetUpControlBindings(int companyID)
        {
            //errCompanyDetails.DataSource = this.bindingSourceCompanyDetails;
            this.bindingSourceCompanyDetails.DataSource = _companyDetails;
            //this.bindingSourceCompanyDetails.DataMember = "AdminUser";
            AddDataBindings();
            this.bindingSourceCompanyTypeList.DataSource = CompanyTypeList.Retrieve;

            isInternalValueChangedEvent = true;
            
            isInternalValueChangedEvent = false;

            this.ctrlAddressDetails1.DataSource = bindingSourceCompanyDetails;
            this.ctrlAddressDetails1.DataMember = "AddressDetails";
            this.ctrlAddressDetails1.PopulateAddressDetails();
            this.ctrlAddressDetails1.IsEnabled = false;

            ResetComboBoxes();
            errCompanyDetails.DataSource = this.bindingSourceCompanyDetails;
            errCompanyDetails.DataMember = "AdminUser";
        }

        /// <summary>
        /// Refreshes the details.
        /// </summary>
        public void RefreshDetails()
        {
            CompanyNameID companyNameID = new CompanyNameID(0, "None");

            PopulateCompanyDetails(companyNameID);           
            
            
        }

        bool isInternalValueChangedEvent = false;

        /// <summary>
        /// Adds the data bindings.
        /// </summary>
        private void AddDataBindings()
        {
            if (_companyDetails == null)
            {
                _companyDetails = new Company();
            }
            txtCompanyFullName.DataBindings.Clear();
            this.txtCompanyFullName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "CompanyNameID.FullName", true));
            txtShortName.DataBindings.Clear();
            this.txtShortName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "CompanyNameID.ShortName", true));
            cmbCompanyType.DataBindings.Clear();
            this.cmbCompanyType.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSourceCompanyDetails, "CompanyType.CompanyTypeID", true));

            this.numericUpDown1.DataBindings.Clear();
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSourceCompanyDetails, "NumberOfUserLicences", true));
            isInternalValueChangedEvent = true;
            cmbUsers.DataBindings.Clear();
            this.cmbUsers.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSourceCompanyDetails, "AdminUser.CompanyUserID", true));
            isInternalValueChangedEvent = false;
            SetDataBindingForAdminUser();
            
        }

        /// <summary>
        /// Adds the data binding for admin user.
        /// </summary>
        private void SetDataBindingForAdminUser()
        {
            //this.bindingSourceCompanyDetails.DataMember = "AdminUser";
            txtACFirstName.DataBindings.Clear();
            this.txtACFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminFirstName", true));
            txtACLastName.DataBindings.Clear();
            this.txtACLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminLastName", true));
            txtACTitle.DataBindings.Clear();
            this.txtACTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminTitle", true));
            txtLogin.DataBindings.Clear();
            this.txtLogin.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.ID", true));
            txtPassword.DataBindings.Clear();
            this.txtPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.Password", true));
            txtACEmail.DataBindings.Clear();
            this.txtACEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminEmail", true));
            txtACWorkNumber.DataBindings.Clear();
            this.txtACWorkNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminWorkNumber", true));
            txtACCellNumber.DataBindings.Clear();
            this.txtACCellNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminCellNumber", true));
            txtPagerNumber.DataBindings.Clear();
            this.txtPagerNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminPagerNumber", true));
            txtHomeNumberRequired.DataBindings.Clear();
            this.txtHomeNumberRequired.DataBindings.Add(new System.Windows.Forms.Binding("Text", _companyDetails, "AdminUser.AdminHomeNumber", true));
            txtFaxNumber.DataBindings.Clear();
            this.txtFaxNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.AdminFaxNumber", true));

            txtConfirmPassword.DataBindings.Clear();
            this.txtConfirmPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSourceCompanyDetails, "AdminUser.ConfirmPassword", true));
        }
   

          /// <summary>
          /// Resets the combo boxes.
          /// </summary>
          private void ResetComboBoxes()
          {
              Utils.UltraComboFilter(this.cmbCompanyType, "Type");
              Utils.UltraComboFilter(this.cmbUsers, "UserName");
          }

          /// <summary>
          /// Handles the Click event of the btnSave control.
          /// </summary>
          /// <param name="sender">The source of the event.</param>
          /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this._companyDetails.ApplyEdit();
            this._companyDetails.AdminUser.ApplyEdit();
            //if (!string.Equals(txtConfirmPassword.Text, _companyDetails.AdminUser.Password))
            //{
            //    MessageBox.Show("Confirm Password does not match the above transaction passowrd.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txtConfirmPassword.Focus();

            //}
            //else if (int.Equals(cmbUsers.Value , 0))
            //{
            //    MessageBox.Show("Please select some user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    cmbUsers.Focus();

            //}
            //else
            //{
                if (_companyDetails.AdminUser.IsValid == true)
                {
                    int numberOfRowsEffected = CompanyManager.SavePMCompanyDetails(_companyDetails);
                    this._companyDetails.BeginEdit();
                    this._companyDetails.AdminUser.BeginEdit();
                    MessageBox.Show(this, "Company Details saved !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            //}
        }

        /// <summary>
        /// Handles the ValueChanged event of the cmbUsers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cmbUsers_ValueChanged(object sender, EventArgs e)
        {
            if (!isInternalValueChangedEvent)
            {
                int selectedUserID = Convert.ToInt32(cmbUsers.Value);
                txtConfirmPassword.Text = string.Empty;
                _companyDetails.AdminUser = CompanyManager.GetCompanyAdminContactDetailsForID(selectedUserID);
                //SetDataBindingForAdminUser();
                errCompanyDetails.DataSource = this.bindingSourceCompanyDetails;
                errCompanyDetails.DataMember = "AdminUser";
            }
        }

        /// <summary>
        /// Handles the Leave event of the txtConfirmPassword control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtConfirmPassword_Leave(object sender, EventArgs e)
        {
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //this
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this._companyDetails.CancelEdit();
            this._companyDetails.AdminUser.CancelEdit();
        }

        
    }
}
