CREATE TABLE [dbo].[T_CVAUECTimeInForce] (
    [CVAUECTimeInForce] BIGINT IDENTITY (1, 1) NOT NULL,
    [CVAUECID]          BIGINT NOT NULL,
    [TimeInForceID]     INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECTimeInForce] PRIMARY KEY CLUSTERED ([CVAUECTimeInForce] ASC)
);

