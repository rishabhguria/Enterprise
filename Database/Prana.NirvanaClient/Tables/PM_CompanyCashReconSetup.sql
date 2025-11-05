CREATE TABLE [dbo].[PM_CompanyCashReconSetup] (
    [CompanyCashReconSetupID] INT NOT NULL,
    [PMCompanyID]             INT NULL,
    [ThirdPartyID]            INT NULL,
    [AcceptDataSource]        BIT CONSTRAINT [DF_PM_CompanyCashReconSetup_AcceptDataSource] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PM_CompanyCashReconSetup] PRIMARY KEY CLUSTERED ([CompanyCashReconSetupID] ASC),
    CONSTRAINT [FK_PM_CompanyCashReconSetup_PM_Company] FOREIGN KEY ([PMCompanyID]) REFERENCES [dbo].[PM_Company] ([PMCompanyID]),
    CONSTRAINT [FK_PM_CompanyCashReconSetup_T_ThirdParty] FOREIGN KEY ([ThirdPartyID]) REFERENCES [dbo].[T_ThirdParty] ([ThirdPartyID])
);

