using ExportGridsData;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.LogManager;
using Prana.Utilities.UI.UIUtilities;
using Prana.WCFConnectionMgr;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.PM.Client.UI.Forms
{
    public partial class CreatePosition : Form, ICreateTransaction, IExportGridData
    {
        //private int _userID;
        //private int _companyID;
        private bool _isPopupFromCloseTrade;
        private CompanyUser _companyUser = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePosition"/> class.
        /// </summary>
        public CreatePosition()
        {
            InitializeComponent();
            InstanceManager.RegisterInstance(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePosition"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="companyID">The company ID.</param>

        //public CreatePosition(int userID, int companyID)
        //{
        //    this._userID = userID;
        //    this._companyID = companyID;
        //    InitializeComponent();
        //}
        // modified by Sandeep as on 05-Dec-2007 as we need more information related to the company user
        public CreatePosition(CompanyUser companyUser)
        {
            try
            {
                InitializeComponent();
                InstanceManager.RegisterInstance(this);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }

        }


        public void SetUp()
        {
            CreateAllocationServicesProxy();
            CreateCashManagementProxy();
        }


        ProxyBase<ICashManagementService> _proxyCashManagementServices = null;
        private void CreateCashManagementProxy()
        {
            _proxyCashManagementServices = new ProxyBase<ICashManagementService>("TradeCashServiceEndpointAddress");

            this.ctrlCreateAndImportPosition1.CashManagementServices = _proxyCashManagementServices;
        }
        ProxyBase<IAllocationManager> _allocationServices = null;

        private void CreateAllocationServicesProxy()
        {
            _allocationServices = new ProxyBase<IAllocationManager>("TradeAllocationServiceNewEndpointAddress");
            ctrlCreateAndImportPosition1.AllocationServices = _allocationServices;
        }

        public void InitControl(CompanyUser loginUser)
        {
            this._companyUser = loginUser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePosition"/> class.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <param name="companyID">The company ID.</param>
        /// <param name="isCloseTradePopUp">if set to <c>true</c> [is close trade pop up].</param>
        public CreatePosition(CompanyUser companyUser, bool isCloseTradePopUp)
        {
            this._companyUser = companyUser;
            //this._userID = companyUser.CompanyUserID;
            //this._companyID = companyUser.CompanyID;
            this._isPopupFromCloseTrade = isCloseTradePopUp;
            InitializeComponent();
            //ctrlRunDownload1.PopulateRunUploadDetails(_companyUser, _securityMaster);
            InstanceManager.RegisterInstance(this);
        }


        /// <summary>
        /// Gets or sets a value indicating whether [add button clicked].
        /// </summary>
        /// <value><c>true</c> if [add button clicked]; otherwise, <c>false</c>.</value>
        public bool IsAddButtonClicked
        {
            get { return ctrlCreateAndImportPosition1.AddButtonClicked; }
            // set { _addButtonClicked = value; }
        }



        public Prana.BusinessObjects.PositionManagement.NetPositionList NetPositions
        {
            get { return ctrlCreateAndImportPosition1.NetPositions; }
            set { ctrlCreateAndImportPosition1.NetPositions = value; }
        }

        /// <summary>
        /// Handles the Load event of the ctrlCreateAndImportPosition1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ctrlCreateAndImportPosition1_Load(object sender, EventArgs e)
        {
            ctrlCreateAndImportPosition1.AllocationServices = _allocationServices;
            ctrlCreateAndImportPosition1.SecurityMaster = _securityMaster;
            ctrlCreateAndImportPosition1.IsCloseTradePopup = this._isPopupFromCloseTrade;
            ctrlCreateAndImportPosition1.PopulateCreatePositionInterface(_companyUser);
        }

        void CreatePosition_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (ctrlCreateAndImportPosition1.IsDataLeftToBeSaved == true)
            {
                DialogResult dlgResult = DialogResult.Yes;
                dlgResult = MessageBox.Show("Do you want to save the data?", "Create Transaction Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlgResult.Equals(DialogResult.Yes))
                {
                    try
                    {
                        List<AllocationGroup> result = ctrlCreateAndImportPosition1.SaveCreateTransactions();
                        if (result.Count == 0)
                        {
                            e.Cancel = true;
                        }
                    }
                    catch (Exception ex)
                    {

                        // Invoke our policy that is responsible for making sure no secure information
                        // gets out of our layer.
                        bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);

                        if (rethrow)
                        {
                            throw;
                        }
                    }
                }
                else if (dlgResult.Equals(DialogResult.Cancel))
                {
                    e.Cancel = true;
                }
            }
            InstanceManager.ReleaseInstance(typeof(CreatePosition));
        }

        private void btnAddToCloseTrade_Click(object sender, EventArgs e)
        {
            try
            {
                OTCPosition otcPositionNew = new OTCPosition(0, 0, 0, 0, string.Empty, 0, 0, 0.0);
                ctrlCreateAndImportPosition1.AddNewRow(otcPositionNew, 0);
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            ctrlCreateAndImportPosition1.Removerow();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //DialogResult result = MessageBox.Show("Choose yes to delete all the rows from the grid, Choose No to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                DialogResult result = MessageBox.Show("Choose yes to delete all the saved rows from the grid, Choose No to Cancel?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    ctrlCreateAndImportPosition1.ClearSavedRows();

                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<AllocationGroup> result = ctrlCreateAndImportPosition1.SaveCreateTransactions();

                if (result.Count > 0)
                {
                    //PostTrade.PostTradeCacheManager.SaveGroups();
                    // _allocationServices.InnerChannel.SaveGroups(result);                    
                }

            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        void CreatePosition_Load(object sender, System.EventArgs e)
        {
            try
            {
                CustomThemeHelper.SetThemeProperties(sender as Form, CustomThemeHelper.THEME_STYLELIBRARYNAME, CustomThemeHelper.THEME_STYLESETNAME_CREATE_TRANSACTION);
                if (CustomThemeHelper.ApplyTheme)
                {
                    this.ultraFormManager1.FormStyleSettings.Caption = "<p style=\"font-family: Mulish;Text-align:Left\">" + CustomThemeHelper.PRODUCT_COMPANY_NAME + "</p>";
                    this.ultraFormManager1.DrawFilter = new FormTitleHelper(CustomThemeHelper.PRODUCT_COMPANY_NAME, this.Text, CustomThemeHelper.UsedFont);
                }

                if (!string.IsNullOrEmpty(CustomThemeHelper.WHITELABELTHEME) && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void SetButtonsColor()
        {
            try
            {
                btnAddToCloseTrade.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnAddToCloseTrade.ForeColor = System.Drawing.Color.White;
                btnAddToCloseTrade.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnAddToCloseTrade.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnAddToCloseTrade.UseAppStyling = false;
                btnAddToCloseTrade.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnClear.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                btnClear.ForeColor = System.Drawing.Color.White;
                btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnClear.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnClear.UseAppStyling = false;
                btnClear.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                btnSave.ForeColor = System.Drawing.Color.White;
                btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnSave.UseAppStyling = false;
                btnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                btnRemove.BackColor = System.Drawing.Color.FromArgb(140, 5, 5);
                btnRemove.ForeColor = System.Drawing.Color.White;
                btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btnRemove.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                btnRemove.UseAppStyling = false;
                btnRemove.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }
        void CreatePosition_Disposed(object sender, System.EventArgs e)
        {
            if (FormClosed != null)
            {
                FormClosed(this, EventArgs.Empty);
            }
        }

        void CreatePosition_FormClosed(object sender, System.EventArgs e)
        {
            //if (FormClosed != null)
            //{
            //    FormClosed(this, EventArgs.Empty);
            //}
        }



        #region ICreateTransaction Members

        public Form Reference()
        {
            return this;
        }

        public new event EventHandler FormClosed;

        #endregion
        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        /// <summary>
        /// used to Export Data for automation
        /// </summary>
        /// <param name="gridName"></param>
        /// <param name="WindowName"></param>
        /// <param name="tabName"></param>
        /// <param name="filePath"></param>
        public void ExportData(string gridName, string WindowName, string tabName, string filePath)
        {
            if (ctrlCreateAndImportPosition1 != null)
            {
                ctrlCreateAndImportPosition1.ExportDataForAutomation(gridName, filePath);
            }
        }
    }
}