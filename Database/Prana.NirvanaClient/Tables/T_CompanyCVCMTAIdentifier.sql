CREATE TABLE [dbo].[T_CompanyCVCMTAIdentifier] (
    [CompanyCVenueCMTAIdentifierID] INT          IDENTITY (1, 1) NOT NULL,
    [CompanyCounterPartyVenueID]    INT          NOT NULL,
    [CMTAIdentifier]                VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyCVCMTAIdentifier] PRIMARY KEY CLUSTERED ([CompanyCVenueCMTAIdentifierID] ASC)
);

