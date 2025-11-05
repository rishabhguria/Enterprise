// ***********************************************************************
// Assembly         : Prana.AllocationNew
// Author           : Shagoon.Gurtata
// Created          : 10-03-2014
//
// Last Modified By : Shagoon.Gurtata
// Last Modified On : 02-23-2015
// ***********************************************************************
// <copyright file="CostAdjustmentControl.cs" company="Microsoft">
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
using Prana.BusinessObjects;
using Prana.BusinessObjects.AppConstants;
using Prana.BusinessObjects.CostAdjustment.Delegates;
using Prana.BusinessObjects.CostAdjustment.Enums;
using Prana.BusinessObjects.CostAdjustment.EventArguments;
using Prana.BusinessObjects.CostAdjustment.Definitions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prana.Global;
using Prana.Utilities.MiscUtilities;
using Prana.Utilities.UIUtilities;

/// <summary>
/// The CostAdjustment namespace.
/// </summary>
namespace Prana.AllocationNew.Allocation.UI.CostAdjustment
{
    /// <summary>
    /// Class CostAdjustmentControl.
    /// </summary>
    public partial class CostAdjustmentControl : UserControl
    {
        /// <summary>
        /// Occurs when [cost preview].
        /// </summary>
        public event EventHandler<EventArgs<bool>> CostPreview;

