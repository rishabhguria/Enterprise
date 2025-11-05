CREATE TABLE [dbo].[T_AuthPermissions] (
    [PermissionId]      INT IDENTITY (1, 1) NOT NULL,
    [PrincipalType]     INT NOT NULL,
    [PricipalValue]     INT NOT NULL,
    [ResourceDataType]  INT NOT NULL,
    [ResourceDataValue] INT NOT NULL,
    [AuthActionValue]   INT NOT NULL,
    CONSTRAINT [PK_T_AuthPermissions] PRIMARY KEY CLUSTERED ([PrincipalType] ASC, [PricipalValue] ASC, [ResourceDataType] ASC, [ResourceDataValue] ASC)
);

