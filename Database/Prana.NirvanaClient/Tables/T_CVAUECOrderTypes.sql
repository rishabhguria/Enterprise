CREATE TABLE [dbo].[T_CVAUECOrderTypes] (
    [CVAUECOrderTypes] BIGINT IDENTITY (1, 1) NOT NULL,
    [CVAUECID]         BIGINT NOT NULL,
    [OrderTypesID]     INT    NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECOrderTypes] PRIMARY KEY CLUSTERED ([CVAUECOrderTypes] ASC)
);

