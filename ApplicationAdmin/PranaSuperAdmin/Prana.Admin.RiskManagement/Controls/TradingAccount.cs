#region Using
using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;
#endregion Using

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void RMTradAccntChangedHandler(System.Object sender, TAValueEventArgs e);

    public partial class TradingAccount : UserControl
    {
        #region Wizard Stuff
        const string C_COMBO_SELECT = "- Select -";
        private const string FORM_NAME = "TRADING ACCOUNT : ";

        private int _companyID = int.MinValue;
        private int _companyTradingAccountID = int.MinValue;
        private int _companyUserID = int.MinValue;
        private int _tradingAccntID = int.MinValue;
        private int _tradingAccntSelectedID = int.MinValue;

        public event RMTradAccntChangedHandler RMTAChanged;

        public TradingAccount()
        {
            InitializeComponent();
        }

        public int CompanyID
        {
            set { _companyID = value; }
        }

        public int CompanyUserID
        {
            set { _companyUserID = value; }
        }

        public int CompanyTradingAccountID
        {
            set { _companyTradingAccountID = value; }
        }

        public int TradingAccountID
        {
            set { _tradingAccntID = value; }
        }

        #endregion Wizard Stuff

        #region Bind Trading Accounts 

        /// <summary>
        /// The method is used to populate the dropdown for Trading Accounts of a selected company.
        /// </summary>
        private void BindTradingAccnt()
        {
            try
            {
                //GetCompanyTradingAccnts method fetches the existing Trading Accounts from the database.
                TradingAccounts tradingAccnts = RMAdminBusinessLogic.GetCompanyTradingAccnts(_companyID);

                //Inserting the - Select - option in the Combo Box at the top.
                tradingAccnts.Insert(0, new Prana.Admin.BLL.TradingAccount(int.MinValue, C_COMBO_SELECT));

                this.cmbTradAccnt.ValueChanged -= new System.EventHandler(this.cmbTradAccnt_ValueChanged);
                //Assigning the datasource in the form of collection.
                this.cmbTradAccnt.DataSource = null;
                this.cmbTradAccnt.DataSource = tradingAccnts;
                //DisplayMember set to the column "TradingAccountName".
                this.cmbTradAccnt.DisplayMember = "TradingShortName";
                //Valuemember set to "TradingAccountsID" column.
                this.cmbTradAccnt.ValueMember = "TradingAccountsID";
                //this.cmbTradAccnt.Text = C_COMBO_SELECT;
                this.cmbTradAccnt.Value = int.MinValue;

                this.cmbTradAccnt.ValueChanged += new System.EventHandler(this.cmbTradAccnt_ValueChanged);

                ColumnsCollection columns = cmbTradAccnt.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    //Columns other than "TradingAccountName" are set as hidden.
                    if (column.Key != "TradingShortName")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        // The headers are set to invisible.
                        cmbTradAccnt.DisplayLayout.Bands[0].ColHeadersVisible = false;
                    }
                }
            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            #endregion Catch

        }

        #endregion BaseCurrency Combo Binding

        #region Validation

        /// <summary>
        /// To check the validation for the controls used in the usercontrol Trading Accounts.
        /// </summary>
        /// <returns></returns>
        private bool Vaildation()
        {
            // A bool is set to false initially.
            bool validationSuccess = true;

            //Sets the error description for the used controls.
            errorProvider1.SetError(cmbTradAccnt, "");
            //errorProvider1.SetError(cmbBaseCurrency, "");
            errorProvider1.SetError(txtExpLtTradAccnt, "");
            errorProvider1.SetError(txtPostivePNLLimit, "");
            errorProvider1.SetError(txtNegativePNLLimit, "");

            if (int.Parse(cmbTradAccnt.Value.ToString()) == int.MinValue)
            {
                cmbTradAccnt.Text = C_COMBO_SELECT;
                errorProvider1.SetError(cmbTradAccnt, "Please select Trading Account!");
                validationSuccess = false;
                cmbTradAccnt.Focus();
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExpLtTradAccnt.Text.Trim()))
            {
                txtExpLtTradAccnt.Text = "";
                errorProvider1.SetError(txtExpLtTradAccnt, "Please enter numeric values for Exposure Limit!");
                validationSuccess = false;
                txtExpLtTradAccnt.Focus();
            }
            else if (txtPostivePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtPostivePNLLimit.Text.Trim()))
                {
                    txtPostivePNLLimit.Text = "";
                    errorProvider1.SetError(txtPostivePNLLimit, "Please enter numeric values for + PNL Limit!");
                    validationSuccess = false;
                    txtPostivePNLLimit.Focus();
                }
            }
            else if (txtNegativePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtNegativePNLLimit.Text.Trim()))
                {
                    txtNegativePNLLimit.Text = "";
                    errorProvider1.SetError(txtNegativePNLLimit, "Please enter numeric values for - PNL Limit!");
                    validationSuccess = false;
                    txtNegativePNLLimit.Focus();
                }
            }

            return validationSuccess;


        }

        #endregion Validation

        #region Save Method

        /// <summary>
        /// This method saves the Trading Accounts details in the database.
        /// </summary>
        /// <param name="companyOverallLimit"></param>
        /// <returns>Returns 1 if saved successfully.</returns>
        public int SaveTradingAccnt(RMTradingAccount rMTradingAccount, int _companyID)
        {
            int result = int.MinValue;
            bool IsTradAccntDetailsValid = Vaildation();
            if (IsTradAccntDetailsValid)
            {

                // Data as input by user is assigned to the respective fields for saving to DB.
                rMTradingAccount.CompanyTradAccntRMID = Convert.ToInt32(txtRMTradingAccntID_Invisible.Text);
                rMTradingAccount.CompanyTradingAccountID = int.Parse(cmbTradAccnt.Value.ToString());
                rMTradingAccount.ExposureLimit = int.Parse(txtExpLtTradAccnt.Text.Trim().ToString());

                if (txtPostivePNLLimit.Text != "")
                {
                    rMTradingAccount.PositivePNL = int.Parse(txtPostivePNLLimit.Text.Trim().ToString());
                }

                rMTradingAccount.NegativePNL = int.Parse(txtNegativePNLLimit.Text.Trim().ToString());

                // Save method is called from the BLL i.e RMAdminBusinessLogic which inturn calls it from DAL.
                // and returns the Id of the row saved.
                int _companyTradAccntRMID = RMAdminBusinessLogic.SaveRMTradingAccnt(rMTradingAccount, _companyID);

                if (_companyTradAccntRMID == -1)
                {
                    //That means existing details have been updated to currently entered data.
                }
                else
                {
                    //Newly entered data is saved.

                    //RefreshTradingAccnt();
                }
                result = rMTradingAccount.CompanyTradingAccountID;
                BindTradAccntGrid();
            }
            else
            {
                //The details are not saved.
            }
            return result;
        }

        #endregion Save Method

        #region Refresh Method

        // Method to refresh and set the state of controls to default state.
        private void RefreshTradingAccnt()
        {
            //cmbTradAccnt.Text = C_COMBO_SELECT;
            //cmbBaseCurrency.Text = C_COMBO_SELECT;
            txtExpLtTradAccnt.Text = "";
            txtPostivePNLLimit.Text = "";
            txtNegativePNLLimit.Text = "";
            txtRMTradingAccntID_Invisible.Text = "-1";
        }

        #endregion Refresh Method

        #region Set Method

        public RMTradingAccount SetRMTradingAccount
        {
            set { SettingRMTradingAccount(value); }
        }

        private void SettingRMTradingAccount(RMTradingAccount rMTradingAccount)
        {
            BindTradingAccnt();
            BindTradAccntGrid();

            //Before setting the data, we check whether the Object is not null .
            if (_companyID != int.MinValue && rMTradingAccount.CompanyTradingAccountID != int.MinValue)
            {

                cmbTradAccnt.Value = int.Parse(rMTradingAccount.CompanyTradingAccountID.ToString());
                txtExpLtTradAccnt.Text = rMTradingAccount.ExposureLimit.ToString();
                if (rMTradingAccount.PositivePNL > 0)
                {
                    txtPostivePNLLimit.Text = rMTradingAccount.PositivePNL.ToString();
                }
                else
                {
                    txtPostivePNLLimit.Text = "";
                }
                txtNegativePNLLimit.Text = rMTradingAccount.NegativePNL.ToString();
                txtRMTradingAccntID_Invisible.Text = rMTradingAccount.CompanyTradAccntRMID.ToString();
            }
            else
            {
                if (_companyTradingAccountID != int.MinValue)
                {
                    RefreshTradingAccnt();
                    cmbTradAccnt.Value = _companyTradingAccountID;
                }
                else
                {
                    //cmbTradAccnt.Text = C_COMBO_SELECT;

                    RefreshTradingAccnt();
                }
            }
        }


        #endregion Set Method

        #region BindTradingAccntGrid

        private void BindTradAccntGrid()
        {
            //Fetching the existing data from the database for all the CompanyTradingAccounts for a particular companyID.
            RMTradingAccounts rMTradingAccounts = RMAdminBusinessLogic.GetRMTradingAccnts(_companyID);

            //Assigning rMTradingAccounts as the datasource to the RMTradingAccounts grid's ,if it contains valid values.
            if (rMTradingAccounts.Count != 0)
            {
                foreach (RMTradingAccount rMTradingAccount in rMTradingAccounts)
                {


                    Prana.Admin.BLL.TradingAccount tradingAccnt = new Prana.Admin.BLL.TradingAccount();
                    tradingAccnt = RMAdminBusinessLogic.GetCompanyTradingAccntDetail(_companyID, rMTradingAccount.CompanyTradingAccountID);
                    string tradAccntName = tradingAccnt.TradingShortName;
                    rMTradingAccount.TradingAccount = tradAccntName;
                    //rMTradingAccounts.Add(rMTradingAccount);

                }
                grdTradingAccnt.DataSource = rMTradingAccounts;
            }
            else
            {
                // Assign an empty row to the grid.
                rMTradingAccounts = new RMTradingAccounts();
                rMTradingAccounts.Add(new RMTradingAccount(int.MinValue, int.MinValue, int.MinValue, Int64.MinValue,
                    Int64.MinValue, Int64.MinValue, ""));
                grdTradingAccnt.DataSource = rMTradingAccounts;
                grdTradingAccnt.DisplayLayout.Rows[0].Delete(false);

                // and refresh the Form controls.
                RefreshTradingAccnt();
            }

        }

        #endregion BindTradingAccntGrid

        #region ComboValueChanged Event
        /// <summary>
        /// The event is raised to do following:
        /// 1) check whether the selected trading account already exist in DB.If yes, then , set the data in controls otherwise, refresh.
        /// 2) select the corresponding row in grid.
        /// 3) TODO: fire and eventhandler to select the node in tree.
        /// 4)TODO:Use a flag to ensure that combo-grid-tree do not fire circular events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTradAccnt_ValueChanged(object sender, EventArgs e)
        {

            int tradingAccntID = int.Parse(cmbTradAccnt.Value.ToString());
            if (tradingAccntID != int.MinValue)
            {
                //if (tradingAccntID != _tradingAccntSelectedID)
                //{
                if (tradingAccntID > 0)
                {
                    _tradingAccntSelectedID = int.Parse(cmbTradAccnt.Value.ToString());

                    TAValueEventArgs tAValueEventArgs = new TAValueEventArgs();
                    tAValueEventArgs.companyTradingAccntID = this.cmbTradAccnt.Value.ToString();

                    if (RMTAChanged != null)
                    {
                        RMTAChanged(this, tAValueEventArgs);
                    }

                    bool isNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = grdTradingAccnt.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (tradingAccntID == Convert.ToInt32(_rwclc[i].Cells["CompanyTradingAccountID"].Value.ToString()))
                        {
                            (grdTradingAccnt.Rows)[i].Selected = true;
                            (grdTradingAccnt.Rows)[i].Activate();
                            SetForm();

                            isNew = false;
                            break;
                        }
                        grdTradingAccnt.Rows[i].Selected = false;
                    }
                    if (isNew)
                    {
                        RefreshTradingAccnt();
                    }
                }
                //}
            }
            else
            {
                RefreshTradingAccnt();
                errorProvider1.SetError(txtExpLtTradAccnt, "");
                errorProvider1.SetError(txtNegativePNLLimit, "");
                errorProvider1.SetError(txtPostivePNLLimit, "");
            }
        }

        #endregion ComboValueChanged Event

        #region Set the data of selected row in controls
        /// <summary>
        /// the method is used to set the values of the selected row in grid in the form controls.
        /// </summary>
        private void SetForm()
        {
            txtExpLtTradAccnt.Text = grdTradingAccnt.Selected.Rows[0].Cells["ExposureLimit"].Text;
            txtPostivePNLLimit.Text = grdTradingAccnt.Selected.Rows[0].Cells["PositivePNL"].Text;
            txtNegativePNLLimit.Text = grdTradingAccnt.Selected.Rows[0].Cells["NegativePNL"].Text;
            txtRMTradingAccntID_Invisible.Text = grdTradingAccnt.Selected.Rows[0].Cells["CompanyTradAccntRMID"].Text;
        }

        #endregion Set the data of selected row in controls

        #region ExposureLimit Validity Check
        /// <summary>
        /// the event is raised to check the validity of the entered exposure limits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLtTradAccnt_TextChanged(object sender, EventArgs e)
        {
            /*Basically ,five types of validation are performed:
            * 1. the entered value is numeric only.
            * 2. the value is not more than the default max value set for the Exp Lt field in DB.
            * 3. the value is valid relative to the parent exposure limit.
            * 4. the TradingAccount has been selected before entering the Exp Lt , to set the base currency for conversion of values.
            * 5. and, the value should not exceed the explt of the users of the tradingAccnt.
            */
            errorProvider1.SetError(cmbTradAccnt, "");
            errorProvider1.SetError(txtExpLtTradAccnt, "");
            if (txtExpLtTradAccnt.Text != "")
            {

                //First, it is checked whether the TradingAccount has been selected before enterng the other data. 
                if (cmbTradAccnt.Text == C_COMBO_SELECT)
                {
                    //The textbox is set to blank, in case tradingAccount has not been set.
                    txtExpLtTradAccnt.Text = "";
                    errorProvider1.SetError(cmbTradAccnt, "Please select a TradingAccount before entering the Exposure Limit !");
                    //The TradingAccount dropdown is set to focus.
                    cmbTradAccnt.Focus();
                }
                else
                {
                    //The entered data is checked for numeric datatype input using ValidateNumeric method.
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtExpLtTradAccnt.Text.Trim());
                    if (!IsValid)
                    {
                        //txtExpLtRMBaseCurrency.Text = "";
                        errorProvider1.SetError(txtExpLtTradAccnt, "Please enter only numeric values for Exposure Limit!");
                        txtExpLtTradAccnt.Focus();
                    }
                    else
                    {
                        // the input value should not be more than the default max value. 
                        Int64 chkvalue = Int64.MinValue;
                        Int64.TryParse(txtExpLtTradAccnt.Text, out chkvalue);
                        if (txtExpLtTradAccnt.Text != "0" && chkvalue == 0)
                        {
                            txtExpLtTradAccnt.Text = "";
                            errorProvider1.SetError(txtExpLtTradAccnt, "You cannot enter a value greater than 9223372036854775807!");
                            txtExpLtTradAccnt.Focus();
                        }
                        else
                        {
                            Int64 maxPermittedExpLt = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, chkvalue);
                            if (maxPermittedExpLt > 0)
                            {
                                txtExpLtTradAccnt.Text = "";
                                errorProvider1.SetError(txtExpLtTradAccnt, "You cannot enter a value greater than Company Exposure Limit i.e. " + maxPermittedExpLt + "!");
                                txtExpLtTradAccnt.Focus();
                            }
                            //Finally, the exp limit entered should be valid relative to companyExpLt.
                            //ValidatingExpLtasperParent(chkvalue);
                            //ValiadtionAspertradngAccntUsers(chkvalue);
                        }
                    }
                }
            }
        }

        ///// <summary>
        ///// The method is used to validate the tradingAccount Exposure limit As per the ExpLt of its users.
        ///// </summary>
        ///// <param name="chkvalue"></param>
        //private void ValiadtionAspertradngAccntUsers(Int64 chkvalue)
        //{
        //  Int64 sumofTradAccntUsers = RMAdminBusinessLogic.ValidTradAccntExpLtasperUsers(_companyID, _tradingAccntID);
        //  if (sumofTradAccntUsers > chkvalue)
        //  { 
        //     if(MessageBox.Show(this," The Exposure Limit of the TradingAccount is less than the total Exposure limit of its users. Do you still want to continue?","RM TradingAccount Alert !",MessageBoxButtons.YesNo) == DialogResult.Yes)
        //     {
        //       MessageBox.Show(this," Then,you will have to change the settings for User's Trading Account ExposureLimit too as per the TradingAccount Exposure Limit! So , until we add a validation form, please dont set change it!");
        //       //Validation form for user level pops up.
        //     }
        //  }
        //}

        ///// <summary>
        ///// the method is used to validate the exposure limit of the tradingAccount as per the CompanyExposurelimit.
        ///// </summary>
        ///// <param name="enterdValue"></param>
        //private void ValidatingExpLtasperParent(long enterdValue)
        //{
        //    //The available max value for exp limit is obtained from the method called from BLL.
        //    Int64 result = RMAdminBusinessLogic.ValidTradAccntExpLtasperCompany(_companyID);
        //    if (enterdValue <= result)
        //    {
        //        //Valid exp lt.
        //    }
        //    else
        //    {
        //        //Incase , the max exp limit has already been achieved.
        //        if (result == 0)
        //        {
        //            //The txt bxs for Exp Lt are set to blank.
        //            txtExpLtTradAccnt.Text = "";
        //            MessageBox.Show("You have already achieved the maximum exposure Limit available in previous Trading Account setting!");
        //            //The AUEC drop down is also set to default .
        //            cmbTradAccnt.Text = C_COMBO_SELECT;
        //        }
        //        else // If the entered value exceeds  the permissible value.
        //        {

        //            txtExpLtTradAccnt.Text = "";
        //            errorProvider1.SetError(txtExpLtTradAccnt, "The maximum Exposure Limit that you can enter is " + result + "!");
        //            txtExpLtTradAccnt.Focus();
        //        }
        //    }
        //}



        #endregion ExposureLimit Validity Check

        #region After Row is set active

        /// <summary>
        /// The event is raised to set the data in the form as per the selected active row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdTradingAccnt_AfterRowActivate(object sender, EventArgs e)
        {
            if (this.grdTradingAccnt.Selected.Rows.Count > 0)
            {
                int tradingAccountID = Convert.ToInt16(grdTradingAccnt.ActiveRow.Cells["CompanyTradingAccountID"].Value.ToString());
                if (tradingAccountID != _tradingAccntSelectedID)
                {
                    _tradingAccntSelectedID = tradingAccountID;
                    cmbTradAccnt.Value = _tradingAccntSelectedID;
                    //SetForm();

                    //TAValueEventArgs tAValueEventArgs = new TAValueEventArgs();
                    //tAValueEventArgs.companyTradingAccntID = this.cmbTradAccnt.Value.ToString();

                    //if (RMTAChanged != null)
                    //{
                    //    RMTAChanged(this, tAValueEventArgs);
                    //}

                }
            }
        }

        #endregion After Row is set active

        #region RMBaseCurrency Labels

        /// <summary>
        /// For displaying the selected RMBasecurrency symbol on CompanyOverallLimit Tab on the labels on this RM_AUEC Usercontrol.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            lblRMBCurr1.Text = e.rMCurrencySymbol;
            lblRMBCurr2.Text = e.rMCurrencySymbol;
            lblRMBCurr3.Text = e.rMCurrencySymbol;
        }
        #endregion RMBaseCurrency Labels

        #region Focus Property

        /// <summary>
        /// The events are raised to set the background color of the controls on focus enter and leave.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbTradAccnt_Leave(object sender, EventArgs e)
        {
            cmbTradAccnt.Appearance.BackColor = Color.White;
        }

        private void cmbTradAccnt_Enter(object sender, EventArgs e)
        {
            cmbTradAccnt.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtExpLtTradAccnt_Enter(object sender, EventArgs e)
        {
            txtExpLtTradAccnt.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtExpLtTradAccnt_Leave(object sender, EventArgs e)
        {
            txtExpLtTradAccnt.Appearance.BackColor = Color.White;
        }

        private void txtPostivePNLLimit_Enter(object sender, EventArgs e)
        {
            txtPostivePNLLimit.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtPostivePNLLimit_Leave(object sender, EventArgs e)
        {
            txtPostivePNLLimit.Appearance.BackColor = Color.White;
        }

        private void txtNegativePNLLimit_Enter(object sender, EventArgs e)
        {
            txtNegativePNLLimit.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtNegativePNLLimit_Leave(object sender, EventArgs e)
        {
            txtNegativePNLLimit.Appearance.BackColor = Color.White;
        }
        #endregion Focus Property

        #region Valid Data Entry
        private void txtPostivePNLLimit_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPostivePNLLimit, "");
            if (txtPostivePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtPostivePNLLimit.Text))
                {
                    errorProvider1.SetError(txtPostivePNLLimit, "Please enter only numeric values");
                }
                else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtPostivePNLLimit.Text))
                {
                    errorProvider1.SetError(txtPostivePNLLimit, "You cannot enter a value greater than 9223372036854775807 !");
                }
            }
        }

        private void txtNegativePNLLimit_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtNegativePNLLimit, "");
            if (txtNegativePNLLimit.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtNegativePNLLimit.Text))
                {
                    errorProvider1.SetError(txtNegativePNLLimit, "Please enter only numeric values");
                }
                else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtNegativePNLLimit.Text))
                {
                    errorProvider1.SetError(txtNegativePNLLimit, "You cannot enter a value greater than 9223372036854775807 !");
                }
            }
        }
        #endregion Valid Data Entry

        #region DataEntry Check
        /// <summary>
        /// The method is used to check the entry of data in controls.
        /// </summary>
        /// <returns></returns>
        public bool CheckForDataEntered()
        {
            bool IsDataEntered = false;

            if (int.Parse(cmbTradAccnt.Value.ToString()) != int.MinValue)
            {
                IsDataEntered = true;
            }
            else if (txtExpLtTradAccnt.Text != "")
            {
                IsDataEntered = true;
            }
            else if (txtNegativePNLLimit.Text != "")
            {
                IsDataEntered = true;
            }
            else if (txtPostivePNLLimit.Text != "")
            {
                IsDataEntered = true;
            }

            return IsDataEntered;
        }
        #endregion DataEntry Check
    }

    #region class TAValueEventArgs

    /// <summary>
    /// This Class is used for the event to pass the TradAccntId of the selected tradingAccnt in combo to other controls
    /// </summary>
    public class TAValueEventArgs : System.EventArgs
    {
        private String str;

        public String companyTradingAccntID
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

    }  // TAValueEventArgs

    #endregion class TAValueEventArgs
}
