using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Nirvana.Admin.BLL;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Properties;

using Infragistics.Win.UltraWinGrid;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlDataSourceDetails : UserControl
    {
        private DataSource _dataSourceDetails;

        public DataSource DataSourceDetails
        {
            get { return _dataSourceDetails; }
            set { _dataSourceDetails = value; }
        }
	
       
        //private BindingSource dataSourceDetailsBindingSource = new BindingSource();

        //private DataSource _newDataSource;

        //public DataSource NewDataSource
        //{
        //    get 
        //    { 
        //        return _newDataSource; 
        //    }
        //    set 
        //    { 
        //        _newDataSource = value;
        //    }
        //}

        // Constants 
        const string C_COMBO_SELECT = "- Select -";

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlDataSourceDetails"/> class.
        /// </summary>
        public CtrlDataSourceDetails()
        {
            InitializeComponent();
            ResetComboBoxes();

//            DoDataBinding();
        
            //cmbSourceType.DataBindings.Add(new Binding("Text", dataSourceDetailsBindingSource, "FullName", true));

            // Connect to various designer services.
            
        }

        public void DoDataBinding()
        {

           // this.dataSourceDetailsBindingSource.DataSource = typeof(DataSource);
            ///TODO : Need to remove when design time property setting is enabled

            //try
            //{
                
                
                
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}

            //this.dataSourceTypeCollectionBindingSource.DataMember = "DataSourceTypeListValue";
            //this.dataSourceTypeCollectionBindingSource.DataSource = this.bindingSourceDataSource;

            //this.dataSourceTypeCollectionBindingSource.DataMember = "DataSourceTypeListValue";
            //this.dataSourceTypeCollectionBindingSource.DataSource = this.bindingSourceDataSource;

           
           // this.dsPrimaryContactDetails1.bindingSourcePrimaryContact.DataMember = "PrimaryContact";            
           // this.dsPrimaryContactDetails1.bindingSourcePrimaryContact.DataSource = this.bindingSourceDataSource;

            
            

            //Binding txtFullNameBinding = new System.Windows.Forms.Binding("Text", this.dataSourceDetailsBindingSource, "FullName");
            //this.txtFullName.DataBindings.Add(txtFullNameBinding);
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm().Close();
        }

        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear all the controls except the selected datasource.
            //cmbSourceType.Text = C_COMBO_SELECT;
            //txtAddress1.Text = string.Empty;
            //txtAddress2.Text = string.Empty;
            //cmbCountry.Text = C_COMBO_SELECT;
            //cmbStateTerritory.Text = C_COMBO_SELECT;
            //txtZip.Text = string.Empty;
            //txtWorkNumber.Text = string.Empty;
            //txtFaxNumber.Text = string.Empty;           
            

            // Clear Primary Contact Details
           // dsPrimaryContactDetails1.Clear();

        }



        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
      //      errDataSource.Clear();
         //   dsPrimaryContactDetails1.errPrimaryDetails.Clear();
            //Validate the values in all the fields.
            //bool isValid = ValidateDataSourceDetails();
            //if (isValid)
            //{ 

            //}
        }

        /// <summary>
        /// Handles the Load event of the CtrlDataSourceDetails control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CtrlDataSourceDetails_Load(object sender, EventArgs e)
        {
    //       SetUpControl();
        }

        /// <summary>
        /// Sets the up control.
        /// </summary>
        private void SetUpControl()
        {
            // Clear the Error Provider
            errDataSource.Clear();
          


            // Fill And Set the Drop Down Controls
         //   BindSourceTypes();
         //   BindCountries();
          //  BindStates();
            
        }


        /// <summary>
        /// Clears the control.
        /// </summary>
        public void ClearControl()
        {
            PopulateDataSourceDetails(new DataSourceNameID());
        }

        /// <summary>
        /// Populates the data source details.
        /// </summary>
        /// <param name="dataSourceNameID">The data source name ID.</param>
        public void PopulateDataSourceDetails(DataSourceNameID dataSourceNameID) //int dataSourceID
        {
            int dataSourceID = dataSourceNameID.ID;
            DataSourceDetails = DataSourceManager.GetDataSourceDetailsForID(dataSourceID);
            this.bindingSourceDataSource.DataSource = DataSourceDetails;

            // These will be uncommented in case we need the drop down again for DataSourceName display
            //this.ctrlSourceName1.InitControl();
            //this.ctrlSourceName1.Enabled = false;
            // Add the databindings for the indiviual controls. 
            AddDataBinding();
            

            this.bindingSourceDataSourceTypeList.DataSource = DataSourceTypeList.Retrieve;
            this.bindingSourceStatusList.DataSource = DataSourceStatusList.Retrieve;


            this.dsPrimaryContactDetails1.DataSource = DataSourceDetails;
            this.dsPrimaryContactDetails1.DataMember = "PrimaryContact";
            this.dsPrimaryContactDetails1.PopulateDetails();

            this.ctrlAddressDetails1.DataSource = DataSourceDetails;
            this.ctrlAddressDetails1.DataMember = "DataSourceAddressDetails";
            this.ctrlAddressDetails1.PopulateAddressDetails();


            

            
                
            
        }

        /// <summary>
        /// Adds the data binding.
        /// Checks their are no databindings, already added.
        /// </summary>
        private void AddDataBinding()
        {
            
            cmbSourceType.DataBindings.Clear();
            cmbSourceType.DataBindings.Add(new System.Windows.Forms.Binding("Value", DataSourceDetails, "TypeID", true));

            cmbStatus.DataBindings.Clear();
            cmbStatus.DataBindings.Add(new System.Windows.Forms.Binding("Value", DataSourceDetails, "StatusID", true));
            
            lblDataSourceName.DataBindings.Clear();
            lblDataSourceName.DataBindings.Add(new Binding("Text", DataSourceDetails.DataSourceNameID, ".FullName", true));

            ////Bind DataSource Combo Value. 
            // These will be uncommented in case we need the drop down again for DataSourceName display
            //Binding ctrlSourceNameBinding = new System.Windows.Forms.Binding("Value", DataSourceDetails.DataSourceNameID, "ID", true);
            //ctrlSourceName1.AddDataBindingForCombo(ctrlSourceNameBinding);
            
        }

        /// <summary>
        /// Binds the source types.
        /// </summary>  
        private void BindSourceTypes()
        {
            //SortableSearchableList<DataSourceType> dataSourceTypeList = new SortableSearchableList<DataSourceType>();

                       
          //  dataSourceTypeCollectionBindingSource.DataSource = DataSourceManager.GetDataSourceTypes();

         //   DataSourceTypeCollection dt = new DataSourceTypeCollection();
            
            //GetDataSourceTypes method fetches the existing countries from the database.
          //  dataSourceTypeList = DataSourceManager.GetDataSourceTypes();

            //create the - Select - option in the Combo Box at the top.
          //  DataSourceType dataSourceTypeSelect = new DataSourceType();
          //  dataSourceTypeSelect.DataSourceTypeID = int.MinValue;
          //  dataSourceTypeSelect.DataSourceTypeName = C_COMBO_SELECT;            

            
            //Insert the Select DataSource
          //  dataSourceTypeList.Insert(0, dataSourceTypeSelect);

            
         //   cmbSourceType.DisplayMember = "DataSourceTypeName";
         //   cmbSourceType.ValueMember = "DataSourceTypeID";
          //  cmbSourceType.DataSource = dataSourceTypeList;

            //cmbSourceType.Rows[0].Selected = true;
            //cmbSourceType.Value = int.MinValue;            
            
        }

        /// <summary>
        /// This method binds the existing <see cref="Countries"/> in the ComboBox control by assigning the 
        /// countries object to its datasource property.
        /// </summary>
        private void BindCountries()
        {
            ////GetCountries method fetches the existing countries from the database.
            //Countries countries = GeneralManager.GetCountries();
            ////Inserting the - Select - option in the Combo Box at the top.
            //countries.Insert(0, new Country(int.MinValue, C_COMBO_SELECT));
            //cmbCountry.DisplayMember = "Name";
            //cmbCountry.ValueMember = "CountryID";
            //cmbCountry.DataSource = countries;
            //cmbCountry.Value = int.MinValue;

            
        }

        /// <summary>
        /// Resets the combo boxes.
        /// </summary>
        private void ResetComboBoxes()
        {
            Utils.UltraComboFilter(cmbSourceType, "DataSourceTypeName");
            Utils.UltraComboFilter(cmbStatus, "Name");

        }

        /// <summary>
        /// This method binds the existing <see cref="States"/> in the ComboBox control by assigning the 
        /// states object to its datasource property.
        /// </summary>
        private void BindStates()
        {
            ////GetStates method fetches the existing states from the database.
            //States states = GeneralManager.GetStates();
            ////Inserting the - Select - option in the Combo Box at the top.
            //states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            //cmbStateTerritory.DisplayMember = "StateName";
            //cmbStateTerritory.ValueMember = "StateID";
            //cmbStateTerritory.DataSource = states;
            //cmbStateTerritory.Value = int.MinValue;
            
        }


        /// <summary>
        /// This method empties the ComboBox from any state by assigning the states object to null value.
        /// </summary>
        private void BindEmptyStates()
        {
            //GetStates method fetches the existing states from the database.
            //States states = new States();
            ////Inserting the - Select - option in the Combo Box at the top.
            //states.Insert(0, new State(int.MinValue, C_COMBO_SELECT, int.MinValue));
            //cmbStateTerritory.DisplayMember = "StateName";
            //cmbStateTerritory.ValueMember = "StateID";
            //cmbStateTerritory.DataSource = states;
            //cmbStateTerritory.Text = C_COMBO_SELECT;
        }

        /// <summary>
        /// Validates the data source details.
        /// </summary>
        /// <returns></returns>
        private bool ValidateDataSourceDetails()
        {
            bool isValidated = false;


            ////  errorProvider1.SetError(txtFirstName, "Please enter First Name in details!");
            ////  txtFirstName.Focus();
            //if (cmbSourceType.Text == C_COMBO_SELECT)
            //{
            //    errDataSource.SetError(cmbSourceType, "Please select the Source Type");
            //    cmbSourceType.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (txtAddress1 == null || txtAddress1.Text.Length == 0)
            //{
            //    errDataSource.SetError(txtAddress1, "Please enter First Line Address!");
            //    txtAddress1.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (cmbCountry.Text == C_COMBO_SELECT)
            //{
            //    errDataSource.SetError(cmbCountry, "Please select the Country");
            //    cmbCountry.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (cmbStateTerritory.Text == C_COMBO_SELECT)
            //{
            //    errDataSource.SetError(cmbStateTerritory, "Please select State/Territory");
            //    cmbStateTerritory.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (txtZip == null || txtZip.Text.Length == 0)
            //{
            //    errDataSource.SetError(txtZip, "Please enter Zip!");
            //    txtAddress1.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (txtWorkNumber == null || txtWorkNumber.Text.Length == 0)
            //{
            //    errDataSource.SetError(txtWorkNumber, "Please enter your Work Phone Number!");
            //    txtAddress1.Focus();
            //    isValidated = false;
            //    return isValidated;
            //}
            //else if (!dsPrimaryContactDetails1.ValidatePrimaryContactDetails())
            //{
            //    isValidated = false;
            //    return isValidated;
            //}
            //else
            //{
            //    int success = DataSourceManager.SaveDataSourceData((DataSource)bindingSourceDataSource.DataSource);

            //    //NewDataSource.FullName = txtFullName.Text;
            //    NewDataSource.TypeID = int.Parse(cmbSourceType.Value.ToString());
            //    //NewDataSource.Address1 = txtAddress1.Text;
            //    //NewDataSource.Address2 = txtAddress2.Text;
            //    //NewDataSource.CountryID = int.Parse(cmbCountry.Value.ToString());
            //    //NewDataSource.StateID = int.Parse(cmbStateTerritory.Value.ToString());
            //    //NewDataSource.Zip = txtZip.Text;
            //    //NewDataSource.WorkNumber = txtWorkNumber.Text;
            //    //NewDataSource.FaxNumber = txtFaxNumber.Text;
            //    //NewDataSource.PrimaryContact = dsPrimaryContactDetails1.PrimaryContact;

            //}




            //// else if(txt)

            return isValidated;
        }

        /// <summary>
        /// Handles the Click event of the btnImport control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This will open up a drop down list, of third parties, and the user will be able to choose one, and copy it's information here.", "Information", MessageBoxButtons.OK);
        }
        
    }
}
