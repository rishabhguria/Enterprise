using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Prana.BusinessObjects;
using Prana.Interfaces;
using Prana.CommonDataCache;
using Prana.Global;
//using Prana.PostTrade;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Utilities.UIUtilities;
using Prana.WCFConnectionMgr;
using System.Configuration;

namespace Prana.AllocationNew
{
    public sealed partial class CommissionForm : Form, ICommissionCalculation
    {

        public CommissionForm()
        {
            InitializeComponent();

            //UIThreadMarshaller.AddFormForMarshalling(UIThreadMarshaller.COMMISSION_FORM, this);
            ctrlRecalculate1.RecalculateCommission += new EventHandler(ctrlRecalculate1_RecalculateCommission);
            ctrlRecalculate1.BulkChangeOnGroupLevel += new EventHandler(ctrlRecalculate1_BulkChangeOnGroupLevel);
            ctrlRecalculate1.DisplayMessage += new EventHandler(ctrlRecalculate1_DisplayMessage);
            //ctrlAmendmend1.allocationDataChange += new AllocationDataChangeHandler(ctrlAmendmend1_allocationDataChange);
            CreateAllocationServicesProxy();
            CreateClosingServicesProxy();
            CreateCashManagementServicesProxy();
            //ctrlRecalculate1_RecalculateCommission += new EventHandler(ctrlRecalculate1_ViewRule);
        }
      
        void ctrlRecalculate1_DisplayMessage(object sender, EventArgs e)
        {
            ctrlAmendmend1.DisplayErrorMessage();
        }

        void ctrlAmendmend1_allocationDataChange(object sender, bool isStarting)
        {
            allocationDataChange(this, new EventArgs<bool>(isStarting));
        }
      
        //void ctrlRecalculate1_ViewRule(object sender, EventArgs e)
        //{
        //    CommissionRule commissionRule = sender as CommissionRule;

        //}


        ISecurityMasterServices _securityMaster = null;
        public ISecurityMasterServices SecurityMaster
        {
            set { _securityMaster = value; }
        }

        void ctrlRecalculate1_RecalculateCommission(object sender, EventArgs e)
        {
            ctrlAmendmend1.RecalculateCommissionAndFees(sender, e);

        }
        void ctrlRecalculate1_BulkChangeOnGroupLevel(object sender, EventArgs e)
        {
            ctrlAmendmend1.BulkChangeOnGroupLevel(sender, e);
        }

        private AllocationGroupCollection _allocationGroups = new AllocationGroupCollection();
        public AllocationGroupCollection ModifiedallocationGroups
        {
            get
            {
                return _allocationGroups;
            }
            set
            {
                value = _allocationGroups;
            }
        }


        private CompanyUser _currentUser;

