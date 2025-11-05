CREATE TABLE [dbo].[T_RoutingLogicGroupClient] (
    [ClientGroupID_FK]   INT NOT NULL,
    [CompanyClientID_FK] INT NOT NULL,
    [ApplyRL]            INT NOT NULL,
    CONSTRAINT [FK_T_RoutingLogicGroupClient_T_CompanyClient] FOREIGN KEY ([CompanyClientID_FK]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID]) ON DELETE CASCADE,
    CONSTRAINT [FK_T_RoutingLogicGroupClient_T_RoutingLogicClientGroup] FOREIGN KEY ([ClientGroupID_FK]) REFERENCES [dbo].[T_RoutingLogicClientGroup] ([ClientGroupID])
);

