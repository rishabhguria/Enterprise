CREATE TABLE [dbo].[PM_CompanyReconSetup] (
    [CompanyReconSetupID] INT NOT NULL,
    [PMCompanyID]         INT NULL,
    [ThirdPartyID]        INT NULL,
    [AcceptDataSource]    BIT CONSTRAINT [DF_PM_CompanyReconSetup_AcceptDataSource] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PM_CompanyReconSetup] PRIMARY KEY CLUSTERED ([CompanyReconSetupID] ASC),
    CONSTRAINT [FK_PM_CompanyReconSetup_PM_Company] FOREIGN KEY ([PMCompanyID]) REFERENCES [dbo].[PM_Company] ([PMCompanyID]),
    CONSTRAINT [FK_PM_CompanyReconSetup_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

