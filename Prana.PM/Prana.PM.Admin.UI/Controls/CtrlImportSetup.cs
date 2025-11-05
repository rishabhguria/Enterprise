using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
//using Prana.PM.Common;
using Prana.PM.Admin.UI.Forms;
using Prana.PM.BLL;
using Prana.PM.DAL;
using Prana.Utilities.UIUtilities;
using Prana.Utilities.MiscUtilities;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Prana.PM.Admin.UI.Controls
{
    public partial class CtrlImportSetup : UserControl
    {
        //BindingSource ImportSetupBindingSource = new BindingSource();

        BindingSource _formBindingSource = new BindingSource();

        private ImportSetup _importSetup = new ImportSetup();

        private Forms.MapColumns _frmMapColumns = null;
        private Forms.MapAUEC _frmMapAUEC = null;
        private Forms.SelectColumns _frmSetupColumns = null;
        //private Forms.MapFunds _frmMapFunds = null;
        private Forms.MapSymbol _frmMapSymbol = null;

	
        public CtrlImportSetup()
        {
            InitializeComponent();
        }
  

        /// <summary>
        /// Populates the data source details.
        /// </summary>
        /// <param name="dataSourceID">The data source ID.</param>
        public void PopulateImportSetUpDetails(Prana.BusinessObjects.PositionManagement.DataSourceNameID dataSourceID)
        {
            try
            {
                

                _importSetup = DataSourceManager.GetImportSetupForID(dataSourceID);
                _formBindingSource.DataSource = _importSetup;

                lblDataSourceName.DataBindings.Clear();
                lblDataSourceName.DataBindings.Add("Text", _formBindingSource, "DataSourceNameID");

                AddDataBindingsForComboBoxes();
                _importSetup.BeginEdit();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds the data bindings for combo boxes.
        /// </summary>
        private void AddDataBindingsForComboBoxes()
        {
            try
            {
                cmbImportMethod.DisplayMember = "DisplayText";
                cmbImportMethod.ValueMember = "Value";
                cmbImportMethod.DataSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ImportMethod));
                Utils.UltraComboFilter(cmbImportMethod, "DisplayText");
                cmbImportMethod.DataBindings.Clear();
                cmbImportMethod.DataBindings.Add("Value", _formBindingSource, "ImportMethod");

                cmbFileFormat.DisplayMember = "DisplayText";
                cmbFileFormat.ValueMember = "Value";
                cmbFileFormat.DataSource = EnumHelper.ConvertEnumForBindingWithAssignedValues(typeof(ImportFormat));

                Utils.UltraComboFilter(cmbFileFormat, "DisplayText");

                cmbFileFormat.DataBindings.Clear();
                cmbFileFormat.DataBindings.Add("Value", _formBindingSource, "ImportFormat");
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Global.ApplicationConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }

            /////////TODO : Irregular behaviour, not able to disable the value for cmbFileFormat as it is written first.
            //////List<EnumerationValue> importMethodList = EnumHelper.ConvertEnumForBinding(typeof(ImportMethod));
            //////importMethodList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
            //////cmbImportMethod.DataSource = importMethodList;
            //////Utils.UltraComboFilter(cmbImportMethod, "DisplayText");
            //////cmbImportMethod.Value = -1;

            //////List<EnumerationValue> importFormatList = EnumHelper.ConvertEnumForBinding(typeof(ImportFormat));
            //////importFormatList.Add(new EnumerationValue(Constants.C_COMBO_SELECT, -1));
            //////cmbFileFormat.DataSource = importFormatList;
            //////Utils.UltraComboFilter(this.cmbFileFormat, "DisplayText");
            //////cmbFileFormat.Value = -1;
        }

        private void btnSelectColumns_Click(object sender, EventArgs e)
        {

            try
            {
                _frmSetupColumns = new Forms.SelectColumns(_importSetup.DataSourceNameID);

                _frmSetupColumns.ShowDialog();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnMapColumns_Click(object sender, EventArgs e)
        {
            try
            {
                _frmMapColumns = new Forms.MapColumns(_importSetup.DataSourceNameID);
                _frmMapColumns.ShowDialog();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }


        private void btnMapAUEC_Click(object sender, EventArgs e)
        {

            try
            {
                _frmMapAUEC = new Forms.MapAUEC(_importSetup.DataSourceNameID);
                _frmMapAUEC.ShowDialog();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
           
        }


        private void btnMapSymbol_Click(object sender, EventArgs e)
        {
            try
            {

                _frmMapSymbol = new Forms.MapSymbol();
                _frmMapSymbol.ShowDialog();

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

     

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Close the Container Form
            this.FindForm().Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _importSetup.ApplyEdit();
                DataSourceManager.SaveImportSetup(_importSetup);
                _importSetup.BeginEdit();
                MessageBox.Show(this, "Import Setup information saved !", "Position Management", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, Prana.Global.ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _importSetup.CancelEdit();
        }
    }

}
