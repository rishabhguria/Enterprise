CREATE TABLE [dbo].[T_DefaultActivityJournalMapping] (
    [ActivityType]     VARCHAR (50) NULL,
    [AmountType]       VARCHAR (50) NULL,
    [DebitAccount]     VARCHAR (50) NULL,
    [CreditAccount]    VARCHAR (50) NULL,
    [CashValueType]    TINYINT      DEFAULT ((0)) NULL,
    [ActivityDateType] INT          DEFAULT ((1)) NULL
);

