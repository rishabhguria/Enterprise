CREATE TABLE [dbo].[T_RoutingLogicClientGroupRL] (
    [ClientGroupID_FK] INT NOT NULL,
    [RLID_FK]          INT NOT NULL,
    [Rank]             INT NOT NULL,
    CONSTRAINT [FK_T_RoutingLogicClientGroupRL_T_RoutingLogic] FOREIGN KEY ([RLID_FK]) REFERENCES [dbo].[T_RoutingLogic] ([RLID]),
    CONSTRAINT [FK_T_RoutingLogicClientGroupRL_T_RoutingLogicClientGroup] FOREIGN KEY ([ClientGroupID_FK]) REFERENCES [dbo].[T_RoutingLogicClientGroup] ([ClientGroupID])
);

