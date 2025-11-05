#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
#endregion Using

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void RMAUECChangedHandler(System.Object sender, AUECValueEventArgs e);

    public partial class RM_AUEC : UserControl
    {

        #region Wizard Stuff
        const string C_COMBO_SELECT = "-Select-";

        private int _rmCurrencyID = int.MinValue;
        private int _basecurrencyID = int.MinValue;
        private Int64 _toRMConversion = Int64.MinValue;
        private Int64 _toBaseConversion = Int64.MinValue;
        private Int64 _maxPNLinBase = Int64.MinValue;
        private Int64 _maxPNLinRM = Int64.MinValue;
        private int _companyID = int.MinValue;
        private int _companyAUECID = int.MinValue;
        private int _auecIDSelected = int.MinValue;

        public event RMAUECChangedHandler RMAUECChanged;
        //public int CompanyID
        //{
        //    set { _companyID = value; }
        //}

        public RM_AUEC()
        {
            InitializeComponent();
        }

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public int CompanyAUECID
        {
            set
            {
                _companyAUECID = value;
            }
        }

        public int RMBaseCurrencyID
        {
            set { _rmCurrencyID = value; }
        }

        #endregion Wizard Stuff

        #region BindAUECs

        /// <summary>
        /// This method Binds the AUECs permitted to a company in a drop down.
        /// </summary>
        /// <returns></returns>
        private void BindCompanyAUECs()
        {
            try
            {
                //If the companyID is valid...
                if (_companyID != int.MinValue)
                {
                    // It fetches all the AUECs for the selected companyID and assigns it to the collection instance "auecs".
                    AUECs auecs = RMAdminBusinessLogic.GetAllCompanyAUECs(_companyID);

                    if (auecs.Count > 0)
                    {
                        // Instantiate a datatable.
                        DataTable dtauec = new DataTable();

                        //Add columns to the datatable.
                        dtauec.Columns.Add("Data");
                        dtauec.Columns.Add("Value");

                        // Add an array of row to the datatable.
                        object[] row = new object[2];

                        // Set the data in the 1st row of the datatable.
                        row[0] = C_COMBO_SELECT;
                        row[1] = int.MinValue;

                        // Add the row to the datatable.
                        dtauec.Rows.Add(row);

                        // While iterating through the auecs collection, added each auec object info to datatable.
                        foreach (AUEC auec in auecs)
                        {
                            //SK 20061009 Compliance is removed from AUEC
                            // New object of currency class.
                            //Currency currency = new Currency();
                            // Assign to this object the data for a particular currencyID. 
                            //currency = RMAdminBusinessLogic.GetCurrency(auec.Compliance.BaseCurrencyID);
                            //

                            // the string is assigned the asset name, underlying name , exchange name and currency symbol in concatenated form.
                            string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencyName.ToString();
                            //string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();

                            // The auecID ia assigned to the integer "Value".
                            int Value = auec.AUECID;

                            // "Data" and "Value" are added to the row array.
                            row[0] = Data;
                            row[1] = Value;

                            // and the row is added to the datatable, each time a new array is assigned.
                            dtauec.Rows.Add(row);
                        }

                        this.cmbAUEC.ValueChanged -= new System.EventHandler(this.cmbAUEC_ValueChanged);
                        // Finally, a datasource in the form of datatable is assigned to ultracombobox.
                        cmbAUEC.DataSource = null;
                        cmbAUEC.DataSource = dtauec;
                        //Displaymember is set to "Data" in column in datatable.
                        cmbAUEC.DisplayMember = "Data";
                        //Value member is set to column "Value".
                        cmbAUEC.ValueMember = "Value";
                        //Initial Text of combo is set to "-select-".
                        cmbAUEC.Text = C_COMBO_SELECT;

                        this.cmbAUEC.ValueChanged += new System.EventHandler(this.cmbAUEC_ValueChanged);

                        // While iterating through each column in in the datasource assigned to the Combo..
                        // certain properties are set for certain columns.
                        ColumnsCollection columns = cmbAUEC.DisplayLayout.Bands[0].Columns;
                        foreach (UltraGridColumn column in columns)
                        {
                            // If the column is not "Data" Column , then it is set as hidden. 
                            if (column.Key != "Data")
                            {
                                column.Hidden = true;
                            }
                            else
                            {
                                // The column headers are set as invisible.
                                cmbAUEC.DisplayLayout.Bands[0].ColHeadersVisible = false;
                            }
                        }
                    }
                    else
                    {
                        // The user is informed that there are no permitted AUECs for the company and that
                        // the AUECs permissions are to be set for the company.
                        MessageBox.Show("Please set the permission for AUECs for the Company in SuperAdmin!");
                    }

                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion BindAUECs

        #region BindRMAUECGrid

        private void BindCompanyAUECGrid()
        {
            //Fetching the existing data from the database for all the CompanyAUECs for a particular companyID.
            RMAUECs rMAUECs = RMAdminBusinessLogic.GetRM_AUECs(_companyID);

            //Assigning rMAUECs as the datasource to the RMAUEC grid's ,if it contains valid values.
            if (rMAUECs.Count != 0)
            {
                foreach (RMAUEC rMAUEC in rMAUECs)
                {
                    int auecID = rMAUEC.AUECID;
                    AUEC auec = RMAdminBusinessLogic.GetAUECDetail(auecID);

                    //SK 20061009 Compliance is removed from AUEC                    
                    //Currency currency = new Currency();
                    //currency = RMAdminBusinessLogic.GetCurrency(auec.Compliance.BaseCurrencyID);
                    //

                    string Data = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.DisplayName.ToString() + "/" + auec.Currency.CurrencyName.ToString();
                    //string auecName = auec.Asset.Name.ToString() + "/" + auec.UnderLying.Name.ToString() + "/" + auec.Exchange.DisplayName.ToString() + "/" + currency.CurrencySymbol.ToString();

                    //AllCommissionRule allCommissonRule = new AllCommissionRule();
                    rMAUEC.AUEC = Data.ToString();
                    // rMAUECs.Add(rMAUEC);
                }
                grdRMAUEC.DataSource = rMAUECs;
            }
            else
            {
                // Assign an empty row to the grid.
                RMAUECs _rMAUECs = new RMAUECs();
                _rMAUECs.Add(new RMAUEC(int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue, int.MinValue));
                grdRMAUEC.DataSource = _rMAUECs;
                grdRMAUEC.DisplayLayout.Rows[0].Delete(false);
                cmbAUEC.Text = C_COMBO_SELECT;
                // and refresh the Form controls.
                RefreshRMAUEC();
            }
        }

        #endregion BindRMAUECGrid

        #region cmbAUEC_ValueChanged Event for: 1)Set BaseCurrency Labels 2)Passing the selected AUECID to tree.

        /// <summary>
        /// Since the base currency is the base currency of AUEC, 
        /// therefor , we use cmbAUEC_valuechanged event to display it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAUEC_ValueChanged(object sender, EventArgs e)
        {
            // For displayin the BaseCurrency per selected AUEC in labels.
            //============================================================
            // Assign the ID of the selected Auec in the drop down.
            int auecID = int.Parse(cmbAUEC.Value.ToString());
            if (auecID > 0)
            {
                // The str is assigned to the labels to display the basecurrency.
                //SK 20061009 REmoved Compliance Class
                String str = AUECManager.GetAUEC(auecID).Currency.CurrencySymbol.ToString();
                lblBaseCurrency.Text = str;
                lblBaseCurrency1.Text = str;
                //
            }
            else
            {
                // labels are set as empty.
                lblBaseCurrency.Text = "";
                lblBaseCurrency1.Text = "";
            }

            //=====================================================================


            if (auecID != _auecIDSelected)
            {
                if (auecID != int.MinValue)
                {
                    _auecIDSelected = int.Parse(cmbAUEC.Value.ToString());

                    AUECValueEventArgs aUECValueEventArgs = new AUECValueEventArgs();
                    aUECValueEventArgs.companyAUECID = this.cmbAUEC.Value.ToString();

                    if (RMAUECChanged != null)
                    {
                        RMAUECChanged(this, aUECValueEventArgs);
                    }

                    bool IsNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdRMAUEC.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (_auecIDSelected == Convert.ToInt32(_rwclc[i].Cells["AUECID"].Value.ToString()))
                        {
                            (grdRMAUEC.Rows)[i].Selected = true;
                            (grdRMAUEC.Rows)[i].Activate();

                            SetForm();

                            IsNew = false;
                            break;

                        }

                        grdRMAUEC.Rows[i].Selected = false;

                    }
                    if (IsNew)
                    {

                        RefreshRMAUEC();

                    }
                }
                else
                {
                    RefreshRMAUEC();
                    errorProvider1.SetError(txtExpLtBaseCurrency, "");
                    errorProvider1.SetError(txtExpLtRMBaseCurrency, "");
                    errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "");
                    errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "");
                }
            }

        }

        #endregion cmbAUEC_ValueChanged Event for: 1)Set BaseCurrency Labels 2)Passing the selected AUECID to tree.

        #region After Row select Event
        /// <summary>
        /// the event is raised to set the data as in selected row in the controls on the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdRMAUEC_AfterRowActivate(object sender, EventArgs e)
        {
            if (this.grdRMAUEC.Selected.Rows.Count > 0)
            {
                int auecID = Convert.ToInt16(grdRMAUEC.ActiveRow.Cells["AUECID"].Value.ToString());
                if (auecID != _auecIDSelected)
                {
                    _auecIDSelected = auecID;
                    cmbAUEC.Value = _auecIDSelected;
                    SetForm();

                    AUECValueEventArgs aUECValueEventArgs = new AUECValueEventArgs();
                    aUECValueEventArgs.companyAUECID = this.cmbAUEC.Value.ToString();

                    if (RMAUECChanged != null)
                    {
                        RMAUECChanged(this, aUECValueEventArgs);
                    }

                }
            }
        }

        #endregion After Row select Event

        #region Setting the Data of the selected row in controls
        /// <summary>
        /// the method is used to set the data per cell of the selected rows in the controls on the form.
        /// </summary>
        private void SetForm()
        {
            txtExpLtRMBaseCurrency.Text = grdRMAUEC.Selected.Rows[0].Cells["ExposureLimit_RMBaseCurrency"].Text;
            txtExpLtBaseCurrency.Text = grdRMAUEC.Selected.Rows[0].Cells["ExposureLimit_BaseCurrency"].Text;
            txtMaxPNLLossRMBaseCurrency.Text = grdRMAUEC.Selected.Rows[0].Cells["MaximumPNLLoss_RMBaseCurrency"].Text;
            txtMaxPNLLossBaseCurrency.Text = grdRMAUEC.Selected.Rows[0].Cells["MaximumPNLLoss_BaseCurrency"].Text;
            txtRMAUECID_Invisible.Text = grdRMAUEC.Selected.Rows[0].Cells["RMAUECID"].Text;
        }

        #endregion Setting the Data of the selected row in controls

        #region Passing RMBaseCurrency Symbol from CompanyOverallLimit Tabpage

        /// <summary>
        /// For displaying the selected RMBasecurrency symbol on CompanyOverallLimit Tab on the labels on this RM_AUEC Usercontrol.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            lblRMCurrency1.Text = e.rMCurrencySymbol;
            lblRMBaseCurrency2.Text = e.rMCurrencySymbol;
            _rmCurrencyID = e.rMCurrencyID;
        }



        #endregion Passing RMBaseCurrency Symbol from CompanyOverallLimit Tabpage

        #region ExposureLimit Conversion From RMBaseCurrency to BaseCurrency and Vice versa


        /// <summary>
        /// The event is raised to check the validity of the entered text for exposure limit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLtRMBaseCurrency_TextChanged(object sender, EventArgs e)
        {
            /*Basically ,four types of validation are performed:
             * 1. the entered value is numeric only.
             * 2. the value is not more than the default max value set for the Exp Lt field in DB.
             * 3. the value is valid relative to the parent exposure limit.
             * 4. and, the AUEC has been selected before entering the Exp Lt , to set the base currency for conversion of values.
             */

            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtExpLtRMBaseCurrency, "");
            if (txtExpLtRMBaseCurrency.Text != "")
            {

                //First, it is checked whether the AUEC has been selected before enterng the other data, in order to set the Base Currency. 
                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    //The textbox is set to blank, in case base currency(auec) has not been set.
                    txtExpLtRMBaseCurrency.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Exposure Limit !");
                    //The AUEc dropdown is set to focus.
                    cmbAUEC.Focus();
                }
                else
                {
                    //The entered data is checked for numeric datatype input using ValidateNumeric method.
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtExpLtRMBaseCurrency.Text.Trim());
                    if (!IsValid)
                    {
                        //txtExpLtRMBaseCurrency.Text = "";
                        errorProvider1.SetError(txtExpLtRMBaseCurrency, "Please enter only numeric values for Exposure Limit!");
                        txtExpLtRMBaseCurrency.Focus();
                        return;

                    }
                    else
                    {
                        // the input value should not be more than the default max value. 
                        Int64 chkvalue = Int64.MinValue;
                        Int64.TryParse(txtExpLtRMBaseCurrency.Text, out chkvalue);
                        if (txtExpLtRMBaseCurrency.Text != "0" && chkvalue == 0)
                        {
                            txtExpLtRMBaseCurrency.Text = "";
                            errorProvider1.SetError(txtExpLtRMBaseCurrency, "You cannot enter a value greater than 9223372036854775807!");
                            txtExpLtRMBaseCurrency.Focus();
                        }
                        else
                        {
                            Int64 maxPermittedExpLt = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, chkvalue);
                            if (maxPermittedExpLt > 0)
                            {
                                txtExpLtRMBaseCurrency.Text = "";
                                errorProvider1.SetError(txtExpLtRMBaseCurrency, "You cannot enter a value greater than Company Exposure Limit i.e." + maxPermittedExpLt + "!");
                                txtExpLtRMBaseCurrency.Focus();
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// The event is raised to populate the exposure limit in BaseCurrency textbox with equivalent expLt entered in RMBaseCurrency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLtRMBaseCurrency_Leave(object sender, EventArgs e)
        {
            txtExpLtRMBaseCurrency.Appearance.BackColor = Color.White;
            //_toBaseConversion is a global variable in this doc., so, if the txtExpLtRMBaseCurrency displayed value is already equal to it
            // then, we need not do nything else...
            if (txtExpLtRMBaseCurrency.Text != Convert.ToString(_toBaseConversion))
            {
                //The textbox should not be empty either.
                if (txtExpLtRMBaseCurrency.Text != "")
                {
                    if (_rmCurrencyID != _basecurrencyID)
                    {
                        //The textbox value is assigned to the integer after conversion.
                        _toBaseConversion = Convert.ToInt64(txtExpLtRMBaseCurrency.Text);
                        //The equivalent converted value in base currency is assigned to the new variable.
                        _toRMConversion = RMAdminBusinessLogic.ConvertExpLt(_rmCurrencyID, _basecurrencyID, _toBaseConversion);
                        if (_toRMConversion > 0)
                        {
                            // and displayed in other textbox.
                            txtExpLtBaseCurrency.Text = Convert.ToString(_toRMConversion);
                        }
                        else
                        {
                            MessageBox.Show(" Conversion rate for the set RMBaseCurrency And BaseCurrency is not available!");
                            txtExpLtBaseCurrency.Text = "";
                            txtExpLtRMBaseCurrency.Text = "";
                            txtMaxPNLLossBaseCurrency.Text = "";
                            txtMaxPNLLossRMBaseCurrency.Text = "";
                        }
                    }
                    else
                    {
                        _toBaseConversion = Convert.ToInt64(txtExpLtRMBaseCurrency.Text);
                        _toRMConversion = _toBaseConversion;
                        txtExpLtBaseCurrency.Text = Convert.ToString(_toRMConversion);

                    }
                }
            }

        }

        /// <summary>
        /// The event is raised to check the validity of the entered text for exposure limit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLtBaseCurrency_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtExpLtBaseCurrency, "");
            if (txtExpLtBaseCurrency.Text != "")
            {

                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    txtExpLtBaseCurrency.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Exposure Limit !");
                    cmbAUEC.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtExpLtBaseCurrency.Text.Trim());
                    if (!IsValid)
                    {
                        //txtExpLtBaseCurrency.Text = "";
                        errorProvider1.SetError(txtExpLtBaseCurrency, "Please enter only numeric values for Exposure Limit!");
                        txtExpLtBaseCurrency.Focus();
                        return;

                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtExpLtBaseCurrency.Text, out chkvalue);
                        if (txtExpLtBaseCurrency.Text != "0" && chkvalue == 0)
                        {
                            txtExpLtBaseCurrency.Text = "";
                            errorProvider1.SetError(txtExpLtBaseCurrency, "You cannot enter a value greater than 9223372036854775807!");
                            txtExpLtBaseCurrency.Focus();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// The event is raised to populate the exposure limit in RMBaseCurrency textbox with equivalent expLt entered in BaseCurrency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLtBaseCurrency_Leave(object sender, EventArgs e)
        {
            txtExpLtBaseCurrency.Appearance.BackColor = Color.White;
            errorProvider1.SetError(txtExpLtRMBaseCurrency, "");
            if (txtExpLtBaseCurrency.Text != Convert.ToString(_toRMConversion))
            {
                if (txtExpLtBaseCurrency.Text != "")
                {
                    if (_rmCurrencyID != _basecurrencyID)
                    {
                        _toRMConversion = int.Parse(txtExpLtBaseCurrency.Text);
                        _toBaseConversion = RMAdminBusinessLogic.ConvertExpLt(_basecurrencyID, _rmCurrencyID, _toRMConversion);

                        if (_toBaseConversion > 0)
                        {
                            //The converted value should also adhere to maximum permissible explt value.
                            //ValidatingExpLt(_toBaseConversion);
                            Int64 maxPermittedValue = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, _toBaseConversion);
                            if (maxPermittedValue > 0)
                            {
                                txtExpLtBaseCurrency.Text = "";
                                txtExpLtRMBaseCurrency.Text = "";
                                errorProvider1.SetError(txtExpLtRMBaseCurrency, "You cannot enter an Exposure Limit Greater than the Company Exposure Limit i.e." + maxPermittedValue + "!");
                                txtExpLtRMBaseCurrency.Focus();
                            }
                            else
                            {
                                txtExpLtRMBaseCurrency.Text = Convert.ToString(_toBaseConversion);
                            }
                        }
                        else
                        {
                            MessageBox.Show(" Conversion rate for the set RMBaseCurrency And BaseCurrency Is not available!");
                            txtExpLtBaseCurrency.Text = "";
                            txtExpLtRMBaseCurrency.Text = "";
                            txtMaxPNLLossBaseCurrency.Text = "";
                            txtMaxPNLLossRMBaseCurrency.Text = "";
                        }
                    }
                    else
                    {
                        _toRMConversion = int.Parse(txtExpLtBaseCurrency.Text);
                        _toBaseConversion = _toRMConversion;
                        //ValidatingExpLt(_toBaseConversion);
                        Int64 maxPermittedValue = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, _toBaseConversion);
                        if (maxPermittedValue > 0)
                        {
                            txtExpLtBaseCurrency.Text = "";
                            txtExpLtRMBaseCurrency.Text = "";
                            errorProvider1.SetError(txtExpLtRMBaseCurrency, "You cannot enter an Exposure Limit Greater than the Company Exposure Limit i.e." + maxPermittedValue + "!");
                            txtExpLtRMBaseCurrency.Focus();
                        }
                        else
                        {
                            txtExpLtRMBaseCurrency.Text = Convert.ToString(_toBaseConversion);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// The event is raised to check the validity of the entered text for Max PNL Loss.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPNLLossRMBaseCurrency_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "");
            if (txtMaxPNLLossRMBaseCurrency.Text != "")
            {

                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    txtMaxPNLLossRMBaseCurrency.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Maximum PNL Loss !");
                    cmbAUEC.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtMaxPNLLossRMBaseCurrency.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossRMBaseCurrency.Text = "";
                        errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "Please enter only numeric values for Maximum PNL Loss!");
                        txtMaxPNLLossRMBaseCurrency.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtMaxPNLLossRMBaseCurrency.Text, out chkvalue);
                        if (txtMaxPNLLossRMBaseCurrency.Text != "0" && chkvalue == 0)
                        {
                            txtMaxPNLLossRMBaseCurrency.Text = "";
                            errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "You cannot enter a value greater than 9223372036854775807!");
                            txtMaxPNLLossRMBaseCurrency.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The event is raised to check the validity of the entered text for Max PNL Loss.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPNLLossBaseCurrency_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "");
            if (txtMaxPNLLossBaseCurrency.Text != "")
            {

                if (cmbAUEC.Text == C_COMBO_SELECT)
                {
                    txtMaxPNLLossBaseCurrency.Text = "";
                    errorProvider1.SetError(cmbAUEC, "Please select an AUEC before entering the Maximum PNL Loss !");
                    cmbAUEC.Focus();
                }
                else
                {
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtMaxPNLLossBaseCurrency.Text.Trim());
                    if (!IsValid)
                    {
                        //txtMaxPNLLossBaseCurrency.Text = "";
                        errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "Please enter only numeric values for Maximum PNL Loss!");
                        txtMaxPNLLossBaseCurrency.Focus();
                        return;
                    }
                    else
                    {
                        Int64 chkvalue = 0;
                        Int64.TryParse(txtMaxPNLLossBaseCurrency.Text, out chkvalue);
                        if (txtMaxPNLLossBaseCurrency.Text != "0" && chkvalue == 0)
                        {
                            txtMaxPNLLossBaseCurrency.Text = "";
                            errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "You cannot enter a value greater than 9223372036854775807!");
                            txtMaxPNLLossBaseCurrency.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The event is raised to populate the Max PNL Loss in RMBaseCurrency textbox with equivalent expLt entered in BaseCurrency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPNLLossBaseCurrency_Leave(object sender, EventArgs e)
        {
            txtMaxPNLLossBaseCurrency.Appearance.BackColor = Color.White;
            if (txtMaxPNLLossBaseCurrency.Text != Convert.ToString(_maxPNLinBase))
            {
                if (txtMaxPNLLossBaseCurrency.Text != "")
                {
                    if (_rmCurrencyID != _basecurrencyID)
                    {
                        _maxPNLinBase = int.Parse(txtMaxPNLLossBaseCurrency.Text);
                        _maxPNLinRM = RMAdminBusinessLogic.ConvertExpLt(_basecurrencyID, _rmCurrencyID, _maxPNLinBase);
                        if (_maxPNLinRM > 0)
                        {
                            txtMaxPNLLossRMBaseCurrency.Text = Convert.ToString(_maxPNLinRM);
                        }
                        else
                        {
                            MessageBox.Show(" Conversion rate for the set RMBaseCurrency And BaseCurrency Is not available!");
                            txtExpLtBaseCurrency.Text = "";
                            txtExpLtRMBaseCurrency.Text = "";
                            txtMaxPNLLossBaseCurrency.Text = "";
                            txtMaxPNLLossRMBaseCurrency.Text = "";
                        }
                    }
                    else
                    {
                        _maxPNLinBase = int.Parse(txtMaxPNLLossBaseCurrency.Text);
                        _maxPNLinRM = _maxPNLinBase;
                        txtMaxPNLLossRMBaseCurrency.Text = Convert.ToString(_maxPNLinRM);

                    }

                }
            }
        }

        /// <summary>
        /// The event is raised to populate the Max PNL Loss in BaseCurrency textbox with equivalent expLt entered in RMBaseCurrency.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxPNLLossRMBaseCurrency_Leave(object sender, EventArgs e)
        {
            txtMaxPNLLossRMBaseCurrency.Appearance.BackColor = Color.White;
            if (txtMaxPNLLossRMBaseCurrency.Text != Convert.ToString(_maxPNLinRM))
            {
                if (txtMaxPNLLossRMBaseCurrency.Text != "")
                {
                    if (_basecurrencyID != _rmCurrencyID)
                    {
                        _maxPNLinRM = int.Parse(txtMaxPNLLossRMBaseCurrency.Text);
                        _maxPNLinBase = RMAdminBusinessLogic.ConvertExpLt(_rmCurrencyID, _basecurrencyID, _maxPNLinRM);
                        if (_maxPNLinBase > 0)
                        {
                            txtMaxPNLLossBaseCurrency.Text = Convert.ToString(_maxPNLinBase);
                        }
                        else
                        {
                            MessageBox.Show(" Conversion rate for the set RMBaseCurrency And BaseCurrency Is not available!");
                            txtExpLtBaseCurrency.Text = "";
                            txtExpLtRMBaseCurrency.Text = "";
                            txtMaxPNLLossBaseCurrency.Text = "";
                            txtMaxPNLLossRMBaseCurrency.Text = "";
                        }
                    }
                    else
                    {
                        _maxPNLinRM = int.Parse(txtMaxPNLLossRMBaseCurrency.Text);
                        _maxPNLinBase = _maxPNLinRM;
                        txtMaxPNLLossBaseCurrency.Text = Convert.ToString(_maxPNLinBase);


                    }

                }
            }
        }

        #endregion ExposureLimit Conversion From RMBaseCurrency to BaseCurrency and Vice versa

        #region Validation

        /// <summary>
        /// To check the validation for the controls used in the usercontrol RM_AUECs.
        /// </summary>
        /// <returns></returns>
        private bool Vaildation()
        {
            // A bool is set to false initially.
            bool validationSuccess = true;

            //Sets the error description for the used controls.
            errorProvider1.SetError(cmbAUEC, "");
            errorProvider1.SetError(txtExpLtRMBaseCurrency, "");
            errorProvider1.SetError(txtExpLtBaseCurrency, "");
            errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "");
            errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "");


            if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
            {
                cmbAUEC.Text = C_COMBO_SELECT;
                errorProvider1.SetError(cmbAUEC, "Please select AUEC!");
                validationSuccess = false;
                cmbAUEC.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExpLtRMBaseCurrency.Text.Trim()))
            {
                txtExpLtRMBaseCurrency.Text = "";
                errorProvider1.SetError(txtExpLtRMBaseCurrency, "Please enter numeric values for Exposure Limit!");
                validationSuccess = false;
                txtExpLtRMBaseCurrency.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExpLtBaseCurrency.Text.Trim()))
            {
                txtExpLtBaseCurrency.Text = "";
                errorProvider1.SetError(txtExpLtBaseCurrency, "Please enter numeric values for Exposure Limit!");
                validationSuccess = false;
                txtExpLtBaseCurrency.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtMaxPNLLossRMBaseCurrency.Text.Trim()))
            {
                txtMaxPNLLossRMBaseCurrency.Text = "";
                errorProvider1.SetError(txtMaxPNLLossRMBaseCurrency, "Please enter numeric values for Maximum PNL Loss!");
                validationSuccess = false;
                txtMaxPNLLossRMBaseCurrency.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtMaxPNLLossBaseCurrency.Text.Trim()))
            {
                txtMaxPNLLossBaseCurrency.Text = "";
                errorProvider1.SetError(txtMaxPNLLossBaseCurrency, "Please enter numeric values for Maximum PNL Loss!");
                validationSuccess = false;
                txtMaxPNLLossBaseCurrency.Focus();
            }

            return validationSuccess;


        }
        #endregion Validation

        #region Save Method

        /// <summary>
        /// This method saves the RM_AUEC details in the database.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int SaveRMAUEC(Prana.Admin.BLL.RMAUEC rMAUEC, int _companyID)
        {
            int result = int.MinValue;
            bool Isvalid = Vaildation();
            if (Isvalid)
            {
                // Data as input by user is assigned to the respective fields for saving to DB.
                rMAUEC.RMAUECID = Convert.ToInt32(txtRMAUECID_Invisible.Text);
                rMAUEC.AUECID = int.Parse(cmbAUEC.Value.ToString());
                rMAUEC.ExposureLimit_RMBaseCurrency = int.Parse(txtExpLtRMBaseCurrency.Text.Trim().ToString());
                rMAUEC.ExposureLimit_BaseCurrency = int.Parse(txtExpLtBaseCurrency.Text.Trim().ToString());
                rMAUEC.MaximumPNLLoss_RMBaseCurrency = int.Parse(txtMaxPNLLossRMBaseCurrency.Text.Trim().ToString());
                rMAUEC.MaximumPNLLoss_BaseCurrency = int.Parse(txtMaxPNLLossBaseCurrency.Text.Trim().ToString());

                // Save method is called from the BLL i.e RMAdminBusinessLogic which inturn calls it from DAL.
                int _rMAUECID = RMAdminBusinessLogic.SaveRMAUEC(rMAUEC, _companyID);

                if (_rMAUECID == -1)
                {
                    // existing data is updated.
                }
                else // New data is saved.
                {
                    //RefreshRMAUEC();
                }
                result = rMAUEC.AUECID;
                BindCompanyAUECGrid();
            }
            return result;
        }

        #endregion Save Method

        #region Refresh Method

        /// <summary>
        /// This function is used to refresh the controls and set them to default values.
        /// </summary>
        private void RefreshRMAUEC()
        {
            //cmbAUEC.Text = C_COMBO_SELECT;
            txtExpLtRMBaseCurrency.Text = "";
            txtExpLtBaseCurrency.Text = "";
            txtMaxPNLLossRMBaseCurrency.Text = "";
            txtMaxPNLLossBaseCurrency.Text = "";
            txtRMAUECID_Invisible.Text = "-1";
            if (int.Parse(cmbAUEC.Value.ToString()) == int.MinValue)
            {
                lblBaseCurrency.Text = "";
                lblBaseCurrency1.Text = "";
            }

        }
        #endregion Refresh Method

        #region Set Method

        /// <summary>
        /// RMAUEC property sets the RMAUEC form by displaying the data for the 
        /// selected companyAUEC in the controls on the RM_AUEC tab page.
        /// </summary>
        public RMAUEC SetRMAUEC
        {
            set { SettingRMAUEC(value); }
        }

        /// <summary>
        /// This method sets the data for a particular RM AUEC from the database in the controls.
        /// </summary>
        /// <param name="rMAUEC"></param>
        private void SettingRMAUEC(RMAUEC rMAUEC)
        {
            BindCompanyAUECs();
            BindCompanyAUECGrid();

            //Before setting the data, we check whether the Object is not null .
            if (rMAUEC != null && rMAUEC.AUECID != int.MinValue)
            {
                txtRMAUECID_Invisible.Text = rMAUEC.RMAUECID.ToString();
                cmbAUEC.Value = int.Parse(rMAUEC.AUECID.ToString());
                txtExpLtRMBaseCurrency.Text = rMAUEC.ExposureLimit_RMBaseCurrency.ToString();
                txtExpLtBaseCurrency.Text = rMAUEC.ExposureLimit_BaseCurrency.ToString();
                txtMaxPNLLossRMBaseCurrency.Text = rMAUEC.MaximumPNLLoss_RMBaseCurrency.ToString();
                txtMaxPNLLossBaseCurrency.Text = rMAUEC.MaximumPNLLoss_BaseCurrency.ToString();
            }
            else
            {
                if (_companyAUECID != int.MinValue)
                {
                    RefreshRMAUEC();
                    cmbAUEC.Value = _companyAUECID;
                }
                else
                {
                    RefreshRMAUEC();
                    cmbAUEC.Text = C_COMBO_SELECT;
                }
            }
        }

        #endregion Set Method

        #region Focus Property
        /// <summary>
        /// The events are raised to set the background color as per the focus status of the  selected control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAUEC_Enter(object sender, EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbAUEC_Leave(object sender, EventArgs e)
        {
            cmbAUEC.Appearance.BackColor = Color.White;
        }

        private void txtExpLtRMBaseCurrency_Enter(object sender, EventArgs e)
        {
            txtExpLtRMBaseCurrency.Appearance.BackColor = Color.FromArgb(255, 250, 205);

        }

        private void txtExpLtBaseCurrency_Enter(object sender, EventArgs e)
        {
            txtExpLtBaseCurrency.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtMaxPNLLossRMBaseCurrency_Enter(object sender, EventArgs e)
        {
            txtMaxPNLLossRMBaseCurrency.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtMaxPNLLossBaseCurrency_Enter(object sender, EventArgs e)
        {
            txtMaxPNLLossBaseCurrency.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        #endregion Focus Property



    }
    #region class AUECValueEventArgs

    /// <summary>
    /// This Class is used for the event to pass the UserId of the selected user in combo to other controls
    /// </summary>
    public class AUECValueEventArgs : System.EventArgs
    {
        private String str;

        public String companyAUECID
        {
            get
            {
                return (str);
            }
            set
            {
                str = value;
            }
        }

    }  // AUECValueEventArgs

    #endregion class AUECValueEventArgs
}
