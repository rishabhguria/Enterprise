namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Module.
    /// </summary>
    public class Module
    {
        int _moduleID = int.MinValue;
        string _moduleName = string.Empty;
        private int _companyID = int.MinValue;
        private int _companyModuleID = int.MinValue;
        private int _readWriteID = 1;
        /// <summary>
        /// The is show export
        /// </summary>
        private bool _isShowExport = true;

        public Module()
        {
        }

        public Module(int moduleID, string moduleName)
        {
            _moduleID = moduleID;
            _moduleName = moduleName;
        }

        public int ModuleID
        {
            get { return _moduleID; }
            set { _moduleID = value; }
        }


        public string ModuleName
        {
            get { return _moduleName; }
            set { _moduleName = value; }
        }

        public int CompanyID
        {
            get { return _companyID; }
            set { _companyID = value; }
        }
        public int CompanyModuleID
        {
            get
            {
                return _companyModuleID;
            }
            set { _companyModuleID = value; }
        }

        public int ReadWriteID
        {
            get { return _readWriteID; }
            set { _readWriteID = value; }
        }
        /// <summary>
        /// The is show export
        /// </summary>
        public bool IsShowExport
        {
            get { return _isShowExport; }
            set { _isShowExport = value; }
        }
    }
}