using Infragistics.Win.UltraWinGrid;
using Prana.Admin.BLL;
using Prana.LogManager;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Prana.Admin.RiskManagement.Controls
{
    public delegate void RMAccountAccntChangedHandler(System.Object sender, FAValueEventArgs e);

    public partial class FundAccount : UserControl
    {
        #region Wizard Stuff
        private string C_COMBO_SELECT = "-Select-";
        private int _companyID = int.MinValue;
        private int _companyAccountAccnt = int.MinValue;
        private int _accountAccntSelectedID = int.MinValue;
        public FundAccount()
        {
            InitializeComponent();
        }

        public int CompanyID
        {
            set { _companyID = (value); }
        }

        public int CompanyFundAccountID
        {
            set { _companyAccountAccnt = value; }
        }

        public event RMAccountAccntChangedHandler RMFAChanged;

        #endregion Wizard Stuff

        #region BindFundAccount

        /// <summary>
        /// The method is used to populate the dropdown with all the permitted accountaccnts of a company.
        /// </summary>
        private void BindAccountAccnt()
        {
            try
            {

                //GetCompanyAccountAccnts method fetches the existing Account Accounts from the database.
                Accounts accounts = RMAdminBusinessLogic.GetCompanyaccounts(_companyID);

                //Inserting the - Select - option in the Combo Box at the top.
                accounts.Insert(0, new Prana.Admin.BLL.Account(int.MinValue, C_COMBO_SELECT));

                this.cmbFundAccount.ValueChanged -= new System.EventHandler(this.cmbFundAccount_ValueChanged);
                //Assigning the datasource in the form of collection.
                this.cmbFundAccount.DataSource = null;
                this.cmbFundAccount.DataSource = accounts;
                //DisplayMember set to the column "TradingAccountName".
                this.cmbFundAccount.DisplayMember = "AccountShortName";
                //Valuemember set to "TradingAccountsID" column.
                this.cmbFundAccount.ValueMember = "AccountID";
                this.cmbFundAccount.Text = C_COMBO_SELECT;

                this.cmbFundAccount.ValueChanged += new System.EventHandler(this.cmbFundAccount_ValueChanged);

                ColumnsCollection columns = cmbFundAccount.DisplayLayout.Bands[0].Columns;
                foreach (UltraGridColumn column in columns)
                {
                    //Columns other than "FundAccountShortName" are set as hidden.
                    if (column.Key != "AccountShortName")
                    {
                        column.Hidden = true;
                    }
                    else
                    {
                        // The headers are set to invisible.
                        cmbFundAccount.DisplayLayout.Bands[0].ColHeadersVisible = false;
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


        #endregion BindFundAccount

        #region BindAccountAccntGrid
        /// <summary>
        /// the method is used to fill the grid with the Accountaccnts data for a particular company.
        /// </summary>
        private void BindAccountAccntGrid()
        {
            //Fetching the existing data from the database for all the CompanyFundAccounts for a particular companyID.
            RMFundAccounts rMFundAccounts = RMAdminBusinessLogic.GetRMFundAccounts(_companyID);

            //Assigning rMFundAccounts as the datasource to the RMFundAccounts grid's ,if it contains valid values.
            if (rMFundAccounts.Count != 0)
            {
                foreach (RMFundAccount rMFundAccount in rMFundAccounts)
                {
                    Prana.Admin.BLL.Account account = RMAdminBusinessLogic.GetCompanyAccount(_companyID);
                    string accountName = account.AccountShortName;
                    rMFundAccount.FundAccount = accountName;
                }
                gridAccountAccnt.DataSource = rMFundAccounts;
            }
            else
            {
                // Assign an empty row to the grid.
                rMFundAccounts = new RMFundAccounts();
                rMFundAccounts.Add(new RMFundAccount(int.MinValue, Int64.MinValue, int.MinValue, Int64.MinValue, Int64.MinValue, int.MinValue));
                gridAccountAccnt.DataSource = rMFundAccounts;
                gridAccountAccnt.DisplayLayout.Rows[0].Delete(false);

                // and refresh the Form controls.
                RefreshAccountAccnt();
            }

        }


        #endregion BindTradingAccntGrid

        #region Refresh
        /// <summary>
        /// the method is used to refresh the form controls
        /// </summary>
        private void RefreshAccountAccnt()
        {
            txtExpLt.Text = "";
            txtNegPNLLoss.Text = "";
            txtPosPNLLoss.Text = "";
            txtRMAccountAccntID_Invisible.Text = "-1";
        }
        #endregion Refresh

        #region Validation

        /// <summary>
        /// The method is used to validate the data entered by the user.
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            bool validationSuccess = true;

            //Sets the error description for the used controls.
            errorProvider1.SetError(cmbFundAccount, "");
            errorProvider1.SetError(txtExpLt, "");


            if (int.Parse(cmbFundAccount.Value.ToString()) == int.MinValue)
            {
                cmbFundAccount.Text = C_COMBO_SELECT;
                errorProvider1.SetError(cmbFundAccount, "Please select Account Account!");
                validationSuccess = false;
                cmbFundAccount.Focus();
                return validationSuccess;
            }
            else if (!DataTypeValidation.ValidateNumeric(txtExpLt.Text.Trim()))
            {
                txtExpLt.Text = "";
                errorProvider1.SetError(txtExpLt, "Please enter numeric values for Exposure Limit!");
                validationSuccess = false;
                txtExpLt.Focus();
                return validationSuccess;
            }
            else if (txtPosPNLLoss.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtPosPNLLoss.Text.Trim()))
                {
                    txtPosPNLLoss.Text = "";
                    errorProvider1.SetError(txtPosPNLLoss, "Please enter numeric values for + PNL Loss!");

                    txtPosPNLLoss.Focus();
                    return validationSuccess;
                }
            }
            else if (txtNegPNLLoss.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtNegPNLLoss.Text.Trim()))
                {
                    txtNegPNLLoss.Text = "";
                    errorProvider1.SetError(txtNegPNLLoss, "Please enter numeric values for - PNL Loss!");

                    txtNegPNLLoss.Focus();
                    return validationSuccess;
                }

            }

            return validationSuccess;

        }
        #endregion Validation

        #region Save Method

        /// <summary>
        /// The method is used to save the details entered by the user for a Company AccountAccnt.
        /// </summary>
        /// <param name="rMFundAccount"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public int SaveFundAccount(RMFundAccount rMFundAccount, int companyID)
        {
            errorProvider1.SetError(txtPosPNLLoss, "");
            errorProvider1.SetError(txtNegPNLLoss, "");
            int result = int.MinValue;
            bool IsAccountAccntDetailsValid = Validation();
            if (IsAccountAccntDetailsValid)
            {
                // Data as input by user is assigned to the respective fields for saving to DB.
                rMFundAccount.CompanyFundAccntRMID = Convert.ToInt32(txtRMAccountAccntID_Invisible.Text);
                rMFundAccount.CompanyFundAccntID = int.Parse(cmbFundAccount.Value.ToString());
                //rMTradingAccount.TABaseCurrencyID = int.Parse(cmbBaseCurrency.Value.ToString());
                rMFundAccount.ExposureLimit_RMBaseCurrency = int.Parse(txtExpLt.Text.Trim().ToString());
                if (txtPosPNLLoss.Text != "")
                {
                    rMFundAccount.Positive_PNL_Loss = int.Parse(txtPosPNLLoss.Text.Trim().ToString());

                }
                if (txtNegPNLLoss.Text != "")
                {
                    rMFundAccount.Negative_PNL_Loss = int.Parse(txtNegPNLLoss.Text.Trim().ToString());
                }

                // Save method is called from the BLL i.e RMAdminBusinessLogic which inturn calls it from DAL.
                // and returns the Id of the row saved.
                int _companyAccountAccntRMID = RMAdminBusinessLogic.SaveRMFundAccnt(rMFundAccount, _companyID);

                if (_companyAccountAccntRMID == -1)
                {
                    //That means existing details have been updated to currently entered data.
                }
                else
                {
                    //Newly entered data is saved.

                    //RefreshAccountAccnt();
                }
                result = rMFundAccount.CompanyFundAccntID;
                BindAccountAccntGrid();
            }
            else
            {
                //The details are not saved.
            }
            return result;
        }

        #endregion Save Method

        #region cmbFundAccount_ValueChanged

        /// <summary>
        /// The event is raised to reflect the change in selected value in drop down on the grid level as well in tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbFundAccount_ValueChanged(object sender, EventArgs e)
        {
            int accountAccntID = int.Parse(cmbFundAccount.Value.ToString());
            if (accountAccntID != _accountAccntSelectedID)
            {
                if (accountAccntID > 0)
                {
                    _accountAccntSelectedID = int.Parse(cmbFundAccount.Value.ToString());

                    FAValueEventArgs fAValueEventArgs = new FAValueEventArgs();
                    fAValueEventArgs.companyFundAccntID = this.cmbFundAccount.Value.ToString();

                    if (RMFAChanged != null)
                    {
                        RMFAChanged(this, fAValueEventArgs);
                    }

                    bool isNew = true;
                    Infragistics.Win.UltraWinGrid.RowsCollection _rwclc = gridAccountAccnt.Rows;
                    for (int i = 0; i < _rwclc.Count; i++)
                    {
                        if (accountAccntID == Convert.ToInt32(_rwclc[i].Cells["CompanyFundAccntID"].Value.ToString()))
                        {
                            (gridAccountAccnt.Rows)[i].Selected = true;
                            (gridAccountAccnt.Rows)[i].Activate();
                            SetForm();

                            isNew = false;
                            break;
                        }
                        gridAccountAccnt.Rows[i].Selected = false;
                    }
                    if (isNew)
                    {
                        RefreshAccountAccnt();
                    }
                }
            }
        }


        #endregion cmbFundAccount_ValueChanged

        #region Set Row data in forms

        /// <summary>
        /// the method is used to set the data from the selected row in the form controls.
        /// </summary>
        private void SetForm()
        {
            txtExpLt.Text = gridAccountAccnt.Selected.Rows[0].Cells["ExposureLimit_RMBaseCurrency"].Text;
            txtPosPNLLoss.Text = gridAccountAccnt.Selected.Rows[0].Cells["Positive_PNL_Loss"].Text;
            txtNegPNLLoss.Text = gridAccountAccnt.Selected.Rows[0].Cells["Negative_PNL_Loss"].Text;
            txtRMAccountAccntID_Invisible.Text = gridAccountAccnt.Selected.Rows[0].Cells["CompanyFundAccntRMID"].Text;
        }
        #endregion Set Row data in forms

        #region Set Method

        public RMFundAccount SetRMFundAccount
        {
            set { SettingRMFundAccount(value); }
        }

        private void SettingRMFundAccount(RMFundAccount rMFundAccount)
        {
            BindAccountAccntGrid();
            BindAccountAccnt();

            //Before setting the data, we check whether the Object is not null .
            if (_companyID != int.MinValue && rMFundAccount.CompanyFundAccntID != int.MinValue)
            {
                txtRMAccountAccntID_Invisible.Text = rMFundAccount.CompanyFundAccntRMID.ToString();
                cmbFundAccount.Value = int.Parse(rMFundAccount.CompanyFundAccntID.ToString());
                txtExpLt.Text = rMFundAccount.ExposureLimit_RMBaseCurrency.ToString();
                txtPosPNLLoss.Text = rMFundAccount.Positive_PNL_Loss.ToString();
                txtNegPNLLoss.Text = rMFundAccount.Negative_PNL_Loss.ToString();
            }
            else
            {
                if (_companyAccountAccnt != int.MinValue)
                {
                    cmbFundAccount.Value = _companyAccountAccnt;
                    RefreshAccountAccnt();
                }
                else
                {
                    cmbFundAccount.Text = C_COMBO_SELECT;
                    RefreshAccountAccnt();
                }
            }
        }

        #endregion Set Method

        #region ValidateExpLt

        /// <summary>
        /// The event is raised to check that the data enetered in exosure limit text box is valid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtExpLt_TextChanged(object sender, EventArgs e)
        {
            /*Basically ,five types of validation are performed:
            * 1. the entered value is numeric only.
            * 2. the value is not more than the default max value set for the Exp Lt field in DB.
            * 3. the value is valid relative to the parent exposure limit.
            */

            if (txtExpLt.Text != "")
            {
                errorProvider1.SetError(cmbFundAccount, "");
                errorProvider1.SetError(txtExpLt, "");
                //First, it is checked whether the FundAccount has been selected before enterng the other data. 
                if (cmbFundAccount.Text == C_COMBO_SELECT)
                {
                    //The textbox is set to blank, in case FundAccount has not been set.
                    txtExpLt.Text = "";
                    errorProvider1.SetError(cmbFundAccount, "Please select a FundAccount before entering the Exposure Limit !");
                    //The TradingAccount dropdown is set to focus.
                    cmbFundAccount.Focus();
                }
                else
                {
                    //The entered data is checked for numeric datatype input using ValidateNumeric method.
                    bool IsValid = DataTypeValidation.ValidateNumeric(txtExpLt.Text.Trim());
                    if (!IsValid)
                    {
                        //txtExpLtRMBaseCurrency.Text = "";
                        errorProvider1.SetError(txtExpLt, "Please enter only numeric values for Exposure Limit!");
                        txtExpLt.Focus();
                    }
                    else
                    {
                        // the input value should not be more than the default max value. 
                        Int64 chkvalue = Int64.MinValue;
                        Int64.TryParse(txtExpLt.Text, out chkvalue);
                        if (txtExpLt.Text != "0" && chkvalue == 0)
                        {
                            txtExpLt.Text = "";
                            errorProvider1.SetError(txtExpLt, "You cannot enter a value greater than 9223372036854775807!");
                            txtExpLt.Focus();
                        }
                        else
                        {
                            Int64 maxPermittedExpLt = RMAdminBusinessLogic.ValidRMAUECExpLt(_companyID, chkvalue);
                            if (maxPermittedExpLt > 0)
                            {
                                txtExpLt.Text = "";
                                errorProvider1.SetError(txtExpLt, "You cannot enter a value greater than Company Exposure Limit" + maxPermittedExpLt + "!");
                                txtExpLt.Focus();
                            }

                        }
                    }
                }
            }
        }

        #endregion ValidateExpLt

        #region After Row Activate

        /// <summary>
        /// the event is raised after a row is set to active row , to display the data of selected row in controls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridAccountAccnt_AfterRowActivate(object sender, EventArgs e)
        {
            if (this.gridAccountAccnt.Selected.Rows.Count > 0)
            {
                int fundAccountID = Convert.ToInt16(gridAccountAccnt.ActiveRow.Cells["CompanyFundAccntID"].Value.ToString());
                if (fundAccountID != _accountAccntSelectedID)
                {
                    _accountAccntSelectedID = fundAccountID;
                    cmbFundAccount.Value = _accountAccntSelectedID;
                    SetForm();

                    FAValueEventArgs fAValueEventArgs = new FAValueEventArgs();
                    fAValueEventArgs.companyFundAccntID = this.cmbFundAccount.Value.ToString();

                    if (RMFAChanged != null)
                    {
                        RMFAChanged(this, fAValueEventArgs);
                    }

                }
            }
        }

        #endregion After Row Select

        #region Focus Property

        private void cmbFundAccount_Enter(object sender, EventArgs e)
        {
            cmbFundAccount.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void cmbFundAccount_Leave(object sender, EventArgs e)
        {
            cmbFundAccount.Appearance.BackColor = Color.White;
        }

        private void txtExpLt_Enter(object sender, EventArgs e)
        {
            txtExpLt.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtExpLt_Leave(object sender, EventArgs e)
        {
            txtExpLt.Appearance.BackColor = Color.White;
        }

        private void txtPosPNLLoss_Enter(object sender, EventArgs e)
        {
            txtPosPNLLoss.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtPosPNLLoss_Leave(object sender, EventArgs e)
        {
            txtPosPNLLoss.Appearance.BackColor = Color.White;
        }

        private void txtNegPNLLoss_Enter(object sender, EventArgs e)
        {
            txtNegPNLLoss.Appearance.BackColor = Color.FromArgb(255, 250, 205);
        }

        private void txtNegPNLLoss_Leave(object sender, EventArgs e)
        {
            txtNegPNLLoss.Appearance.BackColor = Color.White;
        }

        #endregion Focus Property

        #region RMBaseCurrency Labels
        /// <summary>
        /// the method is used to display the RM base currency on Account Account level as selected on CompanyOverallLimits level.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateCurrencyText(System.Object sender, PassValueEventArgs e)
        {
            lblRMCurr.Text = e.rMCurrencySymbol;
            lblRMCurr1.Text = e.rMCurrencySymbol;
            lblRMCurr2.Text = e.rMCurrencySymbol;

            lblRMCurr.AutoSize = true;
            lblRMCurr1.AutoSize = true;
            lblRMCurr2.AutoSize = true;
        }
        #endregion RMBaseCurrency Labels

        #region textChanged event

        private void txtPosPNLLoss_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPosPNLLoss, "");
            if (txtPosPNLLoss.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtPosPNLLoss.Text))
                {
                    errorProvider1.SetError(txtPosPNLLoss, "Please enter only numeric values");
                }
                else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtPosPNLLoss.Text))
                {
                    errorProvider1.SetError(txtPosPNLLoss, "You cannot enter a value greater than 9223372036854775807 !");
                }
            }
        }

        private void txtNegPNLLoss_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtNegPNLLoss, "");
            if (txtNegPNLLoss.Text != "")
            {
                if (!DataTypeValidation.ValidateNumeric(txtNegPNLLoss.Text))
                {
                    errorProvider1.SetError(txtNegPNLLoss, "Please enter only numeric values");
                }
                else if (!DataTypeValidation.ValidateMaxPermiitedNumeric(txtNegPNLLoss.Text))
                {
                    errorProvider1.SetError(txtNegPNLLoss, "You cannot enter a value greater than 9223372036854775807 !");
                }
            }
        }

        #endregion textChanged event

    }

    #region class FAValueEventArgs

    /// <summary>
    /// This Class is used for the event to pass the AccountAccntId of the selected tradingAccnt in combo to other controls
    /// </summary>
    public class FAValueEventArgs : System.EventArgs
    {
        private String str;

        public String companyFundAccntID
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

    }  // FAValueEventArgs

    #endregion class FAValueEventArgs
}
