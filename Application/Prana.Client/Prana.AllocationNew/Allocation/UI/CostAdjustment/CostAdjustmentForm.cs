using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
// ***********************************************************************
// Assembly         : Prana.AllocationNew
// Author           : Shagoon.Gurtata
// Created          : 02-23-2015
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 02-23-2015
// ***********************************************************************
// <copyright file="CostAdjustmentForm.cs" company="Microsoft">
//     Copyright (c) Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentForm.
    /// </summary>
    public partial class CostAdjustmentForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CostAdjustmentForm"/> class.
        /// </summary>
        public CostAdjustmentForm()
        {
            InitializeComponent();
            BindEvents();
        }

        /// <summary>
        /// Bind Events
        /// </summary>
        private void BindEvents()
        {
            try
            {
                costAdjustmentControlMain1.UpdateStatusBar += costAdjustmentControlMain1_UpdateStatusBar;
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
        /// Update message on status bar
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the event</param>
        private void costAdjustmentControlMain1_UpdateStatusBar(object sender, EventArgs<string, bool> e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker updateStatusBar = delegate { costAdjustmentControlMain1_UpdateStatusBar(sender,e); };
                    this.Invoke(updateStatusBar);
                }
                else
                {
                ultraStatusBar1.Text = e.Value;
                ToggleUIElements(e.Value2);
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
        /// Enable/Disable UI controls
        /// </summary>
        /// <param name="status">status for UI elements</param>
        private void ToggleUIElements(bool status)
        {
            try
            {
                costAdjustmentControlMain1.Enabled = status;
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
        /// <param name="openTaxlotsList">The list.</param>
        internal void BindData(List<Prana.BusinessObjects.CostAdjustment.Definitions.CostAdjustmentTaxlot> openTaxlotsList, List<Prana.BusinessObjects.CostAdjustment.Definitions.CostAdjustmentTaxlotForUndo> costAdjustmentTaxlotList)
        {
            try
            {
                // Modified to bind data to new grid which shows cost adjustment saved taxlots
                // http://jira.nirvanasolutions.com:8080/browse/PRANA-7378
                this.costAdjustmentControlMain1.BindData(openTaxlotsList, costAdjustmentTaxlotList);
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
        /// Sets visibility of Save button on cost adjustment
        /// </summary>
        /// <param name="showButton">bool value to set visibility</param>
        internal void ShowSaveButton(bool showButton)
        {
            try
            {
                this.costAdjustmentControlMain1.ShowSaveButton(showButton);
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
                costAdjustmentControlMain1.UpdateStatusBar -= costAdjustmentControlMain1_UpdateStatusBar;
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
        /// Form CLosing Event to Check for changes in Cost Adjustment tab, pop up message box will display to know user response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CostAdjustmentForm_FormClosing(object sender, FormClosingEventArgs e) 
        {
            try
            {
                if (costAdjustmentControlMain1.IsAnythingChanged())
                {
                    DialogResult currentUserChoice = MessageBox.Show(this, "Would you like to save Cost Adjustment changes?", "Nirvana Cost Adjustment", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (currentUserChoice == DialogResult.Yes)
                    {
                        // passing CloseAllocation string so that form is closed after save
                        // http://jira.nirvanasolutions.com:8080/browse/PRANA-8204
                        string actionAfterSave = "CloseAllocation";
                        costAdjustmentControlMain1.SaveCostAdjustment(actionAfterSave);
                    }
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
    }
}
