using Prana.BusinessObjects.PositionManagement;
using System;
using System.ComponentModel;


namespace Prana.PM.BLL
{
    /// <summary>
    /// Business class used to display data on Run Recon Screen of PM
    /// </summary>
    public class DataSourceReconSummaryInfo : INotifyPropertyChanged
    {

        private ThirdPartyNameID _datasourceNameId;

        public ThirdPartyNameID DataSourceNameIDValue
        {
            get { return _datasourceNameId; }
            set
            {
                _datasourceNameId = value;
                OnPropertyChanged("DataSourceNameIDValue");
            }
        }

        private DateTime _reconDate = DateTime.Today;

        public DateTime ReconDate
        {
            get { return _reconDate; }
            set
            {
                _reconDate = value;
                OnPropertyChanged("ReconDate");
            }
        }

        private BindingList<TradeReconSummary> _tradeReconSummary;

        public BindingList<TradeReconSummary> TradeReconSummaryList
        {
            get { return _tradeReconSummary; }
            set
            {
                _tradeReconSummary = value;
                OnPropertyChanged("TradeReconSummaryList");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
