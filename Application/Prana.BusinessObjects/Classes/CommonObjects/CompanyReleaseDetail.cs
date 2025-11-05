using System.Collections.Generic;

namespace Prana.BusinessObjects
{
    /// <summary>
    /// This will get its properties from T_CompanyReleaseDetails 
    /// </summary>
    public class CompanyReleaseDetail
    {
        public CompanyReleaseDetail()
        {
            _companyReleaseID = int.MinValue;
            //_companyID = int.MinValue;
            _releaseName = string.Empty;
            _ip = string.Empty;
            _releasePath = string.Empty;

        }

        //multiple company id exists for 1 Release at JIRA : 646
        private List<int> _companyID = new List<int>();
        public List<int> CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }

        private int _companyReleaseID;
        public int CompanyReleaseID
        {
            get { return _companyReleaseID; }
            set { _companyReleaseID = value; }
        }

        //private int _companyID;
        //public int CompanyID
        //{
        //    get { return _companyID; }
        //    set { _companyID = value; }
        //}

        private string _releaseName;
        public string ReleaseName
        {
            get { return _releaseName; }
            set { _releaseName = value; }
        }

        private string _ip;
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        private string _releasePath;
        public string ReleasePath
        {
            get { return _releasePath; }
            set { _releasePath = value; }
        }


        //multiple account id are there for 1 Release name
        private List<int> _companyAccountID = new List<int>();
        public List<int> CompanyAccountID
        {
            get { return _companyAccountID; }
            set { _companyAccountID = value; }
        }

        private string _nirvanaClient;
        public string NirvanaClient
        {
            get { return _nirvanaClient; }
            set { _nirvanaClient = value; }
        }

        private string _securityMasterDB;
        public string SecurityMasterDB
        {
            get { return _securityMasterDB; }
            set { _securityMasterDB = value; }
        }
    }
}
