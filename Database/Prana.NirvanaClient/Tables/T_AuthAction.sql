CREATE TABLE [dbo].[T_AuthAction] (
    [AuthActionId]   INT            IDENTITY (1, 1) NOT NULL,
    [AuthActionName] NVARCHAR (50)  NOT NULL,
    [Description]    NVARCHAR (200) NULL,
    CONSTRAINT [PK_T_AuthAction] PRIMARY KEY CLUSTERED ([AuthActionId] ASC)
);

