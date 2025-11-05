CREATE TABLE [dbo].[T_OrderType] (
    [OrderTypesID]      INT          IDENTITY (1, 1) NOT NULL,
    [OrderTypes]        VARCHAR (50) NOT NULL,
    [OrderTypeTagValue] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_T_OrderTypes] PRIMARY KEY CLUSTERED ([OrderTypesID] ASC)
);

