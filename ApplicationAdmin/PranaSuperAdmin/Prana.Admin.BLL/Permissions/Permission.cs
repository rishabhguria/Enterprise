namespace Prana.Admin.BLL
{
    /// <summary>
    /// Summary description for Permission.
    /// </summary>
    public class Permission
    {
        #region Private members

        private int _ID = int.MinValue;
        //private string _permission = string.Empty;		
        private bool _isSelected = false;

        private int _permissionTypeID = int.MinValue;
        private int _moduleID = int.MinValue;
        private string _permissionTypeName = string.Empty;
        private string _moduleName = string.Empty;

        #endregion

        #region Constructors

        public Permission()
        {
        }

        //public Permission(int permissionID, string permission)
        //{
        //    _ID = permissionID;
        //    _permission = permission;
        //}

        public Permission(int permissionID, bool isSelected)
        {
            _ID = permissionID;
            _isSelected = isSelected;
        }

        #endregion

        #region Properties

        public int PermissionID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        //public string PermissionName
        //{
        //    get { return _permission; }
        //    set { _permission = value; }
        //}

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public int PermissionTypeID
        {
            get { return _permissionTypeID; }
            set { _permissionTypeID = value; }
        }

        public int ModuleID
        {
            get { return _moduleID; }
            set { _moduleID = value; }
        }

        public string ModuleName
        {
            get
            {
                Module module = ModuleManager.GetModule(_moduleID);
                _moduleName = module.ModuleName;
                return _moduleName;
            }
            set { _moduleName = value; }
        }

        public string PermissionTypeName
        {
            get
            {
                PermissionType permissionType = PermissionManager.GetPermissionType(_permissionTypeID);
                _permissionTypeName = permissionType.PermissionTypeName;
                return _permissionTypeName;
            }
            set { _permissionTypeName = value; }
        }

        #endregion
    }
}
