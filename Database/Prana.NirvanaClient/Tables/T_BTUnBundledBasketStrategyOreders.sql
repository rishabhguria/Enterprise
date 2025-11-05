CREATE TABLE [dbo].[T_BTUnBundledBasketStrategyOreders] (
    [PK]                INT          IDENTITY (1, 1) NOT NULL,
    [BasketID]          VARCHAR (50) NULL,
    [CLOrderID]         VARCHAR (50) NULL,
    [AllocationStateID] INT          NOT NULL,
    CONSTRAINT [PK_T_BTUnBundledBasketStrategyOreders] PRIMARY KEY CLUSTERED ([PK] ASC)
);

