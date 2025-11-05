CREATE TABLE [dbo].[T_CompanyCounterPartyVenueIdentifier] (
    [CompanyCounterPartyVenueIdentifierID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyCounterPartyVenueID]           INT          NOT NULL,
    [CMTAIdentifier]                       VARCHAR (20) NULL,
    [GiveUpIdentifier]                     VARCHAR (20) NULL,
    CONSTRAINT [PK_T_CompanyCounterPartyVenueIdentifier] PRIMARY KEY CLUSTERED ([CompanyCounterPartyVenueIdentifierID] ASC)
);

