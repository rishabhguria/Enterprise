using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCompanyDetails : UserControl
    {
        private Company _companyDetails;

        /// <summary>
        /// Gets or sets the company details.
        /// </summary>
        /// <value>The company details.</value>
        public Company CompanyDetails
        {
            get { return _companyDetails; }
            set { _companyDetails = value; }
        }


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
            PopulateCompanyDetails(0);
        }

          /// <summary>
          /// Populates the company details.
          /// </summary>
          /// <param name="companyID">The company ID</param>
          public void PopulateCompanyDetails(int companyID)
          {

              CompanyDetails = CompanyManager.GetCompanyDetailsForID(companyID);

              AddDataBindings();
              this.bindingSourceCompanyDetails.DataSource = CompanyDetails;

              this.bindingSourceCompanyTypeList.DataSource = CompanyTypeList.Retrieve;
              this.bindingSourceUserList.DataSource = CompanyManager.GetCompanyUserList(companyID);
              
              this.ctrlAddressDetails1.DataSource = CompanyDetails;
              this.ctrlAddressDetails1.DataMember = "AddressDetails";
              this.ctrlAddressDetails1.PopulateAddressDetails();

              ResetComboBoxes();
                
              //this.ctrlSourceName1.InitControl();
              //this.ctrlSourceName1.Enabled = false;
              //this.ctrlSourceName1.DataSource = ds;
              //this.ctrlSourceName1.DataMember = "DataSourceNameID";

            
          }

        private void AddDataBindings()
        {
            txtCompanyFullName.DataBindings.Clear();
            this.txtCompanyFullName.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "CompanyNameID.FullName", true));
            txtShortName.DataBindings.Clear(); 
            this.txtShortName.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "CompanyNameID.ShortName", true));
            cmbCompanyType.DataBindings.Clear();
            this.cmbCompanyType.DataBindings.Add(new System.Windows.Forms.Binding("Value", CompanyDetails, "CompanyType.CompanyTypeID", true));
            txtFaxNumber.DataBindings.Clear();
            this.txtFaxNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminFaxNumber", true));
            txtHomeNumberRequired.DataBindings.Clear();
            this.txtHomeNumberRequired.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminHomeNumber", true));
            txtPagerNumber.DataBindings.Clear();
            this.txtPagerNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminPagerNumber", true));
            txtPassword.DataBindings.Clear();
            this.txtPassword.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminUser.Password", true));
            txtLogin.DataBindings.Clear();
            this.txtLogin.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminUser.ID", true));
            txtACCellNumber.DataBindings.Clear();
            this.txtACCellNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminCellNumber", true));
            txtACWorkNumber.DataBindings.Clear();
            this.txtACWorkNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminWorkNumber", true));
            txtACEmail.DataBindings.Clear();
            this.txtACEmail.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminEmail", true));
            txtACLastName.DataBindings.Clear();
            this.txtACLastName.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminLastName", true));
            txtACTitle.DataBindings.Clear();
            this.txtACTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminTitle", true));
            txtACFirstName.DataBindings.Clear();
            this.txtACFirstName.DataBindings.Add(new System.Windows.Forms.Binding("Text", CompanyDetails, "AdminFirstName", true));
            cmbUsers.DataBindings.Clear();
            this.cmbUsers.DataBindings.Add(new System.Windows.Forms.Binding("Value", CompanyDetails, "AdminUser.ID", true));
        }
   

          /// <summary>
          /// Resets the combo boxes.
          /// </summary>
          private void ResetComboBoxes()
          {
              Utils.UltraComboFilter(this.cmbCompanyType, "Type");
              Utils.UltraComboFilter(this.cmbUsers, "UserName");
          }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
