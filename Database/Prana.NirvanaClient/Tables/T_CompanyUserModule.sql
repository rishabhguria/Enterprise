CREATE TABLE [dbo].[T_CompanyUserModule] (
    [CompanyUserModuleID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyModuleID]     INT NOT NULL,
    [CompanyUserID]       INT NOT NULL,
    [Read_WriteID]        INT NULL,
	[IsShowExport]		  BIT DEFAULT 1 NOT NULL,
    CONSTRAINT [PK_T_CompanyUserModule] PRIMARY KEY CLUSTERED ([CompanyUserModuleID] ASC),
    CONSTRAINT [FK_T_CompanyUserModule_T_CompanyUser] FOREIGN KEY ([CompanyUserID]) REFERENCES [dbo].[T_CompanyUser] ([UserID])
);

