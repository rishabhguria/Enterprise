using Prana.Allocation.Client.Constants;
using Prana.Global;
using Prana.LogManager;
using Prana.MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;

namespace Prana.Allocation.Client.Controls.Allocation.ViewModels
{
    internal class ExternalTransactionIdViewModel : ViewModelBase, IModalDialogViewModel
    {
        #region Events

        /// <summary>
        /// Occurs when [close external transaction].
        /// </summary>
        internal event EventHandler CloseExternalTransaction;

        /// <summary>
        /// Occurs when [update external transaction identifier].
        /// </summary>
        internal event EventHandler<EventArgs<string>> UpdateExternalTransactionID;

        #endregion Events

        #region Members

        /// <summary>
        /// The _external transaction identifier table
        /// </summary>
        private DataTable _externalTransactionTable;

        /// <summary>
        /// The bring to front
        /// </summary>
        private WindowState _bringToFront;

        #endregion Members

        #region Properties

        /// <summary>
        /// Gets or sets the external transaction identifier table.
        /// </summary>
        /// <value>
        /// The external transaction identifier table.
        /// </value>
        public DataTable ExternalTransactionTable
        {
            get { return _externalTransactionTable; }
            set
            {
                _externalTransactionTable = value;
                RaisePropertyChangedEvent("ExternalTransactionTable");
            }
        }

        /// <summary>
        /// Gets or sets the bring to front.
        /// </summary>
        /// <value>
        /// The bring to front.
        /// </value>
        public WindowState BringToFront
        {
            get { return _bringToFront; }
            set
            {
                if (_bringToFront == WindowState.Minimized)
                    _bringToFront = value;
                else
                {
                    if (value == WindowState.Minimized)
                        _bringToFront = value;
                    else
                    {
                        WindowState currentState = _bringToFront;
                        _bringToFront = WindowState.Minimized;
                        RaisePropertyChangedEvent("BringToFront");
                        _bringToFront = currentState;
                    }
                }
                RaisePropertyChangedEvent("BringToFront");
            }
        }

        #endregion Properties

        #region Commands

        /// <summary>
        /// Gets or sets the close external transaction UI.
        /// </summary>
        /// <value>
        /// The close external transaction UI.
        /// </value>
        public RelayCommand<object> CloseExternalTransactionUi { get; set; }

        /// <summary>
        /// Gets or sets the form close button.
        /// </summary>
        /// <value>
        /// The form close button.
        /// </value>
        public RelayCommand<object> FormCloseButton { get; set; }

        /// <summary>
        /// Gets or sets the ok button external transaction UI.
        /// </summary>
        /// <value>
        /// The ok button external transaction UI.
        /// </value>
        public RelayCommand<object> OkButtonExternalTransactionUi { get; set; }

        #endregion Commands

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ExternalTransactionIdViewModel"/> class from being created.
        /// </summary>
        internal ExternalTransactionIdViewModel()
        {
            try
            {
                CloseExternalTransactionUi = new RelayCommand<object>((parameter) => OnCloseExternalTransactionUi(parameter));
                OkButtonExternalTransactionUi = new RelayCommand<object>((parameter) => OnOkClickExternalTransactionUi(parameter));
                FormCloseButton = new RelayCommand<object>((parameter) => OnCloseButton(parameter));
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Called when [close button].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseButton(object parameter)
        {
            try
            {
                if (CloseExternalTransaction != null)
                    CloseExternalTransaction(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDTHROW);
                if (rethrow)
                    throw;
            }
            return null;
        }

        /// <summary>
        /// Called when [close external transaction UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnCloseExternalTransactionUi(object parameter)
        {
            try
            {
                Window externalTransactionUi = parameter as Window;
                if (externalTransactionUi != null)
                    externalTransactionUi.Close();

                if (CloseExternalTransaction != null)
                    CloseExternalTransaction(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        /// <summary>
        /// Called when [ok click external transaction UI].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        private object OnOkClickExternalTransactionUi(object parameter)
        {
            try
            {
                List<string> externalIdList = new List<string>();
                if (ExternalTransactionTable != null && ExternalTransactionTable.Rows.Count > 0)
                {
                    foreach (DataRow row in ExternalTransactionTable.Rows)
                    {
                        if (!string.IsNullOrEmpty(row[AllocationUIConstants.EXTERNAL_TRANSACTION_ID].ToString()))
                        {
                            externalIdList.Add(row[AllocationUIConstants.STRATEGY_ID] + ":" + row[AllocationUIConstants.EXTERNAL_TRANSACTION_ID]);

                        }
                    }
                    if (UpdateExternalTransactionID != null)
                        UpdateExternalTransactionID(this, new EventArgs<string>(String.Join(",", externalIdList.Select(x => x.ToString()).ToArray())));
                }
                OnCloseExternalTransactionUi(parameter);
            }
            catch (Exception ex)
            {
                Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
            }
            return null;
        }

        #endregion Methods

        #region IModalDialogViewModel Members

        public bool? DialogResult
        {
            get { return true; }
        }

        #endregion
    }
}
