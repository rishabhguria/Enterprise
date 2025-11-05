CREATE TABLE [dbo].[T_Venue] (
    [VenueID]     INT          IDENTITY (1, 1) NOT NULL,
    [VenueName]   VARCHAR (50) NOT NULL,
    [VenueTypeID] INT          NOT NULL,
    [Route]       VARCHAR (50) NOT NULL,
    [ExchangeID]  INT          NULL,
    CONSTRAINT [PK_T_Venue] PRIMARY KEY CLUSTERED ([VenueID] ASC),
    CONSTRAINT [FK_T_Venue_T_VenuType] FOREIGN KEY ([VenueTypeID]) REFERENCES [dbo].[T_VenuType] ([VenueTypeID])
);

