CREATE TABLE [dbo].[T_CounterPartyVenue] (
    [CounterPartyVenueID] INT          IDENTITY (1, 1) NOT NULL,
    [CounterPartyID]      INT          NOT NULL,
    [VenueID]             INT          NOT NULL,
    [DisplayName]         VARCHAR (50) NULL,
    [IsElectronic]        INT          NULL,
    [OatsIdentifier]      VARCHAR (50) NULL,
    [SymbolConventionID]  INT          NULL,
    [CurrencyID]          INT          NULL,
    CONSTRAINT [PK_T_CounterPartyVenue] PRIMARY KEY CLUSTERED ([CounterPartyVenueID] ASC),
    CONSTRAINT [FK_T_CounterPartyVenue_T_CounterParty] FOREIGN KEY ([CounterPartyID]) REFERENCES [dbo].[T_CounterParty] ([CounterPartyID]),
    CONSTRAINT [FK_T_CounterPartyVenue_T_Venue] FOREIGN KEY ([VenueID]) REFERENCES [dbo].[T_Venue] ([VenueID])
);

