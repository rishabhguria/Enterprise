using Prana.BusinessObjects.AppConstants;
using Prana.Global;
using System;
using System.Globalization;

namespace Prana.BusinessObjects
{
    public class ReconParameters
    {
        #region Template Key
        string _templateKey = string.Empty;
        public string TemplateKey
        {
            get
            {
                if (string.IsNullOrEmpty(_templateKey))
                {
                    if (!string.IsNullOrEmpty(_clientID) && !string.IsNullOrEmpty(_reconType) && !string.IsNullOrEmpty(_templateName))
                    {
                        _templateKey = _clientID + Seperators.SEPERATOR_6 + _reconType + Seperators.SEPERATOR_6 + _templateName;
                    }
                }
                return _templateKey;
            }
            set { _templateKey = value; }
        }
        #endregion

        #region Client ID
        string _clientID = string.Empty;
        public string ClientID
        {
            get
            {
                if (string.IsNullOrEmpty(_clientID))
                {
                    //if (_reconTemplate != null)
                    //{
                    //    _clientID = _reconTemplate.ClientID.ToString();
                    //}
                    if (!string.IsNullOrEmpty(_templateKey))
                    {
                        _clientID = _templateKey.Split(Seperators.SEPERATOR_6)[0];
                    }
                }
                return _clientID;
            }
            set { _clientID = value; }
        }
        #endregion

        #region Recon Type
        string _reconType = string.Empty;
        public string ReconType
        {
            get
            {
                if (string.IsNullOrEmpty(_reconType))
                {
                    //if (_reconTemplate != null)
                    //{
                    //    _reconType = _reconTemplate.ReconType.ToString();
                    //}
                    if (!string.IsNullOrEmpty(_templateKey))
                    {
                        _reconType = _templateKey.Split(Seperators.SEPERATOR_6)[1];
                    }
                }
                return _reconType;
            }

            set { _reconType = value; }
        }
        #endregion

        #region Template Name
        string _templateName = string.Empty;
        public string TemplateName
        {
            get
            {
                if (string.IsNullOrEmpty(_templateName))
                {
                    //if (_reconTemplate != null)
                    //{
                    //    _templateName = _reconTemplate.TemplateName;
                    //}
                    if (!string.IsNullOrEmpty(_templateKey))
                    {
                        _templateName = _templateKey.Split(Seperators.SEPERATOR_6)[2];
                    }
                }
                return _templateName;
            }

            set { _templateName = value; }
        }
        #endregion

        #region From Date
        string _fromDate = string.Empty;
        public string FromDate
        {
            get { return _fromDate; }
            set
            {
                _fromDate = value;
                _dtFromDate = DateTime.ParseExact(_fromDate, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);

            }
        }
        DateTime _dtFromDate = DateTime.MinValue;
        public DateTime DTFromDate
        {
            get { return _dtFromDate; }
            set
            {
                _dtFromDate = value;
                _fromDate = _dtFromDate.ToString(ApplicationConstants.DateFormat);
            }
        }
        #endregion

        #region To Date
        string _toDate = string.Empty;
        public string ToDate
        {
            get { return _toDate; }
            set
            {
                _toDate = value;
                _dtToDate = DateTime.ParseExact(_toDate, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);
            }
        }
        DateTime _dtToDate = DateTime.MinValue;
        public DateTime DTToDate
        {
            get { return _dtToDate; }
            set
            {
                _dtToDate = value;
                _toDate = _dtToDate.ToString(ApplicationConstants.DateFormat);
            }
        }
        #endregion

        #region Recon File Path
        string _reconFilePath = string.Empty;
        public string ReconFilePath
        {
            get { return _reconFilePath; }
            set { _reconFilePath = value; }
        }
        #endregion

        #region PB File Path
        string _pbFilePath = string.Empty;
        public string PBFilePath
        {
            get { return _pbFilePath; }
            set { _pbFilePath = value; }
        }
        #endregion

        #region SP Name
        string _spName = string.Empty;
        public string SpName
        {
            get { return _spName; }
            set { _spName = value; }
        }
        #endregion

        #region Format Name
        string _formatName = string.Empty;
        public string FormatName
        {
            get { return _formatName; }
            set { _formatName = value; }
        }
        #endregion

        #region Recon Date Type
        ReconDateType _reconDateType = ReconDateType.TradeDate;
        public ReconDateType ReconDateType
        {
            get { return _reconDateType; }
            set { _reconDateType = value; }
        }

        #endregion

        #region Run Date
        string _runDate = string.Empty;
        public string RunDate
        {
            get { return _runDate; }
            set
            {
                _runDate = value;
                _dtRunDate = DateTime.ParseExact(_runDate, ApplicationConstants.DateFormat, CultureInfo.InvariantCulture);

            }
        }
        DateTime _dtRunDate = DateTime.Now;
        public DateTime DTRunDate
        {
            get { return _dtRunDate; }
            set
            {
                _dtRunDate = value;
                _runDate = _dtRunDate.ToString(ApplicationConstants.DateFormat);
            }
        }
        #endregion

        #region Is Recon Report To Be Generated
        bool _isReconReportToBeGenerated = false;
        public bool IsReconReportToBeGenerated
        {
            get { return _isReconReportToBeGenerated; }
            set { _isReconReportToBeGenerated = value; }
        }
        #endregion

        private bool _isShowCAGeneratedTrades = true;
        public bool IsShowCAGeneratedTrades
        {
            get { return _isShowCAGeneratedTrades; }
            set { _isShowCAGeneratedTrades = value; }
        }

        #region Recon Template
        //ReconTemplate _reconTemplate = null;
        //public ReconTemplate ReconTemplate
        //{
        //    get
        //    {
        //        if (_reconTemplate == null)
        //        {
        //            if (!string.IsNullOrEmpty(_templateKey))
        //            {
        //                _reconTemplate = ReconPrefManager.ReconPreferences.GetTemplates(_templateKey);
        //            }
        //        }
        //        return _reconTemplate;
        //    }
        //    set { _reconTemplate = value; }
        //}
        #endregion
    }
}
