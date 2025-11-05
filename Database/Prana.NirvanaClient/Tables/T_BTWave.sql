CREATE TABLE [dbo].[T_BTWave] (
    [WaveID]         VARCHAR (50) NOT NULL,
    [BasketID]       VARCHAR (50) NULL,
    [Percentage]     FLOAT (53)   NULL,
    [TradedBasketID] VARCHAR (50) NULL,
    CONSTRAINT [PK_T_Wave] PRIMARY KEY CLUSTERED ([WaveID] ASC),
    CONSTRAINT [FK_T_BTWave_T_BTUploadedBaskets] FOREIGN KEY ([BasketID]) REFERENCES [dbo].[T_BTUploadedBaskets] ([BasketID])
);

