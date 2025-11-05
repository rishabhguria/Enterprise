CREATE TABLE [dbo].[T_Role] (
    [RoleID]   INT          IDENTITY (1, 1) NOT NULL,
    [RoleName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_Role] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);

