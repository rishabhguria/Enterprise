CREATE TABLE [dbo].[T_CompanyCounterPartyVenueTag] (
    [CompanyCounterPartyVenueTagID] INT          IDENTITY (1, 1) NOT NULL,
    [TagType]                       VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_CompanyCounterPartyVenueTag] PRIMARY KEY CLUSTERED ([CompanyCounterPartyVenueTagID] ASC)
);

