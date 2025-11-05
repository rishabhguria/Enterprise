CREATE TABLE [dbo].[T_Permission] (
    [PermissionID]     INT IDENTITY (1, 1) NOT NULL,
    [PermissionTypeID] INT NULL,
    [ModuleID]         INT NULL,
    CONSTRAINT [PK_T_Permission] PRIMARY KEY CLUSTERED ([PermissionID] ASC)
);

