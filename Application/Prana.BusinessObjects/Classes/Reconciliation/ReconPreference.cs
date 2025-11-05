using System;

namespace Prana.BusinessObjects
{
    [Serializable]
    public class ReconPreference
    {

        private int _clientID;
        public int ClientID
        {
            get { return _clientID; }
            set { _clientID = value; }
        }

        private int _reconTypeID;
        public int ReconTypeID
        {
            get { return _reconTypeID; }
            set { _reconTypeID = value; }
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }

        private string _templateKey;
        public string TemplateKey
        {
            get { return _templateKey; }
            set { _templateKey = value; }
        }

        private bool _isShowCAGeneratedTrades;
        public bool IsShowCAGeneratedTrades
        {
            get { return _isShowCAGeneratedTrades; }
            set { _isShowCAGeneratedTrades = value; }
        }


    }
}




