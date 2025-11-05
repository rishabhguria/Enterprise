CREATE TABLE [dbo].[T_UsersSecuritiesListPermission]
(
	[CompanyUserID]       INT NOT NULL,
    [Read_WriteID]        INT NULL,
    CONSTRAINT [FK_T_UsersSecuritiesListPermission_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID]) ON DELETE CASCADE
)