        /// <summary>
        /// Occurs when [Undo]
        /// </summary>
        public event EventHandler<EventArgs<bool>> Undo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CostAdjustmentControl"/> class.
        /// </summary>
        public CostAdjustmentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the CostAdjustmentControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CostAdjustmentControl_Load(object sender, EventArgs e)
        {
            try
            {
                //Binding methods to combo.
                ultraCmbMethodology.DataSource = EnumHelper.ToList(typeof(CostAdjustmentMethodology));
                ultraCmbMethodology.DisplayMember = "Value";
                ultraCmbMethodology.ValueMember = "Key";

                ultraCmbAdjstType.SelectedIndex = 0;
                ultraCmbMethodology.SelectedIndex = 0;
                ultraCmbCost.SelectedIndex = 0;
                if (!CustomThemeHelper.IsDesignMode() && CustomThemeHelper.WHITELABELTHEME.Equals("Nirvana"))
                {
                    SetButtonsColor();
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

        private void SetButtonsColor()
        {
            try
            {
                ultraBtnSave.BackColor = System.Drawing.Color.FromArgb(104, 156, 46);
                ultraBtnSave.ForeColor = System.Drawing.Color.White;
                ultraBtnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnSave.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnSave.UseAppStyling = false;
                ultraBtnSave.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnUndo.BackColor = System.Drawing.Color.FromArgb(55,67,85);
                ultraBtnUndo.ForeColor = System.Drawing.Color.White;
                ultraBtnUndo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnUndo.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnUndo.UseAppStyling = false;
                ultraBtnUndo.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;

                ultraBtnPreview.BackColor = System.Drawing.Color.FromArgb(55, 67, 85);
                ultraBtnPreview.ForeColor = System.Drawing.Color.White;
                ultraBtnPreview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                ultraBtnPreview.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Button3D;
                ultraBtnPreview.UseAppStyling = false;
                ultraBtnPreview.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            }
            catch (Exception ex)
            {
                // Invoke our policy that is responsible for making sure no secure information
                // gets out of our layer.
                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the ultraBtnPreview control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ultraBtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (CostPreview != null)
                    CostPreview(this, new EventArgs<bool>(true));

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
        /// Updates the total quantity.
        /// </summary>
        /// <param name="totalQty">The total qty.</param>
        /// <param name="totalCost">The total cost.</param>
        internal void UpdateTotalQuantity(decimal totalQty, decimal totalCost)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker updateQuantity = delegate { UpdateTotalQuantity(totalQty, totalCost); };
                    this.Invoke(updateQuantity);
                }
                else
                {
                  ultraTxtQty.Value = totalQty;
                // Showing absolute data for Total Open Quantity to Adjust
                  ultraTxtQty.Text = Math.Abs(totalQty).ToString();
                
                //ultraTxtQty.Text = totalQty.ToString();
                ultraTxtOpenQty.Value = totalQty;

                // Showing absolute data for Quantity to Adjust
                ultraTxtOpenQty.Text = Math.Abs(totalQty).ToString();
                
                //ultraTxtOpenQty.Text = totalQty.ToString();
                ultraTxtCost.Value = totalCost;
                ultraTxtCost.Text = totalCost.ToString();
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
        /// Handles the ValueChanged event of the ultraCmbAdjstType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ultraCmbAdjstType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (ultraCmbAdjstType.SelectedItem.Tag.ToString() == CostAdjustmentType.Total.ToString())
                {
                    ultraCmbCost.Visible = true;
                    ultraLblUnitCost.Visible = false;
                }
                else if (ultraCmbAdjstType.SelectedItem.Tag.ToString() == CostAdjustmentType.Unit.ToString())
                {
                    ultraCmbCost.Visible = false;
                    ultraLblUnitCost.Visible = true;
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
        /// Handles the Click event of the ultraBtnUndo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ultraBtnUndo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Undo != null)
                {
                    Undo(this, new EventArgs<bool>(true));
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
        /// Gets the adjustment parameter.
        /// </summary>
        /// <returns>CostAdjustmentParameter.</returns>
        internal CostAdjustmentParameter GetAdjustmentParameter()
        {
            try
            {
                CostAdjustmentParameter parameter = new CostAdjustmentParameter();
                decimal cost = Convert.ToDecimal(ultraTxtAdjustCost.Text);
                CostAdjustmentType type = (CostAdjustmentType)Enum.Parse(typeof(CostAdjustmentType), ultraCmbAdjstType.SelectedItem.Tag.ToString());
                if (type == CostAdjustmentType.Total && ultraCmbCost.SelectedItem.Tag.ToString() == "Reduce")
                {
                    cost = cost * -1;
                }
                parameter.AdjustCost = cost;
                parameter.AdjustQty = Convert.ToDecimal(ultraTxtQty.Text);
                parameter.Type = type;
                parameter.TotalQuantity = Convert.ToDecimal(ultraTxtOpenQty.Text);
                parameter.TotalCost = Convert.ToDecimal(ultraTxtCost.Text);
                parameter.CostAdjustmentMethod = (CostAdjustmentMethodology)ultraCmbMethodology.Value;
                return parameter;
            }
            catch (Exception ex)
            {

                bool rethrow = ExceptionPolicy.HandleException(ex, ApplicationConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                {
                    throw;
                }
                return null;
            }
        }

        /// <summary>
        /// Handles the Click event of the ultraBtnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void ultraBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (CostPreview != null)
                    CostPreview(this, new EventArgs<bool>(false));
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
                this.ultraBtnSave.Visible = showButton;
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
        /// Enables/disables Undo button on cost adjustment
        /// </summary>
        /// <param name="enableButton">bool value to enable/diable</param>
        internal void EnableDisableUndoButton(bool enableButton)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    MethodInvoker enableDisableButton = delegate { EnableDisableUndoButton(enableButton); };
                    this.Invoke(enableDisableButton);
                }
                else
                {
                this.ultraBtnUndo.Enabled = enableButton;
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
        /// Resets value to 0
        /// </summary>
        internal void ResetValues()
        {
            try
            {
                ultraTxtAdjustCost.Value = 0;
                ultraTxtCost.Value = 0;
                ultraTxtOpenQty.Value = 0;
                ultraTxtQty.Value = 0;
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
        /// Enable/Disable UI elements on cost adjustment control tab
        /// </summary>
        /// <param name="value">Value to be set to UI elemnts</param>
        internal void EnableDisableUIControls(bool value)
        {
            try
            {
                ultraBtnPreview.Enabled = value;
                ultraBtnSave.Enabled = value;
                ultraCmbAdjstType.Enabled = value;
                ultraCmbCost.Enabled = value;
                ultraTxtAdjustCost.Enabled = value;
                ultraTxtCost.Enabled = value;
                ultraTxtOpenQty.Enabled = value;
                ultraTxtQty.Enabled = value;
                ultraCmbMethodology.Enabled = value;
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
    }
}
