// ***********************************************************************
// Assembly         : Prana.AllocationNew
// Author           : Shagoon.Gurtata
// Created          : 10-03-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 02-23-2015
// ***********************************************************************
// <copyright file="CostAdjustmentControlMain.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Prana.AllocationNew.Allocation.BusinessObjects;
using Prana.BusinessObjects;
using Prana.BusinessObjects.CostAdjustment.EventArguments;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.CommonDataCache;
using Prana.BusinessObjects.AppConstants;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentControlMain.
    /// </summary>
    public partial class CostAdjustmentControlMain : UserControl
    {
        /// <summary>
        /// List of CostAdjustmentParameter
        /// </summary>
        private List<CostAdjustmentParameter> _costAdjustmentParameters = null;

        /// <summary>
        /// Occurs when [ToggleUIElements].
        /// </summary>
        public event EventHandler<EventArgs<string, bool>> ToggleUIElements;

        /// <summary>
        /// Occurs when [UpdateStatusBar].
        /// </summary>
        public event EventHandler<EventArgs<string, bool>> UpdateStatusBar;

        /// <summary>
        /// Initializes a new instance of the <see cref="CostAdjustmentControlMain"/> class.
        /// </summary>
        public event EventHandler<EventArgs<bool>> EnableDisableSaveCancelButton;

        public CostAdjustmentControlMain()
        {
            try
            {

                InitializeComponent();
                _costAdjustmentParameters = new List<CostAdjustmentParameter>();
                BindEvents();
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
        /// Binds the events.
        /// </summary>
        private void BindEvents()
        {
            try
            {
                costAdjustmentControl1.CostPreview += costAdjustmentControl1_CostPreview;
                costAdjustmentControl1.Undo += costAdjustmentControl1_Undo;
                costAdjustmentGridControl1.TotalQuantityUpdated += costAdjustmentGridControl1_TotalQuantityUpdated;
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
        /// Handles the Undo event of the costAdjustmentGridControl1 control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event</param>
        void costAdjustmentControl1_Undo(object sender, EventArgs<bool> e)
        {
            try
            {
                // If tab is Apply New, then undo preview, if tab is Applied undo saved cost adjustment
                //Call the UndoSavedCostAdjustment() and UndoCostAdjustment() method to the separate thread
                if (ultraTabControl1.ActiveTab.Index == 1)
                    new System.Threading.Tasks.Task(() => { UndoSavedCostAdjustment(); }).Start();
                else
                    new System.Threading.Tasks.Task(() => { UndoCostAdjustment(); }).Start();
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
        /// Undo saved taxlots on which cost adjustment was applied
        /// </summary>
        private void UndoSavedCostAdjustment()
        {
            try
            {

                if (this.InvokeRequired)
                {
                    MethodInvoker undoSavedCostAdjustment = delegate { UndoSavedCostAdjustment(); };
                    this.Invoke(undoSavedCostAdjustment);
                }
                else
                {
                    // called this event to disable elements on Cost Adjustment Form when undo starts
                    if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string, bool>("Undoing cost adjustment...", false));

                    // Called this event to disable elements on Allocation UI when undo starts
                    if (ToggleUIElements != null)
                        ToggleUIElements(this, new EventArgs<string, bool>("Undoing cost adjustment...", false));

                    List<CostAdjustmentTaxlotForUndo> selectedTaxlots = costAdjustmentUndoGridControl1.GetSelectedTaxlot(false);
                    List<string> caIds = new List<string>();
                    selectedTaxlots.ForEach(x => caIds.Add(x.CAID));
                    string errorMessage = string.Empty;
                    bool isUndoSuccessful = false;
                    if (selectedTaxlots == null || selectedTaxlots.Count <= 0)
                    {
                        errorMessage = " Nothing to Undo.";
                    }
                    if (selectedTaxlots != null && selectedTaxlots.Count > 0)
                    {
                        // to add confirmation popup box on undo button in applied tab in costadjustment,	PRANA-10427
                        DialogResult doUndo = MessageBox.Show(this, "Do you want to undo cost adjustment for this trade?", "Nirvana Cost Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (doUndo == DialogResult.Yes)
                            isUndoSuccessful = AllocationManager.GetInstance().AllocationServices.InnerChannel.UndoSavedCostAdjustment(caIds);
                        else
                            errorMessage = "Undo Cancelled!";
                    }

                    if (isUndoSuccessful)
                    {
                        // update data on applied tab
                        List<CostAdjustmentTaxlotForUndo> allTaxlots = costAdjustmentUndoGridControl1.GetSelectedTaxlot(true);
                        List<string> removeTaxlots = (from t in allTaxlots
                                                      where caIds.Contains(t.CAID)
                                                      select t.TaxlotId).ToList();
                        costAdjustmentUndoGridControl1.RemoveTaxlotsFromGrid(removeTaxlots);

                        List<string> removedTaxlotIDs = (from t in selectedTaxlots
                                                         from taxlot in t.CATaxlotList
                                                         select taxlot.TaxlotId).ToList();
                        List<CostAdjustmentTaxlotForUndo> costAdjustmentSavedTaxlots = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetCostAdjustmentSavedTaxlotsFromId(removedTaxlotIDs);

                        costAdjustmentUndoGridControl1.BindNewTaxlotsToGrid(costAdjustmentSavedTaxlots);

                        //MessageBox.Show(this, "Undo Cost Adjustment done sucessfully.", "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // enable UI elements on Allocation UI and show success message
                        if (ToggleUIElements != null)
                            ToggleUIElements(this, new EventArgs<string, bool>("Undo Cost Adjustment done sucessfully.", true));

                        // enable UI elements of Cost Adjustment Form and show success message
                        if (UpdateStatusBar != null)
                        UpdateStatusBar(this, new EventArgs<string, bool>("Undo Cost Adjustment done sucessfully.",true));
                    }
                    else
                    {
                        //MessageBox.Show(this, "Something went wrong, Please contact the administrator.", "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        if (string.IsNullOrEmpty(errorMessage))
                            errorMessage = "Something went wrong, Please contact the administrator.";
                        // enable UI elements on Allocation UI and show error message
                        if (ToggleUIElements != null)
                            ToggleUIElements(this, new EventArgs<string, bool>(errorMessage, true));

                        // enable UI elements of Cost Adjustment Form and show error message
                        if (UpdateStatusBar != null)
                            UpdateStatusBar(this, new EventArgs<string, bool>(errorMessage, true));
                    }
                }
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
        /// Handles the TotalQuantityUpdated event of the costAdjustmentGridControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CostAdjustmentEventArgs"/> instance containing the event data.</param>
        void costAdjustmentGridControl1_TotalQuantityUpdated(object sender, CostAdjustmentEventArgs e)
        {
            try
            {
                costAdjustmentControl1.UpdateTotalQuantity(e.TotalQty, e.TotalCost);
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
        /// Handles the CostPreview event of the costAdjustmentControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CostAdjustmentEventArgs"/> instance containing the event data.</param>
        void costAdjustmentControl1_CostPreview(object sender, EventArgs<bool> e)
        {
            try
            {
                if (!(e.Value))
                {
                    SaveCostAdjustment();
                }
                else
                {

                    string errorMessage = string.Empty;
                    List<CostAdjustmentParameter> parameterList = new List<CostAdjustmentParameter>();
                    // CostAdjustmentParameter parameter = new CostAdjustmentParameter();
                    CostAdjustmentParameter parameter = costAdjustmentControl1.GetAdjustmentParameter();
                    parameter.UserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                    parameter.Taxlots = costAdjustmentGridControl1.GetSelectedTaxlot(out errorMessage);
                    if (parameter.Taxlots == null || parameter.Taxlots.Count <= 0)
                    {
                        if (errorMessage == string.Empty)
                        {
                            errorMessage = "Please select a taxlot for cost adjustment preview/save.";
                        }
                        MessageBox.Show(this, errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    parameter.isPreview = e.Value;

                    if (!parameter.IsValid(out errorMessage))
                    {
                        MessageBox.Show(this, errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    else
                    {
                        // Passed parameter list to AdjustCost function.
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
                        parameterList.Add(parameter);
                        CostAdjustmentResult result = AllocationManager.GetInstance().AllocationServices.InnerChannel.AdjustCost(parameterList);
                        if (String.IsNullOrEmpty(result.Error))
                        {
                            _costAdjustmentParameters.Add(parameter);
                            costAdjustmentGridControl1.UpdateDataInGrid(result.AdjustedTaxlots, true);
                            costAdjustmentControl1.UpdateTotalQuantity(costAdjustmentGridControl1.TotalQuantity, costAdjustmentGridControl1.TotalCost);
                            costAdjustmentControl1.EnableDisableUndoButton(true);
                        }
                        else
                        {
                            MessageBox.Show(this, result.Error, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            costAdjustmentControl1.ResetValues();
                            costAdjustmentGridControl1.ResetValues();
                        }
                    }
                }
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
        /// Binds the data.
        /// </summary>
        /// <param name="openTaxlotsBindingList">The binding list.</param>
        internal void BindData(List<CostAdjustmentTaxlot> openTaxlotsBindingList, List<CostAdjustmentTaxlotForUndo> costAdjustmentTaxlotsBindingList)
        {
            try
            {
                costAdjustmentGridControl1.BindDataSourceToGrid(openTaxlotsBindingList);
                // Bind data to new grid which shows cost adjustment saved taxlots
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                costAdjustmentUndoGridControl1.BindDataSourceToGrid(costAdjustmentTaxlotsBindingList);
                costAdjustmentGridControl1.ResetValues();
                costAdjustmentControl1.ResetValues();
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
        /// Saves the cost adjustment.
        /// </summary>
        /// <param name="actionAfterSaving">action after saving changes, CloseAllocation in case user is saving changes on close click</param>
        internal void SaveCostAdjustment(string actionAfterSaving = "")
        {
            // called this event to disable elements on Cost Adjustment Form when save starts
            if (UpdateStatusBar != null)
                UpdateStatusBar(this, new EventArgs<string, bool>("Saving cost adjustment changes...", false));

            // Created background worker to save cost adjustment data
            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7227
            BackgroundWorker saveCostAdjustment = new BackgroundWorker();
            saveCostAdjustment.DoWork += new DoWorkEventHandler(saveCostAdjustment_DoWork);
            saveCostAdjustment.RunWorkerCompleted += new RunWorkerCompletedEventHandler(saveCostAdjustment_RunWorkerCompleted);
            saveCostAdjustment.RunWorkerAsync(actionAfterSaving);

            #region commentedCode

            //try
            //{
            //    CostAdjustmentResult result = new CostAdjustmentResult();
            //    string errorString = string.Empty;
            //    string errorMessage = string.Empty;
            //    CostAdjustmentParameter parameter = costAdjustmentControl1.GetAdjustmentParameter();
            //    parameter.UserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
            //    parameter.Taxlots = costAdjustmentGridControl1.GetSelectedTaxlot(out errorMessage);
            //    if ((parameter.Taxlots == null || parameter.Taxlots.Count <= 0) && _costAdjustmentParameters.Count == 0)
            //    {
            //        if (errorMessage == string.Empty)
            //        {
            //            errorMessage = "Please select a taxlot to save.";
            //        }
            //        MessageBox.Show(this,errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }


            //    if (parameter.Taxlots != null && parameter.Taxlots.Count > 0)
            //    {
            //        parameter.isPreview = false;
            //        if (!parameter.IsValid(out errorMessage))
            //        {
            //            MessageBox.Show(this, errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //        else
            //        {
            //            _costAdjustmentParameters.Add(parameter);
            //        }
            //    }

            //    if (_costAdjustmentParameters.Count > 0)
            //    {
            //        _costAdjustmentParameters.ForEach(x => x.isPreview = false);
            //        result = AllocationManager.GetInstance().AllocationServices.InnerChannel.AdjustCost(_costAdjustmentParameters);
            //        if (!(String.IsNullOrEmpty(result.Error)))
            //            errorString = errorString + result.Error;

            //        //foreach (CostAdjustmentParameter param in _costAdjustmentParameters)
            //        //{
            //        //    CostAdjustmentResult result = AllocationManager.GetInstance().AllocationServices.InnerChannel.AdjustCost(param);
            //        //    resultList.Add(result);
            //        //    if (!(String.IsNullOrEmpty(result.Error)))
            //        //        errorString = errorString + result.Error;
            //        //}
            //    }
            //    if (String.IsNullOrEmpty(errorString))
            //    {
            //        costAdjustmentGridControl1.UpdateDataInGridOnSave(result.OriginalTaxlotList);
            //        //Adding Adjusted Addition Taxlot to Grid
            //        AddTaxlots(result.AdjustedAdditionTaxlotList);
            //        costAdjustmentControl1.UpdateTotalQuantity(costAdjustmentGridControl1.TotalQuantity, costAdjustmentGridControl1.TotalCost);

            //        for (int i = 0; i <= _costAdjustmentParameters.Count; i++)
            //        {
            //            CostAdjustmentParameter removeParameter = _costAdjustmentParameters.Last();
            //            _costAdjustmentParameters.Remove(removeParameter);
            //        }
            //        MessageBox.Show(this,"Changes saved successfully.", "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        costAdjustmentControl1.EnableDisableUndoButton(false);
            //    }
            //    else
            //    {
            //        MessageBox.Show(this, errorString, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);                    
            //    }
            //    costAdjustmentControl1.ResetValues();
            //    costAdjustmentGridControl1.ResetValues();
            //}
            //catch (Exception ex)
            //{

            //    bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
            //    if (rethrow)
            //    {
            //        throw;
            //    }
            //}

            #endregion
        }

        /// <summary>
        /// This function saves the cost adjustment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void saveCostAdjustment_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                CostAdjustmentResult result = new CostAdjustmentResult();
                string errorMessage = string.Empty;
                CostAdjustmentParameter parameter = costAdjustmentControl1.GetAdjustmentParameter();
                parameter.UserId = CommonDataCache.CachedDataManager.GetInstance.LoggedInUser.CompanyUserID;
                parameter.Taxlots = costAdjustmentGridControl1.GetSelectedTaxlot(out errorMessage);
                if ((parameter.Taxlots == null || parameter.Taxlots.Count <= 0) && _costAdjustmentParameters.Count == 0)
                {
                    if (errorMessage == string.Empty)
                    {
                        errorMessage = "Nothing to save.";
                    }
                    //MessageBox.Show(this.FindForm(), errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //return;
                    result.Error = errorMessage;
                }


                if (parameter.Taxlots != null && parameter.Taxlots.Count > 0)
                {
                    parameter.isPreview = false;
                    if (!parameter.IsValid(out errorMessage))
                    {
                        //MessageBox.Show(this, errorMessage, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //return;
                        result.Error = errorMessage;
                    }
                    else
                    {
                        _costAdjustmentParameters.Add(parameter);
                    }
                }

                if (_costAdjustmentParameters.Count > 0 && string.IsNullOrEmpty(result.Error))
                {
                    _costAdjustmentParameters.ForEach(x => x.isPreview = false);
                    result = AllocationManager.GetInstance().AllocationServices.InnerChannel.AdjustCost(_costAdjustmentParameters);
                }
                object[] parameters = new object[] { e.Argument, result };
                e.Result = parameters;
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
        /// This function performs action on result returned after saving cost adjustment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void saveCostAdjustment_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // if control has been disposed then there is no need to do perform any action on result
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7414
                if (!this.IsDisposed && !this.Disposing)
                {
                    if ((e.Cancelled == true))
                    {
                        //MessageBox.Show(this, "Cancelled!", "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Show cancelled status on allocation UI status bar if event is cancelled
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                        if (ToggleUIElements != null)
                            ToggleUIElements(this, new EventArgs<string, bool>("Cancelled!", true));
                    }
                    else
                    {
                        object[] parameters = e.Result as object[];
                        string actionAfterSave = parameters[0].ToString();
                        CostAdjustmentResult result = new CostAdjustmentResult();
                        result = (CostAdjustmentResult)parameters[1];
                        if (String.IsNullOrEmpty(result.Error))
                        {
                            costAdjustmentGridControl1.UpdateDataInGridOnSave(result.OriginalTaxlotList);
                            //Adding Adjusted Addition Taxlot to Grid
                            AddTaxlots(result.AdjustedAdditionTaxlotList);
                            costAdjustmentControl1.UpdateTotalQuantity(costAdjustmentGridControl1.TotalQuantity, costAdjustmentGridControl1.TotalCost);

                            for (int i = 0; i <= _costAdjustmentParameters.Count; i++)
                            {
                                CostAdjustmentParameter removeParameter = _costAdjustmentParameters.Last();
                                _costAdjustmentParameters.Remove(removeParameter);
                            }
                            // Commenting this line as saved message is shown on label strip and popup message is not needed
                            // http://jira.nirvanasolutions.com:8080/browse/PRANA-7414
                            //MessageBox.Show(this, "Changes saved successfully.", "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Called this event to enable elements on Allocation UI after save
                            if (ToggleUIElements != null)
                                ToggleUIElements(this, new EventArgs<string, bool>("Cost Adjustment data saved successfully", true));
                            costAdjustmentControl1.EnableDisableUndoButton(false);

                            // update status bar of Cost Adjustment Form
                            if (UpdateStatusBar != null)
                                UpdateStatusBar(this, new EventArgs<string, bool>("Cost Adjustment data saved successfully", true));

                            // Bind data to applied tab
                            List<string> originalTaxlots = (from t in result.OriginalTaxlotList
                                                            select t.TaxLotID).ToList();
                            costAdjustmentUndoGridControl1.RemoveTaxlotsFromGrid(originalTaxlots);

                            List<CostAdjustmentTaxlot> openTaxlots = new List<CostAdjustmentTaxlot>();
                            openTaxlots.AddRange(result.AdjustedAdditionTaxlotList.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                            List<CostAdjustmentTaxlotForUndo> addTaxlotsList = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetCostAdjustmentSavedTaxlots(openTaxlots, result.SavedTaxlotsData);
                            costAdjustmentUndoGridControl1.BindNewTaxlotsToGrid(addTaxlotsList);
                        }
                        else
                        {
                            //MessageBox.Show(this, result.Error, "Nirvana Cost Adjustment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // Called this event to enable elements on Allocation UI after save
                            if (ToggleUIElements != null)
                                ToggleUIElements(this, new EventArgs<string, bool>(result.Error, true));
                            // update status bar of Cost Adjustment Form and enable UI elements
                            if (UpdateStatusBar != null)
                                UpdateStatusBar(this, new EventArgs<string, bool>(result.Error, true));
                        }
                        costAdjustmentControl1.ResetValues();
                        costAdjustmentGridControl1.ResetValues();

                        // checks if actionAfterSave is CloseAllocation, then close allocation UI after saving changes
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                        if (actionAfterSave == "CloseAllocation")
                        {
                            this.FindForm().Close();
                        }
                    }
                }
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
        /// Resets cost adjustment grid
        /// </summary>
        internal void ResetCostAdjustmentGrid()
        {
            try
            {
                if (_costAdjustmentParameters != null || _costAdjustmentParameters.Count > 0)
                {
                    for (int i = 0; i <= _costAdjustmentParameters.Count; i++)
                    {
                        UndoCostAdjustment();
                    }
                }
                // Called this event to enable elements on Allocation UI after reset
                if (ToggleUIElements != null)
                    ToggleUIElements(this, new EventArgs<string, bool>("Cost Adjustment changes have been reverted.", true));

                // Called this event to enable elements on Cost Adjustment Form after reset
                if (UpdateStatusBar != null)
                    UpdateStatusBar(this, new EventArgs<string, bool>("Cost Adjustment changes have been reverted.", true));
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
        /// This function performs undo on Cost Adjustment
        /// </summary>
        internal void UndoCostAdjustment()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker undoCostAdjustment = delegate { UndoCostAdjustment(); };
                    this.Invoke(undoCostAdjustment);
                }
                else
                {
                if (_costAdjustmentParameters != null || _costAdjustmentParameters.Count > 0)
                {
                    CostAdjustmentParameter parameter = _costAdjustmentParameters.Last();
                    costAdjustmentGridControl1.UpdateDataInGrid(parameter.Taxlots, false);
                    _costAdjustmentParameters.Remove(parameter);
                }
                if (_costAdjustmentParameters.Count == 0)
                {
                    costAdjustmentControl1.EnableDisableUndoButton(false);
                }
                costAdjustmentControl1.ResetValues();
                costAdjustmentGridControl1.ResetValues();
            }
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
        /// Checks if anything is changed on cost adjustment tab
        /// </summary>
        /// <returns>Returns true/false</returns>
        public bool IsAnythingChanged()
        {
            try
            {
                if (_costAdjustmentParameters == null || _costAdjustmentParameters.Count <= 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }

                return false;
            }
        }

        /// <summary>
        /// Sets visibility of Save button on cost adjustment
        /// </summary>
        /// <param name="showButton">bool value to set visibility</param>
        internal void ShowSaveButton(bool showButton)
        {
            try
            {
                this.costAdjustmentControl1.ShowSaveButton(showButton);
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
        /// Unbinds the events
        /// </summary>
        private void UnBindEvents()
        {
            try
            {
                costAdjustmentControl1.CostPreview -= costAdjustmentControl1_CostPreview;
                costAdjustmentControl1.Undo -= costAdjustmentControl1_Undo;
                costAdjustmentGridControl1.TotalQuantityUpdated -= costAdjustmentGridControl1_TotalQuantityUpdated;
                //_costAdjustmentParameters = null;
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
        /// Add new taxlots to Cost Adjustment grid
        /// </summary>
        /// <param name="list">Taxlot list</param>
        internal void AddTaxlots(List<TaxLot> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    List<CostAdjustmentTaxlot> newTaxlots = new List<CostAdjustmentTaxlot>();
                    newTaxlots.AddRange(list.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                    costAdjustmentGridControl1.BindTaxlotsToGrid(newTaxlots, true);
                }
                costAdjustmentControl1.ResetValues();
                costAdjustmentGridControl1.ResetValues();
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
        /// Remove taxlots from Cost Adjustment grid
        /// </summary>
        /// <param name="list">Taxlot list</param>
        internal void RemoveTaxlots(List<TaxLot> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    List<CostAdjustmentTaxlot> newTaxlots = new List<CostAdjustmentTaxlot>();
                    newTaxlots.AddRange(list.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                    costAdjustmentGridControl1.BindTaxlotsToGrid(newTaxlots, false);
                }
                costAdjustmentControl1.ResetValues();
                costAdjustmentGridControl1.ResetValues();
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
        /// Update taxlots on Cost Adjustment grid
        /// </summary>
        /// <param name="list">Taxlot list</param>
        internal void UpdateTaxlots(List<TaxLot> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    List<CostAdjustmentTaxlot> newTaxlots = new List<CostAdjustmentTaxlot>();
                    newTaxlots.AddRange(list.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                    costAdjustmentGridControl1.UpdateTaxlots(newTaxlots);
                }
                costAdjustmentControl1.ResetValues();
                costAdjustmentGridControl1.ResetValues();
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
        /// Remove taxlots from Cost Adjustment gid on the basis of group id
        /// </summary>
        /// <param name="list">Taxlot list</param>
        internal void RemoveTaxlotsByGroupID(List<TaxLot> list)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    List<CostAdjustmentTaxlot> removeTaxlots = new List<CostAdjustmentTaxlot>();
                    removeTaxlots.AddRange(list.Select(t => CostAdjustmentTaxlot.GetTaxlot(t)));
                    costAdjustmentGridControl1.RemoveTaxlotsByGroupID(removeTaxlots);
                }
                costAdjustmentControl1.ResetValues();
                costAdjustmentGridControl1.ResetValues();
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
        /// Enables/Diables UI elements on tab change
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event</param>
        private void ultraTabControl1_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            try
            {
                int tabIndex = e.Tab.Index;
                if (tabIndex == 1)
                {
                    costAdjustmentControl1.EnableDisableUIControls(false);
                    costAdjustmentControl1.EnableDisableUndoButton(true);
                    //Bind Taxlot to Applied Tab
                    BindTaxlotToAppliedTab();
                    // event raised to disable save and cancel button on active tab
                    if(EnableDisableSaveCancelButton != null)
                        EnableDisableSaveCancelButton(this, new EventArgs<bool>(false));
                }
                else
                {
                    costAdjustmentControl1.EnableDisableUIControls(true);
                    if(IsAnythingChanged())
                        costAdjustmentControl1.EnableDisableUndoButton(true);
                    else
                        costAdjustmentControl1.EnableDisableUndoButton(false);

                    if (EnableDisableSaveCancelButton != null)
                        EnableDisableSaveCancelButton(this,new EventArgs<bool>(true));
                }
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
        /// Bind Taxlot to Applied Tab
        /// </summary>
        private void BindTaxlotToAppliedTab()
        {
            try
            {
                List<AllocationGroup> groups = AllocationManager.GetInstance().AllocatedGroups.ToList();
                List<string> openTaxlotIds = new List<string>();
                openTaxlotIds.AddRange((from g in groups
                                        from t in g.TaxLots
                                        where t.ClosingStatus != ClosingStatus.Closed && g.ClosingStatus != ClosingStatus.Closed
                                        select t.TaxLotID).ToList());

                List<CostAdjustmentTaxlotForUndo> costAdjustmentSavedTaxlots = AllocationManager.GetInstance().AllocationServices.InnerChannel.GetCostAdjustmentSavedTaxlotsFromId(openTaxlotIds);
                costAdjustmentUndoGridControl1.BindDataSourceToGrid(costAdjustmentSavedTaxlots);
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
        /// Selecte Applied Tab by Default
        /// </summary>
        internal void SelectedTab()
        {
            try
            {
                ultraTabControl1.Tabs[0].Selected = true;
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
        /// to check the active tab on costadjustment grid
        /// </summary>
        /// <returns>index numner of the active tab</returns>
        internal int checkActiveTab()
        {
            try
            {
                return ultraTabControl1.ActiveTab.Index;
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
    }
}
