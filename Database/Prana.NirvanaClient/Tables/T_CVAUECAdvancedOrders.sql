CREATE TABLE [dbo].[T_CVAUECAdvancedOrders] (
    [CounterPartyVenueAUECAdvancedOrderID] INT IDENTITY (1, 1) NOT NULL,
    [CounterPartyVenueAUECID]              INT NOT NULL,
    [AdvancedOrderID]                      INT NOT NULL,
    CONSTRAINT [PK_T_CounterPartyVenueAUECAdvancedOrders] PRIMARY KEY CLUSTERED ([CounterPartyVenueAUECAdvancedOrderID] ASC)
);

