CREATE TABLE [dbo].[T_InterestRates] (
    [AutoInterestRate]   FLOAT (53)   NULL,
    [Date]               VARCHAR (20) NULL,
    [ManualInterestRate] FLOAT (53)   NULL,
    [RateID]             INT          IDENTITY (1, 1) NOT NULL,
    [UserID]             INT          NULL,
    CONSTRAINT [PK_T_InterstRates] PRIMARY KEY CLUSTERED ([RateID] ASC)
);

