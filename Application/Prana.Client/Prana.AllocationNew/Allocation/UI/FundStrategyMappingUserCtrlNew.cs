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
using Prana.CommonDataCache;
using Prana.Allocation.Common.Definitions;
using Prana.AllocationNew.Allocation.UI;
using System.Linq;

namespace Prana.AllocationNew
{
    public partial class AccountStrategyMappingUserCtrlNew : Infragistics.Win.Misc.UltraPanel
    {
         
        List<AllocationCtrl> _controllist = new List<AllocationCtrl>();
        //AllocationCtrl _accountAllocationCtrl = null;
        AccountCollection _accounts = new AccountCollection();
        StrategyCollection _strategies = new StrategyCollection();

        AccountStrategyAllocationControl accountStrategyAllocationControl = new AccountStrategyAllocationControl();

        public AccountStrategyMappingUserCtrlNew()
        {
            InitializeComponent();
        }
        public void SetUp(AccountCollection accounts, StrategyCollection strategies, bool onlyPercentage)
        {
            try
            {
                _accounts = accounts;
                _strategies = strategies;

                //int i = 0;
                //int startpoint = AllocationUIHelper.AddAccountsLabels(_accounts, this);
                //foreach (Strategy strategy in strategies)
                //{
                    
                   
                    
                //    AllocationCtrl allocationCtrl;

                //    if (i == 0)
                //    {
                //        allocationCtrl = new AllocationCtrl();
                //        allocationCtrl.OnlyPercentage = onlyPercentage;
                //        _accountAllocationCtrl = allocationCtrl;
                //        allocationCtrl.SetUp(strategy.StrategyID, accounts, null, "");
                //        allocationCtrl.CheckSumQty += new EventHandler(allocationCtrl_CheckSumQty);
                //        allocationCtrl.CheckSumPercentage += new EventHandler(allocationCtrl_CheckSumPercentage);
                //        //allocationCtrl.QtyPerChanged += new AllocationCtrl.QtyPercentageChangedHandler(allocationCtrl_QtyPerChanged);
                //    }
                //    else
                //    {
                //        allocationCtrl = new AllocationCtrl();
                //        allocationCtrl.OnlyPercentage = onlyPercentage;
                //        string strategyName = CachedDataManager.GetInstance.GetStrategyText(strategy.StrategyID);
                //        //accountAllocationCtrl.BackColor = Color;
                //        //allocationCtrl.QtyPerChanged+=new AllocationCtrl.QtyPercentageChangedHandler(CheckSumOfpercentage);
                //        allocationCtrl.SetUp(strategy.StrategyID, accounts, _accountAllocationCtrl, strategyName);

                //    }

                //    allocationCtrl.AutoSize = true;
                //    allocationCtrl.Location = new System.Drawing.Point(startpoint, 0);
                //    allocationCtrl.Size = new System.Drawing.Size(50, 50);
                //    this.ClientArea.Controls.Add(allocationCtrl);
                //    allocationCtrl.ValueChanged += allocationCtrl_ValueChanged;
                //    _controllist.Add(allocationCtrl);
                   
                //    startpoint += allocationCtrl.Width;
                //    i++;
                //}
                //BindEvents();

                this.ClientArea.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
                accountStrategyAllocationControl.AutoSize = true;
                accountStrategyAllocationControl.Location = new Point(35, 0);
                accountStrategyAllocationControl.Dock = DockStyle.Fill;
                accountStrategyAllocationControl.ValueChanged += allocationCtrl_ValueChanged;                
                accountStrategyAllocationControl.SetUp(CommonDataCache.CachedDataManager.GetInstance.GetUserAccountsAsDict(), CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies());
                this.ClientArea.Controls.Add(accountStrategyAllocationControl);
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
                accountStrategyAllocationControl.SetPrecisionDigit(precisionDigit);
                accountStrategyAllocationControl.SetGridMasking(CommonDataCache.CachedDataManager.GetInstance.GetUserStrategies());
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
        public event EventHandler CheckTotalQty;
        public event EventHandler CheckTotalPercentage;

        void allocationCtrl_CheckSumPercentage(object sender, EventArgs e)
        {
            if (CheckTotalPercentage != null)
            {
                CheckTotalPercentage(this, EventArgs.Empty);
            }
        }

       

        void allocationCtrl_CheckSumQty(object sender, EventArgs e)
        {
            if (CheckTotalQty != null)
            {
                CheckTotalQty(this, EventArgs.Empty);
            }
        }

        public void UnBindEvents()
        {
            if (_controllist != null)
            {
            foreach (AllocationCtrl allocationCtrl in _controllist)
            {
                if (allocationCtrl.StarategyID == int.MinValue)
                {
                    allocationCtrl.QtyPerChanged -= new AllocationCtrl.QtyPercentageChangedHandler(allocationCtrl_QtyPerChanged);
                }
                else
                {
                    allocationCtrl.CalculateTotalStrategyPercentage -= new AllocationCtrl.QtyPercentageChangedHandler(CheckSumOfpercentage);
                }
            }
            }

        }
        public void BindEvents()
        {
            foreach (AllocationCtrl allocationCtrl in _controllist)
            {
                if (allocationCtrl.StarategyID == int.MinValue)
                {
                    allocationCtrl.QtyPerChanged += new AllocationCtrl.QtyPercentageChangedHandler(allocationCtrl_QtyPerChanged);

                }
                else
                {
                    allocationCtrl.CalculateTotalStrategyPercentage += new AllocationCtrl.QtyPercentageChangedHandler(CheckSumOfpercentage);
                }
               
            }
        }
        void allocationCtrl_QtyPerChanged(int location,double qty,bool shouldValidate)
        {
            UnBindEvents();
            BlockChanges();
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                if (accountAllocationCtrl.StarategyID != int.MinValue)
                {
                    accountAllocationCtrl.ReSetQty(location);
                }
            }
            BindEvents();
            AllowChanges();
        }
        void CheckSumOfpercentage(int location,double allowedQty,bool shouldValidate)
        {
            BlockChanges();
            UnBindEvents();
            decimal sumOfQty = 0;
            decimal sumOfPercentage = 0;
            foreach (AllocationCtrl allocationCtrl in _controllist)
            {
                if (allocationCtrl.StarategyID != int.MinValue)
                {
                    sumOfPercentage += Convert.ToDecimal(allocationCtrl.GetPercentageIfValid(location));
                    sumOfQty += Convert.ToDecimal(allocationCtrl.GetQtyIfValid(location));
                }
            }
            if (Math.Round(sumOfPercentage) == 100 || sumOfPercentage == 0)
            {
                if (ChangePreference != null)
                    ChangePreference(this, new EventArgs());
            }
            if (Math.Round(sumOfPercentage) == 100 )
            {
                if (CheckTotalQty != null && !shouldValidate)
                {
                    CheckTotalQty(this, EventArgs.Empty);
                }
                else
                    {
                    if (CheckTotalPercentage != null && shouldValidate)
                        {
                        CheckTotalPercentage(this, EventArgs.Empty);
                            }
                        }
                //double remainingQty = allowedQty - sumOfQty;
                //if (remainingQty != 0)
                //{
                //    UnBindEvents();
                //    if (_controllist.Count > 1)
                //    {
                //        AllocationCtrl firstStrategyCtrl = _controllist[1];
                //        double prevQty = firstStrategyCtrl.GetQtyIfValid(location);
                //        double newQty=prevQty + remainingQty;
                //        foreach (AllocationCtrl allocationCtrl in _controllist)
                //        {
                //            if (allocationCtrl.StarategyID != int.MinValue)
                //            {
                //                allocationCtrl.SetQty(location, newQty);
                //                newQty =double.MinValue;
                //            }
                //        }

                //    }
                    }
            BindEvents();
            AllowChanges();
          
        }
        
        public int AddAccountsLabels()
        {

            Label[] lblAccounts = new Label[_accounts.Count];

            int i = 0;
            int startLabel_X = 5;
            int start_Y = 40;
            int yIncrement = 20;
            int lblLength = 20;
            int lblheight = 20;

            try
            {
                for (i = 0; i < _accounts.Count; i++)
                {
                    Account account = (Account)(_accounts[i]);

                    lblAccounts[i] = new Label();
                    //lblAccounts[i].AutoSize = true;
                    lblAccounts[i].Font = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular);
                    this.ClientArea.Controls.Add(lblAccounts[i]);
                    lblAccounts[i].Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                    //lblAccounts[i].Size = new System.Drawing.Size(lblLength, lblheight);
                    lblAccounts[i].Name = account.AccountID.ToString();
                    lblAccounts[i].AutoSize = true;
                    lblAccounts[i].Text = account.Name;
                    if (lblLength < lblAccounts[i].Width)
                        lblLength = lblAccounts[i].Width;
                }
                Label labelTotal = new Label();
                labelTotal.Location = new System.Drawing.Point(startLabel_X, start_Y + i * yIncrement);
                labelTotal.Size = new System.Drawing.Size(lblLength, lblheight);
               
                labelTotal.AutoSize = true;
                labelTotal.Text = "Total";
                if (lblLength < labelTotal.Width)
                    lblLength = labelTotal.Width;
                this.ClientArea.Controls.Add(labelTotal);

            }
            #region Catch
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

            }
            #endregion
            return lblLength + startLabel_X;
        }
        public AllocationLevelList GetAllocationAccounts(AllocationGroup group)
        {
            int i = 0;
            AllocationLevelList allocationAccounts = null;
            AllocationObjColl allocationobjColl=null;
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                if (i == 0)
                {
                    allocationobjColl = accountAllocationCtrl.GetAllocations(group.CumQty);
                    allocationAccounts = allocationobjColl.GetAllocationLevelList(group.GroupID);
                }
                else
                {

                    AllocationObjColl allocationobjCollL2 = accountAllocationCtrl.GetAllocations(group.CumQty);
                    allocationobjColl.MergerLevel2(allocationAccounts, allocationobjCollL2);
                }
                i++;
            }
            return allocationAccounts;
        }
        private  void BlockChanges()
        {
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                accountAllocationCtrl.BlockChanges();
            }
        }
        private  void AllowChanges()
        {
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                accountAllocationCtrl.AllowChanges();
            }
        }
        public void SetAllocationAccounts(AllocationGroup alloationGroup, bool shouldClear)
        {

            bool islong = PranaBasicBusinessLogic.IsLongSide(alloationGroup.OrderSideTagValue);
            BlockChanges();
            ClearQty();

            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                if (accountAllocationCtrl.StarategyID == int.MinValue)
                {
                    AllocationObjColl objAllocation = new AllocationObjColl();
                    objAllocation.SetValues(alloationGroup.Allocations, islong, alloationGroup.State);
                    accountAllocationCtrl.SetAllocationAccounts(objAllocation, alloationGroup.CumQty, PranaBasicBusinessLogic.IsLongSide(alloationGroup.OrderSideTagValue), alloationGroup.AssetID, shouldClear);
                }
                else
                {
                    AllocationLevelList list = alloationGroup.Allocations.GetSecondLevelAccounts(accountAllocationCtrl.StarategyID);
                  AllocationObjColl objAllocation = new AllocationObjColl();
                  objAllocation.SetValues(list, islong, alloationGroup.State);
                    accountAllocationCtrl.SetAllocationAccounts(objAllocation, alloationGroup.CumQty, PranaBasicBusinessLogic.IsLongSide(alloationGroup.OrderSideTagValue), alloationGroup.AssetID, shouldClear);
                }
            }
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                accountAllocationCtrl.CalculateTotalPercentage();
            }
            AllowChanges();
        }
       
        public void SetSelectionStatus(bool multipleSelected)
        {
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {
                accountAllocationCtrl.SetSelectionStatus(multipleSelected);
            }
        }
        private AllocationDefault _allocationDefault;

        public AllocationDefault AllocationDefault
        {
            get
            {
                
                return _allocationDefault;
            }
           
        }
       

        public void  SetAllocationDefault(AllocationDefault allocationDefault)
        {
            BlockChanges();
            _allocationDefault = allocationDefault;
            foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            {

                if (accountAllocationCtrl.StarategyID == int.MinValue)
                {
                    AllocationObjColl objAllocation = new AllocationObjColl();
                    objAllocation.SetValues(allocationDefault.DefaultAllocationLevelList, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED);
                    accountAllocationCtrl.SetDefaults(objAllocation);
                }
                else
                {
                    AllocationLevelList list = allocationDefault.DefaultAllocationLevelList.GetSecondLevelAccounts(accountAllocationCtrl.StarategyID);
                    AllocationObjColl objAllocation = new AllocationObjColl();
                    objAllocation.SetValues(list, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED);
                    accountAllocationCtrl.SetDefaults(objAllocation);
                }
               
            }
            AllowChanges();
        }
        public void ClearQty()
        {
            accountStrategyAllocationControl.ClearGrid();
            //foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            //{
            //    accountAllocationCtrl.ClearQty();
            //}
        }

        public void ClearPercentage()
        {
            //foreach (AllocationCtrl accountAllocationCtrl in _controllist)
            //{
            //    accountAllocationCtrl.ClearPercentage();
            //}
        }

        #region AllocationOperationPreference
        

        /// <summary>
        /// Set allocation Preferences in Account strategy control.
        /// </summary>
        /// <param name="allocationOperationPreference"></param>
        internal void SetAllocationDefault(AllocationOperationPreference allocationOperationPreference)
        {
            try
            {
                BlockChanges();
                accountStrategyAllocationControl.SetValues(allocationOperationPreference.TargetPercentage, 2);
                //_allocationOperationPreference = allocationOperationPreference;
                //foreach (AllocationCtrl accountAllocationCtrl in _controllist)
                //{

                //    if (accountAllocationCtrl.StarategyID != int.MinValue)
                //    {
                //        AllocationObjColl objAllocation = new AllocationObjColl();
                //        objAllocation.SetValues(allocationOperationPreference.TargetPercentage, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED, 2, accountAllocationCtrl.StarategyID);
                //        accountAllocationCtrl.SetDefaults(objAllocation);
                //    }
                //    else
                //    {
                //        AllocationObjColl objAllocation = new AllocationObjColl();
                //        objAllocation.SetValues(allocationOperationPreference.TargetPercentage, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED, 1, -1);
                //        accountAllocationCtrl.SetDefaults(objAllocation);
                //    }

                //}
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
        /// Event raised when there is any change in value in control.
        /// </summary>
        public event EventHandler ChangePreference;

        /// <summary>
        /// Event raised when textbox value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void allocationCtrl_ValueChanged(object sender, EventArgs e)
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

        /// <summary>
        /// returns target percentage from control.
        /// </summary>
        /// <returns></returns>
        internal SerializableDictionary<int, AccountValue> GetAllocationAccountValue()
        {
            try
            {
                SerializableDictionary<int, AccountValue> targetpercentage = new SerializableDictionary<int, AccountValue>();
                targetpercentage = accountStrategyAllocationControl.GetAllocationAccountValue();
                //AllocationObjColl allocationobjColl = null;
                //foreach (AllocationCtrl accountAllocationCtrl in _controllist)
                //{
                //    if (accountAllocationCtrl.StarategyID == int.MinValue)
                //    {
                //        allocationobjColl = accountAllocationCtrl.GetAllocations(0);
                //        foreach (AllocationCtrlObject obj in allocationobjColl.Collection)
                //        {
                //            //Not adding account with id 0
                //            if (obj.ID != 0)
                //            {
                //                AccountValue account = new AccountValue(obj.ID, Convert.ToDecimal(obj.Percentage));
                //                if (!targetpercentage.ContainsKey(account.AccountId))
                //                    targetpercentage.Add(account.AccountId, account);
                //                else
                //                    targetpercentage[account.AccountId] = account;
                //            }
                //        }
                //    }
                //    else
                //    {

                //        AllocationObjColl allocationobjCollL2 = accountAllocationCtrl.GetAllocations(0);
                //        foreach (AllocationCtrlObject obj in allocationobjCollL2.Collection)
                //        {
                //            if (targetpercentage.ContainsKey(obj.ID))
                //            {
                //                targetpercentage[obj.ID].StrategyValueList.Add(new StrategyValue(accountAllocationCtrl.StarategyID, (decimal)obj.Percentage));
                //            }

                //        }
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
        /// Sets the allocation accounts.
        /// </summary>
        /// <param name="allocationGroup">The allocation group.</param>
        internal void SetAllocationAccounts(AllocationGroup allocationGroup)
        {
            try
            {
                bool islong = PranaBasicBusinessLogic.IsLongSide(allocationGroup.OrderSideTagValue);
                decimal qty = allocationGroup.Allocations.Collection.Sum(x => Convert.ToDecimal(x.AllocatedQty));
                SerializableDictionary<int, AccountValue> targetDict = new SerializableDictionary<int, AccountValue>();
                foreach (AllocationLevelClass allocation in allocationGroup.Allocations.Collection)
                {
                    decimal percentage = qty == 0 ? 0 : ((decimal)allocation.AllocatedQty * 100) / qty;
                    AccountValue account = new AccountValue(allocation.LevelnID, percentage);                    
                    if (allocation.Childs != null)
                    {
                        decimal accountQty = allocation.Childs.Collection.Sum(x => Convert.ToDecimal(x.AllocatedQty));
                        foreach (AllocationLevelClass strategy in allocation.Childs.Collection)
                        {
                            decimal per = accountQty == 0 ? 0 : ((decimal)strategy.AllocatedQty * 100) / accountQty;
                            StrategyValue strategyValue = new StrategyValue(strategy.LevelnID, per);
                            account.StrategyValueList.Add(strategyValue);
                        }
                    }
                    if (targetDict.ContainsKey(account.AccountId))
                        targetDict[account.AccountId] = account;
                    else
                        targetDict.Add(account.AccountId, account);
                }
                accountStrategyAllocationControl.SetValues(targetDict, 2);
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
        internal void SetQuantity(decimal cumQauntity)
        {
            try
            {
                accountStrategyAllocationControl.UpdateQuantity(cumQauntity);
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
                accountStrategyAllocationControl.UpdateTotalNoOfTrades(selected,total);
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
        /// show searched strategy in current view of ultragid
        /// </summary>
        /// <param name="searchStrategy">The strategy name</param>
        internal void ShowSearchedStrategy(string searchStrategy)
        {
            try
            {
                accountStrategyAllocationControl.ShowSearchedStrategy(searchStrategy);
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

        #endregion


    }
}
