namespace Prana.BusinessObjects.Enums
{
    public enum NirvanaRoles
    {
        None = 0,
        User = 1,
        PowerUser = 2,
        Administrator = 3,
        SystemAdministrator = 4,
    }

    public enum NirvanaPrincipalType
    {
        Client = 1,
        Role = 2,
        User = 3
    }

    public enum NirvanaResourceType
    {
        AccountGroup = 1,
        Modules = 2,
    }

    public enum AuthAction
    {
        None = 1,
        Read = 2,
        Write = 3,
        Approve = 4,
    }

}

