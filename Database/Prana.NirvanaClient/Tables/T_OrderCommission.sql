CREATE TABLE [dbo].[T_OrderCommission] (
    [OrderCommissionId]  BIGINT       IDENTITY (1, 1) NOT NULL,
    [ParentClOrderID_FK] VARCHAR (50) NOT NULL,
    [Commission]         FLOAT (53)   CONSTRAINT [DF__T_OrderCo__Commi__5A30FF58] DEFAULT ((0)) NOT NULL,
    [Fees]               FLOAT (53)   CONSTRAINT [DF__T_OrderCom__Fees__5B252391] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__T_OrderCommissio__5C1947CA] PRIMARY KEY CLUSTERED ([OrderCommissionId] ASC) WITH (FILLFACTOR = 100)
);

