CREATE TABLE [dbo].[PM_CompanyRecon] (
    [ReconID]     INT      NOT NULL,
    [PMCompanyID] INT      NOT NULL,
    [ReconBy]     INT      NOT NULL,
    [ReconTime]   DATETIME NOT NULL,
    CONSTRAINT [PK_PM_CompanyRecon] PRIMARY KEY CLUSTERED ([ReconID] ASC),
    CONSTRAINT [FK_PM_CompanyRecon_PM_Company] FOREIGN KEY ([PMCompanyID]) REFERENCES [dbo].[PM_Company] ([PMCompanyID]),
    CONSTRAINT [FK_PM_CompanyRecon_T_User] FOREIGN KEY ([ReconBy]) REFERENCES [dbo].[T_User] ([UserID])
);

