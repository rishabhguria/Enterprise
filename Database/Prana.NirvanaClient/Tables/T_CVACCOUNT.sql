CREATE TABLE [dbo].[T_CVACCOUNT] (
    [CVACCOUNTID]            BIGINT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID] INT    NOT NULL,
    [AccountID]              INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueCVACCOUNTID] PRIMARY KEY CLUSTERED ([CVACCOUNTID] ASC),
    CONSTRAINT [FK_T_CVAccount_T_CounterPartyVenue] FOREIGN KEY ([CounterPartyVenueID]) REFERENCES [dbo].[T_CounterPartyVenue] ([CounterPartyVenueID])
);



