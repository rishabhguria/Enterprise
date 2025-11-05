CREATE TABLE [dbo].[T_CompanyCounterPartyVenues] (
    [CompanyCounterPartyCVID] INT IDENTITY (1, 1) NOT NULL,
    [CompanyID]               INT NOT NULL,
    [CounterPartyVenueID]     INT NOT NULL,
    [CounterPartyID]          INT NULL,
    CONSTRAINT [PK_T_CompanyCounterParty] PRIMARY KEY CLUSTERED ([CompanyCounterPartyCVID] ASC),
    CONSTRAINT [FK_T_CompanyCounterParty_T_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[T_Company] ([CompanyID]),
    CONSTRAINT [FK_T_CompanyCounterPartyVenues_T_CounterParty] FOREIGN KEY ([CounterPartyID]) REFERENCES [dbo].[T_CounterParty] ([CounterPartyID]),
    CONSTRAINT [FK_T_CompanyCounterPartyVenues_T_CounterPartyVenue] FOREIGN KEY ([CounterPartyVenueID]) REFERENCES [dbo].[T_CounterPartyVenue] ([CounterPartyVenueID])
);

