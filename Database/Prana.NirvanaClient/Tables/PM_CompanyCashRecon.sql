CREATE TABLE [dbo].[PM_CompanyCashRecon] (
    [CashReconID]   INT      NOT NULL,
    [PMCompanyID]   INT      NOT NULL,
    [CashReconBy]   INT      NOT NULL,
    [CashReconTime] DATETIME NOT NULL,
    CONSTRAINT [PK_PM_CompanyCashRecon] PRIMARY KEY CLUSTERED ([CashReconID] ASC),
    CONSTRAINT [FK_PM_CompanyCashRecon_PM_Company] FOREIGN KEY ([PMCompanyID]) REFERENCES [dbo].[PM_Company] ([PMCompanyID]),
    CONSTRAINT [FK_PM_CompanyCashRecon_T_User] FOREIGN KEY ([CashReconBy]) REFERENCES [dbo].[T_User] ([UserID])
);

