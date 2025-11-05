CREATE TABLE [dbo].[T_AuthPrincipalType] (
    [TypeId]      INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (200) NULL,
    CONSTRAINT [PK_T_PrincipalType] PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

