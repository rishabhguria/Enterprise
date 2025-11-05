CREATE TABLE [dbo].[T_CompanyClientAUECOrderTypes] (
    [CompanyClientAUECOrderTypesID] INT NOT NULL,
    [CompanyClientAUECID]           INT NOT NULL,
    [OrderTypesID]                  INT NOT NULL,
    CONSTRAINT [PK_T_CompanyClientAUECOrderTypes] PRIMARY KEY CLUSTERED ([CompanyClientAUECOrderTypesID] ASC),
    CONSTRAINT [FK_T_CompanyClientAUECOrderTypes_T_OrderType] FOREIGN KEY ([OrderTypesID]) REFERENCES [dbo].[T_OrderType] ([OrderTypesID])
);