        public CompanyUser CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
            }
        }
        
        //public event EventHandler CommissionAllocationClosed;


        private void CreateAllocationServicesProxy()
        {
            ctrlRecalculate1.AllocationServices = AllocationManager.GetInstance().AllocationServices; 
            ctrlAmendmend1.AllocationServices = AllocationManager.GetInstance().AllocationServices; 
        }


        private void CreateClosingServicesProxy()
        {
           ctrlAmendmend1.ClosingServices = AllocationManager.GetInstance().ClosingServices;
        }


        private void CreateCashManagementServicesProxy()
        {
            ctrlAmendmend1.CashManagementServices = AllocationManager.GetInstance().CashManagementServices;
        }
      
        void CommissionForm_Load(object sender, System.EventArgs e)
        {
            try
            {
                //PostTradeCacheManager.IsAllocDataReferenced = true;
                AllocationManager.GetInstance().IsAllocDataReferenced = true;
                ctrlRecalculate1.InitControl(CurrentUser.CompanyUserID);
                ctrlAmendmend1.SecurityMaster = _securityMaster;
                ctrlAmendmend1.InvokeSecurityMaster();
                //if (PostTradeCacheManager.CommissionCalculationTime == true)
                //{
                //    this.Text = "Commission Calculation: Post Allocation";
                //}
                //else
                //{
                //    this.Text = "Commission Calculation: Pre Allocation";
                //}
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
            }
            
        }


        //public void GetAllAlloctedOrders()
        //{
        //    // _orderAccountAllocationManager = OrderAccountAllocationManager.GetInstance;          
        //    //AllocationGroups allocationGroups = _orderAccountAllocationManager.AllocatedGroups;
        //    foreach (AllocationGroup allocationGroup in _orderAccountAllocationManager.AllocatedGroups)
        //    {
        //        allocationGroup.AssetName = CachedDataManager.GetInstance.GetAssetText(allocationGroup.AssetID);
        //        allocationGroup.UnderlyingName = CachedDataManager.GetInstance.GetUnderLyingText(allocationGroup.UnderlyingID);
        //        allocationGroup.ExchangeName = CachedDataManager.GetInstance.GetExchangeText(allocationGroup.ExchangeID);
        //        if (allocationGroup.CurrencyID != int.MinValue)
        //        {
        //            allocationGroup.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(allocationGroup.CurrencyID);

        //        }
        //        else
        //        {
        //            allocationGroup.CurrencyID = CachedDataManager.GetInstance.GetCurrencyIdByAUECID(allocationGroup.AUECID);
        //            allocationGroup.CurrencyName = CachedDataManager.GetInstance.GetCurrencyText(allocationGroup.CurrencyID);
        //        }
        //        //foreach (TaxLot allocationAccount in allocationGroup.TaxLots)
        //        //{
        //        //    allocationAccount.AccountName = CachedDataManager.GetInstance.GetAccountText(allocationAccount.AccountID);
        //        //}
        //        // allocationGroup.PropertyHasChanged();
        //    }

        //    grdCommision.DataSource = null;
        //    grdCommision.DataSource = _orderAccountAllocationManager.AllocatedGroups;
        //    grdCommision.Refresh();
        //    //grdCommision.DataBind();




        //}
      
        //bool _commissionCalculationTime = false;


        
      

     
        #region PostAllocated_Tab
        /// <summary>
        /// Sets the grid appearance and layout.
        /// </summary>
        /// <param name="grid">The grid.</param>
        private void SetGroupedGridAppearanceAndLayout(ref UltraGridLayout gridLayout)
        {
            gridLayout.Appearance.BackColor = Color.Black;
            gridLayout.Override.SelectedRowAppearance.BorderColor = Color.White;
            gridLayout.Override.SelectedCellAppearance.BackColor = Color.Transparent;
            gridLayout.Override.RowSelectorStyle = Infragistics.Win.HeaderStyle.Default;
            gridLayout.Override.ActiveRowAppearance.BackColor = Color.Transparent;
            gridLayout.Override.ActiveCellAppearance.BackColor = Color.Gold;
            gridLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
            gridLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Solid;
            gridLayout.Override.CellAppearance.BorderColor = Color.Transparent; ;
            gridLayout.Override.RowAppearance.BorderColor = Color.Transparent; ;

            gridLayout.AutoFitStyle = AutoFitStyle.None;
            gridLayout.ScrollBounds = ScrollBounds.ScrollToLastItem;
            //gridLayout.Override.AllowAddNew = AllowAddNew.Yes;
            gridLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.AllowGroupBy = Infragistics.Win.DefaultableBoolean.False;
            gridLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            gridLayout.Override.AllowColSwapping = AllowColSwapping.NotAllowed;
            gridLayout.Override.HeaderAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
            gridLayout.Override.RowFilterMode = RowFilterMode.AllRowsInBand;

            gridLayout.Override.ColumnSizingArea = ColumnSizingArea.EntireColumn;
            gridLayout.Override.ColumnAutoSizeMode = ColumnAutoSizeMode.VisibleRows;
        }




        #endregion





        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to close ?", "Confirm Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.FindForm().Close();
            }
        }





        public void SetUp(CompanyUser loginUser, bool allocationUIOpened)
        {
            CurrentUser = loginUser;
            if (!allocationUIOpened)
            {
                //  string ToAllAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
                //string FromAUECDatesString = TimeZoneHelper.GetAllAUECLocalDatesFromUTCStr(DateTime.UtcNow);
                string ToAllAUECDatesString = DateTime.Now.ToString();
                string FromAUECDatesString = DateTime.Now.ToString();
                //PostTrade.PostTradeCacheManager.Initlise(loginUser);
                // _allocationServices.Initlise(loginUser);
                //TODO
                //ClosingCommonCacheManager.Instance.GetTaxlotsLatestCADates();
                //ClosingCommonCacheManager.Instance.GetTaxlotsLatestClosingDates();
                //ClosingCommonCacheManager.Instance.GetExcercisedTaxlots();
                //PostTrade.PostTradeCacheManager.FillGroupData(ToAllAUECDatesString,FromAUECDatesString);
                AllocationManager.GetInstance().GetGroups(ToAllAUECDatesString, FromAUECDatesString, false, CurrentUser.CompanyUserID);
            }
            ctrlAmendmend1.SetGridDataSources();
            ///commented and preferences are loaded in control only.
            //ctrlAmendmend1.LoadPreferences(loginUser);
        }

        void CommissionForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            //PostTradeCacheManager.IsAllocDataReferenced = false;
            AllocationManager.GetInstance().IsAllocDataReferenced = false;
            // PostTradeCacheManager.Clear();
            if (CommissionFormClosed != null)
            {
                CommissionFormClosed(this, e);
            }
        }





        #region ICommissionCalculation Members

        Form ICommissionCalculation.Reference()
        {
            return this;
        }


        #endregion

        #region ICommissionCalculation Members


        public event EventHandler CommissionFormClosed;

        #endregion

        bool _saveDataIfControlIsDisable = true;
        private void CommissionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_saveDataIfControlIsDisable)
            {
                //form not in use hence commenting function
                //DialogResult userChoice = ctrlAmendmend1.PromptForDataSaving();
            }
            ctrlRecalculate1.RecalculateCommission -= new EventHandler(ctrlRecalculate1_RecalculateCommission);
            ctrlRecalculate1.BulkChangeOnGroupLevel -= new EventHandler(ctrlRecalculate1_BulkChangeOnGroupLevel);
            //CommissionFormClosed(this, null);
        }


        #region ICommissionCalculation Members


        /// <summary>
        /// Toogle status of controls in UI as well as conrols in any child user control (currently ctrlAmendmend1)
        /// </summary>
        /// <param name="message">Message to be displayed on StatusLabel</param>
        /// <param name="elementStatus">True to enable, False to disable</param>
        public void ToggleUIElementWithMessage(string message, bool elementStatus)
        {
            try
            {
                _saveDataIfControlIsDisable = elementStatus;
                ctrlRecalculate1.Enabled = elementStatus;
                ctrlAmendmend1.ToggleUIElementsWithMessage(message, elementStatus);
                //ControlBox = elementStatus;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        #endregion

        #region ICommissionCalculation Members

        //public event AllocationDataChangeHandler allocationDataChange;
        public event EventHandler<EventArgs<bool>> allocationDataChange;

        #endregion
    }
}
