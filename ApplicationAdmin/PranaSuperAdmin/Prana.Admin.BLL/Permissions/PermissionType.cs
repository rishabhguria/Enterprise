namespace Prana.Admin.BLL
{
    public class PermissionType
    {
        int _permissionTypeID = int.MinValue;
        string _permissionTypeName = string.Empty;

        public PermissionType()
        {
        }

        public int PermissionTypeID
        {
            get { return _permissionTypeID; }
            set { _permissionTypeID = value; }
        }


        public string PermissionTypeName
        {
            get { return _permissionTypeName; }
            set { _permissionTypeName = value; }
        }

    }
}
