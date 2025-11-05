namespace Prana.Admin.BLL
{
    /// <summary>
    /// Preferences is a singelton class to kep preferences of the logged in user.
    /// </summary>
    public class Preferences
    {
        private static readonly Preferences instance = new Preferences();
        private Preferences()
        {
        }

        #region Private Variables
        private int _userID = int.MinValue;

        private bool _viewPositionManagement = false;
        private bool _editPositionManagement = false;
        private bool _addPositionManagement = false;
        private bool _deletePositionManagement = false;

        private bool _viewRiskManager = false;
        private bool _editRiskManager = false;
        private bool _addRiskManager = false;
        private bool _deleteRiskManager = false;

        private bool _viewTradingTicket = false;
        private bool _editTradingTicket = false;
        private bool _addTradingTicket = false;
        private bool _deleteTradingTicket = false;

        private bool _viewCommissionRules = false;
        private bool _editCommissionRules = false;
        private bool _addCommissionRules = false;
        private bool _deleteCommissionRules = false;

        private bool _viewVendor = false;
        private bool _editVendor = false;
        private bool _addVendor = false;
        private bool _deleteVendor = false;

        private bool _viewCompanyMaster = false;
        private bool _editCompanyMaster = false;
        private bool _addCompanyMaster = false;
        private bool _deleteCompanyMaster = false;

        #endregion

        public static Preferences Instance
        {
            get { return instance; }
        }

        #region Properties

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        //public bool Maintain_AUEC
        //{
        //    get{return _maintain_auec;}			
        //}

        //public bool Set_Up_Company
        //{
        //    get{return _set_up_company;}			
        //}

        //public bool Maintain_Companies
        //{
        //    get{return _maintain_companies;}			
        //}

        //public bool Maintain_Counter_Parties
        //{
        //    get{return _maintain_counter_parties;}
        //}

        public bool View_PositionManagement
        {
            get { return _viewPositionManagement; }
        }
        public bool Edit_PositionManagement
        {
            get { return _editPositionManagement; }
        }
        public bool Add_PositionManagement
        {
            get { return _addPositionManagement; }
        }
        public bool Delete_PositionManagement
        {
            get { return _deletePositionManagement; }
        }

        public bool View_RiskManager
        {
            get { return _viewRiskManager; }
        }
        public bool Edit_RiskManager
        {
            get { return _editRiskManager; }
        }
        public bool Add_RiskManager
        {
            get { return _addRiskManager; }
        }
        public bool Delete_RiskManager
        {
            get { return _deleteRiskManager; }
        }

        public bool View_TradingTicket
        {
            get { return _viewTradingTicket; }
        }
        public bool Edit_TradingTicket
        {
            get { return _editTradingTicket; }
        }
        public bool Add_TradingTicket
        {
            get { return _addTradingTicket; }
        }
        public bool Delete_TradingTicket
        {
            get { return _deleteTradingTicket; }
        }




        public bool View_CommissionRules
        {
            get { return _viewCommissionRules; }
        }
        public bool Edit_CommissionRules
        {
            get { return _editCommissionRules; }
        }
        public bool Add_CommissionRules
        {
            get { return _addCommissionRules; }
        }
        public bool Delete_CommissionRules
        {
            get { return _deleteCommissionRules; }
        }



        public bool View_Vendor
        {
            get { return _viewVendor; }
        }
        public bool Edit_Vendor
        {
            get { return _editVendor; }
        }
        public bool Add_Vendor
        {
            get { return _addVendor; }
        }
        public bool Delete_Vendor
        {
            get { return _deleteVendor; }
        }

        public bool View_CompanyMaster
        {
            get { return _viewCompanyMaster; }
        }
        public bool Edit_CompanyMaster
        {
            get { return _editCompanyMaster; }
        }
        public bool Add_CompanyMaster
        {
            get { return _addCompanyMaster; }
        }
        public bool Delete_CompanyMaster
        {
            get { return _deleteCompanyMaster; }
        }


        private enum PreferencesType
        {
            MAINTAIN_AUEC = 1,
            SET_UP_COMPANY = 2,
            MAINTAIN_COMPANIES = 3,
            MAINTAIN_COUNTER_PARTIES = 4
        }

        private enum PermisionType
        {
            VIEW = 1,
            EDIT = 2,
            ADD = 3,
            DELETE = 4
        }

        private enum ModuleName
        {
            //Have the values directly from the DB. Depends upon the changes in DB.
            SLSU = 1,
            AUEC = 2,
            CV_MASTER = 3,
            COMMISSION_RULES = 4,
            THIRD_PARTIES = 5,
            VENDOR = 6,
            COMPANY_MASTER = 7,
            POSITION_MANAGEMENT = 8
        }

        #endregion

        private Permissions _userPermissions = null;
        public Permissions UserPermissions
        {
            get { return _userPermissions; }
            set { _userPermissions = value; }
        }

        public bool Reset()
        {
            bool result = false;
            foreach (Permission permission in _userPermissions)
            {

                if (permission.ModuleID == (int)ModuleName.VENDOR && permission.PermissionTypeID == (int)PermisionType.VIEW)
                {
                    _viewVendor = true;
                }
                if (permission.ModuleID == (int)ModuleName.VENDOR && permission.PermissionTypeID == (int)PermisionType.EDIT)
                {
                    _editVendor = true;
                }
                if (permission.ModuleID == (int)ModuleName.VENDOR && permission.PermissionTypeID == (int)PermisionType.ADD)
                {
                    _addVendor = true;
                }
                if (permission.ModuleID == (int)ModuleName.VENDOR && permission.PermissionTypeID == (int)PermisionType.DELETE)
                {
                    _deleteVendor = true;
                }



                if (permission.ModuleID == (int)ModuleName.COMMISSION_RULES && permission.PermissionTypeID == (int)PermisionType.VIEW)
                {
                    _viewCommissionRules = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMMISSION_RULES && permission.PermissionTypeID == (int)PermisionType.EDIT)
                {
                    _editCommissionRules = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMMISSION_RULES && permission.PermissionTypeID == (int)PermisionType.ADD)
                {
                    _addCommissionRules = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMMISSION_RULES && permission.PermissionTypeID == (int)PermisionType.DELETE)
                {
                    _deleteCommissionRules = true;
                }


                if (permission.ModuleID == (int)ModuleName.COMPANY_MASTER && permission.PermissionTypeID == (int)PermisionType.VIEW)
                {
                    _viewCompanyMaster = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMPANY_MASTER && permission.PermissionTypeID == (int)PermisionType.EDIT)
                {
                    _editCompanyMaster = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMPANY_MASTER && permission.PermissionTypeID == (int)PermisionType.ADD)
                {
                    _addCompanyMaster = true;
                }
                if (permission.ModuleID == (int)ModuleName.COMPANY_MASTER && permission.PermissionTypeID == (int)PermisionType.DELETE)
                {
                    _deleteCompanyMaster = true;
                }

                if (permission.ModuleID == (int)ModuleName.POSITION_MANAGEMENT && permission.PermissionTypeID == (int)PermisionType.VIEW)
                {
                    _viewPositionManagement = true;
                }
                if (permission.ModuleID == (int)ModuleName.POSITION_MANAGEMENT && permission.PermissionTypeID == (int)PermisionType.EDIT)
                {
                    _editPositionManagement = true;
                }
                if (permission.ModuleID == (int)ModuleName.POSITION_MANAGEMENT && permission.PermissionTypeID == (int)PermisionType.ADD)
                {
                    _addPositionManagement = true;
                }
                if (permission.ModuleID == (int)ModuleName.POSITION_MANAGEMENT && permission.PermissionTypeID == (int)PermisionType.DELETE)
                {
                    _deletePositionManagement = true;
                }
            }

            return result;
        }
    }
}
