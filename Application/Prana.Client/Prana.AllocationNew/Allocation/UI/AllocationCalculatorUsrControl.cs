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

namespace Prana.AllocationNew
{
    public partial class AllocationCalculatorUsrControl : Infragistics.Win.Misc.UltraPanel, IAllocationCalculator
    {
        AccountCollection _accounts = new AccountCollection();
        StrategyCollection _strategies = new StrategyCollection();



        public AllocationCalculatorUsrControl()
        {
            InitializeComponent();
        }
        
        public void SetUp(AccountCollection accounts)
        {
            try
            {
                
                AllocationManager.GetInstance().FillCurrentPositions();
                _accounts = accounts;
                int startpoint = AllocationUIHelper.AddAccountsLabels(_accounts, this);

                newCtrl.Location = new System.Drawing.Point(startpoint, 0);
                newCtrl.SetUp(int.MinValue, _accounts, null, "New-Allocation");

                startpoint += newCtrl.Width;

                currentCtrl.Location = new System.Drawing.Point(startpoint, 0);
                currentCtrl.SetUp(int.MinValue, _accounts, null, "Current-Allocation");
                currentCtrl.Enabled = false;

                startpoint += currentCtrl.Width;

                targetCtrl.Location = new System.Drawing.Point(startpoint, 0);
                targetCtrl.SetUp(int.MinValue, _accounts, null, "Tgt-Allocation");


                BindEvents();
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
        /// events for inter control changes
        /// </summary>
        public void BindEvents()
        {
            newCtrl.QtyPerChanged += new AllocationCtrl.QtyPercentageChangedHandler(newCtrl_QtyPerChanged);
            targetCtrl.QtyPerChanged += new AllocationCtrl.QtyPercentageChangedHandler(targetCtrl_QtyPerChanged);
            newCtrl.ValueChanged += newCtrl_ValueChanged;
            targetCtrl.ValueChanged += targetCtrl_ValueChanged;
        }
        
        public void UnBindEvents()
        {
            newCtrl.QtyPerChanged -= new AllocationCtrl.QtyPercentageChangedHandler(newCtrl_QtyPerChanged);
            targetCtrl.QtyPerChanged -= new AllocationCtrl.QtyPercentageChangedHandler(targetCtrl_QtyPerChanged);
            newCtrl.ValueChanged -= newCtrl_ValueChanged;
            targetCtrl.ValueChanged -= targetCtrl_ValueChanged;
        }
        /// <summary>
        /// sets new control's qty and percentage
        /// </summary>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="percentage"></param>
        void targetCtrl_QtyPerChanged(int location, double qty, bool shouldValidate)
        {

            try
            {
                UnBindEvents();
                BlockChanges();
                double currentQty = currentCtrl.GetQtyIfValid(location);
                double newQty =Math.Abs( qty - currentQty); // target-current

                //float currentPercentage = currentCtrl.GetPercentageIfValid(location);
                //float newpercentage = percentage - currentPercentage; // new = target-current

                newCtrl.SetQty(location, newQty);
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
                BindEvents();
                AllowChanges();
            }
        }
        /// <summary>
        /// sets target control's qty and percentage
        /// </summary>
        /// <param name="location"></param>
        /// <param name="qty"></param>
        /// <param name="percentage"></param>
        void newCtrl_QtyPerChanged(int location, double qty, bool shouldValidate)
        {
            try
            {
                UnBindEvents();
                BlockChanges();
                double currentQty = currentCtrl.GetQtyIfValid(location);
                double targetQty = qty + currentQty; // target=new +current

                //float currentPercentage = currentCtrl.GetPercentageIfValid(location);
               // float targetpercentage = percentage + currentPercentage; // target=new + current

                targetCtrl.SetQty(location, targetQty);
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
                BindEvents();
                AllowChanges();
            }
        }
        private void BlockChanges()
        {
            newCtrl.BlockChanges();
            currentCtrl.BlockChanges();
            targetCtrl.BlockChanges();
        }
        private void AllowChanges()
        {
            newCtrl.AllowChanges();
            currentCtrl.AllowChanges();
            targetCtrl.AllowChanges();
        }
       
        public void ClearQty()
        {
            newCtrl.ClearQty();
            currentCtrl.ClearQty();
            targetCtrl.ClearQty();
        }

        public void ClearPercentage()
        {
            newCtrl.ClearPercentage();
            currentCtrl.ClearPercentage();
            targetCtrl.ClearPercentage();
        }
       
        public AllocationLevelList GetAllocationAccounts(AllocationGroup group)
        {
            try
            {

                return newCtrl.GetAllocations(group.CumQty).GetAllocationLevelList(group.GroupID);
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
                UnBindEvents();
                BlockChanges();
                AllocationGroup newGroup = (AllocationGroup)group.Clone();
                AllocationObjColl allocationObjColl = new AllocationObjColl();
                //allocationObjColl.SetValues(newGroup.Allocations, PranaBasicBusinessLogic.IsLongSide(newGroup.OrderSideTagValue),group.State );
               
                // set current group's percentages to new group as default
                AllocationGroup currentGroup = CurrentPositionList.GetAllocationGroup(newGroup.Symbol);
                if (currentGroup.Allocations.Collection.Count != 0) // if there is some current group allocation then use it's percentage
                {
                    allocationObjColl.SetPercentageValues(currentGroup.Allocations, PranaBasicBusinessLogic.IsLongSide(newGroup.OrderSideTagValue), newGroup.State);

                    newCtrl.SetOnlyPercentageValues(allocationObjColl, newGroup.CumQty, PranaBasicBusinessLogic.IsLongSide(group.OrderSideTagValue), group.AssetID);
                }
                else // use default percentgaes 
                {
                    allocationObjColl.SetValues(newGroup.Allocations, PranaBasicBusinessLogic.IsLongSide(newGroup.OrderSideTagValue), newGroup.State);

                    newCtrl.SetAllocationAccounts(allocationObjColl, newGroup.CumQty, PranaBasicBusinessLogic.IsLongSide(group.OrderSideTagValue), group.AssetID, false);
                }
                AllocationLevelList allocationLevelList = newCtrl.GetAllocations(group.CumQty).GetAllocationLevelList(group.GroupID);
                newGroup.AddAccounts(allocationLevelList);

                if (!_multipleGroupSelected)
                {
                    AllocationObjColl allocationObjColl2 = new AllocationObjColl();
                    allocationObjColl2.SetValues(currentGroup.Allocations, PranaBasicBusinessLogic.IsLongSide(currentGroup.OrderSideTagValue), currentGroup.State);

                    currentCtrl.SetAllocationAccounts(allocationObjColl2, currentGroup.CumQty, true, currentGroup.AssetID, false);
                    AllocationGroup targetGroup = GetTagretAllocationGroup(currentGroup, newGroup);
                    AllocationObjColl allocationObjColl3 = new AllocationObjColl();
                    allocationObjColl3.SetValues(targetGroup.Allocations, PranaBasicBusinessLogic.IsLongSide(targetGroup.OrderSideTagValue), targetGroup.State);


                    targetCtrl.SetAllocationAccounts(allocationObjColl3, targetGroup.CumQty, true, targetGroup.AssetID, false);
                }
                else
                {
                    targetCtrl.SetGivenValuesToAll("N/A");
                    currentCtrl.SetGivenValuesToAll("N/A");
                }
                //targetCtrl.CalculateTotalPercentage();
                //targetCtrl.CalculateTotalQty();
                //newCtrl.RecalculateQty();
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
                BindEvents();
                AllowChanges();
            }
        }
        private AllocationGroup  GetTagretAllocationGroup(AllocationGroup currentGroup, AllocationGroup newGroup)
        {
            AllocationGroup targetGroup = new AllocationGroup();
            targetGroup.AssetID = newGroup.AssetID;
            double cumQty = 0;// PranaBasicBusinessLogic.IsLongSide(newGroup.OrderSideTagValue) ? newGroup.CumQty : -newGroup.CumQty;
           
            targetGroup.State = PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED;
            foreach (AllocationLevelClass account in newGroup.Allocations.Collection)
            {
                AllocationLevelClass newAccount = (AllocationLevelClass)account.Clone();
                newAccount.AllocatedQty = PranaBasicBusinessLogic.IsLongSide(newGroup.OrderSideTagValue) ? newAccount.AllocatedQty : -newAccount.AllocatedQty;
                targetGroup.AddAccount(newAccount);
            }

            foreach (AllocationLevelClass account in currentGroup.Allocations.Collection)
            {

                AllocationLevelClass newAccount = (AllocationLevelClass)account.Clone();
                //newAccount.AllocatedQty = PranaBasicBusinessLogic.IsLongSide(currentGroup.OrderSideTagValue) ? newAccount.AllocatedQty : -newAccount.AllocatedQty;
                targetGroup.UpdateAccounts(newAccount);
               
            }

            


            
            foreach (AllocationLevelClass account in targetGroup.Allocations.Collection)
            {
                cumQty += Math.Abs(account.AllocatedQty);
                account.Percentage = Math.Abs(Convert.ToSingle(account.AllocatedQty * 100 / cumQty));
               
            }
            targetGroup.CumQty = cumQty;
            return targetGroup;
        }
        private bool _multipleGroupSelected;

        public bool MultipleGroupSelected
        {
            get { return _multipleGroupSelected; }
            set
            {

                _multipleGroupSelected = value;
                // newCtrl.SetQtyFieldsView(!value);

            }
        }

        public void SetSelectionStatus(bool multipleSelected)
        {
            _multipleGroupSelected = multipleSelected;
            newCtrl.SetSelectionStatus(multipleSelected);
            targetCtrl.Enabled = !multipleSelected;
            //currentCtrl.Enabled = !multipleSelected;
           
            //targetCtrl.ClearQty();
           // targetCtrl.SetSelectionStatus(multipleSelected);
           // currentCtrl.SetSelectionStatus(multipleSelected);
           // HideControl(!multipleSelected);
        }

        private AllocationDefault _allocationDefault;

        public AllocationDefault AllocationDefault
        {
            get
            {

                return _allocationDefault;
            }

        }

        public void SetAllocationDefault(AllocationDefault allocationDefault)
        {
            BlockChanges();
            _allocationDefault = allocationDefault;
            AllocationObjColl allocationObjColl = new AllocationObjColl();
            allocationObjColl.SetValues(allocationDefault.DefaultAllocationLevelList, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED);
            newCtrl.SetDefaults(allocationObjColl);
            AllowChanges();
        }
        public void HideControl(bool visible)
        {
            this.Enabled = visible;
        }

        #region AllocationOperationPreference Members

        /// <summary>
        /// Set percentage value in control
        /// </summary>
        /// <param name="allocationOperationPreference"></param>
        public void SetAllocationDefault(AllocationOperationPreference allocationOperationPreference)
        {
            try
            {
                BlockChanges();
                AllocationObjColl allocationObjColl = new AllocationObjColl();
                allocationObjColl.SetValues(allocationOperationPreference.TargetPercentage, true, PostTradeConstants.ORDERSTATE_ALLOCATION.ALLOCATED, 1, -1);
                newCtrl.SetDefaults(allocationObjColl);
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
        /// Set allocation Preferences in Account control.
        /// </summary>
        /// <returns></returns>
        public SerializableDictionary<int, AccountValue> GetAllocationAccountValue()
        {
            try
            {
                SerializableDictionary<int, AccountValue> targetpercentage = new SerializableDictionary<int, AccountValue>();
                AllocationObjColl accountCol = newCtrl.GetAllocations(0);
                foreach (AllocationCtrlObject obj in accountCol.Collection)
                {
                    if (obj.ID != 0)
                    {

                        AccountValue account = new AccountValue(obj.ID, Convert.ToDecimal(obj.Percentage));
                        if (!targetpercentage.ContainsKey(account.AccountId))
                            targetpercentage.Add(account.AccountId, account);
                    else
                            targetpercentage[account.AccountId] = account;
                }
                }
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
        /// On change in control deselect any pref.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void targetCtrl_ValueChanged(object sender, EventArgs e)
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
        /// On change in control deselect any pref.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void newCtrl_ValueChanged(object sender, EventArgs e)
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
        /// <param name="group">The group.</param>
        public void SetAllocationAccounts(AllocationGroup group)
        {
            try
            {
                SetAllocationAccounts(group, false);
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
             


        public void SetQuantity(decimal p)
        {
            //as in this control this method is of no use.
        }

        #endregion
    }
}
