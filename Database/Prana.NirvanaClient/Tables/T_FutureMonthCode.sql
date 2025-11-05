CREATE TABLE [dbo].[T_FutureMonthCode] (
    [FutureMonthCodeID] INT          IDENTITY (1, 1) NOT NULL,
    [FutureMonth]       VARCHAR (50) NOT NULL,
    [Abbreviation]      VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_T_FutureMonthCode] PRIMARY KEY CLUSTERED ([FutureMonthCodeID] ASC)
);

