CREATE TABLE [dbo].[T_CompanyThirdPartyMappingDetails] (
    [CompanyThirdPartyID_FK]     INT          NOT NULL,
    [InternalFundNameID_FK]      INT          NOT NULL,
    [MappedName]                 VARCHAR (50) NOT NULL,
    [FundAccntNo]                VARCHAR (50) NOT NULL,
    [FundTypeID_FK]              INT          NOT NULL,
    [CompanyID_FK]               INT          NOT NULL,
    [ThirdPartyMappingDetailsID] INT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_T_CompanyThirdPartyMappingDetails] PRIMARY KEY CLUSTERED ([ThirdPartyMappingDetailsID] ASC),
    CONSTRAINT [FK_T_CompanyThirdPartyMappingDetails_T_CompanyThirdParty] FOREIGN KEY ([CompanyThirdPartyID_FK]) REFERENCES [dbo].[T_CompanyThirdParty] ([CompanyThirdPartyID])
);

