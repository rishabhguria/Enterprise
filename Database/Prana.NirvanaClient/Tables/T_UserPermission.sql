CREATE TABLE [dbo].[T_UserPermission] (
    [UserPermissionID] INT IDENTITY (1, 1) NOT NULL,
    [UserID]           INT NOT NULL,
    [PermissionID]     INT NOT NULL,
    CONSTRAINT [PK_T_UserPermission] PRIMARY KEY CLUSTERED ([UserPermissionID] ASC),
    CONSTRAINT [FK_T_UserPermission_T_Permission] FOREIGN KEY ([PermissionID]) REFERENCES [dbo].[T_Permission] ([PermissionID])
);

