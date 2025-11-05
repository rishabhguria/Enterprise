CREATE TABLE [dbo].[T_RolePermission] (
    [RolePermissionID] INT IDENTITY (1, 1) NOT NULL,
    [RoleID]           INT NOT NULL,
    [PermissionID]     INT NOT NULL,
    CONSTRAINT [PK_T_RolePermission] PRIMARY KEY CLUSTERED ([RolePermissionID] ASC)
);

