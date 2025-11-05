CREATE TABLE [dbo].[T_RoutingLogicVenues] (
    [RLID_FK]                       INT      NOT NULL,
    [CompanyTradingAccountID_FK]    INT      NULL,
    [CompanyCounterPartyVenueID_FK] INT      NULL,
    [Rank]                          SMALLINT NOT NULL,
    CONSTRAINT [FK_T_RoutingLogicVenues_T_RoutingLogic] FOREIGN KEY ([RLID_FK]) REFERENCES [dbo].[T_RoutingLogic] ([RLID]) ON DELETE CASCADE
);

