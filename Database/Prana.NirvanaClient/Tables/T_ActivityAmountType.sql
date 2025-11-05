CREATE TABLE [dbo].[T_ActivityAmountType] (
    [AmountTypeId]             INT          IDENTITY (1, 1) NOT NULL,
    [AmountType]               VARCHAR (50) NULL,
    [defaultSubaccountAcronym] VARCHAR (40) NULL,
    CONSTRAINT [PK_T_ActivityColumns] PRIMARY KEY CLUSTERED ([AmountTypeId] ASC)
);

