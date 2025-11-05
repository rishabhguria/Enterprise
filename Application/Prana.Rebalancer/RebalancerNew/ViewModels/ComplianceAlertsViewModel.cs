using Prana.BusinessObjects;
using Prana.BusinessObjects.Compliance.Alerting;
using Prana.ComplianceEngine.ComplianceAlertPopup;
using Prana.LogManager;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Prana.Rebalancer.RebalancerNew.ViewModels
{
    public class ComplianceAlertsViewModel : BindableBase, IDisposable
    {
        #region Command

        public DelegateCommand<object> ThresholdActualResultCommand { get; set; }

        #endregion

        #region Properties

        DateTime valTime = DateTime.Now;

        private string label = string.Empty;
        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Alert> _alerts;
        public ObservableCollection<Alert> AlertsList
        {

            get { return _alerts; }
            set
            {
                _alerts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// RemoveFilters
        /// </summary>
        private bool _removeFilters;
        public bool RemoveFilters
        {
            get { return _removeFilters; }
            set
            {
                _removeFilters = value;
                RaisePropertyChangedEvent("RemoveFilters");
            }
        }

        #endregion

        #region Method and Constructor

        /// <summary>
        /// ComplianceAlertsViewModel Constructor
        /// </summary>
        public ComplianceAlertsViewModel()
        {
            ThresholdActualResultCommand = new DelegateCommand<object>(obj => ThresholdActualResultAction(obj));
        }

        /// <summary>
        /// ThresholdActualResultAction
        /// </summary>
        /// <param name="commandParameter"></param>
        private void ThresholdActualResultAction(object commandParameter)
        {
            try
            {
                if (commandParameter != null)
                {
                    Alert alert = commandParameter as Alert;
                    if (alert != null)
                    {
                        string actualResult = alert.ActualResult;
                        string threshold = alert.Threshold;
                        string constraintFields = alert.ConstraintFields;
                        OpenAndBindDataThresholdActualResultView thresHoldView = new OpenAndBindDataThresholdActualResultView();
                        thresHoldView.OpenAndBindDataThresholdActualResultView1(constraintFields, threshold, actualResult);
                    }
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        /// <summary>
        /// Bind Alerts
        /// </summary>
        /// <param name="alerts"></param>
        public void BindAlerts(List<Alert> alerts)
        {
            try
            {
                foreach(Alert alert in alerts)
                {
                    alert.Description = alert.Summary;
                }
                RemoveFilters = true;
                AlertsList = new ObservableCollection<Alert>(alerts);
                DateTime.TryParse(AlertsList[AlertsList.Count - 1].ValidationTime.ToString(), out valTime);
                Label = ComplainceConstants.CONST_PRE_TRADE + valTime.Date.ToString(ComplainceConstants.CONST_DATE_FORMAT) + ", " + ComplainceConstants.CONST_TIME + valTime.ToString(ComplainceConstants.CONST_TIME_FORMAT);
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Dispose() calls Dispose(true)
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposing Objects
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    AlertsList = null;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = Logger.HandleException(ex, LoggingConstants.POLICY_LOGANDSHOW);
                if (rethrow)
                    throw;
            }

        }

        #endregion
    }
}

