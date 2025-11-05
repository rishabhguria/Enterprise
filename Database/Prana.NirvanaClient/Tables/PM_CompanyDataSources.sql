CREATE TABLE [dbo].[PM_CompanyDataSources] (
    [PMCompanyID]  INT NOT NULL,
    [ThirdPartyID] INT NOT NULL,
    CONSTRAINT [PK_PM_CompanyDataSources] PRIMARY KEY CLUSTERED ([PMCompanyID] ASC, [ThirdPartyID] ASC),
    CONSTRAINT [FK_PM_CompanyDataSources_PM_Company] FOREIGN KEY ([PMCompanyID]) REFERENCES [dbo].[PM_Company] ([PMCompanyID]),
    CONSTRAINT [FK_PM_CompanyDataSources_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

