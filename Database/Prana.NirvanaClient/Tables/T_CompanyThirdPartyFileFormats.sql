CREATE TABLE [dbo].[T_CompanyThirdPartyFileFormats] (
    [CompanyID_FK]                   INT NOT NULL,
    [CompanyFundID_FK]               INT NOT NULL,
    [CompanyPrimeBrokerClearerID_FK] INT NOT NULL,
    [CompanyCustodianID_FK]          INT NULL,
    [CompanyAdministratorID_FK]      INT NULL,
    CONSTRAINT [FK_T_CompanyThirdPartyFileFormats_T_Company] FOREIGN KEY ([CompanyID_FK]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_CompanyThirdPartyFileFormats_T_CompanyFunds] FOREIGN KEY ([CompanyFundID_FK]) REFERENCES [dbo].[T_CompanyFunds] ([CompanyFundID]),
    CONSTRAINT [FK_T_CompanyThirdPartyFileFormats_T_CompanyThirdParty] FOREIGN KEY ([CompanyPrimeBrokerClearerID_FK]) REFERENCES [dbo].[T_CompanyThirdParty] ([CompanyThirdPartyID]),
    CONSTRAINT [FK_T_CompanyThirdPartyFileFormats_T_CompanyThirdParty1] FOREIGN KEY ([CompanyCustodianID_FK]) REFERENCES [dbo].[T_CompanyThirdParty] ([CompanyThirdPartyID]),
    CONSTRAINT [FK_T_CompanyThirdPartyFileFormats_T_CompanyThirdParty2] FOREIGN KEY ([CompanyAdministratorID_FK]) REFERENCES [dbo].[T_CompanyThirdParty] ([CompanyThirdPartyID])
);

