CREATE TABLE [dbo].[T_CompanyCVGiveUpIdentifier] (
    [CompanyCVenueGiveupIdentifierID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyCounterPartyVenueID]      INT          NOT NULL,
    [GiveUpIdentifier]                VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyCVGiveUpIdentifier] PRIMARY KEY CLUSTERED ([CompanyCVenueGiveupIdentifierID] ASC)
);

