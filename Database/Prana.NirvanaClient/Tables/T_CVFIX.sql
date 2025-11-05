CREATE TABLE [dbo].[T_CVFIX] (
    [CVFixID]             INT          IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueID] INT          NOT NULL,
    [Acronymn]            VARCHAR (10) NOT NULL,
    [FixVersionID]        INT          NOT NULL,
    [TargetCompID]        VARCHAR (50) NOT NULL,
    [DeliverToCompID]     VARCHAR (50) NULL,
    [DeliverToSubID]      VARCHAR (50) NULL,
    CONSTRAINT [PK_T_CounterPartyVenueFIX] PRIMARY KEY CLUSTERED ([CVFixID] ASC)
);

