CREATE TABLE [dbo].[T_BTTradedBasket] (
    [TradedBasketID]    VARCHAR (50) NOT NULL,
    [BasketID]          VARCHAR (50) NOT NULL,
    [UserID]            INT          NOT NULL,
    [serverReceiveTime] DATETIME     NOT NULL,
    [TradingAccountID]  INT          NOT NULL,
    CONSTRAINT [PK_T_BTTradedBasket] PRIMARY KEY CLUSTERED ([TradedBasketID] ASC),
    CONSTRAINT [FK_T_BTTradedBasket_T_BTUploadedBaskets1] FOREIGN KEY ([BasketID]) REFERENCES [dbo].[T_BTUploadedBaskets] ([BasketID])
);

