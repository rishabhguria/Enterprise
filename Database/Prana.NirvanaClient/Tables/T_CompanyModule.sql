CREATE TABLE [dbo].[T_CompanyModule] (
    [CompanyModuleID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]       INT NOT NULL,
    [ModuleID]        INT NOT NULL,
    [Read_WriteID]    INT NULL,
    CONSTRAINT [PK_T_CompanyModule] PRIMARY KEY CLUSTERED ([CompanyModuleID] ASC),
    CONSTRAINT [FK_T_CompanyModule_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID])
);

