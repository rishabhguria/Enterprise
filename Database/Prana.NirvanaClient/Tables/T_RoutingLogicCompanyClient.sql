CREATE TABLE [dbo].[T_RoutingLogicCompanyClient] (
    [CompanyClientID_FK] INT NOT NULL,
    [RLID_FK]            INT NOT NULL,
    [Rank]               INT NOT NULL,
    [ApplyRL]            INT NOT NULL,
    CONSTRAINT [FK_T_RoutingLogicCompanyClient_T_CompanyClient] FOREIGN KEY ([CompanyClientID_FK]) REFERENCES [dbo].[T_CompanyClient] ([CompanyClientID]),
    CONSTRAINT [FK_T_RoutingLogicCompanyClient_T_RoutingLogic] FOREIGN KEY ([RLID_FK]) REFERENCES [dbo].[T_RoutingLogic] ([RLID])
);

