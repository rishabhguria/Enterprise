CREATE TABLE [dbo].[T_CVAUEC] (
    [CVAUECID]            BIGINT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID] INT    NOT NULL,
    [AUECID]              INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECID] PRIMARY KEY CLUSTERED ([CVAUECID] ASC),
    CONSTRAINT [FK_T_CVAUEC_T_CounterPartyVenue] FOREIGN KEY ([CounterPartyVenueID]) REFERENCES [dbo].[T_CounterPartyVenue] ([CounterPartyVenueID])
);

