using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Nirvana.Admin.PositionManagement.Classes;
using Nirvana.Admin.PositionManagement.Forms;
using Nirvana.Admin.PositionManagement.BusinessObjects;

namespace Nirvana.Admin.PositionManagement.Controls
{
    public partial class CtrlImportSetup : UserControl
    {
        //BindingSource ImportSetupBindingSource = new BindingSource();

        private ImportSetup _importSetup;

        private Forms.MapColumns _frmMapColumns = null;
        private Forms.MapAUEC _frmMapAUEC = null;
        private Forms.SelectColumns _frmSetupColumns = null;
        private Forms.MapFunds _frmMapFunds = null;
        private Forms.MapSymbol _frmMapSymbol = null;

        /// <summary>
        /// Gets or sets the import setup.
        /// </summary>
        /// <value>The import setup.</value>
        public ImportSetup ImportSetupValue
        {
            get { return _importSetup; }
            set { _importSetup = value; }
        }

        private DataSourceNameID _dataSourceNameIDValue;

        public DataSourceNameID DataSourceNameIDValue
        {
            get { return _dataSourceNameIDValue; }
            set { _dataSourceNameIDValue = value; }
        }

	
        public CtrlImportSetup()
        {
            InitializeComponent();
        }

        private void btnSelectColumns_Click(object sender, EventArgs e)
        {

            _frmSetupColumns = new Forms.SelectColumns(_dataSourceNameIDValue);

            _frmSetupColumns.ShowDialog();
        }      

        private void btnMapColumns_Click(object sender, EventArgs e)
        {
            _frmMapColumns = new Forms.MapColumns(_dataSourceNameIDValue);
            _frmMapColumns.ShowDialog();
        }
       

        private void btnMapAUEC_Click(object sender, EventArgs e)
        {
             _frmMapAUEC = new Forms.MapAUEC(_dataSourceNameIDValue);
             _frmMapAUEC.ShowDialog();
        }

       

        private void btnMapSymbol_Click(object sender, EventArgs e)
        {
          
             _frmMapSymbol = new Forms.MapSymbol();
             _frmMapSymbol.ShowDialog();
      
        }

        private void btnMapFunds_Click(object sender, EventArgs e)
        {
            _frmMapFunds = new Forms.MapFunds(_dataSourceNameIDValue);

            _frmMapFunds.ShowDialog();
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            this.FindForm().Close();
        }


        /// <summary>
        /// Binds the data sources.
        /// </summary>
        private void BindDataSources()
        {

            this.bindingSourceDataSourceNameList.DataSource = BusinessObjects.DataSourceNameIDList.GetInstance().Retrieve;
            this.bindingSourceImportFormatList.DataSource = BusinessObjects.ImportMethodList.Retrieve;
            this.bindingSourceImportMethodList.DataSource = BusinessObjects.ImportFormatList.Retrieve;

        }

        /// <summary>
        /// Resets the combo boxes.
        /// </summary>
        private void ResetComboBoxes()
        {
            Utils.UltraComboFilter(cmbFileFormat, "Name");
            Utils.UltraComboFilter(cmbImportMethod, "Name");
            
        }

        

        private void CtrlImportSetup_Load(object sender, EventArgs e)
        {
            //Sugandh - Commented temporarily
            
            //BindDataSources();
           // ResetComboBoxes();
            
        }

        /// <summary>
        /// Adds the data bindings for combo boxes.
        /// </summary>
        private void AddDataBindingsForComboBoxes()
        {
            this.cmbImportMethod.DataBindings.Clear();
            this.cmbImportMethod.DataBindings.Add(new System.Windows.Forms.Binding("Value", ImportSetupValue, "ImportMethod.ID", true));
            this.cmbFileFormat.DataBindings.Clear();
            this.cmbFileFormat.DataBindings.Add(new System.Windows.Forms.Binding("Value", ImportSetupValue, "ImportFormat.ID", true));
            
            Binding ctrlSourceNameBinding = new System.Windows.Forms.Binding("Value", ImportSetupValue, "DataSourceNameID.ID", true);
            ctrlSourceName1.AddDataBindingForCombo(ctrlSourceNameBinding);
        }


        public bool ValidateControl()
        {
            bool validationSuccess = true;

            //ErrorProvider.SetError(cmbSourceName, "");
            //ErrorProvider.SetError(cmbImportMethod, "");
            

            //if (cmbSourceName.Text == Constants.C_COMBO_SELECT && Convert.ToInt32(cmbSourceName.Value) == int.MinValue)
            //{
            //    cmbSourceName.Text = Constants.C_COMBO_SELECT;
            //    ErrorProvider.SetError(cmbSourceName, "Please select Name of Source!");
            //    validationSuccess = false;
            //    cmbSourceName.Focus();
            //}
            //else 
            if (cmbImportMethod.Text == Constants.C_COMBO_SELECT && Convert.ToInt32(cmbImportMethod.Value) == int.MinValue)
            {
                cmbImportMethod.Text = Constants.C_COMBO_SELECT;
                ErrorProvider.SetError(cmbImportMethod, "Please select Import Method!");
                validationSuccess = false;
                cmbImportMethod.Focus();
            }
            return validationSuccess;
        }

        private void cmbSourceName_ValueChanged(object sender, EventArgs e)
        {
            //RaiseDataSourceEvent(sender);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           //To Do: Put Add functionality here!
        }

        

        private void lblMapAUEC_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Clears the control.
        /// </summary>
        public void ClearControl()
        {
            PopulateImportSetUpDetails(0);
        }

        /// <summary>
        /// Populates the data source details.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        public void PopulateImportSetUpDetails(int dataSourceID)
        {
            ImportSetupValue = DataSourceManager.GetImportSetUPForID(dataSourceID);
            
            this.bindingSourceImportSetup.DataSource = ImportSetupValue;

            this.bindingSourceImportFormatList.DataSource = ImportFormatList.Retrieve;
            Utils.UltraComboFilter(this.cmbImportMethod, "Name");

            this.bindingSourceImportMethodList.DataSource = ImportMethodList.Retrieve;
            Utils.UltraComboFilter(this.cmbFileFormat, "Name");

            this.ctrlSourceName1.InitControl();
            this.ctrlSourceName1.Enabled = false;
            
            AddDataBindingsForComboBoxes();

        }
       
    }

}
