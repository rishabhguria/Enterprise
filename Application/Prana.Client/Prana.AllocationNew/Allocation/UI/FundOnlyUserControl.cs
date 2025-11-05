using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.BusinessObjects;
using Prana.Global;
using Prana.Allocation.Common.Definitions;
using Prana.AllocationNew.Allocation.UI;
using Infragistics.Win.Misc;
using System.Linq;

namespace Prana.AllocationNew
{
    public partial class AccountOnlyUserControl : Infragistics.Win.Misc.UltraPanel, IAllocationCalculator
    {
        AllocationCtrl _accountAllocationCtrl = new AllocationCtrl();
        AccountCollection _accounts=new AccountCollection();
        public AccountOnlyUserControl()
        {
            InitializeComponent();
        }

        AccountStrategyAllocationControl accountAllocationControl = new AccountStrategyAllocationControl();
        
        /// <summary>
        /// Maximum length account
        /// </summary>
        private int _maxLength = 300;

        /// <summary>
        /// Set up the accountAllocationControl
        /// </summary>
        /// <param name="accounts"></param>
        public void SetUp(AccountCollection accounts)
        {
            try
            {
                accountAllocationControl.AutoSize = true;
                accountAllocationControl.AutoScroll = false;
                accountAllocationControl.Location = new System.Drawing.Point(15, 20);
                ////count contain no. of accounts permission for the user
                //int count = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.Count;
                _maxLength = (accountAllocationControl.GetMaxAccountLength() * 4) + 250;
                ////Setting the size of control at run time calculated by formula
                //accountAllocationControl.Size = new System.Drawing.Size(_maxLength, ((count * 22) + 189));
                accountAllocationControl.Padding = new Padding(0, 0, 0, 30);	//Set bottom padding for the grid
                this.ClientArea.Controls.Add(accountAllocationControl);
                accountAllocationControl.SetUp(CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict(), null);
                //accountAllocationControl.CheckSumQty += new EventHandler(_accountAllocationCtrl_CheckSumQty);
               // _accountAllocationCtrl.CheckSumPercentage += new EventHandler(_accountAllocationCtrl_CheckSumPercentage);
                accountAllocationControl.ValueChanged += _accountAllocationCtrl_ValueChanged;
                //Event when row changed in account grid
                accountAllocationControl.RowChanged += accountAllocationControl_RowChanged;
                //_accounts = accounts;
              
               
                //int startpoint = AllocationUIHelper.AddAccountsLabels(_accounts, this);
               // _accountAllocationCtrl = new AllocationCtrl();
               
                //_accountAllocationCtrl.AutoSize = true;
                //_accountAllocationCtrl.Location = new System.Drawing.Point(startpoint, 0);
                //_accountAllocationCtrl.Size = new System.Drawing.Size(50, 50);
                //this.ClientArea.Controls.Add(_accountAllocationCtrl);
                //_accountAllocationCtrl.SetUp(int.MinValue, accounts,null,"");
                //_accountAllocationCtrl.CheckSumQty += new EventHandler(_accountAllocationCtrl_CheckSumQty);
                //_accountAllocationCtrl.CheckSumPercentage += new EventHandler(_accountAllocationCtrl_CheckSumPercentage);
                //_accountAllocationCtrl.ValueChanged += _accountAllocationCtrl_ValueChanged;

            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// To check if Edit prefrence checkbox is not checked then set the size of AccountAllocation grid control is fixed
        /// http://jira.nirvanasolutions.com:8080/browse/PRANA-8124
        /// </summary>
        public void SetDockAccountAllocationControl(bool showAllocationRule)
        {
            try
            {
                if (showAllocationRule)
                {
                    this.AutoScroll = true;
                    accountAllocationControl.ShowRule = true;
                    accountAllocationControl.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    //count contain no. of accounts permission for the user
                    int count = CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict().Keys.Count;
                    //Setting the size of control at run time calculated by formula
                    accountAllocationControl.Size = new System.Drawing.Size(_maxLength, count*24+186);
                }
                else
                {
                    this.AutoScroll = false;
                    accountAllocationControl.ShowRule = false;
                    accountAllocationControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom;
                    accountAllocationControl.Size = new System.Drawing.Size(_maxLength, 200);
                }
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// Send precision digit value to AccountStrategyAllocationControl
        /// </summary>
        /// <param name="precisionDigit"></param>
        public void SetPrecisionDigit(int precisionDigit)
        {
            try
            {
                accountAllocationControl.SetPrecisionDigit(precisionDigit);
                accountAllocationControl.SetGridMasking(null);
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
        /// <summary>
        /// Event when row changed in account grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">No of row</param>
        void accountAllocationControl_RowChanged(object sender, EventArgs<int> e)
        {
            try
            {
                   accountAllocationControl.Size = new System.Drawing.Size(_maxLength, ((e.Value * 22) + 189));
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public event EventHandler CheckTotalQty;
        public event EventHandler CheckTotalPercentage;

        void _accountAllocationCtrl_CheckSumPercentage(object sender, EventArgs e)
        {
            if (CheckTotalPercentage != null)
            {
                CheckTotalPercentage(this, EventArgs.Empty);
            }
        }

       
        void _accountAllocationCtrl_CheckSumQty(object sender, EventArgs e)
        {
            if (CheckTotalQty != null)
            {
                CheckTotalQty(this, EventArgs.Empty);
            }
        }

       

        public AllocationLevelList GetAllocationAccounts(AllocationGroup group)
        {
            try
            {
                return _accountAllocationCtrl.GetAllocations(group.CumQty).GetAllocationLevelList(group.GroupID);
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            return null;
        }

        public void SetAllocationAccounts(AllocationGroup group, bool shouldClear)
        {

            try
            {
                BlockChanges();
                AllocationObjColl allocationObjColl = new AllocationObjColl();
                allocationObjColl.SetValues(group.Allocations, PranaBasicBusinessLogic.IsLongSide(group.OrderSideTagValue), PostTradeConstants.ORDERSTATE_ALLOCATION.UNALLOCATED);

                if (shouldClear)
                {
                    _accountAllocationCtrl.ClearQty();
                    _accountAllocationCtrl.ClearPercentage();
                }
                _accountAllocationCtrl.SetAllocationAccounts(allocationObjColl, group.CumQty, PranaBasicBusinessLogic.IsLongSide(group.OrderSideTagValue), group.AssetID, shouldClear);
                
                //_accountAllocationCtrl.RecalculateQty();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
            finally 
            {
                AllowChanges();
            }
        }

        public void ClearQty()
        {
            try
            {
                accountAllocationControl.ClearGrid();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        public void ClearPercentage()
        {
            try
            {
                //_accountAllocationCtrl.ClearPercentage();
            }
            catch (Exception ex)
            {

                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);

                if (rethrow)
                {
                    throw;
                }
            }
        }

        private bool _multipleGroupSelected;
        
        public bool MultipleGroupSelected
        {
            get { return _multipleGroupSelected; }
            set { 
                
                _multipleGroupSelected = value;
               // _accountAllocationCtrl.SetQtyFieldsView(!value);
                
            }
        }

        public void SetSelectionStatus(bool multipleSelected)
        {
            _accountAllocationCtrl.SetSelectionStatus(multipleSelected);
        }

        private AllocationDefault _allocationDefault;

        public AllocationDefault AllocationDefault
        {
            get
            {

                return _allocationDefault;
            }

        }
        private void BlockChanges()
        {
                _accountAllocationCtrl.BlockChanges();
        }
        private void AllowChanges()
        {
            _accountAllocationCtrl.AllowChanges();
        }
        public void SetAllocationDefault(AllocationDefault allocationDefault)
        {
            BlockChanges();
            _allocationDefault = allocationDefault;
            AllocationObjColl allocationObjColl = new AllocationObjColl();
            allocationObjColl.SetValues(allocationDefault.DefaultAllocationLevelList, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED);
            _accountAllocationCtrl.SetDefaults(allocationObjColl);
            AllowChanges();
        }
        public void HideControl(bool visible)
        {
            this.Enabled = visible;
        }

        #region AllocationOperationPreference

        /// <summary>
        /// Set allocation Preferences in Account control.
        /// </summary>
        /// <param name="allocationOperationPreference"></param>
        public void SetAllocationDefault(AllocationOperationPreference allocationOperationPreference)
        {
            try
            {
                BlockChanges();
                accountAllocationControl.SetValues(allocationOperationPreference.TargetPercentage,1);
                // _allocationDefault = allocationDefault;
                //AllocationObjColl allocationObjColl = new AllocationObjColl();
               // allocationObjColl.SetValues(allocationOperationPreference.TargetPercentage, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED, 1, -1);
                //_accountAllocationCtrl.SetDefaults(allocationObjColl);
                AllowChanges();
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

        /// <summary>
        /// Returns current value in control.
        /// </summary>
        /// <returns></returns>
        public SerializableDictionary<int, AccountValue> GetAllocationAccountValue()
        {

            try
            {
                
                SerializableDictionary<int, AccountValue> targetpercentage = new SerializableDictionary<int, AccountValue>();
                targetpercentage = accountAllocationControl.GetAllocationAccountValue();
                //AllocationObjColl accountCol = _accountAllocationCtrl.GetAllocations(0);
                //foreach (AllocationCtrlObject obj in accountCol.Collection)
                //{
                //    if (obj.ID != 0)
                //    {
                //        AccountValue account = new AccountValue(obj.ID, Convert.ToDecimal(obj.Percentage));
                //        if (!targetpercentage.ContainsKey(account.AccountId))
                //            targetpercentage.Add(account.AccountId, account);
                //        else
                //            targetpercentage[account.AccountId] = account;
                //    }
                //}
                return targetpercentage;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Event raised when there is any change in value in control.
        /// </summary>
        public event EventHandler ChangePreference;

        /// <summary>
        /// Event when text is changed in control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _accountAllocationCtrl_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChangePreference != null)
                    ChangePreference(this, new EventArgs());
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

        #region IAllocationCalculator Members


        /// <summary>
        /// Sets the allocation accounts.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        public void SetAllocationAccounts(AllocationGroup allocationGroup)
        {
            try
            {
                decimal qty = allocationGroup.Allocations.Collection.Sum(x => Convert.ToDecimal(x.AllocatedQty));
                SerializableDictionary<int, AccountValue> targetDict = new SerializableDictionary<int, AccountValue>();
                foreach (AllocationLevelClass allocation in allocationGroup.Allocations.Collection)
                {
                    decimal percentage = qty == 0 ? 0 : ((decimal)allocation.AllocatedQty * 100) / qty;
                    AccountValue account = new AccountValue(Convert.ToInt32(allocation.LevelnID), percentage);
                    //if (allocation.Childs != null)
                    //{
                    //    List<StrategyValue> strategylist = new List<StrategyValue>();
                    //    foreach (AllocationLevelClass strategy in allocation.Childs.Collection)
                    //    {
                    //        decimal per = qty == 0 ? 0 : ((decimal)strategy.AllocatedQty / qty) * 100;
                    //        StrategyValue strategyValue = new StrategyValue(Convert.ToInt32(strategy.LevelnAllocationID), per);
                    //        strategylist.Add(strategyValue);
                    //    }
                    //}
                    if (targetDict.ContainsKey(account.AccountId))
                        targetDict[account.AccountId] = account;
                    else
                        targetDict.Add(account.AccountId, account);
                }
                accountAllocationControl.SetValues(targetDict, 1);
                //accountAllocationControl.SetAllocationAccounts(allocationGroup.Allocations);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }


        /// <summary>
        /// Sets the quantity.
        /// </summary>
        /// <param name="cumQauntity">The cum qauntity.</param>
        public void SetQuantity(decimal cumQauntity)
        {
            try
            {
                accountAllocationControl.UpdateQuantity(cumQauntity);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Sets the Total and selected no of trade.
        /// </summary>
        /// <param name="cumQauntity">selected.</param>
        /// /// <param name="cumQauntity">total</param>
        internal void SetTotalNoOfTrades(int selected, int total)
        {
            try
            {
                accountAllocationControl.UpdateTotalNoOfTrades(selected, total);
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

       /// <summary>
        /// Getting the max length of the account
       /// </summary>
        /// <returns>max length of the account</returns>
        internal int GetMaxAccountLength()
        {
            try
            {
                return _maxLength;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return 0;
            }
        }
        #endregion
    }
}
