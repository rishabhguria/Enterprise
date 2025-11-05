CREATE TABLE [dbo].[T_UserRole] (
    [UserRoleID] INT IDENTITY (1, 1) NOT NULL,
    [UserID]     INT NOT NULL,
    [RoleID]     INT NOT NULL,
    CONSTRAINT [PK_T_UserRole] PRIMARY KEY CLUSTERED ([UserRoleID] ASC)
);

