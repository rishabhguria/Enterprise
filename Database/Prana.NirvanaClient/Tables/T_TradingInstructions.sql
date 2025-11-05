CREATE TABLE [dbo].[T_TradingInstructions] (
    [Symbol]           VARCHAR (50)  NOT NULL,
    [SideTagValue]     VARCHAR (10)  NULL,
    [Quantity]         FLOAT (53)    NULL,
    [Instructions]     VARCHAR (200) NULL,
    [ClientOrderID]    VARCHAR (50)  NULL,
    [ClOrderID]        VARCHAR (50)  NULL,
    [CompanyUserID]    INT           NULL,
    [TradingAccountID] INT           NULL,
    [TradingInstID]    INT           IDENTITY (1, 1) NOT NULL,
    [IsAccepted]       INT           NULL,
    [MsgType]          VARCHAR (10)  NULL,
    [OnBehalfOfCompID] VARCHAR (50)  NULL,
    CONSTRAINT [PK_T_TradingInstructions] PRIMARY KEY CLUSTERED ([TradingInstID] ASC)
);

