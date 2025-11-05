CREATE TABLE [dbo].[T_AuthRoles] (
    [RoleID]   INT          IDENTITY (1, 1) NOT NULL,
    [RoleName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_RoleID] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

