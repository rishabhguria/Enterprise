using Prana.BusinessObjects.Classes.SecurityMasterBusinessObjects;
using Prana.Global;
using Prana.LogManager;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Prana.Tools.PL
{
    public partial class DynamicUDAForm : Form
    {
        /// <summary>
        /// To save dynamic UDA
        /// </summary>
        public event EventHandler<EventArgs<DynamicUDA, string>> SaveDynamicUDA;

        /// <summary>
        /// To check the master value is assgned to symbol or not
        /// </summary>
        public event EventHandler<EventArgs<string, string>> CheckMasterValueAssigned;

        /// <summary>
        /// Default Constructor of Dynamic UDA Form
        /// </summary>
        public DynamicUDAForm()
        {
            try
            {
                InitializeComponent();
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

        /// <summary>
        /// Dynamic UDA Form COnstructor that binding the dynamic UDA to the grid
        /// </summary>
        /// <param name="cache">Cache of dynamic UDA names</param>
        /// <param name="otherExistingColumns">List of symbol lookup column names</param>
        public DynamicUDAForm(Dictionary<string, DynamicUDA> cache, List<string> otherExistingColumns)
        {
            try
            {
                InitializeComponent();
                dynamicUserControl1.BindGrid(cache, otherExistingColumns);
                dynamicUserControl1.SaveDynamicUDA += dynamicUserControl1_SaveDynamicUDA;
                dynamicUserControl1.CheckMasterValueAssigned += dynamicUserControl1_CheckMasterValueAssigned;
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
        /// <summary>
        /// To check the master value is assgned to symbol or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dynamicUserControl1_CheckMasterValueAssigned(object sender, EventArgs<string, string> e)
        {
            try
            {
                if (CheckMasterValueAssigned != null)
                    CheckMasterValueAssigned(this, e);
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
        /// <summary>
        /// Saving the Dynamic UDA
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dynamicUserControl1_SaveDynamicUDA(object sender, EventArgs<DynamicUDA, string> e)
        {
            try
            {
                if (SaveDynamicUDA != null)
                    SaveDynamicUDA(this, e);
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

        /// <summary>
        /// Updating the grid
        /// </summary>
        /// <param name="dynamicUDA"></param>
        internal void UpdateDynamicUDAGrid(DynamicUDA dynamicUDA)
        {
            try
            {
                dynamicUserControl1.UpdateDynamicUDAGrid(dynamicUDA);
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

        /// <summary>
        /// Unwire Events
        /// </summary>
        private void UnwireEvents()
        {
            try
            {
                if (SaveDynamicUDA != null)
                    dynamicUserControl1.SaveDynamicUDA -= dynamicUserControl1_SaveDynamicUDA;

                if (CheckMasterValueAssigned != null)
                    dynamicUserControl1.CheckMasterValueAssigned -= dynamicUserControl1_CheckMasterValueAssigned;
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

        /// <summary>
        /// Delete the master value of user choice
        /// </summary>
        /// <param name="result"></param>
        internal void DeleteListViewMasterValue(bool result)
        {
            try
            {
                dynamicUserControl1.DeleteListViewMasterValue(result);
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
    }
}
