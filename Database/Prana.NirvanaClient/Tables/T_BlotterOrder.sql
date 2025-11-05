CREATE TABLE [dbo].[T_BlotterOrder] (
    [BlotterOrderID] INT    IDENTITY (1, 1) NOT NULL,
    [BlotterID]      INT    NOT NULL,
    [OrderID_FK]     BIGINT NOT NULL,
    CONSTRAINT [PK_T_BlotterOrder] PRIMARY KEY CLUSTERED ([BlotterOrderID] ASC),
    CONSTRAINT [FK_T_BlotterOrder_T_Blotter] FOREIGN KEY ([BlotterID]) REFERENCES [dbo].[T_Blotter] ([BlotterID])
);

