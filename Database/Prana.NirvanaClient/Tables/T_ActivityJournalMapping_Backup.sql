CREATE TABLE [dbo].[T_ActivityJournalMapping_Backup] (
    [ActivityTypeId_FK] INT     NOT NULL,
    [AmountTypeId_FK]   INT     NOT NULL,
    [DebitAccount]      INT     NULL,
    [CreditAccount]     INT     NULL,
    [CashValueType]     TINYINT NOT NULL,
    [ActivityDateType]  INT     NOT NULL,
    [Id]                INT     IDENTITY (1, 1) NOT NULL
);

