using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;
using Nirvana.Admin.PositionManagement.Classes;


namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlCompanyApplicationDetails : UserControl
    {

        private CompanyApplicationDetails _companyApplicationDetails;

        /// <summary>
        /// Gets or sets the company Applicaion Details.
        /// </summary>
        /// <value>The company Application details.</value>
        public CompanyApplicationDetails CompanyApplicationDetails
        {
            get { return _companyApplicationDetails; }
            set { _companyApplicationDetails = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CtrlCompanyApplicationDetails"/> class.
        /// </summary>
        public CtrlCompanyApplicationDetails()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clears the control.
        /// </summary>
        public void ClearControl()
        {
            PopulateCompanyApplicationDetails(0);
        }

        /// <summary>
        /// Populates the company application details by fetching them from the dataBase.
        /// Also populates the list box for the DataSources.
        /// </summary>
        /// <param name="companyID">The company ID.</param>
       public void PopulateCompanyApplicationDetails(int companyID)
       {

           
           CompanyApplicationDetails = CompanyManager.GetCompanyApplicationDetailsForID(companyID);
           
           bindingSourceApplicationDetails.DataSource = CompanyApplicationDetails;

           // Add the binding information for the controls. 
           AddDataBindings();

           //populate the list of the DataSources in the CheckList Box.
           PopulateCheckedListBox();

           SortableSearchableList<DataSourceNameID> companyDataSourcesList = CompanyApplicationDetails.DataSourceNameIDList;
                      
           
           for(int counter = 0; counter < checkedListBoxUploadDataSources.Items.Count ; counter ++)
           {
               DataSourceNameID currentDataSource = (DataSourceNameID)checkedListBoxUploadDataSources.Items[counter];
               foreach (DataSourceNameID dataSourceNameIDFromCompany in companyDataSourcesList)
               {
                   if (dataSourceNameIDFromCompany.ID.Equals(currentDataSource.ID))
                   {
                       checkedListBoxUploadDataSources.SetItemChecked(counter, true);
                   }
               }
           }

       }


        /// <summary>
        /// Adds the data bindings for all the controls.
        /// </summary>
        private void AddDataBindings()
        {
            
            rdbModels.DataBindings.Clear();
            rdbModels.DataBindings.Add(new System.Windows.Forms.Binding("Value", CompanyApplicationDetails, "PricingModel.ID", true));

            
            chkBoxAllowDailyImport.DataBindings.Clear();
            chkBoxAllowDailyImport.DataBindings.Add(new System.Windows.Forms.Binding("Checked", CompanyApplicationDetails, "AllowDailyImport", true));
            
            chkBoxAllowDataMapping.DataBindings.Clear();
            chkBoxAllowDataMapping.DataBindings.Add(new System.Windows.Forms.Binding("Checked", CompanyApplicationDetails, "AllowDataMapping", true));
            
            spnMaximumRefreshRate.DataBindings.Clear();
            spnMaximumRefreshRate.DataBindings.Add(new System.Windows.Forms.Binding("Value", CompanyApplicationDetails, "MinimumRefreshRate", true));
        }

       /// <summary>
       /// Populate the checked list box wuth the list of the DatSources.
       /// </summary>
        private void PopulateCheckedListBox()
        {
            DataSourceNameIDList.GetInstance().SelectItemRequired = false;
            DataSourceNameIDList.GetInstance().IsAllDataSourceAvailable = false;
            
            bindingSourceUploadDataSources.DataSource = DataSourceNameIDList.GetInstance().Retrieve;

            checkedListBoxUploadDataSources.DataSource = bindingSourceUploadDataSources;
            checkedListBoxUploadDataSources.DisplayMember = "FullName";
            checkedListBoxUploadDataSources.ValueMember = "ID";
            checkedListBoxUploadDataSources.CheckOnClick = true;
           // checkedListBoxUploadDataSources.SelectionMode = SelectionMode.MultiSimple;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            //ToDo--- 
        }

        /// <summary>
        /// Handles the Click event of the btnClear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            
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
    }
}
